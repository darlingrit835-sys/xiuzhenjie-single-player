package main

import (
	"fmt"
	"slices"
	"testing"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

type transferTestItem struct {
	id    string
	count int
}

func transferTestRequest(t *testing.T, s *Server, id int, to string, items ...transferTestItem) Message {
	t.Helper()
	req := s.pb("RemoteSendGoodsReq_1014005")
	set(req, "TargetFriendId", to)
	for _, item := range items {
		putPB(addMessage(req, "Goods"), Record{"GoodsId": item.id, "Count": item.count})
	}
	data, err := proto.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	return Message{Cmd: 1014005, Msid: id, Data: data}
}

func TestFriendTransferAtomicInventoryAndReplay(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	s.game.transferDiscounts = map[int]int64{101: 5, 103: 20}
	pills := Record{"GoodsId": "pills", "TemplateId": "jade-pill", "Name": "玉丹", "Rank": 2, "Level": 11, "Type": 19, "Count": 6}
	sword := Record{"GoodsId": "sword", "TemplateId": "steel", "Name": "竹剑", "Rank": 1, "Level": 1, "Type": 15, "Enchant": Record{"roll": 47}}
	bound := Record{"GoodsId": "bound", "IsBinding": true}
	a, conn := auctionTestRole(t, s, "sender", []any{pills, sword, bound})
	b, _ := auctionTestRole(t, s, "receiver", nil)
	if err := s.roles.Update("sender", func(r Record) error {
		r["ShinyStone"] = 2000
		r["TitleId"] = []any{101, 103}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	check := func(msg Message, want int64, ok bool) protoreflect.Message {
		t.Helper()
		s.handleGame(a, msg)
		out := auctionTestReply(t, s, conn, msg, want)
		if getBool(out, "IsSuccess") != ok {
			t.Fatal(out)
		}
		return out
	}
	packet := func(id int, items ...transferTestItem) Message {
		return transferTestRequest(t, s, id, "receiver", items...)
	}
	check(packet(1, transferTestItem{"sword", 1}), 4407, false)
	for _, ids := range [][2]string{{"sender", "receiver"}, {"receiver", "sender"}} {
		if err := s.roles.Update(ids[0], func(r Record) error { putFriendEntry(r, friendLinks, ids[1], 0); return nil }); err != nil {
			t.Fatal(err)
		}
	}
	beforeA, beforeB := gameGoodsMD5(s.roles.Get("sender")), gameGoodsMD5(s.roles.Get("receiver"))
	// A rejected final item must roll back previously staged goods, fee and history.
	check(packet(2, transferTestItem{"pills", 2}, transferTestItem{"bound", 1}), 4509, false)
	check(packet(3, transferTestItem{"pills", 2}, transferTestItem{"pills", 1}), 4399, false)
	check(packet(4, transferTestItem{"pills", 7}), 4396, false)
	check(packet(5, transferTestItem{"pills", 0}), 4399, false)
	if gameGoodsMD5(s.roles.Get("sender")) != beforeA || gameGoodsMD5(s.roles.Get("receiver")) != beforeB {
		t.Fatal("rejected transfer edited inventory")
	}
	// Both discounted titles are owned; only the greater discount applies.
	msg := packet(6, transferTestItem{"pills", 2}, transferTestItem{"sword", 1})
	out := check(msg, 4397, true)
	if getInt(out, "CurrentShinyStone") != 1280 {
		t.Fatalf("wrong balance: %v", out)
	}
	check(msg, 4397, true)
	check(packet(6, transferTestItem{"pills", 1}), 4008, false)
	from, to := s.roles.Get("sender"), s.roles.Get("receiver")
	if roleInt(from, "ShinyStone") != 1280 || len(goodsList(to)) != 2 || locateGoods(from, "sword") != nil ||
		transferStackCount(locateGoods(from, "pills")) != 4 || gameGoodsMD5(from) == beforeA || gameGoodsMD5(to) == beforeB {
		t.Fatal("inventory or fee differs after replay")
	}
	received := goodsList(to)[0].(Record)
	if roleString(received, "GoodsId") == "pills" || transferStackCount(received) != 2 || roleInt(locateGoods(to, "sword")["Enchant"].(Record), "roll") != 47 {
		t.Fatal("lost item metadata or reused the ID of a split stack")
	}
	// A new deliberate action of the same amount must be charged and merged.
	check(packet(7, transferTestItem{"pills", 2}), 4397, true)
	check(packet(6, transferTestItem{"pills", 2}, transferTestItem{"sword", 1}), 4397, true)
	if roleInt(s.roles.Get("sender"), "ShinyStone") != 640 || len(goodsList(s.roles.Get("receiver"))) != 2 ||
		transferStackCount(locateGoods(s.roles.Get("receiver"), roleString(received, "GoodsId"))) != 4 {
		t.Fatal("late retry or merge corrupted inventory")
	}
	// Drive the two normal client cache queries on the receiver's own session.
	for _, spec := range []struct {
		cmd       int
		req, resp string
	}{{1004004, "SpaceGoodsCheckReq_1004004", "SpaceGoodsCheckResp_1004004"}, {1004002, "SpaceGoodsReq_1004002", "SpaceGoodsResp_1004002"}} {
		req := s.pb(spec.req)
		set(req, "Location", 10)
		data, err := proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
		s.handleGame(b, Message{Cmd: spec.cmd, Msid: 10, Data: data})
		response := b.conn.(*auctionTestConnection).receive(t, s, spec.cmd, 10, spec.resp)
		if spec.cmd == 1004004 && getString(response, "Md5") != gameGoodsMD5(s.roles.Get("receiver")) {
			t.Fatal("stale client cache MD5")
		}
		if spec.cmd == 1004002 && response.Get(field(response, "GoodsInfos")).List().Len() != 2 {
			t.Fatal("new items missing from original bag response")
		}
	}
	s = auctionTestServer(t, dir)
	outTitles := s.gameResponse("RoleLoginResp_1002002", true, 2000)
	fillFriendTransferTitles(outTitles, s.roles.Get("sender"))
	titles := outTitles.Get(field(outTitles, "TitleId")).List()
	if titles.Len() != 2 || titles.Get(1).Int() != 103 {
		t.Fatal("old title list not returned to client")
	}
	ev, err := s.roles.pendingHistory()
	if err != nil || len(ev) != 4 || ev[0].Actor.RoleID != "sender" || ev[0].Subject.RoleID != "receiver" || ev[1].Actor.RoleID != "receiver" || roleInt(ev[0].Details, "fee") != 720 {
		t.Fatalf("unexpected transfer history %v %v", ev, err)
	}
	if roleInt(s.roles.Get("sender"), "ShinyStone") != 640 || len(goodsList(s.roles.Get("receiver"))) != 2 {
		t.Fatal("transfer did not survive reopen")
	}
}

func TestFriendTransferFullBagAndPaymentFailure(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	current := Record{"GoodsId": "jade", "Count": 4, "Name": "玉珠", "Type": 25}
	first, conn := auctionTestRole(t, s, "first", []any{current})
	all := make([]any, 100)
	for i := range all {
		all[i] = Record{"GoodsId": fmt.Sprint(i), "Type": 15}
	}
	auctionTestRole(t, s, "last", all)
	for _, pair := range [][2]string{{"first", "last"}, {"last", "first"}} {
		if err := s.roles.Update(pair[0], func(r Record) error { putFriendEntry(r, friendLinks, pair[1], 0); r["ShinyStone"] = 0; return nil }); err != nil {
			t.Fatal(err)
		}
	}
	send := func(id int, code int64) {
		t.Helper()
		m := transferTestRequest(t, s, id, "last", transferTestItem{"jade", 1})
		s.handleGame(first, m)
		auctionTestReply(t, s, conn, m, code)
	}
	send(1, 4527)
	if err := s.roles.Update("last", func(r Record) error { r["Goods"] = slices.Delete(goodsList(r), 0, 1); return nil }); err != nil {
		t.Fatal(err)
	}
	send(2, 4398)
	if transferStackCount(locateGoods(s.roles.Get("first"), "jade")) != 4 || len(goodsList(s.roles.Get("last"))) != 99 {
		t.Fatal("could not pay but goods moved")
	}
	if ev, err := s.roles.pendingHistory(); err != nil || len(ev) != 0 {
		t.Fatal("failed transfer retained events", ev, err)
	}
}

func TestFriendTransferConcurrentRequestsCannotSpendSameGoodsTwice(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	item := Record{"GoodsId": "only-item", "Count": 1, "Type": 15}
	one, clientOne := auctionTestRole(t, s, "sender", []any{item})
	two, clientTwo := auctionTestRole(t, s, "sender", nil)
	auctionTestRole(t, s, "recipient", nil)
	for _, pair := range [][2]string{{"sender", "recipient"}, {"recipient", "sender"}} {
		if err := s.roles.Update(pair[0], func(role Record) error { putFriendEntry(role, friendLinks, pair[1], 0); return nil }); err != nil {
			t.Fatal(err)
		}
	}
	msg := transferTestRequest(t, s, 12, "recipient", transferTestItem{"only-item", 1})
	start, done := make(chan struct{}), make(chan struct{})
	go func() { defer close(done); <-start; s.handleGame(one, msg) }()
	close(start)
	s.handleGame(two, msg)
	<-done
	resp1 := clientOne.receive(t, s, msg.Cmd, msg.Msid, "RemoteSendGoodsResp_1014005")
	resp2 := clientTwo.receive(t, s, msg.Cmd, msg.Msid, "RemoteSendGoodsResp_1014005")
	if getBool(resp1, "IsSuccess") == getBool(resp2, "IsSuccess") || roleInt(s.roles.Get("sender"), "ShinyStone") != 900 ||
		len(goodsList(s.roles.Get("sender"))) != 0 || len(goodsList(s.roles.Get("recipient"))) != 1 {
		t.Fatal("concurrent sessions transferred the same item twice")
	}
}
