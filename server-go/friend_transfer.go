package main

import (
	"bytes"
	"crypto/sha256"
	"encoding/json"
	"maps"
	"math"
	"slices"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
)

const maxTransferKinds = 5

// The original PB envelope numbers requests on a single game connection.
type friendTransferReceipt struct {
	requestID int
	actorID   string
	digest    [32]byte
}

type friendTransferReceipts struct {
	recent [16]friendTransferReceipt
	next   int
}

func (s *Server) loadSocialTitles() error {
	var titles []struct{ ID, Type, EffectValue int }
	if err := s.readGameTable("chenghaoinfo.json", &titles); err != nil {
		return err
	}
	s.game.transferDiscounts = make(map[int]int64)
	s.game.graduationTitleLuck = make(map[int]int64)
	s.game.acupointTitleBonus = make(map[int]int)
	for _, title := range titles {
		if title.Type == 14 && title.EffectValue > 0 && title.EffectValue <= 100 {
			s.game.acupointTitleBonus[title.ID] = title.EffectValue
		}
		if title.ID >= 501 && title.ID <= 503 && title.Type == title.ID-430 && title.EffectValue >= 0 {
			s.game.graduationTitleLuck[title.ID] = int64(title.EffectValue)
		}
		if title.Type == 1 && title.EffectValue > 0 && title.EffectValue <= 100 {
			s.game.transferDiscounts[title.ID] = int64(title.EffectValue)
		}
	}
	return nil
}

func friendTitleIDs(r Record) []int {
	switch titles := r["TitleId"].(type) {
	case []int:
		return titles
	case []any:
		ids := make([]int, 0, len(titles))
		for _, item := range titles {
			if id := roleInt(Record{"Id": item}, "Id"); id > 0 && id <= math.MaxInt32 {
				ids = append(ids, int(id))
			}
		}
		return ids
	}
	return nil
}

func fillFriendTransferTitles(out protoreflect.Message, role Record) {
	for _, id := range friendTitleIDs(role) {
		out.Mutable(field(out, "TitleId")).List().Append(protoreflect.ValueOfInt32(int32(id)))
	}
}

func (s *Server) trialFriendTransfer(peer *Peer, req, out protoreflect.Message) (func(), error) {
	actorID, targetID := peer.currentRoleID(), getString(req, "TargetFriendId")
	requested := req.Get(field(req, "Goods")).List()
	if requested.Len() < 1 || requested.Len() > maxTransferKinds {
		return nil, previewReject(4399)
	}
	payload, err := proto.MarshalOptions{Deterministic: true}.Marshal(req.Interface())
	if err != nil {
		return nil, err
	}
	key := friendTransferReceipt{requestID: peer.gameRequestID, actorID: actorID, digest: sha256.Sum256(payload)}
	if receipts := peer.transferReceipts; receipts != nil {
		for _, previous := range receipts.recent {
			if previous.actorID != actorID || previous.requestID != key.requestID {
				continue
			}
			if previous != key {
				return nil, previewReject(4008)
			}
			statusPB(out, true, 4397)
			set(out, "CurrentShinyStone", roleInt(s.roles.project(actorID, []string{"ShinyStone"}), "ShinyStone"))
			return nil, nil
		}
	}
	var remaining int64
	err = s.roles.Transact(func(tx *StoreTx) error {
		from, to, err := validateFriendPair(actorID, targetID, tx)
		if err != nil {
			return err
		}
		if _, ok := friendMap(from, friendLinks)[targetID]; !ok {
			return previewReject(4407)
		}
		if _, ok := friendMap(to, friendLinks)[actorID]; !ok {
			return previewReject(4407)
		}
		from, to = tx.Get(actorID), tx.Get(targetID)
		goods, price, err := transferFriendGoods(from, to, requested)
		if err != nil {
			return err
		}
		var discount int64
		for _, id := range friendTitleIDs(from) {
			discount = max(discount, s.game.transferDiscounts[id])
		}
		// Do not multiply the full price by the percentage: legacy balances are int64.
		price -= price/100*discount + price%100*discount/100
		balance := roleInt(from, "ShinyStone")
		if balance < price {
			return previewReject(4398)
		}
		remaining = balance - price
		from["ShinyStone"] = remaining
		details := Record{"items": goods, "fee": price}
		return tx.RecordHistory(
			socialHistory("friend.goods.sent", actorID, from, targetID, to, details),
			socialHistory("friend.goods.received", targetID, to, actorID, from, details),
		)
	})
	if err != nil {
		return nil, err
	}
	if peer.transferReceipts == nil {
		peer.transferReceipts = new(friendTransferReceipts)
	}
	receipts := peer.transferReceipts
	receipts.recent[receipts.next] = key
	receipts.next = (receipts.next + 1) % len(receipts.recent)
	statusPB(out, true, 4397)
	set(out, "CurrentShinyStone", remaining)
	return nil, nil
}

