package affinity

import (
	"maps"
	"reflect"
	"slices"
	"strings"
	"testing"
)

func alchemyContext(rank int, flame string) AlchemyContext {
	return AlchemyContext{AlchemistRank: rank, FlameID: flame}
}

func mustPlan(t *testing.T, c *Catalog, recipe string, ctx AlchemyContext) AlchemyPlan {
	t.Helper()
	plan, err := c.PlanAlchemy(recipe, ctx)
	if err != nil {
		t.Fatalf("plan %s: %v", recipe, err)
	}
	return plan
}

// The reference preview (tools/preview_alchemy.py) is the source of every
// number below; run it with --json to reproduce them.
func TestAlchemyChiyangMatchesPreview(t *testing.T) {
	c := loadBundled(t)
	ctx := alchemyContext(4, "flame.solar")
	ctx.MethodElement, ctx.MethodRank = 4, 9
	plan := mustPlan(t, c, "dan.chiyang", ctx)

	if plan.PrimaryElementID != 4 || plan.OutputID != "obj.chiyang_pill" || plan.EffectID != "recover_vigor" {
		t.Fatalf("unexpected target %+v", plan)
	}
	if plan.SuccessBPS != 7946 || plan.YieldMultiplierBPS != 13020 {
		t.Fatalf("success/multiplier %d/%d, want 7946/13020", plan.SuccessBPS, plan.YieldMultiplierBPS)
	}
	if want := map[int]int{2: 3960, 3: 6980, 4: 9060}; !maps.Equal(plan.YieldTickets, want) ||
		plan.YieldTotalTickets != 20000 {
		t.Fatalf("yield tickets %v total %d, want %v total 20000", plan.YieldTickets, plan.YieldTotalTickets, want)
	}
	if plan.PotencyBPS != [2]int{10608, 11200} || plan.EffectRange != [2]int{69, 73} {
		t.Fatalf("potency %v effect %v, want [10608 11200] [69 73]", plan.PotencyBPS, plan.EffectRange)
	}
	if len(plan.Adjustments) != 4 {
		t.Fatalf("adjustments %+v, want rank/flame/flameElement/methodElement", plan.Adjustments)
	}
	// 紫霞郡第 7 周期 add 75 bps in the worked example.
	withPlace := ctx
	withPlace.PlaceSuccess, withPlace.PlaceRegionID = 75, 1211
	placed := mustPlan(t, c, "dan.chiyang", withPlace)
	if placed.SuccessBPS != 8021 || !reflect.DeepEqual(placed.YieldTickets, plan.YieldTickets) {
		t.Fatalf("placed success %d, want 8021", placed.SuccessBPS)
	}
}

func TestAlchemySpringFlameLowesOdds(t *testing.T) {
	c := loadBundled(t)
	ctx := alchemyContext(4, "flame.spring")
	ctx.MethodElement, ctx.MethodRank = 4, 9
	plan := mustPlan(t, c, "dan.chiyang", ctx)

	if plan.SuccessBPS != 7546 || plan.YieldMultiplierBPS != 11620 ||
		!maps.Equal(plan.YieldTickets, map[int]int{2: 6760, 3: 8380, 4: 4860}) ||
		plan.PotencyBPS != [2]int{10663, 11200} || plan.EffectRange != [2]int{69, 73} {
		t.Fatalf("unexpected plan %+v", plan)
	}
	// 青泉阴焰 is water against a fire primary: 水克火.
	if plan.Adjustments[2].RelationID != relationOvercomes || plan.Adjustments[2].SuccessBPS != -160 {
		t.Fatalf("flame element adjustment %+v", plan.Adjustments[2])
	}
	withPlace := ctx
	withPlace.PlaceSuccess = 75
	if got := mustPlan(t, c, "dan.chiyang", withPlace).SuccessBPS; got != 7621 {
		t.Fatalf("placed success %d, want 7621", got)
	}
}

func TestAlchemyWeightsSecondaryElements(t *testing.T) {
	c := loadBundled(t)
	plan := mustPlan(t, c, "dan.shuangmai", alchemyContext(4, "flame.spring"))

	// 霜节芦 (water-metal) together with 玄水鳞 (water) yields a mixed
	// relation count: 75% primary / 25% weighted components.
	if plan.PrimaryElementID != 1 || plan.SuccessBPS != 6997 || plan.YieldMultiplierBPS != 11695 ||
		!maps.Equal(plan.YieldTickets, map[int]int{1: 8305, 2: 8305, 3: 3390}) ||
		plan.PotencyBPS != [2]int{12058, 12800} || plan.EffectRange != [2]int{29, 31} {
		t.Fatalf("unexpected plan %+v", plan)
	}
	if plan.Adjustments[2].AlchemyModifiers != (AlchemyModifiers{SuccessBPS: 87, YieldBPS: 195, PotencyBPS: 68}) {
		t.Fatalf("weighted affinity %+v, want 87/195/68", plan.Adjustments[2].AlchemyModifiers)
	}
}

