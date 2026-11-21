package main

import (
	"crypto/sha256"
	"encoding/binary"
	"encoding/json"
	"fmt"
	"maps"
	"slices"
	"strings"
	"xiuzhenjie/server/internal/affinity"
)

const maxForgeBatch = 65

type forgeShape struct {
	balance int
	quality string
}

func (w *smithingWorkshop) shapeForgeOrder(req *forgeRequest) (forgeShape, error) {
	if len(req.Strokes) == 0 {
		return forgeShape{}, nil
	} // Legacy single-piece orders.
	if len(req.Strokes) != 4 {
		return forgeShape{}, forgeFailure("请完成四锤塑形")
	}
	hits, balance := 0, 0
	targets := w.strikeTargets(req.PatternID)
	for i, stroke := range req.Strokes {
		if stroke < 0 || stroke > 29 {
			return forgeShape{}, forgeFailure("落锤位置或力道无效")
		}
		delta := stroke%10 - targets[i]
		if delta >= -1 && delta <= 1 {
			hits++
		}
		balance += 1 - stroke/10 // Front draws out the edge, rear reinforces the seat.
	}
	loss, quality := 2, "粗模"
	if hits >= 3 {
		loss, quality = 0, "精模"
	} else if hits >= 1 {
		loss, quality = 1, "良模"
	}
	grade := slices.IndexFunc(w.grades, func(g Record) bool { return roleInt(g, "id") == int64(req.GradeID) })
	if grade < 0 {
		return forgeShape{}, forgeFailure("目标品阶无效")
	}
	req.GradeID = int(roleInt(w.grades[max(0, grade-loss)], "id"))
	if p := w.patterns[req.PatternID]; p != nil {
		balance = max(-4, min(4, balance+int(roleInt(p, "ShapeBalance"))))
	}
	return forgeShape{balance: balance, quality: quality}, nil
}

func (shape forgeShape) apply(weapon *affinity.ForgedWeapon) {
	// Move actual whole stat points between attack and defense. Preserve a point on each side.
	amount := shape.balance * max(1, min(weapon.BaseAttack, weapon.BaseDefense)/5)
	amount = max(1-weapon.BaseAttack, min(weapon.BaseDefense-1, amount))
	weapon.BaseAttack += amount
	weapon.BaseDefense -= amount
}

func (w *smithingWorkshop) materialVein(id string) (string, int) {
	affixes := w.rules.ForgeAffixes(id)
	best := ""
	for _, stat := range slices.Sorted(maps.Keys(affixes)) {
		if best == "" || affixes[stat] > affixes[best] {
			best = stat
		}
	}
	return best, affixes[best]
}

