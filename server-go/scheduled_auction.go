package main

import (
	"cmp"
	"crypto/sha256"
	"encoding/json"
	"errors"
	"fmt"
	"math"
	"slices"
	"strings"
	"time"
)

const scheduledAuctionStoreID = "$auction:calendar"

type auctionRefusal string

func (e auctionRefusal) Error() string { return string(e) }

var auctionZone = time.FixedZone("UTC+8", 8*60*60)
var auctionTiers = [...]string{"daily", "weekly", "monthly"}

// Calendar values are copied into each session at creation. Restarts with new
// configuration cannot change the deadline of an already escrowed lot.
type auctionCalendarRule struct {
	Limit       int `json:"limit"`
	LotSeconds  int `json:"lotSeconds"`
	CollectHour int `json:"collectHour"`
	PreviewHour int `json:"previewHour"`
	LiveHour    int `json:"liveHour"`
}

var auctionDefaultRules = map[string]auctionCalendarRule{
	"daily":   {8, 45, 9, 18, 19},
	"weekly":  {16, 60, 9, 12, 20},
	"monthly": {24, 75, 9, 12, 21},
}

func validateAuctionCalendar(overrides map[string]auctionCalendarRule) error {
	for tier, c := range overrides {
		_, known := auctionDefaultRules[tier]
		if !known || c.Limit < 1 || c.Limit > 64 || c.LotSeconds < 10 || c.LotSeconds > 300 ||
			c.CollectHour < 0 || c.CollectHour > 23 || c.PreviewHour < 0 || c.PreviewHour >= c.LiveHour || c.LiveHour > 23 ||
			tier == "daily" && c.CollectHour >= c.PreviewHour || c.LiveHour*3600+c.Limit*c.LotSeconds >= 86400 {
			return fmt.Errorf("invalid auctionCalendar entry %q", tier)
		}
	}
	return nil
}

type auctionAction struct {
	ViewTag   string `json:"viewTag,omitempty"`
	Action    string `json:"action"`
	Tier      string `json:"tier"`
	EventID   string `json:"eventId"`
	RequestID string `json:"requestId"`
	LotID     string `json:"lotId"`
	GoodsID   string `json:"goodsId"`
	Amount    int64  `json:"amount"`
	Count     int64  `json:"count"`
	Service   string `json:"service"`
	Text      string `json:"text"`
}

type auctionGoodsView struct {
	ID         string `json:"id"`
	Name       string `json:"name"`
	TemplateID string `json:"templateId"`
	BlankID    string `json:"blankId"`
	Rank       int64  `json:"rank"`
	Kind       int64  `json:"kind"`
	Count      int64  `json:"count"`
}

type auctionDeliveryView struct {
	auctionGoodsView
	Reason string `json:"reason"`
}

type auctionLotView struct {
	auctionGoodsView
	Source       string `json:"source"`
	Seller       string `json:"seller"`
	Leader       string `json:"leader"`
	Status       string `json:"status"`
	Mine         bool   `json:"mine"`
	Leading      bool   `json:"leading"`
	Participated bool   `json:"participated"`
	LastBid      int64  `json:"lastBid"`
	MyMax        int64  `json:"myMax"`
	Start        int64  `json:"start"`
	Price        int64  `json:"price"`
	Step         int64  `json:"step"`
	OpensAt      int64  `json:"opensAt"`
	EndsAt       int64  `json:"endsAt"`
}

