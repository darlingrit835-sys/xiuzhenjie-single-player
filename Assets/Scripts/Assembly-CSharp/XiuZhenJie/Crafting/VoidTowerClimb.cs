using System;
using UnityEngine;

namespace XiuZhenJie
{
    internal sealed class VoidTowerClimb
    {
        private static readonly string[] Realms={"结丹","元婴","化神","炼虚","合体","渡劫","大乘"};
        private readonly VoidFacilityPage page;
        private readonly Action close;
        private readonly Action<int> challenge;
        internal VoidTowerClimb(Window owner,Action quit,Action<int> start){
            page=new VoidFacilityPage(owner,true);close=quit;challenge=start;
        }
        internal void Cancel()=>page.Cancel();
        internal void Home()
        {
            page.Begin("虚神塔","镇守虚境 · 挑战塔层",close);
            page.Fetch(new VoidFacilityPage.Query{action="tower"},r=>{
                page.Copy(Realms[Mathf.Clamp(r.stage-3,0,6)]+"境　·　"+(r.best>0?"最高过关 "+r.best+" 层":"尚无过关记录"));
                page.Action("挑战第 "+r.next+" 层",()=>Brief(r),true);
                page.Action("道境榜　·　同境最高塔层",()=>Rank(r.stage,false));
                page.Action("天骄榜　·　过关时最年轻的修士",()=>Rank(r.stage,true));
            });
        }
        private void Brief(VoidFacilityPage.Reply record)
        {
            page.Begin("第 "+record.next+" 层","虚神塔 · 入阵",Home);
            page.Copy("击败本层全部守影后，塔中会记下成绩。可继续登塔，也可返城后再来。");
            page.Copy("结丹自第一层起，每升一重大境界，起始塔层提高四十层。塔中禁用符箓。");
            page.Action("入塔迎战",()=>challenge(record.next),true);
        }
        private void Rank(int stage,bool age)
        {
            page.Begin(age?"天骄榜":"道境榜",Realms[stage-3]+"境 · "+(age?"四十层关口":"闯塔层数"),Home);
            page.Fetch(new VoidFacilityPage.Query{action="tower",stage=stage,mode=age?"age":"height"},r=>{
                page.Copy(age?"闯过第 "+((stage-2)*40)+" 层时，按过关年龄由小到大排列。":"同一境界内，按实际通关的最高层排列。");
                page.Action("换查境界　·　"+Realms[stage-3],()=>PickStage(age));
                page.Copy(r.mine>0?"本人第 "+r.mine+" 名":"本人尚未入榜");
                var rows=r.ranks??Array.Empty<VoidFacilityPage.Rank>();
                if(rows.Length==0)page.Copy("本境尚无修士留下成绩。");
                for(int i=0;i<rows.Length;i++){
                    var row=rows[i];
                    page.Copy("第 "+(i+1)+" 名　"+row.Name+"　·　"+row.TierNum+" 层"+(age?"　"+row.Age+" 岁":""));
                }
            });
        }
        private void PickStage(bool age)
        {
            page.Begin("按境查榜",age?"天骄榜":"道境榜",Home);
            for(int i=0;i<Realms.Length;i++){int stage=i+3;page.Action(Realms[i],()=>Rank(stage,age));}
        }
    }
}
