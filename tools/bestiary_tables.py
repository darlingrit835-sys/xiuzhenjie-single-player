"""按游戏地图键定位候选池。郡级栖息、怪物属性、产物仍由原生态表维护。"""

import re

if __package__:
    from .ecology_tables import integer, store
else:
    from ecology_tables import integer, store


SHEETS = {
    "遭遇层次": ("encounterWaves", "层次ID 名称 普通位 精英位 兽王位", "id name normal elite boss"),
    "战区群落": ("encounterPools", "群落ID 郡ID 名称", "id regionId name"),
    "群落怪物": ("encounterPoolMonsters", "群落ID 档位ID 怪物ID 局部权重", "poolId tierId monsterId weight"),
    "林地战区": ("encounterMaps", "战区ID 地图键 郡ID 层次ID 群落ID", "id mapKey regionId waveId poolId"),
}

# 这六个郡的 108 个键是已有 Go/Java 地图协议的一部分。后续策划地图可自行登记地图键。
NATIVE_COUNTIES = (1111, 1112, 1121, 1211, 1311, 1411)
NATIVE_MAPS = {f"{county}W{forest}{depth}": (county, depth)
               for county in NATIVE_COUNTIES for forest in range(1, 7) for depth in range(1, 4)}
TIERS = {1: "normal", 3: "elite", 4: "boss"}


def compile_bestiary(sheets, ecology, geography):
    regions = {int(key) for key, row in geography["regions"].items() if row["level"] == 3}
    habitats = ecology["regionMonsters"]
    species = ecology["monsterSpecies"]
    waves, pools, members, maps, keys = {}, {}, {}, {}, {}
    wave_names, pool_names = {}, {}

    def number(row, field, where, minimum=1):
        return integer(row[field], where, field, minimum)

    for where, row in sheets["遭遇层次"]:
        wave = str(number(row, "id", where))
        slots = {str(tier): number(row, field, where, 0) for tier, field in TIERS.items()}
        if not any(slots.values()):
            raise ValueError(f"{where} 波次至少须有一个怪物位")
        store(waves, (wave,), slots, where)
        wave_names[wave] = {"name": row["name"]}

    for where, row in sheets["战区群落"]:
        pool = str(number(row, "id", where))
        county = number(row, "regionId", where)
        if county not in regions:
            raise ValueError(f"{where} 群落须关联已登记的郡")
        store(pools, (pool,), {"regionId": county}, where)
        pool_names[pool] = {"name": row["name"]}

    for where, row in sheets["群落怪物"]:
        pool = str(number(row, "poolId", where))
        tier = number(row, "tierId", where)
        monster = str(number(row, "monsterId", where))
        if pool not in pools or tier not in TIERS:
            raise ValueError(f"{where} 未知群落或档位")
        county = str(pools[pool]["regionId"])
        if monster not in habitats.get(county, {}):
            raise ValueError(f"{where} 怪物未在本郡登记栖息")
        if tier == 4 and not species[monster]["allowBoss"]:
            raise ValueError(f"{where} 该怪物禁止进入兽王位")
        store(members, (pool, str(tier), monster), number(row, "weight", where), where)

    used_pools = set()
    reachable = {}
    active_slots = set()
    for where, row in sheets["林地战区"]:
        key = row["mapKey"]
        zone = number(row, "id", where)
        county = number(row, "regionId", where)
        wave_id, pool = number(row, "waveId", where), number(row, "poolId", where)
        if (not re.fullmatch(r"[A-Za-z0-9][A-Za-z0-9_.-]*", key)
                or re.fullmatch(r"[0-9]+", key)):
            raise ValueError(f"{where} 战区地图键无效或占用了纯数字城镇与设施键")
        if key in NATIVE_MAPS:
            if NATIVE_MAPS[key] != (county, wave_id):
                raise ValueError(f"{where} 原地图的郡或层次与完整工程不符")
        elif re.fullmatch(r"[0-9]+W[0-9]+", key) and int(key.split("W")[0]) in NATIVE_COUNTIES:
            raise ValueError(f"{where} 不存在的原服妖兽林地图键 {key}")
        if str(wave_id) not in waves or str(pool) not in pools or pools[str(pool)]["regionId"] != county:
            raise ValueError(f"{where} 战区层次或本郡群落引用无效")
        store(maps, (str(zone),), {"regionId": county, "waveId": wave_id, "poolId": pool}, where)
        store(keys, (key,), zone, where)
        used_pools.add(str(pool))
        for tier, slots in waves[str(wave_id)].items():
            if not slots:
                continue
            eligible = members.get(str(pool), {}).get(tier, {})
            if not eligible:
                raise ValueError(f"{where} 有波位但无候选：档位 {tier}")
            active_slots.add((str(pool), tier))
            for monster in eligible:
                reachable.setdefault((str(county), monster), set()).add(tier)

    if missing := NATIVE_MAPS.keys() - keys.keys():
        raise ValueError(f"现有战斗地图缺少战区 {sorted(missing)}")
    if missing := pools.keys() - used_pools:
        raise ValueError(f"群落未绑定地图 {sorted(missing)}")
    for pool, groups in members.items():
        for tier in groups:
            if (pool, tier) not in active_slots:
                raise ValueError(f"群落 {pool} 的档位 {tier} 没有可用战区")
    registered = {(county, monster) for county, monsters in habitats.items() for monster in monsters}
    if missing := registered - reachable.keys():
        raise ValueError(f"郡内栖息记录缺少可遭遇地图 {sorted(missing)}")
    for (county, monster), tiers in reachable.items():
        scoped = ecology["monsterDrops"][monster]
        for item, drop in {**scoped.get("0", {}), **scoped.get(county, {})}.items():
            if not any(drop["chanceBps"][tier] for tier in tiers):
                raise ValueError(f"郡 {county} 的怪物 {monster} 没有可掉落 {item} 的有效波位")
    if missing := regions - {row["regionId"] for row in maps.values()}:
        raise ValueError(f"郡未登记战区 {sorted(missing)}")
    return ({"encounterWaves": waves, "encounterPools": pools, "encounterPoolMonsters": members,
             "encounterMaps": maps, "encounterMapIds": keys},
            {"encounterWaves": wave_names, "encounterPools": pool_names})


def map_ecology_summary(config, locale, map_key, tier):
    """以本郡票数乘上群落局部权重，相同物种的掉率继续按原有郡级覆盖规则查询。"""
    if __package__:
        from .region_ecology import region_ecology_summary
    else:
        from region_ecology import region_ecology_summary
    zone = config["encounterMaps"][str(config["encounterMapIds"][map_key])]
    tier_id, pool = str(tier), str(zone["poolId"])
    slots = config["encounterWaves"][str(zone["waveId"])][tier_id]
    candidates = config["encounterPoolMonsters"][pool].get(tier_id, {}) if slots else {}
    summary = region_ecology_summary(config, locale, zone["regionId"], tier, candidates)
    summary.update(mapKey=map_key, wave=locale["encounterWaves"][str(zone["waveId"])]["name"],
                   pool=locale["encounterPools"][pool]["name"], slots=slots)
    return summary
