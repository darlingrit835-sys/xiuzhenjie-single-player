package main

import (
	"fmt"
	"maps"
	"slices"
	"strings"

	"xiuzhenjie/server/internal/affinity"
)

func (w *smithingWorkshop) trainingText(role Record) string {
	rank, exp, _ := smithTraining(role)
	ceiling := int(w.professions[rank].MaxExperience)
	xp := min(exp, ceiling)
	name := w.professions[rank].Name
	if rank == 9 && xp == ceiling {
		return fmt.Sprintf("%s · 圆满 %d/%d", name, ceiling, ceiling)
	}
	if rank == 9 {
		return fmt.Sprintf("%s · 修习 %d/%d · 距圆满 %d", name, xp, ceiling, ceiling-xp)
	}
	return fmt.Sprintf("%s · 修习 %d/%d · 距晋品 %d", name, xp, ceiling, ceiling-xp)
}

// Keep receipts truthful at the end of the ninth rank, including when the
// nominal experience for a furnace would run past the remaining progression.
func (w *smithingWorkshop) earnedExperience(role Record, offered int) int {
	rank, exp, _ := smithTraining(role)
	capacity := 0
	for i := rank; i <= 9; i++ {
		capacity += int(w.professions[i].MaxExperience)
	}
	return min(max(0, offered), max(0, capacity-exp))
}

func (w *smithingWorkshop) flameControlText(m affinity.FlameMastery) string {
	status := "火候得宜"
	gap := m.SmithRank - m.RequiredRank
	if gap < 0 {
		status = fmt.Sprintf("越阶驭火 %d 品", -gap)
	} else if gap > 0 {
		status = fmt.Sprintf("熟练控火 · 高出 %d 品", gap)
	}
	return fmt.Sprintf("%s（宜%s）", status, w.professions[m.RequiredRank].Name)
}

func (w *smithingWorkshop) materialUseText(units map[string]int) string {
	parts := make([]string, 0, len(units))
	for _, id := range slices.Sorted(maps.Keys(units)) {
		if units[id] > 0 {
			parts = append(parts, fmt.Sprintf("%s ×%d", w.text.ItemTemplates[id].Name, units[id]))
		}
	}
	if len(parts) == 0 {
		return "无"
	}
	return strings.Join(parts, "、")
}

func factorLine(label string, factor affinity.ForgeFactor) string {
	var parts []string
	for _, v := range []struct {
		key string
		bps int
	}{{"成器", factor.SuccessBPS}, {"返材", factor.ReturnBPS}, {"成色", factor.RollBiasBPS}, {"经验", factor.ExpBPS}} {
		if v.bps != 0 {
			parts = append(parts, fmt.Sprintf("%s %+.2f%%", v.key, float64(v.bps)/100))
		}
	}
	if len(parts) == 0 {
		return ""
	}
	return label + "　" + strings.Join(parts, " · ")
}

func (order forgeRequest) studyBonus(challenge patternChallenge) int {
	if order.PatternID == "" {
		return 0
	}
	return 2 * max(0, challenge.Level-1)
}

func (w *smithingWorkshop) firstForgeView(role Record, order forgeRequest, targetGrade int, inputs map[string]int, challenge patternChallenge, shape forgeShape, plan affinity.ForgePlan) Record {
	_, smith, method := w.context(role, order)
	fireName := w.text.ForgeFlames[order.FlameID].Name
	headings := [...]string{
		affinity.BaseHeat: "基础炉况", affinity.SmithArts: "师阶工艺", affinity.FlameArts: "炉火 · " + fireName,
		affinity.FireControl: "掌火", affinity.TargetArts: "目标品阶工艺", affinity.TargetDifficulty: "目标品阶难度",
		affinity.PatternLoad: "器图工序", affinity.FlameAffinity: "火与胚料五行", affinity.MethodAffinity: "纳气功法与胚料五行",
		affinity.MaterialGrade: "料阶与目标差", affinity.RecipeMatch: fmt.Sprintf("配方契合 %d/%d 份", plan.Matched, plan.Units),
		affinity.WornTools: "已穿戴炼器词条", affinity.Bounds: "工艺上下限",
	}
	lines := make([]string, 0, len(headings)+1)
	if shape.quality != "" {
		lines = append(lines, fmt.Sprintf("%s定阶　%s → %s", shape.quality, w.gradeName(targetGrade), w.gradeName(order.GradeID)))
	}
	for i, label := range headings {
		if row := factorLine(label, plan.Sources[i]); row != "" {
			lines = append(lines, row)
		}
	}
	bonus := order.studyBonus(challenge)
	successExperience := w.earnedExperience(role, plan.Experience(true)+bonus)
	studyEarned := successExperience - w.earnedExperience(role, plan.Experience(true))
	if bonus > 0 {
		study := fmt.Sprintf("临图研习　成器额外 +%d 点经验", bonus)
		if studyEarned < bonus {
			study += fmt.Sprintf("（圆满前可得 %d 点）", studyEarned)
		}
		lines = append(lines, study)
	}
	vigor := w.forgeVigor(targetGrade, challenge.Level, 1, true)
	lines = append(lines, fmt.Sprintf("开炉精力 %d（当前 %d） · 每炉手作一件", vigor, forgeCurrentVigor(role)))
	out := Record{"vigorCost": vigor, "moldQuality": shape.quality, "smithRankName": smith, "methodName": method, "flameName": fireName,
		"gradeName": w.gradeName(order.GradeID), "gradeId": order.GradeID, "successBps": plan.SuccessBPS, "returnBps": plan.ReturnBPS, "rollBiasBps": plan.RollBiasBPS, "expBps": plan.ExpBPS,
		"successExperience": successExperience, "failureExperience": w.earnedExperience(role, plan.Experience(false)), "studyExperience": studyEarned,
		"costText": w.materialUseText(inputs), "factorText": strings.Join(lines, "\n"), "controlText": w.flameControlText(plan.FlameMastery), "trainingText": w.trainingText(role),
		"processText": fmt.Sprintf("败炉每份返材 %.2f%% · 成色偏移 %+.2f%% · 修习经验 %+.2f%%", float64(plan.ReturnBPS)/100, float64(plan.RollBiasBPS)/100, float64(plan.ExpBPS)/100)}
	challenge.DescribeInto(out)
	return out
}

func addFirstForgeReceipt(view, template Record) Record {
	if snapshot, ok := template["FirstForgeReceipt"].(Record); ok {
		maps.Copy(view, snapshot)
	}
	return view
}
