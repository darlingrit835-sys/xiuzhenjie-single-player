package main

import (
	"math"
	"time"
)

const vigorStepSeconds int64 = 120
const cultivationYearSeconds int64 = 7200
const lifespanBreakCode = 4984

func vigorCapacity(level int64) int64 {
	if level <= 1 {
		return 100
	}
	return 100 + 5*min(90, level)
}

// Keep the unfinished recovery interval across requests, including reconnects.
func recoverRoleVigor(r Record, now int64) int64 {
	level := roleInt(r, "Level")
	capacity := max(vigorCapacity(level), roleInt(r, "Vigor"), roleInt(r, "CurrentVigor"))
	setRoleDefault(r, "CurrentVigor", capacity)
	r["Vigor"] = capacity
	v := max(0, min(capacity, roleInt(r, "CurrentVigor")))
	last := roleInt(r, "VigorRecoveredAt")
	gain := int64(0)
	if last <= 0 || v == capacity {
		r["VigorRecoveredAt"] = now
	} else if now > last {
		gain = min(capacity-v, (now-last)/vigorStepSeconds)
		if gain > 0 {
			last += gain * vigorStepSeconds
			if v+gain == capacity {
				last = now
			}
			r["VigorRecoveredAt"] = last
		}
	}
	r["CurrentVigor"] = v + gain
	return gain
}

func spendArtisanVigor(r Record, cost int64) bool { return spendVigorAt(r, cost, time.Now().Unix()) }

func spendVigorAt(r Record, cost, now int64) bool {
	recoverRoleVigor(r, now)
	v := roleInt(r, "CurrentVigor")
	if cost < 0 || v < cost {
		return false
	}
	r["CurrentVigor"] = v - cost
	return true
}

func breakthroughCost(level int64) (years, vigor int64) {
	if level <= 1 || level >= 90 {
		return 0, 0
	}
	return gameLifespan(level) / 25, 2 + (level-1)/10
}

// Reserve the years required to finish this realm. At the reserve, expeditions,
// pills and teaching can still advance the role, and a longevity pill reopens practice.
func remainingTrainingLifetime(r Record) int64 {
	level := roleInt(r, "Level")
	if level < 1 || level > 90 {
		return 0
	}
	transitions := min(10-(level-1)%10, 90-level)
	if level == 1 {
		transitions--
	} // The tutorial breakthrough has no cost.
	reserve := transitions*(gameLifespan(level)/25) + 1
	free := max(0, max(gameLifespan(level), roleInt(r, "MaxAge"))-max(16, roleInt(r, "CurrentAge"))-reserve)
	return max(0, free*cultivationYearSeconds-roleInt(r, "CultivationYearSeconds"))
}

func ageCultivation(r Record, seconds int64) bool {
	if seconds <= 0 {
		return false
	}
	total := seconds + roleInt(r, "CultivationYearSeconds")
	r["CultivationYearSeconds"] = total % cultivationYearSeconds
	years := total / cultivationYearSeconds
	if years == 0 {
		return false
	}
	r["CurrentAge"] = max(16, roleInt(r, "CurrentAge")) + years
	return true
}

// Charge only the time for which the current bar can absorb passive cultivation.
func productiveTrainingSeconds(r Record, key string, limit, rate float64, available int64) int64 {
	if rate <= 0 || available <= 0 || key == "SuperAir" && roleInt(r, "Level") >= 90 {
		return 0
	}
	space := int64(limit) - roleInt(r, key)
	if space <= 0 {
		return 0
	}
	return min(available, remainingTrainingLifetime(r), int64(math.Ceil(float64(space)/rate)))
}

func breakthroughResourceCode(r Record) int {
	years, vigor := breakthroughCost(roleInt(r, "Level"))
	if max(gameLifespan(roleInt(r, "Level")), roleInt(r, "MaxAge"))-max(16, roleInt(r, "CurrentAge")) <= years {
		return lifespanBreakCode
	}
	if roleInt(r, "CurrentVigor") < vigor {
		return 4290
	}
	return 0
}
