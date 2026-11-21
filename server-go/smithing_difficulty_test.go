package main

import (
	"fmt"
	"reflect"
	"strings"
	"testing"
)

func TestPatternChallengesAndLegacyMold(t *testing.T) {
	s, _ := workshopTestServer(t, t.TempDir())
	w := s.smithing
	levels := map[int]bool{}
	for _, row := range w.patternDirectory(nil) {
		levels[int(roleInt(row, "difficulty"))] = true
		if roleString(row, "difficultyText") == "" {
			t.Fatal("missing plan for", row["name"])
		}
	}
	if len(levels) != 5 || levels[0] {
		t.Fatalf("published patterns cover %v", levels)
	}
	lookup := func(key string) Record { return w.patterns[smithingTemplateID("association.pattern.v1", key)] }
	simple, intricate := lookup("tingxue"), lookup("guichao")
	if simple["Rank"] != intricate["Rank"] || w.moldDifficulty(simple).Level != 1 || w.moldDifficulty(intricate).Level != 3 {
		t.Fatal("patterns in the same grade should differ")
	}
	if hardest := w.moldDifficulty(lookup("jinglei")); hardest.Level != 5 || !strings.Contains(hardest.Text, "四重雷纹") {
		t.Fatal(hardest)
	}
	thunderID := roleString(lookup("jinglei"), "TemplateId")
	exercise := forgeRequest{PatternID: thunderID, GradeID: 6, Strokes: []int{1, 8, 1, 8}}
	if form, err := w.shapeForgeOrder(&exercise); err != nil || form.quality != "精模" {
		t.Fatal("four hits on the drawn guide should earn a fine mold", form, err)
	}
	exercise.GradeID, exercise.Strokes = 6, []int{3, 7, 4, 6}
	if form, err := w.shapeForgeOrder(&exercise); err != nil || form.quality == "精模" {
		t.Fatal("hard drawings should have their own hammer sequence", form, err)
	}
	order := forgeRequest{BlankID: roleString(intricate, "BlankId"), GradeID: int(roleInt(intricate, "Rank")), PatternID: roleString(intricate, "TemplateId")}
	withClash := w.orderDifficulty(order, map[string]int{"mat.hanwen_iron": 2, "mat.cloudwood": 1})
	clear := w.orderDifficulty(order, map[string]int{"mat.hanwen_iron": 2, "mat.cold_dew": 1})
	if !strings.Contains(withClash.Text, "金木相克") || withClash.Level <= clear.Level {
		t.Fatal("switching from wood to dew should relieve the clash", withClash, clear)
	}
	old := Record{"id": "old", "blankId": order.BlankID, "gradeId": order.GradeID, "inputs": []any{
		Record{"templateId": "mat.hanwen_iron", "quantity": 2}, Record{"templateId": "mat.cloudwood", "quantity": 1},
	}}
	role := Record{"ForgeMolds": Record{"old": old}}
	before := cloneRecord(role)
	want := w.ratePattern(order.BlankID, order.GradeID, map[string]int{"mat.hanwen_iron": 2, "mat.cloudwood": 1}, patternWork{})
	if shown, ok := savedChallenge(w.moldDirectory(role)[0]); !ok || shown != want || !reflect.DeepEqual(before, role) {
		t.Fatal("viewing an old mold must derive its original rating without editing the save", shown)
	}
}

func TestApprenticeCopiesAssociationPattern(t *testing.T) {
	s, peer := workshopTestServer(t, t.TempDir())
	w := s.smithing
	p := w.patterns[smithingTemplateID("association.pattern.v1", "guichao")]
	req := forgeRequest{PatternID: roleString(p, "TemplateId"), BlankID: roleString(p, "BlankId"), GradeID: int(roleInt(p, "Rank")),
		FlameID: "flame.ember", Name: "学铸归潮", Inputs: []forgeInput{{"mat.hanwen_iron", 2}, {"mat.cloudwood", 1}}, Strokes: []int{10, 10, 10, 10}}
	invalid := req
	invalid.BlankID = "wp.tingquan_staff"
	workshopCall(t, s, peer, smithingPlanCmd, invalid, false)
	invalid = req
	invalid.GradeID = 15
	workshopCall(t, s, peer, smithingPlanCmd, invalid, false)
	invalid = req
	invalid.PatternID = "unknown"
	workshopCall(t, s, peer, smithingPlanCmd, invalid, false)
	first := workshopCall(t, s, peer, smithingPlanCmd, req, true)
	if roleInt(first, "difficulty") != 3 || first["moldQuality"] != "粗模" || first["gradeName"] != w.gradeName(1) {
		t.Fatal("quality loss must not lower the copied drawing's challenge", first)
	}
	var receipt Record
	for seed := int64(0); seed < 30; seed++ {
		price := smithingSupplyPrice
		if seed == 0 {
			price = 0
		}
		workshopCall(t, s, peer, smithingSupplyCmd, forgeRequest{RequestID: fmt.Sprintf("%032x", 1800+seed), ExpectedPrice: price}, true)
		req.RequestID = fmt.Sprintf("%032x", 1900+seed)
		var err error
		receipt, err = s.resolveSmithing(peer.currentRoleID(), req, seed)
		if err != nil {
			t.Fatal(err)
		}
		if roleBool(receipt, "forged") {
			break
		}
	}
	if !roleBool(receipt, "forged") || receipt["difficultyText"] != first["difficultyText"] || roleInt(receipt, "experience") != roleInt(first, "successExperience") {
		t.Fatal("receipt disagrees with the review sheet", receipt, first)
	}
	id := roleString(receipt, "moldId")
	mold := s.smithingTemplate(id)
	if roleInt(mold, "ShapeBalance") != roleInt(p, "ShapeBalance") {
		t.Fatal("neutral strikes lost the drawing's balance", mold)
	}
	role := s.roles.Get(peer.currentRoleID())
	if !reflect.DeepEqual(w.moldDifficulty(mold), w.moldDifficulty(w.moldDirectory(role)[0])) {
		t.Fatal("different directory rating")
	}
	cast := workshopCall(t, s, peer, smithingCastPlanCmd, forgeRequest{MoldID: id, Inputs: req.Inputs, Quantity: 2}, true)
	if roleInt(cast, "difficulty") != 3 || cast["difficultyText"] != first["difficultyText"] {
		t.Fatal("recasting lost inherited challenge", cast)
	}
	replay, err := s.resolveSmithing(peer.currentRoleID(), req, 79)
	if err != nil || roleString(replay, "goodsId") != roleString(receipt, "goodsId") || !reflect.DeepEqual(role, s.roles.Get(peer.currentRoleID())) {
		t.Fatal("replay changed goods or practice", err)
	}
	req.PatternID = ""
	if _, err = s.resolveSmithing(peer.currentRoleID(), req, 79); err == nil {
		t.Fatal("reused request id changed the design")
	}
}
