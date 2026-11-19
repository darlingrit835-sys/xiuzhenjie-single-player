package main

import (
	"context"
	"math"
	"math/bits"
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
	1005008: {"", "EnterExerciseRoomResp_1005008"},
	1005009: {"ChangeAchievementPanelReq_1005009", "ChangeAchievementPanelResp_1005009"},
	1005011: {"ChangeAchievementReq_1005011", "ChangeAchievementResp_1005011"},
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
			return true
		}
		req = m
	}
	b := s.gameResponse(p.resp, true, 2000)
	r := s.roles.Get(peer.currentRoleID())
	quest := false
	switch msg.Cmd {
	case 1001008:
		if s.canBreak(r) {
			set(b, "Code", 4063)
		} else {
			statusPB(b, false, 4062)
		}
	case 1001011:
		owned := ownedGameMethods(r)
		guide := practiceStarterPending(r)
		if guide {
			fillUnderstandMethod(b, findGameMethod(practiceStarterMethodID), false)
		}
		for i := range methods {
			def := &methods[i]
			if guide && def.id == practiceStarterMethodID {
				continue
			}
			fillUnderstandMethod(b, def, slices.Contains(owned, def.id))
		}
		set(b, "Code", 4263)
	case 1005001:
		putPB(b, Record{"Name": homeName(r), "Rank": gameHomeRank(r) * 4, "Live1Type": 1, "Live2Type": 2})
	case 1005002:
		s.upgradeGameHome(peer, msg, b)
	case 1005008:
		putPB(b, Record{
			"Code": 4128, "LocationType": gameLocation(roleInt(r, "RunMask")),
			"SuperairCircle": s.gameRate(r, 3) * 2, "PhysiqueCircle": s.gameRate(r, 2) * 2, "PerceptionCircle": s.gameRate(r, 1) * 2,
			"SuperairName": equippedMethodName(r, 3), "PhysiqueName": equippedMethodName(r, 2),
			"PerceptionName": equippedMethodName(r, 1), "GatherMatrixRank": roleInt(r, "JuLingRank"),
			"ControllerMatrixRank": roleInt(r, "KongLingRank"), "RefiningType": 3, "IsBreak": s.canBreak(r),
		})
	case 1005009:
		kind := int(getInt(req, "AchievementType"))
		if kind < 1 || kind > 3 {
			kind = 3
		}
		current := equippedGameMethod(r, kind)
		if current != nil {
			d := b.Mutable(field(b, "CurrentAchievement")).Message()
			s.fillCultivationMethod(d, r, current, current)
		}
		owned := ownedGameMethods(r)
		for i := (kind - 1) * 16; i < kind*16; i++ {
			def := &methods[i]
			if slices.Contains(owned, def.id) {
				s.fillCultivationMethod(addMessage(b, "Achievements"), r, def, current)
			}
		}
	case 1001020, 1011004:
		key := "Id"
		if msg.Cmd == 1001020 {
			key = "AchievementId"
		}
		def := findGameMethod(getString(req, key))
		if def == nil {
			statusPB(b, false, 4326)
			break
		}
		putPB(b, Record{"ConversionRate": def.baseCircle, "EffectType1": def.effect1, "EffectValue1": def.effectValue1,
			"EffectType2": def.effect2, "EffectValue2": def.effectValue2})
		if msg.Cmd == 1001020 {
			statusPB(b, false, 4703)
			set(b, "Rank", def.rank)
		} else {
			putPB(b, Record{"ActionScope": def.kind, "Level": def.learningLevel, "StudyLevel": 1,
				"BasedConcentration": 1, "RunSpeed": 2, "CreatedBy": "天道", "UsedCount": 1, "MaxCount": 1})
		}
	default:
		err := s.roles.Transact(func(tx *StoreTx) error {
			role := tx.Get(peer.currentRoleID())
			if role == nil {
				return ErrRecordNotFound
			}
			switch msg.Cmd {
			case 1001005:
				def := findGameMethod(getString(req, "Id"))
				if getInt(req, "Type") == 2 && def != nil {
					learnGameMethod(role, def)
					if def.id == practiceStarterMethodID && practiceStarterPending(role) {
						role["PracticeStarterUnderstood"] = true
					}
					quest = s.gameMethodQuest(role, def)
					statusPB(b, true, 4048)
				} else {
					statusPB(b, false, 4049)
				}
			case 1001007:
				loc := getInt(req, "NewLocation")
				statusPB(b, false, 4059)
				if loc >= 1 && loc <= 3 {
					mask := roleInt(role, "RunMask") ^ (1 << (loc - 1))
					if mask != 0 && (bits.OnesCount64(uint64(mask)) <= 1 || roleInt(role, "Level") >= 31) {
						role["RunMask"], role["RunLocation"] = mask, gameLocation(mask)
						statusPB(b, true, 4058)
					}
				}
			case 1001010:
				if !s.canBreak(role) {
					statusPB(b, false, 4062)
				} else {
					before := roleInt(role, "Level")
					s.levelUpGameRole(role)
					after := roleInt(role, "Level")
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
				def := findGameMethod(getString(req, "AchievementId"))
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
		}
	}
	s.sendGame(peer, msg, b)
	if quest {
		s.gameQuestComplete(peer)
	}
	return true
}

func fillUnderstandMethod(b protoreflect.Message, def *gameMethod, understood bool) {
	putPB(addMessage(b, "Object"), Record{
		"Id": def.id, "Name": def.name, "FiveElement": def.element, "Level": def.learningLevel,
		"Rank": def.rank, "Type": 2, "IsUnderstand": understood,
	})
}

func (s *Server) levelUpGameRole(r Record) {
	level := roleInt(r, "Level") + 1
	lvl := s.levelInfo(level)
	r["Level"] = level
	r["SuperAir"] = int64(lvl.upgradeLimit * .10)
	r["Perception"] = max(roleInt(r, "Perception"), int64(lvl.perception))
	r["Physique"] = max(roleInt(r, "Physique"), int64(lvl.toughTenacity))
	r["CurrentSuperAir"] = roleInt(r, "Perception") * 2
	r["CurrentPerception"], r["CurrentPhysique"] = r["Perception"], r["Physique"]
	pure := max(roleInt(r, "PureBlood"), 100, int64(math.Round(lvl.perception*.01)))
	r["PureBlood"], r["CurrentPureBlood"] = pure, pure
	r["MaxAge"] = gameLifespan(level)
	vigor := 100 + level*5
	r["CurrentVigor"], r["CurrentClever"], r["Vigor"], r["Clever"] = vigor, vigor, vigor, vigor
	r["CurrentPoisonVal"] = 0
	recomputeGamePower(r)
}

func equippedMethodName(r Record, kind int) string {
	if def := equippedGameMethod(r, kind); def != nil {
		return def.name
	}
	return ""
}

func (s *Server) fillCultivationMethod(out protoreflect.Message, r Record, def, equipped *gameMethod) {
	chong, _ := r["GongFaChong"].(map[string]any)
	circle := s.gameRate(r, def.kind) * 2
	mult := gameMethodMultiplier(r, def)
	if equipped == nil || equipped.baseCircle <= 0 {
		circle *= def.baseCircle / .8 * mult
	} else {
		circle *= def.baseCircle / equipped.baseCircle * mult / gameMethodMultiplier(r, equipped)
	}
	putPB(out, Record{
		"Id": def.id, "Name": def.name, "Circle": circle, "AchievementLevel": max(1, roleInt(chong, def.id)),
		"MaxCount": 3, "Level": def.learningLevel, "Rank": def.rank, "FiveElement": def.element,
	})
}

func (s *Server) upgradeGameHome(peer *Peer, msg Message, b protoreflect.Message) {
	code := 4007
	roleID := peer.currentRoleID()
	var reply Record
	err := s.roles.Update(roleID, func(r Record) error {
		reply = r
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
			r["DongFuRank"], r["ShinyStone"] = next, balance-price
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
	carry := make(map[*Peer]*[3]float64)
	tick := 0
	for {
		select {
		case <-ctx.Done():
			return
		case <-timer.C:
			tick++
			peers := s.gamePeers()
			for _, peer := range peers {
				if carry[peer] == nil {
					carry[peer] = new([3]float64)
				}
				s.cultivatePeer(peer, carry[peer], tick)
			}
			for p := range carry {
				if !slices.Contains(peers, p) {
					delete(carry, p)
				}
			}
		}
	}
}

func (s *Server) cultivatePeer(peer *Peer, carry *[3]float64, tick int) {
	type update struct {
		msg proto.Message
		cmd int
	}
	var pushes []update
	nextCarry := *carry
	quest := false
	err := s.roles.UpdateProgress(peer.currentRoleID(), func(r Record) (bool, error) {
		level := s.levelInfo(roleInt(r, "Level"))
		mask := roleInt(r, "RunMask")
		persist := tick%15 == 0
		if mask&4 != 0 {
			rate := s.gameRate(r, 3) * 2
			nextCarry[2] += rate
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
			cap := [...]float64{level.limitPerception, level.limitToughTenacity}[i]
			rate := s.gameRate(r, i+1) * 2
			nextCarry[i] += rate * 0.5
			gain := int64(math.Floor(nextCarry[i]))
			nextCarry[i] -= float64(gain)
			current := roleInt(r, kind)
			if gain > 0 && current < int64(cap*1.5) {
				current = min(current+gain, int64(cap*1.5))
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
		if persist {
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
		return persist || quest, nil
	})
	if err != nil {
		return
	}
	*carry = nextCarry
	for _, push := range pushes {
		s.sendGame(peer, Message{Cmd: push.cmd}, push.msg)
	}
	if quest {
		s.gameQuestComplete(peer)
	}
}
