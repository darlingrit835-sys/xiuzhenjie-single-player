using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class SectCourt
    {
        private static readonly Color Ink = new Color32(39, 57, 49, 255);
        private static readonly Color Subtle = new Color32(78, 90, 75, 255);
        private static readonly Color Ivory = new Color32(255, 246, 221, 255);
        private static readonly Color Gold = new Color32(232, 200, 135, 255);
        private static readonly string[] Scenery = { "foothills", "bamboo", "river", "falls", "peaks", "observatory", "skybridge", "starpalace", "celestial" };
        private static readonly string[] Seals = { "松麓", "竹隐", "临川", "悬瀑", "群峰", "观星", "凌霄", "星阙", "天宫" };
        private static readonly Color32[] Pigments = {
            new Color32(55,84,66,255), new Color32(49,91,72,255), new Color32(48,88,91,255),
            new Color32(55,86,101,255), new Color32(67,79,114,255), new Color32(76,69,106,255),
            new Color32(103,66,68,255), new Color32(102,74,91,255), new Color32(119,77,44,255) };
        private static readonly string[] DockNames = { "山门", "兽潮", "矿脉", "藏宝阁" };
        private static readonly string[] DockIcons = { "gate", "tide", "mine", "treasury" };
        private RectTransform body, pageRoot, hero, replayAnchor;
        private SectCourtLight illumination;
        private Text countdown, replayCaption;
        private ScrollRect scroll;
        private float cursor;
        private Font font;
        private int Grade => Mathf.Clamp(data != null ? data.stage : entryGrade, 1, 9);
        private Color Accent => Pigments[Grade - 1];
        private string GradeText => "一二三四五六七八九"[Grade - 1] + "阶";
        private string TierText => !string.IsNullOrEmpty(data?.tierName) ? data.tierName : Grade <= 3 ? "山门" : Grade <= 6 ? "仙府" : "圣地";

        private static RectTransform Node(Transform parent, string name)
        {
            var child = new GameObject(name, typeof(RectTransform));
            child.layer = parent.gameObject.layer;
            child.transform.SetParent(parent, false);
            return (RectTransform)child.transform;
        }

        private static void Box(RectTransform rect, float x1, float y1, float x2, float y2,
            float left = 0, float bottom = 0, float right = 0, float top = 0)
        {
            rect.anchorMin = new Vector2(x1,y1); rect.anchorMax = new Vector2(x2,y2);
            rect.offsetMin = new Vector2(left,bottom); rect.offsetMax = new Vector2(-right,-top);
        }

        private static void Top(RectTransform rect, float y, float height, float left = 0, float right = 0)
        {
            Box(rect, 0, 1, 1, 1, left, -y-height, right, y);
        }

        private static Image Paint(RectTransform rect, Color color, bool target = false)
        {
            var image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = target;
            return image;
        }

        private Text TextAt(RectTransform parent, string caption, int size, Color ink,
            TextAnchor align = TextAnchor.MiddleLeft, bool bold = false)
        {
            var label = Node(parent, "Text").gameObject.AddComponent<Text>();
            label.font = font; label.text = caption; label.fontSize = size; label.color = ink;
            label.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            label.alignment = align; label.raycastTarget = false;
            label.supportRichText = false;
            label.resizeTextForBestFit = true; label.resizeTextMinSize = Mathf.Max(10, size - 3); label.resizeTextMaxSize = size;
            Box(label.rectTransform, 0, 0, 1, 1);
            return label;
        }

        private Image Emblem(RectTransform parent, string key)
        {
            var obj = Node(parent, key);
            var image = Paint(obj, Color.white);
            image.sprite = ReleasedResources.Load<Sprite>("SectCourt/icons/" + key);
            image.preserveAspect = true;
            return image;
        }

        // Layout margins stay in logical canvas units. Wide labels reserve their own
        // column so the icon and the descriptive line remain distinct on small phones.
        private Button ButtonFace(RectTransform rect, string label, string icon, Action action,
            bool filled = false, bool enabled = true)
        {
            var image = Paint(rect, filled ? Accent : new Color32(237, 230, 207, 255), true);
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.interactable = enabled;
            var colors = button.colors;
            colors.pressedColor = new Color32(194, 175, 134, 255);
            colors.disabledColor = new Color32(204, 205, 190, 255);
            button.colors = colors;
            if (action != null) button.onClick.AddListener(() => action());
            var emblem = Emblem(rect, icon);
            Box(emblem.rectTransform, 0, 0, 0, 1, 5, 4, -32, 4);
            var caption = TextAt(rect, label, 15, enabled ? (filled ? Ivory : Ink) : Subtle, TextAnchor.MiddleCenter, true);
            Box(caption.rectTransform, 0, 0, 1, 1, 32, 1, 4, 1);
            return button;
        }

        private RectTransform Line(string name, float height, float margin = 10, float gap = 5)
        {
            var rect = Node(body, name);
            Top(rect, cursor, height, margin, margin);
            cursor += height + gap;
            return rect;
        }

        private void Title(string title, string icon)
        {
            var row = Line(title, 32, 12, 1);
            var image = Emblem(row, icon);
            Box(image.rectTransform, 0, 0, 0, 1, 0, 5, -25, 5);
            var text = TextAt(row, title, 17, Accent, TextAnchor.MiddleLeft, true);
            Box(text.rectTransform,0,0,1,1,32,0,0,0);
        }

        private void Note(string message, int height = 40)
        {
            var row = Line("Note", height, 15, 4);
            TextAt(row, message, 14, Subtle);
        }

        private RectTransform Tile(string name, string detail, string icon, Action select = null,
            string tag = null, bool primary = false)
        {
            var rect = Line(name, 56);
            Paint(rect, primary ? new Color32(225, 231, 210, 255) : new Color32(245, 238, 217, 255), select != null);
            if (select != null)
            {
                var button = rect.gameObject.AddComponent<Button>();
                button.targetGraphic = rect.GetComponent<Image>();
                button.onClick.AddListener(() => select());
            }
            var rim = Node(rect, "Rule"); Paint(rim, primary ? Accent : new Color32(203, 185, 143, 255));
            Box(rim,0,0,0,1,0,0,-2,0);
            var emblem = Emblem(rect,icon); Box(emblem.rectTransform,0,0,0,1,8,6,-48,6);
            int right = string.IsNullOrEmpty(tag) ? 8 : 70;
            var heading = TextAt(rect,name,17,Ink,TextAnchor.MiddleLeft,true);
            Box(heading.rectTransform,0,.44f,1,1,57,0,right,3);
            var caption = TextAt(rect,detail,13,Subtle);
            Box(caption.rectTransform,0,0,1,.48f,57,3,8,0);
            if (!string.IsNullOrEmpty(tag))
            {
                var value = TextAt(rect,tag,13,Accent,TextAnchor.MiddleRight,true);
                Box(value.rectTransform,1,.44f,1,1,-69,0,9,3);
            }
            return rect;
        }

        private void ActionRow(string name, string icon, Action click, bool enabled = true)
        {
            var rect = Line(name, 42);
            ButtonFace(rect, name, icon, click, enabled, enabled);
        }

        private void ChoiceRow(string[] names, string[] icons, int selected, Action<int> choose)
        {
            var rect = Line("Choices", 37);
            for (int i = 0; i < names.Length; ++i)
            {
                int index = i;
                var tab = Node(rect, names[i]);
                Box(tab,(float)i/names.Length,0,(float)(i+1)/names.Length,1,i == 0 ? 0 : 2,0,i == names.Length-1 ? 0 : 2,0);
                ButtonFace(tab,names[i],icons[i],() => choose(index),selected == i);
            }
        }

        private void Meter(long value, long target)
        {
            var rail = Line("Progress",7,13,9);
            Paint(rail,new Color32(205,198,173,255));
            var fill = Node(rail,"Fill");
            Box(fill,0,0,target > 0 ? Mathf.Clamp01((float)value/target) : 0,1);
            Paint(fill,Accent);
        }

        private void Hero(string imageKey, string title, string subtitle, string stamp, float height)
        {
            hero = Line("Hero", height, 0, 5);
            hero.gameObject.AddComponent<RectMask2D>();
            var plate = Node(hero, "Painting"); Box(plate,0,0,1,1);
            var image = Paint(plate, Color.white);
            image.sprite = ReleasedResources.Load<Sprite>("SectCourt/" + imageKey) ?? InkInterfaceArt.Get("GuildHall");
            if (image.sprite != null)
            {
                plate.anchorMin = plate.anchorMax = new Vector2(.5f,.5f);
                var fit = plate.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = image.sprite.rect.width / image.sprite.rect.height;
            }
            illumination = SectCourtLight.Mount(hero, imageKey, Grade, Accent);
            var shade = Node(hero,"Mist"); Box(shade,0,0,1,.70f);
            var gradient = shade.gameObject.AddComponent<SectCourtShade>(); gradient.raycastTarget = false;
            var tablet = Node(hero, "GradeSeal"); Box(tablet,1,1,1,1,-69,-38,8,8);
            Paint(tablet, Accent);
            TextAt(tablet,stamp,13,Ivory,TextAnchor.MiddleCenter,true);
            var heading = TextAt(hero,title,29,Ivory,TextAnchor.LowerLeft,true);
            Box(heading.rectTransform,0,0,1,0,15,31,12,-75);
            var foot = TextAt(hero,subtitle,14,Gold);
            Box(foot.rectTransform,0,0,1,0,16,7,12,-30);
            var ruling = Node(hero,"Ruling"); Box(ruling,0,0,1,0,0,0,0,-2); Paint(ruling,Accent);
        }

        internal void OpenPeakMap(ZongMenPanel panel)
        {
            if (data != null)
            {
                var scenery = ReleasedResources.Load<Sprite>("SectCourt/" + Scenery[Grade-1]);
                var background = panel.GetComponent<Image>();
                if (scenery != null) { background.sprite = scenery; background.color = Color.white; }
                panel.ZhuFengStageText.text = GradeText + TierText;
                panel.ShouChaoText.text = WaveName(data.waveKind);
            }
            string[] occupations = { "LingZhi","LianQi","QiaoJiang","BuDao","LianDan","LianQi","ZhiFu","ZhenTu","YuShou" };
            for (int i=0; i<panel.ShanFengList.Count && i<occupations.Length;i++)
            {
                var peak=panel.ShanFengList[i];
                Badge(peak.SelfBtn,InkInterfaceArt.Get("Professions/"+occupations[i]),peak.NameText,peak.StageText);
            }
            Badge(panel.KuangMaiBtn,ReleasedResources.Load<Sprite>("SectCourt/icons/mine"),panel.KuangMaiBtn.transform.Find("Text").GetComponent<Text>());
            Badge(panel.HuZongDaZhenBtn,ReleasedResources.Load<Sprite>("SectCourt/icons/formation"),panel.HuZongDaZhenBtn.transform.Find("Text").GetComponent<Text>());
            gameObject.SetActive(false);
        }

        internal static void Badge(Button button, Sprite art, params Text[] labels)
        {
            if (art==null || button==null || button.transform.Find("SectBadge")!=null) return;
            var rect=Node(button.transform,"SectBadge"); Box(rect,0,0,0,1,4,5,-27,5);
            var image=Paint(rect,Color.white); image.sprite=art; image.preserveAspect=true;
            foreach (var label in labels)
            {
                if (label==null || label.transform.parent!=button.transform) continue;
                var current=label.rectTransform.offsetMin;
                label.rectTransform.offsetMin=new Vector2(Mathf.Max(current.x,29),current.y);
            }
        }

        private void Shell()
        {
            // Keep only one active tree, including while a reply arrives in this frame.
            if (pageRoot != null) { pageRoot.gameObject.SetActive(false); Destroy(pageRoot.gameObject); }
            pageRoot = Node(transform, "Folio"); Box(pageRoot,0,0,1,1);
            var paper = Paint(pageRoot,new Color32(252,247,231,255),true);
            InkInterfaceArt.Set(paper,"InkLedgerPaper",false); paper.raycastTarget = true;
            var header = Node(pageRoot,"Masthead"); Top(header,0,42);
            Paint(header,new Color32(239,232,210,255));
            var back = Node(header,"Back"); Box(back,0,0,0,1,3,2,-49,2);
            var backBg = Paint(back,Color.clear,true);
            var button = back.gameObject.AddComponent<Button>(); button.targetGraphic = backBg;
            button.onClick.AddListener(Back);
            var arrow = Node(back,"Arrow"); Box(arrow,0,0,1,1,8,7,8,7);
            var arrowImage = Paint(arrow,Color.white); InkInterfaceArt.Set(arrowImage,"Back");
            var current = TextAt(header,PageName(),19,Ink,TextAnchor.MiddleCenter,true);
            Box(current.rectTransform,0,0,1,1,53,0,71,0);
            var reload = Node(header,"Refresh"); Box(reload,1,0,1,1,-72,4,4,4);
            ButtonFace(reload,"刷新","order",() => Fetch(),false,!busy);
            var viewport = Node(pageRoot,"Pages"); Box(viewport,0,0,1,1,0,57,0,42);
            Paint(viewport,Color.clear,true);
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false; scroll.viewport = viewport; scroll.movementType = ScrollRect.MovementType.Clamped;
            body = Node(viewport,"Contents"); body.anchorMin = new Vector2(0,1); body.anchorMax = Vector2.one;
            body.pivot = new Vector2(.5f,1); body.anchoredPosition = Vector2.zero; body.sizeDelta = Vector2.zero;
            scroll.content = body; cursor = 0;
            var dock = Node(pageRoot,"Dock"); Box(dock,0,0,1,0,0,0,0,-57);
            Paint(dock,new Color32(233,225,202,255),true);
            int active = page == Page.Wave || page == Page.Replay ? 1 : page == Page.Mine ? 2 : page == Page.Treasure ? 3 : 0;
            for (int i=0; i<4; i++)
            {
                int index = i;
                var item = Node(dock,DockNames[i]); Box(item,i/4f,0,(i+1)/4f,1,1,1,1,1);
                var face = Paint(item,i == active ? Accent : new Color32(242,235,213,255),true);
                var tab = item.gameObject.AddComponent<Button>(); tab.targetGraphic = face;
                tab.onClick.AddListener(() => Show((Page)index));
                var icon = Emblem(item,DockIcons[i]);
                Box(icon.rectTransform,0,.37f,1,1,0,0,0,2);
                var title = TextAt(item,DockNames[i],13,i == active ? Ivory : Ink,TextAnchor.MiddleCenter,true);
                Box(title.rectTransform,0,0,1,.4f);
            }
        }
    }

    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class SectCourtShade : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            var rect = GetPixelAdjustedRect();
            var low = new Color32(18,32,37,238); var clear = new Color32(18,32,37,0);
            mesh.AddVert(new Vector2(rect.xMin,rect.yMin),low,Vector2.zero);
            mesh.AddVert(new Vector2(rect.xMin,rect.yMax),clear,Vector2.up);
            mesh.AddVert(new Vector2(rect.xMax,rect.yMax),clear,Vector2.one);
            mesh.AddVert(new Vector2(rect.xMax,rect.yMin),low,Vector2.right);
            mesh.AddTriangle(0,1,2); mesh.AddTriangle(2,3,0);
        }
    }
}
