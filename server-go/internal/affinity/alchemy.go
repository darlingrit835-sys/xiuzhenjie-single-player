package affinity

import (
	"fmt"
	"math"
	"math/bits"
	"math/rand"
	"slices"
)

// Alchemy mirrors the smithing pipeline: a plan exposes the process and the
// target, a resolve consumes nothing and only rolls the batch. The numbers come
// from the 丹方表 / 丹师工艺 / 丹火工艺 / 炼丹生克 / 炼丹药效区间 / 炼丹结算 sheets,
// exported into the catalog under "alchemy".
const alchemyMaxRank = 9

const (
	relationSame        = "same"
	relationGenerates   = "generates"
	relationOvercomes   = "overcomes"
	relationGeneratedBy = "generatedBy"
	relationOvercomeBy  = "overcomeBy"
	relationNeutral     = "neutral"
)

// AlchemyModifiers are basis points; 100 bps is one percentage point.
type AlchemyModifiers struct {
	SuccessBPS int `json:"successBps"`
	YieldBPS   int `json:"yieldBps"`
	PotencyBPS int `json:"potencyBps"`
}

type AlchemyRecipe struct {
	OutputID   string `json:"outputId"`
	PrimaryID  string `json:"primaryId"`
	EffectID   string `json:"effectId"`
	MinRank    int    `json:"minRank"`
	SuccessBPS int    `json:"successBps"`
	MinQty     int    `json:"minQty"`
	MaxQty     int    `json:"maxQty"`
	YieldCap   int    `json:"yieldCap"`
	BaseEffect int    `json:"baseEffect"`
}

type AlchemyFlame struct {
	MinRank int `json:"minRank"`
	AlchemyModifiers
}

type alchemyLimits struct {
	PrimaryWeightBPS      int `json:"primaryWeightBps"`
	SuccessMinBPS         int `json:"successMinBps"`
	SuccessMaxBPS         int `json:"successMaxBps"`
	YieldMultiplierMinBPS int `json:"yieldMultiplierMinBps"`
	YieldMultiplierMaxBPS int `json:"yieldMultiplierMaxBps"`
}

type alchemyTables struct {
	Limits           alchemyLimits               `json:"limits"`
	PotencyByQuality map[int][]int               `json:"potencyByQuality"`
	Ranks            map[int]AlchemyModifiers    `json:"ranks"`
	Flames           map[string]AlchemyFlame     `json:"flames"`
	Relations        map[string]AlchemyModifiers `json:"relations"`
	Effects          map[string]struct{}         `json:"effects"`
	Recipes          map[string]AlchemyRecipe    `json:"recipes"`
	RecipeByOutput   map[string]string           `json:"recipeByOutput"`
}

type alchemyIndex struct {
	tables alchemyTables
}

// AlchemyContext carries what the character brings to the furnace. PlaceSuccess
// is the 炉址地利 success bonus already resolved by the region module; 0 means
// no place bonus.
type AlchemyContext struct {
	AlchemistRank int    `json:"alchemistRank"`
	FlameID       string `json:"flameId"`
	MethodElement int    `json:"methodElement"` // 0 = no equipped qi-absorption method
	MethodRank    int    `json:"methodRank"`
	PlaceSuccess  int    `json:"placeSuccessBps"`
	PlaceRegionID int    `json:"placeRegionId"`
}

// An adjustment records one additive source so the UI can show where the odds
// came from, mirroring the offline preview.
type AlchemyAdjustment struct {
	SourceID   string `json:"sourceId"`
	RelationID string `json:"relationId,omitempty"`
	ElementID  int    `json:"elementId,omitempty"`
	RegionID   int    `json:"regionId,omitempty"`
	AlchemyModifiers
}

type AlchemyPlan struct {
	RecipeID           string              `json:"recipeId"`
	OutputID           string              `json:"outputId"`
	EffectID           string              `json:"effectId"`
	PrimaryElementID   int                 `json:"primaryElementId"`
	Adjustments        []AlchemyAdjustment `json:"adjustments"`
	SuccessBPS         int                 `json:"successBps"`
	YieldMultiplierBPS int                 `json:"yieldMultiplierBps"`
	YieldTickets       map[int]int         `json:"yieldTickets"`
	YieldTotalTickets  int                 `json:"yieldTotalTickets"`
	PotencyBPS         [2]int              `json:"potencyBps"`
	EffectRange        [2]int              `json:"effectRange"`
}

