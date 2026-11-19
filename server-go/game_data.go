package main

import (
	"encoding/json"
	"errors"
	"fmt"
	"math"
	"os"
	"path/filepath"
	"slices"
	"strings"
)

type gameLevel struct {
	upgradeLimit, topLimit, perception, limitPerception, toughTenacity, limitToughTenacity float64
	trainingPerception, trainingToughTenacity, increaseSuperAir                            float64
	acupoints                                                                              int
}

type gameTables struct {
	levels              [91]gameLevel
	quests              map[int64]questDefinition
	rules               map[int64]questRule
	lastTask            int64
	homes               [5]homeGrade
	transferDiscounts   map[int]int64
	graduationTitleLuck map[int]int64
	acupointTitleBonus  map[int]int
	acupoints           map[string]acupointRule
}

func (s *Server) readGameTable(name string, output any) error {
	body, err := readLegacyFile(filepath.Join(s.dataDir, "config", name))
	if errors.Is(err, os.ErrNotExist) {
		return nil
	}
	if err != nil {
		return fmt.Errorf("读取 %s: %w", name, err)
	}
	if err = json.Unmarshal(body, output); err != nil {
		return fmt.Errorf("读取 %s: %w", name, err)
	}
	return nil
}

func (s *Server) loadGameTables() error {
	var levels []Record
	if err := s.readGameTable("xzjrolelevellist.json", &levels); err != nil {
		return err
	}
	for _, record := range levels {
		id := roleInt(record, "id")
		if id < 1 || id > 90 {
			continue
		}
		l := gameLevel{
			upgradeLimit:     numericDefault(record, "upgradeLimit", 100),
			perception:       numericDefault(record, "perception", 500),
			toughTenacity:    numericDefault(record, "toughTenacity", 500),
			increaseSuperAir: numericDefault(record, "increaseSuperAir", 10),
			acupoints:        int(numericDefault(record, "acupointPromotePoint", 3)),
		}
		l.topLimit = numericDefault(record, "topLimit", l.upgradeLimit*1.3)
		l.limitPerception = numericDefault(record, "limitPerception", l.perception)
		l.limitToughTenacity = numericDefault(record, "limitToughTenacity", l.toughTenacity)
		l.trainingPerception = numericDefault(record, "trainingPerception", l.perception)
		l.trainingToughTenacity = numericDefault(record, "trainingToughTenacity", l.toughTenacity)
		s.game.levels[id] = l
	}
	for id := 1; id <= 90; id++ {
		if s.game.levels[id].upgradeLimit != 0 {
			continue
		}
		upgrade := 100 * math.Pow(1.5, float64(id-1))
		perception := 500 * math.Pow(1.18, float64(id-1))
		acupoints := 3
		if id%10 == 1 {
			acupoints = 10
		}
		s.game.levels[id] = gameLevel{
			upgradeLimit: upgrade, topLimit: upgrade * 1.3, perception: perception, limitPerception: perception,
			toughTenacity: perception, limitToughTenacity: perception, trainingPerception: perception,
			trainingToughTenacity: perception, increaseSuperAir: upgrade * 0.1, acupoints: acupoints,
		}
	}
	if err := s.loadHomeGrades(); err != nil {
		return err
	}
	if err := s.loadSocialTitles(); err != nil {
		return err
	}
	if err := s.loadAcupoints(); err != nil {
		return err
	}
	return s.loadQuests()
}

func numericDefault(r Record, key string, fallback float64) float64 {
	if r[key] == nil {
		return fallback
	}
	return roleFloat(r, key)
}

func (s *Server) levelInfo(level int64) gameLevel {
	return s.game.levels[max(1, min(90, level))]
}

func gameLevelName(level int64) string {
	stages := [...]string{"练气", "筑基", "结丹", "元婴", "分神", "炼虚", "合体", "渡劫", "大乘"}
	level = max(1, min(90, level))
	return fmt.Sprintf("%s%d层", stages[(level-1)/10], (level-1)%10+1)
}

func gameLifespan(level int64) int64 {
	return 100 * (1 << min(8, max(0, (level-1)/10)))
}

type gameMethod struct {
	id, name                   string
	kind, rank, element        int
	learningLevel              int
	baseCircle                 float64
	effect1, effect2           int
	effectValue1, effectValue2 int
}

var methods = buildGameMethods()

