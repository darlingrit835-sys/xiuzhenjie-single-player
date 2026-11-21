package main

import (
	"errors"
	"fmt"
	"math"
	"strconv"
	"sync"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	previewAuctionMinStage    = 1
	previewAuctionMaxStage    = 9
	previewAuctionBidStep     = previewTreasurePrice / 10
	previewAuctionBidDuration = 45 * time.Second
)

var previewFixedAuctionListing = previewListing{
	id: "preview-auction-elixir", template: previewTreasureTemplate, name: previewTreasureName,
	shop: 82, kind: 19, rank: previewTreasureRank, stage: 1, level: 1, price: 500, count: 1,
}

// Membership is tied to the game connection. The original room omits the stage
// in the current-lot and lot-list requests and sends its leave on the chat socket.
type previewAuctionRuntime struct {
	mu      sync.Mutex
	stages  map[*Peer]int
	members [previewAuctionMaxStage + 1]map[*Peer]struct{}
	publish [previewAuctionMaxStage + 1]sync.Mutex
}

func (a *previewAuctionRuntime) enter(peer *Peer, stage int) {
	a.mu.Lock()
	defer a.mu.Unlock()
	delete(a.members[a.stages[peer]], peer)
	if a.stages == nil {
		a.stages = make(map[*Peer]int)
	}
	if a.members[stage] == nil {
		a.members[stage] = make(map[*Peer]struct{})
	}
	a.stages[peer] = stage
	a.members[stage][peer] = struct{}{}
}

func (a *previewAuctionRuntime) leave(peer *Peer) {
	a.mu.Lock()
	defer a.mu.Unlock()
	delete(a.members[a.stages[peer]], peer)
	delete(a.stages, peer)
}

func (a *previewAuctionRuntime) stage(peer *Peer) int {
	a.mu.Lock()
	defer a.mu.Unlock()
	return a.stages[peer]
}

func (a *previewAuctionRuntime) viewers(stage int) []*Peer {
	a.mu.Lock()
	defer a.mu.Unlock()
	peers := make([]*Peer, 0, len(a.members[stage]))
	for peer := range a.members[stage] {
		peers = append(peers, peer)
	}
	return peers
}

func previewAuctionRoom(state Record, stage int) Record {
	rooms, _ := state["Rooms"].(Record)
	room, _ := rooms[strconv.Itoa(stage)].(Record)
	return room
}

func editPreviewAuctionRoom(tx *StoreTx, stage int) Record {
	state := tx.GetOrCreate(previewAuctionStoreID)
	rooms, _ := state["Rooms"].(Record)
	if rooms == nil {
		rooms = make(Record)
		state["Rooms"] = rooms
	}
	key := strconv.Itoa(stage)
	room, _ := rooms[key].(Record)
	if room == nil {
		room = make(Record)
		rooms[key] = room
	}
	return room
}

func (s *Server) previewAuctionRoom(stage int) Record {
	return previewAuctionRoom(s.roles.project(previewAuctionStoreID, []string{"Rooms"}), stage)
}

func previewAuctionPrice(room Record) int64 {
	return max(previewTreasurePrice, roleInt(room, "Price"))
}

func previewAuctionOpen(room Record, now time.Time) bool {
	deadline := roleInt(room, "EndsAt")
	return !roleBool(room, "Settled") && (deadline == 0 || now.UnixMilli() < deadline)
}

func previewAuctionStage(req protoreflect.Message) (int, error) {
	stage := int(getInt(req, "Stage"))
	if stage < previewAuctionMinStage || stage > previewAuctionMaxStage {
		return 0, previewReject(4713)
	}
	return stage, nil
}

func previewAuctionEnterGate(role Record, stage int) error {
	if role == nil {
		return previewReject(4007)
	}
	level := max(1, roleInt(role, "Level"))
	realm := (level-1)/10 + 1
	if int64(stage) != realm && int64(stage) != realm+1 {
		return previewReject(4713)
	}
	if roleInt(role, "ShinyJade") < int64(stage*5) {
		return previewReject(4298)
	}
	return nil
}

