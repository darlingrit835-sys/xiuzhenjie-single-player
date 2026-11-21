package main

import (
	"encoding/json"
	"fmt"
	"reflect"
	"strings"
	"testing"

	"google.golang.org/protobuf/proto"
)

func TestConcordDeskBooksToUsableDose(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	p, _ := auctionTestRole(t, s, "reader", nil)
	wire := func(q concordRequest, code int) Record {
		t.Helper()
		b, _ := json.Marshal(q)
		raw := auctionJSONCommandReply(t, s, p, concordCommand, b, code)
		var out Record
		if e := json.Unmarshal(raw, &out); e != nil {
			t.Fatal(e)
		}
		return out
	}
	list := wire(concordRequest{Action: "list"}, 2000)
	if len(list["books"].([]any)) != 0 || roleInt(list, "copyCost") != 120 || !strings.Contains(roleString(list, "primerText"), "凝神诀") {
		t.Fatal(list)
	}
	copyOrder := concordRequest{Action: "copy", Quote: roleString(list, "primerText"), RequestID: strings.Repeat("a", 32)}
	copied := wire(copyOrder, 2000)
	again := wire(copyOrder, 2000)
	if !reflect.DeepEqual(copied, again) || roleInt(again, "stones") != 880 || len(again["books"].([]any)) != 2 {
		t.Fatal(again)
	}
	var metal, water string
	for _, item := range goodsList(s.roles.Get("reader")) {
		g := item.(Record)
		m := s.methods().book(g)
		if m == nil || m.learningLevel != 1 || roleInt(g, "Type") != 1 {
			t.Fatal(g)
		}
		if m.element == 1 {
			metal = roleString(g, "GoodsId")
		}
		if m.element == 3 {
			water = roleString(g, "GoodsId")
		}
	}
	forward := concordRequest{Action: "preview", Main: metal, Second: water}
	q := wire(forward, 2000)
	reverse := wire(concordRequest{Action: "preview", Main: water, Second: metal}, 2000)
	if roleString(q, "relation") != "顺生" || roleInt(q, "points") != 5 || roleInt(q, "pills") != 2 || roleInt(q, "ink") != 8 || roleString(reverse, "relation") != "逆推" || roleInt(reverse, "pills") != 1 {
		t.Fatal(q, reverse)
	}
	submit := concordRequest{Action: "commit", Main: metal, Second: water, Quote: "bad", Revision: 1, RequestID: strings.Repeat("b", 32)}
	wire(submit, 4008)
	if roleInt(s.roles.Get("reader"), "ShinyStone") != 880 {
		t.Fatal("rejected quote spent stones")
	}
	submit.Quote = roleString(q, "quote")
	type outcome struct {
		out Record
		err error
	}
	results := make(chan outcome, 2)
	for i := 0; i < 2; i++ {
		go func() { o, e := s.concordAction("reader", submit); results <- outcome{o, e} }()
	}
	for i := 0; i < 2; i++ {
		v := <-results
		if v.err != nil || roleInt(v.out, "stones") != 872 || roleInt(v.out, "revision") != 2 {
			t.Fatal(v.out, v.err)
		}
	}
	reopened := auctionTestServer(t, dir)
	saved := reopened.roles.Get("reader")
	msg, e := reopened.concordAction("reader", submit)
	if e != nil || roleInt(msg, "stones") != 872 || len(goodsList(saved)) != 1 {
		t.Fatal(msg, e, saved)
	}
	dose := goodsList(saved)[0].(Record)
	if roleString(dose, "TemplateId") != "wu_xing_dan" || transferStackCount(dose) != 2 {
		t.Fatal(dose)
	}
	if e := reopened.roles.Update("reader", func(r Record) error {
		r["Clever"], r["CurrentClever"] = 330, 10
		return nil
	}); e != nil {
		t.Fatal(e)
	}
	player, conn := auctionTestRole(t, reopened, "reader", nil)
	use := gameInventoryProtocols[1004024]
	request := reopened.pb(use.req)
	key := "GoodsId"
	if field(request, key) == nil {
		key = "Id"
	}
	set(request, key, roleString(dose, "GoodsId"))
	data, e := proto.Marshal(request)
	if e != nil {
		t.Fatal(e)
	}
	reopened.handleGame(player, Message{Cmd: 1004024, Msid: 18, Data: data})
	reply := conn.receive(t, reopened, 1004024, 18, use.resp)
	after := reopened.roles.Get("reader")
	if !getBool(reply, "IsSuccess") || roleInt(after, "CurrentClever") != 110 || transferStackCount(locateGoods(after, roleString(dose, "GoodsId"))) != 1 {
		t.Fatal("bag use did not settle", reply, after)
	}
	// An older client retry cannot settle after a later, distinct revision.
	if _, e = reopened.concordAction("reader", copyOrder); e == nil {
		t.Fatal("stale receipt processed twice")
	}
}

