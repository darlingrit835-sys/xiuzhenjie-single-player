"""人物筹划数据和战利品反查的回归用例。"""

from copy import deepcopy
import json
from pathlib import Path
import unittest

if __package__:
    from . import character_tables, export_affinities
else:
    import character_tables
    import export_affinities


class CharacterTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        data = Path(__file__).resolve().parents[1] / "server/data"
        cls.sheets = export_affinities.read_sheets(data / "config_excel/修仙属性基础表.xlsx")
        cls.config = export_affinities.build_config(cls.sheets)
        cls.runtime = json.loads((data / "config/cultivation_affinities.json").read_text(encoding="utf-8"))

    def rejected(self, name, pick, change, message):
        sheets = dict(self.sheets)
        sheets[name] = deepcopy(sheets[name])
        row = next(row for _, row in sheets[name] if pick(row))
        row.update(change)
        with self.assertRaisesRegex(ValueError, message):
            character_tables.compile_characters(self.config, sheets)

    def test_lineage_cultivator_loot_is_filtered_by_loadout(self):
        r = self.runtime
        picked = character_tables.reward_options(r, 3, character_id=81003, map_id=r["encounterMapIds"]["1112W22"])
        books = [e for e in picked["entries"] if e["kindId"] == 1]
        self.assertEqual({73017, 72005}, {e["methodId"] for e in books})
        self.assertTrue(all(e["itemId"] == "obj.method_manual" and e["maxQty"] == 1 for e in books))
        relics = [e for e in picked["entries"] if e["kindId"] == 2]
        self.assertEqual([(85, "obj.reed_bell")], [(e["chanceBps"], e["itemId"]) for e in relics])
        with self.assertRaisesRegex(ValueError, "战区与档位"):
            character_tables.reward_options(r, 1, character_id=81003)
        with self.assertRaisesRegex(ValueError, "战区与档位"):
            character_tables.reward_options(r, 3, character_id=81003, map_id=r["encounterMapIds"]["1111W22"])

    def test_map_and_species_share_one_bonus_pool(self):
        r = self.runtime
        zone = r["encounterMapIds"]["1111W11"]
        pool = character_tables.reward_options(r, 1, map_id=zone)
        self.assertEqual(pool["poolId"], 82001)
        species = character_tables.reward_options(r, 1, monster_id=303)
        self.assertEqual(pool, species)
        # 青岚螳在本郡松风竹海出没，没有专库时继承郡库。
        self.assertEqual(pool, character_tables.reward_options(r, 1, monster_id=301, map_id=zone))
        with self.assertRaisesRegex(ValueError, "不在所选战区"):
            character_tables.reward_options(r, 1, monster_id=324, map_id=zone)

    def test_equipment_methods_and_panel_must_be_valid(self):
        self.rejected("随身器物", lambda r: int(r["slotId"]) == 2, {"itemId": "obj.ninglu_pill"}, "槽位不符")
        self.rejected("修士所学", lambda r: int(r["characterId"]) == 81001, {"methodId": 73013}, "修习等级")
        self.rejected("修士面板", lambda r: True, {"hp": 0}, "之间")
        self.rejected("修士行迹", lambda r: int(r["tierId"]) == 1, {"tierId": 4}, "没有该档位")

    def test_drop_validates_item_ownership_type_and_probability(self):
        self.rejected("奇遇掉落", lambda r: int(r["kindId"]) == 1, {"itemId": "obj.reed_bell"}, "秘籍载体")
        self.rejected("奇遇掉落", lambda r: int(r["kindId"]) == 2, {"maxQty": 2}, "最多出一本")
        self.rejected("奇遇掉落", lambda r: True, {"eliteBps": 10001}, "之间")


if __name__ == "__main__":
    unittest.main()
