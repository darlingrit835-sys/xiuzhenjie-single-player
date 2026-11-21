using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.AuctionHallState;

namespace XiuZhenJie
{
    internal sealed partial class AuctionHall
    {
        internal static readonly Color Night=new Color32(30,38,49,255), Ink=new Color32(45,69,63,255),
            Gold=new Color32(255,214,142,255), Ivory=new Color32(255,248,223,255), Pale=new Color32(239,230,207,255),
            Lacquer=new Color32(139,57,45,255);
        internal static RectTransform Band(Transform host,string name,float top,float h,float left=0,float right=0)
        { var r=RoleFolio.Box(host,name); RoleFolio.Band(r,left,right,top,h); return r; }
        internal static RectTransform Slice(Transform host,float a,float b,float margin=0,float v=0)
        {
            var r=RoleFolio.Box(host,"栏");r.anchorMin=new Vector2(a,0);r.anchorMax=new Vector2(b,1);
            r.offsetMin=new Vector2(margin,v);r.offsetMax=new Vector2(-margin,-v);return r;
        }
        internal static Image Surface(RectTransform r,Color c,bool hit=false)
        { var img=r.gameObject.AddComponent<Image>();img.color=c;img.raycastTarget=hit;return img; }
        internal static Text Caption(RectTransform host,Font f,string name,int size,Color c,TextAnchor align,bool bold=false)
        {
            var t=RoleFolio.Label(host,f,name,size,c,align,bold);
            RoleFolio.Style(t,size,c,align,bold,Mathf.Max(11,size-2));RoleFolio.Fill(t.rectTransform,3,3);return t;
        }
        private Text Text(RectTransform host,string name,int size,Color c,TextAnchor align=TextAnchor.MiddleLeft,bool bold=false)
            =>Caption(host,font,name,size,c,align,bold);
        private static void Bind(RectTransform r,Image img,Action click)
        {
            var b=r.gameObject.AddComponent<Button>(); b.targetGraphic=img;
            b.onClick.AddListener(()=>click());
        }
        private static void Face(RectTransform r,Font f,string name,Action click,Color c)
        {
            Bind(r,Surface(r,c,true),click);
            var gild=RoleFolio.Image(r,"金线",new Color32(202,161,90,230));RoleFolio.Band(gild.rectTransform,3,3,1,1);
            Caption(r,f,name,15,Ivory,TextAnchor.MiddleCenter,true);
        }
        private void Button(RectTransform r,string name,Action click,Color c)=>Face(r,font,name,click,c);
        internal static void StreetEntrances(Transform host,Font f,Action market,Action auction)
        {
            var r=RoleFolio.Box(host,"坊市交易入口");r.anchorMin=Vector2.zero;r.anchorMax=Vector2.right;
            r.offsetMin=new Vector2(3,0);r.offsetMax=new Vector2(-3,35);
            Face(Slice(r,0,.5f,2,0),f,"常驻 · 自由市场",market,Ink);
            Face(Slice(r,.5f,1,2,0),f,"定时 · 金阙拍卖",auction,Lacquer);
        }
        private RectTransform Line(float height,float m=9)
        { var r=Band(body,"行",cursor,height,m,m);cursor+=height+4;return r; }
        private void Title(string title,string aside)
        {
            var line=Line(32); Text(Slice(line,0,.54f),title,18,Ink,TextAnchor.MiddleLeft,true);
            Text(Slice(line,.54f,1),aside,13,Lacquer,TextAnchor.MiddleRight);
            var rule=RoleFolio.Image(line,"朱丝",new Color32(198,171,118,255));RoleFolio.Band(rule.rectTransform,0,0,31,1);
        }
        private void Note(string text,int h=44)=>Text(Line(h,12),text,14,Ink);
        private void MainAction(string text,Action act)=>Button(Line(43),text,act,Lacquer);
        private void Choices(string[] labels,int index,Action<int> choose)
        {
            var line=Line(41,2);
            for(int i=0;i<labels.Length;i++) {int j=i;Button(Slice(line,(float)i/labels.Length,(i+1f)/labels.Length,1,2),labels[i],()=>choose(j),i==index ? Lacquer : Ink);}
        }
        private static Sprite Art(Goods g)=>RingLedger.Artwork(new BaseNaJieGoodEntity { Type=g.kind,TemplateId=g.templateId,BlankId=g.blankId,Name=g.name });
        private void Tile(string title,string description,Sprite art,string tag,Action action)
        {
            var r=Line(56);var bg=Surface(r,Pale,true);Bind(r,bg,action);
            var lip=RoleFolio.Image(r,"墨线",Ink);RoleFolio.Fixed(lip.rectTransform,0,0,3,2,50);
            var icon=RoleFolio.Image(r,"物像",Color.white);icon.sprite=art; icon.preserveAspect=true;
            RoleFolio.Fixed(icon.rectTransform,0,7,5,46,46);
            Text(Band(r,"题",2,27,57,68),title,16,Ink,TextAnchor.MiddleLeft,true);
            Text(Band(r,"款",28,25,57,5),description,13,Ink);
            var tab=RoleFolio.Box(r,"签");RoleFolio.Fixed(tab,1,-5,3,62,25);Text(tab,tag,13,Lacquer,TextAnchor.MiddleRight,true);
        }
        private void Hero(Lot lot)
        {
            bool market=page==Page.Market,live=page==Page.Room;
            float h=market ? 162 : live ? 211 : 158;
            hero=Line(h,0);Surface(hero,Night);hero.gameObject.AddComponent<RectMask2D>();
            var pic=RoleFolio.Image(hero,"楼阁画",Color.white);
            pic.sprite= market ? InkInterfaceArt.Get("MarketStreet") : ReleasedResources.Load<Sprite>("AuctionHall/MoonGallery");
            if(pic.sprite!=null)
            {
                var r=pic.rectTransform; r.anchorMin=r.anchorMax=new Vector2(.5f,.5f);
                var fit=pic.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio=pic.sprite.rect.width/pic.sprite.rect.height;
            }
            if(lot!=null)
            {
                var seal=RoleFolio.Box(hero,"台上真品"); seal.anchorMin=seal.anchorMax=new Vector2(.55f,.62f);
                seal.sizeDelta=Vector2.one*(live?116:90);seal.anchoredPosition=Vector2.zero;
                var dais=seal.gameObject.AddComponent<AuctionDaisLight>();dais.raycastTarget=false;dais.Rank=lot.rank;
                var bed=Slice(seal,0,1,live ? 18 : 13,live ? 18 : 13);
                var goods=RoleFolio.Image(bed,lot.name,Color.white);goods.sprite=Art(lot);goods.preserveAspect=true;RoleFolio.Fill(goods.rectTransform,0,0,0,0);
            }
            var fog=Band(hero,"墨灯",h-55,55);Surface(fog,new Color32(26,36,43,238));
            string name=market ? "百宝通衢" : lot!=null ? lot.name : TierNames[tier];
            Text(Band(fog,"横额",0,33,12,4),name,25,Ivory,TextAnchor.MiddleLeft,true);
            string info=market ? "常年开市  ·  明码挂牌  ·  即时交割" : lot!=null ? $"{RingLedger.Kind(lot.kind)}　×{lot.count}　{(lot.source=="system"?"阁藏":"道友寄售")}　{Money(lot.price)} 灵石" :
                TierTitles[tier]+"　·　"+(state==null ? "金阙宝录" : Phase(state.phase));
            Text(Band(fog,"小款",31,22,13,2),info,13,Gold);
            var sealName=Band(hero,"门签",7,24,8,0);sealName.anchorMax=new Vector2(0,1);sealName.offsetMax=new Vector2(79,-7);
            Surface(sealName,Lacquer);Text(sealName,market ? "自由市场" : TierTitles[tier],13,Ivory,TextAnchor.MiddleCenter,true);
            stageLight=AuctionHallLights.Add(hero,font);
        }