type auctionView struct {
	EventID       string                `json:"eventId"`
	Tier          string                `json:"tier"`
	Phase         string                `json:"phase"`
	Now           int64                 `json:"now"`
	NextCollectAt int64                 `json:"nextCollectAt"`
	CollectAt     int64                 `json:"collectAt"`
	PreviewAt     int64                 `json:"previewAt"`
	LiveAt        int64                 `json:"liveAt"`
	EndsAt        int64                 `json:"endsAt"`
	LotSeconds    int64                 `json:"lotSeconds"`
	Limit         int64                 `json:"limit"`
	Balance       int64                 `json:"balance"`
	Lots          []auctionLotView      `json:"lots"`
	Goods         []auctionGoodsView    `json:"goods"`
	Deliveries    []auctionDeliveryView `json:"deliveries"`
	Services      []auctionService      `json:"services"`
	Seat          Record                `json:"seat"`
	Messages      []auctionChatLine     `json:"messages"`
}

func auctionMap(r Record, key string) Record { child, _ := r[key].(Record); return child }
func auctionMapEdit(r Record, key string) Record {
	if m := auctionMap(r, key); m != nil {
		return m
	}
	m := Record{}
	r[key] = m
	return m
}

func (s *Server) auctionSession(tier string, now time.Time) Record {
	r, ok := s.config.AuctionCalendar[tier]
	if !ok {
		r = auctionDefaultRules[tier]
	}
	d := now.In(auctionZone)
	day := time.Date(d.Year(), d.Month(), d.Day(), 0, 0, 0, 0, auctionZone)
	start := day
	switch tier {
	case "weekly":
		start = day.AddDate(0, 0, -(int(day.Weekday())+6)%7)
	case "monthly":
		start = day.AddDate(0, 0, 1-day.Day())
	}
	if d.Before(start.Add(time.Duration(r.CollectHour) * time.Hour)) {
		switch tier {
		case "daily":
			start = start.AddDate(0, 0, -1)
		case "weekly":
			start = start.AddDate(0, 0, -7)
		case "monthly":
			start = start.AddDate(0, -1, 0)
		}
	}
	liveDay := start
	switch tier {
	case "weekly":
		liveDay = start.AddDate(0, 0, 5)
	case "monthly":
		liveDay = start.AddDate(0, 1, -1)
	}
	open := liveDay.Add(time.Duration(r.LiveHour) * time.Hour)
	next := start.AddDate(0, 0, 1)
	switch tier {
	case "weekly":
		next = start.AddDate(0, 0, 7)
	case "monthly":
		next = start.AddDate(0, 1, 0)
	}
	return Record{"nextCollect": next.Add(time.Duration(r.CollectHour) * time.Hour).UnixMilli(), "id": tier + "-" + liveDay.Format("20060102"), "collect": start.Add(time.Duration(r.CollectHour) * time.Hour).UnixMilli(),
		"preview": liveDay.Add(time.Duration(r.PreviewHour) * time.Hour).UnixMilli(), "live": open.UnixMilli(),
		"end": open.Add(time.Duration(r.Limit*r.LotSeconds) * time.Second).UnixMilli(), "seconds": r.LotSeconds, "limit": r.Limit}
}

func auctionPhase(e Record, ms int64) string {
	switch {
	case ms < roleInt(e, "collect"), ms >= roleInt(e, "end"):
		return "closed"
	case ms >= roleInt(e, "live"):
		return "live"
	case ms >= roleInt(e, "preview"):
		return "preview"
	default:
		return "collect"
	}
}

func auctionNeedsAdvance(e, fresh Record, ms int64) bool {
	return roleString(e, "id") < roleString(fresh, "id") || (!roleBool(e, "selected") && ms >= roleInt(e, "preview")) ||
		roleInt(e, "nextSettlement") > 0 && ms >= roleInt(e, "nextSettlement")
}

func (s *Server) advanceScheduledAuctions(now time.Time) error {
	var pending []error
	for _, tier := range auctionTiers {
		if err := s.advanceAuctionTier(tier, now); err != nil {
			pending = append(pending, fmt.Errorf("%s: %w", tier, err))
		}
	}
	return errors.Join(pending...)
}

