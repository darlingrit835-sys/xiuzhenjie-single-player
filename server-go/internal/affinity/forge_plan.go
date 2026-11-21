package affinity

import (
	"fmt"
	"maps"
	"math"
	"math/bits"
	"math/rand"
	"slices"
)

type ForgeContext struct {
	PracticeBase        int    `json:"-"`
	PatternDifficulty   int    `json:"patternDifficulty,omitempty"`
	WorkshopProgressBPS int    `json:"workshopProgressBps,omitempty"`
	SmithRank           int    `json:"smithRank"`
	MethodElement       int    `json:"methodElement"`
	MethodRank          int    `json:"methodRank"`
	FlameID             string `json:"flameId"`
	GradeID             int    `json:"gradeId"`
}

type ForgeFactor struct {
	SuccessBPS  int `json:"successBps"`
	ReturnBPS   int `json:"returnBps"`
	RollBiasBPS int `json:"rollBiasBps"`
	ExpBPS      int `json:"expBps"`
}

// A preview only shows the process and target. It contains no generated stats.
// The rows are the exact signed terms used to calculate the final plan. A
// fixed array keeps previews comparable and their source order deterministic.
const (
	BaseHeat = iota
	SmithArts
	FlameArts
	FireControl
	TargetArts
	TargetDifficulty
	PatternLoad
	FlameAffinity
	MethodAffinity
	MaterialGrade
	RecipeMatch
	WornTools
	Bounds
	forgeSourceCount
)

type FlameMastery struct {
	RequiredRank, SmithRank int
	ForgeFactor
}

type ForgePlan struct {
	PracticeBase   int                           `json:"-"`
	FlameMastery   FlameMastery                  `json:"-"`
	Sources        [forgeSourceCount]ForgeFactor `json:"-"`
	Matched, Units int                           `json:"-"`
	ForgeFactor
	GradeID int `json:"gradeId"`
}

type ForgeOutcome struct {
	Plan       ForgePlan      `json:"-"`
	Success    bool           `json:"success"`
	Weapon     *ForgedWeapon  `json:"weapon,omitempty"`
	Returned   map[string]int `json:"returned,omitempty"`
	Experience int            `json:"experience"`
}

type ForgeFlame struct {
	ForgeFactor
	ElementID int `json:"elementId"`
	GradeID   int `json:"gradeId"`
}

type ForgeTargetGrade struct {
	ForgeFactor
	DifficultyBPS int `json:"difficultyBps"`
}

type elementCycle struct {
	Generates int `json:"generates"`
	Overcomes int `json:"overcomes"`
}

type forgeFactors struct {
	SmithRanks       map[int]ForgeFactor      `json:"smithRanks"`
	Flames           map[string]ForgeFlame    `json:"flames"`
	TargetGrades     map[int]ForgeTargetGrade `json:"targetGrades"`
	ElementRelations map[string]ForgeFactor   `json:"elementRelations"`
}

const qiAbsorptionMethod = 3

func (f ForgeFactor) add(other ForgeFactor) ForgeFactor {
	return ForgeFactor{f.SuccessBPS + other.SuccessBPS, f.ReturnBPS + other.ReturnBPS, f.RollBiasBPS + other.RollBiasBPS, f.ExpBPS + other.ExpBPS}
}

func (f ForgeFactor) scale(n, d int) ForgeFactor {
	return ForgeFactor{f.SuccessBPS * n / d, f.ReturnBPS * n / d, f.RollBiasBPS * n / d, f.ExpBPS * n / d}
}

// The ratio is authored as two positive integers. Keep the intermediate product
// wide so equivalent ratios like MaxInt/MaxInt still scale the factor exactly.
func (f ForgeFactor) scaleMethod(n, d int) (ForgeFactor, error) {
	for _, field := range [...]*int{&f.SuccessBPS, &f.ReturnBPS, &f.RollBiasBPS, &f.ExpBPS} {
		value := *field
		if value < 0 {
			value = -value
		}
		hi, lo := bits.Mul64(uint64(value), uint64(n))
		if hi >= uint64(d) {
			return ForgeFactor{}, fmt.Errorf("method influence exceeds forge arithmetic range")
		}
		part, _ := bits.Div64(hi, lo, uint64(d))
		// Leave room for other factors and the final 10-point experience award.
		if part > uint64(math.MaxInt/32) {
			return ForgeFactor{}, fmt.Errorf("method influence exceeds forge arithmetic range")
		}
		if *field < 0 {
			*field = -int(part)
		} else {
			*field = int(part)
		}
	}
	return f, nil
}

