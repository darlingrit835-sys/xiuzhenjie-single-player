using System;
using System.Linq;

namespace XiuZhenJie
{
    internal sealed class VoidTempleReading
    {
        private readonly VoidFacilityPage page;
        private readonly Action close, legacy;
        private readonly Action<string> workshop,readWork;
        private VoidFacilityPage.Hall[] halls;
        internal VoidTempleReading(Window owner,Action quit,Action oldDoors,Action<string> enter,Action<string> read)
        {
            page=new VoidFacilityPage(owner,false);close=quit;legacy=oldDoors;workshop=enter;readWork=read;
        }
        internal void Hide()=>page.Hide();
        internal void Home()
        {
            page.Begin("虚神殿","百工经义 · 当值授业",close);
            page.Fetch(new VoidFacilityPage.Query{action="temple"},r=>{
                halls=r.halls??Array.Empty<VoidFacilityPage.Hall>();
                page.Copy("已收经印  "+halls.Sum(x=>x.seals)+" / 27");
                page.Action("问经台　·　选馆研习",Directory,true);
                page.Action("我的谱册　·　查阅亲署作品",()=>Portfolio(0));
                page.Action("七门旧殿　·　原有设施",()=>{page.Hide();legacy();});
            });
        }
        private void Directory()
        {
            page.Begin("问经台","九馆藏经 · 选一门读卷",Home);
            foreach(var h in halls){
                var catalog=VoidRealmCatalog.Find(h.id);if(catalog==null)continue;
                page.Action(catalog.hall+"　"+h.seals+"/3印　·　"+(h.rank>0?VoidRealmCatalog.Rank(h.rank):"旁听"),()=>Hall(h),h.rank>0);
            }
        }
        private void Hall(VoidFacilityPage.Hall hall)
        {
            var book=VoidRealmCreationCatalog.Find(hall.id);var h=VoidRealmCatalog.Find(hall.id);
            page.Begin(h.hall+"经席",book.product+" · 工艺与辨识",Directory);
            page.Copy(book.mechanic);
            for(int i=0;i<book.briefs.Length;i++){
                int brief=i;page.Action(book.briefs[i].name+"　·　开卷",()=>Lecture(hall,brief));
            }
            if(hall.rank>0)page.Action("执印入工坊　·　"+VoidRealmCatalog.Rank(hall.rank),()=>workshop(hall.id),true);
            else page.Copy("可旁听研习。创作本馆作品需"+h.job+"职印。");
        }
        private void Lecture(VoidFacilityPage.Hall hall,int brief)
        {
            page.Begin("开卷问经",VoidRealmCatalog.Find(hall.id).hall,()=>Hall(hall));
            page.Fetch(new VoidFacilityPage.Query{action="lecture",hall=hall.id,brief=brief},r=>{
                page.Begin(r.name,r.passed?"本篇经印已收录":"温经 · 读卷后合卷辨义",()=>Hall(hall));
                page.Copy(r.hint);page.Copy(r.teaching);
                page.Action("合卷辨义",()=>Question(hall,brief,r),true);
            });
        }
        private void Question(VoidFacilityPage.Hall hall,int brief,VoidFacilityPage.Reply lecture)
        {
            page.Begin(lecture.name,"合卷辨义",()=>Lecture(hall,brief));page.Copy(lecture.question);
            for(int i=0;i<lecture.choices.Length;i++){
                int answer=i+1;
                page.Action(lecture.choices[i],()=>page.Fetch(new VoidFacilityPage.Query{action="seal",hall=hall.id,brief=brief,answer=answer},r=>{
                    page.Begin(r.correct?"经印录成":"经义尚有偏差",lecture.name,()=>Lecture(hall,brief));
                    page.Copy(r.correct?"此篇已记入角色殿籍。":r.question);
                    if(r.correct){
                        page.Copy(r.teaching);
                        page.Action("返回殿中览印",Home,true);
                        page.Action("研读下一篇",()=>Lecture(hall,(brief+1)%3));
                        if(hall.rank>0)page.Action("进工坊应用所学",()=>workshop(hall.id));
                    }else page.Action("展卷复读",()=>Lecture(hall,brief),true);
                }));
            }
        }
        private void Portfolio(int index)
        {
            page.Begin("我的谱册","亲署原作 · 按落印时间倒序",Home);
            page.Fetch(new VoidFacilityPage.Query{action="folio",page=index},r=>{
                page.Copy("亲署 "+r.count+" 卷　·　第 "+(index+1)+" 页");
                foreach(var work in r.works??Array.Empty<VoidFacilityPage.Work>()){
                    var h=VoidRealmCatalog.Find(work.hallId);
                    page.Action("《"+work.name+"》　"+(h?.hall??"")+" · "+work.score+"分",()=>readWork(work.id));
                }
                if(r.count==0)page.Copy("在本职工坊完成推演并题名落印，作品会收录在这里。");
                if(index>0)page.Action("上一页",()=>Portfolio(index-1));
                if(r.more)page.Action("下一页",()=>Portfolio(index+1));
            });
        }
    }
}
