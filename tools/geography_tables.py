"""将地域父链、地利词条和修士共鸣编译为按稳定键查找的规则与中文显示资源。"""

import re


SHEETS = {
    "地域总表": ("regions", "地域ID 上级ID 层级 名称 阴阳ID 五行ID 主气ID 地貌说明",
             "id parentId level name polarityId elementId qiId description"),
    "地域词条字典": ("regionEffects", "词条键 名称 玩法键 目标类型ID 结算方式ID 结算口径",
               "id name systemId targetKind modeId description"),
    "地域词条": ("regionModifiers", "地域ID 词条键 目标五行ID 共鸣五行ID 基础基点 自然浮动基点 作物示例模板键 玩家显示",
             "regionId effectId targetElementId resonanceElementId baseBps jitterBps exampleTemplateId label"),
    "修士共鸣": ("regionResonance", "规则键 名称 同属基点 相生基点 被生基点 相克基点 被克基点 修正上限基点 取值约定",
             "id name sameBps generatesBps generatedByBps overcomesBps overcomeByBps capBps description"),
}
OPTIONAL = {"地域词条": {"作物示例模板键"}}


def integer(row, field, location):
    value = str(row[field])
    if not re.fullmatch(r"-?[0-9]+(?:\.0+)?", value):
        raise ValueError(f"{location}「{field}」必须使用整数")
    return int(value.split(".")[0])


def ordered_rows(sheets, sheet, *fields):
    return sorted(sheets[sheet], key=lambda pair: tuple(str(pair[1][field]) for field in fields))


def compile_regions(config, sheets):
    regions, texts = {}, {}
    dimensions = {field: {row["id"] for row in config[table]} for field, table in (
        ("polarityId", "polarities"), ("elementId", "elements"), ("qiId", "qiTypes"))}
    for location, row in ordered_rows(sheets, "地域总表", "id"):
        key = str(integer(row, "id", location))
        node = {field: integer(row, field, location) for field in ("parentId", "level", *dimensions)}
        if key in regions or int(key) <= 0 or node["level"] not in range(4):
            raise ValueError(f"{location} 地域ID重复或层级无效")
        for field, valid in dimensions.items():
            if node[field] not in valid and not (key == "1" and node[field] == 0):
                raise ValueError(f"{location}「{field}」未登记在对应属性字典")
        regions[key] = node
        texts[key] = {"name": row["name"], "description": row["description"]}
    root = regions.get("1")
    if root is None or root["level"] != 0 or root["parentId"] != 0:
        raise ValueError("地域总表缺少原地图的世界根节点 1（层级0，上级0）")
    for key, node in regions.items():
        if key == "1":
            continue
        parent = regions.get(str(node["parentId"]))
        if parent is None or node["level"] != parent["level"] + 1:
            raise ValueError(f"地域总表 地域{key}的上级不存在或层级不连续")
    return regions, texts


def compile_resonance(sheets):
    rows = sheets["修士共鸣"]
    if len(rows) != 1 or rows[0][1]["id"] != "present_element":
        raise ValueError("修士共鸣需要一行 present_element 在地修士规则")
    location, row = rows[0]
    rules = {field: integer(row, field, location) for field in (
        "sameBps", "generatesBps", "generatedByBps", "overcomesBps", "overcomeByBps", "capBps")}
    if not 0 < rules["capBps"] <= 10000 or any(abs(value) > 10000 for value in rules.values()):
        raise ValueError(f"{location} 单人修正须在正负10000基点内，修正上限须为正数")
    return rules, {"name": row["name"], "description": row["description"]}


def compile_geography(config, sheets):
    """接收已校验的基础属性、模板和玩法主表，输出可直接合并的两个片段。"""
    regions, region_texts = compile_regions(config, sheets)
    resonance, resonance_text = compile_resonance(sheets)
    systems = {row["id"] for row in config["typeRegistry"] if row["domain"] == "system"}
    elements = {row["id"]: row for row in config["elements"]}
    templates = {row["id"]: row for row in config["itemTemplates"]}
    profiles = {row["id"]: row for row in config["profiles"]}
    effects, effect_texts = {}, {}
    for location, row in ordered_rows(sheets, "地域词条字典", "id"):
        key = row["id"]
        target_kind = integer(row, "targetKind", location)
        mode = integer(row, "modeId", location)
        if mode not in (1, 2):
            raise ValueError(f"{location} 结算方式ID应为1（相对倍率）或2（概率加百分点）")
        if not re.fullmatch(r"[A-Za-z][A-Za-z0-9._-]*", key) or key in effects:
            raise ValueError(f"{location} 词条键须唯一且为有效的程序键")
        if row["systemId"] not in systems or target_kind not in range(1, 7):
            raise ValueError(f"{location} 未知玩法键或目标类型ID")
        effects[key] = {"targetKind": target_kind, "systemId": row["systemId"], "modeId": mode}
        effect_texts[key] = {"name": row["name"], "description": row["description"]}

    modifiers, modifier_texts = {}, {}
    for location, row in ordered_rows(sheets, "地域词条", "regionId", "effectId", "targetElementId"):
        region = str(integer(row, "regionId", location))
        effect = row["effectId"]
        target = integer(row, "targetElementId", location)
        source = integer(row, "resonanceElementId", location)
        if region not in regions or regions[region]["level"] == 0 or effect not in effects:
            raise ValueError(f"{location} 词条应引用已登记的大陆、洲或地区及有效词条键")
        if target not in elements or source not in elements:
            raise ValueError(f"{location} 目标与共鸣五行都须引用五行字典")
        if effects[effect]["targetKind"] == 6 and elements[source]["overcomes"] != target:
            raise ValueError(f"{location} 地利克制的敌方五行须为共鸣五行所克的目标")
        base = integer(row, "baseBps", location)
        jitter = integer(row, "jitterBps", location)
        cap = resonance["capBps"]
        if jitter < 0 or base - jitter - cap < 0 or base + jitter + cap > 10000:
            raise ValueError(f"{location} 自然浮动幅度须非负，叠加自然浮动与共鸣后的收益须在0至10000基点内")
        detail = {"resonanceElementId": source, "baseBps": base, "jitterBps": jitter}
        example = row["exampleTemplateId"]
        if example:
            if effects[effect]["targetKind"] != 2 or example not in templates:
                raise ValueError(f"{location} 作物示例应属于种植作用对象并引用已登记的物品模板")
            profile = profiles[templates[example]["profileId"]]
            if profile["primaryElementId"] != target:
                raise ValueError(f"{location} 作物示例的主五行与目标五行不一致")
            detail["exampleTemplateId"] = example
        matches = modifiers.setdefault(region, {}).setdefault(effect, {})
        if str(target) in matches:
            raise ValueError(f"{location} 地域、词条与目标五行组合重复")
        matches[str(target)] = detail
        modifier_texts.setdefault(region, {}).setdefault(effect, {})[str(target)] = {"label": row["label"]}
    runtime = {"regions": regions, "regionEffects": effects, "regionModifiers": modifiers, "regionResonance": resonance}
    locale = {"regions": region_texts, "regionEffects": effect_texts,
              "regionModifiers": modifier_texts, "regionResonance": resonance_text}
    return runtime, locale
