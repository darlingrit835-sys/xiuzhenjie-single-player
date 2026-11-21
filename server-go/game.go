package main

import (
	"crypto/rand"
	"encoding/hex"
	"encoding/json"
	"errors"
	"fmt"
	"log"
	"math"
	"strconv"
	"strings"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
	"google.golang.org/protobuf/types/dynamicpb"
)

func roleString(role Record, key string) string {
	value := role[key]
	if value == nil {
		return ""
	}
	if text, ok := value.(string); ok {
		return text
	}
	return fmt.Sprint(value)
}

func roleInt(role Record, key string) int64 {
	switch n := role[key].(type) {
	case int:
		return int64(n)
	case int32:
		return int64(n)
	case int64:
		return n
	case float64:
		return int64(n)
	case json.Number:
		if value, err := n.Int64(); err == nil {
			return value
		}
	}
	return int64(roleFloat(role, key))
}

func roleFloat(role Record, key string) float64 {
	switch n := role[key].(type) {
	case float64:
		return n
	case int:
		return float64(n)
	case int32:
		return float64(n)
	case int64:
		return float64(n)
	case json.Number:
		value, _ := n.Float64()
		return value
	case string:
		value, _ := strconv.ParseFloat(n, 64)
		return value
	}
	return 0
}

func roleBool(role Record, key string) bool {
	if value, ok := role[key].(bool); ok {
		return value
	}
	return strings.EqualFold(roleString(role, key), "true")
}

func newGameID(size int) string {
	var value [16]byte
	if _, err := rand.Read(value[:]); err != nil {
		panic(err) // crypto/rand failure prevents any safe ID allocation.
	}
	return hex.EncodeToString(value[:size])
}

func statusPB(m protoreflect.Message, success bool, code int) {
	set(m, "IsSuccess", success)
	set(m, "Code", code)
}

func putPB(m protoreflect.Message, fields Record) {
	for field, value := range fields {
		set(m, field, value)
	}
}

func copyRolePB(m protoreflect.Message, r Record, fields ...string) {
	for _, field := range fields {
		desc := m.Descriptor().Fields().ByName(protoreflect.Name(field))
		if desc == nil || r[field] == nil {
			continue
		}
		switch desc.Kind() {
		case protoreflect.StringKind:
			set(m, field, roleString(r, field))
		case protoreflect.BoolKind:
			set(m, field, roleBool(r, field))
		case protoreflect.DoubleKind, protoreflect.FloatKind:
			set(m, field, roleFloat(r, field))
		default:
			set(m, field, roleInt(r, field))
		}
	}
}

func (s *Server) gameResponse(name string, success bool, code int) *dynamicpb.Message {
	response := s.pb(name)
	statusPB(response, success, code)
	return response
}

func (s *Server) sendGame(peer *Peer, msg Message, body proto.Message) {
	if err := peer.send(msg.Cmd, msg.Msid, body); err != nil {
		log.Printf("回包 cmd=%d msid=%d: %v", msg.Cmd, msg.Msid, err)
	}
}

func (s *Server) gameNotice(peer *Peer, text string) {
	response := s.pb("BroadcastResp_1001014")
	set(response, "Msg", text)
	s.sendGame(peer, Message{Cmd: 1001014}, response)
}

func (s *Server) gameQuestComplete(peer *Peer) {
	s.sendGame(peer, Message{Cmd: 1016001}, nil)
}