func (f ForgeFactor) valid() bool {
	// Bound the arithmetic, including a quantity-weighted sum and method scaling.
	const limit = math.MaxInt / 1024
	return f.SuccessBPS >= -limit && f.SuccessBPS <= limit && f.ReturnBPS >= -limit && f.ReturnBPS <= limit &&
		f.RollBiasBPS >= -limit && f.RollBiasBPS <= limit && f.ExpBPS >= -limit && f.ExpBPS <= limit
}

func (f ForgeFactor) affixCount() int {
	count := 0
	for _, v := range [...]int{f.SuccessBPS, f.ReturnBPS, f.RollBiasBPS, f.ExpBPS} {
		if v != 0 {
			count++
		}
	}
	return count
}

func (f *forgeIndex) validateFactors(c *Catalog) error {
	factors := f.tables.Factors
	if len(c.methods["smithing"][qiAbsorptionMethod]) == 0 {
		return fmt.Errorf("missing smithing qi-absorption method influences")
	}
	for i := 1; i <= 9; i++ {
		r, ok := factors.SmithRanks[i]
		if !ok || !r.valid() {
			return fmt.Errorf("missing or invalid forge smith rank %d", i)
		}
	}
	for _, id := range f.gradeIDs {
		r, ok := factors.TargetGrades[id]
		if !ok || !r.valid() || r.DifficultyBPS < -math.MaxInt/1024 || r.DifficultyBPS > math.MaxInt/1024 || r.affixCount() != f.tables.AffixCounts[c.grades[id].TierID] {
			return fmt.Errorf("missing or invalid forge target grade %d", id)
		}
	}
	for _, key := range []string{"same", "generates", "overcomes", "generatedBy", "overcomeBy", "neutral"} {
		r, ok := factors.ElementRelations[key]
		if !ok || !r.valid() {
			return fmt.Errorf("missing or invalid forge relation %q", key)
		}
	}
	if len(factors.Flames) == 0 {
		return fmt.Errorf("no forge flames configured")
	}
	for id, flame := range factors.Flames {
		grade, hasGrade := c.grades[flame.GradeID]
		if !hasGrade || grade.Legacy || flame.ElementID < 1 || flame.ElementID > 5 || !flame.ForgeFactor.valid() || flame.affixCount() != f.tables.AffixCounts[grade.TierID] {
			return fmt.Errorf("invalid forge flame %q", id)
		}
	}
	for id := 1; id <= 5; id++ {
		element := f.tables.Elements[id]
		if element.Generates < 1 || element.Generates > 5 || element.Overcomes < 1 || element.Overcomes > 5 ||
			id == element.Generates || id == element.Overcomes || element.Generates == element.Overcomes {
			return fmt.Errorf("invalid forge element cycle %d", id)
		}
	}
	return nil
}

func (c *Catalog) ForgeFlames() []string {
	if c.forging == nil {
		return nil
	}
	return slices.Sorted(maps.Keys(c.forging.tables.Factors.Flames))
}

func (c *Catalog) ForgeFlame(id string) (ForgeFlame, bool) {
	if c.forging == nil {
		return ForgeFlame{}, false
	}
	flame, ok := c.forging.tables.Factors.Flames[id]
	return flame, ok
}

func (c *Catalog) ForgeSmithRank(rank int) (ForgeFactor, bool) {
	if c.forging == nil {
		return ForgeFactor{}, false
	}
	factor, ok := c.forging.tables.Factors.SmithRanks[rank]
	// Promotion adds techniques. A previous rank's best technique in each
	// discipline stays available when a new rank diversifies into other skills.
	if ok {
		for old := 1; old < rank; old++ {
			learned := c.forging.tables.Factors.SmithRanks[old]
			factor.SuccessBPS = max(factor.SuccessBPS, learned.SuccessBPS)
			factor.ReturnBPS = max(factor.ReturnBPS, learned.ReturnBPS)
			factor.RollBiasBPS = max(factor.RollBiasBPS, learned.RollBiasBPS)
			factor.ExpBPS = max(factor.ExpBPS, learned.ExpBPS)
		}
	}
	return factor, ok
}

func (c *Catalog) ForgeTarget(id int) (ForgeTargetGrade, bool) {
	if c.forging == nil {
		return ForgeTargetGrade{}, false
	}
	grade, ok := c.forging.tables.Factors.TargetGrades[id]
	return grade, ok
}

