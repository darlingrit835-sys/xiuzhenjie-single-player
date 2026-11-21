"""从正式阴阳、五行与十二气表编排 120 门系统功法，并可同步 Unity 中文目录。"""

import argparse
from collections import Counter
from itertools import product
import json
from pathlib import Path
import re
from tempfile import NamedTemporaryFile

from generate_affinity_classes import ELEMENTS, POLARITIES, QI_TYPES, find_grade, names, qi_weights


CONFIG_DIR = Path(__file__).resolve().parents[1] / "server/data/config"
RANKS = (1, 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15)
KINDS = (1, 2, 3)


def equipped_effects(kind, element, polarity, qi):
    # Original client supports five per-target flying-weapon and resistance fields.
    # Vary the second affinity across the catalogue to offer a matchup choice.
    secondary = 1 + (element - 1 + 1 + (qi + polarity) % 4) % 5
    if kind == 2:
        return 300 + element, 300 + secondary
    return 105 + element, (601 if kind == 3 else 105 + secondary)
UNITY_FIELDS = ("id", "name", "description", "kind")
UNITY_META = """fileFormatVersion: 2
guid: e569410e26cd45a98877a78ca2585f41
TextScriptImporter:
  externalObjects: {}
  userData:
  assetBundleName:
  assetBundleVariant:
"""

# 按正式 ID 顺序：阴/阳，金木水火土，每行依次风、雷、寒、炎、润、燥、生、煞、清、浊、罡、幻。
# 书名取具体意象。地名、境界、战斗招式都不会从书名反推。
STEMS = {
    (1, 1): "匣风 眠雷 霜台 藏烬 玉露 锁秋 接纹 锈月 素鉴 沉铁 伏铠 匣镜".split(),
    (1, 2): "竹径 桐心 冬芽 炭枝 叶盏 老藤 卧根 蚀柯 澄叶 朽柞 密篱 梦林".split(),
    (1, 3): "洄汀 渊鸣 冰潭 温泉 夜雨 退汐 泉眼 黑涧 澄渊 沉沙 潭幕 蜃楼".split(),
    (1, 4): "灯花 灰雷 冷焰 炉心 蒸霞 慢焙 煨春 余烬 青灯 烟岫 伏焰 灯影".split(),
    (1, 5): "沙纹 地听 冻泉 窑藏 藏泽 封垒 沃田 瘴谷 澄砂 泥塘 岩关 雾垄".split(),
    (2, 1): "剑啸 金霆 冰棱 流金 鸣轮 晒刃 续锋 断戈 镜日 尘钟 金城 分光".split(),
    (2, 2): "千枝 春霆 裂竹 焰树 承霖 曝桐 出芽 荆棘 拂叶 春泥 篱门 千林".split(),
    (2, 3): "翻潮 电潮 破冰 沸泉 骤雨 涌滩 奔泉 逆涡 清浪 浑河 瀑帘 潮光".split(),
    (2, 4): "回焰 飞霆 霜炬 烈炉 云蒸 焦阳 返青 飞烬 明炬 烟涛 赤甲 火轮".split(),
    (2, 5): "沙浪 鸣岫 裂壤 烧陶 盈陂 石垒 春畴 开瘴 淘砂 泥河 擎岳 移丘".split(),
}
SUFFIXES = {
    1: ("照神篇", "观心诀", "守意经", "定神法"),
    2: ("炼骨功", "易筋经", "炼形诀", "锻身篇"),
    3: ("行气诀", "归元经", "吐纳篇", "导引法"),
}
PRACTICE_LINES = {
    1: ("循此气象观想，一周天收束游念，温养神识。",
        "先听息声，再把散开的心念收回，以养神识。",
        "闭目存想，待心头余象散尽，再运一周天养神识。"),
    2: ("随吐息舒肩展背，行过一周天，所养归于体魄。",
        "舒筋时引息，收势时沉气，周而复始以锻体魄。",
        "将一呼一吸分入伸屈两势，徐徐淬炼体魄。"),
    3: ("一息收气，一息归元，行满一周天便积一分修为。",
        "使气息沿脉回环，周天既成，所得累入修为。",
        "将散气徐徐引回丹田，借周天吐纳积蓄修为。"),
}


