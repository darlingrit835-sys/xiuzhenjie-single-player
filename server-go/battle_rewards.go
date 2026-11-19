package main

import (
	"fmt"
	"log"
	"math"
	"math/rand"
	"strconv"
)

type battleDrop struct {
	id, name                        string
	goodsType, rank, level, element int
	count                           int
	x, y                            float64
}

var battleMaterials = [...]string{"妖兽皮", "妖兽血", "妖骨", "妖丹", "灵草", "玄铁矿石"}
var battlePills = [...]string{"纳气丹", "回春散", "解毒丹", "经脉丹"}
var battleWeapons = [...]string{"青锋剑", "寒铁剑", "赤炎剑", "流光剑", "玄天剑", "太虚剑"}
var battleArmors = [...]string{"青布道袍", "玄铁护甲", "赤霞衣", "流云袍", "玄冰甲", "太虚道袍"}
var battleBoots = [...]string{"青布靴", "疾风靴", "踏云靴", "流云履", "玄冰靴", "太虚履"}

func battleRecords(value any) []Record {
	rows, _ := value.([]any)
	out := make([]Record, 0, len(rows))
	for _, row := range rows {
		if record, ok := row.(Record); ok {
			out = append(out, record)
		}
	}
	return out
}

func (s *Server) battleMonsterDead(peer *Peer, msg Message) {
	req, err := s.decode("MonsterDeadReq_8002003", msg.Data)
	if err != nil {
		return
	}
	d := s.dungeonForRole(peer.dungeonID, peer.roleID, false)
	if d == nil {
		return
	}
	index := int(getInt(req, "DeadMonsterIndex"))
	slot := index - 1001
	if slot < 0 || slot >= len(d.monsters) {
		return
	}
	monster := d.monsters[slot]
	x, y := getFloat(req, "PosX"), getFloat(req, "PosY")
	drops, questDone, rewarded := s.settleMonster(d, monster, x, y)
	if !rewarded {
		return
	}
	for _, drop := range drops {
		resp := s.pb("MonsterDeadResp_8002003")
		putPB(resp, map[string]any{
			"DeadMonsterIndex": monster.index, "GoodsId": drop.id, "PosX": drop.x, "PosY": drop.y,
			"FiveElement": drop.element, "GoodsType": drop.goodsType, "Name": drop.name,
			"Level": drop.level, "Rank": drop.rank, "ObjectIndex": monster.index, "Count": drop.count,
		})
		_ = peer.send(msg.Cmd, 0, resp)
	}
	if questDone {
		s.pushGame(peer.roleID, 1016001, nil)
	}
}

func (s *Server) settleMonster(d *battleDungeon, monster battleMonster, x, y float64) ([]battleDrop, bool, bool) {
	d.mu.Lock()
	defer d.mu.Unlock()
	if _, awarded := d.dead[monster.index]; awarded || d.closed {
		return nil, false, false
	}
	count := int(min(math.MaxInt32, max(1, monster.gold)))
	drops := []battleDrop{{
		id: "lingshi_" + strconv.Itoa(count) + "_" + newGameID(7), name: "灵石", goodsType: 100,
		rank: 1, level: monster.level, element: monster.element, count: count, x: x, y: y,
	}}
	questDone := false
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.Get(d.roleID)
		if role == nil {
			return ErrRecordNotFound
		}
		s.grantBattleCultivation(role, monster.unusual)
		event := "KILL"
		if monster.unusual == 4 {
			event = "BOSS_KILL"
			// The generator has stable spawn slots per map and no separate monster template table.
			monsterID := d.mapID + "/" + strconv.Itoa(monster.index)
			if err := tx.RecordHistory(newHistoryEvent("boss.defeated", role, nil, "monster", monsterID, Record{
				"dungeonId": d.id, "mapId": d.mapID, "monsterIndex": monster.index, "monsterName": monster.name,
			})); err != nil {
				return err
			}
		}
		questDone = s.grantQuestEvent(role, event)
		drops = append(drops, rollBattleDrops(monster, x+.6, y+.4)...)
		if len(d.dead)+1 == len(d.monsters) {
			questDone = s.grantQuestEvent(role, "SWEEP") || questDone
			bonus := 30 + int64(d.playerLevel)*12
			role["ShinyStone"] = roleInt(role, "ShinyStone") + bonus
			drops = append(drops, newBattleDrop("妖丹", 11, battleDropRank(d.playerLevel), d.playerLevel, 1, x, y))
			if err := tx.RecordHistory(newHistoryEvent("dungeon.cleared", role, nil, "dungeon", d.mapID, Record{
				"dungeonId": d.id, "mapId": d.mapID, "mapName": gameMapName(d.mapID),
				"monsterCount": len(d.monsters), "shinyStoneReward": bonus,
			})); err != nil {
				return err
			}
		}
		loot, _ := role["BattleLoot"].([]any)
		for _, drop := range drops {
			loot = append(loot, Record{
				"DropId": drop.id, "Name": drop.name, "Type": drop.goodsType, "Rank": drop.rank,
				"Level": drop.level, "FiveElement": drop.element, "Count": drop.count,
				"TemplateId": fmt.Sprintf("drop_%d_%d_%d", drop.goodsType, drop.rank, drop.level),
			})
		}
		role["BattleLoot"] = loot
		return nil
	})
	if err != nil {
		log.Printf("battle reward save: %v", err)
		return nil, false, false
	}
	d.dead[monster.index] = struct{}{}
	return drops, questDone, true
}

