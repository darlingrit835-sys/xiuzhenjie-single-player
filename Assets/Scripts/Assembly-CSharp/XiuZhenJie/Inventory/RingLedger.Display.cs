using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class RingLedger
    {
        private float tabWidth = 75f;
        private int tabsPerView = 1;
        private RingCategoryScroll categoryScroll;
        private Button previousCategories, nextCategories;
        private Image modeBand, previousArrow, nextArrow;
        private RectTransform listTrack, listThumb;

        private void BuildCategoryStrip()
        {
            var strip = (RectTransform)Node(panel.transform, "SectionStrip").transform;
            Place(strip, 0, 1, 5, -5, 104, 35);

            var view = (RectTransform)Node(strip, "CategoryViewport").transform;
            InkDialogArt.Inset(view, 25, 25, 0, 0);
            view.gameObject.AddComponent<Image>().color = new Color32(242, 235, 215, 255);
            view.gameObject.AddComponent<RectMask2D>();
            categoryScroll = view.gameObject.AddComponent<RingCategoryScroll>();
            var content = (RectTransform)Node(view, "CategoryContent").transform;
            content.anchorMin = Vector2.zero;
            content.anchorMax = Vector2.up;
            content.pivot = new Vector2(0, .5f);
            content.sizeDelta = Vector2.zero;
            content.anchoredPosition = Vector2.zero;
            categoryScroll.content = content;
            categoryScroll.viewport = view;
            categoryScroll.horizontal = true;
            categoryScroll.vertical = false;
            categoryScroll.inertia = false;
            categoryScroll.movementType = ScrollRect.MovementType.Clamped;
            categoryScroll.scrollSensitivity = 30f;

            for (int i = 0; i < Sections.Length; i++)
            {
                int id = i;
                var tab = Control(content, "Section" + id, Sections[id], () =>
                {
                    category = id;
                    RevealCategory(id);
                    Redraw(true);
                });
                var mount = Node(tab.transform, "IconPaper").AddComponent<Image>();
                InkInterfaceArt.Set(mount, "LightPaper", false);
                mount.raycastTarget = false;
                Place(mount.rectTransform, 0, 0, 3, 30, 4, 27);
                var art = Node(tab.transform, "SectionArt").AddComponent<Image>();
                art.raycastTarget = false;
                art.preserveAspect = true;
                art.sprite = RingGoodsCatalog.SectionArtwork(id);
                art.color = RingGoodsCatalog.ArtTint(art.sprite);
                Place(art.rectTransform, 0, 0, 5, 28, 6, 23);
                var caption = tab.GetComponentInChildren<Text>();
                InkDialogArt.Inset(caption.rectTransform, 30, 3, 0, 0);
                caption.fontSize = 15;
                caption.resizeTextForBestFit = false;
                tabs.Add(tab);
            }

            previousCategories = Arrow(strip, "PreviousCategories", true, () => PageCategories(-1));
            nextCategories = Arrow(strip, "NextCategories", false, () => PageCategories(1));
            previousArrow = previousCategories.transform.Find("Arrow").GetComponent<Image>();
            nextArrow = nextCategories.transform.Find("Arrow").GetComponent<Image>();
            categoryScroll.onValueChanged.AddListener(_ => UpdateArrows());
            categoryScroll.LayoutTabs = LayoutCategoryTabs;
            categoryScroll.SnapTabs = () => ScrollToCategorySlot(FirstCategorySlot);
        }

        private Button Arrow(Transform parent, string name, bool left, UnityEngine.Events.UnityAction action)
        {
            var tab = Control(parent, name, "", action);
            float edge = left ? 0 : 1;
            Place((RectTransform)tab.transform, edge, edge, left ? 0 : -24, left ? 24 : 0, 0, 35);
            var art = Node(tab.transform, "Arrow").AddComponent<Image>();
            art.raycastTarget = false;
            InkInterfaceArt.Set(art, "Back");
            InkDialogArt.Inset(art.rectTransform, 6, 6, 10, 10);
            art.rectTransform.localScale = new Vector3(left ? 1 : -1, 1, 1);
            return tab;
        }

        internal void RefreshCategoryStrip()
        {
            if (!CategoryOrder.Contains(category)) category = 0;
            LayoutCategoryTabs();
            Redraw(true);
        }

        private int FirstCategorySlot => Mathf.RoundToInt(-categoryScroll.content.anchoredPosition.x / tabWidth);

        private void LayoutCategoryTabs()
        {
            float width = categoryScroll.viewport.rect.width;
            if (width < 1) return;
            tabsPerView = Mathf.Clamp(Mathf.FloorToInt(width / 70f), 1, 5);
            tabWidth = width / tabsPerView;
            var order = CategoryOrder;
            foreach (var tab in tabs) tab.gameObject.SetActive(false);
            for (int i = 0; i < order.Count; i++)
            {
                var tab = tabs[order[i]];
                tab.gameObject.SetActive(true);
                tab.transform.SetAsLastSibling();
                Place((RectTransform)tab.transform, 0, 0, i * tabWidth + 1, (i + 1) * tabWidth - 1, 0, 35);
            }
            categoryScroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(width, order.Count * tabWidth));
            ScrollToCategorySlot(0);
            RevealCategory(category);
        }

        private float CategoryRange => Mathf.Max(0, categoryScroll.content.rect.width - categoryScroll.viewport.rect.width);

        private void RevealCategory(int id)
        {
            var order = CategoryOrder;
            for (int i = 0; i < order.Count; i++)
            {
                if (order[i] != id) continue;
                ScrollToCategorySlot(Mathf.Clamp(FirstCategorySlot, i - tabsPerView + 1, i));
                return;
            }
        }

        private void PageCategories(int step) => ScrollToCategorySlot(FirstCategorySlot + step * tabsPerView);

        private void ScrollToCategorySlot(int slot)
        {
            slot = Mathf.Clamp(slot, 0, Mathf.Max(0, CategoryOrder.Count - tabsPerView));
            categoryScroll.StopMovement();
            categoryScroll.content.anchoredPosition = new Vector2(-slot * tabWidth, 0);
            UpdateArrows();
        }

        private void UpdateArrows()
        {
            float range = CategoryRange;
            float x = -categoryScroll.content.anchoredPosition.x;
            previousCategories.interactable = x > 1f && range > 1f;
            nextCategories.interactable = x < range - 1f;
            previousArrow.canvasRenderer.SetAlpha(previousCategories.interactable ? 1f : .28f);
            nextArrow.canvasRenderer.SetAlpha(nextCategories.interactable ? 1f : .28f);
        }

        private void BuildListIndicator()
        {
            var track = Node(scroll.transform, "ScrollTrack").AddComponent<Image>();
            track.raycastTarget = false;
            track.color = new Color32(205, 195, 167, 210);
            listTrack = track.rectTransform;
            listTrack.anchorMin = new Vector2(1, 0);
            listTrack.anchorMax = Vector2.one;
            listTrack.offsetMin = new Vector2(-4.5f, 3f);
            listTrack.offsetMax = new Vector2(-1.5f, -3f);
            var thumb = Node(listTrack, "Thumb").AddComponent<Image>();
            thumb.raycastTarget = false;
            thumb.color = new Color32(78, 112, 91, 255);
            listThumb = thumb.rectTransform;
            scroll.onValueChanged.AddListener(_ => UpdateListIndicator());
        }

        private void UpdateListIndicator()
        {
            float height = Mathf.Max(1, scroll.content.rect.height);
            float viewport = Mathf.Max(1, scroll.viewport.rect.height);
            bool canScroll = height > viewport + 1;
            listTrack.gameObject.SetActive(canScroll);
            if (!canScroll) return;
            float size = Mathf.Clamp(viewport / height, .055f, 1);
            float bottom = Mathf.Clamp01(scroll.verticalNormalizedPosition) * (1 - size);
            listThumb.anchorMin = new Vector2(0, bottom);
            listThumb.anchorMax = new Vector2(1, bottom + size);
            listThumb.offsetMin = listThumb.offsetMax = Vector2.zero;
        }

        private Row NewRow()
        {
            var row = new Row();
            var obj = Node(panel.NaJieListConstant, "GoodsRow");
            row.Button = obj.AddComponent<Button>();
            row.Button.targetGraphic = obj.AddComponent<Image>();
            InkInterfaceArt.Set(row.Button.image, "LightPaper", false);
            row.Button.onClick.AddListener(() => PressRow(row));
            row.Focus = Node(obj.transform, "FocusFrame").AddComponent<RingJadeFrame>();
            row.Focus.raycastTarget = false;
            InkDialogArt.Inset(row.Focus.rectTransform, 0, 0, 0, 0);
            row.Focus.gameObject.SetActive(false);

            row.Icon = Node(obj.transform, "Artwork").AddComponent<Image>();
            row.Icon.preserveAspect = true;
            row.Icon.raycastTarget = false;
            row.Name = Label(obj.transform, "Name", 15, TextAnchor.MiddleLeft);
            row.Name.fontStyle = FontStyle.Bold;
            row.Detail = Label(obj.transform, "Attributes", 12, TextAnchor.MiddleLeft);
            row.Detail.color = InkInterfaceArt.SoftInk;
            row.Count = Label(obj.transform, "Quantity", 14, TextAnchor.MiddleRight);
            Place(row.Count.rectTransform, 1, 1, -68, -7, 1, 22);
            row.State = Label(obj.transform, "State", 11, TextAnchor.MiddleRight);
            Place(row.State.rectTransform, 1, 1, -90, -7, 22, 18);

            row.Box = Node(obj.transform, "SelectionBox").AddComponent<Image>();
            row.Box.raycastTarget = false;
            Place(row.Box.rectTransform, 0, 0, 6, 24, 12, 18);
            row.Check = (RectTransform)Node(row.Box.transform, "CheckMark").transform;
            InkDialogArt.Inset(row.Check, 0, 0, 0, 0);
            // Two brush strokes keep the tick legible with every bundled Chinese font.
            CheckStroke(row.Check, "Downstroke", new Vector2(-3.3f, -.9f), 5.7f, -48f);
            CheckStroke(row.Check, "Upstroke", new Vector2(2.4f, 1f), 10.9f, 53f);
            return row;
        }

        private static void CheckStroke(RectTransform parent, string name, Vector2 center, float width, float angle)
        {
            var brush = Node(parent, name).AddComponent<Image>();
            brush.raycastTarget = false;
            brush.color = InkDialogArt.Ivory;
            var rect = brush.rectTransform;
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one * .5f;
            rect.anchoredPosition = center;
            rect.sizeDelta = new Vector2(width, 2.1f);
            rect.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private void PressRow(Row row)
        {
            var g = row.Item;
            if (!Choosing)
            {
                rows.TryGetValue(focusedId ?? "", out var previous);
                focusedId = g.GoodsId;
                if (previous != null && previous != row) SetRowFrame(previous, false);
                SetRowFrame(row, true);
                inspect(g, row.Button.gameObject);
                return;
            }
            if (!CanChoose(g)) return;
            bool added = marked.Add(g.GoodsId);
            if (!added) marked.Remove(g.GoodsId);
            Redraw(false);
            row.Check.DOKill();
            if (!added)
            {
                row.Box.rectTransform.DOKill();
                row.Box.rectTransform.localScale = Vector3.one * .85f;
                row.Box.rectTransform.DOScale(1f, .12f).SetUpdate(true);
                return;
            }
            row.Check.localScale = Vector3.one * .6f;
            row.Check.DOScale(1f, .18f).SetEase(Ease.OutBack).SetUpdate(true);
            ((RingJadeFrame)row.Focus).Flash();
        }

        private static void SetRowFrame(Row row, bool selected)
        {
            if (row.Focus.gameObject.activeSelf == selected) return;
            row.Focus.gameObject.SetActive(selected);
            InkInterfaceArt.Set(row.Button.image, selected ? "JadeSelection" : "LightPaper", false);
        }

        private void UpdateRow(Row row, bool markedHere)
        {
            var g = row.Item;
            bool available = CanChoose(g);
            row.Button.interactable = !Choosing || available;
            SetRowFrame(row, Choosing ? markedHere : g.GoodsId == focusedId);
            if (!available && row.Focus.isActiveAndEnabled) ((RingJadeFrame)row.Focus).StopFlash();
            row.Icon.sprite = Artwork(g);
            Color artworkTint = RingGoodsCatalog.ArtTint(row.Icon.sprite);
            row.Icon.color = Choosing && !available ? Color.Lerp(artworkTint, InkInterfaceArt.SoftInk, .33f) : artworkTint;
            float x = Choosing ? 29 : 5;
            Place(row.Icon.rectTransform, 0, 0, x, x + 35, 3, 35);
            Place(row.Name.rectTransform, 0, 1, x + 40, -65, 1, 22);
            Place(row.Detail.rectTransform, 0, 1, x + 40, -87, 22, 18);
            row.Name.text = g.Name;
            row.Count.text = "×" + g.GoodsNum;
            row.Detail.text = RingGoodsCatalog.Kind(g) + " · " + WorldSetting.GetRankAllName(g.Rank)
                + (g.FiveElement >= 1 && g.FiveElement <= 5 ? " · " + g.GetFiveElementName() + "行" : "");
            string protection = Status(g);
            if (guide && PracticeGuideFlow.IsStarter(g) && protection.Length == 0) protection = "引导物品";
            row.State.text = protection.Length > 0 ? protection : g.RecyclePrice < 0 ? "不可出售" : $"单价 {g.RecyclePrice} 灵石";
            row.State.color = protection.Length > 0 ? InkInterfaceArt.Seal : InkInterfaceArt.SoftInk;
            row.Box.gameObject.SetActive(Choosing);
            row.Check.gameObject.SetActive(Choosing && markedHere);
            if (!Choosing) return;
            InkInterfaceArt.Set(row.Box, markedHere ? "JadeButton" : "LightPlaque", false);
            row.Box.fillCenter = markedHere;
            row.Box.color = available ? Color.white : new Color32(145, 140, 122, 175);
        }
    }
}
