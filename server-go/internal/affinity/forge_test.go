package affinity

import (
	"encoding/json"
	"fmt"
	"maps"
	"math"
	"reflect"
	"slices"
	"testing"
)

func forceSuccessfulContext(c *Catalog, gradeID int) ForgeContext {
	c.forging.tables.Factors.TargetGrades[gradeID] = ForgeTargetGrade{ForgeFactor: ForgeFactor{SuccessBPS: 100000}}
	return ForgeContext{SmithRank: 1, MethodElement: 1, MethodRank: 1, FlameID: c.ForgeFlames()[0], GradeID: gradeID}
}

func TestForgeSelectionAndSnapshots(t *testing.T) {
	c := loadBundled(t)
	const blank = "wp.minglei_spear"
	if !slices.Contains(c.ForgeBlanks(), blank) || !slices.Contains(c.ForgeMaterials(), "yun_tie_1") ||
		slices.Contains(c.ForgeBlanks(), "yun_tie_1") || slices.Contains(c.ForgeMaterials(), blank) {
		t.Fatal("incorrect selection of blanks or materials")
	}
	item, _ := c.Item("yun_tie_1")
	grade, _ := c.Grade(item.GradeID)
	affixes := c.ForgeAffixes("yun_tie_1")
	count := c.forging.tables.AffixCounts[grade.TierID]
	if len(affixes) != count {
		t.Fatalf("expected %d fixed affixes, got %v", count, affixes)
	}
	clear(affixes)
	if len(c.ForgeAffixes("yun_tie_1")) != count {
		t.Fatal("caller mutated the catalog")
	}
	inputs := map[string]int{"mat.cloudwood": 2, "mat.thunder_horn": 1}
	ctx := forceSuccessfulContext(c, 5)
	outcome, err := c.ResolveForge(blank, inputs, ctx, 43)
	if err != nil {
		t.Fatal(err)
	}
	if !outcome.Success || outcome.Weapon == nil {
		t.Fatal("expected successful craft")
	}
	got := *outcome.Weapon
	want, err := c.ResolveForge(blank, map[string]int{"mat.thunder_horn": 1, "mat.cloudwood": 2}, ctx, 43)
	if err != nil || !reflect.DeepEqual(outcome, want) {
		t.Fatalf("unstable roll %+v, %v", want, err)
	}
	delete(inputs, "mat.cloudwood")
	if got.Inputs["mat.cloudwood"] != 2 || got.PrototypeID != blank || got.CategoryID != "wp.spear" || got.CatalogVersion != c.Version() {
		t.Fatalf("incorrect snapshot %+v", got)
	}
	data, err := json.Marshal(got)
	if err != nil {
		t.Fatal(err)
	}
	var restored ForgedWeapon
	if err := json.Unmarshal(data, &restored); err != nil || !reflect.DeepEqual(got, restored) {
		t.Fatalf("snapshot round trip %v: %s", err, data)
	}
	for _, bad := range []map[string]int{
		nil, {"mat.cloudwood": 1}, {"mat.cloudwood": 0, "mat.thunder_horn": 2}, {"unknown": 2},
		{blank: 2}, {"mat.cloudwood": math.MaxInt}, {"mat.cloudwood": 7, "mat.thunder_horn": 6},
	} {
		if _, err := c.PlanForge(blank, bad, ctx); err == nil {
			t.Fatalf("accepted materials %v", bad)
		}
	}
	if _, err := c.PlanForge("not-a-blank", map[string]int{"yun_tie_1": 2}, ctx); err == nil {
		t.Fatal("accepted unknown blank")
	}
}

