package main

import (
	"errors"
	"strings"
	"testing"
)

func TestFriendOfferWaitsForSenderReceipt(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	_, _ = auctionTestRole(t, s, "sender", []any{
		Record{"GoodsId": "pills", "TemplateId": "jade-pill", "Name": "玉丹", "Rank": 1, "Level": 1, "Type": 19, "Count": 4},
		Record{"GoodsId": "herb", "TemplateId": "green-herb", "Name": "灵草", "Rank": 1, "Level": 1, "Type": 19, "Count": 3},
	})
	_, _ = auctionTestRole(t, s, "receiver", nil)
	for _, pair := range [][2]string{{"sender", "receiver"}, {"receiver", "sender"}} {
		if err := s.roles.Update(pair[0], func(r Record) error {
			putFriendEntry(r, friendLinks, pair[1], 0)
			r["ShinyStone"] = 500
			return nil
		}); err != nil {
			t.Fatal(err)
		}
	}

	declined, accepted := strings.Repeat("a", 32), strings.Repeat("b", 32)
	propose := func(offerID, itemID string, count int) error {
		t.Helper()
		msg := transferTestRequest(t, s, 1, "receiver", transferTestItem{itemID, count})
		req, err := s.decode("RemoteSendGoodsReq_1014005", msg.Data)
		if err != nil {
			t.Fatal(err)
		}
		return s.createFriendOffer("sender", "receiver", offerID, req.Get(field(req, "Goods")).List())
	}
	for _, action := range []struct {
		id, goods string
		amount    int
	}{{declined, "herb", 1}, {accepted, "pills", 2}, {accepted, "pills", 2}} {
		if err := propose(action.id, action.goods, action.amount); err != nil {
			t.Fatal(err)
		}
	}

	s = auctionTestServer(t, dir) // Receiver comes online after the two durable offers were written.
	from, to := s.roles.Get("sender"), s.roles.Get("receiver")
	if transferStackCount(locateGoods(from, "pills")) != 4 || len(goodsList(to)) != 0 ||
		roleInt(from, "ShinyStone") != 500 || len(friendMap(to, friendOffersKey)) != 2 ||
		!hasFriendOfferLock(locateGoods(from, "pills")) || !hasFriendOfferLock(locateGoods(from, "herb")) {
		t.Fatal("creating or replaying an offer did not reserve the original stacks")
	}
	if err := propose(strings.Repeat("c", 32), "pills", 1); !errors.Is(err, previewReject(4509)) {
		t.Fatalf("reserved stack could be used in a second offer: %v", err)
	}
	if _, _, err := s.decideFriendOffer("sender", accepted, true); !errors.Is(err, previewReject(friendOfferUnavailable)) {
		t.Fatalf("a different player decided a recipient's offer: %v", err)
	}
	if _, changed, err := s.decideFriendOffer("receiver", declined, false); err != nil || !changed {
		t.Fatalf("decline failed: %v", err)
	}
	if _, changed, err := s.decideFriendOffer("receiver", accepted, true); err != nil || !changed {
		t.Fatalf("acceptance failed: %v", err)
	}
	if _, changed, err := s.decideFriendOffer("receiver", accepted, true); err != nil || changed {
		t.Fatalf("replayed choice was not idempotent: %v", err)
	}
	s = auctionTestServer(t, dir) // Sender comes back later to both receipts and still has both locked stacks.
	from, to = s.roles.Get("sender"), s.roles.Get("receiver")
	if transferStackCount(locateGoods(from, "pills")) != 4 || len(friendMap(from, friendOfferReceiptsKey)) != 2 ||
		!hasFriendOfferLock(locateGoods(from, "pills")) || !hasFriendOfferLock(locateGoods(from, "herb")) ||
		roleInt(from, "ShinyStone") != 300 || len(goodsList(to)) != 1 ||
		transferStackCount(goodsList(to)[0].(Record)) != 2 || locateGoods(to, "pills") != nil {
		t.Fatal("decisions changed the source quantities before acknowledgment or duplicated the received item")
	}
	sender, _ := auctionTestRole(t, s, "sender", nil)
	unlock := s.pb("GoodsUnLockReq_1004040")
	set(unlock, "GoodsId", "pills")
	out := s.gameResponse("GoodsUnLockResp_1004040", false, 2000)
	if _, err := s.trialUnlockGoods(sender, unlock, out); !errors.Is(err, previewReject(4509)) {
		t.Fatalf("acknowledgment lock was bypassed: %v", err)
	}
	acceptReceipt, declineReceipt := friendOfferReceiptID("receiver", accepted), friendOfferReceiptID("receiver", declined)
	for _, id := range []string{acceptReceipt, acceptReceipt, declineReceipt} {
		if balance, err := s.acknowledgeFriendOffer("sender", id); err != nil || balance != 300 {
			t.Fatalf("receipt cleanup failed or charged a second fee: %d %v", balance, err)
		}
	}
	from, to = s.roles.Get("sender"), s.roles.Get("receiver")
	if transferStackCount(locateGoods(from, "pills")) != 2 || transferStackCount(locateGoods(from, "herb")) != 3 ||
		inventoryFlag(locateGoods(from, "pills"), "IsLock") || inventoryFlag(locateGoods(from, "herb"), "IsLock") ||
		len(friendMap(from, friendOfferReceiptsKey)) != 0 || len(goodsList(to)) != 1 || roleInt(from, "ShinyStone") != 300 {
		t.Fatal("acknowledgments did not settle each source stack exactly once")
	}
}
