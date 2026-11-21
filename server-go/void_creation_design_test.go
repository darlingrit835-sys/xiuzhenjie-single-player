package main

import (
	"encoding/json"
	"os"
	"regexp"
	"slices"
	"strconv"
	"strings"
	"testing"
)

func TestVoidCreationV2WorldCatalog(t *testing.T) {
	data, err := os.ReadFile("../server/data/config/cultivation_affinities.json")
	if err != nil {
		t.Fatal(err)
	}
	var world struct {
		Elements                            map[int]struct{ Generates, Overcomes int }
		QiTypes                             map[int]struct{}
		ElementQiWeights, PolarityQiWeights map[int]map[int]int
		ItemTemplates                       map[string]struct{ CategoryID, ProfileID string }
		Profiles                            map[string]struct{ PolarityID, PrimaryElementID, SecondaryElementID, PrimaryQiID, SecondaryQiID int }
	}
	if err := json.Unmarshal(data, &world); err != nil {
		t.Fatal(err)
	}
	if len(world.QiTypes) != len(voidCraftV2.Qi) || len(world.Elements) != len(voidCraftV2.Elements) {
		t.Fatal("world catalog and manuscript edition disagree on qi or elements")
	}
	checkRow := func(label string, id int, row [12]int, source map[int]int) {
		t.Helper()
		for i, weight := range row {
			want, ok := source[i+1]
			if !ok || weight != want {
				t.Errorf("%s %d, qi %d: got %d, world catalog has %d (present %v)", label, id, i+1, weight, want, ok)
			}
		}
	}
	for i, row := range voidCraftV2.ElementQiWeights {
		checkRow("element", i+1, row, world.ElementQiWeights[i+1])
		e := world.Elements[i+1]
		if e.Generates != voidCraftV2.Generates[i] || e.Overcomes != voidCraftV2.Overcomes[i] {
			t.Errorf("element %d has a different generating/overcoming cycle", i+1)
		}
	}
	for i, row := range voidCraftV2.PolarityQiWeights {
		checkRow("polarity", i+1, row, world.PolarityQiWeights[i+1])
	}
	for _, h := range voidCraftV2.Halls {
		item := world.ItemTemplates[h.MaterialID]
		if !strings.HasPrefix(item.CategoryID, "mat.") {
			t.Errorf("%s requires %q, missing from world material templates", h.ID, h.MaterialID)
		}
		profile, ok := world.Profiles[item.ProfileID]
		primarySecondary := func(primary, secondary int) []int {
			if secondary == 0 {
				return []int{primary}
			}
			return []int{primary, secondary}
		}
		if !ok || h.MaterialPolarity != profile.PolarityID || !slices.Equal(h.MaterialElements, primarySecondary(profile.PrimaryElementID, profile.SecondaryElementID)) ||
			!slices.Equal(h.MaterialQi, primarySecondary(profile.PrimaryQiID, profile.SecondaryQiID)) || h.MaterialPrice <= 0 {
			t.Errorf("%s has invalid material price or differs from original material profile %q", h.ID, item.ProfileID)
		}
	}
}

func craftTestResult(t *testing.T, role Record, q voidRequest) Record {
	t.Helper()
	r, err := voidCreateV2(role, q)
	if err != nil {
		t.Fatalf("%+v: %v", q, err)
	}
	return r
}

// Apply exactly the edits shown to a player, preserving all other choices.
func craftTestRepair(t *testing.T, role Record, q voidRequest, h *voidCraftHall, hint string) {
	t.Helper()
	q.Steps = slices.Clone(q.Steps)
	for _, edit := range strings.Split(strings.TrimSuffix(hint, "。"), "，") {
		found := false
		for i, stage := range h.Stages {
			for j, option := range stage.Options {
				if edit == stage.Name+"改用「"+option+"」" && q.Steps[i] != j {
					q.Steps[i] = j
					found = true
				}
			}
		}
		if !found {
			t.Fatalf("unusable edit %q in hint %q", edit, hint)
		}
	}
	if !roleBool(craftTestResult(t, role, q), "success") {
		t.Fatalf("applying hint %q still fails", hint)
	}
}

