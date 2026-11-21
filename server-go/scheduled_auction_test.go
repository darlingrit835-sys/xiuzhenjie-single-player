package main

import (
	"encoding/json"
	"errors"
	"os"
	"path/filepath"
	"strconv"
	"sync"
	"testing"
	"time"
)

func scheduledTestAction(t *testing.T, s *Server, id string, req auctionAction, now time.Time) auctionView {
	t.Helper()
	v, err := s.scheduledAuctionAction(id, req, now)
	if err != nil {
		t.Fatal(req.Action, err)
	}
	return v
}

func TestScheduledAuctionEscrowSelectionAndRestart(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	rules := map[string]auctionCalendarRule{"daily": {2, 10, 9, 18, 19}}
	s.config.AuctionCalendar = rules
	for _, id := range []string{"seller", "early", "late"} {
		auctionTestRole(t, s, id, nil)
	}
	if err := s.roles.Update("seller", func(r Record) error {
		r["Goods"] = []any{Record{"GoodsId": "a", "Name": "甲卷", "Type": 1, "Rank": 5, "Count": 2, "Level": 11, "TemplateId": "legible", "Extra": "preserved"},
			Record{"GoodsId": "b", "Name": "乙卷", "Type": 1, "Rank": 5, "Count": 1, "Level": 11},
			Record{"GoodsId": "c", "Name": "丙卷", "Type": 1, "Rank": 1, "Count": 1}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	start := time.Date(2026, 11, 20, 9, 0, 0, 0, auctionZone)
	view := auctionAction{Action: "view", Tier: "daily"}
	first := scheduledTestAction(t, s, "seller", view, start)
	req := auctionAction{Tier: "daily", Action: "submit", Count: 1, Amount: 100, EventID: first.EventID}
	for _, id := range []string{"a", "c", "b"} {
		req.RequestID, req.GoodsID = id, id
		scheduledTestAction(t, s, "seller", req, start)
	}
	// A lost reply and reconnect must not consign a second item.
	scheduledTestAction(t, s, "seller", req, start.Add(time.Second))
	if bag := goodsList(s.roles.Get("seller")); len(bag) != 1 || transferStackCount(bag[0].(Record)) != 1 {
		t.Fatal(bag)
	}
	preview := start.Add(9 * time.Hour)
	book := scheduledTestAction(t, s, "seller", view, preview)
	if book.Phase != "preview" || len(book.Lots) != 2 || book.Lots[0].Name != "甲卷" || book.Lots[1].Name != "乙卷" || len(book.Deliveries) != 1 || book.Deliveries[0].Name != "丙卷" {
		t.Fatal(book)
	}
	lotID := book.Lots[0].ID
	bid := auctionAction{Action: "bid", Tier: "daily", LotID: lotID, Amount: 500, EventID: book.EventID, RequestID: "first"}
	if _, err := s.scheduledAuctionAction("seller", bid, preview); err == nil {
		t.Fatal("seller accepted")
	}
	leader := scheduledTestAction(t, s, "early", bid, preview)
	if leader.Balance != 500 || leader.Lots[0].Price != 100 || leader.Lots[0].MyMax != 500 {
		t.Fatal(leader)
	}
	bid.Amount = 700
	bid.RequestID = "raise"
	leader = scheduledTestAction(t, s, "early", bid, preview)
	if leader.Balance != 300 || leader.Lots[0].Price != 100 {
		t.Fatal(leader)
	}
	bid.RequestID = "equal"
	loser := scheduledTestAction(t, s, "late", bid, preview)
	if loser.Balance != 1000 || loser.Lots[0].Leader != "early" || loser.Lots[0].Price != 700 || loser.Lots[0].MyMax != 0 || !loser.Lots[0].Participated || loser.Lots[0].LastBid != 700 {
		t.Fatal(loser)
	}
	live := preview.Add(time.Hour)
	bid.Amount = 800
	bid.RequestID = "win"
	// Two retries race across independent connections for the same role.
	var wg sync.WaitGroup
	errs := make(chan error, 2)
	for i := 0; i < 2; i++ {
		wg.Add(1)
		go func() { defer wg.Done(); _, err := s.scheduledAuctionAction("late", bid, live); errs <- err }()
	}
	wg.Wait()
	close(errs)
	for err := range errs {
		if err != nil {
			t.Fatal(err)
		}
	}
	if roleInt(s.roles.Get("early"), "ShinyStone") != 1000 || roleInt(s.roles.Get("late"), "ShinyStone") != 200 {
		t.Fatal("hold/refund")
	}
	bid.Amount = 900
	if _, err := s.scheduledAuctionAction("late", bid, live); err == nil {
		t.Fatal("changed replay accepted")
	}
	// Restart while the entire maximum is frozen. The second lot is unsold.
	if err := s.privateChat.close(); err != nil {
		t.Fatal(err)
	}
	s = auctionTestServer(t, dir)
	s.config.AuctionCalendar = rules
	atEnd := live.Add(20 * time.Second)
	if err := s.advanceScheduledAuctions(atEnd); err != nil {
		t.Fatal(err)
	}
	if err := s.advanceScheduledAuctions(atEnd.Add(time.Second)); err != nil {
		t.Fatal(err)
	}
	winner := scheduledTestAction(t, s, "late", view, atEnd)
	if winner.Balance != 295 || roleInt(s.roles.Get("seller"), "ShinyStone") != 1670 || len(winner.Deliveries) != 1 || winner.Lots[0].Status != "sold" || winner.Lots[1].Status != "passed" {
		t.Fatal("expected settlement 705, fee 35", winner, s.roles.Get("seller"))
	}
	nextDay := atEnd.Add(24 * time.Hour)
	bid.RequestID = "late"
	if _, err := s.scheduledAuctionAction("late", bid, nextDay); err == nil {
		t.Fatal("old event accepted")
	}
	next := scheduledTestAction(t, s, "late", view, nextDay)
	claim := auctionAction{Action: "claim", Tier: "daily", RequestID: "collect", EventID: next.EventID, LotID: lotID}
	scheduledTestAction(t, s, "late", claim, nextDay)
	scheduledTestAction(t, s, "late", claim, nextDay)
	inv := goodsList(s.roles.Get("late"))
	if len(inv) != 1 || roleString(inv[0].(Record), "Extra") != "preserved" || transferStackCount(inv[0].(Record)) != 1 || len(auctionMap(s.roles.Get("seller"), "AuctionDeliveries")) != 2 {
		t.Fatal("escrow metadata/returns", inv)
	}
}

func TestScheduledAuctionSpeechEntitlementsAndLotBoundary(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	t.Cleanup(func() { s.privateChat.close() })
	auctionTestRole(t, s, "speaker", nil)
	if err := s.roles.Update("speaker", func(r Record) error { r["ShinyStone"] = 3000; return nil }); err != nil {
		t.Fatal(err)
	}
	live := time.Date(2026, 11, 21, 19, 0, 0, 0, auctionZone)
	view := auctionAction{Action: "view", Tier: "daily"}
	book := scheduledTestAction(t, s, "speaker", view, live)
	if len(book.Lots) != 8 || book.Lots[0].Source != "system" || book.Lots[0].Kind != 1 {
		t.Fatal(book)
	}
	svc := auctionAction{Action: "service", Tier: "daily", Service: "fruit", EventID: book.EventID, RequestID: "fruit"}
	scheduledTestAction(t, s, "speaker", svc, live)
	svc.RequestID, svc.Service = "room", "room"
	v := scheduledTestAction(t, s, "speaker", svc, live)
	if v.Balance != 600 || roleInt(v.Seat, "remaining") != 36 || roleString(v.Seat, "room") != "天字1号" {
		t.Fatal(v.Seat, v.Balance)
	}
	path := filepath.Join(dir, "roles.json")
	before, err := os.Stat(path)
	if err != nil {
		t.Fatal(err)
	}
	chat := auctionAction{Action: "chat", Tier: "daily", Text: "诸位道友，请看此卷。", EventID: v.EventID, LotID: v.Lots[0].ID, RequestID: "speech"}
	spoken := scheduledTestAction(t, s, "speaker", chat, live)
	scheduledTestAction(t, s, "speaker", chat, live)
	if roleInt(spoken.Seat, "remaining") != 35 || len(spoken.Messages) != 1 || spoken.Messages[0].Effect != "gold" {
		t.Fatal(spoken)
	}
	chat.RequestID = "next"
	if _, err := s.scheduledAuctionAction("speaker", chat, live.Add(time.Second)); err == nil {
		t.Fatal("cooldown ignored")
	}
	if err := s.privateChat.close(); err != nil {
		t.Fatal(err)
	}
	s = auctionTestServer(t, dir)
	spoken = scheduledTestAction(t, s, "speaker", chat, live.Add(3*time.Second))
	after, err := os.Stat(path)
	if err != nil {
		t.Fatal(err)
	}
	if !before.ModTime().Equal(after.ModTime()) || roleInt(spoken.Seat, "remaining") != 34 {
		t.Fatal("chat changed economy store or reset quota")
	}
	next := live.Add(45 * time.Second)
	chat.RequestID = "stale"
	if _, err := s.scheduledAuctionAction("speaker", chat, next); err == nil {
		t.Fatal("speech crossed lot")
	}
	current := scheduledTestAction(t, s, "speaker", view, next)
	if len(current.Messages) != 0 || current.Lots[1].Status != "live" {
		t.Fatal(current)
	}
	chat.LotID = current.Lots[1].ID
	chat.Text = "<size=80>旧拍台</size>"
	if _, err := s.scheduledAuctionAction("speaker", chat, next); err == nil {
		t.Fatal("rich text accepted")
	}
}

func TestScheduledAuctionCalendarAndAtomicFailure(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	auctionTestRole(t, s, "buyer", nil)
	before := time.Date(2028, 2, 1, 8, 59, 59, 0, auctionZone)
	after := before.Add(time.Second)
	if roleString(s.auctionSession("monthly", before), "id") != "monthly-20280131" || roleString(s.auctionSession("monthly", after), "id") != "monthly-20280229" {
		t.Fatal("leap month boundary")
	}
	mon := time.Date(2026, 11, 23, 9, 0, 0, 0, auctionZone)
	if roleString(s.auctionSession("weekly", mon), "id") != "weekly-20261128" || roleString(s.auctionSession("weekly", mon.Add(-time.Second)), "id") != "weekly-20261121" {
		t.Fatal("weekly boundary")
	}
	now := time.Date(2026, 11, 20, 18, 30, 0, 0, auctionZone)
	book := scheduledTestAction(t, s, "buyer", auctionAction{Action: "view", Tier: "daily"}, now)
	lot := book.Lots[0]
	req := auctionAction{Action: "bid", Tier: "daily", EventID: book.EventID, RequestID: "rollback", LotID: lot.ID, Amount: lot.Price}
	if err := s.roles.Update("buyer", func(r Record) error { r["ShinyStone"] = req.Amount + 100; return nil }); err != nil {
		t.Fatal(err)
	}
	original := s.roles.path
	s.roles.path = filepath.Dir(original) // rename over a non-empty directory fails
	_, err := s.scheduledAuctionAction("buyer", req, now)
	s.roles.path = original
	var refusal auctionRefusal
	if err == nil || errors.As(err, &refusal) {
		t.Fatalf("expected persistence error: %v", err)
	}
	v := scheduledTestAction(t, s, "buyer", auctionAction{Action: "view", Tier: "daily"}, now)
	if v.Balance != req.Amount+100 || v.Lots[0].Leader != "" {
		t.Fatal("failed write published")
	}
	v = scheduledTestAction(t, s, "buyer", req, now)
	if v.Balance != 100 {
		t.Fatal("retry failed", v)
	}
}

// Exercise the largest default catalogue with a full recent-message window.
// Includes the JSON a room client receives, without simulating network latency.
func BenchmarkScheduledAuctionLiveView(b *testing.B) {
	store, err := OpenStore(b.TempDir(), "roles")
	if err != nil {
		b.Fatal(err)
	}
	s := &Server{roles: store}
	b.Cleanup(func() { s.privateChat.close() })
	now := time.Date(2026, 11, 30, 21, 0, 1, 0, auctionZone)
	view := auctionAction{Action: "view", Tier: "monthly"}
	if err = store.Transact(func(tx *StoreTx) error {
		for i := 0; i < 32; i++ {
			id := strconv.Itoa(i)
			r := tx.GetOrCreate(id)
			r["Id"], r["Name"] = id, "道友"+id
		}
		return nil
	}); err != nil {
		b.Fatal(err)
	}
	v, err := s.scheduledAuctionAction("0", view, now)
	if err != nil || len(v.Lots) != 24 {
		b.Fatal(err, len(v.Lots))
	}
	for i := 0; i < 32; i++ {
		id := strconv.Itoa(i)
		req := auctionAction{Action: "chat", Tier: view.Tier, EventID: v.EventID, LotID: v.Lots[0].ID, RequestID: "first", Text: "请掌柜揭盏，再看此宝的灵纹。"}
		if err = s.sendScheduledChat(id, req, now); err != nil {
			b.Fatal(err)
		}
	}
	v, err = s.scheduledAuctionAction("0", view, now)
	if err != nil || len(v.Messages) != 32 {
		b.Fatal(err, len(v.Messages))
	}
	payload, err := json.Marshal(v)
	if err != nil {
		b.Fatal(err)
	}
	b.ReportAllocs()
	b.ResetTimer()
	b.RunParallel(func(pb *testing.PB) {
		for pb.Next() {
			reply, err := s.scheduledAuctionAction("0", view, now)
			if err == nil {
				_, err = json.Marshal(reply)
			}
			if err != nil {
				b.Error(err)
				return
			}
		}
	})
	b.ReportMetric(float64(len(payload)), "bytes/reply")
}

func TestScheduledAuctionFreeSpeechQuota(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	t.Cleanup(func() { s.privateChat.close() })
	auctionTestRole(t, s, "guest", nil)
	now := time.Date(2026, 11, 21, 19, 0, 0, 0, auctionZone)
	for i := 0; i < 4; i++ {
		v := scheduledTestAction(t, s, "guest", auctionAction{Action: "view", Tier: "daily"}, now)
		var active string
		for _, lot := range v.Lots {
			if lot.Status == "live" {
				active = lot.ID
			}
		}
		req := auctionAction{Action: "chat", Tier: "daily", LotID: active, EventID: v.EventID, Text: "请开匣。", RequestID: strconv.Itoa(i)}
		_, err := s.scheduledAuctionAction("guest", req, now)
		if i < 3 && err != nil || i == 3 && !errors.Is(err, auctionRefusal("本场传音次数已用尽")) {
			t.Fatal(i, err)
		}
		now = now.Add(20 * time.Second)
	}
}
