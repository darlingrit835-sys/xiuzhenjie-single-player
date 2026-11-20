"""按数字 ID 编译修炼途径、功法和兼修规则，保留旧存档的 gf_* 映射。"""

from collections import defaultdict
import re

if __package__:
    from .crafting_tables import limited_value, write_row
else:
    from crafting_tables import limited_value, write_row


SHEETS = {
    "修炼途径": ("paths", "途径ID 稳定键 名称 旧装备栏 成长说明",
             "id key name equipField note"),
    "功法品阶规则": ("gradeRules", "品阶ID 最低角色等级 周天千分下限 周天千分上限 说明",
                 "gradeId minLevel minCircleMilli maxCircleMilli note"),
    "功法目录": ("templates", "功法ID 名称 途径ID 属性对象键 品阶ID 修习等级 基础周天千分 图标ID 旧功法键",
             "id name pathId profileId gradeId minLevel circleMilli iconId legacyId"),
    "功法词条": ("affixes", "功法ID 词条键 固定基点", "methodId statId valueBps"),
    "原功法效果": ("legacyEffects", "功法ID 原效果编号 原面板值", "methodId effectId rawValue"),
    "功法重数": ("mastery", "重数ID 名称 周天倍率基点", "id name circleBps"),
    "兼修组合": ("modes", "组合ID 名称 开放等级 旧位置码 说明",
             "id name minLevel legacyLocation note"),
    "兼修分配": ("allocations", "组合ID 途径ID 本途径效率基点", "modeId pathId efficiencyBps"),
}
OPTIONAL = {"功法目录": {"旧功法键"}, "修炼途径": {"旧装备栏"}, "兼修组合": {"旧位置码"}}
LEGACY_SLOTS = {1: "GongFaLianShen", 2: "GongFaDuanTi", 3: "GongFaNaQi"}
LEGACY_LOCATIONS = {1: (1,), 2: (2,), 3: (3,), 4: (2, 3), 5: (1, 2), 6: (1, 3)}


def positive_id(row, field, location):
    return limited_value(row, field, location, 1, 2147483647)


def compile_paths(sheets):
    paths, labels, keys = {}, {}, set()
    for location, row in sheets["修炼途径"]:
        path = positive_id(row, "id", location)
        key = row["key"]
        if not re.fullmatch(r"[a-z][a-z0-9_]*", key) or key in keys:
            raise ValueError(f"{location} 修炼途径稳定键重复或格式不正确")
        slot = row["equipField"]
        if slot != LEGACY_SLOTS.get(path):
            raise ValueError(f"{location} 旧装备栏须与原存档途径对应，新途径留空")
        entry = {"key": key}
        if slot:
            entry["equipField"] = slot
        write_row(paths, path, entry, location)
        labels[path] = {"name": row["name"], "note": row["note"]}
        keys.add(key)
    if not LEGACY_SLOTS.keys() <= paths.keys():
        raise ValueError("修炼途径缺少原有的炼神、炼体或炼气")
    return paths, labels


def compile_grade_rules(sheets, grades):
    rules, labels = {}, {}
    for location, row in sheets["功法品阶规则"]:
        grade = positive_id(row, "gradeId", location)
        if grade not in grades:
            raise ValueError(f"{location} 未登记的品阶")
        low = positive_id(row, "minCircleMilli", location)
        high = limited_value(row, "maxCircleMilli", location, low, 2147483647)
        write_row(rules, grade, {"minLevel": positive_id(row, "minLevel", location),
                                 "minCircleMilli": low, "maxCircleMilli": high}, location)
        labels[grade] = {"note": row["note"]}
    if rules.keys() != grades:
        raise ValueError("功法品阶规则须覆盖常规品阶及旧极品")
    for previous, current in zip(sorted(grades), sorted(grades)[1:]):
        if (rules[previous]["minLevel"] > rules[current]["minLevel"] or
                rules[previous]["minCircleMilli"] > rules[current]["minCircleMilli"] or
                rules[previous]["maxCircleMilli"] > rules[current]["maxCircleMilli"]):
            raise ValueError(f"功法品阶规则 {current} 的门槛或周天区间较低品阶倒退")
    return rules, labels


