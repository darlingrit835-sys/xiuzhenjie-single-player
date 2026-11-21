"""生成地火丹房的实装药力、由失败率折算的用料成本和药材寻获表。"""
import argparse
from math import ceil
from pathlib import Path

from preview_alchemy import brew_plan, item_element, read_json, trunc_ratio
from preview_region import CONFIG, element_relation
from region_ecology import habitat_weight


def estimate(catalog, locale, recipe, score):
    items = catalog["creationInputs"][recipe["outputId"]]
    primary = recipe["primaryId"]
    aux = {k: n for k, n in items.items() if k != primary}
    units = sum(aux.values())
    values = {"generates": 100, "same": 60, "generatedBy": 35, "overcomes": -65, "overcomeBy": -90, "neutral": 0}
    main_element = item_element(catalog, primary)
    total = 0
    for key, quantity in aux.items():
        element = item_element(catalog, key)
        if element and main_element:
            total += quantity * values[element_relation(catalog["elements"], element, main_element)]
    affinity = trunc_ratio(total, units) if units else 0
    rank = recipe["minRank"]
    plan = brew_plan(catalog, locale, recipe, rank, "flame.ember")
    bonuses = plan["adjustments"]
    base = recipe["successBps"] + sum(i["successBps"] for i in bonuses) + 8 * affinity
    chance = max(500, min(catalog["alchemy"]["limits"]["successMaxBps"], base + (score - 60) * 32)) / 10000
    count = sum(n * tickets for n, tickets in plan["yieldTickets"].items()) / plan["yieldTotalTickets"]
    quality_score = score + 2 * rank + trunc_ratio(affinity, 12)
    quality = 3 if quality_score >= 90 else 2 if quality_score >= 65 else 1
    lo, hi = catalog["alchemy"]["potencyByQuality"][str(quality)]
    pos = max(0, min(100, 45 + trunc_ratio(affinity, 2) + units * 3 +
                     trunc_ratio(sum(i["potencyBps"] for i in bonuses), 120)))
    dose = (recipe["baseEffect"] * (lo + (hi-lo)*pos//100) + 5000)//10000
    prices = catalog["alchemy"]["economy"]["materials"]
    cost = sum(prices[k]["value"]*n for k, n in items.items())
    return chance, count, cost, dose, quality


def sources(catalog, locale, item):
    rows = catalog["resourceSources"][item]
    details = []
    candidates = []
    for region, method in rows.get("gather", []):
        place = str(region)
        pool = catalog["regionResources"][place][str(method)]
        def weight(k):
            profile = catalog["profiles"][catalog["itemTemplates"][k]["profileId"]]
            return habitat_weight(pool[k]["weight"], catalog["regions"][place], profile, catalog["elements"],
                                  catalog["ecologyAffinity"]["resource"])[0]
        chance = weight(item) / sum(weight(k) for k in pool)
        a, b = pool[item]["minQty"], pool[item]["maxQty"]
        candidates.append((chance*(a+b)/2, f'{locale["regions"][place]["name"]}{locale["gatherMethods"][str(method)]["name"]}，{chance:.1%} 出 {a}～{b} 份'))
    if candidates:
        details.append(max(candidates)[1])
    drops = []
    for monster, scope in rows.get("drop", []):
        mid = str(monster)
        for region in ([scope] if scope else catalog["monsterHabitats"][mid]):
            place = str(region)
            table = catalog["monsterDrops"][mid]
            rule = table.get(place, {}).get(item) or table.get("0", {})[item]
            normal, elite, boss = (rule["chanceBps"][v]/10000 for v in ("1","3","4"))
            boss_chance = f"／首领 {boss:.1%}" if catalog["monsterSpecies"][mid]["allowBoss"] else ""
            drops.append((normal*(rule["minQty"]+rule["maxQty"])/2,
                          f'{locale["regions"][place]["name"]}·{locale["monsterSpecies"][mid]["name"]}，普通 {normal:.1%}／精英 {elite:.1%}{boss_chance}'))
    if drops:
        details.append(max(drops)[1])
    if rows.get("legacyTypes"):
        details.append("历练通用掉落" + ("，清场另得一份" if item == "mat.common_core" else ""))
    garden = {"mat.ninglu_herb":"凝露草种 12／60 秒","mat.frost_reed":"霜节芦种 28／300 秒","mat.dream_lotus":"梦泽莲种 54／900 秒"}
    if item in garden:
        details.append("灵田 "+garden[item])
    if item == "mat.cold_dew":
        details.append("符案用凝露草 ×3 取露 ×1")
    return "；".join(details)


def listing_price(cost):
    """按一粒首次成交、无称号优惠计入挂牌费，向上找到首个可回本的单价。"""
    price = max(100, ceil(cost))
    while price - max(1, price // 10) < cost:
        price += 1
    return price


def report(catalog, locale):
    alchemy = catalog["alchemy"]
    econ = alchemy["economy"]
    recipes = alchemy["recipes"]
    title = lambda i: locale["itemTemplates"][i]["name"]
    order = sorted(recipes, key=lambda i:(recipes[i]["minRank"], title(recipes[i]["outputId"])))
    out = ["# 地火丹房收支与用药", "",
           "同一张丹方只锁主药。下表使用丹方簿的推荐辅材，纳戒可以任意换配或只用主药。材料价格、药柜可购范围、回收档位与丹毒均由基础表导出。",
           "", "修为栏列出药品封存的基础药力。服用时按当前人物等级折算。1–40 级保留全额，41–79 级除以 100^((等级−40)/40)，80–89 级除以 100。实际加到本层修为门槛为止。90 级保留丹药。已经制作、交易或囤积的丹也按服用者当时的等级结算。补血、回精、解毒和护盾仍按各自公式计算。", "", "## 药效与用料", "",
           "| 丹阶 | 丹方 | 主药 | 推荐辅材 | 单粒基础药效 | 丹毒 | 适用场景 |",
           "| ---: | --- | --- | --- | --- | ---: | --- |"]
    for k in order:
        r = recipes[k]
        mix = catalog["creationInputs"][r["outputId"]]
        main = r["primaryId"]
        aux = "、".join(f"{title(i)} ×{n}" for i,n in mix.items() if i!=main) or "可空置"
        value, typ = r["baseEffect"], r["effectBasis"]
        fx = locale["alchemy"]["effects"][r["effectId"]]["name"]
        detail = f"{fx} {value} 点" if typ=="flat" else f"{'本层突破门槛' if typ=='realm_air' else '自身气血上限'}的 {value/100:g}%"
        if r["effectId"] == "absorb_damage":
            detail = f"战斗气血的 {min(48, value/20):g}%"
        out.append(f'| {r["minRank"]} | {title(r["outputId"])} | {title(main)} ×{mix[main]} | {aux} | {detail} | {r["poisonCost"]} | {locale["alchemy"]["recipes"][k]["note"]} |')
    out += ["", "## 每炉成本与出丹", "",
            "基准取刚达到丹方门槛的丹师、该方熟练 0、松炭炉火、未装配纳气功法，三步综合火候 80 分，角色无疗伤或阵师护盾加成。成丹率来自手炼公式。成功粒数对每种基础数量及随机进位加权，再按单炉上限截断。留影列假设已经留下本方丹影，保持相同丹阶与用料。",
            "", "炉料估值 = 各材料份数 × 各自单份参考价之和。手炼每粒成本 = 炉料估值 ÷（成功率 × 成功时平均粒数）。留影每粒成本 = 炉料估值 ÷ 成功时平均粒数。单位为灵石，未给失败炉摊入任何产量。自行寻材的直接灵石支出可以为零。这里以材料估值计其机会成本，不给游玩时间强行折现。",
            "", "| 丹方 | 炉料估值 | 手炼成丹率 | 成功粒/炉 | 手炼估值/粒 | 留影估值/粒 | 品质与每粒药力（修为按上式折算） | 标准回收 下/中/上 |",
            "| --- | ---: | ---: | ---: | ---: | ---: | --- | --- |"]
    for k in order:
        r = recipes[k]
        chance, count, cost, dose, quality = estimate(catalog, locale, r, 80)
        unit = f"{dose} 点" if r["effectBasis"]=="flat" else f"{dose/100:g}%"
        if r["effectId"] == "absorb_damage":
            unit = f"战斗气血的 {min(48, dose/20):g}%"
        reclaim = "/".join(map(str,econ["recipes"][k]["sellPrices"]))
        out.append(f'| {title(r["outputId"])} | {cost} | {chance:.2%} | {count:.2f} | {cost/(chance*count):.1f} | {cost/count:.1f} | {["","下品","中品","上品"][quality]} · {unit} | {reclaim} |')
    out += ["", "## 逐品批炼预算", "",
            "每张新丹方先亲手成功一炉，失败可以重试。已有留影者可直接批炼。下表每品取一张本品丹方，沿用上面的本品、80 分、熟练零与推荐辅材预算。材料为整批实际投入，成品为多批平均粒数。每次批炼一次提交、即时入纳戒，实际耗时包括玩家选择和一次网络往返。", "",
            "| 丹师品阶 | 示例丹方 | 手作成功解锁 | 满批炉数 | 主辅药整批需要 | 整批原料估值 | 预计本批出丹 | 本批耗精力 |", "|---:|---|---:|---:|---|---:|---:|---:|"]
    sample_names=["青脉丹","镇壤护体丹","行气散","铁衣丸","碧髓丹","金关丹","岩甲丹","涤尘丹","太清归元丹"]
    for rank,name in enumerate(sample_names,1):
        r=next(v for v in recipes.values() if title(v["outputId"])==name)
        n=1 if rank<3 else 5 if rank<6 else 10 if rank<9 else 20
        mix=catalog["creationInputs"][r["outputId"]]
        items="、".join(f"{title(i)}×{v*n}" for i,v in mix.items())
        _,amount,cost,_,_=estimate(catalog,locale,r,80)
        out.append(f"| {rank} | {name} | 1炉 | {n} | {items} | {cost*n} | {n*amount:.2f}粒 | {(n+2)//3} |")
    out += ["",
            "手炼每次结算扣 1 精力。成功的丹方熟练 +6，失败 +2。已留影后每实际复炼一炉，丹方熟练 +2、获得按本方难度折算的半份成功丹师经验。三、六、九品分别可一次结算 5、10、20 炉，各炉照付材料，产量分别独立随机，回收按每炉实际主药来源计算。材料不足、精力不足或纳戒满，整笔撤回。", "",
            "标准回收为纳戒卖给系统的单粒档价。整包折扣、赠品和可辨识的旧零价主药会压低该批实际回收；药柜单买主药按真实支出封顶。既有丹药保留原批次的属性。",
            "", "## 卖给玩家，多少灵石回本", "",
            "奇珍阁固定价委托的最低单价为 100 灵石，普通挂牌费为总标价的 10%，向下取整、最低 1 灵石，挂牌时即扣。称号优惠后费率最低 5%。以下按无优惠、单挂一粒、首次挂出即成交计算。回本价覆盖上表的材料估值与挂牌费；若成品的实际系统回收更高，可以直接比较两边的净收入。买家是否愿意出价仍取决于药效与供求。",
            "", "| 丹方 | 手炼回本单价 | 留影回本单价 |", "| --- | ---: | ---: |"]
    for recipe_id in ("dan.ninglu", "dan.taiqing", "dan.xuanyue"):
        r = recipes[recipe_id]
        name = title(r["outputId"])
        chance, count, cost, _, _ = estimate(catalog, locale, r, 80)
        out.append(f"| {name} | {listing_price(cost / (chance * count))} | {listing_price(cost / count)} |")
    high = econ["recipes"]["dan.taiqing"]["sellPrices"][2]
    sale = listing_price(high)
    out += ["", f"例如，自采主药所得的上品太清归元丹标准回收 {high} 灵石。单粒挂 {sale}，扣 {sale // 10} 后净得 {sale - sale // 10}。要超过直接回收的收益，最低挂牌价为 {listing_price(high + 1)} 灵石，且需实际成交。", "", "## 十七味药材的取得与价格", "",
            "参考价用于比较配伍。只有标出单价的材料能在丹房补缺。采集概率按该郡同类节点计算，计入地域亲和；示例路线优先取每个同类节点期望份数较高的郡。猎获概率以已经遇到并击败指定怪物为条件，和地图遇敌概率分开。",
            "", "| 药材 | 参考价/份 | 药柜价/份 | 可实际取得的路线示例 |", "| --- | ---: | ---: | --- |"]
    for item,price in sorted(econ["materials"].items(), key=lambda row:row[1]["value"]):
        out.append(f'| {title(item)} | {price["value"]} | {price["buyPrice"] or "不单卖"} | {sources(catalog,locale,item)} |')
    out += ["", "## 平衡取舍", "",
            "- **小伤与重伤用不同丹。** 初建角色约有 100 气血、100 精力。赤苔续血散适合小伤，凝露丹适合急救。按上限回血的碧髓丹、三华复脉丹，更适合高血量角色。超过缺口的部分不会储存。",
            "- **修为药保留成长空间。** 结丹一层的突破门槛为 403,785。基础药力下，青脉丹增加 120 点，金关丹按 2.10% 增加约 8,479 点。服用后只填到本层突破门槛；炼制消耗两份阳燧晶，服用积 5 丹毒。太清归元丹单粒更强，原料和清毒成本也更高。",
            "- **清毒是选择的成本。** 强效丹只在实际服用获益后累积丹毒。100 为上限，超过上限不能继续服该粒。每粒太清归元丹加 10 丹毒，从零起至多连用十粒；基础止瘴丸清 8，霜脉清毒丹清 24，净髓丹清 47，涤尘丹清 82。护体丹在下场历练生效，重复备盾只保留较强的一层。",
            "- **采集、种植、补料各有用途。** 凝露草在药柜每份 24 灵石。种子每粒 12 灵石，基础耗时 60 秒、收 4 株；用默认青壤则 48 秒收 5 株，每株摊种子钱 2.4 灵石。选择留种会少收两株。镇壤熊爪等稀有材料可在特定郡县狩猎，也可向其他玩家收购。",
            "- **用清场奖励校准消费。** 灵石来源包括历练和宗门，按当前服务端实际结算。单买两份凝露草要 48，连一份寒露液一起补要 84。急用时可买，常炼更适合种植、采集或使用折扣炉材包。不存在统一的玩家成交价。",
            "", "配置变更后运行 `python3 -B tools/alchemy_balance.py --write` 重算本表，运行 `--check` 对比当前配置。"]
    return "\n".join(out)+"\n"


if __name__ == "__main__":
    parser=argparse.ArgumentParser(description=__doc__)
    mode=parser.add_mutually_exclusive_group()
    mode.add_argument("--write",action="store_true")
    mode.add_argument("--check",action="store_true")
    args=parser.parse_args()
    dest=Path(__file__).resolve().parents[1]/"docs"/"丹房收支与用药.md"
    text=report(read_json(CONFIG),read_json(CONFIG.with_name(CONFIG.stem+".zh-CN.json")))
    if args.write:
        dest.write_text(text,encoding="utf-8")
    elif args.check:
        if dest.read_text(encoding="utf-8")!=text:
            raise SystemExit("丹房收支表与配置不一致")
    else:
        print(text,end="")
