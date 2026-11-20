"""根据阴阳、五行与十二气配置，以固定种子抽取类别并生成属性加成。"""

import argparse
import json
from itertools import product
from pathlib import Path
import random
from tempfile import NamedTemporaryFile


CONFIG = Path(__file__).resolve().parents[1] / "server/data/config/cultivation_affinities.json"
POLARITIES = (1, 2)
ELEMENTS = range(1, 6)
QI_TYPES = range(1, 13)


def records(config, table, fields):
    source = config.get(table)
    if not isinstance(source, list):
        raise ValueError(f"缺少配置数组 {table}")
    for number, row in enumerate(source, 1):
        label = f"{table} 第 {number} 条"
        if not isinstance(row, dict) or not all(field in row for field in fields):
            raise ValueError(f"{label} 缺少字段，须包含 {', '.join(fields)}")
        yield label, row


def indexed_map(value, label):
    if not isinstance(value, dict):
        raise ValueError(f"{label} 应为按键索引的对象")
    return value


def numbered_entries(value, label):
    for key, row in indexed_map(value, label).items():
        if not isinstance(key, str) or not key.isascii() or not key.isdecimal() or str(int(key)) != key:
            raise ValueError(f"{label} 包含无效编号 {key}")
        yield int(key), row


def names(config, table, required_ids, locale):
    if config.get("schemaVersion") == 3:
        definitions = indexed_map(config.get(table), table)
        labels = indexed_map(locale.get(table), table) if locale is not None else None
        result = {}
        for key in required_ids:
            indexed_map(definitions.get(str(key)), f"{table}.{key}")
            if labels is not None:
                name = indexed_map(labels.get(str(key)), f"语言包 {table}.{key}").get("name")
                if not isinstance(name, str) or not name.strip():
                    raise ValueError(f"语言包 {table}.{key} 缺少名称")
                result[key] = name
        return result
    result = {}
    for label, row in records(config, table, ("id", "name")):
        key, name = row["id"], row["name"]
        if type(key) is not int or key in result or not isinstance(name, str) or not name.strip():
            raise ValueError(f"{label} 的编号或名称无效")
        result[key] = name
    missing = set(required_ids) - result.keys()
    if missing:
        raise ValueError(f"{table} 缺少编号 {sorted(missing)}")
    return result


def qi_weights(config, table, field, allowed_ids):
    result = {}
    if config.get("schemaVersion") == 3:
        weights = ((f"{table}.{dim}.{qi}", (dim, qi), weight)
                   for dim, group in numbered_entries(config.get(table), table)
                   for qi, weight in numbered_entries(group, f"{table}.{dim}"))
    else:
        weights = ((label, (row[field], row["qiId"]), row["weight"])
                   for label, row in records(config, table, (field, "qiId", "weight")))
    for label, key, weight in weights:
        if any(type(value) is not int for value in key) or key[0] not in allowed_ids or key[1] not in QI_TYPES:
            raise ValueError(f"{label} 的 {field} 或 qiId 无效")
        if key in result or type(weight) is not int or weight <= 0:
            raise ValueError(f"{label} 组合 {key} 重复或权重不是正整数")
        result[key] = weight
    missing = set(product(allowed_ids, QI_TYPES)) - result.keys()
    if missing:
        raise ValueError(f"{table} 缺少权重 {sorted(missing)}")
    return result


def bonus_entries(config):
    if config.get("schemaVersion") == 3:
        for polarity, elements in numbered_entries(config.get("bonusRules"), "bonusRules"):
            for element, stats in numbered_entries(elements, f"bonusRules.{polarity}"):
                label = f"bonusRules.{polarity}.{element}"
                for stat, bounds in indexed_map(stats, label).items():
                    if not isinstance(bounds, list) or len(bounds) != 2:
                        raise ValueError(f"{label}.{stat} 需要两个加成边界")
                    yield label, (polarity, element), stat, bounds[0], bounds[1]
        return
    for label, row in records(config, "bonusRules", ("polarityId", "elementId", "statId", "minBps", "maxBps")):
        yield label, (row["polarityId"], row["elementId"]), row["statId"], row["minBps"], row["maxBps"]


def bonus_rules(config):
    result = {pair: {} for pair in product(POLARITIES, ELEMENTS)}
    for label, pair, stat, low, high in bonus_entries(config):
        if any(type(value) is not int for value in pair) or pair not in result:
            raise ValueError(f"{label} 的 polarityId 或 elementId 无效")
        if not isinstance(stat, str) or not stat.strip() or stat in result[pair]:
            raise ValueError(f"{label} 的属性键无效或重复")
        if type(low) is not int or type(high) is not int or not 0 <= low <= high:
            raise ValueError(f"{label} 的加成范围须为非负整数基点，且下限不能超过上限")
        result[pair][stat] = (low, high)
    for pair, rules in result.items():
        if not rules:
            raise ValueError(f"bonusRules 缺少阴阳与五行组合 {pair} 的加成规则")
    return {pair: sorted(rules.items()) for pair, rules in result.items()}


