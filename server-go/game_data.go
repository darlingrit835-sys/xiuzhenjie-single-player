package main

import (
	"encoding/json"
	"errors"
	"fmt"
	"math"
	"os"
	"path/filepath"
	"slices"
	"strconv"
	"strings"
)

type gameLevel struct {
	upgradeLimit, topLimit, perception, limitPerception, toughTenacity, limitToughTenacity float64
	trainingPerception, trainingToughTenacity, increaseSuperAir                            float64
	acupoints                                                                              int
}

type gameTables struct {
	methods             *methodCatalog
	levels              [91]gameLevel
	quests              map[int64]questDefinition
	rules               map[int64]questRule
	lastTask            int64
	homes               [5]homeGrade
	transferDiscounts   map[int]int64
	auctionDiscounts    map[int]int64
	graduationTitleLuck map[int]int64
	acupointTitleBonus  map[int]int
	cultivationTitles   map[int]int
	acupoints           map[string]acupointRule
	meridianTotals      [8]int
	trialShop           map[int64]trialShopProduct
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
	if err := s.loadSystemMethods(); err != nil {
		return err
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
	if err := s.loadTrialShop(); err != nil {
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
	moveSpeedPercent           int
	system                     bool
}

func buildGameMethods() []gameMethod {
	var defs []gameMethod
	names := [...]string{
		"凝神诀 观想图 清心咒 太乙炼神篇 识海无涯经 紫府凝神法 神魂淬炼诀 九幽炼魂经 太虚神识篇 万象归识经 星辰炼神法 混沌神魂诀 太初元神道 鸿蒙神识经 不灭元神篇 天道神魂法",
		"铁骨功 淬体诀 金刚不坏体 龙象般若功 玄龟炼体术 蛮荒战体 不灭金身诀 九幽淬骨经 星辰炼体篇 万劫不坏身 盘古真身诀 混沌魔躯 太初道体 鸿蒙圣体 不灭道躯 天道法身",
		"启灵经 聚元功 青木长生诀 玄水真经 赤炎焚天功 厚土载物诀 紫气东来篇 太虚纳灵经 九转归元功 混元一气诀 大衍聚灵篇 万象归宗经 太初吞天诀 鸿蒙纳气经 混沌无极功 天道归元法",
	}

	for i, group := range names {
		for offset, name := range strings.Fields(group) {
			rank := offset + 1
			tier := offset / 4
			base := 0.8 * [...]float64{1, 4, 16, 64}[tier] * [...]float64{1, 1.1, 1.2, 1.3}[offset%4]
			element := offset%5 + 1
			first, second := 105+element, 106+(element%5)
			if i == 1 {
				first, second = 300+element, 301+(element%5)
			} else if i == 2 {
				second = 601
			}
			defs = append(defs, gameMethod{
				fmt.Sprintf("gf_%d_%d", i+1, rank), name, i + 1, rank, offset%5 + 1, tier*10 + 1,
				base, first, second, 5 + rank, 2 + rank/2, 0, false,
			})
		}
	}
	return defs
}

func ownedGameMethods(r Record) []string {
	return stringRecords(r["GongFaOwned"])
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

func (s *Server) ensureGamesMethods(r Record) {
	if _, ok := r["GongFaChong"].(map[string]any); !ok {
		r["GongFaChong"] = Record{}
	}
	if practiceStarterPending(r) {
		// The original tutorial checkpoint predates the separate read-book list.
		r["PracticeStarterRead"] = true
	}
	for kind := 1; kind <= 3; kind++ {
		if def := s.methods().equipped(r, kind); def != nil {
			// Older saves can retain an equipped technique without a matching ownership record.
			learnGameMethod(r, def)
		}
	}
}

const methodMoveSpeedEffect = 202

// Combat and item settlement also run outside a Server method. Save only derived
// effects from the server's equipped catalogue, refreshed on login and loadout changes.
func (s *Server) refreshMethodAffixes(r Record) {
	effects := Record{}
	for kind := 1; kind <= 3; kind++ {
		if m := s.methods().equipped(r, kind); m != nil {
			for _, stat := range [2][2]int{{m.effect1, m.effectValue1}, {m.effect2, m.effectValue2}} {
				key := strconv.Itoa(stat[0])
				effects[key] = min(int64(35), roleInt(effects, key)+int64(stat[1]))
			}
			if m.moveSpeedPercent > 0 {
				key := strconv.Itoa(methodMoveSpeedEffect)
				effects[key] = min(int64(35), roleInt(effects, key)+int64(m.moveSpeedPercent))
			}
		}
	}
	r["EquippedMethodEffects"] = effects
}

func methodAffixPercent(r Record, effect int) int {
	effects, _ := r["EquippedMethodEffects"].(Record)
	return int(max(0, min(35, roleInt(effects, strconv.Itoa(effect)))))
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
	return s.gameRateFor(r, s.methods().equipped(r, kind), kind)
}

var cultivationRootFields = [...]string{"", "HasGold", "HasWood", "HasWater", "HasFire", "HasEarth"}

// 金生水、木生火、水生木、火生土、土生金。
var generatingElement = [...]int{0, 3, 4, 2, 5, 1}

func (s *Server) cultivationAffinity(r Record, def *gameMethod) float64 {
	if def == nil {
		return 1
	}
	element := def.elementFor(r)
	if element < 1 || element > 5 {
		return 1
	}
	factor := 1.0
	if roleBool(r, cultivationRootFields[element]) {
		factor = 1.2
	}
	mask := roleInt(r, "RunMask")
	if mask&(1<<(def.kind-1)) == 0 || mask&(mask-1) == 0 {
		return factor
	}
	for kind := 1; kind <= 3; kind++ {
		if kind != def.kind && mask&(1<<(kind-1)) != 0 {
			other := s.methods().equipped(r, kind)
			if other != nil && generatingElement[other.elementFor(r)] == element {
				return factor * 1.2
			}
		}
	}
	return factor
}

func (s *Server) gameRateFor(r Record, def *gameMethod, kind int) float64 {
	level := max(1, min(90, roleInt(r, "Level")))
	lvl := s.levelInfo(level)
	gongFa := 0.65
	if def != nil {
		gongFa = (1 + .20*math.Log2(altarMethodCircle(r, def)/.8)) * gameMethodMultiplier(r, def) * s.cultivationAffinity(r, def)
	}
	rootCount := 0
	for _, element := range cultivationRootFields[1:] {
		if roleBool(r, element) {
			rootCount++
		}
	}
	rootFactor := [...]float64{1, 1, .96, .92, .87, .82}[rootCount]
	matrix := 1 + 0.1*float64(max(0, min(4, roleInt(r, "DongFuRank"))))
	matrix *= matrixBenefits(r)
	efficiency := 1.0
	if mask := roleInt(r, "RunMask"); mask&(mask-1) != 0 {
		efficiency = .7
	}
	bonus := matrix * rootFactor * efficiency * s.config.CultivationSpeed
	reference := 1.2 * (1 + .4*float64(min(3, (level-1)/10)))
	seconds := cultivationSeconds(level) * reference / math.Max(.01, gongFa*bonus)
	// The exercise-room protocol reports a two-second circle; body and spirit
	// convert half a circle into permanent attributes in both settlement paths.
	if kind != 3 {
		base := lvl.trainingBase(kind)
		return 2 * (float64(lvl.trainingCap(kind)) - base) / (.8 * seconds * attributeTimeFactor(level))
	}
	rate := .9 * lvl.upgradeLimit / (seconds * lateRealmTimeFactor(level))
	p := s.meridianProgress(r)
	rate *= (100 + p.bonus[1]) / (100 - p.bonus[0])
	rate *= 1 + float64(artisanResearchRank(r, 6))/100
	rate *= 1 + float64(s.cultivationTitlePercent(r))/100
	return rate
}

// Scale breakthrough time smoothly across four realms. Every source of
// advancement uses the recipient's current level; spendable battle mana,
// permanent attribute training and the client-side threshold are independent.
func lateRealmTimeFactor(level int64) float64 {
	if level <= 40 {
		return 1
	}
	if level >= 80 {
		return 100
	}
	return math.Pow(100, float64(level-40)/40)
}

// Body and spirit peak at ten times the early-era training time, keeping combat
// improvements attainable between two breakthroughs.
func attributeTimeFactor(level int64) float64 { return math.Sqrt(lateRealmTimeFactor(level)) }

func breakthroughAttributePercent(level int64) int64 {
	if level < 20 || level >= 90 || level%10 != 0 {
		return 100
	}
	return 100 + 2*(level/10-1)
}
func (s *Server) breakthroughAttributeCode(r Record) int {
	l := roleInt(r, "Level")
	pct := breakthroughAttributePercent(l)
	if pct == 100 {
		return 0
	}
	lvl := s.levelInfo(l)
	if roleInt(r, "Perception") < attributeTarget(lvl, 1, pct) || roleInt(r, "Physique") < attributeTarget(lvl, 2, pct) {
		return 4985
	}
	return 0
}
func attributeTarget(lvl gameLevel, kind int, pct int64) int64 {
	return min(lvl.trainingCap(kind), int64(math.Ceil(lvl.trainingBase(kind)*float64(pct)/100)))
}

// Base training time before applying late-realm pacing, measured from the
// 10% reserve with a matched method, one root and no paid facilities.
func cultivationSeconds(level int64) float64 {
	starts := [...]float64{110, 180, 300, 480, 720, 1080, 1560, 2160, 3000}
	stage, step := (level-1)/10, (level-1)%10
	seconds := starts[stage] * (1 + .045*float64(step))
	if step == 9 && level < 90 {
		seconds *= 1.25
	}
	return seconds
}

func (lvl gameLevel) trainingBase(kind int) float64 {
	if kind == 1 {
		return lvl.perception
	}
	return lvl.toughTenacity
}

func (lvl gameLevel) trainingCap(kind int) int64 {
	return int64(math.Round(lvl.trainingBase(kind) * 1.40))
}
