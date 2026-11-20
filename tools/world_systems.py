"""种植、阵法、组织、斗法与市场的策划表。运行记录使用稳定外键，文案单独导出。"""

from collections import defaultdict, deque

if __package__:
    from .crafting_tables import limited_value, regional_source_check, requirements_by_product, write_row
else:
    from crafting_tables import limited_value, regional_source_check, requirements_by_product, write_row


SHEETS = {
    "灵植栽培": ("crops", "栽培ID 名称 繁育物模板键 消耗份数 收获物模板键 基准生长秒 最少株数 最多株数 地利词条键 说明",
             "id name inputId inputQty outputId growthSeconds minQty maxQty regionEffectId note"),
    "灵田介质": ("soilAffinity", "介质模板键 作物五行ID 生长修正基点 说明", "soilId elementId growthBps note"),
    "采集作业": ("collecting", "采法ID 作业秒 体力消耗 说明", "id workSeconds staminaCost note"),
    "器胚图谱": ("forgePatterns", "器谱ID 名称 器胚模板键 最低师阶ID 说明", "id name blankId minRank note"),
    "布阵阵式": ("formations", "阵式ID 名称 阵法对象键 品阶ID 持续秒 词条键 目标五行ID 加成基点 说明",
             "id name profileId gradeId durationSeconds effectId elementId bonusBps note"),
    "阵材位置": ("formationSlots", "阵式ID 阵位号 用途ID 材料模板键 份数 说明", "formationId slot kindId materialId quantity note"),
    "组织建制": ("organizations", "建制ID 名称 玩法键 驻地郡ID 成员上限 说明", "id name systemId regionId memberCap note"),
    "组织职权": ("offices", "建制ID 职位ID 名称 席位数 招纳权 任命权 交接权 说明",
             "organizationId officeId name seats canRecruit canAppoint canTransfer note"),
    "组织传承": ("traditions", "建制ID 传承ID 名称 传承对象键 功法模板ID 词条键 目标五行ID 加成基点 说明",
             "organizationId traditionId name profileId methodId effectId elementId bonusBps note"),
    "招式作用": ("moveActions", "作用ID 名称 结算键 单位ID 说明", "id name operation unitId note"),
    "战斗招式": ("moves", "招式ID 名称 法术对象键 作用ID 目标气ID 基础强度 灵力消耗 冷却回合 说明",
             "id name profileId actionId targetQiId magnitude energyCost cooldownTurns note"),
    "斗法模式": ("battleModes", "模式ID 名称 玩法键 威力倍率基点 地利克制启用 妖兽掉落启用 双方同意 回合上限 说明",
             "id name systemId powerScaleBps useRegionCounter monsterDrops requireConsent turnCap note"),
    "交易货币": ("currencies", "货币ID 名称 钱包键 说明", "id name walletKey note"),
    "拍卖规约": ("markets", "规约ID 名称 方式ID 货币ID 最低单价 最高单价 单笔数量上限 有效时数 挂牌费基点 优惠费率下限基点 最低起拍加价基点 说明",
             "id name modeId currencyId minUnitPrice maxUnitPrice lotCap durationHours listingFeeBps minFeeBps minRaiseBps note"),
    "拍品分类": ("marketCategories", "规约ID 物品分类键 可上架 说明", "marketId categoryId allowed note"),
}
OPTIONAL = {}
MAX_ID = 2**31 - 1


def number(row, field, loc, low=1, high=MAX_ID):
    return limited_value(row, field, loc, low, high)


def flag(row, field, loc):
    return number(row, field, loc, 0, 1)


def reference(value, choices, loc, field):
    if value not in choices:
        raise ValueError(f"{loc}「{field}」没有可用的引用 {value}")
    return value


