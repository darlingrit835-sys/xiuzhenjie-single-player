using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class MeridianAtlasPanel : MonoBehaviour
    {
        private static readonly Color Night = new Color32(9, 26, 34, 222);
        private static readonly Color Jade = new Color32(141, 221, 196, 255);
        private static readonly Color Silk = new Color32(248, 235, 203, 255);
        private static readonly Color Gold = new Color32(241, 202, 133, 255);
        private static readonly string[] Threads = {"任脉","督脉","冲脉","带脉","阴跷脉","阳跷脉","阴维脉","阳维脉"};
        private NeiShiPanel panel;
        private Font font;
        private Text summary, selection, message, magnification, requirement;
        private RectTransform threadPicker;
        private float messageUntil;
        private InkMeridianSpectrum Atlas => MonoSingleton<JingMaiManager>.Instance?.Spectrum;

        internal static MeridianAtlasPanel Build(NeiShiPanel panel)
        {
            var view=panel.gameObject.AddComponent<MeridianAtlasPanel>();
            view.panel=panel;
            view.font=panel.NengLianText.font;
            view.Layout();
            return view;
        }

        private void Layout()
        {
            var root = (RectTransform)panel.transform;
            RoleFolio.Fill(root);
            RectTransform header = Surface(root,"内景 · 天幕",Night,0,.856f,1,1);
            Trim(header,false);
            Label(header,"内视经脉",26,Silk,.055f,.23f,.76f,.84f);
            summary=Label(header,"奇经八脉",12,Jade,.06f,0,.88f,.35f);
            MoveButton(panel.LiKai_Btn,header,"返回",.75f,.40f,.965f,.88f);
            // Keep the existing help, overview and confirmation buttons as guide targets.
            MoveButton(panel.WenHao,header,"?",.88f,.06f,.965f,.37f);
            var oldTitle = root.Find("Title_image");
            if (oldTitle) oldTitle.gameObject.SetActive(false);
            var balance=panel.NengLianText;
            var oldBalance=balance.transform.parent;
            balance.transform.SetParent(root,false);
            var balanceBar=Surface(root,"丹田经脉点",new Color32(15,38,44,199),.285f,.793f,.715f,.84f);
            balance.transform.SetParent(balanceBar,false);
            Place(balance.rectTransform,0,0,1,1);
            RoleFolio.Style(balance,16,Gold,TextAnchor.MiddleCenter,true);
            if(oldBalance!=root) oldBalance.gameObject.SetActive(false);

            var drawer=Surface(root,"灵台",Night,0,0,1,.215f);
            Trim(drawer,true);
            selection=Label(drawer,"胞中",19,Silk,.045f,.56f,.69f,.93f);
            message=Label(drawer,"",12,Jade,.045f,.34f,.965f,.61f);
            Button zoom=Button(drawer,"近观",.72f,.62f,.96f,.94f,out magnification);
            zoom.onClick.AddListener(()=>{ var a=Atlas;if(a==null)return;a.SetView(!a.IsCloseView);Refresh(); });
            Button select=Button(drawer,"循脉",.04f,.06f,.31f,.33f,out _);
            select.onClick.AddListener(()=>threadPicker.gameObject.SetActive(!threadPicker.gameObject.activeSelf));
            var inspect=Button(drawer,"察穴",.355f,.06f,.64f,.33f,out _);
            inspect.onClick.AddListener(()=>{var a=Atlas;if(a?.FocusedItem!=null)MonoSingleton<JingMaiManager>.Instance.ShowAcupoint(a.FocusedItem);});
            MoveButton(panel.ZongLan_Btn,drawer,"总览",.69f,.06f,.96f,.33f);
            threadPicker=Surface(root,"循脉择道",Night,.035f,.224f,.965f,.463f,true);
            Label(threadPicker,"择一脉循行",15,Gold,.04f,.69f,.70f,.97f);
            var close=Button(threadPicker,"收起",.78f,.70f,.96f,.95f,out _);
            close.onClick.AddListener(()=>threadPicker.gameObject.SetActive(false));
            for(int i=0;i<Threads.Length;i++) {
                string name=Threads[i];
                float x=.035f+.235f*(i%4), y=i<4?.36f:.04f;
                var branch=Button(threadPicker,name,x,y,x+.22f,y+.273f,out _);
                branch.onClick.AddListener(()=>{
                    Atlas?.FollowThread(name);
                    threadPicker.gameObject.SetActive(false); Refresh();
                });
            }
            threadPicker.gameObject.SetActive(false);
            var detail=panel.TiShiPanel;
            detail.SetAsLastSibling();
            RemodelDetail(detail);
        }

        private void RemodelDetail(RectTransform detail)
        {
            // Bound value labels and buttons retain their original objects and listeners.
            var backing=detail.GetComponent<Image>();
            if(backing) RoleFolio.Flat(backing,new Color32(16,36,44,249),true);
            Place(detail,.035f,.045f,.965f,.74f);
            foreach(Transform child in detail) child.gameObject.SetActive(false);
            var band=Surface(detail,"穴位题签",new Color32(18,49,51,250),0,.83f,1,1);
            Trim(band,false);
            Bound(panel.TiShiPanel_TitleText,band,.055f,.07f,.65f,.93f,23,Silk);
            MoveButton(panel.TiShiPanel_CloseBtn,band,"关闭",.78f,.19f,.96f,.81f);
            DetailRow(detail,"归脉",panel.TiShiPanel_GuiMaiText,.73f);
            DetailRow(detail,"当前层数",panel.TiShiPanel_DangQianCengShuText,.635f);
            DetailRow(detail,"所需经脉点",panel.TiShiPanel_TuPoXiaoHaoText,.54f);
            DetailRow(detail,"通穴几率",panel.TiShiPanel_ChengGongJiLvText,.445f);
            requirement=Label(detail,"下一层功效",14,Gold,.06f,.350f,.95f,.448f);
            Bound(panel.TiShiPanel_XiaoGuoText,detail,.06f,.16f,.94f,.36f,13,Silk);
            MoveButton(panel.TiShiPanel_WaitBtn,detail,"收势",.05f,.038f,.35f,.145f);
            MoveButton(panel.TiShiPanel_TongXueBtn,detail,"运气冲穴",.39f,.038f,.95f,.145f);
        }

        private void DetailRow(RectTransform detail,string caption,Text value,float y)
        {
            Surface(detail,"分隔",new Color32(99,138,136,74),.06f,y,.95f,y+.0024f);
            Label(detail,caption,14,Jade,.06f,y,.42f,y+.086f);
            Bound(value,detail,.415f,y,.95f,y+.086f,14,Silk);
            value.alignment=TextAnchor.MiddleRight;
        }

        internal void Refresh()
        {
            var atlas=Atlas;
            var point=atlas?.Focused;
            if(point==null) return;
            summary.text="奇经八脉   /   已通 "+atlas.OpenedCount+" · "+atlas.PointCount+" 穴";
            string status=atlas.FocusedItem.CurrentStatus==XueWeiStatus.Used ? "已通" :
                atlas.FocusedItem.CurrentStatus==XueWeiStatus.Usable ? "可冲穴" : "待前穴";
            selection.text=point.b+"  ·  "+point.c+"  /  "+status;
            magnification.text=atlas.IsCloseView?"全图":"近观";
            if(Time.unscaledTime>=messageUntil)
                message.text=atlas.Thread==null ? "点选星穴察看功效，拖动可移步观脉" :
                    atlas.Thread+"  ·  已通 "+atlas.CountInThread(atlas.Thread,true)+" / "+atlas.CountInThread(atlas.Thread,false)+" 穴";
        }

        internal void SetRequirement(AcupointModel point, bool locked, bool complete)
        {
            if(locked && !string.IsNullOrEmpty(point.g)) {
                var names=new System.Collections.Generic.List<string>();
                foreach(var id in point.g.Split(','))
                    if(Atlas?.NameOf(id) is string name) names.Add(name);
                requirement.text=names.Count>0 ? "须先贯通  " + string.Join(" / ",names) : "下一层功效";
            }
            else requirement.text=complete ? "圆满功效" : "下一层功效";
        }

        internal void Charge(string id) { Atlas?.Charge(id); }
        internal void CancelCharge() { Atlas?.StopCharge(); }
        internal void Resolve(string id,bool won,int level)
        {
            var atlas=Atlas;
            if(atlas==null) return;
            atlas.Resolve(id,won);
            messageUntil=Time.unscaledTime+3.5f;
            message.text=won ? atlas.Focused.b+"贯通  ·  已达"+level+"层" : "气息回落  ·  此次未能破层";
            Refresh();
        }

        private void Update()
        {
            if(messageUntil>0 && Time.unscaledTime>=messageUntil) { messageUntil=0;Refresh(); }
        }

        private static void Place(RectTransform rect,float l,float b,float r,float t)
        {
            rect.anchorMin=new Vector2(l,b);rect.anchorMax=new Vector2(r,t);rect.offsetMin=rect.offsetMax=Vector2.zero;
        }
        private static RectTransform Surface(Transform parent,string name,Color color,float l,float b,float r,float t,bool hit=false)
        {
            var im=RoleFolio.Image(parent,name,color,hit);Place(im.rectTransform,l,b,r,t);return im.rectTransform;
        }
        private static void Trim(Transform area,bool top)
        {
            float t=top?.994f:0;
            Surface(area,"金线",new Color32(202,171,115,193),0,t,1,t+.006f);
            Surface(area,"青金",new Color32(141,223,201,222),.042f,t,.136f,t+.006f);
        }

        private static void Edge(RectTransform rect)
        {
            var frame=new Color32(182,164,113,179);
            Surface(rect,"上沿",frame,0,.969f,1,1);
            Surface(rect,"下沿",frame,0,0,1,.03f);
            Surface(rect,"左沿",frame,0,0,.007f,1);
            Surface(rect,"右沿",frame,.993f,0,1,1);
        }

        internal static Text[] StyleOverview(ZongLanPanel overview)
        {
            var face=(RectTransform)overview.transform.Find("ZongLan");
            var font=overview.RenMaiText.font;
            foreach(Transform child in face) child.gameObject.SetActive(false);
            RoleFolio.Flat(face.GetComponent<Image>(),new Color32(12,31,38,250),true);
            Place(face,.045f,.065f,.955f,.90f);
            var band=Surface(face,"八脉题签",new Color32(20,48,51,250),0,.85f,1,1);
            Trim(band,false);
            Text Heading(Transform parent,string value,int size,Color color,float l,float b,float r,float t) {
                var label=RoleFolio.Label(parent,font,value,size,color);
                Place(label.rectTransform,l,b,r,t);
                return label;
            }
            Heading(band,"八脉总览",23,Silk,.055f,.05f,.68f,.96f);
            var back=overview.CloseBtn;
            back.transform.SetParent(band,false);
            back.gameObject.SetActive(true);
            foreach(Transform child in back.transform) child.gameObject.SetActive(false);
            Place((RectTransform)back.transform,.75f,.19f,.96f,.79f);
            var image=back.GetComponent<Image>();
            RoleFolio.Flat(image,new Color32(38,68,67,255),true);back.targetGraphic=image;
            Edge(image.rectTransform);
            Heading(back.transform,"返回",14,Silk,0,0,1,1).alignment=TextAnchor.MiddleCenter;
            Heading(face,"经脉",12,Jade,.055f,.775f,.27f,.845f);
            Heading(face,"已通穴位",12,Jade,.29f,.775f,.53f,.845f);
            Heading(face,"贯通所得",12,Jade,.55f,.775f,.94f,.845f);
            Text[] counts={overview.RenMaiText,overview.DuMaiNumText,overview.ChongMaiText,overview.DaiMaiText,overview.YinQiaoMaiText,overview.YangQiaoMaiText,overview.YinWeiMaiText,overview.YangWeiMaiText};
            Text[] bonuses={overview.Extra_NaQiSuLvText,overview.Extra_NaQiXiaoLvText,overview.Extra_JingXueShangXianText,overview.Extra_JingXueHuiFuText,overview.Extra_ShenShiShangXianText,overview.Extra_WuXingShangHaiText,overview.Extra_TiPoShangXianText,overview.Extra_WuXingFangYuText};
            string[] captions={"纳气速率","纳气效率","精血上限","精血恢复","神识上限","五行伤害","体魄上限","五行防御"};
            var levels=new Text[Threads.Length];
            for(int i=0;i<Threads.Length;i++) {
                float top=.768f-i*.091f;
                var row=Surface(face,Threads[i],i%2==0 ? new Color32(26,53,57,240):new Color32(20,42,48,232),.034f,top-.083f,.966f,top);
                Heading(row,Threads[i],16,Silk,.027f,0,.26f,1);
                Bound(counts[i],row,.28f,.37f,.50f,.99f,15,Gold);
                levels[i]=Heading(row,"",11,Jade,.28f,0,.53f,.43f);
                Heading(row,captions[i],12,Jade,.525f,0,.78f,1);
                Bound(bonuses[i],row,.752f,0,.978f,1,13,Silk);
                bonuses[i].alignment=TextAnchor.MiddleRight;
            }
            return levels;
        }

        private Text Label(Transform parent,string text,int size,Color color,float l,float b,float r,float t)
        {
            var label=RoleFolio.Label(parent,font,text,size,color);
            Place(label.rectTransform,l,b,r,t);return label;
        }
        private static void Bound(Text text,Transform parent,float l,float b,float r,float t,int size,Color color)
        {
            text.transform.SetParent(parent,false);
            text.gameObject.SetActive(true);
            RoleFolio.Style(text,size,color,TextAnchor.MiddleLeft,false,11);
            text.supportRichText=true;
            Place(text.rectTransform,l,b,r,t);
        }
        private Button Button(Transform parent,string caption,float l,float b,float r,float t,out Text text)
        {
            var surface=Surface(parent,caption,new Color32(51,86,80,255),l,b,r,t,true);
            Edge(surface);
            text=RoleFolio.Label(surface,font,caption,14,Silk,TextAnchor.MiddleCenter);
            RoleFolio.Fill(text.rectTransform);
            var button=surface.gameObject.AddComponent<Button>();button.targetGraphic=surface.GetComponent<Image>();return button;
        }
        private void MoveButton(Button button,Transform parent,string caption,float l,float b,float r,float t)
        {
            button.transform.SetParent(parent,false);
            button.gameObject.SetActive(true);
            foreach(Transform child in button.transform) child.gameObject.SetActive(false);
            var rect=(RectTransform)button.transform;Place(rect,l,b,r,t);
            var fill=button.GetComponent<Image>()??button.gameObject.AddComponent<Image>();
            RoleFolio.Flat(fill,new Color32(53,87,78,255),true);button.targetGraphic=fill;
            Edge(rect);
            var label=RoleFolio.Label(rect,font,caption,14,Silk,TextAnchor.MiddleCenter);
            RoleFolio.Fill(label.rectTransform);
        }
    }
}
