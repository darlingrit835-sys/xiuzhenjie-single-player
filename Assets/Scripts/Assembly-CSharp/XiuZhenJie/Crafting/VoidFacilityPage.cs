using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class VoidFacilityPage
    {
        [Serializable] internal class Query { public string action, hall, mode; public int brief, answer, stage, page; }
        [Serializable] internal class Hall { public string id; public int rank,seals; }
        [Serializable] internal class Work { public string id,name,hallId; public int grade,score; }
        [Serializable] internal class Rank { public string Name,Faction; public int Level,Age,TierNum; }
        [Serializable] internal class Reply
        {
            public string error,name,hint,teaching,question;
            public int stage,best,next,mine,count;
            public bool passed,correct,more;
            public string[] choices;
            public Hall[] halls;
            public Work[] works;
            public Rank[] ranks;
        }

        private readonly Window owner;
        private readonly Font font;
        private readonly bool tower;
        private RectTransform root,body;
        private int epoch;
        private bool waiting;
        private static readonly Color Paper=new Color32(236,226,199,255), Ink=new Color32(53,45,29,255);
        private static readonly Color Jade=new Color32(41,74,66,255), Gold=new Color32(213,181,117,255);

        internal VoidFacilityPage(Window window,bool isTower)
        {
            owner=window;tower=isTower;
            font=window.GameObject.GetComponentInChildren<Text>(true)?.font ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        private static RectTransform Node(Transform parent,string name)
        {
            var o=new GameObject(name,typeof(RectTransform)); var r=(RectTransform)o.transform; r.SetParent(parent,false); return r;
        }
        private static void Place(RectTransform r,float x,float y,float X,float Y,float l=0,float b=0,float e=0,float t=0)
        {
            r.anchorMin=new Vector2(x,y); r.anchorMax=new Vector2(X,Y);r.offsetMin=new Vector2(l,b);r.offsetMax=new Vector2(-e,-t);
        }
        private static Image Paint(RectTransform r,Color color)
        {
            var i=r.gameObject.AddComponent<Image>();i.color=color;i.raycastTarget=false;return i;
        }
        private Text TextOn(RectTransform rect,string words,int size,Color color,TextAnchor anchor=TextAnchor.MiddleLeft)
        {
            var tx=rect.gameObject.AddComponent<Text>();tx.font=font;tx.fontSize=size;tx.text=words;tx.color=color;
            tx.alignment=anchor;tx.raycastTarget=false;tx.supportRichText=false;return tx;
        }
        internal void Cancel(){epoch++;waiting=false;}
        internal void Hide()
        {
            Cancel();
            if(root!=null){root.gameObject.SetActive(false);UnityEngine.Object.Destroy(root.gameObject);root=null;}
        }
        internal void Begin(string title,string subtitle,Action back)
        {
            Hide();
            root=Node(owner.GameObject.transform,title);Place(root,0,0,1,1);
            var paper=Paint(root,Paper);paper.sprite=InkInterfaceArt.Get("Paper");paper.raycastTarget=true;
            var hero=Node(root,"殿景");Place(hero,0,.68f,1,1);hero.gameObject.AddComponent<RectMask2D>();
            var picture=Node(hero,"山水");Place(picture,0,0,1,1);
            var art=Paint(picture,Color.white);art.sprite=ReleasedResources.Load<Sprite>("VoidRealm/"+(tower?"studio":"hall"));
            if(art.sprite!=null){
                var fit=picture.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio=art.sprite.rect.width/art.sprite.rect.height;
            }
            var band=Node(hero,"墨漆题签");Place(band,0,0,1,0,0,0,0,-60);Paint(band,new Color32(21,41,37,232));
            var h=Node(band,"篇名");Place(h,0,0,1,1,14,21,5,2);TextOn(h,title,22,Paper);
            var sub=Node(band,"小题");Place(sub,0,0,1,0,15,4,4,-22);TextOn(sub,subtitle,12,Gold);
            var ret=Node(hero,"返回");Place(ret,0,1,0,1,9,-35,-63,7);MakeButton(ret,"返回",back,true);
            var viewport=Node(root,"卷轴");Place(viewport,0,0,1,.68f,5,0,5,0);
            var surface=Paint(viewport,new Color(1,1,1,.001f));surface.raycastTarget=true;
            viewport.gameObject.AddComponent<RectMask2D>();var scroll=viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport=viewport;scroll.horizontal=false;scroll.movementType=ScrollRect.MovementType.Clamped;
            body=Node(viewport,"卷页");body.anchorMin=new Vector2(0,1);body.anchorMax=Vector2.one;body.pivot=new Vector2(.5f,1);body.sizeDelta=Vector2.zero;
            var layout=body.gameObject.AddComponent<VerticalLayoutGroup>();layout.spacing=5;layout.padding=new RectOffset(9,9,8,12);
            layout.childControlWidth=true;layout.childControlHeight=true;layout.childForceExpandHeight=false;
            body.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;scroll.content=body;
        }
        internal void Copy(string words)
        {
            var line=Node(body,"札记");var tx=TextOn(line,words,14,Ink);
            tx.horizontalOverflow=HorizontalWrapMode.Wrap;tx.verticalOverflow=VerticalWrapMode.Overflow;
            line.gameObject.AddComponent<LayoutElement>().minHeight=25;
        }
        internal void Action(string text,Action click,bool primary=false)
        {
            var row=Node(body,text);row.gameObject.AddComponent<LayoutElement>().preferredHeight=37;MakeButton(row,text,click,primary);
        }
        private void MakeButton(RectTransform row,string text,Action click,bool primary)
        {
            var bg=Paint(row,primary?Jade:new Color32(221,207,173,255));bg.raycastTarget=true;
            var rule=Node(row,"金边");Place(rule,0,0,1,0,0,0,0,-1);Paint(rule,Gold);
            var lab=Node(row,"题字");Place(lab,0,0,1,1,7,1,7,1);TextOn(lab,text,14,primary?Paper:Ink,TextAnchor.MiddleCenter);
            var btn=row.gameObject.AddComponent<Button>();btn.targetGraphic=bg;btn.onClick.AddListener(()=>{if(!waiting)click();});
        }
        internal void Fetch(Query q,Action<Reply> draw)
        {
            if(waiting)return;
            int active=epoch;waiting=true;
            ForgeWire.Send((MsgType)4901076,JsonUtility.ToJson(q),(ok,raw)=>{
                if(active!=epoch||owner.GameObject==null||!owner.GameObject.activeInHierarchy)return;
                waiting=false;Reply answer=null;
                if(!string.IsNullOrEmpty(raw)){try{answer=JsonUtility.FromJson<Reply>(raw);}catch(ArgumentException){}}
                if(!ok||answer==null||!string.IsNullOrEmpty(answer.error)){
                    MonoSingleton<GameStart>.Instance.PopUpAttention(answer?.error??"殿籍暂未送达，请重试。");
                    Action("重试",()=>Fetch(q,draw));return;
                }
                draw(answer);
            });
        }
    }
}
