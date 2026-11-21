package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"os"
	"reflect"
	"strings"
	"testing"
)

func workshopTestServer(t *testing.T, dir string) (*Server, *Peer) {
	t.Helper()
	s := auctionTestServer(t, dir)
	s.dataDir = "../server/data"
	if err := s.loadSmithingWorkshop(); err != nil {
		t.Fatal(err)
	}
	peer, _ := auctionTestRole(t, s, "artist", nil)
	return s, peer
}

func workshopCall(t *testing.T, s *Server, peer *Peer, cmd int, req forgeRequest, ok bool) Record {
	t.Helper()
	b, err := json.Marshal(req)
	if err != nil {
		t.Fatal(err)
	}
	s.handleGame(peer, Message{Cmd: cmd, Msid: 5, Data: offerString(nil, 3, string(b))})
	msg := peer.conn.(*auctionTestConnection).receive(t, s, cmd, 5, "ActionBoolResp_1001001")
	var result Record
	if getBool(msg, "IsSuccess") != ok || json.Unmarshal([]byte(offerToken(msg.GetUnknown())), &result) != nil {
		t.Fatalf("command %d: %v", cmd, msg)
	}
	return result
}

func forgeTestCount(role Record, id string) int64 {
	var count int64
	for _, val := range goodsList(role) {
		g := val.(Record)
		if roleString(g, "TemplateId") == id {
			count += transferStackCount(g)
		}
	}
	return count
}

func noSmithingReveal(t *testing.T, body Record) {
	t.Helper()
	raw, _ := json.Marshal(body)
	for _, field := range []string{"affixText", "processAffixText", "bonusText", "bonuses", "baseAttack", "baseDefense", "classId", "qiName"} {
		if strings.Contains(string(raw), fmt.Sprintf("%q", field)) {
			t.Fatalf("%s revealed before successful forge", field)
		}
	}
}

