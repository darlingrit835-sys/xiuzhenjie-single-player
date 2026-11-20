"""从修仙属性基础表导出游戏配置。只使用 Python 3 标准库。"""

import argparse
from contextlib import ExitStack
from graphlib import CycleError, TopologicalSorter
from itertools import product
import json
from pathlib import Path
import posixpath
import re
import sys
from tempfile import NamedTemporaryFile
import xml.etree.ElementTree as ET
from zipfile import BadZipFile, ZipFile

if __package__:
    from .affinity_runtime import compile_catalog
    from . import alchemy_tables, ecology_tables, geography_tables, inscription_tables, region_ecology
else:
    from affinity_runtime import compile_catalog
    import alchemy_tables
    import ecology_tables
    import geography_tables
    import inscription_tables
    import region_ecology


if __package__:
    from . import bestiary_tables, character_tables, content_affixes, creation_catalog, cultivation_methods, history_projection, monster_stats, world_systems
else:
    import bestiary_tables
    import character_tables
    import content_affixes
    import history_projection
    import monster_stats
    import creation_catalog
    import cultivation_methods
    import world_systems

CONTENT_MODULES = (bestiary_tables, cultivation_methods, monster_stats, world_systems, character_tables, history_projection, creation_catalog, content_affixes)
CONTENT_SHEETS = {name: schema for module in CONTENT_MODULES for name, schema in module.SHEETS.items()}

DATA = Path(__file__).resolve().parents[1] / "server/data"
SHEETS = {
    "阴阳": ("polarities", "编号 名称 含义", "id name meaning"),
    "五行": ("elements", "编号 名称 特性 生 克", "id name meaning generates overcomes"),
    "十二气": ("qiTypes", "编号 名称 辨识 用途举例", "id name meaning example"),
    "类型与玩法": ("typeRegistry", "用途域 稳定键 中文名", "domain id name"),
    "对象属性": ("profiles", "对象键 名称 类别键 来源状态 阴阳 主五行 辅五行 主气 辅气 原对象键 归类说明",
               "id name kindId origin polarityId primaryElementId secondaryElementId primaryQiId secondaryQiId sourceId note"),
    "玩法用法": ("usages", "玩法键 关联对象键 读取谁的属性 使用方式", "systemId profileId subject usage"),
    "加成条目": ("stats", "属性键 名称 作用点 计量口径", "id name effect basis"),
    "阴阳配气": ("polarityQiWeights", "阴阳 气 气相名 表现 亲和权重", "polarityId qiId name meaning weight"),
    "五行配气": ("elementQiWeights", "五行 气 气相名 表现 亲和权重", "elementId qiId name meaning weight"),
    "分支加成": ("bonusRules", "阴阳 五行 属性键 下限基点 上限基点",
               "polarityId elementId statId minBps maxBps"),
    "五行侧重": ("elementFocus", "五行 主属性键 取向说明", "elementId statId meaning"),
    "阴阳侧重": ("polarityFocus", "阴阳 主属性键 取向说明", "polarityId statId meaning"),
    "物品分类": ("itemCategories", "分类键 名称 上级分类键 所属细表 定义", "id name parentId detailTable meaning"),
    "物品总表": ("itemTemplates", "模板键 名称 分类键 属性对象键 来源 原型模板键 说明 品阶ID",
               "id name categoryId profileId origin prototypeId note gradeId"),
    "材料表": ("materialDetails", "模板键 材料形态 用途 加工特性", "templateId form usage processing"),
    "武器表": ("weaponDetails", "模板键 持用方式 作用方式 器胚要求", "templateId grip action blankRequirement"),
    "造物用料": ("creationInputs", "产物模板键 材料模板键 数量 用途", "outputId inputId quantity purpose"),
    "品阶表": ("grades", "品阶ID 阶 品 名称 加成倍率基点", "id tier quality name multiplierBps"),
    "旧极品对照": ("legacyGrades", "品阶ID 阶 品 名称", "id tier quality name"),
    "词条数量": ("affixCounts", "阶ID 阶 词条数 说明", "tierId tier count note"),
    "词条区间": ("affixRanges", "用途域 词条键 名称 品质ID 下限基点 上限基点 计量依据", "domain statId name qualityId low high basis"),
    "物品词条": ("itemAffixes", "模板键 词条键 固定基点", "itemId statId valueBps"),
    "炼器工艺": ("forgeFactors", "工艺域 对象键 名称 品阶ID 五行 成功基点 返料基点 成色基点 熟练基点 难度基点 说明",
              "domain id name gradeId elementId successBps returnBps rollBiasBps expBps difficultyBps note"),
    "功法工艺": ("methodInfluences", "玩法键 功法类型ID 功法品阶ID 五行作用分子 五行作用分母 说明",
              "systemId typeId gradeId numerator denominator note"),
    **geography_tables.SHEETS,
    **ecology_tables.SHEETS,
    **region_ecology.SHEETS,
    **alchemy_tables.SHEETS,
    **inscription_tables.SHEETS,
    **CONTENT_SHEETS,
}
OPTIONAL = {
    "对象属性": {"阴阳", "主五行", "辅五行", "主气", "辅气", "原对象键"},
    "物品分类": {"上级分类键"},
    "物品总表": {"原型模板键"},
    "炼器工艺": {"品阶ID", "五行"},
    **geography_tables.OPTIONAL,
    **ecology_tables.OPTIONAL,
    **inscription_tables.OPTIONAL,
    **{name: labels for module in CONTENT_MODULES for name, labels in getattr(module, "OPTIONAL", {}).items()},
}
# 外键均由对应的主键校验。本表只列可直接产生程序标识的列。
PROGRAM_KEYS = {
    "类型与玩法": {"稳定键"},
    "对象属性": {"对象键", "原对象键"},
    "加成条目": {"属性键"},
    "物品分类": {"分类键"},
    "物品总表": {"模板键"},
    "词条区间": {"词条键"},
    "炼器工艺": {"对象键"},
}
ELEMENTS = "金木水火土"
ELEMENT_IDS = {name: i for i, name in enumerate(ELEMENTS, 1)}
# 按金木水火土的编号顺序列出对应的目标。
GENERATES = "水火木土金"
OVERCOMES = "木土火金水"
X = "{http://schemas.openxmlformats.org/spreadsheetml/2006/main}"
REL = "{http://schemas.openxmlformats.org/officeDocument/2006/relationships}id"


