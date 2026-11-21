package main

import (
	"encoding/json"
	"fmt"
	"slices"
	"sort"
	"time"
)

// The original altar accepts books from the ring, including both weapon and armour blueprints.
type voidAltarPick struct {
	GoodsID string `json:"goodsId"`
	Count   int    `json:"count"`
}

func altarBook(g Record) bool {
	t := roleInt(g, "Type")
	return (t >= 1 && t <= 6 || t == 17 || t == 18) && roleString(g, "GoodsId") != "" && !voidMaterialTemplate(g) && forgeUsable(g)
}

func altarGrade(g Record) int { return max(1, min(9, (int(roleInt(g, "Level"))+9)/10)) }
func altarWorth(g Record) int { return max(1, min(4, (int(roleInt(g, "Rank"))+3)/4)) }

func altarOverview(r Record) Record {
	books := []Record{}
	for _, entry := range goodsList(r) {
		g, _ := entry.(Record)
		if altarBook(g) {
			books = append(books, Record{"goodsId": roleString(g, "GoodsId"), "name": roleString(g, "Name"), "grade": altarGrade(g), "worth": altarWorth(g), "count": transferStackCount(g)})
		}
	}
	sort.Slice(books, func(i, j int) bool {
		a, b := books[i], books[j]
		if a["grade"] != b["grade"] {
			return a["grade"].(int) < b["grade"].(int)
		}
		if a["name"] != b["name"] {
			return a["name"].(string) < b["name"].(string)
		}
		return a["goodsId"].(string) < b["goodsId"].(string)
	})
	reserve, _ := r["VoidAltarIncense"].(Record)
	balances := make([]int, 9)
	for i := range balances {
		balances[i] = int(roleInt(reserve, fmt.Sprint(i+1)))
	}
	return Record{"books": books, "balances": balances, "cap": voidRealmStage(r)}
}

// Altar copies can stack only with other unsaleable, role-bound rewards from this altar.
func altarReward(r Record, grade, count int) bool {
	if count == 0 {
		return true
	}
	template := fmt.Sprintf("de_jing_%d", grade)
	for _, raw := range goodsList(r) {
		item, _ := raw.(Record)
		if roleString(item, "AltarOrigin") == template && roleInt(item, "Type") == 39 && !inventoryFlag(item, "IsLock") && !inventoryFlag(item, "IsUsing") && !hasFriendOfferLock(item) && transferStackCount(item) > 0 {
			item["Count"] = max(1, roleInt(item, "Count")) + int64(count)
			return true
		}
	}
	if len(goodsList(r)) >= ringCapacity(r) {
		return false
	}
	r["Goods"] = append(goodsList(r), Record{"GoodsId": "g_" + newGameID(8), "Name": "德经", "Type": 39, "Rank": 16, "Level": 1 + (grade-1)*10, "FiveElement": 0, "TemplateId": template, "Count": count, "Volume": 1.0, "RecyclePrice": 0, "ConversionRate": 1.0, "IsLock": false, "IsUsing": false, "IsBinding": true, "AltarOrigin": template})
	return true
}

func altarRecipe(owner string, r Record, q voidRequest, picks []voidAltarPick) (Record, error) {
	if q.Grade < 1 || q.Grade > voidRealmStage(r) {
		return nil, forgeFailure("请选择自身境界以内的德经等阶")
	}
	if len(picks) == 0 || len(picks) > 10 {
		return nil, forgeFailure("请从纳戒选一至十本书入炉")
	}
	units, burn := 0, 0
	last := ""
	stock := make([]int64, 0, len(picks))
	for _, p := range picks {
		g := locateGoods(r, p.GoodsID)
		if g == nil || !altarBook(g) || p.GoodsID == last || p.Count < 1 || p.Count > 10 || int64(p.Count) > transferStackCount(g) {
			return nil, forgeFailure("卷册余量或锁定状态已改变，请重选祭书")
		}
		if altarGrade(g) < q.Grade {
			return nil, forgeFailure("祭书的等阶须达到本炉所炼德经的等阶")
		}
		units += p.Count
		if units > 10 {
			return nil, forgeFailure("每炉最多献祭十本书籍")
		}
		last = p.GoodsID
		stock = append(stock, transferStackCount(g))
		burn += p.Count * altarWorth(g)
	}
	bank, _ := r["VoidAltarIncense"].(Record)
	k := fmt.Sprint(q.Grade)
	before := int(roleInt(bank, k))
	copies, after := (before+burn)/8, (before+burn)%8
	draft, _ := json.Marshal(picks)
	return Record{"grade": q.Grade, "count": units, "incense": burn, "before": before, "after": after, "copies": copies, "quoteId": voidDigest("altar", owner, k, string(draft), fmt.Sprint(stock), fmt.Sprint(burn), fmt.Sprint(before))}, nil
}

func altarBurn(r Record, picks []voidAltarPick, result Record) error {
	for _, p := range picks {
		for range p.Count {
			removeGoods(r, p.GoodsID)
		}
	}
	grade := result["grade"].(int)
	if !altarReward(r, grade, result["copies"].(int)) {
		return forgeFailure("纳戒没有空位安放新德经，请在纳戒腾出一格")
	}
	auctionMapEdit(r, "VoidAltarIncense")[fmt.Sprint(grade)] = result["after"]
	return nil
}

func (s *Server) voidAltarAction(owner string, q voidRequest, now time.Time) (out Record, err error) {
	err = s.roles.Transact(func(tx *StoreTx) error {
		r := tx.View(owner)
		if owner == "" || reservedStoreID(owner) || r == nil {
			return forgeFailure("请先登录角色")
		}
		if roleInt(r, "Level") < 21 {
			return forgeFailure("达到结丹一阶后可进入虚神祭坛")
		}
		if q.Action == "altar" {
			out = altarOverview(r)
			return nil
		}
		if q.Action != "altarPreview" && q.Action != "altarOffer" {
			return forgeFailure("未知的祭坛操作")
		}
		picks := slices.Clone(q.AltarItems)
		sort.Slice(picks, func(i, j int) bool { return picks[i].GoodsID < picks[j].GoodsID })
		write := q.Action == "altarOffer"
		key, signature := "", ""
		if write {
			if !validOfferToken(q.RequestID) || q.QuoteID == "" {
				return forgeFailure("请先核对本炉祭书与回报")
			}
			key = voidStorePrefix + "altar:" + voidDigest(owner, q.RequestID)
			raw, _ := json.Marshal(picks)
			signature = voidDigest(fmt.Sprint(q.Grade), string(raw), q.QuoteID)
			if prior := tx.View(key); prior != nil {
				if roleString(prior, "signature") != signature {
					return forgeFailure("此炉已经结算，请重新核香")
				}
				out, _ = prior["result"].(Record)
				return nil
			}
		}
		result, e := altarRecipe(owner, r, q, picks)
		if e != nil {
			return e
		}
		if write && q.QuoteID != result["quoteId"] {
			return forgeFailure("本炉余香或祭书已有变化，请重新核香")
		}
		if write {
			if e = altarBurn(tx.Get(owner), picks, result); e != nil {
				return e
			}
			stamp := tx.GetOrCreate(key)
			stamp["kind"], stamp["signature"], stamp["time"], stamp["result"] = "altar", signature, now.Unix(), result
		} else {
			// Check capacity after removing offerings, without staging any changes.
			if e = altarBurn(cloneRecord(r), picks, result); e != nil {
				return e
			}
		}
		out = result
		return nil
	})
	return
}
