"""校验丹方与已有材料、炉火、功法和产地的引用。"""

import re

if __package__:
    from .crafting_tables import RELATIONS, limited_value, modifiers, regional_source_check, requirements_by_product, write_row
else:
    from crafting_tables import RELATIONS, limited_value, modifiers, regional_source_check, requirements_by_product, write_row


SHEETS = {
    "丹方表": (
        "recipes", "丹方键 成品模板键 主药模板键 最低丹师品阶ID 基础成丹率基点 基础最少粒 基础最多粒 本炉上限粒 药效键 单粒基础药效 说明",
        "id outputId primaryId minRank successBps minQty maxQty yieldCap effectId baseEffect note",
    ),
    "丹药药效": ("effects", "药效键 名称 计量单位 作用说明", "id name unit note"),
    "丹师工艺": ("ranks", "丹师品阶ID 名称 成丹修正基点 出丹修正基点 药效修正基点 说明",
                "id name successBps yieldBps potencyBps note"),
    "丹火工艺": ("flames", "炉火键 最低丹师品阶ID 成丹修正基点 出丹修正基点 药效修正基点 说明",
                "id minRank successBps yieldBps potencyBps note"),
    "炼丹生克": ("relations", "关系键 名称 成丹修正基点 出丹修正基点 药效修正基点 说明",
                "id name successBps yieldBps potencyBps note"),
    "炼丹药效区间": ("potencyByQuality", "品质ID 下限基点 上限基点 说明", "id minBps maxBps note"),
    "炼丹结算": ("limits", "设置键 基点 说明", "id bps note"),
}

LIMIT_KEYS = {
    "successMinBps", "successMaxBps", "yieldMultiplierMinBps", "yieldMultiplierMaxBps", "primaryWeightBps",
}


def compile_controls(config, sheets):
    runtime = {table: {} for table in ("limits", "potencyByQuality", "ranks", "flames", "relations", "effects")}
    locale = {key: {} for key in runtime}
    limits = runtime["limits"]
    for location, row in sheets["炼丹结算"]:
        key = row["id"]
        if key not in LIMIT_KEYS:
            raise ValueError(f"{location} 未知炼丹结算键 {key}")
        high = 10000 if key.startswith("success") or key == "primaryWeightBps" else 30000
        write_row(limits, key, limited_value(row, "bps", location, 0, high), location)
        locale["limits"][key] = {"note": row["note"]}
    if set(limits) != LIMIT_KEYS:
        raise ValueError(f"炼丹结算缺少设置键 {sorted(LIMIT_KEYS - limits.keys())}")
    if (limits["successMinBps"] > limits["successMaxBps"] or
            not 0 < limits["yieldMultiplierMinBps"] <= limits["yieldMultiplierMaxBps"]):
        raise ValueError("炼丹结算上下限无效")

    for location, row in sheets["炼丹药效区间"]:
        quality = limited_value(row, "id", location, 1, 3)
        low = limited_value(row, "minBps", location, 1, 30000)
        high = limited_value(row, "maxBps", location, low, 30000)
        write_row(runtime["potencyByQuality"], quality, [low, high], location)
        locale["potencyByQuality"][quality] = {"note": row["note"]}
    if runtime["potencyByQuality"].keys() != {1, 2, 3}:
        raise ValueError("炼丹药效区间须填写下、中、上三个品质")

    for location, row in sheets["丹师工艺"]:
        rank = limited_value(row, "id", location, 0, 9)
        write_row(runtime["ranks"], rank, modifiers(row, location), location)
        locale["ranks"][rank] = {"name": row["name"], "note": row["note"]}
    if runtime["ranks"].keys() != set(range(10)):
        raise ValueError("丹师工艺须覆盖原职业编号 0～9")

    for location, row in sheets["丹火工艺"]:
        key = row["id"]
        if key not in config["forgeFactors"]["flames"]:
            raise ValueError(f"{location} 炉火键 {key} 须引用「炼器工艺」已登记的炉火")
        minimum = limited_value(row, "minRank", location, 0, 9)
        write_row(runtime["flames"], key, {"minRank": minimum, **modifiers(row, location)}, location)
        locale["flames"][key] = {"note": row["note"]}
    if not runtime["flames"]:
        raise ValueError("丹火工艺至少须登记一项可用炉火")

    for location, row in sheets["炼丹生克"]:
        key = row["id"]
        if key not in RELATIONS:
            raise ValueError(f"{location} 未知五行关系 {key}")
        write_row(runtime["relations"], key, modifiers(row, location), location)
        locale["relations"][key] = {"name": row["name"], "note": row["note"]}
    if runtime["relations"].keys() != RELATIONS:
        raise ValueError(f"炼丹生克缺少关系 {sorted(RELATIONS - runtime['relations'].keys())}")

    methods = config["methodInfluences"].get("alchemy", {})
    if 3 not in methods:
        raise ValueError("功法工艺须登记 alchemy 的纳气功法类型 3")
    for location, row in sheets["丹药药效"]:
        key = row["id"]
        if not re.fullmatch(r"[a-z][a-z0-9_]*", key):
            raise ValueError(f"{location} 药效键须为小写 ASCII 稳定键")
        write_row(runtime["effects"], key, {}, location)
        locale["effects"][key] = {field: row[field] for field in ("name", "unit", "note")}
    return runtime, locale