def read_sheets(path):
    def rich_text(node):
        return "".join(t.text or "" for t in node.iter(X + "t"))

    with ZipFile(path) as archive:
        workbook = ET.fromstring(archive.read("xl/workbook.xml"))
        rels = ET.fromstring(archive.read("xl/_rels/workbook.xml.rels"))
        targets = {r.get("Id"): posixpath.normpath(posixpath.join("xl", r.get("Target"))).lstrip("/")
                   for r in rels}
        paths = {sheet.get("name"): targets[sheet.attrib[REL]]
                 for sheet in workbook.iter(X + "sheet")}
        shared = []
        for r in rels:
            if r.get("Type", "").endswith("/sharedStrings"):
                strings = ET.fromstring(archive.read(targets[r.get("Id")]))
                shared = [rich_text(si) for si in strings]
                break
        result = {}
        has_inscription = bool(inscription_tables.SHEETS.keys() & paths.keys())
        has_content = bool(CONTENT_SHEETS.keys() & paths.keys())
        if has_content and not has_inscription:
            raise ValueError("创作目录需要画符表组")
        for sheet, (_, header, fields) in SHEETS.items():
            if not has_content and sheet in CONTENT_SHEETS:
                continue
            if not has_inscription and sheet in inscription_tables.SHEETS:
                continue
            if sheet not in paths:
                raise ValueError(f"缺少工作表「{sheet}」")
            worksheet = ET.fromstring(archive.read(paths[sheet]))
            columns = {}
            rows = []
            for row in worksheet.iter(X + "row"):
                location = f"{sheet} 第 {row.get('r')} 行"
                values = {}
                for cell in row.findall(X + "c"):
                    address = cell.attrib["r"]
                    column = re.fullmatch(r"([A-Z]+)[0-9]+", address)[1]
                    value = cell.findtext(X + "v", "")
                    if cell.get("t") == "inlineStr":
                        value = rich_text(cell)
                    elif cell.get("t") == "s":
                        try:
                            index = int(value)
                            if index < 0:
                                raise ValueError
                            value = shared[index]
                        except (ValueError, IndexError):
                            raise ValueError(f"{sheet} {address} 的共享文本编号无效") from None
                    elif cell.get("t") == "e" or (cell.find(X + "f") is not None and not value):
                        raise ValueError(f"{sheet} {address} 的公式缺少有效结果")
                    if value.strip():
                        values[column] = value.strip()
                if not values:
                    continue
                if not columns:
                    for label in header.split():
                        matches = [col for col, name in values.items() if name == label]
                        if len(matches) != 1:
                            raise ValueError(f"{location} 需要且只能有一列「{label}」")
                        columns[label] = matches[0]
                    continue
                record = {}
                for (label, column), field in zip(columns.items(), fields.split()):
                    value = values.get(column)
                    if value is None and label not in OPTIONAL.get(sheet, ()):
                        raise ValueError(f"{location}「{label}」必填")
                    if value is not None and label in PROGRAM_KEYS.get(sheet, ()) and not re.fullmatch(r"[!-~]+", value):
                        raise ValueError(f"{location}「{label}」须使用无空白的 ASCII 程序键")
                    record[field] = value
                rows.append((location, record))
            if not columns:
                raise ValueError(f"{sheet} 缺少表头")
            result[sheet] = rows
    return result


