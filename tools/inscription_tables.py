"""校验符方、加工原料和画符规则，按稳定 ID 编译。"""

import re

if __package__:
    from .crafting_tables import RELATIONS, limited_value, modifiers, regional_source_check, requirements_by_product, write_row
else:
    from crafting_tables import RELATIONS, limited_value, modifiers, regional_source_check, requirements_by_product, write_row


SHEETS = {
    "符方表": (
        "recipes",
        "符方键 成品模板键 符纸模板键 符墨模板键 封印法术对象键 最低符师品阶ID 基础成符率基点 基础最少张 基础最多张 符效键 单张基础符效 说明",
        "id outputId paperId inkId spellProfileId minRank successBps minQty maxQty effectId baseEffect note",
    ),
    "符箓符效": ("effects", "符效键 名称 单位ID 计量单位 作用说明", "id name unitId unit note"),
    "符师工艺": ("ranks", "符师品阶ID 名称 成符修正基点 出符修正基点 符效修正基点 说明",
                "id name successBps yieldBps potencyBps note"),
    "符笔工艺": ("brushes", "符笔模板键 最低符师品阶ID 成符修正基点 出符修正基点 符效修正基点 说明",
                "id minRank successBps yieldBps potencyBps note"),
    "画符生克": ("relations", "关系键 名称 成符修正基点 出符修正基点 符效修正基点 说明",
                "id name successBps yieldBps potencyBps note"),
    "画符符效区间": ("potencyByQuality", "品质ID 下限基点 上限基点 说明", "id minBps maxBps note"),
    "画符结算": ("limits", "设置键 基点 说明", "id bps note"),
}
OPTIONAL = {"符方表": {"封印法术对象键"}}
LIMITS = {
    "successMinBps": 10000, "successMaxBps": 10000,
    "yieldMultiplierMinBps": 30000, "yieldMultiplierMaxBps": 30000,
    "patternWeightBps": 10000, "inkWeightBps": 10000,
}


def compile_controls(sheets):
    runtime = {table: {} for table in ("limits", "potencyByQuality", "ranks", "relations", "effects")}
    locale = {key: {} for key in runtime}
    limits = runtime["limits"]
    for location, row in sheets["画符结算"]:
        key = row["id"]
        if key not in LIMITS:
            raise ValueError(f"{location} 未知画符结算键 {key}")
        write_row(limits, key, limited_value(row, "bps", location, 0, LIMITS[key]), location)
        locale["limits"][key] = {"note": row["note"]}
    if limits.keys() != LIMITS.keys():
        raise ValueError(f"画符结算缺少设置键 {sorted(LIMITS.keys() - limits.keys())}")
    if (limits["successMinBps"] > limits["successMaxBps"] or
            not 0 < limits["yieldMultiplierMinBps"] <= limits["yieldMultiplierMaxBps"] or
            limits["patternWeightBps"] + limits["inkWeightBps"] > 10000):
        raise ValueError("画符结算的上下限或五行权重无效")

    for location, row in sheets["画符符效区间"]:
        quality = limited_value(row, "id", location, 1, 3)
        low = limited_value(row, "minBps", location, 1, 30000)
        high = limited_value(row, "maxBps", location, low, 30000)
        write_row(runtime["potencyByQuality"], quality, [low, high], location)
        locale["potencyByQuality"][quality] = {"note": row["note"]}
    if runtime["potencyByQuality"].keys() != {1, 2, 3}:
        raise ValueError("画符符效区间须填写下、中、上三个品质")

    for location, row in sheets["符师工艺"]:
        rank = limited_value(row, "id", location, 0, 9)
        write_row(runtime["ranks"], rank, modifiers(row, location), location)
        locale["ranks"][rank] = {"name": row["name"], "note": row["note"]}
    if runtime["ranks"].keys() != set(range(10)):
        raise ValueError("符师工艺须覆盖原职业编号 0～9")

    for location, row in sheets["画符生克"]:
        key = row["id"]
        if key not in RELATIONS:
            raise ValueError(f"{location} 未知五行关系 {key}")
        write_row(runtime["relations"], key, modifiers(row, location), location)
        locale["relations"][key] = {"name": row["name"], "note": row["note"]}
    if runtime["relations"].keys() != RELATIONS:
        raise ValueError(f"画符生克缺少关系 {sorted(RELATIONS - runtime['relations'].keys())}")

    for location, row in sheets["符箓符效"]:
        key = row["id"]
        if not re.fullmatch(r"[a-z][a-z0-9_]*", key):
            raise ValueError(f"{location} 符效键须为小写 ASCII 稳定键")
        write_row(runtime["effects"], key, {"unitId": limited_value(row, "unitId", location, 1, 2)}, location)
        locale["effects"][key] = {field: row[field] for field in ("name", "unit", "note")}
    return runtime, locale


