"""按丹药或丹方查材料产地，并估算单炉成丹率、出丹数量和药效。"""

import argparse
import json
from pathlib import Path

if __package__:
    from .crafting_tables import MODIFIER_FIELDS
    from .crafting_preview import (
        craft_outcome, crafting_inputs, find_recipe, item_element, method_affinity, print_sources, trunc_ratio,
        yield_distribution,
    )
    from .preview_region import CONFIG, element_relation, catalog_region, preview_region, read_json
else:
    from crafting_tables import MODIFIER_FIELDS
    from crafting_preview import (
        craft_outcome, crafting_inputs, find_recipe, item_element, method_affinity, print_sources, trunc_ratio,
        yield_distribution,
    )
    from preview_region import CONFIG, element_relation, catalog_region, preview_region, read_json


def recipe_lookup(catalog, locale, query):
    return find_recipe(catalog, locale, query, "alchemy", "丹方")


def ingredient_guide(catalog, locale, recipe):
    rows = crafting_inputs(catalog, locale, recipe["outputId"])
    for row in rows:
        row["primary"] = row["itemId"] == recipe["primaryId"]
    return sorted(rows, key=lambda row: not row["primary"])


def element_modifiers(catalog, recipe, element):
    """丹火或功法对主药、辅材的五行贡献。负数和正数都向零取整。"""
    alchemy = catalog["alchemy"]
    main = recipe["primaryId"]
    primary_element = item_element(catalog, main)

    def relation(target):
        return element_relation(catalog["elements"], element, target) if target else "neutral"

    main_relation = relation(primary_element)
    main_values = alchemy["relations"][main_relation]
    other = [(alchemy["relations"][relation(item_element(catalog, item))], qty)
             for item, qty in catalog["creationInputs"][recipe["outputId"]].items() if item != main]
    units = sum(qty for _, qty in other)
    if not units:
        return main_relation, main_values
    main_weight = alchemy["limits"]["primaryWeightBps"]
    totals = {
        field: trunc_ratio(main_values[field] * main_weight * units +
                           sum(values[field] * qty for values, qty in other) * (10000 - main_weight), units * 10000)
        for field in MODIFIER_FIELDS
    }
    return main_relation, totals


def brew_plan(catalog, locale, recipe, rank, flame, method=None, region=None, roles=None, period=0):
    alchemy = catalog["alchemy"]
    if str(rank) not in alchemy["ranks"]:
        raise ValueError("丹师品阶编号须在 0～9")
    fire_values = alchemy["flames"].get(flame)
    if fire_values is None:
        raise ValueError(f"未登记的丹火 {flame}")
    minimum = max(recipe["minRank"], fire_values["minRank"])
    if rank < minimum:
        raise ValueError(f"本丹方和丹火需要至少 {locale['alchemy']['ranks'][str(minimum)]['name']}")

    def contribution(source_id, values, **info):
        return {"sourceId": source_id, **{field: values.get(field, 0) for field in MODIFIER_FIELDS}, **info}

    flame_element = catalog["forgeFactors"]["flames"][flame]["elementId"]
    fire_relation, fire_affinity = element_modifiers(catalog, recipe, flame_element)
    parts = [contribution("rank", alchemy["ranks"][str(rank)]),
             contribution("flame", fire_values),
             contribution("flameElement", fire_affinity, relationId=fire_relation, elementId=flame_element)]
    if method:
        method_element, numerator, denominator = method_affinity(catalog, "alchemy", method)
        relation, values = element_modifiers(catalog, recipe, method_element)
        parts.append(contribution("methodElement",
                                  {key: trunc_ratio(value * numerator, denominator) for key, value in values.items()},
                                  relationId=relation, elementId=method_element))
    primary_element = item_element(catalog, recipe["primaryId"])
    if region is not None:
        county = catalog_region(catalog, region)
        if catalog["regions"].get(county, {}).get("level") != 3:
            raise ValueError("炉址须为已登记的郡或其下级地图")
        local = preview_region(catalog, locale, county, roles, period)
        for effect in local["effects"]:
            if effect["effectKey"] == "brew" and effect["targetElementId"] == primary_element:
                parts.append(contribution("place", {"successBps": effect["totalBps"]}, regionId=effect["sourceRegionId"]))
    return {"primaryElementId": primary_element, **craft_outcome(catalog, alchemy, recipe, parts, recipe["yieldCap"])}


