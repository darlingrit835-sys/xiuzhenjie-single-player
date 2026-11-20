"""核算打完一个战区的若干波位后，各种特色材料的期望份数。"""

import argparse
from collections import defaultdict
from decimal import Decimal, localcontext
from fractions import Fraction
import json
from pathlib import Path

if __package__:
    from .bestiary_tables import map_ecology_summary, TIERS
    from .preview_region import CONFIG, read_json
else:
    from bestiary_tables import map_ecology_summary, TIERS
    from preview_region import CONFIG, read_json


def decimal_value(ratio):
    return Decimal(ratio.numerator) / Decimal(ratio.denominator)


def map_yield(catalog, locale, map_key, kills=None):
    if not catalog.get("catalogVersion") or catalog["catalogVersion"] != locale.get("catalogVersion"):
        raise ValueError("运行配置与中文资源版本不一致")
    if map_key not in catalog["encounterMapIds"]:
        raise ValueError(f"未登记的战斗子图 {map_key}")
    zone = catalog["encounterMaps"][str(catalog["encounterMapIds"][map_key])]
    slots = catalog["encounterWaves"][str(zone["waveId"])]
    full = sum(slots.values())
    count = full if kills is None else kills
    if type(count) is not int or not 0 <= count <= full:
        raise ValueError(f"本轮可击败的波位数为 0～{full}")

    quantities = defaultdict(Fraction)
    remaining = count
    killed = {}
    with localcontext() as ctx:
        ctx.prec = 40
        no_drop = Decimal(1)
        for tier in TIERS:  # 与原波序一致，先普通，再精英，最后兽王。
            taken = min(slots[str(tier)], remaining)
            killed[str(tier)] = taken
            remaining -= taken
            if not taken:
                continue
            encounter = map_ecology_summary(catalog, locale, map_key, tier)
            total = encounter["monsterTickets"]
            if not total:
                raise ValueError(f"{map_key} 的 {tier} 档有波位却没有怪物候选")
            empty_slot = Fraction()
            for monster in encounter["monsters"]:
                share = Fraction(monster["tickets"], total)
                empty_monster = Fraction(1)
                for item in monster["drops"]:
                    chance = Fraction(item["chanceBps"], 10000)
                    average = Fraction(item["minQty"] + item["maxQty"], 2)
                    quantities[item["itemId"]] += taken * share * chance * average
                    empty_monster *= 1 - chance
                empty_slot += share * empty_monster
            no_drop *= decimal_value(empty_slot) ** taken
    return {"mapKey": map_key, "fullWaveSize": full, "kills": count, "slotsKilled": killed,
            "noSpecialDropProbability": str(no_drop),
            "expectedQuantities": {key: {"numerator": qty.numerator, "denominator": qty.denominator}
                                   for key, qty in sorted(quantities.items())}}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--map", required=True, dest="map_key", help="战斗子图地图键，如 1111W11")
    parser.add_argument("--kills", type=int, help="击败本轮前 N 个波位，省略时计算整轮")
    parser.add_argument("--config", type=Path, default=CONFIG)
    parser.add_argument("--locale", type=Path)
    parser.add_argument("--json", action="store_true", help="按材料模板导出期望份数的精确分子、分母")
    args = parser.parse_args()
    try:
        catalog = read_json(args.config)
        locale = read_json(args.locale or args.config.with_name(args.config.stem + ".zh-CN.json"))
        report = map_yield(catalog, locale, args.map_key, args.kills)
    except (OSError, KeyError, ValueError) as err:
        parser.error(str(err))
    if args.json:
        print(json.dumps(report, ensure_ascii=False, indent=2))
        return
    slots = report["slotsKilled"]
    print(f"{args.map_key}　击败 {report['kills']}／{report['fullWaveSize']} 个波位"
          f"（普通 {slots['1']}、精英 {slots['3']}、兽王 {slots['4']}）")
    for key, ratio in report["expectedQuantities"].items():
        average = decimal_value(Fraction(**ratio))
        print(f"  {locale['itemTemplates'][key]['name']}　{average:.6f} 份／本次")
    print(f"本次没有特色材料的概率　{Decimal(report['noSpecialDropProbability']) * 100:.2f}%")


if __name__ == "__main__":
    main()
