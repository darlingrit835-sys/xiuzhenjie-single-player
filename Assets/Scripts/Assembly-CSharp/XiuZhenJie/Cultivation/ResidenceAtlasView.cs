using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	// Illustration-space coordinates use a top-left origin. Each hit area, plaque and
	// light follows the same fitted paper, including when the phone has extra height.
	internal sealed class ResidenceAtlasView
	{
		private static readonly Vector2[] Landmarks =
		{
			new Vector2(.506f, .208f), new Vector2(.211f, .354f), new Vector2(.785f, .377f),
			new Vector2(.229f, .598f), new Vector2(.735f, .604f), new Vector2(.523f, .767f)
		};

		private static readonly Color Ink = new Color32(21, 43, 43, 255);
		private static readonly Color Deep = new Color32(19, 37, 39, 245);
		private static readonly Color Gold = new Color32(238, 206, 143, 255);
		private static readonly Color Jade = new Color32(248, 246, 222, 255);
		private static readonly Color Mist = new Color32(178, 201, 186, 255);
		private static readonly string[] Grades = { "普通", "黄阶", "玄阶", "地阶", "天阶" };
		private static readonly RoleFuZhiType[] CraftTypes = { RoleFuZhiType.LianQi, RoleFuZhiType.ZhiFu, RoleFuZhiType.FaShu };
		private static readonly string[] CraftNames = { "炼器坊", "符室", "战法室" };
		private static readonly string[] CraftDetails = { "铸造法器", "绘制灵符", "演练法术" };

		private readonly DongFuRoomPanel panel;
		private readonly Font font;
		private readonly Text grade, effects;
		private readonly Button retry;
		private readonly RectTransform craftSheet;
		private readonly VoidRealmCareerDesk careers;
		private readonly Text[] craftCaptions = new Text[CraftTypes.Length];

		internal ResidenceAtlasView(DongFuRoomPanel panel, DongFuRoomWnd owner)
		{
			this.panel = panel;
			font = panel.TitleText.font;
			foreach (Transform oldControl in panel.transform)
				oldControl.gameObject.SetActive(false);
			RoleFolio.Flat(panel.GetComponent<Image>(), Ink, true);

			var root = RoleFolio.Box(panel.transform, "ResidenceAtlasView");
			RoleFolio.Fill(root);
			var mount = RoleFolio.Image(root, "装裱", new Color32(56, 83, 77, 255));
			mount.sprite = InkInterfaceArt.Get("InkLandscapePaper");
			RoleFolio.Fill(mount.rectTransform);

			var painting = RoleFolio.Image(root, "洞府山水图", Color.white);
			painting.sprite = InkCultivationInterface.Get("ResidenceAtlas");
			var fit = painting.gameObject.AddComponent<AspectRatioFitter>();
			fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
			fit.aspectRatio = painting.sprite != null ? painting.sprite.rect.width / painting.sprite.rect.height : 2f / 3f;
			RectTransform map = painting.rectTransform;

			var lightLayer = RoleFolio.Box(map, "山门灵光");
			RoleFolio.Fill(lightLayer);
			lightLayer.gameObject.AddComponent<CaveResidenceLights>().SetLandmarks(Landmarks);

			panel.MiShiBtn = Landmark(map, 0, "密室");
			panel.LianGongFaBtn = Landmark(map, 1, "练功房");
			panel.WuDaoTaiBtn = Landmark(map, 2, "悟道台");
			panel.DiHuoShiBtn = Landmark(map, 3, "地火丹炉");
			var crafts = Landmark(map, 4, "百工坊");
			panel.LingTianBtn = Landmark(map, 5, "灵田");

			var header = RoleFolio.Image(root, "府额", Deep);
			RoleFolio.Band(header.rectTransform, 0, 0, 0, 43);
			Rule(header.transform, 0, 0, 1, 0, 0, 0, 0, 1);
			panel.LiKai_Btn = TopButton(header.transform, "离府", 0, 5, 43);
			panel.ShengJi_Btn = TopButton(header.transform, "洞府升阶", 1, -6, 67);
			panel.TitleText = Label(header.transform, "洞府", 20, Jade, TextAnchor.MiddleCenter);
			RoleFolio.Fill(panel.TitleText.rectTransform, 54, 77, 0, 0);

			var footer = RoleFolio.Image(root, "府印", Deep);
			Area(footer.rectTransform, 0, 0, 1, 0, 0, 0, 0, 38);
			Rule(footer.transform, 0, 1, 1, 1, 0, -1, 0, 0);
			grade = Label(footer.transform, "", 16, Gold, TextAnchor.MiddleLeft);
			Area(grade.rectTransform, 0, 0, .34f, 1, 13, 0, 0, 0);
			effects = Label(footer.transform, "", 13, Jade, TextAnchor.MiddleRight);
			Area(effects.rectTransform, .33f, 0, 1, 1, 0, 0, -12, 0);
			retry = TopButton(footer.transform, "重试", 1, -6, 53);
			retry.onClick.AddListener(() => owner.OnShow());

			craftSheet = Workshop(root, owner);
			careers = new VoidRealmCareerDesk(root, font, () => owner.OnShow());
			crafts.onClick.AddListener(() => craftSheet.gameObject.SetActive(true));
			BeginRead();
		}

		internal void BeginRead()
		{
			craftSheet.gameObject.SetActive(false);
			panel.TitleText.text = "洞府";
			grade.text = "府印";
			effects.text = "正在查阅府籍…";
			retry.gameObject.SetActive(false);
			SetOccupations(0, 0);
			careers.Reset();
		}

		internal void ReadFailed()
		{
			grade.text = "府籍未同步";
			effects.text = "";
			retry.gameObject.SetActive(true);
		}

		internal void ShowGrade(int current, string name)
		{
			if (!string.IsNullOrEmpty(name)) panel.TitleText.text = name;
			current = Mathf.Clamp(current, 0, DongFuResidenceState.MaxLevel);
			careers.Grade = current;
			grade.text = Grades[current] + "洞府";
			effects.text = $"修行 ×{(1 + current * .1f):0.0}  ·  灵田 {Mathf.Max(1, current) * 4} 块";
			retry.gameObject.SetActive(false);
		}

		internal void SetOccupations(int first, int second)
		{
			careers.SetRooms(first, second);
			for (int i = 0; i < CraftTypes.Length; i++)
			{
				int type = (int)CraftTypes[i];
				craftCaptions[i].text = (type == first || type == second ? "本修专职  ·  " : "") + CraftDetails[i];
			}
		}

		private Button Landmark(RectTransform map, int index, string name)
		{
			Vector2 point = Landmarks[index];
			float y = 1 - point.y;
			var touch = RoleFolio.Image(map, name, Color.clear, true);
			Area(touch.rectTransform, point.x - .155f, y - .098f, point.x + .155f, y + .063f);
			var plaque = RoleFolio.Image(touch.transform, "题签", Color.white);
			Area(plaque.rectTransform, .04f, 0, .96f, .32f);
			plaque.sprite = InkInterfaceArt.Get("InkPlaque");
			var caption = Label(plaque.transform, name, 17, Jade, TextAnchor.MiddleCenter);
			RoleFolio.Fill(caption.rectTransform, 3, 3);
			var tip = RoleFolio.Image(plaque.transform, "铜钉", Gold);
			var pin = tip.rectTransform;
			pin.anchorMin = pin.anchorMax = new Vector2(.07f, .5f);
			pin.sizeDelta = new Vector2(3, 3);
			pin.anchoredPosition = Vector2.zero;
			pin.localRotation = Quaternion.Euler(0, 0, 45);
			return Hit(touch.gameObject, plaque);
		}

		private RectTransform Workshop(RectTransform root, DongFuRoomWnd owner)
		{
			var veil = RoleFolio.Image(root, "百工坊分馆", new Color32(7, 20, 23, 180), true);
			RoleFolio.Fill(veil.rectTransform);
			var dismiss = Hit(veil.gameObject, veil);
			dismiss.transition = Selectable.Transition.None;
			dismiss.onClick.AddListener(() => veil.gameObject.SetActive(false));
			var page = RoleFolio.Image(veil.transform, "工坊名录", Deep, true);
			Area(page.rectTransform, .06f, .17f, .94f, .77f);
			Rule(page.transform, 0, 1, 1, 1, 0, -2, 0, 0);
			var title = Label(page.transform, "百工坊", 25, Gold, TextAnchor.MiddleLeft);
			Area(title.rectTransform, .07f, .78f, .62f, .97f);
			TopButton(page.transform, "专职内室", 1, -55, 76, .875f).onClick.AddListener(() => careers.Open());
			TopButton(page.transform, "收起", 1, -5, 46, .875f).onClick.AddListener(() => veil.gameObject.SetActive(false));
			for (int i = 0; i < CraftTypes.Length; i++)
			{
				var type = CraftTypes[i];
				float top = .76f - .235f * i;
				var row = RoleFolio.Image(page.transform, CraftNames[i], new Color32(40, 65, 62, 255), true);
				Area(row.rectTransform, .055f, top - .21f, .945f, top);
				var icon = RoleFolio.Image(row.transform, "印", Color.white);
				icon.sprite = ReleasedResources.Load<Sprite>("InkInterface/Professions/" + type);
				icon.preserveAspect = true;
				Area(icon.rectTransform, .02f, .12f, .21f, .88f);
				var name = Label(row.transform, CraftNames[i], 19, Jade);
				Area(name.rectTransform, .24f, .42f, .82f, .95f);
				craftCaptions[i] = Label(row.transform, CraftDetails[i], 13, Mist);
				Area(craftCaptions[i].rectTransform, .24f, .06f, .84f, .46f);
				var enter = Label(row.transform, "前往", 13, Gold, TextAnchor.MiddleRight);
				Area(enter.rectTransform, .81f, .10f, .96f, .90f);
				Hit(row.gameObject, row).onClick.AddListener(() =>
				{
					veil.gameObject.SetActive(false);
					owner.GetRoomClick(type)?.Invoke();
				});
			}
			return veil.rectTransform;
		}

		private Button TopButton(Transform parent, string name, float anchor, float dx, float width, float y = .5f)
		{
			var face = RoleFolio.Image(parent, name, Color.clear, true);
			var rect = face.rectTransform;
			rect.pivot = new Vector2(anchor, .5f);
			rect.anchorMin = rect.anchorMax = new Vector2(anchor, y);
			rect.sizeDelta = new Vector2(width, 37);
			rect.anchoredPosition = new Vector2(dx, 0);
			var caption = Label(rect, name, 14, Gold, TextAnchor.MiddleCenter);
			RoleFolio.Fill(caption.rectTransform);
			return Hit(face.gameObject, caption);
		}

		private Text Label(Transform parent, string text, int size, Color color, TextAnchor alignment = TextAnchor.MiddleLeft)
		{
			var label = RoleFolio.Label(parent, font, text, size, color, alignment);
			label.resizeTextForBestFit = true;
			label.resizeTextMinSize = Mathf.Max(10, size - 4);
			label.resizeTextMaxSize = size;
			return label;
		}

		private static Button Hit(GameObject node, Graphic target)
		{
			var button = node.AddComponent<Button>();
			button.targetGraphic = target;
			var colors = button.colors;
			colors.pressedColor = new Color32(173, 220, 193, 255);
			colors.fadeDuration = .09f;
			button.colors = colors;
			return button;
		}

		private static void Rule(Transform parent, float x1, float y1, float x2, float y2,
			float left, float bottom, float right, float top)
		{
			var rule = RoleFolio.Image(parent, "金线", new Color32(175, 152, 104, 211));
			Area(rule.rectTransform, x1, y1, x2, y2, left, bottom, right, top);
		}

		private static void Area(RectTransform rect, float x1, float y1, float x2, float y2,
			float left = 0, float bottom = 0, float right = 0, float top = 0)
		{
			rect.anchorMin = new Vector2(x1, y1);
			rect.anchorMax = new Vector2(x2, y2);
			rect.offsetMin = new Vector2(left, bottom);
			rect.offsetMax = new Vector2(right, top);
		}
	}
}
