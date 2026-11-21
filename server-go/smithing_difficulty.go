package main

import (
	"fmt"
	"maps"
	"slices"
	"strconv"
	"strings"
)

type patternChallenge struct {
	Level      int
	Name, Text string
}

func (c patternChallenge) DescribeInto(record Record) {
	record["difficulty"], record["difficultyName"], record["difficultyText"] = c.Level, c.Name, c.Text
	record["suggestedSmithRank"] = c.SuggestedRank()
}

func (c patternChallenge) SuggestedRank() int { return 2*c.Level - 1 }

type patternWork struct {
	points int
	label  string
}

var blankWork = map[string]patternWork{
	"wp.sword":  {0, "直脊成锋"},
	"wp.saber":  {1, "弧刃收背"},
	"wp.spear":  {1, "锋身合轴"},
	"wp.staff":  {1, "杖心通脉"},
	"wp.bow":    {2, "双臂同张"},
	"obj.robe":  {0, "牵丝成衣"},
	"obj.boots": {1, "合底定形"},
	"obj.armor": {1, "收边抱心"},
}

var difficultyNames = [...]string{"", "一难·入门", "二难·熟手", "三难·精工", "四难·繁工", "五难·绝艺"}
var difficultyAdvice = [...]string{"", "建议一品", "建议三品", "建议五品", "建议七品", "建议九品"}

// The drawing fixes the hammer sequence. Changing a charge only changes how
// hard it is to fuse the materials; it never invalidates completed strikes.
var patternHeat = [...][4]int{
	{3, 7, 4, 6}, {3, 7, 4, 6}, {3, 6, 2, 7},
	{2, 7, 3, 7}, {2, 8, 2, 6}, {1, 8, 1, 8},
}

func (w *smithingWorkshop) strikeTargets(patternID string) [4]int {
	if p := w.patterns[patternID]; p != nil {
		return patternHeat[w.moldDifficulty(p).Level]
	}
	return patternHeat[0]
}

func (w *smithingWorkshop) orderDifficulty(order forgeRequest, inputs map[string]int) patternChallenge {
	if pattern := w.patterns[order.PatternID]; pattern != nil {
		recipe, _ := pattern["Inputs"].(Record)
		if maps.Equal(recipeQuantities(recipe), inputs) {
			return w.moldDifficulty(pattern)
		}
	}
	return w.ratePattern(order.BlankID, order.GradeID, inputs, w.engravings[order.PatternID])
}

func (w *smithingWorkshop) validatePatternOrder(order forgeRequest) error {
	if order.PatternID == "" {
		return nil
	}
	p := w.patterns[order.PatternID]
	if p == nil || order.BlankID != roleString(p, "BlankId") || order.GradeID != int(roleInt(p, "Rank")) {
		return forgeFailure("临图须使用该图的器胚与目标品阶，请重选器图或自拟形制")
	}
	if len(order.Strokes) != 4 {
		return forgeFailure("临图首炼须在砧上完成四锤塑形")
	}
	return nil
}

func (w *smithingWorkshop) ratePattern(blank string, gradeID int, inputs map[string]int, engraving patternWork) patternChallenge {
	item, _ := w.rules.Item(blank)
	form := blankWork[item.CategoryID]
	grade, _ := w.rules.Grade(gradeID)
	points := form.points + engraving.points + 2*max(0, grade.TierID-1)
	if grade.QualityID == 3 {
		points++
	}
	var reasons []string
	if form.label != "" {
		reasons = append(reasons, form.label)
	}
	if engraving.label != "" {
		reasons = append(reasons, engraving.label)
	}
	if gradeID > 0 {
		reasons = append(reasons, w.gradeName(gradeID))
	}
	elements := map[int]bool{}
	for id := range inputs {
		material, _ := w.rules.Item(id)
		profile, _ := w.rules.Profile(material.ProfileID)
		if profile.PrimaryElementID > 0 {
			elements[profile.PrimaryElementID] = true
		}
	}
	points += min(2, max(0, len(inputs)-2))
	if len(inputs) > 0 {
		reasons = append(reasons, fmt.Sprintf("%d味合炼", len(inputs)))
	}
	// Count each opposing pair once, without depending on map iteration order.
	ordered := slices.Sorted(maps.Keys(elements))
	var clashes []string
	for i, element := range ordered {
		for _, other := range ordered[i+1:] {
			relation := w.rules.ForgeElementRelation(element, other)
			if relation == "overcomes" || relation == "overcomeBy" {
				clashes = append(clashes, w.text.Elements[strconv.Itoa(element)].Name+w.text.Elements[strconv.Itoa(other)].Name)
			}
		}
	}
	if len(clashes) > 0 {
		points += min(2, len(clashes))
		reasons = append(reasons, strings.Join(clashes, "、")+"相克")
	}
	level := min(5, 1+(points+1)/2)
	return patternChallenge{Level: level, Name: difficultyNames[level], Text: difficultyAdvice[level] + " · " + strings.Join(reasons, " · ")}
}

func savedChallenge(t Record) (patternChallenge, bool) {
	c := patternChallenge{int(roleInt(t, "difficulty")), roleString(t, "difficultyName"), roleString(t, "difficultyText")}
	return c, c.Level >= 1 && c.Level <= 5 && c.Name != "" && c.Text != ""
}

// Old player molds keep only their original blank, output grade and ingredient lots.
// Those are sufficient to reconstruct a rating without changing the old item.
func (w *smithingWorkshop) moldDifficulty(t Record) patternChallenge {
	if c, ok := savedChallenge(t); ok {
		return c
	}
	if c, ok := savedChallenge(w.patterns[roleString(t, "MoldId")]); ok {
		return c
	}
	recipe, _ := t["Inputs"].(Record)
	return w.ratePattern(roleString(t, "BlankId"), int(roleInt(t, "Rank")), recipeQuantities(recipe), patternWork{})
}

func recipeQuantities(recipe Record) map[string]int {
	inputs := make(map[string]int, len(recipe))
	for id := range recipe {
		inputs[id] = int(roleInt(recipe, id))
	}
	return inputs
}
