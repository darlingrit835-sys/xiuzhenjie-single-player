from copy import deepcopy
from decimal import Decimal
from fractions import Fraction
import unittest

from bestiary_tables import compile_bestiary, map_ecology_summary, NATIVE_MAPS, SHEETS
from export_affinities import DATA, read_sheets
from preview_alchemy import CONFIG, read_json
from preview_map_yield import map_yield
from preview_region import preview_region


class BestiaryTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.rules = read_json(CONFIG)
        cls.text = read_json(CONFIG.with_name(CONFIG.stem + ".zh-CN.json"))
        all_sheets = read_sheets(DATA / "config_excel" / "修仙属性基础表.xlsx")
        cls.sheets = {key: all_sheets[key] for key in SHEETS}

    def test_existing_map_coverage_and_variety(self):
        rules = self.rules
        habitats = set()
        self.assertTrue(NATIVE_MAPS.keys() <= rules["encounterMapIds"].keys())
        for key in rules["encounterMapIds"]:
            for tier, minimum in [(1, 4), (3, 3), (4, 2)]:
                summary = map_ecology_summary(rules, self.text, key, tier)
                rows = summary["monsters"]
                if key in NATIVE_MAPS and tier == 4 and key.endswith("1"):
                    self.assertEqual(summary["slots"], 0)
                if summary["slots"] == 0:
                    self.assertEqual((summary["slots"], summary["monsterTickets"], rows), (0, 0, []))
                    continue
                self.assertGreaterEqual(len(rows), minimum if key in NATIVE_MAPS else 1, (key, tier))
                for monster in rows:
                    self.assertTrue(monster["drops"])
                    habitats.add((str(summary["regionId"]), str(monster["monsterId"])))
                self.assertEqual(summary["monsterTickets"], sum(row["tickets"] for row in rows))
        self.assertEqual(habitats, {(c, m) for c, ms in rules["regionMonsters"].items() for m in ms})

    def test_missing_map_wrong_county_and_unreachable_loot(self):
        sheets = deepcopy(self.sheets)
        sheets["林地战区"].pop()
        with self.assertRaisesRegex(ValueError, "缺少战区"):
            compile_bestiary(sheets, self.rules, self.rules)
        sheets = deepcopy(self.sheets)
        sheets["群落怪物"][0][1]["monsterId"] = "324"
        with self.assertRaisesRegex(ValueError, "本郡"):
            compile_bestiary(sheets, self.rules, self.rules)
        rules = deepcopy(self.rules)
        # 301 从未进入兽王波位。即使材料外键存在，错误的反查来源也应被拦下。
        drop = next(iter(rules["monsterDrops"]["301"]["0"].values()))
        drop["chanceBps"] = {"1": 0, "3": 0, "4": 800}
        with self.assertRaisesRegex(ValueError, "有效波位"):
            compile_bestiary(self.sheets, rules, rules)

    def test_expected_yield_keeps_integer_tickets_and_wave_order(self):
        report = map_yield(self.rules, self.text, "1111W11")
        # 青木猿与松针猬都出云纹木。猿的区间 1~2 份，猬为固定一份。
        wood = (Fraction(1288980 * 530, 3258780 * 10000) * Fraction(3, 2)
                + Fraction(527800 * 1200, 3258780 * 10000))
        self.assertEqual(Fraction(**report["expectedQuantities"]["mat.cloudwood"]), 5 * wood)
        first_two = map_yield(self.rules, self.text, "1111W11", 2)
        self.assertEqual(first_two["slotsKilled"], {"1": 2, "3": 0, "4": 0})
        self.assertNotIn("mat.thunder_whisker", first_two["expectedQuantities"])
        self.assertEqual(Fraction(**first_two["expectedQuantities"]["mat.cloudwood"]), 2 * wood)
        # 猿的两种特色掉落独立：都不中为 (1-.053)(1-.17)。
        none = (1288980 * Fraction(947, 1000) * Fraction(83, 100)
                + 123600 * Fraction(86, 100) + 1318400 * Fraction(865, 1000)
                + 527800 * Fraction(88, 100)) / 3258780
        actual = Decimal(first_two["noSpecialDropProbability"])
        self.assertAlmostEqual(actual, Decimal(none.numerator ** 2) / Decimal(none.denominator ** 2), places=25)
        zero = map_yield(self.rules, self.text, "1111W11", 0)
        self.assertEqual((zero["expectedQuantities"], Decimal(zero["noSpecialDropProbability"])), ({}, 1))
        with self.assertRaisesRegex(ValueError, "0～6"):
            map_yield(self.rules, self.text, "1111W11", 7)

    def test_player_map_can_reuse_existing_pool_with_new_key(self):
        sheets = deepcopy(self.sheets)
        original = sheets["林地战区"][0][1]
        sheets["林地战区"].append(("新地图", {**original, "id": "9020101", "mapKey": "reserve.moss_garden"}))
        rules, _ = compile_bestiary(sheets, self.rules, self.rules)
        self.assertEqual(rules["encounterMapIds"]["reserve.moss_garden"], 9020101)
        self.assertEqual(rules["encounterMaps"]["9020101"]["poolId"], int(original["poolId"]))
        report = preview_region({**self.rules, **rules}, self.text, "reserve.moss_garden",
                                {"a": {"MapId": "reserve.moss_garden", "Wood": 10}})
        self.assertEqual((report["regionId"], report["residentCount"]), (int(original["regionId"]), 1))
