import unittest

from preview_alchemy import CONFIG, brew_plan, ingredient_guide, read_json, recipe_lookup, yield_distribution


class AlchemyPreviewTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.rules = read_json(CONFIG)
        cls.locale = read_json(CONFIG.with_name(CONFIG.stem + ".zh-CN.json"))

    def lookup(self, key):
        return recipe_lookup(self.rules, self.locale, key)[1]

    def test_lookup_expands_regional_and_processed_ingredients(self):
        recipe = self.lookup("赤阳振元丹")
        self.assertEqual(self.lookup("obj.chiyang_pill"), recipe)
        materials = {row["itemId"]: row for row in ingredient_guide(self.rules, self.locale, recipe)}
        self.assertEqual({key: row["qty"] for key, row in materials.items()},
                         {"mat.ember_moss": 2, "mat.sunstone": 1, "mat.flame_fang": 1})
        ore = materials["mat.sunstone"]["sources"]
        self.assertEqual({row["regionId"] for row in ore["gather"]}, {1211, 1311})
        self.assertEqual([(row["monsterId"], row["regionId"]) for row in ore["drop"]], [(101, 1211)])

        dew = next(row for row in ingredient_guide(self.rules, self.locale, self.lookup("凝露丹"))
                   if row["itemId"] == "mat.cold_dew")
        herb = dew["sources"]["components"][0]
        self.assertEqual((herb["itemId"], herb["qty"]), ("mat.ninglu_herb", 3))
        self.assertIn(1112, {row["regionId"] for row in herb["sources"]["gather"]})
        with self.assertRaisesRegex(ValueError, "未登记"):
            self.lookup("sys_zhu_ji_dan_huang")

    def test_matching_fire_and_method_change_success_and_yield_within_caps(self):
        recipe = self.lookup("dan.chiyang")
        plan = brew_plan(self.rules, self.locale, recipe, 4, "flame.solar", "gf_3_9", "1211W11", period=7)
        cold = brew_plan(self.rules, self.locale, recipe, 4, "flame.spring", "gf_3_3", "1211", period=7)
        self.assertEqual(plan["successBps"], 8021)
        self.assertGreater(plan["successBps"], cold["successBps"])
        self.assertGreater(plan["yieldMultiplierBps"], cold["yieldMultiplierBps"])
        self.assertEqual(plan["effectRange"], [69, 73])
        self.assertEqual(sum(plan["yieldTickets"].values()), 20000)
        self.assertEqual(plan["yieldTickets"][4], 9060)
        self.assertEqual(max(plan["yieldTickets"]), recipe["yieldCap"])
        with self.assertRaisesRegex(ValueError, "至少 四品"):
            brew_plan(self.rules, self.locale, recipe, 3, "flame.solar")

    def test_local_bonus_uses_main_herb_and_yield_rounding_preserves_tail(self):
        recipe = self.lookup("dan.ninglu")
        plan = brew_plan(self.rules, self.locale, recipe, 0, "flame.ember", region="1112")
        self.assertEqual(plan["primaryElementId"], 2)
        self.assertIn("place", [entry["sourceId"] for entry in plan["adjustments"]])
        self.assertEqual(yield_distribution(2, 3, 3, 12500), {2: 5000, 3: 15000})


if __name__ == "__main__":
    unittest.main()
