package main

import (
	"encoding/json"
	"fmt"
	"maps"
	"os"
	"path/filepath"
	"slices"
	"strconv"
)

type elixirRecipe struct {
	OutputID, PrimaryID, EffectID, EffectBasis                            string
	MinRank, SuccessBps, MinQty, MaxQty, YieldCap, BaseEffect, PoisonCost int
}

type elixirModifier struct{ SuccessBps, YieldBps, PotencyBps int }

func (m elixirModifier) scaled(n, d int) elixirModifier {
	return elixirModifier{m.SuccessBps * n / d, m.YieldBps * n / d, m.PotencyBps * n / d}
}
func (m elixirModifier) plus(b elixirModifier) elixirModifier {
	return elixirModifier{m.SuccessBps + b.SuccessBps, m.YieldBps + b.YieldBps, m.PotencyBps + b.PotencyBps}
}

type alchemyRoom struct {
	*smithingWorkshop
	Elements            map[int]elementCycle
	SystemResources     map[string][]string
	LegacyMaterialTypes map[int]string
	ResourceSources     map[string]herbSources
	locale              brewLocale
	server              *Server
	Alchemy             struct {
		Economy           *brewEconomy
		Recipes           map[string]elixirRecipe
		Ranks             map[int]elixirModifier
		Relations, Flames map[string]elixirModifier
		PotencyByQuality  map[int][2]int
		Limits            struct{ PrimaryWeightBps, SuccessMaxBps, YieldMultiplierMinBps, YieldMultiplierMaxBps int }
	}
	rankNames          map[int]string
	rankExp            map[int]int
	effects            map[string]smithingLabel
	recipes, materials []string
}

func (s *Server) loadAlchemy() error {
	if s.smithing == nil {
		return nil
	}
	w := &alchemyRoom{server: s, smithingWorkshop: s.smithing, rankExp: map[int]int{}, rankNames: map[int]string{}}
	read := func(name string, v any) error {
		data, err := os.ReadFile(filepath.Join(s.dataDir, "config", name))
		if err != nil {
			return err
		}
		return json.Unmarshal(data, v)
	}
	if err := read("cultivation_affinities.json", w); err != nil {
		return err
	}
	if len(w.Alchemy.Recipes) == 0 {
		return nil
	} // Installation predates the alchemy catalog.
	if err := read("cultivation_affinities.zh-CN.json", &w.locale); err != nil {
		return err
	}
	w.effects = w.locale.Alchemy.Effects
	var ranks []struct {
		ID                     int
		Name                   string
		PracticeElixirTopValue float64
	}
	if err := read("xzjworldassesselixirlist.json", &ranks); err != nil {
		return err
	}
	for _, r := range ranks {
		w.rankNames[r.ID] = r.Name
		w.rankExp[r.ID] = int(r.PracticeElixirTopValue)
	}
	for i := 0; i <= 9; i++ {
		if w.rankNames[i] == "" || i > 0 && w.rankExp[i] < 1 {
			return fmt.Errorf("丹师等级 %d 配置不完整", i)
		}
	}
	w.materials = w.SystemResources["alchemy"]
	slices.Sort(w.materials)
	w.recipes = slices.Sorted(maps.Keys(w.Alchemy.Recipes))
	slices.SortStableFunc(w.recipes, func(a, b string) int { return w.Alchemy.Recipes[a].MinRank - w.Alchemy.Recipes[b].MinRank })
	for _, id := range w.recipes {
		r := w.Alchemy.Recipes[id]
		if r.EffectBasis == "" {
			r.EffectBasis = "flat"
			w.Alchemy.Recipes[id] = r
		}
		if !validElixirBasis(r) || brewEffectTypes[r.EffectID] == 0 {
			return fmt.Errorf("丹方 %s 药效或计量类型无效", id)
		}
		if !slices.Contains(w.materials, r.PrimaryID) || w.primaryQty(r) < 1 || w.text.ItemTemplates[r.OutputID].Name == "" || w.effects[r.EffectID].Name == "" || r.MinQty < 1 || r.MaxQty < r.MinQty || r.YieldCap < r.MaxQty {
			return fmt.Errorf("丹方 %s 配置不完整", id)
		}
	}
	if err := w.validateEconomy(); err != nil {
		return err
	}
	s.alchemy = w
	return nil
}

