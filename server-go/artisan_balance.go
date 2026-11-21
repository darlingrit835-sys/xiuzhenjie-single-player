package main

import (
	"fmt"
	"math/bits"
)

var artisanCareerFields = []string{"Live1Type", "Live1Level", "Live1Value", "Live2Type", "Live2Level", "Live2Value", "Live3Type", "Live3Level", "Live3Value", "Live4Type", "Live4Level", "Live4Value"}

var artisanSlots = [...]string{"Live1", "Live2", "Live3", "Live4"}

// The recovered client displays these original XP ceilings. Scale the award,
// so a near-rank project takes ten lessons at rank one and 26 at rank nine.
func artisanLesson(ceiling, rank int) int {
	lessons := 8 + 2*max(1, rank)
	return max(1, (ceiling+lessons-1)/lessons)
}

func artisanTraining(role Record, profession int, rankKey, experienceKey string) (rank, xp int, slot string) {
	if rankKey != "" {
		rank, xp = int(roleInt(role, rankKey)), int(roleInt(role, experienceKey))
	}
	bestRank, bestXP := -1, -1
	for _, prefix := range artisanSlots {
		if int(roleInt(role, prefix+"Type")) != profession {
			continue
		}
		level, points := int(roleInt(role, prefix+"Level")), int(roleInt(role, prefix+"Value"))
		if level > bestRank || level == bestRank && points > bestXP {
			bestRank, bestXP, slot = level, points, prefix
		}
		if level > rank || level == rank && points > xp {
			rank, xp = level, points
		}
	}
	return min(9, max(0, rank)), max(0, xp), slot
}

func artisanAddLesson(role Record, profession int, rankKey, xpKey string, weight int) (int, int) {
	rank, xp, slot := artisanTraining(role, profession, rankKey, xpKey)
	rank = max(1, rank)
	xp += max(1, (artisanLesson(sigilRanks[rank], rank)*weight+50)/100)
	for rank < 9 && xp >= sigilRanks[rank] {
		xp -= sigilRanks[rank]
		rank++
	}
	xp = min(sigilRanks[rank], xp)
	role[rankKey], role[xpKey] = rank, xp
	if slot != "" {
		role[slot+"Level"], role[slot+"Value"] = rank, xp
	}
	return rank, xp
}

// A recipe must first be learned by hand. Each unlocked size is a ceiling,
// so players with just a few ingredients can still use a partial batch.
func artisanCopyLimit(rank int) int {
	switch {
	case rank >= 9:
		return 20
	case rank >= 6:
		return 10
	case rank >= 3:
		return 5
	default:
		return 1
	}
}

func artisanBatchCount(requested, rank int) (int, error) {
	if requested == 0 {
		return 1, nil
	} // existing clients submit a single unit
	if requested < 1 || requested > artisanCopyLimit(rank) {
		return 0, forgeFailure(fmt.Sprintf("当前品阶每批最多 %d 份", artisanCopyLimit(rank)))
	}
	return requested, nil
}

func artisanCopyVigor(count, unit int) int64 {
	return int64((count*unit + 2) / 3)
}

func artisanResearchRank(role Record, profession int) int {
	h := fmt.Sprint(profession)
	rank, _, _ := artisanTraining(role, profession, "", "")
	return max(rank, int(min(9, max(0, roleInt(farmMap(farmMap(role, "ArtisanResearch"), h), "Rank")))))
}

func manuscriptTuition(role Record, hall *voidHall, grade, brief int) int64 {
	if (hall.Type != 4 && hall.Type != 5 && hall.Type != 6 && hall.Type != 7 && hall.Type != 9) || grade >= 9 || grade != artisanResearchRank(role, hall.Type) {
		return 0
	}
	entry := farmMap(farmMap(role, "ArtisanResearch"), fmt.Sprint(hall.Type))
	if int(roleInt(entry, "Grade")) == grade && roleInt(entry, "Briefs")&(1<<brief) != 0 {
		return 0
	}
	return int64(4 * grade * (grade + 2))
}

// Each grade has three distinct briefs. Alternate solutions to the same brief
// never advance the discipline twice; the signed work and lesson share a transaction.
func artisanManuscript(role Record, hall *voidHall, grade, brief int) error {
	fee := manuscriptTuition(role, hall, grade, brief)
	if fee == 0 {
		return nil
	}
	if roleInt(role, "ShinyStone") < fee {
		return forgeFailure(fmt.Sprintf("本题署名修习需要 %d 灵石，免费推演可继续", fee))
	}
	before := artisanResearchRank(role, hall.Type)
	key := fmt.Sprint(hall.Type)
	rank := grade
	entry := farmMap(farmMap(role, "ArtisanResearch"), key)
	mask := 0
	if int(roleInt(entry, "Grade")) == grade {
		mask = int(roleInt(entry, "Briefs"))
	}
	mask |= 1 << brief
	if bits.OnesCount(uint(mask)) == len(hall.Briefs) && rank < 9 {
		rank++
	}
	role["ShinyStone"] = roleInt(role, "ShinyStone") - fee
	putFriendEntry(role, "ArtisanResearch", key, Record{"Rank": rank, "Briefs": mask, "Grade": grade})
	for _, slot := range artisanSlots {
		if int(roleInt(role, slot+"Type")) == hall.Type && int(roleInt(role, slot+"Level")) < rank {
			role[slot+"Level"], role[slot+"Value"] = rank, 0
		}
	}
	if hall.Type == 4 && rank != before {
		recomputeGamePower(role)
	}
	return nil
}
