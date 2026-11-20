"""怪物面板、成长、受击抗性与档位招式的策划配置。"""

from collections import defaultdict
from fractions import Fraction

if __package__:
    from .crafting_tables import limited_value, write_row
else:
    from crafting_tables import limited_value, write_row

SHEETS = {
    "怪物战斗面板": ("species", "怪物ID 名称 气血点 护甲点 攻击点 法力点 神识点 移速千分 距离千分 数值取向",
                 "id name hp armor attack mana perception speedMilli rangeMilli note"),
    "怪物档位倍率": ("tiers", "档位ID 名称 气血比例 护甲比例 攻击比例 法力比例 神识比例 移速比例 基础灵石 每级灵石",
                 "id name hp armor attack mana perception speedMilli goldBase goldPerLevel"),
    "怪物等级基准": ("levels", "角色等级 原神识点 原突破修为", "id referencePerception upgradeLimit"),
    "战区成长": ("depths", "层次ID 面板倍率基点 说明", "id statBps note"),
    "怪物抗性": ("resists", "怪物ID 抗性域ID 来袭属性ID 抗性基点", "id domainId targetId resistBps"),
    "怪物招式": ("skills", "招式ID 名称 攻击倍率基点 施法毫秒 冷却秒 持续毫秒", "id name attackBps castMs cooldownSec durationMs"),
    "档位招式": ("tierSkills", "档位ID 招式ID 冷却覆盖秒", "tierId skillId cooldownSec"),
    "怪物结算参数": ("settings", "规则ID 默认难度基点 面板随机幅度基点 灵石随机幅度基点 空修为系数基点 综合抗性上限基点",
                 "id difficultyBps varianceBps goldVarianceBps basePowerBps resistLimitBps"),
}
OPTIONAL = {"档位招式": {"冷却覆盖秒"}}
STAT_FIELDS = ("hp", "armor", "attack", "mana", "perception", "speedMilli")
PANEL_LIMITS = {"hp": (150, 2750), "armor": (0, 700), "attack": (5, 65), "mana": (100, 2400),
                "perception": (100, 1800), "speedMilli": (1000, 7500), "rangeMilli": (1000, 10000)}
MAX_PANEL = 2147483647


def table_keys(actual, expected, name):
    if set(actual) != expected:
        raise ValueError(f"{name} 引用不完整。缺少 {sorted(expected - set(actual))}，多出 {sorted(set(actual) - expected)}")


