package main

import (
	"math"
	"testing"
)

// Reads the shipped level and recipe catalogs and executes the normal crafting
// planners. These rows also make the nine-stage balance sheet reproducible.
func TestArtisanStageCurve(t *testing.T) {
	s, _ := alchemyTestServer(t, t.TempDir())
	if err := s.loadGameTables(); err != nil {
		t.Fatal(err)
	}
	choice, _ := sigilSelect("ward", "strong")
	designs := []string{"songying", "tingxue", "xianyue", "guichao", "baizhi", "chunshan", "feishuang", "liuhui", "jinglei"}
	fires := []string{"ember", "ember", "ember", "spring", "spring", "spring", "solar", "solar", "star"}
	for rank := 1; rank <= 9; rank++ {
		level := int64(1 + 10*(rank-1))
		row := s.levelInfo(level)
		role := Record{"Level": level, "Perception": int64(math.Round(row.perception)), "Physique": int64(math.Round(row.toughTenacity)), "ForgeRank": rank, "GardenRank": rank, "TalismanRank": rank}
		w := s.smithing
		pattern := w.patterns[smithingTemplateID("association.pattern.v1", designs[rank-1])]
		order := forgeRequest{PatternID: roleString(pattern, "TemplateId"), BlankID: roleString(pattern, "BlankId"), GradeID: int(roleInt(pattern, "Rank")), FlameID: "flame." + fires[rank-1]}
		inputs := recipeQuantities(pattern["Inputs"].(Record))
		ctx, _, _ := w.context(role, order)
		challenge := w.orderDifficulty(order, inputs)
		ctx.PatternDifficulty = challenge.Level
		plan, err := w.rules.PlanForge(order.BlankID, inputs, ctx)
		if err != nil {
			t.Fatal(err)
		}
		chance, returned := float64(plan.SuccessBPS)/10000, float64(plan.ReturnBPS)/10000
		if chance <= 0 {
			t.Fatalf("rank %d: no viable forge", rank)
		}
		expectedMats := float64(plan.Units) * (1 + (1-chance)*(1-returned)/chance)
		lesson := artisanLesson(sigilRanks[rank], rank)
		strong := choice.usablePower(role, choice.power(role, true))
		cropSeconds := gardenVariety("lotus").growSeconds(role) * 4 / 5
		if strong*100 > int(equippedBattleStats(role).blood)*12 || lesson < 1 || cropSeconds < 600 {
			t.Fatal(role)
		}
		t.Logf("%d | %d | %d | %d | %d | %s | %.2f%% | %.2f%% | %.2f | %d | %d | %d | %.2f | %d",
			rank, level, equippedBattleStats(role).blood, lesson, (sigilRanks[rank]+lesson-1)/lesson, roleString(pattern, "Name"),
			chance*100, returned*100, expectedMats, plan.Experience(true)+order.studyBonus(challenge), plan.Experience(false), strong,
			100*float64(strong)/float64(equippedBattleStats(role).blood), cropSeconds)
	}
}

func TestManuscriptProgressAndTuition(t *testing.T) {
	// Test the lesson charge in isolation. Publication also bills workshop materials and cost.
	for _, id := range []string{"shu", "zhen", "dao", "qiao", "shou"} {
		h := voidFindHall(id)
		role := Record{"Level": 21, "ShinyStone": 35}
		if voidRank(role, h.Type) != 0 {
			t.Fatalf("untrained role admitted to %s", id)
		}
		role["Live3Type"], role["Live3Level"] = h.Type, 1
		for brief := range 3 {
			if brief == 2 {
				if err := artisanManuscript(role, h, 1, brief); err == nil {
					t.Fatal("unfunded lesson", id)
				}
				role["ShinyStone"] = roleInt(role, "ShinyStone") + 1
			}
			for range 2 {
				if err := artisanManuscript(role, h, 1, brief); err != nil {
					t.Fatal(id, err)
				}
			}
		}
		if roleInt(role, "ShinyStone") != 0 || artisanResearchRank(role, h.Type) != 2 || roleInt(role, "Live3Level") != 2 {
			t.Fatal(id, role)
		}
	}
}

func TestBrewHealingAndRecipientDefense(t *testing.T) {
	s, _ := alchemyTestServer(t, t.TempDir())
	base := Record{"Level": 21, "Perception": 4000, "Physique": 4000, "PureBlood": 1000, "CurrentPureBlood": 10,
		"EquippedGoods": []any{Record{"Type": 15, "TemplateId": "ugc.test", "BlankId": "wp.test", "BaseAttack": 1, "Bonuses": Record{"healing": 500}}}}
	if s.consumeBrewDose(base, Record{"BrewEffect": "recover_blood", "BrewBasis": "max_blood", "EffectValue": 2000}) != 2000 || roleInt(base, "CurrentPureBlood") != 220 {
		t.Fatal("healing affix missing", base)
	}
	if s.consumeBrewDose(base, Record{"BrewEffect": "absorb_damage", "EffectValue": 720}) != 2000 || roleInt(base, "AlchemyWard") != 5760 {
		t.Fatal("shield must be 36 percent of 16k combat health", base)
	}
	if s.consumeBrewDose(base, Record{"BrewEffect": "absorb_damage", "EffectValue": 520}) != 4971 {
		t.Fatal("wasted weaker shield")
	}
	base["Level"] = 90
	if s.consumeBrewDose(base, Record{"BrewEffect": "gain_super_air", "EffectValue": 200, "PoisonValue": 1}) != 4971 || roleInt(base, "CurrentPoisonVal") != 0 {
		t.Fatal("wasted endgame dose")
	}
}
