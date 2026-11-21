package main

import (
	"reflect"
	"strings"
	"testing"
)

func TestForgeLargeOrderAndMaterialBackedRecycle(t *testing.T) {
	s, peer := workshopTestServer(t, t.TempDir())
	mold := smithingTemplateID("association.pattern.v1", "songying")
	req := forgeRequest{MoldID: mold, Quantity: 65, FlameID: "flame.spring", RequestID: strings.Repeat("a", 32),
		Inputs: []forgeInput{{"mat.cloudwood", 2}, {"mat.cold_dew", 1}}}
	if err := s.roles.Update("artist", func(r Record) error {
		r["ForgeRank"], r["Vigor"], r["CurrentVigor"] = 9, 100, 100
		for _, ingredient := range req.Inputs {
			_, err := receiveInventoryGoods(r, Record{"TemplateId": ingredient.TemplateID, "Name": ingredient.TemplateID, "Type": 12, "ForgePaidUnit": 12}, 200)
			if err != nil {
				return err
			}
		}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	before := s.roles.Get("artist")
	q := workshopCall(t, s, peer, smithingCastPlanCmd, req, true)
	upkeep, recycle := roleInt(q, "vigorCost"), roleInt(q, "recyclePrice")
	if upkeep <= 0 || upkeep > 100 || recycle <= 0 || recycle*65 > roleInt(q, "spentUnits")*12*88/100 || roleInt(q, "appraisal") < recycle || !roleBool(q, "canFit") {
		t.Fatal(q)
	}
	if !reflect.DeepEqual(before, s.roles.Get("artist")) {
		t.Fatal("quote mutated role")
	}
	if err := s.roles.Update("artist", func(r Record) error { r["CurrentVigor"] = upkeep - 1; return nil }); err != nil {
		t.Fatal(err)
	}
	low := s.roles.Get("artist")
	workshopCall(t, s, peer, smithingCastCmd, req, false)
	if !reflect.DeepEqual(low, s.roles.Get("artist")) {
		t.Fatal("insufficient vigor charged materials or created output")
	}
	if err := s.roles.Update("artist", func(r Record) error { r["CurrentVigor"] = upkeep; return nil }); err != nil {
		t.Fatal(err)
	}
	result := workshopCall(t, s, peer, smithingCastCmd, req, true)
	after := s.roles.Get("artist")
	ids := map[string]bool{}
	for _, v := range goodsList(after) {
		g, _ := v.(Record)
		if roleString(g, "TemplateId") == mold {
			ids[roleString(g, "GoodsId")] = true
			if roleInt(g, "RecyclePrice") != recycle {
				t.Fatal("item did not retain quoted salvage")
			}
		}
	}
	if len(ids) != 65 || roleInt(after, "CurrentVigor") != 0 || roleInt(result, "vigorCost") != upkeep || roleInt(result, "experience") < 1 || roleInt(after, "ForgeExperience") != roleInt(result, "experience") {
		t.Fatal("incorrect batch size or vigor")
	}
	workshopCall(t, s, peer, smithingCastCmd, req, true)
	if !reflect.DeepEqual(after, s.roles.Get("artist")) {
		t.Fatal("replayed order charged twice")
	}
	ref := s.smithing.patterns[mold]
	_, free := s.smithing.quoteForgedValue(ref, 0, 65)
	if free != 0 {
		t.Fatal("free inputs could produce NPC cash")
	}
	t.Logf("65-piece order: %d units, %d vigor, %d stone appraisal, %d stone per NPC recycle; %d unique equipment", roleInt(q, "spentUnits"), upkeep, roleInt(q, "appraisal"), recycle, len(ids))
}