func (s *Server) listPreviewAuction(_ *Peer, req, out protoreflect.Message) (func(), error) {
	listings := s.fixedAuctionListings("", getInt(req, "ShopType"), time.Now())
	statusPB(out, true, 4160)
	fillPreviewAuctionListings(req, out, listings)
	return nil, nil
}

func (s *Server) buyPreviewAuction(peer *Peer, req, out protoreflect.Message) (func(), error) {
	if getString(req, "AuctionId") != previewFixedAuctionListing.id {
		return s.settleFixedAuction(peer, req, out, false)
	}
	roleID := peer.currentRoleID()
	price := previewFixedAuctionListing.price
	var balance int64
	err := s.roles.Transact(func(tx *StoreTx) error {
		buyer := tx.View(roleID)
		if buyer == nil {
			return previewReject(4007)
		}
		if roleString(tx.View(previewAuctionStoreID), "FixedBuyer") != "" {
			return previewReject(4171)
		}
		balance = roleInt(buyer, "ShinyStone")
		if balance < price {
			return previewReject(4173)
		}
		role := tx.Get(roleID)
		if !givePreviewAuctionElixir(role) {
			return previewReject(4158)
		}
		balance -= price
		role["ShinyStone"] = balance
		tx.GetOrCreate(previewAuctionStoreID)["FixedBuyer"] = roleID
		lot := previewFixedAuctionListing
		addFixedPurchase(role, Record{"Name": lot.name, "Type": lot.kind, "Rank": lot.rank, "Count": lot.count}, "奇珍阁", price, time.Now())
		return tx.RecordHistory(newHistoryEvent("auction.purchase", buyer, nil, "auction.listing", previewFixedAuctionListing.id, Record{
			"amount": price, "currency": "ShinyStone", "templateId": previewFixedAuctionListing.template,
		}))
	})
	if err != nil {
		return nil, err
	}
	statusPB(out, true, 4046)
	set(out, "ShinyStone", balance)
	return nil, nil
}

func (s *Server) listPreviewAuctionRoom(_ *Peer, req, out protoreflect.Message) (func(), error) {
	_, err := previewAuctionStage(req)
	if err != nil {
		return nil, err
	}
	previewTreasureRoom(nil, req, out)
	statusPB(out, true, 4449)
	return nil, nil
}

func (s *Server) enterPreviewAuction(peer *Peer, req, out protoreflect.Message) (func(), error) {
	stage, err := previewAuctionStage(req)
	if err != nil {
		return nil, err
	}
	role := s.roles.project(peer.currentRoleID(), []string{"ShinyStone", "ShinyJade", "Level"})
	if err := previewAuctionEnterGate(role, stage); err != nil {
		return nil, err
	}
	balance := roleInt(role, "ShinyStone")
	if balance < 0 || balance > math.MaxInt32 {
		return nil, previewReject(4008)
	}
	room := s.previewAuctionRoom(stage)
	statusPB(out, true, 4448)
	putPB(out, Record{"TreasureId": stage, "IsMayAddPrice": previewAuctionOpen(room, time.Now()), "ShinyStone": balance})
	s.previewAuction.enter(peer, stage)
	return nil, nil
}

func (s *Server) listPreviewAuctionLots(peer *Peer, _, out protoreflect.Message) (func(), error) {
	stage := s.previewAuction.stage(peer)
	if stage == 0 {
		return nil, previewReject(4450)
	}
	room := s.previewAuctionRoom(stage)
	statusPB(out, true, 4449)
	set(out, "Stage", stage)
	lot := addMessage(out, "GoodsList")
	putPB(lot, Record{"GoodsName": previewTreasureName, "Rank": previewTreasureRank, "FinalPrice": previewAuctionPrice(room)})
	if roleBool(room, "Settled") {
		status := 2 // 流拍
		if roleBool(room, "Delivered") {
			status = 1 // 成交
		}
		set(lot, "Status", status)
	}
	return nil, nil
}

