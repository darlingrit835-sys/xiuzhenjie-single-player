using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkCultivationInterface
	{
		private static readonly Color PaleInk = new Color32(251, 239, 213, 255);
		private static readonly Color SelectedWash = new Color32(175, 212, 191, 255);

		internal static Sprite Get(string name)
		{
			return ReleasedResources.Load<Sprite>("InkCultivation/" + name);
		}

		internal static void StylePractice(DongFuPanel panel)
		{
			StyleTitle(panel.transform, panel.DongFuNameText, panel.LiKaiBtn);
			StyleAction(panel.ShengJiBtn, "IvoryPlaque");
			StylePracticeToggle(panel.NaQiBtn, panel.NaQiGengHuanBtn, "Breath");
			StylePracticeToggle(panel.DuanTiBtn, panel.DuanTiGengHuanBtn, "BodyTraining");
			StylePracticeToggle(panel.LianShenBtn, panel.LianShenGengHuanBtn, "SpiritRefinement");
			StyleFormation(panel.JuLingFaZhenXuanZeBtn, "GatheringFormation");
			StyleFormation(panel.KongLingFaZhenXuanZeBtn, "ControlFormation");
			StylePracticeIndicator(panel.NaQiRunningImg);
			StylePracticeIndicator(panel.DuanTiRunningImg);
			StylePracticeIndicator(panel.LianShenRunningImg);
			StyleMeter(panel.OneXiuLianProgress, panel.OneXiuLianTitleText, panel.OneXiuLianValueText, panel.OneXiuLianProgressText);
			var pair = panel.DoubleXiuLianProgressRect;
			StyleMeter(pair.LeftProgressImg, pair.LeftTitleText, pair.LeftProgressText, pair.LeftProgressPercentText);
			StyleMeter(pair.RightProgressImg, pair.RightTitleText, pair.RightProgressText, pair.RightProgressPercentText);
			SetTextColor(panel.JuLingFaZhenShengYuTimeText, InkInterfaceArt.Ink);
			SetTextColor(panel.KongLingFaZhenShengYuTimeText, InkInterfaceArt.Ink);
			StyleAction(panel.BiGuanTuPoBtn, "SealButton", Get("Breakthrough"));


			InkInterfaceArt.Set(panel.WeiHao?.image, "Help");
			InkCultivationPages.PracticeRoom(panel);
		}

		internal static void StyleResidence(DongFuRoomPanel panel)
		{
			SetArt(panel.GetComponent<Image>(), Get("Residence"), false);
			StyleTitle(panel.transform, panel.TitleText, panel.LiKai_Btn);
			StyleRoomTile(panel.MiShiBtn, Get("InnerChamber"));
			StyleRoomTile(panel.LianGongFaBtn, Get("Breath"));
			StyleRoomTile(panel.DiHuoShiBtn, InkInterfaceArt.Get("ElementHuo"));
			StyleRoomTile(panel.WuDaoTaiBtn, Get("SpiritRefinement"));
			StyleRoomTile(panel.LingTianBtn, InkInterfaceArt.Get("ElementMu"));
			StyleAction(panel.KaiPiLingTianBtn, "IvoryPlaque");
			StyleAction(panel.ShengJi_Btn, "SealButton", Get("Breakthrough"));
			InkInterfaceArt.Set(panel.WeiHao?.image, "Help");
		}

		internal static void StyleResidenceChooser(DongFuChoosePanel panel)
		{
			InkInterfaceArt.Set(panel.GetComponent<Image>(), "Paper", false);
			StyleTitle(panel.transform, panel.TitleText, panel.BackBtn);
		}

		internal static void StyleResidenceOffer(DongFuItem item, int tier, Sprite residence)
		{
			if (residence != null)
			{
				SetArt(item.BgImg, residence, false);
				item.BgImg.color = tier switch
				{
					0 => new Color32(235, 222, 194, 255),
					1 => new Color32(222, 242, 227, 255),
					2 => new Color32(235, 218, 207, 255),
					_ => new Color32(235, 232, 255, 255)
				};
			}

			Transform labels = item.transform.Find("LeftBg");
			InkInterfaceArt.Set(labels?.GetComponent<Image>(), "Paper", false);
			InkInterfaceArt.Set(labels?.Find("Ttitle")?.GetComponent<Image>(), "LightPlaque", false);
			SetTextColor(item.DongFuName, InkInterfaceArt.Ink);
			SetTextColor(item.MiaoShuText, InkInterfaceArt.SoftInk);
			StyleAction(item.GouMaiBtn, "SealButton");
		}

		internal static void StyleTechniqueChooser(GengHuanGongFaPanel panel)
		{
			Transform root = panel.transform;
			Image background = GetOrAddImage(root, "InkCultivationPaper", Vector2.zero, Vector2.one);
			background.transform.SetAsFirstSibling();
			InkInterfaceArt.Set(background, "Paper", false);
			StyleTitle(root, root.Find("Title_image/TitleName")?.GetComponent<Text>(), panel.CloseBtn);
			InkInterfaceArt.Set(root.Find("XiangQing/Bg")?.GetComponent<Image>(), "Paper", false);
			InkInterfaceArt.Set(root.Find("XiangQing/ShuXing")?.GetComponent<Image>(), "Paper", false);
			InkInterfaceArt.Set(root.Find("XiangQing/ShuXing/Scroll View")?.GetComponent<Image>(), "Paper", false);
			InkInterfaceArt.Set(panel.KeYongGongFaScrollView?.GetComponent<Image>(), "Paper", false);
			// Keep the existing Mask/RectMask2D and the ScrollRect hit targets in the bundle.
			InkInterfaceArt.Set(panel.KeYongGongFaScrollView?.viewport?.GetComponent<Image>(), "Paper", false);
			StyleSection(root.Find("CurrentGongFaTitle"));
			StyleSection(root.Find("CurrentKeYongGongFaTitle"));
			StyleSection(root.Find("XiangQing/ShuXing/Title"));
			StyleAction(panel.ShaiXuanBtn, "IvoryPlaque");
			StyleAction(panel.QueRenGengHuan, "SealButton", Get("TechniqueScroll"));
		}

		internal static void StyleTechniqueDetail(RoleZhuangBeiXiangQingItem detail)
		{
			InkInterfaceArt.Set(detail.BackGrounBg, "Paper", false);
			SetTextColor(detail.ContentText, InkInterfaceArt.Ink);
		}

		private static void StyleTitle(Transform root, Text title, Button back)
		{
			InkInterfaceArt.Set(root.Find("Title_image")?.GetComponent<Image>(), "IvoryPlaque", false);
			SetTextColor(title, InkInterfaceArt.Ink);
			UIManager.FitTextToRect(title);
			if (back != null)
			{
				InkInterfaceArt.Set(back.transform.parent.GetComponent<Image>(), "InkPlaque", false);
				InkInterfaceArt.Set(back.transform.Find("Img")?.GetComponent<Image>(), "Depart");
			}
		}

		private static void StylePracticeToggle(Toggle toggle, Button change, string icon)
		{
			if (toggle == null)
			{
				return;
			}
			InkInterfaceArt.Set(toggle.targetGraphic as Image, "IvoryPlaque", false);
			if (toggle.graphic is Image selection)
			{
				InkInterfaceArt.Set(selection, "IvoryPlaque", false);
				selection.color = SelectedWash;
			}
			if (AddIcon(toggle.transform, Get(icon), new Vector2(0.09f, 0.10f), new Vector2(0.34f, 0.90f)))
			{
				MoveLabelRight(toggle.transform.Find("Text")?.GetComponent<Text>(), 13f);
			}
			SetTextColor(toggle.transform.Find("Text")?.GetComponent<Text>(), InkInterfaceArt.Ink);

			if (change != null)
			{
				Transform card = change.transform.parent;
				InkInterfaceArt.Set(card.GetComponent<Image>(), "LightPaper", false);
				InkInterfaceArt.Set(card.Find("Image")?.GetComponent<Image>(), "SealButton", false);
				SetTextColor(card.Find("Image/xiaoLvText")?.GetComponent<Text>(), PaleInk);
				SetTextColor(card.Find("GongFaName")?.GetComponent<Text>(), InkInterfaceArt.Ink);
				InkDialogArt.StyleButton(change, "LightPlaque", InkInterfaceArt.Ink);
			}
		}

		private static void StyleFormation(Button button, string icon)
		{
			if (button == null)
			{
				return;
			}
			Transform title = button.transform.name == "Title" ? button.transform : button.transform.Find("Title");
			Transform card = title == button.transform ? button.transform.parent : button.transform;
			InkInterfaceArt.Set(card.GetComponent<Image>(), "LightPaper", false);
			InkInterfaceArt.Set(title?.GetComponent<Image>(), "IvoryPlaque", false);
			Text name = title?.Find("Text")?.GetComponent<Text>();
			if (title != null && AddIcon(title, Get(icon), new Vector2(0.06f, 0.16f), new Vector2(0.27f, 0.84f)))
			{
				MoveLabelRight(name, 15f);
				UIManager.FitTextToRect(name);
			}
			SetTextColor(name, InkInterfaceArt.Ink);
		}

		private static void StyleMeter(Image fill, Text title, Text value, Text fraction)
		{
			if (fill == null) return;
			InkInterfaceArt.Set(fill.transform.parent.GetComponent<Image>(), "ParchmentHeading", false);
			fill.color = new Color32(150, 189, 158, 220);
			SetTextColor(title, InkInterfaceArt.Ink);
			SetTextColor(value, InkInterfaceArt.Ink);
			SetTextColor(fraction, InkInterfaceArt.Ink);
		}

		private static void StylePracticeIndicator(Image indicator)
		{
			Sprite mote = Get("QiAll");
			if (indicator == null || mote == null)
			{
				return;
			}
			SetArt(indicator, mote, true);
			indicator.color = new Color32(165, 203, 181, 155);
		}

		private static void StyleAction(Button button, string background, Sprite icon = null)
		{
			if (button == null)
			{
				return;
			}
			InkInterfaceArt.Set(button.image, background, false);
			Text label = button.GetComponentInChildren<Text>(true);
			SetTextColor(label, background == "IvoryPlaque" ? InkInterfaceArt.Ink : PaleInk);
			if (AddIcon(button.transform, icon, new Vector2(0.10f, 0.12f), new Vector2(0.27f, 0.88f)))
			{
				MoveLabelRight(label, 20f);
			}
		}

		private static void StyleRoomTile(Button button, Sprite icon)
		{
			if (button == null)
			{
				return;
			}
			Transform card = button.transform.parent;
			InkInterfaceArt.Set(card.GetComponent<Image>(), "IvoryPlaque", false);
			InkInterfaceArt.Set(button.image, "Paper", false);
			Text label = button.GetComponentInChildren<Text>(true);
			SetTextColor(label, InkInterfaceArt.Ink);
			if (AddIcon(button.transform, icon, new Vector2(0.15f, 0.37f), new Vector2(0.85f, 0.98f)) && label != null)
			{
				RectTransform rect = label.rectTransform;
				rect.anchorMin = new Vector2(0.04f, 0f);
				rect.anchorMax = new Vector2(0.96f, 0.36f);
				rect.offsetMin = Vector2.zero;
				rect.offsetMax = Vector2.zero;
				label.alignment = TextAnchor.MiddleCenter;
				UIManager.FitTextToRect(label);
			}
			Transform status = card.Find("Button");
			InkInterfaceArt.Set(status?.GetComponent<Image>(), "LightPlaque", false);
			SetTextColor(status?.GetComponentInChildren<Text>(true), InkInterfaceArt.Ink);
		}

		private static void StyleSection(Transform section)
		{
			InkInterfaceArt.Set(section?.GetComponent<Image>(), "LightPlaque", false);
			SetTextColor(section?.GetComponentInChildren<Text>(true), InkInterfaceArt.Ink);
		}

		private static void MoveLabelRight(Text label, float inset)
		{
			if (label != null)
			{
				RectTransform rect = label.rectTransform;
				rect.offsetMin = new Vector2(rect.offsetMin.x + inset, rect.offsetMin.y);
			}
		}

		private static void SetTextColor(Text text, Color color)
		{
			if (text != null)
			{
				text.color = color;
			}
		}

		private static bool AddIcon(Transform parent, Sprite sprite, Vector2 min, Vector2 max)
		{
			const string name = "InkCultivationIcon";
			if (sprite == null || parent.Find(name) != null)
			{
				return false;
			}
			SetArt(GetOrAddImage(parent, name, min, max), sprite, true);
			return true;
		}

		private static Image GetOrAddImage(Transform parent, string name, Vector2 min, Vector2 max)
		{
			Image previous = parent.Find(name)?.GetComponent<Image>();
			if (previous != null)
			{
				return previous;
			}

			var art = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
			art.layer = parent.gameObject.layer;
			var rect = (RectTransform)art.transform;
			rect.SetParent(parent, false);
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;

			Image image = art.GetComponent<Image>();
			image.raycastTarget = false;
			return image;
		}

		private static void SetArt(Image image, Sprite sprite, bool preserveAspect)
		{
			if (image != null && sprite != null)
			{
				image.sprite = sprite;
				image.overrideSprite = null;
				image.type = Image.Type.Simple;
				image.preserveAspect = preserveAspect;
				image.color = Color.white;
			}
		}
	}
}