def print_plan(plan, recipe, locale):
    effect = locale["alchemy"]["effects"][recipe["effectId"]]
    atoms = plan["yieldTotalTickets"]
    distribution = "、".join(f"{qty}粒 {count * 100 / atoms:.2f}%" for qty, count in plan["yieldTickets"].items())
    low, high = plan["effectRange"]
    print(f"成丹率 {plan['successBps'] / 100:.2f}%")
    print(f"成功后出丹　{distribution}")
    print(f"单粒{effect['name']}　{low}～{high} {effect['unit']}")
    source_names = {"rank": "丹师", "flame": "丹火", "flameElement": "丹火五行",
                    "methodElement": "功法五行", "place": "炉址地利"}
    for part in plan["adjustments"]:
        label = source_names[part["sourceId"]]
        if "relationId" in part:
            element = locale["elements"][str(part["elementId"])]["name"]
            relation = locale["alchemy"]["relations"][part["relationId"]]["name"]
            label += f"（{element}，对主药{relation}）"
        if "regionId" in part:
            label += f"（{locale['regions'][str(part['regionId'])]['name']}）"
        print(f"  {label}　成丹 {part['successBps'] / 100:+.2f} 个百分点，"
              f"出丹 {part['yieldBps'] / 100:+.2f}%，药效倍率 {part['potencyBps'] / 100:+.2f} 个百分点")


def main(argv=None):
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--pill", required=True, help="丹名、丹药模板键或丹方键")
    parser.add_argument("--rank", type=int, help="丹师品阶 0～9，与 --flame 一起用于估算")
    parser.add_argument("--flame", help="丹火键，例如 flame.ember")
    parser.add_argument("--method", help="已装备功法的数字模板 ID 或兼容旧键，如 73017、gf_3_3")
    parser.add_argument("--region", help="开炉所在郡或其下级地图 ID")
    parser.add_argument("--roles", type=Path, help="计算炉址共鸣时可选的只读角色快照")
    parser.add_argument("--period", type=int, default=0, help="地区词条的周期编号")
    parser.add_argument("--config", type=Path, default=CONFIG)
    parser.add_argument("--locale", type=Path)
    parser.add_argument("--json", action="store_true", help="输出结构化数据")
    args = parser.parse_args(argv)
    if args.roles and not args.region:
        parser.error("--roles 需要指定炉址 --region")
    wants_plan = args.rank is not None or bool(args.flame or args.method or args.region)
    if wants_plan and (args.rank is None or not args.flame):
        parser.error("估算同一炉需要同时指定 --rank 和 --flame")
    try:
        config = read_json(args.config)
        locale = read_json(args.locale or args.config.with_name(args.config.stem + ".zh-CN.json"))
        key, recipe = recipe_lookup(config, locale, args.pill.strip())
        materials = ingredient_guide(config, locale, recipe)
        plan = None
        if wants_plan:
            roles = read_json(args.roles, legacy=True) if args.roles else None
            plan = brew_plan(config, locale, recipe, args.rank, args.flame, args.method, args.region, roles, args.period)
    except (OSError, KeyError, ValueError) as error:
        parser.exit(1, f"{error}\n")
    name = locale["itemTemplates"][recipe["outputId"]]["name"]
    if args.json:
        print(json.dumps({"recipeId": key, "name": name, **recipe, "ingredients": materials, "plan": plan}, ensure_ascii=False, indent=2))
        return
    print(f"{name}　{key}")
    for material in materials:
        role = "主药" if material["primary"] else "辅材"
        print(f"  {role}　{material['name']} ×{material['qty']}　{material['purpose']}")
        print_sources(locale, material["sources"], "    ")
    if plan is not None:
        print_plan(plan, recipe, locale)


if __name__ == "__main__":
    main()
