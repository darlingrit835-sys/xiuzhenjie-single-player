package main

import (
	"fmt"
	"maps"
	"slices"
	"strconv"
	"strings"
)

type herbSources struct {
	Gather, Drop [][2]int
	LegacyTypes  []int
}
type brewLocale struct {
	Alchemy                                struct{ Effects map[string]smithingLabel }
	Regions, MonsterSpecies, GatherMethods map[string]smithingLabel
}
type herbTerms struct{ Value, BuyPrice int }
type brewEconomy struct {
	Materials map[string]herbTerms
	Recipes   map[string]struct{ SellPrices []int }
}

var brewBundle = []forgeInput{{"mat.ninglu_herb", 8}, {"mat.cold_dew", 3}, {"mat.wild_herb", 2}, {"mat.ember_moss", 2}, {"mat.wind_slate", 2}}

func (w *alchemyRoom) materialTerms(id string) herbTerms {
	if w.Alchemy.Economy == nil {
		return herbTerms{}
	}
	return w.Alchemy.Economy.Materials[id]
}
func (w *alchemyRoom) recipePrices(id string) []int {
	if w.Alchemy.Economy == nil {
		return []int{0, 0, 0}
	}
	return w.Alchemy.Economy.Recipes[id].SellPrices
}
func (w *alchemyRoom) validateEconomy() error {
	if w.Alchemy.Economy == nil {
		return nil
	} // Older exports still allow brewing with their original zero resale.
	for _, id := range w.materials {
		t, ok := w.Alchemy.Economy.Materials[id]
		if !ok || t.BuyPrice < 0 || t.Value < 1 || t.Value > 10000000 || t.BuyPrice > 10000000 {
			return fmt.Errorf("药材 %s 缺少有效估值或药柜单价", id)
		}
	}
	for _, id := range w.recipes {
		r := w.Alchemy.Recipes[id]
		p := w.recipePrices(id)
		if len(p) != 3 || p[0] < 0 || p[1] < p[0] || p[2] < p[1] || p[2] > 10000000 {
			return fmt.Errorf("丹方 %s 须有三档递增回收单价", id)
		}
		if buy := w.materialTerms(r.PrimaryID).BuyPrice; buy > 0 && int64(p[2])*int64(r.YieldCap) > int64(buy)*int64(w.primaryQty(r)) {
			return fmt.Errorf("丹方 %s 上品最大出丹回收超过单购主药成本", id)
		}
	}
	return nil
}

func (w *alchemyRoom) sourceText(id string) string {
	var parts []string
	if w.materialTerms(id).BuyPrice > 0 {
		parts = append(parts, "丹房药柜补缺")
	}
	for _, line := range brewBundle {
		if line.TemplateID == id {
			parts = append(parts, "丹房药包")
			break
		}
	}
	for _, seed := range gardenSeeds {
		if seed.material == id {
			parts = append(parts, "灵田种植")
			break
		}
	}
	for _, line := range sigilStarter {
		if line.id == id {
			parts = append(parts, "符案料包")
			break
		}
	}
	if id == sigilDew {
		parts = append(parts, "符案凝露")
	}
	src := w.ResourceSources[id]
	var places, beasts []string
	for _, pair := range src.Gather {
		if pair[1] != 1 && pair[1] != 2 {
			continue
		}
		name := w.locale.Regions[strconv.Itoa(pair[0])].Name + w.locale.GatherMethods[strconv.Itoa(pair[1])].Name
		if !slices.Contains(places, name) {
			places = append(places, name)
		}
	}
	for _, pair := range src.Drop {
		name := w.locale.MonsterSpecies[strconv.Itoa(pair[0])].Name
		if pair[1] != 0 {
			name += "（" + w.locale.Regions[strconv.Itoa(pair[1])].Name + "）"
		}
		if name != "" && !slices.Contains(beasts, name) {
			beasts = append(beasts, name)
		}
	}
	parts = append(parts, places...)
	if len(beasts) > 0 {
		parts = append(parts, "猎获 "+strings.Join(beasts, "、"))
	}
	if len(src.LegacyTypes) > 0 {
		parts = append(parts, "历练通用掉落")
	}
	return strings.Join(parts, "、")
}

func (w *alchemyRoom) fillHerb(role Record, id string, n, paidUnit int) error {
	item, ok := w.rules.Item(id)
	if !ok {
		return forgeFailure("药材未登记")
	}
	kind := 12
	switch item.CategoryID {
	case "mat.ore":
		kind = 13
	case "mat.blood":
		kind = 9
	case "mat.core":
		kind = 11
	case "mat.bone":
		kind = 10
	case "mat.beast":
		kind = 8
	}
	g := Record{"TemplateId": id, "Name": w.text.ItemTemplates[id].Name, "Type": kind, "Rank": item.GradeID, "Level": 1, "Stage": 1, "FiveElement": w.element(id),
		"Volume": 0.1, "ConversionRate": 1.0, "RecyclePrice": 0, "BrewPaidUnit": paidUnit, "IsUsing": false, "IsBinding": false, "IsLock": false}
	if _, err := receiveInventoryGoods(role, g, int64(n)); err != nil {
		return forgeFailure("纳戒空间不足，药材与灵石均未扣除")
	}
	return nil
}

