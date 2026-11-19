package main

import (
	"crypto/md5" // Legacy client inventory-change checksum.
	"encoding/hex"
	"math"
	"sort"
	"strconv"
	"strings"

	"google.golang.org/protobuf/reflect/protoreflect"
)

func goodsList(role Record) []any {
	list, _ := role["Goods"].([]any)
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
			if roleString(g, "TemplateId") == template ||
				roleString(g, "Name") == name && roleInt(g, "Rank") == int64(rank) && roleInt(g, "Level") == int64(level) {
				g["Count"] = max(1, roleInt(g, "Count")) + int64(count)
				return g
			}
		}
	}
	if len(list) >= 100 {
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

func primaryGameElement(role Record) int {
	for i, field := range []string{"HasGold", "HasWood", "HasWater", "HasFire", "HasEarth"} {
		if roleBool(role, field) {
			return i + 1
		}
	}
	return 1
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
		addGoods(role, "青布道袍", 16, 1, 1, element, "eq_armor_qingbu", 1, 1, 20)
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
		b.WriteString(strconv.FormatInt(count, 10))
	}
	c := md5.Sum([]byte(strings.ToLower(b.String())))
	return hex.EncodeToString(c[:])
}

func fillGameGoods(m protoreflect.Message, g Record) {
	copyRolePB(m, g, "GoodsId", "Name", "Type", "IsUsing", "IsBinding", "RecyclePrice", "TemplateId", "IsLock")
	putPB(m, Record{"FiveElement": 1, "Rank": 1, "Level": 1, "Volume": 1.0, "ConversionRate": 1.0})
	copyRolePB(m, g, "FiveElement", "Rank", "Level", "Volume", "ConversionRate")
	set(m, "Count", max(1, roleInt(g, "Count")))
}

func bookMethod(g Record) *gameMethod {
	if g == nil {
		return nil
	}
	if def := findGameMethod(roleString(g, "TemplateId")); def != nil {
		return def
	}
	name := roleString(g, "Name")
	for i := range methods {
		if methods[i].name == name {
			return &methods[i]
		}
	}
	return nil
}

var gameInventoryProtocols = map[int]gameMapProtocol{
	1002015: {"", "SearchIsSettingBattleEquipmentResp_1002015"},
	1004002: {"SpaceGoodsReq_1004002", "SpaceGoodsResp_1004002"},
	1004004: {"SpaceGoodsCheckReq_1004004", "SpaceGoodsCheckResp_1004004"},
	1004008: {"GoodsBookAchievementDetailReq_1004008", "GoodsBookAchievementDetailResp_1004008"},
	1004014: {"GoodsArticleDefenseEquipmentReq_1004014", "GoodsArticleDefenseEquipmentResp_1004014"},
	1004018: {"GoodsArticleSetableAttackEquipmentReq_1004018", "GoodsArticleSetableAttackEquipmentResp_1004018"},
	1004019: {"GoodsArticleSetableDefenseEquipmentReq_1004019", "GoodsArticleSetableDefenseEquipmentResp_1004019"},
	1004023: {"GoodsBindingReq_1004023", "GoodsBindingResp_1004023"},
	1004024: {"GoodsUseReq_1004024", "GoodsUseResp_1004024"},
	1004032: {"UsedElixirReq_1004032", "UsedElixirResp_1004032"},
}

