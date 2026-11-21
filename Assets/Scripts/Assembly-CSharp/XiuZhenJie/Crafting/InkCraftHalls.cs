using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkCraftHalls
	{
		internal static void StyleFacility(GameObject root)
		{
			if (root.TryGetComponent<HeDaoTaiPanel>(out var concord))
				StyleAltar(root.transform, "合道台", concord.Item_1_Texts);
			else if (root.TryGetComponent<XuShenJiTanPanel>(out var altar))
				StyleAltar(root.transform, "虚神祭坛", altar.Item_1_Texts);
			else if (root.TryGetComponent<DanShiTaPanel>(out var alchemy))
			{
				StyleDirectory(root.transform, "丹师塔",
					(alchemy.DanShiQueRen, "丹师认证", "参加丹师考核"), (alchemy.DanFangDuiHuan, "丹方兑换", "查阅塔内丹方"),
					(alchemy.MoMiChuangZao, "推演丹方", "模拟炼丹"), (alchemy.DangFangZhuCeQueRen, "丹方注册", "登记自创丹方"));
				PlaceHallNotice(alchemy.Des, "丹师可在塔中推演、登记丹方。初次入塔可先查看丹师认证条件。");
			}
			else if (root.TryGetComponent<SFH_ShenFuHuiPanel>(out var talisman))
			{
				StyleDirectory(root.transform, "神符会",
					(talisman.FuShuDuiHuan, "符书兑换", "查阅符书"), (talisman.MoMiChuangZao, "推演符书", "模拟创符"),
					(talisman.FuShuZhuCe, "符书注册", "登记自创符书"));
				PlaceHallNotice(talisman.Des, "符师可在此推演符书、登记成果。已收录的符书可使用贡献点兑换。");
			}
			else if (root.TryGetComponent<LT_MianPanel>(out var plants))
			{
				StyleDirectory(root.transform, "灵植协会",
					(plants.LingZhiDuiHuan, "灵植兑换", "查阅灵植种子"), (plants.MoMiChuangZao, "培育灵植", "培植与嫁接"),
					(plants.LingZhiZhuCe, "灵植注册", "登记培育成果"));
				PlaceHallNotice(plants.Des, "协会收藏灵植种子。灵植师可进入培育室，登记已经培育出的新品种。");
			}
			else if (root.TryGetComponent<LianQi_ChuangZaoMianPanel>(out var forge))
			{
				StyleDirectory(root.transform, "炼器协会",
					(forge.LingZhiDuiHuan, "器图兑换", "查阅器图"), (forge.MoMiChuangZao, "推演器图", "模拟炼器"),
					(forge.LingZhiZhuCe, "器图注册", "登记自创器图"));
				InkInterfaceArt.Set(root.transform.Find("CityBg_image").GetComponent<Image>(), "ForgeHall", false);
				PlaceHallNotice(forge.Des, "协会收录炼器图谱。炼器师可模拟打造法器，登记自创的器图。");
			}
		}

		internal static void StyleSelection(Transform root, string contentName, string artwork, Button action, Button details, Button puppet)
		{
			InkInterfaceArt.Set(root.Find("Bg")?.GetComponent<Image>(), artwork, false);
			var content = root.Find(contentName);
			InkDialogArt.SetPaper(content?.GetComponent<Image>(), "InkLandscapePaper");
			if (content != null)
			{
				foreach (Transform column in content)
				{
					if (!column.name.StartsWith("ToggleGroup_", System.StringComparison.Ordinal) ||
						!column.TryGetComponent<Image>(out var paper)) continue;
					InkInterfaceArt.Set(paper, "LightPaper", false);
					paper.color = new Color(1f, 1f, 1f, .60f);
				}
			}
			InkDialogArt.StyleButton(action, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(details, "ParchmentHeading", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(puppet, "ParchmentHeading", InkInterfaceArt.Ink);
		}

		internal static Text AddListStatus(RectTransform content, Font font)
		{
			var node = new GameObject("ListStatus", typeof(RectTransform), typeof(Text));
			node.layer = content.gameObject.layer;
			node.transform.SetParent(content.parent, false);
			var label = node.GetComponent<Text>();
			label.font = font;
			label.fontSize = 15;
			label.alignment = TextAnchor.MiddleCenter;
			label.color = InkInterfaceArt.Ink;
			label.raycastTarget = false;
			InkDialogArt.Inset(label.rectTransform, 8, 8, 5, 5);
			return label;
		}

		internal static void PlaceHallNotice(Text label, string notice)
		{
			var sheet = (RectTransform)label.transform.parent;
			sheet.anchorMin = Vector2.zero;
			sheet.anchorMax = Vector2.right;
			sheet.offsetMin = new Vector2(14, 142);
			sheet.offsetMax = new Vector2(-14, 230);
			InkDialogArt.SetPaper(sheet.GetComponent<Image>(), "Parchment");
			var heading = sheet.Find("Title_text").GetComponent<Text>();
			heading.text = "入殿须知";
			heading.color = InkInterfaceArt.Ink;
			heading.raycastTarget = false;
			heading.alignment = TextAnchor.MiddleLeft;
			heading.rectTransform.anchorMin = new Vector2(0, 1);
			heading.rectTransform.anchorMax = Vector2.one;
			heading.rectTransform.offsetMin = new Vector2(12, -26);
			heading.rectTransform.offsetMax = new Vector2(-12, -7);
			label.text = notice;
			label.fontSize = 13;
			label.resizeTextForBestFit = false;
			label.color = InkInterfaceArt.Ink;
			label.alignment = TextAnchor.UpperLeft;
			label.raycastTarget = false;
			InkDialogArt.Inset(label.rectTransform, 12, 12, 30, 7);
		}

		internal static void StyleDirectory(Transform root, string title, params (Button button, string name, string function)[] destinations)
		{
			var landscape = root.Find("CityBg_image").GetComponent<Image>();
			InkInterfaceArt.Set(landscape, "MountainHall", false);
			InkDialogArt.Inset(landscape.rectTransform, 0, 0, 0, 0);
			landscape.transform.SetAsFirstSibling();
			var heading = root.Find("Title_image/TitleName").GetComponent<Text>();
			heading.text = title;
			heading.color = InkInterfaceArt.Ink;
			InkInterfaceArt.Set(heading.transform.parent.GetComponent<Image>(), "ParchmentHeading", false);
			var back = root.Find("Title_image/BackImg/BackBtn").GetComponent<Button>();
			foreach (var caption in back.GetComponentsInChildren<Text>(true))
			{
				caption.text = "返回";
				caption.color = InkInterfaceArt.Ink;
				caption.raycastTarget = false;
			}
			var status = root.Find("SmallTitleBg");
			if (status != null)
			{
				InkDialogArt.SetPaper(status.GetComponent<Image>(), "ParchmentHeading");
				foreach (Transform item in status)
				{
					var background = item.GetComponent<Image>();
					if (background != null) background.color = Color.clear;
				}
				foreach (var caption in status.GetComponentsInChildren<Text>(true))
					caption.color = InkInterfaceArt.Ink;
			}

			StyleDestinations(destinations);
		}

		private static void StyleAltar(Transform root, string title, Text[] effects)
		{
			InkInterfaceArt.Set(root.Find("BG").GetComponent<Image>(), "MountainHall", false);
			InkInterfaceArt.Set(root.Find("Content").GetComponent<Image>(), "InkLedgerPaper", false);
			root.Find("Title_image/TitleName").GetComponent<Text>().text = title;
			foreach (var effect in effects)
			{
				InkInterfaceArt.Set(effect.transform.parent.GetComponent<Image>(), "LightPaper", false);
				effect.color = InkInterfaceArt.Ink;
				effect.text = FormatAltarEffect(effect.text);
			}
		}

		internal static string FormatAltarEffect(string value) =>
			value?.Replace("<color=red>", "<color=#87432E>");

		internal static void StyleDestinations(params (Button button, string name, string function)[] destinations)
		{
			int rowCount = (destinations.Length + 1) / 2;
			for (int i = 0; i < destinations.Length; i++)
			{
				var (button, name, function) = destinations[i];
				var rect = (RectTransform)button.transform;
				float column = (i % 2) * .5f;
				float bottom = 16 + (rowCount - 1 - i / 2) * 60;
				rect.anchorMin = new Vector2(column, 0);
				rect.anchorMax = new Vector2(column + .5f, 0);
				rect.offsetMin = new Vector2(i % 2 == 0 ? 14 : 4, bottom);
				rect.offsetMax = new Vector2(i % 2 == 0 ? -4 : -14, bottom + 52);
				InkDialogArt.StyleButton(button, "BrushPlaque", InkInterfaceArt.Ink);
				button.transform.Find("Image")?.gameObject.SetActive(false);
				Text label = button.GetComponentInChildren<Text>(true);
				label.text = name + "\n<size=13><color=#485B50>" + function + "</color></size>";
				label.supportRichText = true;
				label.fontSize = 16;
				label.resizeTextForBestFit = false;
				label.alignment = TextAnchor.MiddleCenter;
				label.lineSpacing = .95f;
				label.raycastTarget = false;
				InkDialogArt.Inset(label.rectTransform, 2, 2, 1, 1);
			}
		}
	}
}
