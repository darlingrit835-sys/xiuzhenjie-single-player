package main

import (
	"errors"
	"fmt"
	"maps"
	"math"
	"math/rand/v2"
	"slices"
	"strconv"
	"strings"

	"google.golang.org/protobuf/reflect/protoreflect"
)

const (
	firstAcupoint    = "1"
	maxAcupointLevel = 3
)

type acupointRule struct {
	baseCost int64
	parents  []string
	children []string
	threads  []int
}

func (s *Server) loadAcupoints() error {
	var rows []struct {
		ID        string `json:"a"`
		Children  string `json:"h"`
		Meridians string `json:"e"`
		BaseCost  int64  `json:"i"`
	}
	const table = "b.json"
	if err := s.readGameTable(table, &rows); err != nil {
		return err
	}
	if rows == nil {
		return nil
	}
	rules := make(map[string]acupointRule, len(rows))
	var totals [8]int
	for _, row := range rows {
		if _, exists := rules[row.ID]; exists || row.ID == "" || row.BaseCost < 1 || row.BaseCost > math.MaxInt32/5 {
			return fmt.Errorf("穴位表 %s 的编号或点穴消耗无效（编号 %q）", table, row.ID)
		}
		rule := acupointRule{baseCost: row.BaseCost}
		for name := range strings.SplitSeq(row.Meridians, ",") {
			if idx := slices.Index(meridianNames[:], name); idx >= 0 {
				rule.threads = append(rule.threads, idx)
				totals[idx]++
			}
		}
		if row.Children != "" {
			rule.children = strings.Split(row.Children, ",")
		}
		rules[row.ID] = rule
	}
	if _, ok := rules[firstAcupoint]; !ok {
		return fmt.Errorf("穴位表 %s 缺少起始穴位 %s", table, firstAcupoint)
	}
	for _, row := range rows {
		for id := range strings.SplitSeq(row.Children, ",") {
			if id == "" {
				continue
			}
			rule, ok := rules[id]
			if !ok || id == row.ID {
				return fmt.Errorf("穴位表 %s 中穴位 %q 的后继 %q 无效", table, row.ID, id)
			}
			rule.parents = append(rule.parents, row.ID)
			rules[id] = rule
		}
	}
	s.game.acupoints = rules
	s.game.meridianTotals = totals
	return nil
}

var meridianNames = [...]string{"任脉", "督脉", "冲脉", "带脉", "阴跷脉", "阳跷脉", "阴维脉", "阳维脉"}
var meridianPBFields = [...]string{"AirSpeedUp", "AirValueUp", "PureBloodValueUp", "PureBloodRecoveryUp", "PerceptionValueUp", "FiveElementAttackUp", "PhysiqueValueUp", "FiveElementDefenseUp"}

func acupointBalance(role Record) int64 {
	if role["AcupointValue"] == nil && roleInt(role, "Level") >= 2 && len(acupointLevels(role)) == 0 {
		return 1
	}
	return roleInt(role, "AcupointValue")
}

type meridianProgress struct {
	count, floor [8]int
	bonus        [8]float64
}

func (s *Server) meridianProgress(role Record) meridianProgress {
	var progress meridianProgress
	if entries, _ := role["Acupoints"].([]any); len(entries) == 0 {
		return progress
	}
	for id, saved := range acupointLevels(role) {
		level := int(saved)
		for _, index := range s.game.acupoints[id].threads {
			progress.count[index]++
			if progress.floor[index] == 0 || level < progress.floor[index] {
				progress.floor[index] = level
			}
			points := [...]float64{0, .5, .8, 1}[level]
			switch index {
			case 3:
				points = [...]float64{0, 200, 500, 2000}[level]
			case 5, 7:
				points = float64(level)
			}
			progress.bonus[index] += points
		}
	}
	for idx, total := range s.game.meridianTotals {
		if total != progress.count[idx] {
			progress.floor[idx] = 0
		}
	}
	return progress
}

func (s *Server) fillMeridianOverview(role Record, response protoreflect.Message) {
	progress := s.meridianProgress(role)
	for idx, field := range meridianPBFields {
		prefix := "ThreadType" + strconv.Itoa(idx+1)
		set(response, prefix+"Count", progress.count[idx])
		set(response, prefix+"Level", progress.floor[idx])
		value := progress.bonus[idx]
		if idx == 0 {
			value = -value
		}
		set(response, field, 100+value)
	}
}

// Battle loadouts and artisan wards use the same percentages as the overview.
// Refresh at login and after a successful acupoint change so old saves and
// custom acupoint tables acquire their own current values.
func (s *Server) refreshBattleMeridians(role Record) {
	if len(acupointLevels(role)) == 0 {
		delete(role, "BattleMeridianBonuses")
		return
	}
	progress := s.meridianProgress(role)
	bonuses := Record{}
	for i := 2; i < len(progress.bonus); i++ {
		if progress.bonus[i] > 0 {
			bonuses[meridianPBFields[i]] = int64(math.Round(progress.bonus[i] * 100))
		}
	}
	role["BattleMeridianBonuses"] = bonuses
}

func battleMeridianBPS(role Record, field string) int64 {
	bonuses, _ := role["BattleMeridianBonuses"].(Record)
	return roleInt(bonuses, field)
}

