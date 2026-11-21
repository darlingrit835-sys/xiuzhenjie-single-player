using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class SpiritGarden : MonoBehaviour
	{
		[Serializable] private class Request
		{
			public string action, requestId, seed, soil, target, scope, cropId;
			public int plot;
			public bool keepSeed;
		}
		[Serializable] private class Plot
		{
			public int id, remaining, @yield;
			public string soil, seed, name, state, cropId;
			public bool ward, wardUsed, canWard, canSteal, canKeepSeed;
		}
		[Serializable] private class Seed { public string id, name, material, use, soil; public int price, count, seconds, @yield; }
		[Serializable] private class Soil { public string id, name, description; public int cost; }
		[Serializable] private class Neighbor { public string id, name; public bool friend; public int ripe; }
		[Serializable] private class Reply
		{
			public bool own;
			public string error, name, owner, message;
			public int wardCost, wardCharges, attemptsLeft;
			public long stones;
			public Plot[] plots;
			public Seed[] seeds;
			public Soil[] soils;
			public string[] logs;
			public Neighbor[] neighbors;
		}

		private Font font;
		private Action leave, association;
		private Reply garden, visitors;
		private RectTransform body;
		
		private bool busy, refreshFailed;
		private int index = 1;
		private string tab = "田畦", seedID = "dew", soilID = "loam", scope = "friends", notice, pendingKey;
		private float updatedAt, redrawAt;
		private static readonly Color Pine = new Color32(14,35,32,255), Leaf = new Color32(44,76,61,255);
        private static readonly Color Cream = new Color32(248,240,210,255), Brass = new Color32(235,199,128,255);
        private static readonly Color Sage = new Color32(187,205,179,255), Bronze = new Color32(136,129,88,255);
        private static readonly Color Clear = new Color(0,0,0,0);
        private Texture2D landscape, specimens;
        private ScrollRect scroll;
        private string scrollKey;
        private Text clockText;
        private Plot clockPlot;
        private static readonly string[] Pages = { "田畦", "种子", "访田", "田簿" };
        private static readonly SpiritGardenArt.Motif[] Icons = { SpiritGardenArt.Motif.Beds, SpiritGardenArt.Motif.Seed, SpiritGardenArt.Motif.Gate, SpiritGardenArt.Motif.Book };
		

		internal static void Open(RectTransform parent, Font font, Action leave, Action association = null)
		{
			var existing = parent.Find("SpiritGarden");
			if (existing != null) { existing.GetComponent<SpiritGarden>().Refresh(); return; }
			var root = Area(parent, "SpiritGarden", 0, 0, 1, 1);
			root.gameObject.AddComponent<Image>().color = Pine;
			var view = root.gameObject.AddComponent<SpiritGarden>();
			view.font = font;
            view.landscape = ReleasedResources.Load<Texture2D>("Artisan11/GardenHall");
            view.specimens = ReleasedResources.Load<Texture2D>("Artisan11/Herbs");
			view.leave = leave;
			view.association = association;
			view.pendingKey = $"GardenOrder.{Constants.LoginServerIP}.{Constants.LoginServerPort}.{Singleton<RoleManager>.Instance.currentRoleId}";
			view.Refresh();
		}

		private void Refresh()
		{
			if (busy) return;
			var pending = PlayerPrefs.GetString(pendingKey, "");
			if (!string.IsNullOrEmpty(pending)) { Send(JsonUtility.FromJson<Request>(pending)); return; }
			if (tab == "访田") Send(new Request { action = "neighbors", scope = scope });
			else if (garden != null && !garden.own) Visit(garden.owner);
			else Send(new Request { action = "state" });
		}
		private void Home() { tab = "田畦"; Send(new Request { action = "state" }); }
		private void Visit(string id) { tab = "田畦"; index = 1; Send(new Request { action = "visit", target = id }); }
		private void Send(Request req)
		{
			if (busy) return;
			bool write = !string.IsNullOrEmpty(req.requestId);
			string json = JsonUtility.ToJson(req);
			if (write) { PlayerPrefs.SetString(pendingKey, json); PlayerPrefs.Save(); }
			busy = true;
			notice = "正在核对田簿…";
			Draw();
			ForgeWire.Send(MsgType.SpiritGarden, json, (ok, reply) =>
			{
				if (this == null) return;
				busy = false; refreshFailed = !ok;
				Reply data = null;
				try { if (!string.IsNullOrEmpty(reply)) data = JsonUtility.FromJson<Reply>(reply); }
				catch (ArgumentException) { }
				bool valid = req.action == "neighbors" ? data?.neighbors != null : data?.plots != null;
				if (write && (ok && valid || !string.IsNullOrEmpty(data?.error)))
				{ PlayerPrefs.DeleteKey(pendingKey); PlayerPrefs.Save(); }
				if (!ok || !valid)
				{
					notice = data?.error ?? "连接中断，请点「重读田簿」。";
					Draw(); return;
				}
				notice = data.message;
				if (req.action == "neighbors") visitors = data;
				else
				{
					garden = data; updatedAt = Time.realtimeSinceStartup;
					if (data.own && Singleton<RoleManager>.Instance.Role_Entity != null)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = data.stones;
						Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
					}
					if (req.action == "harvest" || req.action == "steal")
					{
						var boxes = Singleton<RoleManager>.Instance.RoleBoxDic;
						if (boxes != null && boxes.TryGetValue(10, out var box))
							box.GetAllGoods(() => Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData(), false);
					}
				}
				Draw();
			});
		}
		private void Write(string action, Plot p = null, bool keep = false)
		{
			Send(new Request { action = action, requestId = Guid.NewGuid().ToString("N"), plot = p?.id ?? 0,
				cropId = p?.cropId, target = garden.own ? null : garden.owner, seed = seedID, soil = soilID, keepSeed = keep });
		}
		private void Go(string page)
		{
			if (page == "访田") { tab = page; visitors = null; Refresh(); return; }
			if (garden == null || !garden.own) { Home(); return; }
			tab = page; notice = null; Draw();
		}
		private int Left(Plot p) => Mathf.Max(0, p.remaining - Mathf.FloorToInt(Time.realtimeSinceStartup - updatedAt));
		private void Update()
		{
			if (busy || tab != "田畦" || garden?.plots == null || Time.realtimeSinceStartup < redrawAt) return;
			redrawAt = Time.realtimeSinceStartup + 1f;
			if (garden.plots.Any(p => p.state == "growing" && Left(p) == 0) && !refreshFailed) Refresh();
			else if (clockText != null && clockPlot != null && clockPlot.state == "growing")
				clockText.text = Duration(Left(clockPlot)) + " 后成熟";
		}

		private static RectTransform Area(Transform parent, string name, float l, float t, float r, float b)
		{
			var obj = new GameObject(name, typeof(RectTransform));
			obj.layer = parent.gameObject.layer;
			var rect = (RectTransform)obj.transform; rect.SetParent(parent, false);
			rect.anchorMin = new Vector2(l, 1-b); rect.anchorMax = new Vector2(r, 1-t);
			rect.offsetMin = rect.offsetMax = Vector2.zero; return rect;
		}
        private Text Label(RectTransform parent, string id, string value, float left, float top, float right, float bottom,
            int size, Color color, TextAnchor align = TextAnchor.MiddleLeft, bool bold = false)
        {
            var t=Area(parent,id,left,top,right,bottom).gameObject.AddComponent<Text>();
            t.font=font; t.text=value; t.color=color; t.alignment=align; t.raycastTarget=false;
            t.supportRichText=false; t.fontStyle=bold?FontStyle.Bold:FontStyle.Normal;
            t.fontSize=size; t.resizeTextForBestFit=true; t.resizeTextMinSize=Mathf.Min(13,size); t.resizeTextMaxSize=size;
            return t;
        }
        private static SpiritGardenArt Art(RectTransform parent, SpiritGardenArt.Motif style, Color a, Color b, Color edge)
        {
            var art=parent.gameObject.AddComponent<SpiritGardenArt>(); art.Set(style,a,b,edge);return art;
        }
        private static void Paint(RectTransform parent, Color color, bool receivesTouch=false)
        {
            var image=parent.gameObject.AddComponent<Image>();image.color=color;image.raycastTarget=receivesTouch;
        }
        private static void ImageArea(RectTransform parent,Texture2D image,Rect region,Vector2 focus)
        {
            if(image==null)return;
            parent.gameObject.AddComponent<SpiritGardenImage>().Set(image,region,focus);
        }
        private void Landscape(RectTransform rect, float focus=.48f)
        {
            if (landscape!=null) ImageArea(rect,landscape,new Rect(0,0,1,1),new Vector2(.5f,focus));
            else {
                var pic=rect.gameObject.AddComponent<Image>();
                InkInterfaceArt.Set(pic,"HerbariumTitle",false);pic.raycastTarget=false;
            }
        }
        private void Herb(RectTransform rect,string seed, bool ward=false)
        {
            int n=seed=="reed"?1:seed=="lotus"?2:0;
            if (specimens!=null) ImageArea(rect,specimens,new Rect(n/3f,0,1/3f,1),new Vector2(.5f,.53f));
            else {
                var pic=rect.gameObject.AddComponent<Image>();InkInterfaceArt.Set(pic,n==2?"Lotus":"Goods/Herb",true);pic.raycastTarget=false;
            }
            if(ward)Art(Area(rect,"Formation",.01f,.35f,.99f,.99f),SpiritGardenArt.Motif.Ward,
                new Color32(201,255,170,255),new Color32(146,244,147,136),new Color32(255,215,108,234));
        }
        private static void Plate(RectTransform r, bool lit=false)
        {
            Art(r,SpiritGardenArt.Motif.Plate,lit?new Color32(88,103,63,255):new Color32(37,65,54,249),
                lit?new Color32(39,67,49,255):new Color32(16,39,35,248),lit?Brass:Bronze);
        }
        private static Button Bind(RectTransform r,Graphic target,Action fire,bool enabled)
        {
            target.raycastTarget=true;
            var b=r.gameObject.AddComponent<Button>(); b.targetGraphic=target; b.interactable=enabled;
            var c=b.colors;c.disabledColor=new Color(.62f,.62f,.58f,.83f);c.highlightedColor=new Color(1f,1f,.91f);
            c.pressedColor=new Color(.75f,.88f,.75f);b.colors=c;b.onClick.AddListener(()=>fire());return b;
        }
        private Button Key(RectTransform r,string caption,Action fire,bool active=false,bool enabled=true,int size=17)
        {
            var p=Art(r,SpiritGardenArt.Motif.Plate,active?new Color32(136,106,56,255):new Color32(54,79,63,255),
                active?new Color32(79,65,40,255):Pine,active?Brass:Bronze);
            var b=Bind(r,p,fire,enabled&&!busy);
            Label(r,"Label",caption,.035f,.04f,.965f,.96f,size,enabled?Cream:Sage,TextAnchor.MiddleCenter,active);return b;
        }
        private RectTransform Row(string id,float height)
        {
            var r=Area(body,id,0,0,1,1);
            r.gameObject.AddComponent<LayoutElement>().preferredHeight=height;return r;
        }
        private void Section(string name,string aside=null)
        {
            var r=Row("Section",34);
            Paint(Area(r,"Rule",.005f,.24f,.012f,.78f),Brass);
            Label(r,"Title",name,.038f,0,.61f,1,19,Brass,TextAnchor.MiddleLeft,true);
            if(!string.IsNullOrEmpty(aside))Label(r,"Detail",aside,.55f,.03f,.99f,1,14,Sage,TextAnchor.MiddleRight);
        }
        private void ActionRow(string id,string caption,Action fire,bool enabled=true,bool active=true)
        {
            Key(Row(id,49),caption,fire,active,enabled);
        }
        private static string Duration(int s) => s>=60?$"{s/60}分{s%60:00}秒":$"{s}秒";
        private static string SoilName(string soil) => soil=="sand"?"寒砂土":soil=="silt"?"灵淤泥":"青壤";
        private static string TabTitle(string tab) => tab=="种子"?"灵植种坊":tab=="访田"?"山间访田":tab=="田簿"?"巡田手记":"洞府灵圃";
        private string FocusName { get { return tab=="田畦" && garden!=null && !garden.own?garden.name+"的灵田":TabTitle(tab); } }

        private void Draw()
        {
            string nextKey=tab+"|"+(garden?.owner??"")+"|"+(tab=="访田"?scope:"");
            float previous=scroll!=null&&scrollKey==nextKey?scroll.content.anchoredPosition.y:0;
            scrollKey=nextKey;
            for(int i=transform.childCount-1;i>=0;i--){var old=transform.GetChild(i).gameObject;old.SetActive(false);Destroy(old);}
            clockText=null;clockPlot=null;
            // The fixed roofline and lower terrace remain visible around the scrollable field book.
            var back=Area(transform,"Valley",0,0,1,1);Landscape(back);
            Art(Area(transform,"Veil",0,0,1,1),SpiritGardenArt.Motif.ShadeBottom,
                new Color32(9,28,25,246),new Color32(14,34,30,66),Clear);
            var header=Area(transform,"Pavilion",0,0,1,.273f);
            Landscape(Area(header,"View",0,0,1,1),.53f);
            Art(Area(header,"SideShade",0,0,.88f,1),SpiritGardenArt.Motif.ShadeLeft,new Color32(8,28,25,206),Clear,Clear);
            Art(Area(header,"Footshade",0,.58f,1,1),SpiritGardenArt.Motif.ShadeBottom,new Color32(10,30,26,240),Clear,Clear);
            var name=Label(header,"Title",FocusName,.056f,.17f,.79f,.58f,39,Cream,TextAnchor.MiddleLeft,true);
            var shadow=name.gameObject.AddComponent<Shadow>();shadow.effectColor=new Color(0,0,0,.58f);shadow.effectDistance=new Vector2(1,-2);
            Key(Area(header,"Leave",.785f,.06f,.956f,.30f),"返回",leave,false,true,16);
            var status=Area(header,"Seal",.055f,.63f,.91f,.92f);
            Paint(Area(status,"Gold",0,.19f,.009f,.83f),Brass);
            string resources=garden==null?"正在展开田簿":tab=="访田"?$"今日尚可偷采 {garden.attemptsLeft} 次":garden.own?$"灵石 {garden.stones}　·　护田符 {garden.wardCharges} 重":$"今日尚可偷采 {garden.attemptsLeft} 次";
            Label(status,"Balance",resources,.034f,.06f,.98f,.94f,17,Cream);
            Paint(Area(transform,"Lintel",0,.271f,1,.274f),Brass);
            var pane=Area(transform,"Scroll",.038f,.285f,.962f,.844f);
            Paint(pane,new Color32(13,36,31,224),true);pane.gameObject.AddComponent<RectMask2D>();
            scroll=pane.gameObject.AddComponent<ScrollRect>();scroll.viewport=pane;scroll.horizontal=false;scroll.movementType=ScrollRect.MovementType.Clamped;
            body=Area(pane,"Contents",0,0,1,0);body.pivot=Vector2.up;
            var layout=body.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding=new RectOffset(8,8,6,12);layout.spacing=6;layout.childControlWidth=layout.childControlHeight=true;
            layout.childForceExpandWidth=true;layout.childForceExpandHeight=false;
            body.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;scroll.content=body;
            bool pending=!busy&&PlayerPrefs.HasKey(pendingKey);
            if(pending)MessageCard("等待回执","重读田簿可核对上次操作结果。",SpiritGardenArt.Motif.Book);
            else if(tab=="访田") DrawVisitors();
            else if(garden?.plots!=null){
                if(tab=="田簿")DrawLog();else if(tab=="种子")DrawSeeds();else DrawPlots();
            }
            else if(busy)MessageCard("正展开田簿","",SpiritGardenArt.Motif.Book);
            var bar=Area(transform,"Footnote",.037f,.848f,.963f,.905f);
            string receipt=notice;
            if(string.IsNullOrEmpty(receipt)&&tab=="田畦"&&garden?.plots!=null)
                receipt=$"田畦 {garden.plots.Length} 块　·　成熟 {garden.plots.Count(x=>x.state=="ripe")} 块";
            Label(bar,"Notice",receipt??"",.005f,.04f,.72f,.98f,15,Cream);
            Key(Area(bar,"Refresh",.735f,.035f,.997f,.97f),"重读田簿",Refresh,false,true,15);
            var nav=Area(transform,"Navigation",0,.914f,1,1);Paint(nav,new Color32(13,31,28,255));
            Paint(Area(nav,"Inlay",0,0,1,.021f),Bronze);
            for(int i=0;i<Pages.Length;i++){
                string page=Pages[i];bool selected=page==tab;
                var cell=Area(nav,page,i/4f+.003f,.045f,(i+1)/4f-.003f,.96f);
                var p=Art(cell,SpiritGardenArt.Motif.Plate,selected?new Color32(96,90,55,255):new Color32(28,51,44,255),Pine,selected?Brass:new Color32(78,91,70,255));
                Bind(cell,p,()=>Go(page),!busy&&!pending&&(page=="访田"||page=="田畦"||garden!=null&&garden.own));
                Art(Area(cell,"Mark",.34f,.06f,.66f,.52f),Icons[i],selected?Brass:Sage,Clear,Clear);
                Label(cell,"Label",page=="种子"?"种坊":page,.04f,.47f,.96f,.98f,15,selected?Brass:Cream,TextAnchor.MiddleCenter,selected);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(body);
            scroll.content.anchoredPosition=new Vector2(0,Mathf.Clamp(previous,0,Mathf.Max(0,body.rect.height-pane.rect.height)));
        }
        private void MessageCard(string title,string detail,SpiritGardenArt.Motif symbol)
        {
            var r=Row("Record",67);Plate(r);
            Art(Area(r,"Seal",.025f,.18f,.12f,.82f),symbol,Brass,Clear,Clear);
            Label(r,"Title",title,.16f,.055f,.96f,string.IsNullOrEmpty(detail)?.94f:.53f,18,Cream);
            if(!string.IsNullOrEmpty(detail))Label(r,"Note",detail,.16f,.49f,.96f,.94f,14,Sage);
        }
        private void DrawVisitors()
        {
            Section("可访洞府",scope=="friends"?"道友名册":"山间修士");
            var row=Row("VisitScope",45);
            Key(Area(row,"Friends",0,0,.492f,1),"我的道友",()=>{scope="friends";Go("访田");},scope=="friends");
            Key(Area(row,"Strangers",.508f,0,1,1),"陌生修士",()=>{scope="strangers";Go("访田");},scope=="strangers");
            if((visitors?.neighbors?.Length??0)==0)
                MessageCard(busy?"寻访中":"暂未找到灵田",scope=="friends"?"可切换到陌生修士名册。":"有修士播种后，可在此寻访。",SpiritGardenArt.Motif.Gate);
            else foreach(var n in visitors.neighbors){
                var card=Row("Neighbor",97);Plate(card);
                Landscape(Area(card,"Door",.012f,.045f,.31f,.955f),n.friend?.65f:.28f);
                Art(Area(card,"Fade",.01f,.045f,.32f,.955f),SpiritGardenArt.Motif.ShadeBottom,new Color32(16,39,32,126),Clear,Clear);
                Label(card,"Grower",n.name,.35f,.08f,.77f,.54f,22,Cream,TextAnchor.MiddleLeft,true);
                Label(card,"Ripe",$"成熟 {n.ripe} 畦",.35f,.52f,.76f,.89f,15,Sage);
                Key(Area(card,"Visit",.775f,.22f,.967f,.80f),"入田",()=>Visit(n.id),true);
            }
            MessageCard("只可偷采成熟灵植","每茬至多一株。触阵也消耗一次机会。",SpiritGardenArt.Motif.Seed);
        }
        private void DrawLog()
        {
            Section("田簿来往",$"今日余 {garden.attemptsLeft} 次");
            var summary=Row("Guard",89);Plate(summary,true);
            Art(Area(summary,"Formation",.02f,.045f,.245f,.955f),SpiritGardenArt.Motif.Ward,Brass,Sage,Brass);
            Label(summary,"Title","护田阵与符力",.27f,.08f,.97f,.50f,21,Cream,TextAnchor.MiddleLeft,true);
            Label(summary,"Note",$"先触地阵，再耗护田符。符力尚余 {garden.wardCharges} 重。",.27f,.46f,.962f,.93f,15,Sage);
            foreach(var msg in garden.logs??Array.Empty<string>())MessageCard(msg,"",SpiritGardenArt.Motif.Gate);
            if((garden.logs?.Length??0)==0)MessageCard("尚无来访记录","偷采、挡阵和自己出访的经过，都会记入田簿。",SpiritGardenArt.Motif.Book);
            if(association!=null)ActionRow("Association","前往灵植协会旧籍",()=>{gameObject.SetActive(false);Destroy(gameObject);association();},true,false);
        }
        private void DrawSeeds()
        {
            Section("种坊藏种",$"{garden.seeds?.Length??0} 种灵植");
            foreach(var x in garden.seeds??Array.Empty<Seed>()){
                var r=Row("Seed_"+x.id,128);Plate(r);
                Herb(Area(r,"Botanical",.01f,.037f,.31f,.963f),x.id);
                Label(r,"Name",x.name,.345f,.04f,.71f,.34f,23,Cream,TextAnchor.MiddleLeft,true);
                Label(r,"Stock","存种 "+x.count,.745f,.06f,.97f,.31f,14,Brass,TextAnchor.MiddleRight);
                Label(r,"Use",x.use,.345f,.31f,.974f,.63f,15,Sage);
                Label(r,"Value",$"{Duration(x.seconds)}\n基础 {x.@yield} 株",.345f,.63f,.60f,.96f,14,Cream);
                Key(Area(r,"Buy_"+x.id,.62f,.645f,.962f,.94f),$"{x.price} 灵石购种",()=>{seedID=x.id;Write("buy");},true,garden.stones>=x.price,15);
            }
        }
        private void DrawPlots()
        {
            var all=garden.plots;
            if(all.Length==0){MessageCard("此处尚无灵田","",SpiritGardenArt.Motif.Beds);return;}
            var plot=Array.Find(all,p=>p.id==index)??all[0];index=plot.id;
            Section(garden.own?"我的梯田":garden.name+"的梯田",$"第 {index} / {all.Length} 畦");
            for(int i=0;i<all.Length;i+=4){
                var row=Row("Beds",71);
                for(int j=0;j<4&&i+j<all.Length;j++){
                    var p=all[i+j];bool selected=p.id==index;
                    var cell=Area(row,"Plot_"+p.id,j/4f+.003f,.015f,(j+1)/4f-.003f,.99f);
                    var plate=Art(cell,SpiritGardenArt.Motif.Plate,selected?new Color32(79,90,55,255):new Color32(45,64,45,255),Pine,selected?Brass:Bronze);
                    Bind(cell,plate,()=>{index=p.id;Draw();},!busy);
                    if(p.state=="empty")
                        Art(Area(cell,"Ridges",.12f,.055f,.88f,.64f),SpiritGardenArt.Motif.Furrows,new Color32(140,153,98,255),Clear,Clear);
                    else Herb(Area(cell,"Specimen",.085f,.07f,.915f,.65f),p.seed,p.ward);
                    string state=p.state=="empty"?"空":p.state=="ripe"?"熟":"青";
                    Label(cell,"Phase",$"{p.id}畦 · {state}",.045f,.63f,.955f,.945f,15,selected?Brass:Cream,TextAnchor.MiddleCenter,selected);
                }
            }
            if(plot.state=="empty"){
                if(garden.own) Planting(plot);
                else {MessageCard($"第 {plot.id} 畦尚未播种","可查看别的田畦。",SpiritGardenArt.Motif.Beds);ActionRow("Home","回我的灵田",Home);}
                return;
            }
            CropCard(plot);
            if(!garden.own){
                ActionRow("Steal",plot.ward?"冒险偷采一株":"偷采一株",()=>Write("steal",plot),plot.canSteal);
                ActionRow("Home","回我的灵田",Home,true,false);return;
            }
            if(plot.canWard&&!plot.ward)ActionRow("Ward",$"布草木阵  ·  {garden.wardCost} 灵石",()=>Write("ward",plot),garden.stones>=garden.wardCost);
            if(plot.state=="ripe"){
                ActionRow("Harvest","全部采收入纳戒",()=>Write("harvest",plot));
                if(plot.canKeepSeed)ActionRow("KeepSeed","拨两株留种，其余入纳戒",()=>Write("harvest",plot,true),true,false);
            }
        }
        private void CropCard(Plot p)
        {
            var card=Row("CropCard",164);Plate(card,true);
            Herb(Area(card,"Botanical",.011f,.035f,.395f,.963f),p.seed,p.ward);
            var variety=Array.Find(garden.seeds??Array.Empty<Seed>(),x=>x.id==p.seed);
            Label(card,"Bed",$"第 {p.id} 畦  ·  {SoilName(p.soil)}",.428f,.065f,.968f,.239f,15,Brass);
            Label(card,"Crop",p.name,.428f,.23f,.967f,.49f,27,Cream,TextAnchor.MiddleLeft,true);
            clockPlot=p;clockText=Label(card,"Maturity",p.state=="ripe"?"已成熟，可采收":Duration(Left(p))+" 后成熟",.428f,.505f,.968f,.685f,18,Sage);
            Label(card,"Use",variety?.use??$"本茬尚有 {p.@yield} 株",.428f,.685f,.962f,.948f,15,Cream);
            var line=Row("GuardStatus",39);
            Label(line,"Count",$"待收 {p.@yield} 株",.026f,0,.30f,1,17,Cream);
            Label(line,"Ward",p.ward?"草木阵守护中":p.wardUsed?"本茬阵力已用尽":"本茬未设阵",.30f,0,.97f,1,16,p.ward?Brass:Sage,TextAnchor.MiddleRight);
        }
        private void Planting(Plot p)
        {
            if((garden.seeds?.Length??0)==0||(garden.soils?.Length??0)==0)return;
            var seed=Array.Find(garden.seeds,x=>x.id==seedID)??garden.seeds[0];seedID=seed.id;
            var soil=Array.Find(garden.soils,x=>x.id==soilID)??garden.soils[0];soilID=soil.id;
            Section("选择灵种",$"第 {p.id} 畦 · 待播种");
            var row=Row("Seeds",77);
            for(int i=0;i<garden.seeds.Length;i++){
                var x=garden.seeds[i];float n=garden.seeds.Length;
                var cell=Area(row,x.id,i/n+.004f,0,(i+1)/n-.004f,1);
                var plate=Art(cell,SpiritGardenArt.Motif.Plate,x.id==seedID?Leaf:Pine,Pine,x.id==seedID?Brass:Bronze);
                Bind(cell,plate,()=>{seedID=x.id;Draw();},!busy);
                Herb(Area(cell,"Herb",.07f,.055f,.93f,.57f),x.id);
                Label(cell,"Label",$"{x.name} ×{x.count}",.015f,.57f,.985f,.96f,15,x.id==seedID?Brass:Cream,TextAnchor.MiddleCenter);
            }
            Section("铺设灵土",$"现用{SoilName(p.soil)}");
            var soils=Row("Soils",40);
            for(int i=0;i<garden.soils.Length;i++){
                var x=garden.soils[i];float n=garden.soils.Length;
                Key(Area(soils,x.id,i/n+.004f,0,(i+1)/n-.004f,1),x.name,()=>{soilID=x.id;Draw();},x.id==soilID);
            }
            bool match=seed.soil==soil.id;int cost=p.soil==soil.id?0:soil.cost;
            var detail=Row("Yield",44);
            Label(detail,"Use",seed.use,.024f,.0f,.982f,.53f,15,Sage);
            Label(detail,"Forecast",$"{(match?"适土":"寻常")}  ·  {Duration(match?seed.seconds*4/5:seed.seconds)}成熟  ·  收 {seed.@yield+(match?1:0)} 株",.024f,.49f,.982f,1,16,Cream);
            ActionRow("Plant",$"播种一粒  ·  换土 {cost} 灵石",()=>Write("plant",p),seed.count>0&&garden.stones>=cost);
            if(seed.count==0)ActionRow("SeedShop","前往灵植种坊",()=>Go("种子"),true,false);
        }
    }
}
