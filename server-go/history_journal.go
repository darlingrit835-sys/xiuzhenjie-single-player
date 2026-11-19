package main

import (
	"bufio"
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"log"
	"os"
	"path/filepath"
	"runtime"
	"strings"
	"sync"
	"time"
)

type historyFirsts struct {
	World       bool `json:"world,omitempty"`
	Account     bool `json:"account,omitempty"`
	Role        bool `json:"role,omitempty"`
	WorldEntity bool `json:"worldEntity,omitempty"`
	RoleEntity  bool `json:"roleEntity,omitempty"`
}

func (f historyFirsts) any() bool {
	return f.World || f.Account || f.Role || f.WorldEntity || f.RoleEntity
}

type historyEntry struct {
	historyRecord
	First historyFirsts `json:"first"`
}

type historyKey struct {
	scope, kind, owner, entityType, entityID string
}

type gameHistory struct {
	dir        string
	mu         sync.Mutex
	day        string
	watermarks map[string]int64
	firsts     map[historyKey]struct{}
	writeFault error
}

func openGameHistory(dir string) (*gameHistory, error) {
	if err := os.MkdirAll(dir, 0o700); err != nil {
		return nil, err
	}
	h := &gameHistory{
		dir: dir, watermarks: make(map[string]int64), firsts: make(map[historyKey]struct{}),
	}
	files, err := historyFiles(dir)
	if err != nil {
		return nil, err
	}
	for index, path := range files {
		if err := readHistoryFile(path, index == len(files)-1, func(event historyEntry) error {
			if event.Sequence != h.watermarks[event.Source]+1 {
				return fmt.Errorf("out of sequence game history event %s", event.ID)
			}
			h.watermarks[event.Source] = event.Sequence
			markHistoryFirsts(event.historyRecord, h.firsts, h.firsts)
			return nil
		}); err != nil {
			return nil, err
		}
		h.day = strings.TrimSuffix(strings.TrimPrefix(filepath.Base(path), "events-"), ".jsonl")
	}
	return h, nil
}

func historyFiles(dir string) ([]string, error) {
	entries, err := os.ReadDir(dir)
	if errors.Is(err, os.ErrNotExist) {
		return nil, nil
	}
	if err != nil {
		return nil, err
	}
	var files []string
	for _, entry := range entries {
		name := entry.Name()
		if !entry.Type().IsRegular() || !strings.HasPrefix(name, "events-") || !strings.HasSuffix(name, ".jsonl") {
			continue
		}
		day := strings.TrimSuffix(strings.TrimPrefix(name, "events-"), ".jsonl")
		if _, err := time.Parse(time.DateOnly, day); err == nil {
			files = append(files, filepath.Join(dir, name))
		}
	}
	return files, nil
}

func readHistoryFile(path string, repairTail bool, visit func(historyEntry) error) error {
	flags := os.O_RDONLY
	if repairTail {
		flags = os.O_RDWR
	}
	file, err := os.OpenFile(path, flags, 0)
	if err != nil {
		return err
	}
	defer file.Close()
	reader := bufio.NewReaderSize(file, maxHistoryLine)
	var offset int64
	for {
		line, err := reader.ReadSlice('\n')
		if errors.Is(err, io.EOF) {
			// Ignore an unfinished last line when reading while the server appends.
			if repairTail && len(line) != 0 {
				if err := file.Truncate(offset); err != nil {
					return err
				}
				return file.Sync()
			}
			return nil
		}
		if err != nil {
			return fmt.Errorf("read game history %s: %w", filepath.Base(path), err)
		}
		offset += int64(len(line))
		var event historyEntry
		decoder := json.NewDecoder(bytes.NewReader(line))
		decoder.UseNumber()
		if err := decoder.Decode(&event); err != nil || event.Source == "" || event.Sequence <= 0 ||
			event.ID == "" || event.Actor.RoleID == "" || event.Kind == "" || event.OccurredAt.IsZero() {
			return fmt.Errorf("invalid game history record in %s at byte %d", filepath.Base(path), offset)
		}
		var trailing any
		if err := decoder.Decode(&trailing); !errors.Is(err, io.EOF) {
			return fmt.Errorf("unexpected data in game history %s at byte %d", filepath.Base(path), offset)
		}
		if err := visit(event); err != nil {
			return err
		}
	}
}

func markHistoryFirsts(event historyRecord, known, batch map[historyKey]struct{}) historyFirsts {
	mark := func(key historyKey) bool {
		if _, exists := known[key]; exists {
			return false
		}
		if _, exists := batch[key]; exists {
			return false
		}
		batch[key] = struct{}{}
		return true
	}
	first := historyFirsts{
		World: mark(historyKey{scope: "world", kind: event.Kind}),
		Role:  mark(historyKey{scope: "role", kind: event.Kind, owner: event.Actor.RoleID}),
	}
	if event.Actor.AccountID != "" {
		first.Account = mark(historyKey{scope: "account", kind: event.Kind, owner: event.Actor.AccountID})
	}
	if event.EntityType != "" && event.EntityID != "" {
		first.WorldEntity = mark(historyKey{
			scope: "world_entity", kind: event.Kind, entityType: event.EntityType, entityID: event.EntityID,
		})
		first.RoleEntity = mark(historyKey{
			scope: "role_entity", kind: event.Kind, owner: event.Actor.RoleID, entityType: event.EntityType, entityID: event.EntityID,
		})
	}
	return first
}

