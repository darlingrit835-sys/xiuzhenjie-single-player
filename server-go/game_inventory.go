package main

import (
	"bytes"
	"crypto/md5" // Legacy client inventory-change checksum.
	"encoding/hex"
	"math"
	"sort"
	"strconv"
	"strings"

	"google.golang.org/protobuf/encoding/protowire"
	"google.golang.org/protobuf/reflect/protoreflect"
)

func goodsList(role Record) []any {
	return goodsListForField(role, "Goods")
}

func goodsListForField(role Record, key string) []any {
	list, _ := role[key].([]any)
	return list
}

func locateGoods(role Record, id string) Record {
	for _, item := range goodsList(role) {
		record, _ := item.(map[string]any)
		if record != nil && roleString(record, "GoodsId") == id {
			return record
		}
	}
	return nil
}

func isGameEquipment(kind int64) bool {
	switch kind {
	case 14, 15, 16, 21, 26, 29, 32:
		return true
	}
	return false
}

func addGoods(role Record, name string, goodsType, rank, level, element int, template string, count int, volume float64, recyclePrice int) Record {
	list := goodsList(role)
	count, rank, level = max(count, 1), max(rank, 1), max(level, 1)
	if !isGameEquipment(int64(goodsType)) {
		for _, item := range list {
			g, _ := item.(map[string]any)
			if roleInt(g, "Type") != int64(goodsType) {
				continue
			}
			if strings.HasPrefix(template, "mat.") && (g["BrewPaidUnit"] != nil || roleString(g, "BrewSource") != "" || roleString(g, "TemplateId") != template || recyclePrice > 0 && roleInt(g, "RecyclePrice") == 0) {
				continue
			}
			if roleString(g, "TemplateId") == template ||
				roleString(g, "Name") == name && roleInt(g, "Rank") == int64(rank) && roleInt(g, "Level") == int64(level) {
				g["Count"] = max(1, roleInt(g, "Count")) + int64(count)
				return g
			}
		}
	}
	if len(list) >= ringCapacity(role) {
		return nil
	}
	if volume <= 0 {
		volume = 1
	}
	goods := Record{
		"GoodsId": "g_" + newGameID(8), "Name": name, "Type": goodsType, "Rank": rank, "Level": level,
		"FiveElement": min(5, max(1, element)), "TemplateId": template, "Count": count,
		"Volume": volume, "RecyclePrice": max(0, recyclePrice), "ConversionRate": 1.0,
		"IsLock": false, "IsUsing": false, "IsBinding": false,
	}
	role["Goods"] = append(list, goods)
	return goods
}

func removeGoods(role Record, id string) {
	list := goodsList(role)
	for i, item := range list {
		g, _ := item.(map[string]any)
		if g == nil || roleString(g, "GoodsId") != id {
			continue
		}
		if count := roleInt(g, "Count"); count > 1 {
			g["Count"] = count - 1
		} else {
			role["Goods"] = append(list[:i], list[i+1:]...)
		}
		return
	}
}

func firstCultivationRoot(role Record) int {
	for i, field := range cultivationRootFields[1:] {
		if roleBool(role, field) {
			return i + 1
		}
	}
	return 0
}

func primaryGameElement(role Record) int {
	return max(1, firstCultivationRoot(role))
}

func ensureStarterItems(role Record) {
	version := roleInt(role, "StarterKitVersion")
	if version >= 2 {
		return
	}
	element := primaryGameElement(role)
	if version < 1 {
		addGoods(role, "启灵经", 1, 1, 1, element, practiceStarterMethodID, 1, 1, 10)
		addGoods(role, "青锋剑", 15, 1, 1, element, "eq_weapon_qingfeng", 1, 1, 20)
		addGoods(role, "青布道袍", 26, 1, 1, element, "eq_armor_qingbu", 1, 1, 20)
		addGoods(role, "纳气丹", 19, 1, 1, element, "pill_naqi", 3, .5, 5)
		addGoods(role, "回春散", 19, 1, 1, element, "pill_huichun", 3, .5, 5)
	}
	addGoods(role, "玄铁护心镜", 16, 2, 1, element, "eq_armor_xuantie", 1, 1, 40)
	addGoods(role, "青布靴", 29, 1, 1, element, "eq_boots_qingbu", 1, 1, 15)
	role["StarterKitVersion"], role["StarterKitNotice"] = 2, true
}