def lookup(value, names, location, column):
    if value is None:
        return None
    if value not in names:
        raise ValueError(f"{location}「{column}」引用了不存在的「{value}」")
    return names[value]


def whole_number(value, location, column):
    if not re.fullmatch(r"[0-9]+(?:\.0+)?", value):
        raise ValueError(f"{location}「{column}」应为非负整数")
    return int(value.split(".")[0])


def attribute_id(value, names, location, column):
    if value is not None and value not in names and re.fullmatch(r"[0-9]+(?:\.0+)?", value):
        key = int(value.split(".")[0])
        if key in names.values():
            return key
    return lookup(value, names, location, column)


def require_complete(sheet, expected, found):
    missing = sorted(expected.difference(found))
    if missing:
        raise ValueError(f"{sheet} 缺少编号或组合 {missing}")


def keyed_rows(rows, field, column):
    by_id = {}
    for location, row in rows:
        key = row[field]
        if key in by_id:
            raise ValueError(f"{location}「{column}」{key} 重复")
        by_id[key] = row
    return by_id


def check_cycles(edges, label):
    graph = TopologicalSorter()
    for source, target in edges:
        graph.add(source, target)
    try:
        graph.prepare()
    except CycleError as error:
        # graphlib 从前置节点指向依赖方；反转后按工作簿的引用方向显示。
        cycle = list(reversed(error.args[1]))
        location = edges[cycle[0], cycle[1]]
        raise ValueError(f"{location}「{label}」出现循环引用：{' → '.join(cycle)}") from None


def item_categories(rows):
    categories = keyed_rows(rows, "id", "分类键")
    tables = {"material", "weapon", "catalog"}
    roots = set()
    parents = set()
    edges = {}
    for location, row in rows:
        category, parent_id, table = row["id"], row["parentId"], row["detailTable"]
        if table not in tables:
            raise ValueError(f"{location}「所属细表」须为 material、weapon 或 catalog")
        if parent_id is None:
            if category != table:
                raise ValueError(f"{location} 根的「分类键」须与「所属细表」相同")
            roots.add(category)
            continue
        parent = lookup(parent_id, categories, location, "上级分类键")
        if parent["detailTable"] != table:
            raise ValueError(f"{location} 与上级分类「{parent_id}」的所属细表不一致")
        parents.add(parent_id)
        edges[category, parent_id] = location
    check_cycles(edges, "上级分类键")
    require_complete("物品分类的一级根", tables, roots)
    return categories, parents


def add_grades(config, sheets):
    legacy_ids = {4, 8, 12, 16}
    current_ids = set(range(1, 17)) - legacy_ids
    for sheet, expected in (("品阶表", current_ids), ("旧极品对照", legacy_ids)):
        grades = {}
        for location, row in sheets[sheet]:
            key = whole_number(row["id"], location, "品阶ID")
            if key not in expected or key in grades:
                raise ValueError(f"{location}「品阶ID」{key} 重复或不属于{sheet}")
            tier_index, quality_index = divmod(key - 1, 4)
            tier, quality = "黄玄地天"[tier_index], "下中上极"[quality_index]
            name = f"{tier}阶{quality}品"
            if (row["tier"], row["quality"], row["name"]) != (tier, quality, name):
                raise ValueError(f"{location}「品阶ID」{key} 对应的阶、品、名称应为 {tier}、{quality}、{name}")
            grade = {**row, "id": key}
            if sheet == "品阶表":
                multiplier = whole_number(row["multiplierBps"], location, "加成倍率基点")
                if multiplier == 0:
                    raise ValueError(f"{location}「加成倍率基点」应为正整数")
                grade["multiplierBps"] = multiplier
            grades[key] = grade
        require_complete(sheet, expected, grades)
        config[SHEETS[sheet][0]] = [grades[key] for key in sorted(grades)]


