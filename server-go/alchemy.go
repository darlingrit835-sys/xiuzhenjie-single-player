package main

import (
	"crypto/sha256"
	"encoding/hex"
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"math/rand/v2"
)

const alchemyCommand = 4901050
const alchemyStorePrefix = "$alchemy:"
const brewSupplyPrice = 90

type brewRequest struct {
	Action        string   `json:"action"`
	RecipeID      string   `json:"recipeId"`
	AuxIDs        []string `json:"auxIds"`
	RequestID     string   `json:"requestId"`
	RunID         string   `json:"runId"`
	Taps          []int    `json:"taps"`
	ExpectedPrice int      `json:"expectedPrice"`
	Quantity      int      `json:"quantity,omitempty"`
}

func (s *Server) handleAlchemy(peer *Peer, msg Message) {
	code := 2000
	role := peer.currentRoleID()
	active, online := s.online.Load(role)
	var req brewRequest
	var reply Record
	var err error
	switch {
	case s.alchemy == nil || s.preview != nil && !s.previewWrites || role == "" || reservedStoreID(role) || !online || active != peer:
		err = forgeFailure("丹房尚未开放，或角色已离线")
	case len(msg.Data) > 2048 || json.Unmarshal([]byte(offerToken(msg.Data)), &req) != nil:
		err = forgeFailure("丹房请求格式有误")
	default:
		reply, err = s.brewAction(role, req)
	}
	if err != nil {
		var reason forgeFailure
		if errors.As(err, &reason) {
			code, reply = 4008, Record{"error": string(reason)}
		} else {
			log.Printf("保存丹炉: %v", err)
			code, reply = 5000, Record{"error": "丹炉未能保存，请重试"}
		}
	}
	data, _ := json.Marshal(reply)
	wire := offerNumber(nil, 1, uint64(code))
	if code == 2000 {
		wire = offerNumber(wire, 2, 1)
	}
	_ = peer.sendPayload(msg.Cmd, msg.Msid, offerString(wire, 3, string(data)))
}

// A saved run freezes its rules and draw at ignition. Reconnecting or clearing bag space never rerolls it.
type activeBrew struct {
	CatalogVersion            string
	Plan                      elixirPlan
	SuccessRoll, QuantityRoll int
}

func (w *alchemyRoom) inputCounts(p elixirPlan) map[string]int {
	r := w.Alchemy.Recipes[p.RecipeID]
	need := map[string]int{r.PrimaryID: w.primaryQty(r)}
	for _, id := range p.AuxIDs {
		need[id]++
	}
	return need
}

// Older battle drops encode kind, rank and level in the template ID. Resolve
// only those exact IDs through the catalog's legacy mapping for alchemy.
func (w *alchemyRoom) herbID(g Record) string {
	id := roleString(g, "TemplateId")
	kind := int(roleInt(g, "Type"))
	if legacy := w.LegacyMaterialTypes[kind]; legacy != "" && id == battleDropTemplateID(kind, int(roleInt(g, "Rank")), int(roleInt(g, "Level"))) {
		return legacy
	}
	return id
}

func (w *alchemyRoom) availableInputs(role Record, p elixirPlan) (map[string]int, error) {
	stock := w.inventoryFor(role, p)
	for _, n := range stock.missing {
		if n > 0 {
			return nil, forgeFailure("可用药材不足，请核对纳戒中的份数或解锁药材")
		}
	}
	return stock.ids, nil
}

func (w *alchemyRoom) quantity(p elixirPlan) int {
	r := w.Alchemy.Recipes[p.RecipeID]
	adjusted := (r.MinQty + rand.IntN(r.MaxQty-r.MinQty+1)) * p.YieldMultiplier
	n := adjusted / 10000
	if rand.IntN(10000) < adjusted%10000 {
		n++
	}
	return max(1, min(r.YieldCap, n))
}

