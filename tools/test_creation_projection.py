import copy
import random
import sqlite3
import unittest

from affinity_runtime import compile_catalog
from export_affinities import DATA, build_config, read_sheets
from history_projection import capture_clear, draw, settle_victory
from preview_projection import open_demo
from preview_region import CONFIG, read_json


class CreationAndProjectionTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.catalog = read_json(CONFIG)
        cls.locale = read_json(CONFIG.with_name(CONFIG.stem + ".zh-CN.json"))

    def setUp(self):
        self.db, self.gear = open_demo(self.catalog, self.locale, 81001)
        self.addCleanup(self.db.close)
        self.panel = self.catalog["characters"]["panels"]["81001"]
        self.zone = self.catalog["encounterMapIds"]["1111W11"]

    def clear(self, event=1, author=11, time=50, equipment=None):
        return capture_clear(self.db, self.catalog, event, author, self.zone, time, "自取道号", 12, self.panel,
                             self.gear if equipment is None else equipment)

    def test_rename_uses_same_icon_and_template(self):
        sheets = read_sheets(DATA / "config_excel/修仙属性基础表.xlsx")
        for tab in ("功法目录", "怪物图鉴"):
            sheets[tab][0][1]["name"] = "自取道号"
        runtime, locale = compile_catalog(build_config(sheets))
        self.assertEqual(runtime["creation"], self.catalog["creation"])
        self.assertEqual(locale["methods"]["templates"][71001]["name"], "自取道号")
        self.assertEqual(locale["monsterSpecies"]["101"]["name"], "自取道号")

    def test_create_original_and_publish_once(self):
        kind = 3
        tid = self.catalog["creation"]["playerIdStart"]
        self.db.execute("INSERT INTO content_template VALUES (?,?,?)", (kind,tid,11))
        self.db.execute("INSERT INTO content_revision VALUES (?,?,1,0,?,?,?)",
                        (kind,tid,"新功法",self.catalog["catalogVersion"],'{"iconId":71001}'))
        self.db.execute("UPDATE content_revision SET state=1 WHERE kind_id=? AND template_id=?", (kind,tid))
        for sql in ("UPDATE content_revision SET name='改旧版' WHERE template_id=?",
                    "INSERT OR REPLACE INTO content_template VALUES (3,?,12)",
                    "DELETE FROM content_template WHERE template_id=?"):
            with self.assertRaises(sqlite3.IntegrityError):
                self.db.execute(sql,(tid,))
        self.db.execute("INSERT INTO content_revision VALUES (?,?,2,0,?,?,?)",
                        (kind,tid,"新道号",self.catalog["catalogVersion"],'{"iconId":71001}'))
        self.db.execute("INSERT INTO content_reference VALUES (3,?,2,1,3,?,1)", (tid,tid))
        self.db.execute("UPDATE content_revision SET state=1 WHERE template_id=? AND revision=2", (tid,))
        self.assertEqual(self.db.execute("SELECT name FROM content_revision WHERE template_id=? ORDER BY revision",
                                         (tid,)).fetchall(), [("新功法",),("新道号",)])
        with self.assertRaises(sqlite3.IntegrityError):
            self.db.execute("INSERT INTO content_template VALUES (12,?,11)",(tid,))

    def test_draw_filters_and_replays_including_misses(self):
        self.assertTrue(self.clear())
        self.assertFalse(self.clear())
        with self.assertRaises(ValueError):
            self.clear(author=12)
        self.assertTrue(self.clear(2,11,75))
        self.assertTrue(self.clear(3,11,65))
        rng = random.Random(31)
        self.assertEqual(draw(self.db,self.catalog,1,22,self.zone,80,rng),2)
        self.assertEqual(draw(self.db,self.catalog,1,22,self.zone,90,random.Random(0)),2)
        self.assertIsNone(draw(self.db,self.catalog,2,11,self.zone,80,random.Random(31)))
        self.assertIsNone(draw(self.db,self.catalog,3,22,self.zone,80,random.Random(0)))
        self.assertIsNone(draw(self.db,self.catalog,3,22,self.zone,81,random.Random(31)))
        self.assertIsNone(draw(self.db,self.catalog,4,22,self.zone,75+72*3600,random.Random(31)))
        with self.assertRaises(ValueError):
            draw(self.db,self.catalog,1,23,self.zone,80,rng)
        self.assertIsNone(draw(self.db,self.catalog,5,22,self.catalog["encounterMapIds"]["1111W12"],80,rng))

    def test_snapshot_is_pinned_and_reward_slot_is_idempotent(self):
        gear = copy.deepcopy(self.gear)
        next(iter(gear["equipment"].values()))["revision"] = 99
        with self.assertRaises(ValueError):
            self.clear(equipment=gear)
        self.assertEqual(self.db.execute("SELECT count(*) FROM projection_run").fetchone()[0],0)
        self.clear()
        draw(self.db,self.catalog,9,22,self.zone,80,random.Random(31))
        self.db.execute("INSERT INTO projection_claim VALUES (9,1)")
        item = next(iter(self.gear["equipment"].values()))["templateId"]
        self.db.execute("INSERT INTO projection_grant VALUES (9,1,1,1,?,1,1,NULL,NULL,NULL)",(item,))
        with self.assertRaises(sqlite3.IntegrityError):
            self.db.execute("INSERT INTO projection_grant VALUES (9,1,1,1,?,1,1,NULL,NULL,NULL)",(item,))
        with self.assertRaises(sqlite3.IntegrityError):
            self.db.execute("UPDATE projection_run SET hp=1 WHERE run_id=1")


    def test_manual_grants_preserve_the_book_and_embedded_method(self):
        class AlwaysHit:
            def randrange(self, limit):
                return 0
            def randint(self, low, high):
                return low

        self.clear()
        draw(self.db, self.catalog, 7, 22, self.zone, 80, AlwaysHit())
        with self.assertRaises(ValueError):
            settle_victory(self.db, self.catalog, 7, 99, AlwaysHit())
        grants = settle_victory(self.db, self.catalog, 7, 22, AlwaysHit())
        manual = next(r for r in grants if r[5] is not None)
        self.assertEqual(manual[2], self.catalog["creation"]["idsByKey"]["1"]["obj.method_manual"])
        self.assertEqual(manual[5:], (3,73003,1))
        self.assertEqual(settle_victory(self.db, self.catalog, 7, 22, random.Random(0)), grants)

    def test_empty_reward_receipt_and_compacted_seats(self):
        from history_projection import remove_from_pool
        for i in range(5):
            self.clear(i+1, 40+i, i+1)
        with self.db:
            remove_from_pool(self.db,self.zone,42)
        seats = self.db.execute("SELECT seat FROM projection_pool WHERE map_id=? ORDER BY seat",(self.zone,))
        self.assertEqual([r[0] for r in seats],[1,2,3,4])
        draw(self.db,self.catalog,8,22,self.zone,80,random.Random(31))
        self.assertEqual(settle_victory(self.db,self.catalog,8,22,random.Random(0)), [])
        self.assertEqual(self.db.execute("SELECT count(*) FROM projection_claim").fetchone()[0], 1)
