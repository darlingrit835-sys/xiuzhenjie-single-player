using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed partial class AlchemyWorkshop
	{
		private static readonly Color32 Ground = new Color32(14, 33, 34, 255);
		private static readonly Color32 PanelInk = new Color32(24, 48, 49, 255);
		private static readonly Color32 Jade = new Color32(49, 92, 78, 255);
		private static readonly Color32 Gold = new Color32(235, 196, 123, 255);
		private static readonly Color32 Ivory = new Color32(248, 240, 210, 255);
		private static readonly Color32 Mist = new Color32(180, 201, 185, 255);
		private static readonly Color32 Rule = new Color32(148, 125, 79, 180);
		private Font font;
		private RectTransform sheet;
		private AlchemyWheelArt wheel;
		private Text metrics, chances, formula, status, stageIndex, stageName, instruction, strikeLabel, auxSummary;
		private Button manual, replay, strikeBtn;
		private readonly List<Action> syncRows = new List<Action>();
		private static readonly string[] Actions = { "引火温炉", "并流合药", "收束凝丹" };
		private static readonly string[] Guidance = { "待火针入光弧，点炉引火。", "循药气回旋，贴近弧心时合药。", "等丹光聚于弧心，点炉收丹。" };

		private static RectTransform Rect(string name, Transform parent)
		{
			var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
			rect.SetParent(parent, false);
			rect.gameObject.layer = parent.gameObject.layer;
			return rect;
		}

		private static void Fill(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
			rect.offsetMin = rect.offsetMax = Vector2.zero;
		}

		// Fractional horizontal extent and top-to-bottom extent; insets are canvas units.
		private static RectTransform Area(string name, Transform parent, float left, float right, float top, float bottom, float inset = 0)
		{
			var rect = Rect(name, parent);
			rect.anchorMin = new Vector2(left, 1 - bottom);
			rect.anchorMax = new Vector2(right, 1 - top);
			rect.offsetMin = new Vector2(inset, inset);
			rect.offsetMax = new Vector2(-inset, -inset);
			return rect;
		}

		private Text TextOn(RectTransform rect, string value, int size, Color tint, TextAnchor align = TextAnchor.MiddleCenter)
		{
			var text = rect.gameObject.AddComponent<Text>();
			text.font = font; text.fontSize = size; text.text = value; text.color = tint;
			text.alignment = align; text.raycastTarget = false;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = Mathf.Max(10, size - 3); text.resizeTextMaxSize = size;
			return text;
		}

		private Text Label(string id, Transform parent, string value, int size, Color tint,
			float left, float right, float top, float bottom, TextAnchor align = TextAnchor.MiddleCenter)
		{
			return TextOn(Area(id, parent, left, right, top, bottom), value, size, tint, align);
		}

		private static Image Tint(RectTransform rect, Color color, bool block = false)
		{
			var image = rect.gameObject.AddComponent<Image>();
			image.color = color; image.raycastTarget = block;
			return image;
		}

		private Button ButtonOn(RectTransform rect, string caption, Action click, bool primary = false)
		{
			var background = Tint(rect, primary ? Gold : Jade, true);
			var button = rect.gameObject.AddComponent<Button>();
			button.targetGraphic = background;
			var colors = button.colors;
			colors.pressedColor = new Color(.74f, .85f, .78f);
			colors.disabledColor = new Color(.6f, .65f, .65f, .70f);
			button.colors = colors;
			if (!string.IsNullOrEmpty(caption))
				TextOn(Area("Text", rect, 0, 1, 0, 1, 2), caption, 15, primary ? Ground : Ivory);
			button.onClick.AddListener(() => click());
			return button;
		}

		private Button ButtonAt(string id, Transform parent, string caption, Action click,
			float left, float right, float top, float bottom, bool primary = false)
		{
			return ButtonOn(Area(id, parent, left, right, top, bottom, 2), caption, click, primary);
		}

		private static void Line(Transform parent, float y)
		{
			var r = Area("Rule", parent, .035f, .965f, y, y);
			r.sizeDelta = new Vector2(r.sizeDelta.x, 1);
			Tint(r, Rule);
		}

		private void Show(Page next)
		{
			CloseOverlay();
			page = next;
			previewAt = -1;
			if (sheet != null) { sheet.gameObject.SetActive(false); Destroy(sheet.gameObject); }
			syncRows.Clear();
			sheet = Rect(next.ToString(), transform); Fill(sheet);
			var backdrop = Area("Hall", sheet, 0, 1, 0, 1);
			var wall = Tint(backdrop, Color.white);
			InkInterfaceArt.Set(wall, "ForgeHall", false);
			wall.color = new Color(.45f, .54f, .50f, .17f);
			Tint(Area("Lintel", sheet, 0, 1, 0, .082f), PanelInk);
			Label("HallTitle", sheet, "地火丹房", 21, Gold, .045f, .355f, .004f, .08f, TextAnchor.MiddleLeft);
			if (next == Page.Prep) ButtonAt("RecipeBook", sheet, "查方", OpenBook, .360f, .535f, .012f, .074f);
			if (next == Page.Prep || next == Page.Loading)
				ButtonAt("Legacy", sheet, "旧丹方", () => Close(onLegacy), .545f, .755f, .012f, .074f);
			ButtonAt("Leave", sheet, "离炉", () => Close(onLeave), .765f, .958f, .012f, .074f);
			Line(sheet, .083f);
		}

		private AlchemyWheelArt Art(string id, float left, float right, float top, float bottom, bool hit = false)
		{
			var area = Area(id, sheet, left, right, top, bottom);
			if (hit)
			{
				var touch = ButtonOn(area, null, Strike);
				touch.targetGraphic.color = Color.clear;
				touch.transition = Selectable.Transition.None;
			}
			return AlchemyWheelArt.Attach(area);
		}

		private void LoadingView(string message, bool retry)
		{
			Show(Page.Loading);
			wheel = Art("Furnace", .10f, .90f, .16f, .62f);
			wheel.SetIdle();
			Label("Status", sheet, message, 16, Ivory, .07f, .93f, .62f, .75f);
			if (retry) ButtonAt("Refresh", sheet, "重读丹方", LoadCatalog, .24f, .76f, .79f, .89f, true);
		}

		private void PendingView(bool waiting)
		{
			Show(Page.Pending);
			wheel = Art("Furnace", .12f, .88f, .16f, .60f); wheel.SetIdle();
			Label("Status", sheet, waiting ? "正在核对本炉回执…" : "上一次操作尚待确认，可取回原回执。", 16, Ivory, .06f, .94f, .63f, .76f);
			if (!waiting) ButtonAt("Retry", sheet, "取回回执", SendPending, .17f, .83f, .79f, .89f, true);
		}

		private void PrepView()
		{
			Show(Page.Prep);
			ButtonAt("PrevRecipe", sheet, "〈", () => NextRecipe(-1), .034f, .148f, .100f, .167f);
			ButtonAt("NextRecipe", sheet, "〉", () => NextRecipe(1), .852f, .966f, .100f, .167f);
			Label("Recipe", sheet, recipe.name, 25, Ivory, .15f, .85f, .088f, .151f);
			int index = Array.IndexOf(catalog.recipes, recipe);
			Label("Index", sheet, $"丹谱  {index + 1} / {catalog.recipes.Length}   ·   {recipe.effectName}", 13, Mist, .15f, .85f, .150f, .186f);
			wheel = Art("Furnace", .22f, .78f, .182f, .447f);
			wheel.SetIdle();
			metrics = Label("Training", sheet, "", 14, Mist, .018f, .235f, .206f, .419f);
			chances = Label("Affinity", sheet, "", 14, Gold, .765f, .982f, .206f, .419f);
			if (!string.IsNullOrEmpty(recipe.memoryQuality) && catalog.batchLimit > 1)
				ButtonAt("BatchSize", sheet, $"切换 · {batchSize}炉", CycleBatch, .352f, .648f, .382f, .447f);
			Line(sheet, .451f);
			Label("MainMaterial", sheet, $"主药  {recipe.primaryName} ×{recipe.primaryCount * batchSize}    ·    持有 {Stock(recipe.primaryId)}",
				15, Ivory, .045f, .725f, .457f, .504f, TextAnchor.MiddleLeft);
			ButtonAt("RecipeLedger", sheet, "药效与价", OpenLedger, .715f, .965f, .455f, .506f);
			auxSummary = Label("AuxSummary", sheet, "", 14, Gold, .045f, .70f, .506f, .558f, TextAnchor.MiddleLeft);
			if (!string.IsNullOrEmpty(recipe.memoryQuality))
				ButtonAt("Restore", sheet, "留影原料", RestoreMaterials, .716f, .965f, .508f, .557f);
			BuildRows();
			formula = Label("Formula", sheet, "", 14, Ivory, .035f, .965f, .794f, .850f);
			status = Label("Status", sheet, "", 13, Mist, .035f, .965f, .844f, .903f);
			var supplies = catalog.canClaimStarter ? "免费领材" : $"补料·{catalog.supplyPrice}灵石";
			ButtonAt("Supply", sheet, supplies, Supply, .026f, .306f, .91f, .986f);
			replay = ButtonAt("Replay", sheet, "留影直炼", Replay, .310f, .620f, .91f, .986f);
			manual = ButtonAt("Plan", sheet, "开炉手炼", StartManual, .624f, .974f, .91f, .986f, true);
		}

		private void BuildRows()
		{
			var viewport = Area("Materials", sheet, .032f, .968f, .566f, .794f);
			Tint(viewport, new Color32(15, 35, 36, 245), true);
			viewport.gameObject.AddComponent<RectMask2D>();
			var scroll = viewport.gameObject.AddComponent<ScrollRect>();
			var content = Rect("Rows", viewport);
			content.anchorMin = Vector2.up; content.anchorMax = Vector2.one; content.pivot = new Vector2(.5f, 1);
			var available = catalog.materials.Where(m => m.id != recipe.primaryId)
				.OrderByDescending(m => m.count > 0).ToArray();
			content.sizeDelta = new Vector2(0, available.Length * 38);
			content.anchoredPosition = Vector2.zero;
			scroll.viewport = viewport; scroll.content = content; scroll.horizontal = false;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			for (int i = 0; i < available.Length; i++)
			{
				MaterialChoice material = available[i];
				var row = Rect("Row_" + material.id, content);
				row.anchorMin = Vector2.up; row.anchorMax = Vector2.one;
				row.offsetMin = new Vector2(1, -38 * (i + 1) + 1); row.offsetMax = new Vector2(-1, -38 * i - 1);
				var bg = Tint(row, PanelInk, true);
				var inspect = row.gameObject.AddComponent<Button>();
				inspect.targetGraphic = bg;
				inspect.onClick.AddListener(() => OpenMaterial(material));
				Label("Material", row, material.name + " ›", 15, Ivory, .025f, .39f, 0, 1, TextAnchor.MiddleLeft);
				Label("Element", row, $"{material.elementName} · 余{material.count}", 13, Mist, .39f, .64f, 0, 1, TextAnchor.MiddleLeft);
				var minus = ButtonAt("Minus_" + material.id, row, "−", () => ChangeAux(material, -1), .65f, .759f, .045f, .955f);
				var qty = Label("Quantity", row, "", 15, Gold, .76f, .853f, .02f, .98f);
				var plus = ButtonAt("Aux_" + material.id, row, "+", () => ChangeAux(material, 1), .86f, .993f, .045f, .955f);
				syncRows.Add(() =>
				{
					int count = auxiliary.Count(id => id == material.id);
					qty.text = (count * batchSize).ToString();
					plus.interactable = !busy && auxiliary.Count < 3;
					qty.color = count * batchSize > material.count ? (Color)new Color32(255, 175, 135, 255) : Gold;
					minus.interactable = !busy && count > 0;
					bg.color = count > 0 ? (Color)new Color32(44, 69, 59, 255) : PanelInk;
				});
			}
		}

		private void UpdatePreparation()
		{
			bool qualified = catalog.rank >= recipe.minRank;
			bool stocked = HaveMaterials(batchSize);
			bool remembered = !string.IsNullOrEmpty(recipe.memoryQuality);
			metrics.text = $"丹师\n{catalog.rankName}\n\n熟练\n{recipe.mastery} / 100";
			chances.text = $"相性\n{(plan == null ? "—" : plan.affinity.ToString("+0;-0;0"))}\n\n留影\n{(remembered ? recipe.memoryQuality : "未录")}";
			auxSummary.text = $"每炉辅材 {auxiliary.Count}/3 · 本批共{auxiliary.Count * batchSize}份";
			formula.text = plan == null ? (qualified ? "估算炉中药性…" : $"此方需 {recipe.minRank} 品丹师") :
				$"成丹 {Rate(plan.minSuccessBps)}–{Rate(plan.maxSuccessBps)}    ·    {(remembered ? recipe.memoryQuality : "中品预估")} {plan.effectName} {Strength(plan.effectBasis, plan.effect)}" + Toxicity(plan.poisonCost);
			string hint = !stocked ? (plan != null && plan.canBuyMissing ? $"本炉缺材可花 {plan.shortageCost} 灵石补齐 · 见「药效与价」。" : "本炉尚缺材料 · 点「药效与价」查看来源。") : remembered ? "留影必成，换辅材可改变药力。" : "首炉成功可留下丹影，随后直接复炼。";
			int copyCost = (batchSize + 2) / 3;
			status.text = notice ?? (remembered && stocked ? $"现有 {catalog.vigor} 精力，手炼1炉耗1，复炼{batchSize}炉耗{copyCost}。" : hint);
			manual.interactable = !busy && !estimating && qualified && (plan == null || HaveMaterials());
			manual.GetComponentInChildren<Text>().text = plan == null && !estimating && qualified ? "重新估炉" : "开炉手炼";
			replay.GetComponentInChildren<Text>().text = remembered ? $"复炼{batchSize}炉" : "尚无留影";
			replay.interactable = !busy && qualified && remembered && stocked && plan != null;
			foreach (var sync in syncRows) sync();
			refreshLedger?.Invoke();
		}

		private static string Rate(int bps) => (bps / 100f).ToString("0.##") + "%";

		private void PlayView()
		{
			Show(Page.Playing);
			Label("Recipe", sheet, recipe.name, 25, Ivory, .04f, .96f, .111f, .181f);
			stageIndex = Label("Stages", sheet, "", 15, Mist, .03f, .97f, .181f, .231f);
			wheel = Art("Hit", .045f, .955f, .247f, .670f, true);
			stageName = Label("Stage", sheet, "", 21, Gold, .04f, .96f, .677f, .738f);
			instruction = Label("Guidance", sheet, "", 14, Ivory, .035f, .965f, .745f, .822f);
			strikeBtn = ButtonAt("Strike", sheet, "引火温炉", Strike, .17f, .83f, .84f, .938f, true);
			strikeLabel = strikeBtn.GetComponentInChildren<Text>();
		}

		private void StepView(Stage s)
		{
			stageIndex.text = string.Join("    ·    ", run.stages.Select((step, i) =>
				i < taps.Count ? $"{step.name} {hits[i]}" : i == taps.Count ? $"〔{step.name}〕" : step.name));
			stageName.text = $"{s.name}    {(s.direction > 0 ? "顺行" : "逆行")}";
			instruction.text = Guidance[taps.Count] + $"\n每圈 {s.periodMs / 1000f:0.0} 秒   ·   光弧 {s.windowBps * .072f:0}°";
			strikeLabel.text = Actions[taps.Count];
			strikeBtn.interactable = true;
		}

		private void HitView(Stage stage, int hit)
		{
			stageName.text = hit >= 90 ? "正中弧心" : hit >= 50 ? "火候恰好" : hit > 0 ? "火候稍偏" : "错过火候";
			instruction.text = $"{stage.name}   {hit} / 100";
			strikeBtn.interactable = false;
		}

		private void ReceiptView(Receipt result, bool recorded)
		{
			Show(Page.Receipt);
			Label("Title", sheet, result.brewed ? "丹 成" : "炉 火 已 息", 27, result.brewed ? Gold : Mist, .05f, .95f, .11f, .20f);
			wheel = Art("Furnace", .085f, .915f, .20f, .565f);
			wheel.SetResult(result.brewed);
			string name = result.recipeName ?? recipe?.name ?? "成丹";
			string outcome = result.brewed ? $"{name} · {result.quality} ×{result.quantity}" : "此炉未成";
			Label("Outcome", sheet, outcome, 21, Ivory, .025f, .975f, .57f, .643f);
			string source = recorded ? $"留影 {Math.Max(1,result.batches)} 炉" : $"火候 {result.score} / 100";
			if (result.vigorSpent > 0) source += $" · 耗精力 {result.vigorSpent}";
			string effect = result.brewed ? $"   ·   {result.effectName} {Strength(result.effectBasis, result.effect)}" : "";
			Label("Process", sheet, $"{source}   ·   成丹率 {Rate(result.successBps)}{effect}", 14, Mist, .03f, .97f, .641f, .704f);
			Line(sheet, .711f);
			string burden = result.brewed ? Toxicity(result.poisonCost) : "";
			string sale = result.brewed && !string.IsNullOrEmpty(result.effectText) ? $"   ·   每粒回收 {result.sellPrice} 灵石" : "";
			if (result.maxSellPrice > result.minSellPrice) sale = $"   ·   回收 {result.minSellPrice}–{result.maxSellPrice} 灵石/粒";
			string memory = string.IsNullOrEmpty(result.memoryQuality) ? "此方尚无留影" : $"丹影已录 · {result.memoryQuality}";
			Label("Status", sheet, (string.IsNullOrEmpty(result.effectText) ? "" : result.effectText + "\n") + $"{memory}{burden}\n此方熟练  {result.mastery} / 100{sale}", 15, Ivory, .06f, .94f, .724f, .833f);
			ButtonAt("Reopen", sheet, "返回配药", () => { notice = null; LoadCatalog(); }, .20f, .80f, .848f, .948f, true);
		}
	}
}
