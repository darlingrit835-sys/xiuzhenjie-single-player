using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class VoidAltarView : MonoBehaviour
    {
        [Serializable] private class Pick { public string goodsId; public int count; }
        [Serializable] private class Order { public string action, quoteId, requestId; public int grade; public Pick[] altarItems; }
        [Serializable] private class Book { public string goodsId, name; public int grade, worth, count; }
        [Serializable] private class Reply
        {
            public string error, quoteId;
            public int before, after, copies, incense, count, grade, cap;
            public int[] balances;
            public Book[] books;
        }
        private static readonly Color Paper=new Color32(246,236,208,255), Ink=new Color32(52,64,52,255),
            Soft=new Color32(107,105,83,255), Pine=new Color32(48,77,67,255), Gold=new Color32(205,164,92,255),
            Cinnabar=new Color32(130,65,48,255), Fog=new Color32(229,219,190,255);
        private readonly Dictionary<string,int> chosen=new Dictionary<string,int>(StringComparer.Ordinal);
        private Font face;
        private Action close;
        private RectTransform hero, body, footer;
        private Text stageMark, status;
        private ScrollRect scroll;
        private VoidAltarIncense incense;
        private Reply ledger, quote;
        private int grade, epoch;
        private bool waiting;
        private string key;

        internal static VoidAltarView Create(Transform parent, Font font, Action exit)
        {
            var root=Node(parent,"虚神祭坛"); Set(root,0,0,1,1);
            var view=root.gameObject.AddComponent<VoidAltarView>();
            view.face=font??Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");view.close=exit;
            view.Build(root);return view;
        }

        private static RectTransform Node(Transform parent,string name)
        {
            var rect=new GameObject(name,typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent,false);rect.gameObject.layer=parent.gameObject.layer;return rect;
        }

        private static void Set(RectTransform r,float x1,float y1,float x2,float y2,float left=0,float bottom=0,float right=0,float top=0)
        {
            r.anchorMin=new Vector2(x1,y1);r.anchorMax=new Vector2(x2,y2);
            r.offsetMin=new Vector2(left,bottom);r.offsetMax=new Vector2(-right,-top);
        }

        private static Image Fill(RectTransform r,Color c,bool hit=false)
        {var im=r.gameObject.AddComponent<Image>();im.color=c;im.raycastTarget=hit;return im;}

        private Text Label(RectTransform r,string words,int size,Color color,TextAnchor anchor)
        {
            var txt=r.gameObject.AddComponent<Text>();txt.font=face;txt.text=words;txt.supportRichText=false;txt.color=color;
            txt.fontSize=size;txt.alignment=anchor;txt.raycastTarget=false;txt.resizeTextForBestFit=true;
            txt.resizeTextMinSize=Mathf.Min(11,size);txt.resizeTextMaxSize=size;return txt;
        }

        private Text Place(RectTransform parent,string name,string words,int size,Color color,
            float x1,float y1,float x2,float y2,TextAnchor alignment=TextAnchor.MiddleLeft)
        {
            var r=Node(parent,name);Set(r,x1,y1,x2,y2);return Label(r,words,size,color,alignment);
        }

        private void Build(RectTransform root)
        {
            var paper=Fill(root,Paper,true);var pageArt=ReleasedResources.Load<Sprite>("InkInterface/InkLedgerPaper");
            if(pageArt!=null){paper.sprite=pageArt;paper.color=Color.white;}
            hero=Node(root,"祭台画幅");Set(hero,0,.63f,1,1);Fill(hero,Pine);hero.gameObject.AddComponent<RectMask2D>();
            var cover=Node(hero,"炉台山色");Set(cover,0,0,1,1);
            var art=Fill(cover,Color.white);
            art.sprite=XiuZhenJie.Client.Release.ReleaseRuntime.LoadContent("assets/gamedata/textures/xushendian/bg_hedaotai.asset",typeof(Sprite)) as Sprite;
            if(art.sprite==null) art.sprite=root.parent.Find("BG")?.GetComponent<Image>()?.sprite??ReleasedResources.Load<Sprite>("InkInterface/MountainHall");
            if(art.sprite!=null){var fit=cover.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.EnvelopeParent;fit.aspectRatio=art.sprite.rect.width/art.sprite.rect.height;}
            var shadow=Node(hero,"翠绢");Set(shadow,0,0,.23f,1);Fill(shadow,new Color32(29,51,45,187));
            Place(hero,"坛额","虚\n神\n祭\n坛",32,Paper,.015f,.19f,.185f,.9f,TextAnchor.MiddleCenter);
            var seal=Node(hero,"祭坛朱印");Set(seal,.175f,.36f,.222f,.73f);Fill(seal,Cinnabar);
            Place(seal,"朱印题字","焚\n卷\n求\n经",10,Paper,0,0,1,1,TextAnchor.MiddleCenter);
            var horizon=Node(hero,"金线");Set(horizon,0,0,1,0,0,0,0,-2);Fill(horizon,Gold);
            var foot=Node(hero,"炉刻");Set(foot,.25f,0,1,.18f);Fill(foot,new Color32(29,49,43,192));
            stageMark=Place(foot,"炉境","",17,Paper,.04f,0,.97f,1,TextAnchor.MiddleRight);
            var mount=Node(hero,"青铜炉台");Set(mount,.48f,.21f,.82f,.85f);var relic=Node(mount,"青铜香炉");Set(relic,0,0,1,1);
            var relicFit=relic.gameObject.AddComponent<AspectRatioFitter>();relicFit.aspectMode=AspectRatioFitter.AspectMode.FitInParent;relicFit.aspectRatio=1;
            var raw=relic.gameObject.AddComponent<RawImage>();raw.texture=ReleasedResources.Load<Texture2D>("VoidRealm/profession-relics");raw.uvRect=new Rect(.001f,2f/3+.001f,1f/3-.002f,1f/3-.002f);raw.raycastTarget=false;raw.enabled=raw.texture!=null;
            var fx=Node(hero,"皈香流火");Set(fx,0,0,1,1);incense=fx.gameObject.AddComponent<VoidAltarIncense>();incense.raycastTarget=false;incense.Bind(relic);
            var rail=Node(root,"祭书册页");Set(rail,0,0,1,.63f,9,83,9,5);
            Fill(rail,new Color(1,1,1,.025f),true);rail.gameObject.AddComponent<RectMask2D>();
            scroll=rail.gameObject.AddComponent<ScrollRect>();scroll.horizontal=false;scroll.viewport=rail;
            scroll.movementType=ScrollRect.MovementType.Clamped;scroll.scrollSensitivity=28;
            body=Node(rail,"卷目");body.anchorMin=new Vector2(0,1);body.anchorMax=Vector2.one;body.pivot=new Vector2(.5f,1);body.sizeDelta=Vector2.zero;
            var layout=body.gameObject.AddComponent<VerticalLayoutGroup>();layout.spacing=4;layout.padding=new RectOffset(4,4,2,8);
            layout.childForceExpandHeight=false;layout.childControlHeight=true;
            body.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;scroll.content=body;
            footer=Node(root,"坛前操作");Set(footer,0,0,1,0,9,9,9,-51);
            var msg=Node(root,"本炉状态");Set(msg,0,0,1,0,15,50,15,-82);status=Label(msg,"",13,Soft,TextAnchor.MiddleCenter);
        }

        internal void Open()
        {
            Cancel();grade=1;ledger=null;quote=null;chosen.Clear();
            var role=Singleton<RoleManager>.Instance;
            key="xzj.void.altar|"+Constants.LoginServerIP+":"+Constants.LoginServerPort+"|"+role.ServerId+"|"+role.currentRoleId;
            Reload();
        }
        internal void Cancel(){epoch++;waiting=false;}
        private void OnDisable(){Cancel();}

        private void Fetch(Order order,Action<Reply> done,Action retry)
        {
            if(waiting)return;
            int token=++epoch;waiting=true;status.text="正在核对炉册…";
            ForgeWire.Send((MsgType)4901070,JsonUtility.ToJson(order),(success,json)=>{
                if(this==null || !isActiveAndEnabled || token!=epoch)return;
                waiting=false;
                Reply result=null;
                try {if(!string.IsNullOrEmpty(json))result=JsonUtility.FromJson<Reply>(json);}catch(ArgumentException){}
                if(!success || result==null)
                {
                    status.text=result?.error??"炉册暂未应答，可重试。";
                    if(order.action=="altarOffer" && result!=null){PlayerPrefs.DeleteKey(key);PlayerPrefs.Save();quote=null;}
                    if(order.action=="altarOffer" && result==null) Controls("离坛",close,"重取本炉回执",()=>Submit(order));
                    else Controls("离坛",close,"重新核对",retry);
                    return;
                }
                status.text="";done(result);
            });
        }
        private void Reload()
        {
            quote=null;
            Controls("离坛",close,"重读炉册",Reload);
            Fetch(new Order{action="altar"},result=>{
                ledger=result;
                grade=Mathf.Clamp(grade,1,Mathf.Max(1,ledger.cap));
                var remaining=ledger.books??Array.Empty<Book>();
                foreach(var id in chosen.Keys.ToArray()){
                    var book=Array.Find(remaining,b=>b.goodsId==id && b.grade>=grade);
                    if(book==null)chosen.Remove(id);else chosen[id]=Math.Min(chosen[id],book.count);
                }
                ShowShelf(true);
                if(PlayerPrefs.HasKey(key)){
                    Order pending=null;try{pending=JsonUtility.FromJson<Order>(PlayerPrefs.GetString(key));}catch(ArgumentException){}
                    if(pending!=null && pending.action=="altarOffer" && !string.IsNullOrEmpty(pending.requestId)){
                        Clear(body);Heading("上炉回执");Copy("上次点火后回执未送达，请先取回本炉记录。",15,2);status.text="上炉回执尚待核对。";Controls("离坛",close,"重取上炉回执",()=>Submit(pending));
                    }
                }
            },Reload);
        }

        private static void Clear(RectTransform parent)
        {for(int i=parent.childCount-1;i>=0;i--){var go=parent.GetChild(i).gameObject;go.SetActive(false);Destroy(go);}}

        private RectTransform Line(string name,float height)
        {var r=Node(body,name);r.gameObject.AddComponent<LayoutElement>().preferredHeight=height;return r;}
        private void Heading(string words)
        {var r=Line(words,34);Label(r,words,22,Ink,TextAnchor.MiddleLeft);}
        private void Copy(string words,int size=15,int rows=2)
        {var r=Line("卷文",rows*20+2);Label(r,words,size,Soft,TextAnchor.MiddleLeft);}
        private void Button(RectTransform parent,string text,Action action,float a,float b,Color tone)
        {
            var r=Node(parent,text);Set(r,a,0,b,1,2,1,2,1);var bg=Fill(r,tone,true);
            var button=r.gameObject.AddComponent<Button>();button.targetGraphic=bg;button.interactable=action!=null;
            if(action!=null)button.onClick.AddListener(()=>{if(!waiting||action==close)action();});
            var t=Node(r,"按钮名");Set(t,0,0,1,1,2,1,2,1);Label(t,text,16,Paper,TextAnchor.MiddleCenter);
        }
        private void Controls(string a,Action first,string b,Action second)
        {Clear(footer);Button(footer,a,first,0,.35f,Pine);Button(footer,b,second,.36f,1,second==null?Soft:Cinnabar);}
        private static string Tier(int n)=>VoidRealmCatalog.Stage(n);
        private int Balance=>ledger?.balances!=null && grade<=ledger.balances.Length?ledger.balances[grade-1]:0;
        private int Count=>chosen.Values.Sum();
        private int Added=>ledger?.books==null?0:ledger.books.Sum(b=>chosen.TryGetValue(b.goodsId,out int n)?n*b.worth:0);
        private void Meter(int amount)
        {
            var line=Line("八瓣炉盘",12);
            for(int i=0;i<8;i++){var light=Node(line,"香瓣");Set(light,i/8f,0,(i+1)/8f,1,1,2,1,2);Fill(light,i<amount?Gold:Fog);}
        }
        private void ShowShelf(bool top)
        {
            quote=null;Clear(body);if(top)scroll.verticalNormalizedPosition=1;
            stageMark.text=Tier(grade)+"经炉　·　"+Balance+"/8 余香";incense.Set(Balance+Added,false);
            Heading("择卷入坛");
            Copy("同阶及更高境界的旧书可入炉。黄、玄、地、天阶每册各生一至四点香，八点凝成一部同阶德经。",14,2);
            var tier=Line("炉阶",36);Button(tier,"−",grade>1?(Action)(()=>ChangeGrade(-1)):null,0,.14f,Pine);
            Place(tier,"炉铭",Tier(grade)+"炉　·　余香 "+Balance+" / 8",16,Ink,.15f,0,.84f,1,TextAnchor.MiddleCenter);
            Button(tier,"＋",grade<ledger.cap?(Action)(()=>ChangeGrade(1)):null,.85f,1,Pine);Meter(Balance);
            var available=(ledger.books??Array.Empty<Book>()).Where(b=>b.grade>=grade).ToList();
            if(available.Count==0)Copy(grade==1?"一阶祭书可在虚神界的合道台抄录，每次 120 灵石得两册。也可从奇珍阁竞得经卷。":"可在奇珍阁购买同境或更高境界的经卷，或至纳戒解锁闲置图谱。",15,3);
            else foreach(var book in available){
                chosen.TryGetValue(book.goodsId,out int count);var row=Line(book.name,45);
                Place(row,"书名",book.name,16,Ink,0,.39f,.56f,1);
                Place(row,"卷价",Tier(book.grade)+" · 每册 "+book.worth+" 香 · 存 "+book.count,13,Soft,0,0,.57f,.48f);
                Button(row,"−",count>0?(Action)(()=>Adjust(book,-1)):null,.58f,.71f,Pine);
                Place(row,"数量",count.ToString(),17,Ink,.71f,0,.84f,1,TextAnchor.MiddleCenter);
                Button(row,"＋",count<book.count&&Count<10?(Action)(()=>Adjust(book,1)):null,.85f,1,Pine);
            }
            status.text=Count==0?"一炉可选一至十册。":"已选 "+Count+" / 10 册　·　添香 "+Added+" 点";
            Controls("离坛",close,"核香预览",Count>0?(Action)Preview:null);
        }
        private void ChangeGrade(int delta){grade+=delta;chosen.Clear();ShowShelf(true);}
        private void Adjust(Book b,int delta)
        {
            float pos=scroll.verticalNormalizedPosition;chosen.TryGetValue(b.goodsId,out int n);
            if(n+delta>0)chosen[b.goodsId]=n+delta;else chosen.Remove(b.goodsId);
            ShowShelf(false);Canvas.ForceUpdateCanvases();scroll.verticalNormalizedPosition=pos;
        }
        private Order OrderFor(string action)=>new Order{action=action,grade=grade,
            altarItems=chosen.OrderBy(p=>p.Key,StringComparer.Ordinal).Select(p=>new Pick{goodsId=p.Key,count=p.Value}).ToArray()};
        private void Preview()
        {
            Fetch(OrderFor("altarPreview"),r=>{quote=r;ShowResult(r,false);},Reload);
        }
        private void ShowResult(Reply result,bool offered)
        {
            Clear(body);scroll.verticalNormalizedPosition=1;incense.Set(result.before+result.incense,offered);
            stageMark.text=offered?"祭火已歇　·　余香入炉":"卷册已齐　·　待点祭火";
            Heading(offered?"祭坛回执":"核香清单");
            Copy(result.count+" 册旧书　·　原有 "+result.before+" 香 ＋ 本炉 "+result.incense+" 香",16,1);
            Meter(result.after);
            Heading(result.copies>0?Tier(result.grade)+"德经 × "+result.copies:"余香 "+result.after+" / 8");
            Copy(result.copies>0?(offered?"德经已收入纳戒。":"点火后德经将收入纳戒。")+" 可在参悟功法时使用同境德经，提升灵衍与品阶。":"此炉尚未满八点，余香会留在本阶炉中。",15,2);
            if(!offered){foreach(var b in ledger.books??Array.Empty<Book>()){if(chosen.TryGetValue(b.goodsId,out int n))Copy(b.name+"　× "+n,14,1);}}
            status.text="此炉结算后留香 "+result.after+" / 8";
            if(offered){Controls("离坛",close,"再择一炉",Reload);return;}
            Controls("返回择卷",()=>ShowShelf(false),"点火献卷",()=>{
                if(quote==null)return;var order=OrderFor("altarOffer");order.quoteId=quote.quoteId;order.requestId=Guid.NewGuid().ToString("N");
                PlayerPrefs.SetString(key,JsonUtility.ToJson(order));PlayerPrefs.Save();Submit(order);
            });
        }
        private void Submit(Order order)
        {
            Fetch(order,result=>{
                PlayerPrefs.DeleteKey(key);PlayerPrefs.Save();chosen.Clear();quote=null;
                if(Singleton<RoleManager>.Instance.RoleBoxDic.TryGetValue(10,out var bag))bag.GetAllGoods(null,false);
                ShowResult(result,true);
            },Reload);
        }
    }
}
