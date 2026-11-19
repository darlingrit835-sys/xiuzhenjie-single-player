package main

import (
	"fmt"
	"slices"
	"strconv"
	"strings"
	"time"
)

type questDefinition struct {
	ID        int64  `json:"id"`
	Condition string `json:"condition"`
	Award     string `json:"award"`
	Type      int    `json:"awardGoodsType"`
	Rank      int    `json:"awardGoodsRank"`
	Stage     int    `json:"awardGoodsStage"`
	Number    int    `json:"awardNum"`
}

type questRule struct {
	Type   string `json:"type"`
	Target int64  `json:"target"`
}

func questType(text string) questRule {
	kind, number, _ := strings.Cut(text, ":")
	count, _ := strconv.ParseInt(number, 10, 64)
	return questRule{kind, max(1, count)}
}

func (s *Server) loadQuests() error {
	var definitions []questDefinition
	if err := s.readGameTable("mission.json", &definitions); err != nil {
		return err
	}
	s.game.quests = make(map[int64]questDefinition, len(definitions))
	s.game.rules = make(map[int64]questRule, len(definitions))
	for _, d := range definitions {
		if d.ID > 0 {
			d.Number = max(1, d.Number)
			s.game.quests[d.ID] = d
			s.game.rules[d.ID] = inferQuest(d.Condition)
			s.game.lastTask = max(s.game.lastTask, d.ID)
		}
	}
	defaults := strings.Fields("CULTIVATE_NAQI KILL AUTO LEVEL:2 AUTO SWEEP AUTO AUTO LEVEL:3 KILL:5 SWEEP:3 AUTO LEVEL:4 CHAT STONE:1500 AUTO GONGFA:3 AUTO AUTO AUTO AUTO AUTO LEVEL:7 AUTO DUNGEON AUTO LEVEL:10 AUTO LEVEL:11 CULTIVATE_DUANTI CYCLES:30 GONGFA AUTO BOSS_KILL SWEEP:2 STONE:15000 CHAT AUTO GONGFA:3 AUTO AUTO AUTO BOSS_KILL")
	for i, rule := range defaults {
		s.game.rules[int64(i+1)] = questType(rule)
	}
	var overrides map[string]questRule
	if err := s.readGameTable("quests.json", &overrides); err != nil {
		return err
	}
	for key, override := range overrides {
		if id, err := strconv.ParseInt(key, 10, 64); err == nil && override.Type != "" {
			s.game.rules[id] = override
		}
	}
	return nil
}

func inferQuest(c string) questRule {
	num := questNumeral(c)
	switch {
	case strings.Contains(c, "突破"):
		for i, realm := range strings.Fields("练气 筑基 结丹 元婴 分神 炼虚 合体 渡劫 大乘") {
			if strings.Contains(c, realm) {
				before, _, found := strings.Cut(c, "层")
				digits := []rune(before)
				tier := int64(1)
				if found && len(digits) > 0 {
					tier = max(1, chineseQuestDigit(digits[len(digits)-1]))
				}
				return questRule{"LEVEL", int64(i*10) + tier}
			}
		}
	case strings.Contains(c, "击杀"):
		if containsQuest(c, "领主", "长老", "首领", "兽王") {
			return questRule{"BOSS_KILL", num}
		}
		return questRule{"KILL", num}
	case strings.Contains(c, "扫荡"):
		return questRule{"SWEEP", num}
	case strings.Contains(c, "灵石") && containsQuest(c, "拥有", "获取", "超过"):
		return questRule{"STONE", num}
	case strings.Contains(c, "功法") && containsQuest(c, "修习", "参悟", "补全"):
		switch {
		case containsQuest(c, "十种", "10种"):
			return questRule{"GONGFA_COUNT", 10}
		case strings.Contains(c, "玄阶"):
			return questRule{"GONGFA", 5}
		case strings.Contains(c, "地阶"):
			return questRule{"GONGFA", 9}
		case strings.Contains(c, "天阶"):
			return questRule{"GONGFA", 13}
		default:
			return questRule{"GONGFA", 1}
		}
	case containsQuest(c, "发言", "传音"):
		return questRule{"CHAT", 1}
	case strings.Contains(c, "秘境"):
		return questRule{"DUNGEON", 1}
	case strings.Contains(c, "锻体修行"):
		if strings.Contains(c, "周天") {
			return questRule{"CYCLES", num}
		}
		return questRule{"CULTIVATE_DUANTI", 1}
	}
	return questRule{"AUTO", 1}
}

