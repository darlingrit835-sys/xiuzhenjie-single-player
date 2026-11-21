package main

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"strings"
)

const talismanCommand = 4901060
const talismanStorePrefix = "$talisman:"
const legacyHasteTalisman = "preview-sign"

type sigilRequest struct {
	Action        string        `json:"action"`
	RequestID     string        `json:"requestId"`
	Effect        string        `json:"effect"`
	Seal          string        `json:"seal"`
	Recipe        string        `json:"recipe"`
	ExpectedPrice int           `json:"expectedPrice"`
	Quantity      int           `json:"quantity,omitempty"`
	GoodsID       string        `json:"goodsId"`
	Strokes       []sigilStroke `json:"strokes"`
}

func (s *Server) handleTalisman(peer *Peer, msg Message) {
	role := peer.currentRoleID()
	active, online := s.online.Load(role)
	var req sigilRequest
	var reply Record
	var err error
	switch {
	case role == "" || reservedStoreID(role) || !online || active != peer:
		err = forgeFailure("请先登录角色")
	case len(msg.Data) > 8192 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil:
		err = forgeFailure("符令格式有误")
	case s.preview != nil && !s.previewWrites && req.Action != "catalog" && req.Action != "plan":
		err = forgeFailure("当前为只读符案")
	default:
		reply, err = s.sigilAction(role, req)
	}
	code := 2000
	if err != nil {
		var reason forgeFailure
		if errors.As(err, &reason) {
			code, reply = 4008, Record{"error": string(reason)}
		} else {
			log.Printf("保存符案: %v", err)
			code, reply = 5000, Record{"error": "未能读取或保存符案，请重新连接"}
		}
	}
	sendAuctionJSON(peer, msg, code, reply)
}

func sigilSupplyPrice(role Record) int {
	if roleBool(role, "TalismanStarter") {
		return 36
	}
	return 0
}
func sigilGoodsChoice(g Record) (sigilChoice, bool) {
	if roleInt(g, "Type") != 21 {
		return sigilChoice{}, false
	}
	if roleString(g, "TemplateId") == legacyHasteTalisman {
		choice, err := sigilSelect("haste", "simple")
		return choice, err == nil
	}
	if roleInt(g, "SigilPower") <= 0 {
		return sigilChoice{}, false
	}
	parts := strings.Split(roleString(g, "TemplateId"), ".")
	if len(parts) != 3 || parts[0] != "sigil" || parts[1] == "" || parts[2] == "" {
		return sigilChoice{}, false
	}
	c, err := sigilSelect(parts[1], parts[2])
	return c, err == nil
}
func (c sigilChoice) itemPower(role, goods Record) int {
	if roleString(goods, "TemplateId") == legacyHasteTalisman {
		return c.effect.base
	}
	return c.usablePower(role, int(roleInt(goods, "SigilPower")))
}

func preparedDungeonHaste(r Record) int64 {
	return max(0, min(35, roleInt(r, "TalismanDungeonHaste")))
}

func consumeDungeonHaste(r Record) int64 {
	bonus := preparedDungeonHaste(r)
	delete(r, "TalismanDungeonHaste")
	return bonus
}

func sigilIsUsable(g Record) bool {
	return !inventoryFlag(g, "IsLock") && !inventoryFlag(g, "IsUsing") && !hasFriendOfferLock(g) && transferStackCount(g) > 0
}
func sigilCounts(role Record) map[string]int {
	counts := map[string]int{}
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		if forgeUsable(g) {
			counts[roleString(g, "TemplateId")] += int(transferStackCount(g))
		}
	}
	return counts
}
func sigilHasBrush(role Record) bool {
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		// Locked or bound brushes can still be held. A brush promised in trade cannot.
		if roleString(g, "TemplateId") == sigilBrush && roleInt(g, "Type") == 22 && transferStackCount(g) > 0 && !hasFriendOfferLock(g) {
			return true
		}
	}
	return false
}
func sigilCanPay(counts map[string]int, inputs []sigilUnit) bool {
	for _, line := range inputs {
		if counts[line.id] < line.n {
			return false
		}
	}
	return true
}
func (r sigilRecipe) available(role Record, counts map[string]int) bool {
	return roleInt(role, "ShinyStone") >= int64(r.stones) && sigilCanPay(counts, r.inputs)
}
func sigilTolerance(role Record, brush bool) int {
	rank, _, _ := sigilTraining(role)
	value := 10 + rank/2
	if brush {
		value += 6
	}
	return value
}

