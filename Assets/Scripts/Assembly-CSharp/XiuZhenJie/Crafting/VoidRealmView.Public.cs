using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class VoidRealmView
    {
        private static readonly string[] Tags = {"全部","求教","心得","邀试"};
        private static readonly string[] TopicTags = {"求教","心得","邀试"};
        private static readonly string[] OccupationIcons = {"","LianDan","LianQi","ZhiFu","FaShu","ZhenTu","BuDao","QiaoJiang","LingZhi","YuShou"};
        private int boardPage, boardFilter, topic, rankPage, rankGrade;
        private string rankHall="", rankMode="quality", attachedId, attachedName, noteDraft="";

        private void DrawFacilities()
        {
            var first = Band("殿塔", 48, 8, 8, 3);
            FacilityDoor(first, false, "虚神殿", "百工研习", temple, Jade);
            FacilityDoor(first, true, "虚神塔", "闯塔争榜", tower, new Color32(76,79,104,255));
            var second = Band("坛台", 48, 8, 8, 4);
            FacilityDoor(second, false, "虚神祭坛", "献卷求经", altar, new Color32(134,64,51,255));
            FacilityDoor(second, true, "合道台", "双卷合参", concord, new Color32(103,87,55,255));
        }

        private void FacilityDoor(RectTransform band, bool right, string title, string detail, Action open, Color color)
        {
            var tile = Node(band, title);
            Box(tile, right ? .5f : 0, 0, right ? 1 : .5f, 1, right ? 2 : 0, 0, right ? 0 : 2, 0);
            var surface = Paint(tile, color, true);
            var button = tile.gameObject.AddComponent<Button>();
            button.targetGraphic = surface;
            button.onClick.AddListener(() => open());
            var trim = Node(tile, "描金");
            Box(trim, 0, 0, 0, 1, 3, 4, -5, 4);
            Paint(trim, Gold);
            var name = Caption(tile, title, 17, Ivory, TextAnchor.MiddleLeft, true);
            Box(name.rectTransform, 0, .38f, 1, 1, 12, 0, 4, 1);
            var note = Caption(tile, detail, 11, new Color32(241,221,178,255));
            Box(note.rectTransform, 0, 0, 1, .43f, 12, 2, 4, 0);
        }

        private void ShowHalls()
        {
            tab=0;
            BeginPage("虚神界","九馆论艺");
            DrawFacilities();
            Request(new VoidRealmClient.Order {action="lobby"},reply =>
            {
                lobby=reply;
                BeginPage("虚神界","九馆论艺");
                DrawFacilities();
                var permitted = reply.halls ?? Array.Empty<VoidRealmClient.Hall>();
                var halls = VoidRealmCatalog.Halls.OrderByDescending(h => Array.Exists(permitted,a => a.id==h.id && a.rank>0)).ToArray();
                var first = Array.Find(halls,h => Array.Exists(permitted,a => a.id==h.id && a.rank>0));
                if(first!=null)
                {
                    var brief = VoidRealmCreationCatalog.Find(first.id)?.briefs?.FirstOrDefault();
                    Feature(brief?.name ?? first.work,first.hall+" · 创作论题","揭题入馆",()=>OpenWorkshop(first.id));
                }
                else Feature("九馆名录","虚神殿",null,null);
                Heading("九馆名录");
                foreach (var h in halls)
                {
                    var access=Array.Find(reply.halls ?? Array.Empty<VoidRealmClient.Hall>(),item=>item.id==h.id);
                    int rank=access?.rank ?? 0;
                    Sprite icon=h.type<OccupationIcons.Length ? InkInterfaceArt.Get("Professions/"+OccupationIcons[h.type]) : null;
                    Color accent=rank>=5 ? new Color32(137,102,49,255) : rank>=3 ? Jade : new Color32(80,89,76,255);
                    string mark=rank>0 ? VoidRealmCatalog.Rank(rank)+" · 入馆" : "未入职";
                    string product=VoidRealmCreationCatalog.Find(h.id)?.product ?? h.work;
                    string detail=rank>0 ? product+" · "+h.theme : h.job+" · "+product;
                    LedgerRow(h.hall,detail,mark,() =>
                    {
                        if(rank==0) { Notice("本馆只接待"+h.job+"，可到洞府百工坊的「专职内室」开辟"+h.room+"。"); return; }
                        OpenWorkshop(h.id);
                    },icon,accent);
                }
            });
        }

        private void ShowBoard()
        {
            boardPage=0;
            ReadBoard();
        }

        private void ShowBoard(string workId)
        {
            tab=1; boardPage=0;
            BeginPage("玉简留言板","与同道论艺");
            Request(new VoidRealmClient.Order {action="work",workId=workId}, reply =>
            {
                if(reply.work==null) { Notice("此卷尚未收录。"); return; }
                attachedId=reply.work.id; attachedName=reply.work.name; topic=2; DrawNote();
            });
        }

        private void ReadBoard()
        {
            tab=1;
            BeginPage("玉简留言板","同服道友 · 论艺留札");
            Request(new VoidRealmClient.Order {action="board",tag=Tags[boardFilter],page=boardPage},reply =>
            {
                Heading("问道留札");
                Choices(Tags,boardFilter,index=>{ boardFilter=index; boardPage=0; ReadBoard(); });
                ActionRow("题一封玉简",DrawNote,true);
                var posts=reply.posts ?? Array.Empty<VoidRealmClient.Post>();
                if(posts.Length==0) Copy("此处尚无"+(boardFilter==0 ? "" : Tags[boardFilter])+"留言。");
                var invite = Array.Find(posts,p => p.tag=="邀试" && !string.IsNullOrEmpty(p.workId));
                if (invite!=null) Feature("附卷邀试",invite.author+" · 邀同道复演","展开附卷",()=>OpenWork(invite.workId));
                foreach (var p in posts)
                {
                    Section(p.author+"　·　"+p.tag);
                    Copy(p.text);
                    if(!string.IsNullOrEmpty(p.workId)) ActionRow("展开所附作品",()=>OpenWork(p.workId));
                }
                Paging(boardPage,reply.more,n=>{boardPage=n;ReadBoard();});
            });
        }

        private void DrawNote()
        {
            tab=1;
            BeginPage("题一封玉简","留言板 · "+lobby.name);
            Choices(TopicTags,topic,index=>{topic=index;DrawNote();});
            var input=Field("写下心得、求教或邀试，最多一百二十字",120,true);
            input.text=noteDraft; input.onValueChanged.AddListener(value=>noteDraft=value);
            if(!string.IsNullOrEmpty(attachedId))
                Row("所附作品","《"+attachedName+"》","取下",()=>{attachedId=null;attachedName=null;DrawNote();});
            ActionRow("留札",()=>
            {
                string words=input.text.Trim();
                if(string.IsNullOrEmpty(words)) { Notice("请先写下留言。"); return; }
                Request(new VoidRealmClient.Order {action="post",tag=TopicTags[topic],text=words,workId=attachedId},reply=>
                {
                    noteDraft="";attachedName=attachedId=null;boardFilter=0;boardPage=0;ReadBoard();
                });
            },true);
            ActionRow("返回留言板",ReadBoard);
        }

        private void ShowRanks()
        {
            rankPage=0;
            ReadRanks();
        }
        private void ReadRanks()
        {
            tab=2;
            BeginPage("虚神金榜","同服作品 · 百工题名");
            Request(new VoidRealmClient.Order {action="rankings",mode=rankMode,hallId=rankHall,grade=rankGrade,page=rankPage},reply=>
            {
                Choices(new[]{"造诣榜","传习榜"},rankMode=="quality" ? 0 : 1,index=>
                {
                    rankMode=index==0 ? "quality" : "study";rankPage=0;ReadRanks();
                });
                var selected=VoidRealmCatalog.Find(rankHall);
                Row("会馆",selected==null ? "全部会馆" : selected.hall,"选择",ChooseRankHall);
                Row("作品等阶",rankGrade==0 ? "全部等阶" : VoidRealmCatalog.Stage(rankGrade),"选择",ChooseRankGrade);
                Copy(rankMode=="quality" ? "每位作者取最高推演分，同分并列。" : "按作品复演合题的道友人数排列，每位道友只计一次。",13);
                var works=reply.works ?? Array.Empty<VoidRealmClient.Work>();
                if(works.Length==0) Copy("本榜尚未收录作品。");
                if(works.Length>0 && rankPage==0)
                {
                    var lead=works[0];
                    Feature(lead.name,(rankMode=="quality" ? "造诣榜" : "传习榜")+" · 榜首作品","阅榜首作品",()=>OpenWork(lead.id));
                }
                foreach(var work in works)
                {
                    string score=rankMode=="quality" ? work.score+" 分" : work.readerCount+" 人";
                    string product=work.version==2 ? VoidRealmCreationCatalog.Find(work.hallId)?.product : VoidRealmCatalog.Find(work.hallId)?.work;
                    Row("第"+work.place+"名　"+work.name,work.author+" · "+VoidRealmCatalog.Stage(work.grade)+product,score,()=>OpenWork(work.id));
                }
                Paging(rankPage,reply.more,n=>{rankPage=n;ReadRanks();});
            });
        }
        private void ChooseRankGrade()
        {
            BeginPage("按阶查榜","虚神金榜");
            for(int i=0;i<=9;i++)
            {
                int grade=i;
                ActionRow(i==0 ? "全部等阶" : VoidRealmCatalog.Stage(i),()=>{rankGrade=grade;ShowRanks();},rankGrade==i);
            }
        }

        private void ChooseRankHall()
        {
            BeginPage("按馆查榜","虚神金榜");
            ActionRow("全部会馆",()=>{rankHall="";ShowRanks();},rankHall=="");
            foreach(var h in VoidRealmCatalog.Halls) ActionRow(h.hall,()=>{rankHall=h.id;ShowRanks();},rankHall==h.id);
        }

        private void Paging(int page,bool more,Action<int> openPage)
        {
            if(page==0 && !more) return;
            Section("第 "+(page+1)+" 页");
            if(page>0) ActionRow("上一页",()=>openPage(page-1));
            if(more) ActionRow("下一页",()=>openPage(page+1));
        }
    }
}
