package main

import (
	"math/rand"
	"testing"
	"time"
)

func huntTestWoodApe(t *testing.T, county *countyEcology) fieldSpecies {
	t.Helper()
	if county != nil {
		for _, species := range county.fauna {
			if species.name == "青木猿" {
				return species
			}
		}
	}
	t.Fatal("county ecology is missing 青木猿")
	return fieldSpecies{}
}

func TestAlchemyHuntRegionalWoodApeDrops(t *testing.T) {
	counties, err := loadBattleEcology("../server/data")
	if err != nil {
		t.Fatal(err)
	}
	const sinew = "mat.wind_sinew"
	const herb = "mat.ninglu_herb"
	for _, county := range []string{"1111", "1112"} {
		ape := huntTestWoodApe(t, counties[county])
		rules := make(map[string]herbDropRule)
		for _, drop := range ape.herbs {
			rules[drop.material.id] = drop.rule
		}
		if got := rules[sinew]; got.MinQty != 1 || got.MaxQty != 1 {
			t.Fatalf("county %s common sinew quantity: %+v", county, got)
		}
		regional, found := rules[herb]
		if found != (county == "1112") {
			t.Fatalf("county %s unexpected 朝露郡-only 凝露草: %+v", county, rules)
		}
		if found && (regional.MinQty != 1 || regional.MaxQty != 2 ||
			regional.ChanceBps[1] != 620 || regional.ChanceBps[3] != 1050 || regional.ChanceBps[4] != 1800) {
			t.Fatalf("county %s regional rates or quantities: %+v", county, regional)
		}
		for _, tc := range []struct{ tier, limit int }{{1, 1700}, {3, 3500}, {4, 6600}} {
			monster := battleMonster{unusual: tc.tier, herbDrops: ape.herbs}
			for _, draw := range []int{tc.limit - 1, tc.limit} {
				drops := rollHerbDrops(monster, 0, 0, func(bound int) int {
					if bound == 10000 {
						return draw
					}
					return bound - 1
				}, 0)
				got := 0
				for _, d := range drops {
					if d.template == sinew {
						got += d.count
					}
				}
				want := 0
				if draw < tc.limit {
					want = 1
				}
				if got != want {
					t.Fatalf("county %s tier %d roll %d: got %d sinew, want %d", county, tc.tier, draw, got, want)
				}
			}
		}
	}
}

func TestAlchemyHuntPickupKeepsTemplate(t *testing.T) {
	s, peer := alchemyTestServer(t, t.TempDir())
	counties, err := loadBattleEcology(s.dataDir)
	if err != nil {
		t.Fatal(err)
	}
	ape := huntTestWoodApe(t, counties["1111"])
	eco := *counties["1111"]
	// Force only the draw in this fixture. The template, quantity, five-element
	// profile and goods type still come from the actual county's candidate.
	for i, herb := range ape.herbs {
		if herb.material.id == "mat.wind_sinew" {
			ape.herbs[i].rule.ChanceBps = map[int]int{1: 10000}
		}
	}
	eco.fauna = []fieldSpecies{ape}
	rng := rand.New(rand.NewSource(7))
	d := &battleDungeon{
		id: "hunt-wood-ape", mapID: "1111W1", county: "1111", roleID: "artist", playerLevel: 1,
		monsters: []battleMonster{{index: 1001, objectType: 2002, unusual: 1, level: 1, gold: 1}},
		expires:  time.Now().Add(time.Minute), dead: make(map[int]struct{}),
	}
	d.world = generateBattleField(d.county, len(d.monsters), rng)
	d.world.populate(d.id, d.monsters, &eco, rng)
	s.battleState.dungeons = map[string]*battleDungeon{d.id: d}
	monster := d.monsters[0]
	d.world.playerX, d.world.playerY = monster.x, monster.y

	drops, _, awarded := s.settleMonster(d, monster, monster.x, monster.y)
	if !awarded {
		t.Fatal("wood ape kill failed to settle")
	}
	var id string
	for _, d := range drops {
		if d.template == "mat.wind_sinew" {
			id = d.id
			break
		}
	}
	if id == "" {
		t.Fatalf("wood ape spawned without its configured herb: %+v", drops)
	}
	goodsID, code, _, _ := s.pickGroundDrop(d.roleID, d.id, id)
	if code != 4526 {
		t.Fatalf("first pickup returned %d", code)
	}
	if _, code, _, _ := s.pickGroundDrop(d.roleID, d.id, id); code != 4536 {
		t.Fatalf("repeat pickup returned %d", code)
	}
	role := s.roles.Get(d.roleID)
	goods := locateGoods(role, goodsID)
	if goods == nil || roleString(goods, "TemplateId") != "mat.wind_sinew" || transferStackCount(goods) != 1 {
		t.Fatalf("lost or duplicated the ecological template in inventory: %+v", goods)
	}
	catalog := brewCall(t, s, peer, brewRequest{Action: "catalog"}, true)
	for _, material := range battleRecords(catalog["materials"]) {
		if roleString(material, "id") == "mat.wind_sinew" && roleInt(material, "count") == 1 {
			return
		}
	}
	t.Fatal("alchemy catalog did not expose the picked sinew as one usable ingredient")
}
