package main

import (
	"crypto/sha256"
	"encoding/hex"
	"errors"
	"log"
	"maps"
	"slices"
	"strings"
	"time"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	friendOfferCreate      = 4901010
	friendOfferList        = 4901011
	friendOfferDecide      = 4901012
	friendOfferNotice      = 4901013
	friendOfferResult      = 4901014
	friendOfferAck         = 4901015
	friendOfferCapability  = 49100
	friendOfferUnavailable = 44901
	friendOffersKey        = "FriendGoodsOffers"
	friendOfferReceiptsKey = "FriendGoodsReceipts"
	friendOfferLockKey     = "FriendOfferLock"
	friendOfferUntilKey    = "FriendOfferUntil"
	maxPendingOffers       = 32
	friendOfferAge         = 7 * 24 * time.Hour
)

func markFriendOffersAvailable(response protoreflect.Message) {
	response.SetUnknown(offerNumber(response.GetUnknown(), friendOfferCapability, 1))
}

func offerNumber(wire []byte, field protowire.Number, value uint64) []byte {
	return protowire.AppendVarint(protowire.AppendTag(wire, field, protowire.VarintType), value)
}

func offerString(wire []byte, field protowire.Number, value string) []byte {
	return protowire.AppendString(protowire.AppendTag(wire, field, protowire.BytesType), value)
}

func offerToken(unknown []byte) string {
	num, typ, size := protowire.ConsumeTag(unknown)
	if num != 3 || typ != protowire.BytesType || size < 0 {
		return ""
	}
	value, used := protowire.ConsumeString(unknown[size:])
	if used < 0 || size+used != len(unknown) {
		return ""
	}
	return value
}

func validOfferToken(id string) bool {
	if len(id) != 32 || id != strings.ToLower(id) {
		return false
	}
	_, err := hex.DecodeString(id)
	return err == nil
}

func (s *Server) handleFriendOffers(peer *Peer, msg Message) {
	role := peer.currentRoleID()
	var err error
	var balance int64
	var sender, recipient string
	var resolved bool
	var accepted bool

	active, online := s.online.Load(role)
	switch {
	case !s.previewWrites || role == "" || !online || active != peer || len(msg.Data) > 16<<10:
		err = previewReject(previewDeniedCode)
	case msg.Cmd == friendOfferList:
		s.expireFriendLocks(role)
		s.replyFriendOffers(peer, msg.Msid)
		return
	case msg.Cmd == friendOfferCreate:
		var req protoreflect.Message
		if req, err = s.decode("RemoteSendGoodsReq_1014005", msg.Data); err == nil {
			recipient = getString(req, "TargetFriendId")
			err = s.createFriendOffer(role, recipient, offerToken(req.GetUnknown()), req.Get(field(req, "Goods")).List())
		}
	case msg.Cmd == friendOfferDecide:
		var req protoreflect.Message
		if req, err = s.decode("SendMessageRoleReq_4001007", msg.Data); err == nil {
			action := getString(req, "Msg")
			if action != "accept" && action != "decline" {
				err = previewReject(previewDeniedCode)
				break
			}
			accepted = action == "accept"
			sender, resolved, err = s.decideFriendOffer(role, getString(req, "RoleId"), accepted)
		}
	case msg.Cmd == friendOfferAck:
		var req protoreflect.Message
		if req, err = s.decode("SendMessageRoleReq_4001007", msg.Data); err == nil {
			balance, err = s.acknowledgeFriendOffer(role, getString(req, "RoleId"))
		}
	}

	code := 2000
	if err != nil {
		var rejected previewReject
		if errors.As(err, &rejected) {
			code = int(rejected)
		} else {
			log.Printf("传物确认 cmd=%d: %v", msg.Cmd, err)
			code = 5000
		}
	}
	if msg.Cmd == friendOfferList {
		_ = peer.sendPayload(friendOfferList, msg.Msid, offerNumber(nil, 1, 0))
		return
	}
	if msg.Cmd == friendOfferAck {
		out := s.gameResponse("RemoteSendGoodsResp_1014005", err == nil, code)
		set(out, "CurrentShinyStone", balance)
		s.sendGame(peer, msg, out)
		return
	}
	s.sendGame(peer, msg, s.gameResponse("ActionBoolResp_1001001", err == nil, code))
	if err != nil {
		return
	}
	if recipient != "" {
		s.pushFriendOffer(recipient, friendOfferNotice, nil)
	}
	if resolved {
		s.pushFriendOffer(sender, friendOfferResult, nil)
	}
}

