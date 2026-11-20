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
	SmithRank     int    `json:"smithRank"`
	MethodElement int    `json:"methodElement"`
	MethodRank    int    `json:"methodRank"`
	FlameID       string `json:"flameId"`
	GradeID       int    `json:"gradeId"`
}

type ForgeFactor struct {
	SuccessBPS  int `json:"successBps"`
	ReturnBPS   int `json:"returnBps"`
	RollBiasBPS int `json:"rollBiasBps"`
	ExpBPS      int `json:"expBps"`
}

// A preview only shows the process and target. It contains no generated stats.
type ForgePlan struct {
	ForgeFactor
	GradeID int `json:"gradeId"`
}

type ForgeOutcome struct {
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
	return f.tables.Factors.ElementRelations[key]
}

func (f *forgeIndex) affinityFactor(source int, order forgeOrder) ForgeFactor {
	var sum ForgeFactor
	for id, qty := range order.inputs {
		sum = sum.add(f.relation(source, f.materials[id].profile.PrimaryElementID).scale(qty, 1))
	}
	return f.relation(source, order.blank.profile.PrimaryElementID).add(sum.scale(1, order.total))
}

func (c *Catalog) plan(order forgeOrder, ctx ForgeContext) (ForgePlan, error) {
	f := c.forging
	t := f.tables.Factors
	grade, ok := c.Grade(ctx.GradeID)
	if !ok || grade.Legacy {
		return ForgePlan{}, fmt.Errorf("unknown or legacy target grade %d", ctx.GradeID)
	}
	noMethod := ctx.MethodElement == 0 && ctx.MethodRank == 0
	if ctx.SmithRank < 1 || ctx.SmithRank > 9 || !noMethod && (ctx.MethodElement < 1 || ctx.MethodElement > 5) {
		return ForgePlan{}, fmt.Errorf("invalid forge smith rank or equipped method")
	}
	flame, ok := t.Flames[ctx.FlameID]
	if !ok {
		return ForgePlan{}, fmt.Errorf("unknown forge flame %q", ctx.FlameID)
	}
	// Each source is compared to the blank once and to materials as a weighted
	// average. The equipped qi-absorption method scales its own relationship
	// contribution by the ratio configured for smithing and that method grade.
	factors := ForgeFactor{SuccessBPS: 7000, ReturnBPS: 1000}
	target := t.TargetGrades[ctx.GradeID]
	factors = factors.add(t.SmithRanks[ctx.SmithRank]).add(flame.ForgeFactor).add(target.ForgeFactor)
	factors.SuccessBPS += target.DifficultyBPS
	factors = factors.add(f.affinityFactor(flame.ElementID, order))
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
	factors = factors.add(method)
	position := (grade.TierID-1)*3 + grade.QualityID
	mixGrade := (order.gradePoints + (order.total+2)/2) / (order.total + 2)
	gradeGap := mixGrade - position
	factors.SuccessBPS += 100*gradeGap + 200*order.matched/order.total
	factors.RollBiasBPS += 120*gradeGap + 300*order.matched/order.total
	factors.SuccessBPS = max(0, min(10000, factors.SuccessBPS))
	factors.ReturnBPS = max(0, min(10000, factors.ReturnBPS))
	factors.RollBiasBPS = max(-10000, min(10000, factors.RollBiasBPS))
	factors.ExpBPS = max(-10000, factors.ExpBPS)
	return ForgePlan{ForgeFactor: factors, GradeID: ctx.GradeID}, nil
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
		return ForgeOutcome{Success: true, Weapon: &weapon, Experience: max(1, (10*(10000+plan.ExpBPS)+5000)/10000)}, nil
	}
	returned := make(map[string]int)
	for _, id := range slices.Sorted(maps.Keys(inputs)) {
		for range inputs[id] {
			if roll.Intn(10000) < plan.ReturnBPS {
				returned[id]++
			}
		}
	}
	return ForgeOutcome{Returned: returned, Experience: max(1, (2*(10000+plan.ExpBPS)+5000)/10000)}, nil
}
