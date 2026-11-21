using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkCultivationPages
	{
		private static readonly string[] ElementNames = { "通用", "金", "木", "水", "火", "土" };
		private static readonly Color[] ElementInks =
		{
			InkInterfaceArt.SoftInk, new Color32(126, 89, 30, 255), new Color32(47, 104, 72, 255),
			new Color32(46, 92, 123, 255), new Color32(152, 65, 47, 255), new Color32(117, 83, 57, 255)
		};

		private static void Place(RectTransform rect, float left, float bottom, float right, float top)
		{
			rect.anchorMin = new Vector2(left, bottom);
			rect.anchorMax = new Vector2(right, top);
			rect.offsetMin = rect.offsetMax = Vector2.zero;
		}

		private static Image Art(Transform parent, string name, Sprite sprite, bool preserve = true)
		{
			var image = parent.Find(name)?.GetComponent<Image>();
			if (image == null)
			{
				var node = new GameObject(name, typeof(RectTransform), typeof(Image));
				node.layer = parent.gameObject.layer;
				node.transform.SetParent(parent, false);
				image = node.GetComponent<Image>();
				image.raycastTarget = false;
			}
			image.sprite = sprite;
			image.type = sprite != null && sprite.border.sqrMagnitude > 0 ? Image.Type.Sliced : Image.Type.Simple;
			image.preserveAspect = preserve && image.type == Image.Type.Simple;
			image.enabled = sprite != null;
			return image;
		}

		private static Text Caption(Transform parent, string name, Font font, string value, int size, Color color)
		{
			var label = parent.Find(name)?.GetComponent<Text>();
			if (label == null)
			{
				var node = new GameObject(name, typeof(RectTransform), typeof(Text));
				node.layer = parent.gameObject.layer;
				node.transform.SetParent(parent, false);
				label = node.GetComponent<Text>();
				label.raycastTarget = false;
			}
			label.font = font;
			label.text = value;
			label.color = color;
			label.supportRichText = false;
			label.fontSize = size;
			label.resizeTextForBestFit = true;
			label.resizeTextMinSize = Mathf.Min(size, 10);
			label.resizeTextMaxSize = size;
			label.alignment = TextAnchor.MiddleLeft;
			return label;
		}

		private static Sprite ElementArt(int element)
		{
			return element >= 1 && element <= 5 ? InkInterfaceArt.Get("Element" + (WuXing)element) : null;
		}

		private static void Band(RectTransform rect, float left, float right, float lower, float upper, bool top = false, float inset = 0)
		{
			float edge = top ? 1 : 0;
			rect.anchorMin = new Vector2(left, edge);
			rect.anchorMax = new Vector2(right, edge);
			rect.offsetMin = new Vector2(inset, lower);
			rect.offsetMax = new Vector2(-inset, upper);
		}

		private static void Fit(Text label, int size, TextAnchor alignment)
		{
			label.alignment = alignment;
			label.fontSize = size;
			label.resizeTextForBestFit = true;
			label.resizeTextMinSize = 10;
			label.resizeTextMaxSize = size;
			label.raycastTarget = false;
		}

		internal static void PracticeRoom(DongFuPanel panel)
		{
			var root = panel.transform;
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			var identity = Art(root, "CultivatorIdentity", InkInterfaceArt.Get("InkLedgerPaper"), false);
			Band(identity.rectTransform, 0, .53f, -132, -80, true, 9);
			var portrait = Art(identity.transform, "Portrait", null);
			Place(portrait.rectTransform, .02f, .06f, .34f, .94f);
			var name = Caption(identity.transform, "Role", panel.DongFuNameText.font, role?.RoleName ?? "", 17, InkInterfaceArt.Ink);
			Place(name.rectTransform, .36f, .47f, .98f, .96f);
			var roots = Caption(identity.transform, "Roots", name.font, RootCaption(role), 11, InkInterfaceArt.SoftInk);
			Place(roots.rectTransform, .36f, .06f, .99f, .46f);
			// Keep the scene, meridians, and real buff buttons visible around the identity strip.
			var meridians = panel.EnterNeiShiBtn;
			Band((RectTransform)meridians.transform, .60f, 1, -128, -84, true, 11);
			InkDialogArt.StyleButton(meridians, "JadeButton", InkDialogArt.Ivory);
			var gate = Art(meridians.transform, "Gate", InkCultivationInterface.Get("GatheringFormation"));
			Place(gate.rectTransform, .055f, .17f, .30f, .83f);
			var entryTitle = Caption(meridians.transform, "MeridianLabel", name.font, "内视经脉", 16, InkDialogArt.Ivory);
			Place(entryTitle.rectTransform, .32f, .02f, .97f, .98f);
			entryTitle.alignment = TextAnchor.MiddleCenter;
			Band(panel.BuffContent, .69f, 1, -198, -135, true, 8);
			Band(panel.LianHuanImg, 0, .41f, -165, -137, true, 9);
			var refinePaper = panel.LianHuanImg.GetComponent<Image>() ?? panel.LianHuanImg.gameObject.AddComponent<Image>();
			InkInterfaceArt.Set(refinePaper, "LightPaper", false);
			refinePaper.raycastTarget = false;
			refinePaper.transform.SetAsFirstSibling();
			foreach (var item in new[] { panel.NaQiImg, panel.DuanTiImg, panel.LianShenImg })
			{
				Place(item.rectTransform, .04f, .12f, .21f, .88f);
				item.preserveAspect = true;
				item.raycastTarget = false;
			}
			InkInterfaceArt.Set(panel.NaQiImg, "Lotus");
			InkInterfaceArt.Set(panel.LianShenImg, "Scroll");
			InkInterfaceArt.Set(panel.DuanTiImg, "Swords");
			var refineTitle = panel.LianHuanImg.Find("ShengYuTitleText").GetComponent<Text>();
			refineTitle.text = "炼化";
			Place(refineTitle.rectTransform, .22f, 0, .49f, 1);
			Fit(refineTitle, 12, TextAnchor.MiddleLeft);
			refineTitle.color = InkInterfaceArt.SoftInk;
			Place(panel.ShengYuTimeText.rectTransform, .49f, 0, .96f, 1);
			Fit(panel.ShengYuTimeText, 14, TextAnchor.MiddleRight);
			panel.ShengYuTimeText.color = InkInterfaceArt.Ink;

			Band(panel.BottomBg, 0, 1, 2, 203, false, 8);
			panel.BottomBg.SetAsFirstSibling();
			if (panel.BottomBg.TryGetComponent<Image>(out var oldBottom)) oldBottom.enabled = false;
			Band(panel.FaZhenBg, 0, 1, 1, 40);
			Band(panel.OneXiuLianProgressPanel, 0, 1, 165, 199);
			Band((RectTransform)panel.DoubleXiuLianProgressRect.transform, 0, 1, 165, 199);
			Place((RectTransform)panel.OneXiuLianProgress.transform.parent, .008f, .1f, .992f, .96f);
			Place(panel.DoubleXiuLianProgressRect.LeftProgressRect, .008f, .1f, .493f, .96f);
			Place(panel.DoubleXiuLianProgressRect.RightProgressRect, .507f, .1f, .992f, .96f);
			Band((RectTransform)panel.BiGuanTuPoBtn.transform, .57f, 1, 207, 243, false, 9);
			PracticeRow(panel.NaQiBtn, panel.NaQiGengHuanBtn, panel.NaQiGongFaName, panel.NaQiGongFaXiaoLv, panel.NaQiRunningImg, 126);
			PracticeRow(panel.DuanTiBtn, panel.DuanTiGengHuanBtn, panel.DuanTiGongFaName, panel.DuanTiGongFaXiaoLv, panel.DuanTiRunningImg, 86);
			PracticeRow(panel.LianShenBtn, panel.LianShenGengHuanBtn, panel.LianShenGongFaName, panel.LianShenGongFaXiaoLv, panel.LianShenRunningImg, 46);
			Place(panel.YuanYinIconImg, .44f, .48f, .53f, .98f);
		}

		internal static void RefreshRolePortrait(Transform portrait)
		{
			if (portrait == null) return;
			var image = portrait.GetComponent<Image>();
			var roles = Singleton<RoleManager>.Instance;
			image.sprite = roles.Role_Entity != null ? roles.GetCurrentRoleSprite() : null;
			image.enabled = image.sprite != null;
		}

		private static string RootCaption(RoleInfoEntity role, bool showMissing = false)
		{
			if (role == null) return "";
			var roots = (role.JinLingGen ? "金" : "") + (role.MuLingGen ? "木" : "") + (role.ShuiLingGen ? "水" : "")
				+ (role.HuoLingGen ? "火" : "") + (role.TuLingGen ? "土" : "");
			if (roots.Length > 0) return roots + "灵根";
			return showMissing ? "无五行灵根" : WorldSetting.GetRoleLevelNameByRoleLevel(role.RoleLevel);
		}

		private static void PracticeRow(Toggle route, Button change, Text name, Text rate, Image running, float bottom)
		{
			var row = (RectTransform)route.transform.parent;
			Band(row, 0, 1, bottom, bottom + 37, false, 9);
			row.Find("Line")?.gameObject.SetActive(false);
			row.Find("TopGreenBall")?.gameObject.SetActive(false);
			var card = (RectTransform)change.transform.parent;
			Place(card, 0, 0, 1, 1);
			card.SetAsFirstSibling();
			InkInterfaceArt.Set(card.GetComponent<Image>(), "LightPaper", false);
			var left = (RectTransform)route.transform;
			Place(left, .01f, .08f, .236f, .92f);
			Place((RectTransform)route.graphic.transform, 0, 0, 1, 1);
			InkInterfaceArt.Set(route.targetGraphic as Image, "ParchmentHeading", false);
			InkInterfaceArt.Set(route.graphic as Image, "JadeSelection", false);
			var title = route.transform.Find("Text").GetComponent<Text>();
			Place(title.rectTransform, .32f, .04f, .98f, .96f);
			Fit(title, 15, TextAnchor.MiddleCenter);
			Place(name.rectTransform, .25f, .06f, .52f, .94f);
			Fit(name, 15, TextAnchor.MiddleLeft);
			var rateBack = (RectTransform)rate.transform.parent;
			Place(rateBack, .52f, .08f, .798f, .92f);
			if (rateBack.TryGetComponent<Image>(out var field)) field.enabled = false;
			rateBack.Find("LeftImg")?.gameObject.SetActive(false);
			rateBack.Find("RightImg")?.gameObject.SetActive(false);
			Place(rate.rectTransform, 0, 0, 1, 1);
			Fit(rate, 12, TextAnchor.MiddleRight);
			rate.color = InkInterfaceArt.SoftInk;
			Place((RectTransform)change.transform, .81f, .1f, .99f, .90f);
			InkDialogArt.StyleButton(change, "JadeButton", InkDialogArt.Ivory);
			var action = change.GetComponentInChildren<Text>(true);
			action.text = "更换";
			Place(action.rectTransform, .02f, 0, .98f, 1);
			Fit(action, 14, TextAnchor.MiddleCenter);
			Place(running.rectTransform, .01f, .10f, .088f, .90f);
			running.raycastTarget = false;
		}

		internal static void StudyShelf(CanWuPanel panel)
		{
			var root = panel.transform;
			var title = root.Find("Title_image");
			InkInterfaceArt.Set(title.GetComponent<Image>(), "ParchmentHeading", false);
			var titleText = title.Find("TitleName").GetComponent<Text>();
			titleText.text = "参悟书目";
			titleText.color = InkInterfaceArt.Ink;
			InkDialogArt.StyleButton(panel.ShaiXuanBtn, "JadeButton", InkDialogArt.Ivory);
			panel.ShaiXuanBtn.transform.Find("ShaiXuan_image")?.gameObject.SetActive(false);
			var rulesLabel = panel.ShaiXuanBtn.GetComponentInChildren<Text>(true);
			rulesLabel.text = "规则";
			Place(rulesLabel.rectTransform, 0, 0, 1, 1);
			rulesLabel.alignment = TextAnchor.MiddleCenter;

			var header = (RectTransform)panel.SmallTitleText.transform.parent;
			header.anchorMin = new Vector2(0, 1);
			header.anchorMax = Vector2.one;
			header.offsetMin = new Vector2(12, -76);
			header.offsetMax = new Vector2(-12, -48);
			InkInterfaceArt.Set(header.GetComponent<Image>(), "InkLedgerPaper", false);
			var roots = Caption(header, "StudyRoots", panel.SmallTitleText.font, "", 13, InkInterfaceArt.Ink);
			Place(roots.rectTransform, .035f, .05f, .53f, .95f);
			Place(panel.SmallTitleText.rectTransform, .54f, .05f, .965f, .95f);
			panel.SmallTitleText.alignment = TextAnchor.MiddleRight;
			panel.SmallTitleText.fontSize = 13;
			UIManager.FitTextToRect(panel.SmallTitleText);
			var fit = panel.ScrollContent.GetComponent<ContentSizeFitter>() ?? panel.ScrollContent.gameObject.AddComponent<ContentSizeFitter>();
			fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			var list = (RectTransform)panel.ScrollView.transform;
			Place(list, 0, 0, 1, 1);
			list.offsetMin = new Vector2(12, 4);
			list.offsetMax = new Vector2(-12, -119);
			panel.TiShiPanel.SetAsLastSibling();
		}

		internal static void RefreshStudyRole(CanWuPanel panel)
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			var header = panel.SmallTitleText.transform.parent;
			header.Find("StudyRoots").GetComponent<Text>().text = RootCaption(role, true);
		}

		internal static void BookHeading(Text title, CanWuEntity book)
		{
			if (book == null || title == null) return;
			var badge = Art(title.transform.parent, "StudyElement", ElementArt(book.FiveElement) ?? InkCultivationInterface.Get("TechniqueScroll"));
			var rect = badge.rectTransform;
			rect.anchorMin = rect.anchorMax = new Vector2(0, .5f);
			rect.sizeDelta = new Vector2(26, 26);
			rect.anchoredPosition = new Vector2(19, 0);
			title.rectTransform.offsetMin = new Vector2(36, title.rectTransform.offsetMin.y);
			title.alignment = TextAnchor.MiddleLeft;
		}

		internal static void Book(UI_GoodsItem item, CanWuEntity book)
		{
			var face = Art(item.transform, "StudyBook", InkInterfaceArt.Get("LightPaper"), false);
			Place(face.rectTransform, 0, 0, 1, 1);
			int element = book.FiveElement >= 1 && book.FiveElement <= 5 ? book.FiveElement : 0;
			var icon = Art(face.transform, "Element", ElementArt(element) ?? item.GoodsIcon.sprite);
			Place(icon.rectTransform, .02f, .14f, .24f, .86f);
			var name = Caption(face.transform, "Name", item.GoodsName.font, book.Name, 14, InkInterfaceArt.Ink);
			Place(name.rectTransform, .27f, .45f, .985f, .96f);
			string grade = WorldSetting.GetLevelByRankName(book.Rank);
			string kind = SystemMethodShelf.PracticeName(book);
			if (kind.Length == 0)
			{
				switch ((CanWuTitleType)book.Type)
				{
					case CanWuTitleType.GongFa: kind = "功法"; break;
					case CanWuTitleType.FaShu: kind = "法术"; break;
					case CanWuTitleType.DanFang: kind = "丹方"; break;
					case CanWuTitleType.QiTu: kind = "器图"; break;
					case CanWuTitleType.ZhenFa: kind = "阵图"; break;
					case CanWuTitleType.FuShu: kind = "符书"; break;
				}
			}
			var info = Caption(face.transform, "Grade", name.font, ElementNames[element] + " · " + kind, 11, ElementInks[element]);
			bool locked = SystemMethodShelf.State(book) == SystemMethodShelf.Shelf.Locked;
			Place(info.rectTransform, .27f, .06f, locked ? .59f : .70f, .45f);
			string note = locked ? WorldSetting.GetRoleLevelNameByRoleLevel(book.Level) : grade;
			var status = Caption(face.transform, "State", name.font, note, 11, InkInterfaceArt.SoftInk);
			Place(status.rectTransform, locked ? .60f : .71f, .06f, .975f, .45f);
			status.alignment = TextAnchor.MiddleRight;
		}

		internal static void TechniqueMote(Image indicator, WuXing element)
		{
			var sprite = InkCultivationInterface.Get("Qi" + element);
			if (indicator == null || sprite == null) return;
			indicator.sprite = sprite;
			indicator.color = Color.white;
		}

		internal static void FormElement(Button button, WuXing element, bool open)
		{
			var plate = (RectTransform)button.transform.parent;
			plate.sizeDelta = new Vector2(72, 33);
			var touch = (RectTransform)button.transform;
			Place(touch, 0, 0, 1, 1);
			InkInterfaceArt.Set(button.image, open ? "JadeSelection" : "LightPaper", false);
			var icon = Art(touch, "Spirit", ElementArt((int)element));
			Place(icon.rectTransform, .015f, .07f, .42f, .93f);
			icon.color = open ? Color.white : new Color(0.7f, 0.7f, 0.7f, .55f);
			var percent = plate.Find("percent").GetComponent<Text>();
			percent.transform.SetAsLastSibling();
			percent.raycastTarget = false;
			Place(percent.rectTransform, .42f, .02f, .98f, .5f);
			percent.fontSize = 11;
			percent.color = InkInterfaceArt.Ink;
			var label = Caption(touch, "Gate", percent.font, ElementNames[(int)element] + (open ? "开" : "闭"), 12, ElementInks[(int)element]);
			Place(label.rectTransform, .42f, .47f, .98f, .98f);
			label.alignment = percent.alignment = TextAnchor.MiddleCenter;
		}

		internal static void Meditation(CanWuZhongPanel panel)
		{
			var backdrop = Art(panel.transform, "MeditationChamber", InkCultivationInterface.Get("Chamber"), false);
			Place(backdrop.rectTransform, 0, 0, 1, 1);
			backdrop.transform.SetAsFirstSibling();
			var figure = Art(backdrop.transform, "Meditator", InkCultivationInterface.Get("Meditator"));
			Place(figure.rectTransform, .23f, .33f, .77f, .65f);
			panel.TitleText.text = "静室参悟";
			InkInterfaceArt.Set(panel.CanWuTitleImg, "ParchmentHeading", false);
			panel.CanWuTitleText.color = InkInterfaceArt.Ink;
			InkInterfaceArt.Set(panel.ProgressBg.Find("ProgressBg")?.GetComponent<Image>(), "LightPaper", false);
			panel.ProgressText.color = InkInterfaceArt.Ink;
			InkInterfaceArt.Set(panel.CompeleteBtn.transform.Find("ProgressBg")?.GetComponent<Image>(), "JadeButton", false);
			var finish = panel.CompeleteBtn.transform.Find("Text")?.GetComponent<Text>();
			if (finish != null) { finish.text = "收功 · 返回书目"; finish.color = InkDialogArt.Ivory; }
		}

		internal static void Altar(WuDaoTaiPanel panel)
		{
			var root = panel.transform;
			var scene = root.Find("WuDaoTaiRenWuDaBJ").GetComponent<Image>();
			scene.sprite = InkCultivationInterface.Get("Chamber");
			Place(scene.rectTransform, 0, 0, 1, 1);
			var person = root.Find("WuDaoTaiRenWuBJ").GetComponent<Image>();
			person.sprite = InkCultivationInterface.Get("Meditator");
			person.preserveAspect = true;
			Place(person.rectTransform, .17f, .42f, .83f, .84f);
			PaperDialog(root, "XiangQing", null, null);
			InkDialogArt.StyleButton(panel.GeneralEnlightenmentBut, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(panel.SeniorTookButton, "LightAction", InkDialogArt.Ivory);
			foreach (var button in new[] { panel.GeneralEnlightenmentBut, panel.SeniorTookButton })
				foreach (var text in button.GetComponentsInChildren<Text>(true)) text.color = InkDialogArt.Ivory;
		}

		internal static void PaperDialog(Transform root, string surface, Button confirm, Button secondary)
		{
			var card = root.Find(surface);
			if (card == null) return;
			InkInterfaceArt.Set(card.GetComponent<Image>(), "InkLedgerPaper", false);
			foreach (var name in new[] { "TitleLine", "Title", "Title_image", "Title/Image", "Contentbg", "BottomBg", "BottomImg", "bottomBg" })
				InkInterfaceArt.Set(card.Find(name)?.GetComponent<Image>(), "ParchmentHeading", false);
			// Only neutral labels on the newly replaced paper/backing take the common dark ink.
			foreach (Text text in card.GetComponentsInChildren<Text>(true))
			{
				var parent = text.transform.parent;
				if (parent == card || parent?.parent == card || parent?.parent?.name == "Title")
					text.color = InkInterfaceArt.Ink;
			}
			if (surface == "FaZhenBg" || surface == "JuLingFaZhenBg")
			{
				var amount = card.Find("Content");
				InkInterfaceArt.Set(amount?.GetComponent<Image>(), "LightPaper", false);
				InkInterfaceArt.Set(amount?.Find("InputField")?.GetComponent<Image>(), "LightPlaque", false);
				var value = amount?.Find("InputField/Text")?.GetComponent<Text>();
				if (value != null) value.color = InkInterfaceArt.Ink;
				InkDialogArt.StyleButton(amount?.Find("RemoveBtn")?.GetComponent<Button>(), "JadeButton", InkDialogArt.Ivory);
				InkDialogArt.StyleButton(amount?.Find("addBtn")?.GetComponent<Button>(), "JadeButton", InkDialogArt.Ivory);
			}
			InkDialogArt.StyleButton(confirm, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(secondary, "LightPlaque", InkInterfaceArt.Ink);
		}
	}
}
