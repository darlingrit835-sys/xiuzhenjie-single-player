"""统一词条覆盖、旧模板去重、玩家继承与量耗结算。"""

from copy import deepcopy
import json
from pathlib import Path
import unittest

if __package__:
    from . import content_affixes as affixes
    from . import export_affinities
else:
    import content_affixes as affixes
    import export_affinities


class ContentAffixTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        data = Path(__file__).resolve().parents[1] / "server/data"
        cls.cat = json.loads((data / "config/cultivation_affinities.json").read_text(encoding="utf-8"))
        cls.rows = export_affinities.read_sheets(data / "config_excel/修仙属性基础表.xlsx")

    def test_every_registered_template_has_one_valid_budget(self):
        cat = self.cat
        for kind, templates in cat["creation"]["templates"].items():
            for tid in templates:
                snapshot = affixes.roll_affixes(cat, int(kind), int(tid), 561)
                self.assertTrue(affixes.stack_affixes(cat, {"instance": snapshot}))
                if kind in ("1", "3"):
                    self.assertEqual(snapshot, affixes.roll_affixes(cat, int(kind), int(tid), 912))
        self.assertEqual(affixes.describe(cat, 4, 4)["gradeId"], cat["itemTemplates"]["obj.zhenrang_pill"]["gradeId"])
        self.assertEqual(affixes.describe(cat, 13, 81003)["gradeId"], 10)
        sword = cat["creation"]["idsByKey"]["1"]["made.frost_sword"]
        snapshot = affixes.roll_affixes(cat, 1, sword, 1)
        self.assertEqual(affixes.stack_affixes(cat, {"sword": snapshot}), cat["itemAffixes"]["made.frost_sword"])

    def test_seeded_weighted_choice_and_creation(self):
        cat = self.cat
        weapon = cat["creation"]["idsByKey"]["1"]["wp.zhufeng_bow"]
        rule = affixes.describe(cat, 1, weapon)
        derived = affixes.roll_created(cat, 1, 15, 72, prototype_id=weapon)
        self.assertEqual(derived["groupId"], rule["groupId"])
        self.assertEqual(len(derived["affixes"]), 4)
        self.assertEqual(derived, affixes.roll_created(cat, 1, 15, 72, prototype_id=weapon))
        self.assertNotEqual(derived, affixes.roll_created(cat, 1, 15, 82, prototype_id=weapon))
        # 已发布玩家作品携带的原型池可以继承；也可明确选一个可用组。
        self.assertEqual(3, affixes.roll_created(cat, 1, 5, 1, inherited_group_id=3)["groupId"])
        self.assertEqual(7, affixes.roll_created(cat, 1, 5, 1, inherited_group_id=3, group_id=7)["groupId"])
        for kind, grade, group in ((1, 4, 3), (1, 1, 10), (12, 1, 10)):
            with self.assertRaises(ValueError):
                affixes.roll_created(cat, kind, grade, 12, group_id=group)
        base = dict(affixes.weighted_candidates(cat, 5, {}))
        tuned = dict(affixes.weighted_candidates(cat, 5, {"polarityId": 2, "elementId": 1, "qiId": 2}))
        self.assertGreater(tuned[1], base[1])  # 金行和雷气对攻击的偏好
        self.assertEqual(tuned[2], base[2])

    def test_stacking_budget_and_caps(self):
        source = {"groupId": 2, "gradeId": 3, "affixes": {1: 400}}
        combined = affixes.stack_affixes(self.cat, {str(i): source for i in range(6)})
        self.assertEqual({"attack": 1050}, combined)
        with self.assertRaisesRegex(ValueError, "品质区间"):
            affixes.stack_affixes(self.cat, {"x": dict(source, affixes={1: 405})})
        with self.assertRaisesRegex(ValueError, "数量不符"):
            affixes.stack_affixes(self.cat, {"x": dict(source, affixes={1: 400, 6: 401})})

    def test_cost_yield_probability_and_quantity_bounds(self):
        c = self.cat
        self.assertEqual(95, affixes.apply_value(c, "material_cost", 100, {"material_cost": 550}))
        self.assertEqual(1, affixes.apply_value(c, "material_cost", 1, {"material_cost": 550}))
        self.assertEqual(0, affixes.apply_value(c, "material_cost", 0, {"material_cost": 550}))
        self.assertEqual(98, affixes.apply_value(c, "listing_fee", 100, {"listing_fee": 500}, floor=98))
        self.assertEqual(4, affixes.apply_value(c, "batch_yield", 3, {"batch_yield": 1000}, ticket=2999))
        self.assertEqual(3, affixes.apply_value(c, "batch_yield", 3, {"batch_yield": 1000}, ticket=3000))
        self.assertEqual(3, affixes.apply_value(c, "batch_yield", 3, {"batch_yield": 1000}, ceiling=3, ticket=0))
        self.assertEqual(0, affixes.apply_value(c, "rare_drop_chance", 0, {"rare_drop_chance": 300}))
        self.assertEqual(9800, affixes.apply_value(c, "success_chance", 9760, {"success_chance": 150}, ceiling=9800))

    def test_duplicate_or_exhausted_pool_and_grade_override_rejected(self):
        rows = dict(self.rows)
        loc, rule = rows["词条组候选"][0]
        rows["词条组候选"] = [*rows["词条组候选"], (loc, rule)]
        with self.assertRaisesRegex(ValueError, "重复"):
            affixes.compile_affix_catalog(self.cat, rows)
        rows["词条组候选"] = [r for r in self.rows["词条组候选"] if r[1]["groupId"] != rule["groupId"]]
        with self.assertRaisesRegex(ValueError, "少于最高品阶"):
            affixes.compile_affix_catalog(self.cat, rows)
        rows = deepcopy(self.rows)
        loc, row = rows["模板词条特例"][0]
        row.update(kindId=1, templateId=1, gradeId=1, groupId=0)
        with self.assertRaisesRegex(ValueError, "没有原品阶"):
            affixes.compile_affix_catalog(self.cat, rows)


if __name__ == "__main__":
    unittest.main()