func (s *Server) pushFriendOffer(id string, cmd int, payload []byte) {
	if client, ok := s.online.Load(id); ok {
		client.(*Peer).queueRolePush(id, cmd, payload)
	}
}

func validateOfferFriends(from, to string, tx *StoreTx) (Record, Record, error) {
	sender, receiver, err := validateFriendPair(from, to, tx)
	if err != nil {
		return nil, nil, err
	}
	if friendMap(sender, friendLinks)[to] == nil || friendMap(receiver, friendLinks)[from] == nil {
		return nil, nil, previewReject(4407)
	}
	return sender, receiver, nil
}

func (s *Server) createFriendOffer(from, to, id string, requested protoreflect.List) error {
	if !validOfferToken(id) || requested.Len() < 1 || requested.Len() > maxTransferKinds {
		return previewReject(4399)
	}
	now := time.Now()
	return s.roles.Transact(func(tx *StoreTx) error {
		sender, receiver, err := validateOfferFriends(from, to, tx)
		if err != nil {
			return err
		}
		if row, _ := friendMap(receiver, friendOffersKey)[id].(Record); row != nil {
			if roleString(row, "sender") == from {
				return nil
			}
			return previewReject(previewDeniedCode)
		}
		selected, price, err := inspectFriendGoods(sender, requested)
		if err != nil {
			return err
		}
		fee := s.friendTransferFee(sender, price)
		if roleInt(sender, "ShinyStone") < fee {
			return previewReject(4398)
		}
		offers := friendMap(tx.Get(to), friendOffersKey)
		if offers == nil {
			offers = Record{}
		}
		open := 0
		for token, value := range offers {
			row, _ := value.(Record)
			if roleInt(row, "until") <= now.Unix() {
				delete(offers, token)
			} else if roleString(row, "state") == "pending" {
				open++
			}
		}
		if open >= maxPendingOffers {
			return previewReject(4410)
		}
		items := make([]any, 0, len(selected))
		until := now.Add(friendOfferAge).Unix()
		lock := friendOfferReceiptID(to, id)
		stagedSender := tx.Get(from)
		for _, item := range selected {
			signature, err := offerItemSignature(item.goods)
			if err != nil {
				return err
			}
			name := roleString(item.goods, "Name")
			if name == "" {
				name = roleString(item.goods, "TemplateId")
			}
			items = append(items, Record{"id": item.id, "name": name, "count": item.count, "signature": signature})
			locked := locateGoods(stagedSender, item.id)
			locked[friendOfferLockKey] = lock
			locked[friendOfferUntilKey] = until
			locked["IsLock"] = true
		}
		offers[id] = Record{
			"sender": from, "name": roleString(sender, "Name"), "fee": fee, "items": items,
			"state": "pending", "created": now.UnixMilli(), "until": until,
		}
		tx.Get(to)[friendOffersKey] = offers
		return nil
	})
}

func offerItemSignature(goods Record) (string, error) {
	metadata := maps.Clone(goods)
	metadata["IsLock"] = false
	delete(metadata, friendOfferLockKey)
	delete(metadata, friendOfferUntilKey)
	wire, err := transferStackAttributes(metadata)
	if err != nil {
		return "", err
	}
	digest := sha256.Sum256(wire)
	return hex.EncodeToString(digest[:]), nil
}

func clearFriendOfferLock(goods Record) {
	delete(goods, friendOfferLockKey)
	delete(goods, friendOfferUntilKey)
	goods["IsLock"] = false
}

func hasFriendOfferLock(goods Record) bool {
	return roleString(goods, friendOfferLockKey) != ""
}

