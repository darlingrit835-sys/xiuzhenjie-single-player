package main

import (
	"encoding/json"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func sectTestDate(month time.Month, day, hour int) time.Time {
	return time.Date(2026, month, day, hour, 0, 0, 0, sectZone)
}

func TestSectCalendarSundayAndMonthEnd(t *testing.T) {
	// November 29 is Sunday; Monday Nov 30 has a separate monthly wave.
	for _, tt := range []struct {
		at, want time.Time
		big      bool
	}{
		{sectTestDate(11, 28, 12), sectTestDate(11, 29, 20), false},
		{sectTestDate(11, 29, 20), sectTestDate(11, 30, 20), true},
		{sectTestDate(11, 30, 20), sectTestDate(12, 6, 20), false},
		// May 31 is both month-end and Sunday: one wave, then June 7.
		{sectTestDate(5, 31, 19), sectTestDate(5, 31, 20), true},
		{sectTestDate(5, 31, 20), sectTestDate(6, 7, 20), false},
	} {
		got := nextSectDeadline(tt.at.UTC())
		if !got.Equal(tt.want) || sectLarge(got.Unix()) != tt.big {
			t.Fatalf("at %v next due %v (large %v)", tt.at, got, sectLarge(got.Unix()))
		}
	}
}

func TestSectAsyncCombatReloadAndRank(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	for _, id := range []string{"leader", "elder", "member", "outsider"} {
		if err := s.roles.Put(id, Record{"Name": id, "Level": 12, "CurrentPower": 120}); err != nil {
			t.Fatal(err)
		}
	}
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{"13": Record{"Name": "明月宗", "LeaderId": "leader", "Members": []string{"elder", "member"}, "Offices": Record{"4": "elder"}, "MapId": "1211W2", "Stage": 3}}}); err != nil {
		t.Fatal(err)
	}
	at := sectTestDate(11, 27, 10)
	read := func(id string) Record {
		t.Helper()
		v, e := s.sectCourt(id, nil, at)
		if e != nil {
			t.Fatal(e)
		}
		return v
	}
	act := func(id string, v Record, action, value string, want bool) Record {
		t.Helper()
		snap, e := s.sectCourt(id, &sectOrder{Action: action, Value: value, Revision: roleInt(v, "revision"), GuildID: roleInt(v, "guildId")}, at)
		if (e == nil) != want {
			t.Fatalf("%s %s returned %v", id, action, e)
		}
		return snap
	}
	if _, e := s.sectCourt("outsider", nil, at); e == nil {
		t.Fatal("outsider received sect snapshot")
	}
	leader := read("leader")
	if roleString(leader, "regionName") != "紫霞郡" || roleInt(leader, "workers") != 10 || roleInt(leader, "tier") != 1 {
		t.Fatal(leader)
	}
	act("member", leader, "assign", "mine", false)
	act("elder", leader, "assign", "mine", true)
	act("elder", read("elder"), "formation", "", false)
	at = at.Add(4 * time.Hour)
	mined := read("leader")
	if roleInt(mined, "ore") != 16*7*3 || roleInt(mined, "crystal") != 16*5 {
		t.Fatalf("manual mine accrual %v", mined)
	}
	formed := act("leader", mined, "formation", "", true)
	act("leader", mined, "formation", "", false)
	if roleInt(formed, "formation") != 2 || roleInt(formed, "ore") != 316 {
		t.Fatal(formed)
	}
	act("member", formed, "fight", "scout", true)
	at = at.Add(3 * time.Hour)
	// A teammate's submission does not invalidate another player's fight.
	saved := act("leader", formed, "fight", "strike", true)
	act("leader", formed, "fight", "strike", false)
	if !roleBool(saved, "joined") || len(saved["echoes"].([]Record)) != 2 {
		t.Fatal(saved)
	}
	if _, err := s.sectCourt("elder", &sectOrder{Action: "fight", Value: "ward", Revision: roleInt(saved, "revision"), GuildID: 99}, at); err == nil {
		t.Fatal("accepted stale guild identity")
	}
	if roleInt(saved, "merit") != 0 || roleInt(saved, "fang") != 0 {
		t.Fatal("paid before closure")
	}
	at = sectTestDate(11, 29, 20)
	err := s.settleSectWorld(at)
	if err != nil {
		t.Fatal(err)
	}
	s.guilds, err = OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	if err = s.settleSectWorld(at); err != nil {
		t.Fatal(err)
	}
	closed := read("leader")
	reports := closed["reports"].([]any)
	if len(reports) != 1 || !roleBool(reports[0].(Record), "victory") || roleInt(closed, "merit") != 6 || roleInt(closed, "fang") != 12 || roleBool(closed, "joined") || closed["waveKind"] != "large" {
		t.Fatal(closed)
	}
	if roleInt(read("member"), "merit") != 6 || roleInt(read("elder"), "merit") != 0 {
		t.Fatal("personal credits escaped participants")
	}
	redeemed := act("member", read("member"), "redeem", "lens", true)
	act("member", redeemed, "redeem", "lens", false)
	boosted := act("member", redeemed, "fight", "scout", true)
	if roleInt(boosted, "merit") != 1 || roleInt(boosted["treasures"].([]Record)[2], "owned") != 0 {
		t.Fatal(boosted)
	}
	act("member", boosted, "promote", "", false)
	promoted := act("leader", read("leader"), "promote", "", true)
	if roleInt(promoted, "stage") != 4 || roleString(promoted, "regionName") != "紫霞洲" || roleInt(promoted, "tier") != 2 {
		t.Fatal(promoted)
	}
	if roleInt(promoted, "target") != roleInt(boosted, "target") || roleInt(promoted, "winFang") != roleInt(boosted, "winFang") ||
		roleInt(promoted, "damage") != roleInt(boosted, "damage") || len(promoted["echoes"].([]Record)) != 1 {
		t.Fatal("promotion recalculated this wave or its existing combat record", promoted)
	}
	if first := reports[0].(Record); len(first["echoes"].([]any)) != 2 {
		t.Fatal("published echoes lost on reload")
	}
	// Enough surplus for subsequent promotions; each one binds its actual map ancestor.
	if err = s.guilds.Update(trialGuildWorld, func(w Record) error {
		g := trialGuildByID(trialGuilds(w), 13)
		st := g.row["Court"].(Record)
		st["Fang"] = 999
		st["Ore"] = 9999
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	for stage := int64(5); stage <= 9; stage++ {
		promoted = act("leader", read("leader"), "promote", "", true)
		if roleInt(promoted, "stage") != stage || stage >= 7 && roleString(promoted, "regionName") != "南岭" {
			t.Fatal(promoted)
		}
	}
	act("leader", promoted, "promote", "", false)
}

func TestSectLossGraceAndIdle(t *testing.T) {
	due := sectTestDate(5, 31, 20)
	makeCourt := func(start time.Time) (trialGuild, Record) {
		g := trialGuild{17, Record{"LeaderId": "solo", "MapId": "1111W1", "AiMemberCount": 6}}
		st := initSectCourt(g, start, "1111")
		st["Ore"] = 100
		return g, st
	}
	loss := func(start time.Time) (Record, Record) {
		g, st := makeCourt(start)
		r := st["Round"].(Record)
		r["Echoes"].(Record)["solo"] = Record{"member": "solo", "tactic": "strike", "damage": 1, "at": start.Unix()}
		r["Damage"] = 1
		advanceSectCourt(g, st, due)
		report := st["Reports"].([]any)[0].(Record)
		return st, report
	}
	st, live := loss(due.Add(-26 * time.Hour))
	if roleInt(live, "ore") != -16 || roleInt(live, "merit") != 4 || roleInt(live, "fang") != 0 || roleBool(live, "victory") {
		t.Fatal(live)
	}
	if roleInt(sectMap(sectMap(st, "Accounts"), "solo"), "Merit") != 4 {
		t.Fatal("no participation pay")
	}
	_, young := loss(due.Add(-time.Hour))
	if roleInt(young, "ore") != 0 || !roleBool(young, "protected") {
		t.Fatal(young)
	}
	g, idle := makeCourt(due.Add(-26 * time.Hour))
	advanceSectCourt(g, idle, due)
	if roleInt(idle, "SlowUntil") != due.Add(2*time.Hour).Unix() || roleInt(idle["Reports"].([]any)[0].(Record), "ore") != 0 {
		t.Fatal(idle)
	}
	startOre, startCrystal := roleInt(idle, "Ore"), roleInt(idle, "Crystal")
	for i := range 8 {
		advanceSectCourt(g, idle, due.Add(time.Duration(i+1)*15*time.Minute))
	}
	if roleInt(idle, "Ore")-startOre != 12 || roleInt(idle, "Crystal")-startCrystal != 4 {
		t.Fatal("quarterly polling changed half-rate yield")
	}
	advanceSectCourt(g, idle, due.Add(2*time.Hour+15*time.Minute))
	if roleInt(idle, "Ore")-startOre != 15 {
		t.Fatal("mine did not recover")
	}
	before := roleInt(idle, "Ore")
	advanceSectCourt(g, idle, due.AddDate(0, 9, 0))
	if len(idle["Reports"].([]any)) != 3 || roleInt(idle, "Ore") > before+72*4*3 {
		t.Fatal("unbounded catch-up or back penalties")
	}
}

func TestSectScheduledSettlementCannotRenewOfflineMine(t *testing.T) {
	start := sectTestDate(11, 20, 10)
	g := trialGuild{17, Record{"LeaderId": "solo", "MapId": "1111W1", "AiMemberCount": 6}}
	st := initSectCourt(g, start, "1111")
	for i := 1; i < 40; i++ {
		advanceSectCourt(g, st, start.AddDate(0, 0, i))
	}
	if got := roleInt(st, "Ore"); got > 3*4*72 || got < 3*4*72-12 {
		t.Fatalf("one offline period produced %d ore", got)
	}
	if len(st["Reports"].([]any)) != 7 {
		t.Fatal("rolling wave archive did not keep seven rounds")
	}
}

func TestSectFoundFromNormalMapAndWire(t *testing.T) {
	s := guildGovernanceTestServer(t, t.TempDir())
	peer, client := auctionTestRole(t, s, "founder", nil)
	if err := s.roles.Update("founder", func(v Record) error { v["MapId"] = "111201"; return nil }); err != nil {
		t.Fatal(err)
	}
	call := func(cmd int, data Record, want bool) protoreflect.Message {
		t.Helper()
		m := auctionTestMessage(t, s, cmd, data)
		s.handleGame(peer, m)
		out := client.receive(t, s, cmd, m.Msid, s.preview[cmd].response)
		if getBool(out, "IsSuccess") != want {
			t.Fatalf("cmd %d: %v", cmd, out)
		}
		return out
	}
	found := Record{"Name": "问道台", "SimpleName": "问道", "TokenIndex": 3, "LastNameIndex": 2}
	call(1015007, found, false)
	if x := call(1015001, Record{"MapId": "1111W1"}, true); x.Has(field(x, "SelfGuild")) {
		t.Fatal("land granted outside home county")
	}
	site := call(1015001, Record{"MapId": "1112W2"}, true)
	if getString(site.Get(field(site, "SelfGuild")).Message(), "GuildMapId") != "1112W2" {
		t.Fatal(site)
	}
	call(1004038, Record{"Id": "1112W1"}, false)
	call(1004038, Record{"Id": "1112W2"}, true)
	created := call(1015007, found, true)
	id := getInt(created, "GuildId")
	if id == 0 || getInt(call(1015007, found, true), "GuildId") != id || getInt(created, "ShinyJade") != 20 {
		t.Fatal(created)
	}
	call(1015005, nil, true)
	s.handleGame(peer, Message{Cmd: guildCourtRead, Msid: 21})
	read := client.receive(t, s, guildCourtRead, 21, "ActionBoolResp_1001001")
	var view Record
	if !getBool(read, "IsSuccess") || json.Unmarshal([]byte(offerToken(read.GetUnknown())), &view) != nil || roleString(view, "regionName") != "朝露郡" {
		t.Fatal(read)
	}
	cmd := Message{Cmd: guildCourtAct, Msid: 22}
	req, _ := json.Marshal(sectOrder{Action: "fight", Value: "strike", Revision: 1, GuildID: id})
	cmd.Data = offerString(nil, 3, string(req))
	s.handleGame(peer, cmd)
	if g := s.guilds.guildForRole("founder"); g.row["Court"].(Record)["Round"].(Record)["Echoes"].(Record)["founder"] == nil {
		t.Fatal("wire action did not persist")
	}
}

func TestSectRejectedFirstOrderCommitsClosingRound(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	auctionTestRole(t, s, "leader", nil)
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{"8": Record{"LeaderId": "leader", "MapId": "1111W2"}}}); err != nil {
		t.Fatal(err)
	}
	due := sectTestDate(11, 29, 20)
	before, err := s.sectCourt("leader", nil, due.Add(-time.Hour))
	if err != nil {
		t.Fatal(err)
	}
	order := sectOrder{Action: "fight", Value: "strike", GuildID: 8, Revision: roleInt(before, "revision")}
	before, err = s.sectCourt("leader", &order, due.Add(-time.Minute))
	if err != nil {
		t.Fatal(err)
	}
	order.Revision = roleInt(before, "revision")
	if _, err = s.sectCourt("leader", &order, due); err == nil {
		t.Fatal("old order accepted in following round")
	}
	reopened, err := OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	st := trialGuildByID(trialGuilds(reopened.Get(trialGuildWorld)), 8).row["Court"].(Record)
	round := st["Round"].(Record)
	accounts := st["Accounts"].(Record)
	if roleInt(round, "Deadline") <= due.Unix() || roleInt(accounts["leader"].(Record), "Merit") != 6 || len(st["Reports"].([]any)) != 1 || len(round["Echoes"].(Record)) != 0 {
		t.Fatal("the refusal did not commit the old round and its sole reward", st)
	}
}

