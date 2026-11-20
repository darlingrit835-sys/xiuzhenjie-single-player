"""查符方、符材与符笔产地，并离线核算一批符箓。"""

import argparse
import json
from pathlib import Path

if __package__:
    from .crafting_preview import craft_outcome, crafting_inputs, crafting_sources, find_recipe, item_element, method_affinity, print_sources, trunc_ratio
    from .crafting_tables import MODIFIER_FIELDS
    from .preview_region import CONFIG, element_relation, map_region, preview_region, read_json
else:
    from crafting_preview import craft_outcome, crafting_inputs, crafting_sources, find_recipe, item_element, method_affinity, print_sources, trunc_ratio
    from crafting_tables import MODIFIER_FIELDS
    from preview_region import CONFIG, element_relation, map_region, preview_region, read_json


def material_guide(catalog, locale, recipe):
    items = crafting_inputs(catalog, locale, recipe["outputId"])
    by_role = {recipe["paperId"]: (0, "paper"), recipe["inkId"]: (1, "ink")}
    for row in items:
        row["role"] = by_role.get(row["itemId"], (2, "auxiliary"))[1]
    return sorted(items, key=lambda row: by_role.get(row["itemId"], (2,))[0])


def draw_plan(catalog, locale, recipe, rank, brush, method=None, region=None, roles=None, period=0):
    rules = catalog["inscription"]
    if str(rank) not in rules["ranks"]:
        raise ValueError("符师品阶编号须在 0～9")
    brush_values = rules["brushes"].get(brush)
    if brush_values is None:
        raise ValueError(f"未登记的符笔 {brush}")
    minimum = max(recipe["minRank"], brush_values["minRank"])
    if rank < minimum:
        raise ValueError(f"本符方与符笔需要至少 {locale['inscription']['ranks'][str(minimum)]['name']}")

    pattern = item_element(catalog, recipe["outputId"])
    ink = item_element(catalog, recipe["inkId"])
    paper = item_element(catalog, recipe["paperId"])
    limits = rules["limits"]
    pattern_weight, ink_weight = limits["patternWeightBps"], limits["inkWeightBps"]
    weights = (pattern_weight, ink_weight, 10000 - pattern_weight - ink_weight)

    def contribution(source, values, **detail):
        return {"sourceId": source, **{field: values.get(field, 0) for field in MODIFIER_FIELDS}, **detail}

    def affinity(source, element, numerator=1, denominator=1):
        relations = [element_relation(catalog["elements"], element, target) for target in (pattern, ink, paper)]
        values = {
            field: trunc_ratio(sum(rules["relations"][relation][field] * weight for relation, weight
                                   in zip(relations, weights)) * numerator, denominator * 10000)
            for field in MODIFIER_FIELDS
        }
        return contribution(source, values, elementId=element,
                            relations=dict(zip(("pattern", "ink", "paper"), relations)))

    parts = [contribution("rank", rules["ranks"][str(rank)]), contribution("brush", brush_values),
             affinity("brushElement", item_element(catalog, brush))]
    if method:
        element, numerator, denominator = method_affinity(catalog, "inscription", method)
        parts.append(affinity("methodElement", element, numerator, denominator))
    if region is not None:
        county = map_region(catalog["regions"], region)
        if catalog["regions"].get(county, {}).get("level") != 3:
            raise ValueError("画符地点须为已登记的郡或其下级地图")
        local = preview_region(catalog, locale, county, roles, period)
        for effect in local["effects"]:
            if effect["effectKey"] == "inscribe" and effect["targetElementId"] == ink:
                parts.append(contribution("place", {"successBps": effect["totalBps"]}, regionId=effect["sourceRegionId"]))
    paper_count = catalog["creationInputs"][recipe["outputId"]][recipe["paperId"]]
    return {"patternElementId": pattern, "inkElementId": ink, "paperCap": paper_count,
            **craft_outcome(catalog, rules, recipe, parts, paper_count)}


