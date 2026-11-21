using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	// The ring owns its presentation order. Never reorder the server snapshot used by inventory sync.
	internal sealed partial class RingLedger
	{
		private static readonly string[] Sections = { "全部", "法器", "材料", "典籍", "丹符", "奇物" };
		private static readonly string[] States = { "全部状态", "可出售", "已锁定", "已认主", "使用中", "赠礼中" };
		private const float RowHeight = 44f;
		private readonly NaJieSpacePanel panel;
		private readonly ScrollRect scroll;
		private readonly Text summary, empty, saleCaption, upgradeCaption;
		private readonly Button sortButton, stateButton;
		private readonly List<Button> tabs = new List<Button>();
		private readonly Dictionary<string, Row> rows = new Dictionary<string, Row>();
		private readonly HashSet<string> marked = new HashSet<string>();
		private readonly InputField search;
		private readonly Action<BaseNaJieGoodEntity, GameObject> inspect;
		private List<BaseNaJieGoodEntity> items = new List<BaseNaJieGoodEntity>();
		private List<BaseNaJieGoodEntity> sectionItems = new List<BaseNaJieGoodEntity>();
		private int category, state;
		private string focusedId;
		private bool guide;
		internal bool Choosing { get; private set; }
		internal Button PracticeBook { get; private set; }
		internal IEnumerable<BaseNaJieGoodEntity> Selected => items.Where(g => marked.Contains(g.GoodsId) && CanChoose(g));
		private Font Font => panel.RongLiangText.font;

		private sealed class Row
		{
			internal Button Button;
			internal Image Icon, Focus, Box;
			internal RectTransform Check;
			internal Text Name, Detail, Count, State;
			internal BaseNaJieGoodEntity Item;
		}

		internal RingLedger(NaJieSpacePanel panel, Action<BaseNaJieGoodEntity, GameObject> inspect)
		{
			this.panel = panel;
			this.inspect = inspect;
			scroll = panel.NaJieListConstant.GetComponentInParent<ScrollRect>();
			panel.NaJieListConstant.GetComponent<GridLayoutGroup>().enabled = false;
			var infinite = panel.NaJieListConstant.GetComponent<ScrollViewInfiniteContent>();
			if (infinite != null) infinite.enabled = false;
			scroll.horizontal = false;
			scroll.vertical = true;
			scroll.inertia = true;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			scroll.scrollSensitivity = 28f;
			scroll.GetComponent<Image>().raycastTarget = true;
			InkDialogArt.Inset((RectTransform)scroll.transform, 5f, 5f, 166f, 49f);
			InkDialogArt.Inset(scroll.viewport, 0, 0, 0, 0);
			BuildListIndicator();
			saleCaption = panel.YiJianChuShouBtn.GetComponentInChildren<Text>();
			upgradeCaption = panel.ShengJiBtn.GetComponentInChildren<Text>();
			var input = DefaultControls.CreateInputField(new DefaultControls.Resources());
			input.name = "GoodsSearch";
			input.transform.SetParent(panel.transform, false);
			input.layer = panel.gameObject.layer;
			Place((RectTransform)input.transform, 0, 1, 7, -149, 70, 32);
			search = input.GetComponent<InputField>();
			InkInterfaceArt.Set(search.image, "ParchmentHeading", false);
			var hint = (Text)search.placeholder;
			hint.text = "搜物品或属性";
			hint.fontStyle = FontStyle.Normal;
			hint.color = InkInterfaceArt.SoftInk;
			search.textComponent.color = InkInterfaceArt.Ink;
			foreach (var text in input.GetComponentsInChildren<Text>())
			{
				text.gameObject.layer = input.layer;
				text.font = Font;
				text.fontSize = 13;
				text.raycastTarget = false;
				InkDialogArt.Inset(text.rectTransform, 9, 8, 0, 0);
			}
			search.onValueChanged.AddListener(_ => Redraw(true));
			var manage = Control(panel.transform, "RingManageTabs", "分类栏", OpenCategoryOptions);
			Place((RectTransform)manage.transform, 1, 1, -146, -90, 70, 32);
			sortButton = Control(panel.transform, "SortOrder", "多项排序", OpenSortOptions);
			Place((RectTransform)sortButton.transform, 1, 1, -87, -7, 70, 32);
			BuildCategoryStrip();
			modeBand = Node(panel.transform, "SelectionMode").AddComponent<Image>();
			InkInterfaceArt.Set(modeBand, "JadeButton", false);
			modeBand.raycastTarget = false;
			Place(modeBand.rectTransform, 0, 1, 7, -96, 142, 22);
			summary = Label(panel.transform, "Summary", 12, TextAnchor.MiddleLeft);
			Place(summary.rectTransform, 0, 1, 13, -99, 141, 24);
			stateButton = Control(panel.transform, "ItemState", States[0], () => {
				state = (state + 1) % States.Length;
				Redraw(true);
			});
			Place((RectTransform)stateButton.transform, 1, 1, -90, -7, 142, 23);
			empty = Label(scroll.transform, "NoMatchingGoods", 14, TextAnchor.MiddleCenter);
			InkDialogArt.Inset(empty.rectTransform, 15, 15, 0, 0);
			empty.gameObject.SetActive(false);
			RefreshCategoryStrip();
		}

		internal static int Section(int type) => RingGoodsCatalog.Section(type);
		internal static int Section(BaseNaJieGoodEntity goods) => RingGoodsCatalog.Section(goods);
		internal static string Kind(int type) => RingGoodsCatalog.Kind(type);
		internal static string Kind(BaseNaJieGoodEntity goods) => RingGoodsCatalog.Kind(goods);
		internal static Sprite Artwork(BaseNaJieGoodEntity goods) => RingGoodsCatalog.Artwork(goods);

		internal static string Status(BaseNaJieGoodEntity g)
		{
			var parts = new List<string>(3);
			if (g.IsUsing) parts.Add("使用中");
			if (g.IsLock) parts.Add("已锁定");
			if (g.IsBinding) parts.Add("已认主");
			if (g.OfferPending) parts.Add("赠礼中");
			return string.Join("·", parts);
		}

		internal static bool Tradeable(BaseNaJieGoodEntity g) => !g.IsLock && !g.IsUsing && !g.IsBinding && !g.OfferPending && g.RecyclePrice >= 0;
		private bool CanChoose(BaseNaJieGoodEntity g) => Tradeable(g) && !(guide && PracticeGuideFlow.IsStarter(g));

		internal void SetItems(List<BaseNaJieGoodEntity> all, List<BaseNaJieGoodEntity> filtered, bool guiding)
		{
			items = all ?? new List<BaseNaJieGoodEntity>();
			var included = filtered == null ? null : new HashSet<string>(filtered.Select(g => g.GoodsId));
			sectionItems = items.Where(g => included == null || included.Contains(g.GoodsId) ||
				guiding && PracticeGuideFlow.IsStarter(g)).ToList();
			bool startGuide = guiding && !guide;
			if (startGuide)
			{
				category = state = 0;
				Choosing = false;
				marked.Clear();
				search.SetTextWithoutNotify("");
				RevealCategory(0);
			}
			guide = guiding;
			if (focusedId != null && !items.Any(g => g.GoodsId == focusedId)) focusedId = null;
			foreach (var id in rows.Keys.Except(items.Select(g => g.GoodsId)).ToList())
			{
				rows[id].Button.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(rows[id].Button.gameObject);
				rows.Remove(id);
			}
			marked.IntersectWith(items.Where(CanChoose).Select(g => g.GoodsId));
			Redraw(startGuide);
		}

		internal void SetChoosing(bool value)
		{
			Choosing = value;
			marked.Clear();
			Redraw(false);
		}

		private bool MatchesState(BaseNaJieGoodEntity g)
		{
			switch (state)
			{
				case 1: return CanChoose(g); case 2: return g.IsLock;
				case 3: return g.IsBinding; case 4: return g.IsUsing; case 5: return g.OfferPending;
				default: return true;
			}
		}

		private void Redraw(bool top)
		{
			string query = search.text.Trim();
			IEnumerable<BaseNaJieGoodEntity> visible = sectionItems.Where(g => (category == 0 || RingGoodsCatalog.Section(g) == category) && MatchesState(g));
			if (query.Length > 0)
			{
				visible = visible.Where(g => string.Join(" ", g.Name, RingGoodsCatalog.Kind(g), WorldSetting.GetRankAllName(g.Rank),
					WorldSetting.GetSignleWuXingName((WuXing)g.FiveElement), Status(g)).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
			}
			var shown = OrderedForDisplay(visible).ToList();
			var visibleIds = new HashSet<string>(shown.Select(g => g.GoodsId));
			foreach (var existing in rows)
				if (existing.Value.Button.gameObject.activeSelf && !visibleIds.Contains(existing.Key))
					existing.Value.Button.gameObject.SetActive(false);
			PracticeBook = null;
			for (int i = 0; i < shown.Count; i++)
			{
				var g = shown[i];
				if (!rows.TryGetValue(g.GoodsId, out var row))
				{
					row = NewRow();
					rows.Add(g.GoodsId, row);
				}
				row.Item = g;
				row.Button.gameObject.SetActive(true);
				Place((RectTransform)row.Button.transform, 0, 1, 3, -7, 2 + RowHeight*i, RowHeight-2);
				UpdateRow(row, marked.Contains(g.GoodsId));
				if (PracticeGuideFlow.IsStarter(g)) PracticeBook = row.Button;
			}
			panel.NaJieListConstant.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4 + shown.Count*RowHeight);
			if (top)
			{
				scroll.StopMovement();
				var content = panel.NaJieListConstant;
				content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0f);
			}
			for (int i = 0; i < tabs.Count; i++)
			{
				var current = category == i;
				InkDialogArt.StyleButton(tabs[i], current ? "JadeButton" : "LightPlaque", current ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
			}
			UpdateListIndicator();
			sortButton.GetComponentInChildren<Text>().text = SortCaption;
			stateButton.GetComponentInChildren<Text>().text = States[state];
			long quantity = shown.Sum(g => (long)g.GoodsNum);
			long selected = Selected.Sum(g => (long)g.GoodsNum);
			modeBand.gameObject.SetActive(Choosing);
			summary.color = Choosing ? InkDialogArt.Ivory : InkInterfaceArt.Ink;
			summary.text = Choosing ? $"勾选出售 · 已选 {selected} 件 · {Selected.Sum(g => (decimal)g.RecyclePrice * g.GoodsNum)} 灵石" : $"{Sections[category]} · {shown.Count} 格 / {quantity} 件";
			InkDialogArt.StyleButton(panel.YiJianChuShouBtn, Choosing ? "JadeButton" : "LightPlaque", Choosing ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(panel.ShengJiBtn, Choosing ? "LightPlaque" : "JadeButton", Choosing ? InkInterfaceArt.Ink : InkDialogArt.Ivory);
			saleCaption.text = Choosing ? "确认出售" : "选物出售";
			upgradeCaption.text = Choosing ? "取消选择" : "扩充纳戒";
			panel.YiJianChuShouBtn.interactable = !Choosing || selected > 0;
			empty.text = items.Count == 0 ? "纳戒中暂无物品" : "没有符合条件的物品";
			empty.gameObject.SetActive(shown.Count == 0);
		}

		private static GameObject Node(Transform parent, string name)
		{
			var obj = new GameObject(name, typeof(RectTransform));
			obj.layer = parent.gameObject.layer;
			obj.transform.SetParent(parent, false);
			return obj;
		}

		private Text Label(Transform parent, string name, int size, TextAnchor align)
		{
			var text = Node(parent, name).AddComponent<Text>();
			text.font = Font;
			text.fontSize = size;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = Mathf.Min(11, size);
			text.resizeTextMaxSize = size;
			text.supportRichText = false;
			text.color = InkInterfaceArt.Ink;
			text.alignment = align;
			text.raycastTarget = false;
			return text;
		}

		private Button Control(Transform parent, string name, string title, UnityAction click)
		{
			var obj = Node(parent, name);
			var image = obj.AddComponent<Image>();
			var button = obj.AddComponent<Button>();
			button.targetGraphic = image;
			var caption = Label(obj.transform, "Caption", 14, TextAnchor.MiddleCenter);
			caption.text = title;
			InkDialogArt.Inset(caption.rectTransform, 2, 2, 0, 0);
			InkDialogArt.StyleButton(button, "LightPlaque", InkInterfaceArt.Ink);
			button.onClick.AddListener(click);
			return button;
		}

		private static void Place(RectTransform rect, float from, float to, float left, float right, float top, float height)
		{
			rect.anchorMin = new Vector2(from, 1);
			rect.anchorMax = new Vector2(to, 1);
			rect.offsetMin = new Vector2(left, -top-height);
			rect.offsetMax = new Vector2(right, -top);
		}
	}
}
