package main

import (
	"math"
	"sync"
	"testing"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/proto"
)

func fieldEntrySupplies(t *testing.T, wire []byte) map[protowire.Number]int64 {
	t.Helper()
	values := make(map[protowire.Number]int64)
	for len(wire) > 0 {
		tag, typ, n := protowire.ConsumeTag(wire)
		if n < 0 || typ != protowire.VarintType {
			t.Fatal("invalid supplemental battle data")
		}
		value, m := protowire.ConsumeVarint(wire[n:])
		if m < 0 {
			t.Fatal("invalid supplemental battle value")
		}
		values[tag], wire = int64(value), wire[n+m:]
	}
	return values
}

func TestDungeonAdmissionVigorAcrossBothSockets(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.config.BattleDurationSeconds = 120
	peer, conn := auctionTestRole(t, s, "explorer", nil)
	if err := s.roles.Update(peer.roleID, func(r Record) error {
		r["CurrentVigor"], r["AlchemyWard"] = 10, 34
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	build := func(region string) string {
		reply := spellTestCall(t, s, peer, 1010001, "BuildDungeonReq_1010001", "BuildDungeonResp_1010001", Record{"MapId": region})
		return getString(reply, "DungeonId")
	}
	join := func(id string, want int64) {
		t.Helper()
		out := spellTestCall(t, s, peer, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"DungeonId": id})
		if getInt(out, "Code") != want {
			t.Fatal(out)
		}
	}
	enter := func(id string, version, want int64) {
		t.Helper()
		req := s.pb("BattleRoleEnterReq_8001001")
		putPB(req, Record{"RoleId": peer.roleID, "DungeonId": id, "WorldVersion": version})
		body, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		s.handleBattle(peer, Message{Cmd: 8001001, Msid: 35, Data: body})
		out := conn.receive(t, s, 8001001, 35, "BattleRoleEnterResp_8001001")
		if getInt(out, "Code") != want {
			t.Fatal(out)
		}
	}
	remaining := func(want int64) {
		t.Helper()
		if got := roleInt(s.roles.Get(peer.roleID), "CurrentVigor"); got != want {
			t.Fatalf("remaining vigor %d, want %d", got, want)
		}
	}
	normal := build("1111W11")
	deep := build("1111W12")
	lair := build("1111W13")
	remaining(10)
	path := s.roles.path
	s.roles.path = dir
	join(normal, 5000)
	s.roles.path = path
	if r := s.roles.Get(peer.roleID); roleBool(r, "FirstDungeonAdmission") || roleInt(r, "AlchemyWard") != 34 {
		t.Fatalf("failed save spent introduction or ward: %v", r)
	}
	join(normal, 4502)
	enter(normal, 0, 4506)
	remaining(10)
	enter(deep, 2, 4503)
	remaining(10)
	if err := s.roles.Update(peer.roleID, func(r Record) error { r["AlchemyWard"] = 17; return nil }); err != nil {
		t.Fatal(err)
	}
	enter(deep, 0, 4506) // Legacy entry bypassing the game admission handler.
	if roleInt(s.roles.Get(peer.roleID), "AlchemyWard") != 17 {
		t.Fatal("a direct scene entry spent a ward it cannot deliver")
	}
	join(deep, 4502)
	enter(deep, 0, 4506)
	remaining(6)
	lairDungeon := s.dungeonForRole(lair, peer.roleID, false)
	var group sync.WaitGroup
	for range 10 {
		group.Go(func() {
			if _, code := s.admitDungeon(lairDungeon); code != 4502 {
				t.Errorf("concurrent entry denied: %d", code)
			}
		})
	}
	group.Wait()
	remaining(0)
	if err := s.roles.Update(peer.roleID, func(r Record) error { r["AlchemyWard"] = 26; return nil }); err != nil {
		t.Fatal(err)
	}
	fourth := build("1111W11")
	enter(fourth, 0, 4290)
	join(fourth, 4290)
	join("expired-dungeon-id", 4503)
	enter("expired-dungeon-id", 0, 4503)
	remaining(0)
	if roleInt(s.roles.Get(peer.roleID), "AlchemyWard") != 26 {
		t.Fatal("unaffordable admission spent ward")
	}
	// Resume from the durable role save. New servers cannot bind old dungeon IDs.
	s = auctionTestServer(t, dir)
	s.config.BattleDurationSeconds = 120
	peer, conn = auctionTestRole(t, s, "explorer", nil)
	join(lair, 4503)
	join(build("1111W11"), 4290)
	if err := s.roles.Update(peer.roleID, func(r Record) error { r["CurrentVigor"] = 2; return nil }); err != nil {
		t.Fatal(err)
	}
	join(build("1111W11"), 4502)
	remaining(0)
}

func TestDungeonValuePerVigorAtLevel80(t *testing.T) {
	s := acupointTestServer(t, t.TempDir())
	var err error
	s.config, err = readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	for _, region := range []struct {
		id              string
		cost            int64
		rewardPct, gear float64
	}{
		{"1111W11", 2, .06975, .405}, {"1111W12", 4, .153, .910}, {"1111W13", 6, .234, 1.55},
	} {
		role := testBattleRole(s, 80, false)
		wave := s.createMonsters(role, region.id)
		count := 0.0
		for _, m := range wave {
			s.grantBattleCultivation(role, m)
			count += dropRates(m.unusual, 0).equipment
		}
		air := (float64(roleInt(role, "SuperAir")) + roleFloat(role, "BattleAirRemainder")) * 100 / s.levelInfo(80).upgradeLimit
		if dungeonVigorCost(region.id) != region.cost || math.Abs(air-region.rewardPct) > 1e-6 || math.Abs(count-region.gear) > 1e-6 {
			t.Fatalf("level 80 %s cost/reward%%/gear: %d / %f / %f", region.id, dungeonVigorCost(region.id), air, count)
		}
	}
}