func containsQuest(text string, words ...string) bool {
	for _, word := range words {
		if strings.Contains(text, word) {
			return true
		}
	}
	return false
}

func chineseQuestDigit(ch rune) int64 {
	for i, digit := range []rune("一二三四五六七八九十") {
		if ch == digit {
			return int64(i + 1)
		}
	}
	if ch == '两' {
		return 2
	}
	return 0
}

func questNumeral(text string) int64 {
	var n int64
	for _, ch := range text {
		if ch >= '0' && ch <= '9' {
			n = n*10 + int64(ch-'0')
		} else if n > 0 {
			break
		}
	}
	if n > 0 {
		if strings.ContainsRune(text, '万') {
			n *= 10000
		}
		return n
	}
	for _, ch := range text {
		if n := chineseQuestDigit(ch); n > 0 {
			return n
		}
	}
	return 1
}

var taskCounters = [...]string{"QuestKills", "QuestBossKills", "QuestSweeps", "QuestChats", "QuestDungeons", "QuestCycles", "QuestGongFaRank"}

func (s *Server) ensureQuest(r Record) {
	if roleInt(r, "TaskId") <= 0 {
		r["TaskId"] = 1
	}
	if roleInt(r, "TaskStatus") != 1 {
		r["TaskStatus"] = 0
	}
	for _, key := range taskCounters {
		setRoleDefault(r, key, 0)
	}
	for _, key := range []string{"QuestGongFaCount", "TaskAutoAt", "QuestTotalKills", "QuestTotalSweeps"} {
		setRoleDefault(r, key, 0)
	}
	syncTaskMethods(r)
}

func syncTaskMethods(r Record) {
	rank := roleInt(r, "QuestGongFaRank")
	for kind := 1; kind <= 3; kind++ {
		if def := equippedGameMethod(r, kind); def != nil {
			rank = max(rank, int64(def.rank))
		}
	}
	r["QuestGongFaRank"] = rank
}

func (s *Server) questRuleFor(r Record) questRule {
	if rule, ok := s.game.rules[roleInt(r, "TaskId")]; ok {
		return rule
	}
	return questRule{"AUTO", 1}
}

func (s *Server) checkQuest(r Record) bool {
	if roleInt(r, "TaskStatus") == 1 || roleBool(r, "TaskCompletedAll") {
		return false
	}
	rule := s.questRuleFor(r)
	fields := map[string]string{
		"LEVEL": "Level", "KILL": "QuestKills", "BOSS_KILL": "QuestBossKills", "SWEEP": "QuestSweeps",
		"STONE": "ShinyStone", "GONGFA": "QuestGongFaRank", "GONGFA_COUNT": "QuestGongFaCount",
		"CHAT": "QuestChats", "DUNGEON": "QuestDungeons", "CYCLES": "QuestCycles",
	}
	target := max(rule.Target, 1)
	done := false
	if field, ok := fields[rule.Type]; ok {
		done = roleInt(r, field) >= target
	} else {
		switch rule.Type {
		case "AUTO":
			at := roleInt(r, "TaskAutoAt")
			done = at > 0 && time.Now().UnixMilli() >= at
		case "CULTIVATE_NAQI":
			done = roleInt(r, "RunMask")&4 != 0
		case "CULTIVATE_DUANTI":
			done = roleInt(r, "RunMask")&2 != 0
		}
	}
	if done {
		r["TaskStatus"], r["TaskAutoAt"] = 1, 0
	}
	return done
}

func (s *Server) grantQuestEvent(r Record, event string) bool {
	counters := map[string][]string{
		"KILL": {"QuestKills", "QuestTotalKills"}, "BOSS_KILL": {"QuestKills", "QuestTotalKills", "QuestBossKills", "QuestTotalBossKills"},
		"SWEEP": {"QuestSweeps", "QuestTotalSweeps"}, "CHAT": {"QuestChats"}, "DUNGEON": {"QuestDungeons"},
	}
	for _, field := range counters[event] {
		r[field] = roleInt(r, field) + 1
	}
	kind := s.questRuleFor(r).Type
	if kind == event || kind == "KILL" && event == "BOSS_KILL" {
		return s.checkQuest(r)
	}
	return false
}

