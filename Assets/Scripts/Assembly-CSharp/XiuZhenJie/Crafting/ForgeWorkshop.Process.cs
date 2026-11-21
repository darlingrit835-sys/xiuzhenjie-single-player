using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class ForgeWorkshop
    {
        [Serializable] private class CastCost
        {
            public string name, relationText;
            public int perPiece, held, spent, saved, savingPercent;
        }

        private readonly string[] stepNames = { "器胚", "配料", "选火", "塑形", "炉析" };
        private readonly Button[] stepTabs = new Button[5];
        private int processStep;
        private Estimate firstEstimate;
        private Button nextStep, previousStep;
        private Image blankArt;
        private Text stepHeading, stepInfo;
        private RectTransform infoViewport;

        private static string ExtraLine(string value) => string.IsNullOrWhiteSpace(value) ? "" : "\n" + value;
        private static string Difficulty(int level, string name) =>
            level < 1 ? "" : $"器图难度　{level}/5 · {name}";

        private void BuildProcess()
        {
            for (int i = 0; i < stepNames.Length; i++)
            {
                int step = i;
                var button = MakeButton(transform, "Step" + i, stepNames[i], () => MoveStep(step));
                HalfTop((RectTransform)button.transform, i / 5f, (i + 1) / 5f, 44, 31);
                stepTabs[i] = button;
            }
            stepHeading = Label(transform, "StepHeading", "", 18);
            stepHeading.fontStyle = FontStyle.Bold;
            Top(stepHeading.rectTransform, 14, 10, 86, 29);
            blankArt = Node("BlankArtwork", transform).gameObject.AddComponent<Image>();
            Top(blankArt.rectTransform, 52, 52, 122, 108);
            blankArt.preserveAspect = true;
            blankArt.raycastTarget = false;
            stepInfo = ReportScroll((RectTransform)transform, "StepInfo", 185, 125);
            infoViewport = (RectTransform)stepInfo.transform.parent;
            previousStep = MakeButton(transform, "StepBack", "上一步", () => MoveStep(processStep - 1));
            nextStep = MakeButton(transform, "StepNext", "下一步", () => MoveStep(processStep + 1));
            Footer((RectTransform)previousStep.transform, 0, .37f);
            Footer((RectTransform)nextStep.transform, .37f, 1);
            InkDialogArt.StyleButton(nextStep, "JadeButton", InkDialogArt.Ivory);
        }

        private void MoveStep(int to)
        {
            if (Locked || catalog == null || activeMold != null) return;
            processStep = Mathf.Clamp(to, 0, stepNames.Length - 1);
            status.text = "";
            UpdateControls();
        }

        private string MissingIngredients()
        {
            return string.Join("、", catalog.materials.Where(m => picked.TryGetValue(m.id, out int n) && n > m.count)
                .Select(m => $"{m.name} 缺 {picked[m.id] - m.count} 份"));
        }

        private string IngredientsText()
        {
            return string.Join("、", catalog.materials.Where(m => picked.ContainsKey(m.id))
                .Select(m => $"{m.name} ×{picked[m.id]}"));
        }

        private void UpdateProcess()
        {
            bool first = activeMold == null;
            bool hasData = catalog?.blanks.Length > 0 && catalog.flames.Length > 0 && catalog.grades.Length > 0;
            bool shapeRequired = catalog != null && catalog.moldVersion > 0;
            bool blankPage = first && processStep == 0;
            bool materialsPage = first && processStep == 1;
            bool firePage = first && processStep == 2;
            bool shapePage = first && processStep == 3;
            bool lastPage = first && processStep == 4;
            bool shapeDone = !shapeRequired || strokes.Count == 4;
            string shortage = hasData && first ? MissingIngredients() : "";
            bool quoteReady = hasData && Total >= 2 && shapeDone && shortage.Length == 0;
            bool named = !string.IsNullOrWhiteSpace(workName.text);
            for (int i = 0; i < stepTabs.Length; i++)
            {
                var tab = stepTabs[i];
                tab.gameObject.SetActive(first);
                tab.interactable = hasData && !Locked;
                bool selected = i == processStep;
                InkDialogArt.StyleButton(tab, selected ? "JadeButton" : "LightPlaque", selected ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
            }
            stepHeading.gameObject.SetActive(first);
            infoViewport.gameObject.SetActive(first && !materialsPage);
            blankArt.gameObject.SetActive(first && (blankPage || shapePage) && hasData);
            bool tools = !first || blankPage;
            blankTitle.gameObject.SetActive(tools);
            previous.gameObject.SetActive(blankPage);
            next.gameObject.SetActive(blankPage);
            gradeButton.gameObject.SetActive(tools);
            flameButton.gameObject.SetActive(!first || firePage);
            training.gameObject.SetActive(!first);
            workName.gameObject.SetActive(blankPage);
            shapeButton.gameObject.SetActive(!first || (shapePage && shapeRequired));
            rows.parent.gameObject.SetActive(!first || materialsPage);
            materialTitle.gameObject.SetActive(!first || materialsPage);
            supply.gameObject.SetActive(!first || materialsPage);
            previousStep.gameObject.SetActive(first && processStep > 0 && !lastPage && pending == null);
            nextStep.gameObject.SetActive(first && !lastPage && pending == null && catalog != null);
            estimate.gameObject.SetActive(!first || lastPage);
            submit.gameObject.SetActive(!first || lastPage || pending != null || (catalog == null && !busy));
            if (!first)
            {
                Top((RectTransform)shapeButton.transform, 12, 12, 167, 30);
                HalfTop((RectTransform)flameButton.transform, 0, .5f, 79, 30);
                HalfTop((RectTransform)gradeButton.transform, .5f, 1f, 79, 30);
                Top(blankTitle.rectTransform, 12, 12, 38, 40);
                Top(materialTitle.rectTransform, 12, 12, 198, 25);
                SetMaterialsBounds(224, 133);
                Footer((RectTransform)supply.transform, 0, .4f);
                Footer((RectTransform)estimate.transform, .4f, .68f);
                Footer((RectTransform)submit.transform, .68f, 1);
                return;
            }

            previousStep.interactable = !Locked;
            nextStep.interactable = hasData && !Locked &&
                (!blankPage || named) && (!materialsPage || Total >= 2 && shortage.Length == 0) && (!shapePage || shapeDone);
            if (!lastPage) nextStep.GetComponentInChildren<Text>(true).text = "下一步 · " + stepNames[processStep + 1];
            Footer((RectTransform)nextStep.transform, processStep == 0 ? 0 : .37f, 1);
            if (materialsPage)
            {
                Left((RectTransform)supply.transform, 12, 118, 137, 31);
                Top(materialTitle.rectTransform, 154, 10, 118, 31);
                SetMaterialsBounds(157, 95);
            }
            if (blankPage)
            {
                Left((RectTransform)previous.transform, 12, 160, 42, 42);
                Right((RectTransform)next.transform, 12, 160, 42, 42);
                blankTitle.alignment = TextAnchor.MiddleCenter;
                Top(blankTitle.rectTransform, 15, 15, 231, 44);
                Top((RectTransform)gradeButton.transform, 12, 12, 277, 33);
                Top((RectTransform)workName.transform, 12, 12, 316, 35);
                PositionInfo(356, 83);
            }
            if (shapePage)
            {
                Top((RectTransform)shapeButton.transform, 12, 12, 236, 39);
                PositionInfo(282, 85);
            }
            if (firePage)
            {
                Top((RectTransform)flameButton.transform, 12, 12, 122, 46);
                PositionInfo(179, 85);
            }
            string[] headings = { "选择器胚与目标品阶", "按份投入材料", "选取本炉火焰", "上砧定形", "核对实投，试算开炉" };
            stepHeading.text = headings[processStep];
            if (!hasData)
            {
                stepInfo.text = ""; return;
            }
            var choice = catalog.blanks[blank];
            var fire = catalog.flames[flame];
            if (blankPage || shapePage) blankArt.sprite = InkInterfaceArt.Get("Goods/" + PatternArt(choice.id));
            if (blankPage) stepInfo.text = sourcePattern == null
                ? (choice.difficulty > 0 ? "器胚基础难度　" + choice.difficultyName + ExtraLine(choice.difficultyText) + "\n实投难度以炉析为准。" : "")
                : "临图 · " + sourcePattern.name + "\n" + Difficulty(sourcePattern.difficulty, sourcePattern.difficultyName) + ExtraLine(sourcePattern.difficultyText);
            if (firePage) stepInfo.text = "本炉火候" + ExtraLine(fire.controlText) + "\n" + Training(catalog.trainingText)
                + "\n\n师阶与火种相合时更容易控火。材料的五行也会改变成器率与返材率，可在炉析查看本次加减。";
            if (shapePage) stepInfo.text = shapeDone && shapeRequired
                ? $"{Quality()} · 合拍 {Hits()}/4 锤\n目标品阶上限 {Cap(Hits() >= 3 ? 0 : Hits() > 0 ? 1 : 2)}"
                : shapeRequired ? "左侧工位偏攻，右侧工位偏守，中间保留原图取向。\n看准青区下锤，合拍越多，模具能保留的品阶越高。" : "此工坊可直接试算本炉。";
            if (lastPage)
            {
                PositionInfo(123, 113);
                stepInfo.text = (sourcePattern == null ? "" : "临图 · " + sourcePattern.name + "\n") + $"{choice.name} · {catalog.grades[grade].name}\n器名　{workName.text}\n\n每炉实投　{IngredientsText()}\n炉火　{fire.name}\n"
                    + (shapeRequired ? $"塑形　{strokes.Count}/4 锤 · {Quality()}\n" : "")
                    + ExtraLine(Training(catalog.trainingText));
                Footer((RectTransform)estimate.transform, 0, .5f);
                Footer((RectTransform)submit.transform, .5f, 1);
                estimate.GetComponentInChildren<Text>(true).text = firstEstimate == null ? "试算本炉" : "查看炉析";
                estimate.interactable = !Locked && quoteReady;
                submit.interactable = !busy && (pending != null || quoteReady && named && firstEstimate != null);
            }
            if (!Locked)
            {
                preparation.text = blankPage && !named ? "请为这件作品命名。"
                    : (materialsPage || lastPage) && shortage.Length > 0 ? shortage
                    : materialsPage ? $"已选 {Total}/12 份，至少投入 2 份。"
                    : lastPage && !quoteReady ? (Total < 2 ? "返回配料，至少投入 2 份材料。" : "返回塑形，完成四锤。")
                    : lastPage && !named ? "返回器胚，为作品命名。"
                    : lastPage && firstEstimate == null ? "试算可查看成器率、炼器经验与败炉返材。" : "";
            }
        }

        private void PositionInfo(float top, float bottom)
        {
            infoViewport.offsetMax = new Vector2(-12, -top);
            infoViewport.offsetMin = new Vector2(12, bottom);
        }

        private void SetMaterialsBounds(float top, float bottom)
        {
            var area = (RectTransform)rows.parent;
            area.offsetMin = new Vector2(10, bottom);
            area.offsetMax = new Vector2(-10, -top);
        }

        private string Training(string detail) => string.IsNullOrEmpty(detail) ? catalog?.smithRankName : detail;

        private Text ReportScroll(RectTransform parent, string textName, int top, int bottom)
        {
            var view = Node("Viewport", parent);
            Stretch(view); view.offsetMax = new Vector2(-12, -top); view.offsetMin = new Vector2(12, bottom);
            view.gameObject.AddComponent<Image>().color = InkDialogArt.Ivory;
            view.gameObject.AddComponent<RectMask2D>();
            var scroll = view.gameObject.AddComponent<ScrollRect>();
            var text = Label(view, textName, "", 15);
            text.alignment = TextAnchor.UpperLeft;
            var rect = text.rectTransform;
            rect.anchorMin = new Vector2(0, 1); rect.anchorMax = Vector2.one; rect.pivot = new Vector2(.5f, 1);
            rect.sizeDelta = new Vector2(-16, 0); rect.anchoredPosition = Vector2.zero;
            text.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scroll.viewport = view; scroll.content = rect; scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            return text;
        }

        private RectTransform ReportSheet(string id, string heading, string body, bool footer)
        {
            var sheet = Node(id, transform); Stretch(sheet);
            InkInterfaceArt.Set(sheet.gameObject.AddComponent<Image>(), "Parchment", false);
            var title = Label(sheet, "Title", heading, 21);
            title.fontStyle = FontStyle.Bold;
            Top(title.rectTransform, 12, 12, 8, 35);
            ReportScroll(sheet, "Details", 51, footer ? 52 : 12).text = body;
            return sheet;
        }

        private static string Sections(params string[] sections) =>
            string.Join("\n\n", sections.Where(s => !string.IsNullOrWhiteSpace(s)));

        private static string Section(string title, string text) => string.IsNullOrWhiteSpace(text) ? "" : title + "\n" + text;

        private void ShowFirstQuote()
        {
            var q = firstEstimate;
            if (q == null || transform.Find("FirstQuote") != null) return;
            string materials = string.IsNullOrEmpty(q.costText) ? IngredientsText() : q.costText;
            string body = Sections($"成器率　{q.successBps / 100f:0.##}%　{q.gradeName} · {q.moldQuality}",
                Difficulty(q.difficulty, q.difficultyName) + ExtraLine(q.difficultyText),
                Section("实际投入", materials), Section("炉火与控火", q.flameName + ExtraLine(q.controlText) + ControlAdvice(q.controlText)),
                Section("修习与晋阶", Training(q.trainingText) +
                    (q.successExperience > 0 || q.failureExperience > 0 ? $"\n成器经验 +{q.successExperience}　败炉经验 +{q.failureExperience}" : "")),
                q.processText, Section("炉析", q.factorText));
            var sheet = ReportSheet("FirstQuote", "首炉试算", body, true);
            var back = MakeButton(sheet, "Back", "返回调整", () => Destroy(sheet.gameObject));
            var pour = MakeButton(sheet, "Create", "开炉留模", () =>
            {
                Destroy(sheet.gameObject);
                Submit();
            });
            pour.interactable = !string.IsNullOrWhiteSpace(workName.text);
            InkDialogArt.StyleButton(pour, "JadeButton", InkDialogArt.Ivory);
            Footer((RectTransform)back.transform, 0, .5f);
            Footer((RectTransform)pour.transform, .5f, 1);
        }

        private static string ControlAdvice(string control) =>
            control != null && control.Contains("越阶") ? "\n换用所需师阶较低的火种，可降低越阶损失。" : "";

        private static string CostBreakdown(Receipt q, bool finished = false)
        {
            if (q.costs == null || q.costs.Length == 0) return q.costText;
            return string.Join("\n", q.costs.Select(c =>
            {
                string stock = finished ? $"实耗 {c.spent}，炉后余 {Mathf.Max(0, c.held - c.spent)}" : $"持有 {c.held} / 实耗 {c.spent}";
                string saving = c.saved > 0 ? $" · 省 {c.saved}" : "";
                string missing = c.spent > c.held ? $" · 缺 {c.spent - c.held}" : "";
                string control = string.IsNullOrEmpty(c.relationText) ? "" : $"\n{c.relationText} · 续件节材率 {c.savingPercent}%";
                return $"{c.name}　{stock}{saving}{missing}{control}";
            }));
        }

        private static string CastQuoteText(Receipt q)
        {
            return Sections($"{q.name} ×{q.quantity}　{q.moldQuality} · {q.gradeName}\n{ForgeSlotName(q.slotType)}　{q.polarityName}{q.elementName}　攻 {q.baseAttack}　防 {q.baseDefense}",
                Difficulty(q.difficulty, q.difficultyName) + ExtraLine(q.difficultyText),
                Section("本炉控火", q.smithRankName + "　" + q.flameName + ExtraLine(q.castingText)),
                Section("整炉耗材" + (q.baseUnits > 0 ? $"　原方 {q.baseUnits} / 实耗 {q.spentUnits} / 节省 {q.savedUnits}" : ""), CostBreakdown(q)),
                $"材料最多可铸 {q.maxByMaterial} 件。扣料后有 {q.freeSlots} 格，本炉需 {q.quantity} 格。\n精力 {q.availableVigor} / 本炉需 {q.vigorCost}。成批研习 +{q.experience}。",
                Section("器印 · 五行共鸣", q.effectText), Section("材料纹理", q.materialText), Section("成品词条", q.bonusText));
        }

        private static string ReceiptText(Receipt q)
        {
            string output = !q.forged ? q.message :
                (q.quantity > 0 ? $"同模浇铸 ×{q.quantity}" : string.IsNullOrEmpty(q.moldId) ? "器成" : q.moldQuality + "已收入模架")
                + $"\n{q.name}（{q.gradeName}）　{ForgeSlotName(q.slotType)}\n攻 {q.baseAttack}　防 {q.baseDefense}　{q.polarityName} · {q.elementName} · {q.qiName}";
            string costs = q.quantity > 0 ? CostBreakdown(q, true) : q.spentText ?? q.costText;
            if (q.quantity > 0 && q.baseUnits > 0)
                costs = $"原方 {q.baseUnits} / 实耗 {q.spentUnits} / 节省 {q.savedUnits}" + ExtraLine(costs);
            return Sections(output, q.quantity == 0 && q.forged && q.vigorCost > 0 ? $"精力 {q.vigorCost} · 成器估值 {q.appraisal} 灵石 · 纳戒回收 {q.recyclePrice} 灵石" : "", Section("实耗材料", costs), Section("实际返还", q.returnedText),
                q.experience > 0 || !string.IsNullOrEmpty(q.trainingText) ? Section("修习与晋阶", $"本炉经验 +{q.experience}" + ExtraLine(q.trainingText)) : "",
                Difficulty(q.difficulty, q.difficultyName) + ExtraLine(q.difficultyText),
                Section("控火复盘", q.quantity > 0 ? q.flameName + ExtraLine(q.castingText) : q.flameName + ExtraLine(q.controlText) + ControlAdvice(q.controlText)),
                Section("本炉因素", q.factorText), Section("器印 · 五行共鸣", q.effectText),
                Section("材料纹理", q.materialText), Section("成品词条", q.bonusText?.Replace(" · ", "\n")), Section("工艺词条", q.processAffixText));
        }
    }
}