func (w *alchemyRoom) primaryQty(r elixirRecipe) int {
	for id, n := range w.rules.Inputs(r.OutputID) {
		if id == r.PrimaryID {
			return n
		}
	}
	return 0
}

func elixirTraining(role Record) (int, int, string) {
	return artisanTraining(role, 1, "AlchemyRank", "AlchemyExperience")
}

func (w *alchemyRoom) advance(rank, exp, gain int, success bool) (int, int) {
	if rank == 0 && success {
		rank = 1
	}
	exp += gain
	for rank > 0 && rank < 9 && exp >= w.rankExp[rank] {
		exp -= w.rankExp[rank]
		rank++
	}
	if rank == 9 {
		exp = min(exp, w.rankExp[rank])
	}
	return rank, exp
}

func (w *alchemyRoom) train(role Record, recipeRank int, success bool) {
	w.trainWeighted(role, recipeRank, success, 100)
}

func (w *alchemyRoom) trainWeighted(role Record, recipeRank int, success bool, weight int) {
	rank, exp, slot := elixirTraining(role)
	earn := func(current int) int {
		base := artisanLesson(w.rankExp[max(1, current)], current)
		switch {
		case current-recipeRank >= 3:
			base /= 4
		case current-recipeRank == 2:
			base /= 2
		}
		if !success {
			base /= 4
		}
		return max(1, (base*weight+50)/100)
	}
	role["AlchemyRank"], role["AlchemyExperience"] = w.advance(rank, exp, earn(rank), success)
	if slot != "" {
		certRank := max(0, min(9, int(roleInt(role, slot+"Level"))))
		certExp := max(0, int(roleInt(role, slot+"Value")))
		role[slot+"Level"], role[slot+"Value"] = w.advance(certRank, certExp, earn(certRank), success)
	}
}

func (w *alchemyRoom) element(id string) int {
	item, _ := w.rules.Item(id)
	p, _ := w.rules.Profile(item.ProfileID)
	return p.PrimaryElementID
}

func (w *alchemyRoom) relation(source, target int) string {
	switch {
	case source == 0 || target == 0:
		return "neutral"
	case source == target:
		return "same"
	case w.Elements[source].Generates == target:
		return "generates"
	case w.Elements[target].Generates == source:
		return "generatedBy"
	case w.Elements[source].Overcomes == target:
		return "overcomes"
	default:
		return "overcomeBy"
	}
}

func (w *alchemyRoom) materialModifier(source int, r elixirRecipe, aux []string) elixirModifier {
	main := w.Alchemy.Relations[w.relation(source, w.element(r.PrimaryID))]
	if len(aux) == 0 {
		return main
	}
	sum := elixirModifier{}
	for _, id := range aux {
		sum = sum.plus(w.Alchemy.Relations[w.relation(source, w.element(id))])
	}
	return main.scaled(w.Alchemy.Limits.PrimaryWeightBps, 1).plus(sum.scaled(10000-w.Alchemy.Limits.PrimaryWeightBps, len(aux))).scaled(1, 10000)
}

type brewStage struct {
	Name      string `json:"name"`
	PeriodMs  int    `json:"periodMs"`
	TargetBps int    `json:"targetBps"`
	WindowBps int    `json:"windowBps"`
	Direction int    `json:"direction"`
}
type elixirPlan struct {
	RecipeID                             string
	AuxIDs                               []string
	Stages                               [3]brewStage
	Rank, Mastery, Affinity              int
	BaseChance, YieldMultiplier, Potency int
}