        private RectTransform Sheet(string name,float height,Action dismiss=null)
        {
            CloseSheet();modal=RoleFolio.Box(transform,"卷宗");RoleFolio.Fill(modal);
            Surface(modal,new Color32(18,27,33,208),true);
            var card=RoleFolio.Box(modal,name);card.anchorMin=new Vector2(.045f,.5f);card.anchorMax=new Vector2(.955f,.5f);
            card.pivot=new Vector2(.5f,.5f);card.sizeDelta=new Vector2(0,height);card.anchoredPosition=Vector2.zero;
            Surface(card,new Color32(250,242,217,255),true);
            var head=Band(card,"印题",0,43);Surface(head,Night);Text(Slice(head,0,.81f,8,0),name,19,Gold,TextAnchor.MiddleLeft,true);
            Button(Slice(head,.81f,1,2,3),"关闭",dismiss??CloseSheet,Ink);return card;
        }
        private void CloseSheet() { if(modal!=null) {modal.gameObject.SetActive(false);Destroy(modal.gameObject);modal=null;} }
        private void Confirm(string title,string msg,string submit,Action action)
        {
            var card=Sheet(title,220);Text(Band(card,"正文",51,86,14,14),msg,16,Ink,TextAnchor.MiddleCenter);
            string eventId=state?.eventId;
            Button(Band(card,"落印",154,46,18,18),submit,()=>
            {
                if (state?.eventId!=eventId) {CloseSheet();notice="场次已经切换，请重新核对。";Render();return;}
                action();
            },Lacquer);
        }
        private void CardError(RectTransform card,string msg)
        {
            var t=card.GetChild(0).GetComponentInChildren<Text>();if(t!=null) {t.text=msg;t.fontSize=13;}
        }
        private InputField Input(RectTransform card,float y,string hint,bool number,string value,float height=40)
        {
            var row=Band(card,"填写",y,height,12,12);
            float split=number ? .32f : 0;
            if(number) Text(Slice(row,0,split),hint,14,Ink);
            var host=Slice(row,split,1,1,0);var bg=Surface(host,Color.white,true);
            var i=host.gameObject.AddComponent<InputField>();i.targetGraphic=bg;
            var label=Text(host,"",16,Ink);RoleFolio.Fill(label.rectTransform,8,8);
            var ph=Text(host,hint,14,new Color32(105,110,100,255));RoleFolio.Fill(ph.rectTransform,8,8);
            i.textComponent=label;i.placeholder=ph;i.lineType=InputField.LineType.SingleLine;
            if(number) { i.contentType=InputField.ContentType.IntegerNumber;i.characterLimit=18; }
            i.text=value;return i;
        }
        private void CloseLotToast(Lot lot)
        {
            var banner=Band(root,"落槌唱词",45,58,8,8);Surface(banner,Night);
            var picture=RoleFolio.Image(banner,"刚结拍品",Color.white);RoleFolio.Fixed(picture.rectTransform,0,8,4,50,50);
            picture.sprite=Art(lot);picture.preserveAspect=true;
            Text(Band(banner,"唱槌",4,50,64,8),HammerCall(lot),14,Gold);
            AuctionHallLights.Add(picture.rectTransform,font).Burst("",true);
            Destroy(banner.gameObject,2.6f);
        }
        private void ReceiptSheet(Delivery item, string detail=null, Action dismiss=null)
        {
            string heading=item?.reason=="fixed" ? "市契 · 成交得宝" : item?.reason=="won" ? "竞得 · 宝物入匣" : item?.reason=="passed" || item?.reason=="notSelected" ? "归还 · 物归原主" : "交割 · 宝物入匣";
            var card=Sheet(heading,294,dismiss);var trophy=Band(card,"得宝",43,127,0,0);Surface(trophy,Night);
            trophy.gameObject.AddComponent<RectMask2D>();
            var hall=RoleFolio.Image(trophy,"楼台",Color.white);hall.sprite=ReleasedResources.Load<Sprite>("AuctionHall/MoonGallery");
            if(hall.sprite!=null)
            {
                hall.rectTransform.anchorMin=hall.rectTransform.anchorMax=new Vector2(.5f,.5f);
                var fit=hall.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio=hall.sprite.rect.width/hall.sprite.rect.height;
            }
            var medal=RoleFolio.Box(trophy,"合契玉轮");medal.anchorMin=medal.anchorMax=new Vector2(.5f,.53f);
            medal.sizeDelta=new Vector2(103,103);medal.anchoredPosition=Vector2.zero;
            var light=medal.gameObject.AddComponent<AuctionDaisLight>();light.raycastTarget=false;light.Rank=item?.rank??1;
            if(item!=null)
            {
                var icon=RoleFolio.Image(medal,"交割物件",Color.white);RoleFolio.Fill(icon.rectTransform,15,15,15,15);
                icon.sprite=Art(item);icon.preserveAspect=true;
            }
            AuctionHallLights.Add(medal,font).Burst("",true);
            Text(Band(card,"入册",174,54,12,12),(item==null ? "宝物" : item.name+" ×"+item.count)+"\n"+(detail??"交割已记入拍卖簿，宝物已收入纳戒。"),15,Ink,TextAnchor.MiddleCenter,true);
            Button(Band(card,"收印",236,44,16,16),"收起宝契",dismiss??CloseSheet,Ink);
        }