func buildGameMethods() []gameMethod {
	var defs []gameMethod
	names := [...]string{
		"凝神诀 观想图 清心咒 太乙炼神篇 识海无涯经 紫府凝神法 神魂淬炼诀 九幽炼魂经 太虚神识篇 万象归识经 星辰炼神法 混沌神魂诀 太初元神道 鸿蒙神识经 不灭元神篇 天道神魂法",
		"铁骨功 淬体诀 金刚不坏体 龙象般若功 玄龟炼体术 蛮荒战体 不灭金身诀 九幽淬骨经 星辰炼体篇 万劫不坏身 盘古真身诀 混沌魔躯 太初道体 鸿蒙圣体 不灭道躯 天道法身",
		"启灵经 聚元功 青木长生诀 玄水真经 赤炎焚天功 厚土载物诀 紫气东来篇 太虚纳灵经 九转归元功 混元一气诀 大衍聚灵篇 万象归宗经 太初吞天诀 鸿蒙纳气经 混沌无极功 天道归元法",
	}
	effects := [...][2]int{{101, 102}, {301, 302}, {105, 601}}
	for i, group := range names {
		for offset, name := range strings.Fields(group) {
			rank := offset + 1
			tier := offset / 4
			base := 0.8 * [...]float64{1, 4, 16, 64}[tier] * [...]float64{1, 1.1, 1.2, 1.3}[offset%4]
			defs = append(defs, gameMethod{
				fmt.Sprintf("gf_%d_%d", i+1, rank), name, i + 1, rank, offset%5 + 1, tier*10 + 1,
				base, effects[i][0], effects[i][1], 5 + rank, 2 + rank/2,
			})
		}
	}
	return defs
}

func findGameMethod(id string) *gameMethod {
	for i := range methods {
		if def := &methods[i]; def.id == id {
			return def
		}
	}
	return nil
}

func ownedGameMethods(r Record) []string {
	switch list := r["GongFaOwned"].(type) {
	case []string:
		return list
	case []any:
		out := make([]string, 0, len(list))
		for _, entry := range list {
			if value, ok := entry.(string); ok {
				out = append(out, value)
			}
		}
		return out
	}
	return nil
}

func learnGameMethod(r Record, def *gameMethod) {
	owned := ownedGameMethods(r)
	if !slices.Contains(owned, def.id) {
		r["GongFaOwned"] = append(owned, def.id)
	}
}

func gameEquipField(kind int) string {
	return [...]string{"", "GongFaLianShen", "GongFaDuanTi", "GongFaNaQi"}[kind]
}

func equippedGameMethod(r Record, kind int) *gameMethod {
	return findGameMethod(roleString(r, gameEquipField(kind)))
}

func ensureGamesMethods(r Record) {
	owned := ownedGameMethods(r)
	if len(owned) == 0 {
		owned = make([]string, 0, len(methods))
		for _, def := range methods {
			owned = append(owned, def.id)
		}
		r["GongFaOwned"] = owned
	}
	if _, ok := r["GongFaChong"].(map[string]any); !ok {
		r["GongFaChong"] = Record{}
	}
	for kind := 1; kind <= 3; kind++ {
		if def := equippedGameMethod(r, kind); def != nil && def.kind == kind {
			continue
		}
		fallback := &methods[(kind-1)*16]
		for _, def := range methods[(kind-1)*16 : kind*16] {
			if slices.Contains(owned, def.id) {
				fallback = findGameMethod(def.id)
				break
			}
		}
		learnGameMethod(r, fallback)
		r[gameEquipField(kind)] = fallback.id
	}
}

func gameRunMask(location int64) int64 {
	switch location {
	case 1, 2:
		return location
	case 4:
		return 6
	case 5:
		return 3
	case 6:
		return 5
	default:
		return 4
	}
}

func gameLocation(mask int64) int64 {
	return [...]int64{3, 1, 2, 5, 3, 6, 4, 3}[mask&7]
}

func gameMethodMultiplier(r Record, def *gameMethod) float64 {
	chong, _ := r["GongFaChong"].(map[string]any)
	switch roleInt(chong, def.id) {
	case 0, 1:
		return 1
	case 2:
		return 1.2
	default:
		return 1.5
	}
}

func (s *Server) gameRate(r Record, kind int) float64 {
	level := max(1, min(90, roleInt(r, "Level")))
	lvl := s.levelInfo(level)
	gongFa := 0.5
	if def := equippedGameMethod(r, kind); def != nil {
		gongFa = def.baseCircle / 0.8 * gameMethodMultiplier(r, def)
	}
	rootCount := 0
	for _, element := range []string{"HasGold", "HasWood", "HasWater", "HasFire", "HasEarth"} {
		if roleBool(r, element) {
			rootCount++
		}
	}
	rootFactor := [...]float64{1, 1, .75, .60, .50, .45}[rootCount]
	matrix := 1 + 0.1*float64(max(0, min(4, roleInt(r, "DongFuRank"))))
	matrix *= 1 + 0.1*float64(max(0, min(4, roleInt(r, "JuLingRank"))))
	if rootCount == 1 {
		matrix *= 1 + 0.1*float64(max(0, min(4, roleInt(r, "KongLingRank"))))
	}
	efficiency := 1.0
	if mask := roleInt(r, "RunMask"); mask&(mask-1) != 0 {
		efficiency = .7
	}
	bonus := matrix * rootFactor * efficiency * s.config.CultivationSpeed
	seconds := (250 + 10*float64(level-1)) / math.Max(0.05, gongFa*bonus)
	return lvl.upgradeLimit / math.Max(1, seconds)
}
