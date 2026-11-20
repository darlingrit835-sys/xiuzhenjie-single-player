import unittest

from preview_region import demo_snapshot, map_region, preview_region, role_element


def fixture():
    regions = {
        str(key): {"parentId": parent, "level": level, "elementId": 2, "polarityId": 1, "qiId": 3}
        for key, parent, level in ((1, 0, 0), (11, 1, 1), (111, 11, 2), (1111, 111, 3), (1112, 111, 3))
    }
    modifiers = {
        "11": {"grow": {"2": {"resonanceElementId": 2, "baseBps": 50, "jitterBps": 2}},
               "counter": {"5": {"resonanceElementId": 2, "baseBps": 40, "jitterBps": 1}}},
        "1111": {"grow": {"2": {"resonanceElementId": 2, "baseBps": 100, "jitterBps": 6,
                                "exampleTemplateId": "mat.leaf"}}},
    }
    config = {
        "catalogVersion": "v", "regions": regions, "regionModifiers": modifiers,
        "regionEffects": {"grow": {"targetKind": 2, "modeId": 1}, "counter": {"targetKind": 6, "modeId": 1}},
        "regionResonance": {"sameBps": 3, "generatesBps": 2, "generatedByBps": 1,
                            "overcomesBps": -2, "overcomeByBps": -1, "capBps": 12},
        "elements": {str(i): {"generates": gen, "overcomes": clash}
                     for i, gen, clash in ((1, 3, 2), (2, 4, 5), (3, 2, 4), (4, 5, 1), (5, 1, 3))},
    }
    locale = {
        "catalogVersion": "v", "regions": {k: {"name": k} for k in regions},
        "itemTemplates": {"mat.leaf": {"name": "测试草"}},
        "regionModifiers": {source: {effect: {target: {"label": f"{effect}-{target}"} for target in targets}
                                     for effect, targets in effects.items()} for source, effects in modifiers.items()},
    }
    return config, locale


class RegionPreviewTest(unittest.TestCase):
    def setUp(self):
        self.config, self.locale = fixture()

    def run_preview(self, snapshot=None, period=0, region="1111"):
        return preview_region(self.config, self.locale, region, snapshot, period)

    def test_nearest_scope_and_stable_separate_fluctuation(self):
        flat = self.run_preview()
        self.assertEqual(flat, self.run_preview(region="111101"))
        forms = {e["effectKey"]: e for e in flat["effects"]}
        self.assertEqual((1111, 100, "测试草"), (forms["grow"]["sourceRegionId"], forms["grow"]["baseBps"],
                                                 forms["grow"]["exampleName"]))
        self.assertEqual((11, 5), (forms["counter"]["sourceRegionId"], forms["counter"]["targetElementId"]))
        fluctuations = set()
        for period in range(80):
            report = self.run_preview(period=period)
            for effect in report["effects"]:
                self.assertLessEqual(abs(effect["periodBps"]), 6 if effect["effectKey"] == "grow" else 1)
                self.assertEqual(effect["totalBps"], effect["baseBps"] + effect["periodBps"])
            fluctuations.add(report["effects"][1]["periodBps"])
        self.assertGreater(len(fluctuations), 1)
        self.assertEqual(flat, self.run_preview())

    def test_exact_location_binding_and_role_snapshot(self):
        regions = self.config["regions"]
        for loc in ("1111", "111101", "11111206", "1111W6", "1111W63"):
            self.assertEqual(map_region(regions, loc), "1111")
        for invalid in ("111100", "111113", "11110001", "11110107", "1111W7", "1111W10", "1111oops", "999901"):
            self.assertIsNone(map_region(regions, invalid))
        snapshot = {
            "wood": {"MapId": "111101", "GongFaNaQi": "gf_3_2", "Fire": 100},
            "water": {"MapId": "1111W63", "GongFaNaQi": "gf_3_3"},
            "gold": {"MapId": "11111206", "Gold": "12", "Wood": 12},
            "empty": {"MapId": "1111W5", "GongFaNaQi": "gf_3_17"},
            "outside": {"MapId": "111201", "Wood": 100},
            "no_county": {"MapId": "111", "Wood": 100},
            "$smithing:reserved": {"MapId": "1111", "Wood": 100},
            "bad": {"MapId": "1111W8", "Wood": 100},
            "legacy-empty": None,
        }
        report = self.run_preview(snapshot)
        self.assertEqual((4, 3), (report["residentCount"], report["resonatingCount"]))
        expected = 3 + 2 - 2
        for effect in report["effects"]:
            self.assertEqual(expected, effect["residentBps"])
        self.assertEqual(self.run_preview(dict(reversed(list(snapshot.items())))), report)
        self.assertEqual(self.run_preview(snapshot, region="111")["residentCount"], 5)
        snapshot["wood"]["MapId"] = "111201"
        self.assertEqual(self.run_preview(snapshot)["effects"][0]["residentBps"], 0)

    def test_counts_are_bounded_after_signed_cancellation(self):
        residents = [{"Id": f"wood-{i}", "MapId": "1111", "GongFaNaQi": "gf_3_2"} for i in range(9)]
        antagonists = [{"Id": f"gold-{i}", "MapId": "1111", "GongFaNaQi": "gf_3_1"} for i in range(9)]
        for rows, expected in ((residents, 12), (antagonists, -12), (residents + antagonists, 9)):
            self.assertTrue(all(e["residentBps"] == expected for e in self.run_preview(rows)["effects"]))
        self.assertEqual(role_element({"GongFaNaQi": "gf_3_16", "Fire": 10}), 1)
        self.assertEqual(role_element({"GongFaNaQi": "gf_1_1", "Fire": 5}), 4)
        self.assertEqual(role_element({"Fire": -3, "Gold": "NaN"}), 0)
        self.assertEqual(self.run_preview(demo_snapshot(self.config, "1111"))["resonatingCount"], 3)

    def test_invalid_keys_and_snapshots(self):
        with self.assertRaisesRegex(ValueError, "版本"):
            preview_region(self.config, {"catalogVersion": "other"}, 1111)
        with self.assertRaisesRegex(ValueError, "地域"):
            self.run_preview(region="111100")
        with self.assertRaisesRegex(ValueError, "周期"):
            self.run_preview(period=-1)
        with self.assertRaisesRegex(ValueError, "重复"):
            self.run_preview([{"Id": "dup", "MapId": "1111"}, {"Id": "dup", "MapId": "1112"}])


if __name__ == "__main__":
    unittest.main()