// Each affinity control is tested in isolation on the same successful plan.
func craftTestAffinity(t *testing.T, role Record, q voidRequest, baseline Record) {
	t.Helper()
	for axis, limit := range []int{5, 3, 12} {
		var changed bool
		for choice := 1; choice <= limit; choice++ {
			alt := q
			alt.Steps = slices.Clone(q.Steps)
			switch axis {
			case 0:
				alt.Route = choice
			case 1:
				alt.Adjust = choice
			case 2:
				alt.Steps[3] = choice
			}
			result := craftTestResult(t, role, alt)
			if axis == 2 {
				alt.Name = roleString(result, "suggestedName")
				if name, err := voidName(alt); err != nil || name == "" || name != alt.Name {
					t.Fatalf("suggested name %q for qi %d is not signable: %q, %v", alt.Name, choice, name, err)
				}
			}
			if roleBool(result, "success") && roleInt(result, "score") != roleInt(baseline, "score") && roleInt(result, "value") != roleInt(baseline, "value") {
				changed = true
			}
		}
		if !changed {
			t.Errorf("affinity control %d has no effect on score and appraised value", axis)
		}
	}
}

var craftDisplayNumbers = regexp.MustCompile(`[0-9]+`)

// A successful preview exposes both totals and all their components to the player.
func craftBreakdown(t *testing.T, r Record, field string, count int) []int {
	t.Helper()
	parts := craftDisplayNumbers.FindAllString(roleString(r, field), -1)
	if len(parts) != count {
		t.Fatalf("missing terms in %s: %q", field, r[field])
	}
	values := make([]int, count)
	for i, s := range parts {
		values[i], _ = strconv.Atoi(s)
	}
	return values
}

func checkCraftRewards(t *testing.T, h *voidCraftHall, q voidRequest, r Record, earned map[string]bool) {
	t.Helper()
	effect := roleString(r, "effectText")
	stone := 3 + 2*q.Grade + q.Steps[0] + q.Steps[1] + q.Steps[2]
	mastery := 0
	if strings.HasPrefix(effect, "已成工艺词条") {
		if strings.Contains(effect, h.EconomyAffix) {
			earned[h.EconomyAffix] = true
			stone -= 2
		}
		if strings.Contains(effect, h.MasterAffix) {
			earned[h.MasterAffix] = true
			mastery = 3
		}
	}
	materialFit := 0
	for _, match := range []bool{q.Adjust == h.MaterialPolarity, slices.Contains(h.MaterialElements, q.Route), slices.Contains(h.MaterialQi, q.Steps[3])} {
		if match {
			materialFit++
		}
	}
	points := craftBreakdown(t, r, "scoreText", 7)
	value := craftBreakdown(t, r, "valueText", 6)
	sum := func(nums []int) int {
		total := 0
		for _, num := range nums {
			total += num
		}
		return total
	}
	needed := 1 + (q.Grade-1)/3
	if points[4] != mastery || points[5] != materialFit || min(points[6], sum(points[:6])) != int(roleInt(r, "score")) {
		t.Fatalf("score does not reflect earned traits/affinity: %q -> %v", r["scoreText"], r["score"])
	}
	if int(roleInt(r, "stoneCost")) != stone || value[0] != stone || value[1] != needed*h.MaterialPrice || value[4] != materialFit*q.Grade || sum(value) != int(roleInt(r, "value")) {
		t.Fatalf("appraisal does not reflect costs/affinity: %q -> %v", r["valueText"], r["value"])
	}
}

