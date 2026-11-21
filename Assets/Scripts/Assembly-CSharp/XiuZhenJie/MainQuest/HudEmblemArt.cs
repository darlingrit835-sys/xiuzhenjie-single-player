using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class HudEmblemArt
	{
		internal static void Apply(MainTitlePanel panel)
		{
			Image wanderer = panel.MenPaiIcon.GetComponent<Image>();
			SetTablet(wanderer, "Wanderer");
			wanderer.raycastTarget = true;
			var caption = wanderer.transform.Find("WandererCaption")?.GetComponent<Text>();
			if (caption == null)
			{
				var node = new GameObject("WandererCaption", typeof(RectTransform), typeof(Text));
				node.layer = wanderer.gameObject.layer;
				node.transform.SetParent(wanderer.transform, false);
				caption = node.GetComponent<Text>();
			}
			caption.font = panel.Name_text.font;
			caption.text = "散修";
			SetInscription(caption, -.5f);

			SetSect(panel.ZongMenIconBtn_1, panel.ZongMenSimpleNameText_1, panel.ZongMenNameIconImg_1, "Sun");
			SetSect(panel.ZongMenIconBtn_2, panel.ZongMenSimpleNameText_2, panel.ZongMenNameIconImg_2, "Moon");
			SetSect(panel.ZongMenIconBtn_3, panel.ZongMenSimpleNameText_3, panel.ZongMenNameIconImg_3, "Star");
			SetSect(panel.ZongMenIconBtn_4, panel.ZongMenSimpleNameText_4, panel.ZongMenNameIconImg_4, "Iron");
		}

		private static void SetSect(Button button, Text name, Image suffix, string art)
		{
			SetTablet(button.image, art);
			SetInscription(name, 2f);
			SetCenteredRect(suffix.rectTransform, 8f, 8f, -8.5f);
			suffix.preserveAspect = true;
			suffix.color = InkInterfaceArt.Seal;
			suffix.raycastTarget = false;
		}

		private static void SetTablet(Image image, string art)
		{
			InkInterfaceArt.Set(image, "HudEmblems/" + art, false);
			RectTransform rect = image.rectTransform;
			rect.anchorMin = rect.anchorMax = new Vector2(0f, 1f);
			rect.pivot = new Vector2(.5f, .5f);
			rect.anchoredPosition = new Vector2(20f, -68.5f);
			rect.sizeDelta = new Vector2(34f, 48f);
		}

		private static void SetInscription(Text caption, float heightFromCenter)
		{
			SetCenteredRect(caption.rectTransform, 28f, 12f, heightFromCenter);
			caption.alignment = TextAnchor.MiddleCenter;
			caption.fontSize = 10;
			caption.resizeTextForBestFit = true;
			caption.resizeTextMinSize = 6;
			caption.resizeTextMaxSize = 10;
			caption.horizontalOverflow = HorizontalWrapMode.Wrap;
			caption.verticalOverflow = VerticalWrapMode.Truncate;
			caption.color = InkInterfaceArt.Ink;
			caption.raycastTarget = false;
		}

		private static void SetCenteredRect(RectTransform rect, float width, float height, float y)
		{
			rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(.5f, .5f);
			rect.sizeDelta = new Vector2(width, height);
			rect.anchoredPosition = new Vector2(0f, y);
		}
	}
}
