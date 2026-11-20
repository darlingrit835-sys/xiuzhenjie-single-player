"""将已校验的策划记录编译为按键索引的程序配置与简体中文展示资源。"""

from hashlib import sha256
import json


if __package__:
    from .content_affixes import compile_affix_catalog
    from .creation_catalog import compile_registry
else:
    from content_affixes import compile_affix_catalog
    from creation_catalog import compile_registry


PROFILE_ORIGINS = {"已有对象": "existing", "设计示例": "design"}
ITEM_ORIGINS = {"项目已有": "existing", "系统设计": "system", "玩家创制示例": "player_example"}


def pick(row, fields):
    return {field: row[field] for field in fields}


def indexed(rows, fields, key="id"):
    return {str(row[key]): pick(row, fields) for row in rows}


def compile_catalog(config):
    runtime = {"schemaVersion": 3}
    locale = {"schemaVersion": 3, "locale": "zh-CN", "classNameFormat": "{polarity}{element}·{qi}气"}
    # 程序字段与展示字段按白名单分别投影，避免将文案夹带进服务端配置。
    for table, program_fields, display_fields in (
        ("polarities", (), ("name", "meaning")),
        ("elements", ("generates", "overcomes"), ("name", "meaning")),
        ("qiTypes", (), ("name", "meaning", "example")),
        ("stats", (), ("name", "effect", "basis")),
        ("itemCategories", ("parentId", "detailTable"), ("name", "meaning")),
        ("itemTemplates", ("categoryId", "profileId", "gradeId", "prototypeId"), ("name", "note")),
        ("profiles", ("kindId", "polarityId", "primaryElementId", "secondaryElementId", "primaryQiId",
                      "secondaryQiId", "sourceId", "classId"), ("name", "note")),
    ):
        runtime[table] = indexed(config[table], program_fields)
        locale[table] = indexed(config[table], display_fields)

    for table, origins, display_key in (("profiles", PROFILE_ORIGINS, "profileOrigins"),
                                        ("itemTemplates", ITEM_ORIGINS, "itemOrigins")):
        for row in config[table]:
            runtime[table][row["id"]]["origin"] = origins[row["origin"]]
        locale[display_key] = {key: {"name": name} for name, key in origins.items()}

    for domain, table in (("kind", "profileKinds"), ("system", "systems")):
        rows = [row for row in config["typeRegistry"] if row["domain"] == domain]
        runtime[table] = indexed(rows, ())
        locale[table] = indexed(rows, ("name",))
    system_profiles = {key: set() for key in runtime["systems"]}
    for row in config["usages"]:
        system_profiles[row["systemId"]].add(row["profileId"])
    for item, systems in config["worldRules"]["resourceUses"].items():
        profile = runtime["itemTemplates"][item]["profileId"]
        for system in systems:
            system_profiles[system].add(profile)
    runtime["systemProfiles"] = {key: sorted(profiles) for key, profiles in system_profiles.items()}

    for table in ("grades", "legacyGrades"):
        runtime[table] = {}
        for row in config[table]:
            tier, quality = divmod(row["id"] - 1, 4)
            grade = {"tierId": tier + 1, "qualityId": quality + 1}
            if table == "grades":
                grade["multiplierBps"] = row["multiplierBps"]
            runtime[table][str(row["id"])] = grade
        locale[table] = indexed(config[table], ("name",))
    locale["tiers"] = {str(i): {"name": name} for i, name in enumerate("黄玄地天", 1)}
    locale["qualities"] = {str(i): {"name": name} for i, name in enumerate("下中上极", 1)}

    for prefix, field in (("polarity", "polarityId"), ("element", "elementId")):
        focus = prefix + "Focus"
        runtime[focus] = {str(row[field]): row["statId"] for row in config[focus]}
        locale[focus] = indexed(config[focus], ("meaning",), field)
        weights = prefix + "QiWeights"
        runtime[weights], locale[weights] = {}, {}
        for row in config[weights]:
            key, qi = str(row[field]), str(row["qiId"])
            runtime[weights].setdefault(key, {})[qi] = row["weight"]
            locale[weights].setdefault(key, {})[qi] = pick(row, ("name", "meaning"))

    runtime["bonusRules"] = {}
    for row in config["bonusRules"]:
        elements = runtime["bonusRules"].setdefault(str(row["polarityId"]), {})
        stats = elements.setdefault(str(row["elementId"]), {})
        stats[row["statId"]] = [row["minBps"], row["maxBps"]]

    for table, fields in (("materialDetails", ("form", "usage", "processing")),
                          ("weaponDetails", ("grip", "action", "blankRequirement"))):
        locale[table] = indexed(config[table], fields, "templateId")
    runtime["creationInputs"], locale["creationInputs"] = {}, {}
    for row in config["creationInputs"]:
        output, material = row["outputId"], row["inputId"]
        runtime["creationInputs"].setdefault(output, {})[material] = row["quantity"]
        locale["creationInputs"].setdefault(output, {})[material] = {"purpose": row["purpose"]}

    for table in ("affixCounts", "affixRanges", "itemAffixes", "forgeEffectRanges", "forgeFactors", "methodInfluences"):
        runtime[table] = config[table]
    locale["forgeEffects"] = {key: {"name": value} for key, value in config["forgeEffectNames"].items()}
    for domain, table in (("smithRanks", "smithRanks"), ("flames", "forgeFlames"),
                          ("targetGrades", "forgeTargets"), ("elementRelations", "forgeRelations")):
        locale[table] = config["forgeFactorLabels"][domain]

    runtime.update(config["worldRules"])
    locale.update(config["worldText"])
    runtime["alchemy"] = config["alchemyRules"]
    locale["alchemy"] = config["alchemyText"]
    if "inscriptionRules" in config:
        runtime["inscription"] = config["inscriptionRules"]
        locale["inscription"] = config["inscriptionText"]
    if "creationSheets" in config:
        runtime["methods"], locale["methods"] = config["methodRules"], config["methodText"]
        runtime["worldSystems"], locale["worldSystems"] = config["worldSystemRules"], config["worldSystemText"]
        runtime["monsterStats"], locale["monsterStats"] = config["monsterStatRules"], config["monsterStatText"]
        runtime["characters"], locale["characters"] = config["characterRules"], config["characterText"]
        runtime["projections"], locale["projections"] = config["projectionRules"], config["projectionText"]
        runtime["creation"], locale["creation"] = compile_registry(runtime, config["creationSheets"])
        runtime["contentAffixes"], locale["contentAffixes"] = compile_affix_catalog(runtime, config["creationSheets"])
    # 哈希包含两包内容，任一侧的有效数据变更都能识别成新的配套版本。
    canonical = json.dumps([runtime, locale], ensure_ascii=False, sort_keys=True, separators=(",", ":"))
    version = sha256(canonical.encode("utf-8")).hexdigest()
    runtime["catalogVersion"] = locale["catalogVersion"] = version
    return runtime, locale