func (s *Server) grantBattleCultivation(role Record, tier int) {
	level := max(1, roleInt(role, "Level"))
	limit := s.levelInfo(level).upgradeLimit
	factor := .02
	switch tier {
	case 3:
		factor = .06
	case 4:
		factor = .15
	}
	gain := max(1, int64(math.Round(limit*factor)))
	attributeGain := max(1, int64(math.Round(float64(gain)*.02)))
	air := min(int64(limit*2), roleInt(role, "SuperAir")+gain)
	perception, physique := roleInt(role, "Perception")+attributeGain, roleInt(role, "Physique")+attributeGain
	role["SuperAir"] = air
	role["Perception"], role["CurrentPerception"] = perception, perception
	role["Physique"], role["CurrentPhysique"] = physique, physique
	power := min(math.MaxInt32, level*1000+perception*10+physique*10+air/10)
	role["Power"], role["CurrentPower"] = power, power
}

func battleDropRank(level int) int {
	stage := max(1, (level-1)/10+1)
	return max(1, min(16, stage*4-3+rand.Intn(3)))
}

func newBattleDrop(name string, goodsType, rank, level, count int, x, y float64) battleDrop {
	return battleDrop{
		id: "loot_" + newGameID(7), name: name, goodsType: goodsType,
		rank: max(1, min(16, rank)), level: max(1, min(90, level)), element: rand.Intn(5) + 1,
		count: max(1, count), x: x, y: y,
	}
}

func rollBattleDrops(monster battleMonster, x, y float64) []battleDrop {
	materialChance, pillChance, equipmentChance := .45, .10, .035
	switch monster.unusual {
	case 3:
		materialChance, pillChance, equipmentChance = .85, .25, .16
	case 4:
		materialChance, pillChance, equipmentChance = 1, .55, .55
	}
	rank := battleDropRank(monster.level)
	drops := make([]battleDrop, 0, 2)
	roll := rand.Float64()
	switch {
	case roll < materialChance:
		idx := rand.Intn(len(battleMaterials))
		count := 1
		if monster.unusual == 4 {
			count = 2
		}
		drops = append(drops, newBattleDrop(battleMaterials[idx], 8+idx, rank, monster.level, count, x, y))
	case roll < materialChance+pillChance:
		name := battlePills[rand.Intn(len(battlePills))]
		drops = append(drops, newBattleDrop(name, 19, rank, monster.level, 1, x, y))
	case roll < materialChance+pillChance+equipmentChance:
		drops = append(drops, rollBattleEquipment(rand.Intn(3), rank, monster.level, x, y))
	}
	if monster.unusual == 4 && len(drops) < 2 {
		drops = append(drops, rollBattleEquipment(1+rand.Intn(2), rank, monster.level, x, y))
	}
	return drops
}

func rollBattleEquipment(kind, rank, level int, x, y float64) battleDrop {
	index := min(len(battleWeapons)-1, max(0, (rank-1)/3))
	switch kind {
	case 0:
		return newBattleDrop(battleWeapons[index], 15, rank, level, 1, x, y)
	case 1:
		return newBattleDrop(battleArmors[index], 16, rank, level, 1, x, y)
	default:
		return newBattleDrop(battleBoots[index], 29, rank, level, 1, x, y)
	}
}

func (s *Server) gamePickUpGoods(peer *Peer, msg Message) {
	req, err := s.decode("PickUpGoodsReq_1004026", msg.Data)
	if err != nil {
		return
	}
	id := getString(req, "GoodsId")
	outID, code, capacity := id, 4536, 0
	questDone := false
	err = s.roles.UpdateProgress(peer.roleID, func(role Record) (bool, error) {
		balance := roleInt(role, "ShinyStone")
		outID, code = collectBattleDrop(role, id)
		if roleInt(role, "ShinyStone") > balance {
			questDone = s.checkQuest(role)
		}
		capacity = len(goodsList(role))
		return code == 4526, nil
	})
	if err != nil {
		log.Printf("battle pickup save: %v", err)
		outID, code = id, 5000
	}
	resp := s.pb("PickUpGoodsResp_1004026")
	statusPB(resp, code == 4526, code)
	set(resp, "NowCapacity", capacity)
	set(resp, "GoodsId", outID)
	_ = peer.send(msg.Cmd, msg.Msid, resp)
	if code == 4526 && questDone {
		_ = peer.send(1016001, 0, nil)
	}
}

func collectBattleDrop(role Record, id string) (string, int) {
	loot, _ := role["BattleLoot"].([]any)
	for i, row := range loot {
		item, ok := row.(Record)
		if !ok || id == "" || roleString(item, "DropId") != id {
			continue
		}
		kind := int(roleInt(item, "Type"))
		count := max(1, int(roleInt(item, "Count")))
		if kind == 100 {
			role["ShinyStone"] = roleInt(role, "ShinyStone") + int64(count)
		} else {
			rank := max(1, int(roleInt(item, "Rank")))
			goods := addGoods(role, roleString(item, "Name"), kind, rank, int(roleInt(item, "Level")),
				int(roleInt(item, "FiveElement")), roleString(item, "TemplateId"), count, 1, 10+rank*5)
			if goods == nil {
				return id, 4527
			}
			id = roleString(goods, "GoodsId")
		}
		role["BattleLoot"] = append(loot[:i], loot[i+1:]...)
		return id, 4526
	}
	return id, 4536
}
