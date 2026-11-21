using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static XiuZhenJie.CaveGradeArt;

namespace XiuZhenJie
{
    internal static class CaveGradeArt
    {
        internal static readonly Color[] SealColors =
        {
            new Color32(248, 240, 210, 255), new Color32(244, 190, 112, 255), new Color32(135, 213, 238, 255),
            new Color32(160, 226, 164, 255), new Color32(235, 201, 255, 255)
        };
        internal static readonly string[] Grades = { "普通", "黄阶", "玄阶", "地阶", "天阶" };
        internal static readonly string[] Motifs = { "静室", "流萤", "水镜", "翠脉", "星宿" };
        internal static int BoxCapacity(int grade) => 200 + 50 * grade;
    }

    internal sealed class CaveChamberView
    {
        private static readonly Color Night = new Color32(23, 40, 39, 255);
        private static readonly Color Wood = new Color32(44, 58, 50, 255);
        private static readonly Color Jade = new Color32(183, 205, 186, 255);
        private static readonly Color Ivory = new Color32(248, 240, 210, 255);
        private static readonly Color Brass = new Color32(213, 178, 116, 255);
        private static readonly Rect[] ChestDetails =
        {
            new Rect(.002f, .342f, .124f, .221f), new Rect(.078f, .222f, .152f, .211f),
            new Rect(.847f, .465f, .104f, .197f), new Rect(.921f, .448f, .079f, .139f),
            new Rect(.941f, .423f, .059f, .105f)
        };

        private readonly DongFuMiShiWnd owner;
        private readonly RectTransform root, content, renameSheet;
        private readonly Font font;
        private readonly Sprite scene;
        private readonly Sprite plaque;
        private readonly CaveChamberEffects effects;
        private readonly ScrollRect scroll;
        private readonly Text stageName, currentName, capacity, conversion, shelfCount, shelfNote, buyLabel, renameTitle;
        private readonly InputField nameInput;
        private readonly Button currentButton, buy, retry, submit, cancel;
        private readonly Button[] gradeButtons = new Button[4], openBoxes = new Button[5], renameBoxes = new Button[5];
        private readonly Image[] seals = new Image[4];
        private readonly Text[] boxTitles = new Text[5];
        private readonly RectTransform[] rows = new RectTransform[5];
        private readonly Button firstChest;
        private readonly Dictionary<string, WorldHomeModel> gradeTable;
        private int renameNumber, shownGrade = -1;
        private bool scrollToNewest;