func (s *Server) advanceAuctionTier(tier string, now time.Time) error {
	store := s.roles
	fresh := s.auctionSession(tier, now)
	ms := now.UnixMilli()
	store.mu.RLock()
	due := auctionNeedsAdvance(auctionMap(store.records[scheduledAuctionStoreID], tier), fresh, ms)
	store.mu.RUnlock()
	if !due {
		return nil
	}
	return store.Transact(func(tx *StoreTx) error {
		e := auctionMap(tx.View(scheduledAuctionStoreID), tier)
		if !auctionNeedsAdvance(e, fresh, ms) {
			return nil
		}
		state := tx.GetOrCreate(scheduledAuctionStoreID)
		e = auctionMap(state, tier)
		if e != nil {
			if err := s.advanceAuctionSession(tx, e, ms, tier); err != nil {
				return err
			}
		}
		if e == nil || roleString(e, "id") < roleString(fresh, "id") {
			// Complete the previous session, even after a long offline period.
			if e != nil && !roleBool(e, "finished") {
				return errors.New("前场交割尚未完成")
			}
			state[tier] = fresh
			return s.advanceAuctionSession(tx, fresh, ms, tier)
		}
		return nil
	})
}

func (s *Server) advanceAuctionSession(tx *StoreTx, e Record, ms int64, tier string) error {
	if ms < roleInt(e, "preview") {
		return nil
	}
	if !roleBool(e, "selected") {
		if err := s.selectAuctionLots(tx, e, tier); err != nil {
			return err
		}
		e["selected"] = true
	}
	list := goodsListForField(e, "lots")
	for i, item := range list {
		lot := item.(Record)
		if roleBool(lot, "settled") {
			continue
		}
		deadline := roleInt(e, "live") + int64(i+1)*roleInt(e, "seconds")*1000
		if ms < deadline {
			e["nextSettlement"] = deadline
			return nil
		}
		if err := settleAuctionLot(tx, lot); err != nil {
			return err
		}
	}
	e["nextSettlement"] = 0
	e["finished"] = true
	return nil
}

func (s *Server) selectAuctionLots(tx *StoreTx, e Record, tier string) error {
	list := goodsListForField(e, "lots")
	slices.SortStableFunc(list, func(a, b any) int {
		x, y := auctionMap(a.(Record), "goods"), auctionMap(b.(Record), "goods")
		if c := cmp.Compare(roleInt(y, "Rank"), roleInt(x, "Rank")); c != 0 {
			return c
		}
		return cmp.Compare(roleInt(y, "Level"), roleInt(x, "Level"))
	})
	limit := int(roleInt(e, "limit"))
	if len(list) > limit {
		for _, v := range list[limit:] {
			lot := v.(Record)
			if err := auctionDelivery(tx, roleString(lot, "seller"), lot, "notSelected"); err != nil {
				return err
			}
		}
		list = list[:limit]
	}
	for len(list) < limit {
		list = append(list, s.auctionHouseLot(tier, len(list), roleInt(e, "live")))
	}
	e["lots"] = list
	return nil
}

