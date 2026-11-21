package main

import (
	"math"
	"strings"
	"testing"

	"google.golang.org/protobuf/proto"
)

func TestArtisanBatchQuantityAndLessons(t *testing.T) {
	for i, expected := range []int{1, 1, 1, 5, 5, 5, 10, 10, 10, 20} {
		if artisanCopyLimit(i) != expected {
			t.Fatal(i, expected)
		}
		if _, err := artisanBatchCount(expected+1, i); err == nil {
			t.Fatal("rank limit", i)
		}
	}
	s, p := alchemyTestServer(t, t.TempDir())
	w := s.alchemy
	req := brewRequest{Action: "plan", RecipeID: "dan.ninglu", AuxIDs: []string{sigilDew}, Quantity: 20}
	if err := s.roles.Update("artist", func(r Record) error {
		r["AlchemyRank"], r["CurrentVigor"], r["ShinyStone"] = 9, 6, 10000
		r["AlchemyJournal"] = Record{req.RecipeID: Record{"Quality": 3, "Potency": 160, "Mastery": 6}}
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	preview := brewCall(t, s, p, req, true)
	price := int(roleInt(preview, "shortageCost"))
	expected := 20 * (2*w.materialTerms(sigilHerb).BuyPrice + w.materialTerms(sigilDew).BuyPrice)
	if price != expected || roleInt(preview, "copyVigor") != 7 {
		t.Fatal(preview)
	}
	req.Action, req.ExpectedPrice, req.RequestID = "buyMissing", price, strings.Repeat("a", 32)
	brewCall(t, s, p, req, true)
	req.Action, req.RequestID = "replay", strings.Repeat("b", 32)
	brewCall(t, s, p, req, false)
	before := s.roles.Get("artist")
	if roleInt(before, "CurrentVigor") != 6 || forgeTestCount(before, sigilHerb) != 40 || forgeTestCount(before, sigilDew) != 20 {
		t.Fatal("failed batch spent resources")
	}
	if err := s.roles.Update("artist", func(r Record) error { r["CurrentVigor"] = 7; return nil }); err != nil {
		t.Fatal(err)
	}
	out := brewCall(t, s, p, req, true)
	again := brewCall(t, s, p, req, true)
	after := s.roles.Get("artist")
	n := roleInt(out, "quantity")
	if n < 20 || n > int64(20*w.Alchemy.Recipes[req.RecipeID].YieldCap) || n != roleInt(again, "quantity") || roleString(out, "quality") != "上品" || roleInt(out, "mastery") != 46 ||
		roleInt(after, "CurrentVigor") != 0 || roleInt(after, "ShinyStone") != int64(10000-price) || forgeTestCount(after, sigilHerb) != 0 || forgeTestCount(after, sigilDew) != 0 || forgeTestCount(after, w.Alchemy.Recipes[req.RecipeID].OutputID) != n {
		t.Fatal(out, after)
	}
	t.Logf("九品复炼20炉，采购%d灵石，扣40主药、20辅药、7精力，实收%d粒上品，方熟练6→46", price, n)
}

func TestArtisanSigilBulkAndMastery(t *testing.T) {
	s, p := workshopTestServer(t, t.TempDir())
	c, _ := sigilSelect("ward", "strong")
	if err := s.roles.Update("artist", func(r Record) error {
		r["TalismanRank"], r["CurrentVigor"], r["ShinyStone"] = 6, 9, 1000
		return nil
	}); err != nil {
		t.Fatal(err)
	}
	req := sigilRequest{Effect: c.effect.id, Seal: c.seal, Action: "supply", Quantity: 10, ExpectedPrice: 324, RequestID: newGameID(16)}
	sigilCall(t, s, p, req, true)
	for _, recipe := range []string{"paper", "ink", "ink"} {
		req.Action, req.Recipe, req.RequestID = "make", recipe, newGameID(16)
		sigilCall(t, s, p, req, true)
	}
	req.Action, req.RequestID = "copy", newGameID(16)
	sigilCall(t, s, p, req, false)
	req.Action, req.Quantity, req.Strokes, req.RequestID = "draw", 1, c.strokes(), newGameID(16)
	sigilCall(t, s, p, req, true)
	req.Action, req.Quantity, req.Strokes, req.RequestID = "copy", 10, nil, newGameID(16)
	for range 2 {
		sigilCall(t, s, p, req, true)
	}
	r := s.roles.Get("artist")
	if roleInt(r, "CurrentVigor") != 0 || roleInt(r, "ShinyStone") != 676 || forgeTestCount(r, c.key()) != 11 || forgeTestCount(r, sigilPaper) != 19 || forgeTestCount(r, sigilInk) != 18 || sigilMastery(r, c.key()) != 32 {
		t.Fatal(r)
	}
	req.Quantity, req.RequestID = 5, newGameID(16)
	sigilCall(t, s, p, req, false)
	if forgeTestCount(s.roles.Get("artist"), sigilPaper) != 19 {
		t.Fatal("failed batch debited")
	}
	field, _ := sigilSelect("field", "simple")
	sigilPractice(r, field.key(), 100)
	if field.power(r, false) != 2 {
		t.Fatal("mastered field pattern has no effect")
	}
	t.Logf("六品首张重封耗2精力，10张复写耗10纸20墨及7精力，职业经验共%d，本式熟练32", roleInt(r, "TalismanExperience"))
}

func TestLateAirDoseInventoryAndRawStock(t *testing.T) {
	s, p := alchemyTestServer(t, t.TempDir())
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	for _, kind := range []string{"ordinary", "flat", "percent"} {
		for _, level := range []int64{40, 60, 81, 90} {
			g := Record{"GoodsId": "old", "TemplateId": "pill_naqi", "Name": "纳气丹", "Type": 19, "Count": 3}
			limit := int64(s.levelInfo(level).upgradeLimit)
			nominal := limit * 8 / 100
			if kind == "flat" {
				g["BrewEffect"], g["EffectValue"], g["RealityEffectValue"] = "gain_super_air", 120, 120
				nominal = 120
			}
			if kind == "percent" {
				g["BrewEffect"], g["BrewBasis"], g["EffectValue"], g["RealityEffectValue"] = "gain_super_air", "realm_air", 395, 395
				nominal = s.elixirPoints(Record{"Level": level}, "realm_air", 395)
			}
			want := int64(math.Round(float64(nominal) / lateRealmTimeFactor(level)))
			if level == 90 {
				want = 0
			}
			if err := s.roles.Update("artist", func(r Record) error { r["Level"], r["SuperAir"], r["Goods"] = level, 0, []any{g}; return nil }); err != nil {
				t.Fatal(err)
			}
			for _, cmd := range []int{1004015, 1004024, 1004032} {
				info := gameInventoryProtocols[cmd]
				request := s.pb(info.req)
				key := "GoodsId"
				if cmd == 1004015 {
					key = "Id"
				}
				set(request, key, "old")
				data, _ := proto.Marshal(request)
				s.handleGame(p, Message{Cmd: cmd, Msid: 7, Data: data})
				out := p.conn.(*auctionTestConnection).receive(t, s, cmd, 7, info.resp)
				if cmd == 1004015 {
					if !getBool(out, "IsSuccess") || int64(math.Round(getFloat(out, "EffectValue"))) != want {
						t.Fatal("detail", kind, level, want, out)
					}
				} else if getBool(out, "IsSuccess") != (want > 0) {
					t.Fatal("consume", kind, level, out)
				}
			}
			r := s.roles.Get("artist")
			count := int64(1)
			if level == 90 {
				count = 3
			}
			if roleInt(r, "SuperAir") != 2*want || transferStackCount(locateGoods(r, "old")) != count {
				t.Fatal(kind, level, want, r)
			}
		}
	}
}

// A working day uses one matching herb batch, a plain protective copy and one
// manual forging attempt. Market values describe inventory opportunity cost.
func TestArtisanDailyRoutes(t *testing.T) {
	s, _ := alchemyTestServer(t, t.TempDir())
	cfg, err := readConfig(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	s.config = cfg
	if err = s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	designs := []string{"songying", "tingxue", "xianyue", "guichao", "baizhi", "chunshan", "feishuang", "liuhui", "jinglei"}
	for i, id := range designs {
		rank := i + 1
		pattern := s.smithing.patterns[smithingTemplateID("association.pattern.v1", id)]
		inputs := recipeQuantities(pattern["Inputs"].(Record))
		grade := int(roleInt(pattern, "Rank"))
		order := forgeRequest{PatternID: roleString(pattern, "TemplateId"), BlankID: roleString(pattern, "BlankId"), GradeID: grade}
		challenge := s.smithing.orderDifficulty(order, inputs)
		coins := 0
		unknown := ""
		for material, needed := range inputs {
			unit := s.alchemy.materialTerms(material).Value
			if unit == 0 {
				unknown += material + " "
			}
			coins += needed * unit
		}
		vigor := s.smithing.forgeVigor(grade, challenge.Level, 1, true)
		n := artisanCopyLimit(rank)
		l := int64(1 + 10*i)
		r := Record{"HasGold": true, "Level": l}
		lvl := s.levelInfo(l)
		pct := func(kind int) float64 {
			m := bestFreeMethod(s, r, kind)
			r["RunMask"] = 1 << (kind - 1)
			rate := s.gameRateFor(r, m, kind)
			if kind == 3 {
				return 100 * math.Min(float64(int64(lvl.upgradeLimit)-realmStartingAir(l, lvl)), math.Floor(900*rate)) / lvl.upgradeLimit
			}
			baseline := lvl.trainingBase(kind)
			return 100 * math.Min(float64(lvl.trainingCap(kind))-baseline, math.Floor(900*rate/2)) / baseline
		}
		t.Logf("L%d first fifteen minutes qi %.5f%% threshold, mind +%.4f%% body +%.4f%% base", l, pct(3), pct(1), pct(2))
		t.Logf("rank %d | forge %s | vigor %d | known material value %d | unpriced %s| daily combined cost %d", rank, roleString(pattern, "Name"), vigor, coins, unknown, vigor+artisanCopyVigor(n, 1)+1)
	}
}