        internal CaveChamberView(DongFuMiShiWnd owner, DongFuMiShiPanel panel)
        {
            this.owner = owner;
            font = panel.CangKu_1_Name.font;
            gradeTable = Singleton<FacadeManager>.Instance.SelectDongFuLevelInfo();
            scene = InkCultivationInterface.Get("VaultSanctum");
            plaque = ReleasedResources.Load<Sprite>("InkInterface/InkPlaque");

            foreach (Transform child in panel.transform) child.gameObject.SetActive(false);
            var panelRect = (RectTransform)panel.transform;
            RoleFolio.Flat(panel.GetComponent<Image>(), Night, true);
            root = RoleFolio.Box(panelRect, "VaultInterior");
            RoleFolio.Fill(root);

            var head = Band(root, "VaultLintel", 0, 42);
            FillImage(head, Night);
            var back = Button(Fixed(head, "ReturnToResidence", 0, 4, 2, 61, 38), owner.Close, Wood);
            Caption(back.transform, "‹  洞府", 16, Ivory, TextAnchor.MiddleCenter);
            Caption(head, "藏珍密室", 22, Ivory, TextAnchor.MiddleCenter, true, 69, 83);
            var upgrade = Button(Fixed(head, "UpgradeResidence", 1, -4, 2, 82, 38), owner.Upgrade, Wood);
            Caption(upgrade.transform, "晋升洞府", 15, Brass, TextAnchor.MiddleCenter);

            var stage = Band(root, "VaultStage", 42, 156);
            stage.gameObject.AddComponent<RectMask2D>();
            var mural = RoleFolio.Image(stage, "VaultSanctum", Color.white);
            mural.sprite = scene;
            RoleFolio.Fill(mural.rectTransform);
            if (scene != null)
            {
                var fit = mural.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = scene.rect.width / scene.rect.height;
            }
            var fx = RoleFolio.Box(mural.transform, "VaultEffects");
            RoleFolio.Fill(fx);
            effects = fx.gameObject.AddComponent<CaveChamberEffects>();
            effects.raycastTarget = false;
            // The active targets follow the painted floor seal and the western chest through cropping.
            var disc = RoleFolio.Box(mural.transform, "StirFormation");
            Place(disc, .40f, .20f, .81f, .52f);
            Button(disc, Pulse, Color.clear, false);
            var chest = RoleFolio.Box(mural.transform, "PaintedChest");
            Place(chest, .006f, .22f, .236f, .56f);
            firstChest = Button(chest, () => owner.OpenBox(1), Color.clear, false);
            var badge = Fixed(stage, "GradePlaque", 0, 7, 6, 114, 32);
            FillImage(badge, new Color32(208, 218, 195, 247), plaque);
            stageName = Caption(badge, "", 17, Brass, TextAnchor.MiddleCenter, true);
            currentButton = Button(Fixed(stage, "UseCurrentGrade", 1, -6, 6, 116, 32),
                owner.ReturnToCurrent, new Color32(30, 48, 42, 245));
            currentName = Caption(currentButton.transform, "", 13, Ivory, TextAnchor.MiddleCenter);

            var choices = Band(root, "GradeSeals", 199, 52, 4, 4);
            for (int i = 0; i < gradeButtons.Length; i++)
            {
                int grade = i + 1;
                var tile = RoleFolio.Box(choices, "Grade" + grade);
                RoleFolio.Span(tile, i / 4f, grade / 4f, 2, 2);
                gradeButtons[i] = Button(tile, () => owner.Preview(grade), Wood);
                var ring = RoleFolio.Image(tile, "FormationSeal", Color.white);
                ring.sprite = InkCultivationInterface.Get("CaveGrade" + grade);
                RoleFolio.Fill(ring.rectTransform, 3, 3, 2, 3);
                ring.preserveAspect = true;
                var text = Caption(tile, Grades[grade], 16, SealColors[grade], TextAnchor.MiddleCenter, true);
                text.gameObject.AddComponent<Shadow>().effectColor = Night;
                seals[i] = RoleFolio.Image(tile, "ActiveSeal", SealColors[grade]);
                Place(seals[i].rectTransform, .18f, .035f, .82f, .085f);
            }
            var readout = Band(root, "GradeBenefits", 254, 43, 8, 8);
            var left = RoleFolio.Box(readout, "Capacity"); RoleFolio.Span(left, 0, .55f);
            var right = RoleFolio.Box(readout, "Conversion"); RoleFolio.Span(right, .55f, 1, 4);
            capacity = Measure(left, "单箱容量");
            conversion = Measure(right, "修炼灵气转化");
            var rule = RoleFolio.Image(root, "BrassDivider", new Color32(153, 132, 91, 255));
            RoleFolio.Band(rule.rectTransform, 8, 8, 300, 1);

            var shelf = Band(root, "BoxShelf", 302, 33, 8, 8);
            Caption(shelf, "芥子储物箱", 18, Ivory, TextAnchor.MiddleLeft, true, 4, 115);
            shelfCount = Caption(shelf, "", 13, Jade, TextAnchor.MiddleRight, false, 106, 4);
            var viewport = RoleFolio.Box(root, "BoxViewport");
            RoleFolio.Fill(viewport, 8, 8, 337, 49);
            FillImage(viewport, new Color32(16, 32, 31, 255), null, true);
            viewport.gameObject.AddComponent<RectMask2D>();
            if (scene != null)
            {
                // The empty shelf is the western cache from the same mural, behind every usable row.
                var carving = RoleFolio.Box(viewport, "WestCabinetInlay").gameObject.AddComponent<RawImage>();
                carving.texture = scene.texture;
                carving.uvRect = new Rect(0f, .16f, .35f, .42f);
                carving.color = new Color32(170, 177, 153, 179);
                carving.raycastTarget = false;
                RoleFolio.Fill(carving.rectTransform);
                var fit = carving.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = scene.rect.width * carving.uvRect.width / (scene.rect.height * carving.uvRect.height);
                var lip = RoleFolio.Image(viewport, "CabinetFoot", new Color32(186, 153, 96, 165));
                lip.rectTransform.anchorMin = Vector2.zero;
                lip.rectTransform.anchorMax = Vector2.right;
                lip.rectTransform.offsetMin = new Vector2(3, 2);
                lip.rectTransform.offsetMax = new Vector2(-3, 3);
            }
            scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            content = RoleFolio.Box(viewport, "Boxes");
            content.pivot = new Vector2(.5f, 1f);
            RoleFolio.Band(content, 0, 0, 0, 0);
            scroll.content = content;
            for (int i = 0; i < 5; i++) BuildBox(i);
            shelfNote = Caption(viewport, "", 15, Jade, TextAnchor.MiddleCenter);
            var foot = Band(root, "VaultFoot", 0, 40, 8, 8);
            foot.anchorMin = Vector2.zero;
            foot.anchorMax = Vector2.right;
            foot.offsetMin = new Vector2(8, 5);
            foot.offsetMax = new Vector2(-8, 45);
            buy = Button(foot, owner.AskBuy, Wood);
            buyLabel = Caption(buy.transform, "", 16, Ivory, TextAnchor.MiddleCenter, true);
            retry = Button(Fixed(shelf, "Retry", 1, 0, 0, 92, 33), owner.Load, Wood);
            Caption(retry.transform, "重新读取", 14, Brass, TextAnchor.MiddleCenter);
            foot.name = "ExpandBox";

            renameSheet = RoleFolio.Box(root, "RenameSheet");
            RoleFolio.Fill(renameSheet);
            FillImage(renameSheet, new Color32(7, 17, 17, 214), null, true);
            var paper = RoleFolio.Box(renameSheet, "NameTablet");
            paper.anchorMin = new Vector2(.055f, .5f);
            paper.anchorMax = new Vector2(.945f, .5f);
            paper.sizeDelta = new Vector2(0, 176);
            paper.anchoredPosition = Vector2.zero;
            FillImage(paper, Wood);
            renameTitle = Caption(Band(paper, "RenameTitle", 7, 33), "", 20, Ivory, TextAnchor.MiddleCenter, true);
            var input = Band(paper, "NewName", 47, 43, 16, 16);
            var field = FillImage(input, new Color32(239, 231, 203, 255), null, true);
            nameInput = input.gameObject.AddComponent<InputField>();
            nameInput.targetGraphic = field;
            nameInput.textComponent = Caption(input, "", 17, Night, TextAnchor.MiddleLeft, false, 9, 9);
            nameInput.characterLimit = 20;
            nameInput.lineType = InputField.LineType.SingleLine;
            Caption(Band(paper, "Fee", 91, 25), "改名消耗 5 灵玉", 13, Brass, TextAnchor.MiddleCenter);
            var actions = Band(paper, "RenameActions", 122, 42, 14, 14);
            var cancelArea = RoleFolio.Box(actions, "CancelRename"); RoleFolio.Span(cancelArea, 0, .44f, 0, 3);
            cancel = Button(cancelArea, CancelRename, Night);
            Caption(cancelArea, "取消", 16, Ivory, TextAnchor.MiddleCenter);
            var submitArea = RoleFolio.Box(actions, "ConfirmRename"); RoleFolio.Span(submitArea, .44f, 1, 3);
            submit = Button(submitArea, () => owner.Rename(renameNumber, nameInput.text), Night);
            Caption(submitArea, "确认改名", 16, Brass, TextAnchor.MiddleCenter, true);
            CancelRename();
        }

