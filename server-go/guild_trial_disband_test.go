package main

import (
	"sync"
	"testing"
	"time"
)

func TestGuildDisbandAccessNotificationsRetriesAndReload(t *testing.T) {
	dir := t.TempDir()
	s := guildGovernanceTestServer(t, dir)
	err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{
		"11": Record{"Name": "主宗", "LeaderId": "leader", "Members": []string{"online", "offline"},
			"Applications": Record{"applicant": 3}, "Offices": Record{"2": "offline"}},
		"22": Record{"Name": "别宗", "LeaderId": "other", "Applications": Record{"applicant": 4}},
	}})
	if err != nil {
		t.Fatal(err)
	}
	first, _ := auctionTestRole(t, s, "leader", nil)
	second, _ := auctionTestRole(t, s, "leader", nil)
	member, memberConn := auctionTestRole(t, s, "online", nil)
	auctionTestRole(t, s, "offline", nil)
	s.online.Delete("offline")
	applicant, _ := auctionTestRole(t, s, "applicant", nil)
	auctionTestRole(t, s, "other", nil)
	call := func(peer *Peer, msid int) (int64, bool) {
		t.Helper()
		msg := Message{Cmd: 1015158, Msid: msid}
		s.handleGame(peer, msg)
		out := peer.conn.(*auctionTestConnection).receive(t, s, msg.Cmd, msid, "GuildJieSanResp_1015158")
		return getInt(out, "Code"), getBool(out, "IsSuccess")
	}
	s.previewWrites = false
	if code, ok := call(first, 1); ok || code != 4008 {
		t.Fatal("read-only preview disband", code, ok)
	}
	s.previewWrites = true
	if code, ok := call(member, 2); ok || code != 4687 {
		t.Fatal("regular member disband", code, ok)
	}
	if code, ok := call(applicant, 3); ok || code != 4420 {
		t.Fatal("nonmember disband", code, ok)
	}
	// An unwritable destination must preserve the guild and must not grant a replay receipt.
	oldPath := s.guilds.path
	s.guilds.path = dir
	if code, ok := call(first, 9); ok || code != 4008 {
		t.Fatal("failed write", code, ok)
	}
	s.guilds.path = oldPath
	if guild := s.currentGuild(member); guild.id != 11 || len(guild.applications()) != 1 {
		t.Fatal(guild)
	}
	if events, err := s.guilds.pendingHistory(); err != nil || len(events) != 0 {
		t.Fatal(events, err)
	}

	notify := &privatePushCapture{auctionTestConnection: memberConn, pushed: make(chan struct{}, 4)}
	member.conn = notify
	type result struct {
		peer *Peer
		code int64
		ok   bool
	}
	replies := make(chan result, 2)
	var wg sync.WaitGroup
	for _, p := range []*Peer{first, second} {
		wg.Go(func() {
			code, ok := call(p, 9)
			replies <- result{p, code, ok}
		})
	}
	wg.Wait()
	close(replies)
	var winner *Peer
	for r := range replies {
		if r.ok && r.code == 2000 && winner == nil {
			winner = r.peer
			continue
		}
		if r.ok || r.code != 4420 {
			t.Fatalf("concurrent disband returned %+v", r)
		}
	}
	if winner == nil {
		t.Fatal("no successful disband")
	}
	select {
	case <-notify.pushed:
	case <-time.After(2 * time.Second):
		t.Fatal("member did not receive 1015159")
	}
	memberConn.receive(t, s, 1015159, 0, "GuildJieSanRoleInformResp_1015159")
	if code, ok := call(winner, 9); !ok || code != 2000 {
		t.Fatal("lost acknowledgment", code, ok)
	}

	reopened := guildGovernanceTestServer(t, dir)
	guilds := trialGuilds(reopened.guilds.Get(trialGuildWorld))
	if len(guilds) != 1 || roleInt(trialGuildByID(guilds, 22).applications(), "applicant") != 4 {
		t.Fatalf("inconsistent removal of applications or other guild: %v", guilds)
	}
	for _, id := range []string{"leader", "online", "offline", "applicant", "other"} {
		out := reopened.pb("RoleLoginResp_1002002")
		reopened.fillGuildLogin(id, out)
		want := int64(0)
		if id == "other" {
			want = 22
		}
		if getInt(out, "GuildId") != want {
			t.Fatalf("relogged %s: %v", id, out)
		}
	}
	events, err := reopened.guilds.pendingHistory()
	if err != nil || len(events) != 3 {
		t.Fatal(events, err)
	}
	found := map[string]int{}
	for _, e := range events {
		if e.Actor.RoleID != "leader" || e.Actor.AccountID != "account-leader" || e.Actor.RoleName != "leader" ||
			e.EntityType != "guild" || e.EntityID != "11" || e.Details["guildName"] != "主宗" {
			t.Fatal(e)
		}
		switch e.Kind {
		case "guild.disbanded":
			if roleInt(e.Details, "memberCount") != 3 || roleInt(e.Details, "applicationCount") != 1 {
				t.Fatal(e)
			}
			found["leader"]++
		case "guild.membership_ended":
			if e.Subject == nil || e.Subject.RoleName != e.Subject.RoleID ||
				e.Subject.AccountID != "account-"+e.Subject.RoleID || e.Details["reason"] != "disbanded" {
				t.Fatal(e)
			}
			found[e.Subject.RoleID]++
		default:
			t.Fatal(e)
		}
	}
	for _, id := range []string{"leader", "online", "offline"} {
		if found[id] != 1 {
			t.Fatal(found)
		}
	}
	if reopened.guilds.Get(guildRejoinRecord) != nil {
		t.Fatal("disband imposed a voluntary exit or expulsion cooldown")
	}

	// Old empty notifications must not clear a later membership. An old request
	// on the same connection must not dissolve another guild that its sender now leads.
	err = s.guilds.Update(trialGuildWorld, func(world Record) error {
		guilds := trialGuilds(world)
		guilds["33"] = Record{"LeaderId": "leader", "Members": []string{"online"}}
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
	if code, ok := call(winner, 9); !ok || code != 2000 || s.currentGuild(member).id != 33 {
		t.Fatal("old request affected new guild", code, ok)
	}
	s.pushGuildMembership("online", 0, 1015159, nil)
	s.pushGuildMembership("online", 11, 1015018, nil)
	if len(memberConn.frames) != 0 || len(notify.pushed) != 0 {
		t.Fatal("late or duplicated guild notification reached the new guild")
	}
}
