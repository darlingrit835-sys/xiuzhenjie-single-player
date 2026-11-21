package main

import (
	"encoding/json"
	"errors"
	"log"
	"math"
	"math/rand"
	"os"
	"path/filepath"
	"strconv"
	"strings"
	"time"

	"google.golang.org/protobuf/reflect/protoreflect"
)

type monsterTier struct {
	Blood, Armor, Attack, Mana, Perception float64
	GoldBase, GoldPerLevel                 int
}

type monsterWave struct {
	Normal, Elite, Boss int
}

type monsterConfiguration struct {
	Difficulty, Variance float64
	Tiers                map[string]monsterTier
	Waves                map[string]monsterWave
}

func defaultMonsterConfiguration() monsterConfiguration {
	return monsterConfiguration{
		Difficulty: 1,
		Variance:   .06,
		Tiers: map[string]monsterTier{
			"normal": {Blood: 5.0, Armor: .68, Attack: .60, Mana: .85, Perception: .60, GoldBase: 12, GoldPerLevel: 2},
			"elite":  {Blood: 11, Armor: 1.60, Attack: 1.45, Mana: 1.30, Perception: .95, GoldBase: 35, GoldPerLevel: 5},
			"boss":   {Blood: 23.5, Armor: 3.80, Attack: 3.05, Mana: 2.00, Perception: 1.40, GoldBase: 86, GoldPerLevel: 13},
		},
		Waves: map[string]monsterWave{
			"outer":   {Normal: 5, Elite: 1},
			"deep":    {Normal: 4, Elite: 1, Boss: 1},
			"lair":    {Normal: 1, Elite: 2, Boss: 2},
			"default": {Normal: 4, Elite: 1},
		},
	}
}

func readMonsterConfiguration(dir string) monsterConfiguration {
	defaults := defaultMonsterConfiguration()
	content, err := os.ReadFile(filepath.Join(dir, "config", "monsters.json"))
	if err != nil {
		if !errors.Is(err, os.ErrNotExist) {
			log.Printf("monster config: %v", err)
		}
		return defaults
	}
	cfg := monsterConfiguration{
		Difficulty: defaults.Difficulty,
		Variance:   defaults.Variance,
	}
	if err := json.Unmarshal(content, &cfg); err != nil {
		log.Printf("monster config: %v", err)
		return defaults
	}
	if cfg.Difficulty <= 0 {
		cfg.Difficulty = defaults.Difficulty
	}
	if cfg.Variance < 0 {
		cfg.Variance = defaults.Variance
	}
	for name, fallback := range defaults.Tiers {
		value, ok := cfg.Tiers[name]
		if !ok {
			continue
		}
		if value.Blood <= 0 {
			value.Blood = fallback.Blood
		}
		if value.Armor < 0 {
			value.Armor = fallback.Armor
		}
		if value.Attack <= 0 {
			value.Attack = fallback.Attack
		}
		if value.Mana <= 0 {
			value.Mana = fallback.Mana
		}
		if value.Perception <= 0 {
			value.Perception = fallback.Perception
		}
		if value.GoldBase <= 0 {
			value.GoldBase = fallback.GoldBase
		}
		if value.GoldPerLevel <= 0 {
			value.GoldPerLevel = fallback.GoldPerLevel
		}
		defaults.Tiers[name] = value
	}
	for name, wave := range cfg.Waves {
		if wave.Normal > 0 || wave.Elite > 0 || wave.Boss > 0 {
			defaults.Waves[name] = wave
		}
	}
	defaults.Difficulty, defaults.Variance = cfg.Difficulty, cfg.Variance
	return defaults
}

type battleMonster struct {
	herbDrops                                  []fieldHerbDrop
	index, level, unusual, objectType, element int
	name                                       string
	icon                                       int
	x, y, speed                                float64
	hp, armor, mana, perception, attack, gold  int64
}

var monsterAnimals = [...]string{"赤炎狼", "玄水蟒", "青木猿", "厚土熊", "金翅雕", "幽冥狐", "雷纹豹", "白泽幼兽"}
var monsterPeople = [...]string{"散修", "邪修", "魔修", "妖修", "剑修", "刀客", "阵师", "丹师"}
var monsterPrefixes = [...]string{"幼年", "成年"}