func gameGoodsMD5(role Record) string {
	list := make([]Record, 0, len(goodsList(role)))
	for _, entry := range goodsList(role) {
		if r, ok := entry.(map[string]any); ok {
			list = append(list, r)
		}
	}
	sort.Slice(list, func(i, j int) bool { return roleString(list[i], "GoodsId") < roleString(list[j], "GoodsId") })
	var b strings.Builder
	for _, r := range list {
		b.WriteString(roleString(r, "GoodsId"))
		count := roleInt(r, "Count")
		if r["Count"] == nil {
			count = 1
		}
		b.WriteString(strconv.FormatInt(max(1, count), 10))
	}
	c := md5.Sum([]byte(strings.ToLower(b.String())))
	return hex.EncodeToString(c[:])
}

func inventoryFlag(goods Record, field string) bool {
	return roleBool(goods, field) || roleInt(goods, field) != 0
}

func fillGameGoods(m protoreflect.Message, g Record, firstRoot int) {
	copyRolePB(m, g, "GoodsId", "Name", "Type", "RecyclePrice", "TemplateId", "BlankId")
	if hasFriendOfferLock(g) {
		set(m, "OfferPending", true)
	}
	for _, flag := range [...]string{"IsUsing", "IsBinding", "IsLock"} {
		set(m, flag, inventoryFlag(g, flag))
	}
	putPB(m, Record{"FiveElement": 1, "Rank": 1, "Level": 1, "Volume": 1.0, "ConversionRate": 1.0})
	copyRolePB(m, g, "FiveElement", "Rank", "Level", "Volume", "ConversionRate")
	if roleInt(g, "Type") == 1 {
		// Legacy quest rewards sometimes used the recipient's element and only stored the title.
		if def := legacyMethods.book(g); def != nil {
			element := def.element
			if def.id == practiceStarterMethodID {
				element = firstRoot
			}
			putPB(m, Record{"FiveElement": element, "Rank": def.rank, "Level": def.learningLevel, "TemplateId": def.id})
		}
	}
	set(m, "Count", max(1, roleInt(g, "Count")))
}

var gameInventoryProtocols = map[int]gameMapProtocol{
	1002015: {"", "SearchIsSettingBattleEquipmentResp_1002015"},
	1004002: {"SpaceGoodsReq_1004002", "SpaceGoodsResp_1004002"},
	1004004: {"SpaceGoodsCheckReq_1004004", "SpaceGoodsCheckResp_1004004"},
	1004008: {"GoodsBookAchievementDetailReq_1004008", "GoodsBookAchievementDetailResp_1004008"},
	1004014: {"GoodsArticleDefenseEquipmentReq_1004014", "GoodsArticleDefenseEquipmentResp_1004014"},
	1004015: {"GoodsArticleElixirReq_1004015", "GoodsArticleElixirResp_1004015"},
	1004018: {"GoodsArticleSetableAttackEquipmentReq_1004018", "GoodsArticleSetableAttackEquipmentResp_1004018"},
	1004019: {"GoodsArticleSetableDefenseEquipmentReq_1004019", "GoodsArticleSetableDefenseEquipmentResp_1004019"},
	1004023: {"GoodsBindingReq_1004023", "GoodsBindingResp_1004023"},
	1004024: {"GoodsUseReq_1004024", "GoodsUseResp_1004024"},
	1004032: {"UsedElixirReq_1004032", "UsedElixirResp_1004032"},
	1011009: {"MethodSkillDetailReq_1011009", "MethodSkillDetailResp_1011009"},
}