func friendOfferReceiptID(receiver, offer string) string {
	digest := sha256.Sum256([]byte(receiver + ":" + offer))
	return hex.EncodeToString(digest[:16])
}

func friendLockExpired(goods Record, now int64) bool {
	until := roleInt(goods, friendOfferUntilKey)
	return hasFriendOfferLock(goods) && until > 0 && until <= now
}

func (s *Server) expireFriendLocks(actor string) {
	now := time.Now().Unix()
	s.roles.mu.RLock()
	expired := false
	for _, row := range goodsList(s.roles.records[actor]) {
		item, _ := row.(Record)
		if friendLockExpired(item, now) {
			expired = true
			break
		}
	}
	s.roles.mu.RUnlock()
	if !expired {
		return
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		for _, row := range goodsList(tx.View(actor)) {
			item, _ := row.(Record)
			if friendLockExpired(item, now) {
				if current := locateGoods(tx.Get(actor), roleString(item, "GoodsId")); current != nil {
					clearFriendOfferLock(current)
				}
			}
		}
		return nil
	})
	if err != nil {
		log.Printf("清理过期传物锁: %v", err)
	}
}

func (s *Server) replyFriendOffers(peer *Peer, msid int) {
	data := s.roles.project(peer.currentRoleID(), []string{friendOffersKey, friendOfferReceiptsKey})
	now := time.Now().Unix()
	offers := friendMap(data, friendOffersKey)
	receipts := friendMap(data, friendOfferReceiptsKey)
	wire := offerNumber(nil, 1, 1)
	for _, id := range sortedFriendOfferIDs(offers) {
		row, _ := offers[id].(Record)
		if roleString(row, "state") == "pending" && roleInt(row, "until") > now {
			wire = appendFriendOffer(wire, 2, id, row)
		}
	}
	for _, id := range sortedFriendOfferIDs(receipts) {
		row, _ := receipts[id].(Record)
		wire = appendFriendOffer(wire, 3, id, row)
	}
	_ = peer.sendPayload(friendOfferList, msid, wire)
}

func sortedFriendOfferIDs(records Record) []string {
	ids := make([]string, 0, len(records))
	for id := range records {
		ids = append(ids, id)
	}
	slices.SortFunc(ids, func(a, b string) int {
		left, _ := records[a].(Record)
		right, _ := records[b].(Record)
		switch left, right := roleInt(left, "created"), roleInt(right, "created"); {
		case left < right:
			return -1
		case left > right:
			return 1
		default:
			return strings.Compare(a, b)
		}
	})
	return ids
}

func appendFriendOffer(wire []byte, number protowire.Number, id string, offer Record) []byte {
	row := offerString(nil, 1, id)
	row = offerString(row, 2, roleString(offer, "name"))
	row = offerString(row, 4, roleString(offer, "sender"))
	if roleString(offer, "state") == "accepted" {
		row = offerNumber(row, 5, 1)
	}
	items, _ := offer["items"].([]any)
	for _, entry := range items {
		item, _ := entry.(Record)
		stack := offerString(nil, 1, roleString(item, "name"))
		stack = offerNumber(stack, 2, uint64(roleInt(item, "count")))
		row = protowire.AppendBytes(protowire.AppendTag(row, 3, protowire.BytesType), stack)
	}
	return protowire.AppendBytes(protowire.AppendTag(wire, number, protowire.BytesType), row)
}

