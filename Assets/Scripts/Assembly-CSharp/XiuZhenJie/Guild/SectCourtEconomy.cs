using System;
using UnityEngine;

namespace XiuZhenJie
{
    internal sealed partial class SectCourt
    {
        private int upgradePath;

        internal static void SyncStones(long balance)
        {
            var role=Singleton<RoleManager>.Instance.Role_Entity;
            if(role==null || role.ShinyStone==balance) return;
            role.ShinyStone=balance;
            Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
        }

        private void Income()
        {
            Hero("treasury","宗门收益",data.guildName+"　·　"+GradeText+TierText,"收益簿",158);
            Title("个人日俸","stipend");
            Tile("当日灵石俸禄",$"纳戒灵石 {data.walletStones:N0}　·　东八区每日零时换日","stipend",null,data.stipend+" 灵石",true);
            ActionRow(data.stipendClaimed ? "今日俸禄已领" : "领取今日俸禄","stipend",
                () => Order("stipend",data.stipendDay,data.revision,"宗门日俸已入灵石钱包。"),
                Ready && !data.stipendClaimed && !string.IsNullOrEmpty(data.stipendDay));
            Note("每位门人当日可领一次。转宗沿用当日记录，往日未领俸禄不累积。兽潮功勋另计。",44);
            Title("宗库入账","mine");
            Tile("矿砂与灵晶",$"{Plan(data.plan)}排班　矿砂 {data.oreRate:0.##}/时　灵晶 {data.crystalRate:0.##}/时","mine",() => Show(Page.Mine),"矿班");
            Tile("本轮兽潮",$"胜守矿砂 {data.winOre}、兽牙 {data.winFang}　每名参战者 {data.winMerit} 功勋","tide",() => Show(Page.Wave),"战录");
            Title("晋阶收益","building");
            Benefits();
            if (Grade<9) ActionRow("查看两种晋阶法","building",() => Show(Page.Region));
        }

        private void Benefits()
        {
            bool next=Grade<9 && data.nextStage>Grade;
            string change(long current,long future) => next ? $"{current:N0} → {future:N0}" : $"{current:N0}";
            string rate(float current,float future) => next ? $"{current:0.##} → {future:0.##}" : $"{current:0.##}";
            Tile(next ? GradeText+" → "+"一二三四五六七八九"[data.nextStage-1]+"阶" : GradeText+"宗门",
                $"{data.regionKind} · {data.regionName}"+(next ? $"  →  {data.nextRegionKind} · {data.nextRegionName}" : "　·　当前驻域"),
                "region",null,next ? "晋封预览" : "当前",true);
            Note($"当前排班：{Plan(data.plan)}。工位 {change(data.workers,data.nextWorkers)} 人，护宗阵上限 {change(data.formationCap,data.nextFormationCap)} 阶。\n矿砂每时 {rate(data.oreRate,data.nextOreRate)}，灵晶每时 {rate(data.crystalRate,data.nextCrystalRate)}。\n每人每日灵石 {change(data.stipend,data.nextStipend)}。",76);
        }

        private void Advancement()
        {
            Title("宗门晋阶","building");
            if (data.nextStage>0 || Grade==9) Benefits();
            if (Grade==9) { Note("宗门已至九阶，统御本陆。",32); return; }
            bool hasBuild=data.buildOre>0 && data.buildCrystal>0;
            if(hasBuild)
                ChoiceRow(new[] {"护山晋阶","营造晋阶"},new[] {"tide","building"},upgradePath,i=>{upgradePath=i; Render(true);});
            bool build=hasBuild && upgradePath==1;
            long cost=build ? data.buildOre : data.promoteOre;
            long other=build ? data.buildCrystal : data.promoteFang;
            string material=build ? "灵晶" : "兽牙";
            long stock=build ? data.crystal : data.fang;
            Tile("矿砂",$"宗库存有 {data.ore:N0}","ore",null,"需 "+cost);
            Tile(material,$"宗库存有 {stock:N0}"+(build ? "　·　来自矿班开采" : "　·　来自兽潮胜守"),build ? "sigil" : "tide",null,"需 "+other);
            bool arrayReady=!build || data.formation>=data.buildFormation;
            if(build)
                Tile("护宗阵阶",$"当前 {data.formation} 阶　·　由宗主或掌阵长老升阵","formation",() => Show(Page.Formation),"需 "+data.buildFormation+"阶");
            bool enough=data.ore>=cost && stock>=other;
            string method=build ? "营造" : "护山";
            string button=!data.canPromote ? "仅宗主可晋封" : !arrayReady ? "护宗阵阶尚缺" : !enough ? "晋阶资粮尚缺" : "以"+method+"晋阶";
            ActionRow(button,build ? "building" : "gate",
                () => Confirm("promote",build ? "build" : "defense",$"采用{method}法，动用矿砂 {cost}、{material} {other} 晋至第 {Grade+1} 阶？","宗门晋封完成。"),
                Ready && enough && arrayReady && data.canPromote);
            if(!enough)
            {
                if(data.ore<cost || (build && stock<other)) ActionRow("查看矿脉产出","mine",()=>Show(Page.Mine));
                if(!build && stock<other) ActionRow("查看兽牙来源","tide",()=>Show(Page.Wave));
            }
        }
    }
}
