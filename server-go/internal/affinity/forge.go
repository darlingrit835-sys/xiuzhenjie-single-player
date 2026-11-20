package affinity

import (
	"fmt"
	"maps"
	"math"
	"math/rand"
	"slices"
)

// A blank is a reusable design. Only the selected material units are consumed.
const MinForgeUnits, MaxForgeUnits = 2, 12

// ForgedWeapon is the finished roll. Persist it under a server-issued template
// ID so trading, display and equipment reads never reroll or follow changed rules.
type ForgedWeapon struct {
	CatalogVersion string         `json:"catalogVersion"`
	PrototypeID    string         `json:"prototypeId"`
	CategoryID     string         `json:"categoryId"`
	ClassID        string         `json:"classId"`
	PolarityID     int            `json:"polarityId"`
	ElementID      int            `json:"elementId"`
	QiID           int            `json:"qiId"`
	GradeID        int            `json:"gradeId"`
	BaseAttack     int            `json:"baseAttack"`
	BaseDefense    int            `json:"baseDefense"`
	BaseBonuses    map[string]int `json:"baseBonuses"` // same roll within the ungraded branch range (lower-quality range for pool-only stats)
	Bonuses        map[string]int `json:"bonuses"`     // final basis points
	Inputs         map[string]int `json:"inputs"`
}

type forgeTables struct {
	Elements      map[int]elementCycle             `json:"elements"`
	Factors       forgeFactors                     `json:"forgeFactors"`
	Stats         map[string]struct{}              `json:"stats"`
	PolarityQi    map[int]map[int]int              `json:"polarityQiWeights"`
	ElementQi     map[int]map[int]int              `json:"elementQiWeights"`
	BonusRules    map[int]map[int]map[string][]int `json:"bonusRules"`
	ElementFocus  map[int]string                   `json:"elementFocus"`
	PolarityFocus map[int]string                   `json:"polarityFocus"`
	AffixCounts   map[int]int                      `json:"affixCounts"`
	AffixRanges   map[string]map[int][]int         `json:"affixRanges"`
	ItemAffixes   map[string]map[string]int        `json:"itemAffixes"`
}

type bonusRange struct {
	stat      string
	low, high int
}

type forgeBlank struct {
	profile  Profile
	grade    int // 1-based position in the twelve current grades
	category string
	recipe   map[string]int
}

type forgeMaterial struct {
	profile Profile
	grade   int
}

type forgeIndex struct {
	blanks                map[string]forgeBlank
	materials             map[string]forgeMaterial
	blankIDs, materialIDs []string
	gradeIDs              [12]int
	// Product of the two configured qi weights for each (polarity, element).
	qi      map[[2]int][12]float64
	bonuses map[[2]int][]bonusRange
	tables  forgeTables
	stats   []string
}