def find_grade(config, grade_id):
    if type(grade_id) is not int or grade_id < 1:
        raise ValueError(f"无效的品阶编号 {grade_id}")
    if config.get("schemaVersion") == 3:
        key = str(grade_id)
        is_legacy = key in indexed_map(config.get("legacyGrades"), "legacyGrades")
        grade = indexed_map(config.get("grades"), "grades").get(key)
    else:
        is_legacy = any(row["id"] == grade_id for _, row in records(config, "legacyGrades", ("id",)))
        matches = [row for _, row in records(config, "grades", ("id", "multiplierBps"))
                   if type(row["id"]) is int and row["id"] == grade_id]
        grade = matches[0] if len(matches) == 1 else None
    if is_legacy:
        raise ValueError(f"品阶编号 {grade_id} 属于旧版极品，不能用于新创制")
    if not isinstance(grade, dict):
        raise ValueError(f"无效的品阶编号 {grade_id}，grades 中缺少或重复登记")
    multiplier = grade.get("multiplierBps")
    if type(multiplier) is not int or multiplier <= 0:
        raise ValueError(f"grades 品阶 {grade_id} 的倍率须为正整数基点")
    return grade


def quality_affix_rules(config, grade, branches):
    tier, quality = grade.get("tierId"), grade.get("qualityId")
    if type(tier) is not int or tier not in (1, 2, 3, 4) or type(quality) is not int or quality not in (1, 2, 3):
        raise ValueError("现行品阶须指定合法的 tierId 与 qualityId")
    counts = indexed_map(config.get("affixCounts"), "affixCounts")
    stats = indexed_map(config.get("stats"), "stats")
    for key in ("1", "2", "3", "4"):
        n = counts.get(key)
        if type(n) is not int or not 1 <= n <= min(4, len(stats)):
            raise ValueError(f"affixCounts 无法为第 {key} 阶提供正确数量的不同词条")
    count = counts[str(tier)]
    ranges = indexed_map(config.get("affixRanges"), "affixRanges")
    quality_ranges, base_ranges = {}, {}
    for stat in stats:
        group = indexed_map(ranges.get(stat), f"affixRanges.{stat}")
        for q, dest in ((quality, quality_ranges), (1, base_ranges)):
            bounds = group.get(str(q))
            if (not isinstance(bounds, list) or len(bounds) != 2 or
                    any(type(value) is not int for value in bounds) or not 0 <= bounds[0] <= bounds[1]):
                raise ValueError(f"affixRanges.{stat}.{q} 需要非负整数上下界")
            dest[stat] = tuple(bounds)
    element_focus = indexed_map(config.get("elementFocus"), "elementFocus")
    polarity_focus = indexed_map(config.get("polarityFocus"), "polarityFocus")
    pools = {}
    for (p, e), branch in branches.items():
        preferred = [element_focus.get(str(e)), polarity_focus.get(str(p))]
        if any(not isinstance(stat, str) or stat not in stats for stat in preferred) or any(stat not in stats for stat, _ in branch):
            raise ValueError(f"阴阳 {p}、五行 {e} 的侧重或分支引用了未登记词条")
        first = list(dict.fromkeys(preferred))[:count]
        extra = sorted(stat for stat, _ in branch if stat not in first)
        others = sorted(stats.keys() - set(first) - set(extra))
        pools[p, e] = (first, extra, others, dict(branch))
    return count, quality_ranges, base_ranges, pools


