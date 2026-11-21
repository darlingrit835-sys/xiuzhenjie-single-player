package main

import (
	"math"
	"sync"
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func fixedAuctionCall(t *testing.T, s *Server, p *Peer, cmd, seq int, values Record, want int64) protoreflect.Message {
	t.Helper()
	msg := auctionTestMessage(t, s, cmd, values)
	msg.Msid = seq
	s.handleGame(p, msg)
	out := auctionTestReply(t, s, p.conn.(*auctionTestConnection), msg, want)
	success := want == 4168 || want == 4253 || want == 4046 || want == 4251 || want == 4160
	if getBool(out, "IsSuccess") != success {
		t.Fatal(out)
	}
	return out
}

func TestFixedAuctionEscrowSaleReplayAndReload(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	seller, _ := auctionTestRole(t, s, "seller", []any{Record{"GoodsId": "private-stack", "TemplateId": "test-item", "Name": "测试物品",
		"Type": 19, "Rank": 2, "Level": 4, "Count": 5, "Extra": Record{"craft": "v1"}}})
	buyer, _ := auctionTestRole(t, s, "buyer", nil)
	second, _ := auctionTestRole(t, s, "second", nil)
	// Older roles may have only the outer ID. Both sides of history must still be identified.
	if err := s.roles.Update("seller", func(r Record) error { delete(r, "Id"); r["TitleId"] = []any{201}; return nil }); err != nil {
		t.Fatal(err)
	}
	s.game.auctionDiscounts = map[int]int64{201: 20}
	req := Record{"GoodsId": "private-stack", "Count": 2, "FixedPrice": 150, "ShopType": 82}
	fixedAuctionCall(t, s, seller, 1006008, 1, Record{"GoodsId": "private-stack", "Count": 2, "FixedPrice": 99, "ShopType": 82}, 4328)
	fixedAuctionCall(t, s, seller, 1006008, 2, Record{"GoodsId": "private-stack", "Count": 2, "FixedPrice": 150, "ShopType": 83}, 4008)
	first := fixedAuctionCall(t, s, seller, 1006008, 3, req, 4168)
	if getInt(first, "NowRingCapacity") != 1 || getInt(first, "ShinyStone") != 985 {
		t.Fatal(first)
	}
	fixedAuctionCall(t, s, seller, 1006008, 3, req, 4168)
	if n := transferStackCount(locateGoods(s.roles.Get("seller"), "private-stack")); n != 3 {
		t.Fatal("escrow quantity", n)
	}
	rows := func(out protoreflect.Message) protoreflect.List { return out.Get(field(out, "Object")).List() }
	my := rows(fixedAuctionCall(t, s, seller, 1006003, 4, Record{"Type": 82}, 4251))
	if my.Len() != 1 {
		t.Fatal(my)
	}
	mine := my.Get(0).Message()
	id := getString(mine, "Id")
	if getInt(mine, "Count") != 2 || getInt(mine, "FixedPrice") != 300 || getInt(mine, "RemainHours") != 24 {
		t.Fatal(mine)
	}
	if rows(fixedAuctionCall(t, s, buyer, 1006003, 4, Record{"Type": 82}, 4251)).Len() != 0 ||
		rows(fixedAuctionCall(t, s, seller, 1006003, 4, Record{"Type": 83}, 4251)).Len() != 0 {
		t.Fatal("my consignments leaked")
	}
	browse := Record{"ShopType": 82, "Name": "测试物品", "GoodsType": 19, "Rank": 2, "MinFinalPrice": 300, "MaxFinalPrice": 300}
	public := rows(fixedAuctionCall(t, s, buyer, 1006001, 5, browse, 4160))
	if public.Len() != 1 {
		t.Fatal(public)
	}
	display := public.Get(0).Message()
	if getString(display, "GoodsId") != id || id == "private-stack" || getInt(display, "Count") != 2 {
		t.Fatal(display)
	}
	if s.inventoryDetailItem("buyer", "private-stack", false) != nil || s.inventoryDetailItem("buyer", id, false) == nil ||
		s.inventoryDetailItem("", id, false) != nil {
		t.Fatal("public details did not isolate the seller's private item IDs")
	}
	target := Record{"AuctionId": id}
	fixedAuctionCall(t, s, buyer, 1006009, 6, target, 4008)
	fixedAuctionCall(t, s, seller, 1006010, 6, target, 4008)
	s = auctionTestServer(t, dir)
	seller, _ = auctionTestRole(t, s, "seller", nil)
	buyer, _ = auctionTestRole(t, s, "buyer", nil)
	second, _ = auctionTestRole(t, s, "second", nil)
	if rows(fixedAuctionCall(t, s, seller, 1006003, 1, Record{"Type": 82}, 4251)).Len() != 1 {
		t.Fatal("escrow missing on restart")
	}
	orig := s.roles.path
	s.roles.path = dir
	fixedAuctionCall(t, s, buyer, 1006010, 2, target, 4008)
	s.roles.path = orig
	if len(goodsList(s.roles.Get("buyer"))) != 0 || roleInt(s.roles.Get("seller"), "ShinyStone") != 985 {
		t.Fatal("failed store replace moved money or goods")
	}
	var wg sync.WaitGroup
	for _, peer := range []*Peer{buyer, second} {
		msg := auctionTestMessage(t, s, 1006010, target)
		wg.Go(func() { s.handleGame(peer, msg) })
	}
	wg.Wait()
	var winner, loser *Peer
	for _, peer := range []*Peer{buyer, second} {
		reply := peer.conn.(*auctionTestConnection).receive(t, s, 1006010, 7, "AuctionBuyResp_1006010")
		switch getInt(reply, "Code") {
		case 4046:
			if winner != nil || getInt(reply, "ShinyStone") != 700 {
				t.Fatal(reply)
			}
			winner = peer
		case 4171:
			loser = peer
		default:
			t.Fatal(reply)
		}
	}
	if winner == nil || loser == nil {
		t.Fatal("competing buyers did not settle once")
	}
	fixedAuctionCall(t, s, winner, 1006010, 7, target, 4046)
	s = auctionTestServer(t, dir)
	goods := goodsList(s.roles.Get(winner.currentRoleID()))
	if len(goods) != 1 || transferStackCount(goods[0].(Record)) != 2 || roleString(goods[0].(Record)["Extra"].(Record), "craft") != "v1" ||
		roleString(goods[0].(Record), "GoodsId") == "private-stack" || roleInt(s.roles.Get("seller"), "ShinyStone") != 1285 ||
		roleInt(s.roles.Get(loser.currentRoleID()), "ShinyStone") != 1000 {
		t.Fatal("final transfer inconsistent", goods)
	}
	if s.inventoryDetailItem(winner.currentRoleID(), id, false) != nil || len(fixedAuctionBook(s.roles.Get(previewAuctionStoreID))) != 0 {
		t.Fatal("sold escrow remained visible")
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 3 {
		t.Fatal(events, err)
	}
	for _, e := range events {
		if e.EntityID != id || roleInt(e.Details, "quantity") != 2 || roleInt(e.Details, "amount") != 300 {
			t.Fatal(e)
		}
		switch e.Kind {
		case "auction.consigned":
			if e.Actor.RoleID != "seller" || roleInt(e.Details, "listingFee") != 15 {
				t.Fatal(e)
			}
		case "auction.purchase":
			if e.Actor.RoleID != winner.currentRoleID() || e.Subject == nil || e.Subject.RoleID != "seller" || e.Subject.AccountID != "account-seller" {
				t.Fatal(e)
			}
		case "auction.sold":
			if e.Actor.RoleID != "seller" || e.Subject == nil || e.Subject.RoleID != winner.currentRoleID() {
				t.Fatal(e)
			}
		default:
			t.Fatal(e)
		}
	}
}

func TestFixedAuctionExpiredCancellationAndBagCapacity(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	seller, _ := auctionTestRole(t, s, "seller", []any{Record{"GoodsId": "unique", "Name": "测试法器", "Type": 15, "Count": 1, "Extra": "unique-stat"}})
	buyer, _ := auctionTestRole(t, s, "buyer", nil)
	req := Record{"GoodsId": "unique", "FixedPrice": 200, "Count": 1, "ShopType": 83}
	for _, rule := range []struct {
		flag string
		code int64
	}{{"IsBinding", 4247}, {"IsUsing", 4546}, {"IsLock", 4509}} {
		if err := s.roles.Update("seller", func(r Record) error { locateGoods(r, "unique")[rule.flag] = true; return nil }); err != nil {
			t.Fatal(err)
		}
		fixedAuctionCall(t, s, seller, 1006008, 1, req, rule.code)
		if err := s.roles.Update("seller", func(r Record) error { delete(locateGoods(r, "unique"), rule.flag); return nil }); err != nil {
			t.Fatal(err)
		}
	}
	out := fixedAuctionCall(t, s, seller, 1006008, 2, req, 4168)
	if getInt(out, "NowRingCapacity") != 0 || getInt(out, "ShinyStone") != 980 {
		t.Fatal(out)
	}
	my := fixedAuctionCall(t, s, seller, 1006003, 3, Record{"Type": 83}, 4251)
	id := getString(my.Get(field(my, "Object")).List().Get(0).Message(), "Id")
	target := Record{"AuctionId": id}
	fill := func(r Record) error {
		items := make([]any, 100)
		for i := range items {
			items[i] = Record{"GoodsId": "filler", "Type": 15}
		}
		r["Goods"] = items
		return nil
	}
	if err := s.roles.Update("buyer", fill); err != nil {
		t.Fatal(err)
	}
	fixedAuctionCall(t, s, buyer, 1006010, 4, target, 4158)
	if err := s.roles.Update("seller", fill); err != nil {
		t.Fatal(err)
	}
	fixedAuctionCall(t, s, seller, 1006009, 5, target, 4158)
	if err := s.roles.Update("buyer", func(r Record) error { r["Goods"] = []any{}; r["ShinyStone"] = 100; return nil }); err != nil {
		t.Fatal(err)
	}
	fixedAuctionCall(t, s, buyer, 1006010, 4, target, 4173)
	if err := s.roles.Update("buyer", func(r Record) error { r["ShinyStone"] = 1000; return nil }); err != nil {
		t.Fatal(err)
	}
	if err := s.roles.Update("seller", func(r Record) error { r["ShinyStone"] = int64(math.MaxInt64); return nil }); err != nil {
		t.Fatal(err)
	}
	fixedAuctionCall(t, s, buyer, 1006010, 4, target, 4008)
	if err := s.roles.Update(previewAuctionStoreID, func(state Record) error {
		fixedAuctionBook(state)[id].(Record)["EndsAt"] = time.Now().Add(-time.Second).UnixMilli()
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	fixedAuctionCall(t, s, buyer, 1006010, 7, target, 4171)
	pub := fixedAuctionCall(t, s, buyer, 1006001, 8, Record{"ShopType": 83}, 4160)
	if pub.Get(field(pub, "Object")).List().Len() != 0 || s.inventoryDetailItem("buyer", id, false) != nil || s.inventoryDetailItem("seller", id, false) == nil {
		t.Fatal("expired public listing still exposed")
	}
	if err := s.roles.Update("seller", func(r Record) error { r["Goods"] = goodsList(r)[:99]; r["ShinyStone"] = 980; return nil }); err != nil {
		t.Fatal(err)
	}
	fixedAuctionCall(t, s, seller, 1006009, 5, target, 4253)
	fixedAuctionCall(t, s, seller, 1006009, 5, target, 4253)
	recovered := s.roles.Get("seller")
	if roleString(locateGoods(recovered, "unique"), "Extra") != "unique-stat" || len(goodsList(recovered)) != 100 || roleInt(recovered, "ShinyStone") != 980 {
		t.Fatal("withdrawal did not preserve instance and charged fee")
	}
	if len(goodsList(s.roles.Get("buyer"))) != 0 || roleInt(s.roles.Get("buyer"), "ShinyStone") != 1000 {
		t.Fatal("rejected buy charged the buyer")
	}
}

func TestFixedAuctionCancelAndBuyCompeteForOneItem(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, _ := auctionTestRole(t, s, "seller", []any{Record{"GoodsId": "only", "Type": 15, "Name": "测试装备"}})
	b, _ := auctionTestRole(t, s, "buyer", nil)
	fixedAuctionCall(t, s, p, 1006008, 1, Record{"GoodsId": "only", "Count": 1, "ShopType": 83, "FixedPrice": 100}, 4168)
	my := fixedAuctionCall(t, s, p, 1006003, 2, Record{"Type": 83}, 4251)
	target := Record{"AuctionId": getString(my.Get(field(my, "Object")).List().Get(0).Message(), "Id")}
	buy, cancel := auctionTestMessage(t, s, 1006010, target), auctionTestMessage(t, s, 1006009, target)
	var wg sync.WaitGroup
	wg.Go(func() { s.handleGame(p, cancel) })
	wg.Go(func() { s.handleGame(b, buy) })
	wg.Wait()
	sold := b.conn.(*auctionTestConnection).receive(t, s, 1006010, 7, "AuctionBuyResp_1006010")
	recalled := p.conn.(*auctionTestConnection).receive(t, s, 1006009, 7, "AuctionGoodsCancelResp_1006009")
	if getBool(recalled, "IsSuccess") == getBool(sold, "IsSuccess") {
		t.Fatal(recalled, sold)
	}
	seller, buyer := s.roles.Get("seller"), s.roles.Get("buyer")
	if locateGoods(seller, "only") != nil && locateGoods(buyer, "only") != nil ||
		len(goodsList(seller))+len(goodsList(buyer)) != 1 || roleInt(seller, "ShinyStone")+roleInt(buyer, "ShinyStone") != 1990 {
		t.Fatal("concurrent operations duplicated or lost assets")
	}
}