def compile_stats(config, sheets):
    world = config["worldRules"]
    species = {int(k): v for k, v in world["monsterSpecies"].items()}
    panels, labels = {}, {}
    for where, row in sheets["怪物战斗面板"]:
        key = limited_value(row, "id", where, 1, MAX_PANEL)
        if key not in species:
            raise ValueError(f"{where} 引用了图鉴以外的怪物ID")
        fields = {key: limited_value(row, key, where, *bounds) for key, bounds in PANEL_LIMITS.items()}
        write_row(panels, key, fields, where)
        labels[key] = {"note": row["note"]}
    table_keys(panels, set(species), "怪物战斗面板")

    settings_rows = sheets["怪物结算参数"]
    if len(settings_rows) != 1:
        raise ValueError("怪物结算参数须有且只有一行")
    where, row = settings_rows[0]
    limited_value(row, "id", where, 1, 1)
    settings = {k: limited_value(row, k, where, low, high) for k, low, high in (
        ("difficultyBps", 1, 100000), ("varianceBps", 0, 2500), ("goldVarianceBps", 0, 3000),
        ("basePowerBps", 1, 10000), ("resistLimitBps", 1, 5000))}
    tiers, tier_names = {}, {}
    for where, row in sheets["怪物档位倍率"]:
        tier = limited_value(row, "id", where, 1, MAX_PANEL)
        fields = {k: limited_value(row, k, where, 1, 30000) for k in STAT_FIELDS}
        for k in ("goldBase", "goldPerLevel"):
            fields[k] = limited_value(row, k, where, 0, MAX_PANEL)
        write_row(tiers, tier, fields, where)
        tier_names[tier] = {"name": row["name"]}
    expected_tiers = {int(k) for wave in world["encounterWaves"].values() for k in wave}
    table_keys(tiers, expected_tiers, "怪物档位倍率")
    if 1 not in tiers:
        raise ValueError("怪物档位倍率需要普通档1作换算标尺")

    levels, depths, depth_labels = {}, {}, {}
    for where, row in sheets["怪物等级基准"]:
        level = limited_value(row, "id", where, 1, 10000)
        base = limited_value(row, "referencePerception", where, 100, MAX_PANEL)
        limit = limited_value(row, "upgradeLimit", where, 1, 2**53-1)
        write_row(levels, level, {"referencePerception": base, "upgradeLimit": limit}, where)
    if not levels or sorted(levels) != list(range(1, max(levels)+1)):
        raise ValueError("怪物等级基准应从1级连续登记")
    if any(levels[i][k] < levels[i-1][k] for i in levels if i > 1 for k in ("referencePerception", "upgradeLimit")):
        raise ValueError("怪物等级的基准神识或突破修为发生倒退")
    for where, row in sheets["战区成长"]:
        depth = limited_value(row, "id", where, 1, MAX_PANEL)
        write_row(depths, depth, limited_value(row, "statBps", where, 1, 30000), where)
        depth_labels[depth] = {"note": row["note"]}
    table_keys(depths, {int(k) for k in world["encounterWaves"]}, "战区成长")

    resists = defaultdict(dict)
    attributes = {1: {r["id"] for r in config["elements"]}, 2: {r["id"] for r in config["qiTypes"]}}
    for where, row in sheets["怪物抗性"]:
        key = limited_value(row, "id", where, 1, MAX_PANEL)
        domain = limited_value(row, "domainId", where, 1, 2)
        target = limited_value(row, "targetId", where, 1, MAX_PANEL)
        value = limited_value(row, "resistBps", where, -settings["resistLimitBps"], settings["resistLimitBps"])
        if key not in species or target not in attributes[domain] or not value:
            raise ValueError(f"{where} 引用了未知的物种、属性或零抗性")
        write_row(resists[key].setdefault(domain, {}), target, value, where)
    for key in species:
        for domain in attributes:
            values = resists[key].get(domain, {}).values()
            if not values or min(values) >= 0 or max(values) <= 0:
                raise ValueError(f"怪物 {key} 的抗性域 {domain} 需填写一项抗性及一项弱点")

    skills, skill_labels, tier_skills = {}, {}, defaultdict(dict)
    for where, row in sheets["怪物招式"]:
        skill = limited_value(row, "id", where, 1, MAX_PANEL)
        values = {k: limited_value(row, k, where, low, high) for k, low, high in
                  (("attackBps", 0, 50000), ("castMs", 0, 120000), ("cooldownSec", 1, 3600), ("durationMs", 0, 3600000))}
        write_row(skills, skill, values, where)
        skill_labels[skill] = {"name": row["name"]}
    for where, row in sheets["档位招式"]:
        tier = limited_value(row, "tierId", where, 1, MAX_PANEL)
        skill = limited_value(row, "skillId", where, 1, MAX_PANEL)
        if tier not in tiers or skill not in skills:
            raise ValueError(f"{where} 引用了未知档位或招式")
        override = (None if row["cooldownSec"] is None else limited_value(row, "cooldownSec", where, 1, 3600))
        write_row(tier_skills[tier], skill, override, where)
    table_keys(tier_skills, set(tiers), "档位招式")
    return ({"species": panels, "tiers": tiers, "levels": levels, "depths": depths, "settings": settings,
             "resists": dict(resists), "skills": skills, "tierSkills": dict(tier_skills)},
            {"species": labels, "tiers": tier_names, "depths": depth_labels, "skills": skill_labels})


def half_up(value):
    """面板输入非负，取 Go math.Round(x) 的正值分支。"""
    return int(value + Fraction(1, 2))


