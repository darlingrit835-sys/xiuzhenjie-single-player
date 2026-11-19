package main

import (
	"bytes"
	"errors"
	"os"
	"path/filepath"
	"sync"
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestTrialGuildApplicationAndApproval(t *testing.T) {
	dir := t.TempDir()
	s, err := openServer(Config{}, dir)
	if err != nil {
		t.Fatal(err)
	}
	s.guilds, err = OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	s.guilds.enableHistory()
	for _, id := range []string{"leader", "other-leader", "member", "applicant", "rejected"} {
		if err := s.roles.Put(id, Record{
			"Id": id, "UserId": "account-" + id, "Name": "角色 " + id,
			"HeadPortraitId": 2, "CurrentAge": 18, "Level": 3,
		}); err != nil {
			t.Fatal(err)
		}
	}
	if err := s.guilds.Put(trialGuildWorld, Record{"Guilds": Record{
		"900001": Record{
			"Name": "临仙山门", "ShortName": "临仙", "MapId": "1111W1", "LeaderId": "leader",
			"Members": []string{"leader", "member"}, "Applications": Record{},
			"Stage": 1, "Fighting": 800, "Influence": 120, "AiMemberCount": 12,
		},
		"900002": Record{"Name": "朝露山门", "MapId": "1112W1", "LeaderId": "other-leader"},
	}}); err != nil {
		t.Fatal(err)
	}
	rolesPath := filepath.Join(dir, "roles.json")
	originalRoles, err := os.ReadFile(rolesPath)
	if err != nil {
		t.Fatal(err)
	}
	registry, err := loadPreview(s.protocol)
	if err != nil {
		t.Fatal(err)
	}
	prepare := func(cmd int, id string, values Record) (*Peer, compiledPreview, protoreflect.Message, protoreflect.Message) {
		route := registry[cmd]
		peer := new(Peer)
		peer.onlineRole.Store(id)
		var request protoreflect.Message
		if route.reqDesc != nil {
			request = s.pb(route.request)
			putPB(request, values)
		}
		out := responseStatus(route.respDesc, true, 2000)
		return peer, route, request, out
	}
	reply := func(cmd int, id string, values Record) protoreflect.Message {
		t.Helper()
		peer, route, request, out := prepare(cmd, id, values)
		_, err := route.live(s, peer, request, out)
		if err != nil {
			t.Fatalf("cmd=%d role=%s: %v", cmd, id, err)
		}
		return out
	}
	denied := func(cmd int, id string, values Record, expected previewReject) {
		t.Helper()
		peer, route, request, out := prepare(cmd, id, values)
		_, err := route.live(s, peer, request, out)
		if !errors.Is(err, expected) {
			t.Fatalf("cmd=%d role=%s: got %v, expected %v", cmd, id, err, expected)
		}
	}
	guildsPath := filepath.Join(dir, "guilds.json")
	assertNoGuildWrite := func(operation string, run func()) {
		t.Helper()
		before, err := os.Stat(guildsPath)
		if err != nil {
			t.Fatal(err)
		}
		run()
		after, err := os.Stat(guildsPath)
		if err != nil {
			t.Fatal(err)
		}
		if !os.SameFile(before, after) {
			t.Fatalf("%s rewrote the guild save", operation)
		}
	}
	list := reply(1015001, "applicant", Record{"MapId": "1111W1"})
	if list.Has(field(list, "SelfGuild")) || list.Get(field(list, "GuildList")).List().Len() != 1 {
		t.Fatal("a nonmember should see only the browse entry")
	}
	list = reply(1015001, "leader", Record{"MapId": "1111W1"})
	if !list.Has(field(list, "SelfGuild")) || list.Get(field(list, "GuildList")).List().Len() != 0 {
		t.Fatal("a leader should see its guild in the member slot")
	}
	detail := reply(1015003, "applicant", Record{"GuildId": 900001})
	if getString(detail, "GuildName") != "临仙山门" || getInt(detail, "GuildRoleCount") != 2 {
		t.Fatal("detail does not match persisted guild")
	}
	denied(1015005, "applicant", nil, 4008)

	apply := Record{"GuildId": 900001}
	if getInt(reply(1015004, "applicant", apply), "Code") != 4417 {
		t.Fatal("client submission message code is incorrect")
	}
	assertNoGuildWrite("application retry", func() {
		reply(1015004, "applicant", apply)
	})
	reply(1015004, "applicant", Record{"GuildId": 900002})
	reply(1015004, "rejected", apply)
	pending := reply(1015010, "leader", nil)
	pendingList := pending.Get(field(pending, "JoinList")).List()
	if pendingList.Len() != 2 || getString(pendingList.Get(0).Message(), "RoleImage") != "2" {
		t.Fatal("pending list should be deduplicated and carry numeric portraits")
	}
	memberPending := reply(1015010, "member", nil)
	if memberPending.Get(field(memberPending, "JoinList")).List().Len() != 0 {
		t.Fatal("requirements should not expose pending applications to ordinary members")
	}
	deny := Record{"RoleId": "applicant", "IsAgree": true}
	assertNoGuildWrite("unauthorized approval", func() {
		denied(1015012, "member", deny, 4008)
	})
	denied(1015012, "applicant", Record{"RoleId": "rejected", "IsAgree": true}, 4008)
	reply(1015012, "leader", Record{"RoleId": "rejected", "IsAgree": false})

	notifications := make(chan bool, 8)
	var approvals sync.WaitGroup
	for range cap(notifications) {
		approvals.Go(func() {
			peer, route, request, out := prepare(1015012, "leader", deny)
			after, err := route.live(s, peer, request, out)
			if err != nil {
				t.Errorf("approve retry: %v", err)
			}
			notifications <- after != nil
		})
	}
	approvals.Wait()
	close(notifications)
	pushes := 0
	for signal := range notifications {
		if signal {
			pushes++
		}
	}
	if pushes != 1 {
		t.Fatalf("duplicate approval produced %d membership notifications", pushes)
	}
	assertNoGuildWrite("approval retry", func() {
		reply(1015012, "leader", deny)
	})
	for _, id := range []string{"leader", "other-leader"} {
		out := reply(1015010, id, nil)
		if out.Get(field(out, "JoinList")).List().Len() != 0 {
			t.Fatalf("unresolved application remained for %s", id)
		}
	}
	denied(1015004, "applicant", Record{"GuildId": 900002}, 4694)
	memberEntrance := reply(1015005, "applicant", nil)
	leaderEntrance := reply(1015005, "leader", nil)
	if memberEntrance.Get(field(memberEntrance, "GuildHills")).List().Len() != 10 ||
		getBool(memberEntrance, "IsGuildMaster") || !getBool(leaderEntrance, "IsGuildMaster") {
		t.Fatal("guild scene requires ten peaks and an accurate leadership flag")
	}
	hall := reply(1015020, "leader", nil)
	if getString(hall.Get(field(hall, "ZongZhuInfo")).Message(), "RoleId") != "leader" {
		t.Fatal("main hall should include the actual leader")
	}

	reopened, err := OpenStore(dir, "guilds")
	if err != nil {
		t.Fatal(err)
	}
	reopened.enableHistory()
	s.guilds = reopened
	login := s.pb("RoleLoginResp_1002002")
	s.fillGuildLogin("applicant", login)
	if getString(login, "GuildName") != "临仙山门" || getInt(login, "GuildId") != 900001 {
		t.Fatal("accepted guild membership did not survive reload")
	}
	members := reply(1015008, "applicant", nil)
	rows := members.Get(field(members, "RoleList")).List()
	if rows.Len() != 3 || getInt(rows.Get(0).Message(), "ZhiWei") != 15 {
		t.Fatal("member roster did not reflect approval and leadership")
	}
	afterRoles, err := os.ReadFile(rolesPath)
	if err != nil || !bytes.Equal(originalRoles, afterRoles) {
		t.Fatal("guild operation changed unrelated role save")
	}

	events, err := reopened.pendingHistory()
	if err != nil {
		t.Fatal(err)
	}
	type eventIdentity struct {
		kind, actor, subject, guildID string
	}
	expected := map[eventIdentity]string{
		{"guild.applied", "applicant", "", "900001"}:        "临仙山门",
		{"guild.applied", "applicant", "", "900002"}:        "朝露山门",
		{"guild.applied", "rejected", "", "900001"}:         "临仙山门",
		{"guild.rejected", "leader", "rejected", "900001"}:  "临仙山门",
		{"guild.approved", "leader", "applicant", "900001"}: "临仙山门",
		{"guild.joined", "applicant", "leader", "900001"}:   "临仙山门",
	}
	for _, event := range events {
		subject := ""
		if event.Subject != nil {
			subject = event.Subject.RoleID
			if event.Subject.AccountID != "account-"+subject || event.Subject.RoleName != "角色 "+subject {
				t.Fatalf("guild history subject snapshot does not match role %s", subject)
			}
		}
		actor := event.Actor.RoleID
		if event.Actor.AccountID != "account-"+actor || event.Actor.RoleName != "角色 "+actor {
			t.Fatalf("guild history actor snapshot does not match role %s", actor)
		}
		key := eventIdentity{event.Kind, actor, subject, event.EntityID}
		name, found := expected[key]
		if !found || event.EntityType != "guild" || event.Details["guildName"] != name {
			t.Fatalf("unexpected or duplicate guild history event: %+v", event)
		}
		delete(expected, key)
	}
	if len(expected) != 0 {
		t.Fatalf("missing guild history events: %v", expected)
	}
}
