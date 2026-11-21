package main

import (
	"fmt"
	"maps"
	"time"
)

// An untrained smith can cast a short order. Each rank adds five places to the furnace.
func forgeBatchLimit(role Record) int {
	rank, _, _ := smithTraining(role)
	return 20 + 5*rank
}

func (w *smithingWorkshop) forgeGradeStep(id int) int64 {
	g, _ := w.rules.Grade(id)
	return int64(max(1, 3*(g.TierID-1)+g.QualityID))
}

func (w *smithingWorkshop) forgeVigor(grade, difficulty, quantity int, handmade bool) int64 {
	step := w.forgeGradeStep(grade)
	if handmade {
		return 2 + int64(difficulty) + (step+1)/2
	}
	// One heat shared across the order, followed by progressively heavier pours.
	return 2 + int64(difficulty) + (int64(quantity)*(4+step)+9)/10
}

func (w *smithingWorkshop) appraisal(t Record) int64 {
	step := w.forgeGradeStep(int(roleInt(t, "Rank")))
	affixes := int64(0)
	for _, key := range []string{"Bonuses", "MaterialAffixes"} {
		stats, _ := t[key].(Record)
		for k := range stats {
			affixes += min(2000, max(0, roleInt(stats, k)))
		}
	}
	return 8 + 7*step + min(1200, roleInt(t, "BaseAttack")+roleInt(t, "BaseDefense"))/2 +
		min(5000, affixes)/50 + int64(moldResonance(roleString(t, "MoldQuality")))
}

// A large order practises maintaining heat. An outgrown diagram teaches less than a new design.
func (w *smithingWorkshop) batchExperience(role Record, difficulty patternChallenge, quantity int) int {
	if quantity < 10 {
		return 0
	}
	rank, _, _ := smithTraining(role)
	coverage := max(20, 100-15*max(0, rank-difficulty.SuggestedRank()))
	lesson := artisanLesson(int(w.professions[rank].MaxExperience), rank)
	return w.earnedExperience(role, max(1, lesson*(quantity/10)*coverage/1000))
}

func forgeCurrentVigor(role Record) int64 {
	snapshot := maps.Clone(role)
	recoverRoleVigor(snapshot, time.Now().Unix())
	return roleInt(snapshot, "CurrentVigor")
}

// Mirror the actual FIFO withdrawal, including locked lots. Bound NPC proceeds
// to paid materials or the salvage value already present in the input stacks.
func forgeMaterialValue(role Record, costs map[string]int) int64 {
	left := maps.Clone(costs)
	var value int64
	for _, v := range goodsList(role) {
		g, _ := v.(Record)
		id := roleString(g, "TemplateId")
		if left[id] < 1 || !forgeUsable(g) {
			continue
		}
		used := min(transferStackCount(g), int64(left[id]))
		left[id] -= int(used)
		credit := max(roleInt(g, "ForgePaidUnit"), roleInt(g, "BrewPaidUnit"), roleInt(g, "RecyclePrice"))
		value += used * min(100000, max(0, credit))
	}
	return value
}

func (w *smithingWorkshop) quoteForgedValue(t Record, budget int64, quantity int) (int64, int64) {
	appraisal := w.appraisal(t)
	// Higher-grade, better worked items retain more of their underlying value.
	percent := min(int64(88), 55+2*w.forgeGradeStep(int(roleInt(t, "Rank")))+int64(moldResonance(roleString(t, "MoldQuality")))+appraisal/60)
	return appraisal, min(appraisal, budget*percent/(100*int64(quantity)))
}

func forgeQuoteText(vigor, appraisal, recycle int64) string {
	return fmt.Sprintf("本炉精力 %d · 每件估值 %d 灵石 · 纳戒回收每件 %d 灵石", vigor, appraisal, recycle)
}
