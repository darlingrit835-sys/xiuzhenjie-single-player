"""炼丹与画符共用的寻材、功法与批次核算。"""

from collections import defaultdict
from functools import lru_cache
import re

if __package__:
    from .crafting_tables import MODIFIER_FIELDS
    from .preview_region import role_element
else:
    from crafting_tables import MODIFIER_FIELDS
    from preview_region import role_element


def find_recipe(catalog, locale, query, system, label):
    if not catalog.get("catalogVersion") or catalog["catalogVersion"] != locale.get("catalogVersion"):
        raise ValueError("运行配置与中文资源版本不一致")
    rules = catalog.get(system)
    if rules is None:
        raise ValueError(f"配置尚未包含{label}规则")
    recipes, by_output = rules["recipes"], rules["recipeByOutput"]
    if query in recipes:
        return query, recipes[query]
    if query in by_output:
        key = by_output[query]
        return key, recipes[key]
    matches = [key for item, key in by_output.items() if locale["itemTemplates"][item]["name"] == query]
    if len(matches) == 1:
        return matches[0], recipes[matches[0]]
    if matches:
        raise ValueError(f"该名称对应多个{label}，请使用{label}键：{'、'.join(matches)}")
    raise ValueError(f"未登记「{query}」的基础{label}")


def crafting_sources(catalog, locale):
    item_names = locale["itemTemplates"]
    quantities = catalog["creationInputs"]

    @lru_cache(maxsize=None)
    def routes(item):
        source = catalog["resourceSources"].get(item, {})
        gathered = []
        for region, method in source.get("gather", ()):
            node = catalog["regionResources"][str(region)][str(method)][item]
            gathered.append({"regionId": region, "methodId": method,
                             "minQty": node["minQty"], "maxQty": node["maxQty"]})

        places = set()
        for monster, scope in source.get("drop", ()):
            counties = [scope] if scope else catalog["monsterHabitats"][str(monster)]
            places.update((monster, county) for county in counties)

        drops = []
        for monster, county in sorted(places):
            scopes = catalog["monsterDrops"][str(monster)]
            entry = scopes.get(str(county), {}).get(item) or scopes.get("0", {})[item]
            drops.append({"monsterId": monster, "regionId": county, **entry})

        components = [{"itemId": key, "name": item_names[key]["name"], "qty": amount, "sources": routes(key)}
                      for key, amount in sorted(quantities.get(item, {}).items())]
        return {"gather": gathered, "drop": drops, "legacyTypes": source.get("legacyTypes", []),
                "components": components}

    return routes


def crafting_inputs(catalog, locale, product):
    routes = crafting_sources(catalog, locale)
    item_names = locale["itemTemplates"]
    amounts = catalog["creationInputs"][product]
    notes = locale["creationInputs"][product]
    return [{"itemId": key, "name": item_names[key]["name"], "qty": amount,
             "purpose": notes[key]["purpose"], "sources": routes(key)}
            for key, amount in sorted(amounts.items())]


def item_element(catalog, item):
    return catalog["profiles"][catalog["itemTemplates"][item]["profileId"]]["primaryElementId"]


def trunc_ratio(numerator, denominator):
    return numerator // denominator if numerator >= 0 else -(-numerator // denominator)


def method_affinity(catalog, system, method):
    equipped = re.fullmatch(r"gf_3_([1-9]|1[0-6])", method)
    if equipped is None:
        raise ValueError("纳气功法须使用角色已装备功法的编号 gf_3_1～gf_3_16")
    numerator, denominator = catalog["methodInfluences"][system]["3"][equipped[1]]
    return role_element({"GongFaNaQi": method}), numerator, denominator


def yield_distribution(minimum, maximum, cap, multiplier):
    """成功后各整数产量的票数。基础数量等概率，余数是多出一件的概率。"""
    tickets = defaultdict(int)
    for base in range(minimum, maximum + 1):
        whole, remainder = divmod(base * multiplier, 10000)
        tickets[max(1, min(cap, whole))] += 10000 - remainder
        if remainder:
            tickets[max(1, min(cap, whole + 1))] += remainder
    return dict(sorted(tickets.items()))


def craft_outcome(catalog, rules, recipe, parts, yield_cap):
    total = {field: sum(part[field] for part in parts) for field in MODIFIER_FIELDS}
    limits = rules["limits"]
    success = max(limits["successMinBps"], min(limits["successMaxBps"], recipe["successBps"] + total["successBps"]))
    multiplier = max(limits["yieldMultiplierMinBps"],
                     min(limits["yieldMultiplierMaxBps"], 10000 + total["yieldBps"]))
    distribution = yield_distribution(recipe["minQty"], recipe["maxQty"], yield_cap, multiplier)

    grade = catalog["grades"][str(catalog["itemTemplates"][recipe["outputId"]]["gradeId"])]
    low, high = rules["potencyByQuality"][str(grade["qualityId"])]
    bounds = [max(low, min(high, edge + total["potencyBps"])) for edge in (low, high)]
    return {"adjustments": parts, "successBps": success,
            "yieldMultiplierBps": multiplier, "yieldTickets": distribution, "yieldTotalTickets": sum(distribution.values()),
            "potencyBps": bounds, "effectRange": [(recipe["baseEffect"] * value + 5000) // 10000 for value in bounds]}


def print_sources(locale, source, indent):
    regions = locale["regions"]
    for items, dictionary, identifier in (
        (source["gather"], "gatherMethods", "methodId"),
        (source["drop"], "monsterSpecies", "monsterId"),
    ):
        grouped = defaultdict(list)
        for route in items:
            grouped[route[identifier]].append(regions[str(route["regionId"])]["name"])
        for key, counties in grouped.items():
            label = locale[dictionary][str(key)]["name"]
            print(f"{indent}{label}　{'、'.join(counties)}")
    for type_id in source["legacyTypes"]:
        print(f"{indent}旧通用掉落　{locale['legacyMaterialTypes'][str(type_id)]['name']}")
    for part in source["components"]:
        print(f"{indent}每份加工用料　{part['name']} ×{part['qty']}")
        print_sources(locale, part["sources"], indent + "  ")
