using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkElementMotion : MonoBehaviour
	{
		private const float FullTurn = Mathf.PI * 2f;
		private static Sprite ring;
		private static readonly Sprite[] stamps = new Sprite[7];
		private static readonly Color32 Gold = new Color32(255, 244, 188, 255);
		private static readonly Color32 Leaf = new Color32(110, 145, 68, 255);
		private static readonly Color32 Foam = new Color32(191, 234, 224, 255);
		private static readonly Color32 Flame = new Color32(255, 204, 117, 255);
		private static readonly Color32 Ember = new Color32(216, 88, 40, 255);
		private static readonly Color32 Stone = new Color32(129, 96, 52, 255);
		private static readonly Color32 Dust = new Color32(178, 139, 86, 255);

		private enum Stamp { Glint, Star, Leaf, Stream, Flame, Stone, Dust }

		private Image icon;
		private Image aura;
		private RectTransform effectsRoot;
		private Image[] effects;
		private WuXing element;
		private Color pigment;
		private float began;
		private float baseOpacity;
		private float effectOpacity;
		private Vector3 baseScale;
		private Quaternion baseRotation;

		internal static void StyleChoice(Button button, WuXing element, string label, Font font, bool selected)
		{
			Sprite sprite = InkInterfaceArt.Get("Element" + element);
			if (sprite == null || button.transform.Find("InkElementIcon") != null)
			{
				return;
			}

			Image target = button.image;
			target.color = new Color(1f, 1f, 1f, 0f);
			button.transition = Selectable.Transition.None;
			RectTransform parent = (RectTransform)button.transform;
			Image halo = null;
			if (selected)
			{
				halo = Child<Image>(parent, "InkElementAura", new Vector2(0.12f, 0.25f), new Vector2(0.88f, 1f));
				halo.sprite = Ring();
				halo.preserveAspect = true;
			}
			Image art = Child<Image>(parent, "InkElementIcon", new Vector2(0.13f, 0.31f), new Vector2(0.87f, 0.99f));
			art.sprite = sprite;
			art.preserveAspect = true;
			art.color = selected ? Color.white : new Color32(255, 255, 255, 190);
			Text text = Child<Text>(parent, "InkElementName", Vector2.zero, new Vector2(1f, 0.32f));
			text.font = font;
			text.text = label;
			text.fontSize = 13;
			text.alignment = TextAnchor.MiddleCenter;
			text.color = selected ? InkInterfaceArt.Seal : InkInterfaceArt.Ink;
			if (selected)
			{
				text.fontStyle = FontStyle.Bold;
			}
			Bind(art, halo, element, selected ? 1f : 0.78f);
		}

		internal static void StyleSummary(Image art, WuXing element)
		{
			InkInterfaceArt.Set(art, "Element" + element);
			if (art.sprite != null && art.sprite.name == "Element" + element && art.GetComponent<InkElementMotion>() == null)
			{
				Bind(art, null, element, 0.9f);
			}
		}

		private static T Child<T>(RectTransform parent, string name, Vector2 min, Vector2 max) where T : Graphic
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));
			node.layer = parent.gameObject.layer;
			var rect = (RectTransform)node.transform;
			rect.SetParent(parent, false);
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = Vector2.zero;
			rect.offsetMax = Vector2.zero;
			var graphic = node.AddComponent<T>();
			graphic.raycastTarget = false;
			return graphic;
		}

		private static Sprite Ring()
		{
			if (ring != null) return ring;

			const int size = 80;
			var texture = new Texture2D(size, size, TextureFormat.RGBA32, false) { name = "Ink element aura" };
			var pixels = new Color32[size * size];
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					var delta = new Vector2((x + 0.5f) / size, (y + 0.5f) / size) - Vector2.one * 0.5f;
					float alpha = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.006f, 0.025f, Mathf.Abs(delta.magnitude - 0.42f)));
					pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
				}
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, true);
			ring = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.one * 0.5f, 100f, 0, SpriteMeshType.FullRect);
			return ring;
		}

		private static void Bind(Image art, Image halo, WuXing element, float effectOpacity)
		{
			var motion = art.gameObject.AddComponent<InkElementMotion>();
			motion.icon = art;
			motion.aura = halo;
			motion.element = element;
			motion.baseOpacity = art.color.a;
			motion.effectOpacity = effectOpacity;
			motion.baseScale = art.rectTransform.localScale;
			motion.baseRotation = art.rectTransform.localRotation;
			motion.pigment = element switch
			{
				WuXing.Jin => new Color32(207, 153, 58, 255),
				WuXing.Mu => new Color32(80, 133, 80, 255),
				WuXing.Shui => new Color32(52, 120, 144, 255),
				WuXing.Huo => new Color32(189, 76, 43, 255),
				_ => new Color32(143, 108, 66, 255)
			};
			motion.CreateEffects();
		}

		private void CreateEffects()
		{
			RectTransform parent = icon.rectTransform;
			var layer = new GameObject("InkElementEffects", typeof(RectTransform));
			layer.layer = parent.gameObject.layer;
			effectsRoot = (RectTransform)layer.transform;
			effectsRoot.SetParent(parent, false);
			effectsRoot.anchorMin = Vector2.one * 0.5f;
			effectsRoot.anchorMax = Vector2.one * 0.5f;
			effectsRoot.anchoredPosition = Vector2.zero;
			effectsRoot.SetAsLastSibling();
			FitEffectLayer();

			Stamp[] shapes = element switch
			{
				WuXing.Jin => new[] { Stamp.Glint, Stamp.Star },
				WuXing.Mu => new[] { Stamp.Leaf, Stamp.Leaf, Stamp.Leaf },
				WuXing.Shui => new[] { Stamp.Stream, Stamp.Stream, Stamp.Stream },
				WuXing.Huo => new[] { Stamp.Flame, Stamp.Flame, Stamp.Flame },
				_ => new[] { Stamp.Stone, Stamp.Stone, Stamp.Stone, Stamp.Dust, Stamp.Dust }
			};
			effects = new Image[shapes.Length];
			for (int i = 0; i < shapes.Length; i++)
			{
				Image mark = Child<Image>(effectsRoot, shapes[i].ToString(), Vector2.one * 0.5f, Vector2.one * 0.5f);
				mark.sprite = GetStamp(shapes[i]);
				mark.color = Color.clear;
				effects[i] = mark;
			}
		}

		private void OnRectTransformDimensionsChange()
		{
			if (effectsRoot != null)
			{
				FitEffectLayer();
			}
		}

		private void FitEffectLayer()
		{
			Rect rect = icon.rectTransform.rect;
			effectsRoot.sizeDelta = Vector2.one * Mathf.Min(rect.width, rect.height);
		}

		private void OnEnable()
		{
			began = Time.unscaledTime;
		}

		private void OnDisable()
		{
			if (icon != null)
			{
				icon.rectTransform.localScale = baseScale;
				icon.rectTransform.localRotation = baseRotation;
				icon.color = new Color(1f, 1f, 1f, baseOpacity);
			}
			if (effects != null)
			{
				foreach (Image mark in effects)
				{
					mark.color = Color.clear;
				}
			}
		}

		private void Update()
		{
			if (icon == null) return;

			float time = Time.unscaledTime - began;
			float scaleX = 1f;
			float scaleY = 1f;
			float angle = 0f;
			float opacity = 1f;
			float auraScale = 1f;
			float auraAlpha = 0.27f;
			switch (element)
			{
			case WuXing.Jin:
				float sweep = Mathf.Repeat(time + 0.28f, 1.95f) / 0.72f;
				float glint = Envelope(sweep, 0.14f, 0.76f);
				scaleX = scaleY = 1f + 0.06f * glint;
				auraAlpha += 0.52f * glint;
				Draw(0, Vector2.Lerp(new Vector2(-0.27f, -0.12f), new Vector2(0.31f, 0.23f), sweep),
					new Vector2(0.18f, 0.58f), -43f, Gold, glint);
				float spark = Envelope((sweep - 0.50f) / 0.65f, 0.27f, 0.61f);
				Draw(1, new Vector2(0.22f, 0.24f), Vector2.one * (0.12f + 0.18f * spark), -10f, Gold, spark);
				break;
			case WuXing.Mu:
				float leaf = Mathf.Sin(time * 2f);
				scaleY += 0.07f * leaf;
				angle = 5f * leaf;
				auraAlpha += 0.13f * leaf;
				DriftLeaves(time);
				break;
			case WuXing.Shui:
				float ripple = Mathf.Repeat(time / 1.65f, 1f);
				angle = 4f * Mathf.Sin(ripple * FullTurn);
				auraScale = Mathf.Lerp(0.70f, 1.22f, ripple);
				auraAlpha = 0.60f * (1f - ripple);
				FlowWater(time);
				break;
			case WuXing.Huo:
				float flame = Mathf.Sin(time * 8f) * 0.6f + Mathf.Sin(time * 13f) * 0.4f;
				scaleY += flame * 0.10f;
				scaleX -= flame * 0.04f;
				opacity = 0.94f + flame * 0.06f;
				auraAlpha += flame * 0.18f;
				BurnFlames(time);
				break;
			case WuXing.Tu:
				float pulse = 0.5f + 0.5f * Mathf.Sin(time * 1.8f);
				scaleX += pulse * 0.08f;
				scaleY -= pulse * 0.035f;
				auraAlpha += pulse * 0.16f;
				DropStones(time);
				break;
			}
			icon.rectTransform.localScale = Vector3.Scale(baseScale, new Vector3(scaleX, scaleY, 1f));
			icon.rectTransform.localRotation = baseRotation * Quaternion.Euler(0f, 0f, angle);
			icon.color = new Color(1f, 1f, 1f, opacity * baseOpacity);
			if (aura != null)
			{
				aura.rectTransform.localScale = Vector3.one * auraScale;
				aura.color = new Color(pigment.r, pigment.g, pigment.b, auraAlpha);
			}
		}

		private void DriftLeaves(float time)
		{
			for (int index = 0; index < 3; index++)
			{
				float flight = Mathf.Repeat((time + index * 0.58f) / 1.95f, 1f);
				float flutter = Mathf.Sin(flight * FullTurn * 1.4f + index * 1.9f);
				var point = new Vector2((index - 1) * 0.18f + 0.22f * flight + 0.11f * flutter, 0.37f - 0.74f * flight);
				Draw(index, point, new Vector2(0.20f, 0.11f + 0.045f * flutter), 32f + 180f * flight + 34f * flutter,
					Leaf, Envelope(flight, 0.13f, 0.73f) * 0.91f);
			}
		}

		private void FlowWater(float time)
		{
			for (int index = 0; index < 3; index++)
			{
				float flow = Mathf.Repeat((time + index * 0.34f) / 1.28f, 1f);
				float curve = Mathf.Sin(flow * Mathf.PI);
				var point = new Vector2(-0.28f + 0.55f * flow, -0.11f + index * 0.09f + 0.17f * curve);
				Draw(index, point, new Vector2(0.24f + 0.15f * curve, 0.09f), Mathf.Lerp(24f, -26f, flow),
					Foam, Envelope(flow, 0.19f, 0.73f) * 0.95f);
			}
		}

		private void BurnFlames(float time)
		{
			for (int index = 0; index < 3; index++)
			{
				float rise = Mathf.Repeat((time + index * 0.24f) / 0.82f, 1f);
				float flicker = Mathf.Sin(time * 16f + index * 2.1f);
				var point = new Vector2((index - 1) * 0.13f + 0.05f * flicker, -0.19f + 0.54f * rise);
				Draw(index, point, new Vector2(0.19f - rise * 0.085f, 0.29f + flicker * 0.05f - rise * 0.11f),
					(index - 1) * 11f + flicker * 8f, index == 1 ? Flame : Ember, Envelope(rise, 0.09f, 0.77f));
			}
		}

		private void DropStones(float time)
		{
			for (int index = 0; index < 3; index++)
			{
				float fall = Mathf.Repeat((time + 0.30f + index * 0.39f) / 1.65f, 1f) / 0.76f;
				float descent = Mathf.Clamp01(fall);
				var point = new Vector2((index - 1) * 0.26f + (index - 1) * 0.12f * descent, 0.38f - 0.70f * descent * descent);
				float size = 0.13f + index * 0.025f;
				Draw(index, point, new Vector2(size, size * 0.82f), (index - 1) * 43f + 155f * descent,
					Stone, Envelope(fall, 0.13f, 0.84f));
			}
			for (int index = 0; index < 2; index++)
			{
				float puff = Mathf.Repeat((time + 0.30f + index * 0.39f) / 1.65f - 0.66f, 1f) / 0.32f;
				float spread = Mathf.Clamp01(puff);
				var at = new Vector2((index - 1) * 0.36f + (index - 0.5f) * spread * 0.17f, -0.29f + spread * 0.13f);
				Draw(index + 3, at, new Vector2(0.17f, 0.10f) * (1f + 0.85f * spread), 0f,
					Dust, Envelope(puff, 0.16f, 0.62f) * 0.53f);
			}
		}

		private void Draw(int index, Vector2 position, Vector2 size, float angle, Color color, float alpha)
		{
			Image mark = effects[index];
			color.a = alpha * effectOpacity;
			mark.color = color;
			if (color.a <= 0.001f)
			{
				return;
			}
			RectTransform rect = mark.rectTransform;
			float side = effectsRoot.rect.width;
			rect.anchoredPosition = position * side;
			rect.sizeDelta = size * side;
			rect.localRotation = Quaternion.Euler(0f, 0f, angle);
		}

		private static float Envelope(float progress, float fadeIn, float fadeOut)
		{
			return Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0f, fadeIn, progress))
				* (1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(fadeOut, 1f, progress)));
		}

		private static Sprite GetStamp(Stamp shape)
		{
			int index = (int)shape;
			if (stamps[index] != null)
			{
				return stamps[index];
			}

			const int side = 40;
			var texture = new Texture2D(side, side, TextureFormat.RGBA32, false)
			{
				name = "Ink element " + shape,
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			var pixels = new Color32[side * side];
			for (int y = 0; y < side; y++)
			{
				for (int x = 0; x < side; x++)
				{
					float px = 2f * (x + 0.5f) / side - 1f;
					float py = 2f * (y + 0.5f) / side - 1f;
					pixels[y * side + x] = RasterStamp(shape, px, py);
				}
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, true);
			stamps[index] = Sprite.Create(texture, new Rect(0f, 0f, side, side), Vector2.one * 0.5f,
				100f, 0, SpriteMeshType.FullRect);
			stamps[index].name = texture.name;
			return stamps[index];
		}

		private static Color32 RasterStamp(Stamp shape, float x, float y)
		{
			float alpha;
			float tone = 1f;
			switch (shape)
			{
			case Stamp.Glint:
				float bar = Mathf.Clamp01(1f - Mathf.Abs(x));
				alpha = bar * (0.28f + 0.72f * bar * bar) * Mathf.Sqrt(Mathf.Clamp01(1f - y * y));
				break;
			case Stamp.Star:
				float arms = Mathf.Sqrt(Mathf.Abs(x)) + Mathf.Sqrt(Mathf.Abs(y));
				alpha = Edge(arms - 0.96f, 0.12f);
				break;
			case Stamp.Leaf:
				float leafProfile = Mathf.Cos(x * Mathf.PI * 0.5f);
				float vein = y - 0.14f * leafProfile;
				alpha = Edge(Mathf.Abs(vein) - 0.75f * leafProfile, 0.09f);
				tone = 0.77f + 0.23f * Edge(Mathf.Abs(vein) - 0.08f, 0.05f);
				break;
			case Stamp.Stream:
				float streamProfile = Mathf.Cos(x * Mathf.PI * 0.5f);
				alpha = Edge(Mathf.Abs(y - 0.36f * Mathf.Sin(x * Mathf.PI)) - 0.35f * streamProfile, 0.17f) * streamProfile;
				break;
			case Stamp.Flame:
				float height = (y + 1f) * 0.5f;
				float width = Mathf.Sin(height * Mathf.PI) * (0.95f - 0.57f * height);
				float bend = 0.32f * Mathf.Sin(height * FullTurn) + height * 0.22f;
				alpha = Edge(Mathf.Abs(x - bend) - width, 0.17f);
				tone = Mathf.Lerp(0.87f, 1f, 1f - height);
				break;
			case Stamp.Stone:
				float facet = Mathf.Max(Mathf.Abs(x + 0.15f * y) + 0.31f * y,
					Mathf.Max(y - 0.36f * x, -y + 0.27f * x));
				alpha = Edge(facet - 0.73f, 0.09f);
				tone = y > -x * 0.29f ? 0.99f : x > 0f ? 0.77f : 0.60f;
				break;
			default:
				float cloud = Mathf.Clamp01(1f - (x * x + y * y));
				alpha = cloud * cloud;
				break;
			}
			byte shade = (byte)(255f * Mathf.Clamp01(tone));
			return new Color32(shade, shade, shade, (byte)(255f * Mathf.Clamp01(alpha)));
		}

		private static float Edge(float distance, float feather)
		{
			return 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(-feather, feather, distance));
		}
	}
}