def compile_inscription(config, sheets):
    runtime, locale = compile_controls(sheets)
    recipes, by_output, recipe_notes, brushes, brush_notes = {}, {}, {}, {}, {}
    templates = {row["id"]: row for row in config["itemTemplates"]}
    profiles = {row["id"]: row for row in config["profiles"]}
    grades = {row["id"] for row in config["grades"]}
    ingredients = requirements_by_product(config)
    world = config["worldRules"]
    has_region_source = regional_source_check(ingredients, world["resourceSources"])

    def profile(item_id, kind, location):
        template = templates.get(item_id)
        info = profiles[template["profileId"]] if template else {}
        if (info.get("kindId") != kind or not info.get("primaryElementId") or
                template["gradeId"] not in grades):
            raise ValueError(f"{location} {item_id} 须引用有主五行的常规品阶模板（{kind}）")
        return info

    for location, row in sheets["符笔工艺"]:
        brush = row["id"]
        profile(brush, "talisman_brush", location)
        if not has_region_source(brush):
            raise ValueError(f"{location} 笔 {brush} 须有可追到地区的来源或加工配方")
        minimum = limited_value(row, "minRank", location, 0, 9)
        write_row(brushes, brush, {"minRank": minimum, **modifiers(row, location)}, location)
        brush_notes[brush] = {"note": row["note"]}
    if not brushes or 3 not in config["methodInfluences"].get("inscription", {}):
        raise ValueError("画符须登记可用符笔和功法工艺的 inscription 纳气功法")

    for location, row in sheets["符方表"]:
        recipe_id, output, paper, ink = (row[field] for field in ("id", "outputId", "paperId", "inkId"))
        if not re.fullmatch(r"[a-z][a-z0-9._-]*", recipe_id):
            raise ValueError(f"{location} 符方键须为小写 ASCII 稳定键")
        target = profile(output, "talisman", location)
        profile(paper, "talisman_paper", location)
        profile(ink, "talisman_ink", location)
        if output in by_output:
            raise ValueError(f"{location} 成品模板 {output} 已有基础符方")
        inputs = ingredients.get(output, {})
        if paper not in inputs or ink not in inputs:
            raise ValueError(f"{location} 符纸和符墨须在「造物用料」登记用量")
        for component in sorted(inputs):
            if "inscription" not in world["resourceUses"].get(component, ()) or not has_region_source(component):
                raise ValueError(f"{location} 材料 {component} 须登记画符用途，并沿采集、掉落或加工查到地区")
        effect = row["effectId"]
        if effect not in runtime["effects"]:
            raise ValueError(f"{location} 未登记符效键 {effect}")
        spell_id = row["spellProfileId"]
        if spell_id:
            spell = profiles.get(spell_id, {})
            if (spell.get("kindId") != "spell" or spell["primaryElementId"] != target["primaryElementId"] or
                    spell["primaryQiId"] != target["primaryQiId"]):
                raise ValueError(f"{location} 封印法术须为同主行、同主气的法术对象")
        minimum = limited_value(row, "minQty", location, 1, min(100, inputs[paper]))
        recipe = {
            "outputId": output, "paperId": paper, "inkId": ink, "effectId": effect,
            "minRank": limited_value(row, "minRank", location, 0, 9),
            "successBps": limited_value(row, "successBps", location, 0, 10000),
            "minQty": minimum, "maxQty": limited_value(row, "maxQty", location, minimum, min(100, inputs[paper])),
            "baseEffect": limited_value(row, "baseEffect", location, 1, 2147483647),
        }
        if spell_id:
            recipe["spellProfileId"] = spell_id
        write_row(recipes, recipe_id, recipe, location)
        by_output[output], recipe_notes[recipe_id] = recipe_id, {"note": row["note"]}
    if not recipes:
        raise ValueError("符方表至少须登记一条符方")
    runtime.update({"recipes": recipes, "recipeByOutput": by_output, "brushes": brushes})
    locale.update({"recipes": recipe_notes, "brushes": brush_notes})
    return ({key: dict(sorted(table.items())) for key, table in runtime.items()},
            {key: dict(sorted(table.items())) for key, table in locale.items()})