func (s *Server) handleGame(peer *Peer, msg Message) {
	switch msg.Cmd {
	case 1005005, 1005007, 1005021, 1005022, 1005023:
		s.handleChamberStorage(peer, msg)
	case caveMatrixCommand:
		s.handleCaveMatrix(peer, msg)
	case voidFacilityCommand:
		s.handleVoidFacility(peer, msg)
	case 1008001, 1008002, 1008004:
		s.handleTowerRecord(peer, msg)
	case voidCommand:
		s.handleVoidRealm(peer, msg)
	case concordCommand:
		s.handleConcordDesk(peer, msg)
	case gardenCommand:
		s.handleGarden(peer, msg)
	case talismanCommand:
		s.handleTalisman(peer, msg)
	case alchemyCommand:
		s.handleAlchemy(peer, msg)
	case fixedPurchaseHistoryCommand:
		s.handleFixedPurchaseHistory(peer, msg)
	case scheduledAuctionCommand:
		s.handleScheduledAuction(peer, msg)
	case guildCourtRead, guildCourtAct, guildCharterRead, guildCharterAct:
		s.handleSectCourt(peer, msg)
	case smithingCatalogCmd, smithingCreateCmd, smithingSupplyCmd, smithingTemplateCmd, smithingPlanCmd, smithingCastPlanCmd, smithingCastCmd, smithingSampleCmd:
		s.handleSmithing(peer, msg)
	case privateChatHistory, privateChatSend:
		s.handlePrivateChat(peer, msg)
	case worldChatJoin, worldChatSend, worldChatLeave:
		s.handleWorldChat(peer, msg)
	case friendOfferCreate, friendOfferList, friendOfferDecide, friendOfferAck:
		s.handleFriendOffers(peer, msg)
	case 9000000:
		return
	case 1008066:
		if s.preview != nil && s.handlePreview(peer, msg) {
			return
		}
		out := s.gameResponse("EnterEquipmentAssociationResp_1008066", true, 2000)
		if peer.currentRoleID() == "" {
			statusPB(out, false, 4007)
		} else {
			s.smithingAssociation(peer, nil, out)
		}
		s.sendGame(peer, msg, out)
	case 1005038:
		s.openSpellPractice(peer, msg)
	case 1005040:
		s.practiceSpell(peer, msg)
	case 1002001:
		s.createGameRole(peer, msg)
	case 1002002:
		s.loginGameRole(peer, msg)
	case 1002003, 1002004:
		s.offlineGameRole(peer, msg)
	case 1002005:
		s.checkGameName(peer, msg)
	default:
		if s.handleDungeonGame(peer, msg) || s.handleMap(peer, msg) ||
			s.handleInventory(peer, msg) || s.handleCultivation(peer, msg) || s.handleQuest(peer, msg) {
			return
		}
		if s.preview != nil && s.handlePreview(peer, msg) {
			return
		}
		s.unknownGame(peer, msg)
	}
}

func (s *Server) closeGame(peer *Peer) {
	if id := peer.currentRoleID(); id != "" {
		if err := s.roles.Update(id, func(r Record) error {
			r["LastOnline"] = time.Now().UnixMilli()
			return nil
		}); err != nil && !errors.Is(err, ErrRecordNotFound) {
			log.Printf("退出角色存档失败: %v", err)
		}
	}
}

func (s *Server) createGameRole(peer *Peer, msg Message) {
	req, err := s.decode("RoleCreateReq_1002001", msg.Data)
	if err != nil {
		return
	}
	name := getString(req, "Name")
	b := s.gameResponse("RoleCreateResp_1002001", false, 3002)
	if name != "" {
		role := s.newRole(req)
		id := roleString(role, "Id")
		err := s.roles.InsertUnique(id, role, func(existing Record) bool {
			return roleString(existing, "Name") == name
		}, newHistoryEvent("role.created", role, nil, "", "", nil))
		switch {
		case err == nil:
			statusPB(b, true, 2000)
			set(b, "RoleId", id)
		case errors.Is(err, ErrRecordExists):
		default:
			log.Printf("建角写入失败: %v", err)
			set(b, "Code", 5000)
		}
	}
	s.sendGame(peer, msg, b)
}

func (s *Server) checkGameName(peer *Peer, msg Message) {
	req, err := s.decode("CheckNameReq_1002005", msg.Data)
	if err != nil {
		return
	}
	name := getString(req, "Name")
	b := s.gameResponse("CheckNameResp_1002005", true, 2000)
	if s.roles.Find(func(role Record) bool { return roleString(role, "Name") == name }) != nil {
		statusPB(b, false, 3002)
	}
	s.sendGame(peer, msg, b)
}

