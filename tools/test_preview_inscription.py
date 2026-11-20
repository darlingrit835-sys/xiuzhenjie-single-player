from copy import deepcopy
import unittest

from crafting_preview import crafting_sources, find_recipe
from export_affinities import build_config, read_sheets
from preview_inscription import CONFIG, draw_plan, material_guide, read_json


class InscriptionPreviewTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.rules = read_json(CONFIG)
        cls.locale = read_json(CONFIG.with_name(CONFIG.stem + ".zh-CN.json"))

    def recipe(self, query):
        return find_recipe(self.rules, self.locale, query, "inscription", "符方")[1]

    def test_reverse_lookup_through_processed_paper_ink_and_brush(self):
        recipe = self.recipe("庚金破甲符")
        self.assertEqual(recipe, self.recipe("obj.gengjin_sign"))
        materials = material_guide(self.rules, self.locale, recipe)
        self.assertEqual([(row["role"], row["qty"]) for row in materials],
                         [("paper", 3), ("ink", 1), ("auxiliary", 1)])
        reeds = materials[0]["sources"]["components"][0]
        self.assertEqual((reeds["itemId"], reeds["qty"]), ("mat.frost_reed", 3))
        self.assertEqual({route["regionId"] for route in reeds["sources"]["gather"]}, {1411})
        ink_parts = {row["itemId"]: row["sources"] for row in materials[1]["sources"]["components"]}
        self.assertIn(1411, {route["regionId"] for route in ink_parts["mat.thunder_horn"]["drop"]})
        sources = crafting_sources(self.rules, self.locale)
        bristles = next(row for row in sources("obj.chijing_brush")["components"]
                        if row["itemId"] == "mat.thunder_whisker")
        self.assertIn(1211, {route["regionId"] for route in bristles["sources"]["drop"]})
        self.assertNotIn("obj.chijing_brush", [row["itemId"] for row in materials])

    def test_matching_brush_and_method_affect_all_three_outputs(self):
        recipe = self.recipe("sign.yinlei")
        self.assertEqual(recipe["spellProfileId"], "spell.yinlei")
        fire = draw_plan(self.rules, self.locale, recipe, 4, "obj.chijing_brush", "gf_3_9")
        water = draw_plan(self.rules, self.locale, recipe, 4, "obj.xuanlin_brush", "gf_3_9")
        self.assertEqual((fire["successBps"], water["successBps"]), (7971, 7610))
        self.assertGreater(fire["yieldTickets"][4], water["yieldTickets"][4])
        self.assertNotEqual(fire["effectRange"], water["effectRange"])
        fire_with_water_method = draw_plan(self.rules, self.locale, recipe, 4, "obj.chijing_brush", "gf_3_3")
        self.assertLess(fire_with_water_method["successBps"], fire["successBps"])
        with self.assertRaisesRegex(ValueError, "四品符师"):
            draw_plan(self.rules, self.locale, recipe, 3, "obj.chijing_brush")

    def test_local_bonus_matches_ink_and_yield_cap_comes_from_paper(self):
        recipe = self.recipe("轻身符")
        baseline = draw_plan(self.rules, self.locale, recipe, 2, "obj.xuanlin_brush", "gf_3_3")
        local = draw_plan(self.rules, self.locale, recipe, 2, "obj.xuanlin_brush", "gf_3_3", "1121W11", period=7)
        self.assertEqual((local["patternElementId"], local["inkElementId"]), (2, 3))
        self.assertEqual(local["successBps"] - baseline["successBps"], 101)
        self.assertEqual(local["yieldTickets"], baseline["yieldTickets"])
        self.assertEqual(local["effectRange"], baseline["effectRange"])

        revised = deepcopy(self.rules)
        revised["creationInputs"][recipe["outputId"]][recipe["paperId"]] = 3
        plan = draw_plan(revised, self.locale, recipe, 9, "obj.chijing_brush", "gf_3_9")
        self.assertEqual(plan["paperCap"], 3)
        self.assertEqual(max(plan["yieldTickets"]), 3)
        self.assertEqual(sum(plan["yieldTickets"].values()), 20000)

    def test_export_rejects_ink_used_as_paper(self):
        workbook = CONFIG.parents[1] / "config_excel" / "修仙属性基础表.xlsx"
        sheets = read_sheets(workbook)
        _, first = sheets["符方表"][0]
        first["paperId"] = first["inkId"]
        with self.assertRaisesRegex(ValueError, "talisman_paper"):
            build_config(sheets)


if __name__ == "__main__":
    unittest.main()