func (f *forgeIndex) relation(source, target int) ForgeFactor {
	return f.tables.Factors.ElementRelations[f.relationKey(source, target)]
}

func (c *Catalog) ForgeElementRelation(source, target int) string {
	if c.forging == nil {
		return "neutral"
	}
	return c.forging.relationKey(source, target)
}

func (f *forgeIndex) relationKey(source, target int) string {
	cycles := f.tables.Elements
	key := "neutral"
	if source != 0 && target != 0 {
		switch {
		case source == target:
			key = "same"
		case cycles[source].Generates == target:
			key = "generates"
		case cycles[source].Overcomes == target:
			key = "overcomes"
		case cycles[target].Generates == source:
			key = "generatedBy"
		case cycles[target].Overcomes == source:
			key = "overcomeBy"
		}
	}
	return key
}

func (f *forgeIndex) affinityFactor(source int, order forgeOrder) ForgeFactor {
	var sum ForgeFactor
	for id, qty := range order.inputs {
		sum = sum.add(f.relation(source, f.materials[id].profile.PrimaryElementID).scale(qty, 1))
	}
	return f.relation(source, order.blank.profile.PrimaryElementID).add(sum.scale(1, order.total))
}

// Twelve fire grades map onto nine profession ranks. Strong fires remain
// usable by novices, with more risk and a small experience incentive.
func (c *Catalog) FlameMastery(flameID string, smithRank int) (FlameMastery, bool) {
	flame, ok := c.ForgeFlame(flameID)
	if !ok || smithRank < 1 || smithRank > 9 {
		return FlameMastery{}, false
	}
	grade, _ := c.Grade(flame.GradeID)
	position := (grade.TierID-1)*3 + grade.QualityID - 1
	required := 1 + position*8/11
	margin := smithRank - required
	result := FlameMastery{RequiredRank: required, SmithRank: smithRank}
	if margin < 0 {
		result.ForgeFactor = ForgeFactor{SuccessBPS: 275 * margin, ReturnBPS: 125 * margin, RollBiasBPS: 150 * margin, ExpBPS: -150 * margin}
	} else {
		mastery := min(4, margin)
		result.ForgeFactor = ForgeFactor{SuccessBPS: 50 * mastery, ReturnBPS: 60 * mastery, RollBiasBPS: 50 * mastery}
	}
	return result, true
}

func (plan ForgePlan) Experience(success bool) int {
	base := plan.PracticeBase
	if base < 1 {
		base = 10
	}
	if !success {
		base = max(1, base/5)
	}
	return max(1, (base*(10000+plan.ExpBPS)+5000)/10000)
}

