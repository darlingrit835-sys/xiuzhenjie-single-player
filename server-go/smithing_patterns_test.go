package main

import (
	"bytes"
	"fmt"
	"os"
	"path/filepath"
	"reflect"
	"strings"
	"testing"
)

func TestWorkshopPublicPatternsInNormalGame(t *testing.T) {
	dir := t.TempDir()
	if err := os.CopyFS(filepath.Join(dir, "config"), os.DirFS("../server/data/config")); err != nil {
		t.Fatal(err)
	}
	start := func() *Server {
		s, err := openServer(Config{}, dir)
		if err != nil {
			t.Fatal(err)
		}
		if s.preview != nil || s.previewWrites {
			t.Fatal("requires the ordinary server configuration")
		}
		return s
	}
	s := start()
	const owner = "new-smith"
	p, _ := auctionTestRole(t, s, owner, nil)
	entry := spellTestCall(t, s, p, 1008066, "", "EnterEquipmentAssociationResp_1008066", nil)
	if !getBool(entry, "IsSuccess") || !bytes.Equal(entry.GetUnknown(), offerNumber(nil, 4, 1)) {
		t.Fatalf("association did not expose the workshop: %v", entry)
	}
	list := func(p *Peer) []Record {
		body := workshopCall(t, s, p, smithingCatalogCmd, forgeRequest{}, true)
		if len(battleRecords(body["molds"])) != 0 {
			t.Fatal("public patterns require no personal molds", body["molds"])
		}
		return battleRecords(body["patterns"])
	}
	rows := list(p)
	if err := s.roles.Update(owner, func(r Record) error { r["Level"] = 1; return nil }); err != nil {
		t.Fatal(err)
	}
	advancedID := smithingTemplateID("association.pattern.v1", "feishuang")
	workshopCall(t, s, p, smithingSampleCmd, forgeRequest{TemplateID: advancedID, RequestID: newGameID(16)}, false)
	if err := s.roles.Update(owner, func(r Record) error { r["Level"] = 31; return nil }); err != nil {
		t.Fatal(err)
	}
	wantNames := map[string]string{}
	for blank, names := range map[string]string{
		"made.frost_sword": "听雪剑 裂缺剑 飞霜剑 流辉剑", "wp.tingquan_staff": "松影杖 白芷杖 春山杖 栖霞杖",
		"wp.duanxia_saber": "归潮刀 破岩刀 赤霄刀", "wp.minglei_spear": "青霜枪 金翎枪 镇泽枪 惊雷枪", "wp.zhufeng_bow": "弦月弓 逐影弓 寒芦弓",
		"obj.yunwen_robe": "云纹法衣", "obj.tafeng_boots": "踏风灵靴", "obj.hanguang_mirror": "寒光护心镜",
	} {
		for _, name := range strings.Fields(names) {
			wantNames[name] = blank
		}
	}
	if len(rows) != len(wantNames) {
		t.Fatalf("got %d public patterns, want %d", len(rows), len(wantNames))
	}
	ids := map[string]bool{}
	for i, row := range rows {
		name, id := roleString(row, "name"), roleString(row, "id")
		if id == "" || ids[id] || wantNames[name] == "" || roleString(row, "blankId") != wantNames[name] || roleBool(row, "claimed") {
			t.Fatalf("unexpected or duplicate pattern %s (%s): %v", name, id, row)
		}
		ids[id] = true
		delete(wantNames, name)
		for _, key := range []string{"grade", "quality", "element", "traits", "material", "effect", "recipe"} {
			if strings.TrimSpace(roleString(row, key)) == "" {
				t.Fatalf("%s: missing %s", name, key)
			}
		}
		if roleInt(row, "attack") <= 0 || roleInt(row, "defense") <= 0 {
			t.Fatalf("%s: %v", name, row)
		}
		var inputs []forgeInput
		for _, mat := range battleRecords(row["inputs"]) {
			item, qty := roleString(mat, "templateId"), int(roleInt(mat, "quantity"))
			if !strings.HasPrefix(item, "mat.") || qty < 1 {
				t.Fatalf("%s: invalid material %v", name, mat)
			}
			inputs = append(inputs, forgeInput{TemplateID: item, Quantity: qty})
		}
		if len(inputs) != 2 || inputs[0].TemplateID == inputs[1].TemplateID {
			t.Fatalf("%s: %v", name, inputs)
		}
		claim := forgeRequest{TemplateID: id, RequestID: fmt.Sprintf("%032x", i+1)}
		sample := workshopCall(t, s, p, smithingSampleCmd, claim, true)
		if roleString(sample, "templateId") != id || roleString(sample, "creator") != "炼器协会" {
			t.Fatal(sample)
		}
		if repeated := workshopCall(t, s, p, smithingSampleCmd, claim, true); repeated["goodsId"] != sample["goodsId"] {
			t.Fatal("reclaim duplicated sample", repeated)
		}
		g := locateGoods(s.roles.Get(owner), roleString(sample, "goodsId"))
		kind, detailCmd := int64(15), 1004018
		switch name {
		case "云纹法衣":
			kind, detailCmd = 26, 1004019
		case "踏风灵靴":
			kind, detailCmd = 29, 1004014
		case "寒光护心镜":
			kind, detailCmd = 16, 1004014
		}
		if !roleBool(g, "IsBinding") || !isSmithingEquipment(g) || roleInt(g, "Type") != kind ||
			roleInt(row, "slotType") != kind || roleInt(sample, "slotType") != kind || roleString(g, "BlankId") != roleString(row, "blankId") ||
			roleInt(g, "BaseAttack") != roleInt(row, "attack") || roleInt(g, "BaseDefense") != roleInt(row, "defense") ||
			len(g["Bonuses"].(Record)) == 0 || len(g["MaterialAffixes"].(Record)) == 0 {
			t.Fatalf("%s: sample lost catalog attributes: %v", name, g)
		}
		detail := smithingInventoryCall(t, s, p, detailCmd, Record{"Id": id})
		if !getBool(detail, "IsSuccess") || getString(detail, "CreatedBy") != "炼器协会" {
			t.Fatal("template detail unavailable", id, detail)
		}
		batch := forgeRequest{MoldID: id, Quantity: 2, Inputs: inputs, RequestID: fmt.Sprintf("%032x", i+100)}
		// Add only the advertised recipe multiplied by two; each batch must consume it all.
		if err := s.roles.Update(owner, func(r Record) error {
			for _, mat := range inputs {
				_, err := receiveInventoryGoods(r, Record{"TemplateId": mat.TemplateID, "Type": 19, "Name": mat.TemplateID}, int64(2*mat.Quantity))
				if err != nil {
					return err
				}
			}
			return nil
		}); err != nil {
			t.Fatal(err)
		}
		before := s.roles.Get(owner)
		plan := workshopCall(t, s, p, smithingCastPlanCmd, batch, true)
		if !reflect.DeepEqual(before, s.roles.Get(owner)) || roleInt(plan, "maxByMaterial") != 2 || !roleBool(plan, "canFit") {
			t.Fatalf("%s preview mutated inventory or mispriced materials: %v", name, plan)
		}
		cast := workshopCall(t, s, p, smithingCastCmd, batch, true)
		if cast["templateId"] != id || roleInt(cast, "slotType") != kind || roleInt(cast, "baseAttack") != roleInt(row, "attack") || roleInt(cast, "baseDefense") != roleInt(row, "defense") ||
			cast["materialText"] != row["material"] || cast["bonusText"] != row["traits"] || cast["effectText"] != row["effect"] || cast["goodsId"] == sample["goodsId"] {
			t.Fatalf("%s batch different from catalog: %v", name, cast)
		}
		after := s.roles.Get(owner)
		if forgeTestCount(after, id) != 3 || roleBool(locateGoods(after, roleString(cast, "goodsId")), "IsBinding") {
			t.Fatal("bound sample and two independent salable pieces expected", name)
		}
		for _, m := range inputs {
			if forgeTestCount(after, m.TemplateID) != 0 {
				t.Fatal("material mismatch", name, m)
			}
		}
		if replay := workshopCall(t, s, p, smithingCastCmd, batch, true); replay["goodsId"] != cast["goodsId"] || !reflect.DeepEqual(after, s.roles.Get(owner)) {
			t.Fatal("replayed batch changed inventory", name)
		}
		if kind != 15 {
			if roleInt(row, "defense") <= roleInt(row, "attack") {
				t.Fatal("protective pattern should favor defense", row)
			}
			move := Record{"EquipmentId": roleString(cast, "goodsId")}
			if !getBool(spellTestCall(t, s, p, 1009001, "DressEquipmentReq_1009001", "DressEquipmentResp_1009001", move), "IsSuccess") {
				t.Fatal("cast piece could not be equipped", name)
			}
			worn := battleRecords(s.roles.Get(owner)["EquippedGoods"])
			if len(worn) != 1 || roleInt(worn[0], "Type") != kind || roleString(worn[0], "TemplateId") != id {
				t.Fatal("cast piece did not reach its slot", name, worn)
			}
			if !getBool(spellTestCall(t, s, p, 1009002, "UndressEquipmentReq_1009002", "UndressEquipmentResp_1009002", move), "IsSuccess") {
				t.Fatal("cast piece could not be taken off", name)
			}
		}
	}
	if len(wantNames) != 0 {
		t.Fatal("patterns missing", wantNames)
	}
	// A previously issued edition can have different stats from today's generator.
	firstID := roleString(rows[0], "id")
	issuedAttack := roleInt(rows[0], "attack") + 1
	if err := s.roles.Update(smithingStorePrefix+firstID, func(r Record) error {
		r["BaseAttack"] = issuedAttack
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	s = start()
	p, _ = auctionTestRole(t, s, owner, nil)
	for _, row := range list(p) {
		if row["id"] == firstID && roleInt(row, "attack") != issuedAttack {
			t.Fatal("catalog preview lost the issued edition's stats")
		}
		if !roleBool(row, "claimed") {
			t.Fatal("sample eligibility lost on restart", row)
		}
	}
	if len(goodsList(s.roles.Get(owner))) != 3*len(rows) {
		t.Fatal("unexpected size of inventory after restart")
	}
	other, _ := auctionTestRole(t, s, "guest", nil)
	for _, row := range list(other) {
		if roleBool(row, "claimed") {
			t.Fatal("sample entitlement leaked across roles")
		}
	}
	first := rows[0]
	id := roleString(first, "id")
	batch := forgeRequest{MoldID: id, Quantity: 1, RequestID: strings.Repeat("a", 32)}
	for _, v := range battleRecords(first["inputs"]) {
		batch.Inputs = append(batch.Inputs, forgeInput{roleString(v, "templateId"), int(roleInt(v, "quantity"))})
	}
	if err := s.roles.Update("guest", func(r Record) error {
		for _, mat := range batch.Inputs {
			if _, e := receiveInventoryGoods(r, Record{"TemplateId": mat.TemplateID, "Type": 19}, int64(mat.Quantity)); e != nil {
				return e
			}
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	workshopCall(t, s, other, smithingCastCmd, batch, true)
	if roleBool(list(other)[0], "claimed") || forgeTestCount(s.roles.Get("guest"), id) != 1 {
		t.Fatal("casting public pattern consumed free sample entitlement")
	}
	gift := workshopCall(t, s, other, smithingSampleCmd, forgeRequest{TemplateID: id, RequestID: strings.Repeat("b", 32)}, true)
	if !roleBool(locateGoods(s.roles.Get("guest"), roleString(gift, "goodsId")), "IsBinding") || forgeTestCount(s.roles.Get(owner), id) != 3 {
		t.Fatal("sample not independent across roles")
	}
}

func TestWorkshopPublicSampleFullBagRollsBack(t *testing.T) {
	s, p := workshopTestServer(t, t.TempDir())
	s.preview, s.previewWrites = nil, false
	roleID := p.currentRoleID()
	rows := battleRecords(workshopCall(t, s, p, smithingCatalogCmd, forgeRequest{}, true)["patterns"])
	if len(rows) == 0 {
		t.Fatal("no public patterns")
	}
	id := roleString(rows[0], "id")
	req := forgeRequest{TemplateID: id, RequestID: strings.Repeat("1", 32)}
	if err := s.roles.Update(roleID, func(r Record) error {
		goods := make([]any, ringCapacity(r))
		for i := range goods {
			goods[i] = Record{"GoodsId": fmt.Sprintf("filled-%d", i), "Type": 15, "Count": 1}
		}
		r["Goods"] = goods
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	before := s.roles.Get(roleID)
	workshopCall(t, s, p, smithingSampleCmd, req, false)
	if !reflect.DeepEqual(before, s.roles.Get(roleID)) || s.smithingTemplate(id) != nil {
		t.Fatal("full bag committed entitlement or template")
	}
	if err := s.roles.Update(roleID, func(r Record) error { removeGoods(r, "filled-0"); return nil }); err != nil {
		t.Fatal(err)
	}
	claim := workshopCall(t, s, p, smithingSampleCmd, req, true)
	if roleString(claim, "goodsId") == "" || forgeTestCount(s.roles.Get(roleID), id) != 1 {
		t.Fatal("could not reclaim after freeing a single slot", claim)
	}
	// Leave exactly one free slot and two ingredient stacks that will not be emptied.
	// The first piece fits, so this specifically exercises a mid-batch inventory rollback.
	order := forgeRequest{MoldID: id, Quantity: 2, RequestID: strings.Repeat("2", 32)}
	for _, v := range battleRecords(rows[0]["inputs"]) {
		order.Inputs = append(order.Inputs, forgeInput{roleString(v, "templateId"), int(roleInt(v, "quantity"))})
	}
	if err := s.roles.Update(roleID, func(r Record) error {
		for i := 1; i <= 3; i++ {
			removeGoods(r, fmt.Sprintf("filled-%d", i))
		}
		for _, mat := range order.Inputs {
			if _, err := receiveInventoryGoods(r, Record{"TemplateId": mat.TemplateID, "Type": 19}, int64(2*mat.Quantity+1)); err != nil {
				return err
			}
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	before = s.roles.Get(roleID)
	plan := workshopCall(t, s, p, smithingCastPlanCmd, order, true)
	if roleBool(plan, "canFit") || roleInt(plan, "freeSlots") != 1 || roleInt(plan, "maxByMaterial") < 2 {
		t.Fatal("incorrect full-bag projection", plan)
	}
	workshopCall(t, s, p, smithingCastCmd, order, false)
	if !reflect.DeepEqual(before, s.roles.Get(roleID)) {
		t.Fatal("public batch partially consumed ingredients or granted a piece")
	}
	if err := s.roles.Update(roleID, func(r Record) error { removeGoods(r, "filled-4"); return nil }); err != nil {
		t.Fatal(err)
	}
	out := workshopCall(t, s, p, smithingCastCmd, order, true)
	if roleString(out, "goodsId") == "" || forgeTestCount(s.roles.Get(roleID), id) != 3 {
		t.Fatal("same batch could not retry once both slots were free", out)
	}

}

func TestStarterBundleFurnishesThreeWearablePatterns(t *testing.T) {
	s, player := workshopTestServer(t, t.TempDir())
	s.preview, s.previewWrites = nil, false
	workshopCall(t, s, player, smithingSupplyCmd, forgeRequest{RequestID: strings.Repeat("a", 32)}, true)
	patterns := battleRecords(workshopCall(t, s, player, smithingCatalogCmd, forgeRequest{}, true)["patterns"])
	for _, row := range patterns {
		kind := roleInt(row, "slotType")
		if kind == 15 {
			continue
		}
		order := forgeRequest{MoldID: roleString(row, "id"), Quantity: 1, RequestID: fmt.Sprintf("%032x", kind)}
		for _, input := range battleRecords(row["inputs"]) {
			order.Inputs = append(order.Inputs, forgeInput{roleString(input, "templateId"), int(roleInt(input, "quantity"))})
		}
		piece := workshopCall(t, s, player, smithingCastCmd, order, true)
		wear := spellTestCall(t, s, player, 1009001, "DressEquipmentReq_1009001", "DressEquipmentResp_1009001", Record{"EquipmentId": piece["goodsId"]})
		if !getBool(wear, "IsSuccess") {
			t.Fatalf("could not wear %v", row["name"])
		}
	}
	role := s.roles.Get(player.currentRoleID())
	if len(battleRecords(role["EquippedGoods"])) != 3 || len(goodsList(role)) != 1 || forgeTestCount(role, "mat.hanwen_iron") != 1 {
		t.Fatal("expected the three protective pieces equipped with one iron left over")
	}
}