        private void BuildBox(int index)
        {
            int number = index + 1;
            RectTransform row = rows[index] = Band(content, "Shelf" + number, 54 * index, 51);
            FillImage(row, Wood);
            var open = RoleFolio.Box(row, "Box" + number);
            RoleFolio.Fill(open, 0, 62);
            openBoxes[index] = Button(open, () => owner.OpenBox(number), Color.clear, false);
            if (scene != null)
            {
                var image = Fixed(open, "ChestDetail", 0, 3, 4, 52, 43).gameObject.AddComponent<RawImage>();
                image.texture = scene.texture;
                image.uvRect = ChestDetails[index];
                image.raycastTarget = false;
            }
            var name = Band(open, "BoxName", 2, 27, 61, 0);
            boxTitles[index] = Caption(name, "", 17, Ivory, TextAnchor.MiddleLeft, true);
            Caption(Band(open, "BoxNumber", 29, 20, 61), "第" + number + "箱  ·  存入 / 取出", 12, Jade, TextAnchor.MiddleLeft);
            var tag = Fixed(row, "Rename" + number, 1, -2, 5, 57, 41);
            renameBoxes[index] = Button(tag, () => owner.AskRename(number), Night);
            Caption(tag, "题名", 14, Brass, TextAnchor.MiddleCenter);
        }