func (s *Server) auctionHouseLot(tier string, index int, liveAt int64) Record {
	// Use playable inventory templates already used in the starter kit and auction.
	type template struct {
		name, id                 string
		kind, rank, count, price int
	}
	catalog := [...]template{
		{"筑基丹", previewTreasureTemplate, 19, 4, 1, 350}, {"纳气丹", "pill_naqi", 19, 1, 3, 120},
		{"玄铁护心镜", "eq_armor_xuantie", 16, 2, 1, 400}, {"启灵经", practiceStarterMethodID, 1, 1, 1, 200},
		{"回春散", "pill_huichun", 19, 1, 6, 180}, {"青锋剑", "eq_weapon_qingfeng", 15, 1, 1, 300},
	}
	scale := 1
	if tier == "weekly" {
		scale = 2
	} else if tier == "monthly" {
		scale = 4
	}
	// Rotate the catalogue by session day. Major sales include higher-rank
	// learnable books, with the final house slot reserved for the featured lot.
	rotation := int(liveAt / 86_400_000)
	t := catalog[(rotation+index+scale-1)%len(catalog)]
	g := Record{}
	count := t.count
	if !isGameEquipment(int64(t.kind)) {
		count *= scale * (1 + index/len(catalog))
	}
	goods := addGoods(g, t.name, t.kind, t.rank, 1, 1, t.id, count, 1, 5)
	start := int64(t.price * count / t.count)
	if index%3 == 0 || index == auctionDefaultRules[tier].Limit-1 {
		rankBand := 1
		if tier == "weekly" {
			rankBand = 5
		} else if tier == "monthly" {
			rankBand = 9
		}
		var books []gameMethod
		for _, m := range legacyMethods.all {
			if m.rank >= rankBand && m.rank <= rankBand+3 {
				books = append(books, m)
			}
		}
		if len(books) > 0 {
			m := books[(rotation+index)%len(books)]
			goods = addGoods(Record{}, m.name, 1, m.rank, m.learningLevel, m.element, m.id, 1, 1, 10*m.rank)
			start = int64(125 * m.rank * m.rank)
		}
	}
	return Record{"id": "lot-" + newGameID(8), "goods": goods, "start": start, "price": start, "step": max(1, start/20)}
}

func auctionDelivery(tx *StoreTx, id string, lot Record, reason string) error {
	role := tx.Get(id)
	if role == nil {
		return errors.New("auction delivery role missing")
	}
	goods := auctionMap(lot, "goods")
	auctionMapEdit(role, "AuctionDeliveries")[roleString(lot, "id")] = Record{"item": goods, "reason": reason}
	return nil
}

func settleAuctionLot(tx *StoreTx, lot Record) error {
	winner, seller := roleString(lot, "leader"), roleString(lot, "seller")
	if winner == "" {
		if seller != "" {
			if err := auctionDelivery(tx, seller, lot, "passed"); err != nil {
				return err
			}
		}
	} else {
		price := roleInt(lot, "price")
		if refund := roleInt(lot, "max") - price; refund > 0 {
			if err := refundPreviewAuctionBid(tx.Get(winner), refund); err != nil {
				return err
			}
		}
		if seller != "" {
			if err := refundPreviewAuctionBid(tx.Get(seller), price-max(1, price/20)); err != nil {
				return err
			}
		}
		if err := auctionDelivery(tx, winner, lot, "won"); err != nil {
			return err
		}
	}
	lot["settled"] = true
	delete(lot, "max")
	return nil
}

func auctionEligible(g Record) bool {
	if g == nil || hasFriendOfferLock(g) || roleString(g, "GoodsId") == "" || fixedAuctionShop(roleInt(g, "Type")) == 0 || transferStackCount(g) <= 0 {
		return false
	}
	for _, flag := range [...]string{"IsBinding", "IsLock", "IsUsing"} {
		if roleBool(g, flag) || roleInt(g, flag) != 0 {
			return false
		}
	}
	return true
}

func auctionSubmit(tx *StoreTx, e Record, roleID string, req auctionAction, ms int64) error {
	if roleBool(e, "selected") || auctionPhase(e, ms) != "collect" {
		return auctionRefusal("本场征集已封册")
	}
	if req.Count < 1 || req.Count > math.MaxInt32 || req.Amount < 100 || req.Amount > 1_000_000_000 {
		return auctionRefusal("起拍总价为100至10亿灵石，数量至少为一")
	}
	role := tx.Get(roleID)
	g := locateGoods(role, req.GoodsID)
	if !auctionEligible(g) || transferStackCount(g) < req.Count {
		return auctionRefusal("此物当前无法送拍，请核对纳戒")
	}
	list := goodsListForField(e, "lots")
	own := 0
	for _, v := range list {
		if roleString(v.(Record), "seller") == roleID {
			own++
		}
	}
	if own >= 3 {
		return auctionRefusal("每位道友每场最多送拍三件")
	}
	item := cloneRecord(g)
	if left := transferStackCount(g) - req.Count; left > 0 {
		g["Count"] = left
		item["GoodsId"] = "g_" + newGameID(8)
	} else {
		role["Goods"] = slices.DeleteFunc(goodsList(role), func(v any) bool { g, _ := v.(Record); return roleString(g, "GoodsId") == req.GoodsID })
	}
	item["Count"] = req.Count
	e["lots"] = append(list, Record{"id": "lot-" + newGameID(8), "goods": item, "seller": roleID, "sellerName": roleString(role, "Name"), "start": req.Amount, "price": req.Amount, "step": max(1, req.Amount/20)})
	return nil
}