def add_method_influences(config, sheets, systems):
    grades = {g["id"]: g for table in ("grades", "legacyGrades") for g in config[table]}
    influences = {}
    for location, row in sheets["功法工艺"]:
        system = row["systemId"]
        lookup(system, systems, location, "玩法键")
        method = whole_number(row["typeId"], location, "功法类型ID")
        grade = whole_number(row["gradeId"], location, "功法品阶ID")
        lookup(grade, grades, location, "功法品阶ID")
        numerator = whole_number(row["numerator"], location, "五行作用分子")
        denominator = whole_number(row["denominator"], location, "五行作用分母")
        if not method or not numerator or not denominator:
            raise ValueError(f"{location} 功法类型ID、五行作用分子和分母须为正整数")
        by_grade = influences.setdefault(system, {}).setdefault(method, {})
        if grade in by_grade:
            raise ValueError(f"{location} 功法工艺组合 {system}/{method}/{grade} 重复")
        by_grade[grade] = [numerator, denominator]
    if 3 not in influences.get("smithing", {}):
        raise ValueError("功法工艺缺少 smithing 的纳气功法（类型ID 3）")
    for system, methods in influences.items():
        for method, by_grade in methods.items():
            require_complete(f"功法工艺 {system}/{method}", set(grades), by_grade)
    config["methodInfluences"] = {
        system: {method: dict(sorted(by_grade.items())) for method, by_grade in sorted(methods.items())}
        for system, methods in sorted(influences.items())
    }


def add_item_tables(config, sheets, profiles):
    categories, parents = item_categories(sheets["物品分类"])
    config["itemCategories"] = list(categories.values())
    template_rows = sheets["物品总表"]
    templates = keyed_rows(template_rows, "id", "模板键")
    types = {}
    current_grades = {grade["id"] for grade in config["grades"]}
    all_grades = current_grades | {grade["id"] for grade in config["legacyGrades"]}
    for location, row in template_rows:
        category = lookup(row["categoryId"], categories, location, "分类键")
        if category["id"] in parents:
            raise ValueError(f"{location}「分类键」{category['id']} 须指向叶子分类")
        lookup(row["profileId"], profiles, location, "属性对象键")
        if row["origin"] not in ("项目已有", "系统设计", "玩家创制示例"):
            raise ValueError(f"{location}「来源」须为项目已有、系统设计或玩家创制示例")
        grade_id = whole_number(row["gradeId"], location, "品阶ID")
        if grade_id not in all_grades:
            raise ValueError(f"{location}「品阶ID」引用了不存在的「{grade_id}」")
        if grade_id not in current_grades and row["origin"] != "项目已有":
            raise ValueError(f"{location}「品阶ID」{grade_id} 是仅限项目已有物品引用的旧极品")
        templates[row["id"]] = {**row, "gradeId": grade_id}
        types[row["id"]] = category["detailTable"]

    prototypes = {}
    for location, row in template_rows:
        prototype = row["prototypeId"]
        if prototype is None:
            if row["origin"] == "玩家创制示例":
                raise ValueError(f"{location} 玩家创制示例须填写「原型模板键」")
            continue
        prototype_table = lookup(prototype, types, location, "原型模板键")
        if prototype_table != types[row["id"]]:
            raise ValueError(f"{location} 须选择所属细表相同的原型模板「{prototype}」")
        prototypes[row["id"], prototype] = location
    check_cycles(prototypes, "原型模板键")
    config["itemTemplates"] = list(templates.values())

    for sheet, table in (("材料表", "material"), ("武器表", "weapon")):
        rows = sheets[sheet]
        details = keyed_rows(rows, "templateId", "模板键")
        for location, row in rows:
            key = row["templateId"]
            if lookup(key, types, location, "模板键") != table:
                raise ValueError(f"{location}「模板键」{key} 的所属细表须为 {table}")
        require_complete(sheet, {key for key in types if types[key] == table}, details)
        config[SHEETS[sheet][0]] = list(details.values())

    inputs = []
    components = {}
    crafted = set()
    for location, row in sheets["造物用料"]:
        output, material = row["outputId"], row["inputId"]
        lookup(output, templates, location, "产物模板键")
        if lookup(material, types, location, "材料模板键") != "material":
            raise ValueError(f"{location}「材料模板键」{material} 须指向材料")
        if (output, material) in components:
            raise ValueError(f"{location} 产物「{output}」重复填写材料「{material}」")
        quantity = whole_number(row["quantity"], location, "数量")
        if quantity == 0:
            raise ValueError(f"{location}「数量」应为正整数")
        components[output, material] = location
        crafted.add(output)
        inputs.append({**row, "quantity": quantity})
    check_cycles(components, "造物用料")
    for location, row in template_rows:
        if row["origin"] == "玩家创制示例" and row["id"] not in crafted:
            raise ValueError(f"{location} 玩家创制示例「{row['id']}」须在造物用料中列出至少一种材料")
    config["creationInputs"] = inputs