def roll_graded_affixes(spec, pair, roll):
    count, quality_ranges, base_ranges, pools = spec
    first, extra, others, branch = pools[pair]
    selected = list(first)
    for pool in (extra, others):
        selected.extend(roll.sample(pool, min(count - len(selected), len(pool))))
    bonuses = []
    for stat in selected:
        percentile = roll.randrange(10001)
        low, high = quality_ranges[stat]
        base_low, base_high = branch.get(stat, base_ranges[stat])
        bonuses.append({"statId": stat,
                        "valueBps": low + ((high - low) * percentile + 5000) // 10000,
                        "baseValueBps": base_low + ((base_high - base_low) * percentile + 5000) // 10000})
    return bonuses


def generate_classes(config, seed, count=48, grade_id=None, locale=None):
    """返回按抽取次序排列的类别。加成单位为基点，100 表示 1%。"""
    if not isinstance(config, dict):
        raise ValueError("配置根节点应为对象")
    if locale is not None:
        if (not isinstance(locale, dict) or not config.get("catalogVersion") or
                locale.get("catalogVersion") != config["catalogVersion"]):
            raise ValueError("配置与语言包的 catalogVersion 不匹配")
    max_count = len(POLARITIES) * len(ELEMENTS) * len(QI_TYPES)
    if type(count) is not int or not 1 <= count <= max_count:
        raise ValueError(f"抽取数量应为 1～{max_count} 的整数")
    if seed is None or not str(seed).strip():
        raise ValueError("种子不能为空")
    polarity_names = names(config, "polarities", POLARITIES, locale)
    element_names = names(config, "elements", ELEMENTS, locale)
    qi_names = names(config, "qiTypes", QI_TYPES, locale)
    polarity_weights = qi_weights(config, "polarityQiWeights", "polarityId", POLARITIES)
    element_weights = qi_weights(config, "elementQiWeights", "elementId", ELEMENTS)
    rules = bonus_rules(config)
    grade = find_grade(config, grade_id) if grade_id is not None else None
    affix_spec = None
    if grade is not None and ("affixCounts" in config or "affixRanges" in config):
        affix_spec = quality_affix_rules(config, grade, rules)

    # 分开抽类别和加成，调整加成范围时可沿用同一批类别。
    draw = random.Random(f"classes:{seed}")
    roll = random.Random(f"bonuses:{seed}")
    pool = [(polarity, element, qi, polarity_weights[polarity, qi] * element_weights[element, qi])
            for polarity, element, qi in product(POLARITIES, ELEMENTS, QI_TYPES)]
    remaining_weight = sum(entry[3] for entry in pool)
    chosen = []
    for _ in range(count):
        ticket = draw.randrange(remaining_weight)
        for index, (polarity, element, qi, weight) in enumerate(pool):
            if ticket < weight:
                break
            ticket -= weight
        pool.pop(index)
        remaining_weight -= weight
        entry = {"id": f"affinity.p{polarity}.e{element}.q{qi:02d}"}
        if locale is not None and config.get("schemaVersion") == 3:
            pattern = locale.get("classNameFormat", "{polarity}{element}·{qi}")
            if not isinstance(pattern, str):
                raise ValueError("语言包的 classNameFormat 须为文本")
            try:
                entry["name"] = pattern.format(polarity=polarity_names[polarity], element=element_names[element], qi=qi_names[qi])
            except (AttributeError, IndexError, KeyError, ValueError) as error:
                raise ValueError("语言包的 classNameFormat 无效") from error
        elif qi_names:
            qi_name = qi_names[qi]
            if not qi_name.endswith("气"):
                qi_name += "气"
            entry["name"] = f"{polarity_names[polarity]}{element_names[element]}·{qi_name}"
        entry.update({
            "polarityId": polarity,
            "elementId": element,
            "qiId": qi,
        })
        if affix_spec is not None:
            entry["bonuses"] = roll_graded_affixes(affix_spec, (polarity, element), roll)
        else:
            entry["bonuses"] = [{"statId": stat, "valueBps": roll.randint(low, high)}
                                for stat, (low, high) in rules[polarity, element]]
        if grade is not None:
            entry["gradeId"] = grade_id
        if grade is not None and affix_spec is None:
            for bonus in entry["bonuses"]:
                base = bonus["valueBps"]
                bonus["baseValueBps"] = base
                bonus["valueBps"] = (base * grade["multiplierBps"] + 5000) // 10000
        chosen.append(entry)
    return chosen


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--seed", required=True, help="随机种子，可用数字或文字")
    parser.add_argument("--count", type=int, default=48, help="从 120 类中抽取的数量，默认 48")
    parser.add_argument("--grade-id", type=int, help="以指定阶的词条数和指定品的数值区间生成加成，同时保留基准点数")
    parser.add_argument("--config", type=Path, default=CONFIG, help="策划表导出的 JSON 路径")
    parser.add_argument("--locale", type=Path, help="可选语言包，默认读取配置同目录的同名 .zh-CN.json")
    parser.add_argument("--output", type=Path, help="输出 JSON 文件，省略时打印到标准输出")
    args = parser.parse_args()
    try:
        config = json.loads(args.config.read_text(encoding="utf-8-sig"))
        locale_path = args.locale or args.config.with_name(args.config.stem + ".zh-CN.json")
        locale = None
        if args.locale is not None or locale_path.exists():
            locale = indexed_map(json.loads(locale_path.read_text(encoding="utf-8-sig")), "语言包")
        result = generate_classes(config, args.seed, args.count, args.grade_id, locale)
        output = json.dumps(result, ensure_ascii=False, indent=2)
        if args.output is None:
            print(output)
        elif args.output.resolve() in (args.config.resolve(), locale_path.resolve()):
            raise ValueError("输出路径须与输入配置和语言包使用不同文件")
        else:
            temp = NamedTemporaryFile(dir=args.output.parent, delete=False)
            pending = Path(temp.name)
            try:
                with temp:
                    temp.write((output + "\n").encode("utf-8"))
                pending.chmod(args.output.stat().st_mode if args.output.exists() else 0o644)
                pending.replace(args.output)
            finally:
                pending.unlink(missing_ok=True)
    except (OSError, ValueError) as error:
        parser.exit(1, f"{error}\n")


if __name__ == "__main__":
    main()
