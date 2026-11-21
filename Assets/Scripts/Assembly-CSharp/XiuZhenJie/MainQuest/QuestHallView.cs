using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    internal sealed class QuestHallView
    {
        private readonly Font font;
        private readonly ScrollRect scroll;
        private readonly Text[] tabs = new Text[3];
        private readonly Image[] marks = new Image[3];
        private readonly CanvasGroup pageFade;
        internal RectTransform Content => scroll.content;

        internal QuestHallView(ChengZhuFuMainPanel panel, UnityAction main, UnityAction bounties, UnityAction stone, UnityAction back)
        {
            font = panel.QieHuanText.font;
            var root = (RectTransform)panel.transform;
            Fill(root, 0, 0, 94, 0);
            for (int i = 0; i < root.childCount; i++)
                root.GetChild(i).gameObject.SetActive(false);
            Flat(root.GetComponent<Image>(), Paper, true);

            var page = RoleFolio.Image(root, "城主府任务厅", Paper, true);
            Fill(page.rectTransform);
            var header = RoleFolio.Image(page.transform, "府衙题图", Card);
            Band(header.rectTransform, 0, 0, 0, 89);
            header.gameObject.AddComponent<RectMask2D>();
            var scene = RoleFolio.Image(header.transform, "山水", Color.white);
            InkInterfaceArt.Set(scene, "InkLandscapePaper", false);
            var crop = scene.gameObject.AddComponent<AspectRatioFitter>();
            crop.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            if (scene.sprite != null) crop.aspectRatio = scene.sprite.rect.width / scene.sprite.rect.height;
            Fill(RoleFolio.Image(header.transform, "衬纸", new Color32(247, 242, 223, 150)).rectTransform);
            var book = RoleFolio.Image(header.transform, "府印", Color.white);
            InkInterfaceArt.Set(book, "Pavilion", false);
            book.preserveAspect = true;
            Fixed(book.rectTransform, 0, 17, 22, 41, 42);
            var title = Label(header.transform, font, "城主府", 27, Ink, TextAnchor.MiddleLeft, true);
            Band(title.rectTransform, 67, 85, 15, 36);
            var sub = Label(header.transform, font, "任务厅", 14, Secondary);
            Band(sub.rectTransform, 68, 80, 50, 22);
            panel.LiKaiBtn = SmallButton(header.transform, "返回", back);
            Fixed((RectTransform)panel.LiKaiBtn.transform, 1, -12, 14, 56, 45);

            var nav = Box(page.transform, "任务分类");
            Band(nav, 14, 14, 91, 42);
            var first = Tab(nav, 0, "主线任务", main);
            var second = Tab(nav, 1, "悬赏任务", bounties);
            Span((RectTransform)first.transform, 0, .34f, 0, 3);
            Span((RectTransform)second.transform, .34f, .68f, 2, 3);
            panel.QieHuanPanelBtn = Tab(nav, 2, "天书碑", stone);
            Span((RectTransform)panel.QieHuanPanelBtn.transform, .70f, 1, 3, 0);
            panel.QieHuanText = panel.QieHuanPanelBtn.GetComponentInChildren<Text>();
            Band(RoleFolio.Image(page.transform, "分栏线", Rule).rectTransform, 14, 14, 138, 1);
            var host = RoleFolio.Image(page.transform, "任务卷", Color.clear, true);
            Fill(host.rectTransform, 0, 0, 142, 8);
            scroll = host.gameObject.AddComponent<ScrollRect>();
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            var viewport = Box(host.transform, "Viewport");
            Fill(viewport);
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll.viewport = viewport;
            var content = Box(viewport, "Content");
            content.pivot = new Vector2(.5f, 1);
            Band(content, 0, 0, 0, 0);
            Stack(content, 14, 9);
            var fitter = content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.content = content;
            pageFade = host.gameObject.AddComponent<CanvasGroup>();
        }

        private Button Tab(Transform parent, int index, string text, UnityAction action)
        {
            var button = SmallButton(parent, text, action);
            tabs[index] = button.GetComponentInChildren<Text>();
            var mark = RoleFolio.Image(button.transform, "墨线", Jade);
            mark.rectTransform.anchorMin = Vector2.zero;
            mark.rectTransform.anchorMax = Vector2.right;
            mark.rectTransform.offsetMin = new Vector2(7, 1);
            mark.rectTransform.offsetMax = new Vector2(-7, 3);
            marks[index] = mark;
            return button;
        }

        private Button SmallButton(Transform parent, string title, UnityAction action)
        {
            var surface = RoleFolio.Image(parent, title, Card, true);
            var button = surface.gameObject.AddComponent<Button>();
            button.targetGraphic = surface;
            var caption = Label(surface.transform, font, title, 15, Jade, TextAnchor.MiddleCenter, true);
            Fill(caption.rectTransform, 3, 3);
            button.onClick.AddListener(action);
            return button;
        }

        internal void Select(int page)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                marks[i].enabled = page == i;
                tabs[i].color = page == i ? Jade : Secondary;
            }
            scroll.StopMovement();
            scroll.verticalNormalizedPosition = 1f;
            scroll.content.anchoredPosition = Vector2.zero;
            pageFade.DOKill();
            pageFade.alpha = .7f;
            pageFade.DOFade(1f, .15f).SetUpdate(true);
        }

        internal void Clear()
        {
            for (int i = Content.childCount - 1; i >= 0; i--)
            {
                var child = Content.GetChild(i);
                child.gameObject.SetActive(false);
                child.SetParent(null, false);
                Object.Destroy(child.gameObject);
            }
        }

        internal void Dispose() => pageFade.DOKill();

        private static void Stack(RectTransform rect, int inset, float gap)
        {
            var group = rect.gameObject.AddComponent<VerticalLayoutGroup>();
            group.padding = new RectOffset(inset, inset, inset, inset);
            group.spacing = gap;
            group.childAlignment = TextAnchor.UpperLeft;
            group.childControlHeight = group.childControlWidth = group.childForceExpandWidth = true;
            group.childForceExpandHeight = false;
        }

        internal RectTransform CardRow(string name)
        {
            var card = RoleFolio.Image(Content, name, Card);
            Stack(card.rectTransform, 13, 6);
            return card.rectTransform;
        }

        internal Text Line(Transform parent, string text, int size = 15, bool heading = false, bool accent = false)
        {
            var label = Label(parent, font, text, size, accent ? Jade : heading ? Ink : Secondary, TextAnchor.MiddleLeft, heading);
            label.lineSpacing = 1.14f;
            return label;
        }

        internal void Empty(string title, string details, UnityAction retry = null)
        {
            var card = CardRow(title);
            Line(card, title, 19, true);
            if (!string.IsNullOrEmpty(details)) Line(card, details);
            if (retry != null) Action(card, "重新加载", retry);
        }

        internal Button Action(Transform parent, string title, UnityAction click, bool enabled = true)
        {
            var surface = RoleFolio.Image(parent, title, enabled ? Jade : new Color32(221, 219, 202, 255), enabled);
            Height(surface.rectTransform, 40);
            var button = surface.gameObject.AddComponent<Button>();
            button.targetGraphic = surface;
            button.interactable = enabled;
            var colors = button.colors;
            colors.disabledColor = Color.white;
            button.colors = colors;
            if (click != null) button.onClick.AddListener(click);
            var caption = Label(surface.transform, font, title, 15, enabled ? Paper : Secondary, TextAnchor.MiddleCenter, true);
            Fill(caption.rectTransform, 5, 5);
            return button;
        }

        internal void Reward(RectTransform parent, string name, int quantity, Sprite icon, string quality)
        {
            var row = Box(parent, "任务奖励");
            Height(row, 50);
            var image = RoleFolio.Image(row, "奖励图标", Color.white);
            if (icon == null) InkInterfaceArt.Set(image, "Gift", false);
            else image.sprite = icon;
            image.preserveAspect = true;
            Fixed(image.rectTransform, 0, 0, 5, 40, 40);
            var caption = Label(row, font, string.IsNullOrEmpty(quality) ? "任务奖励" : "任务奖励  ·  " + quality, 12, Secondary);
            Band(caption.rectTransform, 50, 2, 0, 20);
            var value = Label(row, font, quantity > 0 ? $"{name} ×{quantity}" : name, 16, Ink, TextAnchor.MiddleLeft, true);
            Band(value.rectTransform, 50, 2, 20, 28);
        }
    }
}
