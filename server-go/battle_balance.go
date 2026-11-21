package main

import "math"

var battleElementBonusFields = [...]string{"GoldDamageAdd", "WoodDamageAdd", "WaterDamageAdd", "FireDamageAdd", "EarthDamageAdd"}

type resolvedBattleSpell struct {
	skill              battleSkill
	damage, conversion float64
	resonance, affix   int
}

func (skill battleSkill) affectsMovement() bool {
	return skill.skillType == 40 || skill.skillType == 41 || skill.skillType == 50
}

// Both the preparation window and the battle packet use the same final spell.
func resolvePlayerSpell(role Record, attack int64, skill battleSkill) resolvedBattleSpell {
	damage := float64(attack) * skill.multiplier
	if skill.skillType >= 10 && skill.skillType < 20 {
		damage *= 1 + float64(battleMeridianBPS(role, "FiveElementAttackUp"))/10000
	}
	if skill.skillType == 30 || skill.skillType == 31 {
		damage = 1 // the recovered player interprets this as a buff multiplier
	}
	skill, damage, resonance := applyForgeSkill(role, skill, damage)
	r := resolvedBattleSpell{skill: skill, damage: damage, conversion: 1, resonance: resonance}
	if skill.kind == 1 {
		p := practiceFor(role, skill.id)
		power, _ := p.stats()
		r.conversion, r.affix = float64(power)/100, p.affix
		r.damage *= r.conversion
		r.skill.castMS = max(1, int(math.Round(float64(p.castMS(skill.castMS)*(100-min(9, artisanResearchRank(role, 4))))/100)))
	}
	// Unity reads movement magnitude as a fraction. Practice and resonance
	// extend its duration, and the skill type chooses haste, slow or root.
	if skill.affectsMovement() {
		r.damage = skill.multiplier
		durationBPS := int64(resonance) * 100
		if skill.skillType == 50 {
			durationBPS += forgeAffix(role, "buff_duration")
		}
		r.skill.durationMS = int(math.Round(float64(skill.durationMS) * r.conversion * (1 + float64(durationBPS)/10000)))
	}
	return r
}

func (m battleMonster) skillDamage(skill battleSkill, resist [6]int, meridianDefense int64) float64 {
	if m.objectType == 2001 && skill.affectsMovement() {
		return skill.multiplier
	}
	damage := float64(m.attack) * skill.multiplier
	// The recovered client reads the wood mitigation slot for incoming earth.
	// Pre-apply the actual spell element to each hostile damage payload instead.
	if m.objectType != 2001 || skill.skillType < 20 {
		damage *= 1 - float64(resist[skill.element])/100
		damage /= 1 + float64(meridianDefense)/10000
	}
	return max(1, math.Round(damage))
}

// Neutral one-target sustained output from the strongest equipped cast, plus
// a toggled flying artifact. Other casts can trade damage for control or wards.
// Flight includes the client's mandatory 1.5 s turn and 0.5 s travel budget.
func sustainedBattleDamage(role Record, stats battleRoleStats) float64 {
	spell, artifact := 0.0, 0.0
	for _, skill := range battleLoadoutFor(role) {
		switch skill.skillType {
		case 10, 12, 14, 15:
			r := resolvePlayerSpell(role, stats.attack, skill)
			cast := float64(r.skill.castMS) / 1000
			if skill.kind == 2 {
				var bonus int
				for effect := 106; effect <= 110; effect++ {
					bonus += methodAffixPercent(role, effect)
				}
				artifact = max(artifact, r.damage*(1+float64(bonus)/500)/(2+cast))
			} else {
				damage := r.damage
				if skill.skillType == 14 {
					// The area divides its budget across 1.5 s ticks and applies a .75 multiplier.
					ticks := float64(skill.durationMS) / 1500
					damage *= .75 * math.Ceil(ticks) / ticks
				}
				spell = max(spell, damage/(float64(skill.cooldownS)+cast))
			}
		}
	}
	return spell + artifact
}

// Geometric mean of damage per second and effective health against five
// equally represented elements; currency and unspent cultivation have no weight.
func battlePower(role Record) int64 {
	stats := equippedBattleStats(role)
	pool := float64(stats.defense + stats.blood)
	incoming := 1.0
	for effect := 301; effect <= 305; effect++ {
		incoming -= float64(methodAffixPercent(role, effect)) / 500
	}
	incoming /= 1 + float64(battleMeridianBPS(role, "FiveElementDefenseUp"))/10000
	power := 5 * math.Sqrt(sustainedBattleDamage(role, stats)*pool/incoming)
	return int64(min(float64(math.MaxInt32), math.Round(power)))
}
