package main

import (
	"google.golang.org/protobuf/proto"
	"strconv"
	"testing"
)

func TestVoidFacilityTempleSealsAndOwnFolio(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	p, _ := auctionTestRole(t, s, "reader", nil)
	other, _ := auctionTestRole(t, s, "other", nil)
	ask := func(q voidFacilityRequest) Record {
		t.Helper()
		r, e := s.voidFacilityAction(p.roleID, q)
		if e != nil {
			t.Fatal(e)
		}
		return r
	}
	if n := len(ask(voidFacilityRequest{Action: "temple"})["halls"].([]Record)); n != 9 {
		t.Fatal(n)
	}
	for _, h := range voidCraftV2.Halls {
		for i, b := range h.Briefs {
			q := voidFacilityRequest{Action: "lecture", Hall: h.ID, Brief: i}
			lesson := ask(q)
			if lesson["name"] != b.Name || roleBool(lesson, "passed") {
				t.Fatal(lesson)
			}
			q.Action = "seal"
			q.Answer = 0
			if r := ask(q); roleBool(r, "passed") {
				t.Fatal("incorrect answer granted a seal")
			}
			q.Answer = []int{b.Element, b.Polarity, b.Qi}[i]
			for range 2 {
				if r := ask(q); !roleBool(r, "correct") || !roleBool(r, "passed") {
					t.Fatal(r)
				}
			}
		}
	}
	for i := 0; i < 23; i++ {
		id := "work" + strconv.Itoa(i)
		creator := p.roleID
		if i == 22 {
			creator = other.roleID
		}
		if err := s.roles.Put(voidStorePrefix+id, Record{"id": id, "kind": "work", "name": id, "authorId": creator, "hallId": "dan", "time": i}); err != nil {
			t.Fatal(err)
		}
	}
	s = auctionTestServer(t, dir)
	for _, h := range ask(voidFacilityRequest{Action: "temple"})["halls"].([]Record) {
		if roleInt(h, "seals") != 3 {
			t.Fatal(h)
		}
	}
	first := ask(voidFacilityRequest{Action: "folio"})
	second := ask(voidFacilityRequest{Action: "folio", Page: 1})
	if roleInt(first, "count") != 22 || !roleBool(first, "more") || len(first["works"].([]Record)) != 20 || first["works"].([]Record)[0]["id"] != "work21" || roleBool(second, "more") || len(second["works"].([]Record)) != 2 {
		t.Fatal(first, second)
	}
	if err := s.roles.Update(p.roleID, func(r Record) error { r["Level"] = 20; return nil }); err != nil {
		t.Fatal(err)
	}
	if _, err := s.voidFacilityAction(p.roleID, voidFacilityRequest{Action: "temple"}); err == nil {
		t.Fatal("below-entrance role accepted")
	}
}

