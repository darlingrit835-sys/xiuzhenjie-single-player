package main

import (
	"errors"
	"math"
	"sort"

	"google.golang.org/protobuf/reflect/protoreflect"
)

var battleEquipmentProtocols = map[int]gameMapProtocol{
	1009001: {"DressEquipmentReq_1009001", "DressEquipmentResp_1009001"},
	1009002: {"UndressEquipmentReq_1009002", "UndressEquipmentResp_1009002"},
	1009003: {"SetSkillReq_1009003", "SetSkillResp_1009003"},
	1009004: {"RemoveSkillReq_1009004", "RemoveSkillResp_1009004"},
	1009005: {"", "EnterEquipmentResp_1009005"},
	1009007: {"", "EnterSkillResp_1009007"},
}

var (
	errBattleEquipmentMissing = errors.New("equipment is not available")
	errBattleBagFull          = errors.New("inventory is full")
)

func (s *Server) handleEquipment(peer *Peer, msg Message) bool {
	names, ok := battleEquipmentProtocols[msg.Cmd]
	if !ok {
		return false
	}
	resp := s.gameResponse(names.resp, true, 2000)
	roleID := peer.currentRoleID()
	if names.req == "" {
		role := s.roles.Get(roleID)
		if role == nil {
			statusPB(resp, false, 4007)
		} else if msg.Cmd == 1009005 {
			for _, item := range battleRecords(role["EquippedGoods"]) {
				entry := addMessage(resp, "DressedEquipments")
				putPB(entry, Record{
					"GoodsId": roleString(item, "GoodsId"), "Name": roleString(item, "Name"),
					"Type": roleInt(item, "Type"), "Icon": max(1, roleInt(item, "Type")),
					"FiveElement": max(1, roleInt(item, "FiveElement")), "Rank": max(1, roleInt(item, "Rank")),
					"Level": max(1, roleInt(item, "Level")),
				})
			}
			fillBattleDefense(resp, role, true)
		} else {
			fillBattleSkillSettings(resp, role)
		}
		s.sendGame(peer, msg, resp)
		return true
	}

	req, err := s.decode(names.req, msg.Data)
	if err != nil {
		return true
	}
	err = s.roles.Update(roleID, func(role Record) error {
		switch msg.Cmd {
		case 1009001, 1009002:
			if err := moveBattleEquipment(role, getString(req, "EquipmentId"), msg.Cmd == 1009001); err != nil {
				return err
			}
			fillBattleDefense(resp, role, false)
		case 1009003, 1009004:
			updateBattleSkillSettings(role, req, msg.Cmd == 1009003)
			putPB(resp, Record{
				"CurrentPerception":  roleInt(role, "CurrentPerception"),
				"FullLoadPerception": min(math.MaxInt32, roleInt(role, "Perception")),
			})
		}
		return nil
	})
	switch {
	case errors.Is(err, errBattleEquipmentMissing):
		statusPB(resp, false, 4603)
	case errors.Is(err, errBattleBagFull):
		statusPB(resp, false, 4606)
	case err != nil:
		statusPB(resp, false, 4007)
	}
	s.sendGame(peer, msg, resp)
	return true
}

func battleEquipmentSlot(kind int64) int64 {
	if kind == 26 {
		return 16
	}
	return kind
}

func moveBattleEquipment(role Record, id string, dress bool) error {
	worn := battleRecords(role["EquippedGoods"])
	retained := make([]any, 0, len(worn)+1)
	if dress {
		item := locateGoods(role, id)
		if item == nil || !isGameEquipment(roleInt(item, "Type")) {
			return errBattleEquipmentMissing
		}
		item = cloneRecord(item)
		removeGoods(role, id)
		slot := battleEquipmentSlot(roleInt(item, "Type"))
		for _, other := range worn {
			if battleEquipmentSlot(roleInt(other, "Type")) != slot {
				retained = append(retained, other)
			} else if !returnBattleEquipment(role, other) {
				return errBattleBagFull
			}
		}
		item["Count"], item["IsUsing"], item["IsBinding"] = 1, true, true
		retained = append(retained, item)
	} else {
		moved := false
		for _, item := range worn {
			if roleString(item, "GoodsId") != id || moved {
				retained = append(retained, item)
				continue
			}
			if !returnBattleEquipment(role, item) {
				return errBattleBagFull
			}
			moved = true
		}
		if !moved {
			return errBattleEquipmentMissing
		}
	}
	role["EquippedGoods"] = retained
	return nil
}

