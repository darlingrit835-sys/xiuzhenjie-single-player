package main

import (
	"encoding/json"
	"sync"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestSectPermitMethodsFoundingAndFailureRecovery(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	peer, client := auctionTestRole(t, s, "founder", nil)
	if err := s.roles.Update("founder", func(r Record) error {
		r["MapId"] = "111101"
		r["QuestTotalSweeps"] = 7
		r["ShinyStone"] = 700
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	charter := func(action string, revision int64, ok bool) Record {
		t.Helper()
		msg := Message{Cmd: guildCharterRead, Msid: 5}
		if action != "" {
			msg.Cmd = guildCharterAct
			req, _ := json.Marshal(sectOrder{Action: action, Revision: revision})
			msg.Data = offerString(nil, 3, string(req))
		}
		s.handleGame(peer, msg)
		resp := client.receive(t, s, msg.Cmd, msg.Msid, "ActionBoolResp_1001001")
		body := Record{}
		if err := json.Unmarshal([]byte(offerToken(resp.GetUnknown())), &body); err != nil || getBool(resp, "IsSuccess") != ok {
			t.Fatal(body, err)
		}
		return body
	}
	pb := func(cmd int, fields Record, ok bool) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, fields)
		s.handleGame(peer, msg)
		resp := client.receive(t, s, msg.Cmd, msg.Msid, s.preview[msg.Cmd].response)
		if getBool(resp, "IsSuccess") != ok {
			t.Fatal(resp)
		}
		return resp
	}
	initial := charter("", 0, true)
	if roleInt(initial, "permits") != 0 || roleInt(initial, "fragments") != 7 || !roleBool(initial, "canGrant") || roleInt(initial, "price") != 600 {
		t.Fatal(initial)
	}
	pb(1015001, Record{"MapId": "1111W1"}, true)
	pb(1004038, Record{"Id": "1111W1"}, true)
	found := Record{"Name": "晚霞宗", "SimpleName": "晚霞", "TokenIndex": 4}
	pb(1015007, found, false)
	charter("grant", 0, true)
	charter("grant", 0, true)
	charter("grant", 1, false)
	changed := charter("exchange", 1, true)
	if roleInt(changed, "fragments") != 4 || roleInt(changed, "permits") != 2 {
		t.Fatal(changed)
	}
	charter("exchange", 1, true)
	badpath := s.roles.path
	s.roles.path = dir
	charter("buy", 2, false)
	s.roles.path = badpath
	if r := s.roles.Get("founder"); roleInt(r, "ShinyStone") != 700 || roleInt(r, "SectPermitsIssued") != 2 {
		t.Fatal("failed purchase charged", r)
	}
	paid := charter("buy", 2, true)
	charter("buy", 2, true)
	if roleInt(paid, "permits") != 3 || roleInt(paid, "stones") != 100 || roleBool(paid, "canBuy") {
		t.Fatal(paid)
	}
	charter("buy", 3, false)
	gpath := s.guilds.path
	s.guilds.path = dir
	pb(1015007, found, false)
	s.guilds.path = gpath
	if check := charter("", 0, true); roleInt(check, "permits") != 3 {
		t.Fatal("failed guild commit consumed permit", check)
	}
	a := pb(1015007, found, true)
	b := pb(1015007, found, true)
	if getInt(a, "GuildId") != getInt(b, "GuildId") || getInt(a, "ShinyStone") != 100 {
		t.Fatal(a, b)
	}
	var err error
	s.roles, err = OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	s.guilds, err = OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	prices := charter("", 0, true)
	if roleInt(prices, "permits") != 2 || roleInt(prices, "issued") != 3 || roleInt(prices, "fragments") != 4 || roleInt(prices, "stones") != 100 {
		t.Fatal(prices)
	}
	charter("exchange", 3, true)
	charter("exchange", 4, false)
}

func TestSectOldFirstGrantSurvivesGuildSaveFailure(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	p, c := auctionTestRole(t, s, "old-client", nil)
	if err := s.roles.Update("old-client", func(r Record) error { r["MapId"] = "111101"; r["QuestSweeps"] = 2; return nil }); err != nil {
		t.Fatal(err)
	}
	call := func(cmd int, data Record, ok bool) {
		t.Helper()
		m := auctionTestMessage(t, s, cmd, data)
		s.handleGame(p, m)
		r := c.receive(t, s, m.Cmd, m.Msid, s.preview[cmd].response)
		if getBool(r, "IsSuccess") != ok {
			t.Fatal(r)
		}
	}
	call(1015001, Record{"MapId": "1111W4"}, true)
	call(1004038, Record{"Id": "1111W4"}, true)
	found := Record{"Name": "朝云派", "SimpleName": "朝云", "TokenIndex": 1}
	path := s.guilds.path
	s.guilds.path = dir
	call(1015007, found, false)
	s.guilds.path = path
	read, _ := s.sectCharter("old-client", nil)
	if roleInt(read, "permits") != 1 || roleInt(read, "fragments") != 2 || roleBool(read, "canGrant") {
		t.Fatal(read)
	}
	call(1015007, found, true)
	call(1015007, found, true)
	read, _ = s.sectCharter("old-client", nil)
	if roleInt(read, "permits") != 0 || roleInt(read, "issued") != 1 {
		t.Fatal(read)
	}
	// A real SWEEP event increments the known lifetime total independently of active quest requirements.
	if err := s.roles.Update("old-client", func(r Record) error {
		s.ensureQuest(r)
		s.grantQuestEvent(r, "SWEEP")
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	read, _ = s.sectCharter("old-client", nil)
	if roleInt(read, "fragments") != 3 {
		t.Fatal(read)
	}
}

func TestSectConstructionPreviewAndCosts(t *testing.T) {
	s := guildGovernanceTestServer(t, t.TempDir())
	auctionTestRole(t, s, "leader", nil)
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{"4": Record{"LeaderId": "leader", "Stage": 3, "MapId": "1112W1"}}}); err != nil {
		t.Fatal(err)
	}
	at := sectTestDate(11, 24, 13)
	first, err := s.sectCourt("leader", nil, at)
	if err != nil {
		t.Fatal(err)
	}
	if roleInt(first, "buildOre") != 720 || roleInt(first, "buildCrystal") != 288 || roleInt(first, "nextStage") != 4 ||
		roleInt(first, "workers") != 10 || roleInt(first, "nextWorkers") != 12 || roleInt(first, "oreRate") != 60 || roleInt(first, "nextOreRate") != 96 ||
		roleInt(first, "crystalRate") != 12 || roleInt(first, "nextCrystalRate") != 24 || roleString(first, "nextRegionName") != "青云洲" ||
		roleInt(first, "formationCap") != 5 || roleInt(first, "nextFormationCap") != 6 || roleInt(first, "stipend") != 60 || roleInt(first, "nextStipend") != 70 {
		t.Fatal(first)
	}
	order := sectOrder{Action: "promote", Value: "build", Revision: roleInt(first, "revision"), GuildID: 4}
	edit := func(ore, crystal, formation int) {
		t.Helper()
		if err := s.guilds.Update(trialGuildWorld, func(w Record) error {
			st := trialGuildByID(trialGuilds(w), 4).row["Court"].(Record)
			st["Ore"], st["Crystal"], st["Formation"], st["Fang"] = ore, crystal, formation, 1
			return nil
		}); err != nil {
			t.Fatal(err)
		}
	}
	for _, set := range [][3]int{{719, 288, 3}, {720, 287, 3}, {720, 288, 2}} {
		edit(set[0], set[1], set[2])
		if _, e := s.sectCourt("leader", &order, at); e == nil {
			t.Fatal("accepted missing upgrade requirement", set)
		}
	}
	edit(721, 290, 3)
	up, err := s.sectCourt("leader", &order, at)
	if err != nil {
		t.Fatal(err)
	}
	if roleInt(up, "stage") != 4 || roleInt(up, "buildOre") != 2560 || roleInt(up, "buildCrystal") != 1024 || roleInt(up, "ore") != 1 || roleInt(up, "crystal") != 2 || roleInt(up, "fang") != 1 || roleString(up, "regionName") != "青云洲" {
		t.Fatal(up)
	}
	if _, e := s.sectCourt("leader", &order, at); e == nil {
		t.Fatal("replayed construction payment")
	}
	for stage, want := range map[int64]int64{1: 80, 6: 5760, 7: 11760, 8: 15360} {
		o, c := sectBuildCosts(stage)
		if o != want || c != want*2/5 {
			t.Fatal(stage, o, c)
		}
	}
}

func TestSectStipendAcrossMidnightAndGuildChange(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	auctionTestRole(t, s, "member", nil)
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{
		"1": Record{"LeaderId": "first", "Members": []string{"member"}, "Stage": 2},
		"2": Record{"LeaderId": "second", "Stage": 9},
	}}); err != nil {
		t.Fatal(err)
	}
	at := sectTestDate(11, 21, 0).Add(-time.Second)
	read, err := s.sectCourt("member", nil, at)
	if err != nil {
		t.Fatal(err)
	}
	if roleInt(read, "stipend") != 50 || roleString(read, "stipendDay") != "20261120" || roleBool(read, "stipendClaimed") {
		t.Fatal(read)
	}
	req := sectOrder{Action: "stipend", GuildID: 1, Value: "20261120", Revision: roleInt(read, "revision")}
	s.roles.path = dir
	if _, e := s.sectCourt("member", &req, at); e == nil {
		t.Fatal("unwritable wallet was paid")
	}
	s.roles.path = dir + "/roles.json"
	var wg sync.WaitGroup
	for range 6 {
		wg.Go(func() {
			if _, e := s.sectCourt("member", &req, at); e != nil {
				t.Error(e)
			}
		})
	}
	wg.Wait()
	// Role-day is global across guilds and durable across process restart.
	var openErr error
	s.roles, openErr = OpenStore(dir, "roles")
	if openErr != nil {
		t.Fatal(openErr)
	}
	if err = s.guilds.Update(trialGuildWorld, func(w Record) error {
		gs := trialGuilds(w)
		delete(trialGuildByID(gs, 1).row, "Members")
		trialGuildByID(gs, 2).row["Members"] = []string{"member"}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	req.GuildID = 2
	dup, err := s.sectCourt("member", &req, at)
	if err != nil || !roleBool(dup, "stipendClaimed") || roleInt(dup, "walletStones") != 1050 {
		t.Fatal(dup, err)
	}
	next := at.Add(time.Second).UTC()
	if _, err = s.sectCourt("member", &req, next); err == nil {
		t.Fatal("late previous-day request earned another stipend")
	}
	read, err = s.sectCourt("member", nil, next)
	if err != nil || roleBool(read, "stipendClaimed") || roleInt(read, "stipend") != 120 {
		t.Fatal(read, err)
	}
	req.Value = roleString(read, "stipendDay")
	paid, err := s.sectCourt("member", &req, next)
	if err != nil || roleInt(paid, "walletStones") != 1170 || !roleBool(paid, "stipendClaimed") {
		t.Fatal(paid, err)
	}
	at = next.AddDate(0, 0, 4)
	req.Value = sectSalaryDay(at)
	paid, err = s.sectCourt("member", &req, at)
	if err != nil || roleInt(paid, "walletStones") != 1290 {
		t.Fatal("backfilled missed stipend", paid, err)
	}
}

func TestSectLegacyFragmentsSurviveActualQuestSwitchAndRestart(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	peer, remote := auctionTestRole(t, s, "old", nil)
	s.game.lastTask = 20
	if err := s.roles.Update("old", func(r Record) error {
		r["TaskId"], r["TaskStatus"] = 2, 1
		r["QuestTotalSweeps"], r["QuestSweeps"] = 1, 7
		s.grantQuestEvent(r, "SWEEP")
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	if total := roleInt(s.roles.Get("old"), "QuestTotalSweeps"); total != 8 {
		t.Fatalf("sweep lifetime=%d, want 8", total)
	}
	view, err := s.sectCharter("old", &sectOrder{Action: "exchange", Revision: 0})
	if err != nil || roleInt(view, "fragments") != 5 {
		t.Fatal(view, err)
	}
	req := s.pb("ReceiveAwardReq_1016002")
	set(req, "TaskId", int64(2))
	body, _ := proto.Marshal(req)
	s.handleGame(peer, Message{Cmd: 1016002, Msid: 3, Data: body})
	resp := remote.receive(t, s, 1016002, 3, "ReceiveAwardResp_1016002")
	if !getBool(resp, "IsSuccess") {
		t.Fatal(resp)
	}
	s.roles, err = OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	role := s.roles.Get("old")
	view, err = s.sectCharter("old", nil)
	if err != nil || roleInt(role, "QuestSweeps") != 0 || roleInt(role, "TaskId") != 3 || roleInt(view, "sweeps") != 8 || roleInt(view, "fragments") != 5 {
		t.Fatal(role, view, err)
	}
}

func TestSectConcurrentPurchaseMintsAndChargesOnce(t *testing.T) {
	s := guildGovernanceTestServer(t, t.TempDir())
	auctionTestRole(t, s, "traveler", nil)
	req := sectOrder{Action: "buy", Revision: 0}
	var buyers sync.WaitGroup
	for range 8 {
		buyers.Go(func() {
			view, err := s.sectCharter("traveler", &req)
			if err != nil || roleInt(view, "issued") != 1 || roleInt(view, "stones") != 400 {
				t.Error(view, err)
			}
		})
	}
	buyers.Wait()
	r := s.roles.Get("traveler")
	if roleInt(r, "ShinyStone") != 400 || roleInt(r, "SectPermitsIssued") != 1 {
		t.Fatal(r)
	}
}
