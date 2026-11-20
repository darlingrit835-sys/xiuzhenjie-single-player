"""跨门类词条池、品阶预算、模板继承和整数结算。固定词条与生成词条互斥。"""

from collections import defaultdict
import random
import re

if __package__:
    from .crafting_tables import limited_value, write_row
else:
    from crafting_tables import limited_value, write_row


SHEETS = {
    "内容词条机制": ("effects", "词条ID 作用键 名称 区间参照键 算法ID 叠加件数上限 总基点上限 计量依据",
               "id key name rangeStatId opId maxSources capBps basis"),
    "内容词条组": ("groups", "组ID 名称 适用说明", "id name note"),
    "词条组候选": ("members", "组ID 词条ID 抽取权重", "groupId effectId weight"),
    "门类词条规则": ("kinds", "门类ID 组ID 默认组 默认品阶ID", "kindId groupId isDefault gradeId"),
    "物品分类词条组": ("categories", "分类键 组ID", "categoryId groupId"),
    "模板词条特例": ("overrides", "门类ID 模板ID 组ID 品阶ID 说明", "kindId templateId groupId gradeId note"),
}
OPTIONAL = {}
MAX_ID = 2**31 - 1


def get(table, key):
    """支持内存中整数键和 JSON 中等价的十进制对象键。"""
    if key in table:
        return table[key]
    if isinstance(key, int):
        return table.get(str(key))
    if isinstance(key, str) and key.isdecimal():
        return table.get(int(key))
    return None


def num(row, key, loc, low=1, high=MAX_ID):
    return limited_value(row, key, loc, low, high)


def known(key, table, loc, label):
    if get(table, key) is None:
        raise ValueError(f"{loc} 未登记{label} {key}")
    return key


def source_record(catalog, kind, record):
    path = kind["catalogPath"]
    if path == "itemTemplates":
        return catalog["itemTemplates"][record["key"]]
    if path == "methods.templates":
        return get(catalog["methods"]["templates"], record["key"])
    source = catalog
    for part in path.split("."):
        source = source[part]
    return get(source, record["key"])


def grade_and_affinity(catalog, source):
    for field in ("outputId", "blankId"):
        if source.get(field) in catalog["itemTemplates"]:
            source = catalog["itemTemplates"][source[field]]
            break
    profile = catalog["profiles"].get(source.get("profileId")) or source
    if "regionId" in source and "waveId" in source:
        profile = get(catalog["regions"], source["regionId"])
    return source.get("gradeId"), {
        "polarityId": profile.get("polarityId"),
        "elementId": profile.get("primaryElementId", profile.get("elementId")),
        "qiId": profile.get("primaryQiId", profile.get("qiId")),
    }


