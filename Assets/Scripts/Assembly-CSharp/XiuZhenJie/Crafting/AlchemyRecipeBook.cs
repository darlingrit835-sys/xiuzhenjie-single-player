using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed partial class AlchemyWorkshop
	{
		private RectTransform overlay, bookRows;
		private Action refreshLedger;
		private InputField bookSearch;
		private Text bookCount;
		private int bookFilter;
		private readonly Button[] bookTabs = new Button[3];

		private static string Strength(string basis, int value) =>
			basis == "realm_air" || basis == "max_blood" ? Rate(value) : value.ToString();

		private static string Toxicity(int points) => points > 0 ? "   ·   丹毒 +" + points : "";

		private void CloseOverlay()
		{
			refreshLedger = null;
			if (overlay == null) return;
			overlay.gameObject.SetActive(false);
			Destroy(overlay.gameObject);
			overlay = null;
		}

		private void OpenOverlay(string id, string title)
		{
			CloseOverlay();
			overlay = Rect(id, sheet); Fill(overlay);
			Tint(overlay, Ground, true);
			Tint(Area("Lintel", overlay, 0, 1, 0, .095f), PanelInk);
			Label("Title", overlay, title, 21, Gold, .047f, .75f, .005f, .091f, TextAnchor.MiddleLeft);
			ButtonAt("Back", overlay, "返回", CloseOverlay, .765f, .963f, .012f, .085f);
			Line(overlay, .095f);
		}

		private RectTransform ScrollBody(string name, float top, float bottom)
		{
			var viewport = Area(name, overlay, .036f, .964f, top, bottom);
			Tint(viewport, Ground, true);
			viewport.gameObject.AddComponent<RectMask2D>();
			var scroll = viewport.gameObject.AddComponent<ScrollRect>();
			var rows = Rect("Rows", viewport);
			rows.anchorMin = Vector2.up; rows.anchorMax = Vector2.one; rows.pivot = new Vector2(.5f, 1);
			rows.sizeDelta = Vector2.zero; rows.anchoredPosition = Vector2.zero;
			var layout = rows.gameObject.AddComponent<VerticalLayoutGroup>();
			layout.spacing = 5; layout.padding = new RectOffset(3, 3, 2, 8);
			layout.childControlWidth = true; layout.childForceExpandWidth = true;
			layout.childControlHeight = true; layout.childForceExpandHeight = false;
			rows.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scroll.viewport = viewport; scroll.content = rows; scroll.horizontal = false;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			return rows;
		}

		private Text Paragraph(Transform rows, string id, string value, bool heading = false)
		{
			var rect = Rect(id, rows);
			var text = TextOn(rect, value, heading ? 16 : 14, heading ? Gold : Ivory, TextAnchor.MiddleLeft);
			text.resizeTextForBestFit = false;
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.verticalOverflow = VerticalWrapMode.Overflow;
			return text;
		}

		private void OpenBook()
		{
			if (busy) return;
			OpenOverlay("RecipeLibrary", "丹  谱");
			var input = Area("Search", overlay, .038f, .962f, .114f, .182f);
			Tint(input, PanelInk, true);
			bookSearch = input.gameObject.AddComponent<InputField>();
			bookSearch.textComponent = Label("Text", input, "", 15, Ivory, .035f, .965f, 0, 1, TextAnchor.MiddleLeft);
			bookSearch.placeholder = Label("Placeholder", input, "搜索丹名、药效或主药", 15, Mist, .035f, .965f, 0, 1, TextAnchor.MiddleLeft);
			string[] names = { "全部丹方", "品阶已达", "已有留影" };
			bookFilter = 0;
			for (int i = 0; i < names.Length; i++)
			{
				int filter = i;
				bookTabs[i] = ButtonAt("Filter" + i, overlay, names[i], () => { bookFilter = filter; FilterBook(); },
					.035f + i * .31f, .34f + i * .31f, .193f, .260f);
			}
			bookCount = Label("Count", overlay, "", 13, Mist, .045f, .955f, .263f, .308f, TextAnchor.MiddleLeft);
			bookRows = ScrollBody("Recipes", .315f, .985f);
			bookSearch.onValueChanged.AddListener(_ => FilterBook());
			FilterBook();
		}

		private void FilterBook()
		{
			foreach (Transform row in bookRows) { row.gameObject.SetActive(false); Destroy(row.gameObject); }
			string keyword = bookSearch.text.Trim();
			bool Matches(string value) => !string.IsNullOrEmpty(value) && value.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
			int found = 0;
			foreach (var entry in catalog.recipes)
			{
				if ((bookFilter == 1 && entry.minRank > catalog.rank) || (bookFilter == 2 && string.IsNullOrEmpty(entry.memoryQuality))) continue;
				if (keyword.Length > 0 && !Matches(entry.name) && !Matches(entry.primaryName) && !Matches(entry.effectName) && !Matches(entry.effectDescription)) continue;
				found++;
				var row = Rect("Recipe_" + entry.id, bookRows);
				row.gameObject.AddComponent<LayoutElement>().preferredHeight = 57;
				var bg = Tint(row, entry.id == recipe.id ? Jade : PanelInk, true);
				var pick = row.gameObject.AddComponent<Button>(); pick.targetGraphic = bg;
				pick.onClick.AddListener(() =>
				{
					if (recipe.id != entry.id) auxiliary.Clear();
					recipe = entry; notice = null; Prepare();
				});
				Label("Name", row, entry.name, 17, Ivory, .03f, .68f, .03f, .55f, TextAnchor.MiddleLeft);
				Label("Rank", row, entry.minRank == 0 ? "入门" : entry.minRank + " 品起", 13, entry.minRank > catalog.rank ? Mist : Gold, .70f, .97f, .03f, .55f, TextAnchor.MiddleRight);
				string effect = entry.effectName + (entry.baseEffect > 0 ? " " + Strength(entry.effectBasis, entry.baseEffect) : "");
				Label("Summary", row, $"{effect}   ·   主药 {entry.primaryName}" +
					(string.IsNullOrEmpty(entry.memoryQuality) ? "" : "   ·   " + entry.memoryQuality + "留影"),
					13, Mist, .03f, .98f, .53f, .98f, TextAnchor.MiddleLeft);
			}
			for (int i = 0; i < bookTabs.Length; i++)
				bookTabs[i].targetGraphic.color = i == bookFilter ? (Color)new Color32(95, 99, 66, 255) : Jade;
			bookCount.text = $"共 {found} 方   ·   {catalog.rankName}" + (catalog.rankLimit > 0 ? $"   修习 {catalog.rankExp}/{catalog.rankLimit}" : "");
			if (found == 0) Paragraph(bookRows, "Empty", "未找到相符丹方，可更换关键词或筛选。");
			bookRows.anchoredPosition = Vector2.zero;
		}

		private static string Cost(MaterialChoice m) => m.buyPrice > 0 ? $"药商采购 {m.buyPrice} 灵石 / 份" : m.value > 0 || !string.IsNullOrEmpty(m.sourceText) ? "药商未售单份" : "尚无药商报价";
		private static string Route(MaterialChoice m) => string.IsNullOrEmpty(m.sourceText) ? "尚无来源记录" : m.sourceText;

		private void OpenMaterial(MaterialChoice m)
		{
			if (busy) return;
			OpenOverlay("HerbInfo", m.name);
			var body = ScrollBody("Info", .13f, .978f);
			Paragraph(body, "Type", $"{m.elementName}行   ·   纳戒可用 {m.count} 份", true);
			Paragraph(body, "SourceHeading", "取材", true);
			Paragraph(body, "Source", Route(m));
			Paragraph(body, "PriceHeading", "药价", true);
			Paragraph(body, "Price", (m.value > 0 ? $"每份参考 {m.value} 灵石\n" : "") + Cost(m));
		}

		private void OpenLedger()
		{
			if (busy) return;
			OpenOverlay("RecipeDetails", recipe.name + " · 方解");
			var body = ScrollBody("Ledger", .118f, .836f);
			Paragraph(body, "EffectTitle", recipe.effectName + "   ·   每粒药效", true);
			if (!string.IsNullOrEmpty(recipe.effectDescription)) Paragraph(body, "Effect", recipe.effectDescription);
			if (recipe.poisonCost > 0) Paragraph(body, "Poison", $"每服一粒丹毒 +{recipe.poisonCost}，累积上限 100。");
			var potency = Paragraph(body, "Estimate", "");
			if (recipe.sellPrices != null && recipe.sellPrices.Length == 3)
				Paragraph(body, "Prices", $"每粒常规回收价   下品 {recipe.sellPrices[0]}   ·   中品 {recipe.sellPrices[1]}   ·   上品 {recipe.sellPrices[2]} 灵石");
			Paragraph(body, "Materials", "本炉用料与出处", true);
			int ingredientValue = 0;
			foreach (var line in new[] { recipe.primaryId }.Concat(auxiliary).GroupBy(id => id))
			{
				var m = Array.Find(catalog.materials, candidate => candidate.id == line.Key);
				if (m == null) continue;
				bool main = m.id == recipe.primaryId;
				int n = main ? recipe.primaryCount : line.Count();
				ingredientValue += n * m.value;
				Paragraph(body, "Need_" + m.id, $"{(main ? "主药" : "辅药")}   {m.name} ×{n}   ·   持有 {m.count}", true);
				Paragraph(body, "Source_" + m.id, Route(m) + "\n" + Cost(m));
			}
			Paragraph(body, "QuoteTitle", "这一炉的药账", true);
			if (ingredientValue > 0) Paragraph(body, "IngredientValue", $"本炉用料参考合计 {ingredientValue} 灵石（含已持有材料）。");
			var quote = Paragraph(body, "Quote", "");
			var shortage = Paragraph(body, "Shortage", "");
			var buy = ButtonAt("BuyMissing", overlay, "核对缺材", BuyMissing, .07f, .93f, .864f, .97f, true);
			refreshLedger = () =>
			{
				potency.text = plan == null ? (recipe.baseEffect > 0 ? "基础药力   " + Strength(recipe.effectBasis, recipe.baseEffect) : "") :
					(string.IsNullOrEmpty(recipe.memoryQuality) ? "中品估算   " : recipe.memoryQuality + "留影   ") +
					(string.IsNullOrEmpty(plan.effectText) ? plan.effectName + " " + Strength(plan.effectBasis, plan.effect) : plan.effectText);
				quote.text = plan?.priceText ?? (recipe.minRank > catalog.rank ? $"丹师达 {recipe.minRank} 品可估炉。" : "暂未读到本炉报价。");
				shortage.text = plan?.shortageText ?? "";
				buy.interactable = plan != null && plan.canBuyMissing && plan.shortageCost > 0;
				buy.GetComponentInChildren<Text>().text = buy.interactable ? $"购齐{batchSize}炉缺材 · {plan.shortageCost} 灵石" :
					(plan == null ? "暂无本炉报价" : HaveMaterials(batchSize) ? "本批材料已齐" : "缺材需按出处取得");
			};
			refreshLedger();
		}
	}
}