func compileForge(c *Catalog, tables forgeTables) (*forgeIndex, error) {
	if tables.Stats == nil && tables.PolarityQi == nil && tables.ElementQi == nil && tables.BonusRules == nil {
		return nil, nil // A lookup-only catalog is also supported by Load.
	}
	f := &forgeIndex{blanks: make(map[string]forgeBlank), materials: make(map[string]forgeMaterial), tables: tables, stats: slices.Sorted(maps.Keys(tables.Stats)),
		qi: make(map[[2]int][12]float64), bonuses: make(map[[2]int][]bonusRange)}
	maxMultiplier := 0
	for id, grade := range c.grades {
		if grade.Legacy {
			continue
		}
		if grade.TierID < 1 || grade.TierID > 4 || grade.QualityID < 1 || grade.QualityID > 3 || grade.MultiplierBPS <= 0 || id <= 0 {
			return nil, fmt.Errorf("invalid current forge grade %d", id)
		}
		index := (grade.TierID-1)*3 + grade.QualityID - 1
		if f.gradeIDs[index] != 0 {
			return nil, fmt.Errorf("duplicate tier and quality for forge grade %d", id)
		}
		f.gradeIDs[index] = id
		maxMultiplier = max(maxMultiplier, grade.MultiplierBPS)
	}
	if slices.Contains(f.gradeIDs[:], 0) {
		return nil, fmt.Errorf("forge requires all twelve current grades")
	}
	for tier := 1; tier <= 4; tier++ {
		count := tables.AffixCounts[tier]
		if count < 1 || count > min(4, len(tables.Stats)) {
			return nil, fmt.Errorf("invalid forge affix count for tier %d", tier)
		}
	}
	for _, stat := range f.stats {
		for quality := 1; quality <= 4; quality++ {
			if !validRange(tables.AffixRanges[stat][quality]) {
				return nil, fmt.Errorf("invalid forge affix range for %q quality %d", stat, quality)
			}
		}
	}
	for polarity := 1; polarity <= 2; polarity++ {
		for element := 1; element <= 5; element++ {
			if _, ok := tables.Stats[tables.PolarityFocus[polarity]]; !ok {
				return nil, fmt.Errorf("invalid forge polarity focus %d", polarity)
			}
			if _, ok := tables.Stats[tables.ElementFocus[element]]; !ok {
				return nil, fmt.Errorf("invalid forge element focus %d", element)
			}
			branch := [2]int{polarity, element}
			var weights [12]float64
			for i := range weights {
				p, e := tables.PolarityQi[polarity][i+1], tables.ElementQi[element][i+1]
				if p <= 0 || e <= 0 {
					return nil, fmt.Errorf("invalid forge qi weight for %v qi %d", branch, i+1)
				}
				weights[i] = float64(p) * float64(e)
			}
			f.qi[branch] = weights
			rules := tables.BonusRules[polarity][element]
			if len(rules) == 0 {
				return nil, fmt.Errorf("missing forge bonus rules for %v", branch)
			}
			for _, stat := range slices.Sorted(maps.Keys(rules)) {
				bounds := rules[stat]
				_, known := tables.Stats[stat]
				if !known || !validRange(bounds) {
					return nil, fmt.Errorf("invalid forge bonus range for %v %q", branch, stat)
				}
				f.bonuses[branch] = append(f.bonuses[branch], bonusRange{stat, bounds[0], bounds[1]})
			}
		}
	}
	for id, item := range c.items {
		table := c.categories[item.CategoryID].DetailTable
		if table != "material" && table != "weapon" {
			continue
		}
		profile, ok := c.profiles[item.ProfileID]
		if !ok || profile.PolarityID < 0 || profile.PolarityID > 3 ||
			profile.PrimaryElementID < 0 || profile.PrimaryElementID > 5 || profile.SecondaryElementID < 0 || profile.SecondaryElementID > 5 ||
			profile.PrimaryQiID < 0 || profile.PrimaryQiID > 12 || profile.SecondaryQiID < 0 || profile.SecondaryQiID > 12 {
			return nil, fmt.Errorf("invalid forge profile for item %q", id)
		}
		grade, ok := c.grades[item.GradeID]
		if !ok || grade.TierID < 1 || grade.TierID > 4 || grade.QualityID < 1 || grade.QualityID > 4 {
			return nil, fmt.Errorf("invalid forge item grade for %q", id)
		}
		if len(tables.ItemAffixes[id]) != tables.AffixCounts[grade.TierID] {
			return nil, fmt.Errorf("wrong fixed affix count for %q", id)
		}
		position := (grade.TierID-1)*3 + min(3, grade.QualityID)
		if table == "material" {
			f.materials[id] = forgeMaterial{profile, position}
		} else if !grade.Legacy && profile.PolarityID >= 1 && profile.PolarityID <= 2 && profile.PrimaryElementID != 0 && profile.PrimaryQiID != 0 && len(c.inputs[id]) != 0 {
			f.blanks[id] = forgeBlank{profile, position, item.CategoryID, maps.Collect(c.Inputs(id))}
		}
	}
	for id, affixes := range tables.ItemAffixes {
		item, ok := c.items[id]
		if !ok {
			return nil, fmt.Errorf("unknown fixed affix item %q", id)
		}
		grade := c.grades[item.GradeID]
		for stat, value := range affixes {
			bounds := tables.AffixRanges[stat][grade.QualityID]
			if !validRange(bounds) || value < bounds[0] || value > bounds[1] {
				return nil, fmt.Errorf("invalid fixed affix %q for %q", stat, id)
			}
		}
	}
	if maxMultiplier > (math.MaxInt-5000)/150 {
		return nil, fmt.Errorf("forge grade multiplier overflows base weapon stats")
	}
	if err := f.validateFactors(c); err != nil {
		return nil, err
	}
	f.blankIDs = slices.Sorted(maps.Keys(f.blanks))
	f.materialIDs = slices.Sorted(maps.Keys(f.materials))
	return f, nil
}

