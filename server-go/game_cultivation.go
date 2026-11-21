package main

import (
	"context"
	"log"
	"math"
	"slices"
	"strconv"
	"time"

	"google.golang.org/protobuf/proto"
	"google.golang.org/protobuf/reflect/protoreflect"
	"google.golang.org/protobuf/types/known/wrapperspb"
)

type homeGrade struct {
	ID    string  `json:"id"`
	Level int     `json:"level"`
	Price float64 `json:"price"`
}

func (s *Server) loadHomeGrades() error {
	var grades []homeGrade
	if err := s.readGameTable("xzjworldhomelist.json", &grades); err != nil {
		return err
	}
	for _, grade := range grades {
		n, err := strconv.Atoi(grade.ID)
		if err == nil && n >= 1 && n < len(s.game.homes) && grade.Level == 4*n &&
			grade.Price > 0 && grade.Price <= math.MaxInt32 && math.Trunc(grade.Price) == grade.Price {
			s.game.homes[n] = grade
		}
	}
	return nil
}

func homeName(r Record) string {
	name := roleString(r, "Name")
	if name == "" {
		name = "道友"
	}
	return name + "的洞府"
}

func gameHomeRank(r Record) int64 {
	return max(0, min(4, roleInt(r, "DongFuRank")))
}

const practiceStarterMethodID = "gf_3_1"

const (
	studyRootMismatch = 4961
	studyBookRequired = 4962
	studyBookOnShelf  = 4963
	studyBookAdded    = 4964
)

func (def *gameMethod) elementFor(r Record) int {
	if def.id == practiceStarterMethodID {
		return firstCultivationRoot(r)
	}
	return def.element
}

func (def *gameMethod) matchesRoot(r Record) bool {
	return def.id == practiceStarterMethodID || roleBool(r, cultivationRootFields[def.element])
}

func (def *gameMethod) onStudyShelf(r Record) bool {
	return def.system || slices.Contains(stringRecords(r["GongFaRead"]), def.id) ||
		def.id == practiceStarterMethodID && (roleBool(r, "PracticeStarterRead") || practiceStarterPending(r))
}

func readStudyBook(r Record, def *gameMethod) int {
	if !def.matchesRoot(r) {
		return studyRootMismatch
	}
	firstGuideBook := def.id == practiceStarterMethodID && roleInt(r, "PracticeGuideVal") == 1
	if def.onStudyShelf(r) || !firstGuideBook && slices.Contains(ownedGameMethods(r), def.id) {
		return studyBookOnShelf
	}
	r["GongFaRead"] = append(stringRecords(r["GongFaRead"]), def.id)
	if firstGuideBook {
		r["PracticeStarterRead"] = true
	}
	return studyBookAdded
}

func practiceStarterPending(r Record) bool {
	// Step 2 means the starter book was already read, even in older saves that pre-learned its method.
	return roleInt(r, "PracticeGuideVal") == 2 && !roleBool(r, "PracticeStarterUnderstood")
}

func (s *Server) canBreak(r Record) bool {
	level := roleInt(r, "Level")
	return level < 90 && float64(roleInt(r, "SuperAir")) >= s.levelInfo(level).upgradeLimit
}

var gameCultivationProtocols = map[int]gameMapProtocol{
	1001005: {"UnderstandMethodReq_1001005", "ActionBoolResp_1001001"},
	1001007: {"ChangeRunLocationReq_1001007", "ActionBoolResp_1001001"},
	1001008: {"", "RoleBreakAskResp_1001008"},
	1001010: {"", "RoleBreakRealmResp_1001010"},
	1001011: {"", "OpenUnderstandPanelResp_1001011"},
	1001020: {"UpgradeAchievementConversionRateReq_1001020", "UpgradeAchievementConversionRateResp_1001020"},
	1005001: {"", "EnterHomeResp_1005001"},
	1005002: {"", "UpgradeHomeResp_1005002"},
	1005003: {"BuildLiveTypeReq_1005003", "BuildLiveTypeResp_1005003"},
	1005004: {"RebuildLiveTypeReq_1005004", "RebuildLiveTypeResp_1005004"},
	1005008: {"ChangeRunLocationReq_1001007", "EnterExerciseRoomResp_1005008"},
	1005009: {"ChangeAchievementPanelReq_1005009", "ChangeAchievementPanelResp_1005009"},
	1005011: {"ChangeAchievementReq_1005011", "ChangeAchievementResp_1005011"},
	1005016: {"", "OpenAcupointResp_1005016"},
	1005018: {"BreakAcupointReq_1005018", "BreakAcupointResp_1005018"},
	1005020: {"", "AcupointDetailResp_1005020"},
	1011004: {"MethodAchievementDetailReq_1011004", "MethodAchievementDetailResp_1011004"},
}

