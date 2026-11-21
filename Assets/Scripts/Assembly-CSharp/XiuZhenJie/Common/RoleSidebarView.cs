using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    internal sealed class RoleSidebarView
    {
        private readonly CeHuaPanel panel;
        private readonly RectTransform sheet;
        private readonly Font font;
        private readonly ScrollRect stats;
        private readonly ScrollRect professions;
        private readonly Text empty;
        private readonly Text[] tabs = new Text[2];
        private readonly Image[] tabRules = new Image[2];
        private readonly Text[] moneyLabels = new Text[2];
        private readonly RectTransform[] moneyIcons = new RectTransform[2];
        private RectTransform wallet;
        private bool? balancesStacked;
        private bool open;

        internal RoleSidebarView(CeHuaPanel panel)
        {
            this.panel = panel;
            font = panel.LingGenValue_text.font;
            var root = (RectTransform)panel.transform;
            var header = (RectTransform)root.Find("CeHuaTitle_image");
            var body = (RectTransform)root.Find("ConentPanel");
            var formerFooter = (RectTransform)root.Find("BottomBg");
            var propertyPanel = (RectTransform)body.Find("PropertyContent");
            stats = propertyPanel.GetComponentInChildren<ScrollRect>(true);

            // The window is preloaded beside the HUD. An offscreen sheet must leave the HUD clickable.
            Fill(root);
            Flat(root.GetComponent<Image>(), Color.clear);
            Fill(panel.MaskPanel);
            Flat(panel.Mask_btn.image, new Color32(37, 48, 40, 133), true);
            panel.MaskPanel.SetAsFirstSibling();

            sheet = RoleFolio.Image(root, "修士道籍", Paper, true).rectTransform;
            Span(sheet, 0, .84f);
            sheet.pivot = new Vector2(0, .5f);
            header.SetParent(sheet, false);
            Band(header, 0, 0, 0, 118);
            MakeHeader(header);

            body.SetParent(sheet, false);
            Fill(body, 0, 0, 156, 61);
            Flat(body.GetComponent<Image>(), Color.clear);
            Fill(propertyPanel);
            Flat(propertyPanel.GetComponent<Image>(), Color.clear);
            SetupScroll(stats);
            var content = stats.content;
            Stack(content);
            MakeStats(content);

            professions = MakeProfessionScroll(body);
            var jobs = professions.content;
            Heading(jobs, "所修副职");
            var rows = Box(jobs, "副职条目");
            Stack(rows, false);
            panel.FuZhiContent = rows;
            empty = Label(jobs, font, "尚未修习副职", 16, Secondary, TextAnchor.MiddleCenter);
            Height(empty.rectTransform, 78);

            var strip = Box(sheet, "道籍页签");
            Band(strip, 14, 14, 118, 35);
            Band(RoleFolio.Image(strip, "页签底线", Rule).rectTransform, 0, 0, 34, 1);
            AddTab(strip, 0, "修行");
            AddTab(strip, 1, "副职");

            formerFooter.SetParent(sheet, false);
            formerFooter.anchorMin = Vector2.zero;
            formerFooter.anchorMax = Vector2.right;
            formerFooter.offsetMin = Vector2.zero;
            formerFooter.offsetMax = new Vector2(0, 59);
            Flat(formerFooter.GetComponent<Image>(), Card, true);
            Band(RoleFolio.Image(formerFooter, "页脚线", Rule).rectTransform, 0, 0, 0, 1);
            FooterButton(formerFooter, panel.SheZhi_btn, 0, "设置", null);
            FooterButton(formerFooter, panel.ChuanXin_btn, 1, "传信", "Letter");
            FooterButton(formerFooter, panel.HuoDong_btn, 2, "签到", "AttendanceSeal");

            SelectTab(0);
            SetProfessionCount(0);
        }

        private void MakeHeader(RectTransform header)
        {
            Flat(header.GetComponent<Image>(), Card);
            var artWindow = Box(header, "山水题图");
            Fill(artWindow);
            artWindow.SetAsFirstSibling();
            artWindow.gameObject.AddComponent<RectMask2D>();
            var art = RoleFolio.Image(artWindow, "溪山", Color.white);
            InkInterfaceArt.Set(art, "InkLandscapePaper", false);
            // Crop the existing vertical landscape to a wide strip without stretching its peaks.
            var crop = art.gameObject.AddComponent<AspectRatioFitter>();
            crop.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            if (art.sprite != null)
                crop.aspectRatio = art.sprite.rect.width / art.sprite.rect.height;
            var veil = RoleFolio.Image(artWindow, "浅绢", new Color32(249, 245, 232, 95));
            Fill(veil.rectTransform);

            var kicker = Label(header, font, "修士道籍", 13, Jade, TextAnchor.MiddleLeft, true);
            Band(kicker.rectTransform, 84, 55, 16, 24);
            var frame = RoleFolio.Image(header, "肖像衬纸", new Color32(196, 194, 169, 255));
            Fixed(frame.rectTransform, 0, 15, 46, 57, 57);
            panel.TouXiang_img.transform.SetParent(frame.transform, false);
            Fill(panel.TouXiang_img.rectTransform, 2, 2, 2, 2);
            panel.TouXiang_img.preserveAspect = true;
            panel.TouXiang_img.color = Color.white;
            panel.TouXiang_img.raycastTarget = false;
            Fill(panel.VIP_HeadFrame.rectTransform, -2, -2, -2, -2);
            panel.VIP_HeadFrame.raycastTarget = false;
            Style(panel.RoleName_text, 23, Ink, TextAnchor.MiddleLeft, true, 14);
            Band(panel.RoleName_text.rectTransform, 84, 12, 42, 33);
            var realm = (RectTransform)panel.JingJie_InsBtn.transform.parent;
            realm.SetParent(header, false);
            Band(realm, 84, 12, 75, 33);
            realm.GetChild(0).gameObject.SetActive(false);
            Style(panel.JingJieValue_text, 15, Jade, TextAnchor.MiddleLeft, false, 11);
            Fill(panel.JingJieValue_text.rectTransform, 0, 18);
            var detail = Label(realm, font, "›", 16, Jade, TextAnchor.MiddleRight);
            Fixed(detail.rectTransform, 1, -2, 0, 13, 33);
            Widen(panel.JingJie_InsBtn, realm);

            var close = panel.FanHui_btn;
            close.transform.SetParent(header, false);
            Fixed((RectTransform)close.transform, 1, -6, 8, 49, 38);
            var oldIcon = close.transform.Find("Image");
            if (oldIcon != null) oldIcon.gameObject.SetActive(false);
            var closeText = close.GetComponentInChildren<Text>(true);
            closeText.text = "收起";
            Fill(closeText.rectTransform);
            Style(closeText, 13, Jade, TextAnchor.MiddleCenter, true);
            Touch(close);
        }

        private static void SetupScroll(ScrollRect scroll)
        {
            Fill((RectTransform)scroll.transform);
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            Flat(scroll.GetComponent<Image>(), Color.clear, true);
            Fill(scroll.viewport);
            if (scroll.viewport.TryGetComponent(out Mask mask))
                mask.showMaskGraphic = false;
            scroll.content.pivot = new Vector2(.5f, 1);
            Band(scroll.content, 0, 0, 0, 0);
        }

        private static void Stack(RectTransform content, bool fit = true)
        {
            var layout = content.GetComponent<VerticalLayoutGroup>() ?? content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = fit ? new RectOffset(14, 14, 2, 12) : new RectOffset();
            layout.spacing = fit ? 2 : 5;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = layout.childControlHeight = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            if (!fit) return;
            var fitter = content.GetComponent<ContentSizeFitter>() ?? content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        }

        private ScrollRect MakeProfessionScroll(RectTransform body)
        {
            var host = RoleFolio.Image(body, "副职页", Color.clear, true);
            var scroll = host.gameObject.AddComponent<ScrollRect>();
            var viewport = Box(host.transform, "Viewport");
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll.viewport = viewport;
            scroll.content = Box(viewport, "Content");
            SetupScroll(scroll);
            Stack(scroll.content);
            return scroll;
        }

        private Text Heading(RectTransform content, string title)
        {
            var row = Box(content, title);
            Height(row, 28);
            var label = Label(row, font, title, 14, Jade, TextAnchor.MiddleLeft, true);
            Fill(label.rectTransform);
            Band(RoleFolio.Image(row, "栏线", Rule).rectTransform, 68, 0, 15, .8f);
            return label;
        }

        private void MakeStats(RectTransform content)
        {
            Heading(content, "随身财物");
            wallet = Box(content, "财物");
            Money(wallet, panel.LingShi_btn, panel.LingShiValue_text, 0, "灵石", "SilverIngot");
            Money(wallet, panel.LingYu_btn, panel.LingYuValue_text, 1, "灵玉", "JadePendant");

            Heading(content, "修行根基");
            Detail(content, panel.LingGen_InsBtn, panel.LingGenValue_text, "灵根");
            Detail(content, panel.ShouYuan_InsBtn, panel.ShouYuanValu_text, "寿元");
            Detail(content, panel.ZongMen_InsBtn, panel.ZongMenValue_text, "宗门");

            Heading(content, "身心修为");
            Pair(content, panel.JingXue_InsBtn, panel.JingXueValue_text, panel.ZhenYuan_InsBtn, panel.ZhenYuanValue_text);
            Pair(content, panel.ShenShi_InsBtn, panel.ShenShiValue_text, panel.TiPo_InsBtn, panel.JianRenValue_text);
            Pair(content, panel.TiLi_InsBtn, panel.TiLiValue_text, panel.JingLi_InsBtn, panel.JingLiValue_text);
            Pair(content, panel.WuXing_InsBtn, panel.WuXingValue_text, panel.HunLi_InsBtn, panel.HunLiValue_text);
            Pair(content, panel.DanDu_InsBtn, panel.DanDuValue_text, panel.QiYun_InsBtn, panel.QiYunValue_text);

            var rebirth = panel.ChongShengBtn;
            rebirth.transform.SetParent(content, false);
            Height((RectTransform)rebirth.transform, 38);
            var label = rebirth.GetComponentInChildren<Text>(true);
            label.text = "角色重生";
            Fill(label.rectTransform, 1, 2);
            Style(label, 13, new Color32(118, 75, 61, 255), TextAnchor.MiddleRight);
            Touch(rebirth);
        }

        private static RectTransform PrepareRow(Button button, Text value, RectTransform parent, out Text name)
        {
            var row = (RectTransform)button.transform.parent;
            row.SetParent(parent, false);
            // Existing direct siblings are in bundle order: name, value (or value backing), help.
            name = row.GetChild(0).GetComponent<Text>();
            name.text = name.text.TrimEnd('：', ':');
            if (value.transform.parent != row)
            {
                var backing = value.transform.parent.gameObject;
                value.transform.SetParent(row, false);
                backing.SetActive(false);
            }
            return row;
        }

        private void Money(RectTransform pair, Button button, Text value, int index, string title, string icon)
        {
            var rect = PrepareRow(button, value, pair, out var label);
            var surface = rect.gameObject.AddComponent<Image>();
            Flat(surface, Card);
            var picture = RoleFolio.Image(rect, title + "图", Color.white);
            InkInterfaceArt.Set(picture, icon);
            moneyLabels[index] = label;
            moneyIcons[index] = picture.rectTransform;
            label.text = title;
            Style(label, 12, Secondary, TextAnchor.MiddleLeft);
            Style(value, 16, Ink, TextAnchor.MiddleLeft, true, 10);
            Widen(button, rect);
        }

        internal void FitWallet()
        {
            // Keep long balances together as a single number even on compact screens.
            bool stacked = Mathf.Max(panel.LingShiValue_text.text.Length, panel.LingYuValue_text.text.Length) > 9;
            if (balancesStacked == stacked) return;
            balancesStacked = stacked;
            Height(wallet, stacked ? 80 : 54);
            for (int i = 0; i < 2; i++)
            {
                var value = i == 0 ? panel.LingShiValue_text : panel.LingYuValue_text;
                var card = (RectTransform)value.transform.parent;
                if (stacked)
                {
                    Band(card, 0, 0, i * 42, 38);
                    Fixed(moneyIcons[i], 0, 5, 6, 24, 25);
                    Fixed(moneyLabels[i].rectTransform, 0, 33, 0, 32, 38);
                    value.alignment = TextAnchor.MiddleRight;
                    // Match one line of type so best-fit never breaks the balance into two lines.
                    Band(value.rectTransform, 65, 7, 11, 17);
                }
                else
                {
                    Span(card, i * .5f, (i + 1) * .5f, i == 0 ? 0 : 3, i == 0 ? 3 : 0);
                    Fixed(moneyIcons[i], 0, 5, 12, 27, 29);
                    Band(moneyLabels[i].rectTransform, 36, 3, 3, 20);
                    Band(value.rectTransform, 36, 4, 23, 24);
                    value.alignment = TextAnchor.MiddleLeft;
                }
            }
        }

        private void Detail(RectTransform content, Button button, Text value, string title)
        {
            var row = PrepareRow(button, value, content, out var label);
            // These authored rows started in this content; explicitly place them after the section title.
            row.SetAsLastSibling();
            Height(row, 32);
            label.text = title;
            Style(label, 14, Secondary, TextAnchor.MiddleLeft);
            Fixed(label.rectTransform, 0, 1, 0, 48, 32);
            Fill(value.rectTransform, 51, 17);
            Style(value, 15, Ink, TextAnchor.MiddleRight, false, 10);
            var cue = Label(row, font, "›", 15, Secondary, TextAnchor.MiddleRight);
            Fixed(cue.rectTransform, 1, 0, 0, 12, 32);
            Band(RoleFolio.Image(row, "行线", Rule).rectTransform, 0, 0, 31, .6f);
            Widen(button, row);
        }

        private static void Pair(RectTransform content, Button left, Text leftValue, Button right, Text rightValue)
        {
            var pair = Box(content, "修为数值");
            Height(pair, 40);
            Stat(pair, left, leftValue, 0);
            Stat(pair, right, rightValue, 1);
        }

        private static void Stat(RectTransform pair, Button button, Text value, int index)
        {
            var row = PrepareRow(button, value, pair, out var label);
            Span(row, .5f * index, .5f * (index + 1), index == 0 ? 0 : 5, index == 0 ? 5 : 0);
            Style(label, 12, Secondary, TextAnchor.MiddleLeft);
            Band(label.rectTransform, 1, 0, 0, 16);
            Style(value, 15, Ink, TextAnchor.MiddleLeft, true, 11);
            Band(value.rectTransform, 1, 0, 15, 23);
            Band(RoleFolio.Image(row, "行线", Rule).rectTransform, 0, 0, 39, .6f);
            Widen(button, row);
        }

        private static void Widen(Button button, RectTransform parent)
        {
            button.transform.SetParent(parent, false);
            button.transform.SetAsLastSibling();
            Fill((RectTransform)button.transform);
            Touch(button);
        }

        private void AddTab(RectTransform strip, int index, string name)
        {
            var hit = RoleFolio.Image(strip, name, Color.clear, true);
            Span(hit.rectTransform, index * .5f, (index + 1) * .5f);
            var button = hit.gameObject.AddComponent<Button>();
            button.targetGraphic = hit;
            Touch(button);
            button.onClick.AddListener(() => SelectTab(index));
            var t = Label(hit.transform, font, name, 16, Secondary, TextAnchor.MiddleCenter);
            Fill(t.rectTransform);
            var line = RoleFolio.Image(hit.transform, "墨线", Jade);
            Band(line.rectTransform, 10, 10, 32, 2);
            tabs[index] = t;
            tabRules[index] = line;
        }

        private void SelectTab(int index)
        {
            stats.gameObject.SetActive(index == 0);
            professions.gameObject.SetActive(index == 1);
            for (int i = 0; i < tabs.Length; i++)
            {
                bool active = i == index;
                tabs[i].fontStyle = active ? FontStyle.Bold : FontStyle.Normal;
                tabs[i].color = active ? Jade : Secondary;
                tabRules[i].enabled = active;
            }
        }

        private void FooterButton(RectTransform footer, Button button, int index, string title, string art)
        {
            var icon = button.image.sprite;
            button.transform.SetParent(footer, false);
            var rect = (RectTransform)button.transform;
            Span(rect, index / 3f, (index + 1) / 3f);
            Touch(button);
            var pict = RoleFolio.Image(rect, title + "图", Color.white);
            if (art != null)
                InkInterfaceArt.Set(pict, art);
            else
            {
                pict.sprite = icon;
                pict.preserveAspect = true;
                pict.color = Jade;
            }
            Fixed(pict.rectTransform, .5f, 0, 6, 23, 23);
            var label = button.GetComponentInChildren<Text>(true);
            label.text = title;
            Style(label, 13, Ink, TextAnchor.MiddleCenter);
            Band(label.rectTransform, 0, 0, 31, 22);
        }

        internal void SetProfessionCount(int count)
        {
            empty.gameObject.SetActive(count == 0);
            panel.FuZhiContent.gameObject.SetActive(count > 0);
        }

        internal void SetOpen(bool show, bool animate)
        {
            if (animate && open == show) return;
            open = show;
            sheet.DOKill();
            if (show)
            {
                panel.transform.SetAsLastSibling();
                panel.MaskPanel.gameObject.SetActive(true);
                if (!sheet.gameObject.activeSelf)
                {
                    sheet.gameObject.SetActive(true);
                    sheet.anchoredPosition = new Vector2(-sheet.rect.width, 0);
                }
            }
            float x = show ? 0 : -sheet.rect.width;
            if (!animate)
            {
                sheet.anchoredPosition = new Vector2(x, 0);
                sheet.gameObject.SetActive(show);
                panel.MaskPanel.gameObject.SetActive(show);
                return;
            }
            var slide = sheet.DOAnchorPosX(x, .22f).SetUpdate(true).SetEase(Ease.OutCubic);
            if (!show)
                slide.OnComplete(() =>
                {
                    sheet.gameObject.SetActive(false);
                    panel.MaskPanel.gameObject.SetActive(false);
                });
        }
    }
}
