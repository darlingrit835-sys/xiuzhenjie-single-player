using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed partial class RingLedger
	{
		private static readonly string[] SortTitles = { "品阶", "等级", "类别", "数量", "单价", "名称" };
		private string sortRoleKey;
		private RingSortPreferences savedSort, sortDraft;
		private GameObject sortOverlay;
		private readonly Dictionary<RingSortField, SortRow> sortRows = new Dictionary<RingSortField, SortRow>();

		private sealed class SortRow
		{
			internal RectTransform rect;
			internal Button select, direction, up, down;
			internal RingOrderDrag drag;
			internal Image box;
			internal RectTransform mark;
		}

		private RingSortPreferences CurrentSort
		{
			get
			{
				var current = RingSortPreferences.CurrentRoleKey();
				if (savedSort == null || current != sortRoleKey)
				{
					CloseSortOptions();
					sortRoleKey = current;
					savedSort = RingSortPreferences.Load(current);
				}
				return savedSort;
			}
		}

		private string SortCaption => CurrentSort.SelectedCount == 0 ? "多项排序" : $"排序 · {savedSort.SelectedCount} 项";

		private IEnumerable<BaseNaJieGoodEntity> OrderedForDisplay(IEnumerable<BaseNaJieGoodEntity> visible)
		{
			return CurrentSort.Order(visible, guide);
		}

		private void OpenSortOptions()
		{
			var applied = CurrentSort;
			if (sortOverlay != null) return;
			sortDraft = applied.Copy();
			sortOverlay = Node(panel.transform, "RingSortOptions");
			InkDialogArt.Inset((RectTransform)sortOverlay.transform, 0, 0, 0, 0);
			sortOverlay.AddComponent<Image>();
			InkDialogArt.SetWash(sortOverlay.transform);
			var page = Node(sortOverlay.transform, "SortSheet");
			var rect = (RectTransform)page.transform;
			rect.anchorMin = new Vector2(.025f, .5f);
			rect.anchorMax = new Vector2(.975f, .5f);
			rect.sizeDelta = new Vector2(0f, 382f);
			rect.anchoredPosition = Vector2.zero;
			InkDialogArt.SetPaper(page.AddComponent<Image>());
			var title = Label(rect, "SortHeading", 20, TextAnchor.MiddleLeft);
			title.text = "纳戒排序";
			title.fontStyle = FontStyle.Bold;
			Place(title.rectTransform, 0, 1, 15, -78, 9, 32);
			var clear = Control(rect, "ClearSort", "清空", () => {
				foreach (var rule in sortDraft.Rules) rule.selected = false;
				RefreshSortOptions();
			});
			Place((RectTransform)clear.transform, 1, 1, -65, -12, 10, 31);
			var help = Label(rect, "SortHint", 13, TextAnchor.MiddleLeft);
			help.text = "拖动已勾选的名称调整优先级。全不选则按入戒顺序。";
			Place(help.rectTransform, 0, 1, 15, -10, 46, 26);
			foreach (var rule in sortDraft.Rules)
			{
				var row = new SortRow { rect = (RectTransform)Node(rect, "SortRow" + rule.field).transform };
				row.select = Control(row.rect, "SortToggle" + rule.field, "", () => { sortDraft.Toggle(rule); RefreshSortOptions(); });
				row.drag = RingOrderDrag.Attach(row.select, rect, 40f, delta => { sortDraft.Move(rule, delta); RefreshSortOptions(); });
				row.direction = Control(row.rect, "SortDirection" + rule.field, "", () => { rule.descending = !rule.descending; RefreshSortOptions(); });
				row.up = Control(row.rect, "SortUp" + rule.field, "上移", () => { sortDraft.Move(rule, -1); RefreshSortOptions(); });
				row.down = Control(row.rect, "SortDown" + rule.field, "下移", () => { sortDraft.Move(rule, 1); RefreshSortOptions(); });
				Place((RectTransform)row.select.transform, 0, .39f, 0, -2, 1, 35);
				Place((RectTransform)row.direction.transform, .39f, .62f, 2, -2, 1, 35);
				Place((RectTransform)row.up.transform, .62f, .81f, 2, -2, 1, 35);
				Place((RectTransform)row.down.transform, .81f, 1, 2, 0, 1, 35);
				row.box = Node(row.select.transform, "Checkbox").AddComponent<Image>();
				row.box.raycastTarget = false;
				Place(row.box.rectTransform, 0, 0, 6, 25, 8, 19);
				row.mark = (RectTransform)Node(row.box.transform, "Checkmark").transform;
				InkDialogArt.Inset(row.mark, 0, 0, 0, 0);
				CheckStroke(row.mark, "Downstroke", new Vector2(-3, -1.5f), 6, -45);
				CheckStroke(row.mark, "Upstroke", new Vector2(2, .6f), 12, 48);
				InkDialogArt.Inset(row.select.GetComponentInChildren<Text>().rectTransform, 27, 3, 0, 0);
				sortRows.Add(rule.field, row);
			}
			var cancel = Control(rect, "CancelSort", "取消", CloseSortOptions);
			Place((RectTransform)cancel.transform, 0, .5f, 11, -3, 331, 39);
			var apply = Control(rect, "ApplySort", "应用排序", ApplySort);
			InkDialogArt.StyleButton(apply, "JadeButton", InkDialogArt.Ivory);
			Place((RectTransform)apply.transform, .5f, 1, 3, -11, 331, 39);
			RefreshSortOptions();
		}

		private void RefreshSortOptions()
		{
			int selected = sortDraft.SelectedCount;
			for (int i = 0; i < sortDraft.Rules.Count; i++)
			{
				var rule = sortDraft.Rules[i];
				var row = sortRows[rule.field];
				Place(row.rect, 0, 1, 11, -11, 77 + 40*i, 38);
				row.mark.gameObject.SetActive(rule.selected);
				row.drag.enabled = rule.selected;
				InkInterfaceArt.Set(row.box, rule.selected ? "JadeButton" : "LightPlaque", false);
				InkDialogArt.StyleButton(row.select, rule.selected ? "JadeButton" : "LightPlaque", rule.selected ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
				row.select.GetComponentInChildren<Text>().text = (rule.selected ? (i + 1) + "·" : "") + SortTitles[(int)rule.field];
				row.direction.GetComponentInChildren<Text>().text = rule.descending ? "降序" : "升序";
				SetSortControl(row.direction, rule.selected);
				SetSortControl(row.up, rule.selected && i > 0);
				SetSortControl(row.down, rule.selected && i + 1 < selected);
			}
		}

		private static void SetSortControl(Button button, bool available)
		{
			button.interactable = available;
			button.GetComponentInChildren<Text>().color = available ? InkInterfaceArt.Ink : InkInterfaceArt.SoftInk;
		}

		private void ApplySort()
		{
			// A retained panel may outlive a login. Never write its draft for a different character.
			if (sortRoleKey == RingSortPreferences.CurrentRoleKey())
			{
				savedSort = sortDraft;
				savedSort.Save(sortRoleKey);
			}
			CloseSortOptions();
			Redraw(true);
		}

		internal void CloseSortOptions()
		{
			if (sortOverlay != null)
			{
				sortOverlay.SetActive(false);
				Object.Destroy(sortOverlay);
			}
			sortOverlay = null;
			sortDraft = null;
			sortRows.Clear();
		}
	}
}