func (c sigilChoice) usablePower(role Record, power int) int {
	if c.effect.id == "ward" {
		return int(artisanWard(role, int64(power), 1250))
	}
	if c.effect.id == "haste" {
		return min(power, 35)
	}
	return power
}

func sigilDirectory(role Record, c sigilChoice, message string) Record {
	counts := sigilCounts(role)
	rank, exp, _ := sigilTraining(role)
	brush := sigilHasBrush(role)
	brushName, tier := "云木符笔", 1
	if brush {
		brushName, tier = "金翎符笔", 2
	}
	recipes := make([]Record, 0, len(sigilRecipes))
	for _, r := range sigilRecipes {
		cost := sigilUnitText(r.inputs)
		if r.stones > 0 {
			cost = fmt.Sprintf("%d 灵石", r.stones)
		}
		maxSets := artisanCopyLimit(rank)
		for _, input := range r.inputs {
			maxSets = min(maxSets, counts[input.id]/input.n)
		}
		if r.stones > 0 {
			maxSets = min(maxSets, int(roleInt(role, "ShinyStone")/int64(r.stones)))
		}
		recipes = append(recipes, Record{"maxSets": maxSets, "id": r.id, "name": r.name, "cost": cost, "output": sigilUnitText(r.outputs), "available": r.available(role, counts)})
	}
	materials := make([]Record, 0, len(sigilMaterials))
	for _, m := range sigilMaterials {
		materials = append(materials, Record{"id": m.id, "name": m.name, "source": m.source, "count": counts[m.id]})
	}
	effects := make([]Record, 0, len(sigilEffects))
	for _, e := range sigilEffects {
		effects = append(effects, Record{"id": e.id, "name": e.name, "description": e.description})
	}
	goods := []Record{}
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		if item, ok := sigilGoodsChoice(g); ok {
			goods = append(goods, Record{"id": roleString(g, "GoodsId"), "templateId": roleString(g, "TemplateId"), "name": roleString(g, "Name"), "count": transferStackCount(g), "usable": sigilIsUsable(g), "description": item.detail(item.itemPower(role, g))})
		}
	}
	power := c.usablePower(role, c.power(role, brush))
	plan := Record{"mastery": sigilMastery(role, c.key()), "effect": c.effect.id, "seal": c.seal, "name": c.name(), "description": c.detail(power), "cost": sigilUnitText(c.inputs()), "power": power, "canDraw": sigilCanPay(counts, c.inputs()), "learned": roleBool(farmMap(role, "TalismanPatterns"), c.key()), "tolerance": sigilTolerance(role, brush), "strokes": c.strokes()}
	realm := sigilRealm(role)
	return Record{"batchLimit": artisanCopyLimit(rank), "vigor": forgeCurrentVigor(role), "level": max(1, roleInt(role, "Level")), "realm": sigilRealms[realm], "realmTier": realm + 1, "rank": rank, "training": exp, "nextRank": sigilRanks[rank], "brush": brushName, "brushTier": tier,
		"stones": roleInt(role, "ShinyStone"), "supplyPrice": sigilSupplyPrice(role), "supplyText": sigilUnitText(sigilStarter), "ward": roleInt(role, "TalismanFieldWard"), "battleWard": roleInt(role, "AlchemyWard"), "battleHaste": preparedDungeonHaste(role), "poison": roleInt(role, "CurrentPoisonVal"),
		"effects": effects, "seals": sigilSeals, "recipes": recipes, "materials": materials, "goods": goods, "plan": plan, "message": message}
}

