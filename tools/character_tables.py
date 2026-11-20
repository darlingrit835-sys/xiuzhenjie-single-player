"""命名修士、随身功法与器物、战区行迹和稀有战利品。"""

from collections import defaultdict

if __package__:
    from .crafting_tables import limited_value, write_row
else:
    from crafting_tables import limited_value, write_row


SHEETS = {
    "修士名录": ("templates", "人物ID 姓名 称号 等级 家乡郡ID 阴阳ID 五行ID 主气ID 宝库ID 生平", "id name title level homeRegionId polarityId elementId qiId poolId note"),
    "修士面板": ("panels", "人物ID 气血 灵力 攻击 防御 身法 神识", "id hp energy attack defense speed spirit"),
    "修士所学": ("methods", "人物ID 功法ID", "characterId methodId"),
    "随身器物": ("equipment", "人物ID 槽位ID 物品模板键", "characterId slotId itemId"),
    "修士行迹": ("encounters", "战区ID 人物ID 档位ID 相对权重", "mapId characterId tierId weight"),
    "奇遇宝库": ("pools", "宝库ID 名称 说明", "id name note"),
    "奇遇掉落": ("rewardEntries", "宝库ID 条目ID 奖励类型ID 功法ID 物品模板键 普通掉率基点 精英掉率基点 首领掉率基点 最少份数 最多份数",
             "poolId entryId kindId methodId itemId normalBps eliteBps bossBps minQty maxQty"),
    "地域宝库": ("regionPools", "郡ID 宝库ID", "regionId poolId"),
    "妖兽宝库": ("monsterPools", "怪物ID 宝库ID", "monsterId poolId"),
}
OPTIONAL = {}
TIERS = (1, 3, 4)
PANEL_FIELDS = ("hp", "energy", "attack", "defense", "speed", "spirit")
MAX_ID = 2**31 - 1


def num(row, field, loc, low=1, high=MAX_ID):
    return limited_value(row, field, loc, low, high)


def foreign(row, field, loc, records):
    value = num(row, field, loc)
    if value not in records:
        raise ValueError(f"{loc}「{field}」引用无效 {value}")
    return value


def nested(table, parent, key, value, loc):
    write_row(table.setdefault(parent, {}), key, value, loc)


def carried(entry, methods, equipment):
    return entry["methodId"] in methods if entry["kindId"] == 1 else entry["itemId"] in equipment


