package main

import (
	"encoding/json"
	"fmt"
	"maps"
	"math/rand"
	"os"
	"path/filepath"
	"slices"
	"strings"

	"xiuzhenjie/server/internal/affinity"
)

type herbDropRule struct {
	ChanceBps      map[int]int
	MinQty, MaxQty int
}

type fieldHerbDrop struct {
	material fieldMaterial
	rule     herbDropRule
}

type fieldSpecies struct {
	herbs                        []fieldHerbDrop
	name                         string
	kind, icon, element, tickets int
	boss                         bool
}

type fieldMaterial struct {
	id, name                                     string
	kind, rank, element, tickets, minQty, maxQty int
}

type fieldNode struct {
	item         fieldMaterial
	index, count int
	x, y         float64
	collectedID  string
}

type countyEcology struct {
	people, fauna, bosses []fieldSpecies
	plants, ores          []fieldMaterial
}

type habitat struct{ ElementID, QiID, PolarityID int }
type elementCycle struct{ Generates, Overcomes int }
type habitatRules struct{ PolarityBps, QiBps, ElementBps, GeneratesBps, OvercomesBps, CapBps int }

func (rules habitatRules) tickets(base int, site habitat, cycle elementCycle, profile affinity.Profile) int {
	delta := 0
	if profile.PolarityID != 0 && site.PolarityID == profile.PolarityID {
		delta += rules.PolarityBps
	}
	if profile.PrimaryQiID != 0 && site.QiID == profile.PrimaryQiID {
		delta += rules.QiBps
	}
	switch profile.PrimaryElementID {
	case site.ElementID:
		delta += rules.ElementBps
	case cycle.Generates:
		delta += rules.GeneratesBps
	case cycle.Overcomes:
		delta += rules.OvercomesBps
	}
	return base * (10000 + max(-rules.CapBps, min(delta, rules.CapBps)))
}

func loadBattleEcology(dir string) (map[string]*countyEcology, error) {
	var data struct {
		CatalogVersion  string
		Profiles        map[string]affinity.Profile
		ItemTemplates   map[string]affinity.Item
		Regions         map[string]habitat
		Elements        map[int]elementCycle
		EcologyAffinity map[string]habitatRules
		RegionMonsters  map[string]map[string]int
		MonsterSpecies  map[string]struct {
			ProfileID            string
			ObjectTypeID, IconID int
			AllowBoss            bool
		}
		RegionResources map[string]map[int]map[string]struct{ Weight, MinQty, MaxQty int }
		MonsterDrops    map[string]map[string]map[string]herbDropRule
	}
	var labels struct {
		CatalogVersion                string
		ItemTemplates, MonsterSpecies map[string]struct{ Name string }
	}
	read := func(name string, target any) error {
		f, err := os.Open(filepath.Join(dir, "config", name))
		if err != nil {
			return err
		}
		defer f.Close()
		return json.NewDecoder(f).Decode(target)
	}
	if err := read("cultivation_affinities.json", &data); err != nil {
		return nil, err
	}
	if err := read("cultivation_affinities.zh-CN.json", &labels); err != nil {
		return nil, err
	}
	if data.CatalogVersion == "" || data.CatalogVersion != labels.CatalogVersion {
		return nil, fmt.Errorf("地域生态表与中文名称版本不一致")
	}
	byCounty := make(map[string]*countyEcology)
	for region, habitats := range data.RegionMonsters {
		eco := new(countyEcology)
		env := data.Regions[region]
		cycle := data.Elements[env.ElementID]
		for _, id := range slices.Sorted(maps.Keys(habitats)) {
			s := data.MonsterSpecies[id]
			p := data.Profiles[s.ProfileID]
			m := fieldSpecies{name: labels.MonsterSpecies[id].Name, kind: s.ObjectTypeID, icon: s.IconID, element: p.PrimaryElementID, boss: s.AllowBoss,
				tickets: data.EcologyAffinity["monster"].tickets(habitats[id], env, cycle, p)}
			if m.name == "" || m.tickets <= 0 {
				continue
			}
			// The original catalog only assigned portraits 1–8 to animals.
			// The field atlas also depicts 浊沼鳄. Unknown animals use its generic silhouette.
			if m.kind == 0 {
				m.kind = 2002
				if id == "109" && m.icon == 0 {
					m.icon = 9
				}
			}
			// A county-specific record replaces the general record for the same herb.
			scopes := data.MonsterDrops[id]
			rules := make(map[string]herbDropRule)
			maps.Copy(rules, scopes["0"])
			maps.Copy(rules, scopes[region])
			for _, herb := range slices.Sorted(maps.Keys(rules)) {
				rule := rules[herb]
				item := data.ItemTemplates[herb]
				mat := fieldMaterial{id: herb, name: labels.ItemTemplates[herb].Name, kind: ecologyHerbKind(item.CategoryID),
					rank: item.GradeID, element: data.Profiles[item.ProfileID].PrimaryElementID}
				if rule.MinQty < 1 || rule.MaxQty < rule.MinQty || mat.name == "" {
					return nil, fmt.Errorf("药材掉落无效 %s/%s/%s", region, id, herb)
				}
				m.herbs = append(m.herbs, fieldHerbDrop{mat, rule})
			}
			switch m.kind {
			case 2001:
				eco.people = append(eco.people, m)
			case 2002:
				eco.fauna = append(eco.fauna, m)
			default:
				continue
			}
			if m.boss {
				eco.bosses = append(eco.bosses, m)
			}
		}
		for _, method := range []int{1, 2} {
			items := data.RegionResources[region][method]
			for _, id := range slices.Sorted(maps.Keys(items)) {
				resource, item := items[id], data.ItemTemplates[id]
				p := data.Profiles[item.ProfileID]
				mat := fieldMaterial{id: id, name: labels.ItemTemplates[id].Name, element: p.PrimaryElementID, kind: 11 + method, rank: max(1, item.GradeID),
					tickets: data.EcologyAffinity["resource"].tickets(resource.Weight, env, cycle, p), minQty: resource.MinQty, maxQty: resource.MaxQty}
				if mat.tickets <= 0 || mat.name == "" || mat.minQty < 1 || mat.maxQty < mat.minQty {
					continue
				}
				if method == 1 {
					eco.plants = append(eco.plants, mat)
				} else {
					eco.ores = append(eco.ores, mat)
				}
			}
		}
		if len(eco.fauna) == 0 || len(eco.people) == 0 || len(eco.bosses) == 0 || len(eco.plants) == 0 || len(eco.ores) == 0 {
			return nil, fmt.Errorf("地区 %s 生态候选不完整", region)
		}
		byCounty[region] = eco
	}
	if len(byCounty) == 0 {
		return nil, fmt.Errorf("地域生态表缺少郡")
	}
	return byCounty, nil
}