        internal void ShowGrade(int actual, int selected)
        {
            bool preview = actual != selected;
            stageName.text = Grades[selected] + " · " + Motifs[selected];
            stageName.color = SealColors[selected];
            currentName.text = actual < 0 ? "品阶读取中" : preview ? "返回当前 · " + Grades[actual] : "当前生效 · " + Grades[actual];
            // Keep the badge legible when it serves as a non-interactive status.
            currentButton.enabled = preview && actual >= 0;
            capacity.text = BoxCapacity(selected) + " 立方尺" + (preview ? " · 预览" : "");
            conversion.text = gradeTable != null && gradeTable.TryGetValue(selected.ToString(), out var home)
                ? ((double)home.conversionRate).ToString("0%") : "—";
            if (shownGrade == selected) return;
            shownGrade = selected;
            for (int i = 0; i < gradeButtons.Length; i++)
            {
                bool active = i + 1 == selected;
                seals[i].gameObject.SetActive(active);
                gradeButtons[i].image.color = active ? Color.Lerp(Color.white, SealColors[i + 1], .53f) : new Color32(165, 178, 163, 255);
            }
            effects.SetGrade(selected);
        }

        internal void ShowBoxes(IList<string> names, bool loaded, bool busy, string price, string status, bool retryable, string rankError)
        {
            int count = loaded ? names.Count : 0;
            shelfCount.gameObject.SetActive(!retryable);
            shelfCount.text = loaded ? "已启用  " + count + " / 5" : "";
            shelfNote.text = status ?? "";
            shelfNote.gameObject.SetActive(count == 0);
            if (rankError != null) currentName.text = rankError;
            retry.gameObject.SetActive(retryable && !busy);
            for (int i = 0; i < 5; i++)
            {
                rows[i].gameObject.SetActive(i < count);
                if (i >= count) continue;
                boxTitles[i].text = names[i];
                openBoxes[i].interactable = renameBoxes[i].interactable = !busy;
            }
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 54 * count);
            if (scrollToNewest)
            {
                scroll.verticalNormalizedPosition = 0;
                scrollToNewest = false;
            }
            firstChest.interactable = count > 0 && !busy;
            buy.interactable = loaded && !busy && count < 5;
            buyLabel.text = busy ? "正在核对箱册…" : count >= 5 ? "五箱俱全" :
                string.IsNullOrEmpty(price) ? "增置储物箱" : "增置第" + (count + 1) + "箱    ·    " + price;
            cancel.interactable = submit.interactable = nameInput.interactable = !busy;
        }

        internal void ScrollToNewest() => scrollToNewest = true;
        internal void Pulse() => effects.PlayPulse();
        internal void BeginRename(int number, string value)
        {
            renameNumber = number;
            renameTitle.text = "为第" + number + "箱题名";
            nameInput.text = value;
            renameSheet.gameObject.SetActive(true);
            nameInput.ActivateInputField();
        }
        internal void CancelRename() => renameSheet.gameObject.SetActive(false);

        private Button Button(RectTransform area, UnityAction action, Color color, bool textured = true)
        {
            var face = FillImage(area, textured ? Color.Lerp(color, Color.white, .72f) : color, textured ? plaque : null, true);
            var button = area.gameObject.AddComponent<Button>();
            button.targetGraphic = face;
            owner.AddButtonClickListener(button, action);
            return button;
        }

        private Text Measure(RectTransform area, string title)
        {
            Caption(Band(area, "MeasureTitle", 0, 17), title, 12, Jade, TextAnchor.MiddleLeft);
            return Caption(Band(area, "MeasureValue", 17, 26), "", 17, Ivory, TextAnchor.MiddleLeft, true);
        }

        private Text Caption(Transform parent, string text, int size, Color color, TextAnchor align, bool bold = false,
            float left = 1, float right = 1)
        {
            var label = RoleFolio.Label(parent, font, text, size, color, align, bold);
            RoleFolio.Style(label, size, color, align, bold, Mathf.Min(12, size));
            RoleFolio.Fill(label.rectTransform, left, right);
            return label;
        }

        private static Image FillImage(RectTransform area, Color color, Sprite art = null, bool hit = false)
        {
            var face = area.gameObject.AddComponent<Image>();
            face.sprite = art;
            face.color = color;
            face.raycastTarget = hit;
            return face;
        }

        private static RectTransform Band(Transform parent, string name, float top, float height, float left = 0, float right = 0)
        {
            var area = RoleFolio.Box(parent, name);
            RoleFolio.Band(area, left, right, top, height);
            return area;
        }

        private static RectTransform Fixed(Transform parent, string name, float side, float dx, float top, float width, float height)
        {
            var area = RoleFolio.Box(parent, name);
            RoleFolio.Fixed(area, side, dx, top, width, height);
            return area;
        }

        private static void Place(RectTransform area, float x1, float y1, float x2, float y2)
        {
            area.anchorMin = new Vector2(x1, y1);
            area.anchorMax = new Vector2(x2, y2);
            area.offsetMin = area.offsetMax = Vector2.zero;
        }
    }
}
