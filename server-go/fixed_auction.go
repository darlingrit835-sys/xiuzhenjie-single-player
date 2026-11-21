package main

import (
	"cmp"
	"errors"
	"math"
	"slices"
	"strings"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const fixedAuctionPrefix = "qzg-"

// Fixed-price goods are held in the same Store as both players. Inventory,
// balances, escrow and history are committed in one file replacement.
func fixedAuctionBook(state Record) Record {
	book, _ := state["FixedListings"].(Record)
	return book
}

func fixedAuctionShop(kind int64) int64 {
	// Tabs the original QiZhenGePaiMaiWnd actually enables.
	switch kind {
	case 5, 21:
		return 81
	case 2, 19:
		return 82
	case 4, 15, 16, 26, 29:
		return 83
	case 3, 20:
		return 84
	case 1:
		return 86
	case 6:
		return 87
	case 8, 9, 10, 11, 12, 13, 30, 31, 32:
		return 88
	default:
		return 0
	}
}

func (s *Server) fixedAuctionSummary(id string, sale Record, now time.Time) previewListing {
	goods, _ := sale["Goods"].(Record)
	level := max(1, roleInt(goods, "Level"))
	stage := roleInt(goods, "Stage")
	if stage <= 0 {
		stage = (level-1)/10 + 1
	}
	hours := max(int64(0), roleInt(sale, "EndsAt")-now.UnixMilli()+time.Hour.Milliseconds()-1) / time.Hour.Milliseconds()
	detailID := id
	if isSmithingEquipment(goods) {
		detailID = roleString(goods, "TemplateId")
	}
	kind := roleInt(goods, "Type")
	var subtype int64
	switch kind {
	case 1:
		subtype = roleInt(goods, "ActionScope")
		if method := s.methods().book(goods); method != nil {
			subtype = int64(method.kind)
		}
	case 2, 19:
		subtype = roleInt(goods, "EffectType")
	case 5, 6, 21:
		subtype = roleInt(goods, "SkillType")
	case 3, 20:
		subtype = roleInt(goods, "MatrixType")
	case 4:
		subtype = roleInt(goods, "EquipmentType")
	}
	return previewListing{id: id, template: detailID, name: roleString(goods, "Name"), shop: roleInt(sale, "Shop"),
		kind: kind, subtype: subtype, element: roleInt(goods, "FiveElement"), rank: max(1, roleInt(goods, "Rank")),
		stage: stage, level: level, count: transferStackCount(goods), price: roleInt(sale, "Total"), hours: hours}
}

func (s *Server) fixedAuctionListings(seller string, shop int64, now time.Time) []previewListing {
	store := s.roles
	store.mu.RLock()
	state := store.records[previewAuctionStoreID]
	book := fixedAuctionBook(state)
	var list []previewListing
	if seller == "" && roleString(state, "FixedBuyer") == "" && (shop <= 0 || shop == previewFixedAuctionListing.shop) {
		list = append(list, previewFixedAuctionListing)
	}
	for id, entry := range book {
		sale, _ := entry.(Record)
		if sale == nil || (shop > 0 && roleInt(sale, "Shop") != shop) {
			continue
		}
		if seller != "" {
			if roleString(sale, "Seller") != seller {
				continue
			}
		} else if roleInt(sale, "EndsAt") <= now.UnixMilli() {
			continue
		}
		list = append(list, s.fixedAuctionSummary(id, sale, now))
	}
	store.mu.RUnlock()
	slices.SortFunc(list, func(a, b previewListing) int { return cmp.Compare(a.id, b.id) })
	return list
}

func (s *Server) myFixedAuctions(peer *Peer, req, out protoreflect.Message) (func(), error) {
	list := s.fixedAuctionListings(peer.currentRoleID(), getInt(req, "Type"), time.Now())
	page, size := previewAuctionPage(req, out, len(list))
	for _, r := range list[(page-1)*size : min(page*size, len(list))] {
		putPB(addMessage(out, "Object"), Record{"Id": r.id, "Name": r.name, "GoodsType": r.kind, "FiveElement": r.element,
			"FixedPrice": r.price, "RemainHours": r.hours, "Stage": r.stage, "Rank": r.rank, "Level": r.level, "Count": r.count})
	}
	statusPB(out, true, 4251)
	return nil, nil
}

// Only a public, unexpired listing ID can read escrow details. Public replies
// never expose a player's private inventory ID or accept listing IDs for item use.
func (s *Server) inventoryDetailItem(roleID, id string, equipped bool) Record {
	if owned := s.roles.inventoryItem(roleID, id, equipped); owned != nil {
		return owned
	}
	if !s.previewWrites || roleID == "" || !strings.HasPrefix(id, fixedAuctionPrefix) {
		return nil
	}
	store := s.roles
	store.mu.RLock()
	defer store.mu.RUnlock()
	sale, _ := fixedAuctionBook(store.records[previewAuctionStoreID])[id].(Record)
	if sale == nil || (roleInt(sale, "EndsAt") <= time.Now().UnixMilli() && roleString(sale, "Seller") != roleID) {
		return nil
	}
	goods, _ := sale["Goods"].(Record)
	return cloneRecord(goods)
}

// The legacy client supplies a per-connection Msid and no durable order key.
type fixedAuctionRequest struct {
	cmd, msid          int
	role, id           string
	count, price, shop int64
}
type fixedAuctionReceipt struct {
	request fixedAuctionRequest
	expires time.Time
}
type fixedAuctionReceipts struct {
	recent [32]fixedAuctionReceipt
	next   int
}

func (k fixedAuctionRequest) replay(p *Peer) (bool, error) {
	if k.msid <= 0 || p.auctionReceipts == nil {
		return false, nil
	}
	now := time.Now()
	for _, r := range p.auctionReceipts.recent {
		if r.request.cmd == k.cmd && r.request.msid == k.msid && r.request.role == k.role && now.Before(r.expires) {
			if r.request != k {
				return false, previewReject(4008)
			}
			return true, nil
		}
	}
	return false, nil
}

func (k fixedAuctionRequest) remember(p *Peer) {
	if k.msid <= 0 {
		return
	}
	if p.auctionReceipts == nil {
		p.auctionReceipts = new(fixedAuctionReceipts)
	}
	r := p.auctionReceipts
	r.recent[r.next] = fixedAuctionReceipt{k, time.Now().Add(5 * time.Minute)}
	r.next = (r.next + 1) % len(r.recent)
}

func fixedAuctionStatus(out protoreflect.Message, cmd int, role Record) {
	switch cmd {
	case 1006008:
		statusPB(out, true, 4168)
		putPB(out, Record{"ShinyStone": roleInt(role, "ShinyStone"), "NowRingCapacity": len(goodsList(role))})
	case 1006009:
		statusPB(out, true, 4253)
	case 1006010:
		statusPB(out, true, 4046)
		set(out, "ShinyStone", roleInt(role, "ShinyStone"))
	}
}

func (s *Server) replayFixedAuction(peer *Peer, k fixedAuctionRequest, out protoreflect.Message) (bool, error) {
	previous, err := k.replay(peer)
	if err != nil || !previous {
		return false, err
	}
	s.roles.mu.RLock()
	defer s.roles.mu.RUnlock()
	fixedAuctionStatus(out, k.cmd, s.roles.records[k.role])
	return true, nil
}

func (s *Server) consignFixedAuction(peer *Peer, req, out protoreflect.Message) (func(), error) {
	k := fixedAuctionRequest{cmd: 1006008, msid: peer.gameRequestID, role: peer.currentRoleID(), id: getString(req, "GoodsId"),
		count: getInt(req, "Count"), price: getInt(req, "FixedPrice"), shop: getInt(req, "ShopType")}
	if hit, err := s.replayFixedAuction(peer, k, out); err != nil || hit {
		return nil, err
	}
	if k.id == "" || k.count < 1 || k.count > math.MaxInt32 {
		return nil, previewReject(4169)
	}
	if k.price < 100 {
		return nil, previewReject(4328)
	}
	if k.price > 1_000_000_000 {
		return nil, previewReject(4334)
	}
	total := k.price * k.count // Bounded by int32 quantity and 10^9 unit price.
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(k.role)
		if role == nil {
			return previewReject(4007)
		}
		goods := locateGoods(role, k.id)
		if goods == nil || transferStackCount(goods) < k.count {
			return previewReject(4169)
		}
		if roleBool(goods, "IsBinding") || roleInt(goods, "IsBinding") != 0 {
			return previewReject(4247)
		}
		if roleBool(goods, "IsLock") || roleInt(goods, "IsLock") != 0 {
			return previewReject(4509)
		}
		if roleBool(goods, "IsUsing") || roleInt(goods, "IsUsing") != 0 {
			return previewReject(4546)
		}
		if k.shop <= 0 || fixedAuctionShop(roleInt(goods, "Type")) != k.shop {
			return previewReject(4008)
		}
		rate := int64(10)
		for _, id := range friendTitleIDs(role) {
			rate = min(rate, max(int64(5), 10-s.game.auctionDiscounts[id]))
		}
		fee := max(int64(1), total/100*rate+total%100*rate/100)
		balance := roleInt(role, "ShinyStone")
		if balance < fee {
			return previewReject(4398)
		}
		role = tx.Get(k.role)
		original := locateGoods(role, k.id)
		escrow := cloneRecord(original)
		if left := transferStackCount(original) - k.count; left > 0 {
			original["Count"] = left
			escrow["GoodsId"] = "g_" + newGameID(8)
		} else {
			role["Goods"] = slices.DeleteFunc(goodsList(role), func(x any) bool { r, _ := x.(Record); return roleString(r, "GoodsId") == k.id })
		}
		escrow["Count"] = k.count
		role["ShinyStone"] = balance - fee
		state := tx.GetOrCreate(previewAuctionStoreID)
		book := fixedAuctionBook(state)
		if book == nil {
			book = Record{}
			state["FixedListings"] = book
		}
		id := fixedAuctionPrefix + newGameID(16)
		book[id] = Record{"Seller": k.role, "Shop": k.shop, "Goods": escrow, "Total": total, "EndsAt": time.Now().Add(24 * time.Hour).UnixMilli()}
		fixedAuctionStatus(out, k.cmd, role)
		return tx.RecordHistory(fixedAuctionHistory("auction.consigned", k.role, role, "", nil, id, escrow, total, fee))
	})
	if err == nil {
		k.remember(peer)
	}
	return nil, err
}

