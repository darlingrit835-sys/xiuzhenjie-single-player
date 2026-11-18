using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkElementMotion : MonoBehaviour
	{
		private const float FullTurn = Mathf.PI * 2f;
		private static Sprite ring;
		private Image icon;
		private Image aura;
		private WuXing element;
		private Color pigment;
		private float began;

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
				Bind(art, halo, element);
			}
		}

		internal static void StyleSummary(Image art, WuXing element)
		{
			InkInterfaceArt.Set(art, "Element" + element);
			if (art.sprite != null && art.sprite.name == "Element" + element && art.GetComponent<InkElementMotion>() == null)
			{
				Bind(art, null, element);
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

		private static void Bind(Image art, Image halo, WuXing element)
		{
			var motion = art.gameObject.AddComponent<InkElementMotion>();
			motion.icon = art;
			motion.aura = halo;
			motion.element = element;
			motion.pigment = element switch
			{
				WuXing.Jin => new Color32(207, 153, 58, 255),
				WuXing.Mu => new Color32(80, 133, 80, 255),
				WuXing.Shui => new Color32(52, 120, 144, 255),
				WuXing.Huo => new Color32(189, 76, 43, 255),
				_ => new Color32(143, 108, 66, 255)
			};
		}

		private void OnEnable()
		{
			began = Time.unscaledTime;
		}

		private void OnDisable()
		{
			if (icon != null)
			{
				icon.rectTransform.localScale = Vector3.one;
				icon.rectTransform.localRotation = Quaternion.identity;
				icon.color = Color.white;
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
				float glint = Mathf.Pow(Mathf.Max(0f, Mathf.Cos(time * 3.7f)), 12f);
				scaleX = scaleY = 1f + 0.06f * glint;
				auraAlpha += 0.52f * glint;
				break;
			case WuXing.Mu:
				float leaf = Mathf.Sin(time * 2f);
				scaleY += 0.07f * leaf;
				angle = 5f * leaf;
				auraAlpha += 0.13f * leaf;
				break;
			case WuXing.Shui:
				float ripple = Mathf.Repeat(time / 1.65f, 1f);
				angle = 4f * Mathf.Sin(ripple * FullTurn);
				auraScale = Mathf.Lerp(0.70f, 1.22f, ripple);
				auraAlpha = 0.60f * (1f - ripple);
				break;
			case WuXing.Huo:
				float flame = Mathf.Sin(time * 8f) * 0.6f + Mathf.Sin(time * 13f) * 0.4f;
				scaleY += flame * 0.10f;
				scaleX -= flame * 0.04f;
				opacity = 0.94f + flame * 0.06f;
				auraAlpha += flame * 0.18f;
				break;
			case WuXing.Tu:
				float pulse = 0.5f + 0.5f * Mathf.Sin(time * 1.8f);
				scaleX += pulse * 0.08f;
				scaleY -= pulse * 0.035f;
				auraAlpha += pulse * 0.16f;
				break;
			}
			icon.rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);
			icon.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
			icon.color = new Color(1f, 1f, 1f, opacity);
			if (aura != null)
			{
				aura.rectTransform.localScale = Vector3.one * auraScale;
				aura.color = new Color(pigment.r, pigment.g, pigment.b, auraAlpha);
			}
		}
	}
}