func (s *Server) brewAction(owner string, req brewRequest) (Record, error) {
	w := s.alchemy
	if req.Action == "catalog" || req.Action == "plan" {
		fields := append([]string{"AlchemyRank", "AlchemyExperience", "AlchemyJournal", "CurrentVigor", "Vigor", "VigorRecoveredAt", "GongFaNaQi", "Goods", "EquippedGoods", "BattleMeridianBonuses", "Perception", "Physique", "Level", "SuperAir", "PureBlood", "ArtisanResearch"}, artisanCareerFields...)
		if req.Action == "catalog" {
			fields = append(fields, "AlchemyStarterClaimed")
		}
		role := s.roles.project(owner, fields)
		if role == nil {
			return nil, ErrRecordNotFound
		}
		if req.Action == "catalog" {
			return w.directory(role), nil
		}
		p, err := w.plan(role, req)
		if err != nil {
			return nil, err
		}
		rank, _, _ := elixirTraining(role)
		n, e := artisanBatchCount(req.Quantity, rank)
		if e != nil {
			return nil, e
		}
		return w.batchPlanView(p, role, n), nil
	}
	if !validOfferToken(req.RequestID) {
		return nil, forgeFailure("丹炉请求编号无效")
	}
	raw, _ := json.Marshal(req)
	digest := sha256.Sum256(raw)
	hash := hex.EncodeToString(digest[:])
	receiptID := alchemyStorePrefix + smithingTemplateID(owner, req.RequestID)
	var result Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		if saved := tx.View(receiptID); saved != nil {
			if roleString(saved, "hash") != hash {
				return forgeFailure("这道炉令已作他用，请重新起炉")
			}
			if req.Action == "supply" || req.Action == "buyMissing" {
				result = w.directory(tx.View(owner))
				return nil
			}
			if req.Action == "start" && roleString(tx.View(owner), "AlchemyRunID") != req.RequestID {
				return forgeFailure("旧炉已结束，请重新起炉")
			}
			return json.Unmarshal([]byte(roleString(saved, "answer")), &result)
		}
		role := tx.Get(owner)
		if role == nil {
			return ErrRecordNotFound
		}
		rank, _, _ := elixirTraining(role)
		count, err := artisanBatchCount(req.Quantity, rank)
		if err != nil {
			return err
		}
		if count != 1 && req.Action != "replay" && req.Action != "buyMissing" {
			return forgeFailure("只有留影复炼和配料支持多炉")
		}
		switch req.Action {
		case "buyMissing":
			if err := w.buyMissing(role, req); err != nil {
				return err
			}
			result = w.directory(role)
		case "supply":
			if err := w.supply(role, req.ExpectedPrice); err != nil {
				return err
			}
			result = w.directory(role)
		case "start", "replay":
			p, e := w.plan(role, req)
			if e != nil {
				return e
			}
			if req.Action == "start" {
				if forgeCurrentVigor(role) < 1 {
					return forgeFailure("本炉需要 1 精力")
				}
				if _, err := w.availableInputs(role, p); err != nil {
					return err
				}
			}
			if req.Action == "replay" {
				memory := recipeJournal(role, p.RecipeID)
				q := int(roleInt(memory, "Quality"))
				if q < 1 || q > 3 {
					return forgeFailure("此方尚无留影，须先手炼成丹")
				}
				p.Potency = int(roleInt(memory, "Potency"))
				result, err = w.replayBatch(role, p, q, count)
			} else {
				for i := range p.Stages {
					p.Stages[i].TargetBps = 1800 + rand.IntN(6800)
				}
				run := activeBrew{CatalogVersion: w.rules.Version(), Plan: p, SuccessRoll: rand.IntN(10000), QuantityRoll: w.quantity(p)}
				encoded, _ := json.Marshal(run)
				role["AlchemyRun"], role["AlchemyRunID"] = string(encoded), req.RequestID
				result = w.planView(p, role)
				result["runId"] = req.RequestID
			}
		case "finish":
			if !validOfferToken(req.RunID) || roleString(role, "AlchemyRunID") != req.RunID {
				return forgeFailure("这炉已结束或被替换，请返回丹房")
			}
			var run activeBrew
			if err := json.Unmarshal([]byte(roleString(role, "AlchemyRun")), &run); err != nil {
				return err
			}
			if run.CatalogVersion != w.rules.Version() {
				return forgeFailure("丹方已更新，请退出旧炉重新配药，药材仍在")
			}
			score, e := brewTapScore(run.Plan.Stages, req.Taps)
			if e != nil {
				return e
			}
			chance := w.chance(run.Plan, score)
			q := 0
			if run.SuccessRoll < chance {
				q = elixirQuality(run.Plan, score)
			}
			if !spendArtisanVigor(role, 1) {
				return forgeFailure("本炉需要 1 精力，药材仍在")
			}
			result, err = w.settle(role, run.Plan, score, chance, q, run.QuantityRoll, false)
			if err == nil {
				result["vigorSpent"], result["batches"] = 1, 1
				delete(role, "AlchemyRunID")
				delete(role, "AlchemyRun")
			}
		default:
			return forgeFailure("没有这项丹房操作")
		}
		if err != nil {
			return err
		}
		saved := tx.GetOrCreate(receiptID)
		saved["hash"] = hash
		if req.Action != "supply" && req.Action != "buyMissing" {
			encoded, _ := json.Marshal(result)
			saved["answer"] = string(encoded)
		}
		return nil
	})
	return result, err
}

// Every furnace pays its own ingredient lot and rolls its own yield. A failed
// inventory write aborts the enclosing role transaction, including vigor and XP.
func (w *alchemyRoom) replayBatch(role Record, p elixirPlan, quality, count int) (Record, error) {
	stock := w.batchInventory(role, p, count)
	for _, short := range stock.missing {
		if short > 0 {
			return nil, forgeFailure("本批药材不足，请先补齐整批缺材")
		}
	}
	cost := artisanCopyVigor(count, 1)
	if !spendArtisanVigor(role, cost) {
		return nil, forgeFailure(fmt.Sprintf("本批需要 %d 精力", cost))
	}
	var last Record
	total, lo, hi := 0, -1, 0
	for range count {
		out, err := w.settle(role, p, 100, 10000, quality, w.quantity(p), true)
		if err != nil {
			return nil, err
		}
		total += int(roleInt(out, "quantity"))
		price := int(roleInt(out, "sellPrice"))
		if lo < 0 || price < lo {
			lo = price
		}
		hi = max(hi, price)
		last = out
	}
	last["quantity"], last["batches"], last["vigorSpent"] = total, count, cost
	last["minSellPrice"], last["maxSellPrice"] = lo, hi
	last["message"] = fmt.Sprintf("留影复炼 %d 炉，获得%s%s ×%d，花费 %d 精力", count, elixirQualities[quality], roleString(last, "recipeName"), total, cost)
	return last, nil
}

