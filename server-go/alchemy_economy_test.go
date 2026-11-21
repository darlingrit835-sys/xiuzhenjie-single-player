package main

import (
	"encoding/json"
	"reflect"
	"strings"
	"testing"

	"google.golang.org/protobuf/proto"
)

func brewTestEconomy(w *alchemyRoom) {
	e := &brewEconomy{Materials: map[string]herbTerms{}, Recipes: map[string]struct{ SellPrices []int }{}}
	for _, id := range w.materials {
		e.Materials[id] = herbTerms{50, 50}
	}
	for _, id := range w.recipes {
		e.Recipes[id] = struct{ SellPrices []int }{[]int{5, 10, 15}}
	}
	w.Alchemy.Economy = e
}

func TestAlchemyMissingPurchaseAndBatchResale(t *testing.T) {
	s, p := alchemyTestServer(t, t.TempDir())
	w := s.alchemy
	brewTestEconomy(w)
	buy := brewRequest{Action: "buyMissing", RecipeID: "dan.ninglu", AuxIDs: []string{"mat.cold_dew"}, RequestID: strings.Repeat("e", 32), ExpectedPrice: 150}
	plan := buy
	plan.Action = "plan"
	estimate := brewCall(t, s, p, plan, true)
	if !roleBool(estimate, "canBuyMissing") || roleInt(estimate, "shortageCost") != 150 || roleInt(estimate, "sellPrice") != 10 {
		t.Fatal(estimate)
	}
	w.Alchemy.Economy.Materials["mat.cold_dew"] = herbTerms{50, 0}
	brewCall(t, s, p, buy, false)
	if forgeTestCount(s.roles.Get("artist"), "mat.ninglu_herb") != 0 {
		t.Fatal("partial herb delivery")
	}
	w.Alchemy.Economy.Materials["mat.cold_dew"] = herbTerms{50, 50}
	buy.ExpectedPrice = 100
	brewCall(t, s, p, buy, false)
	buy.ExpectedPrice = 150
	brewCall(t, s, p, buy, true)
	brewCall(t, s, p, buy, true)
	if roleInt(s.roles.Get("artist"), "ShinyStone") != 850 {
		t.Fatal("uncharged purchase or duplicate debit")
	}
	start := buy
	start.Action, start.RequestID = "start", strings.Repeat("c", 32)
	brewCall(t, s, p, start, true)
	run := brewTestRoll(t, s, 0)
	f := brewRequest{Action: "finish", RequestID: strings.Repeat("d", 32), RunID: start.RequestID, Taps: centeredTaps(run.Plan.Stages)}
	result := brewCall(t, s, p, f, true)
	count := roleInt(result, "quantity")
	if roleInt(result, "sellPrice") != 15 {
		t.Fatal(result)
	}
	req := s.pb("GoodsSellReq_1004022")
	for _, g := range battleRecords(s.roles.Get("artist")["Goods"]) {
		if roleString(g, "BrewRecipe") == "dan.ninglu" {
			putPB(addMessage(req, "GoodsInfos"), Record{"GoodsId": g["GoodsId"], "Count": count})
		}
	}
	data, _ := proto.Marshal(req)
	msg := Message{Cmd: 1004022, Msid: 7, Data: data}
	s.handleGame(p, msg)
	reply := auctionTestReply(t, s, p.conn.(*auctionTestConnection), msg, 4077)
	if getInt(reply, "AddShinyStone") != 15*count || roleInt(s.roles.Get("artist"), "ShinyStone") > 1000 {
		t.Fatal(reply)
	}
}