def compile_alchemy(config, sheets, world):
    runtime, locale = compile_controls(config, sheets)
    recipes, by_output, recipe_notes = {}, {}, {}
    templates = {row["id"]: row for row in config["itemTemplates"]}
    profiles = {row["id"]: row for row in config["profiles"]}
    grades = {row["id"] for row in config["grades"]}
    ingredients = requirements_by_product(config)
    has_region_source = regional_source_check(ingredients, world["resourceSources"])

    for location, row in sheets["丹方表"]:
        key, output, primary, effect = (row[field] for field in ("id", "outputId", "primaryId", "effectId"))
        if not re.fullmatch(r"[a-z][a-z0-9._-]*", key):
            raise ValueError(f"{location} 丹方键须为小写 ASCII 稳定键")
        item = templates.get(output)
        if item is None or profiles[item["profileId"]]["kindId"] != "pill" or item["gradeId"] not in grades:
            raise ValueError(f"{location} 成品 {output} 须引用常规品阶的丹药模板")
        if output in by_output:
            raise ValueError(f"{location} 成品模板 {output} 已有基础丹方")
        inputs = ingredients.get(output, set())
        if primary not in inputs or not profiles[templates[primary]["profileId"]]["primaryElementId"]:
            raise ValueError(f"{location} 主药须在「造物用料」且有主五行")
        for component in sorted(inputs):
            if "alchemy" not in world["resourceUses"].get(component, ()) or not has_region_source(component):
                raise ValueError(f"{location} 材料 {component} 须登记炼丹用途，并沿采集、掉落或加工查到地区")
        if effect not in runtime["effects"]:
            raise ValueError(f"{location} 未登记药效键 {effect}")
        minimum = limited_value(row, "minQty", location, 1, 100)
        maximum = limited_value(row, "maxQty", location, minimum, 100)
        recipe = {
            "outputId": output, "primaryId": primary, "effectId": effect,
            "minRank": limited_value(row, "minRank", location, 0, 9),
            "successBps": limited_value(row, "successBps", location, 0, 10000),
            "minQty": minimum, "maxQty": maximum,
            "yieldCap": limited_value(row, "yieldCap", location, maximum, 100),
            "baseEffect": limited_value(row, "baseEffect", location, 1, 2147483647),
        }
        write_row(recipes, key, recipe, location)
        by_output[output], recipe_notes[key] = key, {"note": row["note"]}
    if not recipes:
        raise ValueError("丹方表至少须登记一条丹方")
    runtime.update({"recipes": recipes, "recipeByOutput": by_output})
    locale["recipes"] = recipe_notes
    return ({key: dict(sorted(table.items())) for key, table in runtime.items()},
            {key: dict(sorted(table.items())) for key, table in locale.items()})
