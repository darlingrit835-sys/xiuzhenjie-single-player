using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkDialogArt
	{
		private static readonly Color Wash = new Color32(231, 242, 236, 191);
		internal static readonly Color Ivory = new Color32(255, 251, 243, 255);
		private static readonly Color Rule = new Color32(108, 141, 124, 77);
		private static readonly Color ParchmentShade = new Color32(14, 38, 36, 132);
		private static readonly Color ParchmentRule = new Color32(156, 128, 84, 130);
		internal const float DetailsWidth = 288f;

		internal static void StyleHelp(XiaoWenHaoPanel panel, UnityAction close)
		{
			SetWash(panel.transform, ParchmentShade);
			SetPaper(panel.XianShiYi, "Parchment");
			panel.ContentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320f);
			panel.ContentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 260f);

			var titleBar = (RectTransform)panel.NameBg.transform.parent;
			PlaceAtTop(titleBar, 14f, 52f, 12f, 34f);
			SetHeading(titleBar.GetComponent<Image>(), panel.Name, "ParchmentHeading");
			panel.Name.alignment = TextAnchor.MiddleLeft;
			panel.NameBg.color = Color.clear;
			Inset(panel.NameBg.rectTransform, 0f, 0f, 0f, 0f);
			Inset(panel.Name.rectTransform, 10f, 10f, 3f, 3f);
			if (panel.Name.TryGetComponent(out ContentSizeFitter titleFitter))
				titleFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

			AddRule(panel.ContentParent, 54f, ParchmentRule);
			AddClose(panel.ContentParent, close);
			Inset(panel.ContentBg, 20f, 20f, 63f, 20f);
			Inset(panel.ContentScrollView, 0f, 0f, 0f, 0f);
			Inset(panel.MAaskText, 0f, 0f, 0f, 0f);
			Text body = panel.text.GetComponent<Text>();
			SetCopy(body, scrolls: true);
			panel.text.anchorMin = new Vector2(0f, 1f);
			panel.text.anchorMax = Vector2.one;
			panel.text.pivot = new Vector2(0.5f, 1f);
			panel.text.anchoredPosition = Vector2.zero;
			panel.text.sizeDelta = new Vector2(-4f, panel.text.sizeDelta.y);

			foreach (Image variant in new[] { panel.XianShiLianDan, panel.XianShiZhanDo })
			{
				SetPaper(variant, "Parchment");
				foreach (Text label in variant.GetComponentsInChildren<Text>(true))
				{
					Image plaque = label.transform.parent.GetComponent<Image>();
					if (plaque != null && plaque.transform.parent != null &&
						plaque.transform.parent.name.StartsWith("Title", System.StringComparison.Ordinal))
						SetHeading(plaque, label, "ParchmentHeading");
					else
						label.color = InkInterfaceArt.Ink;
				}
				AddClose(variant.rectTransform, close);
			}
		}

		internal static void StyleAttention(AttentionWndPanel panel)
		{
			if (!(panel.ContentText.transform.parent is RectTransform card))
				return;
			SetWash(panel.transform);
			SetPaper(card.GetComponent<Image>());
			card.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320f);
			card.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 213f);
			var heading = card.Find("Title") as RectTransform;
			if (heading != null)
			{
				PlaceAtTop(heading, 14f, 52f, 12f, 34f);
				SetHeading(heading.GetComponent<Image>(), heading.GetComponentInChildren<Text>(true));
			}
			AddRule(card, 54f);
			Inset(panel.ContentText.rectTransform, 23f, 23f, 62f, 55f);
			SetCopy(panel.ContentText);
			panel.ContentText.alignment = TextAnchor.MiddleCenter;
			UIManager.FitTextToRect(panel.ContentText);
			StyleButton(panel.QueRenBtn, "SealButton", Ivory);
			StyleClose(panel.CloseBtn);
		}

		internal static void StylePopup(PopUpWndPanel panel)
		{
			SetWash(panel.transform, ParchmentShade);
			SetPaper(panel.ContentParent.GetComponent<Image>(), "Parchment");
			panel.ContentParent.anchorMin = panel.ContentParent.anchorMax = new Vector2(0.5f, 0.5f);
			panel.ContentParent.anchoredPosition = Vector2.zero;
			panel.ContentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320f);
			var heading = (RectTransform)panel.TitleText.transform.parent;
			PlaceAtTop(heading, 14f, 52f, 12f, 34f);
			Inset(panel.TitleText.rectTransform, 10f, 10f, 3f, 3f);
			SetHeading(heading.GetComponent<Image>(), panel.TitleText, "ParchmentHeading");
			panel.TitleText.alignment = TextAnchor.MiddleLeft;
			AddRule(panel.ContentParent, 54f, ParchmentRule);
			StyleButton(panel.SureBtn, "JadeButton", Ivory);
			StyleClose(panel.CloseBtn);
			Inset(panel.ContentBg, 20f, 20f, 63f, 77f);
			Inset(panel.ContentScrollView, 0f, 0f, 0f, 0f);
			SetCopy(panel.text.GetComponent<Text>(), scrolls: true);
			if (panel.ContentBg.TryGetComponent(out Image background))
				background.color = Color.clear;
			if (panel.ContentParent.Find("TitleLine")?.GetComponent<Image>() is Image line)
				line.color = Color.clear;
			if (panel.ContentParent.Find("bottomBg")?.GetComponent<Image>() is Image footer)
				footer.color = Color.clear;
		}

		internal static void StyleDetails(Transform overlay, Text title, Button close, ScrollRect scroll)
		{
			SetWash(overlay);
			var card = (RectTransform)scroll.transform.parent;
			card.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, DetailsWidth + 32f);
			SetPaper(card.GetComponent<Image>());
			ClearArt(card.Find("Bg"));
			ClearArt(card.Find("bottomImg"));

			var heading = (RectTransform)card.Find("Title_image");
			PlaceAtTop(heading, 16f, 53f, 12f, 34f);
			ClearArt(title.transform.parent.Find("Image"));
			if (title.transform.parent == heading)
				Inset(title.rectTransform, 8f, 8f, 2f, 2f);
			else
				PlaceAtTop(title.rectTransform, 24f, 61f, 14f, 30f);
			SetHeading(heading.GetComponent<Image>(), title);
			AddRule(card, 54f);
			StyleClose(close);

			Inset((RectTransform)scroll.transform, 16f, 16f, 62f, 82f);
			InkInterfaceArt.Set(scroll.GetComponent<Image>(), "LightPaper", false);
			var layout = scroll.content.GetComponent<VerticalLayoutGroup>();
			layout.padding = new RectOffset(0, 0, 2, 2);
			layout.spacing = 3f;
		}

		internal static void FitDetailRows(ScrollRect scroll)
		{
			RectTransform content = scroll.content;
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, content.GetComponent<VerticalLayoutGroup>().preferredHeight);
			scroll.verticalNormalizedPosition = 1f;
		}

		private static void ClearArt(Transform node)
		{
			if (node != null && node.TryGetComponent(out Image image))
			{
				image.color = Color.clear;
				image.raycastTarget = false;
			}
		}

		internal static void StyleAgreement(Transform overlay, Transform card, Text title, Text copy, Button agree, Button decline)
		{
			if (overlay == null || card == null)
				return;
			SetWash(overlay);
			SetPaper(card.GetComponent<Image>());
			if (card is RectTransform frame)
			{
				frame.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 316f);
				frame.anchoredPosition = Vector2.zero;
			}
			if (title != null)
			{
				Image plaque = title.transform.parent == card ? AddAgreementPlate(title.rectTransform, card)
					: title.transform.parent.GetComponent<Image>();
				SetHeading(plaque, title);
			}
			if (copy != null)
			{
				StyleAgreementCopy(card, copy);
			}
			Image separator = card.Find("TitleLine")?.GetComponent<Image>() ?? card.Find("Image")?.GetComponent<Image>();
			if (separator != null)
				separator.color = Rule;
			if (card.Find("BottomImg")?.GetComponent<Image>() is Image buttonBackground)
				buttonBackground.color = Color.clear;
			StyleButton(agree, "SealButton", Ivory);
			StyleButton(decline, "IvoryPlaque", InkInterfaceArt.Ink);
			if (card.Find("CloseBtn")?.GetComponent<Button>() is Button close)
				StyleButton(close, "Close", InkInterfaceArt.Ink);
			foreach (Transform child in card)
			{
				if (child.GetComponent<Text>() is Text label && label != title && label != copy)
					label.color = InkInterfaceArt.Ink;
			}
		}

		private static void StyleAgreementCopy(Transform card, Text copy)
		{
			SetCopy(copy);
			copy.fontSize = Mathf.Max(15, copy.fontSize);
			copy.resizeTextForBestFit = false;
			copy.alignment = TextAnchor.UpperLeft;
			copy.lineSpacing = 1.28f;
			if (copy.transform.parent == card)
			{
				Inset(copy.rectTransform, 22f, 22f, 69f, 107f);
			}
			else if (copy.transform.parent is RectTransform textContainer && textContainer.parent == card)
			{
				Inset(textContainer, 14f, 14f, 69f, 82f);
				Inset(copy.rectTransform, 0f, 0f, 0f, 0f);
				if (textContainer.TryGetComponent(out Image fill))
					fill.color = Color.clear;
			}
		}

		private static Image AddAgreementPlate(RectTransform title, Transform card)
		{
			Image image = AddImage(card, "InkTitlePlate");
			RectTransform rect = image.rectTransform;
			rect.SetSiblingIndex(title.GetSiblingIndex());
			rect.anchorMin = title.anchorMin;
			rect.anchorMax = title.anchorMax;
			rect.pivot = title.pivot;
			rect.anchoredPosition = title.anchoredPosition;
			rect.sizeDelta = title.sizeDelta + new Vector2(14f, 6f);
			return image;
		}

		private static void AddRule(RectTransform card, float top, Color? tint = null)
		{
			Image rule = AddImage(card, "InkDivider");
			rule.color = tint ?? Rule;
			PlaceAtTop(rule.rectTransform, 19f, 19f, top, 1f);
		}

		private static void AddClose(RectTransform card, UnityAction close)
		{
			Image icon = AddImage(card, "InkClose");
			icon.raycastTarget = true;
			Button button = icon.gameObject.AddComponent<Button>();
			button.targetGraphic = icon;
			button.onClick.AddListener(close);
			StyleClose(button);
		}

		private static void StyleClose(Button button)
		{
			var rect = (RectTransform)button.transform;
			rect.anchorMin = rect.anchorMax = Vector2.one;
			rect.sizeDelta = new Vector2(28f, 28f);
			rect.anchoredPosition = new Vector2(-27f, -29f);
			StyleButton(button, "Close", InkInterfaceArt.Ink);
		}

		private static Image AddImage(Transform parent, string name)
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(Image));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			Image image = node.GetComponent<Image>();
			image.raycastTarget = false;
			return image;
		}

		internal static void SetWash(Transform overlay, Color? shade = null)
		{
			if (!overlay.TryGetComponent(out Image image))
				return;
			image.sprite = null;
			image.overrideSprite = null;
			image.type = Image.Type.Simple;
			image.color = shade ?? Wash;
		}

		internal static void SetPaper(Image image, string art = "LightPaper")
		{
			if (image == null)
				return;
			InkInterfaceArt.Set(image, art, false);
			if (!image.TryGetComponent(out Shadow shadow))
				shadow = image.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color32(75, 106, 93, 51);
			shadow.effectDistance = new Vector2(0f, -3f);
		}

		private static void SetHeading(Image image, Text label, string art = "LightPlaque")
		{
			InkInterfaceArt.Set(image, art, false);
			if (label == null)
				return;
			label.color = InkInterfaceArt.Ink;
			label.alignment = TextAnchor.MiddleCenter;
			UIManager.FitTextToRect(label);
		}

		private static void SetCopy(Text label, bool scrolls = false)
		{
			if (label == null)
				return;
			label.color = InkInterfaceArt.Ink;
			label.horizontalOverflow = HorizontalWrapMode.Wrap;
			label.verticalOverflow = scrolls ? VerticalWrapMode.Overflow : VerticalWrapMode.Truncate;
			label.lineSpacing = 1.18f;
			if (scrolls)
			{
				label.alignment = TextAnchor.UpperLeft;
				label.resizeTextForBestFit = false;
			}
		}

		internal static void StyleButton(Button button, string art, Color labelColor)
		{
			if (button == null || button.image == null)
				return;
			InkInterfaceArt.Set(button.image, art, art == "Close");
			button.transition = Selectable.Transition.ColorTint;
			ColorBlock colors = ColorBlock.defaultColorBlock;
			colors.pressedColor = new Color32(218, 232, 222, 255);
			button.colors = colors;
			foreach (Text label in button.GetComponentsInChildren<Text>(true))
				label.color = labelColor;
		}

		private static void PlaceAtTop(RectTransform rect, float left, float right, float top, float height)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, -top - height);
			rect.offsetMax = new Vector2(-right, -top);
		}

		internal static void Inset(RectTransform rect, float left, float right, float top, float bottom)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, bottom);
			rect.offsetMax = new Vector2(-right, -top);
		}
	}
}