func validRange(b []int) bool {
	return len(b) == 2 && b[0] >= 0 && b[1] >= b[0] && b[1]-b[0] <= (math.MaxInt-5000)/10000
}

func (c *Catalog) ForgeBlanks() []string {
	if c.forging == nil {
		return nil
	}
	return slices.Clone(c.forging.blankIDs)
}

func (c *Catalog) ForgeMaterials() []string {
	if c.forging == nil {
		return nil
	}
	return slices.Clone(c.forging.materialIDs)
}

func (c *Catalog) ForgeAffixes(templateID string) map[string]int {
	if c.forging == nil {
		return nil
	}
	return maps.Clone(c.forging.tables.ItemAffixes[templateID])
}

type forgeAffinity struct {
	polarity [2]int
	element  [5]int
	qi       [12]int
}

func (a *forgeAffinity) add(profile Profile, strength int) {
	if p := profile.PolarityID; p == 1 || p == 2 {
		a.polarity[p-1] += strength * 2
	}
	if id := profile.PrimaryElementID; id > 0 {
		a.element[id-1] += strength * 2
	}
	if id := profile.SecondaryElementID; id > 0 {
		a.element[id-1] += strength
	}
	if id := profile.PrimaryQiID; id > 0 {
		a.qi[id-1] += strength * 2
	}
	if id := profile.SecondaryQiID; id > 0 {
		a.qi[id-1] += strength
	}
}

// Dominant affinity wins. Only equal strongest values need a random tie break.
func dominant(scores []int, roll *rand.Rand) int {
	highest := slices.Max(scores)
	var tied []int
	for i, score := range scores {
		if score == highest {
			tied = append(tied, i+1)
		}
	}
	return tied[roll.Intn(len(tied))]
}

func drawForgeQi(weights [12]float64, affinity [12]int, roll *rand.Rand) int {
	var total float64
	for i := range weights {
		weights[i] *= float64(1 + affinity[i])
		total += weights[i]
	}
	ticket := roll.Float64() * total
	for i, weight := range weights {
		ticket -= weight
		if ticket < 0 {
			return i + 1
		}
	}
	return len(weights)
}

// Interpolate the same roll in the base and quality-specific ranges.
func atRoll(low, high, percentile int) int {
	return low + ((high-low)*percentile+5000)/10000
}

