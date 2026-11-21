package main

import (
	"crypto/sha256"
	"encoding/binary"
	"fmt"
	"maps"
	"slices"
	"strings"
)

const smithingSampleCmd = 4901027

// Each published pattern has a stable edition, a physical blank and a two-material recipe.
type workshopPattern struct {
	key, name, form, primary, secondary string
	primaryCount, grade, balance        int
	engraving                           patternWork
}

var workshopPatterns = []workshopPattern{
	{"tingxue", "听雪剑", "sword", "hanwen_iron", "cold_dew", 2, 2, 2, patternWork{0, "平槽引霜"}},
	{"songying", "松影杖", "staff", "cloudwood", "cold_dew", 2, 2, -2, patternWork{0, "顺纹引灵"}},
	{"guichao", "归潮刀", "saber", "hanwen_iron", "cloudwood", 2, 2, 1, patternWork{1, "回锋起浪"}},
	{"qingshuang", "青霜枪", "spear", "cloudwood", "hanwen_iron", 2, 2, 0, patternWork{0, "单缨通脊"}},
	{"xianyue", "弦月弓", "bow", "cloudwood", "cold_dew", 2, 2, 2, patternWork{1, "弓弦成月"}},
	{"lieque", "裂缺剑", "sword", "thunder_whisker", "black_iron_ore", 2, 5, 3, patternWork{2, "双沟导雷"}},
	{"baizhi", "白芷杖", "staff", "ninglu_herb", "river_scale", 2, 3, -3, patternWork{1, "药脉分枝"}},
	{"poyan", "破岩刀", "saber", "earth_claw", "common_bone", 2, 3, -2, patternWork{1, "厚背分刃"}},
	{"jinling", "金翎枪", "spear", "gold_plume", "common_blood", 2, 5, 3, patternWork{2, "叠翎护锋"}},
	{"zhuying", "逐影弓", "bow", "wind_sinew", "moon_fur", 2, 5, 2, patternWork{2, "双弦追影"}},
	{"feishuang", "飞霜剑", "sword", "frost_silver", "river_scale", 3, 6, 3, patternWork{2, "叠锋锁霜"}},
	{"chunshan", "春山杖", "staff", "wild_herb", "dream_lotus", 2, 5, -2, patternWork{1, "层枝贯芯"}},
	{"chixiao", "赤霄刀", "saber", "ember_moss", "sunstone", 2, 5, 1, patternWork{2, "双刃藏焰"}},
	{"zhenze", "镇泽枪", "spear", "marsh_plate", "earth_claw", 2, 6, -3, patternWork{2, "鳞甲合脊"}},
	{"hanlu", "寒芦弓", "bow", "frost_reed", "common_bone", 3, 3, 1, patternWork{1, "芦节并弦"}},
	{"liuhui", "流辉剑", "sword", "dream_mica", "frost_silver", 2, 6, 2, patternWork{3, "三道回光纹"}},
	{"qixia", "栖霞杖", "staff", "cold_dew", "wind_slate", 3, 3, -1, patternWork{2, "霞纹交织"}},
	{"jinglei", "惊雷枪", "spear", "thunder_horn", "thunder_whisker", 2, 6, 3, patternWork{4, "四重雷纹套合"}},
	{"yunwen", "云纹法衣", "robe", "cloudwood", "cold_dew", 2, 2, -2, patternWork{1, "衣襟织护纹"}},
	{"tafeng", "踏风灵靴", "boots", "common_hide", "cloudwood", 2, 2, 0, patternWork{1, "左右靴纹相应"}},
	{"hanguang", "寒光护心镜", "ward", "hanwen_iron", "cold_dew", 2, 2, -3, patternWork{1, "镜心淬寒"}},
}

var patternBlanks = map[string]string{
	"sword": "made.frost_sword", "staff": "wp.tingquan_staff", "saber": "wp.duanxia_saber",
	"spear": "wp.minglei_spear", "bow": "wp.zhufeng_bow",
	"robe": "obj.yunwen_robe", "boots": "obj.tafeng_boots", "ward": "obj.hanguang_mirror",
}

