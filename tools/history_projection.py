"""历史投影的只读策划规则与本地 SQLite 写读样例。实际历练接入须提供服务端战报。"""

from contextlib import contextmanager
import json
import random
import re

if __package__:
    from .ecology_tables import integer, store
else:
    from ecology_tables import integer, store


SHEETS = {
    "投影规则": ("projectionRules", "规则ID 名称 出现率基点 保留小时 奖励池ID 奖励档位ID 说明",
             "id name chanceBps hours poolId tierId note"),
    "投影适用战区": ("projectionWaves", "层次ID 规则ID", "waveId ruleId"),
}


def compile_projection(config, sheets):
    rules, labels, waves = {}, {}, {}
    pools = {str(key) for key in config["characterRules"]["pools"]}
    for where, row in sheets["投影规则"]:
        rid = str(integer(row["id"], where, "规则ID", 1))
        pool = integer(row["poolId"], where, "奖励池ID", 1)
        if str(pool) not in pools:
            raise ValueError(f"{where} 投影奖励池不存在")
        tier = integer(row["tierId"], where, "奖励档位ID", 1, 4)
        if tier not in (1, 3, 4):
            raise ValueError(f"{where} 未知的奖励档位 {tier}")
        store(rules, (rid,), {"chanceBps": integer(row["chanceBps"], where, "出现率基点", 0, 10000),
                             "hours": integer(row["hours"], where, "保留小时", 1, 8760), "poolId": pool, "tierId": tier}, where)
        labels[rid] = {"name": row["name"], "note": row["note"]}
    for where, row in sheets["投影适用战区"]:
        wave = str(integer(row["waveId"], where, "层次ID", 1))
        rule = str(integer(row["ruleId"], where, "规则ID", 1))
        if wave not in config["worldRules"]["encounterWaves"] or rule not in rules:
            raise ValueError(f"{where} 未登记的层次或投影规则")
        store(waves, (wave,), int(rule), where)
    return {"rules": rules, "byWave": waves}, {"rules": labels}


PANEL = ("hp", "energy", "attack", "defense", "speed", "spirit")
SQL_INT = 2**63 - 1


def positive(value, label, low=1):
    if type(value) is not int or not low <= value <= SQL_INT:
        raise ValueError(f"{label} 超出可存储的整数范围")
    return value


def rule_for(catalog, map_id):
    positive(map_id, "地图ID")
    zone = catalog["encounterMaps"].get(str(map_id))
    if zone is None:
        raise ValueError("未知战区")
    rule = catalog["projections"]["byWave"].get(str(zone["waveId"]))
    return (rule, catalog["projections"]["rules"][str(rule)]) if rule else (None, None)


@contextmanager
def transaction(db):
    db.execute("BEGIN IMMEDIATE")
    try:
        yield
    except BaseException:
        db.rollback()
        raise
    else:
        db.commit()