func (s *Server) createMonsters(role Record, mapID string) []battleMonster {
	state := &s.battleState
	state.loadOnce.Do(func() { state.balance = readMonsterConfiguration(s.dataDir) })
	cfg := state.balance

	level, waveName := encounterLevel(roleInt(role, "Level"), mapID)
	base := max(100, s.levelInfo(level).perception)
	difficulty := cfg.Difficulty * max(.1, s.config.MonsterDifficulty)
	wave := cfg.Waves[waveName]
	normal, elite, boss := max(0, wave.Normal), max(0, wave.Elite), max(0, wave.Boss)
	waveSize := max(1, normal+elite+boss)
	count := max(1, s.config.MonsterCount)

	monsters := make([]battleMonster, 0, count)
	for i := range count {
		slot := i % waveSize
		tierName, prefix, unusual, speed := "normal", monsterPrefixes[i%len(monsterPrefixes)], 1, 4.4
		if slot >= normal+elite {
			tierName, prefix, unusual, speed = "boss", "领主", 4, 3.3
		} else if slot >= normal {
			tierName, prefix, unusual, speed = "elite", "精英", 3, 3.9
		}
		tier := cfg.Tiers[tierName]
		variance := 1 + (rand.Float64()*2-1)*cfg.Variance
		baseName, objectType := monsterAnimals[i%len(monsterAnimals)], 2002
		if i%3 == 2 && unusual != 4 {
			baseName, objectType = monsterPeople[i%len(monsterPeople)], 2001
		}
		x := -8 + 16*(float64(i)+.5)/float64(count)
		y := .5 + 5*float64(i%3)/2
		m := monsterAtLevel(level, base, tier, difficulty, variance, s.config.DropMultiplier*(.85+rand.Float64()*.3))
		m.index, m.unusual, m.objectType, m.element = i+1001, unusual, objectType, i%5+1
		m.name, m.icon, m.speed = prefix+"·"+baseName, i%8+1, speed
		m.x, m.y = math.Floor(x*10+.5)/10, math.Round(y*10)/10
		monsters = append(monsters, m)
	}
	return monsters
}

// Forest pairs offer three level bands in every county. Choosing a harder
// area can cross a realm boundary, without scaling enemies to the player's gear.
func encounterLevel(playerLevel int64, mapID string) (int64, string) {
	_, path, ok := strings.Cut(mapID, "W")
	if !ok || gameMapFind(mapID) == nil || len(path) == 0 {
		return max(1, min(90, playerLevel)), "default"
	}
	offset := int64(path[0]-'1') / 2
	wave := "default"
	if len(path) == 2 {
		offset += int64(path[1]) - int64('2')
		wave = [...]string{"outer", "deep", "lair"}[path[1]-'1']
	}
	return max(1, min(90, playerLevel+offset)), wave
}

func monsterAtLevel(level int64, base float64, tier monsterTier, difficulty, variance, payout float64) battleMonster {
	pressure := difficulty * variance * (.92 + .0033*float64(level-1))
	return battleMonster{
		level:      int(level),
		hp:         max(30, int64(math.Round(base*tier.Blood*pressure))),
		armor:      max(0, int64(math.Round(base*tier.Armor*pressure))),
		attack:     max(1, int64(math.Round(base*tier.Attack*pressure))),
		mana:       max(50, int64(math.Round(base*tier.Mana*difficulty))),
		perception: min(math.MaxInt32, int64(math.Round(base*tier.Perception*difficulty))),
		gold:       max(1, int64(math.Round(float64(tier.GoldBase+int(level)*tier.GoldPerLevel)*payout))),
	}
}

type battleSkill struct {
	id, name                                     string
	kind, skillType                              int
	multiplier                                   float64
	castMS, cooldownS, durationMS, element, icon int
}

var battleSkills = [...]battleSkill{
	{"skill_yujian", "御剑术", 1, 10, 1, 100, 1, 0, 1, 1},
	{"skill_liehuo", "烈火咒", 1, 12, 1.8, 300, 7, 5000, 4, 2},
	{"skill_fentian", "焚天诀", 1, 14, 1.2, 500, 10, 4000, 4, 3},
	{"skill_jingang", "金刚护体", 1, 20, 2, 300, 14, 8000, 5, 4},
	{"skill_juling", "聚灵诀", 1, 30, 0, 200, 14, 8000, 2, 5},
	{"skill_xuangui", "玄龟诀", 1, 31, 0, 200, 14, 8000, 3, 6},
	{"skill_hanbing", "寒冰咒", 1, 40, .3, 250, 9, 2000, 3, 7},
	{"skill_dingshen", "定身符", 1, 41, 0, 250, 12, 1000, 5, 8},
	{"skill_qingfeng", "青锋剑", 2, 15, 1.2, 100, 1, 0, 1, 9},
	{"skill_feimang", "碎金芒", 1, 10, .78, 70, 1, 0, 1, 1},
	{"skill_jianyu", "庚金剑雨", 1, 14, 1.55, 650, 12, 5000, 1, 3},
	{"skill_jinzhao", "金钟罩", 1, 20, 2.8, 650, 20, 10000, 1, 4},
	{"skill_tengshi", "藤蚀诀", 1, 12, 1.1, 180, 5, 7000, 2, 2},
	{"skill_fugen", "缚根咒", 1, 41, 0, 180, 8, 1000, 2, 8},
	{"skill_muye", "木叶飞针", 1, 10, 1.25, 190, 2, 0, 2, 1},
	{"skill_chaoxi", "潮汐阵", 1, 14, .88, 260, 6, 3500, 3, 3},
	{"skill_bingjia", "玄冰甲", 1, 20, 1.4, 150, 8, 5000, 3, 4},
	{"skill_zhuyang", "逐阳指", 1, 10, 2.65, 620, 6, 0, 4, 1},
	{"skill_chixin", "赤心印", 1, 30, 0, 120, 18, 6000, 4, 5},
	{"skill_luoshi", "落石诀", 1, 14, 2.2, 850, 15, 6000, 5, 3},
	{"skill_liusha", "流沙咒", 1, 40, .34, 330, 6, 2000, 5, 7},
	{"skill_tafeng", "踏风诀", 1, 50, .25, 180, 12, 6000, 2, 50},
}