func acupointLevels(role Record) map[string]int64 {
	levels := make(map[string]int64)
	rows, _ := role["Acupoints"].([]any)
	for _, value := range rows {
		row, _ := value.(Record)
		id, level := roleString(row, "Id"), roleInt(row, "BrokenLevel")
		if id != "" && level > 0 {
			levels[id] = max(levels[id], min(level, maxAcupointLevel))
		}
	}
	return levels
}

func fillAcupoints(role Record, out protoreflect.Message) {
	set(out, "AcupointValue", min(max(0, acupointBalance(role)), math.MaxInt32))
	levels := acupointLevels(role)
	for _, id := range slices.Sorted(maps.Keys(levels)) {
		entry := addMessage(out, "Acupoints")
		set(entry, "Id", id)
		set(entry, "BrokenLevel", levels[id])
	}
}

type acupointResult struct {
	code   int
	level  int64
	points int64
}

func (result acupointResult) write(out protoreflect.Message) {
	statusPB(out, true, result.code)
	set(out, "BreakSuccess", result.code == 4142)
	set(out, "BrokenLevel", result.level)
	set(out, "AcupointValue", result.points)
}

type acupointReceipt struct {
	requestID int
	roleID    string
	pointID   string
	result    acupointResult
}

type acupointReceipts struct {
	recent [16]acupointReceipt
	next   int
}

func (s *Server) breakAcupoint(peer *Peer, msg Message, req, out protoreflect.Message) {
	err := s.commitAcupoint(peer, msg.Msid, req, out)
	if err != nil {
		var code previewReject
		if !errors.As(err, &code) {
			code = 5000
		}
		statusPB(out, false, int(code))
	}
}

func (s *Server) commitAcupoint(peer *Peer, requestID int, req, out protoreflect.Message) error {
	if peer.currentRoleID() == "" || (s.preview != nil && !s.previewWrites) {
		return previewReject(previewDeniedCode)
	}
	roleID, pointID := peer.currentRoleID(), getString(req, "AcupointId")
	if peer.acupointReceipts != nil && requestID > 0 {
		for _, receipt := range peer.acupointReceipts.recent {
			if receipt.requestID != requestID || receipt.roleID != roleID {
				continue
			}
			if receipt.pointID != pointID {
				return previewReject(4008)
			}
			receipt.result.write(out)
			return nil
		}
	}
	result, err := s.applyAcupointBreak(roleID, pointID, rand.IntN)
	if err != nil {
		return err
	}
	if requestID > 0 {
		if peer.acupointReceipts == nil {
			peer.acupointReceipts = new(acupointReceipts)
		}
		receipts := peer.acupointReceipts
		receipts.recent[receipts.next] = acupointReceipt{requestID, roleID, pointID, result}
		receipts.next = (receipts.next + 1) % len(receipts.recent)
	}
	result.write(out)
	return nil
}

func (s *Server) applyAcupointBreak(roleID, pointID string, roll func(int) int) (acupointResult, error) {
	if len(s.game.acupoints) == 0 || reservedStoreID(roleID) {
		return acupointResult{}, previewReject(4008)
	}
	rule, ok := s.game.acupoints[pointID]
	if !ok {
		return acupointResult{}, previewReject(4146)
	}
	var result acupointResult
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(roleID)
		if role == nil {
			return previewReject(4007)
		}
		levels := acupointLevels(role)
		current := levels[pointID]
		if current >= maxAcupointLevel {
			return previewReject(4141)
		}
		if roleBool(role, "IsBrokenAcupoint") {
			return previewReject(4143)
		}
		if current == 0 && pointID != firstAcupoint && !slices.ContainsFunc(rule.parents, func(id string) bool { return levels[id] > 0 }) {
			return previewReject(4146)
		}
		cost := rule.baseCost * (2*current + 1)
		points := acupointBalance(role)
		if points < cost {
			return previewReject(4144)
		}
		if points > math.MaxInt32 {
			return previewReject(4008)
		}
		chance := 100
		if current > 0 {
			var bonus int
			for _, id := range friendTitleIDs(role) {
				bonus = max(bonus, s.game.acupointTitleBonus[id])
			}
			links := 0
			for _, id := range rule.parents {
				if levels[id] > 0 {
					links++
				}
			}
			for _, id := range rule.children {
				if levels[id] > 0 && !slices.Contains(rule.parents, id) {
					links++
				}
			}
			chance = min(100, int(90-40*current)+bonus+min(2, links)*10)
		}
		updated := tx.Get(roleID)
		result = acupointResult{code: 4145, level: current, points: points - cost}
		updated["AcupointValue"] = result.points
		event := "cultivation.acupoint_failed"
		if chance == 100 || roll(100) < chance {
			result.code, result.level = 4142, current+1
			setAcupointLevel(updated, pointID, result.level)
			s.refreshBattleMeridians(updated)
			recomputeGamePower(updated)
			event = "cultivation.acupoint_broken"
		}
		history := newHistoryEvent(event, role, nil, "acupoint", pointID,
			Record{"level": result.level, "previousLevel": current, "pointsSpent": cost, "pointsRemaining": result.points})
		history.Actor.RoleID = roleID
		return tx.RecordHistory(history)
	})
	return result, err
}

func setAcupointLevel(role Record, pointID string, level int64) {
	rows, _ := role["Acupoints"].([]any)
	for _, value := range rows {
		if row, ok := value.(Record); ok && roleString(row, "Id") == pointID {
			row["BrokenLevel"] = level
			return
		}
	}
	role["Acupoints"] = append(rows, Record{"Id": pointID, "BrokenLevel": level})
}
