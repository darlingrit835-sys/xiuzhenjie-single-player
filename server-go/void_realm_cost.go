package main

import (
	"encoding/json"
	"fmt"
	"slices"
)

// These are public properties of a published manuscript. Holdings and account identifiers
// belong only on the author's private quotation or receipt.
var voidCreationFields = []string{"steps", "stoneCost", "insightCost", "tuition", "materials", "value", "valueText", "craftText", "effectText", "qualityText", "affixText", "affinityText", "scoreText", "axes"}

func voidWallet(role Record) Record {
	return Record{"stones": roleInt(role, "ShinyStone"), "insight": roleInt(role, "CurrentClever")}
}

func voidPlanV2(owner string, q voidRequest) string {
	choices, _ := json.Marshal([]any{q.HallID, q.Grade, q.Brief, q.Route, q.Adjust, q.Steps})
	return voidDigest("v2", owner, string(choices))
}

func voidSavedSteps(work Record) ([]int, error) {
	// Stored JSON numbers decode as json.Number on restart; decode the same wire type
	// for both freshly signed manuscripts and their persisted counterparts.
	body, err := json.Marshal(work["steps"])
	var steps []int
	if err != nil || json.Unmarshal(body, &steps) != nil || len(steps) == 0 {
		return nil, forgeFailure("此作品的工序记录不完整")
	}
	return steps, nil
}

func voidPreview(role Record, q voidRequest) (Record, error) {
	switch q.Version {
	case 0, 1:
		return voidSimulate(role, q)
	case 2:
	default:
		return nil, forgeFailure("尚不支持该版工序")
	}
	result, err := voidCreateV2(role, q)
	if err != nil {
		return nil, err
	}
	usage, err := voidMaterials(result)
	if err != nil {
		return nil, err
	}
	hall := voidFindHall(q.HallID)
	tuition := manuscriptTuition(role, hall, q.Grade, q.Brief)
	stones, insight := roleInt(result, "stoneCost"), roleInt(result, "insightCost")
	if stones < 0 || insight < 0 {
		return nil, fmt.Errorf("negative registered cost for void hall %s", q.HallID)
	}
	// A signature identifies the exact quote, excluding current holdings. Spending or
	// receiving goods elsewhere does not invalidate the price, but stock is rechecked.
	quote, _ := json.Marshal([]any{voidPlanV2("", q), stones, insight, tuition, usage, result["value"]})
	result["quoteId"] = voidDigest("quote", string(quote))
	result["wallet"], result["tuition"] = voidWallet(role), tuition
	if tuition > 0 {
		result["costText"] = fmt.Sprintf("%s 本题修习另收 %d 灵石，本次共需 %d 灵石。", roleString(result, "costText"), tuition, stones+tuition)
	}
	affordable := roleInt(role, "ShinyStone") >= stones+tuition && roleInt(role, "CurrentClever") >= insight
	owned := map[string]int64{}
	for _, entry := range goodsList(role) {
		item, _ := entry.(Record)
		id := roleString(item, "TemplateId")
		if usage[id] != 0 && forgeUsable(item) {
			owned[id] += transferStackCount(item)
		}
	}
	display := []Record{}
	for _, material := range result["materials"].([]Record) {
		id := roleString(material, "templateId")
		display = append(display, Record{"templateId": id, "name": material["name"], "count": usage[id], "owned": owned[id]})
		if int64(usage[id]) > owned[id] {
			affordable = false
		}
	}
	result["materials"] = display
	result["affordable"] = affordable
	return result, nil
}

func voidMaterials(result Record) (map[string]int, error) {
	rows, ok := result["materials"].([]Record)
	if result["materials"] == nil {
		rows = []Record{}
		result["materials"] = rows
	} else if !ok {
		return nil, fmt.Errorf("invalid void material list")
	}
	needed := make(map[string]int, len(rows))
	for _, row := range rows {
		id, n := roleString(row, "templateId"), roleInt(row, "count")
		if id == "" || n < 1 || n > 10000 || needed[id] != 0 {
			return nil, fmt.Errorf("invalid void material cost for %q", id)
		}
		needed[id] = int(n)
	}
	return needed, nil
}

func voidPayCreation(role, work Record, q voidRequest, quote Record) error {
	stones, insight, tuition := roleInt(quote, "stoneCost"), roleInt(quote, "insightCost"), roleInt(quote, "tuition")
	if roleInt(role, "ShinyStone") < stones+tuition {
		return forgeFailure(fmt.Sprintf("本次创制需要 %d 灵石（含修习费 %d），灵石不足", stones+tuition, tuition))
	}
	if roleInt(role, "CurrentClever") < insight {
		return forgeFailure(fmt.Sprintf("本次创制需要 %d 悟性，当前悟性不足", insight))
	}
	usage, err := voidMaterials(quote)
	if err != nil {
		return err
	}
	if len(usage) > 0 {
		if err := consumeForgeInputs(role, usage); err != nil {
			return err
		}
	}
	// artisanManuscript debits tuition and credits lesson progress in this same StoreTx.
	role["ShinyStone"] = roleInt(role, "ShinyStone") - stones
	role["CurrentClever"] = roleInt(role, "CurrentClever") - insight
	for _, key := range voidCreationFields {
		if quote[key] != nil {
			work[key] = quote[key]
		}
	}
	work["steps"] = slices.Clone(q.Steps)
	materials := []Record{}
	for _, cost := range quote["materials"].([]Record) {
		materials = append(materials, Record{"templateId": cost["templateId"], "name": cost["name"], "count": cost["count"]})
	}
	work["materials"] = materials
	return nil
}
