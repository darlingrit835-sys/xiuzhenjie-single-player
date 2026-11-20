import json
from pathlib import Path
import unittest

from region_ecology import habitat_weight, region_ecology_summary, source_indexes


class RegionEcologyTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        folder = Path(__file__).resolve().parents[1] / "server/data/config"
        cls.rules = json.loads((folder / "cultivation_affinities.json").read_text())
        cls.locale = json.loads((folder / "cultivation_affinities.zh-CN.json").read_text())

    def test_region_weights_and_enemy_tiers(self):
        rules = self.rules
        for rid, region in rules["regions"].items():
            if region["level"] != 3:
                continue
            for tier in (1, 3, 4):
                result = region_ecology_summary(rules, self.locale, rid, tier)
                self.assertGreater(result["monsterTickets"], 0)
                self.assertTrue(result["gathering"])
                self.assertEqual(sum(m["tickets"] for m in result["monsters"]), result["monsterTickets"])
                for monster in result["monsters"]:
                    mid = str(monster["monsterId"])
                    if tier == 4:
                        self.assertTrue(rules["monsterSpecies"][mid]["allowBoss"])
                    drops = rules["monsterDrops"][mid]
                    by_item = {**drops.get("0", {}), **drops.get(rid, {})}
                    expected = {k: v["chanceBps"][str(tier)] for k, v in by_item.items() if v["chanceBps"][str(tier)]}
                    self.assertEqual({d["itemId"]: d["chanceBps"] for d in monster["drops"]}, expected)

    def test_affinity_caps_apply_even_to_weight_one(self):
        region = {"polarityId": 1, "elementId": 4, "qiId": 3}
        profile = {"polarityId": 1, "primaryElementId": 4, "primaryQiId": 3}
        rule = dict(polarityBps=80, elementBps=150, generatesBps=95, overcomesBps=-110, qiBps=95, capBps=300)
        self.assertEqual(habitat_weight(1, region, profile, self.rules["elements"], rule), (10300, 300))
        profile.update(polarityId=2, primaryElementId=1, primaryQiId=12)
        self.assertEqual(habitat_weight(1, region, profile, self.rules["elements"], rule), (9890, -110))

    def test_reverse_refs_resolve_to_source_rows(self):
        rules = self.rules
        self.assertEqual(source_indexes(rules)["resourceSources"], rules["resourceSources"])
        for item, sources in rules["resourceSources"].items():
            self.assertTrue(rules["resourceUses"][item])
            for region, method in sources.get("gather", ()):
                self.assertIn(item, rules["regionResources"][str(region)][str(method)])
            for monster, region in sources.get("drop", ()):
                self.assertIn(item, rules["monsterDrops"][str(monster)][str(region)])
            for type_id in sources.get("legacyTypes", ()):
                self.assertEqual(rules["legacyMaterialTypes"][str(type_id)], item)
        for system, items in rules["systemResources"].items():
            for item in items:
                self.assertIn(system, rules["resourceUseRoles"][item])