func (s *Server) currentPreviewAuctionLot(peer *Peer, _, out protoreflect.Message) (func(), error) {
	stage := s.previewAuction.stage(peer)
	if stage == 0 {
		return nil, previewReject(4450)
	}
	room := s.previewAuctionRoom(stage)
	previewCurrentTreasure(nil, nil, out)
	statusPB(out, true, 4457)
	putPB(out, Record{
		"Id": stage, "CurrentPrice": previewAuctionPrice(room), "CurrentPerson": roleString(room, "BidderID"),
		"PersonName": roleString(room, "BidderName"), "PersonImage": roleInt(room, "BidderPortrait"),
		"PersonSex": roleBool(room, "BidderSex"),
	})
	return func() { s.notifyPreviewAuctionViewer(peer, stage, room) }, nil
}

func (s *Server) bidPreviewAuction(peer *Peer, req, out protoreflect.Message) (func(), error) {
	stage := s.previewAuction.stage(peer)
	if stage == 0 || int64(stage) != getInt(req, "TreasureId") {
		return nil, previewReject(4450)
	}
	roleID := peer.currentRoleID()
	price := getInt(req, "Price")
	var balance, refund int64
	var refundedRole string
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(roleID)
		if err := previewAuctionEnterGate(role, stage); err != nil {
			return err
		}
		now := time.Now()
		room := previewAuctionRoom(tx.View(previewAuctionStoreID), stage)
		if !previewAuctionOpen(room, now) {
			return previewReject(4453)
		}
		previousPrice := previewAuctionPrice(room)
		if price > math.MaxInt32 || price <= previousPrice || price-previousPrice < previewAuctionBidStep {
			return previewReject(4039)
		}
		leader := roleString(room, "BidderID")
		debit := price
		if leader == roleID {
			debit -= previousPrice
		}
		balance = roleInt(role, "ShinyStone")
		if balance > math.MaxInt32 || balance < debit {
			return previewReject(4452)
		}
		if !canReceivePreviewAuctionElixir(role) {
			return previewReject(4158)
		}
		var previousBidder Record
		if leader != "" && leader != roleID {
			previousBidder = tx.View(leader)
			if err := refundPreviewAuctionBid(tx.Get(leader), previousPrice); err != nil {
				return err
			}
			refundedRole, refund = leader, previousPrice
		}
		balance -= debit
		tx.Get(roleID)["ShinyStone"] = balance
		room = editPreviewAuctionRoom(tx, stage)
		room["Price"] = price
		room["BidderID"] = roleID
		room["BidderName"] = roleString(role, "Name")
		room["BidderPortrait"] = roleInt(role, "HeadPortraitId")
		room["BidderSex"] = roleInt(role, "Sex") == 1 || roleBool(role, "Sex")
		if roleInt(room, "EndsAt") == 0 {
			room["EndsAt"] = now.Add(previewAuctionBidDuration).UnixMilli()
		}
		lotID := strconv.Itoa(stage)
		bidEvent := newHistoryEvent("auction.bid", role, nil, "auction.lot", lotID, Record{
			"stage": stage, "amount": price, "debited": debit, "currency": "ShinyStone", "templateId": previewTreasureTemplate,
		})
		if previousBidder != nil {
			return tx.RecordHistory(bidEvent, newHistoryEvent("auction.outbid", previousBidder, role, "auction.lot", lotID, Record{
				"stage": stage, "amount": refund, "newBidAmount": price, "currency": "ShinyStone", "templateId": previewTreasureTemplate,
			}))
		}
		return tx.RecordHistory(bidEvent)
	})
	if err != nil {
		return nil, err
	}
	statusPB(out, true, 4456)
	set(out, "ShinyStone", balance)
	return func() { s.notifyPreviewAuctionBid(stage, refundedRole, refund) }, nil
}