func (s *Server) loginGameRole(peer *Peer, msg Message) {
	req, err := s.decode("RoleLoginReq_1002002", msg.Data)
	if err != nil {
		return
	}
	id := getString(req, "RoleId")
	b := s.gameResponse("RoleLoginResp_1002002", false, 3001)
	if reservedStoreID(id) {
		s.sendGame(peer, msg, b)
		return
	}
	notice := false
	err = s.roles.Transact(func(tx *StoreTx) error {
		r := tx.Get(id)
		if r == nil {
			return ErrRecordNotFound
		}
		if s.accountSessionsRequired() && !s.verifyPreviewSession(getString(req, "GameServerIpManageId"), roleString(r, "UserId"), time.Now()) {
			return previewReject(previewDeniedCode)
		}
		if s.previewWrites {
			syncTrialSignInLogin(r, time.Now())
		}
		s.ensureGameRole(r)
		resumePracticeGuide(r)
		s.checkQuest(r)
		s.applyOffline(r)
		_, achievementsReady := achievementSummary(r)
		r["TianShuBeiStatus"] = 0
		if achievementsReady {
			r["TianShuBeiStatus"] = achievementReady
		}
		m := b.ProtoReflect()
		// RoleLoginResp fields deliberately share the Java JSON keys except for TianDaoLing.
		fields := m.Descriptor().Fields()
		for i := 0; i < fields.Len(); i++ {
			field := fields.Get(i)
			if !field.IsList() && !field.IsMap() && field.Message() == nil {
				copyRolePB(m, r, string(field.Name()))
			}
		}
		if name := gameCityName(roleString(r, "MapId")); name != "" {
			set(b, "MapName", name)
		}
		fillFriendTransferTitles(b, r)
		if s.previewWrites {
			set(b, "IsNewLetter", mailUnread(r))
		}
		set(b, "IsTianDaoLing", roleBool(r, "TianDaoLing"))
		if s.config.CompleteBattleGuide {
			set(b, "BattleGuideVal", 100)
		}
		if s.config.SkipAcupointGuide {
			set(b, "AcupointGuideVal", 100)
		}
		notice = roleBool(r, "StarterKitNotice")
		if notice {
			r["StarterKitNotice"] = false
		}
		if tx.store.historyEnabled && !roleBool(r, "_historyLoginRecorded") {
			event := newHistoryEvent("role.login", r, nil, "", "", nil)
			event.Actor.RoleID = id
			if err := tx.RecordHistory(event); err != nil {
				return err
			}
			r["_historyLoginRecorded"] = true
		}
		return nil
	})
	if err == nil {
		if s.previewWrites {
			s.fillGuildLogin(id, b)
			markFriendOffersAvailable(b.ProtoReflect())
			s.expireFriendLocks(id)
		}
		s.bindGameRole(peer, id)
		statusPB(b, true, 2000)
	} else if errors.Is(err, previewReject(previewDeniedCode)) {
		set(b, "Code", previewDeniedCode)
	} else if !errors.Is(err, ErrRecordNotFound) {
		log.Printf("角色登录写入失败: %v", err)
		b = s.gameResponse("RoleLoginResp_1002002", false, 5000)
	}
	s.sendGame(peer, msg, b)
	if err == nil && notice {
		s.gameNotice(peer, "新手礼包已放入纳戒：启灵经、青锋剑、青布道袍、玄铁护心镜、青布靴、纳气丹×3、回春散×3")
	}
}

func (s *Server) offlineGameRole(peer *Peer, msg Message) {
	r := s.roles.Get(peer.currentRoleID())
	if msg.Cmd == 1002003 {
		b := s.gameResponse("RoleOfflineResp_1002003", r != nil, 4181)
		putPB(b, Record{
			"SuperAir":    roleInt(r, "OfflineSuperAir"),
			"Perception":  roleInt(r, "OfflinePerception"),
			"Physique":    roleInt(r, "OfflinePhysique"),
			"OfflineTime": roleInt(r, "OfflineTime"), "IsFullTime": roleBool(r, "OfflineIsFull"),
			"OfflineVigor": roleInt(r, "OfflineVigor"), "OfflineClever": 0, "Age": roleInt(r, "OfflineAge"),
		})
		s.sendGame(peer, msg, b)
		return
	}
	if _, err := s.decode("RoleGetOfflineReq_1002004", msg.Data); err != nil {
		return
	}
	b := s.gameResponse("RoleGetOfflineResp_1002004", r != nil, 2000)
	copyRolePB(b, r, "SuperAir", "CurrentSuperAir", "Perception", "CurrentPerception",
		"Physique", "CurrentPhysique", "PureBlood", "CurrentPureBlood")
	putPB(b, Record{
		"Age": roleInt(r, "CurrentAge"), "Vigor": roleInt(r, "CurrentVigor"),
		"Clever": roleInt(r, "CurrentClever"), "PoisonVal": roleInt(r, "CurrentPoisonVal"),
		"Power": roleInt(r, "CurrentPower"),
	})
	s.sendGame(peer, msg, b)
}

