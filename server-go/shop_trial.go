package main

import (
	"maps"
	"slices"
	"strconv"
	"strings"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const shopLimitedCategory = 4

type trialShopProduct struct {
	id, currency, category int64
	price                  int64
	goods                  Record
}

func (s *Server) loadTrialShop() error {
	var rows []Record
	if err := s.readGameTable("xuantianlougoods.json", &rows); err != nil {
		return err
	}
	s.game.trialShop = make(map[int64]trialShopProduct, len(rows))
	for _, row := range rows {
		id, currency, price := roleInt(row, "Id"), roleInt(row, "BuyType"), roleInt(row, "GoodsPrice")
		name, template := roleString(row, "Name"), roleString(row, "TemplateId")
		kind := roleInt(row, "GoodsType")
		if id < 1 || (currency != 1 && currency != 2) || price < 1 || name == "" || template == "" || kind < 1 {
			continue
		}
		goods := Record{
			"Name": name, "TemplateId": template, "Type": kind, "Count": 1,
			"FiveElement": roleInt(row, "FiveElement"), "Rank": max(1, roleInt(row, "Rank")),
			"Level": max(1, roleInt(row, "Level")), "Volume": shopProductVolume(row),
			"IsBinding": false, "IsUsing": false, "IsLock": false,
		}
		s.game.trialShop[id] = trialShopProduct{id, currency, roleInt(row, "XTL_GoodsType"), price, goods}
	}
	return nil
}

func shopProductVolume(row Record) float64 {
	for _, detail := range stringRecords(row["GoodsDetail"]) {
		if text, ok := strings.CutPrefix(detail, "占用体积,"); ok {
			if n, err := strconv.ParseFloat(strings.TrimSuffix(text, "立方"), 64); err == nil && n > 0 {
				return n
			}
		}
	}
	return 1
}

func trialShopDailyLimit(vip int64) int64 {
	for _, threshold := range [...]struct{ level, bonus int64 }{
		{12, 10}, {10, 9}, {9, 8}, {8, 7}, {7, 6}, {4, 5}, {3, 4}, {2, 3},
	} {
		if vip >= threshold.level {
			return 10 + threshold.bonus
		}
	}
	return 10
}

func trialShopPurchases(role Record, day string) Record {
	if roleString(role, "ShopPurchaseDay") != day {
		return nil
	}
	counts, _ := role["ShopDailyPurchases"].(Record)
	return counts
}

func (s *Server) trialShopCatalog(peer *Peer, _, out protoreflect.Message) (func(), error) {
	role := s.roles.project(peer.currentRoleID(), []string{"ShopDailyPurchases", "ShopPurchaseDay"})
	if role == nil {
		return nil, previewReject(4007)
	}
	counts := trialShopPurchases(role, mentorGameDay(time.Now()))
	for _, id := range slices.Sorted(maps.Keys(s.game.trialShop)) {
		product := s.game.trialShop[id]
		if product.category != shopLimitedCategory {
			continue
		}
		putPB(addMessage(out, "Goods"), Record{"Id": id, "SystemPrice": product.price,
			"Count": roleInt(counts, strconv.FormatInt(id, 10))})
	}
	return nil, nil
}

type trialShopReceipt struct {
	seq       int
	roleID    string
	productID int64
}

type trialShopReceipts struct {
	recent [16]trialShopReceipt
	next   int
}

func (s *Server) trialShopBuy(peer *Peer, req, out protoreflect.Message) (func(), error) {
	owner, id := peer.currentRoleID(), getInt(req, "Id")
	product, ok := s.game.trialShop[id]
	if !ok {
		return nil, previewReject(5001)
	}
	if receipts := peer.shopReceipts; receipts != nil && peer.gameRequestID > 0 {
		for _, receipt := range receipts.recent {
			if receipt.seq != peer.gameRequestID || receipt.roleID != owner {
				continue
			}
			if receipt.productID != id {
				return nil, previewReject(4008)
			}
			statusPB(out, true, 4299)
			copyRolePB(out, s.roles.project(owner, []string{"ShinyStone", "ShinyJade"}), "ShinyStone", "ShinyJade")
			return nil, nil
		}
	}
	day := mentorGameDay(time.Now())
	var balances Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(owner)
		if role == nil {
			return previewReject(4007)
		}
		field, insufficient := "ShinyStone", 4297
		if product.currency == 2 {
			field, insufficient = "ShinyJade", 4298
		}
		balance := roleInt(role, field)
		if balance < product.price {
			return previewReject(insufficient)
		}
		key := strconv.FormatInt(id, 10)
		count := roleInt(trialShopPurchases(role, day), key)
		if product.category == shopLimitedCategory && count >= trialShopDailyLimit(roleInt(role, "VipLevel")) {
			return previewReject(4296)
		}

		role = tx.Get(owner)
		if _, err := receiveInventoryGoods(role, product.goods, 1); err != nil {
			return err
		}
		role[field] = balance - product.price
		if product.category == shopLimitedCategory {
			counts := trialShopPurchases(role, day)
			if counts == nil {
				counts = Record{}
				role["ShopPurchaseDay"], role["ShopDailyPurchases"] = day, counts
			}
			counts[key] = count + 1
		}

		balances = Record{"ShinyStone": roleInt(role, "ShinyStone"), "ShinyJade": roleInt(role, "ShinyJade")}
		event := newHistoryEvent("shop.purchased", role, nil, "shop_product", key,
			Record{"currency": field, "amount": product.price, "templateId": roleString(product.goods, "TemplateId")})
		event.Actor.RoleID = owner
		return tx.RecordHistory(event)
	})
	if err != nil {
		return nil, err
	}
	if peer.gameRequestID > 0 {
		if peer.shopReceipts == nil {
			peer.shopReceipts = new(trialShopReceipts)
		}
		receipts := peer.shopReceipts
		receipts.recent[receipts.next] = trialShopReceipt{peer.gameRequestID, owner, id}
		receipts.next = (receipts.next + 1) % len(receipts.recent)
	}
	statusPB(out, true, 4299)
	putPB(out, balances)
	return nil, nil
}