func (w *smithingWorkshop) preparePatterns(store *Store) error {
	w.patterns = make(map[string]Record)
	w.engravings = make(map[string]patternWork)
	for _, p := range workshopPatterns {
		blank := patternBlanks[p.form]
		inputs := map[string]int{"mat." + p.primary: p.primaryCount, "mat." + p.secondary: 1}
		// Earlier data packs can still use the patterns whose underlying items they contain.
		if _, ok := w.rules.Item(blank); !ok {
			continue
		}
		if _, ok := w.rules.Item("mat." + p.primary); !ok {
			continue
		}
		if _, ok := w.rules.Item("mat." + p.secondary); !ok {
			continue
		}
		if !slices.Contains(w.rules.ForgeBlanks(), blank) {
			continue
		}
		id := smithingTemplateID("association.pattern.v1", p.key)
		w.engravings[id] = p.engraving
		// The first issued copy fixes the edition even after a later balance update.
		if issued := store.Get(smithingStorePrefix + id); issued != nil {
			if _, frozen := savedChallenge(issued); !frozen {
				recipe, _ := issued["Inputs"].(Record)
				w.ratePattern(roleString(issued, "BlankId"), int(roleInt(issued, "Rank")), recipeQuantities(recipe), p.engraving).DescribeInto(issued)
			}
			w.patterns[id] = issued
			w.patternOrder = append(w.patternOrder, id)
			continue
		}
		seed := sha256.Sum256([]byte(id))
		weapon, err := w.rules.CastForge(blank, inputs, inputs, p.grade, int64(binary.LittleEndian.Uint64(seed[:])))
		if err != nil {
			return fmt.Errorf("器图 %s: %w", p.key, err)
		}
		shape := forgeShape{balance: p.balance, quality: "良模"}
		shape.apply(weapon)
		t := Record{"TemplateId": id, "Name": p.name, "CreatedBy": "炼器协会", "MoldId": id, "MoldQuality": shape.quality, "ShapeBalance": p.balance}
		w.setForgedWeapon(t, weapon)
		w.ratePattern(blank, p.grade, inputs, p.engraving).DescribeInto(t)
		w.patterns[id] = t
		w.patternOrder = append(w.patternOrder, id)
	}
	return nil
}

func (w *smithingWorkshop) sampleLevel(pattern Record) int {
	grade, _ := w.rules.Grade(int(roleInt(pattern, "Rank")))
	return 1 + 10*max(0, 3*(grade.TierID-1)+grade.QualityID-2)
}

func (w *smithingWorkshop) patternDirectory(role Record) []Record {
	claimed, _ := role["WorkshopSamples"].(Record)
	rows := make([]Record, 0, len(w.patternOrder))
	for _, id := range w.patternOrder {
		t := w.patterns[id]
		recipe := t["Inputs"].(Record)
		inputs := make([]forgeInput, 0, len(recipe))
		costs := make([]string, 0, len(recipe))
		for _, mat := range slices.Sorted(maps.Keys(recipe)) {
			count := int(roleInt(recipe, mat))
			inputs = append(inputs, forgeInput{TemplateID: mat, Quantity: count})
			costs = append(costs, fmt.Sprintf("%s×%d", w.text.ItemTemplates[mat].Name, count))
		}
		view := w.weaponView(t)
		row := Record{"id": id, "name": t["Name"], "blankId": t["BlankId"], "gradeId": t["Rank"], "grade": view["gradeName"], "balance": t["ShapeBalance"], "slotType": t["Type"],
			"element": view["elementName"], "quality": t["MoldQuality"], "inputs": inputs, "recipe": strings.Join(costs, "、"),
			"attack": t["BaseAttack"], "defense": t["BaseDefense"], "traits": view["bonusText"], "material": view["materialText"], "effect": view["effectText"], "claimed": claimed[id] != nil}
		w.moldDifficulty(t).DescribeInto(row)
		row["difficultyText"] = fmt.Sprintf("%s · 认主样品 %d 级可领", roleString(row, "difficultyText"), w.sampleLevel(t))
		row["heatTargets"] = w.strikeTargets(id)
		rows = append(rows, row)
	}
	return rows
}

// Collection and the entitlement share the inventory transaction, including retries after reconnect.
func (s *Server) claimWorkshopSample(owner, id string) (Record, error) {
	prototype := s.smithing.patterns[id]
	if prototype == nil {
		return nil, forgeFailure("这张器图没有陈列样品")
	}
	var result Record
	err := s.roles.Transact(func(tx *StoreTx) error {
		r := tx.Get(owner)
		if r == nil {
			return ErrRecordNotFound
		}
		samples, _ := r["WorkshopSamples"].(Record)
		if samples == nil {
			samples = Record{}
			r["WorkshopSamples"] = samples
		}
		t := tx.GetOrCreate(smithingStorePrefix + id)
		if len(t) == 0 {
			maps.Copy(t, prototype)
		}
		if samples[id] == nil {
			if need := s.smithing.sampleLevel(prototype); int(max(1, roleInt(r, "Level"))) < need {
				return forgeFailure(fmt.Sprintf("此样品 %d 级可领，备齐材料可先依图铸造", need))
			}
			g := forgedGoods(t)
			g["IsBinding"] = true
			goodsID, err := receiveInventoryGoods(r, g, 1)
			if err != nil {
				return forgeFailure("纳戒已满，腾出一格即可领取样品")
			}
			samples[id] = goodsID
		}
		result = s.smithing.weaponView(t)
		result["quantity"], result["forged"], result["goodsId"] = 1, true, samples[id]
		return nil
	})
	return result, err
}