func (h *gameHistory) appendPending(events []historyRecord) (int64, error) {
	if len(events) == 0 {
		return 0, nil
	}
	h.mu.Lock()
	defer h.mu.Unlock()
	if h.writeFault != nil {
		return 0, h.writeFault
	}
	source := events[0].Source
	watermark := h.watermarks[source]
	firsts := make(map[historyKey]struct{})
	var batch bytes.Buffer
	encoder := json.NewEncoder(&batch)
	encoder.SetEscapeHTML(false)
	for _, event := range events {
		if event.Source != source {
			return 0, errors.New("mixed sources in game history batch")
		}
		if event.Sequence <= watermark {
			continue // The last export may have succeeded before the save was acknowledged.
		}
		if event.Sequence != watermark+1 {
			return 0, fmt.Errorf("missing game history before %s", event.ID)
		}
		entry := historyEntry{historyRecord: event, First: markHistoryFirsts(event, h.firsts, firsts)}
		if err := encoder.Encode(entry); err != nil {
			return 0, err
		}
		watermark = event.Sequence
	}
	if batch.Len() == 0 {
		return watermark, nil
	}
	day := max(h.day, time.Now().UTC().Format(time.DateOnly))
	if err := h.writeBatch(day, batch.Bytes()); err != nil {
		return 0, err
	}
	h.day = day
	h.watermarks[source] = watermark
	for key := range firsts {
		h.firsts[key] = struct{}{}
	}
	return watermark, nil
}

func (h *gameHistory) writeBatch(day string, body []byte) error {
	path := filepath.Join(h.dir, "events-"+day+".jsonl")
	file, err := os.OpenFile(path, os.O_CREATE|os.O_RDWR|os.O_APPEND, 0o600)
	if err != nil {
		return err
	}
	defer file.Close()
	info, err := file.Stat()
	if err != nil {
		return err
	}
	_, err = file.Write(body)
	if err == nil {
		err = file.Sync()
	}
	if err == nil && h.day != day && runtime.GOOS != "windows" {
		dir, openErr := os.Open(h.dir)
		if openErr != nil {
			err = openErr
		} else {
			err = dir.Sync()
			_ = dir.Close()
		}
	}
	if err != nil {
		if rollback := file.Truncate(info.Size()); rollback != nil {
			h.writeFault = errors.Join(err, rollback)
			return h.writeFault
		}
		if rollback := file.Sync(); rollback != nil {
			h.writeFault = errors.Join(err, rollback)
			return h.writeFault
		}
	}
	return err
}

func (h *gameHistory) flush(stores ...*Store) error {
	var failures []error
	for _, store := range stores {
		if store == nil {
			continue
		}
		events, err := store.pendingHistory()
		if err == nil && len(events) != 0 {
			var through int64
			through, err = h.appendPending(events)
			if err == nil {
				err = store.acknowledgeHistory(through)
			}
		}
		if err != nil {
			failures = append(failures, fmt.Errorf("%s: %w", store.historySource(), err))
		}
	}
	return errors.Join(failures...)
}

// checkStores verifies that the archive and its transactional outboxes are
// from a consistent backup before any new business writes are accepted.
func (h *gameHistory) checkStores(stores ...*Store) error {
	for _, store := range stores {
		if store == nil {
			continue
		}
		outbox := store.Get(historyOutboxID)
		sequence := roleInt(outbox, "sequence")
		acknowledged := sequence - int64(len(historyQueue(outbox)))
		source := store.historySource()
		if archived := h.watermarks[source]; archived < acknowledged || archived > sequence {
			return fmt.Errorf("game history and %s save disagree (archive %d, save %d, acknowledged %d)", source, archived, sequence, acknowledged)
		}
	}
	return nil
}

func (h *gameHistory) run(ctx context.Context, stores ...*Store) {
	ticker := time.NewTicker(time.Second)
	defer ticker.Stop()
	var lastFailure string
	for {
		if err := h.flush(stores...); err != nil {
			if err.Error() != lastFailure {
				lastFailure = err.Error()
				log.Printf("游戏历史导出失败，事件保留在存档队列: %v", err)
			}
		} else if lastFailure != "" {
			log.Print("游戏历史导出已恢复")
			lastFailure = ""
		}
		select {
		case <-ctx.Done():
			return
		case <-ticker.C:
		}
	}
}
