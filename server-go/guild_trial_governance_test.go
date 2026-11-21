package main

import (
	"errors"
	"slices"
	"sync"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func guildGovernanceTestServer(t *testing.T, dir string) *Server {
	t.Helper()
	s := auctionTestServer(t, dir)
	var err error
	s.guilds, err = OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	s.guilds.enableHistory()
	return s
}

func TestGuildGovernanceClientLifecycle(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{
		"12": Record{"Name": "测试宗", "LeaderId": "old", "Members": []string{"new", "first", "second"}},
		"13": Record{"Name": "他宗", "LeaderId": "other"},
	}}); err != nil {
		t.Fatal(err)
	}
	peers := make(map[string]*Peer)
	for _, id := range []string{"old", "new", "first", "second", "other"} {
		if err := s.roles.Put(id, Record{"UserId": "user-" + id, "Name": "name-" + id}); err != nil {
			t.Fatal(err)
		}
		peers[id], _ = auctionTestRole(t, s, id, nil)
	}
	call := func(id string, cmd int, values Record, success bool, code int64) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, values)
		peer := peers[id]
		s.handleGame(peer, msg)
		out := auctionTestReply(t, s, peer.conn.(*auctionTestConnection), msg, code)
		if getBool(out, "IsSuccess") != success {
			t.Fatalf("%d %s: %v", cmd, id, out)
		}
		return out
	}
	appoint := func(actor, target string, office int, success bool, code int64) {
		t.Helper()
		call(actor, 1015025, Record{"RoleId": target, "Type": office}, success, code)
	}
	list := func(want map[int64]string) {
		t.Helper()
		out := call("new", 1015023, nil, true, 2000)
		rows := out.Get(field(out, "HillZhangLaoInfo")).List()
		if rows.Len() != 16 {
			t.Fatalf("client needs all 16 office choices: %v", out)
		}
		for i := range rows.Len() {
			row := rows.Get(i).Message()
			if getInt(row, "Type") != int64(i) || getString(row, "RoleName") != want[int64(i)] {
				t.Fatalf("office %d: %v", i, row)
			}
		}
	}
	target := func(id string) Record { return Record{"RoleId": id} }
	call("old", 1015014, nil, false, 4437)
	call("old", 1015013, target("old"), false, 4437)
	call("other", 1015013, target("new"), false, 4644)
	call("first", 1015022, target("second"), false, 4687)
	appoint("new", "first", 1, false, 4687)
	appoint("old", "first", -1, false, 4008)
	appoint("old", "old", 0, false, 4646)
	appoint("old", "other", 1, false, 4644)
	appoint("old", "new", 1, true, 4647)
	appoint("old", "first", 1, false, 4646)
	appoint("old", "new", 1, false, 4646)
	appoint("old", "new", 2, true, 4647)
	appoint("old", "first", 1, true, 4647)
	list(map[int64]string{1: "name-first", 2: "name-new", 15: "name-old"})
	appoint("old", "first", 0, true, 4647)
	call("old", 1015022, target("new"), true, 4645)
	call("old", 1015022, target("second"), false, 4687)
	list(map[int64]string{15: "name-new"})
	appoint("new", "old", 15, true, 4645)
	appoint("old", "new", 15, true, 4645)
	appoint("new", "first", 3, true, 4647)
	call("new", 1015013, target("first"), true, 2000)
	peers["first"].conn.(*auctionTestConnection).receive(t, s, 1015019, 0, "GuildExpelRoleInformResp_1015019")
	call("new", 1015013, target("first"), false, 4644)
	appoint("new", "second", 3, true, 4647)
	call("second", 1015014, nil, true, 2000)
	call("second", 1015014, nil, false, 4420)
	list(map[int64]string{15: "name-new"})

	s = guildGovernanceTestServer(t, dir)
	for id := range peers {
		peers[id], _ = auctionTestRole(t, s, id, nil)
		login := s.pb("RoleLoginResp_1002002")
		s.fillGuildLogin(id, login)
		expected := map[string]int64{"old": 12, "new": 12, "other": 13}
		if getInt(login, "GuildId") != expected[id] {
			t.Fatalf("reopened guild of %s: %v", id, login)
		}
	}
	list(map[int64]string{15: "name-new"})
	roster := call("old", 1015008, nil, true, 2000)
	rows := roster.Get(field(roster, "RoleList")).List()
	if rows.Len() != 2 || getInt(rows.Get(0).Message(), "ZhiWei") != 15 || getInt(rows.Get(1).Message(), "ZhiWei") != 0 {
		t.Fatalf("reopened roster %v", roster)
	}
	call("first", 1015004, Record{"GuildId": 13}, false, 4695)
	call("second", 1015004, Record{"GuildId": 12}, false, 4696)
	// Also enforce cooldown when a stale application already exists in a legacy save.
	if err := s.guilds.Update(trialGuildWorld, func(world Record) error {
		trialGuildByID(trialGuilds(world), 13).row["Applications"] = Record{"first": 1}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	call("other", 1015012, Record{"RoleId": "first", "IsAgree": true}, false, 4695)
	events, err := s.guilds.pendingHistory()
	if err != nil {
		t.Fatal(err)
	}
	counts := map[string]int{}
	for _, event := range events {
		counts[event.Kind]++
		if event.EntityID != "12" || event.Actor.RoleName != "name-"+event.Actor.RoleID ||
			event.Actor.AccountID != "user-"+event.Actor.RoleID {
			t.Fatalf("history identities: %v", event)
		}
		if event.Kind != "guild.left" && (event.Subject == nil || event.Subject.RoleName != "name-"+event.Subject.RoleID ||
			event.Subject.AccountID != "user-"+event.Subject.RoleID) {
			t.Fatalf("history missing target %v", event)
		}
	}
	for kind, want := range map[string]int{"guild.office_assigned": 5, "guild.office_revoked": 1,
		"guild.leadership_transferred": 3, "guild.expelled": 1, "guild.left": 1} {
		if counts[kind] != want {
			t.Fatalf("history counts: %v", counts)
		}
	}
	err = s.guilds.Transact(func(tx *StoreTx) error {
		for id, rule := range map[string]struct {
			code     previewReject
			duration time.Duration
		}{
			"first": {4695, 24 * time.Hour}, "second": {4696, 72 * time.Hour},
		} {
			roleMap := tx.View(guildRejoinRecord)["Roles"].(Record)
			until := time.UnixMilli(roleInt(roleMap[id].(Record), "Until"))
			started := until.Add(-rule.duration)
			if time.Since(started) < 0 || time.Since(started) > time.Minute {
				t.Fatalf("incorrect rejoin delay for %s: %v", id, until)
			}
			if err := guildRejoinCheck(tx, id, until.Add(-time.Millisecond)); !errors.Is(err, rule.code) {
				t.Fatalf("early rejoin %s: %v", id, err)
			}
			if err := guildRejoinCheck(tx, id, until); err != nil {
				t.Fatalf("expiry boundary %s: %v", id, err)
			}
		}
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
}

func TestGuildGovernanceConcurrentTransferAndRollback(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{
		"21": Record{"LeaderId": "lead", "Members": []string{"a", "b"}},
	}}); err != nil {
		t.Fatal(err)
	}
	for _, id := range []string{"lead", "a", "b"} {
		auctionTestRole(t, s, id, nil)
	}
	// Fail the atomic replace without modifying permissions (works under privileged test runners).
	path := s.guilds.path
	s.guilds.path = dir
	err := s.removeGuildMember("lead", "a", true)
	s.guilds.path = path
	if err == nil || s.guilds.Get(guildRejoinRecord) != nil {
		t.Fatal("failed expulsion committed state")
	}
	if events, err := s.guilds.pendingHistory(); err != nil || len(events) != 0 {
		t.Fatal(events, err)
	}

	var wg sync.WaitGroup
	results := make(chan error, 2)
	for _, id := range []string{"a", "b"} {
		wg.Go(func() {
			peer := new(Peer)
			peer.onlineRole.Store("lead")
			out := s.pb("XieRenResp_1015022")
			results <- s.changeGuildOffice(peer, id, guildLeaderOffice, out)
		})
	}
	wg.Wait()
	close(results)
	passed := 0
	for err := range results {
		if err == nil {
			passed++
		} else if !errors.Is(err, previewReject(4687)) {
			t.Fatal(err)
		}
	}
	reopened := guildGovernanceTestServer(t, dir)
	guild := trialGuildByID(trialGuilds(reopened.guilds.Get(trialGuildWorld)), 21)
	events, err := reopened.guilds.pendingHistory()
	if err != nil || passed != 1 || len(events) != 1 || guild.leader() == "lead" ||
		!slices.Contains(guild.members(), "a") || len(guild.members()) != 3 {
		t.Fatalf("partial or duplicated transfer: %d %v %v %v", passed, guild.row, events, err)
	}
}

func TestGamePushChecksCurrentRoleAtWrite(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	peer, conn := auctionTestRole(t, s, "before", nil)
	s.bindGameRole(peer, "after")

	// Simulate an old lookup still resolving to this connection while the role switch completes.
	s.online.Store("before", peer)
	s.pushGame("before", 1015019, nil)
	if len(conn.frames) != 0 {
		t.Fatal("expulsion for the previous role reached the new role")
	}
	s.pushGame("after", 1015019, nil)
	conn.receive(t, s, 1015019, 0, "GuildExpelRoleInformResp_1015019")
	out := s.gameResponse("GuildInformRoleJoinSuccessResp_1015018", true, 2004)
	set(out, "GuildId", 12)
	s.pushGame("after", 1015018, out)
	notice := conn.receive(t, s, 1015018, 0, "GuildInformRoleJoinSuccessResp_1015018")
	if getInt(notice, "GuildId") != 12 {
		t.Fatal(notice)
	}
}