def add_bonus_tables(config, sheets, indexes):
    stat_ids = set()
    config["stats"] = []
    for location, row in sheets["加成条目"]:
        if row["id"] in stat_ids:
            raise ValueError(f"{location}「属性键」{row['id']} 重复")
        stat_ids.add(row["id"])
        config["stats"].append(row)

    polarities = {name: indexes["阴阳"][name] for name in ("阴", "阳")}
    elements = indexes["五行"]
    qi_types = indexes["十二气"]
    dimensions = (("阴阳", "polarityId", polarities), ("五行", "elementId", elements))
    focus = {}
    for dimension, field, names in dimensions:
        weight_sheet = dimension + "配气"
        weights = []
        pairs = set()
        for location, row in sheets[weight_sheet]:
            key = attribute_id(row[field], names, location, dimension)
            qi_id = attribute_id(row["qiId"], qi_types, location, "气")
            if (key, qi_id) in pairs:
                raise ValueError(f"{location}「{dimension}、气」组合 {(key, qi_id)} 重复")
            pairs.add((key, qi_id))
            weight = whole_number(row["weight"], location, "亲和权重")
            if weight == 0:
                raise ValueError(f"{location}「亲和权重」应为正整数")
            weights.append({**row, field: key, "qiId": qi_id, "weight": weight})
        require_complete(weight_sheet, set(product(names.values(), qi_types.values())), pairs)
        config[SHEETS[weight_sheet][0]] = weights

        focus_sheet = dimension + "侧重"
        focus_stats = {}
        focus_rows = []
        for location, row in sheets[focus_sheet]:
            key = attribute_id(row[field], names, location, dimension)
            if key in focus_stats:
                raise ValueError(f"{location}「{dimension}」{key} 重复")
            stat_id = row["statId"]
            if stat_id not in stat_ids:
                raise ValueError(f"{location}「主属性键」引用了不存在的「{stat_id}」")
            focus_stats[key] = stat_id
            focus_rows.append({**row, field: key})
        require_complete(focus_sheet, set(names.values()), focus_stats)
        focus[field] = focus_stats
        config[SHEETS[focus_sheet][0]] = focus_rows

    branches = {}
    rules = []
    for location, row in sheets["分支加成"]:
        polarity = attribute_id(row["polarityId"], polarities, location, "阴阳")
        element = attribute_id(row["elementId"], elements, location, "五行")
        stat_id = row["statId"]
        if stat_id not in stat_ids:
            raise ValueError(f"{location}「属性键」引用了不存在的「{stat_id}」")
        branch = branches.setdefault((polarity, element), set())
        if stat_id in branch:
            raise ValueError(f"{location} 同一分支的「属性键」{stat_id} 重复")
        branch.add(stat_id)
        minimum = whole_number(row["minBps"], location, "下限基点")
        maximum = whole_number(row["maxBps"], location, "上限基点")
        if minimum > maximum:
            raise ValueError(f"{location}「下限基点」不能大于「上限基点」")
        rules.append({"polarityId": polarity, "elementId": element, "statId": stat_id,
                      "minBps": minimum, "maxBps": maximum})
    require_complete("分支加成", set(product(polarities.values(), elements.values())), branches)
    for (polarity, element), stats in branches.items():
        required = {focus["polarityId"][polarity], focus["elementId"][element]}
        require_complete(f"分支加成（阴阳 {polarity}，五行 {element}）的侧重属性", required, stats)
    config["bonusRules"] = rules


FORGE_EFFECTS = ("successBps", "returnBps", "rollBiasBps", "expBps")
FORGE_RELATIONS = {"same", "generates", "overcomes", "generatedBy", "overcomeBy", "neutral"}


def forge_delta(value, location):
    if not re.fullmatch(r"-?[0-9]+(?:\.0+)?", value):
        raise ValueError(f"{location} 工艺基点须为整数")
    delta = int(value.split(".")[0])
    if not -10000 <= delta <= 10000:
        raise ValueError(f"{location} 单项工艺修正超出 -10000～10000 基点")
    return delta


