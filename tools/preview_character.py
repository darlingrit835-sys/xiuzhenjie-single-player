"""查询命名修士、怪物或郡战区的稀有战利品。"""

import argparse
import json
from pathlib import Path

if __package__:
    from .character_tables import reward_options
else:
    from character_tables import reward_options


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    source = parser.add_mutually_exclusive_group()
    source.add_argument("--character", help="人物编号或完整中文姓名")
    source.add_argument("--monster", type=int, help="物种编号")
    parser.add_argument("--map", help="真实战区地图键（例如 1111W11）")
    parser.add_argument("--tier", type=int, choices=(1, 3, 4), required=True, help="1普通、3精英、4首领")
    parser.add_argument("--json", action="store_true")
    args = parser.parse_args()
    root = Path(__file__).resolve().parents[1] / "server/data/config"
    runtime, locale = (json.loads((root / f"cultivation_affinities{s}.json").read_text(encoding="utf-8"))
                       for s in ("", ".zh-CN"))
    if runtime["catalogVersion"] != locale["catalogVersion"]:
        parser.error("运行配置与中文包版本不配套")
    labels = locale["characters"]["templates"]
    char = args.character
    if char and not char.isdecimal():
        matches = [k for k, attrs in labels.items() if attrs["name"] == char]
        if len(matches) != 1:
            parser.error("没有唯一匹配的修士，请改用人物编号")
        char = matches[0]
    zone = runtime["encounterMapIds"].get(args.map) if args.map else None
    if args.map and zone is None:
        parser.error("战区地图键不存在")
    try:
        result = reward_options(runtime, args.tier, character_id=char, monster_id=args.monster, map_id=zone)
    except ValueError as err:
        parser.error(str(err))
    for item in result["entries"]:
        if item["kindId"] == 1:
            item["name"] = locale["methods"]["templates"][str(item["methodId"])]["name"] + "摹本"
        else:
            item["name"] = locale["itemTemplates"][item["itemId"]]["name"]
    if char:
        result["character"] = {"id": int(char), **labels[str(char)], **runtime["characters"]["templates"][str(char)],
                               "panel": runtime["characters"]["panels"][str(char)],
                               "mapsByTier": runtime["characters"]["mapsByCharacter"][str(char)]}
    if args.json:
        print(json.dumps(result, ensure_ascii=False, indent=2))
        return
    if char:
        info = result["character"]
        print(f"{info['title']}·{info['name']}（{char}，{info['level']}级）")
    pool = locale["characters"]["pools"].get(str(result["poolId"]))
    print(f"奇遇宝库：{pool['name'] if pool else '无'}，档位 {args.tier}")
    for item in result["entries"]:
        print(f"  {item['name']}　{item['chanceBps']/100:.2f}%　×{item['minQty']}～{item['maxQty']}")


if __name__ == "__main__":
    main()