// AlchemyOutcome is the settled batch. Persist it with the recipe key, the
// output template and the rule version so a later balance change cannot alter
// pills already in a bag.
type AlchemyOutcome struct {
	Success     bool        `json:"success"`
	Count       int         `json:"count"`
	PotencyBPS  int         `json:"potencyBps"`
	EffectValue int         `json:"effectValue"`
	Plan        AlchemyPlan `json:"plan"`
}

func (m AlchemyModifiers) add(other AlchemyModifiers) AlchemyModifiers {
	return AlchemyModifiers{
		SuccessBPS: m.SuccessBPS + other.SuccessBPS,
		YieldBPS:   m.YieldBPS + other.YieldBPS,
		PotencyBPS: m.PotencyBPS + other.PotencyBPS,
	}
}

// truncScale multiplies each field by numerator/denominator, truncating toward
// zero. The intermediate product stays wide so equivalent ratios like
// MaxInt/MaxInt still scale exactly.
func (m AlchemyModifiers) truncScale(numerator, denominator int) (AlchemyModifiers, error) {
	if denominator <= 0 || numerator < 0 {
		return AlchemyModifiers{}, fmt.Errorf("alchemy scaling needs a positive ratio")
	}
	for _, field := range [...]*int{&m.SuccessBPS, &m.YieldBPS, &m.PotencyBPS} {
		value := *field
		negative := value < 0
		if negative {
			value = -value
		}
		hi, lo := bits.Mul64(uint64(value), uint64(numerator))
		if hi >= uint64(denominator) {
			return AlchemyModifiers{}, fmt.Errorf("alchemy method influence exceeds arithmetic range")
		}
		part, _ := bits.Div64(hi, lo, uint64(denominator))
		if part > uint64(math.MaxInt/32) {
			return AlchemyModifiers{}, fmt.Errorf("alchemy method influence exceeds arithmetic range")
		}
		if negative {
			*field = -int(part)
		} else {
			*field = int(part)
		}
	}
	return m, nil
}

func (m AlchemyModifiers) valid() bool {
	// Bound the arithmetic, including a quantity-weighted sum and method scaling.
	const limit = math.MaxInt / 1024
	for _, v := range [...]int{m.SuccessBPS, m.YieldBPS, m.PotencyBPS} {
		if v < -limit || v > limit {
			return false
		}
	}
	return true
}

// truncRatio divides toward zero, matching the exported spreadsheet rules.
func truncRatio(numerator, denominator int64) int64 {
	if denominator == 0 {
		return 0
	}
	return numerator / denominator
}