func (c *Catalog) plan(order forgeOrder, ctx ForgeContext) (ForgePlan, error) {
	f := c.forging
	t := f.tables.Factors
	grade, ok := c.Grade(ctx.GradeID)
	if !ok || grade.Legacy {
		return ForgePlan{}, fmt.Errorf("unknown or legacy target grade %d", ctx.GradeID)
	}
	noMethod := ctx.MethodElement == 0 && ctx.MethodRank == 0
	if ctx.PatternDifficulty < 0 || ctx.PatternDifficulty > 5 || ctx.SmithRank < 1 || ctx.SmithRank > 9 || !noMethod && (ctx.MethodElement < 1 || ctx.MethodElement > 5) {
		return ForgePlan{}, fmt.Errorf("invalid forge smith rank or equipped method")
	}
	flame, ok := t.Flames[ctx.FlameID]
	if !ok {
		return ForgePlan{}, fmt.Errorf("unknown forge flame %q", ctx.FlameID)
	}
	// Each source is compared to the blank once and to materials as a weighted
	// average. The equipped qi-absorption method scales its own relationship
	// contribution by the ratio configured for smithing and that method grade.
	mastery, _ := c.FlameMastery(ctx.FlameID, ctx.SmithRank)
	base := ctx.PracticeBase
	if ctx.PatternDifficulty > 0 {
		gap := ctx.SmithRank - (2*ctx.PatternDifficulty - 1)
		if gap >= 4 {
			base = max(1, base/4)
		} else if gap >= 2 {
			base = max(1, base/2)
		}
	}
	plan := ForgePlan{PracticeBase: base, GradeID: ctx.GradeID, FlameMastery: mastery, Matched: order.matched, Units: order.total}
	terms := &plan.Sources
	terms[BaseHeat] = ForgeFactor{SuccessBPS: 7000, ReturnBPS: 1000}
	terms[SmithArts], _ = c.ForgeSmithRank(ctx.SmithRank)
	terms[FlameArts] = flame.ForgeFactor
	terms[FireControl] = mastery.ForgeFactor
	target := t.TargetGrades[ctx.GradeID]
	terms[TargetArts] = target.ForgeFactor
	terms[TargetDifficulty] = ForgeFactor{SuccessBPS: target.DifficultyBPS}
	load := max(0, ctx.PatternDifficulty-1)
	outstanding := max(0, load-(ctx.SmithRank-1)/2)
	terms[PatternLoad] = ForgeFactor{SuccessBPS: -50*load - 300*outstanding, ReturnBPS: -150 * outstanding}
	terms[FlameAffinity] = f.affinityFactor(flame.ElementID, order)
	method := f.affinityFactor(ctx.MethodElement, order)
	if !noMethod {
		n, d, ok := c.MethodInfluence("smithing", qiAbsorptionMethod, ctx.MethodRank)
		if !ok {
			return ForgePlan{}, fmt.Errorf("unknown equipped qi-absorption method grade %d", ctx.MethodRank)
		}
		var err error
		method, err = method.scaleMethod(n, d)
		if err != nil {
			return ForgePlan{}, err
		}
	}
	terms[MethodAffinity] = method
	position := (grade.TierID-1)*3 + grade.QualityID
	mixGrade := (order.gradePoints + (order.total+2)/2) / (order.total + 2)
	gradeGap := mixGrade - position
	terms[MaterialGrade] = ForgeFactor{SuccessBPS: 100 * gradeGap, RollBiasBPS: 120 * gradeGap}
	terms[RecipeMatch] = ForgeFactor{SuccessBPS: 200 * order.matched / order.total, RollBiasBPS: 300 * order.matched / order.total}
	var total ForgeFactor
	for _, term := range terms {
		total = total.add(term)
	}
	// Only the achievable chance is amplified. Clamp the input before
	// multiplication so user-authored method ratios cannot overflow here.
	baseChance := min(10000, max(0, total.SuccessBPS))
	terms[WornTools] = ForgeFactor{SuccessBPS: baseChance * min(10000, max(0, ctx.WorkshopProgressBPS)) / 10000}
	total = total.add(terms[WornTools])
	plan.ForgeFactor = ForgeFactor{SuccessBPS: min(10000, max(0, total.SuccessBPS)), ReturnBPS: min(10000, max(0, total.ReturnBPS)),
		RollBiasBPS: min(10000, max(-10000, total.RollBiasBPS)), ExpBPS: max(-10000, total.ExpBPS)}
	terms[Bounds] = ForgeFactor{plan.SuccessBPS - total.SuccessBPS, plan.ReturnBPS - total.ReturnBPS, plan.RollBiasBPS - total.RollBiasBPS, plan.ExpBPS - total.ExpBPS}
	return plan, nil
}

// PlanForge is deterministic. Caller supplies the character's authenticated
// profession rank and equipped method; flame and target grade are player choices.
func (c *Catalog) PlanForge(blankID string, inputs map[string]int, ctx ForgeContext) (ForgePlan, error) {
	order, err := c.prepareForge(blankID, inputs)
	if err != nil {
		return ForgePlan{}, err
	}
	return c.plan(order, ctx)
}

// ResolveForge consumes no inventory itself. On failure, Returned contains the
// units to keep per selected template. Only a successful roll creates a weapon.
func (c *Catalog) ResolveForge(blankID string, inputs map[string]int, ctx ForgeContext, seed int64) (ForgeOutcome, error) {
	order, err := c.prepareForge(blankID, inputs)
	if err != nil {
		return ForgeOutcome{}, err
	}
	plan, err := c.plan(order, ctx)
	if err != nil {
		return ForgeOutcome{}, err
	}
	roll := rand.New(rand.NewSource(seed))
	if roll.Intn(10000) < plan.SuccessBPS {
		weapon := c.finishForge(order, plan, roll)
		return ForgeOutcome{Plan: plan, Success: true, Weapon: &weapon, Experience: plan.Experience(true)}, nil
	}
	returned := make(map[string]int)
	for _, id := range slices.Sorted(maps.Keys(inputs)) {
		for range inputs[id] {
			if roll.Intn(10000) < plan.ReturnBPS {
				returned[id]++
			}
		}
	}
	return ForgeOutcome{Plan: plan, Returned: returned, Experience: plan.Experience(false)}, nil
}
