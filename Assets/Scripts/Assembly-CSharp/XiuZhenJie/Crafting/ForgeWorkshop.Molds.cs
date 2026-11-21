using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class ForgeWorkshop
    {
        [Serializable]
        private class Mold
        {
            public string id, name, blankId, quality, difficultyName, difficultyText;
            public int difficulty;
            public int gradeId;
            public Ingredient[] inputs;
        }
        private readonly List<int> strokes = new List<int>(4);
        private static readonly int[] defaultHeatTargets = { 3, 7, 4, 6 };
        private int[] HeatTargets => sourcePattern?.heatTargets?.Length == 4 ? sourcePattern.heatTargets : defaultHeatTargets;
        private Mold activeMold;
        private Button moldRack, shapeButton, batchLess, batchMore, batchLessTen, batchMoreTen;
        private Text batchLabel;
        private int batchCount = 1;
        private Receipt castEstimate;
        private RectTransform shapeSheet, heatMarker;
        private Text strikeHint;
        private Button[] hammers;
        private ForgeProfileGraphic profile;
        private float heatStart;
        private int force;

        private void BuildMoldControls()
        {
            moldRack = MakeButton(transform, "MoldRack", "我的模具", OpenMolds);
            Right((RectTransform)moldRack.transform, 66, 5, 78, 28);
            shapeButton = MakeButton(transform, "ShapeBlank", "上砧塑形", OpenShaping);
            Top((RectTransform)shapeButton.transform, 12, 12, 167, 30);
            batchLess = MakeButton(transform, "Fewer", "−", () => ChangeBatch(-1));
            batchMore = MakeButton(transform, "More", "＋", () => ChangeBatch(1));
            Left((RectTransform)batchLess.transform, 12, 134, 46, 32);
            Right((RectTransform)batchMore.transform, 12, 134, 46, 32);
            batchLessTen = MakeButton(transform, "FewerTen", "−10", () => ChangeBatch(-10));
            batchMoreTen = MakeButton(transform, "MoreTen", "＋10", () => ChangeBatch(10));
            Left((RectTransform)batchLessTen.transform, 62, 134, 50, 32);
            Right((RectTransform)batchMoreTen.transform, 62, 134, 50, 32);
            batchLabel = Label(transform, "BatchCount", "", 16);
            batchLabel.alignment = TextAnchor.MiddleCenter;
            Top(batchLabel.rectTransform, 116, 116, 134, 32);
        }

        private void InvalidateCast()
        {
            castEstimate = null;
            firstEstimate = null;
        }
        private void ChangeBatch(int delta)
        {
            if (Locked || activeMold == null)
                return;
            batchCount = Mathf.Clamp(batchCount + delta, 1, catalog.batchLimit);
            InvalidateCast();
            status.text = "";
            UpdateControls();
        }

        private void UpdateMoldControls()
        {
            bool enabled = catalog != null && catalog.moldVersion > 0;
            bool cast = enabled && activeMold != null;
            moldRack.gameObject.SetActive(enabled);
            shapeButton.gameObject.SetActive(enabled);
            batchLess.gameObject.SetActive(cast);
            batchMore.gameObject.SetActive(cast);
            batchLessTen.gameObject.SetActive(cast);
            batchMoreTen.gameObject.SetActive(cast);
            batchLabel.gameObject.SetActive(cast);
            workName.gameObject.SetActive(!cast);
            if (!enabled)
                return;
            moldRack.interactable = shapeButton.interactable = !Locked;
            var label = shapeButton.GetComponentInChildren<Text>(true);
            if (!cast)
            {
                label.text = strokes.Count == 4 ? Quality() + " · 查看或重塑" : $"上砧塑形　{strokes.Count}/4 锤";
                submit.GetComponentInChildren<Text>(true).text = pending != null ? "重试上一笔" : "开炉留模";
                if (pending == null && strokes.Count != 4)
                {
                    submit.interactable = estimate.interactable = false;
                    preparation.text = "四锤定形，首件炼成后留模。";
                }
                return;
            }
            int unitCount = (activeMold.inputs ?? Array.Empty<Ingredient>()).Sum(x => x.quantity);
            label.text = "恢复原方　·　可替换等份材料";
            gradeButton.interactable = previous.interactable = next.interactable = false;
            blankTitle.text = activeMold.name + "　·　" + activeMold.quality;
            gradeButton.GetComponentInChildren<Text>(true).text = "模具上限 · " + GradeName(activeMold.gradeId);
            flameButton.GetComponentInChildren<Text>(true).text = "选火 · " + catalog.flames[flame].name;
            training.text = Difficulty(activeMold.difficulty, activeMold.difficultyName) + "　" + catalog.smithRankName;
            batchLabel.text = $"{batchCount} / {catalog.batchLimit} 件";
            batchLess.interactable = !Locked && batchCount > 1;
            batchMore.interactable = !Locked && batchCount < catalog.batchLimit;
            batchLessTen.interactable = batchLess.interactable;
            batchMoreTen.interactable = batchMore.interactable;
            materialTitle.text = $"每件用料　{Total} / {unitCount} 份";
            estimate.interactable = !Locked && Total == unitCount;
            estimate.GetComponentInChildren<Text>(true).text = "验料";
            submit.GetComponentInChildren<Text>(true).text = pending != null ? "重试上一笔" : $"浇铸 ×{batchCount}";
            submit.interactable = !busy && (pending != null || castEstimate != null && castEstimate.canFit &&
                                                                   batchCount <= castEstimate.maxByMaterial && castEstimate.availableVigor >= castEstimate.vigorCost);
            preparation.text = Total != unitCount ? $"请为每件选足 {unitCount} 份材料。"
                               : castEstimate == null
                                   ? "点验料查看成品与整批消耗。"
                                   : $"料可铸 {castEstimate.maxByMaterial} 件　·　扣料后可用 {castEstimate.freeSlots} 格";
        }

        private string GradeName(int id) => catalog.grades.FirstOrDefault(g => g.id == id)?.name ?? "";
        private void SelectMold(Mold mold)
        {
            activeMold = mold;
            sourcePattern = null;
            processStep = 0;
            picked.Clear();
            batchCount = 1;
            InvalidateCast();
            if (mold != null)
            {
                blank = Mathf.Max(0, Array.FindIndex(catalog.blanks, b => b.id == mold.blankId));
                foreach (var ingredient in mold.inputs ?? Array.Empty<Ingredient>())
                    picked[ingredient.templateId] = ingredient.quantity;
            }
            else
            {
                strokes.Clear();
                estimate.GetComponentInChildren<Text>(true).text = "估火候";
            }
            status.text = "";
            UpdateControls();
        }

        private void OpenMolds()
        {
            if (Locked || transform.Find("Molds") != null)
                return;
            var sheet = Node("Molds", transform);
            Stretch(sheet);
            InkInterfaceArt.Set(sheet.gameObject.AddComponent<Image>(), "Parchment", false);
            Top(Label(sheet, "Title", "模架", 20).rectTransform, 12, 68, 6, 32);
            Right((RectTransform)MakeButton(sheet, "Back", "返回", () => Destroy(sheet.gameObject)).transform, 8, 6, 56, 30);
            Top((RectTransform)MakeButton(sheet, "New", "新制器模",
                                          () =>
                                          {
                                              SelectMold(null);
                                              Destroy(sheet.gameObject);
                                          })
                    .transform,
                12, 12, 48, 40);
            var items = catalog.molds ?? new Mold[0];
            if (items.Length == 0)
            {
                Top(Label(sheet, "Empty", "手塑器胚并炼成首件后，模具将陈列于此。", 16).rectTransform, 14, 14, 100, 50);
                return;
            }
            var view = Node("Viewport", sheet);
            Stretch(view);
            view.offsetMin = new Vector2(10, 12);
            view.offsetMax = new Vector2(-10, -98);
            view.gameObject.AddComponent<Image>().color = InkDialogArt.Ivory;
            view.gameObject.AddComponent<RectMask2D>();
            var scroll = view.gameObject.AddComponent<ScrollRect>();
            var list = Node("List", view);
            list.anchorMin = new Vector2(0, 1);
            list.anchorMax = Vector2.one;
            list.pivot = new Vector2(.5f, 1);
            list.sizeDelta = new Vector2(0, items.Length * 51);
            list.anchoredPosition = Vector2.zero;
            scroll.content = list;
            scroll.viewport = view;
            scroll.horizontal = false;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            for (int i = 0; i < items.Length; i++)
            {
                var m = items[i];
                var b = MakeButton(list, "Mold_" + m.id, $"{m.name}　{m.quality} · {GradeName(m.gradeId)}{ExtraLine(Difficulty(m.difficulty, m.difficultyName))}",
                                   () =>
                                   {
                                       SelectMold(m);
                                       Destroy(sheet.gameObject);
                                   });
                Top((RectTransform)b.transform, 2, 2, i * 51, 46);
            }
        }

        private void EstimateCast()
        {
            busy = true;
            InvalidateCast();
            UpdateControls();
            ForgeWire.Send(
                MsgType.ForgeCastEstimate, JsonUtility.ToJson(BuildOrder()),
                (ok, json) =>
                {
                    if (this == null)
                        return;
                    busy = false;
                    if (ok)
                    {
                        castEstimate = JsonUtility.FromJson<Receipt>(json);
                        status.text =
                            $"{castEstimate.gradeName} · {castEstimate.elementName}　攻 {castEstimate.baseAttack}　防 {castEstimate.baseDefense}\n{castEstimate.materialText}　{castEstimate.bonusText}\n{castEstimate.costText}";
                    ShowCastQuote();
                    }
                    else
                        status.text = ErrorText(json, "验料未完成");
                    UpdateControls();
                });
        }

        private void ShowCastQuote()
        {
            var q = castEstimate;
            var sheet = ReportSheet("CastQuote", "浇铸验料单", CastQuoteText(q), true);
            var back = MakeButton(sheet,"Back","调整配料",()=>Destroy(sheet.gameObject));
            bool enough = q.maxByMaterial >= batchCount && q.canFit && q.availableVigor >= q.vigorCost;
            var pour = MakeButton(sheet,"Pour", enough ? $"浇铸 ×{batchCount}" : !q.canFit ? "纳戒空间不足" : q.availableVigor < q.vigorCost ? "精力不足" : "用料不足",()=>
            {
                Destroy(sheet.gameObject);
                SubmitCast();
            });
            pour.interactable = enough;
            InkDialogArt.StyleButton(pour,"JadeButton",InkDialogArt.Ivory);
            Footer((RectTransform)back.transform,0,.5f);
            Footer((RectTransform)pour.transform,.5f,1);
        }

        private void SubmitCast()
        {
            if (castEstimate == null || !castEstimate.canFit || batchCount > castEstimate.maxByMaterial || castEstimate.availableVigor < castEstimate.vigorCost)
                return;
            var order = BuildOrder();
            order.requestId = Guid.NewGuid().ToString("N");
            SaveAndSend(MsgType.ForgeCast, JsonUtility.ToJson(order));
        }

        private int Hits() => strokes.Where((s, i) => Mathf.Abs(s % 10 - HeatTargets[i]) <= 1).Count();
        private string Quality() => Hits() >= 3 ? "精模" : Hits() > 0 ? "良模" : "粗模";
        private string Cap(int loss) => catalog.grades[Mathf.Max(0, grade - loss)].name;

        private void OpenShaping()
        {
            if (Locked || shapeSheet != null)
                return;
            if (activeMold != null)
            {
                SelectMold(activeMold);
                return;
            }
            shapeSheet = Node("Shaping", transform);
            Stretch(shapeSheet);
            InkInterfaceArt.Set(shapeSheet.gameObject.AddComponent<Image>(), "Parchment", false);
            Top(Label(shapeSheet, "Title", catalog.blanks[blank].name + " · 锻砧", 20).rectTransform, 12, 65, 6, 36);
            Right((RectTransform)MakeButton(shapeSheet, "Back", "返回", () => Destroy(shapeSheet.gameObject)).transform, 8,
                  7, 52, 30);
            var art = Node("ForgeHall", shapeSheet);
            Top(art, 12, 12, 52, 126);
            var backdrop = art.gameObject.AddComponent<Image>();
            InkInterfaceArt.Set(backdrop, "ForgeHall", false);
            backdrop.color = new Color(1f, 1f, 1f, .36f);
            var silhouette = Node("Profile", art);
            Stretch(silhouette, 12);
            profile = silhouette.gameObject.AddComponent<ForgeProfileGraphic>();
            profile.raycastTarget = false;
            strikeHint = Label(shapeSheet, "StrikeHint", "", 15);
            Top(strikeHint.rectTransform, 14, 14, 182, 49);
            // A ten-stop heat scale gives the same sampled force the server grades.
            var gauge = Node("Gauge", shapeSheet);
            Top(gauge, 24, 24, 242, 16);
            gauge.gameObject.AddComponent<Image>().color = new Color32(179, 164, 125, 255);
            var sweet = Node("StrikeWindow", gauge);
            sweet.gameObject.AddComponent<Image>().color = new Color32(103, 148, 110, 255);
            heatMarker = Node("HeatMarker", gauge);
            heatMarker.gameObject.AddComponent<Image>().color = InkInterfaceArt.Seal;
            hammers = new Button[3];
            string[] names;
            switch (catalog.blanks[blank].slotType)
            {
                case 26: names = new[] { "袖口 · 贯气", "衣襟 · 织纹", "内衬 · 护体" }; break;
                case 16: names = new[] { "镜面 · 聚光", "镜心 · 刻印", "镜缘 · 固胎" }; break;
                case 29: names = new[] { "靴尖 · 引风", "靴面 · 系纹", "靴底 · 固形" }; break;
                default: names = new[] { "前端 · 引锋", "中段 · 匀脊", "根部 · 固座" }; break;
            }
            for (int i = 0; i < 3; i++)
            {
                int zone = i;
                hammers[i] = MakeButton(shapeSheet, "Strike" + i, names[i], () => Strike(zone));
                HalfTop((RectTransform)hammers[i].transform, i / 3f, (i + 1) / 3f, 275, 43);
            }
            Top(Label(
                    shapeSheet, "Guide",
                    (sourcePattern?.heatTargets?.Length == 4 ? $"{sourcePattern.difficultyName} · 火候 {string.Join(" / ", HeatTargets)}\n" : "") +
                    $"青区下锤，前端偏攻，根部偏防。\n合拍 3–4 锤 · 精模 {Cap(0)}\n合拍 1–2 锤 · 良模 {Cap(1)}\n合拍 0 锤 · 粗模 {Cap(2)}",
                    14)
                    .rectTransform,
                14, 14, 325, 148);
            var reset = MakeButton(shapeSheet, "Reset", "重新起胚",
                                   () =>
                                   {
                                       strokes.Clear();
                                       InvalidateCast();
                                       RefreshShape();
                                   });
            var keep = MakeButton(shapeSheet, "Keep", "收锤定形",
                                  () =>
                                  {
                                      Destroy(shapeSheet.gameObject);
                                      UpdateControls();
                                  });
            Footer((RectTransform)reset.transform, 0, .5f);
            Footer((RectTransform)keep.transform, .5f, 1);
            RefreshShape();
        }

        private void RefreshShape()
        {
            var counts = new int[3];
            foreach (int stroke in strokes)
                counts[stroke / 10]++;
            profile.Set(catalog.blanks[blank].id, catalog.blanks[blank].slotType, counts);
            bool done = strokes.Count == 4;
            foreach (var hammer in hammers)
                hammer.interactable = !done;
            var gauge = (RectTransform)heatMarker.parent;
            gauge.gameObject.SetActive(!done);
            if (!done)
            {
                var target = (RectTransform)gauge.Find("StrikeWindow");
                float v = HeatTargets[strokes.Count];
                target.anchorMin = new Vector2(Mathf.Clamp01((v - 1.5f) / 9), 0);
                target.anchorMax = new Vector2(Mathf.Clamp01((v + 1.5f) / 9), 1);
                target.offsetMin = target.offsetMax = Vector2.zero;
                heatStart = Time.unscaledTime;
                SetHeatPosition(0f);
            }
            strikeHint.text = done ? $"四锤定形 · {Quality()}　合拍 {Hits()}/4\n可收锤查看炉析，也可重新起胚。"
                                   : $"第 {strokes.Count+1} 锤　合拍 {Hits()} 次\n红针进入青区时，点下要锤打的位置。";
            UpdateControls();
        }

        private void Update()
        {
            if (shapeSheet == null || heatMarker == null || strokes.Count >= 4)
                return;
            float position = Mathf.PingPong((Time.unscaledTime - heatStart) * .65f * (sourcePattern?.heatTargets?.Length == 4 ? 1f + .06f * Mathf.Clamp(sourcePattern.difficulty - 1, 0, 4) : 1f), 1f);
            SetHeatPosition(position);
        }

        private void SetHeatPosition(float position)
        {
            force = Mathf.RoundToInt(position * 9);
            heatMarker.anchorMin = new Vector2(position, 0);
            heatMarker.anchorMax = new Vector2(position, 1);
            heatMarker.offsetMin = new Vector2(-2, -3);
            heatMarker.offsetMax = new Vector2(2, 3);
        }

        private void Strike(int zone)
        {
            if (strokes.Count >= 4)
                return;
            InvalidateCast();
            strokes.Add(zone * 10 + force);
            RefreshShape();
            ForgeEffects.Play(profile.transform, "hammer");
        }
    }
}