func (s *Server) handleInventory(peer *Peer, msg Message) bool {
	p, ok := gameInventoryProtocols[msg.Cmd]
	if !ok {
		return s.handleEquipment(peer, msg)
	}
	if msg.Cmd == 1002015 {
		b := s.gameResponse(p.resp, true, 2000)
		putPB(b, Record{"Code": 4507, "IsSettingEquipmentBattle": true})
		s.sendGame(peer, msg, b)
		return true
	}
	req, err := s.decode(p.req, msg.Data)
	if err != nil {
		return true
	}
	if msg.Cmd == 1004002 || msg.Cmd == 1004004 {
		if s.previewWrites {
			s.expireFriendLocks(peer.currentRoleID())
		}
		s.readInventory(peer, msg, req, p.resp)
		return true
	}
	b := s.gameResponse(p.resp, true, 2000)
	roleID := peer.currentRoleID()
	switch msg.Cmd {
	case 1011009:
		skill, found := battleSkillByID(getString(req, "Id"))
		if roleID == "" || !found || skill.kind != 1 {
			statusPB(b, false, 4605)
			break
		}
		role := s.roles.Get(roleID)
		resolved := resolvePlayerSpell(role, equippedBattleStats(role).attack, skill)
		if skill.affectsMovement() && resolved.damage > 0 {
			b.SetUnknown(protowire.AppendFixed64(protowire.AppendTag(b.GetUnknown(), 10, protowire.Fixed64Type), math.Float64bits(resolved.damage)))
		}
		putPB(b, Record{
			"SkillType": skill.skillType, "CastTime": resolved.skill.castMS, "ContinueTime": resolved.skill.durationMS,
			"ConversionRate": resolved.conversion, "CreatedBy": "修真界",
		})
	case 1004008:
		def := s.methods().book(s.inventoryDetailItem(roleID, getString(req, "Id"), false))
		if def == nil {
			statusPB(b, false, 4605)
		} else {
			putPB(b, Record{
				"ActionScope": def.kind, "Level": def.learningLevel, "BasedConcentration": def.baseCircle,
				"ConversionRate": def.baseCircle, "RunSpeed": int(math.Round(def.baseCircle * 10)),
				"CreatedBy": "修真界", "Volume": 1.0,
			})
			def.writeEffects(b)
		}
	case 1004015:
		g := s.inventoryDetailItem(roleID, getString(req, "Id"), false)
		s.elixirDetail(roleID, b, g)
	case 1004014, 1004018, 1004019:
		id := getString(req, "Id")
		var g Record
		if (s.preview == nil || s.previewWrites) && roleID != "" && strings.HasPrefix(id, "ugc.") {
			g = s.smithingTemplate(id)
		} else {
			g = s.inventoryDetailItem(roleID, id, true)
		}
		if g == nil && msg.Cmd == 1004018 && roleID != "" {
			if skill, found := battleSkillByID(id); found && skill.kind == 2 {
				role := s.roles.project(roleID, []string{"Perception"})
				putPB(b, Record{"CoolDown": skill.cooldownS * 1000, "ConversionRate": 1.0, "UsedPerception": max(100, roleInt(role, "Perception"))/10, "CreatedBy": "修真界"})
				break
			}
		}
		if g == nil {
			statusPB(b, false, 4605)
		} else if isSmithingEquipment(g) {
			fillSmithingEquipmentDetail(b, g)
		} else {
			rank := max(1, roleInt(g, "Rank"))
			putPB(b, Record{
				"ConversionRate": 1.0, "CreatedBy": "修真界", "Volume": 1.0,
			})
			copyRolePB(b, g, "ConversionRate", "Volume", "EffectType1", "EffectValue1", "EffectType2", "EffectValue2", "EffectType3", "EffectValue3")
			if msg.Cmd == 1004014 {
				putPB(b, Record{"ToughTenacity": 12 * rank})
				copyRolePB(b, g, "UsedPhysique")
				break
			}
			putPB(b, Record{"MinSpiritualityValue": 10 * rank, "MaxSpiritualityValue": 100 * rank})
			if msg.Cmd == 1004018 {
				putPB(b, Record{"ToughTenacity": 10 * rank})
			} else {
				putPB(b, Record{"ToughTenacity": 12 * rank})
			}
		}
	default:
		err = s.roles.Update(roleID, func(r Record) error {
			id := getString(req, "GoodsId")
			g := locateGoods(r, id)
			if g == nil {
				code := 4603
				if msg.Cmd == 1004032 {
					code = 4605
				}
				statusPB(b, false, code)
				if msg.Cmd == 1004024 {
					set(b, "NowCapacity", len(goodsList(r)))
				}
				return nil
			}
			if hasFriendOfferLock(g) {
				statusPB(b, false, 4509)
				return nil
			}
			switch msg.Cmd {
			case 1004023:
				g["IsBinding"] = true
			case 1004024:
				kind := roleInt(g, "Type")
				switch {
				case kind == 1:
					def := s.methods().book(g)
					if def == nil {
						statusPB(b, false, 4605)
						break
					}
					code := readStudyBook(r, def)
					if code == studyBookAdded {
						removeGoods(r, id)
					}
					statusPB(b, code == studyBookAdded, code)
				case kind == 21 && (strings.HasPrefix(roleString(g, "TemplateId"), "sigil.") || roleString(g, "TemplateId") == legacyHasteTalisman):
					if _, useErr := useInscribedTalisman(r, g); useErr != nil {
						statusPB(b, false, 4605)
					}
				case kind == 19:
					if code := s.consumeGamePill(r, g); code != 2000 {
						statusPB(b, false, code)
						return nil
					}
					removeGoods(r, id)
				case isGameEquipment(kind):
					statusPB(b, false, 4604)
				default:
					statusPB(b, false, 4605)
				}
				set(b, "NowCapacity", len(goodsList(r)))
			case 1004032:
				if roleInt(g, "Type") != 19 {
					statusPB(b, false, 4605)
					return nil
				}
				if code := s.consumeGamePill(r, g); code != 2000 {
					statusPB(b, false, code)
					return nil
				}
				removeGoods(r, id)
				fillPillRole(b, r)
				set(b, "NowCapacity", len(goodsList(r)))
			}
			return nil
		})
		if err != nil {
			statusPB(b, false, 4007)
		}
	}
	s.sendGame(peer, msg, b)
	return true
}

