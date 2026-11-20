"""按一份角色位置快照，预览地区词条及其修士修正。"""

import argparse
from collections import Counter
import hashlib
import json
import math
from pathlib import Path
import re


CONFIG = Path(__file__).resolve().parents[1] / "server/data/config/cultivation_affinities.json"


def read_json(path, legacy=False):
    data = path.read_bytes()
    try:
        text = data.decode("utf-8-sig")
    except UnicodeDecodeError:
        if not legacy:
            raise
        text = data.decode("gbk")
    return json.loads(text)


def snapshot_roles(snapshot):
    """支持 Go 的角色键控对象，以及用于审表的精简角色数组。"""
    if isinstance(snapshot, dict):
        entries = snapshot.items()
        seen = None
    elif isinstance(snapshot, list):
        seen = set()
        entries = ((role.get("Id") if isinstance(role, dict) else None, role) for role in snapshot)
    else:
        raise ValueError("角色快照须为角色键控对象，或含 Id 的角色数组")

    for role_id, role in entries:
        if not isinstance(role_id, str) or not role_id:
            raise ValueError("快照中的每位修士须有角色 ID")
        if role_id.startswith("$") or role is None:
            continue  # Go also permits system records and empty legacy records.
        if not isinstance(role, dict):
            raise ValueError("角色记录须为对象")
        if role.get("Id", role_id) != role_id:
            raise ValueError("角色 ID 与快照键不一致")
        if seen is not None:
            if role_id in seen:
                raise ValueError("角色 ID 重复")
            seen.add(role_id)
        yield role


def points(bps, unit):
    return f"{bps / 100:+.2f}{unit}"


def element_relation(elements, source, target):
    if source == target:
        return "same"
    origin = elements[str(source)]
    if origin["generates"] == target:
        return "generates"
    if origin["overcomes"] == target:
        return "overcomes"
    if elements[str(target)]["generates"] == source:
        return "generatedBy"
    if elements[str(target)]["overcomes"] == source:
        return "overcomeBy"
    return "neutral"


def role_element(role):
    # buildGameMethods assigns the equipped method's element by rank offset.
    match = re.fullmatch(r"gf_3_([1-9]|1[0-6])", str(role.get("GongFaNaQi", "")))
    if match:
        return (int(match[1]) - 1) % 5 + 1
    strongest, chosen = 0, 0
    for element, key in enumerate(("Gold", "Wood", "Water", "Fire", "Earth"), 1):
        try:
            value = float(role.get(key, 0))
        except (TypeError, ValueError, OverflowError):
            continue
        if math.isfinite(value) and value > strongest:
            strongest, chosen = value, element
    return chosen


def region_path(regions, region):
    path = []
    while region != "0":
        if region in path or region not in regions:
            raise ValueError("地域父级引用缺失或成环")
        path.append(region)
        region = str(regions[region]["parentId"])
    return path[::-1]


def map_region(regions, map_id):
    """只按现有地图中的有效市镇、设施和林地区号定位郡。"""
    key = str(map_id)
    if key in regions:
        return key
    # See newGeography, gameForest and gameForestArea in game_map.go.
    forest = re.fullmatch(r"([0-9]+)W[1-6][1-3]?", key)
    if forest and regions.get(forest[1], {}).get("level") == 3:
        return forest[1]
    for length, pattern in ((2, r"(0[1-9]|1[0-2])"), (4, r"(0[1-9]|1[0-2])0[1-6]")):
        county = key[:-length]
        if regions.get(county, {}).get("level") == 3 and re.fullmatch(pattern, key[-length:]):
            return county
    return None


def period_offset(period, region, effect, target, bound):
    # Include the resolved region so neighbouring districts may differ within the same period.
    digest = hashlib.sha256(f"{period}|{region}|{effect}|{target}".encode("utf-8")).digest()
    return int.from_bytes(digest[:8], "big") % (bound * 2 + 1) - bound


def preview_region(config, locale, region_id, roles=None, period=0):
    if not config.get("catalogVersion") or config["catalogVersion"] != locale.get("catalogVersion"):
        raise ValueError("运行配置与中文资源版本不一致")
    if type(period) is not int or period < 0:
        raise ValueError("周期编号须为非负整数")
    regions = config["regions"]
    region = map_region(regions, region_id)
    if region is None:
        raise ValueError(f"未登记的地域或地图 ID {region_id}")
    path = region_path(regions, region)
    within = {None: False}
    counts = Counter()
    for role in snapshot_roles(roles if roles is not None else {}):
        place = map_region(regions, role.get("MapId"))
        if place not in within:
            within[place] = regions[place]["level"] == 3 and region in region_path(regions, place)
        if within[place]:
            counts[role_element(role)] += 1

    # The nearest ancestor defining the same effect AND target takes precedence.
    inherited = {}
    for source in path:
        for effect, targets in config["regionModifiers"].get(source, {}).items():
            for target, rule in targets.items():
                inherited[effect, int(target)] = (source, rule)

    effects = []
    resonance = config["regionResonance"]
    for (effect, target), (source, rule) in sorted(inherited.items()):
        total = 0
        for element, count in counts.items():
            if element:
                relation = element_relation(config["elements"], element, rule["resonanceElementId"])
                total += count * resonance[relation + "Bps"]
        residents = max(-resonance["capBps"], min(resonance["capBps"], total))
        drift = period_offset(period, region, effect, target, rule["jitterBps"])
        definition = config["regionEffects"][effect]
        entry = {
            "effectKey": effect, "targetKind": definition["targetKind"], "modeId": definition["modeId"],
            "targetElementId": target, "sourceRegionId": int(source),
            "label": locale["regionModifiers"][source][effect][str(target)]["label"],
            "baseBps": rule["baseBps"], "periodBps": drift, "residentBps": residents,
            "totalBps": rule["baseBps"] + drift + residents,
        }
        example = rule.get("exampleTemplateId")
        if example:
            entry["exampleTemplateId"] = example
            entry["exampleName"] = locale["itemTemplates"][example]["name"]
        effects.append(entry)
    geography = regions[region]
    return {
        "polarityId": geography["polarityId"], "elementId": geography["elementId"], "qiId": geography["qiId"],
        "regionId": int(region), "path": [locale["regions"][key]["name"] for key in path],
        "period": period, "residentCount": sum(counts.values()),
        "residentsByElement": {str(key): counts[key] for key in sorted(counts) if key},
        "resonatingCount": sum(counts.values()) - counts[0], "effects": effects,
    }