func (w *smithingWorkshop) setForgedWeapon(t Record, weapon *affinity.ForgedWeapon) {
	maps.Copy(t, Record{"BlankId": weapon.PrototypeID, "CategoryId": weapon.CategoryID, "PolarityId": weapon.PolarityID, "FiveElement": weapon.ElementID,
		"QiId": weapon.QiID, "ClassId": weapon.ClassID, "Rank": weapon.GradeID, "Type": w.rules.ForgeEquipmentType(weapon.CategoryID), "Level": 1, "Stage": 1,
		"BaseAttack": weapon.BaseAttack, "BaseDefense": weapon.BaseDefense, "Bonuses": intsRecord(weapon.Bonuses), "BaseBonuses": intsRecord(weapon.BaseBonuses),
		"Inputs": intsRecord(weapon.Inputs), "CatalogVersion": weapon.CatalogVersion})
	// A material transfers its strongest native affix in proportion to its
	// share of the mix. Ties are resolved by catalog key to make the preview stable.
	if roleString(t, "MoldQuality") != "" {
		veins := make(map[string]int)
		total := 0
		for id, qty := range weapon.Inputs {
			stat, bps := w.materialVein(id)
			if stat != "" {
				veins[stat] += bps * qty
			}
			total += qty
		}
		for stat, strength := range veins {
			veins[stat] = (strength + total/2) / total
		}
		t["MaterialAffixes"] = intsRecord(veins)
	}
	maps.Copy(t, buildSmithingEquipmentFields(t))
}
func forgedGoods(t Record) Record {
	g := Record{"IsBinding": false, "IsUsing": false, "IsLock": false}
	for _, k := range []string{"TemplateId", "Name", "Type", "Rank", "FiveElement", "Level", "Stage"} {
		g[k] = t[k]
	}
	maps.Copy(g, buildSmithingEquipmentFields(t))
	return g
}
func saveForgeMold(role, t Record, inputs map[string]int, shape forgeShape) {
	id := roleString(t, "TemplateId")
	t["MoldId"], t["MoldQuality"], t["ShapeBalance"] = id, shape.quality, shape.balance
	molds, _ := role["ForgeMolds"].(Record)
	if molds == nil {
		molds = Record{}
		role["ForgeMolds"] = molds
	}
	ingredients := make([]any, 0, len(inputs))
	for _, mat := range slices.Sorted(maps.Keys(inputs)) {
		ingredients = append(ingredients, Record{"templateId": mat, "quantity": inputs[mat]})
	}
	mold := Record{"id": id, "name": t["Name"], "blankId": t["BlankId"], "gradeId": t["Rank"], "quality": shape.quality, "inputs": ingredients}
	if c, ok := savedChallenge(t); ok {
		c.DescribeInto(mold)
	}
	molds[id] = mold
}
func (w *smithingWorkshop) moldDirectory(role Record) []Record {
	molds, _ := role["ForgeMolds"].(Record)
	result := make([]Record, 0, len(molds))
	for _, id := range slices.Sorted(maps.Keys(molds)) {
		if m, ok := molds[id].(Record); ok {
			row := maps.Clone(m)
			if _, ok := savedChallenge(row); !ok {
				inputs := make(map[string]int)
				for _, lot := range battleRecords(row["inputs"]) {
					inputs[roleString(lot, "templateId")] = int(roleInt(lot, "quantity"))
				}
				w.ratePattern(roleString(row, "blankId"), int(roleInt(row, "gradeId")), inputs, patternWork{}).DescribeInto(row)
			}
			result = append(result, row)
		}
	}
	return result
}

// Only later pieces share the heat of the first pour. Round down per material,
// so a single piece always uses its exact recipe and every material is paid.
func batchMaterialCost(perPiece, quantity, savingPercent int) (spent, saved int) {
	saved = perPiece * (quantity - 1) * savingPercent / 100
	return perPiece*quantity - saved, saved
}

func furnaceMaterialAffinity(relation string) (string, int) {
	switch relation {
	case "same":
		return "火材同相", 16
	case "generates":
		return "火生其材", 20
	case "generatedBy":
		return "材助炉火", 8
	case "overcomes":
		return "火克其材", -7
	case "overcomeBy":
		return "材克炉火", -10
	default:
		return "无五行偏向", 0
	}
}

func batchRankAdvice(label string, current, suggested int) string {
	gap := suggested - current
	switch {
	case gap > 0:
		return fmt.Sprintf("%s建议 %d 品（尚差 %d 品）", label, suggested, gap)
	case gap < 0:
		return fmt.Sprintf("%s建议 %d 品（高出 %d 品）", label, suggested, -gap)
	default:
		return fmt.Sprintf("%s建议 %d 品（已达标）", label, suggested)
	}
}

