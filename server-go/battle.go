package main

import (
	"log"
	"math"
	"slices"
	"sync"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

type battleState struct {
	mu          sync.Mutex
	dungeons    map[string]*battleDungeon
	loadOnce    sync.Once
	balance     monsterConfiguration
	ecologyOnce sync.Once
	ecology     map[string]*countyEcology
}

type battleDungeon struct {
	id          string
	mapID       string
	county      string
	roleID      string
	playerLevel int
	towerFloor  int
	towerStage  int
	expires     time.Time
	monsters    []battleMonster

	mu                sync.Mutex
	dead              map[int]struct{}
	entered           bool
	closed            bool
	world             *battleField
	skills            []battleSkill
	nextCast          []time.Time
	activePeer        *Peer
	admitted          bool
	suppliesDelivered bool
	supplies          dungeonSupplies
	elementResistance [6]int
	meridianDefense   int64
}

func (s *Server) createDungeon(role Record, mapID string) *battleDungeon {
	if mapID == "" {
		mapID = "1"
	}
	now := time.Now()
	loadout := battleLoadoutFor(role)
	d := &battleDungeon{
		id:              "d_" + newGameID(16),
		mapID:           mapID,
		county:          fieldCounty(mapID, roleString(role, "MapId")),
		roleID:          roleString(role, "Id"),
		playerLevel:     int(max(1, roleInt(role, "Level"))),
		expires:         now.Add(time.Duration(s.config.BattleDurationSeconds) * time.Second),
		monsters:        s.createMonsters(role, mapID),
		dead:            make(map[int]struct{}),
		skills:          loadout.inBattle(),
		meridianDefense: battleMeridianBPS(role, "FiveElementDefenseUp"),
	}
	for element := 1; element <= 5; element++ {
		d.elementResistance[element] = methodAffixPercent(role, 300+element)
	}
	if stage := towerStageForMap(mapID); stage > 0 {
		d.towerStage, d.towerFloor = stage, towerNextFloor(role)
		d.monsters = s.towerMonsters(d.towerFloor)
	}
	state := &s.battleState
	state.mu.Lock()
	defer state.mu.Unlock()
	if state.dungeons == nil {
		state.dungeons = make(map[string]*battleDungeon)
	}
	for id, other := range state.dungeons {
		if now.After(other.expires) {
			delete(state.dungeons, id)
		}
	}
	state.dungeons[d.id] = d
	return d
}

func (s *Server) dungeonForRole(id, roleID string, findRecent bool) *battleDungeon {
	state := &s.battleState
	state.mu.Lock()
	defer state.mu.Unlock()
	now := time.Now()
	d := state.dungeons[id]
	if d != nil && d.roleID == roleID && now.Before(d.expires) {
		return d
	}
	if !findRecent || roleID == "" {
		return nil
	}
	var recent *battleDungeon
	for _, candidate := range state.dungeons {
		if candidate.roleID == roleID && now.Before(candidate.expires) &&
			(recent == nil || candidate.expires.After(recent.expires)) {
			recent = candidate
		}
	}
	return recent
}

func (s *Server) closeDungeon(id, roleID string, peer *Peer) int {
	if roleID == "" || id == "" {
		return 4503
	}
	state := &s.battleState
	state.mu.Lock()
	d := state.dungeons[id]
	state.mu.Unlock()
	if d == nil {
		return 4537
	} // Repeated leave after an acknowledgement.
	if d.roleID != roleID {
		return 4503
	}
	d.mu.Lock()
	defer d.mu.Unlock()
	if d.closed {
		return 4537
	}
	if peer != nil && d.activePeer != peer {
		return 4503
	}
	// The return-to-town window sends 1004037 on the game socket. The final
	// 8001008 is ordered after combat messages on the battle socket.
	if peer == nil && d.world != nil {
		return 4537
	}
	if err := s.roles.UpdateProgress(roleID, func(role Record) (bool, error) {
		loot, _ := role["BattleLoot"].([]any)
		keep := slices.DeleteFunc(loot, func(entry any) bool {
			item, _ := entry.(Record)
			source := roleString(item, "DungeonId")
			return source == id || source == ""
		})
		if len(keep) == len(loot) {
			return false, nil
		}
		role["BattleLoot"] = keep
		return true, nil
	}); err != nil {
		log.Printf("battle leave save: %v", err)
		return 5000
	}
	d.closed = true
	state.mu.Lock()
	if state.dungeons[id] == d {
		delete(state.dungeons, id)
	}
	state.mu.Unlock()
	return 4537
}

func (s *Server) handleDungeonGame(peer *Peer, msg Message) bool {
	switch msg.Cmd {
	case 1010001:
		s.gameBuildDungeon(peer, msg)
	case 1010002:
		s.gameJoinDungeon(peer, msg)
	case 1004037:
		req, err := s.decode("BattleEndLeaveReq_1004037", msg.Data)
		code := 4503
		if err == nil {
			code = s.closeDungeon(getString(req, "BattleId"), peer.roleID, nil)
		}
		_ = peer.send(msg.Cmd, msg.Msid, s.gameResponse("BattleEndLeaveResp_1004037", code == 4537, code))
	case 1010004, 1004036:
		// The separate sweep screen requires a recorded sweep entitlement and a
		// settlement inventory. A generic empty success would reset the UI wallet.
		s.sendFallback(peer, msg, false, previewDeniedCode)
	case 1004026:
		s.gamePickUpGoods(peer, msg)
	default:
		return false
	}
	return true
}

func (s *Server) gameBuildDungeon(peer *Peer, msg Message) {
	req, err := s.decode("BuildDungeonReq_1010001", msg.Data)
	if err != nil {
		return
	}
	resp := s.pb("BuildDungeonResp_1010001")
	role := s.roles.Get(peer.roleID)
	if role == nil {
		statusPB(resp, false, 4503)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}

	mapID := getString(req, "MapId")
	if getInt(req, "MapType") == 63 {
		stage := voidRealmStage(role)
		if stage < 3 {
			statusPB(resp, false, 4008)
			_ = peer.send(msg.Cmd, msg.Msid, resp)
			return
		}
		mapID = towerMapID(stage)
	} else if towerStageForMap(mapID) > 0 {
		statusPB(resp, false, 4008)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	questDone := false
	err = s.roles.Update(peer.roleID, func(current Record) error {
		questDone = s.grantQuestEvent(current, "DUNGEON")
		return nil
	})
	if err != nil {
		log.Printf("build dungeon save: %v", err)
		statusPB(resp, false, 4503)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	role["Id"] = peer.roleID
	d := s.createDungeon(role, mapID)
	statusPB(resp, true, 4500)
	if d.towerFloor > 0 {
		resp.SetUnknown(offerNumber(resp.GetUnknown(), 30, uint64(d.towerFloor)))
	}
	putPB(resp, map[string]any{
		"DungeonId": d.id, "MapId": d.mapID, "Address": s.config.PublicIP,
		"Port": s.config.BattlePort, "CurrentPower": roleInt(role, "CurrentPower"),
	})
	_ = peer.send(msg.Cmd, msg.Msid, resp)
	if questDone {
		_ = peer.send(1016001, 0, nil)
	}
}

func (s *Server) gameJoinDungeon(peer *Peer, msg Message) {
	req, err := s.decode("RoleJoinDungeonReq_1010002", msg.Data)
	if err != nil {
		return
	}
	resp := s.pb("RoleJoinDungeonResp_1010002")
	role := s.roles.Get(peer.roleID)
	if role == nil {
		statusPB(resp, false, 4503)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	id := getString(req, "DungeonId")
	d := s.dungeonForRole(id, peer.roleID, false)
	if d == nil && (id != "" || towerStageForMap(getString(req, "MapId")) > 0) {
		statusPB(resp, false, 4503)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	if d == nil {
		role["Id"] = peer.roleID
		d = s.createDungeon(role, getString(req, "MapId"))
	}
	stats := equippedBattleStats(role)
	perception, physique := stats.perception, stats.defense
	supplies, code := s.admitDungeon(d)
	if code != 4502 {
		statusPB(resp, false, code)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	level := max(1, roleInt(role, "Level"))
	statusPB(resp, true, 4502)
	if supplies.ward > 0 {
		resp.SetUnknown(offerNumber(resp.GetUnknown(), 29, uint64(supplies.ward)))
	}
	balance := s.roles.project(peer.roleID, []string{"CurrentVigor"})
	resp.SetUnknown(offerNumber(resp.GetUnknown(), 30, uint64(roleInt(balance, "CurrentVigor"))))
	// Parts per million of max battle blood per second while out of combat.
	resp.SetUnknown(offerNumber(resp.GetUnknown(), 31, uint64(scaleForgeStat(500, battleMeridianBPS(role, "PureBloodRecoveryUp")))))
	if supplies.hastePercent > 0 {
		resp.SetUnknown(offerNumber(resp.GetUnknown(), 32, uint64(supplies.hastePercent)))
	}
	if supplies.methodPercent > 0 {
		resp.SetUnknown(offerNumber(resp.GetUnknown(), 33, uint64(supplies.methodPercent)))
	}
	putPB(resp, map[string]any{
		"DungeonId": d.id, "RoleId": peer.roleID, "Name": roleString(role, "Name"), "Level": level,
		"CurrentPhysiqueDefense": physique, "CurrentPureBloodDefense": stats.blood, "CurrentSuperAir": perception * 2,
		"CurrentPerception": min(math.MaxInt32, perception), "CurrentMoveSpeed": supplies.moveSpeed,
		"PhysiqueFiveElement": primaryGameElement(role), "SuperAirFiveElement": primaryGameElement(role), "PerceptionFiveElement": primaryGameElement(role), "Icon": 1,
		"SuperAirRecoverValue": float64(perception) * .05, "DefenseRecoverValue": float64(physique) * .05,
		"SuperAirRecoverValueOnBattle": float64(perception) * .02, "DefenseRecoverValueOnBattle": float64(physique) * .02,
		"GoldDamageAvoidance": 1.0, "WoodDamageAvoidance": 1.0, "WaterDamageAvoidance": 1.0,
		"FireDamageAvoidance": 1.0, "EarthDamageAvoidance": 1.0, "AttackDistance": 6.0,
	})
	for index, skill := range d.skills {
		slot := addMessage(resp, "Skills")
		if skill.id == "" {
			continue
		}
		resolved := resolvePlayerSpell(role, stats.attack, skill)
		setBattleSkillFields(slot, resolved.skill, resolved.damage, int(level), index+1, 1, time.Millisecond)
		putPB(slot, Record{"ForgeResonancePercent": resolved.resonance, "PracticeAffix": resolved.affix, "ConversionRate": resolved.conversion})
		if skill.kind == 2 {
			// The flying artifact debits mana for each launch and reserves perception while active.
			putPB(slot, Record{"UsedSuperAir": max(1, perception/20), "UsedPerception": max(1, perception/10)})
			for i, field := range battleElementBonusFields {
				set(slot, field, 1+float64(methodAffixPercent(role, 106+i))/100)
			}
		}
	}
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}

func (s *Server) handleBattle(peer *Peer, msg Message) {
	switch msg.Cmd {
	case 8001001:
		s.battleEnter(peer, msg)
	case 8001005:
		s.battleUseSkill(peer, msg)
	case 8001007:
		resp := s.pb("RolePickGoodsResp_8001007")
		statusPB(resp, true, 4526)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
	case 8001009:
		s.towerNextBattleFloor(peer, msg)
	case 8001008:
		s.battleLeave(peer, msg)
	case 8002003:
		s.battleMonsterDead(peer, msg)
	case 8001004:
		s.battleMove(peer, msg)
	case 8001006, 8001014, 9000000:
		// The original combat runtime simulates damage locally.
	}
}

func (s *Server) battleEnter(peer *Peer, msg Message) {
	req, err := s.decode("BattleRoleEnterReq_8001001", msg.Data)
	if err != nil {
		return
	}
	roleID := getString(req, "RoleId")
	version := getInt(req, "WorldVersion")
	id := getString(req, "DungeonId")
	d := s.dungeonForRole(id, roleID, version == 0 && id == "")
	if d == nil && version == 0 && id == "" {
		if role := s.roles.Get(roleID); role != nil {
			role["Id"] = roleID
			d = s.createDungeon(role, "1")
		}
	}
	resp := s.pb("BattleRoleEnterResp_8001001")
	reconnect := false
	if d != nil {
		d.mu.Lock()
		reconnect = d.entered
		d.mu.Unlock()
	}
	code := 4503
	if d != nil {
		code = s.enterField(d, version)
	}
	if code != 4506 {
		statusPB(resp, false, code)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	peer.roleID, peer.dungeonID = roleID, d.id
	statusPB(resp, true, 4506)
	d.mu.Lock()
	d.activePeer = peer
	putPB(resp, map[string]any{
		"MapMinX": -10.0, "MapMaxX": 10.0, "MapMinY": -2.0, "MapMaxY": 7.0,
		"PosY": -2.0, "Angle": 90.0, "RoleIndex": 1,
		"OverTime": max(60000, int64(s.config.BattleDurationSeconds)*1000),
	})
	if w := d.world; w != nil {
		putPB(resp, Record{"MapMinX": -float64(w.width), "MapMaxX": float64(w.width), "MapMinY": -float64(w.height), "MapMaxY": float64(w.height),
			"PosX": w.playerX, "PosY": w.playerY, "WorldTiles": w.tiles, "RegionId": w.region, "Explored": slices.Clone(w.explored), "Landmarks": w.landmarks,
			"OverTime": max(int64(1), time.Until(d.expires).Milliseconds())})
		for id, node := range w.nodes {
			if node.collectedID != "" {
				continue
			}
			fieldName := "Plants"
			if node.item.kind == 13 {
				fieldName = "Minerals"
			}
			putPB(addMessage(resp, fieldName), Record{"ObjectIndex": node.index, "Name": node.item.name, "GoodsId": id, "ObjectType": node.item.kind,
				"Rank": node.item.rank, "FiveElement": node.item.element, "Level": 1, "PosX": node.x, "PosY": node.y, "Count": node.count})
		}
	}
	for _, monster := range d.monsters {
		if _, dead := d.dead[monster.index]; d.world != nil && dead {
			continue
		}
		d.putMonster(addMessage(resp, "Objects"), monster)
	}
	d.mu.Unlock()
	if peer.send(msg.Cmd, msg.Msid, resp) == nil && version == 1 && reconnect {
		s.replayFieldDrops(peer, d.id)
	}
}

func (s *Server) battleUseSkill(peer *Peer, msg Message) {
	code := 4503
	if d := s.dungeonForRole(peer.dungeonID, peer.roleID, false); d != nil {
		d.mu.Lock()
		if !d.closed && d.activePeer == peer {
			req, err := s.decode("BattleRoleUseSkillReq_8001005", msg.Data)
			if err == nil && getInt(req, "RoleIndex") == 1 {
				code = d.castFieldSkill(int(getInt(req, "SkillIndex")), int(getInt(req, "TargetObjectIndex")), time.Now())
			}
		}
		d.mu.Unlock()
	}
	resp := s.pb("BattleRoleUseSkillResp_8001005")
	statusPB(resp, code == 4534, code)
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}

func (d *battleDungeon) castFieldSkill(index, target int, now time.Time) int {
	if index < 1 || index > len(d.skills) || d.skills[index-1].id == "" {
		return 4516
	}
	slot := index - 1
	skill := d.skills[slot]
	if (skill.skillType == 40 || skill.skillType == 41) && target == 0 {
		return 4535
	}
	// Haste targets the caster even if their selected enemy has just died.
	if target != 0 && skill.skillType != 50 {
		if len(d.monsters) == 0 || target < d.monsters[0].index || target-d.monsters[0].index >= len(d.monsters) {
			return 4535
		}
		if _, dead := d.dead[target]; dead {
			return 4535
		}
	}
	if skill.kind != 1 {
		return 4534
	} // Flying artifacts are toggles.
	if now.Before(d.nextCast[slot]) {
		return 4535
	}
	d.nextCast[slot] = now.Add(time.Duration(skill.cooldownS) * time.Second)
	return 4534
}

func (s *Server) battleMove(peer *Peer, msg Message) {
	d := s.dungeonForRole(peer.dungeonID, peer.roleID, false)
	if d == nil {
		return
	}
	d.mu.Lock()
	defer d.mu.Unlock()
	if d.world == nil || d.closed || d.activePeer != peer {
		return
	}
	req, err := s.decode("BattleRoleMoveReq_8001004", msg.Data)
	if err == nil && getInt(req, "RoleIndex") == 1 {
		d.world.move(getFloat(req, "PosX"), getFloat(req, "PosY"))
	}
}

func (s *Server) acceptDungeonEntry(d *battleDungeon) bool {
	return s.enterField(d, 0) == 4506
}

func (s *Server) enterField(d *battleDungeon, version int64) int {
	d.mu.Lock()
	defer d.mu.Unlock()
	if d.closed || !time.Now().Before(d.expires) {
		return 4503
	}
	if version != 0 && version != 1 {
		return 4503
	}
	if d.entered {
		if (d.world != nil) == (version == 1) {
			return 4506
		}
		return 4503
	}
	var eco *countyEcology
	region := d.county
	if version == 1 {
		state := &s.battleState
		state.ecologyOnce.Do(func() {
			var err error
			state.ecology, err = loadBattleEcology(s.dataDir)
			if err != nil {
				log.Printf("battle ecology: %v", err)
			}
		})
		eco = state.ecology[region]
		if eco == nil {
			return 4503
		}
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(d.roleID)
		if role == nil {
			return ErrRecordNotFound
		}
		if !d.admitted {
			if err := spendDungeonAdmission(tx.Get(d.roleID), d.mapID); err != nil {
				return err
			}
		}
		return tx.RecordHistory(newHistoryEvent("dungeon.entered", role, nil, "dungeon", d.mapID, Record{
			"dungeonId": d.id, "mapId": d.mapID, "mapName": gameMapName(d.mapID),
		}))
	})
	if err != nil {
		return dungeonEntryErrorCode(err)
	}
	d.admitted = true
	if eco != nil {
		rng := dungeonRand(d.id)
		d.world = generateBattleField(region, len(d.monsters), rng)
		d.world.populate(d.id, d.monsters, eco, rng)
		if d.towerFloor > 0 {
			clear(d.world.nodes)
		}
	}
	d.nextCast = make([]time.Time, len(d.skills))
	d.entered = true
	return 4506
}

func (s *Server) battleLeave(peer *Peer, msg Message) {
	req, err := s.decode("RoleLeaveDungeonReq_8001008", msg.Data)
	code := 4503
	if err == nil && (getString(req, "DungeonId") == "" || getString(req, "DungeonId") == peer.dungeonID) {
		code = s.closeDungeon(peer.dungeonID, peer.roleID, peer)
	}
	// Retain the closed ID on this socket so a lost leave reply can be retried.
	// A subsequent successful entry binds the socket to its new dungeon.
	_ = peer.send(msg.Cmd, msg.Msid, s.gameResponse("RoleLeaveDungeonResp_8001008", code == 4537, code))
}

func (d *battleDungeon) putMonster(object protoreflect.Message, monster battleMonster) {
	putPB(object, map[string]any{
		"ObjectIndex": monster.index, "PosX": monster.x, "PosY": monster.y,
		"FiveElement": monster.element, "ObjectType": monster.objectType, "Name": monster.name,
		"Level": monster.level, "CurrentPhysiqueDefense": min(math.MaxInt32, monster.armor),
		"CurrentPureBloodDefense": min(math.MaxInt32, monster.hp), "CurrentSuperAir": min(math.MaxInt32, monster.mana),
		"CurrentPerception": monster.perception, "CurrentMoveSpeed": monster.speed, "Icon": monster.icon,
		"SuperAirRecoverValue": float64(monster.mana) * .01, "DefenseRecoverValue": float64(monster.armor) * .01,
		"SuperAirRecoverValueOnBattle": float64(monster.mana) * .005,
		"DefenseRecoverValueOnBattle":  float64(monster.armor) * .005,
		"SuperAirPower":                float64(monster.attack), "AttackDistance": 5.0, "UnusualLevel": monster.unusual,
	})
	// Animal AI passes the wire value directly to its seconds-based timer.
	// Human spell AI and the player HUD divide the wire value by 1000.
	unit := time.Second
	if monster.objectType == 2001 {
		unit = time.Millisecond
	}
	for _, skillType := range monster.skillTypes() {
		skill := monster.skill(skillType)
		slot := addMessage(object, "Skills")
		damage := monster.skillDamage(skill, d.elementResistance, d.meridianDefense)
		setBattleSkillFields(slot, skill, damage, monster.level, skillType, monster.unusual, unit)
	}
}
