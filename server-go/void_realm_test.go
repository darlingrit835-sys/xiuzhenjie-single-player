package main

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"fmt"
	"strings"
	"sync"
	"testing"
	"time"
)

func TestVoidRealmModelMatchesBoardV1(t *testing.T) {
	// Digest of all 1,701 trials from voidRealmRules.simulate in the interactive board,
	// including both axis readings, pass/fail, score, node count and estimated cost.
	hash := sha256.New()
	for _, h := range voidHalls {
		if h.Type == 7 || h.Type == 9 {
			continue // These halls were introduced in v2.
		}
		role := Record{"Level": 81, "Live4Type": h.Type, "Live4Level": 9}
		for grade := 1; grade <= 9; grade++ {
			for b := range 3 {
				for r := range 3 {
					for a := range 3 {
						q := voidRequest{HallID: h.ID, Grade: grade, Brief: b, Route: r, Adjust: a}
						result, err := voidSimulate(role, q)
						if err != nil {
							t.Fatal(q, err)
						}
						pass := 0
						if roleBool(result, "success") {
							pass = 1
						}
						axes := result["axes"].([]Record)
						fmt.Fprintf(hash, "%s,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d,%d\n", h.ID, grade, b, r, a,
							roleInt(axes[0], "value"), roleInt(axes[0], "target"), roleInt(axes[1], "value"), roleInt(axes[1], "target"),
							pass, roleInt(result, "score"), roleInt(result, "nodes"), roleInt(result, "cost"))
					}
				}
			}
		}
	}
	if got := hex.EncodeToString(hash.Sum(nil)); got != "f11088ec5e19eed64d9af330d37a2ad1aca7cd9ecca4e957de68d93018f47e39" {
		t.Fatalf("incompatible v1 readings: %s", got)
	}
}