def phase_label(locale, table, axis_id, qi):
    value = locale.get(table, {}).get(str(axis_id), {}).get(str(qi), {}).get("name")
    if not isinstance(value, str) or not value.strip():
        raise ValueError(f"中文表缺少 {table}.{axis_id}.{qi} 的气相名称")
    return value


def make_catalog(config, locale):
    if config.get("schemaVersion") != 3 or not config.get("catalogVersion") or config["catalogVersion"] != locale.get("catalogVersion"):
        raise ValueError("需要同版的运行属性表与中文资源")
    for table, axis in (("polarities", POLARITIES), ("elements", ELEMENTS), ("qiTypes", QI_TYPES)):
        names(config, table, axis, locale)
    polar_weights = qi_weights(config, "polarityQiWeights", "polarityId", POLARITIES)
    element_weights = qi_weights(config, "elementQiWeights", "elementId", ELEMENTS)
    grades = {rank: find_grade(config, rank) for rank in RANKS}
    rows = []
    for polarity, element, qi in product(POLARITIES, ELEMENTS, QI_TYPES):
        branch = (polarity - 1) * 5 + element - 1
        # 每个分支各含十二品阶。每三种相邻主气分配给三条修途。
        rank = RANKS[(qi - 1 + 5 * branch) % len(RANKS)]
        kind = 1 + (qi - 1 + branch) % 3
        tier = grades[rank]["tierId"]
        quality = grades[rank]["qualityId"]
        p_phase = phase_label(locale, "polarityQiWeights", polarity, qi)
        e_phase = phase_label(locale, "elementQiWeights", element, qi)
        label = STEMS[polarity, element][qi - 1] + SUFFIXES[kind][(qi - 1 + 2 * branch) % 4]
        prose = PRACTICE_LINES[kind][(qi // 3 + element + polarity) % 3]
        description = f"取「{e_phase}」之象，气势属「{p_phase}」。{prose}"
        # Go 旧功法的周天底值为 0.8 × 4^(阶-1) × 同阶品质倍率(1, 1.1, 1.2)。
        # 两表权重合计 6 为中值，每差 1 变动 125 基点，最多正负 500 基点。
        weight = polar_weights[polarity, qi] + element_weights[element, qi]
        shift_bps = max(-500, min(500, (weight - 6) * 125))
        base_circle = round(.8 * 4 ** (tier - 1) * (1 + (quality - 1) / 10) * (10000 + shift_bps) / 10000, 6)
        effect1, effect2 = equipped_effects(kind, element, polarity, qi)
        move_speed = 2 + 2 * tier if qi == 1 else 0
        if move_speed:
            description += f"装备时借风行步，历练移动速度提高 {move_speed}%。"
        rows.append({
            "id": f"sys_{polarity}_{element}_{qi}", "name": label, "description": description,
            "polarity": polarity, "element": element, "qi": qi, "kind": kind, "rank": rank,
            "learningLevel": 1 + 10 * (tier - 1), "baseCircle": base_circle,
            "effect1": effect1, "effectValue1": 5 + rank, "effect2": effect2, "effectValue2": 2 + rank // 2,
            **({"moveSpeedPercent": move_speed} if move_speed else {}),
        })
    validate(rows)
    return {"methods": rows}


def validate(rows):
    expected = set(product(POLARITIES, ELEMENTS, QI_TYPES))
    triples = {(r["polarity"], r["element"], r["qi"]) for r in rows}
    if len(rows) != len(expected) or triples != expected:
        raise ValueError("阴阳、五行、十二气的 120 种组合必须各有一门功法")
    if len({r["name"] for r in rows}) != len(rows) or any(not re.fullmatch(r"[\u3400-\u9fff]{4,7}", r["name"]) for r in rows):
        raise ValueError("功法名须为四至七个汉字且不能重名")
    if Counter(r["rank"] for r in rows) != {rank: 10 for rank in RANKS}:
        raise ValueError("十二种常规品阶须各有十门")
    if Counter(r["kind"] for r in rows) != {kind: 40 for kind in KINDS}:
        raise ValueError("三条修途须各有四十门")
    learners = Counter((r["kind"], r["element"]) for r in rows if r["learningLevel"] == 1)
    if learners != {pair: 2 for pair in product(KINDS, ELEMENTS)}:
        raise ValueError("黄阶须覆盖全部修途与五行，每组两门")


def as_json(value):
    return json.dumps(value, ensure_ascii=False, indent=2) + "\n"


def output_file(path, body, check):
    if path.is_file() and path.read_bytes() == body.encode("utf-8"):
        return
    if check:
        raise ValueError(f"文件缺失或需要重新生成：{path}")
    path.parent.mkdir(parents=True, exist_ok=True)
    with NamedTemporaryFile("w", dir=path.parent, encoding="utf-8", delete=False) as temp:
        temp.write(body)
    staging = Path(temp.name)
    try:
        staging.chmod(path.stat().st_mode & 0o777 if path.exists() else 0o644)
        staging.replace(path)
    finally:
        staging.unlink(missing_ok=True)


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--config-dir", type=Path, default=CONFIG_DIR, help="阴阳五行属性表及系统功法输出目录")
    parser.add_argument("--web-dir", type=Path, help="同时生成或检查独立设计画板的数据镜像")
    parser.add_argument("--unity-project", type=Path, help="同时生成或检查该工程的中文功法目录")
    parser.add_argument("--check", action="store_true", help="只核对文件与当前生成结果")
    args = parser.parse_args()
    try:
        config = json.loads((args.config_dir / "cultivation_affinities.json").read_text(encoding="utf-8"))
        locale = json.loads((args.config_dir / "cultivation_affinities.zh-CN.json").read_text(encoding="utf-8"))
        catalog = make_catalog(config, locale)
        output_file(args.config_dir / "system_cultivation_methods.json", as_json(catalog), args.check)
        web = args.web_dir
        if web is None and args.config_dir.resolve() == CONFIG_DIR:
            candidate = Path(__file__).resolve().parents[1] / "design/修真界UI"
            if candidate.is_dir():
                web = candidate
        if web:
            web_fields = UNITY_FIELDS + ("polarity", "element", "qi", "rank", "learningLevel", "baseCircle")
            book = {"methods": [{key: row[key] for key in web_fields} for row in catalog["methods"]]}
            for field, table in (("polarity", "polarities"), ("element", "elements"), ("qi", "qiTypes"), ("rank", "grades")):
                book[field] = {str(code): locale[table][str(code)]["name"] for code in sorted({r[field] for r in catalog["methods"]})}
            script = "// Generated by tools/generate_system_methods.py for offline boards.\nwindow.systemMethodCatalog = " + as_json(book).rstrip() + ";\n"
            output_file(web / "source/system-methods.js", script, args.check)
        if args.unity_project:
            target = args.unity_project / "Assets/Resources/InkCultivation/SystemMethods.json"
            projected = [{k: row[k] for k in UNITY_FIELDS} for row in catalog["methods"]]
            output_file(target, as_json({"methods": projected}), args.check)
            # Unity may have generated the .meta already. Retain its assigned asset GUID.
            meta = target.with_suffix(".json.meta")
            if not meta.exists():
                output_file(meta, UNITY_META, args.check)
            elif not re.search(r"^guid: [0-9a-f]{32}$", meta.read_text(encoding="utf-8"), re.M):
                raise ValueError(f"Unity 资源 GUID 无效：{meta}")
    except (ValueError, KeyError, OSError) as exc:
        parser.exit(1, f"{exc}\n")
    print("120 门功法已" + ("核对" if args.check else "生成"))


if __name__ == "__main__":
    main()
