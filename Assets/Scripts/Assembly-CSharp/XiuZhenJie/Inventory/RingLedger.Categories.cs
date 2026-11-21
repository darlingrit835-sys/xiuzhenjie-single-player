using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed partial class RingLedger
	{
		private string categoryRoleKey;
		// Signed section IDs keep hidden tabs in their chosen positions.
		private int[] savedTabs, draftTabs;
		private IReadOnlyList<int> visibleTabs;
		private GameObject categoryOverlay;
		private readonly Dictionary<int, CategoryOption> categoryRows = new Dictionary<int, CategoryOption>();

		private sealed class CategoryOption
		{
			internal RectTransform row, mark;
			internal Button toggle, up, down;
		}

		internal IReadOnlyList<int> CategoryOrder
		{
			get
			{
				ReloadCategories();
				return visibleTabs;
			}
		}

		internal bool ReloadCategories()
		{
			var key = RingSortPreferences.RolePreferenceKey("RingSections");
			if (savedTabs != null && categoryRoleKey == key) return false;
			CloseCategoryOptions();
			categoryRoleKey = key;
			var tabs = new List<int>();
			if (key != null)
				foreach (string value in PlayerPrefs.GetString(key, "").Split(','))
				{
					if (!int.TryParse(value, out var tab) || tab == 0 || tab <= -Sections.Length || tab >= Sections.Length) continue;
					if (!tabs.Contains(tab) && !tabs.Contains(-tab)) tabs.Add(tab);
				}
			for (int i = 1; i < Sections.Length; i++)
				if (!tabs.Contains(i) && !tabs.Contains(-i)) tabs.Add(i);
			SetSavedTabs(tabs.ToArray());
			return true;
		}

		private void SetSavedTabs(int[] tabs)
		{
			savedTabs = tabs;
			visibleTabs = new[] { 0 }.Concat(tabs.Where(t => t > 0)).ToArray();
		}

		private void OpenCategoryOptions()
		{
			ReloadCategories();
			if (categoryOverlay != null) return;
			draftTabs = (int[])savedTabs.Clone();
			categoryOverlay = Node(panel.transform, "RingCategoryOptions");
			InkDialogArt.Inset((RectTransform)categoryOverlay.transform, 0, 0, 0, 0);
			categoryOverlay.AddComponent<Image>();
			InkDialogArt.SetWash(categoryOverlay.transform);
			var sheet = Node(categoryOverlay.transform, "CategorySheet");
			var rect = (RectTransform)sheet.transform;
			rect.anchorMin = new Vector2(.025f, .5f);
			rect.anchorMax = new Vector2(.975f, .5f);
			rect.sizeDelta = new Vector2(0, 348);
			rect.anchoredPosition = Vector2.zero;
			InkDialogArt.SetPaper(sheet.AddComponent<Image>());

			var heading = Label(sheet.transform, "Heading", 20, TextAnchor.MiddleLeft);
			heading.fontStyle = FontStyle.Bold;
			heading.text = "常用分类";
			Place(heading.rectTransform, 0, 1, 15, -90, 8, 34);
			var reset = Control(rect, "DefaultTabs", "恢复默认", () => {
				draftTabs = Enumerable.Range(1, Sections.Length - 1).ToArray();
				RefreshCategoryOptions();
			});
			Place((RectTransform)reset.transform, 1, 1, -85, -12, 10, 32);
			var help = Label(rect, "Hint", 13, TextAnchor.MiddleLeft);
			help.text = "勾选常用分类，拖动名称调整顺序。「全部」始终显示。";
			Place(help.rectTransform, 0, 1, 15, -12, 47, 29);

			for (int i = 1; i < Sections.Length; i++)
			{
				int id = i;
				var row = new CategoryOption { row = (RectTransform)Node(rect, "CategoryOption" + id).transform };
				row.toggle = Control(row.row, "TabToggle" + id, Sections[id], () => {
					int at = TabIndex(id);
					draftTabs[at] = -draftTabs[at];
					RefreshCategoryOptions();
				});
				RingOrderDrag.Attach(row.toggle, rect, 42f, delta => MoveTab(id, delta));
				Place((RectTransform)row.toggle.transform, 0, .6f, 0, -2, 0, 37);
				var check = Node(row.toggle.transform, "CheckBox").AddComponent<Image>();
				check.raycastTarget = false;
				InkInterfaceArt.Set(check, "LightPlaque", false);
				Place(check.rectTransform, 0, 0, 6, 26, 9, 19);
				row.mark = (RectTransform)Node(check.transform, "Mark").transform;
				InkDialogArt.Inset(row.mark, 0, 0, 0, 0);
				CheckStroke(row.mark, "Downstroke", new Vector2(-3, -1.5f), 6, -45);
				CheckStroke(row.mark, "Upstroke", new Vector2(2, .6f), 12, 48);
				var mount = Node(row.toggle.transform, "IconPaper").AddComponent<Image>();
				InkInterfaceArt.Set(mount, "LightPaper", false);
				mount.raycastTarget = false;
				Place(mount.rectTransform, 0, 0, 30, 60, 3, 31);
				var art = Node(row.toggle.transform, "Icon").AddComponent<Image>();
				art.raycastTarget = false;
				art.preserveAspect = true;
				art.sprite = RingGoodsCatalog.SectionArtwork(id);
				art.color = RingGoodsCatalog.ArtTint(art.sprite);
				Place(art.rectTransform, 0, 0, 32, 58, 5, 27);
				var caption = row.toggle.GetComponentInChildren<Text>();
				InkDialogArt.Inset(caption.rectTransform, 63, 4, 0, 0);
				caption.alignment = TextAnchor.MiddleLeft;
				row.up = Control(row.row, "TabUp" + id, "上移", () => MoveTab(id, -1));
				row.down = Control(row.row, "TabDown" + id, "下移", () => MoveTab(id, 1));
				Place((RectTransform)row.up.transform, .6f, .8f, 2, -2, 0, 37);
				Place((RectTransform)row.down.transform, .8f, 1, 2, 0, 0, 37);
				categoryRows.Add(id, row);
			}
			var cancel = Control(rect, "CancelTabs", "取消", CloseCategoryOptions);
			var apply = Control(rect, "ApplyTabs", "应用分类", ApplyCategoryOptions);
			Place((RectTransform)cancel.transform, 0, .5f, 11, -3, 294, 40);
			Place((RectTransform)apply.transform, .5f, 1, 3, -11, 294, 40);
			InkDialogArt.StyleButton(apply, "JadeButton", InkDialogArt.Ivory);
			RefreshCategoryOptions();
		}

		private int TabIndex(int id) => Array.FindIndex(draftTabs, tab => Math.Abs(tab) == id);

		private void MoveTab(int id, int delta)
		{
			int old = TabIndex(id), next = old + delta;
			if (next < 0 || next >= draftTabs.Length) return;
			var moved = draftTabs[old];
			draftTabs[old] = draftTabs[next];
			draftTabs[next] = moved;
			RefreshCategoryOptions();
		}

		private void RefreshCategoryOptions()
		{
			for (int i = 0; i < draftTabs.Length; i++)
			{
				int tab = draftTabs[i];
				var row = categoryRows[Math.Abs(tab)];
				Place(row.row, 0, 1, 11, -11, 79 + 42 * i, 40);
				bool enabled = tab > 0;
				row.mark.gameObject.SetActive(enabled);
				InkInterfaceArt.Set(row.mark.parent.GetComponent<Image>(), enabled ? "JadeButton" : "LightPlaque", false);
				InkDialogArt.StyleButton(row.toggle, enabled ? "JadeButton" : "LightPlaque", enabled ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
				row.up.interactable = i > 0;
				row.down.interactable = i < draftTabs.Length - 1;
			}
		}

		private void ApplyCategoryOptions()
		{
			if (categoryRoleKey == RingSortPreferences.RolePreferenceKey("RingSections"))
			{
				SetSavedTabs(draftTabs);
				if (categoryRoleKey != null)
				{
					PlayerPrefs.SetString(categoryRoleKey, string.Join(",", savedTabs));
					PlayerPrefs.Save();
				}
			}
			CloseCategoryOptions();
			RefreshCategoryStrip();
		}

		internal void CloseCategoryOptions()
		{
			if (categoryOverlay != null)
			{
				categoryOverlay.SetActive(false);
				UnityEngine.Object.Destroy(categoryOverlay);
			}
			categoryOverlay = null;
			draftTabs = null;
			categoryRows.Clear();
		}
	}
}