func (w *alchemyRoom) supply(role Record, expectedPrice int) error {
	price := 0
	if roleBool(role, "AlchemyStarterClaimed") {
		price = brewSupplyPrice
	}
	if price != expectedPrice {
		return forgeFailure("领药价已变化，请刷新药柜")
	}
	if roleInt(role, "ShinyStone") < int64(price) {
		return forgeFailure("灵石不足，无法购药")
	}
	units := 0
	for _, line := range brewBundle {
		units += line.Quantity
	}
	for _, line := range brewBundle {
		if err := w.fillHerb(role, line.TemplateID, line.Quantity, price/units); err != nil {
			return err
		}
	}
	role["AlchemyStarterClaimed"], role["ShinyStone"] = true, roleInt(role, "ShinyStone")-int64(price)
	return nil
}

func (w *alchemyRoom) settle(role Record, p elixirPlan, score, chance, quality, quantity int, replay bool) (Record, error) {
	r := w.Alchemy.Recipes[p.RecipeID]
	stock := w.inventoryFor(role, p)
	for _, n := range stock.missing {
		if n > 0 {
			return nil, forgeFailure("可用药材不足，请核对纳戒中的份数或解锁药材")
		}
	}
	if err := consumeForgeInputs(role, stock.ids); err != nil {
		return nil, err
	}
	name := w.text.ItemTemplates[r.OutputID].Name
	effectName := w.effects[r.EffectID].Name
	effect, price := 0, 0
	effectText := ""
	if quality > 0 {
		price = w.actualPrice(p, stock, quality)
		effect = w.effect(p, quality)
		effectText = w.server.elixirEffectText(role, r.EffectID, r.EffectBasis, effectName, effect)
		item, _ := w.rules.Item(r.OutputID)
		grade, _ := w.rules.Grade(item.GradeID)
		g := Record{"TemplateId": r.OutputID, "Name": name + "·" + elixirQualities[quality], "Type": 19, "Rank": (grade.TierID-1)*4 + quality, "Level": 1, "FiveElement": w.element(r.OutputID),
			"Volume": 0.1, "ConversionRate": 1.0, "RecyclePrice": price, "IsUsing": false, "IsBinding": false, "IsLock": false,
			"CreatedBy": roleString(role, "Name"), "BrewEffect": r.EffectID, "EffectType": brewEffectTypes[r.EffectID], "EffectValue": effect, "RealityEffectValue": effect,
			"BrewQuality": quality, "BrewRecipe": p.RecipeID}
		if r.PoisonCost > 0 {
			g["PoisonValue"] = r.PoisonCost
		}
		if r.EffectBasis != "flat" {
			g["BrewBasis"] = r.EffectBasis
		}
		if _, err := receiveInventoryGoods(role, g, int64(quantity)); err != nil {
			return nil, forgeFailure("纳戒空间不足，请腾出位置再开炉。药材仍在")
		}
	} else {
		quantity = 0
	}
	journal, _ := role["AlchemyJournal"].(Record)
	if journal == nil {
		journal = Record{}
		role["AlchemyJournal"] = journal
	}
	row, _ := journal[p.RecipeID].(Record)
	if row == nil {
		row = Record{}
		journal[p.RecipeID] = row
	}
	msg := "火候散去，未能成丹"
	if replay {
		row["Mastery"] = min(100, int(roleInt(row, "Mastery"))+2)
		w.trainWeighted(role, r.MinRank, true, 50)
	} else {
		progress := 2
		if quality > 0 {
			progress = 6
		}
		row["Mastery"] = min(100, int(roleInt(row, "Mastery"))+progress)
		w.train(role, r.MinRank, quality > 0)
		if quality > int(roleInt(row, "Quality")) {
			row["Quality"], row["AuxIDs"], row["Potency"] = quality, p.AuxIDs, p.Potency
		}
	}
	if quality > 0 {
		msg = fmt.Sprintf("%s%s ×%d · 每粒%s", elixirQualities[quality], name, quantity, effectText)
	}
	return Record{"poisonCost": r.PoisonCost, "sellPrice": price, "effectBasis": w.server.brewDisplayBasis(role, r, effect), "effectText": effectText, "recipeName": name, "memoryQuality": elixirQualities[roleInt(row, "Quality")], "brewed": quality > 0, "quality": elixirQualities[quality], "effect": w.server.brewDisplayValue(role, r, effect), "effectName": effectName, "quantity": quantity,
		"successBps": chance, "score": score, "mastery": roleInt(row, "Mastery"), "message": msg}, nil
}
