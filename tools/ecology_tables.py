"""Compile regional encounter, habitat, material and usage relations from the design workbook."""

import re


SHEETS = {
    "怪物图鉴": ("monsterSpecies", "怪物ID 名称 属性对象键 旧对象类型ID 旧图标ID 可为兽王 说明",
             "id name profileId objectTypeId iconId allowBoss note"),
    "地区怪物": ("regionMonsters", "地区ID 怪物ID 出现权重 说明", "regionId monsterId weight note"),
    "怪物产出": ("monsterDrops", "怪物ID 地区ID 产物模板键 普通率基点 精英率基点 兽王率基点 最少份数 最多份数 说明",
             "monsterId regionId itemId normal elite boss minQty maxQty note"),
    "采集方式": ("gatherMethods", "采法ID 名称 说明", "id name note"),
    "地区物产": ("regionResources", "地区ID 采法ID 产物模板键 节点权重 最少份数 最多份数 说明",
             "regionId methodId itemId weight minQty maxQty note"),
    "资源用途": ("resourceUses", "物品模板键 玩法键 用途角色键 用法名称 用法说明", "itemId systemId roleId name note"),
    "旧掉落对照": ("legacyMaterialTypes", "旧物品类型ID 旧名称 资源模板键 说明", "typeId name itemId note"),
}
OPTIONAL = {}


def integer(value, location, label, minimum=0, maximum=2147483647):
    if not re.fullmatch(r"[0-9]+(?:\.0+)?", str(value)):
        raise ValueError(f"{location} {label}须为整数")
    value = int(str(value).split(".")[0])
    if not minimum <= value <= maximum:
        raise ValueError(f"{location} {label}须在 {minimum}～{maximum}")
    return value


def store(rows, keys, value, location):
    for k in keys[:-1]:
        rows = rows.setdefault(k, {})
    if keys[-1] in rows:
        raise ValueError(f"{location} 联合键 {keys} 重复")
    rows[keys[-1]] = value


