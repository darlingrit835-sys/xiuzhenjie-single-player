import json
from pathlib import Path
import subprocess
import sys
from tempfile import TemporaryDirectory
import unittest

from generate_affinity_classes import generate_classes


def sample_config():
    return {
        "legacyGrades": [{"id": i} for i in (4, 8, 12, 16)],
        "polarities": [{"id": i, "name": name} for i, name in enumerate(("阴", "阳", "平和"), 1)],
        "elements": [{"id": i, "name": name} for i, name in enumerate("金木水火土", 1)],
        "qiTypes": [{"id": i, "name": f"{i}气"} for i in range(1, 13)],
        "polarityQiWeights": [{"polarityId": p, "qiId": q, "weight": 1}
                              for p in (1, 2) for q in range(1, 13)],
        "elementQiWeights": [{"elementId": e, "qiId": q, "weight": 1}
                             for e in range(1, 6) for q in range(1, 13)],
        "bonusRules": [{"polarityId": p, "elementId": e, "statId": stat, "minBps": low, "maxBps": high}
                       for p in (1, 2) for e in range(1, 6)
                       for stat, low, high in (("primary", 100 * e, 100 * e + 1), ("secondary", p, p))],
    }


def indexed_sample(config):
    runtime = {"schemaVersion": 3, "catalogVersion": "test"}
    locale = {"catalogVersion": "test"}
    for table in ("polarities", "elements", "qiTypes"):
        runtime[table] = {str(row["id"]): {} for row in config[table]}
        locale[table] = {str(row["id"]): {"name": row["name"]} for row in config[table]}
    for table, field in (("polarityQiWeights", "polarityId"), ("elementQiWeights", "elementId")):
        runtime[table] = {}
        for row in config[table]:
            runtime[table].setdefault(str(row[field]), {})[str(row["qiId"])] = row["weight"]
    runtime["bonusRules"] = {}
    for row in config["bonusRules"]:
        elements = runtime["bonusRules"].setdefault(str(row["polarityId"]), {})
        stats = elements.setdefault(str(row["elementId"]), {})
        stats[row["statId"]] = [row["minBps"], row["maxBps"]]
    runtime["legacyGrades"] = {str(row["id"]): {} for row in config["legacyGrades"]}
    runtime["grades"] = {"1": {"multiplierBps": 12500}}
    return runtime, locale


