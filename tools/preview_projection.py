"""在临时内存 SQLite 中用一名预设修士模拟玩家通关、留影和另一位玩家入场。"""

import argparse
import json
from pathlib import Path
import random
import sqlite3

if __package__:
    from .history_projection import capture_clear, draw, rule_for, settle_victory
    from .preview_region import CONFIG, read_json
else:
    from history_projection import capture_clear, draw, rule_for, settle_victory
    from preview_region import CONFIG, read_json


SCHEMA = Path(__file__).resolve().parents[1] / "server/data/schema"


def open_demo(catalog, locale, character_id):
    db = sqlite3.connect(":memory:")
    for name in ("player_content", "history_projection"):
        db.executescript((SCHEMA / (name + ".sql")).read_text(encoding="utf-8"))
    creation = catalog["creation"]
    kinds = {entry["catalogPath"]:int(key) for key, entry in creation["kinds"].items()}
    for key, row in creation["kinds"].items():
        db.execute("INSERT INTO content_kind VALUES (?,?,?)", (int(key), row["creatable"], row["needsPrototype"]))
    loadout = {"methods": {}, "equipment": {}}
    for group, domain in (("methods", "methods.templates"), ("equipment", "itemTemplates")):
        kind = kinds[domain]
        table = locale["methods"]["templates"] if group == "methods" else locale["itemTemplates"]
        source = catalog["methods"]["templates"] if group == "methods" else catalog["itemTemplates"]
        for native, definition in source.items():
            tid = creation["idsByKey"][str(kind)][native]
            db.execute("INSERT INTO content_template VALUES (?,?,0)", (kind,tid))
            db.execute("INSERT INTO content_revision VALUES (?,?,1,0,?,?,?)",
                       (kind,tid,table[native]["name"],catalog["catalogVersion"],
                        json.dumps(definition, separators=(",", ":"))))
            db.execute("UPDATE content_revision SET state=1 WHERE kind_id=? AND template_id=?", (kind,tid))
        inventory = catalog["characters"][group][str(character_id)]
        for slot, native in inventory.items():
            tid = creation["idsByKey"][str(kind)][str(native)]
            loadout[group][slot] = {"templateId": tid, "revision": 1}
    db.commit()
    return db, loadout


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--map", default="1111W11", help="战区地图键")
    parser.add_argument("--character", type=int, default=81001, help="模拟角色采用的人物模板")
    parser.add_argument("--seed", type=int, default=31, help="复现实验用随机种子")
    parser.add_argument("--victory", action="store_true", help="模拟战报已确认胜利并打印一次奖励回执")
    args = parser.parse_args()
    catalog = read_json(CONFIG)
    locale = read_json(CONFIG.with_name(CONFIG.stem + ".zh-CN.json"))
    if catalog["catalogVersion"] != locale["catalogVersion"]:
        parser.error("运行包和中文包版本不匹配")
    map_id = catalog["encounterMapIds"].get(args.map)
    key = str(args.character)
    if not map_id or key not in catalog["characters"]["templates"]:
        parser.error("找不到指定战区或人物")
    _, rule = rule_for(catalog, map_id)
    if rule is None:
        parser.error("此战区未配置历史投影")
    db, loadout = open_demo(catalog, locale, args.character)
    try:
        actor = catalog["characters"]["templates"][key]
        name = locale["characters"]["templates"][key]["name"]
        capture_clear(db, catalog, 1001, 11, map_id, 100, name, actor["level"], catalog["characters"]["panels"][key],
                      loadout)
        encounter = draw(db, catalog, 2001, 22, map_id, 101, random.Random(args.seed))
        print(f"{args.map}　投影出现率 {rule['chanceBps']/100:.2f}%　保留 {rule['hours']} 小时")
        if encounter is None:
            print("本次未遇到历史投影。")
            return
        panel = db.execute("SELECT public_name,level,hp,attack,defense FROM projection_run WHERE run_id=?",
                           (encounter,)).fetchone()
        print(f"遇到 {panel[0]} 的历史投影　等级 {panel[1]}　气血 {panel[2]}　攻击 {panel[3]}　防御 {panel[4]}")
        print(f"来源战报 {encounter}　功法 {len(loadout['methods'])} 部　随身器物 {len(loadout['equipment'])} 件")
        print("本场奖励池 " + locale["characters"]["pools"][str(rule["poolId"])]["name"])
        if args.victory:
            grants = settle_victory(db, catalog, 2001, 22, random.Random(args.seed + 1))
            print(f"胜利回执已保存，共 {len(grants)} 项奖励")
            for _, kind, template, revision, quantity, _, method, method_version in grants:
                key = catalog["creation"]["templates"][str(kind)][str(template)]["key"]
                name = locale["itemTemplates"][key]["name"]
                if method:
                    name += f"（{locale['methods']['templates'][str(method)]['name']} v{method_version}）"
                print(f"  {name} ×{quantity}　模板 {template} v{revision}")
    finally:
        db.close()


if __name__ == "__main__":
    main()
