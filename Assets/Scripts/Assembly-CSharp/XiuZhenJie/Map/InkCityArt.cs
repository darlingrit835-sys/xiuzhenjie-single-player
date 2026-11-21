using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkCityArt
	{
		private static readonly Color FinePaper = new Color32(251, 243, 220, 235);
		private static readonly Color SignLettering = new Color32(255, 243, 212, 255);

		internal static Button Apply(MainCityPanel panel)
		{
			var map = (RectTransform)panel.transform.Find("CityBg_image");
			InkInterfaceArt.Set(map.GetComponent<Image>(), "CityLandmarks", false);
			panel.transform.Find("GongGao_image").gameObject.SetActive(false);
			StyleHeading(panel);

			// Bounds are normalized against the CityLandmarks painting, measured from its top left.
			PlaceBuilding(panel.ChengZhuFuBtn, "城主府", 0.025f, 0.102f, 0.465f, 0.291f);
			PlaceBuilding(panel.XuanTianLouBtn, "玄天楼", 0.69f, 0.059f, 0.965f, 0.273f);
			PlaceBuilding(panel.ZhongJieFangShiBtn, "坊市", 0.015f, 0.306f, 0.421f, 0.455f);
			PlaceBuilding(panel.LingQuanJiuSiBtn, "酒肆", 0.55f, 0.282f, 0.984f, 0.464f);
			PlaceBuilding(panel.XuShenJieBtn, "虚神界", 0.027f, 0.466f, 0.424f, 0.657f);
			PlaceBuilding(panel.ChuanSongDaZhenBtn, "传送阵", 0.591f, 0.483f, 0.982f, 0.678f);

			var marker = panel.ChengZhuFuHintImg.rectTransform;
			marker.anchorMin = marker.anchorMax = new Vector2(0.78f, 0.85f);
			marker.anchoredPosition = Vector2.zero;
			panel.ChengZhuFuHintImg.raycastTarget = false;

			Button notice = MakeNoticeBoard(map, panel.TitleText.font);
			StyleTownTools(panel);
			CityLandmarkMotion.Attach(map, panel, notice);
			return notice;
		}

		private static void PlaceBuilding(Button button, string title, float left, float top, float right, float bottom)
		{
			var area = (RectTransform)button.transform;
			SetMapBounds(area, left, top, right, bottom);
			MakeHitArea(button.image);

			Text label = area.Find("Text").GetComponent<Text>();
			var plaque = MakeImage(area, "NamePlaque");
			InkInterfaceArt.Set(plaque, "InkPlaque", false);
			RectTransform sign = plaque.rectTransform;
			sign.anchorMin = sign.anchorMax = new Vector2(0.5f, 0f);
			sign.sizeDelta = new Vector2(title.Length * 13f + 14f, 19f);
			sign.anchoredPosition = new Vector2(0f, 11f);

			label.rectTransform.SetParent(sign, false);
			InkDialogArt.Inset(label.rectTransform, 3f, 3f, 1f, 1f);
			StyleLabel(label, title, SignLettering, 13);
			SetPressFeedback(button, plaque);
		}

		private static Button MakeNoticeBoard(RectTransform map, Font font)
		{
			Image hit = MakeImage(map, "NoticeBoard");
			SetMapBounds(hit.rectTransform, 0.285f, 0.722f, 0.716f, 0.921f);
			MakeHitArea(hit);
			Button button = hit.gameObject.AddComponent<Button>();

			Image plaque = MakeImage(hit.rectTransform, "NoticePlaque");
			InkInterfaceArt.Set(plaque, "InkPlaque", false);
			RectTransform sign = plaque.rectTransform;
			sign.anchorMin = sign.anchorMax = new Vector2(0.5f, 0.67f);
			sign.sizeDelta = new Vector2(64f, 19f);
			sign.anchoredPosition = Vector2.zero;
			Text label = MakeLabel(sign, font, "公告栏", SignLettering, 14);
			InkDialogArt.Inset(label.rectTransform, 3f, 3f, 1f, 1f);
			SetPressFeedback(button, plaque);
			return button;
		}

		private static void SetMapBounds(RectTransform rect, float left, float top, float right, float bottom)
		{
			rect.anchorMin = new Vector2(left, 1f - bottom);
			rect.anchorMax = new Vector2(right, 1f - top);
			rect.offsetMin = rect.offsetMax = Vector2.zero;
		}

		private static void MakeHitArea(Image image)
		{
			image.sprite = null;
			image.overrideSprite = null;
			image.type = Image.Type.Simple;
			image.color = Color.clear;
			image.raycastTarget = true;
		}

		private static void SetPressFeedback(Button button, Graphic visual)
		{
			button.targetGraphic = visual;
			button.transition = Selectable.Transition.ColorTint;
			var colors = ColorBlock.defaultColorBlock;
			colors.pressedColor = new Color32(190, 210, 185, 255);
			button.colors = colors;
		}

		private static void StyleHeading(MainCityPanel panel)
		{
			var heading = (RectTransform)panel.TitleText.transform.parent;
			heading.anchorMin = new Vector2(0f, 1f);
			heading.anchorMax = Vector2.one;
			heading.offsetMin = new Vector2(11f, -40f);
			heading.offsetMax = new Vector2(-11f, -5f);
			Image titleSurface = heading.GetComponent<Image>();
			titleSurface.color = Color.clear;
			titleSurface.raycastTarget = false;

			RectTransform name = panel.TitleText.rectTransform;
			name.anchorMin = Vector2.zero;
			name.anchorMax = new Vector2(0.58f, 1f);
			name.offsetMin = new Vector2(6f, 0f);
			name.offsetMax = Vector2.zero;
			StyleLabel(panel.TitleText, panel.TitleText.text, InkInterfaceArt.Ink, 26);
			panel.TitleText.alignment = TextAnchor.MiddleLeft;
			var halo = panel.TitleText.gameObject.AddComponent<Outline>();
			halo.effectColor = FinePaper;
			halo.effectDistance = new Vector2(0.65f, -0.65f);

			Text lifespan = panel.ShengYuShouYuanText;
			RectTransform chip = (RectTransform)lifespan.transform.parent;
			chip.anchorMin = chip.anchorMax = Vector2.one;
			chip.sizeDelta = new Vector2(98f, 24f);
			chip.anchoredPosition = new Vector2(-58f, -21f);
			InkInterfaceArt.Set(chip.GetComponent<Image>(), "ParchmentHeading", false);
			chip.GetComponent<Image>().color = FinePaper;
			chip.GetComponent<Image>().raycastTarget = false;
			InkDialogArt.Inset(lifespan.rectTransform, 5f, 5f, 1f, 1f);
			StyleLabel(lifespan, lifespan.text, InkInterfaceArt.Ink, 13);
		}

		private static void StyleTownTools(MainCityPanel panel)
		{
			RectTransform row = panel.CeHuaTitleRect;
			row.anchorMin = new Vector2(0f, 0f);
			row.anchorMax = new Vector2(1f, 0f);
			row.offsetMin = new Vector2(14f, 4f);
			row.offsetMax = new Vector2(-84f, 43f);
			Image surface = row.GetComponent<Image>();
			InkInterfaceArt.Set(surface, "Parchment", false);
			surface.color = FinePaper;
			surface.raycastTarget = false;

			var layout = row.gameObject.AddComponent<HorizontalLayoutGroup>();
			layout.padding = new RectOffset(4, 4, 0, 0);
			layout.childControlWidth = layout.childControlHeight = true;
			layout.childForceExpandWidth = layout.childForceExpandHeight = true;

			Font font = panel.TitleText.font;
			StyleTool(row.Find("QianDao"), panel.QianDaoBtn, font, "签到", "AttendanceSeal");
			StyleTool(row.Find("Rank"), panel.RankBtn, font, "排行", "Ranking");
			StyleTool(row.Find("GongLue"), panel.GongLueBtn, font, "攻略", "Scroll");
			StyleTool(row.Find("TianDaoLing"), panel.TianDaoLingBtn, font, "天道令", "Dispatch");
			StyleTool(row.Find("XuanTianLing"), panel.YueKe, font, "玄天令", "JadePendant");
			StyleTool(row.Find("GuangGao"), panel.HuoDongBtn, font, "观影", "Gift");
			StyleTool(row.Find("HuoDong"), panel.ReturnLiBaoBtn, font, "活动", "Gift");

			var mail = (RectTransform)panel.XinJianBtn.transform;
			mail.anchorMin = mail.anchorMax = new Vector2(1f, 0f);
			mail.sizeDelta = new Vector2(65f, 39f);
			mail.anchoredPosition = new Vector2(-47f, 23.5f);
			StyleTool(mail, panel.XinJianBtn, font, "传信", "Letter");
			InkInterfaceArt.Set(panel.XinJianBtn.gameObject.GetComponent<Image>(), "Parchment", false);
			panel.XinJianBtn.gameObject.GetComponent<Image>().color = FinePaper;
		}

		private static void StyleTool(Transform slot, Button button, Font font, string title, string icon)
		{
			if (slot != button.transform)
				InkDialogArt.Inset((RectTransform)button.transform, 0f, 0f, 0f, 0f);

			Image oldButtonArt = button.image;
			MakeHitArea(oldButtonArt);
			Image slotArt = slot.GetComponent<Image>();
			if (slotArt != null && slotArt != oldButtonArt)
			{
				slotArt.color = Color.clear;
				slotArt.raycastTarget = false;
			}

			foreach (Text oldLabel in slot.GetComponentsInChildren<Text>(true))
				oldLabel.gameObject.SetActive(false);

			Image picture = MakeImage(button.transform, "ToolIllustration");
			InkInterfaceArt.Set(picture, icon);
			picture.rectTransform.anchorMin = new Vector2(0.15f, 0.42f);
			picture.rectTransform.anchorMax = new Vector2(0.85f, 0.99f);
			picture.rectTransform.offsetMin = picture.rectTransform.offsetMax = Vector2.zero;

			Text label = MakeLabel(button.transform, font, title, InkInterfaceArt.Ink, 11);
			label.rectTransform.anchorMin = Vector2.zero;
			label.rectTransform.anchorMax = new Vector2(1f, 0.44f);
			label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero;
			SetPressFeedback(button, picture);
		}

		private static Image MakeImage(Transform parent, string name)
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(Image));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			Image image = node.GetComponent<Image>();
			image.raycastTarget = false;
			return image;
		}

		private static Text MakeLabel(Transform parent, Font font, string title, Color color, int size)
		{
			var node = new GameObject(title, typeof(RectTransform), typeof(Text));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			Text label = node.GetComponent<Text>();
			label.font = font;
			StyleLabel(label, title, color, size);
			return label;
		}

		private static void StyleLabel(Text label, string title, Color color, int size)
		{
			label.text = title;
			label.color = color;
			label.fontSize = size;
			label.resizeTextForBestFit = true;
			label.resizeTextMinSize = 9;
			label.resizeTextMaxSize = size;
			label.alignment = TextAnchor.MiddleCenter;
			label.raycastTarget = false;
		}
	}
}