func refundPreviewAuctionBid(role Record, amount int64) error {
	if role == nil {
		return errors.New("auction bidder save is missing")
	}
	balance := roleInt(role, "ShinyStone")
	if amount <= 0 || balance < 0 || balance > math.MaxInt64-amount {
		return errors.New("invalid auction refund balance")
	}
	role["ShinyStone"] = balance + amount
	return nil
}

func canReceivePreviewAuctionElixir(role Record) bool {
	for _, item := range goodsList(role) {
		goods, _ := item.(Record)
		if roleInt(goods, "Type") == 19 && (roleString(goods, "TemplateId") == previewTreasureTemplate ||
			roleString(goods, "Name") == "筑基丹" && roleInt(goods, "Rank") == previewTreasureRank && roleInt(goods, "Level") == 1) {
			return roleInt(goods, "Count") < math.MaxInt32
		}
	}
	return len(goodsList(role)) < ringCapacity(role)
}

func givePreviewAuctionElixir(role Record) bool {
	if !canReceivePreviewAuctionElixir(role) {
		return false
	}
	goods := addGoods(role, "筑基丹", 19, previewTreasureRank, 1, 0, previewTreasureTemplate, 1, 0.3, 1)
	if goods == nil {
		return false
	}
	goods["FiveElement"] = 0
	return true
}

func previewAuctionSettlementDue(room Record, now time.Time) bool {
	deadline := roleInt(room, "EndsAt")
	return deadline > 0 && deadline <= now.UnixMilli() && !roleBool(room, "Settled")
}

func (s *Server) settlePreviewAuctions(now time.Time) error {
	state := s.roles.project(previewAuctionStoreID, []string{"Rooms"})
	var pendingErrors []error
	for stage := previewAuctionMinStage; stage <= previewAuctionMaxStage; stage++ {
		if !previewAuctionSettlementDue(previewAuctionRoom(state, stage), now) {
			continue
		}
		settled := false
		err := s.roles.Transact(func(tx *StoreTx) error {
			room := previewAuctionRoom(tx.View(previewAuctionStoreID), stage)
			if !previewAuctionSettlementDue(room, now) {
				return nil
			}
			winnerID := roleString(room, "BidderID")
			winnerBeforeSettlement := tx.View(winnerID)
			if winnerBeforeSettlement == nil {
				return errors.New("auction winner save is missing")
			}
			winner := tx.Get(winnerID)
			amount := previewAuctionPrice(room)
			delivered := givePreviewAuctionElixir(winner)
			kind := "auction.won"
			details := Record{"stage": stage, "amount": amount, "currency": "ShinyStone", "templateId": previewTreasureTemplate}
			if !delivered {
				if err := refundPreviewAuctionBid(winner, amount); err != nil {
					return err
				}
				kind = "auction.refunded"
				details["reason"] = "bag_full"
			}
			room = editPreviewAuctionRoom(tx, stage)
			room["Settled"], room["Delivered"] = true, delivered
			settled = true
			return tx.RecordHistory(newHistoryEvent(kind, winnerBeforeSettlement, nil, "auction.lot", strconv.Itoa(stage), details))
		})
		if err != nil {
			pendingErrors = append(pendingErrors, fmt.Errorf("auction stage %d: %w", stage, err))
		} else if settled {
			s.notifyPreviewAuctionSettlement(stage)
		}
	}
	return errors.Join(pendingErrors...)
}

func (s *Server) auctionRoomEvent(peer *Peer, stage, eventType int, text string, bidder Record) {
	out := s.gameResponse("ReceiveMessageResp_4001002", true, 2000)
	putPB(out, Record{"ChatId": strconv.Itoa(stage), "Type": eventType, "Msg": text})
	if bidder != nil {
		putPB(out, Record{
			"RoleName": roleString(bidder, "BidderName"), "HeadPortraitId": strconv.FormatInt(roleInt(bidder, "BidderPortrait"), 10),
		})
		if roleBool(bidder, "BidderSex") {
			set(out, "Sex", 1)
		}
	}
	_ = peer.send(4001002, 0, out)
}

