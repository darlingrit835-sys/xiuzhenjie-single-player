package main

import (
	"math"
	"time"
)

const (
	guildCharterRead          = 4901032
	guildCharterAct           = 4901033
	sectPermitPrice     int64 = 600
	sectPermitFragments int64 = 3
)

var sectPermitFields = []string{"SectPermitsIssued", "SectPermitGranted", "SectFragmentsSpent", "SectLastIssue", "QuestTotalSweeps", "QuestSweeps", "ShinyStone"}

func sectFragments(role Record) (sweeps, available int64) {
	sweeps = max(0, roleInt(role, "QuestTotalSweeps"), roleInt(role, "QuestSweeps"))
	return sweeps, max(0, sweeps-roleInt(role, "SectFragmentsSpent"))
}

func sectPermitSpent(world Record, id string) int64 {
	used, _ := world["PermitsUsed"].(Record)
	return roleInt(used, id)
}

func sectPermitView(role Record, used int64) Record {
	sweeps, fragments := sectFragments(role)
	issued := roleInt(role, "SectPermitsIssued")
	return Record{"permits": max(0, issued-used), "fragments": fragments, "sweeps": sweeps, "issued": issued, "revision": issued,
		"canGrant": !roleBool(role, "SectPermitGranted"), "canExchange": fragments >= sectPermitFragments, "canBuy": roleInt(role, "ShinyStone") >= sectPermitPrice,
		"stones": roleInt(role, "ShinyStone"), "price": sectPermitPrice, "fragmentsCost": sectPermitFragments}
}

// Role-side issuance is monotonic, and each charge commits in the same save as
// the permit it buys. The guild-side spent count commits with the actual guild.
func (s *Server) issueSectPermit(id string, req sectOrder) (Record, error) {
	var result Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		base := tx.View(id)
		if base == nil {
			return sectRefusal("角色记录不存在")
		}
		issued := roleInt(base, "SectPermitsIssued")
		if req.Revision >= 0 && req.Revision < issued && issued-req.Revision == 1 && roleString(base, "SectLastIssue") == req.Action {
			result = base
			return nil
		}
		if req.Revision != issued || issued < 0 || issued == math.MaxInt64 {
			return sectRefusal("筹建令簿已更新，请刷新")
		}
		role := tx.Get(id)
		switch req.Action {
		case "grant":
			if roleBool(role, "SectPermitGranted") {
				return sectRefusal("本角色已领过城主府首枚开山令")
			}
			role["SectPermitGranted"] = true
		case "exchange":
			if _, available := sectFragments(role); available < sectPermitFragments {
				return sectRefusal("堪舆残图不足，清场历练可得残图")
			}
			role["SectFragmentsSpent"] = roleInt(role, "SectFragmentsSpent") + sectPermitFragments
		case "buy":
			stone := roleInt(role, "ShinyStone")
			if stone < sectPermitPrice {
				return sectRefusal("灵石不足，无法请印")
			}
			role["ShinyStone"] = stone - sectPermitPrice
		default:
			return sectRefusal("未知的开山令取得方式")
		}
		role["SectPermitsIssued"], role["SectLastIssue"] = issued+1, req.Action
		result = role
		return nil
	})
	return result, err
}

func (s *Server) sectCharter(id string, req *sectOrder) (Record, error) {
	s.guilds.mu.RLock()
	defer s.guilds.mu.RUnlock()
	used := sectPermitSpent(s.guilds.records[trialGuildWorld], id)
	if req != nil {
		role, err := s.issueSectPermit(id, *req)
		if err != nil {
			return nil, err
		}
		return sectPermitView(role, used), nil
	}
	role := s.roles.project(id, sectPermitFields)
	if role == nil {
		return nil, sectRefusal("角色记录不存在")
	}
	return sectPermitView(role, used), nil
}

func sectSalary(stage int64) int64 { return 30 + 10*stage }

func sectSalaryDay(at time.Time) string { return at.In(sectZone).Format("20060102") }

func (s *Server) claimSectSalary(id string, req sectOrder, snapshot Record) error {
	day := roleString(snapshot, "stipendDay")
	if req.Value != day {
		return sectRefusal("俸禄日期已更新，请刷新")
	}
	s.guilds.mu.RLock()
	defer s.guilds.mu.RUnlock()
	g := trialGuildForRole(trialGuilds(s.guilds.records[trialGuildWorld]), id)
	if g.row == nil || req.GuildID != g.id || roleInt(snapshot, "guildId") != g.id || roleInt(snapshot, "stage") != sectStage(g) {
		return sectRefusal("宗门归属已变更，请刷新")
	}
	return s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(id)
		if role == nil {
			return sectRefusal("角色记录不存在")
		}
		if roleString(role, "SectStipendDay") >= day {
			snapshot["walletStones"], snapshot["stipendClaimed"] = roleInt(role, "ShinyStone"), true
			return nil
		}
		balance := roleInt(role, "ShinyStone")
		salary := sectSalary(sectStage(g))
		if balance < 0 || balance > math.MaxInt64-salary {
			return sectRefusal("当前钱包无法收入俸禄")
		}
		role = tx.Get(id)
		role["ShinyStone"], role["SectStipendDay"] = balance+salary, day
		snapshot["walletStones"], snapshot["stipendClaimed"] = balance+salary, true
		return nil
	})
}