func (f *forgeIndex) rollAffixes(result *ForgedWeapon, grade Grade, bias int, roll *rand.Rand) {
	t := f.tables
	branch := [2]int{result.PolarityID, result.ElementID}
	weights := make(map[string]int)
	baseRanges := make(map[string]bonusRange)
	for _, rule := range f.bonuses[branch] {
		baseRanges[rule.stat] = rule
		weights[rule.stat] += 2
	}
	weights[t.PolarityFocus[result.PolarityID]] += 10
	addSource := func(id string, profile Profile, quantity int) {
		strength := quantity
		if profile.PrimaryQiID == result.QiID {
			strength *= 3
		} else if profile.SecondaryQiID == result.QiID {
			strength *= 2
		}
		for stat, bps := range t.ItemAffixes[id] {
			weights[stat] += strength * (1 + min(20, bps/100))
		}
	}
	addSource(result.PrototypeID, f.blanks[result.PrototypeID].profile, 2)
	for id, qty := range result.Inputs {
		addSource(id, f.materials[id].profile, qty)
	}
	stat := t.ElementFocus[result.ElementID]
	for len(result.Bonuses) < t.AffixCounts[grade.TierID] {
		pct := max(0, min(10000, roll.Intn(10001)+bias))
		bounds := t.AffixRanges[stat][grade.QualityID]
		result.Bonuses[stat] = atRoll(bounds[0], bounds[1], pct)
		base, ok := baseRanges[stat]
		if !ok {
			low := t.AffixRanges[stat][1]
			base = bonusRange{stat, low[0], low[1]}
		}
		result.BaseBonuses[stat] = atRoll(base.low, base.high, pct)
		if len(result.Bonuses) == t.AffixCounts[grade.TierID] {
			return
		}
		// Draw without replacement. Only consult the full stat pool once
		// all affinities supported by these ingredients and this blank are used.
		total := 0
		for _, candidate := range f.stats {
			if _, used := result.Bonuses[candidate]; !used {
				total += weights[candidate]
			}
		}
		usePool := total == 0
		if usePool {
			total = len(f.stats) - len(result.Bonuses)
		}
		ticket := roll.Intn(total)
		for _, candidate := range f.stats {
			if _, used := result.Bonuses[candidate]; used {
				continue
			}
			weight := weights[candidate]
			if usePool {
				weight = 1
			}
			ticket -= weight
			if ticket < 0 {
				stat = candidate
				break
			}
		}
	}
}

type forgeOrder struct {
	blankID                     string
	blank                       forgeBlank
	inputs                      map[string]int
	affinities                  forgeAffinity
	total, matched, gradePoints int
}

func (c *Catalog) prepareForge(blankID string, inputs map[string]int) (forgeOrder, error) {
	if c.forging == nil {
		return forgeOrder{}, fmt.Errorf("forge tables unavailable")
	}
	f := c.forging
	blank, ok := f.blanks[blankID]
	if !ok {
		return forgeOrder{}, fmt.Errorf("unknown forge blank %q", blankID)
	}
	var affinities forgeAffinity
	affinities.add(blank.profile, 2)
	total, matched, gradePoints := 0, 0, blank.grade*2
	for id, qty := range inputs {
		material, exists := f.materials[id]
		if !exists || qty <= 0 || qty > MaxForgeUnits-total {
			return forgeOrder{}, fmt.Errorf("invalid forge material or quantity for %q", id)
		}
		match := min(qty, blank.recipe[id])
		affinities.add(material.profile, qty+match)
		total += qty
		matched += match
		gradePoints += qty * material.grade
	}
	if total < MinForgeUnits {
		return forgeOrder{}, fmt.Errorf("forge requires %d to %d material units", MinForgeUnits, MaxForgeUnits)
	}
	return forgeOrder{blankID, blank, inputs, affinities, total, matched, gradePoints}, nil
}

func (c *Catalog) finishForge(order forgeOrder, plan ForgePlan, roll *rand.Rand) ForgedWeapon {
	f := c.forging
	polarity := dominant(order.affinities.polarity[:], roll)
	element := dominant(order.affinities.element[:], roll)
	branch := [2]int{polarity, element}
	qi := drawForgeQi(f.qi[branch], order.affinities.qi, roll)
	gradeID := plan.GradeID
	grade := c.grades[gradeID]
	position := (grade.TierID-1)*3 + grade.QualityID
	result := ForgedWeapon{
		CatalogVersion: c.version, PrototypeID: order.blankID, CategoryID: order.blank.category,
		PolarityID: polarity, ElementID: element, QiID: qi, ClassID: fmt.Sprintf("affinity.p%d.e%d.q%02d", polarity, element, qi), GradeID: gradeID,
		BaseAttack: ((10*position+2+order.matched)*grade.MultiplierBPS + 5000) / 10000, BaseDefense: ((2*position+order.blank.grade)*grade.MultiplierBPS + 5000) / 10000,
		BaseBonuses: make(map[string]int), Bonuses: make(map[string]int), Inputs: maps.Clone(order.inputs),
	}
	f.rollAffixes(&result, grade, plan.RollBiasBPS, roll)
	return result
}
