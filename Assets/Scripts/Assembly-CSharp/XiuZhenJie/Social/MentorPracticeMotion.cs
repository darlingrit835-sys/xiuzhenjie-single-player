using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class MentorPracticeMotion : MonoBehaviour
	{
		private const float Duration = 0.88f;
		private static readonly Color Jade = new Color32(62, 143, 113, 255);
		private static readonly Color Gold = new Color32(187, 131, 48, 255);
		private static readonly Color Vermilion = new Color32(178, 64, 49, 255);
		private static readonly Color TeacherInk = new Color32(255, 241, 219, 255);
		private static readonly Color DiscipleInk = new Color32(206, 233, 221, 255);
		private static Sprite wash;
		private static Sprite ribbon;
		private static Sprite glint;
		private static Sprite meditator;

		private ShiTuToZiList action;
		private CanvasGroup opacity;
		private Image ink;
		private Image emblem;
		private Image seal;
		private Image teacher;
		private Image disciple;
		private Image[] acupoints;
		private Text caption;
		private readonly Image[] strokes = new Image[5];
		private readonly Image[] motes = new Image[10];
		private float began;
		private Action completed;

		internal static void Play(RectTransform card, ShiTuToZiList action, string label, Action completed)
		{
			if (card == null || !card.gameObject.activeInHierarchy)
			{
				completed?.Invoke();
				return;
			}

			var layer = new GameObject("MentorPracticeMotion", typeof(RectTransform), typeof(CanvasGroup));
			layer.layer = card.gameObject.layer;
			var rect = (RectTransform)layer.transform;
			rect.SetParent(card, false);
			rect.anchorMin = rect.anchorMax = Vector2.one * 0.5f;
			rect.sizeDelta = new Vector2(Mathf.Min(card.rect.width - 2f, 340f), Mathf.Min(card.rect.height - 2f, 298f));
			layer.AddComponent<MentorPracticeMotion>().Begin(card, action, label, completed);
		}

		private void Begin(RectTransform card, ShiTuToZiList kind, string label, Action callback)
		{
			completed = callback;
			action = kind;
			opacity = GetComponent<CanvasGroup>();
			opacity.blocksRaycasts = false;
			opacity.interactable = false;
			opacity.alpha = 0f;
			wash = wash ?? ReleasedResources.Load<Sprite>("InkCultivation/QiAll");
			ribbon = ribbon ?? ReleasedResources.Load<Sprite>("InkCultivation/QiShui");
			glint = glint ?? ReleasedResources.Load<Sprite>("InkCultivation/QiJin");

			Image paper = AddImage("RicePaper", null);
			InkDialogArt.SetPaper(paper);
			InkDialogArt.Inset(paper.rectTransform, 0f, 0f, 0f, 0f);
			ink = AddImage("InkWash", wash);
			if (action != ShiTuToZiList.ChuShi)
			{
				meditator = meditator ?? ReleasedResources.Load<Sprite>("InkCultivation/Meditator");
				if (action == ShiTuToZiList.ChuangGong)
					teacher = AddSeatedFigure("TeacherInkFigure");
				disciple = AddSeatedFigure("DiscipleInkFigure");
				if (teacher != null)
					disciple.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
			}
			for (int i = 0; i < strokes.Length; i++)
				strokes[i] = AddImage("BrushStroke", ribbon);
			for (int i = 0; i < motes.Length; i++)
				motes[i] = AddImage("SpiritMote", glint);

			Sprite emblemSprite = action switch
			{
				ShiTuToZiList.ChuShi => InkInterfaceArt.Get("Scroll"),
				ShiTuToZiList.ToMai => InkInterfaceArt.Get("JadePendant"),
				_ => ReleasedResources.Load<Sprite>("InkCultivation/Breath")
			};
			emblem = AddImage("PracticeEmblem", emblemSprite);
			if (action == ShiTuToZiList.ChuShi)
				seal = AddImage("GraduationSeal", InkInterfaceArt.Get("AttendanceSeal"));
			if (action == ShiTuToZiList.ToMai)
			{
				acupoints = new Image[7];
				for (int i = 0; i < acupoints.Length; i++)
					acupoints[i] = AddImage("MeridianPoint", glint);
			}

			var node = new GameObject("PracticeCaption", typeof(RectTransform), typeof(Text));
			node.layer = gameObject.layer;
			node.transform.SetParent(transform, false);
			caption = node.GetComponent<Text>();
			caption.raycastTarget = false;
			caption.font = card.GetComponentInChildren<Text>(true)?.font;
			caption.fontSize = 20;
			caption.alignment = TextAnchor.MiddleCenter;
			caption.rectTransform.sizeDelta = new Vector2(190f, 34f);
			caption.rectTransform.anchoredPosition = new Vector2(0f, -79f);
			caption.text = label ?? (action switch
			{
				ShiTuToZiList.ChuShi => "结业授印",
				ShiTuToZiList.ToMai => "灵络贯通",
				_ => "灵气流转"
			});
			began = Time.unscaledTime;
		}

		private Image AddImage(string label, Sprite sprite)
		{
			var node = new GameObject(label, typeof(RectTransform), typeof(Image));
			node.layer = gameObject.layer;
			node.transform.SetParent(transform, false);
			Image image = node.GetComponent<Image>();
			image.sprite = sprite;
			image.raycastTarget = false;
			image.color = Color.clear;
			return image;
		}

		private Image AddSeatedFigure(string label)
		{
			Image figure = AddImage(label, meditator);
			figure.rectTransform.pivot = new Vector2(0.5f, 0.12f);
			return figure;
		}

		private void Update()
		{
			float t = (Time.unscaledTime - began) / Duration;
			if (t >= 1f)
			{
				Destroy(gameObject);
				completed?.Invoke();
				return;
			}

			opacity.alpha = Ease(t, 0f, 0.14f) * (1f - Ease(t, 0.87f, 1f));
			caption.color = Tint(InkInterfaceArt.Ink, Ease(t, 0.26f, 0.52f));
			if (action == ShiTuToZiList.ChuShi)
				DrawGraduation(t);
			else
				DrawPractice(t, action == ShiTuToZiList.ToMai);
		}

		private void DrawPractice(float t, bool meridians)
		{
			Color color = meridians ? Jade : Gold;
			float travel = Ease(t, 0.10f, 0.68f);
			float pulse = Mathf.Sin(Mathf.PI * travel);
			float impact = Ease(t, 0.60f, 0.67f) * (1f - Ease(t, 0.73f, 1f));
			Vector2 focus = FlowPoint(travel, meridians);
			Draw(ink, focus, new Vector2(124f + 88f * pulse, 62f + 42f * pulse), -14f, color, 0.48f + 0.28f * impact);
			if (meridians)
				DrawMeridianFigure(t);
			else
				DrawPowerFigures(t, pulse, impact);

			for (int i = 0; i < strokes.Length; i++)
			{
				float sweep = Ease(t, 0.05f + i * 0.07f, 0.64f + i * 0.055f);
				Vector2 center = FlowPoint(sweep, meridians) + new Vector2(0f, (i - 2) * (meridians ? 10f : 6f));
				float thickness = (meridians ? 51f : 36f) - i * 5f;
				float angle = Mathf.Lerp(23f, -13f, sweep) + (i - 2) * (meridians ? 12f : -6f);
				float strength = Mathf.Sin(Mathf.PI * sweep);
				Draw(strokes[i], center, new Vector2(69f + 64f * strength, thickness), angle,
					Color.Lerp(color, meridians ? Gold : Jade, i * 0.13f), strength * 0.88f);
			}

			for (int i = 0; i < motes.Length; i++)
			{
				float flight = Ease(t, 0.12f + i * 0.035f, 0.66f + i * 0.024f);
				float spark = Mathf.Sin(Mathf.PI * flight);
				float y = Mathf.Sin(i * 2.4f + flight * 7f) * (10f + 15f * spark);
				Vector2 center = FlowPoint(flight, meridians) + new Vector2(i % 2 == 0 ? -11f : 11f, y);
				Draw(motes[i], center, Vector2.one * (8f + (i % 4) * 3f + 11f * impact), i * 32f + flight * 110f,
					i % 3 == 0 ? Jade : Gold, 0.92f * spark);
			}

			float size = (meridians ? 26f : 41f) + 18f * pulse + 13f * impact;
			Draw(emblem, focus, Vector2.one * size, (meridians ? 12f : 83f) * travel - 13f,
				Color.white, 0.42f + 0.58f * Ease(t, 0.30f, 0.64f));
		}

		private void DrawPowerFigures(float t, float pulse, float impact)
		{
			float reveal = Ease(t, 0.04f, 0.25f);
			float inhale = Mathf.Sin(Mathf.PI * Ease(t, 0.04f, 0.48f));
			float receive = Mathf.Sin(Mathf.PI * Ease(t, 0.38f, 0.95f));
			Draw(teacher, new Vector2(-78f + 10f * pulse, -44f), new Vector2(126f + 5f * inhale, 116f + 13f * inhale),
				-8f * pulse, TeacherInk, 0.95f * reveal);
			Draw(disciple, new Vector2(79f - 6f * receive, -46f + 4f * impact), new Vector2(108f + 9f * receive, 100f + 17f * receive),
				7f * receive - 4f * impact, DiscipleInk, 0.94f * reveal);
		}

		private void DrawMeridianFigure(float t)
		{
			float breath = Mathf.Sin(Mathf.PI * Ease(t, 0.10f, 0.91f));
			float reveal = Ease(t, 0.04f, 0.24f);
			float sway = 3f * Mathf.Sin(t * Mathf.PI * 2f);
			float lift = 7f * breath;
			Draw(disciple, new Vector2(sway, -49f), new Vector2(153f + 6f * breath, 141f + 12f * breath),
				-sway, DiscipleInk, 0.98f * reveal);

			for (int i = 0; i < acupoints.Length; i++)
			{
				bool shoulder = i >= 5;
				float start = 0.18f + Mathf.Min(i, 5) * 0.09f;
				float light = Ease(t, start, start + 0.10f);
				float settle = Ease(t, start + 0.09f, start + 0.22f);
				Vector2 center = shoulder
					? new Vector2(sway + (i == 5 ? -31f : 31f), 35f + lift)
					: new Vector2(sway + Mathf.Sin(i * 0.9f) * 3f, -22f + 21f * i + lift);
				Color pigment = Color.Lerp(Jade, Gold, 0.65f * settle);
				Draw(acupoints[i], center, Vector2.one * (13f + (shoulder ? 20f : 14f) * light * (1f - settle)),
					shoulder ? 21f : 0f, pigment, light * (1f - 0.44f * settle));
			}
		}

		private void DrawGraduation(float t)
		{
			float drop = Ease(t, 0.15f, 0.52f);
			float release = Ease(t, 0.53f, 0.93f);
			float mark = Ease(t, 0.47f, 0.56f) * (1f - Ease(t, 0.78f, 1f));
			Draw(ink, new Vector2(24f, 4f), new Vector2(116f + release * 84f, 77f + release * 35f), 8f, Vermilion, 0.72f * mark);
			Draw(emblem, new Vector2(-27f, 16f), Vector2.one * (108f + 8f * mark), -19f + 8f * drop, Color.white, 1f);
			Draw(seal, new Vector2(35f, Mathf.Lerp(67f, 11f, drop) + 8f * release), Vector2.one * (84f - 11f * release),
				Mathf.Lerp(17f, -8f, drop), Color.white, Ease(t, 0.08f, 0.20f));

			for (int i = 0; i < strokes.Length; i++)
			{
				float angle = 22f + i * 73f;
				Vector2 center = new Vector2(21f, 0f) + Direction(angle) * Mathf.Lerp(18f, 82f, release);
				Draw(strokes[i], center, new Vector2(75f - 23f * release, 18f), angle, i % 2 == 0 ? Vermilion : Gold, mark * 0.9f);
			}

			for (int i = 0; i < motes.Length; i++)
			{
				float angle = i * 36f + 8f;
				float distance = Mathf.Lerp(17f, 79f + i % 3 * 12f, release);
				Vector2 center = new Vector2(21f, 1f) + Direction(angle) * distance;
				Draw(motes[i], center, Vector2.one * (14f + i % 4 * 4f), angle + 40f * release, Gold, mark);
			}
		}

		private static Vector2 FlowPoint(float travel, bool meridians)
		{
			float wave = meridians ? Mathf.Sin(travel * 2f * Mathf.PI) * 22f : Mathf.Sin(travel * Mathf.PI) * 46f;
			return new Vector2(Mathf.Lerp(-92f, 92f, travel), 3f + wave);
		}

		private static Vector2 Direction(float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
		}

		private static float Ease(float t, float start, float end)
		{
			return Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(start, end, t));
		}

		private static Color Tint(Color color, float alpha)
		{
			color.a = Mathf.Clamp01(alpha);
			return color;
		}

		private static void Draw(Image image, Vector2 center, Vector2 size, float angle, Color color, float alpha)
		{
			RectTransform rect = image.rectTransform;
			rect.anchoredPosition = center;
			rect.sizeDelta = size;
			rect.localRotation = Quaternion.Euler(0f, 0f, angle);
			image.color = Tint(color, alpha);
		}
	}
}
