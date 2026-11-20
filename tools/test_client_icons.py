import copy
import json
import unittest

from creation_catalog import RESOURCES, UI_POOL_SHEET, client_icon_index
from export_affinities import DATA, build_config, compile_catalog, read_sheets


class ClientIconIndexTest(unittest.TestCase):
    def test_legacy_namespaces_and_reusable_art(self):
        runtime = json.loads((DATA / "config/cultivation_affinities.json").read_text())
        index = json.loads((RESOURCES / "CatalogIcons/index.json").read_text())
        self.assertEqual(client_icon_index(runtime), index)
        monsters = {(r["oldType"], r["oldIcon"]): r["id"] for r in index["monsters"] if r["oldType"]}
        self.assertEqual((monsters[2002, 1], monsters[2001, 1]), (101, 201))
        covers = {r["legacy"]: r["icon"] for r in index["methods"] if r["legacy"]}
        self.assertEqual(covers["gf_3_4"], 74015)
        pool = next(p for p in index["pools"] if p["key"] == "beast_wave")
        self.assertGreater(len(pool["icons"]), 1)
        kinds = {r["id"]: r["kind"] for r in index["icons"]}
        self.assertEqual({kinds[icon] for icon in pool["icons"]}, {2})

    def test_pool_rejects_duplicate_and_wrong_kind(self):
        sheets = read_sheets(DATA / "config_excel/修仙属性基础表.xlsx")
        for field, value in (("iconId", "74002"), ("kindId", "3"), ("iconId", None)):
            changed = copy.copy(sheets)
            rows = list(sheets[UI_POOL_SHEET])
            loc, member = rows[0]
            invalid = dict(member)
            if value is None:
                rows.append((loc, invalid))
            else:
                invalid[field] = value
                rows[0] = (loc, invalid)
            changed[UI_POOL_SHEET] = rows
            with self.subTest(field=field, value=value), self.assertRaisesRegex(ValueError, "图池"):
                compile_catalog(build_config(changed))
