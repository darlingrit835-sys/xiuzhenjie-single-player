using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class VoidRealmView : MonoBehaviour
    {
        private static readonly Color Ink = new Color32(39,61,47,255), Muted = new Color32(89,100,78,255),
            Jade = new Color32(38,76,65,255), Gold = new Color32(209,180,120,255), Ivory = new Color32(255,241,205,255),
            LineInk = new Color32(191,177,139,255);
        private static readonly string[] DockTitles = {"大厅","留言板","排行榜"};
        private Font font;
        private Action leave, temple, tower, altar, concord;
        private Action<string> legacy;
        private RectTransform pageRoot, body, viewport, message, feature;
        private ScrollRect scroll;
        private VoidRealmLight lightFx;
        private VoidRealmClient.Reply lobby;
        private int epoch, tab;
        private bool busy, restore, resumeNeeded;
        private float cursor, offset, messageUntil;
        private string pageTitle, location;

        internal static VoidRealmView Open(Transform parent, Font font, Action close, Action temple, Action tower,
            Action altar, Action concord, Action<string> legacyHall)
        {
            var existing = parent.Find("VoidRealmView");
            if (existing != null) return existing.GetComponent<VoidRealmView>();
            var root = Node(parent, "VoidRealmView");
            Box(root,0,0,1,1);
            Paint(root,Ivory,true);
            var view = root.gameObject.AddComponent<VoidRealmView>();
            view.font = font ?? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            view.leave = close; view.temple = temple; view.tower = tower; view.altar = altar;
            view.concord = concord; view.legacy = legacyHall;
            view.ShowHalls();
            return view;
        }

        private static RectTransform Node(Transform parent, string name)
        {
            var obj = new GameObject(name,typeof(RectTransform));
            obj.layer = parent.gameObject.layer; obj.transform.SetParent(parent,false);
            return (RectTransform)obj.transform;
        }

        private static void Box(RectTransform r,float x1,float y1,float x2,float y2,
            float l=0,float b=0,float right=0,float top=0)
        {
            r.anchorMin = new Vector2(x1,y1); r.anchorMax = new Vector2(x2,y2);
            r.offsetMin = new Vector2(l,b); r.offsetMax = new Vector2(-right,-top);
        }

        private static Image Paint(RectTransform r,Color tint,bool hit=false)
        {
            var image = r.gameObject.AddComponent<Image>(); image.color = tint; image.raycastTarget = hit; return image;
        }

        private Text Caption(RectTransform parent,string value,int size,Color color,TextAnchor align=TextAnchor.MiddleLeft,bool bold=false)
        {
            var rect = Node(parent,"题字");
            Box(rect,0,0,1,1);
            var t = rect.gameObject.AddComponent<Text>(); t.text = value ?? ""; t.font = font; t.fontSize = size;
            t.color = color; t.supportRichText = false; t.alignment = align; t.raycastTarget = false;
            if (bold) t.fontStyle = FontStyle.Bold;
            return t;
        }

        private Button Face(RectTransform rect,string text,Action action,bool primary)
        {
            var img = Paint(rect,primary ? Jade : new Color32(237,230,208,255),true);
            var rim = Node(rect,"描金"); Box(rim,0,0,1,0,1,0,1,-1); Paint(rim,Gold);
            var label = Caption(rect,text,14,primary ? Ivory : Ink,TextAnchor.MiddleCenter,true);
            Box(label.rectTransform,0,0,1,1,3,1,3,1);
            var button = rect.gameObject.AddComponent<Button>(); button.targetGraphic = img; button.interactable = action != null;
            if (action != null) button.onClick.AddListener(() => action());
            return button;
        }

        private void BeginPage(string title,string subtitle,bool studio=false,int grade=1,string ownerHall=null,VoidRealmClient.Work signed=null)
        {
            bool same = pageTitle == title && location == ownerHall && scroll != null;
            offset = same ? Mathf.Max(0,body.anchoredPosition.y) : 0;
            restore = same;
            if (pageRoot != null) { pageRoot.gameObject.SetActive(false); Destroy(pageRoot.gameObject); }
            pageTitle = title; location = ownerHall; busy = false; ++epoch; cursor = 0; message = null;
            pageRoot = Node(transform,"虚神界 · "+title); Box(pageRoot,0,0,1,1);
            var sheet = Node(pageRoot,"绢本"); Box(sheet,0,0,1,1);
            Paint(sheet,Color.white,true).sprite = InkInterfaceArt.Get("Paper");
            var hero = Node(pageRoot,"虚境山水"); Box(hero,0,.62f,1,1); hero.gameObject.AddComponent<RectMask2D>();
            var painting = Node(hero,"场景"); Box(painting,0,0,1,1);
            var art = Paint(painting,Color.white);
            art.sprite = ReleasedResources.Load<Sprite>("VoidRealm/"+(studio ? "studio" : "hall"));
            if (art.sprite != null)
            {
                painting.pivot = new Vector2(.5f,studio ? .42f : .68f);
                var fit = painting.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = art.sprite.rect.width/art.sprite.rect.height;
            }
            lightFx = VoidRealmLight.Mount(hero,painting,grade,studio);
            bool workInProgress = signed == null && creation != null && ownerHall == creation.id;
            if (studio && (workInProgress || signed?.version == 2))
            {
                var vessel = Node(hero,"案上法相");
                Box(vessel,.33f,.20f,.84f,.88f);
                if (signed != null)
                    VoidRealmRelic.Mount(vessel,ownerHall,grade,signed.route,2,true,signed.steps[3]);
                else
                {
                    int completed = process == null ? 0 : process.Count(s => s >= 0);
                    VoidRealmRelic.Mount(vessel,ownerHall,grade,craftRoute,completed-1,craftResult != null && craftResult.success,mainQi);
                }
            }
            var gradient = Node(hero,"墨色底衬"); Box(gradient,0,0,1,.66f);
            gradient.gameObject.AddComponent<SectCourtShade>().raycastTarget = false;
            var gallery = studio ? VoidRealmCatalog.Find(ownerHall) : null;
            var inscription = gallery?.hall ?? (tab==1 ? "留音壁" : tab==2 ? "百工金榜" : "虚神界");
            var titleText = Caption(hero,string.Join("\n",inscription.Select(c=>c.ToString())),36,Ivory,TextAnchor.MiddleCenter,true);
            Box(titleText.rectTransform,0,.08f,0,.79f,22,0,-70,0);
            titleText.lineSpacing=.78f; titleText.resizeTextForBestFit=true;
            titleText.resizeTextMinSize=12; titleText.resizeTextMaxSize=36;
            var shadow=titleText.gameObject.AddComponent<Shadow>(); shadow.effectColor=new Color(0,.09f,.05f,.87f);
            shadow.effectDistance=new Vector2(1.3f,-1.3f);
            string seal=gallery?.work ?? (tab==1 ? "诸馆留言" : tab==2 ? "作品题名" : "虚神殿");
            var stamp=Node(hero,"朱砂辅签"); Box(stamp,0,.47f,0,.78f,9,0,-24,0);
            Paint(stamp,new Color32(134,58,43,235));
            var redLetter=Caption(stamp,string.Join("\n",seal.Select(c=>c.ToString())),11,Ivory,TextAnchor.MiddleCenter);
            redLetter.lineSpacing=.77f;redLetter.resizeTextForBestFit=true;redLetter.resizeTextMinSize=8;redLetter.resizeTextMaxSize=11;
            feature=null;
            Feature(hero,title,subtitle,null,null);
            var frame=Node(hero,"金丝绢边"); Box(frame,0,0,1,1,5,5,5,5);
            FrameRule(frame,true,false);FrameRule(frame,true,true);FrameRule(frame,false,false);FrameRule(frame,false,true);
            var back = Node(hero,"返城"); Box(back,0,1,0,1,8,-34,-63,6); Face(back,"返城",leave,true);
            var signature = Node(hero,"署名玉牌"); Box(signature,1,1,1,1,-117,-32,8,6); Paint(signature,new Color32(34,62,50,225));
            Caption(signature,lobby == null ? "虚神殿" : lobby.name + " · " + RealmName(lobby.roleLevel),12,Ivory,TextAnchor.MiddleCenter);
            var edge = Node(hero,"金线"); Box(edge,0,0,1,0,0,0,0,-2); Paint(edge,Gold);
            viewport = Node(pageRoot,"名录"); Box(viewport,0,0,1,.62f,0,0,0,41);
            Paint(viewport,Color.clear,true);
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll = viewport.gameObject.AddComponent<ScrollRect>(); scroll.viewport = viewport; scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            body = Node(viewport,"名录正文"); body.anchorMin = new Vector2(0,1); body.anchorMax=Vector2.one;
            body.pivot = new Vector2(.5f,1); body.sizeDelta = Vector2.zero; body.anchoredPosition = Vector2.zero;
            scroll.content = body;
            var dock = Node(pageRoot,"分栏"); Box(dock,0,.62f,1,.62f,0,-40,0,0);
            Paint(dock,new Color32(239,231,207,255),true);
            string[] glyphs={"殿","言","榜"};
            for (int i=0;i<DockTitles.Length;i++)
            {
                int selected=i;
                var rect=Node(dock,DockTitles[i]); Box(rect,i/3f,0,(i+1)/3f,1,0,0,0,0);
                var hit=Paint(rect,tab==i?new Color32(218,209,180,130):Color.clear,true);
                var button=rect.gameObject.AddComponent<Button>();button.targetGraphic=hit;button.onClick.AddListener(()=>GoTab(selected));
                var sealBox=Node(rect,"栏印");Box(sealBox,.5f,.5f,.5f,.5f,-32,-8,16,-8);
                Paint(sealBox,tab==i?Jade:new Color32(194,196,166,255));
                Caption(sealBox,glyphs[i],11,tab==i?Ivory:Ink,TextAnchor.MiddleCenter);
                var name=Caption(rect,DockTitles[i],14,Ink,TextAnchor.MiddleCenter,tab==i);Box(name.rectTransform,0,0,1,1,14,0,0,0);
                if(tab==i) { var line=Node(rect,"金线");Box(line,.15f,0,.85f,0,0,0,0,-2);Paint(line,new Color32(151,116,54,255)); }
            }
        }

        private static void FrameRule(RectTransform parent,bool horizontal,bool far)
        {
            var line=Node(parent,"金线");
            if(horizontal) Box(line,0,far?1:0,1,far?1:0,0,far?-1:0,0,far?0:-1);
            else Box(line,far?1:0,0,far?1:0,1,far?-1:0,0,far?0:-1,0);
            Paint(line,new Color32(230,206,156,135));
        }

        private void Feature(RectTransform hero,string title,string subtitle,string actionText,Action action)
        {
            if(feature!=null){feature.gameObject.SetActive(false);Destroy(feature.gameObject);}
            feature=Node(hero,"揭题");Box(feature,.29f,0,1,0,0,7,11,-91);
            float bottom=action==null?0:24;
            var sub=Caption(feature,subtitle,12,Gold,TextAnchor.MiddleRight,true);
            Box(sub.rectTransform,0,0,1,0,0,bottom+26,0,-bottom-46);
            var heading=Caption(feature,title,21,Ivory,TextAnchor.MiddleRight,true);
            Box(heading.rectTransform,0,0,1,0,0,bottom,0,-bottom-30);
            heading.gameObject.AddComponent<Shadow>().effectColor=new Color(0,.10f,.08f,.85f);
            if(action==null) return;
            var open=Node(feature,actionText);Box(open,1,0,1,0,-100,0,0,-23);
            Face(open,actionText,action,true);
        }

        private void Feature(string title,string header,string button,Action action)
        {
            Feature((RectTransform)feature.parent,title,header,button,action);
        }

        private void OnDisable()
        {
            resumeNeeded=busy;busy=false;++epoch;
        }
        private void OnEnable()
        {
            if(!resumeNeeded)return;
            resumeNeeded=false;ShowHalls();
        }

        private static string RealmName(int level) => WorldSetting.GetRoleLevelNameByRoleLevel(level);

        private void GoTab(int index)
        {
            if (index == 0 || lobby == null) ShowHalls();
            else if (index == 1) ShowBoard();
            else ShowRanks();
        }

        private void LateUpdate()
        {
            if (restore && viewport != null)
            {
                restore = false;
                body.anchoredPosition = new Vector2(0,Mathf.Clamp(offset,0,Mathf.Max(0,body.rect.height-viewport.rect.height)));
            }
            if (message != null && messageUntil > 0 && Time.unscaledTime >= messageUntil) ClearNotice();
        }

        private RectTransform Band(string id,float height,float left=12,float right=12,float gap=4)
        {
            var row = Node(body,id); Box(row,0,1,1,1,left,-cursor-height,right,cursor);
            cursor += height+gap; body.sizeDelta = new Vector2(0,cursor+6);
            return row;
        }

        private void Heading(string text) { Caption(Band("篇题",35),text,23,Ink,TextAnchor.MiddleLeft,true); }
        private void Section(string heading)
        {
            var row = Band("章节",29); var rule = Node(row,"金线"); Box(rule,0,0,1,0,0,0,0,-1);
            Paint(rule,LineInk); Caption(row,heading,16,Ink,TextAnchor.MiddleLeft,true);
        }

        private void Copy(string text,int size=14)
        {
            if (string.IsNullOrEmpty(text)) return;
            var row = Band("正文",1); var label = Caption(row,text,size,Muted);
            float width = Mathf.Max(100,((RectTransform)transform).rect.width - 26);
            var settings = label.GetGenerationSettings(new Vector2(width,0));
            float h = Mathf.Max(size*1.5f,Mathf.Ceil(label.cachedTextGeneratorForLayout.GetPreferredHeight(text,settings)/label.pixelsPerUnit)+4);
            row.offsetMin -= Vector2.up*(h-1); cursor += h-1; body.sizeDelta = new Vector2(0,cursor+6);
        }

        private void ActionRow(string text,Action action,bool primary=false) { Face(Band(text,37),text,action,primary); }

        private InputField Field(string placeholder,int limit,bool multiline=false)
        {
            var rect = Band("录入",multiline ? 75 : 39); var image = Paint(rect,new Color32(249,244,224,255),true);
            var bar = Node(rect,"墨线"); Box(bar,0,0,1,0,0,0,0,-1); Paint(bar,LineInk);
            var label = Caption(rect,"",14,Ink,multiline ? TextAnchor.UpperLeft : TextAnchor.MiddleLeft);
            var hint = Caption(rect,placeholder,14,Muted,label.alignment);
            Box(label.rectTransform,0,0,1,1,8,4,8,4); Box(hint.rectTransform,0,0,1,1,8,4,8,4);
            var input = rect.gameObject.AddComponent<InputField>(); input.targetGraphic = image; input.textComponent=label;
            input.placeholder=hint; input.characterLimit=limit;
            input.lineType=multiline ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine;
            return input;
        }

        private void Row(string title,string detail,string mark,Action action) { LedgerRow(title,detail,mark,action,null,Jade); }

        private void LedgerRow(string title,string detail,string mark,Action action,Sprite icon,Color accent)
        {
            var row = Band(title,56,8,8,0); var surface = Paint(row,new Color(1,.99f,.91f,.24f),true);
            if (action!=null) { var btn=row.gameObject.AddComponent<Button>(); btn.targetGraphic=surface; btn.onClick.AddListener(() => action()); }
            var line=Node(row,"界线"); Box(line,0,0,1,0,0,0,0,-1); Paint(line,LineInk);
            float left=6;
            if (icon != null)
            {
                var seal=Node(row,"职业印"); Box(seal,0,.5f,0,.5f,4,-20,-44,-20);
                var image=Paint(seal,Color.white); image.sprite=icon; image.preserveAspect=true; left=50;
            }
            float right=string.IsNullOrEmpty(mark) ? 8 : 76;
            var name=Caption(row,title,17,Ink,TextAnchor.MiddleLeft,true); Box(name.rectTransform,0,.44f,1,1,left,0,right,1);
            var note=Caption(row,detail,12,Muted); Box(note.rectTransform,0,0,1,.48f,left,2,right,0);
            if (!string.IsNullOrEmpty(mark))
            {
                var badge=Node(row,"印记"); Box(badge,1,.5f,1,.5f,-73,-14,4,-14); Paint(badge,accent);
                Caption(badge,mark,12,Ivory,TextAnchor.MiddleCenter,true);
            }
        }

        private void Choices(string[] labels,int selected,Action<int> choose)
        {
            var row=Band("筛选",33);
            for (int i=0;i<labels.Length;i++)
            {
                int n=i; var btn=Node(row,labels[i]);
                Box(btn,i/(float)labels.Length,0,(i+1)/(float)labels.Length,1,1,0,1,0);
                Face(btn,labels[i],() => choose(n),i==selected);
            }
        }

        private void Burst(bool success,int grade) { if(lightFx!=null) lightFx.Burst(success,grade); }
        private void Notice(string text) { ShowNotice(text,null); }
        private void ClearNotice()
        {
            if (message != null) { message.gameObject.SetActive(false); Destroy(message.gameObject); message=null; }
        }
        private void ShowNotice(string text,Action retry)
        {
            ClearNotice(); message=Node(pageRoot,"回音"); Box(message,0,0,1,0,5,5,5,-58);
            Paint(message,new Color32(37,65,54,247),true);
            var copy=Caption(message,text,13,Ivory); Box(copy.rectTransform,0,0,1,1,7,2,retry==null?7:64,2);
            if (retry!=null)
            {
                var b=Node(message,"重试"); Box(b,1,0,1,1,-59,5,5,5);
                Face(b,"重试",retry,false);
            }
            messageUntil=retry==null ? Time.unscaledTime+3.5f : 0;
        }

        private void Request(VoidRealmClient.Order order,Action<VoidRealmClient.Reply> onReply)
        {
            if (busy) return;
            busy=true; int version=epoch; ShowNotice("正在传讯…",null);
            VoidRealmClient.Send(order,(reply,error) =>
            {
                if (this==null || !isActiveAndEnabled || version!=epoch) return;
                busy=false; ClearNotice();
                if (reply == null)
                {
                    ShowNotice(string.IsNullOrEmpty(error) ? "未收到虚神界回音。" : error,() => Request(order,onReply));
                    return;
                }
                onReply(reply);
            });
        }

        private void LegacyHall(string hallId) { legacy?.Invoke(hallId); }
    }
}
