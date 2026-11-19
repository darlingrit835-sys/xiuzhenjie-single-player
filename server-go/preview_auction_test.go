package main

import (
	"encoding/binary"
	"math"
	"sync"
	"testing"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

type auctionTestConnection struct {
	previewSink
	frames [][]byte
}

func (c *auctionTestConnection) Write(data []byte) (int, error) {
	c.frames = append(c.frames, append([]byte(nil), data...))
	return len(data), nil
}

func (c *auctionTestConnection) receive(t *testing.T, s *Server, cmd, messageID int, response string) protoreflect.Message {
	t.Helper()
	if len(c.frames) == 0 {
		t.Fatalf("no frame for response %d", cmd)
	}
	frame := c.frames[0]
	c.frames = c.frames[1:]
	length, offset := binary.Uvarint(frame)
	if offset <= 0 || length != uint64(len(frame)-offset) {
		t.Fatalf("invalid frame length %d", length)
	}
	envelope, err := s.decode("Msg", frame[offset:])
	if err != nil {
		t.Fatal(err)
	}
	if getInt(envelope, "Cmd") != int64(cmd) || getInt(envelope, "Msid") != int64(messageID) {
		t.Fatalf("unexpected frame: %v", envelope)
	}
	out, err := s.decode(response, envelope.Get(field(envelope, "Data")).Bytes())
	if err != nil {
		t.Fatal(err)
	}
	return out
}

func auctionTestServer(t *testing.T, dir string) *Server {
	t.Helper()
	protocol, err := loadProtocol()
	if err != nil {
		t.Fatal(err)
	}
	registry, err := loadPreview(protocol)
	if err != nil {
		t.Fatal(err)
	}
	roles, err := OpenStore(dir, "roles")
	if err != nil {
		t.Fatal(err)
	}
	roles.enableHistory()
	return &Server{protocol: protocol, preview: registry, previewWrites: true, roles: roles}
}

func auctionTestRole(t *testing.T, s *Server, id string, goods []any) (*Peer, *auctionTestConnection) {
	t.Helper()
	if s.roles.Get(id) == nil {
		role := Record{"Id": id, "UserId": "account-" + id, "Name": id, "Level": 31, "ShinyStone": 1000, "ShinyJade": 20, "Goods": goods}
		if err := s.roles.Put(id, role); err != nil {
			t.Fatal(err)
		}
	}
	client := &auctionTestConnection{}
	peer := &Peer{conn: client}
	s.bindGameRole(peer, id)
	return peer, client
}

func auctionTestMessage(t *testing.T, s *Server, cmd int, values Record) Message {
	t.Helper()
	route := s.preview[cmd]
	msg := Message{Cmd: cmd, Msid: 7}
	if route.reqDesc != nil {
		req := s.pb(route.request)
		putPB(req, values)
		var err error
		msg.Data, err = proto.Marshal(req)
		if err != nil {
			t.Fatal(err)
		}
	}
	return msg
}

func auctionTestReply(t *testing.T, s *Server, c *auctionTestConnection, msg Message, code int64) protoreflect.Message {
	t.Helper()
	out := c.receive(t, s, msg.Cmd, msg.Msid, s.preview[msg.Cmd].response)
	if getInt(out, "Code") != code {
		t.Fatalf("Cmd %d: expected status code %d, got %v", msg.Cmd, code, out)
	}
	return out
}

func auctionTestEvent(t *testing.T, s *Server, c *auctionTestConnection, eventType int64) protoreflect.Message {
	t.Helper()
	out := c.receive(t, s, 4001002, 0, "ReceiveMessageResp_4001002")
	if !getBool(out, "IsSuccess") || getString(out, "ChatId") != "4" || getInt(out, "Type") != eventType {
		t.Fatalf("unexpected auction room event: %v", out)
	}
	return out
}

func TestPreviewAuctionOnlyOneFixedPurchasePersists(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	first, one := auctionTestRole(t, s, "试用甲", nil)
	second, two := auctionTestRole(t, s, "试用乙", nil)
	list := auctionTestMessage(t, s, 1006001, Record{"ShopType": 82, "FiveElement": -1, "PageNum": 1, "PageSize": 10})
	buy := auctionTestMessage(t, s, 1006010, Record{"AuctionId": previewFixedAuctionListing.id})
	s.handleGame(first, list)
	listed := auctionTestReply(t, s, one, list, 4160)
	if listed.Get(field(listed, "Object")).List().Len() != 1 {
		t.Fatalf("missing trial listing: %v", listed)
	}
	s.previewWrites = false
	s.handleGame(first, buy)
	auctionTestReply(t, s, one, buy, previewDeniedCode)
	s.previewWrites = true
	path := s.roles.path
	s.roles.path = t.TempDir()
	s.handleGame(first, buy)
	s.roles.path = path
	auctionTestReply(t, s, one, buy, previewDeniedCode)
	assertAuctionHistory(t, s)
	var both sync.WaitGroup
	both.Go(func() { s.handleGame(first, buy) })
	both.Go(func() { s.handleGame(second, buy) })
	both.Wait()
	firstOut := one.receive(t, s, buy.Cmd, buy.Msid, s.preview[buy.Cmd].response)
	secondOut := two.receive(t, s, buy.Cmd, buy.Msid, s.preview[buy.Cmd].response)
	if getBool(firstOut, "IsSuccess") == getBool(secondOut, "IsSuccess") {
		t.Fatalf("exactly one purchase must succeed: %v / %v", firstOut, secondOut)
	}
	winner, bidderOut, loserOut := first, firstOut, secondOut
	if getBool(secondOut, "IsSuccess") {
		winner, bidderOut, loserOut = second, secondOut, firstOut
	}
	if getInt(bidderOut, "ShinyStone") != 500 || getInt(loserOut, "Code") != 4171 {
		t.Fatalf("invalid purchase or replay response: %v / %v", bidderOut, loserOut)
	}
	reopened := auctionTestServer(t, dir)
	for _, original := range []*Peer{first, second} {
		id := original.currentRoleID()
		role := reopened.roles.Get(id)
		if original == winner {
			goods := goodsList(role)
			if roleInt(role, "ShinyStone") != 500 || len(goods) != 1 || roleString(goods[0].(Record), "TemplateId") != previewTreasureTemplate {
				t.Fatalf("winning debit and item did not persist together: %v", role)
			}
		} else if roleInt(role, "ShinyStone") != 1000 || len(goodsList(role)) != 0 {
			t.Fatalf("losing purchase changed its role save: %v", role)
		}
	}
	peer, client := auctionTestRole(t, reopened, winner.currentRoleID(), nil)
	reopened.handleGame(peer, list)
	listed = auctionTestReply(t, reopened, client, list, 4160)
	if listed.Get(field(listed, "Object")).List().Len() != 0 {
		t.Fatalf("sold listing returned after reopen: %v", listed)
	}
	reopened.handleGame(peer, buy)
	auctionTestReply(t, reopened, client, buy, 4171)
	assertAuctionHistory(t, reopened, auctionHistoryExpectation{
		kind: "auction.purchase", actor: winner.currentRoleID(), entityType: "auction.listing", entityID: previewFixedAuctionListing.id, amount: 500,
	})
}

func TestPreviewAuctionRefundAndOneSettlementAfterReopen(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	first, one := auctionTestRole(t, s, "南峰", nil)
	second, two := auctionTestRole(t, s, "北峰", nil)
	enter := auctionTestMessage(t, s, 1006017, Record{"Stage": 4})
	current := auctionTestMessage(t, s, 1006021, nil)
	list := auctionTestMessage(t, s, 1006018, nil)
	s.handleGame(first, list)
	auctionTestReply(t, s, one, list, 4450)
	for _, client := range []struct {
		peer *Peer
		conn *auctionTestConnection
	}{{first, one}, {second, two}} {
		s.handleGame(client.peer, enter)
		entered := auctionTestReply(t, s, client.conn, enter, 4448)
		if getInt(entered, "TreasureId") != 4 || !getBool(entered, "IsMayAddPrice") {
			t.Fatalf("valid realm or bidding state incorrect: %v", entered)
		}
		s.handleGame(client.peer, current)
		lot := auctionTestReply(t, s, client.conn, current, 4457)
		if getInt(lot, "Id") != 4 || getInt(lot, "CurrentPrice") != 350 {
			t.Fatalf("wrong initial room lot: %v", lot)
		}
		auctionTestEvent(t, s, client.conn, 4)
		auctionTestEvent(t, s, client.conn, 3)
	}
	for _, rejected := range []struct {
		price int64
		code  int64
	}{{-50, 4039}, {384, 4039}, {math.MaxInt64, 4039}, {1100, 4452}} {
		bid := auctionTestMessage(t, s, 1006020, Record{"TreasureId": 4, "Price": rejected.price})
		s.handleGame(first, bid)
		auctionTestReply(t, s, one, bid, rejected.code)
	}
	for _, offer := range []struct {
		peer   *Peer
		client *auctionTestConnection
		price  int
		coins  int64
	}{{first, one, 385, 615}, {first, one, 420, 580}, {second, two, 455, 545}} {
		bid := auctionTestMessage(t, s, 1006020, Record{"TreasureId": 4, "Price": offer.price})
		s.handleGame(offer.peer, bid)
		out := auctionTestReply(t, s, offer.client, bid, 4456)
		if getInt(out, "ShinyStone") != offer.coins {
			t.Fatalf("wrong escrow debit: %v", out)
		}
		for _, client := range []*auctionTestConnection{one, two} {
			event := auctionTestEvent(t, s, client, 1)
			if getString(event, "RoleName") != offer.peer.currentRoleID() {
				t.Fatalf("wrong current bidder: %v", event)
			}
			auctionTestEvent(t, s, client, 3)
		}
		s.handleGame(offer.peer, bid)
		auctionTestReply(t, s, offer.client, bid, 4039)
	}
	s = auctionTestServer(t, dir)
	firstRole, secondRole := s.roles.Get("南峰"), s.roles.Get("北峰")
	if roleInt(firstRole, "ShinyStone") != 1000 || roleInt(secondRole, "ShinyStone") != 545 || len(goodsList(secondRole)) != 0 {
		t.Fatalf("refund or escrow was not persisted before settlement: %v / %v", firstRole, secondRole)
	}
	second, two = auctionTestRole(t, s, "北峰", nil)
	s.handleGame(second, enter)
	auctionTestReply(t, s, two, enter, 4448)
	deadline := time.UnixMilli(roleInt(s.previewAuctionRoom(4), "EndsAt"))
	if err := s.settlePreviewAuctions(deadline); err != nil {
		t.Fatal(err)
	}
	auctionTestEvent(t, s, two, 2)
	auctionTestEvent(t, s, two, 3)
	if err := s.settlePreviewAuctions(deadline.Add(time.Second)); err != nil {
		t.Fatal(err)
	}
	s.handleGame(second, enter)
	entered := auctionTestReply(t, s, two, enter, 4448)
	if getBool(entered, "IsMayAddPrice") || getInt(entered, "ShinyStone") != 545 {
		t.Fatalf("completed room did not reopen in read-only state: %v", entered)
	}
	s.handleGame(second, list)
	listed := auctionTestReply(t, s, two, list, 4449)
	lot := listed.Get(field(listed, "GoodsList")).List().Get(0).Message()
	if getInt(lot, "FinalPrice") != 455 || getInt(lot, "Status") != 1 {
		t.Fatalf("completed listing state is wrong: %v", lot)
	}
	s.handleGame(second, current)
	final := auctionTestReply(t, s, two, current, 4457)
	if getInt(final, "CurrentPrice") != 455 || getString(final, "CurrentPerson") != "北峰" {
		t.Fatalf("settled lot lost its winning bid: %v", final)
	}
	auctionTestEvent(t, s, two, 3)
	lateBid := auctionTestMessage(t, s, 1006020, Record{"TreasureId": 4, "Price": 500})
	s.handleGame(second, lateBid)
	auctionTestReply(t, s, two, lateBid, 4453)
	reopened := auctionTestServer(t, dir)
	winner := reopened.roles.Get("北峰")
	goods := goodsList(winner)
	if len(goods) != 1 || roleInt(goods[0].(Record), "Count") != 1 || roleInt(winner, "ShinyStone") != 545 {
		t.Fatalf("settlement must deliver exactly once and consume the escrow: %v", winner)
	}
	assertAuctionHistory(t, reopened,
		auctionHistoryExpectation{kind: "auction.bid", actor: "南峰", entityType: "auction.lot", entityID: "4", amount: 385, stage: 4, debited: 385},
		auctionHistoryExpectation{kind: "auction.bid", actor: "南峰", entityType: "auction.lot", entityID: "4", amount: 420, stage: 4, debited: 35},
		auctionHistoryExpectation{kind: "auction.bid", actor: "北峰", entityType: "auction.lot", entityID: "4", amount: 455, stage: 4, debited: 455},
		auctionHistoryExpectation{kind: "auction.outbid", actor: "南峰", subject: "北峰", entityType: "auction.lot", entityID: "4", amount: 420, stage: 4, newBid: 455},
		auctionHistoryExpectation{kind: "auction.won", actor: "北峰", entityType: "auction.lot", entityID: "4", amount: 455, stage: 4},
	)
}

func TestPreviewAuctionRefundsIfBagFillsBeforeSettlement(t *testing.T) {
	dir := t.TempDir()
	s := auctionTestServer(t, dir)
	peer, client := auctionTestRole(t, s, "纳戒已满", nil)
	enter := auctionTestMessage(t, s, 1006017, Record{"Stage": 4})
	s.handleGame(peer, enter)
	auctionTestReply(t, s, client, enter, 4448)
	bid := auctionTestMessage(t, s, 1006020, Record{"TreasureId": 4, "Price": 385})
	s.handleGame(peer, bid)
	auctionTestReply(t, s, client, bid, 4456)
	auctionTestEvent(t, s, client, 1)
	auctionTestEvent(t, s, client, 3)
	if err := s.roles.Update(peer.currentRoleID(), func(role Record) error {
		bag := make([]any, 100)
		for i := range bag {
			bag[i] = Record{"Name": "灵草", "Type": 12}
		}
		role["Goods"] = bag
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	deadline := time.UnixMilli(roleInt(s.previewAuctionRoom(4), "EndsAt"))
	var both sync.WaitGroup
	errors := make(chan error, 2)
	for range 2 {
		both.Go(func() { errors <- s.settlePreviewAuctions(deadline) })
	}
	both.Wait()
	close(errors)
	for err := range errors {
		if err != nil {
			t.Fatal(err)
		}
	}
	auctionTestEvent(t, s, client, 2)
	auctionTestEvent(t, s, client, 3)
	list := auctionTestMessage(t, s, 1006018, nil)
	s.handleGame(peer, list)
	listed := auctionTestReply(t, s, client, list, 4449)
	lot := listed.Get(field(listed, "GoodsList")).List().Get(0).Message()
	if getInt(lot, "Status") != 2 {
		t.Fatalf("undelivered lot did not become 流拍: %v", lot)
	}
	reopened := auctionTestServer(t, dir)
	role := reopened.roles.Get(peer.currentRoleID())
	if roleInt(role, "ShinyStone") != 1000 || len(goodsList(role)) != 100 {
		t.Fatalf("full bag should receive exactly one refund: %v", role)
	}
	assertAuctionHistory(t, reopened,
		auctionHistoryExpectation{kind: "auction.bid", actor: "纳戒已满", entityType: "auction.lot", entityID: "4", amount: 385, stage: 4, debited: 385},
		auctionHistoryExpectation{kind: "auction.refunded", actor: "纳戒已满", entityType: "auction.lot", entityID: "4", amount: 385, stage: 4, reason: "bag_full"},
	)
}

type auctionHistoryExpectation struct {
	kind, actor, subject, entityType, entityID, reason string
	amount, stage, debited, newBid                     int64
}

func assertAuctionHistory(t *testing.T, s *Server, want ...auctionHistoryExpectation) {
	t.Helper()
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != len(want) {
		t.Fatalf("expected %d durable auction events, got %d (%v)", len(want), len(events), err)
	}
	for index, event := range events {
		expected := want[index]
		if event.Kind != expected.kind || event.Actor.RoleID != expected.actor ||
			event.Actor.RoleName != expected.actor || event.Actor.AccountID != "account-"+expected.actor ||
			event.EntityType != expected.entityType || event.EntityID != expected.entityID ||
			roleInt(event.Details, "amount") != expected.amount || roleInt(event.Details, "stage") != expected.stage ||
			roleInt(event.Details, "debited") != expected.debited || roleInt(event.Details, "newBidAmount") != expected.newBid ||
			roleString(event.Details, "reason") != expected.reason || roleString(event.Details, "currency") != "ShinyStone" ||
			roleString(event.Details, "templateId") != previewTreasureTemplate {
			t.Fatalf("event %d differs from %+v: %+v", index, expected, event)
		}
		if expected.subject == "" && event.Subject != nil || expected.subject != "" &&
			(event.Subject == nil || event.Subject.RoleID != expected.subject || event.Subject.AccountID != "account-"+expected.subject) {
			t.Fatalf("event %d recorded the wrong counterpart: %+v", index, event)
		}
	}
}
