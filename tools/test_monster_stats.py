import copy
import json
import unittest

from export_affinities import DATA, build_config, read_sheets
from monster_stats import compile_stats, incoming_hit, monster_panel


class MonsterStatsTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.sheets = read_sheets(DATA / "config_excel/修仙属性基础表.xlsx")
        cls.base = build_config(cls.sheets)
        cls.catalog = json.loads((DATA / "config/cultivation_affinities.json").read_text())

    def test_all_monsters_have_panels_in_their_actual_maps_and_tiers(self):
        cfg = self.catalog
        reachable = set()
        for map_key, zone_id in cfg["encounterMapIds"].items():
            zone = cfg["encounterMaps"][str(zone_id)]
            wave = cfg["encounterWaves"][str(zone["waveId"])]
            for tier, monsters in cfg["encounterPoolMonsters"][str(zone["poolId"])].items():
                if wave[tier]:
                    for species in monsters:
                        row = monster_panel(cfg, species, map_key, tier, 1)
                        reachable.add(species)
                        self.assertGreaterEqual(row["panel"]["hp"], 30)
                        self.assertLessEqual(row["panel"]["mana"], 2147483647)
                        self.assertTrue(row["skills"])
        self.assertEqual(reachable, set(cfg["monsterSpecies"]))

    def test_neutral_panel_matches_go_calibration_and_variance_channels(self):
        cfg = self.catalog
        args = (cfg, 201, "1111W11", 1, 11)
        balanced = monster_panel(*args, perception=1000)
        self.assertEqual(balanced["panel"], {"hp": 900, "armor": 150, "attack": 22, "mana": 850,
                                             "perception": 600, "speedMilli": 4400, "rangeMilli": 5000, "gold": 48})
        weak = monster_panel(*args, perception=1000, super_air=0, variance_bps=-600)["panel"]
        self.assertEqual((weak["hp"], weak["armor"], weak["attack"]), (719, 120, 18))
        for stat in ("mana", "perception", "speedMilli", "gold"):
            self.assertEqual(weak[stat], balanced["panel"][stat])
        self.assertEqual(balanced["panel"], monster_panel(*args, perception=1000, super_air=10**12)["panel"])
        self.assertEqual(monster_panel(*args)["rolePerception"], 1379)
        self.assertEqual(monster_panel(*args, perception=0, server_difficulty_bps=0)["panel"]["hp"], 30)
        self.assertEqual(monster_panel(cfg, 316, "1211W13", 4, 90, perception=2147483647)["panel"]["hp"], 2147483647)

    def test_depth_and_tiers_give_expected_panels_and_boss_skill_override(self):
        cfg = self.catalog
        elite = monster_panel(cfg, 316, "1211W12", 3, 11, perception=1000)
        boss = monster_panel(cfg, 316, "1211W13", 4, 11, perception=1000)
        self.assertEqual((elite["panel"]["hp"], elite["panel"]["attack"], elite["panel"]["armor"]), (2730, 54, 666))
        self.assertEqual((boss["panel"]["hp"], boss["panel"]["attack"], boss["panel"]["armor"]), (4576, 90, 1122))
        self.assertEqual(next(s for s in boss["skills"] if s["id"] == 1)["cooldownSec"], 2)
        self.assertEqual(next(s for s in boss["skills"] if s["id"] == 2)["baseDamage"], 50)
        with self.assertRaisesRegex(ValueError, "不能遭遇"):
            monster_panel(cfg, 301, "1111W11", 4, 11)
        with self.assertRaisesRegex(ValueError, "面板浮动"):
            monster_panel(cfg, 316, "1211W13", 4, 11, variance_bps=601)

    def test_resistance_weakness_and_signed_cap(self):
        cfg = self.catalog
        self.assertEqual(incoming_hit(cfg, 316, 1000, 4, 11), {"resistBps": 840, "damage": 916})
        self.assertEqual(incoming_hit(cfg, 316, 1000, 3, 8), {"resistBps": -761, "damage": 1076})
        self.assertEqual(incoming_hit(cfg, 316, 1000, 2, 1), {"resistBps": 0, "damage": 1000})
        adjusted = copy.deepcopy(cfg)
        adjusted["monsterStats"]["settings"]["resistLimitBps"] = 500
        self.assertEqual(incoming_hit(adjusted, 316, 1000, 4, 11)["resistBps"], 500)
        self.assertEqual(incoming_hit(adjusted, 316, 1000, 3, 8)["resistBps"], -500)

    def test_new_species_and_validation(self):
        config = copy.deepcopy(self.base)
        sheets = copy.deepcopy(self.sheets)
        config["worldRules"]["monsterSpecies"]["50001"] = dict(config["worldRules"]["monsterSpecies"]["301"])
        config["worldText"]["monsterSpecies"]["50001"] = {"name": "试定砂螳"}
        with self.assertRaisesRegex(ValueError, "怪物战斗面板"):
            compile_stats(config, sheets)
        _, sample = next(pair for pair in sheets["怪物战斗面板"] if pair[1]["id"] == "301")
        sheets["怪物战斗面板"].append(("玩家样例", dict(sample, id="50001", name="试定砂螳")))
        for where, row in self.sheets["怪物抗性"]:
            if row["id"] == "301":
                sheets["怪物抗性"].append((where, dict(row, id="50001")))
        rules, _ = compile_stats(config, sheets)
        self.assertEqual(rules["species"][50001]["hp"], 570)
        sheets["怪物抗性"].append(sheets["怪物抗性"][-1])
        with self.assertRaisesRegex(ValueError, "重复"):
            compile_stats(config, sheets)
