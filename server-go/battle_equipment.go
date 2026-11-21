package main

import (
	"errors"
	"math"
	"strings"

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
		// Earlier saves could wear consumables or multiple pieces now sharing one slot.
		// Rehome them before listing gear so the player can still reach every item.
		if msg.Cmd == 1009005 && equipmentNeedsRepair(role) {
			if err := s.roles.Update(roleID, restoreLegacyEquipment); err != nil {
				statusPB(resp, false, 5000)
				s.sendGame(peer, msg, resp)
				return true
			}
			role = s.roles.Get(roleID)
		}
		if role == nil {
			statusPB(resp, false, 4007)
		} else if msg.Cmd == 1009005 {
			for _, item := range battleRecords(role["EquippedGoods"]) {
				entry := addMessage(resp, "DressedEquipments")
				copyRolePB(entry, item, "BlankId", "TemplateId")
				putPB(entry, Record{
					"GoodsId": roleString(item, "GoodsId"), "Name": roleString(item, "Name"),
					"IsLock": inventoryFlag(item, "IsLock"),
					"Type":   roleInt(item, "Type"), "Icon": max(1, roleInt(item, "Type")),
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
			if err := updateBattleSkillSettings(role, req, msg.Cmd == 1009003); err != nil {
				return err
			}
			putPB(resp, Record{
				"CurrentPerception":  roleInt(role, "CurrentPerception"),
				"FullLoadPerception": min(math.MaxInt32, roleInt(role, "Perception")),
			})
		}
		return nil
	})
	var rejected previewReject
	switch {
	case errors.Is(err, errBattleEquipmentMissing):
		statusPB(resp, false, 4520)
	case errors.Is(err, errBattleBagFull):
		statusPB(resp, false, 4513)
	case errors.As(err, &rejected):
		statusPB(resp, false, int(rejected))
	case err != nil:
		statusPB(resp, false, 4007)
	}
	s.sendGame(peer, msg, resp)
	return true
}

type equipmentSlot int

const (
	noEquipmentSlot equipmentSlot = iota
	mainHandSlot
	bodySlot
	wardSlot
	feetSlot
	equipmentSlotCount
)

func battleEquipmentSlot(item Record) equipmentSlot {
	switch roleInt(item, "Type") {
	case 15:
		return mainHandSlot
	case 16:
		id := roleString(item, "TemplateId")
		// All historical type-16 dungeon drops were robes or cuirasses.
		if id == "eq_armor_qingbu" || strings.HasPrefix(id, "drop_16_") {
			return bodySlot
		}
		return wardSlot
	case 26:
		return bodySlot
	case 29:
		return feetSlot
	}
	return noEquipmentSlot
}

func wornEquipment(role Record) (slots [equipmentSlotCount]Record, surplus []Record) {
	for _, item := range battleRecords(role["EquippedGoods"]) {
		slot := battleEquipmentSlot(item)
		if slot == noEquipmentSlot || slots[slot] != nil {
			surplus = append(surplus, item)
		} else {
			slots[slot] = item
		}
	}
	return slots, surplus
}

func equipmentNeedsRepair(role Record) bool {
	slots, surplus := wornEquipment(role)
	if len(surplus) != 0 {
		return true
	}
	for _, item := range slots {
		if item != nil {
			id := roleString(item, "GoodsId")
			if id == "" || locateGoods(role, id) != nil {
				return true
			}
		}
	}
	return false
}

func restoreLegacyEquipment(role Record) error {
	slots, surplus := wornEquipment(role)
	for _, item := range surplus {
		// Permit existing possessions to return even when the old bag was full.
		// Ordinary pickup and unequip still enforce capacity.
		storeReturnedEquipment(role, item)
	}
	kept := make([]any, 0, len(slots))
	for _, item := range slots {
		if item != nil {
			// Older stacked equipment sometimes shared its ID with the bag remainder.
			if id := roleString(item, "GoodsId"); id == "" || locateGoods(role, id) != nil {
				item["GoodsId"] = "g_" + newGameID(8)
			}
			kept = append(kept, item)
		}
	}
	role["EquippedGoods"] = kept
	recomputeGamePower(role)
	return nil
}

func moveBattleEquipment(role Record, id string, dress bool) error {
	worn := battleRecords(role["EquippedGoods"])
	retained := make([]any, 0, len(worn)+1)
	if dress {
		item := locateGoods(role, id)
		if item == nil || id == "" {
			return errBattleEquipmentMissing
		}
		slot := battleEquipmentSlot(item)
		if slot == noEquipmentSlot {
			return previewReject(4510)
		}
		if hasFriendOfferLock(item) {
			return previewReject(4509)
		}
		if (max(1, roleInt(role, "Level"))-1)/10 < (max(1, roleInt(item, "Level"))-1)/10 {
			return previewReject(4105)
		}
		displaced := make([]Record, 0, 1)
		for _, other := range worn {
			if battleEquipmentSlot(other) == slot {
				displaced = append(displaced, other)
			} else {
				retained = append(retained, other)
			}
		}
		used := len(goodsList(role))
		after := used + len(displaced)
		if roleInt(item, "Count") <= 1 {
			after--
		}
		if after > max(ringCapacity(role), used) {
			return errBattleBagFull
		}
		item = cloneRecord(item)
		removeGoods(role, id)
		if rest := locateGoods(role, id); rest != nil {
			// Legacy stacks need distinct IDs for the worn copy and the remainder.
			rest["GoodsId"] = "g_" + newGameID(8)
		}
		for _, other := range displaced {
			storeReturnedEquipment(role, other)
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
			if len(goodsList(role)) >= ringCapacity(role) {
				return errBattleBagFull
			}
			storeReturnedEquipment(role, item)
			moved = true
		}
		if !moved {
			return errBattleEquipmentMissing
		}
	}
	role["EquippedGoods"] = retained
	recomputeGamePower(role)
	return nil
}

func storeReturnedEquipment(role, item Record) {
	returned := cloneRecord(item)
	if id := roleString(item, "GoodsId"); id == "" || locateGoods(role, id) != nil {
		returned["GoodsId"] = "g_" + newGameID(8)
	}
	returned["Count"], returned["IsUsing"] = max(1, roleInt(item, "Count")), false
	role["Goods"] = append(goodsList(role), returned)
}

func battleEquipmentBonuses(role Record) (attack, defense int64) {
	perception, physique := battleTrainedStats(role)
	baseAttack := int64(math.Round(float64(perception) * .8))
	baseDefense := physique
	// A piece retains its stated flat value. Its rank also earns a bounded
	// share of the wearer's trained stat, so high-realm drops remain meaningful.
	contribution := func(base, flat, bps int64) int64 {
		return max(flat, int64(math.Round(float64(base)*float64(bps)/10000)))
	}
	worn, _ := wornEquipment(role)
	for slot, item := range worn {
		if item == nil {
			continue
		}
		if isSmithingEquipment(item) && (roleInt(item, "BaseAttack") > 0 || roleInt(item, "BaseDefense") > 0) {
			a, d := roleInt(item, "BaseAttack"), roleInt(item, "BaseDefense")
			attack += contribution(baseAttack, a, min(1800, a*22))
			defense += contribution(baseDefense, d, min(1800, d*22))
			continue
		}
		rank, level := min(16, max(1, roleInt(item, "Rank"))), max(1, roleInt(item, "Level"))
		switch equipmentSlot(slot) {
		case mainHandSlot:
			attack += contribution(baseAttack, rank*10+level*2, 350+rank*50)
		case bodySlot, feetSlot:
			defense += contribution(baseDefense, rank*8+level*2, 300+rank*50)
		case wardSlot:
			defense += contribution(baseDefense, rank*4+level, 150+rank*25)
		}
	}
	if worn[bodySlot] != nil && worn[feetSlot] != nil {
		defense += max(1, int64(math.Round(float64(defense)*.15)))
		if worn[mainHandSlot] != nil {
			attack += max(1, attack/10)
		}
	}
	return scaleForgeStat(baseAttack+attack, forgeAffix(role, "attack")) - baseAttack,
		scaleForgeStat(baseDefense+defense, forgeAffix(role, "defense")) - baseDefense
}

type battleRoleStats struct {
	perception, attack, defense, blood int64
	speed                              float64
}

func battleTrainedStats(role Record) (perception, physique int64) {
	return scaleForgeStat(max(100, roleInt(role, "Perception")), battleMeridianBPS(role, "PerceptionValueUp")),
		scaleForgeStat(max(100, roleInt(role, "Physique")), battleMeridianBPS(role, "PhysiqueValueUp"))
}

func equippedBattleStats(role Record) battleRoleStats {
	attackBonus, defenseBonus := battleEquipmentBonuses(role)
	perception, physique := battleTrainedStats(role)
	stats := battleRoleStats{
		perception: perception, attack: max(10, int64(math.Round(float64(perception)*.8))+attackBonus),
		defense: physique + defenseBonus,
		blood:   max(1000, perception+3*physique),
		speed:   battleMovementSpeed(role, methodAffixPercent(role, methodMoveSpeedEffect), 0),
	}
	worn, _ := wornEquipment(role)
	if ward := worn[wardSlot]; ward != nil {
		rank := min(16, max(1, roleInt(ward, "Rank")))
		flat := 40*rank + 10*max(1, roleInt(ward, "Level"))
		stats.blood += max(flat, int64(math.Round(float64(stats.blood)*float64(200+rank*25)/10000)))
	}
	stats.blood = min(math.MaxInt32, scaleForgeStat(stats.blood, forgeAffix(role, "max_hp")+battleMeridianBPS(role, "PureBloodValueUp")))
	return stats
}

func battleMovementSpeed(role Record, methodPercent int, talismanPercent int64) float64 {
	base := 5.0
	worn, _ := wornEquipment(role)
	if shoes := worn[feetSlot]; shoes != nil {
		base += .5 + .1*float64(min(16, max(1, roleInt(shoes, "Rank"))))
	}
	return base * (1 + float64(int64(methodPercent)+talismanPercent)/100)
}

func fillBattleDefense(resp protoreflect.Message, role Record, listView bool) {
	stats := equippedBattleStats(role)
	perception, physique := stats.perception, stats.defense
	putPB(resp, Record{
		"CurrentPhysiqueDefense": physique, "CurrentPureBloodDefense": stats.blood,
		"CurrentMoveSpeed": stats.speed, "SuperAirRecoverValue": float64(perception) * .05, "DefenseRecoverValue": float64(physique) * .05,
		"SuperAirRecoverValueOnBattle": float64(perception) * .02, "DefenseRecoverValueOnBattle": float64(physique) * .02,
	})
	if listView {
		if haste := preparedDungeonHaste(role); haste > 0 {
			resp.SetUnknown(offerNumber(resp.GetUnknown(), 14, uint64(haste)))
		}
		if method := methodAffixPercent(role, methodMoveSpeedEffect); method > 0 {
			resp.SetUnknown(offerNumber(resp.GetUnknown(), 15, uint64(method)))
		}
		putPB(resp, Record{
			"FullLoadPerception": min(math.MaxInt32, perception), "CanUsePhysique": min(math.MaxInt32, physique),
			"SuperAirPower": scaleForgeStat(stats.attack, battleMeridianBPS(role, "FiveElementAttackUp")),
		})
	} else {
		set(resp, "CanUsedPhysique", physique)
	}
}

func updateBattleSkillSettings(role Record, req protoreflect.Message, assign bool) error {
	id := getString(req, "SkillId")
	slots := battleLoadoutFor(role)
	skill, ok := battleSkillByID(id)
	if !ok {
		return previewReject(4516)
	}
	var slot int64
	if assign {
		slot = getInt(req, "CombatSettingId")
		if slot < 0 || slot >= int64(len(slots)) {
			return previewReject(5001)
		}
	}
	for i, equipped := range slots {
		if equipped.id == id {
			slots[i] = battleSkill{}
		}
	}
	if assign {
		slots[slot] = skill
	}
	settings := make([]any, 0, len(slots))
	for index, equipped := range slots {
		if equipped.id != "" {
			settings = append(settings, Record{"SkillId": equipped.id, "SkillType": equipped.kind, "CombatSettingId": index})
		}
	}
	role["SkillSettings"] = settings
	recomputeGamePower(role)
	return nil
}

func fillBattleSkillSettings(resp protoreflect.Message, role Record) {
	level := max(1, roleInt(role, "Level"))
	configured := make(map[string]bool)
	for index, skill := range battleLoadoutFor(role) {
		if skill.id != "" {
			configured[skill.id] = true
			fillBattleSkillSetting(addMessage(resp, "SetableSkills"), skill, level, int64(index))
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
		"Level": level, "Type": skill.skillType, "SettingType": skill.kind, "CombatSettingId": slot,
	})
}
