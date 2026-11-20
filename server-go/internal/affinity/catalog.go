// Package affinity reads the machine catalog by stable ID. A loaded Catalog can
// be shared by concurrent readers. Display text is supplied separately by the UI.
package affinity

import (
	"cmp"
	"encoding/json"
	"fmt"
	"io"
	"iter"
	"slices"
)

type Category struct {
	ParentID    string `json:"parentId"`
	DetailTable string `json:"detailTable"`
}

type Item struct {
	CategoryID  string `json:"categoryId"`
	ProfileID   string `json:"profileId"`
	GradeID     int    `json:"gradeId"`
	PrototypeID string `json:"prototypeId"`
	Origin      string `json:"origin"`
}

// Missing optional IDs have their zero value.
type Profile struct {
	KindID             string `json:"kindId"`
	PolarityID         int    `json:"polarityId"`
	PrimaryElementID   int    `json:"primaryElementId"`
	SecondaryElementID int    `json:"secondaryElementId"`
	PrimaryQiID        int    `json:"primaryQiId"`
	SecondaryQiID      int    `json:"secondaryQiId"`
	SourceID           string `json:"sourceId"`
	ClassID            string `json:"classId"`
	Origin             string `json:"origin"`
}

// Legacy grades carry the original ID but do not define a crafting multiplier.
type Grade struct {
	TierID        int  `json:"tierId"`
	QualityID     int  `json:"qualityId"`
	MultiplierBPS int  `json:"multiplierBps"`
	Legacy        bool `json:"-"`
}

type recipeEntry struct {
	id       string
	quantity int
}

type recipeIndex map[string][]recipeEntry

type Catalog struct {
	version    string
	categories map[string]Category
	items      map[string]Item
	profiles   map[string]Profile
	grades     map[int]Grade
	inputs     recipeIndex
	outputs    recipeIndex
	forging    *forgeIndex
	methods    map[string]map[int]map[int][]int
}

func Load(r io.Reader) (*Catalog, error) {
	var data struct {
		forgeTables
		SchemaVersion  int                              `json:"schemaVersion"`
		CatalogVersion string                           `json:"catalogVersion"`
		Categories     map[string]Category              `json:"itemCategories"`
		Items          map[string]Item                  `json:"itemTemplates"`
		Profiles       map[string]Profile               `json:"profiles"`
		Grades         map[int]Grade                    `json:"grades"`
		LegacyGrades   map[int]Grade                    `json:"legacyGrades"`
		Inputs         map[string]map[string]int        `json:"creationInputs"`
		Systems        map[string]struct{}              `json:"systems"`
		Methods        map[string]map[int]map[int][]int `json:"methodInfluences"`
	}
	decoder := json.NewDecoder(r)
	if err := decoder.Decode(&data); err != nil {
		return nil, fmt.Errorf("affinity catalog: %w", err)
	}
	if err := decoder.Decode(&struct{}{}); err != io.EOF {
		return nil, fmt.Errorf("affinity catalog: unexpected trailing data")
	}
	if data.SchemaVersion != 3 {
		return nil, fmt.Errorf("affinity catalog: unsupported schema version %d", data.SchemaVersion)
	}
	if data.CatalogVersion == "" || data.Categories == nil || data.Items == nil ||
		data.Profiles == nil || data.Grades == nil || data.LegacyGrades == nil || data.Inputs == nil {
		return nil, fmt.Errorf("affinity catalog: missing catalogVersion or required lookup table")
	}
	for id, grade := range data.LegacyGrades {
		if _, found := data.Grades[id]; found {
			return nil, fmt.Errorf("affinity catalog: duplicate grade %d", id)
		}
		grade.Legacy = true
		data.Grades[id] = grade
	}
	catalog := &Catalog{
		version: data.CatalogVersion, categories: data.Categories, items: data.Items,
		profiles: data.Profiles, grades: data.Grades, inputs: make(recipeIndex), outputs: make(recipeIndex),
		methods: data.Methods,
	}
	for system, types := range catalog.methods {
		if _, ok := data.Systems[system]; !ok {
			return nil, fmt.Errorf("affinity catalog: unknown method influence system %q", system)
		}
		for kind, grades := range types {
			if kind < 1 || len(grades) != len(catalog.grades) {
				return nil, fmt.Errorf("affinity catalog: incomplete method influences for %s/%d", system, kind)
			}
			for id := range catalog.grades {
				ratio := grades[id]
				if len(ratio) != 2 || ratio[0] <= 0 || ratio[1] <= 0 {
					return nil, fmt.Errorf("affinity catalog: invalid method influence for %s/%d/%d", system, kind, id)
				}
			}
		}
	}
	for output, inputs := range data.Inputs {
		if _, found := data.Items[output]; !found {
			return nil, fmt.Errorf("affinity catalog: unknown recipe output %q", output)
		}
		for input, quantity := range inputs {
			if _, found := data.Items[input]; !found || quantity <= 0 {
				return nil, fmt.Errorf("affinity catalog: invalid input %q (quantity %d) for %q", input, quantity, output)
			}
			catalog.inputs[output] = append(catalog.inputs[output], recipeEntry{input, quantity})
			catalog.outputs[input] = append(catalog.outputs[input], recipeEntry{output, quantity})
		}
	}
	catalog.inputs.sortByID()
	catalog.outputs.sortByID()
	var err error
	catalog.forging, err = compileForge(catalog, data.forgeTables)
	if err != nil {
		return nil, fmt.Errorf("affinity catalog: %w", err)
	}
	return catalog, nil
}

func (index recipeIndex) sortByID() {
	for _, entries := range index {
		slices.SortFunc(entries, func(a, b recipeEntry) int { return cmp.Compare(a.id, b.id) })
	}
}

func (c *Catalog) Version() string { return c.version }

func (c *Catalog) Category(id string) (Category, bool) {
	category, ok := c.categories[id]
	return category, ok
}

func (c *Catalog) Item(id string) (Item, bool) {
	item, ok := c.items[id]
	return item, ok
}

func (c *Catalog) Profile(id string) (Profile, bool) {
	profile, ok := c.profiles[id]
	return profile, ok
}

func (c *Catalog) Grade(id int) (Grade, bool) {
	grade, ok := c.grades[id]
	return grade, ok
}

// MethodInfluence returns the configured numerator and denominator for an
// equipped method's type and grade in the requested game system.
func (c *Catalog) MethodInfluence(system string, methodType, gradeID int) (int, int, bool) {
	ratio := c.methods[system][methodType][gradeID]
	if len(ratio) != 2 {
		return 0, 0, false
	}
	return ratio[0], ratio[1], true
}

// Inputs yields (material ID, quantity) for a product in ascending ID order.
func (c *Catalog) Inputs(outputID string) iter.Seq2[string, int] {
	return c.inputs.entries(outputID)
}

// Outputs yields (product ID, quantity required) for a material in ascending ID order.
func (c *Catalog) Outputs(inputID string) iter.Seq2[string, int] {
	return c.outputs.entries(inputID)
}

func (index recipeIndex) entries(id string) iter.Seq2[string, int] {
	entries := index[id]
	return func(yield func(string, int) bool) {
		for _, entry := range entries {
			if !yield(entry.id, entry.quantity) {
				return
			}
		}
	}
}
