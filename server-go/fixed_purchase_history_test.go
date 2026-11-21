package main

import (
	"bytes"
	"encoding/json"
	"strconv"
	"testing"
	"time"
)

func fixedPurchaseQuery(t *testing.T, s *Server, p *Peer, body string, want int) Record {
	t.Helper()
	raw := auctionJSONCommandReply(t, s, p, fixedPurchaseHistoryCommand, []byte(body), want)
	var result Record
	if err := json.Unmarshal(raw, &result); err != nil {
		t.Fatal(err)
	}
	return result
}

func TestFixedAuctionPurchaseHistoryOwnershipAndCommit(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	seller, _ := auctionTestRole(t, s, "secret-seller-id", []any{Record{"GoodsId": "secret-instance", "Name": "回春散", "Type": 19, "Rank": 2, "Count": 2}})
	if err := s.roles.Update("secret-seller-id", func(r Record) error { r["Name"] = "松烟客"; return nil }); err != nil {
		t.Fatal(err)
	}
	buyer, _ := auctionTestRole(t, s, "buyer", nil)
	page := fixedPurchaseQuery(t, s, buyer, `{"page":1}`, 2000)
	if roleInt(page, "pages") != 1 || len(goodsListForField(page, "rows")) != 0 {
		t.Fatal("old save", page)
	}
	fixedAuctionCall(t, s, seller, 1006008, 2, Record{"GoodsId": "secret-instance", "Count": 2, "FixedPrice": 150, "ShopType": 82}, 4168)
	list := s.fixedAuctionListings("secret-seller-id", 82, time.Now())
	if len(list) != 1 {
		t.Fatal(list)
	}
	order := Record{"AuctionId": list[0].id}
	fixedAuctionCall(t, s, buyer, 1006010, 3, order, 4046)
	fixedAuctionCall(t, s, buyer, 1006010, 3, order, 4046)
	fixedAuctionCall(t, s, buyer, 1006010, 4, Record{"AuctionId": previewFixedAuctionListing.id}, 4046)
	fixedAuctionCall(t, s, buyer, 1006010, 5, Record{"AuctionId": previewFixedAuctionListing.id}, 4171)
	// Reopening the old store returns each committed sale exactly once.
	s = auctionTestServer(t, dir)
	buyer, _ = auctionTestRole(t, s, "buyer", nil)
	other, _ := auctionTestRole(t, s, "other", nil)
	page = fixedPurchaseQuery(t, s, buyer, `{"page":1}`, 2000)
	rows := goodsListForField(page, "rows")
	if len(rows) != 2 {
		t.Fatal(page)
	}
	npc, player := rows[0].(Record), rows[1].(Record)
	if roleString(npc, "seller") != "奇珍阁" || roleInt(npc, "price") != 500 || roleInt(npc, "count") != 1 ||
		roleString(player, "seller") != "松烟客" || roleInt(player, "price") != 300 || roleInt(player, "count") != 2 || roleInt(player, "rank") != 2 || roleInt(player, "kind") != 19 || roleInt(player, "at") == 0 {
		t.Fatal(rows)
	}
	raw, _ := json.Marshal(rows)
	if bytes.Contains(raw, []byte("secret-")) {
		t.Fatal("private ID exposed")
	}
	if got := fixedPurchaseQuery(t, s, other, `{"page":1,"roleId":"buyer"}`, 2000); len(goodsListForField(got, "rows")) != 0 {
		t.Fatal(got)
	}
	auctionTestRole(t, s, "buyer", nil)
	if got := fixedPurchaseQuery(t, s, buyer, `{"page":1}`, 4008); got["error"] == nil {
		t.Fatal("stale connection could read", got)
	}
}

func TestFixedAuctionHistoryRetentionAndPagination(t *testing.T) {
	s := auctionTestServer(t, t.TempDir())
	p, _ := auctionTestRole(t, s, "buyer", nil)
	err := s.roles.Update("buyer", func(r Record) error {
		for i := 1; i <= 56; i++ {
			addFixedPurchase(r, Record{"Name": "药" + strconv.Itoa(i), "Type": 19}, "奇珍阁", int64(i), time.Unix(int64(i), 0))
		}
		// Early shop receipts recorded zero for a single elixir.
		history := goodsListForField(r, "FixedPurchases")
		history[len(history)-1].(Record)["count"] = 0
		return nil
	})
	if err != nil {
		t.Fatal(err)
	}
	last := fixedPurchaseQuery(t, s, p, `{"page":999999}`, 2000)
	rows := goodsListForField(last, "rows")
	if roleInt(last, "pages") != 5 || roleInt(last, "page") != 5 || len(rows) != 10 || roleInt(rows[0].(Record), "price") != 16 || roleInt(rows[9].(Record), "price") != 7 {
		t.Fatal(last)
	}
	first := fixedPurchaseQuery(t, s, p, `{"page":1}`, 2000)
	newest := goodsListForField(first, "rows")[0].(Record)
	if roleInt(newest, "price") != 56 || roleInt(newest, "count") != 1 {
		t.Fatal(first)
	}
}