func TestVoidRealmPersistentSocialAndAdmissions(t *testing.T) {
	dir := t.TempDir()
	open := func(path string) *Server {
		st, err := OpenStore(path, "roles")
		if err != nil {
			t.Fatal(err)
		}
		return &Server{roles: st}
	}
	s := open(dir)
	at := time.Unix(1800000000, 0)
	for _, row := range []struct {
		id, user    string
		level, rank int
	}{{"author", "user1", 31, 3}, {"reader", "user2", 21, 2}, {"alt", "user2", 21, 2}, {"ownalt", "user1", 21, 3},
		{"novice", "user3", 20, 9}, {"outsider", "user4", 31, 0}} {
		role := Record{"Id": row.id, "Name": row.id, "UserId": row.user, "Level": row.level, "Live1Type": 1, "Live1Level": 0,
			"Live4Type": 1, "Live4Level": row.rank, "ForgeRank": 2, "CurrentClever": 100}
		if err := s.roles.Put(row.id, role); err != nil {
			t.Fatal(err)
		}
	}
	run := func(owner string, q voidRequest) Record {
		t.Helper()
		result, err := s.voidAction(owner, q, at)
		if err != nil {
			t.Fatal(q, err)
		}
		return result
	}
	deny := func(owner string, q voidRequest) {
		t.Helper()
		if _, err := s.voidAction(owner, q, at); err == nil {
			t.Fatalf("unexpectedly accepted %s %+v", owner, q)
		}
	}
	deny("novice", voidRequest{Action: "lobby"})
	deny("outsider", voidRequest{Action: "enter", HallID: "dan"})
	run("outsider", voidRequest{Action: "enter", HallID: "qi", Grade: 2})
	deny("outsider", voidRequest{Action: "enter", HallID: "qi", Grade: 3})
	run("outsider", voidRequest{Action: "lobby"})
	run("author", voidRequest{Action: "enter", HallID: "dan", Grade: 3})
	deny("author", voidRequest{Action: "enter", HallID: "dan", Grade: 4})
	pub := voidRequest{Action: "publish", HallID: "dan", Grade: 1, Name: "巡山丹", RequestID: newGameID(16)}
	fail := pub
	fail.Route = 1
	deny("author", fail)
	invalid := pub
	invalid.Name = "伪作刀"
	deny("author", invalid)
	// The old client never sends a paid v2 quote. Its existing receipts and
	// manuscripts remain readable, but it cannot publish additional free copies.
	deny("author", pub)
	seedV1 := func(owner string, q voidRequest) {
		t.Helper()
		body, _ := json.Marshal(q)
		role := s.roles.Get(owner)
		result, err := voidSimulate(role, q)
		if err != nil || !roleBool(result, "success") {
			t.Fatal(q, err)
		}
		id := voidDigest(owner, q.RequestID)
		rec := Record{"kind": "work", "version": 1, "id": id, "hash": voidDigest(string(body)),
			"authorId": owner, "author": owner, "maker": voidDigest("account:" + roleString(role, "UserId")),
			"hallId": q.HallID, "name": q.Name, "grade": q.Grade, "brief": q.Brief, "route": q.Route, "adjust": q.Adjust,
			"score": roleInt(result, "score"), "time": at.Unix()}
		if err := s.roles.Put(voidStorePrefix+id, rec); err != nil {
			t.Fatal(err)
		}
	}
	seedV1("author", pub)
	work := run("author", pub)["work"].(Record)
	id := roleString(work, "id")
	if id == "" || roleInt(work, "score") != 60 || work["maker"] != nil || !roleBool(work, "own") {
		t.Fatal(work)
	}
	if altView := run("ownalt", voidRequest{Action: "work", WorkID: id})["work"].(Record); !roleBool(altView, "own") {
		t.Fatal("another character on the author's account should see self-verification", altView)
	}
	if replay := run("author", pub)["work"].(Record); roleString(replay, "id") != id {
		t.Fatal(replay)
	}
	dup := pub
	dup.Name = "行山丹"
	deny("author", dup)
	dup.RequestID = newGameID(16)
	deny("author", dup)
	copyName := pub
	copyName.RequestID = newGameID(16)
	deny("reader", copyName)
	another := pub
	another.RequestID, another.Name, another.Route, another.Adjust = newGameID(16), "回山丹", 2, 2
	seedV1("author", another)
	run("author", another)
	copyName.Name = "留露丹"
	seedV1("reader", copyName)
	run("reader", copyName)
	ranks := run("outsider", voidRequest{Action: "rankings", Mode: "quality"})["works"].([]Record)
	if len(ranks) != 2 || roleInt(ranks[0], "place") != 1 || roleInt(ranks[1], "place") != 1 {
		t.Fatal(ranks)
	}
	post := voidRequest{Action: "post", Text: "来试巡山一方", Tag: "邀试", WorkID: id, RequestID: newGameID(16)}
	postID := roleString(run("outsider", post)["post"].(Record), "id")
	run("outsider", post)
	deny("novice", post)
	deny("outsider", voidRequest{Action: "post", Text: "无效附卷", Tag: "求教", RequestID: newGameID(16), WorkID: "missing"})
	list := run("author", voidRequest{Action: "board", Tag: "邀试"})["posts"].([]Record)
	if len(list) != 1 || roleString(list[0], "id") != postID || roleString(list[0], "workId") != id {
		t.Fatal(list)
	}
	q := voidRequest{Action: "study", WorkID: id}
	deny("author", q)
	deny("ownalt", q)
	deny("outsider", q)
	ch := make(chan error, 16)
	var wg sync.WaitGroup
	for i := range 16 {
		wg.Add(1)
		go func() {
			defer wg.Done()
			actor := []string{"reader", "alt"}[i%2]
			v, e := s.voidAction(actor, q, at)
			if e == nil && roleInt(v["work"].(Record), "readerCount") != 1 {
				e = fmt.Errorf("duplicate account counted twice: %v", v)
			}
			ch <- e
		}()
	}
	wg.Wait()
	close(ch)
	for e := range ch {
		if e != nil {
			t.Fatal(e)
		}
	}
	s = open(dir)
	reread := run("reader", q)["work"].(Record)
	if roleBool(reread, "own") || !roleBool(reread, "studied") || roleInt(reread, "readerCount") != 1 || roleInt(s.roles.Get("reader"), "CurrentClever") != 100 {
		t.Fatal(reread)
	}
	board := run("outsider", voidRequest{Action: "rankings", Mode: "study"})["works"].([]Record)
	if len(board) != 3 || roleString(board[0], "id") != id || roleInt(board[0], "place") != 1 || roleInt(board[1], "place") != 2 || roleInt(board[2], "place") != 2 {
		t.Fatal(board)
	}
	higher := pub
	higher.Grade, higher.Name, higher.RequestID = 2, "再行丹", newGameID(16)
	seedV1("author", higher)
	run("author", higher)
	for grade, want := range map[int]int{0: 2, 1: 2, 2: 1, 3: 0} {
		entries := run("outsider", voidRequest{Action: "rankings", HallID: "dan", Mode: "quality", Grade: grade})["works"].([]Record)
		if len(entries) != want || want > 0 && roleInt(entries[0], "place") != 1 ||
			grade > 0 && want > 0 && roleInt(entries[0], "grade") != int64(grade) {
			t.Fatal("grade filter before author deduplication and placement", grade, entries)
		}
	}
	deny("outsider", voidRequest{Action: "rankings", Grade: 10})
	run("outsider", post)
	if s.roles.Find(func(r Record) bool { return roleString(r, "id") == id }) != nil {
		t.Fatal("work leaked into role index")
	}
	s = open(t.TempDir())
	if err := s.roles.Put("reader", Record{"Level": 31, "Live1Type": 1, "Live1Level": 3}); err != nil {
		t.Fatal(err)
	}
	deny("reader", voidRequest{Action: "work", WorkID: id})
	if n := len(run("reader", voidRequest{Action: "rankings"})["works"].([]Record)); n != 0 {
		t.Fatal("another server saw manuscripts", n)
	}
}