func auctionLotStatus(e, lot Record, i int, ms int64) string {
	if roleBool(lot, "settled") {
		if roleString(lot, "leader") == "" {
			return "passed"
		}
		return "sold"
	}
	switch auctionPhase(e, ms) {
	case "collect":
		return "pending"
	case "preview":
		return "preview"
	}
	open := roleInt(e, "live") + int64(i)*roleInt(e, "seconds")*1000
	if ms >= open && ms < open+roleInt(e, "seconds")*1000 {
		return "live"
	}
	return "queued"
}

func auctionBid(tx *StoreTx, e Record, roleID string, req auctionAction, ms int64) error {
	for i, v := range goodsListForField(e, "lots") {
		lot := v.(Record)
		if roleString(lot, "id") != req.LotID {
			continue
		}
		status := auctionLotStatus(e, lot, i, ms)
		if status != "preview" && status != "live" {
			return auctionRefusal("请在预展或该件举槌期间出价")
		}
		if roleString(lot, "seller") == roleID {
			return auctionRefusal("不能竞买自己的委托")
		}
		holder, oldMax := roleString(lot, "leader"), roleInt(lot, "max")
		step, price := roleInt(lot, "step"), roleInt(lot, "price")
		minimum := price
		if holder != "" {
			minimum = price + step
		}
		if holder == roleID {
			minimum = oldMax + 1
		}
		if req.Amount < minimum || req.Amount > 1_000_000_000 {
			return auctionRefusal(fmt.Sprintf("本次出价至少%d灵石，上限10亿", minimum))
		}
		actor := tx.Get(roleID)
		balance := roleInt(actor, "ShinyStone")
		debit := req.Amount
		if holder == roleID {
			debit -= oldMax
		}
		if balance < debit {
			return auctionRefusal("可用灵石不足以缴纳最高委托额")
		}
		switch {
		case holder == roleID:
			actor["ShinyStone"] = balance - debit
			lot["max"] = req.Amount
		case holder == "" || req.Amount > oldMax:
			if holder != "" {
				if err := refundPreviewAuctionBid(tx.Get(holder), oldMax); err != nil {
					return err
				}
				lot["price"] = min(req.Amount, oldMax+step)
			}
			actor["ShinyStone"] = balance - debit
			lot["max"], lot["leader"], lot["leaderName"] = req.Amount, roleID, roleString(actor, "Name")
		default:
			// Same maximum preserves the earlier leader. A losing bid's hold and
			// refund net to zero within this transaction.
			lot["price"] = min(oldMax, req.Amount+step)
		}
		return nil
	}
	return auctionRefusal("未找到本场拍品")
}

func auctionDeliveryGoods(entry Record) Record {
	if goods := auctionMap(entry, "item"); goods != nil {
		return goods
	}
	return entry // Earlier delivery records held the inventory item directly.
}

func auctionClaim(tx *StoreTx, roleID, id string) error {
	r := tx.Get(roleID)
	book := auctionMap(r, "AuctionDeliveries")
	entry := auctionMap(book, id)
	g := auctionDeliveryGoods(entry)
	if g == nil {
		return auctionRefusal("此件已领取或不在交割簿中")
	}
	if _, err := receiveInventoryGoods(r, g, transferStackCount(g)); err != nil {
		if errors.Is(err, previewReject(4527)) {
			return auctionRefusal("请先为纳戒腾出空间")
		}
		return err
	}
	delete(book, id)
	return nil
}

