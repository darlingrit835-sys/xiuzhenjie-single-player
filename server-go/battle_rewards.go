package main

import (
	"fmt"
	"log"
	"math"
	"math/rand"
	"slices"
	"strconv"
	"strings"
	"time"
)

type battleDrop struct {
	template                        string
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
	d.mu.Lock()
	if len(d.monsters) == 0 {
		d.mu.Unlock()
		return
	}
	slot := index - d.monsters[0].index
	if d.activePeer != peer || slot < 0 || slot >= len(d.monsters) {
		d.mu.Unlock()
		return
	}
	monster := d.monsters[slot]
	x, y := getFloat(req, "PosX"), getFloat(req, "PosY")
	if w := d.world; w != nil {
		cell := w.cell(x, y)
		// A moving monster dies where the client fought it. Only accept a discovered,
		// walkable site in the current field for combat drops.
		if !w.passable(cell) || w.explored[cell>>3]&(1<<uint(cell&7)) == 0 {
			d.mu.Unlock()
			return
		}
	}
	d.mu.Unlock()
	drops, questDone, rewarded := s.settleMonster(d, monster, x, y)
	if !rewarded {
		return
	}
	for _, drop := range drops {
		s.pushBattleDrop(peer, monster.index, drop)
	}

	if questDone {
		s.pushGame(peer.roleID, 1016001, nil)
	}
}

func (s *Server) pushBattleDrop(peer *Peer, index int, drop battleDrop) {
	resp := s.pb("MonsterDeadResp_8002003")
	putPB(resp, Record{"DeadMonsterIndex": index, "GoodsId": drop.id, "PosX": drop.x, "PosY": drop.y, "FiveElement": drop.element,
		"GoodsType": drop.goodsType, "Name": drop.name, "Level": drop.level, "Rank": drop.rank, "ObjectIndex": index, "Count": drop.count})
	_ = peer.send(8002003, 0, resp)
}

func (s *Server) replayFieldDrops(peer *Peer, dungeonID string) {
	role := s.roles.project(peer.roleID, []string{"BattleLoot"})
	for _, item := range battleRecords(role["BattleLoot"]) {
		if roleString(item, "DungeonId") != dungeonID {
			continue
		}
		s.pushBattleDrop(peer, int(roleInt(item, "ObjectIndex")), battleDrop{
			id: roleString(item, "DropId"), name: roleString(item, "Name"), goodsType: int(roleInt(item, "Type")),
			rank: int(roleInt(item, "Rank")), level: int(roleInt(item, "Level")), element: int(roleInt(item, "FiveElement")),
			count: int(roleInt(item, "Count")), x: roleFloat(item, "PosX"), y: roleFloat(item, "PosY"),
		})
	}
}