func TestVoidFacilityTowerBattleAndReadback(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.dataDir = "../server/data"
	s.config = Config{BattleDurationSeconds: 300, MonsterCount: 6, MonsterDifficulty: 1, DropMultiplier: 1}
	p, conn := auctionTestRole(t, s, "climber", nil)
	if err := s.roles.Update(p.roleID, func(r Record) error {
		r["Level"] = 21
		r["CurrentVigor"] = 100
		r["CurrentAge"] = 27
		r["RushTowerNum"] = 39
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	call := func(cmd int, request, response string, values Record) int64 {
		t.Helper()
		return getInt(spellTestCall(t, s, p, cmd, request, response, values), "MyRanked")
	}
	if n := getInt(spellTestCall(t, s, p, 1008001, "", "EnterUnrealTowerResp_1008001", nil), "RushTowerNum"); n != 39 {
		t.Fatal(n)
	}
	built := spellTestCall(t, s, p, 1010001, "BuildDungeonReq_1010001", "BuildDungeonResp_1010001", Record{"MapType": 63})
	d := s.dungeonForRole(getString(built, "DungeonId"), p.roleID, false)
	if d == nil || d.towerFloor != 40 || d.towerStage != 3 {
		t.Fatal(built, d)
	}
	joined := spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"DungeonId": d.id, "MapId": d.mapID})
	if !getBool(joined, "IsSuccess") {
		t.Fatal(joined)
	}
	battleRequest := func(cmd int, seq int, name string, data Record) Message {
		m := Message{Cmd: cmd, Msid: seq}
		if name != "" {
			in := s.pb(name)
			putPB(in, data)
			body, e := proto.Marshal(in)
			if e != nil {
				t.Fatal(e)
			}
			m.Data = body
		}
		s.handleBattle(p, m)
		return m
	}
	enter := battleRequest(8001001, 31, "BattleRoleEnterReq_8001001", Record{"DungeonId": d.id, "RoleId": p.roleID, "WorldVersion": 1})
	initial := conn.receive(t, s, enter.Cmd, 31, "BattleRoleEnterResp_8001001")
	if !getBool(initial, "IsSuccess") || len(d.world.nodes) != 0 || len(d.monsters) != 3 {
		t.Fatal("tower layout or entry")
	}
	next := func() bool {
		m := battleRequest(8001009, 32, "", nil)
		r := conn.receive(t, s, m.Cmd, m.Msid, "EnterTowerNextLevelResp_8001009")
		return getBool(r, "IsSuccess")
	}
	if next() {
		t.Fatal("uncleared floor advanced")
	}
	current := d.monsters[0].index
	for _, guard := range d.monsters {
		battleRequest(8002003, 33, "MonsterDeadReq_8002003", Record{"DeadMonsterIndex": guard.index, "PosX": d.world.playerX, "PosY": d.world.playerY})
	}
	if n := towerBest(s.roles.Get(p.roleID), 3); n != 40 {
		t.Fatal(n)
	}
	if call(1008002, "EnterRushTowerTierNumRankedReq_1008002", "EnterRushTowerTierNumRankedResp_1008002", Record{"Stage": 3}) != 1 || call(1008004, "EnterRushTowerAgeRankedReq_1008004", "EnterRushTowerAgeRankedResp_1008004", Record{"Stage": 3}) != 1 {
		t.Fatal("missing tower rank")
	}
	if !next() || next() || d.towerFloor != 41 || d.monsters[0].index <= current || len(d.dead) != 0 {
		t.Fatal("next floor did not reopen exactly once")
	}
	battleRequest(8002003, 34, "MonsterDeadReq_8002003", Record{"DeadMonsterIndex": current, "PosX": d.world.playerX, "PosY": d.world.playerY})
	if len(d.dead) != 0 {
		t.Fatal("last floor's notification changed new floor")
	}
	// Retrying a lost continuation reply returns the same wave and no new score.
	same := Message{Cmd: 8001009, Msid: 42, Data: offerNumber(nil, 1, 41)}
	s.handleBattle(p, same)
	reply := conn.receive(t, s, 8001009, 42, "EnterTowerNextLevelResp_8001009")
	if !getBool(reply, "IsSuccess") || len(d.dead) != 0 || d.towerFloor != 41 {
		t.Fatal("continuation receipt", reply)
	}
	bad := Message{Cmd: 8001009, Msid: 43, Data: offerNumber(nil, 1, 90)}
	s.handleBattle(p, bad)
	if getBool(conn.receive(t, s, 8001009, 43, "EnterTowerNextLevelResp_8001009"), "IsSuccess") {
		t.Fatal("unearned floor")
	}
	leave := battleRequest(8001008, 78, "RoleLeaveDungeonReq_8001008", Record{"RoleIndex": 1})
	closed := conn.receive(t, s, leave.Cmd, leave.Msid, "RoleLeaveDungeonResp_8001008")
	if !getBool(closed, "IsSuccess") || s.dungeonForRole(d.id, p.roleID, false) != nil {
		t.Fatal("retreat did not close the tower", closed)
	}
	s = auctionTestServer(t, dir)
	info, e := s.voidFacilityAction(p.roleID, voidFacilityRequest{Action: "tower", Mode: "age", Stage: 3})
	if e != nil || roleInt(info, "next") != 41 || roleInt(info, "mine") != 1 || roleInt(info["ranks"].([]Record)[0], "Age") != 27 {
		t.Fatal(info, e)
	}
	if err := s.roles.Update(p.roleID, func(r Record) error { r["Level"] = 31; return nil }); err != nil {
		t.Fatal(err)
	}
	fresh, e := s.voidFacilityAction(p.roleID, voidFacilityRequest{Action: "tower", Stage: 3})
	if e != nil || roleInt(fresh, "mine") != 1 || roleInt(fresh, "next") != 41 {
		t.Fatal("old stage record", fresh, e)
	}
}

func TestVoidTowerFirstFloorAndRoleGate(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	s.dataDir = "../server/data"
	s.config = Config{BattleDurationSeconds: 60, MonsterCount: 3, MonsterDifficulty: 1, DropMultiplier: 1}
	p, _ := auctionTestRole(t, s, "novice", nil)
	if e := s.roles.Update(p.roleID, func(r Record) error { r["Level"] = 20; return nil }); e != nil {
		t.Fatal(e)
	}
	build := func(fields Record) bool {
		return getBool(spellTestCall(t, s, p, 1010001, "BuildDungeonReq_1010001", "BuildDungeonResp_1010001", fields), "IsSuccess")
	}
	if build(Record{"MapType": 63}) || build(Record{"MapId": towerMapID(9)}) {
		t.Fatal("role gate or map reservation")
	}
	if getBool(spellTestCall(t, s, p, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"MapId": towerMapID(9)}), "IsSuccess") {
		t.Fatal("direct join created an unauthorized tower")
	}
	if err := s.roles.Update(p.roleID, func(r Record) error { r["Level"] = 21; return nil }); err != nil {
		t.Fatal(err)
	}
	built := spellTestCall(t, s, p, 1010001, "BuildDungeonReq_1010001", "BuildDungeonResp_1010001", Record{"MapType": 63, "MapId": towerMapID(9)})
	d := s.dungeonForRole(getString(built, "DungeonId"), p.roleID, false)
	if d == nil || d.towerStage != 3 || d.towerFloor != 1 || len(d.monsters) == 0 || d.monsters[0].level != 21 {
		t.Fatal("first floor", built, d)
	}
	if err := s.roles.Update(p.roleID, func(r Record) error { r["Level"] = 81; return nil }); err != nil {
		t.Fatal(err)
	}
	info, e := s.voidFacilityAction(p.roleID, voidFacilityRequest{Action: "tower", Mode: "age", Stage: 3})
	if e != nil || roleInt(info, "next") != 241 || roleInt(info, "best") != 0 || roleInt(info, "mine") != 0 {
		t.Fatal("realm-based starting floors must not count as clears", info, e)
	}
}