// Legacy kits predate purchase-basis tracking. Only their zero-recycle material
// types need a conservative cap. Harvests and priced drops retain normal value.
func (w *alchemyRoom) primaryPaidUnit(g Record) (int, bool) {
	if g["BrewPaidUnit"] != nil {
		return max(0, int(roleInt(g, "BrewPaidUnit"))), true
	}
	if roleString(g, "BrewSource") == "garden" || roleInt(g, "RecyclePrice") > 0 {
		return 0, false
	}
	id := w.herbID(g)
	for _, line := range brewBundle {
		if line.TemplateID == id {
			return 0, true
		}
	}
	for _, line := range sigilStarter {
		if line.id == id {
			return 0, true
		}
	}
	for _, line := range forgeSupplyBundle {
		if line.id == id {
			return 0, true
		}
	}
	return 0, false
}

type brewInventory struct {
	ids, missing               map[string]int
	normalPrimary, paidPrimary int
}

func (w *alchemyRoom) inventoryFor(role Record, p elixirPlan) brewInventory {
	return w.batchInventory(role, p, 1)
}

func (w *alchemyRoom) batchInventory(role Record, p elixirPlan, count int) brewInventory {
	plan := brewInventory{ids: map[string]int{}, missing: w.inputCounts(p)}
	for id, units := range plan.missing {
		plan.missing[id] = units * count
	}
	primary := w.Alchemy.Recipes[p.RecipeID].PrimaryID
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		id := w.herbID(g)
		if plan.missing[id] == 0 || !forgeUsable(g) {
			continue
		}
		n := int(min(int64(plan.missing[id]), transferStackCount(g)))
		plan.ids[roleString(g, "TemplateId")] += n
		plan.missing[id] -= n
		if id == primary {
			if unit, capped := w.primaryPaidUnit(g); capped {
				plan.paidPrimary += unit * n
			} else {
				plan.normalPrimary += n
			}
		}
	}
	return plan
}
func (w *alchemyRoom) actualPrice(p elixirPlan, stock brewInventory, quality int) int {
	r := w.Alchemy.Recipes[p.RecipeID]
	nominal := w.recipePrices(p.RecipeID)[quality-1]
	need := w.primaryQty(r)
	// A preview includes the cost of missing primary units at the counter.
	paid := stock.paidPrimary + stock.missing[r.PrimaryID]*w.materialTerms(r.PrimaryID).BuyPrice
	normal := stock.normalPrimary
	if w.materialTerms(r.PrimaryID).BuyPrice == 0 {
		normal += stock.missing[r.PrimaryID]
	}
	return min(nominal, (nominal*normal*r.YieldCap+paid*need)/(need*r.YieldCap))
}
func (w *alchemyRoom) missingCost(stock brewInventory) (cost int, canBuy bool, text string) {
	canBuy = true
	var lines []string
	for _, id := range slices.Sorted(maps.Keys(stock.missing)) {
		n := stock.missing[id]
		if n == 0 {
			continue
		}
		price := w.materialTerms(id).BuyPrice
		label := fmt.Sprintf("%s×%d", w.text.ItemTemplates[id].Name, n)
		if price == 0 {
			canBuy = false
			label += "（需寻材）"
		} else {
			cost += price * n
		}
		lines = append(lines, label)
	}
	if len(lines) == 0 {
		return 0, false, "本炉材料已齐"
	}
	if !canBuy {
		cost = 0
	}
	return cost, canBuy, strings.Join(lines, "、")
}
func (w *alchemyRoom) quote(role Record, p elixirPlan, q int) Record {
	stock := w.inventoryFor(role, p)
	cost, canBuy, short := w.missingCost(stock)
	price := w.actualPrice(p, stock, q)
	nominal := w.recipePrices(p.RecipeID)[q-1]
	text := fmt.Sprintf("%s每粒回收 %d 灵石", elixirQualities[q], price)
	if price < nominal {
		text += fmt.Sprintf("（常规 %d，按主药来源折算）", nominal)
	}
	return Record{"shortageCost": cost, "canBuyMissing": canBuy, "shortageText": short, "sellPrice": price, "priceText": text}
}
func (w *alchemyRoom) buyMissing(role Record, req brewRequest) error {
	p, err := w.plan(role, req)
	if err != nil {
		return err
	}
	rank, _, _ := elixirTraining(role)
	count, err := artisanBatchCount(req.Quantity, rank)
	if err != nil {
		return err
	}
	stock := w.batchInventory(role, p, count)
	cost, canBuy, _ := w.missingCost(stock)
	if !canBuy {
		return forgeFailure("本炉已备齐或仍有须外出寻得的材料")
	}
	if cost != req.ExpectedPrice {
		return forgeFailure("药材缺口或售价已变化，请重新估炉")
	}
	if roleInt(role, "ShinyStone") < int64(cost) {
		return forgeFailure("灵石不足，无法补齐本炉药材")
	}
	for _, id := range slices.Sorted(maps.Keys(stock.missing)) {
		if n := stock.missing[id]; n > 0 {
			if err = w.fillHerb(role, id, n, w.materialTerms(id).BuyPrice); err != nil {
				return err
			}
		}
	}
	role["ShinyStone"] = roleInt(role, "ShinyStone") - int64(cost)
	return nil
}
