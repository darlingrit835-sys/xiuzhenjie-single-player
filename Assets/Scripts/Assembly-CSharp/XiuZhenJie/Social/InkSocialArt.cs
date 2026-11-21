using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkSocialArt
	{
		private const string BackdropName = "InkSocialBackdrop";
		private static Sprite landscape;

		internal static void ApplyBackdrop(Transform root, Image background = null)
		{
			if (landscape == null)
				landscape = ReleasedResources.Load<Sprite>("InkSocial/CompanionLandscape") ??
					ReleasedResources.Load<Sprite>("InkBrightLandscape");
			if (landscape == null || root == null)
				return;

			if (background == null)
			{
				var existing = root.Find(BackdropName);
				if (existing == null)
				{
					var canvas = new GameObject(BackdropName, typeof(RectTransform), typeof(Image));
					canvas.layer = root.gameObject.layer;
					canvas.transform.SetParent(root, false);
					canvas.transform.SetAsFirstSibling();
					background = canvas.GetComponent<Image>();
					RectTransform rect = background.rectTransform;
					rect.anchorMin = Vector2.zero;
					rect.anchorMax = Vector2.one;
					rect.offsetMin = rect.offsetMax = Vector2.zero;
					var fit = canvas.AddComponent<AspectRatioFitter>();
					fit.aspectRatio = landscape.rect.width / landscape.rect.height;
					fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
				}
				else
				{
					background = existing.GetComponent<Image>();
				}
			}
			background.sprite = landscape;
			background.overrideSprite = null;
			background.type = Image.Type.Simple;
			background.preserveAspect = false;
			background.color = Color.white;
			background.raycastTarget = false;
		}

		internal static void StyleInput(InputField input, string placeholder)
		{
			InkInterfaceArt.Set(input.image, "LightPaper", false);
			input.textComponent.color = InkInterfaceArt.Ink;
			input.customCaretColor = true;
			input.caretColor = InkInterfaceArt.Ink;
			input.selectionColor = new Color32(174, 207, 191, 135);
			if (input.placeholder is Text hint)
			{
				hint.color = InkInterfaceArt.SoftInk;
				hint.text = placeholder;
			}
		}

		internal static void StyleQuietMenu(Button button)
		{
			DaoYouItem.StyleAction(button, "···");
			button.image.overrideSprite = null;
			button.image.sprite = null;
			button.image.color = new Color32(207, 221, 209, 104);
			var caption = button.transform.Find("Text")?.GetComponent<Text>() ?? button.GetComponentInChildren<Text>(true);
			if (caption != null)
				DaoYouItem.StyleText(caption, 18, InkInterfaceArt.SoftInk);
		}

		internal static void LayoutChatBubble(RectTransform row, RectTransform bubble, ContentSizeFitter fitter,
			Text message, Image portrait, Text time, bool own)
		{
			fitter.enabled = false;
			if (bubble.GetComponent<LayoutGroup>() is LayoutGroup layout)
				layout.enabled = false;
			if (message.GetComponent<ContentSizeFitter>() is ContentSizeFitter messageFitter)
				messageFitter.enabled = false;

			var image = bubble.GetComponent<Image>();
			InkDialogArt.SetPaper(image);
			image.color = own ? new Color32(208, 233, 218, 255) : new Color32(255, 249, 235, 255);
			image.raycastTarget = false;
			message.fontSize = 14;
			message.lineSpacing = 1.16f;
			message.resizeTextForBestFit = false;
			message.color = InkInterfaceArt.Ink;
			message.alignment = TextAnchor.UpperLeft;
			message.horizontalOverflow = HorizontalWrapMode.Wrap;
			message.verticalOverflow = VerticalWrapMode.Overflow;
			message.raycastTarget = false;

			float available = ((RectTransform)row.parent).rect.width;
			float limit = Mathf.Clamp(available - 105f, 95f, 310f);
			float width = Mathf.Clamp(message.preferredWidth + 24f, 112f, Mathf.Max(112f, limit));
			float side = own ? 1f : 0f;
			float inset = own ? -49f : 49f;
			bubble.anchorMin = bubble.anchorMax = new Vector2(side, 1f);
			bubble.pivot = new Vector2(side, 1f);
			bubble.sizeDelta = new Vector2(width, 38f);
			bubble.anchoredPosition = new Vector2(inset, -19f);
			InkDialogArt.Inset(message.rectTransform, 12f, 12f, 9f, 9f);

			var avatar = portrait.rectTransform;
			avatar.anchorMin = avatar.anchorMax = new Vector2(side, 1f);
			avatar.pivot = new Vector2(side, 1f);
			avatar.anchoredPosition = new Vector2(own ? -8f : 8f, -19f);
			avatar.sizeDelta = new Vector2(34f, 34f);
			portrait.preserveAspect = true;
			portrait.raycastTarget = false;

			DaoYouItem.StyleText(time, 10, InkInterfaceArt.SoftInk, own ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft);
			time.raycastTarget = false;
			time.rectTransform.SetParent(row, false);
			time.rectTransform.anchorMin = time.rectTransform.anchorMax = new Vector2(side, 1f);
			time.rectTransform.pivot = new Vector2(side, 1f);
			time.rectTransform.anchoredPosition = new Vector2(inset, -1f);
			time.rectTransform.sizeDelta = new Vector2(width, 16f);
		}

		internal static void FitChatBubbleHeight(RectTransform row, RectTransform bubble, Text message)
		{
			int columns = Mathf.Max(1, Mathf.FloorToInt((bubble.rect.width - 24f) / (message.fontSize * 1.05f)));
			int lines = 0;
			foreach (string paragraph in message.text.Split('\n'))
				lines += Mathf.Max(1, Mathf.CeilToInt((float)paragraph.Length / columns));
			float textHeight = Mathf.Max(message.preferredHeight, lines * message.fontSize * 1.55f);
			float height = Mathf.Max(38f, Mathf.Ceil(textHeight) + 20f);
			bubble.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			row.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 21f + Mathf.Max(34f, height));
		}
	}
}