def compile_characters(config, sheets):
    rules = {d[0]: {} for d in SHEETS.values()}
    labels = {"templates": {}, "pools": {}}
    world, cultivation = config["worldRules"], config["methodRules"]
    counties = {int(k) for k, v in world["regions"].items() if v["level"] == 3}
    zones = {int(k): v for k, v in world["encounterMaps"].items()}
    items = {v["id"]: v for v in config["itemTemplates"]}
    profiles = {v["id"]: v for v in config["profiles"]}
    categories = {v["id"]: v for v in config["itemCategories"]}
    methods = cultivation["templates"]

    for loc, row in sheets["奇遇宝库"]:
        pool = num(row, "id", loc)
        write_row(rules["pools"], pool, {}, loc)
        labels["pools"][pool] = {"name": row["name"], "note": row["note"]}

    signatures = defaultdict(set)
    for loc, row in sheets["奇遇掉落"]:
        pool = foreign(row, "poolId", loc, rules["pools"])
        kind, method = num(row, "kindId", loc, 1, 3), num(row, "methodId", loc, 0)
        item = row["itemId"]
        if item not in items:
            raise ValueError(f"{loc} 未登记掉落物品 {item}")
        item_kind = profiles[items[item]["profileId"]]["kindId"]
        if kind == 1 and (method not in methods or item_kind != "method_manual"):
            raise ValueError(f"{loc} 秘籍需要引用功法和秘籍载体")
        if kind != 1 and (method or item_kind == "method_manual" or (kind == 2) != (item_kind == "artifact")):
            raise ValueError(f"{loc} 法器、普通物品和秘籍的种类不符")
        signature = (item, method)
        if signature in signatures[pool]:
            raise ValueError(f"{loc} 同一宝库重复登记战利品 {signature}")
        signatures[pool].add(signature)
        low = num(row, "minQty", loc)
        high = num(row, "maxQty", loc, low)
        if kind in (1, 2) and high != 1:
            raise ValueError(f"{loc} 每个命中条目最多出一本秘籍或一件法器")
        rates = {tier: num(row, field, loc, 0, 10000) for tier, field in
                 zip(TIERS, ("normalBps", "eliteBps", "bossBps"))}
        if not any(rates.values()):
            raise ValueError(f"{loc} 战利品需要至少一档正掉率")
        nested(rules["rewardEntries"], pool, num(row, "entryId", loc), {
            "kindId": kind, "methodId": method, "itemId": item, "chanceBps": rates,
            "minQty": low, "maxQty": high,
        }, loc)
    if rules["pools"].keys() != rules["rewardEntries"].keys():
        raise ValueError("每个奇遇宝库均需登记战利品")

    dimensions = {field: {r["id"] for r in config[key]} for field, key in
                  (("polarityId", "polarities"), ("elementId", "elements"), ("qiId", "qiTypes"))}
    for loc, row in sheets["修士名录"]:
        char = num(row, "id", loc)
        entry = {field: foreign(row, field, loc, ids) for field, ids in dimensions.items()}
        entry.update(level=num(row, "level", loc), homeRegionId=foreign(row, "homeRegionId", loc, counties),
                     poolId=foreign(row, "poolId", loc, rules["pools"]))
        write_row(rules["templates"], char, entry, loc)
        labels["templates"][char] = {field: row[field] for field in ("name", "title", "note")}
    for loc, row in sheets["修士面板"]:
        char = foreign(row, "id", loc, rules["templates"])
        write_row(rules["panels"], char, {f: num(row, f, loc, 1 if f in ("hp", "attack", "speed") else 0) for f in PANEL_FIELDS}, loc)
    for loc, row in sheets["修士所学"]:
        char = foreign(row, "characterId", loc, rules["templates"])
        method = foreign(row, "methodId", loc, methods)
        if methods[method]["minLevel"] > rules["templates"][char]["level"]:
            raise ValueError(f"{loc} 修士未达功法修习等级")
        nested(rules["methods"], char, methods[method]["pathId"], method, loc)
    for loc, row in sheets["随身器物"]:
        char = foreign(row, "characterId", loc, rules["templates"])
        slot, item = num(row, "slotId", loc, 1, 4), row["itemId"]
        if item not in items:
            raise ValueError(f"{loc} 未登记随身物品 {item}")
        record = items[item]
        kind = profiles[record["profileId"]]["kindId"]
        choices = {1: categories[record["categoryId"]]["detailTable"] == "weapon", 2: kind == "artifact",
                   3: kind in ("formation", "talisman_brush"), 4: kind in ("pill", "talisman")}
        if not choices[slot]:
            raise ValueError(f"{loc} 随身物品与槽位不符 {item}")
        nested(rules["equipment"], char, slot, item, loc)

    for loc, row in sheets["修士行迹"]:
        zone = foreign(row, "mapId", loc, zones)
        char = foreign(row, "characterId", loc, rules["templates"])
        tier = foreign(row, "tierId", loc, TIERS)
        if not world["encounterWaves"][str(zones[zone]["waveId"])][str(tier)]:
            raise ValueError(f"{loc} 当前战区没有该档位")
        nested(rules["encounters"], zone, char, {"weight": num(row, "weight", loc), "tierId": tier}, loc)

    activities = defaultdict(dict)
    for zone, candidates in sorted(rules["encounters"].items()):
        for char, entry in candidates.items():
            activities[char].setdefault(entry["tierId"], []).append(zone)
    rules["mapsByCharacter"] = dict(activities)
    for char, definition in rules["templates"].items():
        active = rules["methods"].get(char, {})
        viable = any(mode["minLevel"] <= definition["level"] and set(mode["efficiencyByPath"]) == active.keys()
                     for mode in cultivation["modes"].values())
        if not viable or not activities[char] or char not in rules["panels"]:
            raise ValueError(f"修士 {char} 缺少可用修炼组合、面板或活动战区")
        loot = rules["rewardEntries"][definition["poolId"]].values()
        gear = rules["equipment"].get(char, {}).values()
        if not any(carried(e, active.values(), gear) and any(e["chanceBps"][t] for t in activities[char]) for e in loot):
            raise ValueError(f"修士 {char} 在活动档位没有与随身物品或功法相符的掉落")

    for sheet, field, target in (("地域宝库", "regionId", counties),
                               ("妖兽宝库", "monsterId", {int(k) for k in world["monsterSpecies"]})):
        for loc, row in sheets[sheet]:
            source = foreign(row, field, loc, target)
            write_row(rules[SHEETS[sheet][0]], source, foreign(row, "poolId", loc, rules["pools"]), loc)
    return rules, labels


def reward_options(catalog, tier, *, character_id=None, monster_id=None, map_id=None):
    """查询导出 JSON 内单次胜利的有效稀有掉落。人物只可掉出随身物或所修功法的秘籍副本。"""
    if tier not in TIERS or (character_id is not None and monster_id is not None):
        raise ValueError("档位需为1、3或4，同时只可选择一种敌方")
    rules = catalog["characters"]
    zone = None
    if map_id is not None:
        zone = catalog["encounterMaps"].get(str(map_id))
        if not zone or not catalog["encounterWaves"][str(zone["waveId"])][str(tier)]:
            raise ValueError("战区不存在或未配置该档位")
    if character_id is not None:
        key = str(character_id)
        if key not in rules["templates"]:
            raise ValueError("修士不存在")
        maps = rules["mapsByCharacter"][key].get(str(tier), [])
        if not maps or (zone and int(map_id) not in maps):
            raise ValueError("修士不在所选战区与档位")
        pool = rules["templates"][key]["poolId"]
    elif monster_id is not None:
        key = str(monster_id)
        if key not in catalog["monsterSpecies"]:
            raise ValueError("物种不存在")
        if tier == 4 and not catalog["monsterSpecies"][key]["allowBoss"]:
            raise ValueError("该物种不在首领档活动")
        if zone and key not in catalog["encounterPoolMonsters"][str(zone["poolId"])].get(str(tier), {}):
            raise ValueError("怪物不在所选战区与档位")
        pool = rules["monsterPools"].get(key)
        if pool is None:
            if zone is None:
                raise ValueError("此怪物按当地郡选取奖励，请指定战区")
            pool = rules["regionPools"].get(str(zone["regionId"]))
    elif zone:
        pool = rules["regionPools"].get(str(zone["regionId"]))
    else:
        raise ValueError("至少指定一名修士、一种怪物或一个战区")
    entries = []
    for entry_id, entry in rules["rewardEntries"].get(str(pool), {}).items():
        chance = entry["chanceBps"][str(tier)]
        if not chance or (character_id is not None and not carried(entry, rules["methods"][key].values(),
                                                                  rules["equipment"].get(key, {}).values())):
            continue
        entries.append(dict(entry, entryId=int(entry_id), chanceBps=chance))
    return {"poolId": pool, "entries": entries}