func transferFriendGoods(from, to Record, requested protoreflect.List) ([]any, int64, error) {
	seen := make(map[string]bool, requested.Len())
	items := make([]any, 0, requested.Len())
	var price int64
	for i := 0; i < requested.Len(); i++ {
		row := requested.Get(i).Message()
		id, count := getString(row, "GoodsId"), getInt(row, "Count")
		if id == "" || count < 1 || seen[id] {
			return nil, 0, previewReject(4399)
		}
		seen[id] = true
		goods := locateGoods(from, id)
		if goods == nil || transferStackCount(goods) < count {
			return nil, 0, previewReject(4396)
		}
		for _, flag := range [...]string{"IsBinding", "IsLock", "IsUsing"} {
			if roleBool(goods, flag) || roleInt(goods, flag) != 0 {
				return nil, 0, previewReject(4509)
			}
		}
		rank := max(1, roleInt(goods, "Rank"))
		stage := (max(1, roleInt(goods, "Level"))-1)/10 + 1
		budget := (math.MaxInt64 - price) / 100 / count
		if rank > budget/stage {
			return nil, 0, previewReject(4398)
		}
		price += 100 * count * rank * stage
		receivedID, err := receiveFriendGoods(to, goods, count)
		if err != nil {
			return nil, 0, err
		}
		items = append(items, Record{"goodsId": id, "receivedGoodsId": receivedID, "templateId": roleString(goods, "TemplateId"), "count": count})
		left := transferStackCount(goods) - count
		if left > 0 {
			goods["Count"] = left
		} else {
			from["Goods"] = slices.DeleteFunc(goodsList(from), func(entry any) bool {
				g, _ := entry.(Record)
				return roleString(g, "GoodsId") == id
			})
		}
	}
	return items, price, nil
}

func transferStackCount(goods Record) int64 {
	if goods["Count"] == nil {
		return 1 // Legacy single-item records omit Count.
	}
	return roleInt(goods, "Count")
}

// Compare all item attributes, including save fields unknown to this server.
// Only the per-instance ID and quantity may differ between mergeable stacks.
func transferStackAttributes(goods Record) ([]byte, error) {
	metadata := maps.Clone(goods)
	delete(metadata, "GoodsId")
	delete(metadata, "Count")
	return json.Marshal(metadata)
}

func receiveFriendGoods(to, goods Record, count int64) (string, error) {
	items := goodsList(to)
	if !isGameEquipment(roleInt(goods, "Type")) {
		attributes, err := transferStackAttributes(goods)
		if err != nil {
			return "", err
		}
		for _, item := range items {
			target, _ := item.(Record)
			if roleString(target, "TemplateId") != roleString(goods, "TemplateId") || roleInt(target, "Type") != roleInt(goods, "Type") {
				continue
			}
			other, err := transferStackAttributes(target)
			if err != nil {
				return "", err
			}
			available := transferStackCount(target)
			if bytes.Equal(attributes, other) && roleString(target, "GoodsId") != "" && available > 0 && available <= math.MaxInt32-count {
				target["Count"] = available + count
				return roleString(target, "GoodsId"), nil
			}
		}
	}
	if len(items) >= 100 {
		return "", previewReject(4527)
	}
	received := cloneRecord(goods)
	id := roleString(goods, "GoodsId")
	if count < transferStackCount(goods) || locateGoods(to, id) != nil {
		id = "g_" + newGameID(8)
	}
	received["GoodsId"], received["Count"] = id, count
	to["Goods"] = append(items, received)
	return id, nil
}
