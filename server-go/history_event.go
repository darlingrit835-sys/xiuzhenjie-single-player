package main

import (
	"bytes"
	"encoding/json"
	"errors"
	"fmt"
	"path/filepath"
	"strings"
	"time"
)

const (
	historyOutboxID   = "$history:outbox"
	maxHistoryPending = 1024
	maxHistoryLine    = 16 << 10
)

type historyActor struct {
	RoleID    string `json:"roleId"`
	AccountID string `json:"accountId,omitempty"`
	RoleName  string `json:"roleName,omitempty"`
}

type historyEvent struct {
	Kind       string        `json:"kind"`
	OccurredAt time.Time     `json:"occurredAt"`
	Actor      historyActor  `json:"actor"`
	Subject    *historyActor `json:"subject,omitempty"`
	EntityType string        `json:"entityType,omitempty"`
	EntityID   string        `json:"entityId,omitempty"`
	Details    Record        `json:"details,omitempty"`
}

type historyRecord struct {
	Version  int    `json:"version"`
	ID       string `json:"id"`
	Source   string `json:"source"`
	Sequence int64  `json:"sequence"`
	historyEvent
}

func reservedStoreID(id string) bool {
	return id == previewAuctionStoreID || id == historyOutboxID
}

func historyRole(role Record) historyActor {
	return historyActor{
		RoleID: roleString(role, "Id"), AccountID: roleString(role, "UserId"), RoleName: roleString(role, "Name"),
	}
}

func newHistoryEvent(kind string, actor, subject Record, entityType, entityID string, details Record) historyEvent {
	event := historyEvent{
		Kind: kind, OccurredAt: time.Now().UTC(), Actor: historyRole(actor),
		EntityType: entityType, EntityID: entityID, Details: details,
	}
	if participant := historyRole(subject); participant.RoleID != "" {
		event.Subject = &participant
	}
	return event
}

func (s *Store) enableHistory() {
	s.mu.Lock()
	defer s.mu.Unlock()
	s.historyEnabled = true
}

func (s *Store) historySource() string {
	return strings.TrimSuffix(filepath.Base(s.path), ".json")
}

func historyQueue(record Record) []any {
	queued, _ := record["pending"].([]any)
	return queued
}

// RecordHistory includes events in the same durable save as the associated state change.
func (tx *StoreTx) RecordHistory(events ...historyEvent) error {
	if !tx.store.historyEnabled || len(events) == 0 {
		return nil
	}
	outbox := tx.GetOrCreate(historyOutboxID)
	queue := historyQueue(outbox)
	if len(queue)+len(events) > maxHistoryPending {
		return errors.New("game history is awaiting export")
	}
	sequence := roleInt(outbox, "sequence")
	source := tx.store.historySource()
	for _, event := range events {
		if event.Kind == "" || len(event.Kind) > 64 || event.Actor.RoleID == "" || event.OccurredAt.IsZero() {
			return errors.New("invalid game history event")
		}
		sequence++
		entry := historyRecord{
			Version: 1, ID: fmt.Sprintf("%s-%d", source, sequence), Source: source, Sequence: sequence, historyEvent: event,
		}
		body, err := json.Marshal(entry)
		// Keep space for the first-occurrence flags added by the journal.
		if err != nil || len(body) > maxHistoryLine-512 {
			return errors.New("game history event could not be encoded within its size limit")
		}
		var row Record
		decoder := json.NewDecoder(bytes.NewReader(body))
		decoder.UseNumber()
		if err := decoder.Decode(&row); err != nil {
			return fmt.Errorf("stage game history event: %w", err)
		}
		queue = append(queue, row)
	}
	outbox["sequence"], outbox["pending"] = sequence, queue
	return nil
}

func (s *Store) pendingHistory() ([]historyRecord, error) {
	queue := historyQueue(s.Get(historyOutboxID))
	events := make([]historyRecord, 0, len(queue))
	for _, row := range queue {
		body, err := json.Marshal(row)
		if err != nil {
			return nil, err
		}
		var event historyRecord
		decoder := json.NewDecoder(bytes.NewReader(body))
		decoder.UseNumber()
		if err := decoder.Decode(&event); err != nil {
			return nil, err
		}
		if event.Version != 1 || event.Source != s.historySource() || event.Sequence <= 0 || event.ID == "" || event.Kind == "" {
			return nil, errors.New("invalid pending game history event")
		}
		events = append(events, event)
	}
	return events, nil
}

func (s *Store) acknowledgeHistory(sequence int64) error {
	return s.Transact(func(tx *StoreTx) error {
		queue := historyQueue(tx.View(historyOutboxID))
		count := 0
		for _, value := range queue {
			row, _ := value.(Record)
			current := roleInt(row, "sequence")
			if current <= 0 || current > sequence {
				break
			}
			count++
		}
		if count != 0 {
			outbox := tx.Get(historyOutboxID)
			pending := historyQueue(outbox)
			outbox["pending"] = append([]any(nil), pending[count:]...)
		}
		return nil
	})
}
