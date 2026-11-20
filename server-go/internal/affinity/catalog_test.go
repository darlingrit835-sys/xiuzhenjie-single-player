package affinity

import (
	"iter"
	"os"
	"slices"
	"strings"
	"testing"
)

const sample = `{
  "schemaVersion": 3, "catalogVersion": "fixture-v1",
  "itemCategories":{"metal":{"parentId":null,"detailTable":"material"},"weapon":{"detailTable":"weapon"}},
  "itemTemplates":{
    "ore":{"categoryId":"metal","profileId":"stone","gradeId":4,"origin":"existing"},
    "coal":{"categoryId":"metal","gradeId":1},
    "sword":{"categoryId":"weapon","gradeId":1,"prototypeId":"blade"},
    "blade":{"categoryId":"weapon","gradeId":1}
  },
  "profiles":{"stone":{"kindId":"ore","primaryElementId":5,"primaryQiId":8,"sourceId":"5","origin":"existing"}},
  "grades":{"1":{"tierId":1,"qualityId":1,"multiplierBps":10000}},
  "legacyGrades":{"4":{"tierId":1,"qualityId":4}},
  "creationInputs":{"sword":{"ore":2,"coal":1},"blade":{"ore":3}}
}`

func TestLookups(t *testing.T) {
	c, err := Load(strings.NewReader(sample))
	if err != nil {
		t.Fatal(err)
	}
	item, found := c.Item("ore")
	if !found || item.Origin != "existing" || c.Version() != "fixture-v1" {
		t.Fatalf("item %+v, version %q", item, c.Version())
	}
	cat, found := c.Category(item.CategoryID)
	if !found || cat.DetailTable != "material" || cat.ParentID != "" {
		t.Fatalf("category %+v", cat)
	}
	profile, found := c.Profile(item.ProfileID)
	if !found || profile.KindID != "ore" || profile.SourceID != "5" || profile.PrimaryQiID != 8 || profile.PolarityID != 0 {
		t.Fatalf("profile %+v", profile)
	}
	grade, found := c.Grade(item.GradeID)
	if !found || !grade.Legacy || grade.QualityID != 4 || grade.MultiplierBPS != 0 {
		t.Fatalf("legacy grade %+v", grade)
	}
	grade, found = c.Grade(1)
	if !found || grade.Legacy || grade.MultiplierBPS != 10000 {
		t.Fatalf("creatable grade %+v", grade)
	}
	for _, test := range []struct {
		query iter.Seq2[string, int]
		want  []recipeEntry
	}{
		{c.Inputs("sword"), []recipeEntry{{"coal", 1}, {"ore", 2}}},
		{c.Outputs("ore"), []recipeEntry{{"blade", 3}, {"sword", 2}}},
		{c.Inputs("ore"), nil},
		{c.Outputs("unknown"), nil},
	} {
		var got []recipeEntry
		for id, quantity := range test.query {
			got = append(got, recipeEntry{id, quantity})
		}
		if !slices.Equal(got, test.want) {
			t.Errorf("recipe entries %v, want %v", got, test.want)
		}
	}
	// Mutating a returned value cannot change the catalog.
	item.GradeID = 16
	item, _ = c.Item("ore")
	if item.GradeID != 4 {
		t.Fatal("returned item mutated the catalog")
	}
}

func TestInvalidCatalog(t *testing.T) {
	for _, invalid := range []string{
		strings.Replace(sample, `"schemaVersion": 3`, `"schemaVersion": 2`, 1),
		strings.Replace(sample, `"catalogVersion": "fixture-v1"`, `"catalogVersion": ""`, 1),
		strings.Replace(sample, `"ore":2`, `"unknown":2`, 1),
		strings.Replace(sample, `"coal":1`, `"coal":0`, 1),
		strings.Replace(sample, `"legacyGrades":{"4"`, `"legacyGrades":{"1"`, 1),
		sample + `{}`,
	} {
		if _, err := Load(strings.NewReader(invalid)); err == nil {
			t.Error("accepted invalid catalog")
		}
	}
}

func TestMethodInfluenceLookup(t *testing.T) {
	// This catalog fixture has two grades: regular 1 and legacy 4. Two
	// independent systems can assign different ratios to the same method.
	const tables = `"systems":{"smithing":{},"alchemy":{}},"methodInfluences":{"smithing":{"3":{"1":[17,15],"4":[43,19]}},"alchemy":{"3":{"1":[1,3],"4":[9,7]}}},`
	withMethods := strings.Replace(sample, `"schemaVersion"`, tables+`"schemaVersion"`, 1)
	c, err := Load(strings.NewReader(withMethods))
	if err != nil {
		t.Fatal(err)
	}
	for _, tc := range []struct {
		system         string
		kind, id, n, d int
	}{
		{"smithing", 3, 4, 43, 19}, {"alchemy", 3, 4, 9, 7}, {"smithing", 3, 1, 17, 15},
	} {
		n, d, ok := c.MethodInfluence(tc.system, tc.kind, tc.id)
		if !ok || n != tc.n || d != tc.d {
			t.Fatalf("method lookup %+v: %d/%d, found=%t", tc, n, d, ok)
		}
	}
	if _, _, ok := c.MethodInfluence("smithing", 1, 1); ok {
		t.Fatal("unknown method type resolved")
	}
	for _, invalid := range []string{
		strings.Replace(withMethods, `"4":[43,19]`, `"5":[43,19]`, 1),
		strings.Replace(withMethods, `[43,19]`, `[0,19]`, 1),
		strings.Replace(withMethods, `[43,19]`, `[43,0]`, 1),
		strings.Replace(withMethods, `[43,19]`, `[43,19,1]`, 1),
		strings.Replace(withMethods, `"alchemy":{}`, `"other":{}`, 1),
	} {
		if _, err := Load(strings.NewReader(invalid)); err == nil {
			t.Fatal("invalid method influence accepted")
		}
	}
}

func loadBundled(t testing.TB) *Catalog {
	t.Helper()
	f, err := os.Open("../../../server/data/config/cultivation_affinities.json")
	if err != nil {
		t.Fatal(err)
	}
	defer f.Close()
	catalog, err := Load(f)
	if err != nil {
		t.Fatal(err)
	}
	return catalog
}

func TestBundledCatalog(t *testing.T) {
	catalog := loadBundled(t)
	for id, item := range catalog.items {
		if _, ok := catalog.Category(item.CategoryID); !ok {
			t.Errorf("item %s: unknown category %q", id, item.CategoryID)
		}
		if _, ok := catalog.Profile(item.ProfileID); !ok {
			t.Errorf("item %s: unknown profile %q", id, item.ProfileID)
		}
		if _, ok := catalog.Grade(item.GradeID); !ok {
			t.Errorf("item %s: unknown grade %d", id, item.GradeID)
		}
	}
	for output := range catalog.inputs {
		for material, quantity := range catalog.Inputs(output) {
			found := false
			for product, required := range catalog.Outputs(material) {
				if product == output && required == quantity {
					found = true
					break
				}
			}
			if !found {
				t.Errorf("missing reverse lookup for %s -> %s", material, output)
			}
		}
	}
}

func BenchmarkBundledRecipeLookup(b *testing.B) {
	c := loadBundled(b)
	const material = "mat.cold_dew"
	if len(c.outputs[material]) == 0 {
		b.Skip("catalog has no recipes for this material")
	}
	b.ReportAllocs()
	for b.Loop() {
		for id := range c.Outputs(material) {
			item, _ := c.Item(id)
			c.Grade(item.GradeID)
		}
	}
}