func (s *Server) handleInventory(peer *Peer, msg Message) bool {
	p, ok := gameInventoryProtocols[msg.Cmd]
	if !ok {
		return s.handleEquipment(peer, msg)
	}
	b := s.gameResponse(p.resp, true, 2000)
	if msg.Cmd == 1002015 {
		putPB(b, Record{"Code": 4507, "IsSettingEquipmentBattle": true})
		s.sendGame(peer, msg, b)
		return true
	}
	req, err := s.decode(p.req, msg.Data)
	if err != nil {
		return true
	}
	roleID := peer.currentRoleID()
	role := s.roles.Get(roleID)
	quest := false
	switch msg.Cmd {
	case 1004002:
		loc := getInt(req, "Location")
		if loc == 10 || loc == 0 {
			for _, entry := range goodsList(role) {
				if g, ok := entry.(map[string]any); ok {
					fillGameGoods(addMessage(b, "GoodsInfos"), g)
				}
			}
		}
		putPB(b, Record{"Location": loc, "NowCapacity": len(goodsList(role)), "MaxCapacity": 100})
	case 1004004:
		putPB(b, Record{"Location": getInt(req, "Location"), "Md5": gameGoodsMD5(role)})
	case 1004008:
		def := bookMethod(locateGoods(role, getString(req, "Id")))
		if def == nil {
			statusPB(b, false, 4605)
		} else {
			putPB(b, Record{
				"ActionScope": def.kind, "Level": def.learningLevel, "BasedConcentration": def.baseCircle,
				"ConversionRate": def.baseCircle, "RunSpeed": int(math.Round(def.baseCircle * 10)),
				"CreatedBy": "修真界", "EffectType1": def.effect1, "EffectValue1": def.effectValue1,
				"EffectType2": def.effect2, "EffectValue2": def.effectValue2, "Volume": 1.0,
			})
		}
	case 1004014, 1004018, 1004019:
		id := getString(req, "Id")
		g := locateGoods(role, id)
		if g == nil && id != "" {
			for _, worn := range battleRecords(role["EquippedGoods"]) {
				if roleString(worn, "GoodsId") == id {
					g = worn
					break
				}
			}
		}
		if g == nil {
			statusPB(b, false, 4605)
		} else {
			rank := max(1, roleInt(g, "Rank"))
			putPB(b, Record{
				"ConversionRate": 1.0, "EffectValue1": 5 + rank, "CreatedBy": "修真界", "Volume": 1.0,
			})
			copyRolePB(b, g, "ConversionRate", "Volume")
			if msg.Cmd == 1004014 {
				putPB(b, Record{"ToughTenacity": 12 * rank, "EffectType1": 302})
				copyRolePB(b, g, "UsedPhysique")
				break
			}
			putPB(b, Record{"MinSpiritualityValue": 10 * rank, "MaxSpiritualityValue": 100 * rank})
			if msg.Cmd == 1004018 {
				putPB(b, Record{"ToughTenacity": 10 * rank, "EffectType1": 301, "EffectType2": 302, "EffectValue2": 2 + rank/2})
			} else {
				putPB(b, Record{"ToughTenacity": 12 * rank, "EffectType1": 302})
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
			switch msg.Cmd {
			case 1004023:
				g["IsBinding"] = true
			case 1004024:
				kind := roleInt(g, "Type")
				switch {
				case kind == 1 && bookMethod(g) != nil:
					def := bookMethod(g)
					removeGoods(r, id)
					waitForGuide := def.id == practiceStarterMethodID &&
						(roleInt(r, "PracticeGuideVal") == 1 || practiceStarterPending(r))
					if !waitForGuide {
						learnGameMethod(r, def)
						r[gameEquipField(def.kind)] = def.id
						quest = s.gameMethodQuest(r, def)
					}
				case kind == 19:
					s.gamePillAir(r, .35, true)
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
				name := roleString(g, "Name")
				switch {
				case strings.Contains(name, "纳气") || strings.Contains(name, "聚气") || strings.Contains(name, "补气"):
					s.gamePillAir(r, .35, true)
				case strings.Contains(name, "回春") || strings.Contains(name, "疗伤"):
					r["CurrentVigor"] = roleInt(r, "CurrentVigor") + 30
					r["CurrentPureBlood"] = max(1, roleInt(r, "CurrentPureBlood")) + 30
				case strings.Contains(name, "解毒"):
					r["CurrentPoisonVal"] = max(0, roleInt(r, "CurrentPoisonVal")-20)
				default:
					s.gamePillAir(r, .15, false)
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
	if role == nil && (msg.Cmd == 1004002 || msg.Cmd == 1004004) {
		statusPB(b, false, 4007)
	}
	s.sendGame(peer, msg, b)
	if quest && err == nil {
		s.gameQuestComplete(peer)
	}
	return true
}

func (s *Server) gamePillAir(r Record, factor float64, atLeastOne bool) {
	lvl := s.levelInfo(roleInt(r, "Level"))
	gain := int64(math.Round(lvl.upgradeLimit * factor))
	if atLeastOne {
		gain = max(1, gain)
	}
	r["SuperAir"] = min(int64(lvl.upgradeLimit*3), roleInt(r, "SuperAir")+gain)
}

func fillPillRole(b protoreflect.Message, role Record) {
	copyRolePB(b, role, "PureBlood", "FlexibleRoot", "SuperAir", "Perception", "Physique", "CurrentSuperAir",
		"CurrentPerception", "CurrentPhysique", "CurrentVigor", "CurrentClever", "CurrentPoisonVal", "CurrentPureBlood",
		"HasGold", "HasWood", "HasWater", "HasFire", "HasEarth", "HunLiVal", "Power", "CurrentPower", "MaxAge")
	set(b, "Age", roleInt(role, "CurrentAge"))
}