func ecologyHerbKind(category string) int {
	switch category {
	case "mat.ore":
		return 13
	case "mat.blood":
		return 9
	case "mat.bone":
		return 10
	case "mat.core":
		return 11
	case "mat.beast":
		return 8
	default:
		return 12
	}
}

func weightedFieldPick[T any](rng *rand.Rand, pool []T, weight func(T) int) T {
	total := 0
	for _, v := range pool {
		total += weight(v)
	}
	ticket := rng.Intn(total)
	for _, v := range pool {
		ticket -= weight(v)
		if ticket < 0 {
			return v
		}
	}
	return pool[len(pool)-1]
}

func (w *battleField) populate(id string, monsters []battleMonster, eco *countyEcology, rng *rand.Rand) {
	// Draw cells without replacement from terrain-specific buckets. Even large waves
	// avoid scanning the map per actor or spawning a resource beneath an enemy.
	spawn := w.cell(w.playerX, w.playerY)
	var habitatCells, entranceCells [7][]int
	// Eight 2-unit cells also clear the legacy 15-unit discovery trigger.
	const safeRadius = 8
	firstHerb := -1
	reserved := make(map[int]bool, len(w.landmarks)/3)
	for off := 0; off < len(w.landmarks); off += 3 {
		reserved[int(w.landmarks[off])|int(w.landmarks[off+1])<<8] = true
	}
	for _, i := range rng.Perm(len(w.tiles)) {
		if !w.passable(i) || i == spawn || reserved[i] {
			continue
		}
		h := w.tiles[i]
		// Give the entry clearing a visible, reachable forage point.
		if h == fieldGrove && firstHerb < 0 && w.explored[i>>3]&(1<<uint(i&7)) != 0 {
			firstHerb = i
			continue
		}
		dx, dy := i%w.width-spawn%w.width, i/w.width-spawn/w.width
		if dx*dx+dy*dy <= safeRadius*safeRadius {
			entranceCells[h] = append(entranceCells[h], i)
		} else {
			habitatCells[h] = append(habitatCells[h], i)
		}
	}
	take := func(kinds ...byte) int {
		for _, kind := range kinds {
			list := habitatCells[kind]
			if len(list) > 0 {
				cell := list[len(list)-1]
				habitatCells[kind] = list[:len(list)-1]
				return cell
			}
		}
		return -1
	}
	for index, m := range monsters {
		species := eco.fauna
		if m.unusual == 4 {
			species = eco.bosses
		} else if m.objectType == 2001 {
			species = eco.people
		}
		chosen := weightedFieldPick(rng, species, func(m fieldSpecies) int { return m.tickets })
		m.objectType, m.element, m.icon = chosen.kind, chosen.element, chosen.icon
		m.herbDrops = chosen.herbs
		prefix, _, _ := strings.Cut(m.name, "·")
		m.name = prefix + "·" + chosen.name
		var cell int
		switch {
		case chosen.kind == 2001:
			cell = take(fieldCamp, fieldRuin, fieldPath)
		case chosen.element == 1 || chosen.element == 4 || chosen.element == 5:
			cell = take(fieldRidge, fieldGrove, fieldRuin, fieldPath)
		default:
			cell = take(fieldGrove, fieldRidge, fieldRuin, fieldPath)
		}
		if cell < 0 {
			cell = take(fieldRuin, fieldCamp, fieldRidge, fieldGrove, fieldPath)
		}
		m.x, m.y = w.center(cell)
		monsters[index] = m
	}
	// The safe approach may still contain resources in their natural terrain.
	for _, ground := range []byte{fieldGrove, fieldRidge} {
		habitatCells[ground] = append(entranceCells[ground], habitatCells[ground]...)
	}
	nodeIndex := 1001 + len(monsters)
	for _, placement := range []struct {
		tile  byte
		table []fieldMaterial
	}{{fieldGrove, eco.plants}, {fieldRidge, eco.ores}} {
		for i := range max(2, len(monsters)/3) {
			var cell int
			if i == 0 && placement.tile == fieldGrove && firstHerb >= 0 {
				cell = firstHerb
			} else {
				cell = take(placement.tile)
			}
			if cell < 0 {
				break
			}
			item := weightedFieldPick(rng, placement.table, func(m fieldMaterial) int { return m.tickets })
			qty := item.minQty + rng.Intn(item.maxQty-item.minQty+1)
			x, y := w.center(cell)
			w.nodes[fmt.Sprintf("gather_%s_%d", id, nodeIndex)] = &fieldNode{item: item, index: nodeIndex, count: qty, x: x, y: y}
			nodeIndex++
		}
	}
}
