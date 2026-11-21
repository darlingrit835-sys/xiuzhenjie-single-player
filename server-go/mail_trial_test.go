package main

import (
	"os"
	"path/filepath"
	"sync"
	"testing"

	"google.golang.org/protobuf/proto"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func trialMailTestRole() Record {
	return Record{
		"ShinyStone": 20, "ShinyJade": 9,
		mailBoxField: Record{
			"gift": Record{"Title": "附件测试", "Content": "测试正文", "SentAt": 1, "Awards": []any{
				Record{"Id": "currency", "GoodsType": 100, "GoodsCount": 10},
				Record{"Id": "item", "GoodsName": "测试物品", "GoodsType": 9, "GoodsCount": 2, "Goods": Record{
					"Name": "测试物品", "Type": 9, "Level": 1, "Rank": 1, "TemplateId": "fixture", "Count": 2,
				}},
			}},
			"read": Record{"Title": "旧信", "Status": true},
		},
	}
}

func callMail(t *testing.T, s *Server, owner string, cmd int, id string, ids []string, code int64) protoreflect.Message {
	t.Helper()
	peer, connection := auctionTestRole(t, s, owner, nil)
	msg := auctionTestMessage(t, s, cmd, nil)
	if id != "" || ids != nil {
		req := s.pb(s.preview[cmd].request)
		if ids != nil {
			list := req.Mutable(field(req, "Id")).List()
			for _, id := range ids {
				list.Append(protoreflect.ValueOfString(id))
			}
		} else {
			set(req, "Id", id)
		}
		payload, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		msg.Data = payload
	}
	s.handleGame(peer, msg)
	return auctionTestReply(t, s, connection, msg, code)
}

func TestTrialMailboxReadClaimDeleteAndReopen(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	if err := s.roles.Put("owner", trialMailTestRole()); err != nil {
		t.Fatal(err)
	}
	if out := callMail(t, s, "other", 1003001, "", nil, 4055); out.Get(field(out, "Letters")).List().Len() != 0 {
		t.Fatal(out)
	}
	callMail(t, s, "other", 1003006, "gift", nil, 4203)
	out := callMail(t, s, "owner", 1003001, "", nil, 4055)
	letters := out.Get(field(out, "Letters")).List()
	gift := letters.Get(0).Message()
	if letters.Len() != 2 || getString(gift, "Id") != "gift" || !getBool(gift, "HasAward") || getBool(gift, "Status") || getBool(gift, "IsGetAll") || !mailUnread(s.roles.Get("owner")) {
		t.Fatal(out)
	}
	callMail(t, s, "owner", 1003004, "", []string{"read", "gift"}, 4008)
	if len(trialLetters(s.roles.Get("owner"))) != 2 {
		t.Fatal("batch deletion partially removed a protected mailbox")
	}
	callMail(t, s, "owner", 1003005, "gift", nil, 4066)
	callMail(t, s, "owner", 1003004, "", []string{"gift"}, 4008)

	var wg sync.WaitGroup
	for i := 0; i < 4; i++ {
		wg.Go(func() {
			claimed := callMail(t, s, "owner", 1003006, "gift", nil, 4206)
			if !getBool(claimed, "IsSuccess") || getInt(claimed, "ShinyStone") != 30 || getInt(claimed, "ShinyJade") != 9 {
				t.Errorf("inaccurate claim or replay: %v", claimed)
			}
		})
	}
	wg.Wait()
	s = auctionTestServer(t, dir)
	owner := s.roles.Get("owner")
	inventory := goodsList(owner)
	if len(inventory) != 1 || transferStackCount(inventory[0].(Record)) != 2 || roleString(inventory[0].(Record), "GoodsId") == "" || mailUnread(owner) {
		t.Fatalf("inventory or unread state lost: %v", owner)
	}
	out = callMail(t, s, "owner", 1003001, "", nil, 4055)
	first := out.Get(field(out, "Letters")).List().Get(0).Message()
	if !getBool(first, "IsGetAll") || !getBool(first, "Status") || !getBool(first.Get(field(first, "Awards")).List().Get(1).Message(), "Status") {
		t.Fatal(first)
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 1 || events[0].Kind != "mail.attachments_claimed" || events[0].Actor.RoleID != "owner" {
		t.Fatalf("mail claim should write one history entry: %v %v", events, err)
	}
	callMail(t, s, "owner", 1003004, "", []string{"gift", "read"}, 4054)
	callMail(t, s, "owner", 1003004, "", []string{"gift", "read"}, 4054)
	s = auctionTestServer(t, dir)
	if len(trialLetters(s.roles.Get("owner"))) != 0 {
		t.Fatal("deleted letters returned after restart")
	}
}

func TestTrialMailClaimRollsBackOnCapacityAndWriteFailure(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	role := trialMailTestRole()
	goods := make([]any, 100)
	for i := range goods {
		goods[i] = Record{"Type": 15}
	}
	role["Goods"] = goods
	if err := s.roles.Put("owner", role); err != nil {
		t.Fatal(err)
	}
	callMail(t, s, "owner", 1003006, "gift", nil, 4205)
	checkPending := func() {
		t.Helper()
		refreshed := s.roles.Get("owner")
		if roleInt(refreshed, "ShinyStone") != 20 || !mailPending(mailAwards(trialLetter(refreshed, "gift"))) || !mailUnread(refreshed) {
			t.Fatal("failed claim mutated balances or mail")
		}
	}
	checkPending()
	if err := s.roles.Update("owner", func(r Record) error { delete(r, "Goods"); return nil }); err != nil {
		t.Fatal(err)
	}
	blocked := filepath.Join(dir, "blocked")
	if err := os.WriteFile(blocked, []byte("parent is a file"), 0600); err != nil {
		t.Fatal(err)
	}
	old := s.roles.path
	s.roles.path = filepath.Join(blocked, "roles.json")
	callMail(t, s, "owner", 1003006, "gift", nil, 4008)
	s.roles.path = old
	checkPending()
	callMail(t, s, "owner", 1003006, "gift", nil, 4206)
}