def demo_snapshot(config, region_id):
    regions = config["regions"]
    region = map_region(regions, region_id)
    if region is None:
        raise ValueError(f"未登记的地域或地图 ID {region_id}")
    # For a province preview, place the demo residents in a real county below it.
    counties = sorted(key for key, node in regions.items() if node["level"] == 3 and region in region_path(regions, key))
    if not counties:
        return {}
    region = counties[0]
    element = regions[region]["elementId"] or 1
    return {
        "demo-a": {"MapId": region, "GongFaNaQi": f"gf_3_{element}"},
        "demo-b": {"MapId": region, "GongFaNaQi": f"gf_3_{element}"},
        "demo-c": {"MapId": region, "Water": 8},
    }


def render(result, locale):
    names = []
    for table, field in (("polarities", "polarityId"), ("elements", "elementId"), ("qiTypes", "qiId")):
        if result[field]:
            names.append(locale[table][str(result[field])]["name"])
    print(" / ".join(result["path"]) + ("　" + "·".join(names) if names else ""))
    distribution = "、".join(f"{locale['elements'][key]['name']}{count}人"
                            for key, count in result["residentsByElement"].items())
    if distribution:
        distribution = f"（{distribution}）"
    print(f"周期 {result['period']}　辖内修士 {result['residentCount']} 人，"
          f"参与共鸣 {result['resonatingCount']} 人{distribution}")
    for entry in result["effects"]:
        action = locale["regionEffects"][entry["effectKey"]]["name"]
        source = locale["regions"][str(entry["sourceRegionId"])]["name"]
        example = f"（如{entry['exampleName']}）" if "exampleName" in entry else ""
        unit = " 个百分点" if entry["modeId"] == 2 else "%"
        print(f"  {action} · {entry['label']}{example}　{points(entry['totalBps'], unit)}"
              f"（{source}基础 {points(entry['baseBps'], unit)}，周期 {points(entry['periodBps'], unit)}，"
              f"修士 {points(entry['residentBps'], unit)}）")


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--region", required=True, help="大陆、洲、郡或已有城镇/设施/妖兽林地地图 ID")
    parser.add_argument("--period", type=int, default=0, help="周期编号，默认 0；同一期内浮动固定")
    parser.add_argument("--config", type=Path, default=CONFIG)
    parser.add_argument("--locale", type=Path, help="默认使用配置旁的同版本 .zh-CN.json")
    inputs = parser.add_mutually_exclusive_group()
    inputs.add_argument("--roles", type=Path, help="只读角色快照，支持服务端键控 JSON 或含 Id 的角色数组")
    inputs.add_argument("--demo", action="store_true", help="使用本地三名虚拟修士比较词条")
    parser.add_argument("--ecology", action="store_true", help="附加本郡的怪物掉落、采集资源与用途")
    parser.add_argument("--tier", type=int, choices=(1, 3, 4), default=1, help="生态选档，1 普通、3 精英、4 兽王")
    parser.add_argument("--json", action="store_true", help="输出带有整数基点的结构化预览")
    args = parser.parse_args()
    try:
        config = read_json(args.config)
        locale = read_json(args.locale or args.config.with_name(args.config.stem + ".zh-CN.json"))
        roles = read_json(args.roles, legacy=True) if args.roles else {}
        if args.demo:
            roles = demo_snapshot(config, args.region)
        result = preview_region(config, locale, args.region, roles, args.period)
        if args.ecology:
            if __package__:
                from .region_ecology import format_ecology, region_ecology_summary
            else:
                from region_ecology import format_ecology, region_ecology_summary
            if config["regions"][str(result["regionId"])]["level"] != 3:
                raise ValueError("生态预览请选择郡或郡下地图 ID")
            result["ecology"] = region_ecology_summary(config, locale, result["regionId"], args.tier)
        if args.json:
            print(json.dumps(result, ensure_ascii=False, indent=2))
        else:
            render(result, locale)
            if args.ecology:
                print(format_ecology(result["ecology"]))
    except (OSError, UnicodeError, ValueError, KeyError) as error:
        parser.exit(1, f"预览失败：{error}\n")


if __name__ == "__main__":
    main()
