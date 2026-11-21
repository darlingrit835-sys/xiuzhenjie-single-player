"""从运行配置导出可直接从本地文件打开的 09 炼丹画板数据。"""

import argparse
import json
from pathlib import Path

from region_ecology import region_ecology_summary

ROOT = Path(__file__).resolve().parents[1]
CONFIG = ROOT / "server/data/config"
TARGET = ROOT / "design/修真界UI/source/alchemy-catalog.js"
TIERS = (1, 3, 4)


def load(name):
    return json.loads((CONFIG / name).read_text(encoding="utf-8"))


def build():
    data = load("cultivation_affinities.json")
    locale = load("cultivation_affinities.zh-CN.json")
    if not data["catalogVersion"] or data["catalogVersion"] != locale["catalogVersion"]:
        raise ValueError("运行配置和中文说明版本不一致")
    alchemy = data["alchemy"]
    economy = alchemy["economy"]
    labels = locale["alchemy"]
    materials = {}
    for key in data["systemResources"]["alchemy"]:
        item = data["itemTemplates"][key]
        template = locale["itemTemplates"][key]
        materials[key] = {
            "name": template["name"], "element": data["profiles"][item["profileId"]]["primaryElementId"],
            **economy["materials"][key], "fieldSell": 10 + 5 * item["gradeId"], "gather": [], "drops": [],
            "legacy": data["resourceSources"].get(key, {}).get("legacyTypes", []),
        }
    for region in sorted(data["regionMonsters"]):
        county = locale["regions"][region]["name"]
        by_monster = {}
        for i, tier in enumerate(TIERS):
            summary = region_ecology_summary(data, locale, region, tier)
            if i == 0:
                for group in summary["gathering"]:
                    if int(group["methodId"]) not in (1, 2):
                        continue
                    for item in group["items"]:
                        if item["itemId"] in materials:
                            materials[item["itemId"]]["gather"].append({
                                "place": county, "method": group["name"],
                                "bps": round(item["tickets"] * 10000 / group["totalTickets"]),
                                "min": item["minQty"], "max": item["maxQty"],
                            })
            for monster in summary["monsters"]:
                for drop in monster["drops"]:
                    key = drop["itemId"]
                    if key not in materials:
                        continue
                    row = by_monster.setdefault((key, monster["monsterId"]), {
                        "place": county, "name": monster["name"], "bps": [0, 0, 0],
                        "min": drop["minQty"], "max": drop["maxQty"],
                    })
                    row["bps"][i] = drop["chanceBps"]
        for (key, _), row in by_monster.items():
            materials[key]["drops"].append(row)
    book = {}
    for key, recipe in sorted(alchemy["recipes"].items(), key=lambda row: (row[1]["minRank"], row[0])):
        out = recipe["outputId"]
        effect = labels["effects"][recipe["effectId"]]
        book[key] = {
            "id": key, "name": locale["itemTemplates"][out]["name"], "main": recipe["primaryId"],
            "mainQty": data["creationInputs"][out][recipe["primaryId"]], "rank": recipe["minRank"],
            "base": recipe["successBps"], "minQty": recipe["minQty"], "maxQty": recipe["maxQty"],
            "cap": recipe["yieldCap"], "strength": recipe["baseEffect"], "effect": effect["name"],
            "basis": recipe.get("effectBasis", "flat"), "poison": recipe.get("poisonCost", 0),
            "note": labels["recipes"][key]["note"],
            "prices": economy["recipes"][key]["sellPrices"],
        }
    levels = load("xzjrolelevellist.json")
    realms = [{"name": row["level"], "air": row["upgradeLimit"],
               "blood": max(100, int(row["perception"] * 0.01 + .5))}
              for row in levels if row["id"] % 10 == 1 or row["id"] == levels[-1]["id"]]
    fire = "flame.ember"
    return {
        "materials": materials, "book": book, "realms": realms, "elements": data["elements"],
        "flame": alchemy["flames"][fire], "flameElement": data["forgeFactors"]["flames"][fire]["elementId"],
        "ranks": alchemy["ranks"], "relations": alchemy["relations"], "bands": alchemy["potencyByQuality"],
        "limits": alchemy["limits"], "economy": {k: v for k, v in economy.items() if k not in ("recipes", "materials")},
    }


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true")
    args = parser.parse_args()
    output = ("/* 由 tools/export_alchemy_board.py 从游戏配置生成。 */\nwindow.alchemyBoardCatalog = "
              + json.dumps(build(), ensure_ascii=False, indent=1) + ";\n")
    if args.check:
        if not TARGET.exists() or TARGET.read_text(encoding="utf-8") != output:
            parser.exit(1, "09 炼丹画板数据需要重新导出\n")
    else:
        TARGET.write_text(output, encoding="utf-8")


if __name__ == "__main__":
    main()