func (w *smithingWorkshop) priceBatch(role, source Record, req forgeRequest, inputs map[string]int, result Record) (map[string]int, error) {
	rank, _, _ := smithTraining(role)
	challenge := w.moldDifficulty(source)
	challenge.DescribeInto(result)
	smithName, flameName := w.professions[rank].Name, "常炉"
	patternRank := challenge.SuggestedRank()
	patternAdvice := fmt.Sprintf("当前师阶 %d 品 · %s", rank, batchRankAdvice("器图", rank, patternRank))
	control := patternAdvice + "。常炉依原方投料。"
	baseRate, element := 0, 0
	if req.FlameID != "" {
		flame, ok := w.rules.ForgeFlame(req.FlameID)
		if !ok {
			return nil, forgeFailure("炉火不在目录中，请重新验料")
		}
		mastery, _ := w.rules.FlameMastery(req.FlameID, rank)
		element = flame.ElementID
		flameName = w.text.ForgeFlames[req.FlameID].Name
		// Higher fire has more potential, but a smith below its required rank loses
		// more to heat than the stronger fire contributes. Difficult molds do likewise.
		baseRate = 12 + 2*rank + 2*(mastery.RequiredRank-1) -
			4*max(0, mastery.RequiredRank-rank) - 4*max(0, patternRank-rank)
		control = patternAdvice + " · " + batchRankAdvice(flameName, rank, mastery.RequiredRank) +
			"。首件足料，续件按火材相性逐料节省整份。"
	}
	result["smithRank"], result["smithRankName"], result["flameName"], result["castingText"] = rank, smithName, flameName, control

	stock := make(map[string]int64, len(inputs))
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		id := roleString(g, "TemplateId")
		if inputs[id] > 0 && forgeUsable(g) {
			stock[id] += transferStackCount(g)
		}
	}
	total := make(map[string]int, len(inputs))
	rows := make([]any, 0, len(inputs))
	var costs []string
	baseUnits, spentUnits, budget := 0, 0, forgeBatchLimit(role)
	for _, id := range slices.Sorted(maps.Keys(inputs)) {
		perPiece := inputs[id]
		item, _ := w.rules.Item(id)
		profile, _ := w.rules.Profile(item.ProfileID)
		materialElement := profile.PrimaryElementID
		relation, bonus := furnaceMaterialAffinity(w.rules.ForgeElementRelation(element, materialElement))
		savingPercent := 0
		if element != 0 {
			savingPercent = max(0, min(50, baseRate+bonus))
		}
		spent, saved := batchMaterialCost(perPiece, req.Quantity, savingPercent)
		total[id] = spent
		baseUnits += perPiece * req.Quantity
		spentUnits += spent
		held := stock[id]
		for budget > 0 {
			need, _ := batchMaterialCost(perPiece, budget, savingPercent)
			if int64(need) <= held {
				break
			}
			budget--
		}
		name := w.text.ItemTemplates[id].Name
		text := fmt.Sprintf("%s ×%d（有 %d", name, spent, held)
		if saved > 0 {
			text += fmt.Sprintf("，省 %d", saved)
		}
		if int64(spent) > held {
			text += fmt.Sprintf("，缺 %d", int64(spent)-held)
		}
		costs = append(costs, text+"）")
		rows = append(rows, Record{"templateId": id, "name": name, "elementName": w.text.Elements[fmt.Sprint(materialElement)].Name,
			"relationText": relation, "perPiece": perPiece, "held": held, "base": perPiece * req.Quantity,
			"spent": spent, "saved": saved, "savingPercent": savingPercent})
	}
	result["quantity"], result["maxByMaterial"], result["costText"], result["costs"] = req.Quantity, budget, strings.Join(costs, "、"), rows
	result["baseUnits"], result["spentUnits"], result["savedUnits"] = baseUnits, spentUnits, baseUnits-spentUnits
	return total, nil
}