def compile_affix_catalog(runtime, sheets):
    registry = runtime["creation"]
    effects, groups, kinds, categories, overrides, codes = {}, {}, {}, {}, {}, {}
    labels = {"effects": {}, "groups": {}, "overrides": {}}
    for loc, row in sheets["内容词条机制"]:
        eid = num(row, "id", loc)
        code = row["key"]
        if not re.fullmatch(r"[a-z][a-z0-9_]*", code) or code in codes:
            raise ValueError(f"{loc} 作用键须为唯一小写 ASCII 程序键")
        curve = known(row["rangeStatId"], runtime["affixRanges"], loc, "品质区间")
        cap = num(row, "capBps", loc, 1, 10000)
        if any(not 0 <= bounds[0] <= bounds[1] <= cap for bounds in runtime["affixRanges"][curve].values()):
            raise ValueError(f"{loc} 叠加上限不足以容纳一次品质区间")
        codes[code] = eid
        write_row(effects, eid, {"key": code, "rangeStatId": curve, "opId": num(row, "opId", loc, 1, 4),
                                 "maxSources": num(row, "maxSources", loc, 1, 64), "capBps": cap}, loc)
        labels["effects"][eid] = {"name": row["name"], "basis": row["basis"]}
    for loc, row in sheets["内容词条组"]:
        group = num(row, "id", loc)
        write_row(groups, group, {}, loc)
        labels["groups"][group] = {"name": row["name"], "note": row["note"]}
    for loc, row in sheets["词条组候选"]:
        group = known(num(row, "groupId", loc), groups, loc, "词条组")
        eid = known(num(row, "effectId", loc), effects, loc, "词条")
        write_row(groups[group], eid, num(row, "weight", loc), loc)
    budget = max(runtime["affixCounts"].values())
    for group, pool in groups.items():
        if len(pool) < budget:
            raise ValueError(f"词条组 {group} 只有 {len(pool)} 个候选，少于最高品阶的 {budget} 种")

    for loc, row in sheets["门类词条规则"]:
        kind = known(num(row, "kindId", loc), registry["kinds"], loc, "门类")
        group = known(num(row, "groupId", loc), groups, loc, "词条组")
        entry = kinds.setdefault(kind, {"allowedGroups": []})
        if group in entry["allowedGroups"]:
            raise ValueError(f"{loc} 重复登记门类词条组 {kind}/{group}")
        entry["allowedGroups"].append(group)
        grade = num(row, "gradeId", loc, 0)
        if num(row, "isDefault", loc, 0, 1):
            if "defaultGroupId" in entry:
                raise ValueError(f"{loc} 门类只能有一个默认词条组")
            entry.update(defaultGroupId=group, defaultGradeId=known(grade, runtime["grades"], loc, "常规品阶"))
        elif grade != 0:
            raise ValueError(f"{loc} 非默认组的默认品阶应为 0")
    for kind, rule in kinds.items():
        if "defaultGroupId" not in rule:
            raise ValueError(f"门类 {kind} 未指定默认词条组")
        rule["allowedGroups"].sort()

    by_path = {info["catalogPath"]: int(k) for k, info in registry["kinds"].items()}
    for path in ("itemTemplates", "monsterSpecies", "methods.templates", "characters.templates"):
        if by_path.get(path) not in kinds:
            raise ValueError(f"核心目录 {path} 缺少词条规则")
    item_rule = kinds[by_path["itemTemplates"]]
    for loc, row in sheets["物品分类词条组"]:
        cat = known(row["categoryId"], runtime["itemCategories"], loc, "物品分类")
        group = num(row, "groupId", loc)
        if group not in item_rule["allowedGroups"]:
            raise ValueError(f"{loc} 分类指定的词条组不可用于物品")
        write_row(categories, cat, group, loc)
    for loc, row in sheets["模板词条特例"]:
        kind = known(num(row, "kindId", loc), kinds, loc, "有词条规则的门类")
        tid = known(num(row, "templateId", loc), get(registry["templates"], kind), loc, "模板")
        group, grade = num(row, "groupId", loc, 0), num(row, "gradeId", loc, 0)
        if not (group or grade) or (group and group not in kinds[kind]["allowedGroups"]):
            raise ValueError(f"{loc} 特例应覆盖品阶或本门类允许的词条组")
        baseline = source_record(runtime, get(registry["kinds"], kind), get(get(registry["templates"], kind), tid))
        baseline_grade, _ = grade_and_affinity(runtime, baseline)
        if grade and (get(runtime["grades"], grade) is None or baseline_grade is not None):
            raise ValueError(f"{loc} 特例只能为没有原品阶的对象指定常规品阶")
        write_row(overrides.setdefault(kind, {}), tid, [group, grade], loc)
        labels["overrides"].setdefault(kind, {})[tid] = {"note": row["note"]}

    # 已有固定值只转换键。区间、计数仍归属于原总表；发行新机制前一起验证。
    fixture_sets = [*runtime["itemAffixes"].values(), *(m["affixes"] for m in runtime["methods"]["templates"].values())]
    for stat in {stat for fixture in fixture_sets for stat in fixture}:
        if stat not in codes or effects[codes[stat]]["rangeStatId"] != stat:
            raise ValueError(f"已有固有词条 {stat} 需要同名作用键及原品质区间")
    rules = {"effects": effects, "idByKey": codes, "groups": groups, "kinds": kinds,
             "itemCategories": categories, "overrides": overrides}
    return rules, labels