func (s *Server) handleCultivation(peer *Peer, msg Message) bool {
	p, ok := gameCultivationProtocols[msg.Cmd]
	if !ok {
		return false
	}
	var req protoreflect.Message
	if p.req != "" {
		m, err := s.decode(p.req, msg.Data)
		if err != nil {
			s.sendGame(peer, msg, s.gameResponse(p.resp, false, 4008))
			return true
		}
		req = m
	}
	b := s.gameResponse(p.resp, true, 2000)
	var r Record
	switch msg.Cmd {
	case 1005001:
		r = s.roles.project(peer.currentRoleID(), []string{"Name", "DongFuRank", "Live1Type", "Live2Type"})
	case 1005002, 1005003, 1005004:
		// These handlers read the role inside their own transaction.
	default:
		r = s.roles.Get(peer.currentRoleID())
	}
	quest := false
	var ageUpdate proto.Message
	switch msg.Cmd {
	case 1005016, 1005020:
		if r == nil {
			statusPB(b, false, 4007)
		} else if msg.Cmd == 1005016 {
			fillAcupoints(r, b)
		} else {
			s.fillMeridianOverview(r, b)
		}
	case 1005018:
		s.breakAcupoint(peer, msg, req, b)
	case 1001008:
		if s.canBreak(r) {
			recoverRoleVigor(r, time.Now().Unix())
			if code := s.breakthroughAttributeCode(r); code != 0 {
				statusPB(b, false, code)
			} else if code := breakthroughResourceCode(r); code != 0 {
				statusPB(b, false, code)
			} else {
				set(b, "Code", 4063)
			}
		} else {
			statusPB(b, false, 4062)
		}
	case 1001011:
		if r == nil {
			statusPB(b, false, 4007)
			break
		}
		methods := s.methods().all
		owned := ownedGameMethods(r)
		for i := range methods {
			def := &methods[i]
			understood := slices.Contains(owned, def.id)
			if def.id == practiceStarterMethodID && practiceStarterPending(r) {
				understood = false
			}
			if understood || def.matchesRoot(r) && def.onStudyShelf(r) {
				fillUnderstandMethod(b, r, def, understood)
			}
		}
		set(b, "Code", 4263)
	case 1005001:
		if r == nil {
			statusPB(b, false, 4007)
			break
		}
		putPB(b, Record{"Name": homeName(r), "Rank": gameHomeRank(r) * 4})
		copyRolePB(b, r, "Live1Type", "Live2Type")
	case 1005002:
		s.upgradeGameHome(peer, msg, b)
	case 1005003, 1005004:
		s.changeResidenceCareer(peer, msg.Cmd, req, b)
	case 1005008:
		if r == nil {
			statusPB(b, false, 4007)
			break
		}
		code := 4128
		if loc := getInt(req, "NewLocation"); loc != 0 {
			if !canRunLocation(r, loc) {
				statusPB(b, false, 4059)
				break
			}
			// Get returned a snapshot. Only 1001007 persists a route.
			r["RunMask"] = gameRunMask(loc)
			code = 2000
		}
		putPB(b, Record{
			"Code": code, "LocationType": gameLocation(roleInt(r, "RunMask")),
			"SuperairCircle": s.gameRate(r, 3) * 2, "PhysiqueCircle": s.gameRate(r, 2) * 2, "PerceptionCircle": s.gameRate(r, 1) * 2,
			"SuperairName": s.equippedMethodName(r, 3), "PhysiqueName": s.equippedMethodName(r, 2),
			"PerceptionName": s.equippedMethodName(r, 1), "GatherMatrixRank": 4 * max(0, min(4, roleInt(r, "JuLingRank"))),
			"ControllerMatrixRank": 4 * max(0, min(4, roleInt(r, "KongLingRank"))), "RefiningType": 3, "IsBreak": s.canBreak(r),
		})
	case 1005009:
		kind := int(getInt(req, "AchievementType"))
		if kind < 1 || kind > 3 {
			kind = 3
		}
		current := s.methods().equipped(r, kind)
		if current != nil {
			d := b.Mutable(field(b, "CurrentAchievement")).Message()
			s.fillCultivationMethod(d, r, current)
		}
		owned := ownedGameMethods(r)
		methods := s.methods().all
		for i := range methods {
			def := &methods[i]
			if def.kind == kind && slices.Contains(owned, def.id) {
				s.fillCultivationMethod(addMessage(b, "Achievements"), r, def)
			}
		}
	case 1001020:
		s.applyMethodScripture(peer.currentRoleID(), getString(req, "AchievementId"), b)
	case 1011004:
		def := s.methods().byID[getString(req, "Id")]
		if def == nil {
			statusPB(b, false, 4326)
			break
		}
		set(b, "ConversionRate", altarMethodCircle(r, def))
		def.writeEffects(b)
		author, studyLevel := "天道", int64(1)
		if def.system {
			author = "天道传承"
			levels, _ := r["GongFaChong"].(map[string]any)
			studyLevel = max(1, roleInt(levels, def.id))
		}
		putPB(b, Record{"ActionScope": def.kind, "Level": def.learningLevel, "StudyLevel": studyLevel,
			"BasedConcentration": 1, "RunSpeed": 2, "CreatedBy": author, "UsedCount": 1, "MaxCount": 1})
	default:
		err := s.roles.Transact(func(tx *StoreTx) error {
			role := tx.Get(peer.currentRoleID())
			if role == nil {
				return ErrRecordNotFound
			}
			switch msg.Cmd {
			case 1001005:
				def := s.methods().byID[getString(req, "Id")]
				if getInt(req, "Type") == 2 && def != nil {
					starterGuide := def.id == practiceStarterMethodID && practiceStarterPending(role)
					switch {
					case !def.matchesRoot(role):
						statusPB(b, false, studyRootMismatch)
					case slices.Contains(ownedGameMethods(role), def.id) && !starterGuide:
						statusPB(b, true, 4048)
					case !def.onStudyShelf(role):
						statusPB(b, false, studyBookRequired)
					case roleInt(role, "Level") < int64(def.learningLevel):
						statusPB(b, false, 4057)
					default:
						learnGameMethod(role, def)
						if starterGuide {
							role["PracticeStarterUnderstood"] = true
						}
						quest = s.gameMethodQuest(role, def)
						statusPB(b, true, 4048)
					}
				} else {
					statusPB(b, false, 4049)
				}
			case 1001007:
				loc := getInt(req, "NewLocation")
				statusPB(b, false, 4059)
				if canRunLocation(role, loc) {
					role["RunMask"], role["RunLocation"] = gameRunMask(loc), loc
					statusPB(b, true, 4058)
				}
			case 1001010:
				recoverRoleVigor(role, time.Now().Unix())
				if !s.canBreak(role) {
					statusPB(b, false, 4062)
				} else if code := s.breakthroughAttributeCode(role); code != 0 {
					statusPB(b, false, code)
				} else if code := breakthroughResourceCode(role); code != 0 {
					statusPB(b, false, code)
				} else {
					before := roleInt(role, "Level")
					s.levelUpGameRole(role)
					after := roleInt(role, "Level")
					ageUpdate = s.realmAgeUpdate(role)
					if err := tx.RecordHistory(newHistoryEvent("cultivation.breakthrough", role, nil, "realm", strconv.FormatInt(after, 10),
						Record{"previousLevel": before, "level": after})); err != nil {
						return err
					}
					if s.questRuleFor(role).Type == "LEVEL" {
						quest = s.checkQuest(role)
					}
					statusPB(b, true, 4306)
				}
				copyRolePB(b, role, "PureBlood", "FlexibleRoot", "SuperAir", "Perception", "Physique", "CurrentSuperAir",
					"CurrentPerception", "CurrentPhysique", "CurrentVigor", "CurrentClever", "CurrentPoisonVal", "CurrentPureBlood", "CurrentPower")
				set(b, "Age", roleInt(role, "MaxAge"))
			case 1005011:
				def := s.methods().byID[getString(req, "AchievementId")]
				scope := int(getInt(req, "ScopeType"))
				if scope < 1 || scope > 3 {
					scope = 3
				}
				switch {
				case def == nil || def.kind != scope || int64(def.learningLevel) > roleInt(role, "Level"):
					statusPB(b, false, 4057)
				case !slices.Contains(ownedGameMethods(role), def.id):
					statusPB(b, false, 4326)
				default:
					role[gameEquipField(scope)] = def.id
					s.refreshMethodAffixes(role)
					recomputeGamePower(role)
					quest = s.gameMethodQuest(role, def)
					statusPB(b, true, 4056)
					set(b, "AchievementCircleValue", s.gameRate(role, scope)*2)
				}
			}
			return nil
		})
		if err != nil {
			statusPB(b, false, 4007)
			quest = false
			ageUpdate = nil
		}
	}
	s.sendGame(peer, msg, b)
	if ageUpdate != nil {
		s.pushGame(peer.currentRoleID(), 9001007, ageUpdate)
	}
	if quest {
		s.gameQuestComplete(peer)
	}
	return true
}

