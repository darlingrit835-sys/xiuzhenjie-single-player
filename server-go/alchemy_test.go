package main

import (
	"encoding/json"
	"reflect"
	"strings"
	"testing"

	"google.golang.org/protobuf/proto"
)

func alchemyTestServer(t *testing.T, dir string) (*Server, *Peer) {
	t.Helper()
	s, p := workshopTestServer(t, dir)
	if err := s.loadAlchemy(); err != nil {
		t.Fatal(err)
	}
	if s.alchemy == nil {
		t.Fatal("missing alchemy table")
	}
	return s, p
}

func brewCall(t *testing.T, s *Server, peer *Peer, req brewRequest, ok bool) Record {
	t.Helper()
	payload, err := json.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	s.handleGame(peer, Message{Cmd: alchemyCommand, Msid: 9, Data: offerString(nil, 3, string(payload))})
	out := peer.conn.(*auctionTestConnection).receive(t, s, alchemyCommand, 9, "ActionBoolResp_1001001")
	var body Record
	if json.Unmarshal([]byte(offerToken(out.GetUnknown())), &body) != nil || getBool(out, "IsSuccess") != ok {
		t.Fatalf("%s: %v (%v)", req.Action, out, body)
	}
	return body
}

func brewTestRoll(t *testing.T, s *Server, roll int) activeBrew {
	t.Helper()
	var run activeBrew
	err := s.roles.Update("artist", func(role Record) error {
		if err := json.Unmarshal([]byte(roleString(role, "AlchemyRun")), &run); err != nil {
			return err
		}
		run.SuccessRoll = roll
		data, _ := json.Marshal(run)
		role["AlchemyRun"] = string(data)
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
	return run
}

func centeredTaps(stages [3]brewStage) []int {
	taps := make([]int, len(stages))
	for i, s := range stages {
		target := (s.Direction*s.TargetBps + 10000) % 10000
		taps[i] = (target*s.PeriodMs + 5000) / 10000
	}
	return taps
}

func TestAlchemyWireSettlementMemoryAndRestart(t *testing.T) {
	dir := t.TempDir()
	s, p := alchemyTestServer(t, dir)
	req := func(act, token string) brewRequest {
		return brewRequest{Action: act, RecipeID: "dan.ninglu", RequestID: strings.Repeat(token, 32)}
	}
	d := brewCall(t, s, p, req("catalog", ""), true)
	if len(battleRecords(d["recipes"])) != len(s.alchemy.recipes) || len(battleRecords(d["materials"])) != len(s.alchemy.materials) || roleInt(d, "rank") != 0 {
		t.Fatal(d)
	}
	brewCall(t, s, p, req("start", "1"), false)
	brewCall(t, s, p, req("plan", ""), true)
	supply := req("supply", "2")
	brewCall(t, s, p, supply, true)
	brewCall(t, s, p, supply, true)
	if forgeTestCount(s.roles.Get("artist"), "mat.ninglu_herb") != 8 {
		t.Fatal("double supply")
	}
	brewCall(t, s, p, req("replay", "3"), false)
	start := req("start", "4")
	start.AuxIDs = []string{"mat.cold_dew"}
	first := brewCall(t, s, p, start, true)
	if !reflect.DeepEqual(first, brewCall(t, s, p, start, true)) {
		t.Fatal("start retry changed the dial")
	}
	run := brewTestRoll(t, s, 9999)
	finish := req("finish", "5")
	finish.RunID = start.RequestID
	finish.Taps = centeredTaps(run.Plan.Stages)
	failed := brewCall(t, s, p, finish, true)
	if roleBool(failed, "brewed") || roleInt(failed, "quantity") != 0 || roleInt(failed, "mastery") != 2 || roleInt(failed, "score") != 100 {
		t.Fatal(failed)
	}
	brewCall(t, s, p, req("replay", "3"), false)
	start.RequestID = strings.Repeat("6", 32)
	brewCall(t, s, p, start, true)
	run = brewTestRoll(t, s, 0)
	finish.RequestID, finish.RunID = strings.Repeat("7", 32), start.RequestID
	finish.Taps = centeredTaps(run.Plan.Stages)
	// A run survives the restart before the first successful settlement.
	s, p = alchemyTestServer(t, dir)
	batch := brewCall(t, s, p, finish, true)
	if !roleBool(batch, "brewed") || roleString(batch, "quality") != "上品" || roleInt(batch, "mastery") != 8 {
		t.Fatal(batch)
	}
	// The identical finish can be recovered after server restart without re-consuming stock.
	s, p = alchemyTestServer(t, dir)
	if !reflect.DeepEqual(batch, brewCall(t, s, p, finish, true)) {
		t.Fatal("lost receipt on restart")
	}
	role := s.roles.Get("artist")
	if forgeTestCount(role, "mat.ninglu_herb") != 4 || forgeTestCount(role, "mat.cold_dew") != 1 || roleInt(role, "AlchemyRank") != 1 {
		t.Fatal(role)
	}
	var record Record
	for _, r := range battleRecords(brewCall(t, s, p, req("catalog", ""), true)["recipes"]) {
		if roleString(r, "id") == "dan.ninglu" {
			record = r
			break
		}
	}
	if roleString(record, "memoryQuality") != "上品" || record["memoryAuxIds"].([]any)[0] != "mat.cold_dew" {
		t.Fatal(record)
	}
	direct := req("replay", "8")
	replay := brewCall(t, s, p, direct, true)
	if roleString(replay, "quality") != "上品" || roleInt(replay, "effect") == roleInt(batch, "effect") || roleInt(replay, "successBps") != 10000 || roleInt(replay, "mastery") != 10 {
		t.Fatal(batch, replay)
	}
	direct.AuxIDs = []string{"mat.wild_herb"}
	brewCall(t, s, p, direct, false)
	after := s.roles.Get("artist")
	if forgeTestCount(after, "mat.ninglu_herb") != 2 || roleInt(after, "AlchemyExperience") != roleInt(role, "AlchemyExperience")+5 {
		t.Fatal("wrong replay ingredient cost or half lesson")
	}
	// Returning to the recorded combination also reproduces its exact potency.
	// Two clients can retry one order concurrently without paying twice.
	original := req("replay", "9")
	original.AuxIDs = []string{"mat.cold_dew"}
	type answer struct {
		body Record
		err  error
	}
	answers := make(chan answer, 2)
	for range 2 {
		go func() { body, err := s.brewAction("artist", original); answers <- answer{body, err} }()
	}
	for range 2 {
		a := <-answers
		if a.err != nil || roleInt(a.body, "effect") != roleInt(batch, "effect") {
			t.Fatal(a)
		}
	}
	if forgeTestCount(s.roles.Get("artist"), "mat.ninglu_herb") != 0 {
		t.Fatal("recorded recipe charged incorrectly")
	}
	var brewed Record
	for _, g := range battleRecords(after["Goods"]) {
		if roleString(g, "BrewRecipe") == "dan.ninglu" && roleInt(g, "EffectValue") == roleInt(replay, "effect") {
			brewed = g
			break
		}
	}
	if brewed == nil || roleInt(brewed, "Rank") != 3 || roleString(brewed, "BrewEffect") != "recover_blood" {
		t.Fatal(after["Goods"])
	}
	for _, command := range []int{1004015, 1004032} {
		s.roles.Update("artist", func(role Record) error { role["PureBlood"], role["CurrentPureBlood"] = 500, 1; return nil })
		protoInfo := gameInventoryProtocols[command]
		pb := s.pb(protoInfo.req)
		if command == 1004015 {
			set(pb, "Id", roleString(brewed, "GoodsId"))
		} else {
			set(pb, "GoodsId", roleString(brewed, "GoodsId"))
		}
		body, _ := proto.Marshal(pb)
		s.handleGame(p, Message{Cmd: command, Msid: 7, Data: body})
		out := p.conn.(*auctionTestConnection).receive(t, s, command, 7, protoInfo.resp)
		if !getBool(out, "IsSuccess") {
			t.Fatal(out)
		}
		if command == 1004015 && int64(getFloat(out, "EffectValue")) != roleInt(replay, "effect") {
			t.Fatal(out)
		}
		if command == 1004032 && getInt(out, "CurrentPureBlood") != 1+roleInt(replay, "effect") {
			t.Fatal(out)
		}
	}
}
func TestAlchemyDifficultyAndAtomicFailure(t *testing.T) {
	s, p := alchemyTestServer(t, t.TempDir())
	w := s.alchemy
	base := brewRequest{Action: "plan", RecipeID: "dan.ninglu"}
	easy := base
	easy.AuxIDs = []string{"mat.cold_dew"}
	hard := base
	hard.AuxIDs = []string{"mat.frost_silver"}
	beginner, _ := w.plan(Record{}, hard)
	expected, _ := w.plan(Record{}, easy)
	expert, _ := w.plan(Record{"Live2Type": 1, "Live2Level": 6, "AlchemyJournal": Record{"dan.ninglu": Record{"Mastery": 80}}}, easy)
	if beginner.Affinity >= expected.Affinity || expected.BaseChance <= beginner.BaseChance || expert.BaseChance <= expected.BaseChance {
		t.Fatal(beginner, expected, expert)
	}
	for i := range beginner.Stages {
		a, b, c := beginner.Stages[i], expected.Stages[i], expert.Stages[i]
		if !(a.WindowBps < b.WindowBps && b.WindowBps < c.WindowBps && a.PeriodMs < b.PeriodMs && b.PeriodMs < c.PeriodMs) {
			t.Fatal(a, b, c)
		}
	}
	old := Record{"Goods": []any{
		Record{"GoodsId": "old", "TemplateId": "drop_9_1_4", "Type": 9, "Rank": 1, "Level": 4, "Count": 2},
		Record{"GoodsId": "herb", "TemplateId": "mat.ninglu_herb", "Type": 12, "Count": 2},
	}}
	oldPlan, _ := w.plan(old, brewRequest{RecipeID: "dan.ninglu", AuxIDs: []string{"mat.common_blood"}})
	if inputs, err := w.availableInputs(old, oldPlan); err != nil || inputs["drop_9_1_4"] != 1 {
		t.Fatal(inputs, err)
	}
	if _, err := w.plan(Record{}, brewRequest{RecipeID: "dan.chiyang"}); err == nil {
		t.Fatal("locked recipe")
	}
	for _, ids := range [][]string{{"fake"}, {"mat.ninglu_herb"}, {"mat.cold_dew", "mat.cold_dew", "mat.cold_dew", "mat.cold_dew"}} {
		invalid := base
		invalid.AuxIDs = ids
		if _, err := w.plan(Record{}, invalid); err == nil {
			t.Fatal(ids)
		}
	}
	brewCall(t, s, p, brewRequest{Action: "supply", RequestID: strings.Repeat("a", 32)}, true)
	start := easy
	start.Action, start.RequestID = "start", strings.Repeat("b", 32)
	brewCall(t, s, p, start, true)
	run := brewTestRoll(t, s, 0)
	// Fill the ring with no stack freed by these ingredients. Failed delivery rolls back the entire batch.
	s.roles.Update("artist", func(role Record) error {
		rows := goodsList(role)
		for len(rows) < ringCapacity(role) {
			rows = append(rows, Record{"GoodsId": newGameID(8), "Type": 15})
		}
		role["Goods"] = rows
		return nil
	})
	before := s.roles.Get("artist")
	f := brewRequest{Action: "finish", RequestID: strings.Repeat("c", 32), RunID: start.RequestID, Taps: centeredTaps(run.Plan.Stages)}
	brewCall(t, s, p, f, false)
	if !reflect.DeepEqual(before, s.roles.Get("artist")) {
		t.Fatal("lost inputs on full inventory")
	}
	s.roles.Update("artist", func(role Record) error { role["Goods"] = goodsList(role)[:99]; return nil })
	if !roleBool(brewCall(t, s, p, f, true), "brewed") {
		t.Fatal("cannot retry after freeing space")
	}
}

func TestAlchemyPillEffectsAndWard(t *testing.T) {
	s, peer := alchemyTestServer(t, t.TempDir())
	s.game.levels[1] = gameLevel{upgradeLimit: 100}
	role := Record{"Level": 1, "PureBlood": 200, "CurrentPureBlood": 182, "Vigor": 140, "CurrentVigor": 90, "SuperAir": 95, "CurrentPoisonVal": 13}
	for _, row := range []struct {
		effect, key string
		want        int64
	}{
		{"recover_blood", "CurrentPureBlood", 200}, {"recover_vigor", "CurrentVigor", 120}, {"gain_super_air", "SuperAir", 100}, {"reduce_poison", "CurrentPoisonVal", 0}, {"absorb_damage", "AlchemyWard", artisanWard(role, 30, 2000)},
	} {
		if s.consumeBrewDose(role, Record{"BrewEffect": row.effect, "EffectValue": 30}) != 2000 || roleInt(role, row.key) != row.want {
			t.Fatalf("%s: %v", row.effect, role)
		}
	}
	s.config.BattleDurationSeconds = 120
	if err := s.roles.Update("artist", func(r Record) error {
		r["AlchemyWard"], r["CurrentVigor"] = role["AlchemyWard"], 12
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	join := func(req Record, shield, vigor int64) string {
		reply := spellTestCall(t, s, peer, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", req)
		supplies := fieldEntrySupplies(t, reply.GetUnknown())
		if !getBool(reply, "IsSuccess") || supplies[29] != shield || supplies[30] != vigor {
			t.Fatalf("unexpected dungeon supplies %+v", supplies)
		}
		return getString(reply, "DungeonId")
	}
	ward := roleInt(role, "AlchemyWard")
	id := join(Record{"MapId": "1211W13"}, ward, 12)
	if roleInt(s.roles.Get("artist"), "AlchemyWard") != 0 {
		t.Fatal("ward not spent on entry")
	}
	join(Record{"DungeonId": id}, ward, 12)
	join(Record{"MapId": "1211W13"}, 0, 6)
}
