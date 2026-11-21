using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Book = XiuZhenJie.ConcordDeskClient.Book;
using Order = XiuZhenJie.ConcordDeskClient.Order;
using Reply = XiuZhenJie.ConcordDeskClient.Reply;

namespace XiuZhenJie
{
    internal sealed class ConcordDeskView : MonoBehaviour
    {
        static readonly Color Ink = new Color32(36,57,50,255), Muted = new Color32(83,97,75,255),
            Jade = new Color32(33,75,65,255), Deep = new Color32(27,48,43,248),
            Gold = new Color32(209,175,112,255), Cream = new Color32(255,245,218,255),
            Sheet = new Color32(240,231,205,255), Clay = new Color32(118,69,46,255);
        Font font;
        Action leave;
        RectTransform controls, body;
        VoidRealmLight lightFx;
        Reply state, trial;
        Book first, second;
        string feedback = "", confirm = "";
        int picker, ticket;
        bool busy, suspended;

        internal static ConcordDeskView Open(Transform parent, Sprite art, Font font, Action leave)
        {
            var root = Node(parent, "合道台 · 书卷合参", 0,0,1,1);
            Fill(root, Deep, true);
            var self = root.gameObject.AddComponent<ConcordDeskView>();
            self.font = font ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            self.leave = leave;
            var hero = Node(root,"合道山台",0,.59f,1,1);
            hero.gameObject.AddComponent<RectMask2D>();
            var painting = Node(hero,"山台原画",0,0,1,1);
            var image = Fill(painting,Color.white);
            image.sprite = art ?? InkInterfaceArt.Get("MountainHall");
            if (image.sprite != null)
            {
                var fit = painting.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = image.sprite.rect.width / image.sprite.rect.height;
            }
            Fill(Node(hero,"远山薄墨",0,0,1,1),new Color(0.06f,.15f,.12f,.30f));
            self.lightFx = VoidRealmLight.Mount(hero,painting,4,true);
            self.controls = Node(hero,"书案",0,0,1,1);
            var paper = Node(root,"经笺",0,0,1,.59f);
            var paperImage = Fill(paper,Sheet,true);
            paperImage.sprite = InkInterfaceArt.Get("InkLedgerPaper");
            Fill(Node(paper,"鎏金界线",0,.992f,1,1),Gold);
            self.body = Node(paper,"书案内容",.035f,.01f,.965f,.985f);
            return self;
        }

