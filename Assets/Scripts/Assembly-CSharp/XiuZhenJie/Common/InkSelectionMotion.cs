using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	[DisallowMultipleComponent]
	internal sealed class InkSelectionMotion : MonoBehaviour
	{
		private const float SelectedScale = 1.045f;
		private const float SelectDuration = 0.33f;
		private const float DeselectDuration = 0.21f;
		private static readonly Color32 Amber = new Color32(240, 175, 78, 255);
		private static Sprite haloSprite;

		private struct Pose
		{
			internal float Scale;
			internal float Lift;
			internal float Light;
		}

		private RectTransform content;
		private Image halo;
		private Vector3 restingScale;
		private Vector2 restingPosition;
		private Pose current;
		private Pose from;
		private float began;
		private bool selected;
		private bool transitioning;

		// Motion belongs to a child so the Selectable and its hit box stay at rest.
		internal static InkSelectionMotion Attach(Selectable selectable, RectTransform content, float contentFramePadding = 0f)
		{
			if (selectable == null || content == null || content == selectable.transform ||
				!content.IsChildOf(selectable.transform))
			{
				return null;
			}
			if (!selectable.TryGetComponent(out InkSelectionMotion motion))
			{
				motion = selectable.gameObject.AddComponent<InkSelectionMotion>();
			}
			motion.Bind(content, contentFramePadding);
			return motion;
		}

		internal void SetSelected(bool selected, bool animate)
		{
			if (this.selected == selected && animate)
			{
				return;
			}
			this.selected = selected;
			began = Time.unscaledTime;
			transitioning = animate && isActiveAndEnabled;
			from = current;
			if (!transitioning && isActiveAndEnabled)
			{
				Apply(SteadyPose(0f));
			}
		}

		private void Bind(RectTransform visual, float contentFramePadding)
		{
			if (visual == content)
			{
				return;
			}
			Restore();
			content = visual;
			restingScale = visual.localScale;
			restingPosition = visual.anchoredPosition;
			if (halo == null)
			{
				var node = new GameObject("InkSelectionHalo", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
				node.layer = gameObject.layer;
				node.GetComponent<LayoutElement>().ignoreLayout = true;

				halo = node.GetComponent<Image>();
				halo.sprite = GetHaloSprite();
				halo.type = Image.Type.Sliced;
				halo.color = Amber;
				halo.raycastTarget = false;
			}
			RectTransform frame = halo.rectTransform;
			bool followsContent = contentFramePadding > 0f;
			frame.SetParent(followsContent ? visual : transform, false);
			frame.SetAsLastSibling();
			frame.anchorMin = Vector2.zero;
			frame.anchorMax = Vector2.one;
			Vector2 inset = Vector2.one * (followsContent ? -contentFramePadding : 1f);
			frame.offsetMin = inset;
			frame.offsetMax = -inset;
			if (isActiveAndEnabled)
			{
				Apply(SteadyPose(0f));
			}
			else
			{
				Restore();
			}
		}

		private void OnEnable()
		{
			began = Time.unscaledTime;
			Apply(SteadyPose(0f));
		}

		private void OnDisable()
		{
			transitioning = false;
			Restore();
		}

		private void Update()
		{
			if (content == null || (!selected && !transitioning))
			{
				return;
			}
			float elapsed = Time.unscaledTime - began;
			Pose target = SteadyPose(elapsed);
			if (!transitioning)
			{
				Apply(target);
				return;
			}
			float t = Mathf.Clamp01(elapsed / (selected ? SelectDuration : DeselectDuration));
			float rest = 1f - t;
			float ease = 1f - rest * rest * rest;
			float kick = selected ? Mathf.Sin(t * Mathf.PI) : 0f;
			Apply(new Pose
			{
				Scale = Mathf.LerpUnclamped(from.Scale, target.Scale, ease) + 0.065f * kick * kick,
				Lift = Mathf.LerpUnclamped(from.Lift, target.Lift, ease),
				Light = Mathf.LerpUnclamped(from.Light, target.Light, ease) + 0.34f * kick
			});
			transitioning = t < 1f;
		}

		private Pose SteadyPose(float time)
		{
			if (!selected)
			{
				return new Pose { Scale = 1f };
			}
			float drift = 0.5f - 0.5f * Mathf.Cos(time * 2.8f);
			return new Pose
			{
				Scale = SelectedScale + 0.012f * drift,
				Lift = 2.2f * drift,
				Light = 0.67f + 0.18f * drift
			};
		}

		private void Restore()
		{
			Apply(new Pose { Scale = 1f });
		}

		private void Apply(Pose pose)
		{
			current = pose;
			if (content != null)
			{
				content.localScale = new Vector3(restingScale.x * pose.Scale, restingScale.y * pose.Scale, restingScale.z);
				content.anchoredPosition = restingPosition + new Vector2(0f, pose.Lift);
			}
			if (halo != null)
			{
				bool visible = pose.Light > 0.005f;
				if (halo.enabled != visible)
				{
					halo.enabled = visible;
				}
				if (visible)
				{
					halo.canvasRenderer.SetAlpha(Mathf.Clamp01(pose.Light));
				}
			}
		}

		private static Sprite GetHaloSprite()
		{
			if (haloSprite != null)
			{
				return haloSprite;
			}

			// A shared nine-slice keeps the golden rim round on both portraits and wide buttons.
			const int size = 64;
			const float radius = 8f;
			const float core = 19f;
			var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
			{
				name = "Ink selection glow",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			var pixels = new Color32[size * size];
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					float dx = Mathf.Abs(x + 0.5f - size * 0.5f) - core;
					float dy = Mathf.Abs(y + 0.5f - size * 0.5f) - core;
					float outerX = Mathf.Max(dx, 0f);
					float outerY = Mathf.Max(dy, 0f);
					float distance = Mathf.Sqrt(outerX * outerX + outerY * outerY) +
						Mathf.Min(Mathf.Max(dx, dy), 0f) - radius;
					float edge = Mathf.Abs(distance);
					float rim = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.65f, 1.65f, edge));
					float glow = Mathf.Clamp01(1f - edge / (distance > 0f ? 4.2f : 9f));
					float alpha = Mathf.Clamp01(rim * 0.85f + glow * glow * 0.54f);
					pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha * 255f));
				}
			}
			texture.SetPixels32(pixels);
			texture.Apply(false, true);
			haloSprite = Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f),
				100f, 0, SpriteMeshType.FullRect, new Vector4(17f, 17f, 17f, 17f));
			haloSprite.name = "Ink selection glow";
			return haloSprite;
		}
	}
}