func TestForgeGradesRangesAndAffinity(t *testing.T) {
	c := loadBundled(t)
	tiers := make(map[int]bool)
	qualities := make(map[int]bool)
	rolling := make(map[string]bool)
	for _, mix := range []map[string]int{
		{"mat.chilie_sand": 12}, {"mat.hanwen_iron": 12},
		{"mat.chilie_sand": 6, "yun_tie_1": 6}, {"yun_tie_1": 12},
	} {
		for seed := int64(0); seed < 128; seed++ {
			gradeID := c.forging.gradeIDs[seed%12]
			ctx := forceSuccessfulContext(c, gradeID)
			out, err := c.ResolveForge("wp.minglei_spear", mix, ctx, seed)
			if err != nil {
				t.Fatal(err)
			}
			if !out.Success || out.Weapon.GradeID != gradeID {
				t.Fatalf("target grade was not produced: %+v", out)
			}
			w := out.Weapon
			grade, ok := c.Grade(w.GradeID)
			if !ok || grade.Legacy || len(w.Bonuses) != c.forging.tables.AffixCounts[grade.TierID] || len(w.BaseBonuses) != len(w.Bonuses) ||
				w.QiID < 1 || w.QiID > 12 || w.ClassID != fmt.Sprintf("affinity.p%d.e%d.q%02d", w.PolarityID, w.ElementID, w.QiID) ||
				w.BaseAttack <= 0 || w.BaseDefense <= 0 {
				t.Fatalf("invalid weapon %+v", w)
			}
			if _, ok := w.Bonuses[c.forging.tables.ElementFocus[w.ElementID]]; !ok {
				t.Fatalf("missing elemental focus: %+v", w)
			}
			for stat, value := range w.Bonuses {
				rangeBPS := c.forging.tables.AffixRanges[stat][grade.QualityID]
				if value < rangeBPS[0] || value > rangeBPS[1] {
					t.Fatalf("stat %s outside actual quality range %+v", stat, w)
				}
				if _, ok := w.BaseBonuses[stat]; !ok {
					t.Fatalf("missing base roll for %q", stat)
				}
				rolling[fmt.Sprintf("%s/%d", stat, value)] = true
			}
			if mix["mat.chilie_sand"] == 12 && (w.PolarityID != 2 || w.ElementID != 5) ||
				mix["mat.hanwen_iron"] == 12 && (w.PolarityID != 1 || w.ElementID != 1) {
				t.Fatalf("dominant bulk material did not affect finished affinity: %+v", w)
			}
			tiers[grade.TierID], qualities[grade.QualityID] = true, true
		}
	}
	if len(tiers) != 4 || len(qualities) != 3 || len(rolling) < 20 {
		t.Fatalf("insufficient variability: tiers=%v qualities=%v values=%d", tiers, qualities, len(rolling))
	}
}

func TestForgeIngredientAffixesInfluenceSelection(t *testing.T) {
	c := loadBundled(t)
	// Isolate one ingredient's fixed stats. The blank, grade, qi tables,
	// quantity and seed sequence stay identical across the two runs.
	seen := func(favored string) int {
		c.forging.tables.ItemAffixes["yun_tie_1"] = map[string]int{favored: 2000}
		n := 0
		for seed := int64(0); seed < 128; seed++ {
			out, err := c.ResolveForge("wp.minglei_spear", map[string]int{"yun_tie_1": 12}, forceSuccessfulContext(c, 13), seed)
			if err != nil {
				t.Fatal(err)
			}
			if _, ok := out.Weapon.Bonuses["healing"]; ok {
				n++
			}
		}
		return n
	}
	a, b := seen("healing"), seen("smithing")
	if a <= b {
		t.Fatalf("ingredient stats had no positive influence: %d vs %d", a, b)
	}
	// Every caller gets its own snapshot of affixes, suitable for bag and auction.
	copy := c.ForgeAffixes("yun_tie_1")
	if !maps.Equal(copy, map[string]int{"smithing": 2000}) {
		t.Fatal(copy)
	}
}

