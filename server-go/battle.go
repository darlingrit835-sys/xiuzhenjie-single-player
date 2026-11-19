package main

import (
	"log"
	"math"
	"sync"
	"time"
)

type battleState struct {
	mu       sync.Mutex
	dungeons map[string]*battleDungeon
	loadOnce sync.Once
	balance  monsterConfiguration
}

type battleDungeon struct {
	id          string
	mapID       string
	roleID      string
	playerLevel int
	expires     time.Time
	monsters    []battleMonster

	mu      sync.Mutex
	dead    map[int]struct{}
	entered bool
	closed  bool
}

func (s *Server) createDungeon(role Record, mapID string) *battleDungeon {
	if mapID == "" {
		mapID = "1"
	}
	now := time.Now()
	d := &battleDungeon{
		id:          "d_" + newGameID(16),
		mapID:       mapID,
		roleID:      roleString(role, "Id"),
		playerLevel: int(max(1, roleInt(role, "Level"))),
		expires:     now.Add(time.Duration(s.config.BattleDurationSeconds) * time.Second),
		monsters:    s.createMonsters(role, mapID),
		dead:        make(map[int]struct{}),
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

func (s *Server) closeDungeon(id, roleID string) {
	state := &s.battleState
	state.mu.Lock()
	d := state.dungeons[id]
	if d == nil || d.roleID != roleID {
		state.mu.Unlock()
		return
	}
	delete(state.dungeons, id)
	state.mu.Unlock()

	d.mu.Lock()
	d.closed = true
	d.mu.Unlock()
}

func (s *Server) handleDungeonGame(peer *Peer, msg Message) bool {
	switch msg.Cmd {
	case 1010001:
		s.gameBuildDungeon(peer, msg)
	case 1010002:
		s.gameJoinDungeon(peer, msg)
	case 1004037:
		req, err := s.decode("BattleEndLeaveReq_1004037", msg.Data)
		if err == nil {
			s.closeDungeon(getString(req, "BattleId"), peer.roleID)
			resp := s.pb("BattleEndLeaveResp_1004037")
			statusPB(resp, true, 4537)
			_ = peer.send(msg.Cmd, msg.Msid, resp)
		}
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
	d := s.createDungeon(role, getString(req, "MapId"))
	statusPB(resp, true, 4500)
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
	d := s.dungeonForRole(getString(req, "DungeonId"), peer.roleID, false)
	if d == nil {
		d = s.createDungeon(role, getString(req, "MapId"))
	}
	attackBonus, defenseBonus := battleEquipmentBonuses(role)
	perception := max(100, roleInt(role, "Perception"))
	physique := max(100, roleInt(role, "Physique")+defenseBonus)
	hp := min(math.MaxInt32, max(1000, perception*4))
	level := max(1, roleInt(role, "Level"))
	statusPB(resp, true, 4502)
	putPB(resp, map[string]any{
		"DungeonId": d.id, "RoleId": peer.roleID, "Name": roleString(role, "Name"), "Level": level,
		"CurrentPhysiqueDefense": physique, "CurrentPureBloodDefense": hp, "CurrentSuperAir": perception * 2,
		"CurrentPerception": min(math.MaxInt32, perception), "CurrentMoveSpeed": 5.0,
		"PhysiqueFiveElement": 1, "SuperAirFiveElement": 1, "PerceptionFiveElement": 1, "Icon": 1,
		"SuperAirRecoverValue": float64(perception) * .05, "DefenseRecoverValue": float64(physique) * .05,
		"SuperAirRecoverValueOnBattle": float64(perception) * .02, "DefenseRecoverValueOnBattle": float64(physique) * .02,
		"GoldDamageAvoidance": 1.0, "WoodDamageAvoidance": 1.0, "WaterDamageAvoidance": 1.0,
		"FireDamageAvoidance": 1.0, "EarthDamageAvoidance": 1.0, "AttackDistance": 6.0,
	})
	attack := max(10, math.Round(float64(perception)*.8)+float64(attackBonus))
	for index, skill := range battleSkillsFor(role) {
		damage := attack * skill.multiplier
		if skill.skillType == 30 || skill.skillType == 31 {
			damage = 1
		}
		slot := addMessage(resp, "Skills")
		setBattleSkillFields(slot, skill, damage, int(level), index+1, 1)
	}
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}

func (s *Server) handleBattle(peer *Peer, msg Message) {
	switch msg.Cmd {
	case 8001001:
		s.battleEnter(peer, msg)
	case 8001005:
		resp := s.pb("BattleRoleUseSkillResp_8001005")
		statusPB(resp, true, 4534)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
	case 8001007:
		resp := s.pb("RolePickGoodsResp_8001007")
		statusPB(resp, true, 4526)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
	case 8001008:
		s.battleLeave(peer, msg)
	case 8002003:
		s.battleMonsterDead(peer, msg)
	case 8001004, 8001006, 8001014, 9000000:
		// 移动、伤害和心跳由客户端侧处理，沿用 Java 战斗服的无回包行为。
	}
}

func (s *Server) battleEnter(peer *Peer, msg Message) {
	req, err := s.decode("BattleRoleEnterReq_8001001", msg.Data)
	if err != nil {
		return
	}
	roleID := getString(req, "RoleId")
	d := s.dungeonForRole(getString(req, "DungeonId"), roleID, true)
	if d == nil {
		if role := s.roles.Get(roleID); role != nil {
			d = s.createDungeon(role, "1")
		}
	}
	resp := s.pb("BattleRoleEnterResp_8001001")
	if d == nil || !s.acceptDungeonEntry(d) {
		statusPB(resp, false, 4503)
		_ = peer.send(msg.Cmd, msg.Msid, resp)
		return
	}
	peer.roleID, peer.dungeonID = roleID, d.id
	statusPB(resp, true, 4506)
	putPB(resp, map[string]any{
		"MapMinX": -10.0, "MapMaxX": 10.0, "MapMinY": -2.0, "MapMaxY": 7.0,
		"PosY": -2.0, "Angle": 90.0, "RoleIndex": 1,
		"OverTime": max(60000, int64(s.config.BattleDurationSeconds)*1000),
	})
	for _, monster := range d.monsters {
		object := addMessage(resp, "Objects")
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
		for _, skillType := range monster.skillTypes() {
			skill := monster.skill(skillType)
			slot := addMessage(object, "Skills")
			damage := max(1, math.Round(float64(monster.attack)*skill.multiplier))
			setBattleSkillFields(slot, skill, damage, monster.level, skillType, monster.unusual)
		}
	}
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}

func (s *Server) acceptDungeonEntry(d *battleDungeon) bool {
	d.mu.Lock()
	defer d.mu.Unlock()
	if d.closed || !time.Now().Before(d.expires) {
		return false
	}
	if d.entered {
		return true
	}
	err := s.roles.Transact(func(tx *StoreTx) error {
		role := tx.View(d.roleID)
		if role == nil {
			return ErrRecordNotFound
		}
		return tx.RecordHistory(newHistoryEvent("dungeon.entered", role, nil, "dungeon", d.mapID, Record{
			"dungeonId": d.id, "mapId": d.mapID, "mapName": gameMapName(d.mapID),
		}))
	})
	if err != nil {
		log.Printf("battle entry save: %v", err)
		return false
	}
	d.entered = true
	return true
}

func (s *Server) battleLeave(peer *Peer, msg Message) {
	s.closeDungeon(peer.dungeonID, peer.roleID)
	if peer.roleID != "" {
		err := s.roles.Update(peer.roleID, func(role Record) error {
			role["BattleLoot"] = []any{}
			return nil
		})
		if err != nil {
			log.Printf("battle leave save: %v", err)
		}
	}
	peer.roleID, peer.dungeonID = "", ""
	resp := s.pb("RoleLeaveDungeonResp_8001008")
	statusPB(resp, true, 4537)
	_ = peer.send(msg.Cmd, msg.Msid, resp)
}