func canRunLocation(r Record, location int64) bool {
	return location >= 1 && location <= 6 && (location <= 3 || roleInt(r, "Level") >= 31)
}

func fillUnderstandMethod(b protoreflect.Message, r Record, def *gameMethod, understood bool) {
	putPB(addMessage(b, "Object"), Record{
		"Id": def.id, "Name": def.name, "FiveElement": def.elementFor(r), "Level": def.learningLevel,
		"Rank": altarMethodRank(r, def), "Type": 2, "IsUnderstand": understood,
	})
}

func realmStartingAir(level int64, lvl gameLevel) int64 {
	if level == 1 {
		return int64(lvl.upgradeLimit)
	} // Tutorial opens with a first breakthrough.
	return int64(lvl.upgradeLimit * .10)
}

func (s *Server) levelUpGameRole(r Record) { s.levelUpGameRoleAt(r, time.Now().Unix()) }

func (s *Server) levelUpGameRoleAt(r Record, now int64) {
	oldLevel := roleInt(r, "Level")
	years, cost := breakthroughCost(oldLevel)
	r["CurrentAge"] = max(16, roleInt(r, "CurrentAge")) + years
	spendVigorAt(r, cost, now)
	delete(r, "BattleAirRemainder")
	delete(r, "BattlePerceptionRemainder")
	delete(r, "BattlePhysiqueRemainder")
	level := oldLevel + 1
	lvl := s.levelInfo(level)
	r["AcupointValue"] = min(int64(math.MaxInt32), max(int64(0), acupointBalance(r))+int64(lvl.acupoints))
	r["Level"] = level
	r["SuperAir"] = realmStartingAir(level, lvl)
	r["Perception"] = max(roleInt(r, "Perception"), int64(lvl.perception))
	r["Physique"] = max(roleInt(r, "Physique"), int64(lvl.toughTenacity))
	r["CurrentSuperAir"] = roleInt(r, "Perception") * 2
	r["CurrentPerception"], r["CurrentPhysique"] = r["Perception"], r["Physique"]
	pure := max(roleInt(r, "PureBlood"), 100, int64(math.Round(lvl.perception*.01)))
	r["PureBlood"], r["CurrentPureBlood"] = pure, pure
	r["MaxAge"] = min(math.MaxInt32, gameLifespan(level)+roleInt(r, "BonusLifespan"))
	vigor := vigorCapacity(level)
	r["CurrentClever"], r["Vigor"], r["Clever"] = vigor, max(roleInt(r, "Vigor"), vigor), vigor
	r["CurrentPoisonVal"] = 0
	recomputeGamePower(r)
}