func recipeJournal(role Record, id string) Record {
	all, _ := role["AlchemyJournal"].(Record)
	row, _ := all[id].(Record)
	return row
}
func (w *alchemyRoom) plan(role Record, req brewRequest) (elixirPlan, error) {
	r, ok := w.Alchemy.Recipes[req.RecipeID]
	if !ok {
		return elixirPlan{}, forgeFailure("请选择已登记丹方")
	}
	rank, _, _ := elixirTraining(role)
	if rank < r.MinRank {
		return elixirPlan{}, forgeFailure("丹师品阶不足")
	}
	if len(req.AuxIDs) > 3 {
		return elixirPlan{}, forgeFailure("一炉最多投入三份辅药")
	}
	p := elixirPlan{RecipeID: req.RecipeID, AuxIDs: req.AuxIDs, Rank: rank, Mastery: int(roleInt(recipeJournal(role, req.RecipeID), "Mastery"))}
	for _, id := range req.AuxIDs {
		if id == r.PrimaryID || !slices.Contains(w.materials, id) {
			return elixirPlan{}, forgeFailure("辅药须选主药以外的炼丹材料")
		}
		switch w.relation(w.element(id), w.element(r.PrimaryID)) {
		case "generates":
			p.Affinity += 100
		case "same":
			p.Affinity += 60
		case "generatedBy":
			p.Affinity += 35
		case "overcomes":
			p.Affinity -= 65
		case "overcomeBy":
			p.Affinity -= 90
		}
	}
	if len(req.AuxIDs) > 0 {
		p.Affinity /= len(req.AuxIDs)
	}
	fire, _ := w.rules.ForgeFlame("flame.ember")
	m := w.Alchemy.Ranks[rank].plus(w.Alchemy.Flames["flame.ember"]).plus(w.materialModifier(fire.ElementID, r, req.AuxIDs))
	if method := w.methods.equipped(role, 3); method != nil && method.kind == 3 {
		if n, d, ok := w.rules.MethodInfluence("alchemy", 3, method.rank); ok {
			m = m.plus(w.materialModifier(method.element, r, req.AuxIDs).scaled(n, d))
		}
	}
	p.BaseChance = r.SuccessBps + m.SuccessBps + p.Mastery*5 + p.Affinity*8
	p.YieldMultiplier = max(w.Alchemy.Limits.YieldMultiplierMinBps, min(w.Alchemy.Limits.YieldMultiplierMaxBps, 10000+m.YieldBps))
	p.Potency = m.PotencyBps
	for i, name := range []string{"温炉", "合药", "凝丹"} {
		ease := rank*45 + p.Mastery*4 + p.Affinity*2 - r.MinRank*115
		p.Stages[i] = brewStage{Name: name, PeriodMs: max(850, min(3800, 2400-i*260+ease)),
			TargetBps: []int{2200, 6150, 8120}[i], WindowBps: max(450, min(2300, 1550-i*235+ease)), Direction: []int{1, -1, 1}[i]}
	}
	return p, nil
}
func (w *alchemyRoom) chance(p elixirPlan, score int) int {
	return max(500, min(w.Alchemy.Limits.SuccessMaxBps, p.BaseChance+(score-60)*32))
}
func brewTapScore(stages [3]brewStage, taps []int) (int, error) {
	if len(taps) != 3 {
		return 0, forgeFailure("三道火候尚未点齐")
	}
	weighted := 0
	for i, stage := range stages {
		if taps[i] < 0 || taps[i] > 86400000 {
			return 0, forgeFailure("火候落点超出范围")
		}
		phase := (stage.Direction*(taps[i]%stage.PeriodMs)*10000/stage.PeriodMs + 10000) % 10000
		d := max(phase-stage.TargetBps, stage.TargetBps-phase)
		d = min(d, 10000-d)
		weighted += max(0, 100-50*d/stage.WindowBps) * []int{25, 35, 40}[i]
	}
	return (weighted + 50) / 100, nil
}

var elixirQualities = [...]string{"", "下品", "中品", "上品"}