def describe(catalog, kind_id, template_id):
    """按两级数字主键读取模板、实际品阶和词条组。无需为每一件物品复制词条组。"""
    rules = catalog["contentAffixes"]
    kind = get(catalog["creation"]["kinds"], kind_id)
    policy = get(rules["kinds"], kind_id)
    if not kind or not policy:
        raise ValueError("门类未注册词条规则")
    record = get(get(catalog["creation"]["templates"], kind_id), template_id)
    if record is None:
        raise ValueError("未登记的内置模板")
    source = source_record(catalog, kind, record)
    grade, affinity = grade_and_affinity(catalog, source)
    grade, group = grade or policy["defaultGradeId"], policy["defaultGroupId"]
    path = kind["catalogPath"]
    fixed = None
    if path == "itemTemplates":
        fixed = catalog["itemAffixes"][record["key"]]
        category = source["categoryId"]
        while category:
            if category in rules["itemCategories"]:
                group = rules["itemCategories"][category]
                break
            category = catalog["itemCategories"][category]["parentId"]
    elif path == "methods.templates":
        fixed = source["affixes"]
    override = get(get(rules["overrides"], kind_id) or {}, template_id)
    if override:
        group, grade = override[0] or group, override[1] or grade
    return {"groupId": group, "gradeId": grade, "fixed": fixed, "affinity": affinity}


def weighted_candidates(catalog, group, affinity):
    rules = catalog["contentAffixes"]
    members = get(rules["groups"], group)
    bonuses = defaultdict(int)
    if affinity:
        for field, table in (("polarityId", "polarities"), ("elementId", "elements"), ("qiId", "qiTypes")):
            if affinity.get(field) is not None and get(catalog[table], affinity[field]) is None:
                raise ValueError("词条偏性引用了未知阴阳、五行或气")
        for prefix, field in (("polarity", "polarityId"), ("element", "elementId")):
            focus = get(catalog[prefix + "Focus"], affinity.get(field))
            weights = get(catalog[prefix + "QiWeights"], affinity.get(field))
            rate = get(weights, affinity.get("qiId")) if weights else None
            if focus and rate:
                bonuses[focus] += rate
    return sorted((int(eid), w * (10 + bonuses[get(rules["effects"], eid)["key"]])) for eid, w in members.items())


def draw(catalog, group, grade, seed, affinity=None):
    if type(seed) is not int or not 0 <= seed < 2**63:
        raise ValueError("种子必须为非负的 63 位整数")
    rules = catalog["contentAffixes"]
    grade_info = get(catalog["grades"], grade)
    if not grade_info:
        raise ValueError("新抽取须选常规天地玄黄、上中下品阶")
    count = get(catalog["affixCounts"], grade_info["tierId"])
    quality = grade_info["qualityId"]
    rng = random.Random(seed)
    candidates = weighted_candidates(catalog, group, affinity)
    affixes = {}
    for _ in range(count):
        ticket = rng.randrange(sum(w for _, w in candidates))
        for index, (eid, weight) in enumerate(candidates):
            if ticket < weight:
                candidates.pop(index)
                break
            ticket -= weight
        curve = get(rules["effects"], eid)["rangeStatId"]
        low, high = get(catalog["affixRanges"][curve], quality)
        affixes[eid] = rng.randint(low, high)
    return {"groupId": group, "gradeId": grade, "affixes": affixes}


def roll_affixes(catalog, kind_id, template_id, seed):
    """已有物品与功法返回其唯一固有快照；无固有值的怪、人物等按种子生成初始快照。"""
    rule = describe(catalog, kind_id, template_id)
    if rule["fixed"] is None:
        return draw(catalog, rule["groupId"], rule["gradeId"], seed, rule["affinity"])
    return {"groupId": rule["groupId"], "gradeId": rule["gradeId"],
            "affixes": {catalog["contentAffixes"]["idByKey"][key]: v for key, v in rule["fixed"].items()}}