def add_forge_tables(config, sheets):
    counts = {}
    for location, row in sheets["词条数量"]:
        tier = whole_number(row["tierId"], location, "阶ID")
        count = whole_number(row["count"], location, "词条数")
        if tier not in range(1, 5) or tier in counts or row["tier"] != "黄玄地天"[tier-1]:
            raise ValueError(f"{location} 阶名、编号无效或重复")
        if not 1 <= count <= min(len(config["stats"]), len(FORGE_EFFECTS)):
            raise ValueError(f"{location} 词条数须为正整数，且不超过物品和工艺各自可选的种类数")
        counts[tier] = count
    require_complete("词条数量", set(range(1, 5)), counts)
    config["affixCounts"] = counts

    stats = {stat["id"]: stat["name"] for stat in config["stats"]}
    ranges = {"item": {}, "process": {}}
    expected = {"item": set(stats), "process": set(FORGE_EFFECTS)}
    effect_names = {}
    for location, row in sheets["词条区间"]:
        domain, stat = row["domain"], row["statId"]
        quality = whole_number(row["qualityId"], location, "品质ID")
        if stat not in expected.get(domain, set()) or quality not in range(1, 5):
            raise ValueError(f"{location} 用途域、词条键或品质ID无效")
        low = whole_number(row["low"], location, "下限基点")
        high = whole_number(row["high"], location, "上限基点")
        bounds = ranges[domain].setdefault(stat, {})
        if high < low or quality in bounds:
            raise ValueError(f"{location} 区间倒置或同词条、品质重复")
        bounds[quality] = [low, high]
        label = effect_names.setdefault(stat, row["name"]) if domain == "process" else stats[stat]
        if row["name"] != label:
            raise ValueError(f"{location} 词条键 {stat} 对应名称应为「{label}」")
    for domain, ids in expected.items():
        require_complete(f"词条区间 {domain}", ids, ranges[domain])
        for stat, bounds in ranges[domain].items():
            require_complete(f"词条区间 {stat}", set(range(1, 5)), bounds)
    config["affixRanges"], config["forgeEffectRanges"] = ranges["item"], ranges["process"]
    config["forgeEffectNames"] = effect_names

    templates = {item["id"]: item for item in config["itemTemplates"]}
    affixes = {}
    for location, row in sheets["物品词条"]:
        key, stat = row["itemId"], row["statId"]
        item = lookup(key, templates, location, "模板键")
        lookup(stat, stats, location, "词条键")
        quality = (item["gradeId"] - 1) % 4 + 1
        low, high = ranges["item"][stat][quality]
        value = whole_number(row["valueBps"], location, "固定基点")
        entries = affixes.setdefault(key, {})
        if stat in entries or not low <= value <= high:
            raise ValueError(f"{location} 词条重复或基点超出本品质区间 [{low}, {high}]")
        entries[stat] = value
    for key, item in templates.items():
        tier = (item["gradeId"] - 1) // 4 + 1
        if len(affixes.get(key, {})) != counts[tier]:
            raise ValueError(f"物品词条 {key} 应有 {counts[tier]} 条不同词条")
    config["itemAffixes"] = affixes

    grades = {g["id"]: g for g in config["grades"]}
    factors = {domain: {} for domain in ("smithRanks", "flames", "targetGrades", "elementRelations")}
    labels = {domain: {} for domain in factors}
    for location, row in sheets["炼器工艺"]:
        domain, key = row["domain"], row["id"]
        if domain not in factors or key in factors[domain]:
            raise ValueError(f"{location} 工艺域无效或对象键重复")
        values = {k: forge_delta(row[k], location) for k in FORGE_EFFECTS}
        difficulty = forge_delta(row["difficultyBps"], location)
        if domain == "elementRelations":
            if key not in FORGE_RELATIONS or row["gradeId"] is not None:
                raise ValueError(f"{location} 无效的生克关系或多余的品阶")
        else:
            grade = whole_number(row["gradeId"] or "", location, "品阶ID")
            lookup(grade, grades, location, "品阶ID")
            tier, quality = divmod(grade-1, 4)
            if sum(v != 0 for v in values.values()) != counts[tier+1]:
                raise ValueError(f"{location} 本阶工艺应有 {counts[tier+1]} 条非零词条")
            for stat, value in values.items():
                low, high = ranges["process"][stat][quality+1]
                if value and not low <= value <= high:
                    raise ValueError(f"{location} {stat} 应在本品质区间 [{low}, {high}]")
            if domain == "targetGrades" and key != str(grade):
                raise ValueError(f"{location} 目标对象键须与品阶ID相同")
            if domain == "smithRanks" and key not in {str(i) for i in range(1, 10)}:
                raise ValueError(f"{location} 炼器师对象键须为 1～9")
            values["gradeId"] = grade
        if domain == "flames":
            values["elementId"] = attribute_id(row["elementId"], ELEMENT_IDS, location, "五行")
            if values["elementId"] is None:
                raise ValueError(f"{location} 炉火须填五行")
        elif row["elementId"] is not None:
            raise ValueError(f"{location} 仅炉火可填五行")
        if domain == "targetGrades":
            values["difficultyBps"] = difficulty
        elif difficulty:
            raise ValueError(f"{location} 仅目标品阶可设置难度修正")
        factors[domain][key] = values
        labels[domain][key] = {"name": row["name"], "note": row["note"]}
    for domain, required in (("smithRanks", {str(i) for i in range(1, 10)}),
                             ("targetGrades", {str(g) for g in grades}), ("elementRelations", FORGE_RELATIONS)):
        require_complete(f"炼器工艺 {domain}", required, factors[domain])
    if not factors["flames"]:
        raise ValueError("炼器工艺至少须登记一种炉火")
    config["forgeFactors"], config["forgeFactorLabels"] = factors, labels


