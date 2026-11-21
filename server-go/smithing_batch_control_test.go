package main

import (
	"crypto/sha256"
	"encoding/json"
	"fmt"
	"os"
	"path/filepath"
	"reflect"
	"strings"
	"testing"
)

func TestBatchFireQuoteSettlementAndLegacyReplay(t *testing.T) {
	dir := t.TempDir()
	if err := os.CopyFS(filepath.Join(dir, "config"), os.DirFS("../server/data/config")); err != nil {
		t.Fatal(err)
	}
	open := func() *Server {
		s, err := openServer(Config{}, dir)
		if err != nil {
			t.Fatal(err)
		}
		if s.preview != nil || s.previewWrites {
			t.Fatal("expected normal server")
		}
		return s
	}
	s := open()
	const owner = "batch-smith"
	p, _ := auctionTestRole(t, s, owner, nil)
	const wood, dew = "mat.cloudwood", "mat.cold_dew"
	id := smithingTemplateID("association.pattern.v1", "songying")
	order := forgeRequest{MoldID: id, Quantity: 8, Inputs: []forgeInput{{wood, 2}, {dew, 1}}, FlameID: "flame.spring", RequestID: strings.Repeat("9", 32)}
	setRank := func(rank int) {
		t.Helper()
		if err := s.roles.Update(owner, func(r Record) error {
			r["ForgeRank"], r["ForgeExperience"] = rank, 13
			return nil
		}); err != nil {
			t.Fatal(err)
		}
	}
	quote := func(o forgeRequest) Record { return workshopCall(t, s, p, smithingCastPlanCmd, o, true) }
	rowFor := func(plan Record, mat string) Record {
		t.Helper()
		for _, row := range battleRecords(plan["costs"]) {
			if roleString(row, "templateId") == mat {
				return row
			}
		}
		t.Fatalf("missing ingredient %s in %v", mat, plan)
		return nil
	}
	setRank(1)
	for _, sample := range []struct {
		key         string
		level, rank int
	}{
		{"guichao", 3, 5},
		{"jinglei", 5, 9},
	} {
		publicOrder := order
		publicOrder.MoldID = smithingTemplateID("association.pattern.v1", sample.key)
		template := s.smithing.patterns[publicOrder.MoldID]
		recipe, _ := template["Inputs"].(Record)
		publicOrder.Inputs = nil
		for id, qty := range recipeQuantities(recipe) {
			publicOrder.Inputs = append(publicOrder.Inputs, forgeInput{id, qty})
		}
		result := quote(publicOrder)
		advice := roleString(result, "castingText")
		wantRank := fmt.Sprintf("器图建议 %d 品（尚差 %d 品）", sample.rank, sample.rank-1)
		if roleInt(result, "difficulty") != int64(sample.level) || roleInt(result, "suggestedSmithRank") != int64(sample.rank) ||
			!strings.Contains(advice, wantRank) || !strings.Contains(advice, "青泉阴焰建议 3 品（尚差 2 品）") {
			t.Fatal("public pattern and selected fire need independent rank advice", sample, result)
		}
		setRank(sample.rank)
		if ready := roleString(quote(publicOrder), "castingText"); !strings.Contains(ready, fmt.Sprintf("器图建议 %d 品（已达标）", sample.rank)) {
			t.Fatal("target rank should master its pattern", sample, ready)
		}
		setRank(1)
	}
	novice := quote(order)
	// Holding the same recipe and fire constant isolates the diagram pressure.
	// A master should be able to run either plan at the same efficiency.
	source := cloneRecord(s.smithing.patterns[id])
	price := func(level, rank int) int64 {
		t.Helper()
		patternChallenge{level, "测试器图", "同方比较"}.DescribeInto(source)
		result := Record{}
		_, err := s.smithing.priceBatch(Record{"ForgeRank": rank}, source, order, map[string]int{wood: 2, dew: 1}, result)
		if err != nil {
			t.Fatal(err)
		}
		return roleInt(rowFor(result, wood), "savingPercent")
	}
	if price(5, 1) >= price(1, 1) || price(5, 9) != price(1, 9) {
		t.Fatal("diagram difficulty did not distinguish novice and mastered control")
	}
	setRank(9)
	master := quote(order)
	if roleInt(master, "difficulty") < 1 || roleString(master, "difficultyText") == "" || roleInt(master, "smithRank") != 9 ||
		roleInt(master, "savedUnits") <= roleInt(novice, "savedUnits") || roleString(master, "castingText") == "" {
		t.Fatalf("rank / pattern difficulty absent: novice %v master %v", novice, master)
	}
	waterWood, waterDew := rowFor(master, wood), rowFor(master, dew)
	if waterWood["relationText"] != "火生其材" || waterDew["relationText"] != "火材同相" || roleInt(waterWood, "saved") < 1 || roleInt(waterDew, "saved") < 1 {
		t.Fatal("material affinities absent", waterWood, waterDew)
	}
	alternate := order
	alternate.FlameID = "flame.ember"
	ember := quote(alternate)
	if roleInt(rowFor(ember, wood), "savingPercent") >= roleInt(waterWood, "savingPercent") || roleInt(ember, "spentUnits") <= roleInt(master, "spentUnits") {
		t.Fatal("changing the fire did not change actual material quantities", master, ember)
	}
	for _, key := range []string{"templateId", "gradeId", "baseAttack", "baseDefense", "effectText", "materialText", "bonusText"} {
		if master[key] != novice[key] || ember[key] != master[key] {
			t.Fatal("heat changed the product", key, master, ember)
		}
	}
	one := order
	one.Quantity = 1
	if view := quote(one); roleInt(view, "baseUnits") != 3 || roleInt(view, "spentUnits") != 3 || roleInt(view, "savedUnits") != 0 {
		t.Fatal(view)
	}
	legacy := order
	legacy.FlameID = ""
	if view := quote(legacy); roleInt(view, "spentUnits") != 24 || roleInt(view, "savedUnits") != 0 {
		t.Fatal(view)
	}
	invalid := order
	invalid.FlameID = "unknown"
	workshopCall(t, s, p, smithingCastPlanCmd, invalid, false)
	if err := s.roles.Update(owner, func(r Record) error {
		r["Goods"] = []any{}
		for _, row := range battleRecords(master["costs"]) {
			mat := roleString(row, "templateId")
			_, err := receiveInventoryGoods(r, Record{"TemplateId": mat, "Name": mat, "Type": 19}, roleInt(row, "spent"))
			if err != nil {
				return err
			}
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	stock := s.roles.Get(owner)
	plan := quote(order)
	if !reflect.DeepEqual(stock, s.roles.Get(owner)) || roleInt(plan, "maxByMaterial") != int64(order.Quantity) ||
		!roleBool(plan, "canFit") || roleInt(plan, "freeSlots") != int64(ringCapacity(stock)) {
		t.Fatal("incorrect discounted quote", plan)
	}
	workshopCall(t, s, p, smithingCastCmd, alternate, false)
	if !reflect.DeepEqual(stock, s.roles.Get(owner)) {
		t.Fatal("failed hotter order withdrew materials")
	}
	out := workshopCall(t, s, p, smithingCastCmd, order, true)
	for _, k := range []string{"costs", "baseUnits", "spentUnits", "savedUnits", "costText", "maxByMaterial", "canFit"} {
		if !reflect.DeepEqual(plan[k], out[k]) {
			t.Fatal("priced and billed differently", k, plan[k], out[k])
		}
	}
	after := s.roles.Get(owner)
	if forgeTestCount(after, wood) != 0 || forgeTestCount(after, dew) != 0 || forgeTestCount(after, id) != int64(order.Quantity) ||
		roleInt(after, "ForgeExperience") != 13 || roleInt(out, "experience") != 0 {
		t.Fatal("stock / experience mismatch", out)
	}
	workshopCall(t, s, p, smithingCastCmd, alternate, false)
	if !reflect.DeepEqual(after, s.roles.Get(owner)) {
		t.Fatal("changed fire on replay mutated inventory")
	}

	// A new process must return the original receipt even if the player has since
	// changed rank and no longer has any of the ingredients.
	setRank(2)
	s = open()
	p, _ = auctionTestRole(t, s, owner, nil)
	replayed := workshopCall(t, s, p, smithingCastCmd, order, true)
	if !reflect.DeepEqual(out, replayed) || roleInt(s.roles.Get(owner), "ForgeRank") != 2 {
		t.Fatal("replay recalculated cost", replayed)
	}

	// Before flame-aware batches, both UI and command-line clients used the same
	// signature even when the UI sent a flame. Replay the actual old wire format.
	oldOrder := order
	oldOrder.RequestID = strings.Repeat("d", 32)
	inputs, _ := selectForgeInputs(oldOrder)
	signature, _ := json.Marshal([]any{oldOrder.MoldID, inputs, oldOrder.Quantity})
	receiptKey := smithingStorePrefix + "batch." + smithingTemplateID(owner, oldOrder.RequestID)
	if err := s.roles.Transact(func(tx *StoreTx) error {
		r := tx.GetOrCreate(receiptKey)
		r["RequestHash"] = fmt.Sprintf("%x", sha256.Sum256(signature))
		r["Result"] = Record{"forged": true, "goodsId": "old-id", "quantity": oldOrder.Quantity}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	if old := workshopCall(t, s, p, smithingCastCmd, oldOrder, true); old["goodsId"] != "old-id" {
		t.Fatal("lost pre-upgrade receipt", old)
	}
	oldOrder.Quantity++
	workshopCall(t, s, p, smithingCastCmd, oldOrder, false)
}

func TestBatchSavingRoundsPerMaterialAndPerOrder(t *testing.T) {
	for units := 1; units <= 12; units++ {
		for rate := 0; rate <= 50; rate++ {
			prev := 0
			for pieces := 1; pieces <= maxForgeBatch; pieces++ {
				used, saved := batchMaterialCost(units, pieces, rate)
				if used < units || used < prev || used+saved != units*pieces || saved != (pieces-1)*units*rate/100 {
					t.Fatal(units, rate, pieces, used, saved)
				}
				prev = used
			}
		}
	}
}