func (s *Server) equippedMethodName(r Record, kind int) string {
	if def := s.methods().equipped(r, kind); def != nil {
		return def.name
	}
	return ""
}

func (m *gameMethod) writeEffects(out protoreflect.Message) {
	putPB(out, Record{"EffectType1": m.effect1, "EffectValue1": m.effectValue1, "EffectType2": m.effect2, "EffectValue2": m.effectValue2})
	if m.moveSpeedPercent > 0 {
		putPB(out, Record{"EffectType3": methodMoveSpeedEffect, "EffectValue3": m.moveSpeedPercent})
	}
}

func (s *Server) fillCultivationMethod(out protoreflect.Message, r Record, def *gameMethod) {
	chong, _ := r["GongFaChong"].(map[string]any)
	circle := s.gameRateFor(r, def, def.kind) * 2
	putPB(out, Record{
		"Id": def.id, "Name": def.name, "Circle": circle, "AchievementLevel": max(1, roleInt(chong, def.id)),
		"MaxCount": 3, "Level": def.learningLevel, "Rank": altarMethodRank(r, def), "FiveElement": def.elementFor(r),
	})
}

func (s *Server) upgradeGameHome(peer *Peer, msg Message, b protoreflect.Message) {
	code := 4007
	roleID := peer.currentRoleID()
	var reply Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		r := tx.View(roleID)
		active, online := s.online.Load(roleID)
		if r == nil || !online || active != peer {
			return nil
		}
		reply = r
		if s.preview != nil && !s.previewWrites {
			code = previewDeniedCode
			return nil
		}
		grade := gameHomeRank(r)
		next := grade + 1
		if msg.Msid > 0 {
			if msg.Msid <= peer.lastHomeUpgradeMessageID {
				code = 4008
				return nil
			}
			peer.lastHomeUpgradeMessageID = msg.Msid
		}
		if grade == 4 {
			code = 4110
			return nil
		}
		if len(msg.Data) > 0 {
			var requested wrapperspb.Int32Value
			if proto.Unmarshal(msg.Data, &requested) != nil || int64(requested.Value) != next {
				code = 4008
				return nil
			}
		}
		if (max(1, roleInt(r, "Level"))-1)/10+1 < next {
			code = 4105
			return nil
		}
		price := int64(s.game.homes[next].Price)
		if roleInt(r, "VipLevel") >= 4 {
			price = price * 9 / 10
		}
		balance := roleInt(r, "ShinyStone")
		switch {
		case price <= 0:
			code = 5000
		case balance < price:
			code = 4112
		default:
			code = 4111
			reply = tx.Get(roleID)
			reply["DongFuRank"], reply["ShinyStone"] = next, balance-price
		}
		return nil
	})
	if err != nil && reply != nil {
		code = 5000
		reply = s.roles.Get(roleID)
	}
	statusPB(b, code == 4111, code)
	putPB(b, Record{"Rank": gameHomeRank(reply) * 4, "Name": homeName(reply), "ShinyStone": roleInt(reply, "ShinyStone")})
}