func compileAlchemy(c *Catalog, tables alchemyTables) (*alchemyIndex, error) {
	limits := tables.Limits
	if limits.PrimaryWeightBPS < 0 || limits.PrimaryWeightBPS > 10000 {
		return nil, fmt.Errorf("alchemy primary weight %d out of range", limits.PrimaryWeightBPS)
	}
	if limits.SuccessMinBPS > limits.SuccessMaxBPS || limits.SuccessMaxBPS <= 0 ||
		limits.YieldMultiplierMinBPS <= 0 || limits.YieldMultiplierMinBPS > limits.YieldMultiplierMaxBPS {
		return nil, fmt.Errorf("alchemy limits are inconsistent")
	}
	for quality := 1; quality <= 3; quality++ {
		bounds, ok := tables.PotencyByQuality[quality]
		if !ok || len(bounds) != 2 || bounds[0] < 1 || bounds[0] > bounds[1] {
			return nil, fmt.Errorf("invalid alchemy potency range for quality %d", quality)
		}
	}
	if len(tables.PotencyByQuality) != 3 {
		return nil, fmt.Errorf("alchemy potency ranges must cover qualities 1-3")
	}
	for rank := 0; rank <= alchemyMaxRank; rank++ {
		if factor, ok := tables.Ranks[rank]; !ok || !factor.valid() {
			return nil, fmt.Errorf("missing or invalid alchemy rank %d", rank)
		}
	}
	if len(tables.Ranks) != alchemyMaxRank+1 {
		return nil, fmt.Errorf("alchemy ranks must cover 0-%d", alchemyMaxRank)
	}
	for _, key := range []string{relationSame, relationGenerates, relationOvercomes, relationGeneratedBy, relationOvercomeBy, relationNeutral} {
		if factor, ok := tables.Relations[key]; !ok || !factor.valid() {
			return nil, fmt.Errorf("missing or invalid alchemy relation %q", key)
		}
	}
	if len(tables.Flames) == 0 {
		return nil, fmt.Errorf("no alchemy flames configured")
	}
	if c.forging == nil {
		return nil, fmt.Errorf("alchemy needs the forge flame catalogue")
	}
	for id, flame := range tables.Flames {
		if _, ok := c.forging.tables.Factors.Flames[id]; !ok {
			return nil, fmt.Errorf("alchemy flame %q is not a registered forge flame", id)
		}
		if flame.MinRank < 0 || flame.MinRank > alchemyMaxRank || !flame.valid() {
			return nil, fmt.Errorf("invalid alchemy flame %q", id)
		}
	}
	if len(c.methods["alchemy"][qiAbsorptionMethod]) == 0 {
		return nil, fmt.Errorf("missing alchemy qi-absorption method influences")
	}
	if len(tables.Effects) == 0 {
		return nil, fmt.Errorf("no alchemy effects configured")
	}
	if len(tables.Recipes) == 0 {
		return nil, fmt.Errorf("no alchemy recipes configured")
	}
	byOutput := make(map[string]string, len(tables.Recipes))
	for id, recipe := range tables.Recipes {
		item, ok := c.items[recipe.OutputID]
		if !ok {
			return nil, fmt.Errorf("alchemy recipe %q: unknown output %q", id, recipe.OutputID)
		}
		profile, ok := c.profiles[item.ProfileID]
		if !ok || profile.KindID != "pill" {
			return nil, fmt.Errorf("alchemy recipe %q: output %q must be a pill template", id, recipe.OutputID)
		}
		grade, ok := c.grades[item.GradeID]
		if !ok || grade.Legacy {
			return nil, fmt.Errorf("alchemy recipe %q: output %q needs a regular grade", id, recipe.OutputID)
		}
		if _, found := byOutput[recipe.OutputID]; found {
			return nil, fmt.Errorf("alchemy recipe %q: output %q already has a recipe", id, recipe.OutputID)
		}
		inputs := make(map[string]int)
		for material, quantity := range c.Inputs(recipe.OutputID) {
			inputs[material] = quantity
		}
		primary, ok := inputs[recipe.PrimaryID]
		if !ok || primary <= 0 {
			return nil, fmt.Errorf("alchemy recipe %q: primary %q is not a component", id, recipe.PrimaryID)
		}
		primaryItem, ok := c.items[recipe.PrimaryID]
		if !ok {
			return nil, fmt.Errorf("alchemy recipe %q: unknown primary %q", id, recipe.PrimaryID)
		}
		primaryProfile, ok := c.profiles[primaryItem.ProfileID]
		if !ok || primaryProfile.PrimaryElementID < 1 || primaryProfile.PrimaryElementID > 5 {
			return nil, fmt.Errorf("alchemy recipe %q: primary %q has no primary element", id, recipe.PrimaryID)
		}
		if _, ok := tables.Effects[recipe.EffectID]; !ok {
			return nil, fmt.Errorf("alchemy recipe %q: unknown effect %q", id, recipe.EffectID)
		}
		if recipe.MinRank < 0 || recipe.MinRank > alchemyMaxRank ||
			recipe.SuccessBPS < 0 || recipe.SuccessBPS > 10000 ||
			recipe.MinQty < 1 || recipe.MaxQty < recipe.MinQty || recipe.YieldCap < recipe.MaxQty ||
			recipe.BaseEffect < 1 {
			return nil, fmt.Errorf("alchemy recipe %q has an invalid range", id)
		}
		if tables.RecipeByOutput[recipe.OutputID] != id {
			return nil, fmt.Errorf("alchemy recipe %q: recipeByOutput disagrees", id)
		}
		byOutput[recipe.OutputID] = id
	}
	if len(tables.RecipeByOutput) != len(tables.Recipes) {
		return nil, fmt.Errorf("alchemy recipeByOutput must list every recipe once")
	}
	for output, id := range tables.RecipeByOutput {
		if byOutput[output] != id {
			return nil, fmt.Errorf("alchemy recipeByOutput points %q at unknown recipe %q", output, id)
		}
	}
	return &alchemyIndex{tables: tables}, nil
}

