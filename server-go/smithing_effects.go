package main

import "math"

func moldResonance(quality string) int {
	switch quality {
	case "粗模":
		return 3
	case "良模":
		return 6
	case "精模":
		return 9
	}
	return 0
}

func forgeAffix(role Record, stat string) int64 {
	var bps int64
	worn, _ := wornEquipment(role)
	for _, item := range worn {
		if !isSmithingEquipment(item) {
			continue
		}
		bonuses, _ := item["Bonuses"].(Record)
		material, _ := item["MaterialAffixes"].(Record)
		bps += roleInt(bonuses, stat) + roleInt(material, stat)
	}
	return bps
}

func scaleForgeStat(base, bps int64) int64 {
	return int64(math.Round(float64(base) * (1 + float64(bps)/10000)))
}

func applyForgeSkill(role Record, skill battleSkill, power float64) (battleSkill, float64, int) {
	resonance := 0
	worn, _ := wornEquipment(role)
	for _, item := range worn {
		if isSmithingEquipment(item) && roleInt(item, "FiveElement") == int64(skill.element) && skill.kind == 1 {
			resonance = max(resonance, moldResonance(roleString(item, "MoldQuality")))
		}
	}
	// Buff magnitude is encoded as a discrete unit by the recovered client.
	// Duration therefore carries resonance for buffs, magnitude for other spells.
	if skill.skillType == 30 || skill.skillType == 31 {
		skill.durationMS = int(scaleForgeStat(int64(skill.durationMS), forgeAffix(role, "buff_duration")+int64(resonance)*100))
	} else {
		power *= 1 + float64(resonance)/100
		if skill.skillType == 10 || skill.skillType == 11 || skill.skillType == 13 || skill.skillType == 15 {
			power *= 1 + float64(forgeAffix(role, "direct_damage"))/10000
		}
	}
	return skill, power, resonance
}
