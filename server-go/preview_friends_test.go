package main

import (
	"os"
	"path/filepath"
	"testing"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func TestFriendCardsAndReciprocalLinksAfterReopen(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	first, _ := auctionTestRole(t, s, "role-first", nil)
	second, _ := auctionTestRole(t, s, "role-second", nil)
	third, _ := auctionTestRole(t, s, "role-third", nil)
	if err := s.roles.Update("role-second", func(r Record) error {
		delete(r, "Id") // This legacy save only has the outer role key.
		r["Name"], r["HeadPortraitId"], r["Sex"], r["Live1Type"], r["Live2Type"] = " 镜月 ", 2, 1, 2, 4
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	call := func(peer *Peer, cmd int, values Record, wantCode int64, success bool) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, cmd, values)
		s.handleGame(peer, msg)
		out := auctionTestReply(t, s, peer.conn.(*auctionTestConnection), msg, wantCode)
		if getBool(out, "IsSuccess") != success {
			t.Fatalf("unexpected status on %d: %v", cmd, out)
		}
		return out
	}
	target := func(id string) Record { return Record{"TargetFriendId": id} }
	call(&Peer{conn: &auctionTestConnection{}}, 1014007, Record{"Name": " 镜月 "}, previewDeniedCode, false)
	call(first, 1014007, Record{"Name": "无此人"}, 4402, false)
	call(first, 1014008, target(historyOutboxID), 4401, false)
	call(first, 1014007, Record{"Name": "role-first"}, 4413, false)
	call(first, 1014007, Record{"Name": " 镜月 "}, 4404, true)
	call(first, 1014008, target("role-second"), 4404, true)
	call(first, 1014006, target("role-second"), 4008, false)
	call(third, 1014006, target("role-first"), 4008, false)
	if count := getInt(call(second, 1014001, nil, 2000, true), "StrangerCount"); count != 1 {
		t.Fatalf("duplicate invite produced %d cards", count)
	}
	// Resume with the receiver alone online. The original sender's card persists.
	s = auctionTestServer(t, dir)
	second, _ = auctionTestRole(t, s, "role-second", nil)
	inbox := call(second, 1014003, nil, 2000, true)
	cards := inbox.Get(field(inbox, "Stranger")).List()
	if cards.Len() != 1 || getString(cards.Get(0).Message(), "Id") != "role-first" || getBool(cards.Get(0).Message(), "IsOnLine") {
		t.Fatalf("unexpected pending offline card: %v", inbox)
	}

	// Failing mid-accept must preserve the card and both friendship sides.
	savePath := s.roles.path
	s.roles.path = filepath.Join(dir, "unwritable", "roles.json")
	if err := os.WriteFile(filepath.Dir(s.roles.path), []byte("blocks mkdir"), 0600); err != nil {
		t.Fatal(err)
	}
	call(second, 1014006, target("role-first"), previewDeniedCode, false)
	s.roles.path = savePath
	if count := getInt(call(second, 1014001, nil, 2000, true), "StrangerCount"); count != 1 {
		t.Fatal("save failure consumed the card")
	}
	call(second, 1014008, target("role-first"), 4404, true)
	call(second, 1014006, target("role-first"), 4400, true)
	call(second, 1014006, target("role-first"), 4403, false)
	call(second, 1014008, target("role-first"), 4403, false)
	s = auctionTestServer(t, dir)
	first, _ = auctionTestRole(t, s, "role-first", nil)
	second, _ = auctionTestRole(t, s, "role-second", nil)
	checkFriend := func(peer *Peer, id string) protoreflect.Message {
		t.Helper()
		out := call(peer, 1014001, nil, 2000, true)
		list := out.Get(field(out, "Friend")).List()
		if list.Len() != 1 || getString(list.Get(0).Message(), "Id") != id || getInt(out, "StrangerCount") != 0 {
			t.Fatalf("unexpected reopened friendship: %v", out)
		}
		return list.Get(0).Message()
	}
	other := checkFriend(first, "role-second")
	checkFriend(second, "role-first")
	types := other.Get(field(other, "LiveType")).List()
	if getString(other, "Name") != " 镜月 " || getString(other, "HeadPortraitId") != "2" ||
		!getBool(other, "Sex") || !getBool(other, "IsOnLine") || types.Len() != 2 ||
		types.Get(0).Int() != 2 || types.Get(1).Int() != 4 {
		t.Fatalf("client fields missing: %v", other)
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 4 || events[2].Kind != "friend.added" || events[3].Kind != "friend.added" ||
		events[2].Actor.RoleID != "role-second" || events[2].Actor.AccountID != "account-role-second" || events[2].Subject == nil || events[2].Subject.RoleID != "role-first" ||
		events[3].Actor.RoleID != "role-first" || events[3].Subject == nil || events[3].Subject.RoleName != " 镜月 " {
		t.Fatalf("unexpected friendship history: %#v %v", events, err)
	}
	call(first, 1014010, target("role-second"), 2004, true)
	for _, peer := range []*Peer{first, second} {
		out := call(peer, 1014001, nil, 2000, true)
		if out.Get(field(out, "Friend")).List().Len() != 0 {
			t.Fatal("removal did not clear both sides")
		}
	}
	call(first, 1014008, target("role-second"), 4404, true)
	call(second, 1014009, target("role-first"), 2004, true)
	call(second, 1014006, target("role-first"), 4008, false)
	s = auctionTestServer(t, dir)
	second, _ = auctionTestRole(t, s, "role-second", nil)
	list := call(second, 1014001, nil, 2000, true)
	if list.Get(field(list, "Friend")).List().Len() != 0 || getInt(list, "StrangerCount") != 0 {
		t.Fatalf("unexpected reopened state after removal and refusal: %v", list)
	}
}

func TestCrossedFriendCardsAcceptedConcurrently(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	first, firstConn := auctionTestRole(t, s, "first", nil)
	second, secondConn := auctionTestRole(t, s, "second", nil)
	acceptFirst := auctionTestMessage(t, s, 1014006, Record{"TargetFriendId": "second"})
	acceptSecond := auctionTestMessage(t, s, 1014006, Record{"TargetFriendId": "first"})
	for _, pair := range []struct{ from, to string }{{"first", "second"}, {"second", "first"}} {
		out := s.gameResponse("AddFriendByNameResp_1014007", false, 0)
		if err := s.sendFriendRequest(pair.from, pair.to, "", out); err != nil {
			t.Fatal(err)
		}
	}
	start := make(chan struct{})
	done := make(chan struct{})
	go func() {
		defer close(done)
		<-start
		s.handleGame(first, acceptFirst)
	}()
	close(start)
	s.handleGame(second, acceptSecond)
	<-done
	left := firstConn.receive(t, s, 1014006, acceptFirst.Msid, "AddFriendResp_1014006")
	right := secondConn.receive(t, s, 1014006, acceptSecond.Msid, "AddFriendResp_1014006")
	if getBool(left, "IsSuccess") == getBool(right, "IsSuccess") {
		t.Fatal("crossed accepts must result in exactly one relationship transition")
	}
	for _, peerID := range []string{"first", "second"} {
		role := s.roles.Get(peerID)
		if len(friendMap(role, friendLinks)) != 1 || len(friendMap(role, friendInbox)) != 0 {
			t.Fatalf("inconsistent relationship for %s", peerID)
		}
	}
	history, err := s.roles.pendingHistory()
	if err != nil || len(history) != 4 {
		t.Fatalf("concurrent acceptance duplicated history: %d, %v", len(history), err)
	}
}
