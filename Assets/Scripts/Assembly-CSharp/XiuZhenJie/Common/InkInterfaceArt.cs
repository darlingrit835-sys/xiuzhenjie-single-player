using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkInterfaceArt
	{
		internal static readonly Color Ink = new Color32(36, 62, 53, 255);
		internal static readonly Color SoftInk = new Color32(65, 91, 81, 255);
		internal static readonly Color Seal = new Color32(151, 56, 43, 255);

		private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
		private static Font fallbackFont;
		private static readonly Dictionary<string, string> SharedSprites = new Dictionary<string, string>(StringComparer.Ordinal)
		{
			["返回@2x"] = "Back",
			["CloseBtnIcon"] = "Close",
			["wenhao"] = "Help",
			["BtnWhiteImg"] = "IvoryPlaque",
			["WhiteBtn_2"] = "IvoryPlaque",
			["分组 10@2x"] = "JadeButton",
			["BtnBlackStyle"] = "JadeButton",
			["BattleLiKaiBtnBg"] = "JadeButton",
			["矩形 copy 3@3x"] = "IvoryPlaque",
			["titlebg"] = "InkPlaque",
			["ZhoMen_HeiTitle"] = "InkPlaque",
			["ZhoMen_Huangtitle"] = "IvoryPlaque",
			["ZhoMen_JuXingShuRu"] = "IvoryPlaque",
			["ZhuCeInputBg"] = "IvoryPlaque",
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
			name = name switch
			{
				"Paper" => "PaperTexture",
				"IvoryPlaque" => "LightPlaque",
				"SealButton" => "LightAction",
				_ => name
			};
			if (!Sprites.TryGetValue(name, out Sprite sprite))
			{
				sprite = name switch
				{
					"PaperTexture" => ReleasedResources.Load<Sprite>("InkInterface/Paper"),
					"BrushPlaque" => ReleasedResources.Load<Sprite>("InkInterface/IvoryPlaque"),
					"Rank_Huang" => CreateItemFrame(name, new Color32(209, 207, 188, 255)),
					"Rank_Xuan" => CreateItemFrame(name, new Color32(136, 184, 145, 255)),
					"Rank_Di" => CreateItemFrame(name, new Color32(185, 154, 196, 255)),
					"Rank_Tian" => CreateItemFrame(name, new Color32(220, 171, 110, 255)),
					"LightPaper" => CreatePanelSprite(name, new Color32(255, 250, 235, 255),
						new Color32(243, 234, 211, 255), new Color32(191, 172, 135, 255)),
					"LightPlaque" => CreatePanelSprite(name, new Color32(248, 243, 222, 255),
						new Color32(227, 217, 182, 255), new Color32(155, 136, 95, 255)),
					"LightAction" => CreatePanelSprite(name, new Color32(184, 69, 52, 255),
						new Color32(153, 53, 42, 255), new Color32(137, 44, 36, 255)),
					"Parchment" => CreatePanelSprite(name, new Color32(251, 245, 232, 255),
						new Color32(239, 228, 202, 255), new Color32(178, 160, 123, 255)),
					"ParchmentHeading" => CreatePanelSprite(name, new Color32(250, 242, 222, 255),
						new Color32(232, 219, 188, 255), new Color32(184, 165, 128, 255)),
					"JadeSelection" => CreatePanelSprite(name, new Color32(217, 234, 212, 255),
						new Color32(183, 209, 185, 255), new Color32(93, 132, 106, 255)),
					"JadeButton" => CreatePanelSprite(name, new Color32(66, 107, 94, 255),
						new Color32(33, 63, 55, 255), new Color32(33, 62, 51, 255)),
					"InkSelectedCheck" => CreateSelectedCheck(),
					_ => ReleasedResources.Load<Sprite>("InkInterface/" + name)
				};
				Sprites.Add(name, sprite);
			}
			return sprite;
		}

		private static Sprite CreateSelectedCheck()
		{
			const int size = 32;
			var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
			texture.name = "InkSelectedCheck";
			texture.wrapMode = TextureWrapMode.Clamp;
			var pixels = new Color32[size * size];
			var start = new Vector2(7, 16);
			var joint = new Vector2(13, 10);
			var end = new Vector2(25, 22);
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					var point = new Vector2(x + .5f, y + .5f);
					float distance = Mathf.Min(DistanceToLine(point, start, joint), DistanceToLine(point, joint, end));
					Color color = Color.Lerp(new Color32(44, 82, 67, 255), InkDialogArt.Ivory, Mathf.Clamp01(2.5f - distance));
					float corner = Vector2.Max(Vector2.zero, new Vector2(Mathf.Abs(point.x - 16), Mathf.Abs(point.y - 16)) - Vector2.one * 12f).magnitude;
					color.a = Mathf.Clamp01(4f - corner);
					pixels[y * size + x] = color;
				}
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, true);
			var sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(.5f, .5f), 100f);
			sprite.name = texture.name;
			return sprite;
		}

		private static float DistanceToLine(Vector2 point, Vector2 a, Vector2 b)
		{
			var edge = b - a;
			var closest = a + edge * Mathf.Clamp01(Vector2.Dot(point - a, edge) / edge.sqrMagnitude);
			return Vector2.Distance(point, closest);
		}

		private static Sprite CreateItemFrame(string name, Color rim)
		{
			return CreatePanelSprite(name, new Color32(83, 102, 93, 255), new Color32(57, 73, 67, 255), rim);
		}

		private static Sprite CreatePanelSprite(string name, Color top, Color bottom, Color edge)
		{
			const int size = 48;
			const float radius = 3.5f;
			var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
			texture.name = name;
			texture.wrapMode = TextureWrapMode.Clamp;
			var pixels = new Color32[size * size];
			for (int y = 0; y < size; y++)
			{
				Color fill = Color.Lerp(bottom, top, y / (size - 1f));
				for (int x = 0; x < size; x++)
				{
					var corner = new Vector2(Mathf.Abs(x + 0.5f - size * 0.5f), Mathf.Abs(y + 0.5f - size * 0.5f));
					corner -= Vector2.one * (size * 0.5f - radius);
					float distance = Vector2.Max(corner, Vector2.zero).magnitude + Mathf.Min(Mathf.Max(corner.x, corner.y), 0f) - radius;
					Color pixel = Color.Lerp(fill, edge, Mathf.InverseLerp(-1.6f, -0.35f, distance));
					// A hairline and corner stops scale with the slice border, like the ruled edge of a scroll.
					float rim = Mathf.Min(Mathf.Min(x, size - 1 - x), Mathf.Min(y, size - 1 - y));
					if (rim == 3f)
						pixel = Color.Lerp(pixel, edge, 0.52f);
					pixel.a = Mathf.Clamp01(0.5f - distance);
					pixels[y * size + x] = pixel;
				}
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, true);
			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f),
				100f, 0, SpriteMeshType.FullRect, Vector4.one * 10f);
			sprite.name = name;
			return sprite;
		}

		// The legacy UI prefabs are loaded from Android bundles even in Play Mode.
		internal static void Apply(GameObject root, bool forge = false, string windowName = null, bool isListItem = false)
		{
			Text[] labels = root.GetComponentsInChildren<Text>(true);
			Image[] images = root.GetComponentsInChildren<Image>(true);
			InputField[] fields = root.GetComponentsInChildren<InputField>(true);
			foreach (Text text in labels)
			{
				// Trailing blank lines in static bundle captions consume the label's visible height.
				if (text.text.EndsWith("\n", StringComparison.Ordinal) && text.GetComponentInParent<InputField>() == null)
					text.text = text.text.TrimEnd('\r', '\n');
				if (text.font == null)
				{
					// The bundled loading screen references the same embedded Chinese typeface in the player and editor.
					if (fallbackFont == null)
						fallbackFont = ReleasedResources.Load<GameObject>("prefab/window/StartLoadSceneWnd")?.GetComponentInChildren<Text>(true)?.font;
					text.font = fallbackFont;
				}
				Color original = text.color;
				float brightest = Mathf.Max(original.r, original.g, original.b);
				float darkest = Mathf.Min(original.r, original.g, original.b);
				if (brightest - darkest < 0.025f && (brightest <= 0.25f || darkest >= 0.98f))
				{
					Color neutral = brightest <= 0.25f ? Ink : InkDialogArt.Ivory;
					neutral.a = original.a;
					text.color = neutral;
				}
			}
			foreach (InputField field in fields)
			{
				field.selectionColor = new Color32(102, 163, 136, 115);
			}
			foreach (Image image in images)
			{
				string name = image.sprite != null ? image.sprite.name : string.Empty;
				if (name.StartsWith("Man_", StringComparison.Ordinal) || name.StartsWith("Women_", StringComparison.Ordinal))
				{
					Sprite portrait = ReleasedResources.Load<Sprite>("InkPortraits/" + name);
					if (portrait != null)
					{
						image.sprite = portrait;
					}
				}
				else if (name == "OptionBg" || name == "ZhoMen_BG")
				{
					float alpha = image.color.a;
					Set(image, name == "ZhoMen_BG" ? "GuildHall" : forge ? "ForgeHall" : "MountainHall", false);
					image.color = new Color(1f, 0.99f, 0.95f, alpha);
					StyleSurfaceLabels(image, Ink);
				}
				else if (SharedSprites.TryGetValue(name, out string replacement))
				{
					bool preserveAspect = replacement != "IvoryPlaque" && replacement != "InkPlaque" &&
						replacement != "JadeButton" && replacement != "SealButton";
					float alpha = image.color.a;
					Set(image, replacement, preserveAspect);
					Color color = image.color;
					color.a = alpha;
					image.color = color;
					if (!preserveAspect)
						StyleSurfaceLabels(image, replacement == "IvoryPlaque" ? Ink : new Color32(255, 244, 216, 255));
				}
				else if ((name == "Mask" || image.sprite == null) && IsLightBackground(image))
				{
					Set(image, "Paper", false);
					StyleSurfaceLabels(image, Ink);
				}
			}
			if (!InkModuleArt.HasDedicatedStyle(windowName ?? root.name))
				InkModuleArt.Apply(root, isListItem, images, labels, fields);
			if (isListItem && (windowName == "ZongMenChatWnd" || windowName == "ZongMenChatWnd.prefab") &&
				root.TryGetComponent<ChatItem>(out var chat))
				chat.UsePaperCaptions();
			if (isListItem && (windowName == "DiHuoGeWnd" || windowName == "DiHuoGeWnd.prefab" ||
				windowName == "HuaFuChooseWnd" || windowName == "HuaFuChooseWnd.prefab"))
			{
				if (root.TryGetComponent<Toggle>(out var stage) &&
					(root.TryGetComponent<Common_TogItem>(out _) || root.TryGetComponent<DiHuoGeChooseSmallItem>(out _)))
				{
					Set(stage.targetGraphic as Image, "LightPaper", false);
					Set(stage.graphic as Image, "JadeButton", false);
					var caption = root.GetComponentInChildren<Text>(true);
					if (caption != null) caption.color = stage.isOn ? InkDialogArt.Ivory : Ink;
				}
				if (root.TryGetComponent<UI_GoodsTogItem>(out var item) && item.ClickTog.graphic is Image selected)
				{
					// A corner mark leaves the five-element badge and its text clear in both states.
					Set(selected, "InkSelectedCheck");
					var mark = selected.rectTransform;
					mark.anchorMin = mark.anchorMax = mark.pivot = Vector2.one;
					mark.sizeDelta = new Vector2(14f, 14f);
					mark.anchoredPosition = new Vector2(-1f, -1f);
				}
			}
			if (!isListItem)
			{
				InkCraftHalls.StyleFacility(root);
				ArtisanArchiveArt.Apply(root, windowName ?? root.name);
			}
			if (!isListItem && root.TryGetComponent<CreateRolePanel>(out var rolePanel))
				rolePanel.StyleGenderCaptions();
			if (!isListItem && root.TryGetComponent<QiZhenGePanel>(out var market) && market.SouSuo != null)
			{
				if (market.SouSuo.placeholder is Text caption) caption.color = SoftInk;
				if (market.SouSuo.textComponent != null) market.SouSuo.textComponent.color = Ink;
			}
			if (!isListItem && root.TryGetComponent<CashGoodsPanel>(out _))
			{
				var benefit = root.transform.Find("Content/YueKaPanel/XuanTianLingBg/Text_5")?.GetComponent<Text>();
				if (benefit != null && benefit.preferredHeight > benefit.rectTransform.rect.height)
					benefit.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, benefit.preferredHeight);
			}
		}

		// Only tint captions belonging to this surface. Nested portraits, badges and rarity text keep their own palette.
		private static void StyleSurfaceLabels(Image surface, Color ink)
		{
			foreach (Text label in surface.GetComponentsInChildren<Text>(true))
			{
				Color previous = label.color;
				if (Mathf.Max(previous.r, previous.g, previous.b) - Mathf.Min(previous.r, previous.g, previous.b) > 0.18f)
					continue;
				var parent = label.transform.parent;
				while (parent != surface.transform && parent != null)
				{
					if (parent.TryGetComponent<Image>(out var background) && background.enabled && background.color.a > .1f)
						break;
					parent = parent.parent;
				}
				if (parent != surface.transform) continue;
				ink.a = previous.a;
				label.color = ink;
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
			bool hasBorder = sprite.border.sqrMagnitude > 0f;
			image.type = hasBorder ? Image.Type.Sliced : Image.Type.Simple;
			image.preserveAspect = !hasBorder && preserveAspect;
			image.color = Color.white;
		}

		internal static void StyleMap(WorldMapPanel panel)
		{
			Set(panel.transform.Find("Bg")?.GetComponent<Image>(), "Paper", false);
			Set(panel.transform.Find("Title_image")?.GetComponent<Image>(), "LightPlaque", false);
			Set(panel.transform.Find("Title_image/BackImg")?.GetComponent<Image>(), "LightPaper", false);
			Image backIcon = panel.BackBtn.transform.Find("Img")?.GetComponent<Image>();
			Set(backIcon, "Back");
			panel.TitleText.color = Ink;
			UIManager.FitTextToRect(panel.TitleText);
			StyleMapTab(panel.LiLianDiTuTog);
			StyleMapTab(panel.ZongPaiShanMenTog);
		}

		private static void StyleMapTab(Toggle tab)
		{
			Set(tab.targetGraphic as Image, "LightPaper", false);
			Set(tab.graphic as Image, "LightPlaque", false);
			Text label = tab.GetComponentInChildren<Text>(true);
			if (label != null)
				label.color = Ink;
		}
	}
}