func (s *Server) advanceCultivation(ctx context.Context) {
	timer := time.NewTicker(2 * time.Second)
	defer timer.Stop()
	type progress struct {
		role         string
		gain         [3]float64
		achievements int64
	}
	carry, current := make(map[*Peer]progress), make(map[*Peer]progress)
	tick := 0
	for {
		select {
		case <-ctx.Done():
			return
		case <-timer.C:
			tick++
			for _, peer := range s.gamePeers() {
				roleID := peer.currentRoleID()
				if roleID == "" {
					continue
				}
				value := carry[peer]
				if value.role != roleID {
					value = progress{role: roleID}
				}
				earned, ready := s.cultivatePeerAs(peer, roleID, &value.gain, tick)
				if ready && earned > value.achievements {
					s.pushGame(roleID, achievementNoticeCmd, s.gameResponse("InformTianShuBeiResp_1016003", true, 2000))
				}
				value.achievements = max(value.achievements, earned)
				current[peer] = value
			}
			carry, current = current, carry
			clear(current)
			if tick%15 == 0 && len(carry) != 0 {
				if err := s.roles.flushProgress(); err != nil {
					log.Printf("保存修炼进度失败: %v", err)
				}
			}
		}
	}
}

func (s *Server) cultivatePeerAs(peer *Peer, roleID string, carry *[3]float64, tick int) (earned int64, ready bool) {
	type update struct {
		msg proto.Message
		cmd int
	}
	var pushes []update
	nextCarry := *carry
	quest := false
	err := s.roles.updateCultivationProgress(roleID, func(r Record) (bool, error) {
		level := s.levelInfo(roleInt(r, "Level"))
		mask := roleInt(r, "RunMask")
		persist := false
		r["LastOnline"] = time.Now().UnixMilli()
		var elapsed int64
		if mask&4 != 0 {
			rate := s.gameRate(r, 3) * 2
			active := productiveTrainingSeconds(r, "SuperAir", level.upgradeLimit, rate/2, 2)
			elapsed = active
			nextCarry[2] += rate * float64(active) / 2
			gain := int64(math.Floor(nextCarry[2]))
			nextCarry[2] -= float64(gain)
			air, cap := roleInt(r, "SuperAir"), int64(level.upgradeLimit)
			before := air
			// Bonus cultivation granted by pills or a teacher can already exceed
			// the passive training limit. A passive tick may only add progress.
			if gain > 0 && air < cap {
				air += min(gain, cap-air)
				r["SuperAir"] = air
			}
			broke := before < cap && air >= cap
			persist = persist || broke
			mana := max(1, roleInt(r, "Perception")*2)
			r["CurrentSuperAir"] = mana
			if gain > 0 || tick%5 == 0 || broke {
				out := s.pb("SuperAirUpResp_9001004")
				putPB(out, Record{"CurrentSuperAir": mana, "SuperAir": air, "IsBreak": broke, "CurrentRunValue": rate})
				pushes = append(pushes, update{out, 9001004})
			}
		}
		for i, kind := range [...]string{"Perception", "Physique"} {
			if mask&(1<<i) == 0 {
				continue
			}
			cap := level.trainingCap(i + 1)
			rate := s.gameRate(r, i+1) * 2
			active := productiveTrainingSeconds(r, kind, float64(cap), rate/4, 2)
			elapsed = max(elapsed, active)
			nextCarry[i] += rate * float64(active) / 4
			gain := int64(math.Floor(nextCarry[i]))
			nextCarry[i] -= float64(gain)
			current := roleInt(r, kind)
			if gain > 0 && current < cap {
				current = min(current+gain, cap)
				r[kind], r["Current"+kind] = current, current
				if i == 1 {
					pure := max(100, roleInt(r, "PureBlood"), int64(math.Round(float64(current)*.01)))
					r["PureBlood"], r["CurrentPureBlood"] = pure, pure
				}
			}
			if gain > 0 || tick%5 == 0 {
				cmd := [...]int{9001006, 9001005}[i]
				out := s.pb(kind + "UpResp_" + strconv.Itoa(cmd))
				copyRolePB(out, r, kind, "Current"+kind)
				if i == 1 {
					copyRolePB(out, r, "PureBlood", "CurrentPureBlood")
				}
				set(out, "CurrentRunValue", rate)
				pushes = append(pushes, update{out, cmd})
			}
		}
		if ageCultivation(r, elapsed) {
			pushes = append(pushes, update{s.realmAgeUpdate(r), 9001007})
			persist = true
		}
		recoverRoleVigor(r, time.Now().Unix())
		if r["LastReportedVigor"] != nil && roleInt(r, "LastReportedVigor") != roleInt(r, "CurrentVigor") {
			out := s.pb("VigorUpResp_9001001")
			set(out, "CurrentVigor", roleInt(r, "CurrentVigor"))
			pushes = append(pushes, update{out, 9001001})
		}
		r["LastReportedVigor"] = roleInt(r, "CurrentVigor")
		if persist || tick%15 == 0 {
			recomputeGamePower(r)
		}
		if roleInt(r, "TaskStatus") == 0 && !roleBool(r, "TaskCompletedAll") {
			switch s.questRuleFor(r).Type {
			case "AUTO":
				if roleInt(r, "TaskAutoAt") <= 0 {
					r["TaskAutoAt"] = time.Now().Add(5 * time.Second).UnixMilli()
				}
			case "CYCLES":
				if mask&2 != 0 {
					r["QuestCycles"] = roleInt(r, "QuestCycles") + 1
				}
			}
			quest = s.checkQuest(r)
		}
		earned, ready = achievementSummary(r)
		return persist || quest, nil
	})
	if err != nil {
		return 0, false
	}
	*carry = nextCarry
	for _, push := range pushes {
		if payload, err := proto.Marshal(push.msg); err == nil {
			peer.writeFrameForRole(roleID, encodeFrame(push.cmd, 0, payload), nil)
		}
	}
	if quest {
		s.pushGame(roleID, 1016001, nil)
	}
	return earned, ready
}

func (s *Server) realmAgeUpdate(r Record) proto.Message {
	out := s.pb("AgeUpResp_9001007")
	putPB(out, Record{"Age": roleInt(r, "CurrentAge"), "MaxAge": roleInt(r, "MaxAge")})
	return out
}