func elixirQuality(p elixirPlan, score int) int {
	color := score + p.Rank*2 + p.Mastery/10 + p.Affinity/12
	if color >= 90 {
		return 3
	}
	if color >= 65 {
		return 2
	}
	return 1
}
func (w *alchemyRoom) effect(p elixirPlan, quality int) int {
	band := w.Alchemy.PotencyByQuality[quality]
	low, high := band[0], band[1]
	// Keep the material choice visible even within a fixed imprint grade.
	position := max(0, min(100, 45+p.Affinity/2+len(p.AuxIDs)*3+p.Potency/120))
	return max(1, (w.Alchemy.Recipes[p.RecipeID].BaseEffect*(low+(high-low)*position/100)+5000)/10000)
}
func (w *alchemyRoom) planView(p elixirPlan, role Record) Record {
	memory := recipeJournal(role, p.RecipeID)
	q := int(roleInt(memory, "Quality"))
	estimate := q
	if estimate == 0 {
		estimate = 2
	} else {
		p.Potency = int(roleInt(memory, "Potency"))
	}
	r := w.Alchemy.Recipes[p.RecipeID]
	value := w.effect(p, estimate)
	out := w.quote(role, p, estimate)
	out["poisonCost"] = r.PoisonCost
	out["effectBasis"], out["effectText"] = w.server.brewDisplayBasis(role, r, value), w.server.elixirEffectText(role, r.EffectID, r.EffectBasis, w.effects[r.EffectID].Name, value)
	out["successBps"], out["minSuccessBps"], out["maxSuccessBps"] = w.chance(p, 60), w.chance(p, 0), w.chance(p, 100)
	out["affinity"], out["mastery"], out["quality"] = p.Affinity, p.Mastery, elixirQualities[q]
	out["effect"], out["effectName"], out["stages"] = w.server.brewDisplayValue(role, r, value), w.effects[r.EffectID].Name, p.Stages
	return out
}
func (w *alchemyRoom) batchPlanView(p elixirPlan, role Record, count int) Record {
	out := w.planView(p, role)
	if count > 1 {
		stock := w.batchInventory(role, p, count)
		out["shortageCost"], out["canBuyMissing"], out["shortageText"] = w.missingCost(stock)
	}
	out["copyVigor"], out["batches"] = artisanCopyVigor(count, 1), count
	return out
}
func (w *alchemyRoom) directory(role Record) Record {
	counts := map[string]int64{}
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		if forgeUsable(g) {
			counts[w.herbID(g)] += transferStackCount(g)
		}
	}
	var materials, recipes []Record
	for _, id := range w.materials {
		materials = append(materials, Record{"id": id, "value": w.materialTerms(id).Value, "buyPrice": w.materialTerms(id).BuyPrice, "sourceText": w.sourceText(id), "name": w.text.ItemTemplates[id].Name, "count": counts[id], "elementName": w.text.Elements[strconv.Itoa(w.element(id))].Name})
	}
	for _, id := range w.recipes {
		r := w.Alchemy.Recipes[id]
		j := recipeJournal(role, id)
		recipes = append(recipes, Record{"id": id, "baseEffect": w.server.brewDisplayValue(role, r, r.BaseEffect), "poisonCost": r.PoisonCost, "sellPrices": w.recipePrices(id), "effectBasis": w.server.brewDisplayBasis(role, r, r.BaseEffect), "effectDescription": w.server.elixirEffectText(role, r.EffectID, r.EffectBasis, w.effects[r.EffectID].Name, r.BaseEffect), "name": w.text.ItemTemplates[r.OutputID].Name, "primaryId": r.PrimaryID, "primaryName": w.text.ItemTemplates[r.PrimaryID].Name,
			"primaryCount": w.primaryQty(r), "minRank": r.MinRank, "mastery": roleInt(j, "Mastery"), "memoryQuality": elixirQualities[roleInt(j, "Quality")], "memoryAuxIds": j["AuxIDs"], "effectName": w.effects[r.EffectID].Name})
	}
	rank, exp, _ := elixirTraining(role)
	return Record{"batchLimit": artisanCopyLimit(rank), "vigor": forgeCurrentVigor(role), "rankExp": exp, "rankLimit": w.rankExp[rank], "rank": rank, "rankName": w.rankNames[rank], "canClaimStarter": !roleBool(role, "AlchemyStarterClaimed"), "supplyPrice": brewSupplyPrice, "recipes": recipes, "materials": materials}
}