func TestAlchemyRankZeroRecipe(t *testing.T) {
	c := loadBundled(t)
	plan := mustPlan(t, c, "dan.ninglu", alchemyContext(0, "flame.ember"))

	if plan.SuccessBPS != 8360 || plan.YieldMultiplierBPS != 10072 ||
		!maps.Equal(plan.YieldTickets, map[int]int{2: 9856, 3: 9928, 4: 216}) ||
		plan.PotencyBPS != [2]int{11300, 12800} || plan.EffectRange != [2]int{90, 102} {
		t.Fatalf("unexpected plan %+v", plan)
	}
	if plan.Adjustments[2].AlchemyModifiers != (AlchemyModifiers{SuccessBPS: 0, YieldBPS: 22, PotencyBPS: 0}) {
		t.Fatalf("weighted affinity %+v, want 0/22/0", plan.Adjustments[2].AlchemyModifiers)
	}
}

func TestAlchemyRejectsImpossibleBatches(t *testing.T) {
	c := loadBundled(t)
	for _, test := range []struct {
		name   string
		recipe string
		ctx    AlchemyContext
	}{
		{"unknown recipe", "dan.missing", alchemyContext(4, "flame.solar")},
		{"unknown flame", "dan.chiyang", alchemyContext(4, "flame.missing")},
		{"rank above nine", "dan.chiyang", alchemyContext(10, "flame.solar")},
		{"negative rank", "dan.chiyang", alchemyContext(-1, "flame.solar")},
		{"below recipe rank", "dan.chiyang", alchemyContext(2, "flame.ember")},
		{"below flame rank", "dan.chiyang", alchemyContext(4, "flame.star")},
		{"bad method element", "dan.chiyang", AlchemyContext{AlchemistRank: 4, FlameID: "flame.solar", MethodElement: 7}},
		{"method grade without element", "dan.chiyang", AlchemyContext{AlchemistRank: 4, FlameID: "flame.solar", MethodRank: 9}},
		{"unknown method grade", "dan.chiyang", AlchemyContext{AlchemistRank: 4, FlameID: "flame.solar", MethodElement: 4, MethodRank: 17}},
	} {
		if _, err := c.PlanAlchemy(test.recipe, test.ctx); err == nil {
			t.Errorf("%s accepted", test.name)
		}
	}
}

func TestAlchemyResolveIsStableAndBounded(t *testing.T) {
	c := loadBundled(t)
	ctx := alchemyContext(4, "flame.solar")
	ctx.MethodElement, ctx.MethodRank = 4, 9
	plan := mustPlan(t, c, "dan.chiyang", ctx)

	first, err := c.ResolveAlchemy("dan.chiyang", ctx, 43)
	if err != nil {
		t.Fatal(err)
	}
	again, err := c.ResolveAlchemy("dan.chiyang", ctx, 43)
	if err != nil || !reflect.DeepEqual(first, again) {
		t.Fatalf("unstable roll %+v %v", again, err)
	}

	successes := 0
	const runs = 2000
	for seed := int64(0); seed < runs; seed++ {
		out, err := c.ResolveAlchemy("dan.chiyang", ctx, seed)
		if err != nil {
			t.Fatal(err)
		}
		if !out.Success {
			if out.Count != 0 {
				t.Fatalf("failed batch produced %d pills", out.Count)
			}
			continue
		}
		successes++
		if out.Count < 1 || out.Count > 4 {
			t.Fatalf("count %d outside 1-4", out.Count)
		}
		if out.PotencyBPS < plan.PotencyBPS[0] || out.PotencyBPS > plan.PotencyBPS[1] {
			t.Fatalf("potency %d outside %v", out.PotencyBPS, plan.PotencyBPS)
		}
		if out.EffectValue < plan.EffectRange[0] || out.EffectValue > plan.EffectRange[1] {
			t.Fatalf("effect %d outside %v", out.EffectValue, plan.EffectRange)
		}
	}
	rate := float64(successes) / runs
	if rate < 0.77 || rate > 0.83 {
		t.Fatalf("success rate %.3f, want near 0.795", rate)
	}
}