def roll_created(catalog, kind_id, grade_id, seed, *, prototype_id=None, group_id=None, inherited_group_id=None, affinity=None):
    """创作一份新版本。已发布玩家原型可由服务端解析出其继承组，再交给此纯函数。"""
    kind = get(catalog["creation"]["kinds"], kind_id)
    policy = get(catalog["contentAffixes"]["kinds"], kind_id)
    if not kind or not policy or not kind["creatable"]:
        raise ValueError("本门类未开放创作")
    if prototype_id is not None and inherited_group_id is not None:
        raise ValueError("内置原型和已发布玩家原型只能选一个")
    if kind["needsPrototype"] and prototype_id is None and inherited_group_id is None:
        raise ValueError("此门类需要指定原型")
    if inherited_group_id is not None and inherited_group_id not in policy["allowedGroups"]:
        raise ValueError("玩家原型的词条组不适用于本门类")
    base = inherited_group_id if inherited_group_id is not None else policy["defaultGroupId"]
    if prototype_id is not None:
        prototype = describe(catalog, kind_id, prototype_id)
        base = prototype["groupId"]
        if affinity is None:
            affinity = prototype["affinity"]
    chosen = base if group_id is None else group_id
    if chosen not in policy["allowedGroups"]:
        raise ValueError("指定词条组不适用于本门类")
    return draw(catalog, chosen, grade_id, seed, affinity)


def stack_affixes(catalog, sources):
    """每件来源以唯一的库存实例/状态快照标识为键。返回各作用通道的合计整数基点。"""
    rules = catalog["contentAffixes"]
    values = defaultdict(list)
    for snapshot in sources.values():
        grade_id = snapshot.get("gradeId")
        grade = get(catalog["grades"], grade_id) or get(catalog["legacyGrades"], grade_id)
        affixes = snapshot.get("affixes")
        if (not grade or get(rules["groups"], snapshot.get("groupId")) is None or not isinstance(affixes, dict) or
                len(affixes) != get(catalog["affixCounts"], grade["tierId"])):
            raise ValueError("来源快照的品阶与词条数量不符")
        seen = set()
        for raw_id, bps in affixes.items():
            if type(raw_id) is not int and (not isinstance(raw_id, str) or not re.fullmatch(r"[1-9][0-9]*", raw_id)):
                raise ValueError("无效词条编号")
            eid = int(raw_id)
            effect = get(rules["effects"], eid)
            if (not effect or eid in seen or type(bps) is not int or not 0 <= bps <= effect["capBps"]):
                raise ValueError("词条编号重复或基点超出预算")
            low, high = get(catalog["affixRanges"][effect["rangeStatId"]], grade["qualityId"])
            if not low <= bps <= high:
                raise ValueError("来源词条超出本品质区间")
            seen.add(eid)
            values[eid].append(bps)
    result = {}
    for eid, amounts in values.items():
        effect = get(rules["effects"], eid)
        result[effect["key"]] = min(effect["capBps"], sum(sorted(amounts, reverse=True)[:effect["maxSources"]]))
    return result