def compile_modes(sheets, paths):
    modes, labels, old_locations, allocations = {}, {}, {}, defaultdict(dict)
    for location, row in sheets["兼修组合"]:
        mode = positive_id(row, "id", location)
        write_row(modes, mode, {"minLevel": positive_id(row, "minLevel", location)}, location)
        if row["legacyLocation"] is not None:
            code = limited_value(row, "legacyLocation", location, 1, 6)
            write_row(old_locations, code, mode, location)
        labels[mode] = {"name": row["name"], "note": row["note"]}
    for location, row in sheets["兼修分配"]:
        mode = positive_id(row, "modeId", location)
        path = positive_id(row, "pathId", location)
        if mode not in modes or path not in paths:
            raise ValueError(f"{location} 未登记的兼修组合或修炼途径")
        weight = limited_value(row, "efficiencyBps", location, 1, 10000)
        write_row(allocations[mode], path, weight, location)

    combinations = set()
    for mode, definition in modes.items():
        selected = tuple(sorted(allocations[mode]))
        if not selected or selected in combinations:
            raise ValueError(f"兼修组合 {mode} 的途径为空或与另一组合相同")
        if len(selected) > 1 and definition["minLevel"] < 31:
            raise ValueError(f"兼修组合 {mode} 须达到原兼修门槛31级")
        if sum(allocations[mode].values()) > 15000:
            raise ValueError(f"兼修组合 {mode} 总效率超出15000基点")
        definition["efficiencyByPath"] = dict(sorted(allocations[mode].items()))
        combinations.add(selected)
    if not {(path,) for path in paths} <= combinations:
        raise ValueError("每条修炼途径均须配置可用的单修组合")
    if (old_locations.keys() != LEGACY_LOCATIONS.keys() or
            any(tuple(modes[mode]["efficiencyByPath"]) != LEGACY_LOCATIONS[code]
                for code, mode in old_locations.items())):
        raise ValueError("旧位置码与原单修、双修组合不一致")
    return modes, labels, old_locations


