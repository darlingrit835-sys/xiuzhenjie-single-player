using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkModuleArt
	{
		// These windows build and colour their own content, including their dynamic rows.
		private static readonly HashSet<string> DedicatedViews = new HashSet<string>(StringComparer.Ordinal)
		{
			"SignInWnd", "SignUpWnd", "OptionWnd", "MainTitleWnd", "MainCityWnd", "ZhongJieFangShiWnd", "WorldMapWnd",
			"PopUpWnd", "KeyBoardWnd", "Common_ChooseGoodsNumWnd", "ZongMenWnd", "XuShenDianWnd",
			"WanFaGeWnd", "NaJieSpaceWnd", "DiHuoGeWnd", "HuaFuChooseWnd", "CanWuWnd", "DaoYouChatWnd", "LingQuanJiuSiWnd",
			"QiZhenGeWnd", "LianQi_ChuangZaoMianWnd", "LT_LingTianMainWnd", "ZhanDouSettlementWnd",
			"BattleWnd", "StartLogoWnd", "StartLoadSceneWnd", "LoadingWnd", "MaskWnd"
		};

		internal static bool HasDedicatedStyle(string name)
		{
			return DedicatedViews.Contains(name.Replace("(Clone)", "").Replace(".prefab", ""));
		}

		internal static void Apply(GameObject root, bool listItem, Image[] images, Text[] labels, InputField[] fields)
		{
			var paperCaptions = new Dictionary<Transform, Color>();
			var extents = new Dictionary<RectTransform, Vector2>();
			var rootRect = root.transform as RectTransform;
			bool guide = root.TryGetComponent<GuideNewPanel>(out _);
			bool guildChat = root.TryGetComponent<ZongMenChatPanel>(out _);
			bool techniqueGrid = !listItem && root.TryGetComponent<ChuangZhaoGongFaPanel>(out _);
			if (techniqueGrid)
			{
				// The hexagonal hit map keeps its own rays and textures above this opaque underlay.
				var backing = root.GetComponent<Image>();
				if (backing == null)
				{
					backing = root.AddComponent<Image>();
					backing.raycastTarget = false;
				}
				InkInterfaceArt.Set(backing, "InkLedgerPaper", false);
				paperCaptions[root.transform] = InkInterfaceArt.Ink;
			}
			if (!listItem && (root.TryGetComponent<TuPoDuJiePanel>(out _) || root.TryGetComponent<TuPoDuJieExmplePanel>(out _)))
			{
				// The label and the animated gauge are siblings. A lower paper layer gives the row a consistent tone.
				var row = root.transform.Find("ZhenYuanProgress");
				if (row != null)
				{
					var backing = row.GetComponent<Image>() ?? row.gameObject.AddComponent<Image>();
					backing.raycastTarget = false;
					SetSurface(backing, "LightPaper", paperCaptions);
				}
			}
			foreach (var image in images)
			{
				if (!image.enabled || image.color.a < .16f || image.type == Image.Type.Filled ||
					(image.TryGetComponent<Mask>(out var stencil) && !stencil.showMaskGraphic))
					continue;
				string sprite = image.sprite != null ? image.sprite.name : "";
				string name = image.name.Trim();
				// A patterned stencil can carry its own non-rectangular alpha silhouette.
				if (stencil != null && sprite != "" && sprite != "Mask") continue;
				if (guildChat && image.transform == root.transform && sprite == "JiuShiBgImg")
				{
					SetSurface(image, "GuildHall", paperCaptions);
					continue;
				}
				if (guildChat && name == "ChatContent" && IsPlainSurface(sprite))
				{
					SetSurface(image, "InkLedgerPaper", paperCaptions);
					image.color = new Color(1f, 1f, 1f, .97f);
					continue;
				}
				if (guide && name == "Img" && (sprite.StartsWith("JuXin", StringComparison.Ordinal) || sprite == "BeiJing"))
				{
					// The speaker, arrow and cut-out target are independent children of the dialogue plate.
					SetDarkPlate(image, paperCaptions);
					continue;
				}
				if (rootRect != null && StylePageSurface(image, rootRect, listItem, name, sprite, extents, paperCaptions))
					continue;
				if (StyleNamedSurface(image, name, sprite, paperCaptions))
					continue;

				if (!IsPlainSurface(sprite) || !IsNeutral(image.color)) continue;
				bool windowRoot = !listItem && image.gameObject == root;
				bool panel = IsPanelName(name) && HasPanelSize(image.rectTransform);
				if (image.color.a < .91f)
				{
					// Full-screen washes must still reveal the underlying scene and block the same taps.
					if (image.color.maxColorComponent < .42f && (windowRoot || panel || IsWashName(name) || guide && image.transform.parent == root.transform) &&
						HasPanelSize(image.rectTransform))
					{
						Color wash = new Color32(33, 50, 47, 255);
						wash.a = image.color.a;
						image.color = wash;
					}
					continue;
				}

				if (image.TryGetComponent<InputField>(out _))
					SetSurface(image, "LightPaper", paperCaptions);
				else if (image.TryGetComponent<Button>(out _) && !HasPanelSize(image.rectTransform) &&
					image.GetComponentInChildren<Text>(true) != null)
					SetSurface(image, "ParchmentHeading", paperCaptions);
				else if (windowRoot)
					SetSurface(image, "Paper", paperCaptions);
				else if (IsHeadingName(name) || IsBottomBar(name))
					SetSurface(image, "ParchmentHeading", paperCaptions);
				else if (panel)
					SetSurface(image, "Parchment", paperCaptions);
			}

			foreach (Toggle toggle in root.GetComponentsInChildren<Toggle>(true))
				StyleTextTab(toggle, paperCaptions);
			foreach (var label in labels)
			{
				if (techniqueGrid && label.name == "GreenText")
				{
					label.color = new Color32(11, 48, 34, 255);
					continue;
				}
				if (label.name.EndsWith("KuCunShuBaiFenBiText", StringComparison.Ordinal))
				{
					// The bar fill moves underneath the percentage as the stock changes.
					label.color = InkInterfaceArt.Ink;
					var outline = label.GetComponent<Outline>() ?? label.gameObject.AddComponent<Outline>();
					outline.effectColor = InkDialogArt.Ivory;
					outline.effectDistance = new Vector2(.7f, -.7f);
					continue;
				}
				for (Transform parent = label.transform.parent; parent != null; parent = parent.parent)
				{
					if (paperCaptions.TryGetValue(parent, out Color ink))
					{
						label.color = LegibleColor(label.color, ink);
						break;
					}
					// Keep labels on untouched illustrations, item grades, effects and dark badges legible.
					if (parent == root.transform ||
						(parent.TryGetComponent<Image>(out var surface) && surface.enabled && surface.color.a > .2f &&
						 !surface.TryGetComponent<Mask>(out _) && !surface.TryGetComponent<RectMask2D>(out _)))
						break;
				}
			}
			foreach (var field in fields)
			{
				if (!paperCaptions.ContainsKey(field.transform)) continue;
				if (field.textComponent != null) field.textComponent.color = InkInterfaceArt.Ink;
				if (field.placeholder is Text hint) hint.color = InkInterfaceArt.SoftInk;
			}
		}

		private static bool StylePageSurface(Image image, RectTransform root, bool listItem, string name, string sprite,
			Dictionary<RectTransform, Vector2> extents, Dictionary<Transform, Color> captions)
		{
			bool plain = IsPlainSurface(sprite);
			bool neutralPaper = sprite == "Paper" || sprite == "PaperTexture" || sprite == "Parchment" || sprite == "LightPaper";
			if ((!plain && !neutralPaper) || !IsNeutral(image.color) || image.TryGetComponent<Selectable>(out _)) return false;
			var size = SurfaceSize(image.rectTransform, root, listItem, extents);
			// Do not treat a stretched checkmark or a small rank badge as a large surface.
			if (size.x < 128f || size.y < 42f) return false;
			bool isRoot = image.transform == root;
			bool recipeBacking = name == "Image" && image.transform.parent == root &&
				(root.name.StartsWith("DiHuoGeLianZhiBuZhouSheZhiWnd", StringComparison.Ordinal) ||
				 root.name.StartsWith("ChuangZhao_LianZhiDengJieWnd", StringComparison.Ordinal));
			if (image.color.a < .925f && !recipeBacking) return false;
			if (IsHeadingName(name) || IsBottomBar(name) || name.StartsWith("Title", StringComparison.OrdinalIgnoreCase) ||
				name.IndexOf("Progress", StringComparison.OrdinalIgnoreCase) >= 0 ||
				name.IndexOf("Portrait", StringComparison.OrdinalIgnoreCase) >= 0 ||
				name.IndexOf("TouXiang", StringComparison.OrdinalIgnoreCase) >= 0 ||
				name == "Fill" || name == "Checkmark") return false;
			bool wide = size.x >= 186f && size.y >= 95f;
			bool panel = isRoot || IsPanelName(name) || image.TryGetComponent<ScrollRect>(out _) ||
				wide && (name.StartsWith("Image", StringComparison.Ordinal) || name.StartsWith("BG_", StringComparison.OrdinalIgnoreCase));
			if (!panel && image.GetComponentInChildren<Text>(true) == null) return false;
			if (name == "Mask" && image.transform.childCount == 0) return false;
			bool page = size.x >= 190f && size.y >= 210f && size.y > size.x * .82f &&
				!HasIllustratedAncestor(image.transform, root);
			SetSurface(image, page ? "InkLandscapePaper" : "InkLedgerPaper", captions);
			if (recipeBacking)
			{
				image.color = Color.white;
				captions[root] = InkInterfaceArt.Ink;
			}
			return true;
		}

		private static bool HasIllustratedAncestor(Transform item, Transform root)
		{
			if (item == root) return false;
			for (var parent = item.parent; parent != null; parent = parent.parent)
			{
				if (parent.TryGetComponent<Image>(out var image) && image.sprite != null &&
					(image.sprite.name == "InkLandscapePaper" || image.sprite.name == "InkLedgerPaper")) return true;
				if (parent == root) break;
			}
			return false;
		}

		private static Vector2 SurfaceSize(RectTransform rect, RectTransform root, bool listItem, Dictionary<RectTransform, Vector2> extents)
		{
			if (extents.TryGetValue(rect, out var size)) return size;
			var parent = rect.parent as RectTransform;
			if (rect == root || parent == null)
			{
				size = rect.rect.size;
				if (!listItem)
				{
					// The initial window is styled before it is parented to the game's portrait Canvas.
					var nominal = Vector2.Scale(rect.anchorMax - rect.anchorMin, new Vector2(360f, 800f)) + rect.sizeDelta;
					// Pooled windows can have a temporary 100 x 100 parent before Open attaches them.
					size = Vector2.Max(size, nominal);
				}
			}
			else
				size = Vector2.Scale(rect.anchorMax - rect.anchorMin, SurfaceSize(parent, root, listItem, extents)) + rect.sizeDelta;
			size = Vector2.Max(Vector2.zero, size);
			extents[rect] = size;
			return size;
		}

		private static bool StyleNamedSurface(Image image, string name, string sprite, Dictionary<Transform, Color> paperCaptions)
		{
			var parent = image.transform.parent;
			// Legacy pop-ups place their caption beside the decorative strip, instead of inside it.
			if ((sprite == "InkPlaque" || sprite == "titleBG 1" || sprite == "AH_AnBiaoTiBG") &&
				parent != null && parent.Find("TitleName") != null && (name == "Image" || name == "BG_Image"))
			{
				SetSurface(image, "ParchmentHeading", paperCaptions);
				paperCaptions[parent] = InkInterfaceArt.Ink;
				return true;
			}
			switch (sprite)
			{
			case "Paper":
			case "PaperTexture":
			case "LightPaper":
			case "InkLandscapePaper":
			case "InkLedgerPaper":
			case "InkRankLedger":
			case "Parchment":
			case "LightPlaque":
			case "ParchmentHeading":
			case "JadeSelection":
				paperCaptions[image.transform] = InkInterfaceArt.Ink;
				return true;
			case "JadeButton":
			case "LightAction":
			case "InkPlaque":
				paperCaptions[image.transform] = InkDialogArt.Ivory;
				return true;
			case "HuaFuBoxBg":
			case "CanWuBatBJ":
			case "矩形 copy 5@3x(4)":
			case "BtnBg_Green":
			case "Cash_TitleBtnNormal":
			case "Cash_TitleBtnChoose":
				if (name != "Background" && name != "Selected" && name != "Checkmark") return false;
				var toggle = image.GetComponentInParent<Toggle>(true);
				if (toggle == null) return false;
				SetSurface(image, name == "Background" ? "ParchmentHeading" : "JadeSelection", paperCaptions);
				if (sprite == "HuaFuBoxBg" && toggle.graphic is Image activeMark && activeMark.transform.parent == image.transform)
					SetSurface(activeMark, "JadeSelection", paperCaptions);
				SetTabCaption(toggle, paperCaptions);
				return true;
			case "矩形 copy 13@3x(1)":
				if (!image.GetComponentInChildren<Text>(true)) return false;
				SetSurface(image, "LightPaper", paperCaptions);
				return true;
			case "ZhoMen_JuXingHuangBian":
			case "ZhoMen_FangXingHuangBian":
				if (name == "LingShiImage" || name == "DanYaoinImage" || name == "FaQiImage")
				{
					SetDarkPlate(image, paperCaptions);
					return true;
				}
				if (name == "ZhiFaZhangLao")
				{
					SetSurface(image, "Parchment", paperCaptions);
					image.color = Color.white;
					return true;
				}
				// These sprites only draw a fine border; captions share the surface behind it.
				for (var ancestor = parent; ancestor != null; ancestor = ancestor.parent)
				{
					if (!paperCaptions.TryGetValue(ancestor, out var foreground)) continue;
					paperCaptions[image.transform] = foreground;
					return true;
				}
				return false;
			case "ZhoMen_JuZengBut":
				if (image.GetComponentInChildren<Text>(true) == null) return false;
				SetDarkPlate(image, paperCaptions);
				return true;
			case "ProgressBg":
				if (name != "ProgressBg" || !image.GetComponentInChildren<Text>(true)) return false;
				SetDarkPlate(image, paperCaptions);
				return true;
			case "Back":
				if (name == "FanHui_btn" && image.GetComponentInChildren<Text>(true) != null)
				{
					SetSurface(image, "ParchmentHeading", paperCaptions);
					return true;
				}
				if (name != "QueDing" || !image.GetComponentInChildren<Text>(true)) return false;
				SetDarkPlate(image, paperCaptions);
				return true;
			case "YellowCubeBtn":
				if (!image.TryGetComponent<Button>(out _)) return false;
				SetSurface(image, "ParchmentHeading", paperCaptions);
				return true;
			case "BlueCubeBg":
				if (image.GetComponentInChildren<Text>(true) == null) return false;
				paperCaptions[image.transform] = InkInterfaceArt.Ink;
				return true;
			case "AH_HengFu":
			case "CanWuZhongTiShiImg":
			case "FloorTitle":
			case "GongXianImgIcon":
				if (image.GetComponentInChildren<Text>(true) == null) return false;
				SetSurface(image, "ParchmentHeading", paperCaptions);
				return true;
			case "矩形 copy 12@3x":
				if (name != "NengLiang") return false;
				SetSurface(image, "ParchmentHeading", paperCaptions);
				return true;
			case "ZhoMen_AnSe":
				if (image.GetComponentInChildren<Text>(true) == null) return false;
				SetSurface(image, "InkLedgerPaper", paperCaptions);
				return true;
			case "":
			case "Mask":
				if (name == "JiaJia" && image.transform.Find("ShuLiang_text") != null)
				{
					SetSurface(image, "LightPaper", paperCaptions);
					return true;
				}
				if (name == "JiaJia" && parent != null && parent.Find("QiPaiInput") != null)
				{
					SetSurface(image, "LightPaper", paperCaptions);
					paperCaptions[parent.Find("QiPaiInput")] = InkInterfaceArt.Ink;
					return true;
				}
				if (!int.TryParse(name, out _) || !image.GetComponentInChildren<Text>(true)) return false;
				// Preserve each currency's metal colour and choose a legible foreground for the value.
				paperCaptions[image.transform] = Luminance(image.color) > .2f ? InkInterfaceArt.Ink : InkDialogArt.Ivory;
				return true;
			case "ChooseBg":
				if (!name.Equals("Choose_bg", StringComparison.OrdinalIgnoreCase)) return false;
				SetDarkPlate(image, paperCaptions);
				return true;
			case "BG_xiao 1":
			case "BG":
				if (name != "BG_Shang" && name != "BG_Xia") return false;
				SetSurface(image, name == "BG_Shang" ? "InkLandscapePaper" : "InkLedgerPaper", paperCaptions);
				return true;
			case "ChouJiang_Bg":
			case "ChouJiang_Bg_2":
			case "RankWndBg":
				if (name != "Content" && name != "ContentBg" && name != "GuiZe") return false;
				SetSurface(image, "InkLandscapePaper", paperCaptions);
				return true;
			case "XuShenBangDanBg":
				if (name != "Content") return false;
				SetSurface(image, "InkRankLedger", paperCaptions);
				return true;
			case "Mask@3x":
				if (!name.Equals("Bg", StringComparison.OrdinalIgnoreCase)) return false;
				SetSurface(image, "InkLandscapePaper", paperCaptions);
				if (parent != null) paperCaptions[parent] = InkInterfaceArt.Ink;
				return true;
			case "bg":
				if (name != "CityBg_image") return false;
				SetSurface(image, "InkLandscapePaper", paperCaptions);
				return true;
			case "ChuangZaoFaShuBg":
				if (name != "Bg") return false;
				SetSurface(image, "InkLandscapePaper", paperCaptions);
				return true;
			case "ChuangZaoFaShuTableBg":
				if (name != "TableContent") return false;
				var gridInk = InkInterfaceArt.SoftInk;
				gridInk.a = image.color.a;
				image.color = gridInk;
				paperCaptions[image.transform] = InkInterfaceArt.Ink;
				return true;
			case "ChuangZhaoGongFaBg":
				if (name != "Bg") return false;
				image.color = new Color(1f, 1f, 1f, .30f);
				paperCaptions[image.transform] = InkInterfaceArt.Ink;
				return true;
			case "ChuangZhaoGongFaBgMask":
				if (name != "BgMask") return false;
				image.color = new Color(1f, 1f, 1f, .04f);
				return true;
			case "base@2x 1":
				if (name != "InputBg") return false;
				SetSurface(image, "InkLandscapePaper", paperCaptions);
				return true;
			case "Cover@2x":
			case "Cover@3x":
			case "Cover Copy@3x":
			case "矩形@3x":
				if (!IsBottomBar(name) && !IsHeadingName(name)) return false;
				SetSurface(image, "ParchmentHeading", paperCaptions);
				return true;
			case "矩形@2x":
			case "Mask Copy 2@3x":
			case "Mask@3x(13)":
			case "矩形 copy 5@3x":
			case "JuXingBai":
				if (!IsHeadingName(name) && !name.Equals("ContentBg", StringComparison.OrdinalIgnoreCase)) return false;
				SetSurface(image, "ParchmentHeading", paperCaptions);
				return true;
			case "矩形 copy 12@2x":
				if (name != "ValueBg") return false;
				SetSurface(image, "LightPaper", paperCaptions);
				return true;
			case "JuXing_Huang":
				if (!image.TryGetComponent<Button>(out _)) return false;
				SetSurface(image, "ParchmentHeading", paperCaptions);
				return true;
			case "分组 6@3x(1)":
				if (name != "Bg") return false;
				SetSurface(image, "Parchment", paperCaptions);
				return true;
			case "DanShiTaBg":
				if (name != "CityBg_image") return false;
				SetSurface(image, "MountainHall", paperCaptions);
				return true;
			case "DiHuoGeBG":
				if (name != "Bg") return false;
				SetSurface(image, "ForgeHall", paperCaptions);
				return true;
			default:
				return false;
			}
		}

		private static void StyleTextTab(Toggle toggle, Dictionary<Transform, Color> paperCaptions)
		{
			if (!(toggle.targetGraphic is Image face) || !(toggle.graphic is Image mark) || face == mark ||
				!IsPlainSurface(face.sprite != null ? face.sprite.name : "") || face.color.a < .9f ||
				!IsNeutral(face.color) || toggle.GetComponentInChildren<Text>(true) == null) return;

			string sprite = mark.sprite != null ? mark.sprite.name : "";
			// Checkbox and radio glyphs keep their shape. Only recolour a full tile selection.
			if (!IsPlainSurface(sprite) && sprite != "矩形 copy 7@3x" && sprite != "分组 11@3x") return;
			var r = mark.rectTransform;
			Vector2 span = r.anchorMax - r.anchorMin;
			bool fullFace = span.x >= .8f && span.y >= .8f || r.sizeDelta.x >= 45 && r.sizeDelta.y >= 19;
			if (!fullFace) return;

			SetSurface(face, "LightPaper", paperCaptions);
			SetSurface(mark, "JadeSelection", paperCaptions);
			SetTabCaption(toggle, paperCaptions);
		}

		private static void SetTabCaption(Toggle toggle, Dictionary<Transform, Color> paperCaptions)
		{
			paperCaptions[toggle.transform] = InkInterfaceArt.Ink;
			if (toggle.TryGetComponent<ToggleItemTextChangeColor>(out var textState))
			{
				textState.IsOnFalseColor = InkInterfaceArt.SoftInk;
				textState.IsOnTrueColor = InkInterfaceArt.Ink;
			}
		}

		private static float Luminance(Color color)
		{
			var linear = color.linear;
			return .2126f * linear.r + .7152f * linear.g + .0722f * linear.b;
		}

		private static Color LegibleColor(Color previous, Color foreground)
		{
			if (IsNeutral(previous))
			{
				if (foreground == InkInterfaceArt.Ink && previous.grayscale > .32f && previous.grayscale < .76f)
					foreground = InkInterfaceArt.SoftInk;
			}
			else
			{
				bool onDark = foreground == InkDialogArt.Ivory;
				float luminance = Luminance(previous);
				if (onDark ? luminance >= .52f : luminance <= .15f) return previous;
				Color.RGBToHSV(previous, out var hue, out var saturation, out var value);
				foreground = onDark
					? Color.HSVToRGB(hue, Mathf.Min(saturation, .28f), Mathf.Max(value, .95f))
					: Color.HSVToRGB(hue, Mathf.Max(saturation, .48f), Mathf.Min(value, .47f));
			}
			foreground.a = previous.a;
			return foreground;
		}

		private static bool IsPlainSurface(string sprite) => sprite == "" || sprite == "Mask" ||
			sprite == "UISprite" || sprite == "Background" || sprite == "YuanJiaoMask";

		private static bool IsNeutral(Color c)
		{
			return Mathf.Max(c.r, Mathf.Max(c.g, c.b)) - Mathf.Min(c.r, Mathf.Min(c.g, c.b)) <= .18f;
		}

		private static bool HasPanelSize(RectTransform rect)
		{
			Vector2 span = rect.anchorMax - rect.anchorMin;
			return (rect.sizeDelta.x >= 126f || span.x >= .48f) && (rect.sizeDelta.y >= 48f || span.y >= .28f);
		}

		private static bool IsBottomBar(string name) => name.Equals("BottomBg", StringComparison.OrdinalIgnoreCase) ||
			name.Equals("BottomImg", StringComparison.OrdinalIgnoreCase) || name.Equals("buttonBg", StringComparison.OrdinalIgnoreCase) ||
			name == "BottomTitle_image";

		private static bool IsHeadingName(string name) => name.Equals("Title", StringComparison.OrdinalIgnoreCase) ||
			name.Equals("Name_bg", StringComparison.OrdinalIgnoreCase) || name.EndsWith("TitleBg", StringComparison.OrdinalIgnoreCase) ||
			name.EndsWith("TitleImg", StringComparison.OrdinalIgnoreCase) || name == "Title_image";

		private static bool IsWashName(string name) => name.IndexOf("Mask", StringComparison.OrdinalIgnoreCase) >= 0 ||
			name.Equals("Bg_Btn", StringComparison.OrdinalIgnoreCase) || name.Equals("BackGround", StringComparison.OrdinalIgnoreCase);

		private static bool IsPanelName(string name) => name.Equals("Bg", StringComparison.OrdinalIgnoreCase) ||
			name.Equals("BackGround", StringComparison.OrdinalIgnoreCase) || name.EndsWith("Bg", StringComparison.OrdinalIgnoreCase) ||
			name.EndsWith("Content", StringComparison.OrdinalIgnoreCase) || name.EndsWith("Panel", StringComparison.OrdinalIgnoreCase) ||
			name.EndsWith("Wnd", StringComparison.Ordinal) || name == "Des" || name == "XiangQing" ||
			name == "Tip" || name == "ZhiFaZhangLao_Wu" || name == "Window";

		private static void SetDarkPlate(Image image, Dictionary<Transform, Color> paperCaptions)
		{
			SetSurface(image, "JadeButton", paperCaptions);
			paperCaptions[image.transform] = InkDialogArt.Ivory;
		}

		private static void SetSurface(Image image, string art, Dictionary<Transform, Color> paperCaptions)
		{
			float alpha = image.color.a;
			InkInterfaceArt.Set(image, art, false);
			var color = image.color;
			color.a = alpha;
			image.color = color;
			paperCaptions[image.transform] = InkInterfaceArt.Ink;
		}
	}
}
