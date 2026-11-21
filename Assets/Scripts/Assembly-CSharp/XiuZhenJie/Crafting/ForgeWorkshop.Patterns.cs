using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class ForgeWorkshop
    {
        [Serializable] private class Pattern
        {
            public string id, name, blankId, grade, element, quality, recipe, traits, material, effect;
            public int gradeId, attack, defense, difficulty, balance, slotType;
            public int[] heatTargets;
            public string difficultyName, difficultyText;
            public bool claimed;
            public Ingredient[] inputs;
        }

        [Serializable] private class SampleRequest { public string requestId, templateId; }

        private Button patternButton;
        private Pattern sourcePattern;
        private bool openLibraryOnLoad;
        private int patternIndex;
        private int patternSlot;

        internal static string ForgeSlotName(int kind)
        {
            switch (kind)
            {
                case 16: return "护身";
                case 26: return "衣甲";
                case 29: return "足部";
                default: return "主手";
            }
        }

        internal static void AttachLibraryEntry(Transform header, Transform page, Action onClose)
        {
            var font = header.GetComponentInChildren<Text>().font;
            var plate = InkBattleHud.Make<Image>(header, "ArmoryEntry");
            InkInterfaceArt.Set(plate, "JadeButton", false);
            var rect = plate.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(1, .5f);
            rect.pivot = new Vector2(1, .5f);
            rect.anchoredPosition = new Vector2(-42, 0);
            rect.sizeDelta = new Vector2(76, 30);
            var button = plate.gameObject.AddComponent<Button>();
            button.targetGraphic = plate;
            var caption = InkBattleHud.Label(rect, font, "Caption", "炼器图录", 14, InkDialogArt.Ivory);
            caption.alignment = TextAnchor.MiddleCenter;
            InkDialogArt.Inset(caption.rectTransform, 2, 2, 0, 0);
            button.onClick.AddListener(() => OpenIn(page, font, onClose, libraryFirst: true));
        }

        private void BuildPatternLibrary()
        {
            patternButton = MakeButton(transform, "PatternLibrary", "器图", OpenPatterns);
            Right((RectTransform)patternButton.transform, 143, 5, 54, 28);
        }

        private void UpdatePatternLibrary()
        {
            bool available = catalog?.patterns != null && catalog.patterns.Length > 0;
            patternButton.gameObject.SetActive(available);
            patternButton.interactable = available && !Locked;
            if (openLibraryOnLoad && available && !Locked)
            {
                openLibraryOnLoad = false;
                OpenPatterns();
            }
        }

        private void OpenPatterns()
        {
            if (Locked || catalog?.patterns == null || catalog.patterns.Length == 0 ||
                transform.Find("PatternBook") != null) return;
            var sheet = Node("PatternBook", transform);
            Stretch(sheet);
            InkInterfaceArt.Set(sheet.gameObject.AddComponent<Image>(), "Parchment", false);
            var title = Label(sheet, "Title", "协会炼器图录", 22);
            title.fontStyle = FontStyle.Bold;
            Top(title.rectTransform, 12, 65, 6, 34);
            Right((RectTransform)MakeButton(sheet, "Back", "返回", () => Destroy(sheet.gameObject)).transform, 8, 7, 54, 30);

            var scrollArea = Node("PatternList", sheet);
            Top(scrollArea, 12, 12, 85, 100);
            scrollArea.gameObject.AddComponent<Image>().color = InkDialogArt.Ivory;
            scrollArea.gameObject.AddComponent<RectMask2D>();
            var scroll = scrollArea.gameObject.AddComponent<ScrollRect>();
            var list = Node("List", scrollArea);
            list.anchorMin = new Vector2(0, 1);
            list.anchorMax = list.pivot = Vector2.one;
            list.anchoredPosition = Vector2.zero;
            scroll.viewport = scrollArea;
            scroll.content = list;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;

            var art = Node("Artwork", sheet).gameObject.AddComponent<Image>();
            Left(art.rectTransform, 12, 192, 48, 48);
            art.preserveAspect = true;
            art.raycastTarget = false;
            var name = Label(sheet, "SelectedPattern", "", 20);
            name.fontStyle = FontStyle.Bold;
            Top(name.rectTransform, 67, 8, 190, 31);
            var stats = Label(sheet, "WeaponStats", "", 14);
            Top(stats.rectTransform, 67, 8, 217, 25);
            var detail = ReportScroll(sheet, "PatternDetails", 245, 50);
            var sample = MakeButton(sheet, "ClaimSample", "领用样品", () =>
            {
                var pattern = catalog.patterns[patternIndex];
                Destroy(sheet.gameObject);
                var request = new SampleRequest { requestId = Guid.NewGuid().ToString("N"), templateId = pattern.id };
                SaveAndSend(MsgType.ForgeClaimSample, JsonUtility.ToJson(request));
            });
            var cast = MakeButton(sheet, "UsePattern", "依图铸造", () =>
            {
                var p = catalog.patterns[patternIndex];
                SelectMold(new Mold { id = p.id, name = p.name, blankId = p.blankId, gradeId = p.gradeId, quality = p.quality, inputs = p.inputs, difficulty = p.difficulty, difficultyName = p.difficultyName, difficultyText = p.difficultyText });
                Destroy(sheet.gameObject);
            });
            var trace = MakeButton(sheet, "TracePattern", "临图首炼", () =>
            {
                var pattern = catalog.patterns[patternIndex];
                SelectMold(null);
                sourcePattern = pattern;
                blank = Mathf.Max(0, Array.FindIndex(catalog.blanks, b => b.id == pattern.blankId));
                grade = Mathf.Max(0, Array.FindIndex(catalog.grades, g => g.id == pattern.gradeId));
                foreach (var input in pattern.inputs ?? Array.Empty<Ingredient>()) picked[input.templateId] = input.quantity;
                workName.text = pattern.name;
                Destroy(sheet.gameObject);
                UpdateControls();
            });
            Footer((RectTransform)sample.transform, 0, 1f/3);
            Footer((RectTransform)trace.transform, 1f/3, 2f/3);
            Footer((RectTransform)cast.transform, 2f/3, 1);
            InkDialogArt.StyleButton(trace, "JadeButton", InkDialogArt.Ivory);
            var buttons = new Button[catalog.patterns.Length];
            void SelectPattern(int index)
            {
                patternIndex = index;
                var p = catalog.patterns[index];
                name.text = p.name + " · " + p.grade;
                stats.text = $"{ForgeSlotName(p.slotType)}　{p.element}行　攻 {p.attack}　防 {p.defense}";
                art.sprite = InkInterfaceArt.Get("Goods/" + PatternArt(p.blankId));
                string tendency = p.balance > 0 ? "偏攻" : p.balance < 0 ? "偏守" : "均衡";
                detail.text = Difficulty(p.difficulty, p.difficultyName) + ExtraLine(p.difficultyText)
                    + (p.difficulty > 0 ? $"\n临图首炼　{tendency}，落锤可微调" + (p.difficulty > 1 ? $" · 成器额外经验 +{2 * (p.difficulty - 1)}" : "") : "")
                    + "\n\n" + $"每件用料　{p.recipe}\n\n器物词条　{p.traits}\n材料词条　{p.material}\n器印　{p.effect}";
                sample.interactable = !p.claimed;
                sample.GetComponentInChildren<Text>().text = p.claimed ? "样品已领" : "领用样品";
                for (int i = 0; i < buttons.Length; i++)
                    InkDialogArt.StyleButton(buttons[i], i == index ? "JadeButton" : "LightPlaque", i == index ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i] = MakeButton(list, "Pattern_" + i, catalog.patterns[i].name + "　" + catalog.patterns[i].difficultyName, () => SelectPattern(index));
            }
            int[] slots = { 0, 15, 26, 16, 29 };
            var tabs = new Button[slots.Length];
            int Slot(Pattern p) => p.slotType == 0 ? 15 : p.slotType;
            void Filter(int slot)
            {
                patternSlot = slot;
                int count = 0, first = -1, selectedRow = 0;
                for (int i = 0; i < buttons.Length; i++)
                {
                    bool visible = slot == 0 || Slot(catalog.patterns[i]) == slot;
                    buttons[i].gameObject.SetActive(visible);
                    if (!visible) continue;
                    if (first < 0) first = i;
                    if (i == patternIndex) selectedRow = count / 2;
                    int col = count % 2;
                    HalfTop((RectTransform)buttons[i].transform, col * .5f, (col + 1) * .5f, count / 2 * 40, 36);
                    count++;
                }
                for (int i = 0; i < tabs.Length; i++)
                    InkDialogArt.StyleButton(tabs[i], slots[i] == slot ? "JadeButton" : "LightPlaque",
                        slots[i] == slot ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
                int rows = (count + 1) / 2;
                list.sizeDelta = new Vector2(0, rows * 40);
                SelectPattern(patternIndex >= 0 && patternIndex < buttons.Length && buttons[patternIndex].gameObject.activeSelf ? patternIndex : first);
                scroll.StopMovement();
                Canvas.ForceUpdateCanvases();
                scroll.verticalNormalizedPosition = rows < 3 ? 1 : 1 - Mathf.Clamp01((selectedRow * 40f - 30f) / (rows * 40 - 100));
            }
            for (int i = 0; i < slots.Length; i++)
            {
                int slot = slots[i];
                tabs[i] = MakeButton(sheet, "PatternSlot_" + slot, slot == 0 ? "全部" : ForgeSlotName(slot), () => Filter(slot));
                HalfTop((RectTransform)tabs[i].transform, i / 5f, (i + 1) / 5f, 45, 34);
                tabs[i].interactable = slot == 0 || Array.Exists(catalog.patterns, p => Slot(p) == slot);
            }
            Filter(Array.Exists(catalog.patterns, p => Slot(p) == patternSlot) ? patternSlot : 0);
        }

        internal static string PatternArt(string blank)
        {
            switch (blank)
            {
                case "wp.minglei_spear": return "Spear";
                case "wp.zhufeng_bow": return "Bow";
                case "wp.tingquan_staff": return "Staff";
                case "wp.duanxia_saber": return "Saber";
                case "obj.hanguang_mirror": return "Compass";
                case "obj.yunwen_robe": return "Robe";
                case "obj.tafeng_boots": return "Boots";
                default: return "Sword";
            }
        }
    }
}