def compile_ecology(config, sheets, geography):
    """Return machine and presentation tables sharing IDs; all probability fields are basis points."""
    runtime = {schema[0]: {} for schema in SHEETS.values()}
    locale = {schema[0]: {} for schema in SHEETS.values()}
    items = {row["id"]: row for row in config["itemTemplates"]}
    profiles = {row["id"]: row for row in config["profiles"]}
    systems = {row["id"] for row in config["typeRegistry"] if row["domain"] == "system"}
    categories = {row["id"]: row for row in config["itemCategories"]}
    counties = {key for key, row in geography["regions"].items() if row["level"] == 3}

    def number_key(row, location, field, allowed):
        key = str(integer(row[field], location, field, 1))
        if key not in allowed:
            raise ValueError(f"{location} 未定义的 {field} {key}")
        return key

    def put(table, keys, data, presentation, location):
        store(runtime[table], keys, data, location)
        store(locale[table], keys, presentation, location)

    def amounts(row, location):
        low = integer(row["minQty"], location, "最少份数", 1)
        high = integer(row["maxQty"], location, "最多份数", low)
        return {"minQty": low, "maxQty": high}

    def resource(row, location):
        key = row["itemId"]
        if key not in items:
            raise ValueError(f"{location} 产物或用途引用未知模板 {key}")
        return key

    legacy_monsters = set()
    monster_profiles = set()
    for location, row in sheets["怪物图鉴"]:
        key = str(integer(row["id"], location, "怪物ID", 1))
        profile = profiles.get(row["profileId"])
        if not profile or profile["kindId"] not in ("monster", "cultivator_style") or not profile["primaryElementId"] or not profile["primaryQiId"]:
            raise ValueError(f"{location} 怪物须引用具有主五行、主气的怪物或修士属性对象")
        if row["profileId"] in monster_profiles:
            raise ValueError(f"{location} 同一怪物属性对象被重复登记")
        monster_profiles.add(row["profileId"])
        obj = integer(row["objectTypeId"], location, "旧对象类型ID")
        icon = integer(row["iconId"], location, "旧图标ID")
        boss = bool(integer(row["allowBoss"], location, "可为兽王", 0, 1))
        if bool(obj) != bool(icon) or obj and (obj, icon) in legacy_monsters:
            raise ValueError(f"{location} 旧对象类型和图标须同时填写且组合唯一，新增物种两列均填0")
        if obj:
            legacy_monsters.add((obj, icon))
        if obj == 2001 and boss:
            raise ValueError(f"{location} 原服人形怪没有兽王波位")
        put("monsterSpecies", (key,), {"profileId": row["profileId"], "objectTypeId": obj, "iconId": icon,
            "allowBoss": boss}, {"name": row["name"], "note": row["note"]}, location)
    for location, row in sheets["采集方式"]:
        key = str(integer(row["id"], location, "采法ID", 1))
        put("gatherMethods", (key,), {}, {"name": row["name"], "note": row["note"]}, location)

    habitats = {}
    for location, row in sheets["地区怪物"]:
        region = number_key(row, location, "regionId", counties)
        monster = number_key(row, location, "monsterId", runtime["monsterSpecies"])
        habitats.setdefault(monster, set()).add(region)
        put("regionMonsters", (region, monster), integer(row["weight"], location, "出现权重", 1),
            {"note": row["note"]}, location)

    used = set()
    for location, row in sheets["怪物产出"]:
        monster = number_key(row, location, "monsterId", runtime["monsterSpecies"])
        region = str(integer(row["regionId"], location, "地区ID"))
        if region != "0" and region not in habitats.get(monster, set()):
            raise ValueError(f"{location} 限定地区须为该怪栖息的郡")
        item = resource(row, location)
        chances = {key: integer(row[name], location, name, 0, 10000)
                   for key, name in (("1", "normal"), ("3", "elite"), ("4", "boss"))}
        if not any(chances.values()) or chances["4"] and not runtime["monsterSpecies"][monster]["allowBoss"]:
            raise ValueError(f"{location} 须有可触发的掉率，禁止兽王的物种须填0兽王率")
        put("monsterDrops", (monster, region, item), {"chanceBps": chances, **amounts(row, location)},
            {"note": row["note"]}, location)
        used.add(item)
    for location, row in sheets["地区物产"]:
        region = number_key(row, location, "regionId", counties)
        method = number_key(row, location, "methodId", runtime["gatherMethods"])
        item = resource(row, location)
        put("regionResources", (region, method, item), {"weight": integer(row["weight"], location, "节点权重", 1),
            **amounts(row, location)}, {"note": row["note"]}, location)
        used.add(item)
    for location, row in sheets["旧掉落对照"]:
        type_id = str(integer(row["typeId"], location, "旧物品类型ID", 1))
        item = resource(row, location)
        if categories[items[item]["categoryId"]]["detailTable"] != "material":
            raise ValueError(f"{location} 旧物料族须指向材料表模板")
        put("legacyMaterialTypes", (type_id,), item, {"name": row["name"], "note": row["note"]}, location)
        used.add(item)
    runtime["resourceUseRoles"] = {}
    locale["resourceUseRoleNames"] = {}
    for location, row in sheets["资源用途"]:
        item, system = resource(row, location), row["systemId"]
        if system not in systems:
            raise ValueError(f"{location} 未登记的用途玩法键 {system}")
        role = row["roleId"]
        if not re.fullmatch(r"[a-z][a-z0-9_]*", role):
            raise ValueError(f"{location} 用途角色键须用稳定英文键")
        names = locale["resourceUseRoleNames"].setdefault(system, {})
        if names.setdefault(role, {"name": row["name"]})["name"] != row["name"]:
            raise ValueError(f"{location} 同一玩法的同一用途角色键需使用同一名称")
        store(runtime["resourceUseRoles"], (item, system), role, location)
        put("resourceUses", (item, system), True, {"name": row["name"], "note": row["note"]}, location)
    if missing := used - runtime["resourceUses"].keys():
        raise ValueError(f"生态产物未登记用途 {sorted(missing)}")
    if missing := runtime["monsterSpecies"].keys() - habitats.keys():
        raise ValueError(f"怪物未登记栖息地 {sorted(missing)}")
    if missing := runtime["monsterSpecies"].keys() - runtime["monsterDrops"].keys():
        raise ValueError(f"怪物未登记特色产出 {sorted(missing)}")
    runtime["resourceUses"] = {key: sorted(value) for key, value in runtime["resourceUses"].items()}
    return runtime, locale