func TestSectRepresentationSurvivesOfficesAndChangingGuild(t *testing.T) {
	s := guildGovernanceTestServer(t, t.TempDir())
	for _, id := range []string{"leader", "traveler", "ally"} {
		auctionTestRole(t, s, id, nil)
	}
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{
		"7": Record{"LeaderId": "leader", "Members": []string{"traveler"}, "Stage": 1, "MapId": "1111W1"},
		"8": Record{"LeaderId": "ally", "Members": []string{}, "Stage": 1, "MapId": "1112W3"},
	}}); err != nil {
		t.Fatal(err)
	}
	at := sectTestDate(11, 25, 13)
	snap, err := s.sectCourt("traveler", nil, at)
	if err != nil {
		t.Fatal(err)
	}
	order := sectOrder{Action: "fight", Value: "ward", Revision: roleInt(snap, "revision"), GuildID: 7}
	snap, err = s.sectCourt("traveler", &order, at)
	if err != nil {
		t.Fatal(err)
	}
	if err = s.guilds.Update(trialGuildWorld, func(w Record) error {
		g := trialGuildByID(trialGuilds(w), 7)
		g.row["Offices"] = Record{"4": "traveler"}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	order.Revision = roleInt(snap, "revision")
	if _, err = s.sectCourt("traveler", &order, at); err == nil {
		t.Fatal("office change erased prior echo")
	}
	if err = s.guilds.Update(trialGuildWorld, func(w Record) error {
		gs := trialGuilds(w)
		g := trialGuildByID(gs, 7)
		g.row["Members"] = []string{}
		delete(g.row, "Offices")
		trialGuildByID(gs, 8).row["Members"] = []string{"traveler"}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	snap, err = s.sectCourt("traveler", nil, at)
	if err != nil {
		t.Fatal(err)
	}
	if !roleBool(snap, "joined") || !roleBool(snap, "joinedElsewhere") || len(snap["echoes"].([]Record)) != 0 {
		t.Fatal(snap)
	}
	order.GuildID, order.Revision = 8, roleInt(snap, "revision")
	if _, err = s.sectCourt("traveler", &order, at); err == nil {
		t.Fatal("same role fought for a second guild")
	}
	at = sectTestDate(11, 29, 20)
	snap, err = s.sectCourt("traveler", nil, at)
	if err != nil {
		t.Fatal(err)
	}
	if roleBool(snap, "joined") || roleBool(snap, "joinedElsewhere") {
		t.Fatal("new round retained old attendance")
	}
	order.Revision = roleInt(snap, "revision")
	if _, err = s.sectCourt("traveler", &order, at); err != nil {
		t.Fatal(err)
	}
}

func TestSectLegacyLandAndDisbandIdentity(t *testing.T) {
	s := guildGovernanceTestServer(t, t.TempDir())
	peer, client := auctionTestRole(t, s, "founder", nil)
	if err := s.roles.Update("founder", func(r Record) error { r["MapId"] = "131101"; return nil }); err != nil {
		t.Fatal(err)
	}
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{"1000001": Record{"LeaderId": "founder", "Name": "云栖阁", "MapId": "retired-plot", "Stage": 4}}}); err != nil {
		t.Fatal(err)
	}
	call := func(cmd int, args Record, want bool) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, args)
		s.handleGame(peer, msg)
		out := client.receive(t, s, cmd, msg.Msid, s.preview[cmd].response)
		if getBool(out, "IsSuccess") != want {
			t.Fatal(out)
		}
		return out
	}
	browse := call(1015001, Record{"MapId": "1311W1"}, true)
	if !browse.Has(field(browse, "SelfGuild")) {
		t.Fatal("unmigrated guild not visible in legacy leader's region")
	}
	at := time.Now()
	snap, err := s.sectCourt("founder", nil, at)
	if err != nil {
		t.Fatal(err)
	}
	if roleString(snap, "regionName") != "黄沙洲" || roleString(s.guilds.guildForRole("founder").row, "MapId") != "retired-plot" {
		t.Fatal(snap)
	}
	call(1015158, nil, true)
	params := Record{"Name": "云栖阁", "SimpleName": "云栖", "TokenIndex": 1, "LastNameIndex": 5}
	call(1015001, Record{"MapId": "1311W1"}, true)
	call(1015007, params, false)
	call(1004038, Record{"Id": "1311W1"}, true)
	auctionTestRole(t, s, "founder", nil)
	call(1015007, params, false)
	s.bindGameRole(peer, "founder")
	newID := getInt(call(1015007, params, true), "GuildId")
	if newID <= 1000001 {
		t.Fatal("disbanded guild identity reused", newID)
	}
	// A new connection or sequence must be used for another deliberate disband;
	// the legacy empty request deduplicates its original request sequence.
	msg := Message{Cmd: 1015158, Msid: 19}
	s.handleGame(peer, msg)
	if !getBool(client.receive(t, s, 1015158, 19, s.preview[1015158].response), "IsSuccess") {
		t.Fatal("disband failed")
	}
	call(1015007, params, false)
	if g := s.guilds.guildForRole("founder"); g.row != nil {
		t.Fatal("replayed founder operation re-created a dissolved guild")
	}
}