func (s *Server) gameMethodQuest(r Record, def *gameMethod) bool {
	r["QuestGongFaRank"] = max(roleInt(r, "QuestGongFaRank"), int64(def.rank))
	kind := s.questRuleFor(r).Type
	return (kind == "GONGFA" || kind == "GONGFA_COUNT") && s.checkQuest(r)
}

func gameAwardGoods(r Record, name string, itemType, rank, level, count int) {
	addGoods(r, name, itemType, rank, level, primaryGameElement(r), fmt.Sprintf("reward_%d_%d_%d", itemType, max(1, rank), max(1, level)), count, .5, 5+rank*5)
}

func grantGameQuest(r Record, d questDefinition) string {
	amount := max(1, d.Number)
	r["ShinyStone"] = roleInt(r, "ShinyStone") + 30*int64(amount)
	suffix := ""
	if d.Type == 99 {
		r["ShinyJade"] = roleInt(r, "ShinyJade") + int64(amount)
	} else {
		if d.Type > 0 {
			gameAwardGoods(r, d.Award, d.Type, d.Rank, d.Stage, amount)
		}
		suffix = "（已放入纳戒）"
	}
	bag, _ := r["QuestBag"].([]any)
	bag = append(bag, Record{"name": d.Award, "type": d.Type, "rank": d.Rank, "stage": d.Stage, "num": amount, "taskId": d.ID, "time": time.Now().UnixMilli()})
	r["QuestBag"] = bag[max(0, len(bag)-80):]
	r["LastQuestId"], r["LastQuestAward"] = d.ID, d.Award
	return fmt.Sprintf("获得任务奖励：%s×%d%s", d.Award, amount, suffix)
}

func (s *Server) handleQuest(peer *Peer, msg Message) bool {
	switch msg.Cmd {
	case 1002013, 1002014:
		s.handleGuide(peer, msg)
	case 1016002:
		s.claimQuest(peer, msg)
	case 1012001:
		complete := false
		err := s.roles.Update(peer.currentRoleID(), func(r Record) error {
			complete = s.grantQuestEvent(r, "CHAT")
			return nil
		})
		s.sendGame(peer, msg, s.gameResponse("BuildChatResp_1012001", false, 3003))
		if err == nil && complete {
			s.gameQuestComplete(peer)
		}
	default:
		return false
	}
	return true
}

func (s *Server) claimQuest(peer *Peer, msg Message) {
	req, err := s.decode("ReceiveAwardReq_1016002", msg.Data)
	if err != nil {
		return
	}
	id := getInt(req, "TaskId")
	b := s.gameResponse("ReceiveAwardResp_1016002", true, 2000)
	notice := ""
	complete := false
	err = s.roles.Update(peer.currentRoleID(), func(r Record) error {
		switch {
		case id != roleInt(r, "TaskId"):
			statusPB(b, false, 4601)
			return nil
		case roleInt(r, "TaskStatus") != 1:
			statusPB(b, false, 4602)
			return nil
		}
		if d, found := s.game.quests[id]; found {
			notice = grantGameQuest(r, d)
		}
		r["TaskStatus"] = 0
		if id >= s.game.lastTask {
			r["TaskCompletedAll"] = true
			return nil
		}
		r["TaskId"], r["TaskAutoAt"] = id+1, 0
		for _, field := range taskCounters {
			r[field] = 0
		}
		syncTaskMethods(r)
		if s.questRuleFor(r).Type == "AUTO" {
			r["TaskAutoAt"] = time.Now().Add(5 * time.Second).UnixMilli()
		} else {
			complete = s.checkQuest(r)
		}
		set(b, "Status", roleInt(r, "TaskStatus"))
		return nil
	})
	if err != nil {
		statusPB(b, false, 4007)
	}
	s.sendGame(peer, msg, b)
	if notice != "" && err == nil {
		s.gameNotice(peer, notice)
	}
	if complete && err == nil {
		s.gameQuestComplete(peer)
	}
}

