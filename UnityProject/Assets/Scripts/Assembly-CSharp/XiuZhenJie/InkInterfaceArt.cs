using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkInterfaceArt
	{
		internal static readonly Color Ink = new Color32(45, 54, 47, 255);
		internal static readonly Color SoftInk = new Color32(71, 83, 69, 255);
		internal static readonly Color Seal = new Color32(135, 56, 43, 255);

		private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
		private static readonly Dictionary<string, string> SharedSprites = new Dictionary<string, string>(StringComparer.Ordinal)
		{
			["返回@2x"] = "Back",
			["fanhui"] = "Back",
			["return"] = "Back",
			["WenHao"] = "Help",
			["HeiWenHao"] = "Help",
			["titlebg2"] = "IvoryPlaque",
			["BtnImg"] = "SealButton",
			["DongFuIcon"] = "Pavilion",
			["NaJieIcon"] = "Satchel",
			["ZhanDouIcon"] = "Swords",
			["CanWuIcon"] = "Book",
			["TitleDaoYouIcon"] = "Chat",
			["HuoBanIcon"] = "FoldedMap",
			["GongLueIcon"] = "Scroll",
			["youjian"] = "Letter",
			["laba"] = "Bell",
			["LingYuIcon"] = "JadePendant",
			["LingShiDiaoLuo"] = "SilverIngot"
		};

		internal static Sprite Get(string name)
		{
			if (!Sprites.TryGetValue(name, out Sprite sprite))
			{
				sprite = Resources.Load<Sprite>("InkInterface/" + name);
				Sprites.Add(name, sprite);
			}
			return sprite;
		}

		// The legacy UI prefabs are loaded from Android bundles even in Play Mode.
		internal static void Apply(GameObject root)
		{
			foreach (Image image in root.GetComponentsInChildren<Image>(true))
			{
				string name = image.sprite != null ? image.sprite.name : string.Empty;
				if (name.StartsWith("Man_", StringComparison.Ordinal) || name.StartsWith("Women_", StringComparison.Ordinal))
				{
					Sprite portrait = Resources.Load<Sprite>("InkPortraits/" + name);
					if (portrait != null)
					{
						image.sprite = portrait;
					}
				}
				else if (SharedSprites.TryGetValue(name, out string replacement))
				{
					bool preserveAspect = replacement != "IvoryPlaque" && replacement != "SealButton";
					float alpha = image.color.a;
					Set(image, replacement, preserveAspect);
					Color color = image.color;
					color.a = alpha;
					image.color = color;
				}
				else if ((name == "Mask" || image.sprite == null) && IsLightBackground(image))
				{
					Set(image, "Paper", false);
				}
			}
		}

		private static bool IsLightBackground(Image image)
		{
			Color color = image.color;
			if (color.a < 0.95f || Mathf.Min(color.r, Mathf.Min(color.g, color.b)) < 0.85f ||
				image.GetComponent<Selectable>() != null || image.GetComponent<Mask>() != null)
			{
				return false;
			}

			switch (image.gameObject.name)
			{
			case "Bg":
			case "BG":
			case "TopBg":
			case "BottomBg":
			case "Part_1":
			case "Part_2":
			case "Part_3":
			case "BottomTitle_image":
				return true;
			default:
				return false;
			}
		}

		internal static void Set(Image image, string spriteName, bool preserveAspect = true)
		{
			Sprite sprite = Get(spriteName);
			if (image == null || sprite == null)
			{
				return;
			}
			image.sprite = sprite;
			image.overrideSprite = null;
			image.type = Image.Type.Simple;
			image.preserveAspect = preserveAspect;
			image.color = Color.white;
		}

		internal static void StyleCity(MainCityPanel panel)
		{
			Set(panel.transform.Find("CityBg_image")?.GetComponent<Image>(), "CityScene", false);
			StyleBuilding(panel.ChengZhuFuBtn, "CityGate");
			StyleBuilding(panel.LingQuanJiuSiBtn, "Village");
			StyleBuilding(panel.ZhongJieFangShiBtn, "Town");
			StyleBuilding(panel.XuanTianLouBtn, "Pavilion");
			StyleBuilding(panel.ChuanSongDaZhenBtn, "Compass");
			StyleBuilding(panel.XuShenJieBtn, "Lotus");
			Transform actions = panel.transform.Find("CeTitle");
			if (actions != null)
			{
				Set(actions.GetComponent<Image>(), "Paper", false);
				StyleQuickAction(actions.Find("QianDao"), "AttendanceSeal");
				StyleQuickAction(actions.Find("Rank"), "Ranking");
				StyleQuickAction(actions.Find("GongLue"), "Scroll");
				StyleQuickAction(actions.Find("TianDaoLing"), "Dispatch");
				StyleQuickAction(actions.Find("XuanTianLing"), "JadePendant");
				Set(actions.Find("GuangGao")?.GetComponent<Image>(), "Gift");
				Set(actions.Find("HuoDong/HuoDongBtn")?.GetComponent<Image>(), "Gift");
				foreach (Text label in actions.GetComponentsInChildren<Text>(true))
				{
					label.color = Ink;
				}
			}
		}

		private static void StyleQuickAction(Transform action, string icon)
		{
			if (action == null || Get(icon) == null)
			{
				return;
			}
			Image previous = action.GetComponent<Image>();
			if (previous != null)
			{
				previous.color = Color.clear;
			}

			var art = new GameObject("InkActionIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
			art.layer = action.gameObject.layer;
			var rect = (RectTransform)art.transform;
			rect.SetParent(action, false);
			rect.anchorMin = new Vector2(0.21f, 0.38f);
			rect.anchorMax = new Vector2(0.79f, 0.97f);
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;

			Image image = art.GetComponent<Image>();
			Set(image, icon);
			image.raycastTarget = false;
		}

		private static void StyleBuilding(Button button, string icon)
		{
			Set(button.image, icon);
			foreach (Text label in button.GetComponentsInChildren<Text>(true))
			{
				label.color = Ink;
			}
			Set(button.transform.Find("Image")?.GetComponent<Image>(), "IvoryPlaque", false);
		}

		internal static void StyleMap(WorldMapPanel panel)
		{
			Set(panel.transform.Find("Bg")?.GetComponent<Image>(), "Paper", false);
			Set(panel.transform.Find("Title_image/BackImg")?.GetComponent<Image>(), "InkPlaque", false);
			Image backIcon = panel.BackBtn.transform.Find("Img")?.GetComponent<Image>();
			Set(backIcon, "Depart");
			panel.TitleText.color = Ink;
			UIManager.FitTextToRect(panel.TitleText);
		}
	}
}
