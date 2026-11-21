using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class ShiTuArt
	{
		internal const float RowPitch = 84f;
		internal readonly Button Back;
		internal readonly Button Disciples;
		internal readonly Button Refresh;
		internal readonly Button Manage;
		internal readonly Button Journal;
		internal readonly Text Empty;
		internal readonly Text Status;
		internal readonly Text FormTitle;
		internal readonly Text FormNote;
		private readonly ShiTuPanel panel;
		private readonly Text heading;
		private readonly ScrollRect disciplesScroll;
		private readonly ScrollRect applicationsScroll;

		internal ShiTuArt(ShiTuPanel panel)
		{
			this.panel = panel;
			var root = (RectTransform)panel.transform;
			InkDialogArt.Inset(root, 0f, 0f, 93f, 8f);
			InkSocialArt.ApplyBackdrop(root);

			var header = (RectTransform)panel.WenHaoButton.transform.parent;
			var hero = (RectTransform)header.parent;
			InkDialogArt.SetPaper(hero.GetComponent<Image>());
			AtTop(hero, 10f, 10f, 8f, 154f);
			AtTop(header, 8f, 8f, 7f, 35f);
			InkInterfaceArt.Set(header.GetComponent<Image>(), "LightPlaque", false);
			HideGraphic(header.Find("Image"));
			heading = header.Find("TitleName")?.GetComponent<Text>();
			if (heading != null)
			{
				heading.text = "师徒";
				StyleText(heading, 20, InkInterfaceArt.Ink, TextAnchor.MiddleCenter);
				InkDialogArt.Inset(heading.rectTransform, 60f, 46f, 0f, 0f);
			}

			Back = header.Find("BackBtn")?.GetComponent<Button>();
			if (Back != null)
			{
				Back.gameObject.SetActive(true);
				PlaceTop((RectTransform)Back.transform, false, 4f, 3f, 55f, 29f);
				Image icon = Back.transform.Find("Img")?.GetComponent<Image>();
				InkInterfaceArt.Set(icon, "Back");
				if (icon != null)
					PlaceTop(icon.rectTransform, false, 4f, 7f, 14f, 14f);
				ButtonStyle(Back, "返回", false);
				if (Back.transform.Find("Text")?.GetComponent<Text>() is Text backLabel)
					InkDialogArt.Inset(backLabel.rectTransform, 16f, 2f, 0f, 0f);
			}
			PlaceTop((RectTransform)panel.WenHaoButton.transform, true, 4f, 2f, 32f, 31f);
			InkDialogArt.StyleButton(panel.WenHaoButton, "Help", InkInterfaceArt.Ink);

			AtTop(panel.WuShiFuImage.rectTransform, 8f, 8f, 47f, 99f);
			InkInterfaceArt.Set(panel.WuShiFuImage, "LightPlaque", false);
			FormTitle = AddText(panel.WuShiFuImage.transform, "MentorApplyTitle", panel.NameText.font, "拜师名帖", 15);
			AtTop(FormTitle.rectTransform, 12f, 12f, 5f, 22f);
			InkSocialArt.StyleInput(panel.NaneInput, "输入师尊的完整道号");
			AtTop((RectTransform)panel.NaneInput.transform, 12f, 109f, 30f, 34f);
			PlaceTop((RectTransform)panel.BaiShiButton.transform, true, 12f, 30f, 88f, 34f);
			ButtonStyle(panel.BaiShiButton, "递拜师帖", true);
			FormNote = AddText(panel.WuShiFuImage.transform, "MentorApplyNote", panel.NameText.font, "", 12, subdued: true);
			AtTop(FormNote.rectTransform, 12f, 12f, 68f, 25f);

			AtTop(panel.ShiFuBeiJinImage.rectTransform, 8f, 8f, 47f, 99f);
			StylePerson(panel.ShiFuBeiJinImage.rectTransform, panel.ShiFuToXiangImage, panel.NameText, panel.GradeText, 102f);
			StyleRelation(panel.ShiFuBeiJinImage.rectTransform, panel.IntimacyGradeText, panel.IntimacyText);
			PlaceTop((RectTransform)panel.QingAnButton.transform, true, 9f, 10f, 81f, 32f);
			PlaceTop((RectTransform)panel.ChuShiButton.transform, true, 9f, 49f, 81f, 32f);
			ButtonStyle(panel.QingAnButton, "请安", true);
			ButtonStyle(panel.ChuShiButton, "离开师门", false);

			var list = (RectTransform)root.Find("Image (1)");
			InkDialogArt.Inset(list, 10f, 10f, 170f, 49f);
			var listPaper = list.GetComponent<Image>();
			InkDialogArt.SetPaper(listPaper);
			listPaper.color = new Color32(255, 255, 255, 218);
			HideGraphic(list.Find("Title"));
			var tabs = (RectTransform)panel.BaiShiListButton.transform.parent;
			tabs.SetParent(list, false);
			AtTop(tabs, 8f, 8f, 7f, 36f);
			HideGraphic(tabs);
			HideGraphic(tabs.Find("Image"));
			panel.BiaoTiText.gameObject.SetActive(false);
			panel.ZhenKai.gameObject.SetActive(false);
			panel.BaiShiListImage.gameObject.SetActive(false);

			Disciples = AddButton(tabs, "MentorDisciplesTab", panel.NameText.font, "弟子");
			PlaceTop((RectTransform)Disciples.transform, false, 0f, 1f, 78f, 34f);
			PlaceTop((RectTransform)panel.BaiShiListButton.transform, false, 85f, 1f, 86f, 34f);
			Refresh = AddButton(tabs, "MentorRefresh", panel.NameText.font, "刷新");
			PlaceTop((RectTransform)Refresh.transform, true, 0f, 1f, 46f, 34f);
			Manage = AddButton(tabs, "MentorManage", panel.NameText.font, "管理");
			PlaceTop((RectTransform)Manage.transform, true, 51f, 1f, 45f, 34f);

			Status = AddText(list, "MentorStatus", panel.NameText.font, "", 12, subdued: true);
			AtTop(Status.rectTransform, 12f, 12f, 43f, 22f);
			disciplesScroll = PrepareScroll(list, panel.TudiListRectTransform);
			applicationsScroll = PrepareScroll(list, panel.PlayerInfoRect);
			Empty = AddText(list, "MentorEmptyState", panel.NameText.font, "", 15, subdued: true);
			StyleText(Empty, 15, InkInterfaceArt.SoftInk, TextAnchor.MiddleCenter);
			InkDialogArt.Inset(Empty.rectTransform, 22f, 22f, 72f, 16f);
			panel.TuDiNumText.gameObject.SetActive(false);

			var backToFriends = (RectTransform)panel.DaoYouButton.transform;
			backToFriends.anchorMin = backToFriends.anchorMax = new Vector2(0.5f, 0f);
			backToFriends.anchoredPosition = new Vector2(-64f, 24f);
			backToFriends.sizeDelta = new Vector2(118f, 36f);
			HideGraphic(backToFriends.Find("Image"));
			ButtonStyle(panel.DaoYouButton, "返回道友", false);
			Journal = AddButton(root, "MentorJournal", panel.NameText.font, "师门札记");
			var journalRect = (RectTransform)Journal.transform;
			journalRect.anchorMin = journalRect.anchorMax = new Vector2(0.5f, 0f);
			journalRect.anchoredPosition = new Vector2(64f, 24f);
			journalRect.sizeDelta = backToFriends.sizeDelta;
			panel.DaoLvButton.gameObject.SetActive(false);
		}

		internal void SetGraduatedCount(int count)
		{
			if (heading != null)
				heading.text = count > 0 ? $"师徒 · 已出师 {count} 人" : "师徒";
		}

		internal void SetTab(bool inbox, int count, int requests, bool managing)
		{
			ButtonStyle(Disciples, $"弟子 {count}/3", !inbox);
			ButtonStyle(panel.BaiShiListButton, requests > 0 ? $"拜师帖 {requests}" : "拜师帖", inbox);
			if (Manage != null)
			{
				Manage.gameObject.SetActive(!inbox);
				ButtonStyle(Manage, managing ? "完成" : "管理", false);
			}
			disciplesScroll.gameObject.SetActive(!inbox);
			applicationsScroll.gameObject.SetActive(inbox);
		}

		private static ScrollRect PrepareScroll(RectTransform paper, RectTransform content)
		{
			var scroll = content.GetComponentInParent<ScrollRect>(true);
			var rect = (RectTransform)scroll.transform;
			rect.SetParent(paper, false);
			InkDialogArt.Inset(rect, 10f, 10f, 66f, 9f);
			HideGraphic(rect);
			if (scroll.viewport.TryGetComponent(out Mask mask))
				mask.showMaskGraphic = false;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			return scroll;
		}

		internal static void StylePerson(RectTransform row, Image portrait, Text name, Text level, float right)
		{
			InkInterfaceArt.Set(row.GetComponent<Image>(), "LightPaper", false);
			var frame = (RectTransform)portrait.transform.parent;
			PlaceTop(frame, false, 10f, 10f, 55f, 55f);
			InkInterfaceArt.Set(frame.GetComponent<Image>(), "LightPlaque", false);
			foreach (Transform part in frame)
			{
				if (part != portrait.transform)
					HideGraphic(part);
			}
			InkDialogArt.Inset(portrait.rectTransform, 3f, 3f, 3f, 3f);
			portrait.preserveAspect = true;
			portrait.raycastTarget = false;
			AtTop(name.rectTransform, 76f, right, 9f, 22f);
			AtTop(level.rectTransform, 76f, right, 34f, 17f);
			StyleText(name, 16, InkInterfaceArt.Ink);
			StyleText(level, 12, InkInterfaceArt.SoftInk);
			HideGraphic(row.Find("QinMiDuIcon"));
		}

		internal static void StyleRelation(RectTransform row, Text relation, Text intimacy)
		{
			intimacy.transform.SetParent(row, false);
			HideGraphic(row.Find("FuZhiYi"));
			PlaceTop(relation.rectTransform, false, 76f, 55f, 57f, 17f);
			PlaceTop(intimacy.rectTransform, false, 135f, 55f, 73f, 17f);
			StyleText(relation, 11, InkInterfaceArt.Seal);
			StyleText(intimacy, 11, InkInterfaceArt.SoftInk);
		}

		internal static void StyleText(Text text, int size, Color color, TextAnchor alignment = TextAnchor.MiddleLeft)
		{
			text.fontSize = size;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = 10;
			text.resizeTextMaxSize = size;
			text.alignment = alignment;
			text.color = color;
			text.raycastTarget = false;
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.verticalOverflow = VerticalWrapMode.Truncate;
		}

		internal static void ButtonStyle(Button button, string label, bool primary)
		{
			InkDialogArt.StyleButton(button, primary ? "LightAction" : "LightPlaque",
				primary ? InkDialogArt.Ivory : InkInterfaceArt.Ink);
			Text text = button.transform.Find("Text")?.GetComponent<Text>();
			if (text != null)
			{
				text.text = label;
				StyleText(text, 13, primary ? InkDialogArt.Ivory : InkInterfaceArt.Ink, TextAnchor.MiddleCenter);
			}
		}

		internal static void AtTop(RectTransform rect, float left, float right, float top, float height)
		{
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, -top - height);
			rect.offsetMax = new Vector2(-right, -top);
		}

		internal static void PlaceTop(RectTransform rect, bool right, float side, float top, float width, float height)
		{
			float x = right ? 1f : 0f;
			rect.anchorMin = rect.anchorMax = new Vector2(x, 1f);
			rect.pivot = new Vector2(x, 1f);
			rect.sizeDelta = new Vector2(width, height);
			rect.anchoredPosition = new Vector2(right ? -side : side, -top);
		}

		internal static void HideGraphic(Transform item)
		{
			if (item != null && item.TryGetComponent(out Image image))
			{
				image.color = Color.clear;
				image.raycastTarget = false;
			}
		}

		private static Text AddText(Transform parent, string key, Font font, string copy, int size, bool subdued = false)
		{
			var item = new GameObject(key, typeof(RectTransform), typeof(Text));
			item.layer = parent.gameObject.layer;
			item.transform.SetParent(parent, false);
			Text text = item.GetComponent<Text>();
			text.font = font;
			text.text = copy;
			StyleText(text, size, subdued ? InkInterfaceArt.SoftInk : InkInterfaceArt.Ink);
			return text;
		}

		private static Button AddButton(Transform parent, string key, Font font, string caption)
		{
			var item = new GameObject(key, typeof(RectTransform), typeof(Image), typeof(Button));
			item.layer = parent.gameObject.layer;
			item.transform.SetParent(parent, false);
			Button button = item.GetComponent<Button>();
			button.targetGraphic = item.GetComponent<Image>();
			Text text = AddText(item.transform, "Text", font, caption, 13);
			InkDialogArt.Inset(text.rectTransform, 3f, 3f, 1f, 1f);
			ButtonStyle(button, caption, false);
			return button;
		}
	}
}