func (s *Server) decideFriendOffer(receiver, id string, accept bool) (sender string, changed bool, err error) {
	if !validOfferToken(id) {
		return "", false, previewReject(previewDeniedCode)
	}
	result := "declined"
	if accept {
		result = "accepted"
	}
	receiptID := friendOfferReceiptID(receiver, id)
	err = s.roles.Transact(func(tx *StoreTx) error {
		offers := friendMap(tx.View(receiver), friendOffersKey)
		entry, _ := offers[id].(Record)
		if entry == nil || roleInt(entry, "until") <= time.Now().Unix() {
			return previewReject(friendOfferUnavailable)
		}
		state := roleString(entry, "state")
		if result == state {
			return nil
		}
		if state != "pending" {
			return previewReject(friendOfferUnavailable)
		}
		sender = roleString(entry, "sender")
		from, to := tx.Get(sender), tx.Get(receiver)
		if from == nil {
			return previewReject(friendOfferUnavailable)
		}
		order, _ := entry["items"].([]any)
		if accept {
			if _, _, err := validateOfferFriends(sender, receiver, tx); err != nil {
				return err
			}
			if err := deliverFriendOffer(tx, from, to, sender, receiver, receiptID, order, roleInt(entry, "fee")); err != nil {
				return err
			}
		}

		// Once decided, reserved stacks wait for the sender to acknowledge.
		for _, value := range order {
			item, _ := value.(Record)
			goods := locateGoods(from, roleString(item, "id"))
			if roleString(goods, friendOfferLockKey) == receiptID {
				delete(goods, friendOfferUntilKey)
			}
		}
		putFriendEntry(from, friendOfferReceiptsKey, receiptID, Record{
			"name": roleString(to, "Name"), "sender": receiver, "state": result, "items": order,
			"created": time.Now().UnixMilli(),
		})
		current, _ := friendMap(to, friendOffersKey)[id].(Record)
		current["state"] = result
		changed = true
		return nil
	})
	return
}

func deliverFriendOffer(tx *StoreTx, from, to Record, sender, receiver, receiptID string, order []any, fee int64) error {
	balance := roleInt(from, "ShinyStone")
	if balance < fee {
		return previewReject(4398)
	}
	items := make([]any, 0, len(order))
	for _, value := range order {
		item, _ := value.(Record)
		id, count := roleString(item, "id"), roleInt(item, "count")
		goods := locateGoods(from, id)
		if goods == nil || roleString(goods, friendOfferLockKey) != receiptID || count < 1 || transferStackCount(goods) < count {
			return previewReject(4396)
		}
		signature, err := offerItemSignature(goods)
		if err != nil || signature != roleString(item, "signature") {
			return previewReject(4396)
		}
		copy := cloneRecord(goods)
		clearFriendOfferLock(copy)
		delete(copy, "GoodsId") // The receiver owns a distinct instance until the sender finishes cleanup.
		received, err := receiveInventoryGoods(to, copy, count)
		if err != nil {
			return err
		}
		items = append(items, Record{"goodsId": id, "receivedGoodsId": received, "templateId": roleString(goods, "TemplateId"), "count": count})
	}
	from["ShinyStone"] = balance - fee
	details := Record{"items": items, "fee": fee}
	return tx.RecordHistory(
		socialHistory("friend.goods.sent", sender, from, receiver, to, details),
		socialHistory("friend.goods.received", receiver, to, sender, from, details),
	)
}

func (s *Server) acknowledgeFriendOffer(sender, id string) (int64, error) {
	if !validOfferToken(id) {
		return 0, previewReject(previewDeniedCode)
	}
	var balance int64
	err := s.roles.Transact(func(tx *StoreTx) error {
		view := tx.View(sender)
		if view == nil {
			return previewReject(previewDeniedCode)
		}
		balance = roleInt(view, "ShinyStone")
		receipt, _ := friendMap(view, friendOfferReceiptsKey)[id].(Record)
		if receipt == nil {
			return nil // A retried acknowledgment must not remove more items.
		}
		record := tx.Get(sender)
		items, _ := receipt["items"].([]any)
		accepted := roleString(receipt, "state") == "accepted"
		for _, value := range items {
			item, _ := value.(Record)
			goodsID := roleString(item, "id")
			goods := locateGoods(record, goodsID)
			if roleString(goods, friendOfferLockKey) != id {
				if accepted {
					return previewReject(4396)
				}
				continue
			}
			if accepted {
				left := transferStackCount(goods) - roleInt(item, "count")
				if left == 0 {
					removeGoods(record, goodsID)
					continue
				}
				if left < 0 {
					return previewReject(4396)
				}
				goods["Count"] = left
			}
			clearFriendOfferLock(goods)
		}
		delete(friendMap(record, friendOfferReceiptsKey), id)
		return nil
	})
	return balance, err
}