func (s *Server) settleMonster(d *battleDungeon, monster battleMonster, x, y float64) ([]battleDrop, bool, bool) {
	d.mu.Lock()
	defer d.mu.Unlock()
	if _, awarded := d.dead[monster.index]; awarded || len(d.monsters) == 0 || monster.index < d.monsters[0].index || monster.index > d.monsters[len(d.monsters)-1].index || d.closed || !time.Now().Before(d.expires) {
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
		if d.towerFloor > 0 {
			drops = nil
			if len(d.dead)+1 == len(d.monsters) {
				towerRecordClear(role, d.towerStage, d.towerFloor)
			}
			return nil
		}
		s.grantBattleCultivation(role, monster)
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
		dropX, dropY := x+.6, y+.4
		if d.world != nil && !d.world.clearSegment(x, y, dropX, dropY) {
			dropX, dropY = x, y
		}
		luck := methodAffixPercent(role, 601)
		drops = append(drops, rollBattleDrops(monster, dropX, dropY, luck, rand.Float64)...)
		drops = append(drops, rollHerbDrops(monster, dropX, dropY, rand.Intn, luck)...)
		if len(d.dead)+1 == len(d.monsters) {
			questDone = s.grantQuestEvent(role, "SWEEP") || questDone
			bonus := 30 + int64(d.playerLevel)*12
			role["ShinyStone"] = roleInt(role, "ShinyStone") + bonus
			role[achievementStones] = roleInt(role, achievementStones) + bonus
			drops = append(drops, newBattleDrop("妖丹", 11, battleDropRank(d.playerLevel), d.playerLevel, 1, x, y))
			if err := tx.RecordHistory(newHistoryEvent("dungeon.cleared", role, nil, "dungeon", d.mapID, Record{
				"dungeonId": d.id, "mapId": d.mapID, "mapName": gameMapName(d.mapID),
				"monsterCount": len(d.monsters), "shinyStoneReward": bonus,
			})); err != nil {
				return err
			}
		}
		loot, _ := role["BattleLoot"].([]any)
		now := time.Now().UnixMilli()
		loot = slices.DeleteFunc(loot, func(entry any) bool {
			item, _ := entry.(Record)
			expiry := roleInt(item, "ExpiresAt")
			return expiry != 0 && now >= expiry
		})
		for _, drop := range drops {
			template := drop.template
			if template == "" {
				template = battleDropTemplateID(drop.goodsType, drop.rank, drop.level)
				if drop.goodsType == 19 {
					template += "_" + drop.name
				}
			}
			loot = append(loot, Record{
				"DropId": drop.id, "Name": drop.name, "Type": drop.goodsType, "Rank": drop.rank,
				"DungeonId": d.id, "ObjectIndex": monster.index, "PosX": drop.x, "PosY": drop.y,
				"FieldDrop": d.world != nil, "ExpiresAt": d.expires.UnixMilli(),
				"Level": drop.level, "FiveElement": drop.element, "Count": drop.count,
				"TemplateId": template,
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

func battleTrainingShare(tier int) float64 {
	switch tier {
	case 3:
		return .033
	case 4:
		return .072
	default:
		return .012
	}
}

// Early encounters retain their historical integer payouts. Late encounters
// accumulate fractions so that many small hits are worth their actual total.
func battleAirReward(level int64, limit, share float64) float64 {
	return scaledEncounterReward(limit*share, lateRealmTimeFactor(level))
}

func scaledEncounterReward(points, factor float64) float64 {
	if factor == 1 {
		return max(1, math.Round(points))
	}
	return points / factor
}

func creditBattleProgress(role Record, key, remainderKey string, reward float64, ceiling int64) {
	value := roleInt(role, key)
	whole, fraction := math.Modf(reward)
	fraction += roleFloat(role, remainderKey)
	carry, fraction := math.Modf(fraction)
	add := min(int64(whole+carry), max(0, ceiling-value))
	role[key] = value + add
	if value+add < ceiling && fraction > 0 {
		role[remainderKey] = fraction
	} else {
		delete(role, remainderKey)
	}
}

func (s *Server) grantBattleCultivation(role Record, monster battleMonster) {
	level := max(1, roleInt(role, "Level"))
	lvl := s.levelInfo(level)
	// An open low-level encounter cannot award high-level progress after a breakthrough.
	share := battleTrainingShare(monster.unusual) * math.Pow(.75, float64(max(0, level-int64(monster.level))))
	creditBattleProgress(role, "SuperAir", "BattleAirRemainder", battleAirReward(level, lvl.upgradeLimit, share), int64(lvl.upgradeLimit))
	for i, key := range [...]string{"Perception", "Physique"} {
		base := [...]float64{lvl.perception, lvl.toughTenacity}[i]
		limit := lvl.trainingCap(i + 1)
		increase := scaledEncounterReward(float64(limit-int64(base))*share, attributeTimeFactor(level))
		creditBattleProgress(role, key, "Battle"+key+"Remainder", increase, limit)
		role["Current"+key] = role[key]
	}
	recomputeGamePower(role)
}

func battleDropRank(level int) int {
	return battleMaterialRank(level, rand.Intn(3))
}

func battleMaterialRank(level, offset int) int {
	return max(1, min(16, 1+(max(1, min(90, level))-1)*13/89+offset))
}

func battleDropTemplateID(kind, rank, level int) string {
	if kind == 16 {
		return fmt.Sprintf("ward_%d_%d", rank, level)
	}
	return fmt.Sprintf("drop_%d_%d_%d", kind, rank, level)
}

func battleMaterialPrice(rank int) int { return 10 + rank*5 }

func newBattleDrop(name string, goodsType, rank, level, count int, x, y float64) battleDrop {
	return battleDrop{
		id: "loot_" + newGameID(7), name: name, goodsType: goodsType,
		rank: max(1, min(16, rank)), level: max(1, min(90, level)), element: rand.Intn(5) + 1,
		count: max(1, count), x: x, y: y,
	}
}

func rollHerbDrops(monster battleMonster, x, y float64, draw func(int) int, luck int) []battleDrop {
	var out []battleDrop
	for _, herb := range monster.herbDrops {
		chance := min(10000, herb.rule.ChanceBps[monster.unusual]*(100+luck)/100)
		if chance <= 0 || draw(10000) >= chance {
			continue
		}
		count := herb.rule.MinQty + draw(herb.rule.MaxQty-herb.rule.MinQty+1)
		m := herb.material
		out = append(out, battleDrop{id: "loot_" + newGameID(7), template: m.id, name: m.name, goodsType: m.kind,
			rank: m.rank, level: 1, element: m.element, count: count, x: x, y: y})
	}
	return out
}

type battleDropRates struct{ material, pill, equipment float64 }

func dropRates(tier, luck int) battleDropRates {
	base := battleDropRates{.40, .035, .045}
	switch tier {
	case 3:
		base = battleDropRates{.75, .075, .18}
	case 4:
		base = battleDropRates{1, .15, .55}
	}
	factor := 1 + float64(luck)/100
	return battleDropRates{min(1, base.material*factor), min(1, base.pill*factor), min(1, base.equipment*factor)}
}

func rollBattleDrops(monster battleMonster, x, y float64, luck int, draw func() float64) []battleDrop {
	rates := dropRates(monster.unusual, luck)
	rank := battleDropRank(monster.level)
	var drops []battleDrop
	// Independent slots ensure an elite or boss can actually yield a pill and
	// any equipment slot, even when its material roll is guaranteed.
	if draw() < rates.material {
		idx := rand.Intn(len(battleMaterials))
		count := 1
		if monster.unusual == 4 {
			count = 2
		}
		drops = append(drops, newBattleDrop(battleMaterials[idx], 8+idx, rank, monster.level, count, x, y))
	}
	if draw() < rates.pill {
		name := battlePills[rand.Intn(len(battlePills))]
		drops = append(drops, newBattleDrop(name, 19, rank, monster.level, 1, x, y))
	}
	if draw() < rates.equipment {
		drops = append(drops, rollBattleEquipment(rand.Intn(4), rank, monster.level, x, y))
	}
	return drops
}

func rollBattleEquipment(kind, rank, level int, x, y float64) battleDrop {
	index := min(len(battleWeapons)-1, max(0, (rank-1)/3))
	switch kind {
	case 0:
		return newBattleDrop(battleWeapons[index], 15, rank, level, 1, x, y)
	case 1:
		return newBattleDrop(battleArmors[index], 26, rank, level, 1, x, y)
	case 3:
		return newBattleDrop("护心灵镜", 16, rank, level, 1, x, y)
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
	if strings.HasPrefix(id, "gather_") {
		s.pickFieldNode(peer, msg, getString(req, "BattleId"), id)
		return
	}
	outID, code, capacity, questDone := s.pickGroundDrop(peer.roleID, getString(req, "BattleId"), id)
	resp := s.pb("PickUpGoodsResp_1004026")
	statusPB(resp, code == 4526, code)
	set(resp, "NowCapacity", capacity)
	set(resp, "GoodsId", outID)
	_ = peer.send(msg.Cmd, msg.Msid, resp)
	if code == 4526 && questDone {
		_ = peer.send(1016001, 0, nil)
	}
}

func (s *Server) pickGroundDrop(roleID, dungeonID, id string) (outID string, code, capacity int, questDone bool) {
	var location Record
	for _, row := range battleRecords(s.roles.project(roleID, []string{"BattleLoot"})["BattleLoot"]) {
		if roleString(row, "DropId") == id {
			location = row
			break
		}
	}
	if location == nil {
		return id, 4536, 0, false
	}
	if roleBool(location, "FieldDrop") {
		owner := roleString(location, "DungeonId")
		if dungeonID != "" && owner != dungeonID {
			return id, 4536, 0, false
		}
		d := s.dungeonForRole(owner, roleID, false)
		if d == nil {
			return id, 4536, 0, false
		}
		d.mu.Lock()
		defer d.mu.Unlock()
		x, y := roleFloat(location, "PosX"), roleFloat(location, "PosY")
		if d.closed || d.world == nil || !d.world.withinReach(x, y) {
			return id, 4536, 0, false
		}
	}
	outID, code = id, 4536

	err := s.roles.UpdateProgress(roleID, func(role Record) (bool, error) {
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
	return
}

func (s *Server) pickFieldNode(peer *Peer, msg Message, dungeonID, id string) {
	out, code, capacity := id, 4536, 0
	if d := s.dungeonForRole(dungeonID, peer.roleID, false); d != nil {
		out, code, capacity = s.collectFieldNode(d, id)
	}
	resp := s.pb("PickUpGoodsResp_1004026")
	statusPB(resp, code == 4526, code)
	putPB(resp, Record{"NowCapacity": capacity, "GoodsId": out})
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}

func (s *Server) collectFieldNode(d *battleDungeon, id string) (out string, code, capacity int) {
	d.mu.Lock()
	defer d.mu.Unlock()
	w := d.world
	if d.closed || w == nil || !time.Now().Before(d.expires) {
		return id, 4536, 0
	}
	node := w.nodes[id]
	if node == nil {
		return id, 4536, 0
	}
	if node.collectedID != "" {
		return node.collectedID, 4526, len(goodsList(s.roles.Get(d.roleID)))
	}
	if !w.withinReach(node.x, node.y) {
		return id, 4536, 0
	}
	out, code = id, 4527
	err := s.roles.UpdateProgress(d.roleID, func(role Record) (bool, error) {
		item := node.item
		goods := addGoods(role, item.name, item.kind, item.rank, 1, item.element, item.id, node.count, 1, battleMaterialPrice(item.rank))
		capacity = len(goodsList(role))
		if goods == nil {
			return false, nil
		}
		out, code = roleString(goods, "GoodsId"), 4526
		return true, nil
	})
	if err != nil {
		log.Printf("battle resource save: %v", err)
		return id, 5000, 0
	}
	if code == 4526 {
		node.collectedID = out
	}
	return out, code, capacity
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
			role[achievementStones] = roleInt(role, achievementStones) + int64(count)
		} else {
			rank := max(1, int(roleInt(item, "Rank")))
			goods := addGoods(role, roleString(item, "Name"), kind, rank, int(roleInt(item, "Level")),
				int(roleInt(item, "FiveElement")), roleString(item, "TemplateId"), count, 1, battleMaterialPrice(rank))
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