func fixedAuctionHistory(kind, actorID string, actor Record, otherID string, other Record, id string, goods Record, total, fee int64) historyEvent {
	event := newHistoryEvent(kind, actor, other, "auction.listing", id, Record{"amount": total, "currency": "ShinyStone",
		"quantity": transferStackCount(goods), "templateId": roleString(goods, "TemplateId"), "goodsId": roleString(goods, "GoodsId"), "listingFee": fee})
	event.Actor.RoleID = actorID
	if otherID != "" {
		subject := historyRole(other)
		subject.RoleID = otherID
		event.Subject = &subject
	}
	return event
}

func (s *Server) cancelFixedAuction(peer *Peer, req, out protoreflect.Message) (func(), error) {
	return s.settleFixedAuction(peer, req, out, true)
}

func (s *Server) settleFixedAuction(peer *Peer, req, out protoreflect.Message, cancel bool) (func(), error) {
	cmd := 1006010
	if cancel {
		cmd = 1006009
	}
	k := fixedAuctionRequest{cmd: cmd, msid: peer.gameRequestID, role: peer.currentRoleID(), id: getString(req, "AuctionId")}
	if hit, err := s.replayFixedAuction(peer, k, out); err != nil || hit {
		return nil, err
	}
	if !strings.HasPrefix(k.id, fixedAuctionPrefix) {
		return nil, previewReject(4038)
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		sale, _ := fixedAuctionBook(tx.View(previewAuctionStoreID))[k.id].(Record)
		if sale == nil || !cancel && roleInt(sale, "EndsAt") <= time.Now().UnixMilli() {
			return previewReject(4171)
		}
		sellerID := roleString(sale, "Seller")
		if cancel != (sellerID == k.role) {
			return previewReject(4008)
		}
		seller, recipient := tx.View(sellerID), tx.View(k.role)
		if seller == nil || recipient == nil {
			return previewReject(4007)
		}
		goods, _ := sale["Goods"].(Record)
		total := roleInt(sale, "Total")
		count := transferStackCount(goods)
		if goods == nil || count < 1 || count > math.MaxInt32 || total <= 0 {
			return previewReject(4169)
		}
		if !cancel {
			if roleInt(recipient, "ShinyStone") < total {
				return previewReject(4173)
			}
			if balance := roleInt(seller, "ShinyStone"); balance < 0 || balance > math.MaxInt64-total {
				return previewReject(4008)
			}
		}
		recipient = tx.Get(k.role)
		_, err := receiveInventoryGoods(recipient, goods, count)
		if errors.Is(err, previewReject(4527)) {
			return previewReject(4158)
		}
		if err != nil {
			return err
		}
		delete(fixedAuctionBook(tx.Get(previewAuctionStoreID)), k.id)
		fixedAuctionStatus(out, k.cmd, recipient)
		if cancel {
			return tx.RecordHistory(fixedAuctionHistory("auction.cancelled", sellerID, recipient, "", nil, k.id, goods, total, 0))
		}
		seller = tx.Get(sellerID)
		seller["ShinyStone"] = roleInt(seller, "ShinyStone") + total
		recipient["ShinyStone"] = roleInt(recipient, "ShinyStone") - total
		addFixedPurchase(recipient, goods, roleString(seller, "Name"), total, time.Now())
		set(out, "ShinyStone", roleInt(recipient, "ShinyStone"))
		return tx.RecordHistory(fixedAuctionHistory("auction.purchase", k.role, recipient, sellerID, seller, k.id, goods, total, 0),
			fixedAuctionHistory("auction.sold", sellerID, seller, k.role, recipient, k.id, goods, total, 0))
	})
	if err == nil {
		k.remember(peer)
	}
	return nil, err
}