func TestForgePreviewAndFailure(t *testing.T) {
	c := loadBundled(t)
	const blank = "wp.minglei_spear"
	inputs := map[string]int{"mat.cloudwood": 2, "mat.thunder_horn": 1}
	ctx := ForgeContext{SmithRank: 1, FlameID: c.ForgeFlames()[0], GradeID: 1}
	plan, err := c.PlanForge(blank, inputs, ctx)
	if err != nil {
		t.Fatal(err)
	}
	again, err := c.PlanForge(blank, inputs, ctx)
	if err != nil || again != plan {
		t.Fatalf("non-deterministic preview: %v %v", again, err)
	}
	data, err := json.Marshal(plan)
	if err != nil {
		t.Fatal(err)
	}
	var preview map[string]any
	if err := json.Unmarshal(data, &preview); err != nil {
		t.Fatal(err)
	}
	if len(preview) != 5 || preview["successBps"] == nil || preview["gradeId"] == nil || preview["expBps"] == nil {
		t.Fatalf("preview exposed or omitted fields: %s", data)
	}
	for _, bad := range []ForgeContext{
		{SmithRank: 1, FlameID: ctx.FlameID, GradeID: 4}, {SmithRank: 0, FlameID: ctx.FlameID, GradeID: 1},
		{SmithRank: 1, FlameID: "unknown", GradeID: 1}, {SmithRank: 1, MethodElement: 2, FlameID: ctx.FlameID, GradeID: 1},
		{SmithRank: 1, MethodRank: 2, FlameID: ctx.FlameID, GradeID: 1},
	} {
		if _, err := c.PlanForge(blank, inputs, bad); err == nil {
			t.Fatalf("accepted invalid context %+v", bad)
		}
	}
	// Force the signed factor boundaries. Preview and resolution must agree
	// without exposing any result stat or refunding more than was selected.
	c.forging.tables.Factors.TargetGrades[1] = ForgeTargetGrade{ForgeFactor: ForgeFactor{SuccessBPS: -100000, ReturnBPS: 100000, ExpBPS: 10000}}
	plan, err = c.PlanForge(blank, inputs, ctx)
	if err != nil || plan.SuccessBPS != 0 || plan.ReturnBPS != 10000 {
		t.Fatalf("failure preview %+v %v", plan, err)
	}
	out, err := c.ResolveForge(blank, inputs, ctx, 5)
	wantExp := max(1, (2*(10000+plan.ExpBPS)+5000)/10000)
	if err != nil || out.Success || out.Weapon != nil || !maps.Equal(out.Returned, inputs) || out.Experience != wantExp {
		t.Fatalf("invalid failure settlement %+v %v", out, err)
	}
}

func TestEquippedMethodContributesToForgePlan(t *testing.T) {
	c := loadBundled(t)
	const blank = "wp.minglei_spear"
	inputs := map[string]int{"mat.hanwen_iron": 3} // blank and all materials are metal
	ctx := ForgeContext{SmithRank: 1, FlameID: c.ForgeFlames()[0], GradeID: 5}
	// Isolate the method from other percentage modifiers using the additive
	// experience stat, which has no 100% probability cap.
	for key := range c.forging.tables.Factors.ElementRelations {
		c.forging.tables.Factors.ElementRelations[key] = ForgeFactor{}
	}
	c.forging.tables.Factors.ElementRelations["same"] = ForgeFactor{ExpBPS: 100}
	c.forging.tables.Factors.ElementRelations["overcomes"] = ForgeFactor{ExpBPS: -50}
	c.methods["smithing"][qiAbsorptionMethod][1] = []int{3, 4}
	c.methods["smithing"][qiAbsorptionMethod][16] = []int{7, 3}
	none, _ := c.PlanForge(blank, inputs, ctx)
	ctx.MethodElement, ctx.MethodRank = 1, 1
	metal, _ := c.PlanForge(blank, inputs, ctx)
	ctx.MethodRank = 16
	strongMetal, _ := c.PlanForge(blank, inputs, ctx)
	ctx.MethodElement = 4 // fire overcomes metal
	fire, _ := c.PlanForge(blank, inputs, ctx)
	if metal.ExpBPS != none.ExpBPS+150 || strongMetal.ExpBPS != none.ExpBPS+466 || fire.ExpBPS != none.ExpBPS-233 {
		t.Fatalf("incorrect equipped method effects: %+v %+v %+v %+v", none, metal, strongMetal, fire)
	}
}

