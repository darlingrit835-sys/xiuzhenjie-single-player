"""按战区、实际可遇选项和角色数值预览单个怪物。"""
import argparse
import json

if __package__:
    from .monster_stats import incoming_hit, monster_panel
    from .preview_region import CONFIG, read_json
else:
    from monster_stats import incoming_hit, monster_panel
    from preview_region import CONFIG, read_json


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--map", required=True, help="战区地图键，例如 1111W11")
    parser.add_argument("--monster", required=True, help="数字物种ID或中文名")
    parser.add_argument("--tier", type=int, default=1, help="普通1、精英3、兽王4")
    parser.add_argument("--level", type=int, default=1, help="角色等级，决定缺省神识、修为和灵石基准")
    parser.add_argument("--perception", type=int, help="实际角色神识点")
    parser.add_argument("--super-air", type=int, help="当前修为点，默认取本级满进度")
    parser.add_argument("--variance-bps", type=int, default=0, help="选择面板浮动，当前范围 ±600 基点")
    parser.add_argument("--difficulty-bps", type=int, default=10000, help="服务器难度的基点倍率")
    parser.add_argument("--hit", type=int, nargs=3, metavar=("伤害", "五行ID", "气ID"), help="预览一次受击修正")
    parser.add_argument("--json", action="store_true")
    args = parser.parse_args()
    try:
        cfg = read_json(CONFIG)
        loc = read_json(CONFIG.with_name(CONFIG.stem+".zh-CN.json"))
        if not cfg.get("catalogVersion") or cfg["catalogVersion"] != loc.get("catalogVersion") or "monsterStats" not in cfg:
            raise ValueError("需要版本一致且包含怪物数值的运行配置、中文资源")
        query = args.monster
        if query not in cfg["monsterSpecies"]:
            matches = [key for key, row in loc["monsterSpecies"].items() if row["name"] == query]
            if len(matches) != 1:
                raise ValueError("怪物名称未登记或对应多个模板，请使用图鉴中的数字物种ID")
            query = matches[0]
        result = monster_panel(cfg, query, args.map, args.tier, args.level, perception=args.perception,
                               super_air=args.super_air, variance_bps=args.variance_bps, server_difficulty_bps=args.difficulty_bps)
        if args.hit:
            result["hit"] = incoming_hit(cfg, query, *args.hit)
        if args.json:
            print(json.dumps(result, ensure_ascii=False, indent=2))
            return
        name = loc["monsterSpecies"][query]["name"]
        rank = loc["monsterStats"]["tiers"][str(args.tier)]["name"]
        print(f"{args.map}　{name}（{query}）　{rank}　等级 {args.level}")
        panel = result["panel"]
        print(f"气血 {panel['hp']}　护甲 {panel['armor']}　攻击 {panel['attack']}　法力 {panel['mana']}　神识 {panel['perception']}")
        print(f"移速 {panel['speedMilli']/1000:g}　攻击距离 {panel['rangeMilli']/1000:g}　灵石 {panel['gold']}")
        terms = [f"{loc[dictionary][target]['name']}{value/100:+.2f}%"
                 for domain, dictionary in (("1", "elements"), ("2", "qiTypes"))
                 for target, value in result["resists"][domain].items()]
        print("受击抗性　" + "　".join(terms))
        for skill in result["skills"]:
            name = loc["monsterStats"]["skills"][str(skill["id"])]["name"]
            print(f"{name}　基础值 {skill['baseDamage']}　施法 {skill['castMs']} 毫秒　冷却 {skill['cooldownSec']} 秒　持续 {skill['durationMs']} 毫秒")
        if args.hit:
            print(f"来袭 {args.hit[0]} 点　抗性修正后 {result['hit']['damage']} 点")
    except (KeyError, ValueError) as exc:
        parser.error(str(exc))


if __name__ == "__main__":
    main()