func TestAlchemyCatalogValidation(t *testing.T) {
	for _, test := range []struct {
		name   string
		mutate func(*alchemyTables)
	}{
		{"inconsistent limits", func(t *alchemyTables) { t.Limits.SuccessMaxBPS = t.Limits.SuccessMinBPS - 1 }},
		{"missing rank", func(t *alchemyTables) { delete(t.Ranks, 5) }},
		{"missing relation", func(t *alchemyTables) { delete(t.Relations, relationGenerates) }},
		{"unknown flame", func(t *alchemyTables) {
			t.Flames["flame.unknown"] = AlchemyFlame{MinRank: 1}
		}},
		{"flame min rank out of range", func(t *alchemyTables) {
			flame := t.Flames["flame.ember"]
			flame.MinRank = 10
			t.Flames["flame.ember"] = flame
		}},
		{"duplicate output", func(t *alchemyTables) {
			recipe := t.Recipes["dan.ninglu"]
			recipe.OutputID = "obj.chiyang_pill"
			t.Recipes["dan.ninglu"] = recipe
		}},
		{"primary not a component", func(t *alchemyTables) {
			recipe := t.Recipes["dan.chiyang"]
			recipe.PrimaryID = "mat.cold_dew"
			t.Recipes["dan.chiyang"] = recipe
		}},
		{"recipe off the output map", func(t *alchemyTables) { delete(t.RecipeByOutput, "obj.chiyang_pill") }},
		{"quality range unordered", func(t *alchemyTables) { t.PotencyByQuality[2] = []int{11200, 10000} }},
	} {
		c := loadBundled(t)
		tables := c.alchemy.tables
		tables.Ranks = maps.Clone(tables.Ranks)
		tables.Flames = maps.Clone(tables.Flames)
		tables.Relations = maps.Clone(tables.Relations)
		tables.Recipes = maps.Clone(tables.Recipes)
		tables.RecipeByOutput = maps.Clone(tables.RecipeByOutput)
		tables.PotencyByQuality = maps.Clone(tables.PotencyByQuality)
		test.mutate(&tables)
		if _, err := compileAlchemy(c, tables); err == nil {
			t.Errorf("%s accepted", test.name)
		}
	}
}

func TestAlchemyCatalogWithoutRules(t *testing.T) {
	c, err := Load(strings.NewReader(sample))
	if err != nil {
		t.Fatalf("catalog without alchemy rules must still load: %v", err)
	}
	if _, err := c.PlanAlchemy("dan.ninglu", alchemyContext(0, "flame.ember")); err == nil {
		t.Fatal("expected a missing-rule error")
	}
	if c.AlchemyRecipes() != nil {
		t.Fatal("catalog without alchemy rules listed recipes")
	}
}

// SettleAlchemy is the seam the 1005027 handler uses: it must accept the item
// template the client sends, reject reports that cannot belong to the recipe and
// never trust a client-supplied success flag.
func TestAlchemySettleValidatesClientReport(t *testing.T) {
	c := loadBundled(t)
	ctx := alchemyContext(4, "flame.solar")
	ctx.MethodElement, ctx.MethodRank = 4, 9

	good := []AlchemyStep{
		{StepID: "step_wash", GoodsID: "mat.ember_moss", BestTime: 1.2},
		{StepID: "step_grind", GoodsID: "mat.sunstone", BestTime: 0.8},
		{StepID: "step_fire", GoodsID: "mat.flame_fang", BestTime: 2.5},
		{StepID: "step_control", BestTime: 1.0},
	}
	_, recipeID, err := c.SettleAlchemy("obj.chiyang_pill", ctx, good, 11)
	if err != nil || recipeID != "dan.chiyang" {
		t.Fatalf("settle by output failed: %q %v", recipeID, err)
	}
	if _, id, err := c.SettleAlchemy("dan.chiyang", ctx, good, 11); err != nil || id != "dan.chiyang" {
		t.Fatalf("settle by recipe key failed: %q %v", id, err)
	}

	for _, test := range []struct {
		name  string
		key   string
		steps []AlchemyStep
	}{
		{"unknown key", "obj.missing_pill", good},
		{"no steps", "dan.chiyang", nil},
		{"foreign material", "dan.chiyang", append(slices.Clone(good), AlchemyStep{StepID: "x", GoodsID: "mat.cold_dew"})},
		{"negative timing", "dan.chiyang", append(slices.Clone(good), AlchemyStep{StepID: "x", BestTime: -1})},
	} {
		if _, _, err := c.SettleAlchemy(test.key, ctx, test.steps, 11); err == nil {
			t.Errorf("%s accepted", test.name)
		}
	}

	// The rank gate belongs to the recipe, not to the client.
	low := alchemyContext(2, "flame.ember")
	if _, _, err := c.SettleAlchemy("dan.chiyang", low, good, 11); err == nil {
		t.Fatal("recipe rank gate was skipped")
	}

	// The same report settles identically; success is recomputed, not echoed.
	first, _, err := c.SettleAlchemy("dan.chiyang", ctx, good, 43)
	if err != nil {
		t.Fatal(err)
	}
	again, _, err := c.SettleAlchemy("dan.chiyang", ctx, good, 43)
	if err != nil || !reflect.DeepEqual(first, again) {
		t.Fatalf("unstable settlement %+v %v", again, err)
	}
}

func TestAlchemyUnitsFeedTheVigorCost(t *testing.T) {
	c := loadBundled(t)
	for _, test := range []struct {
		recipe string
		units  int
	}{
		{"dan.ninglu", 3}, {"dan.chiyang", 4}, {"dan.shuangmai", 4},
	} {
		_, recipe, ok := c.RecipeByKey(test.recipe)
		if !ok {
			t.Fatalf("unknown recipe %q", test.recipe)
		}
		if got := AlchemyUnits(c, recipe); got != test.units {
			t.Errorf("%s units %d, want %d", test.recipe, got, test.units)
		}
	}
}
