package main

import (
	"fmt"
	"maps"
	"math"
	"math/rand/v2"
	"slices"
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
}

func (s *Server) loadAcupoints() error {
	var rows []struct {
		ID       string `json:"a"`
		Children string `json:"h"`
		BaseCost int64  `json:"i"`
	}
	const table = "b.json"
	if err := s.readGameTable(table, &rows); err != nil {
		return err
	}
	if rows == nil {
		return nil
	}
	rules := make(map[string]acupointRule, len(rows))
	for _, row := range rows {
		if _, exists := rules[row.ID]; exists || row.ID == "" || row.BaseCost < 1 || row.BaseCost > math.MaxInt32/5 {
			return fmt.Errorf("穴位表 %s 的编号或点穴消耗无效（编号 %q）", table, row.ID)
		}
		rules[row.ID] = acupointRule{baseCost: row.BaseCost}
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
	return nil
}

func acupointPreviewRoutes() []previewRoute {
	return []previewRoute{
		{cmd: 1005016, module: "洞府与修炼", response: "OpenAcupointResp_1005016", summary: "内视读取角色已保存的经脉点与穴位", roleFields: []string{"AcupointValue", "Acupoints"}, build: previewAcupoints},
		{cmd: 1005018, module: "洞府与修炼", request: "BreakAcupointReq_1005018", response: "BreakAcupointResp_1005018", summary: "点通或突破已解锁穴位，按当前层数扣经脉点并保存结果", blocked: true, live: (*Server).breakAcupoint},
	}
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

func previewAcupoints(role Record, _, out protoreflect.Message) {
	set(out, "AcupointValue", min(max(0, roleInt(role, "AcupointValue")), math.MaxInt32))
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

func (s *Server) breakAcupoint(peer *Peer, req, out protoreflect.Message) (func(), error) {
	roleID, pointID := peer.currentRoleID(), getString(req, "AcupointId")
	if peer.acupointReceipts != nil && peer.gameRequestID > 0 {
		for _, receipt := range peer.acupointReceipts.recent {
			if receipt.requestID != peer.gameRequestID || receipt.roleID != roleID {
				continue
			}
			if receipt.pointID != pointID {
				return nil, previewReject(4008)
			}
			receipt.result.write(out)
			return nil, nil
		}
	}
	result, err := s.applyAcupointBreak(roleID, pointID, rand.IntN)
	if err != nil {
		return nil, err
	}
	if peer.gameRequestID > 0 {
		if peer.acupointReceipts == nil {
			peer.acupointReceipts = new(acupointReceipts)
		}
		receipts := peer.acupointReceipts
		receipts.recent[receipts.next] = acupointReceipt{peer.gameRequestID, roleID, pointID, result}
		receipts.next = (receipts.next + 1) % len(receipts.recent)
	}
	result.write(out)
	return nil, nil
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
		points := roleInt(role, "AcupointValue")
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
			chance = min(100, int(90-40*current)+bonus)
		}
		updated := tx.Get(roleID)
		result = acupointResult{code: 4145, level: current, points: points - cost}
		updated["AcupointValue"] = result.points
		event := "cultivation.acupoint_failed"
		if chance == 100 || roll(100) < chance {
			result.code, result.level = 4142, current+1
			setAcupointLevel(updated, pointID, result.level)
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
