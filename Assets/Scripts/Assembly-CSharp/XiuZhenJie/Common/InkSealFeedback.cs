using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkSealFeedback : MonoBehaviour
	{
		private const float Impact = 0.09f;
		private const float Hold = 0.24f;
		private const float Duration = 0.52f;
		private static readonly Color32 Cinnabar = new Color32(181, 64, 49, 255);
		private static Sprite impressionSprite;
		private static InkSealFeedback active;

		private CanvasGroup impression;
		private Image stamp;
		private float size;
		private float elapsed;

		internal static InkSealFeedback Play(Button button, string words)
		{
			RectTransform front = Singleton<UIManager>.Instance.m_UiRoot?.Find("WndRootFront") as RectTransform;
			if (front == null || button == null || !front.gameObject.activeInHierarchy)
				return null;

			if (active != null)
				active.Finish();

			var node = new GameObject("InkSealFeedback", typeof(RectTransform));
			node.layer = front.gameObject.layer;
			var frame = (RectTransform)node.transform;
			var source = (RectTransform)button.transform;
			Vector2 position = front.InverseTransformPoint(source.TransformPoint(source.rect.center));
			float size = Mathf.Clamp(source.rect.height * 1.5f, 52f, 66f);
			Place(frame, front, size);
			frame.anchoredPosition = position + Vector2.up * 5f;
			frame.SetAsLastSibling();

			active = node.AddComponent<InkSealFeedback>();
			active.Build(frame, button.GetComponentInChildren<Text>(true)?.font, words, size);
			return active;
		}

		private void Build(RectTransform frame, Font font, string words, float size)
		{
			this.size = size;
			Image print = AddImage(frame, "CinnabarImpression", GetImpressionSprite(), size, Cinnabar);
			impression = print.gameObject.AddComponent<CanvasGroup>();
			impression.blocksRaycasts = false;
			impression.interactable = false;

			if (font != null)
			{
				var textNode = new GameObject("SealLabel", typeof(RectTransform), typeof(Text));
				textNode.layer = frame.gameObject.layer;
				Text label = textNode.GetComponent<Text>();
				Place(label.rectTransform, print.rectTransform, size - 8f);
				label.font = font;
				label.fontSize = Mathf.RoundToInt(size * 0.35f);
				label.alignment = TextAnchor.MiddleCenter;
				label.color = InkDialogArt.Ivory;
				label.text = words;
				label.raycastTarget = false;
			}

			stamp = AddImage(frame, "StampTool", InkInterfaceArt.Get("AttendanceSeal"), size * 1.18f, Color.white);
			stamp.preserveAspect = true;
			ApplyPose();
		}

		private static Image AddImage(RectTransform parent, string name, Sprite sprite, float size, Color color)
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(Image));
			node.layer = parent.gameObject.layer;
			Image image = node.GetComponent<Image>();
			Place(image.rectTransform, parent, size);
			image.sprite = sprite;
			image.color = color;
			image.raycastTarget = false;
			image.enabled = sprite != null;
			return image;
		}

		private static void Place(RectTransform rect, RectTransform parent, float size)
		{
			rect.SetParent(parent, false);
			rect.anchorMin = rect.anchorMax = parent.pivot;
			rect.sizeDelta = Vector2.one * size;
			rect.anchoredPosition = Vector2.zero;
		}

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += SceneChanged;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= SceneChanged;
			if (active == this)
				active = null;
			if (gameObject.activeSelf)
				Destroy(gameObject);
		}

		private void SceneChanged(Scene previous, Scene current)
		{
			Finish();
		}

		private void Update()
		{
			// A slow frame can delay the request kickoff without skipping the stamp's impact pose.
			elapsed += Mathf.Min(Time.unscaledDeltaTime, 0.085f);
			if (elapsed >= Duration)
				Finish();
			else
				ApplyPose();
		}

		private void ApplyPose()
		{
			float drop = Mathf.Clamp01(elapsed / Impact);
			float rebound = Mathf.Sin(Mathf.PI * Mathf.InverseLerp(Impact, Hold, elapsed));
			float fade = 1f - Mathf.InverseLerp(Hold, Duration, elapsed);
			impression.alpha = Mathf.Lerp(0.45f, 1f, drop) * fade;
			impression.transform.localScale = Vector3.one * (Mathf.Lerp(0.86f, 1f, drop) + 0.13f * rebound);
			impression.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-11f, -6f, drop));
			stamp.rectTransform.anchoredPosition = new Vector2(size * 0.25f, size * Mathf.Lerp(0.64f, 0.11f, drop * drop));
			stamp.color = new Color(1f, 1f, 1f, (1f - Mathf.InverseLerp(0.035f, 0.19f, elapsed)) * 0.9f);
		}

		internal void Finish()
		{
			gameObject.SetActive(false);
			Destroy(gameObject);
		}

		private static Sprite GetImpressionSprite()
		{
			if (impressionSprite != null)
				return impressionSprite;

			const int resolution = 64;
			var texture = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false)
			{
				name = "Cinnabar seal paper impression",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			var pixels = new Color32[resolution * resolution];
			for (int y = 0; y < resolution; y++)
			{
				for (int x = 0; x < resolution; x++)
				{
					int edge = Mathf.Min(Mathf.Min(x, resolution - 1 - x), Mathf.Min(y, resolution - 1 - y));
					int grain = ((x * 13 + y * 37) ^ (x * 71 + y * 11)) & 31;
					int opacity = edge == 0 || edge == 5 ? 0 : 238 + grain / 2;
					if (edge < 3 || grain == 0)
						opacity = opacity * (grain + 5) / 36;
					pixels[y * resolution + x] = new Color32(255, 255, 255, (byte)opacity);
				}
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, true);
			impressionSprite = Sprite.Create(texture, new Rect(0f, 0f, resolution, resolution), Vector2.one * 0.5f);
			impressionSprite.name = "Cinnabar impression";
			return impressionSprite;
		}
	}
}