        static RectTransform Node(Transform parent,string name,float x,float y,float x2,float y2)
        {
            var o = new GameObject(name,typeof(RectTransform)); o.layer=parent.gameObject.layer;
            var r=(RectTransform)o.transform; r.SetParent(parent,false);
            r.anchorMin=new Vector2(x,y);r.anchorMax=new Vector2(x2,y2);r.offsetMin=r.offsetMax=Vector2.zero;
            return r;
        }
        static Image Fill(RectTransform r,Color color,bool hit=false)
        {
            var im=r.gameObject.AddComponent<Image>();im.color=color;im.raycastTarget=hit;return im;
        }
        Text Label(Transform p,string text,float x,float y,float x2,float y2,int size,Color color,TextAnchor align=TextAnchor.MiddleLeft)
        {
            var rt=Node(p,"题字",x,y,x2,y2);var t=rt.gameObject.AddComponent<Text>();
            t.font=font;t.text=text;t.color=color;t.fontSize=size;t.alignment=align;t.raycastTarget=false;
            t.supportRichText=false;t.resizeTextForBestFit=true;t.resizeTextMinSize=Mathf.Min(12,size);t.resizeTextMaxSize=size;
            t.verticalOverflow=VerticalWrapMode.Truncate;t.horizontalOverflow=HorizontalWrapMode.Wrap;
            return t;
        }
        Button Pad(Transform p,string title,float x,float y,float x2,float y2,Color color,Action tap)
        {
            var r=Node(p,title,x,y,x2,y2);var im=Fill(r,color,true);
            var b=r.gameObject.AddComponent<Button>();b.targetGraphic=im;b.interactable=tap!=null;
            if(tap!=null)b.onClick.AddListener(()=>tap());
            return b;
        }
        void ButtonText(Transform p,string title,float x,float y,float x2,float y2,Color color,Action tap,int size=19)
        {
            var button=Pad(p,title,x,y,x2,y2,color,tap);
            Label(button.transform,title,.025f,.04f,.975f,.96f,size,Cream,TextAnchor.MiddleCenter);
        }
        static void Clear(RectTransform r)
        {
            for(int i=r.childCount-1;i>=0;i--){var item=r.GetChild(i).gameObject;item.SetActive(false);Destroy(item);}
        }
        internal void Suspend(){suspended=true;busy=false;++ticket;}
        internal void Refresh()
        {
            suspended=false;picker=0;confirm="";state=null;trial=null;first=second=null;
            Query(new Order{action="list"},r=>{state=r;feedback="";});
        }
        void Query(Order order,Action<Reply> accept)
        {
            int mine=++ticket;busy=true;Render();
            ConcordDeskClient.Send(order,(r,error)=>
            {
                if(this==null||suspended||mine!=ticket)return;
                busy=false;
                if(r==null) {feedback=error;confirm="";}
                else accept(r);
                Render();
            });
        }
        void Render()
        {
            Clear(controls);Clear(body);
            Label(controls,"合道台",.05f,.82f,.59f,.98f,31,Cream);
            if (state != null && !busy && ConcordDeskClient.Pending() == null) ButtonText(controls,"刷新",.60f,.82f,.76f,.96f,Deep,Refresh,16);
            ButtonText(controls,"离开",.79f,.82f,.96f,.96f,Deep,leave,17);
            Label(controls,"书 卷 合 参",.05f,.71f,.72f,.83f,16,Gold);
            Slot(1,first,.07f,.43f);Slot(2,second,.57f,.93f);
            Label(controls,"合",.43f,.26f,.57f,.48f,29,Gold,TextAnchor.MiddleCenter);
            if(picker!=0){DrawPicker();return;}
            if(state==null)
            {
                Label(body,busy?"正在取纳戒书册…":feedback,.03f,.66f,.96f,.89f,20,Ink);
                if(!busy)ButtonText(body,"重新取册",.15f,.25f,.85f,.41f,Jade,Refresh);return;
            }
            if(ConcordDeskClient.Pending()!=null && !busy){DrawPending();return;}
            if(confirm=="copy"||confirm=="commit"){DrawConfirm();return;}
            DrawDesk();
        }
        void Slot(int number,Book book,float x,float x2)
        {
            var b=Pad(controls,number==1?"主卷":"参卷",x,.065f,x2,.665f,number==1?Jade:Clay,
                !busy&&state!=null&&ConcordDeskClient.Pending()==null?(Action)(()=>{picker=number;confirm="";feedback="";Render();}):null);
            Fill(Node(b.transform,"装订",.07f,.07f,.08f,.93f),Gold);
            Label(b.transform,number==1?"主 卷":"参 卷",.15f,.67f,.96f,.95f,16,Gold);
            Label(b.transform,book?.name??"选取书卷",.15f,.26f,.95f,.71f,23,Cream);
            Label(b.transform,book!=null?book.element+"行 · "+book.grade+"品":"点此开纳戒",.15f,.05f,.96f,.28f,14,Cream);
        }
        void DrawDesk()
        {
            Label(body,trial?.relation??"两卷并读",.02f,.83f,.66f,.98f,26,Ink);
            Label(body,"灵石 "+state.stones,.64f,.84f,.98f,.97f,16,Muted,TextAnchor.MiddleRight);
            if(trial!=null)
            {
                Label(body,"主卷 "+trial.main.grade+"  ＋  参卷 "+trial.second.grade+"  ＋  "+trial.relation+" "+trial.bonus+" ＝ "+trial.points+" 道意",.02f,.65f,.98f,.82f,19,Ink);
                Label(body,"各用一册 · 墨钱 "+trial.ink+" 灵石\n所得禅悟丹 ×"+trial.pills+"，每枚服用可恢复100悟性。",.02f,.40f,.98f,.65f,18,Ink);
            }
            else
            {
                int n=state.books==null?0:state.books.Count(b=>b.free>0);
                Label(body,n==0?"纳戒暂无可投入的典籍。可先抄两本基础功法，再回书案选卷。":
                    "从纳戒各置一册。主卷生参卷得三点道意，同源得两点，逆序相生得一点。",.02f,.49f,.98f,.81f,19,Ink);
            }
            string note=feedback.Length>0?feedback:(busy?"正在核对书案…":trial!=null?"每五点道意多得一枚禅悟丹。":"功法、丹方、器图等均可合参。珍本与《启灵经》保留末本。");
            Label(body,note,.02f,.23f,.98f,.43f,16,feedback.Length>0?Clay:Muted);
            bool canCopy = !string.IsNullOrEmpty(state.primerText);
            ButtonText(body,canCopy?"抄录两册":"抄本暂缺",.01f,.045f,.33f,.21f,Clay,!busy && canCopy?(Action)(()=>{confirm="copy";Render();}):null);
            Action primary=null;string title="选择主卷";
            if(first==null){primary=()=>{picker=1;Render();};}
            else if(second==null){title="选择参卷";primary=()=>{picker=2;Render();};}
            else if(trial==null){title="试读两卷";primary=Preview;}
            else {title=trial.affordable?"合参 · 禅悟丹 ×"+trial.pills:"墨钱不足";if(trial.affordable)primary=()=>{confirm="commit";Render();};}
            ButtonText(body,busy?"正在核对…":title,.36f,.045f,.99f,.21f,Jade,busy?null:primary);
        }
        void DrawPicker()
        {
            Label(body,picker==1?"选择主卷":"选择参卷",.02f,.83f,.66f,.98f,24,Ink);
            ButtonText(body,"返回",.76f,.84f,.98f,.97f,Jade,()=>{picker=0;Render();},16);
            var list=state?.books??Array.Empty<Book>();
            if(list.Length==0)
            {
                Label(body,"纳戒中还没有书卷。抄录两册基础功法可开始第一次合参。",.03f,.46f,.97f,.77f,19,Ink);
                bool canCopy = !string.IsNullOrEmpty(state.primerText);
                ButtonText(body,canCopy?"前往抄录":"抄本暂缺",.12f,.14f,.88f,.35f,Clay,canCopy?(Action)(()=>{picker=0;confirm="copy";Render();}):null);return;
            }
            var frame=Node(body,"纳戒书目",0,.015f,1,.81f);frame.gameObject.AddComponent<RectMask2D>();
            Fill(frame,new Color(0,0,0,.001f),true);
            var listScroll=frame.gameObject.AddComponent<ScrollRect>();listScroll.horizontal=false;
            listScroll.movementType=ScrollRect.MovementType.Clamped;listScroll.viewport=frame;
            var content=Node(frame,"书册",0,1,1,1);content.pivot=new Vector2(.5f,1);content.sizeDelta=new Vector2(0,list.Length*56);
            listScroll.content=content;var other=picker==1?second:first;
            for(int i=0;i<list.Length;i++)
            {
                var b=list[i];long free=b.free-(other!=null&&other.id==b.id?1:0);
                var row=Pad(content,b.name,0,1,1,1,free>0?Sheet:new Color32(217,213,193,255),free>0?(Action)(()=>Pick(b)):null);
                var rt=(RectTransform)row.transform;rt.pivot=new Vector2(.5f,1);rt.offsetMin=new Vector2(0,-(i+1)*56+3);rt.offsetMax=new Vector2(0,-i*56);
                Fill(Node(rt,"行标",0,.1f,.012f,.9f),free>0?Jade:Muted);
                Label(rt,b.name,.04f,.41f,.68f,.96f,20,Ink);
                Label(rt,b.kind+" · "+b.grade+"品 · "+b.element+"行",.04f,.04f,.68f,.45f,14,Muted);
                Label(rt,free>0?"可用 "+free:!string.IsNullOrEmpty(b.note)?b.note:"已选用",.67f,.12f,.97f,.88f,15,Muted,TextAnchor.MiddleRight);
            }
        }
        void Pick(Book b)
        {
            if(picker==1)first=b;else second=b;picker=0;trial=null;confirm="";feedback="";
            if(first!=null&&second!=null)Preview();else Render();
        }
        void Preview()
        {
            Query(new Order{action="preview",main=first.id,second=second.id},r=>{trial=r;feedback="";lightFx.Burst(true,(first.grade+second.grade)/2);});
        }
        void DrawConfirm()
        {
            bool copying=confirm=="copy";
            Label(body,copying?"抄录基础典籍":"落印合参",.02f,.80f,.98f,.98f,26,Ink);
            string text=copying?"支出 "+state.copyCost+" 灵石，将"+state.primerText+"抄入纳戒。":
                "投入《"+first.name+"》与《"+second.name+"》各一册，支出 "+trial.ink+" 灵石，所得禅悟丹 ×"+trial.pills+"。";
            Label(body,text,.02f,.44f,.98f,.78f,20,Ink);
            Label(body,copying?"两本均可用来合参或练功。":"所选两册将从纳戒扣除。",.02f,.27f,.98f,.44f,16,Muted);
            ButtonText(body,"返回",.01f,.05f,.32f,.22f,Clay,!busy?(Action)(()=>{confirm="";Render();}):null);
            bool enough=!copying||state.stones>=state.copyCost;
            ButtonText(body,busy?"正在登记…":!enough?"灵石不足":copying?"确认抄录":"确认用卷",.36f,.05f,.99f,.22f,Jade,!busy&&enough?(Action)(()=>Write(copying?new Order{action="copy",quote=state.primerText,revision=state.revision}:new Order{action="commit",revision=state.revision,main=first.id,second=second.id,quote=trial.quote})):null);
        }
        void DrawPending()
        {
            Label(body,"上次书案回执",.02f,.72f,.98f,.94f,25,Ink);
            Label(body,feedback.Length>0?feedback:"有一笔操作尚待核对。按原单查询即可确认结果。",.02f,.40f,.98f,.72f,18,Muted);
            ButtonText(body,"核对上次操作",.10f,.09f,.90f,.28f,Jade,()=>Write(ConcordDeskClient.Pending()));
        }
        void Write(Order order)
        {
            if(order==null)return;
            Query(order,r=>{state=r;trial=null;confirm="";first=second=null;picker=0;
                feedback=r.receipt?.text??"书案已登记。";lightFx.Burst(true,order.action=="copy"?2:7);});
        }
    }
}