func auctionFingerprint(req auctionAction) string {
	req.ViewTag = "" // A changed read cursor does not change the order.
	b, _ := json.Marshal(req)
	return fmt.Sprintf("%x", sha256.Sum256(b))
}

func (s *Server) scheduledAuctionAction(roleID string, req auctionAction, now time.Time) (auctionView, error) {
	var empty auctionView
	if roleID == "" || reservedStoreID(roleID) {
		return empty, auctionRefusal("请先登入角色")
	}
	if _, ok := auctionDefaultRules[req.Tier]; !ok {
		return empty, auctionRefusal("未识别的拍卖场次")
	}
	if err := s.advanceAuctionTier(req.Tier, now); err != nil {
		return empty, err
	}
	if req.Action != "view" {
		if req.RequestID == "" || len(req.RequestID) > 80 || req.EventID == "" {
			return empty, auctionRefusal("本次委托凭据不完整，请刷新")
		}
		var err error
		if req.Action == "chat" {
			err = s.sendScheduledChat(roleID, req, now)
		} else {
			err = s.roles.Transact(func(tx *StoreTx) error {
				if tx.View(roleID) == nil {
					return auctionRefusal("角色不存在")
				}
				e := auctionMap(tx.View(scheduledAuctionStoreID), req.Tier)
				if req.EventID != roleString(e, "id") {
					return auctionRefusal("场次已更新，请重开拍册")
				}
				digest := auctionFingerprint(req)
				// Keep receipt histories on the owning role. A bid in a crowded
				// room should not clone every other participant's receipts.
				ledger := auctionMap(auctionMap(tx.View(roleID), "AuctionReceipts"), req.Tier)
				var receipts Record
				if roleString(ledger, "event") == req.EventID {
					receipts = auctionMap(ledger, "keys")
				}
				if previous := roleString(receipts, req.RequestID); previous != "" {
					if previous == digest {
						return nil
					}
					return auctionRefusal("委托凭据重复")
				}
				if len(receipts) >= 2048 {
					return auctionRefusal("本场委托笔数已满")
				}
				e = auctionMap(tx.Get(scheduledAuctionStoreID), req.Tier)
				var err error
				switch req.Action {
				case "submit":
					err = auctionSubmit(tx, e, roleID, req, now.UnixMilli())
				case "bid":
					err = auctionBid(tx, e, roleID, req, now.UnixMilli())
				case "service":
					err = buyAuctionService(tx, e, roleID, req.Service, now.UnixMilli())
				case "claim":
					err = auctionClaim(tx, roleID, req.LotID)
				default:
					return auctionRefusal("未识别的拍卖操作")
				}
				if err != nil {
					return err
				}
				ledger = auctionMapEdit(auctionMapEdit(tx.Get(roleID), "AuctionReceipts"), req.Tier)
				if roleString(ledger, "event") != req.EventID {
					ledger["event"] = req.EventID
					ledger["keys"] = Record{}
					delete(ledger, "bids")
				}
				if req.Action == "bid" {
					auctionMapEdit(ledger, "bids")[req.LotID] = req.Amount
				}
				auctionMapEdit(ledger, "keys")[req.RequestID] = digest
				return nil
			})
		}
		if err != nil {
			return empty, err
		}
	}
	return s.scheduledAuctionView(roleID, req.Tier, now)
}

func auctionGoodsRow(id string, g Record) auctionGoodsView {
	return auctionGoodsView{ID: id, Name: roleString(g, "Name"), TemplateID: roleString(g, "TemplateId"), BlankID: roleString(g, "BlankId"), Rank: roleInt(g, "Rank"), Kind: roleInt(g, "Type"), Count: transferStackCount(g)}
}

