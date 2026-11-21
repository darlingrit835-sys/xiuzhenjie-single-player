package main

import (
	_ "embed"
	"encoding/json"
	"fmt"
	"slices"
	"strings"
)

// The versioned copy freezes the affinity weights, names and puzzles used to
// sign a manuscript. Later editions must not reinterpret the author's choices.
//
//go:embed void_creation_v2.json
var voidCreationEdition []byte

type voidCraftStage struct {
	Name    string
	Options [3]string
}
type voidCraftBrief struct {
	Name, Hint            string
	Target                [1]int
	Polarity, Element, Qi int
}
type voidCraftHall struct {
	ID, Product, Mechanic, Unit, MaterialID, MaterialName string
	EconomyAffix, MasterAffix, Traits, NameEnding         string
	MaterialPolarity, MaterialPrice                       int
	MaterialElements, MaterialQi                          []int
	MaterialNote                                          string
	Stages                                                [3]voidCraftStage
	Briefs                                                [3]voidCraftBrief
}
type voidCraftBook struct {
	Polarities           [3]string
	Elements             [5]string
	Qi                   [12]string
	Generates, Overcomes [5]int
	PolarityQiWeights    [2][12]int
	ElementQiWeights     [5][12]int
	Halls                []voidCraftHall
}

var voidCraftV2 = func() voidCraftBook {
	var book voidCraftBook
	if err := json.Unmarshal(voidCreationEdition, &book); err != nil {
		panic(err)
	}
	return book
}()

func voidCraftHallFor(id string) *voidCraftHall {
	for i := range voidCraftV2.Halls {
		if voidCraftV2.Halls[i].ID == id {
			return &voidCraftV2.Halls[i]
		}
	}
	return nil
}

// The same three gestures have different consequences at each workbench.
func voidCraftMeasure(id string, a, b, c int) int {
	switch id {
	case "dan":
		return (a+1)*(b+1) - c
	case "qi":
		return (a+2)*(b+1) - 2*c
	case "fu":
		return (2*a + b + 1 + [3]int{0, 2, -1}[c]) % 6
	case "shu":
		return a + 1 + (b+1)*(3-c)
	case "zhen":
		return (a + 2) + (2*b + 2) - c
	case "dao":
		return min(a+2, b+2) * (c + 1)
	case "qiao":
		return (a + 1) * (b + 2) / (c + 1)
	case "zhi":
		return max(0, 2*(a+1)-max(0, 3-b-c))
	case "shou":
		return 2*(a+1) - 2*voidDistance(a-b) + c
	}
	panic("missing v2 workbench: " + id)
}

// Economy and mastery describe physical properties of this particular recipe.
func voidCraftTraits(id string, a, b, c int) (economy, mastery bool) {
	switch id {
	case "dan":
		return a == 0 && b >= 1, a == 2 && b >= 1 && c > 0
	case "qi":
		return a == 0 && c == 0, b > 0 && c == 2
	case "fu":
		return c == 0, 2*a+b+1 >= 6 && c == 2
	case "shu":
		return b == 0, b == 2 && c == 2
	case "zhen":
		return a == 0, a == 2 && b >= 1 && c > 0
	case "dao":
		return a == b, min(a, b) >= 1 && c == 2
	case "qiao":
		whole := (a+1)*(b+2)%(c+1) == 0
		return c > 0 && whole, a == 2 && c > 0 && whole
	case "zhi":
		return b+c == 3, b == 2 && c == 1
	case "shou":
		return a == b, a >= 1 && a == b && c == 2
	}
	return false, false
}

// Suggest the fewest edits. Ties favour a lighter process and keeping the
// earlier stages, so a failed attempt can be repaired without restarting.
func voidCraftRepair(h *voidCraftHall, target int, steps []int) string {
	best, weight := [3]int{}, 10000
	for a := 0; a < 3; a++ {
		for b := 0; b < 3; b++ {
			for c := 0; c < 3; c++ {
				if voidCraftMeasure(h.ID, a, b, c) != target {
					continue
				}
				proposal := [3]int{a, b, c}
				w := a + b + c
				for i, s := range proposal {
					if s != steps[i] {
						w += 100 + (2-i)*10
					}
				}
				if w < weight {
					weight, best = w, proposal
				}
			}
		}
	}
	var changes []string
	for i, s := range best {
		if s != steps[i] {
			changes = append(changes, h.Stages[i].Name+"改用「"+h.Stages[i].Options[s]+"」")
		}
	}
	return strings.Join(changes, "，") + "。"
}