func (s *Server) notifyPreviewAuctionViewer(peer *Peer, stage int, previous Record) {
	publish := &s.previewAuction.publish[stage]
	publish.Lock()
	defer publish.Unlock()
	room := s.previewAuctionRoom(stage)
	now := time.Now()
	if previewAuctionOpen(room, now) {
		if previewAuctionPrice(room) > previewAuctionPrice(previous) {
			s.auctionRoomEvent(peer, stage, 1, strconv.FormatInt(previewAuctionPrice(room), 10), room)
		}
		// Enables the original quick-bid button after its initial lot response.
		s.auctionRoomEvent(peer, stage, 4, "", nil)
		s.auctionRoomEvent(peer, stage, 3, previewAuctionCountdown(room, now), nil)
	} else if roleBool(room, "Settled") {
		if !roleBool(previous, "Settled") {
			s.auctionRoomEvent(peer, stage, 2, "", nil)
		}
		s.auctionRoomEvent(peer, stage, 3, previewAuctionResult(room, peer.currentRoleID()), nil)
	}
}

func (s *Server) notifyPreviewAuctionBid(stage int, refundedRole string, refund int64) {
	// Concurrent responses may be written in either order. Read the latest
	// committed state while serializing room pushes so prices never go backward.
	publish := &s.previewAuction.publish[stage]
	publish.Lock()
	defer publish.Unlock()
	room := s.previewAuctionRoom(stage)
	now := time.Now()
	open := previewAuctionOpen(room, now)
	text := previewAuctionCountdown(room, now)
	for _, viewer := range s.previewAuction.viewers(stage) {
		if open {
			s.auctionRoomEvent(viewer, stage, 1, strconv.FormatInt(previewAuctionPrice(room), 10), room)
		}
		if refundedRole != "" && viewer.currentRoleID() == refundedRole {
			s.auctionRoomEvent(viewer, stage, 3, fmt.Sprintf("有道友出价更高，已退还%d灵石。重新入场即可刷新余额。%s", refund, text), nil)
		} else if open {
			s.auctionRoomEvent(viewer, stage, 3, text, nil)
		}
	}
}

func previewAuctionCountdown(room Record, now time.Time) string {
	deadline := roleInt(room, "EndsAt")
	if deadline == 0 {
		return fmt.Sprintf("竞价已开始，首个有效出价后%d秒结算。", previewAuctionBidDuration/time.Second)
	}
	seconds := (deadline - now.UnixMilli() + 999) / 1000
	if seconds <= 0 {
		return "拍品正在结算。"
	}
	return fmt.Sprintf("距本件拍品结算约%d秒。", seconds)
}

func (s *Server) notifyPreviewAuctionSettlement(stage int) {
	publish := &s.previewAuction.publish[stage]
	publish.Lock()
	defer publish.Unlock()
	room := s.previewAuctionRoom(stage)
	for _, viewer := range s.previewAuction.viewers(stage) {
		// The original client asks 1006021 again and disables further bidding.
		s.auctionRoomEvent(viewer, stage, 2, "", nil)
		s.auctionRoomEvent(viewer, stage, 3, previewAuctionResult(room, viewer.currentRoleID()), nil)
	}
}

func previewAuctionResult(room Record, viewerID string) string {
	if !roleBool(room, "Delivered") {
		if viewerID == roleString(room, "BidderID") {
			return "拍卖已结束。您的纳戒空间不足，竞价灵石已退还。重新入场即可刷新余额。"
		}
		return "拍卖已结束，本件拍品未交割。"
	}
	if viewerID == roleString(room, "BidderID") {
		return fmt.Sprintf("拍卖成交，已将筑基丹放入您的纳戒。成交价%d灵石。", previewAuctionPrice(room))
	}
	return fmt.Sprintf("拍卖成交，%s以%d灵石拍得筑基丹。", roleString(room, "BidderName"), previewAuctionPrice(room))
}