func TestForgeMasteryChallengeAndIngredientTradeoffs(t *testing.T) {
	c := loadBundled(t)
	const blank = "wp.minglei_spear"
	recipe := map[string]int{"mat.thunder_horn": 1, "mat.cloudwood": 2}
	preview := func(rank int, fire string, difficulty int, material map[string]int) ForgePlan {
		t.Helper()
		ctx := ForgeContext{SmithRank: rank, FlameID: fire, GradeID: 7, PatternDifficulty: difficulty}
		p, err := c.PlanForge(blank, material, ctx)
		if err != nil {
			t.Fatal(err)
		}
		var sum ForgeFactor
		for _, row := range p.Sources {
			sum = sum.add(row)
		}
		out, err := c.ResolveForge(blank, material, ctx, 818)
		if err != nil || out.Plan != p || p.ForgeFactor != sum {
			t.Fatalf("estimate and settlement differ: %v vs %v, rows sum to %v (%v)", p, out.Plan, sum, err)
		}
		return p
	}
	apprentice := preview(1, "flame.star", 5, recipe)
	ember := preview(1, "flame.ember", 5, recipe)
	master := preview(9, "flame.star", 5, recipe)
	if apprentice.FlameMastery.RequiredRank != 9 || apprentice.SuccessBPS <= 0 || apprentice.SuccessBPS >= ember.SuccessBPS ||
		apprentice.ExpBPS <= ember.ExpBPS || master.SuccessBPS <= apprentice.SuccessBPS || master.RollBiasBPS <= apprentice.RollBiasBPS ||
		master.ReturnBPS <= apprentice.ReturnBPS || master.Sources[FireControl] != (ForgeFactor{}) {
		t.Fatalf("fire/rank tradeoffs missing: apprentice %+v, ember %+v, master %+v", apprentice, ember, master)
	}
	if apprentice.Sources[PatternLoad] != (ForgeFactor{SuccessBPS: -1400, ReturnBPS: -600}) ||
		master.Sources[PatternLoad] != (ForgeFactor{SuccessBPS: -200}) ||
		preview(1, "flame.star", 1, recipe).Sources[PatternLoad] != (ForgeFactor{}) {
		t.Fatal("blueprint complexity did not respond to proficiency")
	}
	substitute := preview(1, "flame.star", 5, map[string]int{"mat.chilie_sand": 3})
	if apprentice.Matched != 3 || substitute.Matched != 0 || apprentice.Sources[RecipeMatch].RollBiasBPS <= substitute.Sources[RecipeMatch].RollBiasBPS ||
		apprentice.Sources[FlameAffinity] == substitute.Sources[FlameAffinity] || apprentice.Sources[MaterialGrade] == substitute.Sources[MaterialGrade] {
		t.Fatal("recipe, material grades and element relationships must track the actual inputs")
	}
	previous := apprentice.ForgeFactor
	for rank := 2; rank <= 9; rank++ {
		now := preview(rank, "flame.star", 5, recipe)
		if now.SuccessBPS < previous.SuccessBPS || now.ReturnBPS < previous.ReturnBPS || now.RollBiasBPS < previous.RollBiasBPS {
			t.Fatalf("rank %d lost a learned art", rank)
		}
		previous = now.ForgeFactor
	}
	t.Logf("grade 7 spear, challenge 5, success/return/bias/XP bps: rank 1 star %+v; rank 1 ember %+v; rank 9 star %+v; rank 1 star alternate sand %+v", apprentice.ForgeFactor, ember.ForgeFactor, master.ForgeFactor, substitute.ForgeFactor)
}

func TestWideMethodInfluence(t *testing.T) {
	factor := ForgeFactor{SuccessBPS: 200, ReturnBPS: -50, ExpBPS: 13}
	got, err := factor.scaleMethod(math.MaxInt, math.MaxInt)
	if err != nil || got != factor {
		t.Fatalf("equivalent ratio was not preserved: %+v %v", got, err)
	}
	if _, err = factor.scaleMethod(math.MaxInt, 1); err == nil {
		t.Fatal("unrepresentable method factor accepted")
	}
}

func TestForgeCountsFollowConfiguredTierTable(t *testing.T) {
	c := loadBundled(t)
	tables := c.forging.tables
	// A designer gives earth-tier objects one effect. Align the fixed item and
	// process affixes with this table before recompiling the catalog.
	tables.AffixCounts[3] = 1
	for id, affixes := range tables.ItemAffixes {
		if grade := c.grades[c.items[id].GradeID]; grade.TierID == 3 {
			keys := slices.Sorted(maps.Keys(affixes))
			tables.ItemAffixes[id] = map[string]int{keys[0]: affixes[keys[0]]}
		}
	}
	for id, target := range tables.Factors.TargetGrades {
		if c.grades[id].TierID == 3 {
			target.ForgeFactor = ForgeFactor{SuccessBPS: 100}
			tables.Factors.TargetGrades[id] = target
		}
	}
	for id, flame := range tables.Factors.Flames {
		if c.grades[flame.GradeID].TierID == 3 {
			flame.ForgeFactor = ForgeFactor{SuccessBPS: 100}
			tables.Factors.Flames[id] = flame
		}
	}
	var err error
	c.forging, err = compileForge(c, tables)
	if err != nil {
		t.Fatal(err)
	}
	out, err := c.ResolveForge("wp.minglei_spear", map[string]int{"yun_tie_1": 2}, forceSuccessfulContext(c, 9), 7)
	if err != nil || out.Weapon == nil || len(out.Weapon.Bonuses) != 1 {
		t.Fatalf("expected one earth-tier affix: %+v %v", out, err)
	}
}