def build_config(sheets):
    config = {"schemaVersion": 2}
    indexes = {}
    for sheet, count in (("阴阳", 3), ("五行", 5), ("十二气", 12)):
        ids = set()
        names = {}
        records = []
        for location, source in sheets[sheet]:
            row = source.copy()
            if not re.fullmatch(r"[0-9]+(?:\.0+)?", row["id"]):
                raise ValueError(f"{location}「编号」应为整数")
            key = int(row["id"].split(".")[0])
            if not 1 <= key <= count or key in ids:
                raise ValueError(f"{location}「编号」{key} 重复或超出 1～{count}")
            name = row["name"]
            if name in names:
                raise ValueError(f"{location}「名称」{name} 重复")
            if sheet == "阴阳" and name != ("阴", "阳", "平和")[key - 1]:
                raise ValueError(f"{location} 的编号与阴阳约定不符")
            if sheet == "五行":
                if name != ELEMENTS[key - 1]:
                    raise ValueError(f"{location} 的编号与五行约定不符")
                for field, label, cycle in (("generates", "生", GENERATES), ("overcomes", "克", OVERCOMES)):
                    target = cycle[key - 1]
                    if attribute_id(row[field], ELEMENT_IDS, location, label) != ELEMENT_IDS[target]:
                        raise ValueError(f"{location}「{label}」应为「{target}」")
                    row[field] = ELEMENT_IDS[target]
            row["id"] = key
            ids.add(key)
            names[name] = key
            records.append(row)
        if len(records) != count:
            missing = sorted(set(range(1, count + 1)) - ids)
            raise ValueError(f"{sheet} 缺少编号 {missing}")
        config[SHEETS[sheet][0]] = sorted(records, key=lambda row: row["id"])
        indexes[sheet] = names

    registry = {"kind": {}, "system": {}}
    config["typeRegistry"] = []
    for location, row in sheets["类型与玩法"]:
        if row["domain"] not in registry:
            raise ValueError(f"{location}「用途域」须为 kind 或 system")
        entries = registry[row["domain"]]
        if row["id"] in entries:
            raise ValueError(f"{location}「稳定键」{row['id']} 在 {row['domain']} 域中重复")
        entries[row["id"]] = row
        config["typeRegistry"].append(row)

    profiles = {}
    for location, source in sheets["对象属性"]:
        row = source.copy()
        if row["id"] in profiles:
            raise ValueError(f"{location}「对象键」{row['id']} 重复")
        lookup(row["kindId"], registry["kind"], location, "类别键")
        if row["origin"] not in ("已有对象", "设计示例"):
            raise ValueError(f"{location}「来源状态」应为已有对象或设计示例")
        if (row["origin"] == "已有对象") != bool(row["sourceId"]):
            raise ValueError(f"{location}「原对象键」在已有对象时必填，在设计示例时应留空")
        row["polarityId"] = attribute_id(row["polarityId"], indexes["阴阳"], location, "阴阳")
        for primary, secondary, label, dictionary in (
            ("primaryElementId", "secondaryElementId", "五行", "五行"),
            ("primaryQiId", "secondaryQiId", "气", "十二气"),
        ):
            row[primary] = attribute_id(row[primary], indexes[dictionary], location, "主" + label)
            row[secondary] = attribute_id(row[secondary], indexes[dictionary], location, "辅" + label)
            if row[secondary] is not None and row[primary] in (None, row[secondary]):
                raise ValueError(f"{location} 填写「辅{label}」时须有不同的「主{label}」")
        polarity, element, qi = row["polarityId"], row["primaryElementId"], row["primaryQiId"]
        row["classId"] = None
        if polarity in (1, 2) and element is not None and qi is not None:
            row["classId"] = f"affinity.p{polarity}.e{element}.q{qi:02d}"
        profiles[row["id"]] = row
    config["profiles"] = list(profiles.values())
    config["usages"] = []
    for location, row in sheets["玩法用法"]:
        lookup(row["systemId"], registry["system"], location, "玩法键")
        lookup(row["profileId"], profiles, location, "关联对象键")
        config["usages"].append(row)
    add_bonus_tables(config, sheets, indexes)
    add_grades(config, sheets)
    add_method_influences(config, sheets, registry["system"])
    add_item_tables(config, sheets, profiles)
    add_forge_tables(config, sheets)
    regions, region_text = geography_tables.compile_geography(config, sheets)
    ecology, ecology_text = ecology_tables.compile_ecology(config, sheets, regions)
    for region_id, effects in regions["regionModifiers"].items():
        for targets in effects.values():
            for rule in targets.values():
                item = rule.get("exampleTemplateId")
                if item and ecology["resourceUseRoles"].get(item, {}).get("cultivation_field") not in ("seed", "propagule"):
                    raise ValueError(f"地域 {region_id} 的作物示例 {item} 须在「资源用途」登记为种子或可繁育植株")
    habitat, habitat_text = region_ecology.compile_affinity(sheets)
    config["worldRules"] = {**regions, **ecology, **region_ecology.source_indexes(ecology), "ecologyAffinity": habitat}
    config["worldText"] = {**region_text, **ecology_text, "ecologyAffinity": habitat_text}
    config["alchemyRules"], config["alchemyText"] = alchemy_tables.compile_alchemy(config, sheets, config["worldRules"])
    if "符方表" in sheets:
        config["inscriptionRules"], config["inscriptionText"] = inscription_tables.compile_inscription(config, sheets)
    if "创作门类" in sheets:
        bestiary, bestiary_text = bestiary_tables.compile_bestiary(sheets, ecology, regions)
        config["worldRules"].update(bestiary)
        config["worldText"].update(bestiary_text)
        config["methodRules"], config["methodText"] = cultivation_methods.compile_methods(config, sheets)
        config["worldSystemRules"], config["worldSystemText"] = world_systems.compile_world(config, sheets)
        config["monsterStatRules"], config["monsterStatText"] = monster_stats.compile_stats(config, sheets)
        config["characterRules"], config["characterText"] = character_tables.compile_characters(config, sheets)
        config["projectionRules"], config["projectionText"] = history_projection.compile_projection(config, sheets)
        config["creationSheets"] = sheets
    return config