// AlchemyRecipes lists recipe keys in ascending order.
func (c *Catalog) AlchemyRecipes() []string {
	if c.alchemy == nil {
		return nil
	}
	keys := make([]string, 0, len(c.alchemy.tables.Recipes))
	for key := range c.alchemy.tables.Recipes {
		keys = append(keys, key)
	}
	slices.Sort(keys)
	return keys
}

func (c *Catalog) AlchemyRecipe(id string) (AlchemyRecipe, bool) {
	if c.alchemy == nil {
		return AlchemyRecipe{}, false
	}
	recipe, ok := c.alchemy.tables.Recipes[id]
	return recipe, ok
}

func (c *Catalog) AlchemyRecipeForOutput(outputID string) (string, AlchemyRecipe, bool) {
	if c.alchemy == nil {
		return "", AlchemyRecipe{}, false
	}
	id, ok := c.alchemy.tables.RecipeByOutput[outputID]
	if !ok {
		return "", AlchemyRecipe{}, false
	}
	return id, c.alchemy.tables.Recipes[id], true
}

func (c *Catalog) AlchemyFlames() []string {
	if c.alchemy == nil {
		return nil
	}
	keys := make([]string, 0, len(c.alchemy.tables.Flames))
	for id := range c.alchemy.tables.Flames {
		keys = append(keys, id)
	}
	slices.Sort(keys)
	return keys
}

func (c *Catalog) AlchemyRank(rank int) (AlchemyModifiers, bool) {
	if c.alchemy == nil {
		return AlchemyModifiers{}, false
	}
	factor, ok := c.alchemy.tables.Ranks[rank]
	return factor, ok
}

func (c *Catalog) alchemyRelation(source, target int) (string, AlchemyModifiers) {
	tables := c.alchemy.tables
	key := relationNeutral
	if source != 0 && target != 0 {
		cycles := c.forging.tables.Elements
		switch {
		case source == target:
			key = relationSame
		case cycles[source].Generates == target:
			key = relationGenerates
		case cycles[source].Overcomes == target:
			key = relationOvercomes
		case cycles[target].Generates == source:
			key = relationGeneratedBy
		case cycles[target].Overcomes == source:
			key = relationOvercomeBy
		}
	}
	return key, tables.Relations[key]
}

// elementAffinity compares one source element with the primary material (75%
// of the weight) and the remaining components (25%, weighted by units).
func (c *Catalog) elementAffinity(recipe AlchemyRecipe, element int) (string, AlchemyModifiers, error) {
	tables := c.alchemy.tables
	primaryItem, ok := c.items[recipe.PrimaryID]
	if !ok {
		return "", AlchemyModifiers{}, fmt.Errorf("alchemy: unknown primary %q", recipe.PrimaryID)
	}
	primaryProfile, ok := c.profiles[primaryItem.ProfileID]
	if !ok {
		return "", AlchemyModifiers{}, fmt.Errorf("alchemy: primary %q has no profile", recipe.PrimaryID)
	}
	mainRelation, mainValues := c.alchemyRelation(element, primaryProfile.PrimaryElementID)

	type weighted struct {
		values AlchemyModifiers
		units  int64
	}
	var others []weighted
	var units int64
	for material, quantity := range c.Inputs(recipe.OutputID) {
		if material == recipe.PrimaryID || quantity <= 0 {
			continue
		}
		item, ok := c.items[material]
		if !ok {
			return "", AlchemyModifiers{}, fmt.Errorf("alchemy: unknown component %q", material)
		}
		profile, ok := c.profiles[item.ProfileID]
		if !ok {
			return "", AlchemyModifiers{}, fmt.Errorf("alchemy: component %q has no profile", material)
		}
		_, values := c.alchemyRelation(element, profile.PrimaryElementID)
		others = append(others, weighted{values: values, units: int64(quantity)})
		units += int64(quantity)
	}
	if units == 0 {
		return mainRelation, mainValues, nil
	}
	weight := int64(tables.Limits.PrimaryWeightBPS)
	rest := int64(10000) - weight
	total := AlchemyModifiers{}
	for _, field := range []struct {
		name string
		main int
		pick func(AlchemyModifiers) int
		dest *int
	}{
		{"success", mainValues.SuccessBPS, func(m AlchemyModifiers) int { return m.SuccessBPS }, &total.SuccessBPS},
		{"yield", mainValues.YieldBPS, func(m AlchemyModifiers) int { return m.YieldBPS }, &total.YieldBPS},
		{"potency", mainValues.PotencyBPS, func(m AlchemyModifiers) int { return m.PotencyBPS }, &total.PotencyBPS},
	} {
		numerator := int64(field.main) * weight * units
		for _, other := range others {
			numerator += int64(field.pick(other.values)) * other.units * rest
		}
		part := truncRatio(numerator, units*10000)
		if part > math.MaxInt/32 || part < -math.MaxInt/32 {
			return "", AlchemyModifiers{}, fmt.Errorf("alchemy: %s affinity exceeds arithmetic range", field.name)
		}
		*field.dest = int(part)
	}
	return mainRelation, total, nil
}