def print_plan(catalog, locale, recipe, plan):
    effect_id = recipe["effectId"]
    effect = locale["inscription"]["effects"][effect_id]
    distribution = "、".join(f"{qty}张 {tickets * 100 / plan['yieldTotalTickets']:.2f}%"
                            for qty, tickets in plan["yieldTickets"].items())
    low, high = plan["effectRange"]
    if catalog["inscription"]["effects"][effect_id]["unitId"] == 2:
        bounds = f"{low / 100:.2f}%～{high / 100:.2f}%"
    else:
        bounds = f"{low}～{high} {effect['unit']}"
    print(f"成符率 {plan['successBps'] / 100:.2f}%")
    print(f"成功后出符　{distribution}（最多 {plan['paperCap']} 张）")
    print(f"单张{effect['name']}　{bounds}")

    source_names = {"rank": "符师", "brush": "符笔", "brushElement": "符笔五行", "methodElement": "功法五行", "place": "地区地利"}
    target_names = {"pattern": "符方", "ink": "符墨", "paper": "符纸"}
    for part in plan["adjustments"]:
        name = source_names[part["sourceId"]]
        if "relations" in part:
            relation_names = locale["inscription"]["relations"]
            element = locale["elements"][str(part["elementId"])]["name"]
            relations = "、".join(f"对{target_names[key]}{relation_names[value]['name']}"
                                 for key, value in part["relations"].items())
            name += f"（{element}，{relations}）"
        if "regionId" in part:
            name += f"（{locale['regions'][str(part['regionId'])]['name']}）"
        print(f"  {name}　成符 {part['successBps'] / 100:+.2f} 个百分点，"
              f"出符 {part['yieldBps'] / 100:+.2f}%，符效倍率 {part['potencyBps'] / 100:+.2f} 个百分点")


def main(argv=None):
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--talisman", required=True, help="符名、成品模板键或符方键")
    parser.add_argument("--brush", help="符笔模板键，例如 obj.yunmu_brush")
    parser.add_argument("--rank", type=int, help="符师品阶 0～9，与 --brush 一起用于估算")
    parser.add_argument("--method", help="角色已装备纳气功法的编号 gf_3_1～gf_3_16")
    parser.add_argument("--region", help="画符所在郡或其下级地图 ID")
    parser.add_argument("--roles", type=Path, help="可选的只读角色快照，用于计算地区共鸣")
    parser.add_argument("--period", type=int, default=0, help="地区词条的周期编号")
    parser.add_argument("--config", type=Path, default=CONFIG)
    parser.add_argument("--locale", type=Path)
    parser.add_argument("--json", action="store_true", help="输出含地区、采法、掉率及加工树的结构化数据")
    args = parser.parse_args(argv)
    if args.roles and not args.region:
        parser.error("--roles 需要指定画符地点 --region")
    wants_plan = args.rank is not None or bool(args.method or args.region)
    if wants_plan and (args.rank is None or not args.brush):
        parser.error("估算同批符箓需要同时指定 --rank 和 --brush")
    try:
        config = read_json(args.config)
        locale = read_json(args.locale or args.config.with_name(args.config.stem + ".zh-CN.json"))
        recipe_id, recipe = find_recipe(config, locale, args.talisman.strip(), "inscription", "符方")
        materials = material_guide(config, locale, recipe)
        brush = None
        if args.brush:
            if args.brush not in config["inscription"]["brushes"]:
                raise ValueError(f"未登记的符笔 {args.brush}")
            brush = {"itemId": args.brush, "name": locale["itemTemplates"][args.brush]["name"],
                     "sources": crafting_sources(config, locale)(args.brush)}
        plan = None
        if wants_plan:
            roles = read_json(args.roles, legacy=True) if args.roles else None
            plan = draw_plan(config, locale, recipe, args.rank, args.brush, args.method, args.region, roles, args.period)
    except (OSError, KeyError, ValueError) as error:
        parser.exit(1, f"{error}\n")
    name = locale["itemTemplates"][recipe["outputId"]]["name"]
    if args.json:
        print(json.dumps({"recipeId": recipe_id, "name": name, **recipe, "ingredients": materials,
                          "brush": brush, "plan": plan}, ensure_ascii=False, indent=2))
        return
    print(f"{name}　{recipe_id}")
    labels = {"paper": "符纸", "ink": "符墨", "auxiliary": "辅材"}
    for item in materials:
        print(f"  {labels[item['role']]}　{item['name']} ×{item['qty']}　{item['purpose']}")
        print_sources(locale, item["sources"], "    ")
    if brush:
        print(f"  符笔　{brush['name']}")
        print_sources(locale, brush["sources"], "    ")
    if plan is not None:
        print_plan(config, locale, recipe, plan)


if __name__ == "__main__":
    main()
