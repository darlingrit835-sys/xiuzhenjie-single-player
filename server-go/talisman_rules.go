package main

import (
	"fmt"
	"math"
	"strings"
)

const (
	sigilReed  = "mat.frost_reed"
	sigilHerb  = "mat.ninglu_herb"
	sigilDew   = "mat.cold_dew"
	sigilLotus = "mat.dream_lotus"
	sigilPlume = "mat.gold_plume"
	sigilWood  = "mat.cloudwood"
	sigilPaper = "mat.shuanglu_paper"
	sigilInk   = "mat.chenglu_ink"
	sigilBrush = "obj.jinling_brush"
)

type sigilMaterial struct {
	id, name, source    string
	kind, rank, element int
}

var sigilMaterials = []sigilMaterial{
	{sigilReed, "霜节芦", "灵田种植、寒渊采集或领取基础料包", 12, 2, 1},
	{sigilHerb, "凝露草", "灵田种植、朝露采集或领取基础料包", 12, 2, 2},
	{sigilDew, "寒露液", "凝露草凝炼或领取基础料包", 12, 3, 3},
	{sigilLotus, "梦泽莲", "灵田种植，也可在云梦采集", 12, 2, 3},
	{sigilPlume, "金翎", "符案购入笔料", 7, 2, 1},
	{sigilWood, "云纹木", "符案购入笔料、炼器工坊领料", 12, 2, 2},
	{sigilPaper, "霜芦符纸", "以霜节芦捣浆抄纸", 23, 2, 1},
	{sigilInk, "澄露墨", "以凝露草、寒露液调墨，或用莲瓣研浓墨", 24, 3, 3},
	{sigilBrush, "金翎符笔", "以金翎扎锋、云纹木作杆。留在纳戒即可执笔", 22, 5, 1},
}

func sigilMat(id string) sigilMaterial {
	for _, m := range sigilMaterials {
		if m.id == id {
			return m
		}
	}
	return sigilMaterial{}
}

type sigilUnit struct {
	id string
	n  int
}
type sigilRecipe struct {
	id, name        string
	inputs, outputs []sigilUnit
	stones          int
}

var sigilRecipes = []sigilRecipe{
	{"paper", "抄纸", []sigilUnit{{sigilReed, 3}}, []sigilUnit{{sigilPaper, 3}}, 0},
	{"ink", "调墨", []sigilUnit{{sigilHerb, 2}, {sigilDew, 1}}, []sigilUnit{{sigilInk, 2}}, 0},
	{"lotusInk", "研莲墨", []sigilUnit{{sigilLotus, 1}, {sigilDew, 1}}, []sigilUnit{{sigilInk, 3}}, 0},
	{"dew", "凝露", []sigilUnit{{sigilHerb, 3}}, []sigilUnit{{sigilDew, 1}}, 0},
	{"brushKit", "购入笔料", nil, []sigilUnit{{sigilPlume, 1}, {sigilWood, 1}}, 90},
	{"brush", "扎制符笔", []sigilUnit{{sigilPlume, 1}, {sigilWood, 1}}, []sigilUnit{{sigilBrush, 1}}, 0},
}
var sigilStarter = []sigilUnit{{sigilReed, 6}, {sigilHerb, 4}, {sigilDew, 2}}

func sigilMultiply(lines []sigilUnit, n int) []sigilUnit {
	out := make([]sigilUnit, len(lines))
	for i, line := range lines {
		out[i] = sigilUnit{line.id, line.n * n}
	}
	return out
}

func sigilMastery(r Record, key string) int {
	return int(min(100, max(0, roleInt(farmMap(r, "TalismanMastery"), key))))
}

func sigilPractice(r Record, key string, points int) {
	m := farmMap(r, "TalismanMastery")
	if m == nil {
		m = Record{}
		r["TalismanMastery"] = m
	}
	m[key] = min(100, sigilMastery(r, key)+points)
}

func sigilUnitText(lines []sigilUnit) string {
	parts := make([]string, len(lines))
	for i, line := range lines {
		parts[i] = fmt.Sprintf("%s ×%d", sigilMat(line.id).name, line.n)
	}
	return strings.Join(parts, "、")
}

// Coordinates are percentages of the sheet, with the origin at its upper left.
// The semantic middle stroke and the closing seal compose independently.
type sigilPoint struct {
	X int `json:"x"`
	Y int `json:"y"`
}
type sigilStroke struct {
	Name   string       `json:"name"`
	Points []sigilPoint `json:"points"`
}
type sigilEffect struct {
	id, name, description string
	base, element         int
	stroke                sigilStroke
}

var sigilEffects = []sigilEffect{
	{"ward", "护身", "下次进入历练战斗获得一次护盾，同类护盾取高值", 30, 5, sigilStroke{"折锋护身", []sigilPoint{{25, 35}, {75, 35}, {72, 54}, {50, 68}, {29, 54}}}},
	{"cleanse", "净秽", "立即消去身上积累的毒性，无毒时保留此符", 12, 3, sigilStroke{"撇捺涤秽", []sigilPoint{{31, 38}, {61, 45}, {39, 55}, {66, 66}}}},
	{"field", "护田", "居所灵田每受一次有效偷采消耗一重护持，最多存三重", 1, 2, sigilStroke{"围田结界", []sigilPoint{{28, 36}, {72, 36}, {72, 63}, {28, 63}, {28, 36}}}},
	{"haste", "轻身", "预备下次历练的整场移速增益，同类取高", 12, 2, sigilStroke{"折笔乘风", []sigilPoint{{29, 61}, {49, 48}, {38, 46}, {67, 33}}}},
}

