import copy
import json
from pathlib import Path
import shutil
import subprocess
import sys
from tempfile import TemporaryDirectory
import unittest

from generate_system_methods import CONFIG_DIR, RANKS, UNITY_FIELDS, make_catalog, validate


class SystemMethodTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.runtime = json.loads((CONFIG_DIR / "cultivation_affinities.json").read_text(encoding="utf-8"))
        cls.locale = json.loads((CONFIG_DIR / "cultivation_affinities.zh-CN.json").read_text(encoding="utf-8"))
        cls.rows = make_catalog(cls.runtime, cls.locale)["methods"]

    def test_identity_grades_and_elemental_effects(self):
        self.assertEqual(len(self.rows), 120)
        self.assertEqual(len({m["id"] for m in self.rows}), 120)
        self.assertEqual(len({m["description"] for m in self.rows}), 120)
        for m in self.rows:
            rank = m["rank"]
            tier, quality = divmod(rank - 1, 4)
            self.assertEqual(m["id"], f'sys_{m["polarity"]}_{m["element"]}_{m["qi"]}')
            self.assertIn(rank, RANKS)
            self.assertEqual(m["learningLevel"], 1 + 10 * tier)
            start = 300 if m["kind"] == 2 else 105
            self.assertEqual(m["effect1"], start + m["element"])
            if m["kind"] == 3:
                self.assertEqual(m["effect2"], 601)
            else:
                self.assertIn(m["effect2"], range(start + 1, start + 6))
                self.assertNotEqual(m["effect1"], m["effect2"])
            self.assertEqual((m["effectValue1"], m["effectValue2"]), (5 + rank, 2 + rank // 2))
            prior_base = .8 * 4 ** tier * (1 + quality / 10)
            self.assertLessEqual(abs(m["baseCircle"] / prior_base - 1), .05000001)

    def test_data_drives_forms_and_aptitude(self):
        runtime, locale = copy.deepcopy(self.runtime), copy.deepcopy(self.locale)
        runtime["polarityQiWeights"]["1"]["1"] = 5
        runtime["elementQiWeights"]["1"]["1"] = 5
        locale["elementQiWeights"]["1"]["1"]["name"] = "玉响"
        changed = make_catalog(runtime, locale)["methods"]
        method = next(m for m in changed if m["id"] == "sys_1_1_1")
        self.assertEqual(method["baseCircle"], .84)
        self.assertIn("玉响", method["description"])
        self.assertEqual([m["id"] for m in changed], [m["id"] for m in self.rows])
        locale["catalogVersion"] += "-changed"
        with self.assertRaisesRegex(ValueError, "同版"):
            make_catalog(runtime, locale)

    def test_rejects_duplicate_and_missing_entries(self):
        with self.assertRaisesRegex(ValueError, "组合"):
            validate(self.rows[:-1])
        with self.assertRaisesRegex(ValueError, "重名"):
            validate([self.rows[0], {**self.rows[1], "name": self.rows[0]["name"]}, *self.rows[2:]])
        with self.assertRaisesRegex(ValueError, "品阶"):
            validate([{**self.rows[0], "rank": 4}, *self.rows[1:]])

    def test_disk_check_and_optional_unity_projection(self):
        with TemporaryDirectory() as tmp:
            root = Path(tmp)
            for name in ("cultivation_affinities.json", "cultivation_affinities.zh-CN.json"):
                shutil.copy(CONFIG_DIR / name, root / name)
            script = Path(__file__).with_name("generate_system_methods.py")
            base_cmd = [sys.executable, str(script), "--config-dir", str(root)]
            both_cmd = base_cmd + ["--unity-project", str(root / "UnityProject"), "--web-dir", str(root / "boards")]
            subprocess.run(both_cmd, check=True, capture_output=True)
            subprocess.run(both_cmd + ["--check"], check=True, capture_output=True)
            unity_path = root / "UnityProject/Assets/Resources/InkCultivation/SystemMethods.json"
            unity_rows = json.loads(unity_path.read_text(encoding="utf-8"))["methods"]
            self.assertEqual(unity_rows, [{k: row[k] for k in UNITY_FIELDS} for row in self.rows])
            web_path = root / "boards/source/system-methods.js"
            script_text = web_path.read_text(encoding="utf-8")
            payload = json.loads(script_text.split("window.systemMethodCatalog = ", 1)[1].rstrip(";\n"))
            self.assertEqual([m["id"] for m in payload["methods"]], [m["id"] for m in self.rows])
            self.assertEqual(payload["qi"]["12"], "幻")
            web_path.write_text("// stale", encoding="utf-8")
            self.assertNotEqual(subprocess.run(both_cmd + ["--check"], capture_output=True).returncode, 0)
            web_path.write_text(script_text, encoding="utf-8")
            unity_path.write_text("{}", encoding="utf-8")
            mismatch = subprocess.run(both_cmd + ["--check"], capture_output=True)
            self.assertNotEqual(mismatch.returncode, 0)
            self.assertEqual(unity_path.read_text(encoding="utf-8"), "{}")
            # A server-only checkout may check its own release data without installing Unity.
            subprocess.run(base_cmd + ["--check"], check=True, capture_output=True)


if __name__ == "__main__":
    unittest.main()
