"""地域阴阳、主行、主气如何影响同一候选池中的相对丰度。权重与击杀掉率分别计算。"""

from collections import defaultdict
import re

SHEETS = {
    "生态亲和": ("ecologyAffinity", "来源域 同阴阳基点 同行基点 地生物基点 地克物基点 同气基点 偏移上限基点 说明",
             "domain polarityBps elementBps generatesBps overcomesBps qiBps capBps note"),
}


def compile_affinity(sheets):
    rules, labels = {}, {}
    for location, row in sheets["生态亲和"]:
        domain = row["domain"]
        if domain not in ("monster", "resource") or domain in rules:
            raise ValueError(f"{location} 来源域须分别登记 monster 和 resource")
        values = {}
        for key in ("polarityBps", "elementBps", "generatesBps", "overcomesBps", "qiBps", "capBps"):
            if not re.fullmatch(r"-?[0-9]+(?:\.0+)?", str(row[key])):
                raise ValueError(f"{location} {key} 须为整数基点")
            values[key] = int(str(row[key]).split(".")[0])
        if not 0 <= values["capBps"] < 10000 or any(abs(n) > 10000 for n in values.values()):
            raise ValueError(f"{location} 单项应在 ±10000 基点内，偏移上限须在 0～9999 之间")
        rules[domain], labels[domain] = values, {"note": row["note"]}
    if set(rules) != {"monster", "resource"}:
        raise ValueError("生态亲和缺少怪物或物产的权重规则")
    return dict(sorted(rules.items())), dict(sorted(labels.items()))


def habitat_weight(base, region, profile, elements, rule):
    """保留整数票数，基础权重为 1 的稀有候选也能体现微小的地域差异。"""
    delta = 0
    if profile["polarityId"] and profile["polarityId"] == region["polarityId"]:
        delta += rule["polarityBps"]
    if profile["primaryQiId"] and profile["primaryQiId"] == region["qiId"]:
        delta += rule["qiBps"]
    env, species = region["elementId"], profile["primaryElementId"]
    cycle = elements[str(env)]
    if env == species:
        delta += rule["elementBps"]
    elif cycle["generates"] == species:
        delta += rule["generatesBps"]
    elif cycle["overcomes"] == species:
        delta += rule["overcomesBps"]
    delta = max(-rule["capBps"], min(delta, rule["capBps"]))
    return base * (10000 + delta), delta


def source_indexes(catalog):
    """反向索引保留原关系的键。概率、数量和权重仅在各自细表保存一次。"""
    sources = defaultdict(dict)
    habitats = defaultdict(list)
    for region, monsters in catalog["regionMonsters"].items():
        for monster in monsters:
            habitats[monster].append(int(region))
    for region, methods in catalog["regionResources"].items():
        for method, items in methods.items():
            for item in items:
                sources[item].setdefault("gather", []).append([int(region), int(method)])
    for monster, scopes in catalog["monsterDrops"].items():
        for region, items in scopes.items():
            for item in items:
                sources[item].setdefault("drop", []).append([int(monster), int(region)])
    for item_type, item in catalog["legacyMaterialTypes"].items():
        sources[item].setdefault("legacyTypes", []).append(int(item_type))
    for groups in sources.values():
        for pairs in groups.values():
            pairs.sort()
    systems = defaultdict(list)
    for item, uses in catalog["resourceUses"].items():
        for system in uses:
            systems[system].append(item)
    return {"resourceSources": dict(sorted(sources.items())),
            "monsterHabitats": {key: sorted(values) for key, values in sorted(habitats.items())},
            "systemResources": {key: sorted(values) for key, values in sorted(systems.items())}}


def region_ecology_summary(config, locale, region_id, tier=1):
    """地区怪物、逐种掉率、分采法物产。返回中文展示所需的数据及明确的概率分母。"""
    rid, tier = str(region_id), int(tier)
    region = config["regions"][rid]
    if region["level"] != 3 or tier not in (1, 3, 4):
        raise ValueError("请选择一个地区和普通(1)、精英(3)或兽王(4)档")

    def weight_info(base, profile_id, domain):
        tickets, delta = habitat_weight(base, region, config["profiles"][profile_id],
                                        config["elements"], config["ecologyAffinity"][domain])
        return {"baseWeight": base, "habitatBps": delta, "tickets": tickets}

    def item_info(item):
        uses = config["resourceUses"].get(item, ())
        return {"itemId": item, "name": locale["itemTemplates"][item]["name"],
                "uses": [locale["systems"][key]["name"] + "·" + locale["resourceUses"][item][key]["name"]
                         for key in uses]}

    monsters = []
    for monster, base in sorted(config["regionMonsters"].get(rid, {}).items()):
        definition = config["monsterSpecies"][monster]
        if tier == 4 and not definition["allowBoss"]:
            continue
        result = weight_info(base, definition["profileId"], "monster")
        # 同一个怪、同一材料只有一次机会。指定郡覆盖通用行，其余通用材料仍保留。
        scopes = config["monsterDrops"].get(monster, {})
        items = {**scopes.get("0", {}), **scopes.get(rid, {})}
        drops = []
        for item, entry in sorted(items.items()):
            chance = entry["chanceBps"][str(tier)]
            if chance:
                drops.append({**item_info(item), "chanceBps": chance,
                              "minQty": entry["minQty"], "maxQty": entry["maxQty"]})
        monsters.append({**result, "monsterId": int(monster), "name": locale["monsterSpecies"][monster]["name"],
                         "drops": drops})

    gathering = []
    for method, items in sorted(config["regionResources"].get(rid, {}).items()):
        nodes = []
        for item, entry in sorted(items.items()):
            weights = weight_info(entry["weight"], config["itemTemplates"][item]["profileId"], "resource")
            nodes.append({**weights, **item_info(item), "minQty": entry["minQty"], "maxQty": entry["maxQty"]})
        gathering.append({"methodId": int(method), "name": locale["gatherMethods"][method]["name"],
                          "totalTickets": sum(n["tickets"] for n in nodes), "items": nodes})
    return {"regionId": int(rid), "tier": tier, "monsterTickets": sum(m["tickets"] for m in monsters),
            "monsters": monsters, "gathering": gathering}


def format_ecology(summary):
    tier_name = {1: "普通", 3: "精英", 4: "兽王"}[summary["tier"]]
    lines = [f"{tier_name}怪物（本郡同档相遇占比；材料另按击败一只的概率判定）"]
    for monster in summary["monsters"]:
        percent = monster["tickets"] * 100 / summary["monsterTickets"]
        lines.append(f"  {monster['name']}　{percent:.2f}%")
        for item in monster["drops"]:
            lines.append(f"    {item['name']}　{item['chanceBps']/100:.2f}%　"
                         f"{item['minQty']}～{item['maxQty']} 份　{'、'.join(item['uses'])}")
    for group in summary["gathering"]:
        lines.append(f"{group['name']}（同郡、同采法的资源点占比）")
        for item in group["items"]:
            percent = item["tickets"] * 100 / group["totalTickets"]
            lines.append(f"  {item['name']}　{percent:.2f}%　{item['minQty']}～{item['maxQty']} 份　"
                         f"{'、'.join(item['uses'])}")
    return "\n".join(lines)
