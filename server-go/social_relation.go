package main

import "strings"

func (s *Server) resolveSocialName(name string) (string, string) {
	if trimmed := strings.TrimSpace(name); trimmed != "" {
		// Prefer the exact saving key for older roles whose names include spaces.
		if id := s.roles.roleIDByName(name); id != "" {
			return id, name
		}
		if trimmed != name {
			return s.roles.roleIDByName(trimmed), trimmed
		}
	}
	return "", ""
}

// Legacy role records may store their ID only in the enclosing save key.
func socialHistory(kind, actorID string, actor Record, subjectID string, subject, details Record) historyEvent {
	event := newHistoryEvent(kind, actor, subject, "role", subjectID, details)
	event.Actor.RoleID = actorID
	participant := historyRole(subject)
	participant.RoleID = subjectID
	event.Subject = &participant
	return event
}
