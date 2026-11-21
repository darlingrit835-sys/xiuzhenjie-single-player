package main

import (
	"fmt"
	"reflect"
	"strings"
	"testing"
)

func TestMoldGradesMaterialsBatchAndEquipment(t *testing.T) {
	dir := t.TempDir()
	s, peer := workshopTestServer(t, dir)
	const iron, wood = "mat.hanwen_iron", "mat.cloudwood"
	for i := 0; i < 4; i++ {
		price := smithingSupplyPrice
		if i == 0 {
			price = 0
		}
		workshopCall(t, s, peer, smithingSupplyCmd, forgeRequest{RequestID: fmt.Sprintf("%032x", 100+i), ExpectedPrice: price}, true)
	}
	req := forgeRequest{Name: "青锋试器", BlankID: "wp.duanxia_saber", FlameID: "flame.ember", GradeID: 7, Inputs: []forgeInput{{iron, 2}}}
	prototypes := []struct {
		strokes []int
		quality string
		grade   int64
	}{
		{[]int{0, 0, 0, 0}, "粗模", 5}, {[]int{3, 0, 0, 0}, "良模", 6}, {[]int{3, 7, 4, 6}, "精模", 7},
	}
	var model Record
	for i, example := range prototypes {
		req.Strokes = example.strokes
		req.RequestID = fmt.Sprintf("%032x", i+200)
		estimate := workshopCall(t, s, peer, smithingPlanCmd, req, true)
		if estimate["moldQuality"] != example.quality || estimate["gradeName"] != s.smithing.gradeName(int(example.grade)) {
			t.Fatal(estimate)
		}
		adjusted := req
		if _, err := s.smithing.shapeForgeOrder(&adjusted); err != nil {
			t.Fatal(err)
		}
		ctx, _, _ := s.smithing.context(s.roles.Get("artist"), adjusted)
		ctx.PatternDifficulty = s.smithing.orderDifficulty(req, map[string]int{iron: 2}).Level
		for seed := int64(0); ; seed++ {
			out, err := s.smithing.rules.ResolveForge(req.BlankID, map[string]int{iron: 2}, ctx, seed)
			if err != nil || seed > 10000 {
				t.Fatal("no valid forging roll", err)
			}
			if !out.Success {
				continue
			}
			model, err = s.resolveSmithing("artist", req, seed)
			if err != nil {
				t.Fatal(err)
			}
			break
		}
		if model["moldQuality"] != example.quality || roleInt(model, "gradeId") != example.grade || model["moldId"] != model["templateId"] {
			t.Fatal(model)
		}
	}
	if got := len(s.smithing.moldDirectory(s.roles.Get("artist"))); got != 3 {
		t.Fatal(got)
	}
	id := roleString(model, "moldId")
	order := forgeRequest{MoldID: id, Quantity: 2, Inputs: []forgeInput{{wood, 2}}, RequestID: strings.Repeat("a", 32)}
	before := s.roles.Get("artist")
	plan := workshopCall(t, s, peer, smithingCastPlanCmd, order, true)
	if plan["materialText"] == model["materialText"] || !strings.Contains(roleString(plan, "materialText"), "生命上限") || plan["templateId"] == id || plan["moldId"] != id || roleInt(plan, "maxByMaterial") != 6 {
		t.Fatal(plan)
	}
	if s.smithingTemplate(roleString(plan, "templateId")) != nil || !reflect.DeepEqual(before, s.roles.Get("artist")) {
		t.Fatal("preview wrote variant or role")
	}
	out := workshopCall(t, s, peer, smithingCastCmd, order, true)
	after := s.roles.Get("artist")
	variant := roleString(out, "templateId")
	if out["materialText"] != plan["materialText"] || out["bonusText"] != plan["bonusText"] || out["effectText"] != plan["effectText"] || forgeTestCount(after, wood) != 8 || forgeTestCount(after, variant) != 2 || roleInt(before, "ForgeExperience") != roleInt(after, "ForgeExperience") {
		t.Fatal(out)
	}
	var ids []string
	for _, v := range goodsList(after) {
		g, _ := v.(Record)
		if roleString(g, "TemplateId") == variant {
			ids = append(ids, roleString(g, "GoodsId"))
		}
	}
	if len(ids) != 2 || ids[0] == ids[1] {
		t.Fatal("pieces need independent equipment ids", ids)
	}
	if err := moveBattleEquipment(after, ids[0], true); err != nil {
		t.Fatal(err)
	}
	attack, defense := battleEquipmentBonuses(after)
	if attack < roleInt(out, "baseAttack") || defense < roleInt(out, "baseDefense") {
		t.Fatal("shaped values lost on equip", attack, defense, out)
	}
	if forgeAffix(after, "max_hp") < 275 {
		t.Fatal("material vein did not affect actual equipped stats")
	}
	skill := battleSkills[0]
	skill.element = int(roleInt(out, "elementId"))
	_, power, percent := applyForgeSkill(after, skill, 100)
	if percent != 9 || power < 109 {
		t.Fatal("same-element forged weapon did not resonate", power, percent)
	}
	skill.element = skill.element%5 + 1
	if _, _, percent := applyForgeSkill(after, skill, 100); percent != 0 {
		t.Fatal("unmatched element resonated")
	}
	if err := s.roles.Update("artist", func(role Record) error {
		return moveBattleEquipment(role, ids[0], true)
	}); err != nil {
		t.Fatal(err)
	}
	var matching battleSkill
	for _, candidate := range battleSkills {
		if candidate.kind == 1 && candidate.element == int(roleInt(out, "elementId")) {
			matching = candidate
			break
		}
	}
	if matching.id == "" {
		t.Fatal("test needs a matching equipped spell")
	}
	if err := s.roles.Update("artist", func(role Record) error {
		role["SkillSettings"] = []any{Record{"SkillId": matching.id, "CombatSettingId": 0}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	s.config.BattleDurationSeconds = 120
	join := spellTestCall(t, s, peer, 1010002, "RoleJoinDungeonReq_1010002", "RoleJoinDungeonResp_1010002", Record{"MapId": "1211W13"})
	first := join.Get(field(join, "Skills")).List().Get(0).Message()
	if getInt(first, "ForgeResonancePercent") != 9 || getInt(join, "CurrentPureBloodDefense") <= 1000 {
		t.Fatal("batch equipment lost properties in actual dungeon packet", join)
	}
	s, peer = workshopTestServer(t, dir)
	retry := workshopCall(t, s, peer, smithingCastCmd, order, true)
	if retry["goodsId"] != out["goodsId"] || forgeTestCount(s.roles.Get("artist"), variant) != 1 {
		t.Fatal("batch replay duplicated equipment", retry)
	}
	order.Quantity = 3
	workshopCall(t, s, peer, smithingCastCmd, order, false)
	order.RequestID = strings.Repeat("c", 32)
	order.Quantity = 20
	unchanged := s.roles.Get("artist")
	workshopCall(t, s, peer, smithingCastCmd, order, false)
	if !reflect.DeepEqual(unchanged, s.roles.Get("artist")) {
		t.Fatal("out-of-stock batch changed bag")
	}
	order.Quantity = 1
	order.Inputs = []forgeInput{{wood, 1}, {iron, 2}}
	workshopCall(t, s, peer, smithingCastPlanCmd, order, false)
	order.Inputs = []forgeInput{{wood, 2}}
	other, _ := auctionTestRole(t, s, "buyer", nil)
	workshopCall(t, s, other, smithingCastCmd, order, false)
	if err := s.roles.Update("artist", func(role Record) error {
		for i := len(goodsList(role)); i < ringCapacity(role)-1; i++ {
			role["Goods"] = append(goodsList(role), Record{"GoodsId": fmt.Sprintf("filler-%d", i), "Count": 1, "Type": 19})
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	order.Quantity = 2
	unchanged = s.roles.Get("artist")
	workshopCall(t, s, peer, smithingCastCmd, order, false)
	if !reflect.DeepEqual(unchanged, s.roles.Get("artist")) {
		t.Fatal("partially inserted a batch into a full bag")
	}
}

func TestForgeShapeTradesAttackForDefense(t *testing.T) {
	s, _ := workshopTestServer(t, t.TempDir())
	req := forgeRequest{GradeID: 7, Strokes: []int{3, 7, 4, 6}}
	front, _ := s.smithing.shapeForgeOrder(&req)
	req.Strokes = []int{23, 27, 24, 26}
	rear, _ := s.smithing.shapeForgeOrder(&req)
	if front.quality != rear.quality || front.balance != 4 || rear.balance != -4 {
		t.Fatal(front, rear)
	}
	req.Strokes = []int{3, 7, 4, 30}
	if _, err := s.smithing.shapeForgeOrder(&req); err == nil {
		t.Fatal("invalid hammer station accepted")
	}
}