func (s *Server) castFromMold(owner string, req forgeRequest, commit bool) (Record, error) {
	if req.Quantity < 1 || req.Quantity > maxForgeBatch {
		return nil, forgeFailure("每炉可浇铸 1 至 65 件")
	}
	inputs, err := selectForgeInputs(req)
	if err != nil {
		return nil, err
	}
	signature, _ := json.Marshal([]any{req.MoldID, inputs, req.Quantity, req.FlameID})
	hash := fmt.Sprintf("%x", sha256.Sum256(signature))
	receiptKey := smithingStorePrefix + "batch." + smithingTemplateID(owner, req.RequestID)
	var result Record
	err = s.roles.Transact(func(tx *StoreTx) error {
		if commit {
			if receipt := tx.View(receiptKey); receipt != nil {
				expectedHash := hash
				if roleInt(receipt, "CastVersion") == 0 {
					// Older servers ignored flameId and never recorded it in the receipt.
					legacy, _ := json.Marshal([]any{req.MoldID, inputs, req.Quantity})
					expectedHash = fmt.Sprintf("%x", sha256.Sum256(legacy))
				}
				if roleString(receipt, "RequestHash") != expectedHash {
					return forgeFailure("请求编号已用于另一批铸件")
				}
				view, _ := receipt["Result"].(Record)
				result = cloneRecord(view)
				return nil
			}
		}
		role := tx.View(owner)
		if role == nil {
			return forgeFailure("角色存档不存在")
		}
		if req.Quantity > forgeBatchLimit(role) {
			return forgeFailure(fmt.Sprintf("当前师阶每炉最多 %d 件", forgeBatchLimit(role)))
		}
		molds, _ := role["ForgeMolds"].(Record)
		public := s.smithing.patterns[req.MoldID]
		if public == nil && molds[req.MoldID] == nil {
			return forgeFailure("请选择自己的模具或协会器图")
		}
		source := tx.View(smithingStorePrefix + req.MoldID)
		if source == nil && public != nil {
			source = public
			if commit {
				maps.Copy(tx.GetOrCreate(smithingStorePrefix+req.MoldID), source)
			}
		}
		if source == nil || (public == nil && roleString(source, "CreatorRoleId") != owner) {
			return forgeFailure("模具原型遗失")
		}
		recipe, _ := source["Inputs"].(Record)
		original := recipeQuantities(recipe)
		t := source
		if !maps.Equal(original, inputs) {
			variant, _ := json.Marshal(inputs)
			id := smithingTemplateID(req.MoldID, string(variant))
			t = tx.View(smithingStorePrefix + id)
			if t == nil {
				digest := sha256.Sum256([]byte(id))
				weapon, err := s.smithing.rules.CastForge(roleString(source, "BlankId"), original, inputs, int(roleInt(source, "Rank")), int64(binary.LittleEndian.Uint64(digest[:])))
				if err != nil {
					return forgeFailure("每件用料总数须与模具原方相同，请选择目录中的材料")
				}
				shape := forgeShape{balance: int(roleInt(source, "ShapeBalance"))}
				shape.apply(weapon)
				t = cloneRecord(source)
				t["TemplateId"], t["Experience"] = id, 0
				delete(t, "OriginalGoodsId")
				delete(t, "RequestHash")
				s.smithing.setForgedWeapon(t, weapon)
				if commit {
					maps.Copy(tx.GetOrCreate(smithingStorePrefix+id), t)
				}
			}
		}
		result = s.smithing.weaponView(t)
		difficulty := s.smithing.moldDifficulty(source)
		experience := s.smithing.batchExperience(role, difficulty, req.Quantity)
		result["experience"] = experience
		total, err := s.smithing.priceBatch(role, source, req, inputs, result)
		if err != nil {
			return err
		}
		// Count empty slots after withdrawing the same lots consumeForgeInputs visits.
		slots := ringCapacity(role) - len(goodsList(role))
		left := maps.Clone(total)
		for _, v := range goodsList(role) {
			g, _ := v.(Record)
			id := roleString(g, "TemplateId")
			if left[id] <= 0 || !forgeUsable(g) {
				continue
			}
			count := transferStackCount(g)
			used := min(count, int64(left[id]))
			left[id] -= int(used)
			if used == count {
				slots++
			}
		}
		result["freeSlots"], result["canFit"] = slots, slots >= req.Quantity
		vigor := s.smithing.forgeVigor(int(roleInt(t, "Rank")), difficulty.Level, req.Quantity, false)
		estimate, recycle := s.smithing.quoteForgedValue(t, forgeMaterialValue(role, total), req.Quantity)
		result["vigorCost"], result["availableVigor"], result["appraisal"], result["recyclePrice"] = vigor, forgeCurrentVigor(role), estimate, recycle
		result["castingText"] = roleString(result, "castingText") + "\n" + forgeQuoteText(vigor, estimate, recycle)
		if !commit {
			return nil
		}
		if roleInt(result, "maxByMaterial") < int64(req.Quantity) {
			return forgeFailure("可用材料不足，整批尚未开炉")
		}
		role = tx.Get(owner)
		if !spendArtisanVigor(role, vigor) {
			return forgeFailure(fmt.Sprintf("本批浇铸需 %d 精力", vigor))
		}
		if err := consumeForgeInputs(role, total); err != nil {
			return err
		}
		var firstID string
		for i := 0; i < req.Quantity; i++ {
			g := forgedGoods(t)
			g["RecyclePrice"] = recycle
			id, err := receiveInventoryGoods(role, g, 1)
			if err != nil {
				return forgeFailure("纳戒空间不足，整批未扣料")
			}
			if i == 0 {
				firstID = id
			}
		}
		if experience > 0 {
			result["trainingText"] = s.smithing.awardExperience(role, experience)
		}
		result["forged"], result["goodsId"] = true, firstID
		maps.Copy(tx.GetOrCreate(receiptKey), Record{"CastVersion": 1, "RequestHash": hash, "Result": cloneRecord(result)})
		return nil
	})
	return result, err
}
