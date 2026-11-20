"""炼丹与画符共用的工艺点数校验和产地引用。"""

from collections import defaultdict
from functools import lru_cache

if __package__:
    from .geography_tables import integer
else:
    from geography_tables import integer


RELATIONS = {"same", "generates", "generatedBy", "overcomes", "overcomeBy", "neutral"}
MODIFIER_FIELDS = ("successBps", "yieldBps", "potencyBps")


def limited_value(row, field, location, low, high):
    value = integer(row, field, location)
    if not low <= value <= high:
        raise ValueError(f"{location}「{field}」须在 {low}～{high} 之间")
    return value


def modifiers(row, location):
    return {field: limited_value(row, field, location, -10000, 10000) for field in MODIFIER_FIELDS}


def write_row(table, key, value, location):
    if key in table:
        raise ValueError(f"{location} 规则键 {key} 重复")
    table[key] = value


def requirements_by_product(config):
    ingredients = defaultdict(dict)
    for row in config["creationInputs"]:
        ingredients[row["outputId"]][row["inputId"]] = row["quantity"]
    return ingredients


def regional_source_check(ingredients, sources):
    @lru_cache(maxsize=None)
    def has_region_source(item):
        source = sources.get(item, {})
        if "gather" in source or "drop" in source:
            return True
        # 造物用料已完成无环校验。加工料的原料都必须可追到地区。
        inputs = ingredients.get(item)
        return bool(inputs) and all(has_region_source(component) for component in inputs)

    return has_region_source