func TestVoidRealmWireUsesAuthenticatedRole(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	t.Cleanup(func() { _ = s.privateChat.close() })
	old, _ := auctionTestRole(t, s, "writer", nil)
	current, _ := auctionTestRole(t, s, "writer", nil)
	if err := s.roles.Update("writer", func(r Record) error { r["Level"], r["Live4Type"], r["Live4Level"] = 31, 6, 2; return nil }); err != nil {
		t.Fatal(err)
	}
	data := []byte(`{"action":"lobby","roleId":"some-other-player"}`)
	auctionJSONCommandReply(t, s, old, voidCommand, data, 4007)
	var reply Record
	if err := json.Unmarshal(auctionJSONCommandReply(t, s, current, voidCommand, data, 2000), &reply); err != nil || roleString(reply, "roleId") != "writer" {
		t.Fatal(reply, err)
	}
	q := []byte(`{"action":"enter","hallId":"dan","grade":1,"slots":[{"type":1,"level":9}]}`)
	if reason := string(auctionJSONCommandReply(t, s, current, voidCommand, q, 4008)); !strings.Contains(reason, "丹师") {
		t.Fatal(reason)
	}
	if err := s.roles.Update("writer", func(r Record) error {
		r["Live1Type"], r["Live1Level"], r["CurrentClever"] = 1, 1, 20
		r["Goods"] = []any{Record{"GoodsId": "one-herb", "TemplateId": "mat.ninglu_herb", "Count": 1}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	order := voidRequest{Version: 2, Action: "simulate", Grade: 1, HallID: "dan", Brief: 0, Route: 3, Adjust: 1, Steps: []int{0, 1, 0, 3}}
	wire := func(p *Peer, ticket voidRequest) Record {
		t.Helper()
		b, err := json.Marshal(ticket)
		if err != nil {
			t.Fatal(err)
		}
		var out Record
		if err := json.Unmarshal(auctionJSONCommandReply(t, s, p, voidCommand, b, 2000), &out); err != nil {
			t.Fatal(err)
		}
		return out
	}
	price := wire(current, order)
	order.Action, order.Name, order.QuoteID, order.RequestID = "publish", "归元丹", roleString(price, "quoteId"), newGameID(16)
	published := wire(current, order)
	paid := published["wallet"].(Record)
	work := published["work"].(Record)
	if roleInt(paid, "stones") != 1000-roleInt(price, "stoneCost") || roleInt(paid, "insight") != 20-roleInt(price, "insightCost") ||
		roleString(work, "craftText") == "" || roleInt(work, "value") != roleInt(price, "value") {
		t.Fatal(price, published)
	}
	reader, _ := auctionTestRole(t, s, "reader", nil)
	if err := s.roles.Update("reader", func(r Record) error { r["Live2Type"], r["Live2Level"] = 1, 1; return nil }); err != nil {
		t.Fatal(err)
	}
	studied := wire(reader, voidRequest{Action: "study", WorkID: roleString(work, "id")})
	if result := studied["result"].(Record); roleInt(result, "cost") != 0 || !roleBool(result, "success") ||
		roleInt(studied["work"].(Record), "readerCount") != 1 {
		t.Fatal(studied)
	}
}