func TestAlchemyKitResaleBounds(t *testing.T) {
	s, _ := alchemyTestServer(t, t.TempDir())
	w := s.alchemy
	brewTestEconomy(w)
	for _, origin := range []string{"free", "paid", "sigil", "legacy", "garden", "counter"} {
		role := Record{"ShinyStone": 1000, "AlchemyRank": 9}
		unitLimit := -1
		switch origin {
		case "free", "paid":
			price := 0
			if origin == "paid" {
				price = brewSupplyPrice
				role["AlchemyStarterClaimed"] = true
			}
			if err := w.supply(role, price); err != nil {
				t.Fatal(err)
			}
			unitLimit = price / 17
		case "sigil":
			price := sigilSupplyPrice(Record{"TalismanStarter": true})
			if err := sigilReceivePaid(role, sigilStarter, price); err != nil {
				t.Fatal(err)
			}
			unitLimit = price / 12
		case "legacy", "garden":
			g := Record{"TemplateId": "mat.ninglu_herb", "Type": 12, "RecyclePrice": 0, "Count": 2}
			if origin == "garden" {
				g["BrewSource"] = "garden"
			} else {
				unitLimit = 0
			}
			if _, err := receiveInventoryGoods(role, g, 2); err != nil {
				t.Fatal(err)
			}
		case "counter":
			if err := w.fillHerb(role, "mat.ninglu_herb", 2, 50); err != nil {
				t.Fatal(err)
			}
			unitLimit = 50
		}
		// Check every usable recipe, so combining an entire bag into many recipes cannot beat its purchase price.
		for _, recipe := range w.recipes {
			p, _ := w.plan(role, brewRequest{RecipeID: recipe})
			stock := w.inventoryFor(role, p)
			r := w.Alchemy.Recipes[recipe]
			if stock.missing[r.PrimaryID] > 0 {
				continue
			}
			quote := w.actualPrice(p, stock, 3)
			if unitLimit >= 0 && quote*r.YieldCap > unitLimit*w.primaryQty(r) {
				t.Fatalf("%s %s may mint currency: %d", origin, recipe, quote)
			}
			if unitLimit < 0 && quote != 15 {
				t.Fatalf("%s earned herbs incorrectly discounted for %s: %d", origin, recipe, quote)
			}
		}
	}
}
func TestAlchemyScaledDoseAndProfessionalProgress(t *testing.T) {
	s, p := alchemyTestServer(t, t.TempDir())
	s.game.levels[1] = gameLevel{upgradeLimit: 100}
	s.game.levels[2] = gameLevel{upgradeLimit: 2000}
	r := Record{"Live2Type": 1, "Live2Level": 6, "Live2Value": 17}
	s.alchemy.train(r, 6, true)
	if roleInt(r, "Live2Value") != 330 || roleInt(r, "Live2Level") != 6 || roleInt(r, "AlchemyExperience") != 330 {
		t.Fatal(r)
	}
	for range 19 {
		s.alchemy.train(r, 6, true)
	}
	if roleInt(r, "AlchemyRank") != 7 || roleInt(r, "Live2Level") != 7 {
		t.Fatal(r)
	}
	if err := s.roles.Update("artist", func(r Record) error {
		r["Level"], r["CurrentPoisonVal"], r["SuperAir"] = 1, 98, 0
		r["Goods"] = []any{Record{"GoodsId": "dose", "Name": "试丹", "Type": 19, "Count": 3, "BrewEffect": "gain_super_air", "BrewBasis": "realm_air", "PoisonValue": 3, "EffectValue": 300, "RealityEffectValue": 300}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	call := func(command int, id string) int64 {
		info := gameInventoryProtocols[command]
		req := s.pb(info.req)
		if command == 1004015 {
			set(req, "Id", id)
		} else {
			set(req, "GoodsId", id)
		}
		b, _ := proto.Marshal(req)
		s.handleGame(p, Message{Cmd: command, Msid: 5, Data: b})
		out := p.conn.(*auctionTestConnection).receive(t, s, command, 5, info.resp)
		if command == 1004015 {
			if getInt(out, "PoisonValue") != 3 || len(out.GetUnknown()) == 0 {
				t.Fatal(out)
			}
			return int64(getFloat(out, "EffectValue"))
		}
		return getInt(out, "Code")
	}
	if got := call(1004015, "dose"); got != 3 {
		t.Fatal(got)
	}
	for _, cmd := range []int{1004024, 1004032} {
		before := s.roles.Get("artist")
		if call(cmd, "dose") != 4972 || !reflect.DeepEqual(before, s.roles.Get("artist")) {
			t.Fatal("toxicity refusal mutated role", cmd)
		}
	}
	s.roles.Update("artist", func(r Record) error { r["CurrentPoisonVal"] = 97; r["Level"] = 2; return nil })
	if call(1004015, "dose") != 60 || call(1004032, "dose") != 2000 {
		t.Fatal("basis didn't follow recipient realm")
	}
	after := s.roles.Get("artist")
	if roleInt(after, "CurrentPoisonVal") != 100 || roleInt(after, "SuperAir") != 60 || transferStackCount(locateGoods(after, "dose")) != 2 {
		t.Fatal(after)
	}
	s.roles.Update("artist", func(r Record) error { r["CurrentPoisonVal"] = 0; r["SuperAir"] = 2000; return nil })
	if call(1004024, "dose") != 4971 || roleInt(s.roles.Get("artist"), "CurrentPoisonVal") != 0 {
		t.Fatal("full recipient wasted a dose")
	}
	g := Record{"BrewEffect": "recover_blood", "BrewBasis": "max_blood", "EffectValue": 2500}
	r = Record{"PureBlood": 81, "CurrentPureBlood": 1}
	if s.consumeBrewDose(r, g) != 2000 || roleInt(r, "CurrentPureBlood") != 21 {
		raw, _ := json.Marshal(r)
		t.Fatal(string(raw))
	}
}

func TestAlchemyBundledRecipesAcrossRealms(t *testing.T) {
	s, _ := alchemyTestServer(t, t.TempDir())
	w := s.alchemy
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	if w.Alchemy.Economy == nil || len(w.recipes) < 20 {
		t.Fatal("missing expanded catalog")
	}
	bases := map[string]bool{}
	for _, id := range w.recipes {
		r := w.Alchemy.Recipes[id]
		bases[r.EffectBasis] = true
		for q := 1; q <= 3; q++ {
			role := Record{"Level": 90, "PureBlood": 8000, "CurrentPureBlood": 5, "Vigor": 2000, "CurrentVigor": 0, "CurrentPoisonVal": 15, "AlchemyRank": 9}
			g := Record{"TemplateId": r.PrimaryID, "Type": 12, "Rank": 1, "Level": 1, "RecyclePrice": 15}
			if _, err := receiveInventoryGoods(role, g, int64(w.primaryQty(r))); err != nil {
				t.Fatal(err)
			}
			p, err := w.plan(role, brewRequest{RecipeID: id})
			if err != nil {
				t.Fatal(err)
			}
			out, err := w.settle(role, p, 90, 9500, q, r.YieldCap, false)
			if err != nil {
				t.Fatal(id, err)
			}
			pill := battleRecords(role["Goods"])[0]
			if roleInt(pill, "RecyclePrice") != int64(w.recipePrices(id)[q-1]) || int64(s.brewDisplayValue(role, r, int(roleInt(pill, "EffectValue")))) != roleInt(out, "effect") || roleInt(pill, "PoisonValue") != int64(r.PoisonCost) {
				t.Fatal(id, pill, out)
			}
			if roleString(out, "effectBasis") != "flat" && !strings.Contains(roleString(out, "effectText"), "%") {
				t.Fatal("basis lost from receipt", id)
			}
			recipient := cloneRecord(role)
			recipient["Level"], recipient["PureBlood"] = 1, 100
			if s.consumeBrewDose(recipient, pill) != 2000 {
				t.Fatal(id, recipient, pill)
			}
			if r.EffectBasis == "realm_air" && roleInt(recipient, "SuperAir") > 100 || r.EffectBasis == "max_blood" && roleInt(recipient, "CurrentPureBlood") > 100 {
				t.Fatal("received high-level crafter's stats", id, recipient)
			}
		}
	}
	if !bases["realm_air"] || !bases["max_blood"] || !bases["flat"] {
		t.Fatal(bases)
	}
}