func (s *Server) scheduledAuctionView(roleID, tier string, now time.Time) (auctionView, error) {
	v := auctionView{Tier: tier, Now: now.UnixMilli(), Goods: []auctionGoodsView{}, Deliveries: []auctionDeliveryView{}, Messages: []auctionChatLine{}, Services: auctionServices}
	s.roles.mu.RLock()
	role := s.roles.records[roleID]
	if role == nil {
		s.roles.mu.RUnlock()
		return v, auctionRefusal("角色不存在")
	}
	e := auctionMap(s.roles.records[scheduledAuctionStoreID], tier)
	v.NextCollectAt = roleInt(e, "nextCollect")
	v.EventID, v.Phase = roleString(e, "id"), auctionPhase(e, v.Now)
	v.CollectAt, v.PreviewAt, v.LiveAt, v.EndsAt = roleInt(e, "collect"), roleInt(e, "preview"), roleInt(e, "live"), roleInt(e, "end")
	v.LotSeconds, v.Limit, v.Balance = roleInt(e, "seconds"), roleInt(e, "limit"), roleInt(role, "ShinyStone")
	ledger := auctionMap(auctionMap(role, "AuctionReceipts"), tier)
	var myBids Record
	if roleString(ledger, "event") == v.EventID {
		myBids = auctionMap(ledger, "bids")
	}
	rows := goodsListForField(e, "lots")
	capacity := len(rows)
	if v.Phase == "collect" {
		capacity = min(capacity, 3)
	}
	v.Lots = make([]auctionLotView, 0, capacity)
	for i, x := range rows {
		lot := x.(Record)
		mine := roleString(lot, "seller") == roleID
		if v.Phase == "collect" && !mine {
			continue
		}
		g := auctionLotView{auctionGoodsView: auctionGoodsRow(roleString(lot, "id"), auctionMap(lot, "goods")), Source: "system", Seller: "商会",
			Mine: mine, Leader: roleString(lot, "leaderName"), Leading: roleString(lot, "leader") == roleID,
			Start: roleInt(lot, "start"), Price: roleInt(lot, "price"), Step: roleInt(lot, "step"), Status: auctionLotStatus(e, lot, i, v.Now),
			OpensAt: v.LiveAt + int64(i)*v.LotSeconds*1000, EndsAt: v.LiveAt + int64(i+1)*v.LotSeconds*1000}
		if roleString(lot, "seller") != "" {
			g.Source, g.Seller = "player", roleString(lot, "sellerName")
		}
		g.LastBid = roleInt(myBids, g.ID)
		g.Participated = g.LastBid > 0
		if v.Phase == "collect" {
			g.OpensAt, g.EndsAt = 0, 0
		}
		if g.Leading {
			g.MyMax = roleInt(lot, "max")
		}
		v.Lots = append(v.Lots, g)
	}
	if v.Phase == "collect" {
		for _, x := range goodsList(role) {
			g, _ := x.(Record)
			if auctionEligible(g) {
				v.Goods = append(v.Goods, auctionGoodsRow(roleString(g, "GoodsId"), g))
			}
		}
	}
	for id, x := range auctionMap(role, "AuctionDeliveries") {
		entry := x.(Record)
		v.Deliveries = append(v.Deliveries, auctionDeliveryView{auctionGoodsRow(id, auctionDeliveryGoods(entry)), roleString(entry, "reason")})
	}
	slices.SortFunc(v.Deliveries, func(a, b auctionDeliveryView) int { return strings.Compare(a.ID, b.ID) })
	seat := auctionSeat(e, roleID)
	v.Seat = seat
	s.roles.mu.RUnlock()
	if v.Phase != "collect" {
		if err := s.readScheduledChat(&v, roleID); err != nil {
			return v, err
		}
	}
	return v, nil
}