func (s *Server) sigilAction(owner string, req sigilRequest) (Record, error) {
	c, err := sigilSelect(req.Effect, req.Seal)
	if err != nil {
		return nil, err
	}
	if req.Action == "catalog" || req.Action == "plan" {
		r := s.roles.Get(owner)
		if r == nil {
			return nil, ErrRecordNotFound
		}
		return sigilDirectory(r, c, ""), nil
	}
	if !validOfferToken(req.RequestID) {
		return nil, forgeFailure("符令编号无效")
	}
	raw, _ := json.Marshal(req)
	digest := sha256.Sum256(raw)
	hash := hex.EncodeToString(digest[:])
	id := talismanStorePrefix + smithingTemplateID(owner, req.RequestID)
	var reply Record
	err = s.roles.Transact(func(tx *StoreTx) error {
		if tx.View(owner) == nil {
			return ErrRecordNotFound
		}
		if receipt := tx.View(id); receipt != nil {
			if roleString(receipt, "Hash") != hash {
				return forgeFailure("此符令已用于其他操作，请重新提交")
			}
			reply = sigilDirectory(tx.View(owner), c, roleString(receipt, "Message"))
			return nil
		}
		r := tx.Get(owner)
		message, err := sigilMutate(r, c, req)
		if err != nil {
			return err
		}
		receipt := tx.GetOrCreate(id)
		receipt["Hash"], receipt["Message"] = hash, message
		reply = sigilDirectory(r, c, message)
		return nil
	})
	return reply, err
}

func sigilReceive(r Record, lines []sigilUnit) error { return sigilReceivePaid(r, lines, 0) }
func sigilReceivePaid(r Record, lines []sigilUnit, price int) error {
	units := 0
	for _, line := range lines {
		units += line.n
	}
	for _, line := range lines {
		m := sigilMat(line.id)
		item := Record{"TemplateId": m.id, "Name": m.name, "Type": m.kind, "Rank": m.rank, "Level": 1, "Stage": 1, "FiveElement": m.element, "Volume": 0.1, "RecyclePrice": 0, "ConversionRate": 1.0, "IsUsing": false, "IsBinding": false, "IsLock": false}
		if line.id == sigilReed || line.id == sigilHerb || line.id == sigilDew {
			item["BrewPaidUnit"] = price / units
		}
		if _, err := receiveInventoryGoods(r, item, int64(line.n)); err != nil {
			return forgeFailure("纳戒空间不足，请腾出位置。原料仍在")
		}
	}
	return nil
}
func sigilConsume(r Record, lines []sigilUnit) error {
	inputs := make(map[string]int, len(lines))
	for _, line := range lines {
		inputs[line.id] = line.n
	}
	return consumeForgeInputs(r, inputs)
}
func sigilMutate(r Record, c sigilChoice, req sigilRequest) (string, error) {
	rank, _, _ := sigilTraining(r)
	count, err := artisanBatchCount(req.Quantity, rank)
	if err != nil {
		return "", err
	}
	if count != 1 && req.Action != "copy" && req.Action != "make" && req.Action != "supply" {
		return "", forgeFailure("只有符谱复写、备料或领料支持批量")
	}
	switch req.Action {
	case "supply":
		price := sigilSupplyPrice(r) + (count-1)*36
		if price != req.ExpectedPrice {
			return "", forgeFailure("料包价格已变，请重新查看")
		}
		if roleInt(r, "ShinyStone") < int64(price) {
			return "", forgeFailure("灵石不足，无法购入料包")
		}
		if err := sigilReceivePaid(r, sigilMultiply(sigilStarter, count), price); err != nil {
			return "", err
		}
		r["TalismanStarter"], r["ShinyStone"] = true, roleInt(r, "ShinyStone")-int64(price)
		return "已入纳戒：" + sigilUnitText(sigilMultiply(sigilStarter, count)), nil
	case "make":
		for _, recipe := range sigilRecipes {
			if recipe.id != req.Recipe {
				continue
			}
			if (recipe.id == "brushKit" || recipe.id == "brush") && count != 1 {
				return "", forgeFailure("符笔与笔料按支订制")
			}
			recipe.inputs = sigilMultiply(recipe.inputs, count)
			recipe.outputs = sigilMultiply(recipe.outputs, count)
			recipe.stones *= count
			if !recipe.available(r, sigilCounts(r)) {
				return "", forgeFailure("原料或灵石不足，请核对可用份数")
			}
			if err := sigilConsume(r, recipe.inputs); err != nil {
				return "", err
			}
			if err := sigilReceive(r, recipe.outputs); err != nil {
				return "", err
			}
			r["ShinyStone"] = roleInt(r, "ShinyStone") - int64(recipe.stones)
			return "已入纳戒：" + sigilUnitText(recipe.outputs), nil
		}
		return "", forgeFailure("没有这项制材配方")
	case "draw", "copy":
		brush := sigilHasBrush(r)
		known := farmMap(r, "TalismanPatterns")
		if req.Action == "copy" {
			if !roleBool(known, c.key()) {
				return "", forgeFailure("请先在纸上完成此式，再用符谱复写")
			}
		} else if err := c.validate(req.Strokes, sigilTolerance(r, brush)); err != nil {
			return "", err
		}
		unit := 1
		if c.seal == "strong" {
			unit = 2
		}
		cost := int64(unit)
		if req.Action == "copy" {
			cost = artisanCopyVigor(count, unit)
		}
		if !spendArtisanVigor(r, cost) {
			return "", forgeFailure(fmt.Sprintf("本次制符需要 %d 精力", cost))
		}
		if err := sigilConsume(r, sigilMultiply(c.inputs(), count)); err != nil {
			return "", err
		}
		power := c.power(r, brush)
		item := Record{"TemplateId": c.key(), "Name": c.name(), "Type": 21, "Rank": max(1, rank)*4 - 3, "Level": 1, "Stage": 1, "FiveElement": c.effect.element, "Volume": 0.1, "ConversionRate": 1.0, "RecyclePrice": 0,
			"IsUsing": false, "IsBinding": false, "IsLock": false, "CreatedBy": roleString(r, "Name"), "SigilPower": power}
		if _, err := receiveInventoryGoods(r, item, int64(count)); err != nil {
			return "", forgeFailure("纳戒空间不足，请腾出位置再收符。符材仍在")
		}
		if req.Action == "draw" {
			if known == nil {
				known = Record{}
				r["TalismanPatterns"] = known
			}
			known[c.key()] = true
			sigilTrain(r)
			sigilPractice(r, c.key(), 12)
		} else {
			for range count {
				artisanAddLesson(r, 3, "TalismanRank", "TalismanExperience", 50)
			}
			sigilPractice(r, c.key(), 2*count)
		}
		return fmt.Sprintf("制成%s ×%d，耗 %d 精力 · %s", c.name(), count, cost, c.detail(c.usablePower(r, power))), nil
	case "use":
		return useInscribedTalisman(r, locateGoods(r, req.GoodsID))
	default:
		return "", forgeFailure("没有这项符案操作")
	}
}