func craftTestMaterialFit(t *testing.T, role Record, h *voidCraftHall, q voidRequest) {
	t.Helper()
	// Grade four uses two real units. Compare all three material matches with none.
	q.Grade = 4
	q.Steps = slices.Clone(q.Steps)
	q.Route, q.Adjust, q.Steps[3] = h.MaterialElements[0], h.MaterialPolarity, h.MaterialQi[0]
	checkCraftRewards(t, h, q, craftTestResult(t, role, q), map[string]bool{})
	for element := 1; element <= 5; element++ {
		if !slices.Contains(h.MaterialElements, element) {
			q.Route = element
			break
		}
	}
	q.Adjust = h.MaterialPolarity%3 + 1
	for qi := 1; qi <= 12; qi++ {
		if !slices.Contains(h.MaterialQi, qi) {
			q.Steps[3] = qi
			break
		}
	}
	checkCraftRewards(t, h, q, craftTestResult(t, role, q), map[string]bool{})
}

func TestVoidCreationV2PlayableManuscripts(t *testing.T) {
	wanted := []string{"dan", "qi", "fu", "shu", "zhen", "dao", "qiao", "zhi", "shou"}
	if len(voidCraftV2.Halls) != len(wanted) {
		t.Fatalf("wanted %d halls, got %d", len(wanted), len(voidCraftV2.Halls))
	}
	for _, id := range wanted {
		t.Run(id, func(t *testing.T) {
			h, hall := voidCraftHallFor(id), voidFindHall(id)
			if h == nil || hall == nil {
				t.Fatal("hall missing from rulebook or admission service")
			}
			role := Record{"Level": 81, "Live1Type": hall.Type, "Live1Level": 9}
			earned := map[string]bool{}
			for i, brief := range h.Briefs {
				t.Run(brief.Name, func(t *testing.T) {
					wins, failures := 0, 0
					for n := range 27 {
						q := voidRequest{Version: 2, HallID: id, Grade: 1, Brief: i, Route: brief.Element, Adjust: brief.Polarity,
							Steps: []int{n / 9, n / 3 % 3, n % 3, brief.Qi}}
						r := craftTestResult(t, role, q)
						if !roleBool(r, "success") {
							failures++
							if roleInt(r, "value") != 0 {
								t.Fatal("failed work has appraised value", r)
							}
							craftTestRepair(t, role, q, h, roleString(r, "next"))
							continue
						}
						wins++
						if wins == 1 {
							craftTestAffinity(t, role, q, r)
							craftTestMaterialFit(t, role, h, q)
						}
						mats, ok := r["materials"].([]Record)
						if !ok || len(mats) != 1 || roleString(mats[0], "templateId") != h.MaterialID || roleInt(mats[0], "count") < 1 {
							t.Fatalf("work has no usable material requirement: %v", mats)
						}
						checkCraftRewards(t, h, q, r, earned)
					}
					if wins == 0 || failures == 0 {
						t.Errorf("expected both working and repairable plans among 27 choices, got %d/%d", wins, failures)
					}
				})
			}
			for _, trait := range []string{h.EconomyAffix, h.MasterAffix} {
				if trait == "" || !earned[trait] {
					t.Errorf("no successful manuscript earns trait %q in any brief", trait)
				}
			}
		})
	}
}

func TestVoidRealmRetiredCareerIgnoresWorkshopRank(t *testing.T) {
	for _, h := range voidHalls {
		t.Run(h.ID, func(t *testing.T) {
			key := strconv.Itoa(h.Type)
			role := Record{"Level": 81, "AlchemyRank": 5, "ForgeRank": 5, "TalismanRank": 5, "GardenRank": 5,
				"ArtisanResearch": Record{key: Record{"Rank": 5}}}
			if err := voidAccess(role, &h, 5); err != nil {
				t.Fatal("existing workshop qualification was lost", err)
			}
			role["ResidenceRetiredCareers"] = Record{key: true}
			if err := voidAccess(role, &h, 1); err == nil {
				t.Fatal("retired career regained access through a workshop or research record")
			}
			role["Live4Type"], role["Live4Level"] = h.Type, 2
			if err := voidAccess(role, &h, 2); err != nil || voidAccess(role, &h, 3) == nil {
				t.Fatalf("retirement plus a surviving slot must use only that slot's rank: %v", err)
			}
		})
	}
}
