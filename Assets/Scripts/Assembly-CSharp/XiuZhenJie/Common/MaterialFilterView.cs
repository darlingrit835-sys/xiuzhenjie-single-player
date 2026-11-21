using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // The shipped bundle owns the serialized event targets. Arrange those same controls on the folio.
    internal sealed class MaterialFilterView
    {
        private static readonly Color Ink = new Color32(45, 62, 49, 255);
        private static readonly Color QuietInk = new Color32(64, 77, 61, 255);
        private static readonly Color Jade = new Color32(54, 88, 69, 255);
        private static readonly Color PaleJade = new Color32(218, 228, 211, 255);
        private static readonly Color Paper = new Color32(250, 246, 232, 255);
        private static readonly Color Chip = new Color32(240, 235, 218, 255);
        private static readonly Color Rule = new Color32(206, 201, 180, 255);
        private static readonly string[] GroupNames = { "五行", "四气", "五味", "补泻", "归经" };
        private static readonly int[] PreferredColumns = { 6, 5, 6, 3, 5 };
        private static readonly Color[] ElementColors =
        {
            new Color32(152, 127, 64, 255), new Color32(100, 132, 95, 255),
            new Color32(90, 130, 144, 255), new Color32(164, 100, 76, 255),
            new Color32(144, 115, 81, 255)
        };

        private readonly CaiLiaoShaiXuanPanel panel;
        private readonly Font font;
        private readonly RectTransform[] groups;
        private readonly Text[] headings = new Text[5];
        private readonly ScrollRect attributes;
        private readonly Text chosen;
        private readonly RectTransform areaTabs;
        private readonly Text fixedPlace;
        private float layoutWidth;

        internal MaterialFilterView(CaiLiaoShaiXuanPanel panel)
        {
            this.panel = panel;
            font = panel.DaLuText.font;
            var root = (RectTransform)panel.transform;
            var sheet = (RectTransform)root.Find("Bg");
            var footer = (RectTransform)root.Find("Image");
            var tabs = (RectTransform)panel.ZhiWu.transform.parent;
            var location = (RectTransform)panel.XuanZeGameObject.transform;
            attributes = panel.ShuXingGameObject.GetComponent<ScrollRect>();
            groups = new[] { panel.WuXingRectTransform, panel.SiQiRectTransform, panel.WuWeiRectTransform,
                panel.BuXieRectTransform, panel.GuiJingRectTransform };

            InkDialogArt.SetWash(panel.BackGroundBtn.transform, new Color32(38, 55, 46, 174));
            sheet.SetAsLastSibling();
            InkDialogArt.Inset(sheet, 8, 8, 30, 12);
            Flat(sheet.GetComponent<Image>(), Paper, true);

            var botanical = ImageNode("本草题图", sheet, Color.white);
            InkInterfaceArt.Set(botanical, "HerbariumTitle", false);
            Band(botanical.rectTransform, 0, 0, 0, 94);
            var imprint = Label("题签", sheet, "地火阁  ·  炼丹用材", 12, QuietInk);
            Band(imprint.rectTransform, 16, 100, 14, 19);
            var title = Label("题名", sheet, "寻材图录", 28, Ink, true);
            Band(title.rectTransform, 15, 82, 34, 42);
            MakeClose(sheet);

            tabs.SetParent(sheet, false);
            Band(tabs, 14, 14, 96, 33);
            SetupCategory(panel.ZhiWu, "草木", 0);
            SetupCategory(panel.YaoShou, "妖兽", 1);
            SetupCategory(panel.KuangWu, "自创灵植", 2);
            Band(ImageNode("页签横线", sheet, Rule).rectTransform, 14, 14, 129, 1);

            attributes.transform.SetParent(sheet, false);
            InkDialogArt.Inset((RectTransform)attributes.transform, 14, 14, 202, 80);
            PrepareScroll(attributes);
            var content = panel.ZongGanDu;
            if (content.TryGetComponent(out VerticalLayoutGroup oldLayout))
                oldLayout.enabled = false;
            if (content.TryGetComponent(out ContentSizeFitter oldFitter))
                oldFitter.enabled = false;
            content.pivot = new Vector2(.5f, 1);
            string[] headingObjects = { "WuXing", "SiQi", "WuWei", "BuXie", "GuiJing" };
            for (int i = 0; i < groups.Length; i++)
            {
                var heading = content.Find(headingObjects[i]).GetComponent<Text>();
                headings[i] = heading;
                heading.text = GroupNames[i];
                TextStyle(heading, 16, Ink, true, TextAnchor.MiddleLeft);
                var line = ImageNode("栏线", heading.transform, Rule);
                Band(line.rectTransform, 44, 0, 14, .8f);
            }

            location.SetParent(sheet, false);
            InkDialogArt.Inset(location, 14, 14, 135, 80);
            var caption = location.Find("Text").GetComponent<Text>();
            caption.text = "产地";
            TextStyle(caption, 14, Ink, true, TextAnchor.MiddleLeft);
            Band(caption.rectTransform, 0, 0, 0, 20);
            Band(ImageNode("产地栏线", location, Rule).rectTransform, 44, 0, 11, .8f);
            areaTabs = (RectTransform)panel.DaLuToggle.transform.parent;
            Band(areaTabs, 0, 0, 24, 34);
            // The original county control exists and is wired but ships inactive.
            panel.JunToggle.gameObject.SetActive(true);
            SetupArea(panel.DaLuToggle, panel.DaLuText, 0);
            SetupArea(panel.ZhouToggle, panel.ZhouText, 1);
            SetupArea(panel.JunToggle, panel.JunText, 2);
            fixedPlace = Label("固定产地", location, "虚神界  ·  灵植协会", 15, QuietInk);
            Band(fixedPlace.rectTransform, 2, 2, 24, 34);
            SetupList(panel.DaLuObject);
            SetupList(panel.ZhouObject);
            SetupList(panel.JunObject);

            footer.SetParent(sheet, false);
            footer.anchorMin = Vector2.zero;
            footer.anchorMax = Vector2.right;
            footer.offsetMin = new Vector2(14, 0);
            footer.offsetMax = new Vector2(-14, 75);
            Flat(footer.GetComponent<Image>(), Paper, true);
            Band(ImageNode("书口横线", footer, Rule).rectTransform, 0, 0, 0, 1);
            chosen = Label("药性摘要", footer, "", 13, QuietInk);
            Band(chosen.rectTransform, 1, 0, 4, 21);
            SetupAction(panel.QingKongXuanZeButton, footer, "重置条件", false);
            SetupAction(panel.QueDingButton, footer, "查找材料", true);
            Canvas.willRenderCanvases += CheckWidth;
        }

        private void MakeClose(RectTransform sheet)
        {
            var hit = ImageNode("合上图录", sheet, Color.clear, true);
            hit.rectTransform.anchorMin = hit.rectTransform.anchorMax = Vector2.one;
            hit.rectTransform.pivot = Vector2.one;
            hit.rectTransform.anchoredPosition = new Vector2(-4, -4);
            hit.rectTransform.sizeDelta = new Vector2(36, 36);
            var button = hit.gameObject.AddComponent<Button>();
            button.targetGraphic = hit;
            button.onClick.AddListener(panel.BackGroundBtn.onClick.Invoke);
            var face = ImageNode("合页底", hit.transform, new Color32(247, 241, 223, 238));
            InkDialogArt.Inset(face.rectTransform, 6, 6, 6, 6);
            foreach (float angle in new[] { 45f, -45f })
            {
                var stroke = ImageNode("墨线", face.transform, QuietInk).rectTransform;
                stroke.anchorMin = stroke.anchorMax = new Vector2(.5f, .5f);
                stroke.sizeDelta = new Vector2(13, 1.4f);
                stroke.anchoredPosition = Vector2.zero;
                stroke.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private static void SetupCategory(Toggle toggle, string title, int index)
        {
            var label = toggle.GetComponentInChildren<Text>(true);
            label.text = title;
            Cell((RectTransform)toggle.transform, index / 3f, (index + 1) / 3f, 0, 0);
            DressToggle(toggle, label, 15, Paper, PaleJade);
            var mark = ImageNode("页签墨线", toggle.transform, Jade);
            mark.rectTransform.anchorMin = Vector2.zero;
            mark.rectTransform.anchorMax = Vector2.right;
            mark.rectTransform.offsetMin = new Vector2(12, 0);
            mark.rectTransform.offsetMax = new Vector2(-12, 2);
            mark.gameObject.SetActive(toggle.isOn);
            toggle.onValueChanged.AddListener(mark.gameObject.SetActive);
        }

        private void SetupArea(Toggle toggle, Text label, int index)
        {
            Cell((RectTransform)toggle.transform, index / 3f, (index + 1) / 3f, index == 0 ? 0 : 2, index == 2 ? 0 : 2);
            DressToggle(toggle, label, 13, Chip, PaleJade);
            InkDialogArt.Inset(label.rectTransform, 2, 12, 0, 0);
            // Leave the chevron outside the text so long map names can shrink independently.
            label.resizeTextForBestFit = true;
            label.resizeTextMinSize = 10;
            label.resizeTextMaxSize = 13;
            var arrow = Label("展开", toggle.transform, "▼", 8, QuietInk);
            arrow.alignment = TextAnchor.MiddleCenter;
            var arrowRect = arrow.rectTransform;
            arrowRect.anchorMin = new Vector2(1, 0);
            arrowRect.anchorMax = Vector2.one;
            arrowRect.offsetMin = new Vector2(-16, 0);
            arrowRect.offsetMax = new Vector2(-3, 0);
        }

        private static void SetupList(GameObject holder)
        {
            var rect = (RectTransform)holder.transform;
            Band(rect, 0, 0, 60, 140);
            var backing = holder.GetComponent<Image>() ?? holder.AddComponent<Image>();
            Flat(backing, Rule, true);
            var scroll = holder.GetComponentInChildren<ScrollRect>(true);
            InkDialogArt.Inset((RectTransform)scroll.transform, 1, 1, 1, 1);
            PrepareScroll(scroll);
            Flat(scroll.GetComponent<Image>(), Paper, true);
            var content = scroll.content;
            content.pivot = new Vector2(.5f, 1);
            Band(content, 2, 2, 0, 1);
            var oldExample = content.Find("Image");
            if (oldExample != null)
                oldExample.gameObject.SetActive(false);
            var layout = content.GetComponent<VerticalLayoutGroup>();
            layout.childControlHeight = layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.spacing = 2;
            layout.padding = new RectOffset(3, 3, 3, 3);
        }

        private static void PrepareScroll(ScrollRect scroll)
        {
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.transform.localScale = Vector3.one;
            Flat(scroll.GetComponent<Image>(), Color.clear, true);
            InkDialogArt.Inset(scroll.viewport, 0, 0, 0, 0);
            var mask = scroll.viewport.GetComponent<Mask>();
            if (mask != null)
                mask.showMaskGraphic = false;
        }

        internal void StyleAttribute(CaiLiaoShaiXuanItem item)
        {
            DressToggle(item.toggle, item.Name, 14, Chip, PaleJade);
            if (item.transform.parent != groups[0])
                return;
            int index = System.Array.IndexOf(new[] { "金", "木", "水", "火", "土" }, item.Name.text);
            if (index < 0)
                return;
            var mark = ImageNode("五行色记", item.transform, ElementColors[index]).rectTransform;
            mark.anchorMin = mark.anchorMax = new Vector2(.5f, .5f);
            mark.anchoredPosition = new Vector2(-9, 0);
            mark.sizeDelta = new Vector2(3.8f, 3.8f);
            mark.localRotation = Quaternion.Euler(0, 0, 45);
            InkDialogArt.Inset(item.Name.rectTransform, 7, 0, 0, 0);
        }

        internal void StylePlace(CaiLiaoShaiXuanWeiZhiItem item)
        {
            DressToggle(item.toggle, item.Name, 14, Paper, PaleJade);
            var element = item.GetComponent<LayoutElement>() ?? item.gameObject.AddComponent<LayoutElement>();
            element.preferredHeight = element.minHeight = 32;
        }

        private static void DressToggle(Toggle toggle, Text label, int size, Color background, Color selection)
        {
            toggle.transition = Selectable.Transition.None;
            toggle.toggleTransition = Toggle.ToggleTransition.None;
            Flat(toggle.targetGraphic as Image, background, true);
            if (toggle.graphic is Image active && active != toggle.targetGraphic)
            {
                Flat(active, selection, false);
                InkDialogArt.Inset(active.rectTransform, 0, 0, 0, 0);
                active.canvasRenderer.SetAlpha(toggle.isOn ? 1 : 0);
            }
            InkDialogArt.Inset(label.rectTransform, 0, 0, 0, 0);
            TextStyle(label, size, QuietInk, false, TextAnchor.MiddleCenter);
            PaintSelection(label, toggle.isOn);
            toggle.onValueChanged.AddListener(on => PaintSelection(label, on));
        }

        private static void PaintSelection(Text label, bool on)
        {
            label.color = on ? Jade : QuietInk;
            label.fontStyle = on ? FontStyle.Bold : FontStyle.Normal;
        }

        private void SetupAction(Button button, RectTransform footer, string caption, bool primary)
        {
            var rect = (RectTransform)button.transform;
            rect.SetParent(footer, false);
            Cell(rect, primary ? .36f : 0f, primary ? 1f : .36f, primary ? 4 : 0, primary ? 0 : 4);
            rect.offsetMin = new Vector2(rect.offsetMin.x, 9);
            rect.offsetMax = new Vector2(rect.offsetMax.x, -29);
            Flat(button.image, primary ? Jade : Chip, true);
            var text = button.GetComponentInChildren<Text>(true);
            text.text = caption;
            InkDialogArt.Inset(text.rectTransform, 1, 1, 0, 0);
            TextStyle(text, primary ? 16 : 14, primary ? Paper : QuietInk, primary, TextAnchor.MiddleCenter);
        }

        internal void Refresh(CaiLiaoXuanZheModel model)
        {
            bool cultivated = model.materialType == 3;
            areaTabs.gameObject.SetActive(!cultivated);
            fixedPlace.gameObject.SetActive(cultivated);
            var area = new[] { panel.DaLuToggle, panel.ZhouToggle, panel.JunToggle };
            var labels = new[] { panel.DaLuText, panel.ZhouText, panel.JunText };
            for (int i = 0; i < area.Length; i++)
            {
                labels[i].color = area[i].interactable ? Jade : new Color32(100, 102, 88, 255);
                area[i].transform.Find("展开").gameObject.SetActive(area[i].interactable);
            }

            var selected = new List<string>(5);
            foreach (var group in groups)
                foreach (var item in group.GetComponentsInChildren<CaiLiaoShaiXuanItem>(false))
                    if (item.toggle.isOn && item.Name.text != "不限")
                        selected.Add(item.Name.text);
            chosen.text = selected.Count == 0 ? "药性不限" : "已选药性　" + string.Join(" · ", selected);
        }

        internal void LayoutAttributes()
        {
            float width = attributes.viewport.rect.width;
            if (width < 1f)
                return;
            layoutWidth = width;
            float top = 0f;
            for (int i = 0; i < groups.Length; i++)
            {
                Band(headings[i].rectTransform, 0, 0, top, 25);
                top += 26;
                var group = groups[i];
                var grid = group.GetComponent<GridLayoutGroup>();
                int columns = Mathf.Clamp(Mathf.FloorToInt((width + 4) / (i == 4 ? 57f : 40f)), 1, PreferredColumns[i]);
                int rows = Mathf.CeilToInt((float)group.childCount / columns);
                const float cellHeight = 32, gap = 4;
                grid.startAxis = GridLayoutGroup.Axis.Horizontal;
                grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
                grid.childAlignment = TextAnchor.UpperLeft;
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = columns;
                grid.padding = new RectOffset(0, 0, 0, 0);
                grid.spacing = new Vector2(gap, gap);
                grid.cellSize = new Vector2((width - (columns - 1) * gap) / columns, cellHeight);
                float height = rows * cellHeight + Mathf.Max(0, rows - 1) * gap;
                Band(group, 0, 0, top, height);
                top += height + 8;
            }
            var content = panel.ZongGanDu;
            float scrolled = Mathf.Clamp(content.anchoredPosition.y, 0, Mathf.Max(0, top - attributes.viewport.rect.height));
            Band(content, 0, 0, -scrolled, top);
        }

        private void CheckWidth()
        {
            if (Mathf.Abs(layoutWidth - attributes.viewport.rect.width) > .25f)
                LayoutAttributes();
        }

        internal void Dispose()
        {
            Canvas.willRenderCanvases -= CheckWidth;
        }

        private static Image ImageNode(string name, Transform parent, Color color, bool hit = false)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Image));
            obj.layer = parent.gameObject.layer;
            obj.transform.SetParent(parent, false);
            var image = obj.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = hit;
            return image;
        }

        private Text Label(string name, Transform parent, string caption, int size, Color color, bool bold = false)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Text));
            obj.layer = parent.gameObject.layer;
            obj.transform.SetParent(parent, false);
            var label = obj.GetComponent<Text>();
            label.font = font;
            label.text = caption;
            TextStyle(label, size, color, bold, TextAnchor.MiddleLeft);
            return label;
        }

        private static void TextStyle(Text label, int size, Color color, bool bold, TextAnchor alignment)
        {
            label.fontSize = size;
            label.resizeTextForBestFit = false;
            label.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
            label.color = color;
            label.alignment = alignment;
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Truncate;
            label.raycastTarget = false;
        }

        private static void Flat(Image image, Color color, bool hit)
        {
            if (image == null)
                return;
            image.sprite = null;
            image.overrideSprite = null;
            image.type = Image.Type.Simple;
            image.color = color;
            image.raycastTarget = hit;
        }

        private static void Band(RectTransform rect, float left, float right, float top, float height)
        {
            rect.anchorMin = Vector2.up;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, -top - height);
            rect.offsetMax = new Vector2(-right, -top);
        }

        private static void Cell(RectTransform rect, float start, float end, float left, float right)
        {
            rect.anchorMin = new Vector2(start, 0);
            rect.anchorMax = new Vector2(end, 1);
            rect.offsetMin = new Vector2(left, 0);
            rect.offsetMax = new Vector2(-right, 0);
        }
    }
}
