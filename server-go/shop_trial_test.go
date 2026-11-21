package main

import (
	"testing"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func openTrialShopTestServer(t *testing.T, dir string) *Server {
	t.Helper()
	s := auctionTestServer(t, dir)
	s.dataDir = "../server/data"
	if err := s.loadTrialShop(); err != nil {
		t.Fatal(err)
	}
	if len(s.game.trialShop) == 0 {
		t.Fatal("bundled shop catalog is missing")
	}
	return s
}

func TestTrialShopBuysFromCatalogAndPersists(t *testing.T) {
	dir := t.TempDir()
	s := openTrialShopTestServer(t, dir)
	peer, conn := auctionTestRole(t, s, "buyer", nil)
	buy := func(id, seq int, code int64) protoreflect.Message {
		t.Helper()
		msg := auctionTestMessage(t, s, 1004035, Record{"Id": id})
		msg.Msid = seq
		s.handleGame(peer, msg)
		return auctionTestReply(t, s, conn, msg, code)
	}
	if getBool(buy(-1, 1, 5001), "IsSuccess") {
		t.Fatal("unknown catalog ID was accepted")
	}
	if reply := buy(1, 2, 4299); getInt(reply, "ShinyStone") != 940 || getInt(reply, "ShinyJade") != 20 || !getBool(reply, "IsSuccess") {
		t.Fatal(reply)
	}
	buy(1, 2, 4299)
	buy(21, 2, 4008)
	if reply := buy(21, 3, 4299); getInt(reply, "ShinyJade") != 0 || getInt(reply, "ShinyStone") != 940 || !getBool(reply, "IsSuccess") {
		t.Fatal(reply)
	}
	buy(21, 4, 4298)
	s = openTrialShopTestServer(t, dir)
	peer, conn = auctionTestRole(t, s, "buyer", nil)
	catalog := auctionTestMessage(t, s, 1004033, nil)
	s.handleGame(peer, catalog)
	out := auctionTestReply(t, s, conn, catalog, 2000)
	goods := out.Get(field(out, "Goods")).List()
	if goods.Len() != 3 || getInt(goods.Get(0).Message(), "Id") != 21 || getInt(goods.Get(0).Message(), "Count") != 1 {
		t.Fatal(out)
	}
	bag := goodsList(s.roles.Get("buyer"))
	if len(bag) != 2 || transferStackCount(bag[0].(Record)) != 1 || transferStackCount(bag[1].(Record)) != 1 {
		t.Fatalf("replayed purchase or goods save is inconsistent: %v", bag)
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 2 {
		t.Fatalf("purchase history was duplicated: %v %v", events, err)
	}
}

func TestTrialShopLimitAndBagFailureAreAtomic(t *testing.T) {
	s := openTrialShopTestServer(t, t.TempDir())
	day := mentorGameDay(time.Now())
	inventory := make([]any, 100)
	for i := range inventory {
		inventory[i] = Record{"Type": 15}
	}
	if err := s.roles.Put("buyer", Record{"ShinyJade": 100, "ShopPurchaseDay": day, "ShopDailyPurchases": Record{"21": 10}, "Goods": inventory}); err != nil {
		t.Fatal(err)
	}
	peer, connection := auctionTestRole(t, s, "buyer", nil)
	try := func(code int64) {
		t.Helper()
		msg := auctionTestMessage(t, s, 1004035, Record{"Id": 21})
		s.handleGame(peer, msg)
		auctionTestReply(t, s, connection, msg, code)
	}
	try(4296)
	if err := s.roles.Update("buyer", func(role Record) error {
		role["ShopPurchaseDay"] = "2000-01-01"
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	try(4527)
	if roleInt(s.roles.Get("buyer"), "ShinyJade") != 100 {
		t.Fatal("full bag charged the buyer")
	}
	if err := s.roles.Update("buyer", func(role Record) error {
		delete(role, "Goods")
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	try(4299)
	role := s.roles.Get("buyer")
	if roleInt(role, "ShinyJade") != 80 || roleInt(trialShopPurchases(role, day), "21") != 1 {
		t.Fatalf("daily reset or deduction did not persist: %v", role)
	}
}