var sigilSeals = []Record{
	{"id": "simple", "name": "平收", "description": "三笔落成，消耗一份墨"},
	{"id": "strong", "name": "重封", "description": "叠加一道封笔，多耗一份墨，护身、净秽与轻身效力提高五成，护田多封一重"},
}
var sigilRanks = [...]int{100, 100, 300, 800, 1600, 3000, 6250, 10000, 20000, 40000}
var sigilRealms = [...]string{"练气", "筑基", "结丹", "元婴", "分神", "炼虚", "合体", "渡劫", "大乘"}

func sigilTraining(r Record) (rank, experience int, slot string) {
	return artisanTraining(r, 3, "TalismanRank", "TalismanExperience")
}

func sigilTrain(r Record) {
	artisanAddLesson(r, 3, "TalismanRank", "TalismanExperience", 100)
}

type sigilChoice struct {
	effect sigilEffect
	seal   string
}

func sigilSelect(effectID, seal string) (sigilChoice, error) {
	if effectID == "" {
		effectID = "ward"
	}
	if seal == "" {
		seal = "simple"
	}
	if seal == "simple" || seal == "strong" {
		for _, e := range sigilEffects {
			if e.id == effectID {
				return sigilChoice{e, seal}, nil
			}
		}
	}
	return sigilChoice{}, forgeFailure("未收录的符意或收笔法")
}

func (c sigilChoice) strokes() []sigilStroke {
	head := sigilStroke{"点锋引气", []sigilPoint{{32, 22}, {48, 12}, {65, 23}}}
	tail := sigilStroke{"横画平收", []sigilPoint{{29, 78}, {73, 78}}}
	path := []sigilStroke{head, c.effect.stroke, tail}
	if c.seal == "strong" {
		path = append(path, sigilStroke{"回钩重封", []sigilPoint{{53, 71}, {53, 90}, {65, 84}}})
	}
	return path
}
func (c sigilChoice) inputs() []sigilUnit {
	ink := 1
	if c.seal == "strong" {
		ink = 2
	}
	return []sigilUnit{{sigilPaper, 1}, {sigilInk, ink}}
}
func (c sigilChoice) key() string { return "sigil." + c.effect.id + "." + c.seal }
func (c sigilChoice) name() string {
	name := c.effect.name + "符"
	if c.seal == "strong" {
		name += "·重封"
	}
	return name
}
func (c sigilChoice) power(r Record, brush bool) int {
	rank, _, _ := sigilTraining(r)
	tier := sigilRealm(r)
	if c.effect.id == "field" {
		n := 1
		if c.seal == "strong" {
			n++
		}
		if brush && rank >= 3 || sigilMastery(r, c.key()) >= 100 {
			n++
		}
		return n
	}
	pct := 100 + tier*5 + rank*3 + sigilMastery(r, c.key())/10
	if brush {
		pct += 15
	}
	if c.seal == "strong" {
		pct = pct * 3 / 2
	}
	return (c.effect.base*pct + 50) / 100
}
func sigilRealm(r Record) int { return int((min(90, max(1, roleInt(r, "Level"))) - 1) / 10) }
func (c sigilChoice) detail(power int) string {
	switch c.effect.id {
	case "ward":
		return fmt.Sprintf("入战护盾 %d 点 · 下次进入历练生效，同类取高", power)
	case "cleanse":
		return fmt.Sprintf("立即消去 %d 点毒性 · 无毒时保留", power)
	case "haste":
		return fmt.Sprintf("历练移动速度 +%d%% · 下次入场至本场结束，同类取高", power)
	default:
		return fmt.Sprintf("灵田护持 +%d 重 · 每重挡一次偷采，最多三重", power)
	}
}

// Both guided drags and accessible taps submit one point per visited waypoint.
// No timer or calligraphy recognition is involved; retries never consume paper.
func (c sigilChoice) validate(trace []sigilStroke, tolerance int) error {
	pattern := c.strokes()
	if len(trace) != len(pattern) {
		return forgeFailure("笔画尚未写全，请照淡纹补写")
	}
	for i, line := range trace {
		expected := pattern[i].Points
		if len(line.Points) != len(expected) {
			return forgeFailure(fmt.Sprintf("请重描第 %d 笔「%s」", i+1, pattern[i].Name))
		}
		for j, p := range line.Points {
			q := expected[j]
			if p.X < 0 || p.X > 100 || p.Y < 0 || p.Y > 100 || math.Hypot(float64(p.X-q.X), float64(p.Y-q.Y)) > float64(tolerance) {
				return forgeFailure(fmt.Sprintf("第 %d 笔「%s」偏离淡纹，可再描或辅笔", i+1, pattern[i].Name))
			}
		}
	}
	return nil
}