def compile_world(config, sheets):
    tables = {values[0]: {} for values in SHEETS.values()}
    locale = {key: {} for key in tables}
    world = config["worldRules"]
    profiles = {r["id"]: r for r in config["profiles"]}
    items = {r["id"]: r for r in config["itemTemplates"]}
    categories = {r["id"]: r for r in config["itemCategories"]}
    grades = {r["id"] for r in config["grades"]}
    elements = {r["id"] for r in config["elements"]}
    systems = {r["id"] for r in config["typeRegistry"] if r["domain"] == "system"}
    use_roles = world["resourceUseRoles"]
    county_ids = {int(key) for key, r in world["regions"].items() if r["level"] == 3}
    ingredients = requirements_by_product(config)
    sourced = regional_source_check(ingredients, world["resourceSources"])

    def store(sheet, loc, row, key, rule, parent=None):
        table = SHEETS[sheet][0]
        values, texts = tables[table], locale[table]
        if parent is not None:
            values, texts = values.setdefault(parent, {}), texts.setdefault(parent, {})
        write_row(values, key, rule, loc)
        texts[key] = {k: row[k] for k in ("name", "note") if k in row}

    def element(row, loc):
        return reference(number(row, "elementId", loc), elements, loc, "目标五行")

    for loc, row in sheets["灵植栽培"]:
        key = number(row, "id", loc)
        prop, crop = row["inputId"], row["outputId"]
        if use_roles.get(prop, {}).get("cultivation_field") not in ("propagule", "seed"):
            raise ValueError(f"{loc} 繁育物 {prop} 需要在资源用途里登记为种子或可繁育植株")
        if crop not in items or profiles[items[crop]["profileId"]]["kindId"] != "spirit_plant":
            raise ValueError(f"{loc} 收获物 {crop} 应引用灵植模板")
        effect = reference(row["regionEffectId"], world["regionEffects"], loc, "地利词条键")
        if world["regionEffects"][effect]["targetKind"] != 2:
            raise ValueError(f"{loc} 生长地利须作用于作物")
        low = number(row, "minQty", loc)
        store("灵植栽培", loc, row, key, {
            "inputId": prop, "inputQty": number(row, "inputQty", loc), "outputId": crop,
            "growthSeconds": number(row, "growthSeconds", loc), "minQty": low,
            "maxQty": number(row, "maxQty", loc, low), "regionEffectId": effect,
        })

    # 农作物可作下一个品种的繁育物。只允许有天然/加工起点的繁育链。
    cultivated = defaultdict(set)
    for crop in tables["crops"].values():
        cultivated[crop["inputId"]].add(crop["outputId"])
    reachable = {item for item in cultivated if sourced(item)}
    queue = deque(reachable)
    while queue:
        for output in cultivated.get(queue.popleft(), ()):
            if output not in reachable:
                reachable.add(output)
                queue.append(output)
    missing = cultivated.keys() - reachable
    if missing:
        raise ValueError(f"灵植栽培的繁育链缺少地区来源 {sorted(missing)}")

    for loc, row in sheets["灵田介质"]:
        soil = row["soilId"]
        if use_roles.get(soil, {}).get("cultivation_field") != "medium":
            raise ValueError(f"{loc} {soil} 缺少种植介质用途")
        store("灵田介质", loc, row, element(row, loc), number(row, "growthBps", loc, -2500, 2500), soil)

    for loc, row in sheets["采集作业"]:
        key = reference(number(row, "id", loc), {int(k) for k in world["gatherMethods"]}, loc, "采法ID")
        store("采集作业", loc, row, key, {"workSeconds": number(row, "workSeconds", loc),
                                          "staminaCost": number(row, "staminaCost", loc, 0)})
    if set(map(int, world["gatherMethods"])) != tables["collecting"].keys():
        raise ValueError("采集作业应覆盖采法表中的每种作业")

    weapon_ids = {r["templateId"] for r in config["weaponDetails"]}
    rank_ids = {int(k) for k in config["forgeFactors"]["smithRanks"]}
    for loc, row in sheets["器胚图谱"]:
        blank = reference(row["blankId"], weapon_ids, loc, "器胚模板键")
        if blank not in ingredients:
            raise ValueError(f"{loc} 器胚 {blank} 缺少造物用料")
        store("器胚图谱", loc, row, number(row, "id", loc), {"blankId": blank, "minRank":
              reference(number(row, "minRank", loc, 0), rank_ids, loc, "最低师阶ID")})

    for loc, row in sheets["布阵阵式"]:
        p = row["profileId"]
        if profiles.get(p, {}).get("kindId") != "formation":
            raise ValueError(f"{loc} 阵法对象 {p} 未登记")
        effect = reference(row["effectId"], world["regionEffects"], loc, "词条键")
        target = element(row, loc)
        if world["regionEffects"][effect]["targetKind"] == 6:
            source = profiles[p]["primaryElementId"]
            cycle = {r["id"]: r["overcomes"] for r in config["elements"]}
            if cycle.get(source) != target:
                raise ValueError(f"{loc} 克制阵要求目标五行为阵法主行所克")
        store("布阵阵式", loc, row, number(row, "id", loc), {
            "profileId": p, "gradeId": reference(number(row, "gradeId", loc), grades, loc, "品阶ID"),
            "durationSeconds": number(row, "durationSeconds", loc),
            "effectId": effect,
            "elementId": target, "bonusBps": number(row, "bonusBps", loc, 1, 1000),
        })

    for loc, row in sheets["阵材位置"]:
        formation = reference(number(row, "formationId", loc), tables["formations"], loc, "阵式ID")
        kind = number(row, "kindId", loc, 1, 2)
        material = row["materialId"]
        if use_roles.get(material, {}).get("array_craft") != {1: "anchor", 2: "mark"}[kind] or (material not in reachable and not sourced(material)):
            raise ValueError(f"{loc} 阵材 {material} 须符合阵位用途且能追溯到地区")
        store("阵材位置", loc, row, number(row, "slot", loc), {
            "kindId": kind, "materialId": material, "quantity": number(row, "quantity", loc),
        }, formation)
    for formation in tables["formations"]:
        kinds = [r["kindId"] for r in tables["formationSlots"].get(formation, {}).values()]
        if kinds.count(1) != 1 or 2 not in kinds:
            raise ValueError(f"阵式 {formation} 需要一个阵眼与至少一处阵纹")

    for loc, row in sheets["组织建制"]:
        system = reference(row["systemId"], systems & {"sect", "lineage"}, loc, "组织玩法")
        store("组织建制", loc, row, number(row, "id", loc), {
            "systemId": system, "regionId": reference(number(row, "regionId", loc), county_ids, loc, "驻地郡ID"),
            "memberCap": number(row, "memberCap", loc),
        })
    for loc, row in sheets["组织职权"]:
        org = reference(number(row, "organizationId", loc), tables["organizations"], loc, "建制ID")
        role = {field: flag(row, field, loc) for field in ("canRecruit", "canAppoint", "canTransfer")}
        role["seats"] = number(row, "seats", loc)
        store("组织职权", loc, row, number(row, "officeId", loc), role, org)
    for org, definition in tables["organizations"].items():
        offices = tables["offices"].get(org, {}).values()
        leaders = [office for office in offices if office["canTransfer"]]
        if len(leaders) != 1 or leaders[0]["seats"] != 1 or not leaders[0]["canAppoint"]:
            raise ValueError(f"建制 {org} 需要一个可任命、可交接的单席首领职位")
        if sum(office["seats"] for office in offices) > definition["memberCap"]:
            raise ValueError(f"建制 {org} 职位席位超过成员上限")
    for loc, row in sheets["组织传承"]:
        org = reference(number(row, "organizationId", loc), tables["organizations"], loc, "建制ID")
        family = tables["organizations"][org]["systemId"] == "lineage"
        p = row["profileId"]
        if profiles.get(p, {}).get("kindId") != ("lineage_method" if family else "sect_peak"):
            raise ValueError(f"{loc} 传承对象 {p} 应引用对应的家传心法或宗门山峰")
        method = number(row, "methodId", loc, 0)
        if family or method:
            template = config["methodRules"]["templates"].get(method)
            if template is None or (family and template["profileId"] != p):
                raise ValueError(f"{loc} 功法模板 {method} 与传承对象 {p} 不符")
        store("组织传承", loc, row, number(row, "traditionId", loc), {
            "profileId": p, "methodId": method, "effectId": reference(row["effectId"], world["regionEffects"], loc, "词条键"),
            "elementId": element(row, loc), "bonusBps": number(row, "bonusBps", loc, 1, 1000),
        }, org)

    # 单位 1 为基础伤害系数（基点），2 为层，3 为当前护盾的比例（基点）。
    action_units = {"direct_damage": 1, "dispel_qi": 2, "break_guard": 3}
    qi_types = {r["id"] for r in config["qiTypes"]}
    for loc, row in sheets["招式作用"]:
        operation = reference(row["operation"], action_units, loc, "结算键")
        unit = number(row, "unitId", loc)
        if unit != action_units[operation]:
            raise ValueError(f"{loc} 作用与强度单位不符")
        store("招式作用", loc, row, number(row, "id", loc), {"operation": operation, "unitId": unit})
    for loc, row in sheets["战斗招式"]:
        p = row["profileId"]
        if profiles.get(p, {}).get("kindId") != "spell":
            raise ValueError(f"{loc} 招式须引用已登记的法术对象 {p}")
        action = reference(number(row, "actionId", loc), tables["moveActions"], loc, "作用ID")
        unit = tables["moveActions"][action]["unitId"]
        qi = number(row, "targetQiId", loc, 0)
        if (unit == 2 and qi not in qi_types) or (unit != 2 and qi != 0):
            raise ValueError(f"{loc} 净化须引用目标气，伤害及破护的目标气填零")
        store("战斗招式", loc, row, number(row, "id", loc), {
            "profileId": p, "actionId": action, "targetQiId": qi,
            "magnitude": number(row, "magnitude", loc, 1, {1: 50000, 2: 16, 3: 10000}[unit]), "energyCost": number(row, "energyCost", loc),
            "cooldownTurns": number(row, "cooldownTurns", loc, 0),
        })
    for loc, row in sheets["斗法模式"]:
        system = reference(row["systemId"], systems & {"battle", "pvp"}, loc, "斗法玩法")
        mode = {field: flag(row, field, loc) for field in ("monsterDrops", "useRegionCounter", "requireConsent")}
        if system == "pvp" and (mode["monsterDrops"] or mode["useRegionCounter"] or not mode["requireConsent"]):
            raise ValueError(f"{loc} 对战须双端同意并关闭妖兽产出和克制地利")
        store("斗法模式", loc, row, number(row, "id", loc), {
            "systemId": system, **mode, "powerScaleBps": number(row, "powerScaleBps", loc, 1, 50000),
            "turnCap": number(row, "turnCap", loc),
        })
    wallets = set()
    for loc, row in sheets["交易货币"]:
        wallet = row["walletKey"]
        if not wallet.isascii() or not wallet.isidentifier() or wallet in wallets:
            raise ValueError(f"{loc} 钱包键须是唯一的 ASCII 标识")
        wallets.add(wallet)
        store("交易货币", loc, row, number(row, "id", loc), {"walletKey": wallet})
    for loc, row in sheets["拍卖规约"]:
        kind = number(row, "modeId", loc, 1, 2)
        low = number(row, "minUnitPrice", loc)
        price = number(row, "maxUnitPrice", loc, low, 2**63-1)
        cap = number(row, "lotCap", loc)
        fee = number(row, "listingFeeBps", loc, 0, 10000)
        raise_bps = number(row, "minRaiseBps", loc, 0, 10000)
        if (kind == 1) != (raise_bps == 0) or price * cap > 2**63-1:
            raise ValueError(f"{loc} 一口价加价须为零，竞价须有正加价，整单总价须在64位整数范围内")
        store("拍卖规约", loc, row, number(row, "id", loc), {
            "modeId": kind, "currencyId": reference(number(row, "currencyId", loc), tables["currencies"], loc, "货币ID"),
            "minUnitPrice": low, "maxUnitPrice": price, "lotCap": cap, "durationHours": number(row, "durationHours", loc),
            "listingFeeBps": fee, "minFeeBps": number(row, "minFeeBps", loc, 0, fee), "minRaiseBps": raise_bps,
        })
    for loc, row in sheets["拍品分类"]:
        market = reference(number(row, "marketId", loc), tables["markets"], loc, "规约ID")
        category = reference(row["categoryId"], categories, loc, "物品分类键")
        store("拍品分类", loc, row, category, flag(row, "allowed", loc), market)
    for market in tables["markets"]:
        if not any(tables["marketCategories"].get(market, {}).values()):
            raise ValueError(f"规约 {market} 须登记可上架的物品分类")

    for table, field, index in (("crops", "outputId", "cropsByHarvest"),
                                ("forgePatterns", "blankId", "forgePatternsByBlank"),
                                ("moves", "profileId", "movesByProfile")):
        groups = defaultdict(list)
        for key, rule in sorted(tables[table].items()):
            groups[rule[field]].append(key)
        tables[index] = dict(groups)
    by_material = defaultdict(set)
    for key, slots in tables["formationSlots"].items():
        for slot in slots.values():
            by_material[slot["materialId"]].add(key)
    tables["formationsByMaterial"] = {material: sorted(keys) for material, keys in by_material.items()}
    return tables, locale


def listing_allowed(categories, policy, category_id):
    """对已通过主表校验的分类树就近继承。未登记或无准入规则的类别不能挂牌。"""
    while category_id in categories:
        if category_id in policy:
            return bool(policy[category_id])
        category_id = categories[category_id]["parentId"]
    return False