func TestConcordDeskProtectionAndRollback(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	r := Record{"Id": "protected", "Level": 21, "ShinyStone": 1000, "GongFaLianShen": "gf_1_1"}
	other := addGoods(r, "金刚不坏体", 1, 3, 1, 3, "gf_2_3", 1, 1, 30)
	goods := func(id string, rank, count int) Record {
		g := Record{"GoodsId": id, "TemplateId": id, "Name": id, "Type": 2, "Rank": rank, "Count": count, "FiveElement": 1}
		r["Goods"] = append(goodsList(r), g)
		return g
	}
	for _, k := range []string{"IsLock", "IsUsing", "IsBinding", friendOfferLockKey} {
		g := goods(k, 2, 5)
		if k == friendOfferLockKey {
			g[k] = "offer"
		} else {
			g[k] = true
		}
	}
	herb := goods("old-herb", 1, 8)
	herb["Type"], herb["TemplateId"] = 18, "mat.ninglu_herb"
	goods("rare", 13, 1)
	goods("rare-spare", 13, 2)
	equipped := goods("equipped", 1, 1)
	equipped["Type"], equipped["TemplateId"] = 1, "gf_1_1"
	starter := goods("starter", 1, 1)
	starter["Type"], starter["TemplateId"] = 1, practiceStarterMethodID
	available := goods("double", 1, 2)
	if err := s.roles.Put("protected", r); err != nil {
		t.Fatal(err)
	}
	for _, id := range []string{"IsLock", "IsUsing", "IsBinding", friendOfferLockKey, "old-herb", "rare", "equipped", "starter"} {
		if _, err := s.concordAction("protected", concordRequest{Action: "preview", Main: id, Second: roleString(other, "GoodsId")}); err == nil {
			t.Fatal("protected", id)
		}
	}
	for _, tc := range []struct {
		a, b    string
		allowed bool
	}{{"rare-spare", "rare-spare", false}, {"rare-spare", "double", true}, {"double", "double", true}, {roleString(other, "GoodsId"), roleString(other, "GoodsId"), false}} {
		_, err := s.concordAction("protected", concordRequest{Action: "preview", Main: tc.a, Second: tc.b})
		if (err == nil) != tc.allowed {
			t.Fatal(tc, err)
		}
	}
	// New copies never merge into a locked stack, and avoid currently equipped books.
	other["IsLock"] = true
	if e := s.roles.Put("protected", r); e != nil {
		t.Fatal(e)
	}
	copyQuote := roleString(s.concordList(r), "primerText")
	result, e := s.concordAction("protected", concordRequest{Action: "copy", Quote: copyQuote, RequestID: strings.Repeat("c", 32)})
	if e != nil || strings.Contains(roleString(result, "primerText"), "凝神诀") {
		t.Fatal(result, e)
	}
	fresh := s.roles.Get("protected")
	if roleInt(locateGoods(fresh, roleString(other, "GoodsId")), "Count") != 1 {
		t.Fatal("locked stack modified")
	}
	for _, wanted := range []string{"gf_1_2", "gf_2_3"} {
		found := false
		for _, item := range goodsList(fresh) {
			g := item.(Record)
			b, _ := s.concordBook(fresh, g)
			if roleString(g, "TemplateId") == wanted && b.Free > 0 {
				found = true
			}
		}
		if !found {
			t.Fatal("new usable primer missing", wanted)
		}
	}
	// Keep stacks in place after both inputs are consumed. No slot remains for the payout.
	available["Count"] = 3
	for len(goodsList(r)) < ringCapacity(r) {
		goods(fmt.Sprint("filler", len(goodsList(r))), 1, 1)
	}
	if e = s.roles.Put("protected", r); e != nil {
		t.Fatal(e)
	}
	before := s.roles.Get("protected")
	q := concordRequest{Action: "preview", Main: "double", Second: "double"}
	p, e := s.concordAction("protected", q)
	if e != nil {
		t.Fatal(e)
	}
	q.Action, q.RequestID, q.Quote = "commit", strings.Repeat("d", 32), roleString(p, "quote")
	if _, e = s.concordAction("protected", q); e == nil || !reflect.DeepEqual(before, s.roles.Get("protected")) {
		t.Fatal("partial settlement on full bag", e)
	}
	s.game.methods = indexMethods(nil)
	list, e := s.concordAction("protected", concordRequest{Action: "list"})
	if e != nil || roleString(list, "primerText") != "" {
		t.Fatal("missing legacy catalog", list, e)
	}
	if _, e = s.concordAction("protected", concordRequest{Action: "copy", RequestID: strings.Repeat("e", 32)}); e == nil || !reflect.DeepEqual(before, s.roles.Get("protected")) {
		t.Fatal("incomplete catalog spent resources", e)
	}
}

func TestConcordDeskAccess(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	old, _ := auctionTestRole(t, s, "reader", nil)
	current, _ := auctionTestRole(t, s, "reader", nil)
	list := []byte(`{"action":"list"}`)
	auctionJSONCommandReply(t, s, old, concordCommand, list, 4007)
	s.previewWrites = false
	auctionJSONCommandReply(t, s, current, concordCommand, list, 2000)
	auctionJSONCommandReply(t, s, current, concordCommand, []byte(`{"action":"copy","requestId":"12345678123456781234567812345678"}`), 4008)
	if roleInt(s.roles.Get("reader"), "ShinyStone") != 1000 {
		t.Fatal("preview server spent stones")
	}
	s.previewWrites = true
	if e := s.roles.Update("reader", func(r Record) error { r["Level"] = 20; return nil }); e != nil {
		t.Fatal(e)
	}
	auctionJSONCommandReply(t, s, current, concordCommand, list, 4008)
}
