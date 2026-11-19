package main

import (
	"bytes"
	"encoding/json"
	"errors"
	"os"
	"path/filepath"
	"testing"
)

func TestHistoryRecoversUnacknowledgedEventsAndPreservesFirsts(t *testing.T) {
	dir := t.TempDir()
	archive := filepath.Join(dir, "history")
	store, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	store.enableHistory()
	first := Record{"Id": "role-a", "UserId": "account-a", "Name": "晴川"}
	second := Record{"Id": "role-b", "UserId": "account-a", "Name": "浮舟"}
	counterparty := Record{"Id": "role-c", "UserId": "account-b", "Name": "岚客"}
	firstEvent := newHistoryEvent("dungeon.cleared", first, counterparty, "dungeon", "stone-pass",
		Record{"total": int64(9007199254740993)})
	if err := store.InsertUnique("role-a", first, nil, firstEvent); err != nil {
		t.Fatal(err)
	}
	if err := store.InsertUnique("role-a", first, nil, firstEvent); !errors.Is(err, ErrRecordExists) {
		t.Fatalf("duplicate role inserted: %v", err)
	}
	aborted := errors.New("discard mutation")
	if err := store.Transact(func(tx *StoreTx) error {
		tx.Get("role-a")["Name"] = "discarded name"
		if err := tx.RecordHistory(newHistoryEvent("dungeon.cleared", second, nil, "dungeon", "stone-pass", nil)); err != nil {
			return err
		}
		return aborted
	}); !errors.Is(err, aborted) {
		t.Fatal(err)
	}
	if roleString(store.Get("role-a"), "Name") != "晴川" {
		t.Fatal("aborted role changes were published")
	}
	if store.Find(func(r Record) bool { return roleString(r, "Id") == "" }) != nil {
		t.Fatal("history outbox appeared in role search")
	}
	history, err := openGameHistory(archive)
	if err != nil {
		t.Fatal(err)
	}
	events, err := store.pendingHistory()
	if err != nil || len(events) != 1 {
		t.Fatalf("queued events = %d, error = %v", len(events), err)
	}
	// A shutdown between writing the archive and acknowledging the store
	// leaves the same event in both places. The next batch can also tear.
	if _, err := history.appendPending(events); err != nil {
		t.Fatal(err)
	}
	files, err := historyFiles(archive)
	if err != nil || len(files) != 1 {
		t.Fatalf("archive files: %v, error = %v", files, err)
	}
	tail, err := os.OpenFile(files[0], os.O_APPEND|os.O_WRONLY, 0)
	if err != nil {
		t.Fatal(err)
	}
	if _, err := tail.WriteString(`{"unfinished":`); err != nil {
		tail.Close()
		t.Fatal(err)
	}
	if err := tail.Close(); err != nil {
		t.Fatal(err)
	}
	history, err = openGameHistory(archive)
	if err != nil {
		t.Fatal(err)
	}
	store, err = OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	store.enableHistory()
	if err := store.InsertUnique("role-b", second, nil,
		newHistoryEvent("dungeon.cleared", second, nil, "dungeon", "stone-pass", nil),
		newHistoryEvent("dungeon.cleared", second, nil, "dungeon", "jade-pass", nil)); err != nil {
		t.Fatal(err)
	}
	if err := history.checkStores(store); err != nil {
		t.Fatal(err)
	}
	if err := history.flush(store); err != nil {
		t.Fatal(err)
	}
	store, err = OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	events, err = store.pendingHistory()
	if err != nil || len(events) != 0 {
		t.Fatalf("pending after archive = %d, error = %v", len(events), err)
	}
	missing, err := openGameHistory(filepath.Join(dir, "empty-history"))
	if err != nil {
		t.Fatal(err)
	}
	if err := missing.checkStores(store); err == nil {
		t.Fatal("startup accepted a save with missing acknowledged history")
	}
	var archived []historyEntry
	if err := readHistoryFile(files[0], false, func(event historyEntry) error {
		archived = append(archived, event)
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	if len(archived) != 3 || archived[0].ID != "roles-1" || archived[1].ID != "roles-2" || archived[2].ID != "roles-3" {
		t.Fatalf("unexpected history: %#v", archived)
	}
	if archived[0].Actor != historyRole(first) || archived[0].Subject == nil ||
		*archived[0].Subject != historyRole(counterparty) ||
		archived[0].Details["total"] != json.Number("9007199254740993") {
		t.Fatalf("unexpected participant or value: %#v", archived[0])
	}
	if archived[0].First != (historyFirsts{World: true, Account: true, Role: true, WorldEntity: true, RoleEntity: true}) ||
		archived[1].First != (historyFirsts{Role: true, RoleEntity: true}) ||
		archived[2].First != (historyFirsts{WorldEntity: true, RoleEntity: true}) {
		t.Fatalf("unexpected firsts: %#v", archived)
	}

	for _, tc := range []struct {
		query historyQuery
		want  string
	}{
		{historyQuery{mode: "events", roleID: "role-c", limit: 1}, "roles-1"},
		{historyQuery{mode: "firsts", roleID: "role-c", limit: 1}, ""},
		{historyQuery{mode: "firsts", scope: "world", limit: 1}, "roles-1"},
		{historyQuery{mode: "firsts", scope: "role", roleID: "role-b", limit: 1}, "roles-2"},
		{historyQuery{mode: "firsts", scope: "world-entity", limit: 1}, "roles-3"},
		{historyQuery{mode: "events", kind: "guild.applied", limit: 1}, ""},
	} {
		var out bytes.Buffer
		if err := tc.query.write(archive, &out); err != nil {
			t.Fatal(err)
		}
		if tc.want == "" {
			if out.Len() != 0 {
				t.Fatalf("query %+v: expected no entries, got %s", tc.query, &out)
			}
			continue
		}
		var found historyEntry
		if err := json.Unmarshal(out.Bytes(), &found); err != nil || found.ID != tc.want {
			t.Fatalf("query %+v: want %s, got %s, error = %v", tc.query, tc.want, &out, err)
		}
	}
}
