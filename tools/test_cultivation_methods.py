import copy
import json
import unittest

from crafting_preview import method_affinity
from cultivation_methods import compile_methods
from export_affinities import DATA, build_config, read_sheets
from preview_region import role_element


class CultivationMethodsTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.sheets = read_sheets(DATA / "config_excel/修仙属性基础表.xlsx")
        cls.base = build_config(cls.sheets)
        cls.catalog = json.loads((DATA / "config/cultivation_affinities.json").read_text())

    def test_legacy_ids_grades_elements_and_all_panel_effects(self):
        expected_effects = {1: (101, 102), 2: (301, 302), 3: (105, 601)}
        rules = self.catalog["methods"]
        for path, (first, second) in expected_effects.items():
            for grade in range(1, 17):
                template_id = rules["byLegacyId"][f"gf_{path}_{grade}"]
                definition = rules["templates"][str(template_id)]
                tier, quality = divmod(grade-1, 4)
                base = 80 * (1, 4, 16, 64)[tier] * (10 + quality)
                self.assertEqual((path, grade, base, 1 + tier*10), (
                    definition["pathId"], definition["gradeId"], definition["circleMilli"], definition["minLevel"]))
                self.assertEqual(self.catalog["profiles"][definition["profileId"]]["primaryElementId"], (grade-1) % 5 + 1)
                self.assertEqual(rules["legacyEffects"][str(template_id)], {str(first): 5+grade, str(second): 2+grade//2})

    def test_legacy_and_numeric_crafting_and_resident_snapshot(self):
        for system in ("alchemy", "inscription", "smithing"):
            self.assertEqual(method_affinity(self.catalog, system, "gf_3_3"),
                             method_affinity(self.catalog, system, 73003))
            self.assertEqual(method_affinity(self.catalog, system, 73017)[0], 2)
        with self.assertRaisesRegex(ValueError, "途径"):
            method_affinity(self.catalog, "alchemy", 71017)
        self.assertEqual(role_element({"GongFaNaQi": 73017, "Fire": 10}, self.catalog), 2)
        self.assertEqual(role_element({"GongFaNaQi": 71017, "Fire": 10}, self.catalog), 4)

    def test_new_path_template_and_companion_mode_can_be_added_as_rows(self):
        sheets = copy.deepcopy(self.sheets)
        sheets["修炼途径"].append(("拓展修法", {"id": "4", "key": "meridian", "name": "通脉", "equipField": None, "note": "测试"}))
        sheets["兼修组合"].extend([
            ("第四路单修", {"id": "8", "name": "养脉", "minLevel": "11", "legacyLocation": None, "note": "测试"}),
            ("通脉兼修", {"id": "9", "name": "脉气同修", "minLevel": "31", "legacyLocation": None, "note": "测试"}),
        ])
        for mode, path, value in [(8, 4, 10000), (9, 4, 6000), (9, 3, 7000)]:
            sheets["兼修分配"].append(("新途径分配", {"modeId": str(mode), "pathId": str(path), "efficiencyBps": str(value)}))
        config = copy.deepcopy(self.base)
        src = next(row for row in config["profiles"] if row["id"] == "method.zhaoying")
        for method_id, profile, name in [(74001, "method.meridian", "温脉篇"), (74002, "method.meridian_ii", "分脉诀")]:
            config["profiles"].append(dict(src, id=profile, name=name))
            sheets["功法目录"].append(("新增功法", {"id": str(method_id), "name": name, "pathId": "4", "profileId": profile,
                "gradeId": "1", "minLevel": "11", "circleMilli": "800", "iconId": str(method_id), "legacyId": None}))
            sheets["功法词条"].append(("新增功法词条", {"methodId": str(method_id), "statId": "healing", "valueBps": "150"}))
        rules, _ = compile_methods(config, sheets)
        self.assertEqual(rules["modes"][9]["efficiencyByPath"], {3: 7000, 4: 6000})
        self.assertTrue(all(rules["templates"][key]["pathId"] == 4 for key in (74001, 74002)))

    def test_prevents_invalid_creation_or_allocation(self):
        examples = [
            ("功法目录", {"circleMilli": "999999"}, "之间"),
            ("功法目录", {"gradeId": "16", "legacyId": None}, "常规品阶"),
            ("功法词条", {"valueBps": "99999"}, "之间"),
            ("兼修分配", {"efficiencyBps": "9999"}, "总效率"),
            ("原功法效果", {"methodId": "71017"}, "旧功法"),
        ]
        for table, invalid, reason in examples:
            with self.subTest(table=table, reason=reason):
                sheets = copy.deepcopy(self.sheets)
                target = next(row for _, row in sheets[table] if
                              (table != "兼修分配" or row["modeId"] == "4") and
                              (table != "功法目录" or row["id"] == "71017"))
                target.update(invalid)
                with self.assertRaisesRegex(ValueError, reason):
                    compile_methods(self.base, sheets)


if __name__ == "__main__":
    unittest.main()
