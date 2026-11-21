using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkMainHud
	{
		internal static void Apply(MainTitlePanel panel)
		{
			SetBarSurface(panel.transform.Find("TopBg")?.GetComponent<Image>());
			StyleText(panel.Name_text, InkInterfaceArt.Ink, 17);
			StyleText(panel.Level_text, InkInterfaceArt.SoftInk, 12);
			StyleText(panel.TiLi_Text, InkInterfaceArt.SoftInk, 12);
			StyleCurrency(panel.DuiHuanLingShi_btn, panel.LingShi_text, "LinShi_text", "SilverIngot", false);
			StyleCurrency(panel.DuiHuanLingYu_btn, panel.LingYu_text, "XianYu_text", "JadePendant", true);
			RectTransform dot = panel.AddRedPointImg.rectTransform;
			dot.anchorMin = dot.anchorMax = Vector2.one;
			dot.anchoredPosition = new Vector2(-4f, -3f);
			dot.SetAsLastSibling();
			HudEmblemArt.Apply(panel);

			SetBarSurface(panel.Bottom_img);
			RectTransform bar = panel.Bottom_img.rectTransform;
			StyleNavigation(panel.DongFu_btn, bar.Find("DongFuImg"), "Pavilion", 0);
			StyleNavigation(panel.NaJie_btn, bar.Find("NaJieImg"), "Satchel", 1);
			StyleNavigation(panel.LiLian_btn, bar.Find("LiLianImg"), "FoldedMap", 2);
			StyleNavigation(panel.ZhanDou_btn, bar.Find("ZhanDouImg"), "Swords", 3);
			StyleNavigation(panel.CanWu_btn, bar.Find("CanWuImg"), "Book", 4);
			StyleNavigation(panel.DaoYou_btn, bar.Find("DaoYouImg"), "Chat", 5);
			if (!panel.TryGetComponent<InkHudMotion>(out _))
				panel.gameObject.AddComponent<InkHudMotion>();
		}

		private static void SetBarSurface(Image surface)
		{
			if (surface == null)
				return;
			surface.sprite = null;
			surface.overrideSprite = null;
			surface.type = Image.Type.Simple;
			surface.color = new Color32(251, 246, 232, 255);
		}

		private static void StyleCurrency(Button button, Text value, string labelName, string icon, bool jade)
		{
			var row = (RectTransform)button.transform;
			row.anchorMin = row.anchorMax = Vector2.one;
			row.pivot = new Vector2(1f, .5f);
			row.anchoredPosition = new Vector2(-6f, jade ? -78f : -50f);
			row.sizeDelta = new Vector2(128f, 26f);
			var paper = button.GetComponent<Image>();
			InkInterfaceArt.Set(paper, "BrushPlaque", false);
			paper.color = jade ? new Color32(216, 235, 216, 255) : new Color32(251, 235, 204, 255);
			paper.raycastTarget = true;
			// Both original click handlers live on these buttons. The legacy jade Button is disabled in the prefab.
			button.enabled = true;
			button.targetGraphic = paper;
			var oldPlus = row.Find("icon");
			if (oldPlus != null) oldPlus.gameObject.SetActive(false);

			Image art = ChildGraphic<Image>(row, "CurrencyArt");
			InkInterfaceArt.Set(art, icon);
			PlaceInRow(art.rectTransform, 3f, 27f);

			var caption = (row.Find(labelName) ?? row.parent.Find(labelName)).GetComponent<Text>();
			caption.rectTransform.SetParent(row, false);
			PlaceInRow(caption.rectTransform, 31f, 25f);
			Color ink = jade ? new Color32(49, 91, 77, 255) : new Color32(106, 78, 48, 255);
			StyleText(caption, ink, 11);
			caption.alignment = TextAnchor.MiddleLeft;
			caption.raycastTarget = false;

			var balance = (RectTransform)value.transform.parent;
			balance.SetParent(row, false);
			balance.GetComponent<Image>().enabled = false;
			PlaceInRow(balance, 57f, 52f);
			InkDialogArt.Inset(value.rectTransform, 0f, 0f, 2f, 2f);
			StyleText(value, InkInterfaceArt.Ink, 14);
			value.alignment = TextAnchor.MiddleRight;
			value.raycastTarget = false;

			Text more = ChildGraphic<Text>(row, "CurrencyAction");
			PlaceInRow(more.rectTransform, 111f, 14f);
			more.text = "+";
			more.font = caption.font;
			more.fontSize = 14;
			more.color = ink;
			more.alignment = TextAnchor.MiddleCenter;
		}

		private static T ChildGraphic<T>(RectTransform parent, string name) where T : Graphic
		{
			var existing = parent.Find(name);
			if (existing != null) return existing.GetComponent<T>();
			var node = new GameObject(name, typeof(RectTransform), typeof(T));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			var graphic = node.GetComponent<T>();
			graphic.raycastTarget = false;
			return graphic;
		}

		private static void PlaceInRow(RectTransform rect, float left, float width)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.up;
			rect.pivot = new Vector2(.5f, .5f);
			rect.offsetMin = new Vector2(left, 0f);
			rect.offsetMax = new Vector2(left + width, 0f);
		}

		private static void StyleNavigation(Button button, Transform caption, string icon, int column)
		{
			// Keep the existing Button as the navigation target for callbacks and guides.
			var cell = (RectTransform)button.transform;
			cell.anchorMin = new Vector2(column / 6f, 0f);
			cell.anchorMax = new Vector2((column + 1) / 6f, 1f);
			cell.offsetMin = new Vector2(1f, 4f);
			cell.offsetMax = new Vector2(-1f, -4f);
			Image hit = button.GetComponent<Image>();
			hit.sprite = hit.overrideSprite = null;
			hit.type = Image.Type.Simple;
			hit.color = Color.clear;
			hit.raycastTarget = true;
			hit.raycastPadding = Vector4.zero;

			Image art = cell.Find("NavigationArt")?.GetComponent<Image>();
			if (art == null)
			{
				var node = new GameObject("NavigationArt", typeof(RectTransform), typeof(Image));
				node.layer = cell.gameObject.layer;
				node.transform.SetParent(cell, false);
				art = node.GetComponent<Image>();
			}
			art.rectTransform.anchorMin = new Vector2(0.14f, 0.41f);
			art.rectTransform.anchorMax = new Vector2(0.86f, 0.89f);
			art.rectTransform.offsetMin = art.rectTransform.offsetMax = Vector2.zero;
			InkInterfaceArt.Set(art, icon);
			art.raycastTarget = false;
			button.targetGraphic = art;

			RectTransform labelRect = (RectTransform)caption;
			labelRect.anchorMin = new Vector2(column / 6f, 0.08f);
			labelRect.anchorMax = new Vector2((column + 1) / 6f, 0.40f);
			labelRect.offsetMin = new Vector2(1f, 0f);
			labelRect.offsetMax = new Vector2(-1f, 0f);
			labelRect.SetAsLastSibling();
			Image plate = caption.GetComponent<Image>();
			if (plate != null)
			{
				plate.color = Color.clear;
				plate.raycastTarget = false;
			}
			Text label = caption.GetComponentInChildren<Text>(true);
			InkDialogArt.Inset(label.rectTransform, 1f, 1f, 0f, 0f);
			StyleText(label, InkInterfaceArt.Ink, 14);
			label.alignment = TextAnchor.MiddleCenter;
			label.raycastTarget = false;
		}

		internal static void SetNavigationState(Button button, Sprite icon, bool open)
		{
			button.image.sprite = icon;
			button.GetComponent<Image>().color = open ? new Color32(221, 230, 210, 255) : Color.clear;
			button.GetComponentInParent<InkHudMotion>(true)?.SetOpen(button.image, open);
		}

		private static void StyleText(Text label, Color ink, int size)
		{
			label.color = ink;
			label.fontSize = size;
			label.resizeTextForBestFit = true;
			label.resizeTextMaxSize = size;
			label.resizeTextMinSize = 9;
		}
	}
}