func useInscribedTalisman(r, g Record) (string, error) {
	c, ok := sigilGoodsChoice(g)
	if !ok || !sigilIsUsable(g) {
		return "", forgeFailure("符箓不可用，请核对纳戒与锁定状态")
	}
	power := int64(c.itemPower(r, g))
	var message string
	switch c.effect.id {
	case "ward":
		if roleInt(r, "AlchemyWard") >= power {
			return "", forgeFailure("已有同等或更强护盾，此符仍在")
		}
		r["AlchemyWard"] = power
		message = fmt.Sprintf("已备下 %d 点护盾，下次进入历练时生效", power)
	case "cleanse":
		current := roleInt(r, "CurrentPoisonVal")
		if current <= 0 {
			return "", forgeFailure("当前没有毒性，此符仍在")
		}
		cleared := min(power, current)
		r["CurrentPoisonVal"] = current - cleared
		message = fmt.Sprintf("消去 %d 点毒性，余毒 %d 点", cleared, current-cleared)
	case "field":
		current := roleInt(r, "TalismanFieldWard")
		if current >= 3 {
			return "", forgeFailure("灵田已满三重护持，此符仍在")
		}
		r["TalismanFieldWard"] = min(3, current+power)
		message = fmt.Sprintf("已为灵田加护，现有 %d 重", roleInt(r, "TalismanFieldWard"))
	case "haste":
		if preparedDungeonHaste(r) >= power {
			return "", forgeFailure("已备下同等或更强的轻身符，此符仍在")
		}
		r["TalismanDungeonHaste"] = power
		message = fmt.Sprintf("已备轻身符，下次历练全场移动速度 +%d%%", power)
	}
	removeGoods(r, roleString(g, "GoodsId"))
	return message, nil
}
