package main

import (
	"testing"
	"time"
)

func TestCollectBattleDropCreditsEachStoneDropOnce(t *testing.T) {
	role := Record{
		"ShinyStone": 50,
		"BattleLoot": []any{
			Record{"DropId": "lingshi_21_a", "Type": 100, "Count": 21},
			Record{"DropId": "lingshi_21_b", "Type": 100, "Count": 21},
		},
	}
	for _, step := range []struct {
		id      string
		code    int
		balance int64
	}{
		{"lingshi_21_a", 4526, 71},
		{"lingshi_21_a", 4536, 71},
		{"lingshi_21_b", 4526, 92},
		{"lingshi_21_b", 4536, 92},
	} {
		id, code := collectBattleDrop(role, step.id)
		if id != step.id || code != step.code || roleInt(role, "ShinyStone") != step.balance {
			t.Fatalf("pickup %q returned id=%q code=%d balance=%d", step.id, id, code, roleInt(role, "ShinyStone"))
		}
	}
	if len(battleRecords(role["BattleLoot"])) != 0 || len(goodsList(role)) != 0 {
		t.Fatal("stone pickup retained a drop or added it to the bag")
	}
}

func TestDungeonHistoryIsCommittedOnceWithRewards(t *testing.T) {
	dir := t.TempDir()
	store, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	if err := store.Put("role", Record{"Id": "role", "UserId": "account", "Name": "试炼修士", "Level": 1, "ShinyStone": 50}); err != nil {
		t.Fatal(err)
	}
	store.enableHistory()
	s := &Server{roles: store}
	wave := []battleMonster{
		{index: 1001, level: 1, unusual: 1, gold: 5},
		{index: 1002, level: 1, unusual: 4, gold: 5, name: "领主·玄水蟒"},
	}
	d := &battleDungeon{
		id: "dungeon", mapID: "1111W1", roleID: "role", playerLevel: 1,
		expires: time.Now().Add(time.Minute), monsters: wave, dead: make(map[int]struct{}),
	}
	expectEvents := func(store *Store, expected int) []historyRecord {
		t.Helper()
		events, err := store.pendingHistory()
		if err != nil || len(events) != expected {
			t.Fatalf("pending history count=%d, expected=%d, err=%v", len(events), expected, err)
		}
		return events
	}

	path := store.path
	store.path = dir // Fail the atomic rename without modifying the original save.
	accepted := s.acceptDungeonEntry(d)
	store.path = path
	if accepted || d.entered {
		t.Fatal("entry accepted after save failed")
	}
	expectEvents(store, 0)
	if !s.acceptDungeonEntry(d) || !s.acceptDungeonEntry(d) {
		t.Fatal("entry or reconnect was denied")
	}
	expectEvents(store, 1)
	if _, _, awarded := s.settleMonster(d, wave[0], 0, 0); !awarded {
		t.Fatal("ordinary kill was not awarded")
	}
	expectEvents(store, 1)
	lootCount := len(battleRecords(store.Get(d.roleID)["BattleLoot"]))

	store.path = dir
	_, _, awarded := s.settleMonster(d, wave[1], 0, 0)
	store.path = path
	role := store.Get(d.roleID)
	if awarded || len(d.dead) != 1 || roleInt(role, "ShinyStone") != 50 || len(battleRecords(role["BattleLoot"])) != lootCount {
		t.Fatal("failed clear leaked kill state, wallet credit or loot")
	}
	expectEvents(store, 1)
	if _, _, awarded := s.settleMonster(d, wave[1], 0, 0); !awarded {
		t.Fatal("clear retry did not commit")
	}
	if _, _, awarded := s.settleMonster(d, wave[1], 0, 0); awarded {
		t.Fatal("duplicate clear was awarded")
	}

	reopened, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	role = reopened.Get(d.roleID)
	if roleInt(role, "ShinyStone") != 92 || len(battleRecords(role["BattleLoot"])) <= lootCount {
		t.Fatal("clear reward was not saved")
	}
	eventIDs := map[string]string{"dungeon.entered": d.mapID, "boss.defeated": d.mapID + "/1002", "dungeon.cleared": d.mapID}
	for _, event := range expectEvents(reopened, len(eventIDs)) {
		if id, found := eventIDs[event.Kind]; !found || event.EntityID != id ||
			event.Actor.RoleID != d.roleID || event.Actor.AccountID != "account" || event.Actor.RoleName != "试炼修士" ||
			event.Details["dungeonId"] != d.id || event.Details["mapId"] != d.mapID {
			t.Fatalf("unexpected dungeon history event: %+v", event)
		}
		delete(eventIDs, event.Kind)
	}
}