func TestWorkshopSettlesOnceAndOnlyRevealsOnSuccess(t *testing.T) {
	dir := t.TempDir()
	s, peer := workshopTestServer(t, dir)
	directory := workshopCall(t, s, peer, smithingCatalogCmd, forgeRequest{}, true)
	noSmithingReveal(t, directory)
	if len(directory["grades"].([]any)) != 12 || len(directory["flames"].([]any)) < 3 {
		t.Fatal("missing workshop choices", directory)
	}
	starter := forgeRequest{RequestID: strings.Repeat("a", 32)}
	workshopCall(t, s, peer, smithingSupplyCmd, starter, true)
	workshopCall(t, s, peer, smithingSupplyCmd, starter, true)
	workshopCall(t, s, peer, smithingSupplyCmd, forgeRequest{RequestID: strings.Repeat("e", 32)}, false)
	const mat = "mat.hanwen_iron"
	if forgeTestCount(s.roles.Get("artist"), mat) != 3 || roleInt(s.roles.Get("artist"), "ShinyStone") != 1000 {
		t.Fatal("invalid starter settlement")
	}
	req := forgeRequest{Name: "北斗寒刃", BlankID: s.smithing.blanks[0].ID, FlameID: "flame.ember", GradeID: 7, Inputs: []forgeInput{{mat, 2}}, RequestID: strings.Repeat("b", 32)}
	// Before formal certification the workshop reads the trial apprentice rank.
	// An existing certified profession in either slot and the equipped method take priority.
	if err := s.roles.Transact(func(tx *StoreTx) error {
		r := tx.Get("artist")
		r["Live2Type"], r["Live2Level"], r["Live2Value"], r["GongFaNaQi"] = 2, 3, 0, "gf_3_4"
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	preview := workshopCall(t, s, peer, smithingPlanCmd, req, true)
	noSmithingReveal(t, preview)
	if !strings.Contains(roleString(preview, "smithRankName"), "三品") || !strings.Contains(roleString(preview, "methodName"), "玄水") || roleInt(preview, "successBps") <= 0 {
		t.Fatal(preview)
	}
	if s.smithingTemplate(smithingTemplateID("artist", req.RequestID)) != nil {
		t.Fatal("preview published a template")
	}
	inputs, err := selectForgeInputs(req)
	if err != nil {
		t.Fatal(err)
	}
	seedFor := func(success bool) int64 {
		t.Helper()
		ctx, _, _ := s.smithing.context(s.roles.Get("artist"), req)
		ctx.PatternDifficulty = s.smithing.orderDifficulty(req, inputs).Level
		for seed := int64(0); seed < 10000; seed++ {
			out, err := s.smithing.rules.ResolveForge(req.BlankID, inputs, ctx, seed)
			if err != nil {
				t.Fatal(err)
			}
			if out.Success == success {
				return seed
			}
		}
		t.Fatal("test recipe must allow both outcomes", preview)
		return 0
	}
	failure, err := s.resolveSmithing("artist", req, seedFor(false))
	if err != nil || roleBool(failure, "forged") {
		t.Fatal(failure, err)
	}
	noSmithingReveal(t, failure)
	refunded := roleInt(failure["returned"].(Record), mat)
	after := s.roles.Get("artist")
	if forgeTestCount(after, mat) != 1+refunded || roleInt(after, "Live2Value") != roleInt(failure, "experience") ||
		roleInt(preview, "failureExperience") != roleInt(failure, "experience") ||
		failure["spentText"] != s.smithing.materialUseText(map[string]int{mat: 2 - int(refunded)}) || failure["factorText"] != preview["factorText"] {
		t.Fatal("incorrect failed settlement", failure, after)
	}
	rejectedID := smithingTemplateID("artist", req.RequestID)
	workshopCall(t, s, peer, smithingTemplateCmd, forgeRequest{TemplateID: rejectedID}, false)
	retry := workshopCall(t, s, peer, smithingCreateCmd, req, true)
	noSmithingReveal(t, retry)
	if roleBool(retry, "forged") || retry["trainingText"] != failure["trainingText"] || retry["spentText"] != failure["spentText"] || !reflect.DeepEqual(after, s.roles.Get("artist")) {
		t.Fatal("failure replay wrote again")
	}
	altered := req
	altered.GradeID = 1
	workshopCall(t, s, peer, smithingCreateCmd, altered, false)
	buy := forgeRequest{RequestID: strings.Repeat("c", 32), ExpectedPrice: smithingSupplyPrice}
	workshopCall(t, s, peer, smithingSupplyCmd, buy, true)
	workshopCall(t, s, peer, smithingSupplyCmd, buy, true)
	if roleInt(s.roles.Get("artist"), "ShinyStone") != 880 {
		t.Fatal("repeated purchase")
	}
	req.RequestID = strings.Repeat("d", 32)
	success, err := s.resolveSmithing("artist", req, seedFor(true))
	target, _ := s.smithing.rules.ForgeTarget(req.GradeID)
	if err != nil || !roleBool(success, "forged") || roleInt(success, "gradeId") != 7 || len(success["bonuses"].(Record)) != len(forgeEffectRecord(target.ForgeFactor)) ||
		!strings.Contains(roleString(success, "processAffixText"), "目标品阶") {
		t.Fatal(success, err)
	}
	id := roleString(success, "templateId")
	after = s.roles.Get("artist")
	if forgeTestCount(after, id) != 1 || forgeTestCount(after, mat) != 2+refunded || roleInt(after, "Live2Value") != roleInt(success, "experience")+roleInt(failure, "experience") {
		t.Fatal("successful forge not atomic")
	}
	s, peer = workshopTestServer(t, dir)
	retry = workshopCall(t, s, peer, smithingCreateCmd, req, true)
	detail := workshopCall(t, s, peer, smithingTemplateCmd, forgeRequest{TemplateID: id}, true)
	if roleString(retry, "goodsId") != roleString(success, "goodsId") || retry["trainingText"] != success["trainingText"] || retry["spentText"] != success["spentText"] || roleString(detail, "bonusText") != roleString(success, "bonusText") ||
		roleString(detail, "processAffixText") != roleString(success, "processAffixText") || forgeTestCount(s.roles.Get("artist"), id) != 1 {
		t.Fatal("lost successful template on reload", retry, detail)
	}
	req.RequestID = strings.Repeat("f", 32)
	req.Inputs = []forgeInput{{mat, 12}}
	before := s.roles.Get("artist")
	workshopCall(t, s, peer, smithingCreateCmd, req, false)
	if !reflect.DeepEqual(before, s.roles.Get("artist")) || s.smithingTemplate(smithingTemplateID("artist", req.RequestID)) != nil {
		t.Fatal("invalid input cost resources")
	}
	buyer, _ := auctionTestRole(t, s, "collector", nil)
	fixedAuctionCall(t, s, peer, 1006008, 6, Record{
		"GoodsId": success["goodsId"], "ShopType": 83, "Count": 1, "FixedPrice": 200,
	}, 4168)
	search := fixedAuctionCall(t, s, buyer, 1006001, 7, Record{
		"ShopType": 83, "GoodsType": 14, "Type": 15, "Name": id,
	}, 4160)
	listings := search.Get(field(search, "Object")).List()
	if listings.Len() != 1 || getString(listings.Get(0).Message(), "GoodsId") != id {
		t.Fatal("could not find the forged weapon by its original template", search)
	}
	fixedAuctionCall(t, s, buyer, 1006010, 8, Record{"AuctionId": getString(listings.Get(0).Message(), "Id")}, 4046)
	bought := workshopCall(t, s, buyer, smithingTemplateCmd, forgeRequest{TemplateID: id}, true)
	if forgeTestCount(s.roles.Get("collector"), id) != 1 || !reflect.DeepEqual(detail, bought) {
		t.Fatal("settled weapon differs from the workshop template", bought)
	}
}

func TestWorkshopInventoryRollbackAndConcurrentPromotion(t *testing.T) {
	dir := t.TempDir()
	s, _ := workshopTestServer(t, dir)
	const owner = "artist"
	const material = "mat.hanwen_iron"
	threshold := int(s.smithing.professions[2].MaxExperience) // xzjlianqilist.json, plantTopValue
	edit := func(fn func(Record)) {
		t.Helper()
		if err := s.roles.Update(owner, func(r Record) error { fn(r); return nil }); err != nil {
			t.Fatal(err)
		}
	}
	edit(func(r Record) {
		r["Live1Type"], r["Live1Level"], r["Live1Value"] = 1, 2, 17
		r["Live2Type"], r["Live2Level"], r["Live2Value"] = 2, 2, threshold-1
		goods := []any{Record{"GoodsId": "iron", "TemplateId": material, "Count": 3, "Type": 13}}
		for i := 1; i < 100; i++ {
			goods = append(goods, Record{"GoodsId": fmt.Sprintf("filler-%d", i), "Count": 1, "Type": 15})
		}
		r["Goods"] = goods
	})
	req := forgeRequest{Name: "封炉验刃", BlankID: "wp.minglei_spear", FlameID: "flame.ember", GradeID: 1, Inputs: []forgeInput{{material, 2}}, RequestID: strings.Repeat("a", 32)}
	ctx, _, _ := s.smithing.context(s.roles.Get(owner), req)
	ctx.PatternDifficulty = s.smithing.orderDifficulty(req, map[string]int{material: 2}).Level
	// Independent server rolls and the same request ID must all replay whichever
	// success commits first. All candidate seeds here succeed so the test always
	// exercises the output slot and experience promotion.
	var seeds []int64
	for seed := int64(0); len(seeds) < 8 && seed < 10000; seed++ {
		out, err := s.smithing.rules.ResolveForge(req.BlankID, map[string]int{material: 2}, ctx, seed)
		if err != nil {
			t.Fatal(err)
		}
		if out.Success {
			seeds = append(seeds, seed)
		}
	}
	if len(seeds) != 8 {
		t.Fatal("test setup could not produce eight successful rolls")
	}
	id := smithingTemplateID(owner, req.RequestID)
	readSave := func() []byte {
		t.Helper()
		raw, err := os.ReadFile(s.roles.path)
		if err != nil {
			t.Fatal(err)
		}
		return raw
	}
	rejected := func(message string) {
		t.Helper()
		mem, disk := s.roles.Get(owner), readSave()
		_, err := s.resolveSmithing(owner, req, seeds[0])
		if err == nil || !strings.Contains(err.Error(), message) {
			t.Fatalf("expected %q, got %v", message, err)
		}
		if !reflect.DeepEqual(mem, s.roles.Get(owner)) || !bytes.Equal(disk, readSave()) || s.smithingTemplate(id) != nil {
			t.Fatalf("%q changed in-memory or durable inventory, experience or template", message)
		}
	}
	rejected("纳戒空间不足") // Using two out of a stack of three does not free its slot.
	edit(func(r Record) {
		r["Goods"] = goodsList(r)[:99]
		locateGoods(r, "iron")["IsLock"] = true
	})
	rejected("可用材料不足")
	edit(func(r Record) { locateGoods(r, "iron")["IsLock"] = false })
	type reply struct {
		result Record
		err    error
	}
	start := make(chan struct{})
	replies := make(chan reply, len(seeds))
	for _, seed := range seeds {
		go func() { <-start; result, err := s.resolveSmithing(owner, req, seed); replies <- reply{result, err} }()
	}
	close(start)
	var first Record
	for range seeds {
		response := <-replies
		if response.err != nil || !roleBool(response.result, "forged") {
			t.Fatalf("concurrent reply: %v %v", response.result, response.err)
		}
		if first == nil {
			first = response.result
		} else if !reflect.DeepEqual(first, response.result) {
			t.Fatal("retry returned a different weapon or receipt")
		}
	}
	s, _ = workshopTestServer(t, dir)
	r := s.roles.Get(owner)
	earned := roleInt(first, "experience")
	if earned < 1 || forgeTestCount(r, id) != 1 || forgeTestCount(r, material) != 1 || roleInt(r, "Live2Level") != 3 ||
		roleInt(r, "Live2Value") != earned-1 || roleInt(r, "ForgeRank") != 3 || roleInt(r, "ForgeExperience") != earned-1 ||
		roleInt(r, "Live1Level") != 2 || roleInt(r, "Live1Value") != 17 || roleString(s.smithingTemplate(id), "OriginalGoodsId") != roleString(first, "goodsId") {
		t.Fatal("reopened inventory or profession did not reflect exactly one creation and one promotion", r)
	}
	events, err := s.roles.pendingHistory()
	if err != nil || len(events) != 1 || events[0].Kind != "smithing.weapon_created" {
		t.Fatalf("expected a single durable creation event: %+v, %v", events, err)
	}
}

func TestWorkshopPatternStudyPromotesThirdProfession(t *testing.T) {
	s, peer := workshopTestServer(t, t.TempDir())
	w := s.smithing
	const owner = "artist"
	id := smithingTemplateID("association.pattern.v1", "jinglei")
	pattern := w.patterns[id]
	if pattern == nil {
		t.Fatal("missing association blueprint")
	}
	inputs := recipeQuantities(pattern["Inputs"].(Record))
	req := forgeRequest{RequestID: strings.Repeat("9", 32), Name: "初试雷枪", BlankID: roleString(pattern, "BlankId"), GradeID: int(roleInt(pattern, "Rank")),
		PatternID: id, FlameID: "flame.ember", Strokes: []int{0, 0, 0, 0}}
	var goods []any
	for mat, n := range inputs {
		req.Inputs = append(req.Inputs, forgeInput{mat, n})
		goods = append(goods, Record{"GoodsId": mat, "TemplateId": mat, "Count": 3 * n, "Type": 13})
	}
	cap := int(w.professions[4].MaxExperience)
	if err := s.roles.Update(owner, func(r Record) error {
		r["ForgeRank"], r["ForgeExperience"] = 2, 17
		r["Live1Type"], r["Live1Level"], r["Live1Value"] = 1, 9, 23
		r["Live2Type"], r["Live2Level"], r["Live2Value"] = 2, 3, 44
		r["Live3Type"], r["Live3Level"], r["Live3Value"] = 2, 4, cap-2
		r["Goods"] = goods
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	unmatched := req
	unmatched.GradeID = 1
	workshopCall(t, s, peer, smithingPlanCmd, unmatched, false)
	workshopCall(t, s, peer, smithingCreateCmd, unmatched, false)
	preview := workshopCall(t, s, peer, smithingPlanCmd, req, true)
	difficulty := w.moldDifficulty(pattern).Level
	if difficulty != 5 || roleInt(preview, "difficulty") != int64(difficulty) || roleInt(preview, "gradeId") >= int64(req.GradeID) ||
		roleInt(preview, "studyExperience") != 8 || !strings.Contains(roleString(preview, "factorText"), "临图研习") ||
		!strings.Contains(roleString(preview, "trainingText"), "距晋品 2") || !strings.Contains(roleString(preview, "smithRankName"), "四品") {
		t.Fatal("bad blueprint study estimate", preview)
	}
	adjusted := req
	if _, err := w.shapeForgeOrder(&adjusted); err != nil {
		t.Fatal(err)
	}
	ctx, _, _ := w.context(s.roles.Get(owner), adjusted)
	ctx.PatternDifficulty = difficulty
	var successSeed int64
	for ; successSeed < 10000; successSeed++ {
		out, err := w.rules.ResolveForge(adjusted.BlankID, inputs, ctx, successSeed)
		if err != nil {
			t.Fatal(err)
		}
		if out.Success {
			if int64(out.Experience)+8 != roleInt(preview, "successExperience") || int64(out.Plan.Experience(false)) != roleInt(preview, "failureExperience") {
				t.Fatal("study credited on failure or missing on success", preview)
			}
			break
		}
	}
	if successSeed == 10000 {
		t.Fatal("expected a reachable first forge")
	}
	out, err := s.resolveSmithing(owner, req, successSeed)
	if err != nil {
		t.Fatal(err)
	}
	role := s.roles.Get(owner)
	earned := roleInt(out, "experience")
	if earned != roleInt(preview, "successExperience") || out["spentText"] != preview["costText"] ||
		out["factorText"] != preview["factorText"] || !strings.Contains(roleString(out, "trainingText"), "晋品！五品") ||
		roleInt(role, "ForgeRank") != 5 || roleInt(role, "Live3Level") != 5 || roleInt(role, "Live3Value") != earned-2 ||
		roleInt(role, "Live1Value") != 23 || roleInt(role, "Live2Value") != 44 {
		t.Fatal("third-slot promotion or receipt mismatch", out, role)
	}
	molds := role["ForgeMolds"].(Record)
	mold := molds[roleString(out, "moldId")].(Record)
	if roleInt(mold, "difficulty") != int64(difficulty) || roleInt(mold, "gradeId") != roleInt(preview, "gradeId") {
		t.Fatal("poor mold rewrote its original blueprint challenge", mold)
	}
	// A failed study may keep individual lots. Settle a partial return, then
	// check the quantities in the receipt against the actual bag and XP.
	failedOrder := req
	failedOrder.RequestID = strings.Repeat("8", 32)
	failedPlan := workshopCall(t, s, peer, smithingPlanCmd, failedOrder, true)
	ctx, _, _ = w.context(role, adjusted)
	ctx.PatternDifficulty = difficulty
	var failed Record
	for seed := int64(0); seed < 10000; seed++ {
		outcome, err := w.rules.ResolveForge(adjusted.BlankID, inputs, ctx, seed)
		if err != nil {
			t.Fatal(err)
		}
		returned := 0
		for _, n := range outcome.Returned {
			returned += n
		}
		if outcome.Success || returned == 0 || returned == outcome.Plan.Units {
			continue
		}
		failed, err = s.resolveSmithing(owner, failedOrder, seed)
		if err != nil {
			t.Fatal(err)
		}
		spent := make(map[string]int)
		bag := s.roles.Get(owner)
		for mat, n := range inputs {
			spent[mat] = n - outcome.Returned[mat]
			if forgeTestCount(role, mat)-forgeTestCount(bag, mat) != int64(spent[mat]) {
				t.Fatal("incorrect failed-study material deduction", failed)
			}
		}
		if roleBool(failed, "forged") || roleInt(failed, "studyExperience") != 0 || failed["spentText"] != w.materialUseText(spent) ||
			failed["returnedText"] != "收回 "+w.materialUseText(outcome.Returned) || roleInt(failed, "experience") != roleInt(failedPlan, "failureExperience") ||
			roleInt(bag, "Live3Value")-roleInt(role, "Live3Value") != int64(outcome.Plan.Experience(false)) {
			t.Fatal("failed study awarded a successful-study bonus or reported false material returns", failed)
		}
		break
	}
	if failed == nil {
		t.Fatal("expected a failed study with a partial material return")
	}
	if err := s.roles.Update(owner, func(r Record) error { r["ForgeRank"], r["ForgeExperience"] = 9, 39999; return nil }); err != nil {
		t.Fatal(err)
	}
	full := workshopCall(t, s, peer, smithingPlanCmd, req, true)
	if roleInt(full, "successExperience") != 1 || roleInt(full, "failureExperience") != 1 || roleInt(full, "studyExperience") != 0 {
		t.Fatal("overstated earnings at ninth rank cap", full)
	}
	replayed := workshopCall(t, s, peer, smithingCreateCmd, req, true)
	if replayed["trainingText"] != out["trainingText"] || replayed["factorText"] != out["factorText"] || roleInt(s.roles.Get(owner), "ForgeExperience") != 39999 {
		t.Fatal("replayed first forge refreshed or reawarded experience", replayed)
	}
	capped := s.roles.Get(owner)
	w.awardExperience(capped, 18)
	if !strings.Contains(w.trainingText(capped), "圆满") || w.earnedExperience(capped, 18) != 0 || roleInt(capped, "Live3Value") != 40000 || roleInt(capped, "Live3Level") != 9 {
		t.Fatal("ninth rank was reduced or over-awarded", capped)
	}
	t.Logf("blueprint %s, %s; gain %d of which %d from study; %s", roleString(pattern, "Name"), roleString(out, "difficultyName"), earned, roleInt(out, "studyExperience"), roleString(out, "trainingText"))
}
