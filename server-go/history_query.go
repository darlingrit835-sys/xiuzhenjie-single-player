package main

import (
	"encoding/json"
	"fmt"
	"io"
)

type historyQuery struct {
	mode, kind, roleID, scope string
	limit                     int
}

func (q historyQuery) write(dir string, out io.Writer) error {
	switch q.mode {
	case "events", "firsts":
	default:
		return fmt.Errorf("-history-query must be events or firsts")
	}
	switch q.scope {
	case "", "world", "account", "role", "world-entity", "role-entity":
	default:
		return fmt.Errorf("invalid -history-scope %q", q.scope)
	}
	if q.scope != "" && q.mode != "firsts" {
		return fmt.Errorf("-history-scope requires -history-query firsts")
	}
	if q.limit < 1 || q.limit > 1000 {
		return fmt.Errorf("-history-limit must be between 1 and 1000")
	}

	files, err := historyFiles(dir)
	if err != nil {
		return err
	}
	recent := make([]historyEntry, q.limit)
	count := 0
	for _, path := range files {
		if err := readHistoryFile(path, false, func(event historyEntry) error {
			if q.matches(event) {
				recent[count%len(recent)] = event
				count++
			}
			return nil
		}); err != nil {
			return err
		}
	}
	encoder := json.NewEncoder(out)
	encoder.SetEscapeHTML(false)
	for i := max(0, count-len(recent)); i < count; i++ {
		if err := encoder.Encode(recent[i%len(recent)]); err != nil {
			return err
		}
	}
	return nil
}

func (q historyQuery) matches(event historyEntry) bool {
	if q.kind != "" && q.kind != event.Kind {
		return false
	}
	if q.roleID != "" && q.roleID != event.Actor.RoleID &&
		(event.Subject == nil || q.roleID != event.Subject.RoleID) {
		return false
	}
	if q.mode == "events" {
		return true
	}
	// Role-scoped firsts belong to the actor. The subject remains available
	// when searching the broader event log.
	if q.roleID != "" && q.roleID != event.Actor.RoleID {
		return false
	}
	switch q.scope {
	case "world":
		return event.First.World
	case "account":
		return event.First.Account
	case "role":
		return event.First.Role
	case "world-entity":
		return event.First.WorldEntity
	case "role-entity":
		return event.First.RoleEntity
	default:
		return event.First.any()
	}
}