// yieldDistribution returns the tickets for each integer count. The base
// quantity is uniform inside the recipe range and the remainder is the chance
// of one extra pill, exactly as the preview prints it.
func yieldDistribution(minimum, maximum, cap, multiplier int) map[int]int {
	tickets := make(map[int]int)
	clamp := func(value int) int { return max(1, min(cap, value)) }
	for base := minimum; base <= maximum; base++ {
		whole, remainder := base*multiplier/10000, base*multiplier%10000
		tickets[clamp(whole)] += 10000 - remainder
		if remainder != 0 {
			tickets[clamp(whole+1)] += remainder
		}
	}
	return tickets
}

func (c *Catalog) alchemyParts(recipe AlchemyRecipe, ctx AlchemyContext) (int, []AlchemyAdjustment, error) {
	if ctx.AlchemistRank < 0 || ctx.AlchemistRank > alchemyMaxRank {
		return 0, nil, fmt.Errorf("alchemist rank %d out of range 0-%d", ctx.AlchemistRank, alchemyMaxRank)
	}
	flame, ok := c.alchemy.tables.Flames[ctx.FlameID]
	if !ok {
		return 0, nil, fmt.Errorf("unknown alchemy flame %q", ctx.FlameID)
	}
	if minimum := max(recipe.MinRank, flame.MinRank); ctx.AlchemistRank < minimum {
		return 0, nil, fmt.Errorf("recipe %q needs alchemist rank %d", recipe.OutputID, minimum)
	}
	flameFactor, ok := c.forging.tables.Factors.Flames[ctx.FlameID]
	if !ok || flameFactor.ElementID < 1 || flameFactor.ElementID > 5 {
		return 0, nil, fmt.Errorf("alchemy flame %q has no element", ctx.FlameID)
	}
	if ctx.MethodElement != 0 && (ctx.MethodElement < 1 || ctx.MethodElement > 5) {
		return 0, nil, fmt.Errorf("invalid alchemy method element %d", ctx.MethodElement)
	}
	if ctx.MethodElement == 0 && ctx.MethodRank != 0 {
		return 0, nil, fmt.Errorf("alchemy method element is required to scale a method")
	}
	primaryItem, ok := c.items[recipe.PrimaryID]
	if !ok {
		return 0, nil, fmt.Errorf("alchemy: unknown primary %q", recipe.PrimaryID)
	}
	primaryProfile, ok := c.profiles[primaryItem.ProfileID]
	if !ok || primaryProfile.PrimaryElementID < 1 || primaryProfile.PrimaryElementID > 5 {
		return 0, nil, fmt.Errorf("alchemy: primary %q has no primary element", recipe.PrimaryID)
	}
	rank, ok := c.alchemy.tables.Ranks[ctx.AlchemistRank]
	if !ok {
		return 0, nil, fmt.Errorf("unknown alchemy rank %d", ctx.AlchemistRank)
	}

	var parts []AlchemyAdjustment
	parts = append(parts, AlchemyAdjustment{SourceID: "rank", AlchemyModifiers: rank})
	parts = append(parts, AlchemyAdjustment{SourceID: "flame", AlchemyModifiers: flame.AlchemyModifiers})

	flameRelation, flameAffinity, err := c.elementAffinity(recipe, flameFactor.ElementID)
	if err != nil {
		return 0, nil, err
	}
	parts = append(parts, AlchemyAdjustment{
		SourceID: "flameElement", RelationID: flameRelation, ElementID: flameFactor.ElementID,
		AlchemyModifiers: flameAffinity,
	})

	if ctx.MethodElement != 0 {
		relation, affinity, err := c.elementAffinity(recipe, ctx.MethodElement)
		if err != nil {
			return 0, nil, err
		}
		numerator, denominator, ok := c.MethodInfluence("alchemy", qiAbsorptionMethod, ctx.MethodRank)
		if !ok {
			return 0, nil, fmt.Errorf("unknown equipped qi-absorption method grade %d", ctx.MethodRank)
		}
		scaled, err := affinity.truncScale(numerator, denominator)
		if err != nil {
			return 0, nil, err
		}
		parts = append(parts, AlchemyAdjustment{
			SourceID: "methodElement", RelationID: relation, ElementID: ctx.MethodElement,
			AlchemyModifiers: scaled,
		})
	}
	if ctx.PlaceSuccess != 0 {
		parts = append(parts, AlchemyAdjustment{
			SourceID: "place", RegionID: ctx.PlaceRegionID,
			AlchemyModifiers: AlchemyModifiers{SuccessBPS: ctx.PlaceSuccess},
		})
	}
	return primaryProfile.PrimaryElementID, parts, nil
}