def capture_clear(db, catalog, run_id, actor_id, map_id, cleared_at, name, level, panel, loadout):
    """调用方先验证战斗已完成、同意公开及角色归属。入参均是结算时的服务端快照。"""
    positive(run_id, "战报号")
    positive(actor_id, "角色ID")
    positive(cleared_at, "结算时间", 0)
    rule_id, rule = rule_for(catalog, map_id)
    if not rule:
        return False
    if not isinstance(name, str) or not name.strip() or len(name.strip()) > 48 or set(panel) != set(PANEL):
        raise ValueError("公开名或六项面板不完整")
    positive(level, "等级")
    values = [positive(panel[k], k, 1 if k in ("hp", "attack", "speed") else 0) for k in PANEL]
    if not isinstance(loadout, dict) or set(loadout) != {"methods", "equipment"}:
        raise ValueError("投影需要功法与装备快照")
    # 同版本可包含玩家创作，实体引用由内容版本表核实。实例数值随快照固定。
    with transaction(db):
        old = db.execute("SELECT actor_id,map_id,cleared_at FROM projection_run WHERE run_id=?", (run_id,)).fetchone()
        if old:
            if tuple(old) != (actor_id, map_id, cleared_at):
                raise ValueError("战报号与已有归属、地图或结算时间冲突")
            return False
        kinds_by_path = {entry["catalogPath"]:int(key) for key,entry in catalog["creation"]["kinds"].items()}
        groups = {"methods": kinds_by_path["methods.templates"], "equipment": kinds_by_path["itemTemplates"]}
        for group, kind in groups.items():
            slots = loadout[group]
            if not isinstance(slots, dict):
                raise ValueError("功法与装备按槽位存储")
            for slot, ref in slots.items():
                if not isinstance(slot, str) or not re.fullmatch(r"[1-9][0-9]*", slot) or not isinstance(ref, dict):
                    raise ValueError("快照槽位须为正数字符串")
                if set(ref) - {"templateId", "revision", "stats"} or "templateId" not in ref or "revision" not in ref:
                    raise ValueError("快照须引用已发布的模板版本")
                tid = positive(ref["templateId"], "模板ID")
                version = positive(ref["revision"], "创作版本")
                stats = ref.get("stats", {})
                if not isinstance(stats, dict) or any(not isinstance(k,str) or not k.isascii() or type(v) is not int
                                                      for k,v in stats.items()):
                    raise ValueError("实例词条须使用程序键和整数值")
                valid = db.execute("SELECT 1 FROM content_revision WHERE kind_id=? AND template_id=? AND revision=? "
                                   "AND state IN (1,2)", (kind,tid,version)).fetchone()
                if not valid:
                    raise ValueError("快照指向未发布或缺失的模板版本")
        expires = positive(cleared_at + rule["hours"] * 3600, "到期时间")
        snapshot = json.dumps(loadout, ensure_ascii=True, sort_keys=True, separators=(",", ":"))
        db.execute("INSERT INTO projection_run VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                   (run_id, actor_id, map_id, cleared_at, expires, rule_id,
                    catalog["catalogVersion"], name.strip(), level, *values, snapshot))
        active = db.execute("""SELECT r.cleared_at,r.run_id FROM projection_pool p
            JOIN projection_run r ON p.run_id=r.run_id WHERE p.map_id=? AND p.actor_id=?""",
            (map_id,actor_id)).fetchone()
        if active is None:
            seat = last_seat(db, map_id) + 1
            db.execute("INSERT INTO projection_pool VALUES (?,?,?,?,?)", (map_id,actor_id,run_id,seat,expires))
        elif (cleared_at,run_id) > tuple(active):
            db.execute("UPDATE projection_pool SET run_id=?,expires_at=? WHERE map_id=? AND actor_id=?",
                       (run_id,expires,map_id,actor_id))
    return True


def last_seat(db, map_id):
    return db.execute("SELECT COALESCE(MAX(seat),0) FROM projection_pool WHERE map_id=?", (map_id,)).fetchone()[0]


def remove_from_pool(db, map_id, actor_id):
    """在调用者的事务里撤下此人，用末席填洞。历史回执仍能解引用快照。"""
    seat = db.execute("SELECT seat FROM projection_pool WHERE map_id=? AND actor_id=?",(map_id,actor_id)).fetchone()
    if seat is None:
        return
    last = last_seat(db, map_id)
    db.execute("DELETE FROM projection_pool WHERE map_id=? AND actor_id=?",(map_id,actor_id))
    if seat[0] != last:
        db.execute("UPDATE projection_pool SET seat=? WHERE map_id=? AND seat=?",(seat[0],map_id,last))


def sample_active(db, map_id, viewer_id, now, rng):
    # 按索引清少量过期席，每个候选作者等权。正常命中只查末席、本人和一个随机席。
    expired = db.execute("SELECT actor_id FROM projection_pool WHERE map_id=? AND expires_at<=? LIMIT 16",
                         (map_id, now)).fetchall()
    for (actor_id,) in expired:
        remove_from_pool(db, map_id, actor_id)
    last = last_seat(db, map_id)
    own = db.execute("SELECT seat FROM projection_pool WHERE map_id=? AND actor_id=?", (map_id,viewer_id)).fetchone()
    count = last - int(own is not None)
    if count == 0:
        return None
    for _ in range(4):
        seat = 1 + rng.randrange(count)
        if own and seat >= own[0]:
            seat += 1
        row = db.execute("""SELECT r.run_id FROM projection_pool p JOIN projection_run r ON r.run_id=p.run_id
            WHERE p.map_id=? AND p.seat=? AND r.cleared_at<=? AND r.expires_at>?""",(map_id,seat,now,now)).fetchone()
        if row:
            return row[0]
    # 异常积压时只在本图有效席内抽取，不偏向高频通关者。
    active = """ FROM projection_pool p JOIN projection_run r ON r.run_id=p.run_id
        WHERE p.map_id=? AND p.actor_id<>? AND r.cleared_at<=? AND r.expires_at>?"""
    params = (map_id,viewer_id,now,now)
    count = db.execute("SELECT COUNT(*)" + active, params).fetchone()[0]
    if count:
        return db.execute("SELECT p.run_id" + active + " ORDER BY p.actor_id LIMIT 1 OFFSET ?",
                          (*params,rng.randrange(count))).fetchone()[0]
    return None