def apply_value(catalog, channel, base, stacked, *, ceiling=None, floor=0, ticket=None):
    """对调用方给出的单一基础量结算。产量的小数需使用服务端该批专属的 0～9999 抽签值。"""
    eid = catalog["contentAffixes"]["idByKey"].get(channel)
    if eid is None or type(base) is not int or base < 0:
        raise ValueError("通道无效或基础量须为非负整数")
    rule = get(catalog["contentAffixes"]["effects"], eid)
    bps = stacked.get(channel, 0)
    if type(bps) is not int or not 0 <= bps <= rule["capBps"]:
        raise ValueError("须使用已封顶的词条基点")
    if type(floor) is not int or floor < 0 or (ceiling is not None and (type(ceiling) is not int or ceiling < floor)):
        raise ValueError("结算上下限应为非负整数且顺序有效")
    op = rule["opId"]
    if op == 1:  # 点数、效力按比例增幅，向下取整。
        value = base * (10000 + bps) // 10000
    elif op == 2:  # 本次必付的资源/时间向上取整，不把一份必需投入抹掉。
        value = max(1 if base else 0, (base * (10000 - bps) + 9999) // 10000)
    elif op == 3:  # 成功率等绝对概率基点；原系统仍可指定更低上限。
        if base > 10000 or floor > 10000:
            raise ValueError("基础概率或概率下限超出10000基点")
        if base == 0:
            return 0
        value = min(10000, base + bps)
    else:  # 产量向下取整，余数作为多出一件的概率，数量为零时仍然为零。
        whole, remainder = divmod(base * (10000 + bps), 10000)
        if remainder and (type(ticket) is not int or not 0 <= ticket < 10000):
            raise ValueError("整数产量含小数余量时需要0至9999的批次抽签值")
        value = whole + int(bool(remainder) and ticket < remainder)
    if op == 2 and base and ceiling == 0:
        raise ValueError("必需投入数量的上限不能为零")
    if base == 0:
        return 0
    value = max(value, floor)
    return min(value, ceiling) if ceiling is not None else value


def main():
    import argparse
    import json
    from pathlib import Path

    parser = argparse.ArgumentParser(description="按内容门类、模板和种子预览最终词条")
    parser.add_argument("--kind", type=int, required=True, help="创作门类ID")
    parser.add_argument("--template", type=int, help="内置模板ID，创作时可选作原型")
    parser.add_argument("--seed", type=int, default=0)
    parser.add_argument("--create-grade", type=int, help="以该常规品阶生成新的创作快照")
    parser.add_argument("--group", type=int, help="创作时选本门类允许的组")
    parser.add_argument("--channel", help="要结算的作用键，例如 batch_yield、material_cost")
    parser.add_argument("--base", type=int, help="该通道的基础数量或基点")
    parser.add_argument("--ticket", type=int, help="产量余数抽签值 0～9999")
    parser.add_argument("--ceiling", type=int, help="玩法最终上限，例如实际投入的符纸张数")
    parser.add_argument("--floor", type=int, default=0, help="玩法最终下限，例如挂牌费下限")
    parser.add_argument("--json", action="store_true")
    opts = parser.parse_args()
    root = Path(__file__).resolve().parents[1] / "server/data/config"
    cat, locale = [json.loads((root / f"cultivation_affinities{s}.json").read_text(encoding="utf-8"))
                   for s in ("", ".zh-CN")]
    if cat["catalogVersion"] != locale["catalogVersion"]:
        parser.error("运行包与中文包版本不一致")
    try:
        if opts.create_grade is not None:
            result = roll_created(cat, opts.kind, opts.create_grade, opts.seed,
                                  prototype_id=opts.template, group_id=opts.group)
        elif opts.template is None or opts.group is not None:
            parser.error("普通预览需要模板；选组需同时指定创作品阶")
        else:
            result = roll_affixes(cat, opts.kind, opts.template, opts.seed)
        if (opts.base is None) != (opts.channel is None):
            parser.error("--base 和 --channel 需要一起提供")
        if opts.channel is not None:
            total = stack_affixes(cat, {"preview": result})
            applied = apply_value(cat, opts.channel, opts.base, total, ticket=opts.ticket,
                                  ceiling=opts.ceiling, floor=opts.floor)
            result["settlement"] = {"channel": opts.channel, "base": opts.base, "bps": total.get(opts.channel, 0),
                                    "value": applied}
    except ValueError as error:
        parser.error(str(error))
    if opts.json:
        print(json.dumps(result, ensure_ascii=False, indent=2))
        return
    texts = locale["contentAffixes"]
    grade = get(locale["grades"], result["gradeId"]) or get(locale["legacyGrades"], result["gradeId"])
    print(f"{grade['name']} · {get(texts['groups'], result['groupId'])['name']}")
    for eid, value in result["affixes"].items():
        effect = get(cat["contentAffixes"]["effects"], eid)
        text = get(texts["effects"], eid)
        symbol = "−" if effect["opId"] == 2 else "+"
        unit = "个百分点" if effect["opId"] == 3 else "%"
        print(f"{text['name']}　{symbol}{value/100:.2f}{unit}　{text['basis']}")
    if "settlement" in result:
        line = result["settlement"]
        print(f"本次结算　{line['channel']}　基础 {line['base']} → {line['value']}")


if __name__ == "__main__":
    main()