func (s *Server) newRole(req protoreflect.Message) Record {
	lvl := s.levelInfo(1)
	mapID := getString(req, "MapId")
	mapName := gameCityName(mapID)
	if mapName == "" {
		mapName = "新手村"
	}
	role := Record{
		"Id": newGameID(16), "UserId": getString(req, "UserId"),
		"Name": getString(req, "Name"), "MapId": mapID, "MapName": mapName,
		"Sex": 0, "HeadPortraitId": getInt(req, "HeadPortraitId"),
		"Level": 1, "CurrentAge": 16, "MaxAge": 100, "ShinyStone": 500, "ShinyJade": 10,
		"FlexibleRoot": "0", "SuperAir": realmStartingAir(1, lvl), "Perception": int64(lvl.perception),
		"Physique": int64(lvl.toughTenacity), "PureBlood": max(100, int64(math.Round(lvl.perception*0.01))),
		"RunMask": 4, "RunLocation": 3, "DongFuRank": 0, "JuLingRank": 0, "KongLingRank": 0,
		"CurrentVigor": 100, "CurrentClever": 100, "Vigor": 100, "Clever": 100, "CurrentPoisonVal": 0,
		"LastOnline": time.Now().UnixMilli(), "StatsInitialized": true, "StatsVersion": 2,
		"TaskId": 1, "TaskStatus": 0, "TianShuBeiStatus": 0, "YueKaDays": 0, "Title": 0,
		"IsGetSignIn": false, "SignInDay": 0, "IsNewLetter": false, "LiveRelId": 0,
	}
	if getBool(req, "Sex") {
		role["Sex"] = 1
	}
	var roots []int
	for i, element := range [...]string{"Gold", "Wood", "Water", "Fire", "Earth"} {
		value := getFloat(req, element)
		role[element], role["Has"+element] = value, value > 0
		if value > 0 {
			roots = append(roots, i+1)
		}
	}
	for i := 0; i < 4; i++ {
		key := "Live" + strconv.Itoa(i+1)
		role[key+"Type"], role[key+"Level"], role[key+"Value"] = 0, 0, 0
		if i < len(roots) {
			role[key+"Type"], role[key+"Level"] = roots[i], 1
		}
	}
	s.ensureGameRole(role)
	return role
}

func setRoleDefault(r Record, field string, value any) {
	if r[field] == nil {
		r[field] = value
	}
}

func (s *Server) ensureGameRole(r Record) {
	for _, key := range cultivationRootFields[1:] {
		setRoleDefault(r, key, roleFloat(r, strings.TrimPrefix(key, "Has")) > 0)
	}
	level := max(1, min(90, roleInt(r, "Level")))
	lvl := s.levelInfo(level)
	r["Level"] = level
	if r["AcupointValue"] == nil && level >= 2 {
		r["AcupointValue"] = acupointBalance(r)
	}
	first := !roleBool(r, "StatsInitialized") || roleInt(r, "StatsVersion") < 2
	if r["SuperAir"] == nil || roleInt(r, "SuperAir") < 0 {
		r["SuperAir"] = realmStartingAir(level, lvl)
	}
	base := Record{
		"Perception": int64(lvl.perception), "Physique": int64(lvl.toughTenacity),
		"PureBlood": max(100, int64(math.Round(lvl.perception*0.01))), "MaxAge": gameLifespan(level),
	}
	for key := range base {
		if first || roleInt(r, key) <= 0 {
			r[key] = max(roleInt(r, key), roleInt(base, key))
		}
	}
	if roleInt(r, "CurrentAge") <= 0 {
		r["CurrentAge"] = 16
	}
	current := Record{
		"CurrentSuperAir": roleInt(r, "Perception") * 2, "CurrentPerception": roleInt(r, "Perception"),
		"CurrentPhysique": roleInt(r, "Physique"), "CurrentPureBlood": roleInt(r, "PureBlood"),
		"CurrentClever": 100 + level*5,
	}
	for key, val := range current {
		if first || roleInt(r, key) <= 0 {
			r[key] = val
		}
	}
	r["Vigor"] = max(vigorCapacity(level), roleInt(r, "Vigor"), roleInt(r, "CurrentVigor"))
	setRoleDefault(r, "CurrentVigor", r["Vigor"])
	r["Clever"] = max(100, roleInt(r, "Clever"), roleInt(r, "CurrentClever"))
	if roleInt(r, "HunLiVal") < 500 {
		r["HunLiVal"] = 1000
	}
	if mask := roleInt(r, "RunMask"); mask < 1 || mask > 7 {
		r["RunMask"] = gameRunMask(roleInt(r, "RunLocation"))
	}
	r["RunLocation"] = gameLocation(roleInt(r, "RunMask"))
	for _, key := range []string{"DongFuRank", "JuLingRank", "KongLingRank"} {
		setRoleDefault(r, key, 0)
	}
	for _, key := range []string{"BattleGuideVal", "PracticeGuideVal", "AcupointGuideVal",
		"ShopGuideVal", "SignGuideVal", "ElixirGuideVal"} {
		if roleInt(r, key) <= 0 {
			r[key] = 1
		}
	}
	if roleInt(r, "LastOnline") <= 0 {
		r["LastOnline"] = time.Now().UnixMilli()
	}
	s.ensureGamesMethods(r)
	s.refreshMethodAffixes(r)
	s.refreshBattleMeridians(r)
	s.ensureQuest(r)
	ensureStarterItems(r)
	r["StatsInitialized"], r["StatsVersion"] = true, 2
	recomputeGamePower(r)
}