// The preparation screen stores three groups of three zero-based slots.
type battleLoadout [9]battleSkill

func battleSkillByID(id string) (battleSkill, bool) {
	for _, skill := range battleSkills {
		if skill.id == id {
			return skill, true
		}
	}
	return battleSkill{}, false
}

func battleLoadoutFor(role Record) battleLoadout {
	var slots battleLoadout
	if role["SkillSettings"] == nil {
		slots[0], slots[1], slots[2] = battleSkills[0], battleSkills[1], battleSkills[8]
		return slots
	}
	used := make(map[string]bool)
	for _, setting := range battleRecords(role["SkillSettings"]) {
		index := roleInt(setting, "CombatSettingId")
		id := roleString(setting, "SkillId")
		if index < 0 || index >= int64(len(slots)) || slots[index].id != "" || used[id] {
			continue
		}
		if skill, ok := battleSkillByID(id); ok {
			slots[index] = skill
			used[id] = true
		}
	}
	return slots
}

// Legacy clients select a group by repeated-field position. Keep interior gaps
// and omit trailing empty slots, including the entire field for an empty loadout.
func (slots *battleLoadout) inBattle() []battleSkill {
	n := len(slots)
	for n > 0 && slots[n-1].id == "" {
		n--
	}
	return slots[:n]
}

func (m battleMonster) skillTypes() []int {
	switch m.unusual {
	case 4:
		return []int{1, 4, 2, 3, 5, 8}
	case 3:
		return []int{1, 4, 2, 7}
	default:
		return []int{1, 2}
	}
}

type monsterSkillStats struct {
	name                 string
	multiplier           float64
	cast, cool, duration int
}

var monsterSkills = [...]monsterSkillStats{
	1: {"扑咬", 1, 300, 3, 0},
	2: {"噬咬", .55, 300, 8, 5000},
	3: {"冲锋", .45, 400, 10, 0},
	4: {"妖术", 1.15, 800, 6, 0},
	5: {"厚皮", .10, 500, 15, 8000},
	6: {"吞噬", .10, 500, 15, 8000},
	7: {"束缚", .25, 400, 10, 3000},
	8: {"震慑", .25, 500, 12, 3000},
}

func (m battleMonster) skill(index int) battleSkill {
	stats := monsterSkills[index]
	if index == 1 && m.unusual == 4 {
		stats.cool = 2
	}
	if m.objectType == 2001 {
		// Human AI dispatches FaShuType (10, 12, ...), while beast AI dispatches the
		// monster ability slot. Reuse the eight matching cultivator spell templates.
		spell := battleSkills[index-1]
		if !spell.affectsMovement() {
			spell.multiplier = stats.multiplier
		}
		spell.castMS, spell.cooldownS = stats.cast, stats.cool
		return spell
	}
	return battleSkill{
		id: "monster_skill_" + strconv.Itoa(index), name: stats.name,
		kind: index, skillType: index, multiplier: stats.multiplier,
		castMS: stats.cast, cooldownS: stats.cool, durationMS: stats.duration, element: m.element, icon: index,
	}
}

func setBattleSkillFields(message protoreflect.Message, skill battleSkill, damage float64, level, index, rank int, cooldownUnit time.Duration) {
	fields := Record{
		"Icon": skill.icon, "Type": skill.kind, "SkillType": skill.skillType, "Name": skill.name,
		"TemplateId": skill.id, "CastTime": skill.castMS, "ContinueTime": skill.durationMS,
		"CoolDown": int(time.Duration(skill.cooldownS) * time.Second / cooldownUnit), "FiveElement": skill.element, "ConversionRate": 1.0, "BaseDamage": damage,
		"Level": level, "SkillIndex": index, "CanUseCount": 9999, "Rank": rank,
		"GoldDamageAdd": 1.0, "WoodDamageAdd": 1.0, "WaterDamageAdd": 1.0, "FireDamageAdd": 1.0, "EarthDamageAdd": 1.0,
	}
	// The tower continuation schema omits optional Rank and elemental bonuses.
	for name, value := range fields {
		if message.Descriptor().Fields().ByName(protoreflect.Name(name)) != nil {
			set(message, name, value)
		}
	}
}
