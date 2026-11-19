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
		Variance:   .08,
		Tiers: map[string]monsterTier{
			"normal": {Blood: .90, Armor: .15, Attack: .022, Mana: .85, Perception: .60, GoldBase: 15, GoldPerLevel: 3},
			"elite":  {Blood: 1.50, Armor: .28, Attack: .038, Mana: 1.30, Perception: .95, GoldBase: 55, GoldPerLevel: 8},
			"boss":   {Blood: 2.40, Armor: .45, Attack: .060, Mana: 2.00, Perception: 1.40, GoldBase: 180, GoldPerLevel: 22},
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

	level := max(1, roleInt(role, "Level"))
	perception := float64(max(100, roleInt(role, "Perception")))
	progress := 0.0
	if limit := s.levelInfo(level).upgradeLimit; limit > 0 {
		progress = max(0, min(1, float64(roleInt(role, "SuperAir"))/limit))
	}
	power := .85 + .15*progress
	difficulty := cfg.Difficulty * max(.1, s.config.MonsterDifficulty)
	waveName := "default"
	mapName := gameMapName(mapID)
	switch {
	case strings.Contains(mapName, "兽王巢"):
		waveName = "lair"
	case strings.Contains(mapName, "深处"):
		waveName = "deep"
	case strings.Contains(mapName, "外围"):
		waveName = "outer"
	}
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
		monsters = append(monsters, battleMonster{
			index: i + 1001, level: int(level), unusual: unusual, objectType: objectType, element: i%5 + 1,
			name: prefix + "·" + baseName, icon: i%8 + 1, speed: speed,
			x: math.Floor(x*10+.5) / 10, y: math.Round(y*10) / 10,
			hp:         max(30, int64(math.Round(perception*tier.Blood*difficulty*variance*power))),
			armor:      max(0, int64(math.Round(perception*tier.Armor*difficulty*variance*power))),
			mana:       max(50, int64(math.Round(perception*tier.Mana*difficulty))),
			perception: min(math.MaxInt32, int64(math.Round(perception*tier.Perception*difficulty))),
			attack:     max(1, int64(math.Round(perception*tier.Attack*difficulty*variance*power))),
			gold: max(1, int64(math.Round(float64(int64(tier.GoldBase)+level*int64(tier.GoldPerLevel))*
				s.config.DropMultiplier*(.85+rand.Float64()*.3)))),
		})
	}
	return monsters
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
	{"skill_hanbing", "寒冰咒", 1, 40, .3, 250, 9, 0, 3, 7},
	{"skill_dingshen", "定身符", 1, 41, .2, 250, 12, 0, 5, 8},
	{"skill_qingfeng", "青锋剑", 2, 15, 1.2, 100, 1, 0, 1, 9},
}

func battleSkillsFor(role Record) []battleSkill {
	settings := battleRecords(role["SkillSettings"])
	chosen := make([]battleSkill, 0, len(battleSkills))
	var used [len(battleSkills)]bool
	for _, setting := range settings {
		id := roleString(setting, "SkillId")
		for i, skill := range battleSkills {
			if skill.id == id && !used[i] {
				chosen = append(chosen, skill)
				used[i] = true
				break
			}
		}
	}
	if len(chosen) == 0 {
		chosen = append(chosen, battleSkills[0], battleSkills[1], battleSkills[8])
		used[0], used[1], used[8] = true, true, true
	}
	for i, skill := range battleSkills {
		if !used[i] {
			chosen = append(chosen, skill)
		}
	}
	return chosen
}

func battleEquipmentBonuses(role Record) (attack, defense int64) {
	weapon, armor, boots := false, false, false
	for _, goods := range battleRecords(role["EquippedGoods"]) {
		rank, level := max(1, roleInt(goods, "Rank")), max(1, roleInt(goods, "Level"))
		switch roleInt(goods, "Type") {
		case 15:
			attack += rank*10 + level*2
			weapon = true
		case 16, 26:
			defense += rank*8 + level*2
			armor = true
		case 29:
			defense += rank*8 + level*2
			boots = true
		}
	}
	if weapon && armor && boots {
		attack += max(1, attack/10)
	}
	if armor && boots {
		defense += max(1, int64(math.Round(float64(defense)*.15)))
	}
	return attack, defense
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
	7: {"束缚", .25, 400, 10, 0},
	8: {"震慑", .25, 500, 12, 0},
}

func (m battleMonster) skill(index int) battleSkill {
	stats := monsterSkills[index]
	if index == 1 && m.unusual == 4 {
		stats.cool = 2
	}
	return battleSkill{
		id: "monster_skill_" + strconv.Itoa(index), name: stats.name,
		kind: index, skillType: 1, multiplier: stats.multiplier,
		castMS: stats.cast, cooldownS: stats.cool, durationMS: stats.duration, element: m.element, icon: index,
	}
}

func setBattleSkillFields(message protoreflect.Message, skill battleSkill, damage float64, level, index, rank int) {
	putPB(message, map[string]any{
		"Icon": skill.icon, "Type": skill.kind, "SkillType": skill.skillType, "Name": skill.name,
		"TemplateId": skill.id, "CastTime": skill.castMS, "ContinueTime": skill.durationMS,
		"CoolDown": skill.cooldownS, "FiveElement": skill.element, "ConversionRate": 1.0, "BaseDamage": damage,
		"Level": level, "SkillIndex": index, "CanUseCount": 9999, "Rank": rank,
		"GoldDamageAdd": 1.0, "WoodDamageAdd": 1.0, "WaterDamageAdd": 1.0, "FireDamageAdd": 1.0, "EarthDamageAdd": 1.0,
	})
}