func fillPillRole(b protoreflect.Message, role Record) {
	copyRolePB(b, role, "PureBlood", "FlexibleRoot", "SuperAir", "Perception", "Physique", "CurrentSuperAir",
		"CurrentPerception", "CurrentPhysique", "CurrentVigor", "CurrentClever", "CurrentPoisonVal", "CurrentPureBlood",
		"HasGold", "HasWood", "HasWater", "HasFire", "HasEarth", "HunLiVal", "Power", "CurrentPower", "MaxAge")
	set(b, "Age", roleInt(role, "CurrentAge"))
}

func receiveInventoryGoods(to, goods Record, count int64) (string, error) {
	return receiveInventoryStack(to, goods, count, ringCapacity(to))
}

func receiveInventoryStack(to, goods Record, count int64, slots int) (string, error) {
	items := goodsList(to)
	if !isGameEquipment(roleInt(goods, "Type")) {
		attributes, err := transferStackAttributes(goods)
		if err != nil {
			return "", err
		}
		for _, item := range items {
			target, _ := item.(Record)
			if roleString(target, "TemplateId") != roleString(goods, "TemplateId") || roleInt(target, "Type") != roleInt(goods, "Type") {
				continue
			}
			other, err := transferStackAttributes(target)
			if err != nil {
				return "", err
			}
			available := transferStackCount(target)
			if bytes.Equal(attributes, other) && roleString(target, "GoodsId") != "" && available > 0 && available <= math.MaxInt32-count {
				target["Count"] = available + count
				return roleString(target, "GoodsId"), nil
			}
		}
	}
	if slots > 0 && len(items) >= slots {
		return "", previewReject(4527)
	}
	received := cloneRecord(goods)
	id := roleString(goods, "GoodsId")
	if id == "" || count < transferStackCount(goods) || locateGoods(to, id) != nil {
		id = "g_" + newGameID(8)
	}
	received["GoodsId"], received["Count"] = id, count
	to["Goods"] = append(items, received)
	return id, nil
}