class GenerateClassesTest(unittest.TestCase):
    def test_replay_prefix_full_pool_and_bonus_bounds(self):
        config = sample_config()
        full = generate_classes(config, "初始世界", count=120)
        self.assertEqual(generate_classes(config, "初始世界"), full[:48])
        self.assertEqual(len({row["id"] for row in full}), 120)
        self.assertNotEqual(generate_classes(config, "另一个世界"), full[:48])
        offsets = set()
        for row in full:
            p, e, q = row["polarityId"], row["elementId"], row["qiId"]
            self.assertEqual(row["id"], f"affinity.p{p}.e{e}.q{q:02d}")
            self.assertEqual(row["name"], f"{'阴阳'[p-1]}{'金木水火土'[e-1]}·{q}气")
            self.assertEqual([b["statId"] for b in row["bonuses"]], ["primary", "secondary"])
            offsets.add(row["bonuses"][0]["valueBps"] - 100 * e)
            self.assertEqual(row["bonuses"][1]["valueBps"], p)
        self.assertEqual(offsets, {0, 1})

    def test_both_weights_contribute_to_selection(self):
        config = sample_config()
        config["polarityQiWeights"][0]["weight"] = 10**9
        config["elementQiWeights"][0]["weight"] = 10**9
        for seed in range(10):
            first = generate_classes(config, seed, 1)[0]
            self.assertEqual(first["id"], "affinity.p1.e1.q01")

    def test_reordering_configuration_rows_preserves_rolls(self):
        config = sample_config()
        expected = generate_classes(config, 17)
        reordered = {table: list(reversed(rows)) for table, rows in config.items()}
        self.assertEqual(generate_classes(reordered, 17), expected)

    def test_grade_scales_existing_rolls_with_half_up_rounding(self):
        config = sample_config()
        multipliers = {1: 10000, 15: 15000}
        config["grades"] = [{"id": grade, "multiplierBps": value} for grade, value in multipliers.items()]
        base = generate_classes(config, 19)
        for grade, multiplier in multipliers.items():
            graded = generate_classes(config, 19, grade_id=grade)
            self.assertEqual([r["id"] for r in graded], [r["id"] for r in base])
            for original, adjusted in zip(base, graded):
                self.assertNotIn("gradeId", original)
                self.assertEqual(adjusted["gradeId"], grade)
                pairs = [(b["statId"], b["baseValueBps"]) for b in adjusted["bonuses"]]
                self.assertEqual(pairs, [(b["statId"], b["valueBps"]) for b in original["bonuses"]])
                for bonus in adjusted["bonuses"]:
                    quotient, remainder = divmod(bonus["baseValueBps"] * multiplier, 10000)
                    self.assertEqual(bonus["valueBps"], quotient + int(remainder >= 5000))
        self.assertTrue(any(b["baseValueBps"] % 2 for row in graded for b in row["bonuses"]))

    def test_rejects_historical_unknown_or_unconfigured_grades(self):
        config = {**sample_config(), "grades": [{"id": 1, "multiplierBps": 10000}]}
        for grade in (4, 8, 12, 16):
            with self.assertRaisesRegex(ValueError, "旧版极品"):
                generate_classes(config, 3, grade_id=grade)
        for grade in (0, 17, "1", True):
            with self.assertRaisesRegex(ValueError, "无效的品阶"):
                generate_classes(config, 3, grade_id=grade)
        with self.assertRaisesRegex(ValueError, "缺少或重复"):
            generate_classes(config, 3, grade_id=15)
        config["grades"][0]["multiplierBps"] = 0
        with self.assertRaisesRegex(ValueError, "正整数基点"):
            generate_classes(config, 3, grade_id=1)

    def test_invalid_or_incomplete_configuration(self):
        config = sample_config()
        for count in (0, -1, 121):
            with self.assertRaisesRegex(ValueError, "抽取数量"):
                generate_classes(config, 42, count)
        for table in ("polarityQiWeights", "elementQiWeights"):
            with self.assertRaisesRegex(ValueError, table):
                generate_classes({**config, table: config[table][1:]}, 42)
        with self.assertRaisesRegex(ValueError, "bonusRules"):
            generate_classes({**config, "bonusRules": []}, 42)
        for field, value, message in (("minBps", 9999, "加成范围"), ("minBps", -1, "加成范围"),
                                      ("statId", "", "属性键"), ("statId", 7, "属性键")):
            with self.subTest(field=field, value=value):
                invalid = {**config["bonusRules"][0], field: value}
                with self.assertRaisesRegex(ValueError, message):
                    generate_classes({**config, "bonusRules": [invalid, *config["bonusRules"][1:]]}, 42)

    def test_indexed_rules_replay_legacy_snapshot_in_both_modes(self):
        previous = sample_config()
        previous["grades"] = [{"id": 1, "multiplierBps": 12500}]
        runtime, locale = indexed_sample(previous)
        for grade in (None, 1):
            expected = generate_classes(previous, 31, grade_id=grade)
            translated = generate_classes(runtime, 31, grade_id=grade, locale=locale)
            self.assertEqual(json.dumps(translated), json.dumps(expected))
            raw = generate_classes(runtime, 31, grade_id=grade)
            self.assertEqual(raw, [{k: v for k, v in row.items() if k != "name"} for row in expected])
        with self.assertRaisesRegex(ValueError, "旧版极品"):
            generate_classes(runtime, 31, grade_id=12)
        with self.assertRaisesRegex(ValueError, "catalogVersion"):
            generate_classes(runtime, 31, locale={**locale, "catalogVersion": "other"})
        del runtime["elementQiWeights"]["1"]["2"]
        with self.assertRaisesRegex(ValueError, "elementQiWeights"):
            generate_classes(runtime, 31)

    def test_cli_discovers_matching_locale_and_requires_explicit_path(self):
        runtime, locale = indexed_sample(sample_config())
        script = Path(__file__).with_name("generate_affinity_classes.py").resolve()
        with TemporaryDirectory() as directory:
            folder = Path(directory)
            config_path = folder / "custom.json"
            pack_path = folder / "custom.zh-CN.json"
            config_path.write_text(json.dumps(runtime), encoding="utf-8")
            cmd = [sys.executable, str(script), "--config", str(config_path), "--seed", "31", "--count", "1"]
            self.assertNotIn("name", json.loads(subprocess.check_output(cmd, cwd=folder))[0])
            missing = subprocess.run(cmd + ["--locale", str(pack_path)], cwd=folder, capture_output=True)
            self.assertNotEqual(missing.returncode, 0)
            pack_path.write_text(json.dumps(locale), encoding="utf-8")
            self.assertIn("name", json.loads(subprocess.check_output(cmd, cwd=folder))[0])
            pack_path.write_text(json.dumps({**locale, "catalogVersion": "other"}), encoding="utf-8")
            mismatch = subprocess.run(cmd, cwd=folder, capture_output=True)
            self.assertNotEqual(mismatch.returncode, 0)
            self.assertIn("catalogVersion", mismatch.stderr.decode())

    def test_graded_affix_count_focus_range_and_replay(self):
        runtime, _ = indexed_sample(sample_config())
        ungraded = generate_classes(runtime, 31)
        runtime.update({
            "grades": {str(t * 4 + q): {"tierId": t + 1, "qualityId": q, "multiplierBps": 11000}
                       for t in range(4) for q in range(1, 4)},
            "stats": {stat: {} for stat in ("primary", "secondary", "extra_a", "extra_b", "extra_c")},
            "affixCounts": {str(t): t for t in range(1, 5)},
            "elementFocus": {str(e): "primary" for e in range(1, 6)},
            "polarityFocus": {str(p): "secondary" for p in (1, 2)},
        })
        runtime["affixRanges"] = {s: {str(q): [1000 * q, 1000 * q + 20] for q in range(1, 5)}
                                  for s in runtime["stats"]}
        self.assertEqual(generate_classes(runtime, 31), ungraded)
        for key, grade in runtime["grades"].items():
            full = generate_classes(runtime, 31, grade_id=int(key))
            self.assertEqual(full[:3], generate_classes(runtime, 31, 3, int(key)))
            self.assertEqual([r["id"] for r in full], [r["id"] for r in ungraded])
            for row in full:
                stats = [b["statId"] for b in row["bonuses"]]
                self.assertEqual(len(stats), grade["tierId"])
                self.assertEqual(len(set(stats)), len(stats))
                self.assertEqual(stats[:2], ["primary", "secondary"][:len(stats)])
                for b in row["bonuses"]:
                    low, high = runtime["affixRanges"][b["statId"]][str(grade["qualityId"])]
                    self.assertLessEqual(low, b["valueBps"])
                    self.assertLessEqual(b["valueBps"], high)
        with self.assertRaisesRegex(ValueError, "旧版极品"):
            generate_classes(runtime, 31, grade_id=16)
        runtime["affixCounts"].update({"1": 2, "3": 1})
        for key, expected_count in ((1, 2), (9, 1), (15, 4)):
            test = generate_classes(runtime, 31, grade_id=key)
            self.assertTrue(all(len(row["bonuses"]) == expected_count for row in test))
        runtime["affixRanges"]["extra_b"].pop("3")
        with self.assertRaisesRegex(ValueError, "affixRanges.extra_b.3"):
            generate_classes(runtime, 31, grade_id=15)


if __name__ == "__main__":
    unittest.main()