// PlanAlchemy is deterministic and consumes nothing. The caller supplies the
// character's authenticated profession rank and equipped method; flame and recipe
// are player choices.
func (c *Catalog) PlanAlchemy(recipeID string, ctx AlchemyContext) (AlchemyPlan, error) {
	if c.alchemy == nil {
		return AlchemyPlan{}, fmt.Errorf("catalog has no alchemy rules")
	}
	recipe, ok := c.alchemy.tables.Recipes[recipeID]
	if !ok {
		return AlchemyPlan{}, fmt.Errorf("unknown alchemy recipe %q", recipeID)
	}
	primaryElement, parts, err := c.alchemyParts(recipe, ctx)
	if err != nil {
		return AlchemyPlan{}, err
	}
	tables := c.alchemy.tables
	total := AlchemyModifiers{}
	for _, part := range parts {
		total = total.add(part.AlchemyModifiers)
	}
	success := max(tables.Limits.SuccessMinBPS, min(tables.Limits.SuccessMaxBPS, recipe.SuccessBPS+total.SuccessBPS))
	multiplier := max(tables.Limits.YieldMultiplierMinBPS, min(tables.Limits.YieldMultiplierMaxBPS, 10000+total.YieldBPS))
	tickets := yieldDistribution(recipe.MinQty, recipe.MaxQty, recipe.YieldCap, multiplier)
	totalTickets := 0
	for _, count := range tickets {
		totalTickets += count
	}

	item, ok := c.items[recipe.OutputID]
	if !ok {
		return AlchemyPlan{}, fmt.Errorf("unknown alchemy output %q", recipe.OutputID)
	}
	grade, ok := c.grades[item.GradeID]
	if !ok {
		return AlchemyPlan{}, fmt.Errorf("unknown alchemy output grade %d", item.GradeID)
	}
	quality, ok := tables.PotencyByQuality[grade.QualityID]
	if !ok || len(quality) != 2 {
		return AlchemyPlan{}, fmt.Errorf("alchemy output %q has no potency range", recipe.OutputID)
	}
	low, high := quality[0], quality[1]
	potency := [2]int{
		max(low, min(high, low+total.PotencyBPS)),
		max(low, min(high, high+total.PotencyBPS)),
	}
	effect := [2]int{
		(recipe.BaseEffect*potency[0] + 5000) / 10000,
		(recipe.BaseEffect*potency[1] + 5000) / 10000,
	}
	return AlchemyPlan{
		RecipeID: recipeID, OutputID: recipe.OutputID, EffectID: recipe.EffectID,
		PrimaryElementID: primaryElement, Adjustments: parts,
		SuccessBPS: success, YieldMultiplierBPS: multiplier,
		YieldTickets: tickets, YieldTotalTickets: totalTickets,
		PotencyBPS: potency, EffectRange: effect,
	}, nil
}