func returnBattleEquipment(role, item Record) bool {
	count := max(1, roleInt(item, "Count"))
	if existing := locateGoods(role, roleString(item, "GoodsId")); existing != nil {
		existing["Count"] = max(1, roleInt(existing, "Count")) + count
		return true
	}
	goods := goodsList(role)
	if len(goods) >= 100 {
		return false
	}
	returned := cloneRecord(item)
	returned["Count"], returned["IsUsing"] = count, false
	role["Goods"] = append(goods, returned)
	return true
}

func fillBattleDefense(resp protoreflect.Message, role Record, listView bool) {
	attackBonus, defenseBonus := battleEquipmentBonuses(role)
	perception := max(100, roleInt(role, "Perception"))
	physique := max(100, roleInt(role, "Physique")+defenseBonus)
	putPB(resp, Record{
		"CurrentPhysiqueDefense": physique, "CurrentPureBloodDefense": min(math.MaxInt32, max(1000, perception*4)),
		"CurrentMoveSpeed": 5.0, "SuperAirRecoverValue": float64(perception) * .05, "DefenseRecoverValue": float64(physique) * .05,
		"SuperAirRecoverValueOnBattle": float64(perception) * .02, "DefenseRecoverValueOnBattle": float64(physique) * .02,
	})
	if listView {
		putPB(resp, Record{
			"FullLoadPerception": min(math.MaxInt32, perception), "CanUsePhysique": min(math.MaxInt32, physique),
			"SuperAirPower": max(10, int64(math.Round(float64(perception)*.8))+attackBonus),
		})
	} else {
		set(resp, "CanUsedPhysique", physique)
	}
}

func updateBattleSkillSettings(role Record, req protoreflect.Message, assign bool) {
	id := getString(req, "SkillId")
	slot := int64(-1)
	if assign {
		slot = getInt(req, "CombatSettingId")
	}
	settings := battleRecords(role["SkillSettings"])
	retained := make([]any, 0, len(settings)+1)
	for _, setting := range settings {
		if roleString(setting, "SkillId") == id || assign && roleInt(setting, "CombatSettingId") == slot {
			continue
		}
		retained = append(retained, setting)
	}
	if assign {
		retained = append(retained, Record{"SkillId": id, "SkillType": getInt(req, "SkillType"), "CombatSettingId": slot})
		sort.SliceStable(retained, func(i, j int) bool {
			return roleInt(retained[i].(Record), "CombatSettingId") < roleInt(retained[j].(Record), "CombatSettingId")
		})
	}
	role["SkillSettings"] = retained
}

func fillBattleSkillSettings(resp protoreflect.Message, role Record) {
	level := max(1, roleInt(role, "Level"))
	configured := make(map[string]bool)
	for _, setting := range battleRecords(role["SkillSettings"]) {
		id := roleString(setting, "SkillId")
		for _, skill := range battleSkills {
			if skill.id == id {
				configured[id] = true
				fillBattleSkillSetting(addMessage(resp, "SetableSkills"), skill, level, roleInt(setting, "CombatSettingId"))
				break
			}
		}
	}
	for _, skill := range battleSkills {
		if !configured[skill.id] {
			fillBattleSkillSetting(addMessage(resp, "CanuseSkills"), skill, level, 0)
		}
	}
}

func fillBattleSkillSetting(resp protoreflect.Message, skill battleSkill, level, slot int64) {
	putPB(resp, Record{
		"GoodsId": skill.id, "Name": skill.name, "FiveElement": skill.element, "Rank": 1,
		"Level": level, "Type": skill.kind, "SettingType": skill.kind, "CombatSettingId": slot,
	})
}