def compile_methods(config, sheets):
    legacy_grades = {row["id"] for row in config["legacyGrades"]}
    grades = legacy_grades | {row["id"] for row in config["grades"]}
    grade_rules, grade_labels = compile_grade_rules(sheets, grades)
    paths, path_labels = compile_paths(sheets)
    for system, influences in config["methodInfluences"].items():
        if not influences.keys() <= paths.keys():
            raise ValueError(f"功法工艺 {system} 引用了未登记的途径")
    modes, mode_labels, old_locations = compile_modes(sheets, paths)
    profiles = {row["id"]: row for row in config["profiles"]}
    templates, by_legacy, used_profiles = {}, {}, set()
    method_labels = {}
    for location, row in sheets["功法目录"]:
        key = positive_id(row, "id", location)
        grade = positive_id(row, "gradeId", location)
        path = positive_id(row, "pathId", location)
        if path not in paths or grade not in grade_rules:
            raise ValueError(f"{location} 未登记的功法途径或品阶")
        profile_id = row["profileId"]
        profile = profiles.get(profile_id, {})
        if (profile_id in used_profiles or profile.get("kindId") not in {"cultivation_method", "lineage_method"}
                or not all(profile.get(f) for f in ("polarityId", "primaryElementId", "primaryQiId"))):
            raise ValueError(f"{location} 应引用对应的功法属性对象，并填写阴阳、五行和主气")
        used_profiles.add(profile_id)
        method_labels[key] = {"name": row["name"]}
        legacy = row["legacyId"]
        if legacy:
            if (legacy != f"gf_{path}_{grade}" or path not in LEGACY_SLOTS or grade not in range(1, 17)
                    or profile["sourceId"] != legacy or profile["primaryElementId"] != (grade - 1) % 5 + 1):
                raise ValueError(f"{location} 旧功法键、品阶、原对象键或五行与既有定义不一致")
            write_row(by_legacy, legacy, key, location)
        elif grade in legacy_grades or re.fullmatch(r"gf_\d+_\d+", profile["sourceId"] or ""):
            raise ValueError(f"{location} 新功法须选常规品阶，原功法须登记旧键")
        limits = grade_rules[grade]
        method = {"pathId": path, "profileId": profile_id, "gradeId": grade, "iconId": positive_id(row, "iconId", location),
                  "minLevel": limited_value(row, "minLevel", location, limits["minLevel"], 2147483647),
                  "circleMilli": limited_value(row, "circleMilli", location, limits["minCircleMilli"], limits["maxCircleMilli"])}
        write_row(templates, key, method, location)
    expected_legacy = {f"gf_{path}_{grade}" for path in LEGACY_SLOTS for grade in range(1, 17)}
    if by_legacy.keys() != expected_legacy:
        raise ValueError(f"功法目录缺少旧功法键 {sorted(expected_legacy - by_legacy.keys())}")
    if {m["pathId"] for m in templates.values()} != paths.keys():
        raise ValueError("每条修炼途径至少需要一本功法")

    affixes = defaultdict(dict)
    for location, row in sheets["功法词条"]:
        method = positive_id(row, "methodId", location)
        stat = row["statId"]
        if method not in templates or stat not in config["affixRanges"]:
            raise ValueError(f"{location} 功法词条引用了未登记的功法或词条")
        quality = (templates[method]["gradeId"] - 1) % 4 + 1
        low, high = config["affixRanges"][stat][quality]
        amount = limited_value(row, "valueBps", location, low, high)
        write_row(affixes[method], stat, amount, location)
    for method, definition in templates.items():
        tier = (definition["gradeId"] - 1) // 4 + 1
        if len(affixes[method]) != config["affixCounts"][tier]:
            raise ValueError(f"功法 {method} 需按品阶填写 {config['affixCounts'][tier]} 个不同词条")
        definition["affixes"] = dict(sorted(affixes[method].items()))

    legacy_templates = set(by_legacy.values())
    legacy_effects = defaultdict(dict)
    for location, row in sheets["原功法效果"]:
        key = positive_id(row, "methodId", location)
        if key not in legacy_templates:
            raise ValueError(f"{location} 原面板效果只供旧功法兼容")
        effect = positive_id(row, "effectId", location)
        write_row(legacy_effects[key], effect, positive_id(row, "rawValue", location), location)
    if set(legacy_effects) != legacy_templates or any(len(v) != 2 for v in legacy_effects.values()):
        raise ValueError("每本旧功法需登记两个原面板效果")

    mastery, mastery_labels = {}, {}
    for location, row in sheets["功法重数"]:
        stage = limited_value(row, "id", location, 1, 1000)
        write_row(mastery, stage, limited_value(row, "circleBps", location, 1, 30000), location)
        mastery_labels[stage] = {"name": row["name"]}
    if (not mastery or sorted(mastery) != list(range(1, max(mastery) + 1)) or
            mastery[1] != 10000 or any(mastery[s] < mastery[s-1] for s in mastery if s > 1)):
        raise ValueError("功法重数须从一重、10000基点起连续递增或持平")

    return ({"paths": paths, "templates": templates, "byLegacyId": by_legacy, "gradeRules": grade_rules,
             "mastery": mastery, "modes": modes, "byLegacyLocation": old_locations, "legacyEffects": dict(legacy_effects)},
            {"paths": path_labels, "templates": method_labels, "gradeRules": grade_labels, "mastery": mastery_labels, "modes": mode_labels})


def resolve_method(catalog, equipped):
    """在已导出的运行配置中以数字模板 ID 或旧服功法键查表。"""
    rules = catalog["methods"]
    key = str(equipped)
    return rules["templates"].get(str(rules["byLegacyId"].get(key, key)))