// ResolveAlchemy settles one batch. It consumes no inventory itself: the caller
// removes the recipe components, and only a success produces pills.
func (c *Catalog) ResolveAlchemy(recipeID string, ctx AlchemyContext, seed int64) (AlchemyOutcome, error) {
	plan, err := c.PlanAlchemy(recipeID, ctx)
	if err != nil {
		return AlchemyOutcome{}, err
	}
	roll := rand.New(rand.NewSource(seed))
	if roll.Intn(10000) >= plan.SuccessBPS {
		return AlchemyOutcome{Success: false, Plan: plan}, nil
	}
	counts := make([]int, 0, len(plan.YieldTickets))
	for count := range plan.YieldTickets {
		counts = append(counts, count)
	}
	slices.Sort(counts)
	ticket := roll.Intn(plan.YieldTotalTickets)
	for _, count := range counts {
		ticket -= plan.YieldTickets[count]
		if ticket < 0 {
			weight := plan.PotencyBPS[1] - plan.PotencyBPS[0]
			potency := plan.PotencyBPS[0]
			if weight > 0 {
				potency += roll.Intn(weight + 1)
			}
			effect := (c.alchemy.tables.Recipes[recipeID].BaseEffect*potency + 5000) / 10000
			return AlchemyOutcome{Success: true, Count: count, PotencyBPS: potency, EffectValue: effect, Plan: plan}, nil
		}
	}
	return AlchemyOutcome{}, fmt.Errorf("alchemy: yield tickets for %q do not cover the roll", recipeID)
}

// AlchemyStep is one furnace step reported by the client: the tool or
// ingredient, and the timed 火候 the player achieved.
type AlchemyStep struct {
	StepID   string  `json:"stepId"`
	GoodsID  string  `json:"goodsId"`
	BestTime float64 `json:"bestTime"`
}

// RecipeByKey accepts either the recipe key or the output template, because the
// client sends MethodElixirId and older clients send the item template.
func (c *Catalog) RecipeByKey(key string) (string, AlchemyRecipe, bool) {
	if c.alchemy == nil {
		return "", AlchemyRecipe{}, false
	}
	if recipe, ok := c.alchemy.tables.Recipes[key]; ok {
		return key, recipe, true
	}
	return c.AlchemyRecipeForOutput(key)
}

// AlchemyUnits is the total component count of one batch. The game layer uses
// it for the 精力 cost and the bag transfer.
func AlchemyUnits(c *Catalog, recipe AlchemyRecipe) int {
	units := 0
	for _, quantity := range c.Inputs(recipe.OutputID) {
		units += quantity
	}
	return units
}

// StepsMatchRecipe rejects a batch whose reported steps cannot belong to the
// recipe: every goods must be a component of that recipe and 火候 must be non
// negative. An empty list is rejected; the client always sends one step per
// completed operation.
func StepsMatchRecipe(c *Catalog, recipe AlchemyRecipe, steps []AlchemyStep) bool {
	if len(steps) == 0 {
		return false
	}
	components := make(map[string]struct{})
	for material := range c.Inputs(recipe.OutputID) {
		components[material] = struct{}{}
	}
	for _, step := range steps {
		if step.BestTime < 0 {
			return false
		}
		if step.GoodsID == "" {
			continue // a pure operation step, e.g. 控火
		}
		if _, ok := components[step.GoodsID]; !ok {
			return false
		}
	}
	return true
}

// SettleAlchemy validates the client report and settles it on the server. The
// client's own success flag is never trusted: success is recomputed from the
// same tables, so a modified packet cannot add pills.
func (c *Catalog) SettleAlchemy(key string, ctx AlchemyContext, steps []AlchemyStep, seed int64) (AlchemyOutcome, string, error) {
	recipeID, recipe, ok := c.RecipeByKey(key)
	if !ok {
		return AlchemyOutcome{}, "", fmt.Errorf("unknown alchemy recipe or pill %q", key)
	}
	if ctx.AlchemistRank < recipe.MinRank {
		return AlchemyOutcome{}, recipeID, fmt.Errorf("recipe %q needs alchemist rank %d", recipeID, recipe.MinRank)
	}
	if !StepsMatchRecipe(c, recipe, steps) {
		return AlchemyOutcome{}, recipeID, fmt.Errorf("alchemy steps do not match recipe %q", recipeID)
	}
	outcome, err := c.ResolveAlchemy(recipeID, ctx, seed)
	return outcome, recipeID, err
}