type guideStep struct {
	field string
	last  int64
}

var guideSteps = map[int64]guideStep{
	1: {"PracticeGuideVal", 4}, 2: {"BattleGuideVal", 5}, 5: {"ShopGuideVal", 2},
	6: {"AcupointGuideVal", 3}, 7: {"SignGuideVal", 2}, 8: {"ElixirGuideVal", 2},
}

func (s *Server) handleGuide(peer *Peer, msg Message) {
	name := "GuideInfoUpdate"
	if msg.Cmd == 1002014 {
		name = "GuideInfoSkip"
	}
	suffix := "_" + strconv.Itoa(msg.Cmd)
	req, err := s.decode(name+"Req"+suffix, msg.Data)
	if err != nil {
		return
	}
	b := s.gameResponse(name+"Resp"+suffix, true, 2000)
	kind := getInt(req, "Type")
	step, found := guideSteps[kind]
	practiceUpdate := kind == 1 && msg.Cmd == 1002013
	notice := ""
	err = s.roles.Update(peer.currentRoleID(), func(r Record) error {
		if practiceUpdate {
			if msg.Msid > 0 && msg.Msid <= peer.lastPracticeGuideID {
				return nil
			}
			if practiceStarterPending(r) {
				statusPB(b, false, 4008)
				return nil
			}
		}
		if found {
			value := max(1, roleInt(r, step.field)) + 1
			if msg.Cmd == 1002014 || value >= step.last {
				value = 100
			}
			r[step.field] = value
			notice = rewardGuide(r, kind, value)
		} else if msg.Cmd == 1002014 {
			r["PracticeGuideVal"], r["BattleGuideVal"] = 100, 100
			notice = rewardGuide(r, 1, 100)
			rewardGuide(r, 2, 100)
		}
		return nil
	})
	if err != nil {
		statusPB(b, false, 4007)
	} else if practiceUpdate && msg.Msid > peer.lastPracticeGuideID && getBool(b, "IsSuccess") {
		peer.lastPracticeGuideID = msg.Msid
	}
	s.sendGame(peer, msg, b)
	if notice != "" && err == nil {
		s.gameNotice(peer, notice)
	}
}

func stringRecords(value any) []string {
	switch list := value.(type) {
	case []string:
		return list
	case []any:
		out := make([]string, 0, len(list))
		for _, entry := range list {
			if text, ok := entry.(string); ok {
				out = append(out, text)
			}
		}
		return out
	}
	return nil
}

func rewardGuide(r Record, kind, step int64) string {
	key := fmt.Sprintf("%d_%d", kind, step)
	claimed := stringRecords(r["GuideRewardClaimed"])
	if slices.Contains(claimed, key) {
		return ""
	}
	r["GuideRewardClaimed"] = append(claimed, key)
	stones, jade := int64(100), int64(0)
	message := "引导奖励：灵石×100"
	switch {
	case kind == 1 && step < 100:
		gameAwardGoods(r, "纳气丹", 19, 1, 1, 1)
		message = "引导奖励：灵石×100、纳气丹×1"
	case kind == 1:
		stones, jade = 300, 10
		gameAwardGoods(r, "纳气丹", 19, 1, 1, 3)
		gameAwardGoods(r, "聚元功", 1, 2, 1, 1)
		message = "修行引导完成：灵石×300、灵玉×10、纳气丹×3、聚元功"
	case kind == 2 && step == 2:
		gameAwardGoods(r, "回春散", 19, 1, 1, 1)
		message = "引导奖励：灵石×100、回春散×1"
	case kind == 2 && step >= 100:
		stones, jade = 300, 10
		gameAwardGoods(r, "回春散", 19, 1, 1, 3)
		message = "战斗引导完成：灵石×300、灵玉×10、回春散×3"
	case kind == 6 && step >= 100:
		stones, jade = 300, 10
		message = "经脉引导完成：灵石×300、灵玉×10"
	}
	r["ShinyStone"] = roleInt(r, "ShinyStone") + stones
	if jade > 0 {
		r["ShinyJade"] = roleInt(r, "ShinyJade") + jade
	}
	return message
}