func recomputeGamePower(r Record) {
	power := battlePower(r)
	r["Power"], r["CurrentPower"] = power, power
}

func (s *Server) applyOffline(r Record) { s.applyOfflineAt(r, time.Now().UnixMilli()) }

func (s *Server) applyOfflineAt(r Record, now int64) {
	seconds := (now - roleInt(r, "LastOnline")) / 1000
	setRoleDefault(r, "VigorRecoveredAt", roleInt(r, "LastOnline")/1000)
	r["OfflineVigor"] = recoverRoleVigor(r, now/1000)
	r["LastReportedVigor"] = roleInt(r, "CurrentVigor")
	r["LastOnline"] = now
	r["OfflineIsFull"], r["OfflineAge"] = false, int64(0)
	r["OfflineTime"], r["OfflineSuperAir"], r["OfflinePerception"], r["OfflinePhysique"] = 0, 0, 0, 0
	if seconds < 60 {
		return
	}
	limitSeconds := int64(s.config.OfflineCapHours) * 3600
	reachedTimeLimit := seconds >= limitSeconds
	seconds = min(seconds, limitSeconds)
	lvl := s.levelInfo(roleInt(r, "Level"))
	mask := roleInt(r, "RunMask")
	var elapsed int64
	allFull := true
	for index, key := range [...]string{"Perception", "Physique", "SuperAir"} {
		if mask&(1<<index) == 0 {
			continue
		}
		rate := s.gameRate(r, index+1) * 0.8
		ceiling := lvl.upgradeLimit
		if index < 2 {
			rate *= .5
			ceiling = float64(lvl.trainingCap(index + 1))
		}
		total := roleInt(r, key)
		active := productiveTrainingSeconds(r, key, ceiling, rate, seconds)
		elapsed = max(elapsed, active)
		applied := min(int64(math.Floor(rate*float64(active))), max(0, int64(ceiling)-total))
		r[key], r["Offline"+key] = total+applied, applied
		if total+applied < int64(ceiling) && !(index == 2 && roleInt(r, "Level") >= 90) {
			allFull = false
		}
		if index < 2 && applied > 0 {
			r["Current"+key] = total + applied
		}
	}
	ageBefore := max(16, roleInt(r, "CurrentAge"))
	ageCultivation(r, elapsed)
	r["OfflineAge"] = max(0, roleInt(r, "CurrentAge")-ageBefore)
	if roleInt(r, "OfflinePhysique") > 0 {
		blood := max(100, roleInt(r, "PureBlood"), int64(math.Round(float64(roleInt(r, "Physique"))*.01)))
		r["PureBlood"], r["CurrentPureBlood"] = blood, blood
	}
	r["OfflineTime"], r["OfflineIsFull"] = seconds, reachedTimeLimit || mask != 0 && allFull
	recomputeGamePower(r)
}