        internal static void PurchaseReceipt(Transform host,Font f,QZG_ItemModel item,Action dismiss)
        {
            if(host==null) return;
            var r=RoleFolio.Box(host,"自由市成交宝契");RoleFolio.Fill(r);
            var view=r.gameObject.AddComponent<AuctionHall>();view.font=f;
            view.ReceiptSheet(new Delivery { name=item.name,kind=item.goodsType,rank=item.rank,count=Math.Max(1,item.Num),reason="fixed" },
                "实付 "+Money(item.fixedPrice)+" 灵石，宝物已入纳戒。",dismiss);
        }
    }

    internal sealed class AuctionHallLights : MonoBehaviour
    {
        private Font font;
        internal static AuctionHallLights Add(RectTransform r,Font font)
        {var light=r.gameObject.AddComponent<AuctionHallLights>();light.font=font;return light;}
        internal void Burst(string seal,bool hammer) => StartCoroutine(Strokes(seal,hammer));
        private IEnumerator Strokes(string seal,bool hammer)
        {
            var area=(RectTransform)transform;var halo=RoleFolio.Box(area,"金箔印阵");
            halo.anchorMin=halo.anchorMax=new Vector2(.55f,.62f); halo.anchoredPosition=Vector2.zero;
            float size=Mathf.Min(Mathf.Max(70,area.rect.height*.92f),151);halo.sizeDelta=Vector2.one*size;
            var cel=halo.gameObject.AddComponent<RawImage>();cel.raycastTarget=false;cel.texture=ReleasedResources.Load<Texture2D>("AuctionHall/HammerSeal");
            if(cel.texture==null)cel.enabled=false;
            InkRitualEffect.Play(halo,hammer ? InkRitualEffect.Mark.Hammer : InkRitualEffect.Mark.Resonance,AuctionHall.Gold,true);
            if(!string.IsNullOrEmpty(seal))
            {
                var mark=RoleFolio.Label(halo,font,seal,19,AuctionHall.Gold,TextAnchor.MiddleCenter,true);RoleFolio.Fill(mark.rectTransform);
                var shadow=mark.gameObject.AddComponent<Shadow>();shadow.effectColor=AuctionHall.Night;shadow.effectDistance=new Vector2(1,-1);
            }
            float start=Time.unscaledTime,life=hammer ? .95f : .68f;
            while(Time.unscaledTime-start<life)
            {
                float p=(Time.unscaledTime-start)/life;int frame=Mathf.Min(15,Mathf.FloorToInt(p*16));
                cel.uvRect=new Rect((frame%4)*.25f,(3-frame/4)*.25f,.25f,.25f);
                halo.localScale=Vector3.one*Mathf.Lerp(.78f,1.19f,Mathf.SmoothStep(0,1,p));
                yield return null;
            }
            Destroy(halo.gameObject);
        }
        internal void Speak(Message m,int lane)=>StartCoroutine(Ribbon(m,lane));
        private IEnumerator Ribbon(Message m,int lane)
        {
            var rect=(RectTransform)transform;
            string label=(string.IsNullOrEmpty(m.room) ? m.name : m.room+" · "+m.name)+"　"+m.text;
            bool gold=m.effect=="gold",wine=m.effect=="amber" || m.effect=="wine";
            float width;
            var r=RoleFolio.Box(rect,"席间传音");r.pivot=new Vector2(0,1);r.anchorMin=r.anchorMax=Vector2.up;r.sizeDelta=new Vector2(100,24);
            AuctionHall.Surface(r,gold ? new Color32(111,72,42,245) : wine ? new Color32(101,57,61,235) :
                m.effect=="jade" ? new Color32(52,87,70,234) : new Color32(38,56,62,222));
            var text=AuctionHall.Caption(r,font,label,13,gold ? AuctionHall.Gold : AuctionHall.Ivory,TextAnchor.MiddleCenter,gold);
            text.resizeTextForBestFit=false;text.horizontalOverflow=HorizontalWrapMode.Overflow;
            width=Mathf.Max(76,text.preferredWidth+16);r.sizeDelta=new Vector2(width,24);
            float age=0;float duration=Mathf.Max(4f,(rect.rect.width+width)/(gold ? 95f : 115f));
            while(age<duration)
            {
                r.anchoredPosition=new Vector2(Mathf.Lerp(rect.rect.width,-width,age/duration),-5-(lane%3)*26);
                age+=Time.unscaledDeltaTime;yield return null;
            }
            Destroy(r.gameObject);
        }
    }
}
