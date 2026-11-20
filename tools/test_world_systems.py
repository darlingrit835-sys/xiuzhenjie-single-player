"""世界玩法细表的跨域引用、反查和界限回归。"""

from copy import deepcopy
from pathlib import Path
import unittest

if __package__:
    from . import export_affinities, world_systems
else:
    import export_affinities
    import world_systems


class WorldSystemsTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        workbook = Path(__file__).resolve().parents[1] / "server/data/config_excel/修仙属性基础表.xlsx"
        cls.sheets = export_affinities.read_sheets(workbook)
        cls.config = export_affinities.build_config(cls.sheets)
        cls.rules, cls.labels = world_systems.compile_world(cls.config, cls.sheets)

    def rejected(self, sheet, id_field, key, change, error):
        sheets = dict(self.sheets)
        sheets[sheet] = deepcopy(sheets[sheet])
        row = next(row for _, row in sheets[sheet] if str(row[id_field]) == str(key))
        row.update(change)
        with self.assertRaisesRegex(ValueError, error):
            world_systems.compile_world(self.config, sheets)

    def test_crafting_and_farming_reuse_existing_outputs_and_sources(self):
        self.assertIn(1101, self.rules["cropsByHarvest"]["mat.wild_herb"])
        self.assertIn(1107, self.rules["cropsByHarvest"]["mat.wild_herb"])
        self.assertEqual([6103], self.rules["forgePatternsByBlank"]["wp.tingquan_staff"])
        pattern = self.rules["formationsByMaterial"]["mat.frost_silver"][0]
        self.assertEqual("array.mizong", self.rules["formations"][pattern]["profileId"])
        self.assertIn([1411, 2], self.config["worldRules"]["resourceSources"]["mat.frost_silver"]["gather"])
        self.rejected("阵材位置", "materialId", "mat.frost_silver", {"kindId": 1}, "阵位用途")
        self.rejected("布阵阵式", "id", 2102, {"elementId": 1}, "阵法主行所克")

    def test_cultivated_propagules_need_one_reachable_origin(self):
        cfg = deepcopy(self.config)
        cfg["worldRules"]["resourceSources"].pop("mat.dream_lotus")
        with self.assertRaisesRegex(ValueError, "繁育链缺少地区来源"):
            world_systems.compile_world(cfg, self.sheets)
        sheets = dict(self.sheets)
        rows = list(sheets["灵植栽培"])
        loc, crop = rows[0]
        rows.append((loc, dict(crop, id=90001101, inputId="mat.ninglu_herb", outputId="mat.dream_lotus")))
        sheets["灵植栽培"] = rows
        rules, _ = world_systems.compile_world(cfg, sheets)
        self.assertIn(90001101, rules["cropsByHarvest"]["mat.dream_lotus"])

    def test_traditions_and_leadership(self):
        tradition = self.rules["traditions"][3102][1]
        self.assertEqual(tradition["profileId"], self.config["methodRules"]["templates"][tradition["methodId"]]["profileId"])
        self.rejected("组织职权", "officeId", 1, {"seats": 2}, "单席首领")
        self.rejected("组织传承", "organizationId", 3102, {"methodId": 0}, "与传承对象")

    def test_combat_actions_and_mode_boundaries(self):
        move = self.rules["moves"][4102]
        self.assertEqual((10, 1), (move["targetQiId"], move["magnitude"]))
        self.assertEqual("dispel_qi", self.rules["moveActions"][move["actionId"]]["operation"])
        self.rejected("斗法模式", "id", 4202, {"monsterDrops": 1}, "关闭妖兽产出")
        self.rejected("战斗招式", "id", 4102, {"targetQiId": 0}, "净化须引用目标气")
        self.rejected("战斗招式", "id", 4103, {"magnitude": 10001}, "之间")

    def test_market_categories_inherit_and_math_is_bounded(self):
        categories = {row["id"]: row for row in self.config["itemCategories"]}
        policy = dict(self.rules["marketCategories"][5101], **{"wp.sword": 0})
        self.assertTrue(world_systems.listing_allowed(categories, policy, "wp.bow"))
        self.assertFalse(world_systems.listing_allowed(categories, policy, "wp.sword"))
        self.assertFalse(world_systems.listing_allowed(categories, {}, "wp.staff"))
        self.rejected("拍卖规约", "id", 5101, {"maxUnitPrice": 2**63-1}, "整单总价")
        self.rejected("拍卖规约", "id", 5102, {"minRaiseBps": 0}, "竞价须有正加价")

    def test_player_variants_share_profile_but_keep_distinct_keys(self):
        sheets = dict(self.sheets)
        loc, move = sheets["战斗招式"][0]
        sheets["战斗招式"] = [*sheets["战斗招式"], (loc, dict(move, id=90004101, name=move["name"]))]
        rules, labels = world_systems.compile_world(self.config, sheets)
        self.assertIn(90004101, rules["movesByProfile"][move["profileId"]])
        self.assertNotIn("name", rules["moves"][90004101])
        self.assertEqual(move["name"], labels["moves"][90004101]["name"])


if __name__ == "__main__":
    unittest.main()