def monster_panel(catalog, monster, map_key, tier, level, *, perception=None, super_air=None,
                  variance_bps=0, server_difficulty_bps=10000, gold_roll_bps=0, drop_multiplier_bps=10000):
    """按指定战区实际可刷的物种档位预览，无随机采样；浮动可在区间内指定。"""
    rules = catalog["monsterStats"]
    key, tier_key, level_key = str(monster), str(tier), str(level)
    if map_key not in catalog["encounterMapIds"]:
        raise ValueError(f"战区地图不存在 {map_key}")
    if key not in rules["species"] or tier_key not in rules["tiers"] or level_key not in rules["levels"]:
        raise ValueError("请选择已登记的怪物、档位和角色等级")
    zone = catalog["encounterMaps"][str(catalog["encounterMapIds"][map_key])]
    wave = str(zone["waveId"])
    pool = catalog["encounterPoolMonsters"][str(zone["poolId"])]
    if not catalog["encounterWaves"][wave].get(tier_key) or key not in pool.get(tier_key, {}):
        raise ValueError(f"{map_key} 的该档位不能遭遇怪物 {key}")
    limits = rules["settings"]
    for value, lo, hi, label in (
        (variance_bps, -limits["varianceBps"], limits["varianceBps"], "面板浮动"),
        (gold_roll_bps, -limits["goldVarianceBps"], limits["goldVarianceBps"], "灵石浮动"),
        (server_difficulty_bps, 0, 100000, "服务器难度"), (drop_multiplier_bps, 0, 100000, "掉落倍率"),
    ):
        if type(value) is not int or not lo <= value <= hi:
            raise ValueError(f"{label}须在 {lo}～{hi} 基点内")
    role = rules["levels"][level_key]
    if perception is None:
        perception = role["referencePerception"]
    if super_air is None:
        super_air = role["upgradeLimit"]
    if type(perception) is not int or not 0 <= perception <= MAX_PANEL or type(super_air) is not int or super_air < 0:
        raise ValueError("角色神识须为非负32位整数，修为须为非负整数")
    source = Fraction(max(100, perception), 1000)
    progress = Fraction(min(super_air, role["upgradeLimit"]), role["upgradeLimit"])
    power = (limits["basePowerBps"] + (10000-limits["basePowerBps"]) * progress) / 10000
    difficulty = Fraction(limits["difficultyBps"] * max(1000, server_difficulty_bps), 10000**2)
    depth = Fraction(rules["depths"][wave], 10000)
    variability = Fraction(10000+variance_bps, 10000)
    species, ordinary, band = rules["species"][key], rules["tiers"]["1"], rules["tiers"][tier_key]
    panel = {}
    for stat, floor in (("hp", 30), ("armor", 0), ("attack", 1), ("mana", 50), ("perception", 0)):
        value = source * species[stat] * Fraction(band[stat], ordinary[stat]) * difficulty * depth
        if stat in ("hp", "armor", "attack"):
            value *= power * variability
        rounded = max(floor, half_up(value))
        panel[stat] = rounded if stat == "attack" else min(MAX_PANEL, rounded)
    panel["speedMilli"] = half_up(Fraction(species["speedMilli"] * band["speedMilli"], ordinary["speedMilli"]))
    panel["rangeMilli"] = species["rangeMilli"]
    panel["gold"] = max(1, half_up(Fraction(
        (band["goldBase"] + int(level)*band["goldPerLevel"]) * drop_multiplier_bps * (10000+gold_roll_bps), 10000**2)))
    profile_id = catalog["monsterSpecies"][key]["profileId"]
    profile = catalog["profiles"][profile_id]
    skills = []
    for skill_id, override in rules["tierSkills"][tier_key].items():
        skill = rules["skills"][skill_id]
        base_damage = max(1, half_up(Fraction(panel["attack"] * skill["attackBps"], 10000)))
        skills.append({"id": int(skill_id), "baseDamage": base_damage, "castMs": skill["castMs"],
                       "cooldownSec": override or skill["cooldownSec"], "durationMs": skill["durationMs"]})
    return {"mapKey": map_key, "regionId": zone["regionId"], "waveId": zone["waveId"], "monsterId": int(monster),
            "tierId": int(tier), "level": int(level), "rolePerception": perception, "roleSuperAir": super_air,
            "upgradeLimit": role["upgradeLimit"], "elementId": profile["primaryElementId"], "qiId": profile["primaryQiId"],
            "panel": panel, "resists": rules["resists"][key], "skills": skills}


def incoming_hit(catalog, monster, damage, element_id, qi_id):
    """拟接入的受击层：外层已结算的伤害只经过一次五行与气抗修正。"""
    if type(damage) is not int or damage < 0:
        raise ValueError("伤害须为非负整数")
    if str(element_id) not in catalog["elements"] or str(qi_id) not in catalog["qiTypes"]:
        raise ValueError("来袭五行或气性须在对应字典内")
    rules = catalog["monsterStats"]
    resist = rules["resists"][str(monster)]
    cap = rules["settings"]["resistLimitBps"]
    bps = max(-cap, min(cap, resist["1"].get(str(element_id), 0) + resist["2"].get(str(qi_id), 0)))
    return {"resistBps": bps, "damage": half_up(Fraction(damage*(10000-bps), 10000))}