func voidCreateV2(role Record, q voidRequest) (Record, error) {
	if err := voidAccess(role, voidFindHall(q.HallID), q.Grade); err != nil {
		return nil, err
	}
	h := voidCraftHallFor(q.HallID)
	if h == nil || q.Brief < 0 || q.Brief >= len(h.Briefs) || len(q.Steps) != 4 || q.Route < 1 || q.Route > 5 || q.Adjust < 1 || q.Adjust > 3 {
		return nil, forgeFailure("请选定论题、气性、五行和完整的三段工序")
	}
	for _, s := range q.Steps[:3] {
		if s < 0 || s > 2 {
			return nil, forgeFailure("此工序没有这道手法")
		}
	}
	mainQi := q.Steps[3]
	if mainQi < 1 || mainQi > 12 {
		return nil, forgeFailure("须从十二气中选择一种主气")
	}
	book := &voidCraftV2
	b := h.Briefs[q.Brief]
	target := b.Target[0]
	a, mid, c := q.Steps[0], q.Steps[1], q.Steps[2]
	measure := voidCraftMeasure(h.ID, a, mid, c)
	gap := voidDistance(target - measure)
	if h.ID == "fu" {
		gap = min(gap, 6-gap)
	}
	// The five-element cycle and qi weights come from cultivation_affinities.
	elementFit, elementSeal := 0, "异行"
	switch {
	case q.Route == b.Element:
		elementFit, elementSeal = 3, "同源"
	case book.Generates[q.Route-1] == b.Element:
		elementFit, elementSeal = 2, "相生"
	case book.Overcomes[q.Route-1] == b.Element:
		elementFit, elementSeal = -2, "相克"
	}
	qiWeight := book.ElementQiWeights[q.Route-1][mainQi-1]
	polarWeight := 3 // 平和 has no preferred qi in the world catalog.
	if q.Adjust < 3 {
		polarWeight = book.PolarityQiWeights[q.Adjust-1][mainQi-1]
	}
	fit := elementFit + qiWeight + polarWeight - 5
	if q.Adjust == b.Polarity {
		fit += 2
	}
	if mainQi == b.Qi {
		fit += 4
	}
	fit = max(0, min(14, fit))
	effort := a + mid + c
	economy, mastery := voidCraftTraits(h.ID, a, mid, c)
	masteryBonus := 0
	if mastery {
		masteryBonus = 3
	}
	materialFit := 0
	if q.Adjust == h.MaterialPolarity {
		materialFit++
	}
	if slices.Contains(h.MaterialElements, q.Route) {
		materialFit++
	}
	if slices.Contains(h.MaterialQi, mainQi) {
		materialFit++
	}
	quality := min(100, 50+4*q.Grade+fit+(6-effort)/2+masteryBonus+materialFit)
	stone := 3 + 2*q.Grade + effort
	if economy {
		stone -= 2
	}
	insight := q.Grade
	if h.ID == "dao" || h.ID == "shu" || h.ID == "zhen" {
		insight += mid + 1
	}
	materialCount := 1 + (q.Grade-1)/3
	value := stone + materialCount*h.MaterialPrice + insight*2 + fit*q.Grade + materialFit*q.Grade + quality/5
	affinity := fmt.Sprintf("%s · %s行 · %s气", book.Polarities[q.Adjust-1], book.Elements[q.Route-1], book.Qi[mainQi-1])
	reason := fmt.Sprintf("这次得到 %d%s，命题目标为 %d%s。", measure, h.Unit, target, h.Unit)
	var traits []string
	if economy {
		traits = append(traits, h.EconomyAffix+"（制谱省 2 灵石）")
	}
	if mastery {
		traits = append(traits, h.MasterAffix+"（合题加 3 分）")
	}
	effect := h.Traits
	if len(traits) > 0 {
		effect = "已成工艺词条　" + strings.Join(traits, " · ")
	}
	affix := fmt.Sprintf("%s · 行气亲和 %d/5 · 气性亲和 %d/5", elementSeal, qiWeight, polarWeight)
	affix += fmt.Sprintf(" · %s合料 %d/3", h.MaterialName, materialFit)
	if mainQi == b.Qi {
		affix += " · 主气应题"
	}
	if q.Adjust == b.Polarity {
		affix += " · 阴阳契题"
	}
	success := gap == 0
	title, next := "三工合题", "可题名入册，供同道按原谱复演。"
	if !success {
		title, next = "草谱未合", voidCraftRepair(h, target, q.Steps)
		quality = max(0, 40-4*gap+fit)
		value = 0
	}
	craft := make([]string, 3)
	for i, stage := range h.Stages {
		craft[i] = stage.Name + "·" + stage.Options[q.Steps[i]]
	}
	scoreText := fmt.Sprintf("合题 50 ＋ 等阶 %d ＋ 契合 %d ＋ 节工 %d ＋ 工艺词条 %d ＋ 合料 %d，上限 100。", 4*q.Grade, fit, (6-effort)/2, masteryBonus, materialFit)
	valueText := fmt.Sprintf("制谱 %d ＋ 材料折价 %d ＋ 悟性折价 %d ＋ 契合 %d ＋ 合料 %d ＋ 品评 %d。", stone, materialCount*h.MaterialPrice, insight*2, fit*q.Grade, materialFit*q.Grade, quality/5)
	qualityText := "工稳"
	if fit >= 11 && mastery {
		qualityText = "妙造"
	} else if fit >= 9 {
		qualityText = "精工"
	}
	if !success {
		qualityText = "待修"
		valueText = "草谱尚待合题。"
	}
	suggested := string([]rune(b.Name)[:2]) + book.Qi[mainQi-1] + h.NameEnding
	if h.ID == "fu" {
		suggested, _ = voidName(q)
	}
	if !success {
		scoreText = fmt.Sprintf("偏题 %d 格，草谱暂得 %d 分。", gap, quality)
	}
	return Record{"eligible": true, "success": success, "score": quality, "scoreText": scoreText, "title": title,
		"reason": reason, "next": next, "axes": []Record{{"name": h.Unit, "value": measure, "target": target}},
		"stoneCost": stone, "insightCost": insight, "materials": []Record{{"templateId": h.MaterialID, "name": h.MaterialName, "count": materialCount}},
		"value": value, "valueText": valueText, "qualityText": "品评 · " + qualityText, "suggestedName": suggested, "effectText": effect, "affinityText": affinity, "affixText": affix, "craftText": strings.Join(craft, " → "),
		"cost": stone, "costText": fmt.Sprintf("署名用 %d 灵石、%d 悟性和%s ×%d。", stone, insight, h.MaterialName, materialCount)}, nil
}