def write_pair(outputs):
    # 两份文件先在各自的目录落盘，再发布中文包，最后切换运行包。
    def stage(path, data, cleanup):
        with NamedTemporaryFile(dir=path.parent, delete=False) as stream:
            pending = Path(stream.name)
            cleanup.callback(pending.unlink, missing_ok=True)
            stream.write(data)
        pending.chmod(path.stat().st_mode if path.exists() else 0o644)
        return pending

    (locale_path, locale_data), (runtime_path, runtime_data) = outputs
    with ExitStack() as cleanup:
        locale = stage(locale_path, locale_data, cleanup)
        runtime = stage(runtime_path, runtime_data, cleanup)
        backup = stage(locale_path, locale_path.read_bytes(), cleanup) if locale_path.exists() else None
        locale.replace(locale_path)
        try:
            runtime.replace(runtime_path)
        except OSError:
            if backup is None:
                locale_path.unlink(missing_ok=True)
            else:
                backup.replace(locale_path)
            raise


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--workbook", type=Path, default=DATA / "config_excel/修仙属性基础表.xlsx")
    parser.add_argument("--output", type=Path, default=DATA / "config/cultivation_affinities.json")
    parser.add_argument("--locale-output", type=Path, help="简体中文展示资源路径，默认与运行配置同目录，后缀 .zh-CN.json")
    parser.add_argument("--check", action="store_true", help="核对运行配置与简体中文资源，发现差异时返回非零")
    args = parser.parse_args()
    try:
        locale_path = args.locale_output or args.output.with_name(args.output.stem + ".zh-CN.json")
        if len({path.resolve() for path in (args.workbook, args.output, locale_path)}) != 3:
            raise ValueError("工作簿、运行配置、简体中文资源须使用三个不同的文件路径")
        runtime, locale = compile_catalog(build_config(read_sheets(args.workbook)))
        runtime_data = (json.dumps(runtime, ensure_ascii=False, separators=(",", ":")) + "\n").encode("ascii")
        locale_data = (json.dumps(locale, ensure_ascii=False, indent=2) + "\n").encode("utf-8")
        outputs = ((locale_path, locale_data), (args.output, runtime_data))
        if args.check:
            differences = [str(path) for path, data in outputs if not path.is_file() or path.read_bytes() != data]
            if differences:
                raise ValueError(f"{'、'.join(differences)} 与基础表不同，请重新导出")
            print(f"基础表与 {args.output}、{locale_path} 一致")
        else:
            write_pair(outputs)
            print(f"已导出 {args.output}、{locale_path}")
    except (OSError, BadZipFile, ET.ParseError, KeyError, ValueError) as error:
        print(error, file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    sys.exit(main())