def draw(db, catalog, encounter_id, viewer_id, map_id, now, rng=None):
    """返回战报号或 None。固定服务端遭遇号，重试复用首次结果（也含未命中）。"""
    positive(encounter_id, "遭遇号")
    positive(viewer_id, "角色ID")
    positive(now, "遭遇时间", 0)
    _, rule = rule_for(catalog, map_id)
    rng = rng if rng is not None else random.SystemRandom()
    with transaction(db):
        old = db.execute("SELECT viewer_id,map_id,run_id FROM projection_encounter WHERE encounter_id=?",
                         (encounter_id,)).fetchone()
        if old:
            if tuple(old[:2]) != (viewer_id,map_id):
                raise ValueError("遭遇号与已有归属或地图冲突")
            return old[2]
        chosen = None
        if rule and rng.randrange(10000) < rule["chanceBps"]:
            chosen = sample_active(db, map_id, viewer_id, now, rng)
        reward = (rule["poolId"], rule["tierId"], catalog["catalogVersion"]) if chosen else (None, None, None)
        db.execute("INSERT INTO projection_encounter VALUES (?,?,?,?,?,?,?,?)",
                   (encounter_id,viewer_id,map_id,now,chosen,*reward))
    return chosen




def settle_victory(db, catalog, encounter_id, viewer_id, rng=None):
    """离线胜利结算。正式调用者负责校验胜负与归属，并与实际入纳戒共用事务。"""
    positive(encounter_id, "遭遇号")
    positive(viewer_id, "角色ID")
    rng = rng if rng is not None else random.SystemRandom()
    with transaction(db):
        row = db.execute("""SELECT e.run_id,e.reward_pool,e.reward_tier,e.reward_catalog,r.loadout
            FROM projection_encounter e JOIN projection_run r ON e.run_id=r.run_id
            WHERE e.encounter_id=? AND e.viewer_id=?""",(encounter_id,viewer_id)).fetchone()
        if not row:
            raise ValueError("未命中历史投影或遭遇归属不符")
        run_id, pool, tier, version, snapshot = row
        recorded = db.execute("SELECT 1 FROM projection_claim WHERE encounter_id=?",(encounter_id,)).fetchone()
        if not recorded:
            if version != catalog["catalogVersion"]:
                raise ValueError("请按遭遇中的版本载入奖励规则")
            db.execute("INSERT INTO projection_claim VALUES (?,?)",(encounter_id,run_id))
            creation = catalog["creation"]
            kinds = {entry["catalogPath"]:int(k) for k,entry in creation["kinds"].items()}
            item_kind, method_kind = kinds["itemTemplates"], kinds["methods.templates"]
            gear = json.loads(snapshot)
            carried = {ref["templateId"]: ref["revision"] for ref in gear["equipment"].values()}
            learned = {ref["templateId"]: ref["revision"] for ref in gear["methods"].values()}
            for entry_id, entry in catalog["characters"]["rewardEntries"][str(pool)].items():
                item = creation["idsByKey"][str(item_kind)][entry["itemId"]]
                method = entry["methodId"]
                if (method and method not in learned) or (not method and item not in carried):
                    continue
                if rng.randrange(10000) >= entry["chanceBps"][str(tier)]:
                    continue
                embedded = (None,None,None)
                if method:
                    embedded = (method_kind,method,learned[method])
                    carrier = db.execute("""SELECT revision FROM content_revision WHERE kind_id=? AND template_id=?
                        AND catalog_version=? AND state IN (1,2) ORDER BY revision DESC LIMIT 1""",
                        (item_kind,item,version)).fetchone()
                    if carrier is None:
                        raise ValueError("奖励所需的秘籍载体版本尚未入库")
                    revision = carrier[0]
                else:
                    revision = carried[item]
                qty = rng.randint(entry["minQty"],entry["maxQty"])
                db.execute("INSERT INTO projection_grant VALUES (?,?,?,?,?,?,?,?,?,?)",
                           (encounter_id,run_id,int(entry_id),item_kind,item,revision,qty,*embedded))
        return db.execute("""SELECT reward_slot,kind_id,template_id,revision,quantity,
            embedded_kind,embedded_id,embedded_revision FROM projection_grant WHERE encounter_id=?
            ORDER BY reward_slot""",(encounter_id,)).fetchall()
