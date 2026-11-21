using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkArtGlint : MonoBehaviour
	{
		private static readonly int SpriteUv = Shader.PropertyToID("_SpriteUV");
		private static readonly int Sweep = Shader.PropertyToID("_Sweep");
		private Image overlay, target;
		private Material material;
		private Vector4 spriteUv;
		private float started, delay;

		// Windows use a fixed illustration. The HUD moves one overlay between its icons.
		internal static void Attach(Image art, Color tint, float offset = 0f)
		{
			if (art == null) return;
			var effect = art.transform.Find("HudGlint")?.GetComponent<InkArtGlint>() ?? Create(art.transform, tint);
			if (effect == null) return;
			effect.target = art;
			effect.overlay.color = tint;
			effect.delay = offset;
			effect.enabled = true;
		}

		internal static InkArtGlint Create(Transform parent, Color tint)
		{
			var shader = ReleasedResources.Load<Shader>("shaders/HudGlint");
			if (shader == null || !shader.isSupported) return null;
			var node = new GameObject("HudGlint", typeof(RectTransform), typeof(Image));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			var effect = node.AddComponent<InkArtGlint>();
			effect.overlay = node.GetComponent<Image>();
			effect.material = new Material(shader);
			effect.overlay.material = effect.material;
			effect.overlay.raycastTarget = false;
			effect.overlay.color = tint;
			effect.enabled = false;
			effect.Hide();
			return effect;
		}

		internal void Show(Image art, float phase, Color tint)
		{
			var sprite = art.overrideSprite;
			if (sprite == null || !art.isActiveAndEnabled)
			{
				Hide();
				return;
			}
			if (transform.parent != art.transform || overlay.sprite != sprite)
			{
				var rect = overlay.rectTransform;
				rect.SetParent(art.transform, false);
				rect.SetAsFirstSibling();
				rect.pivot = art.rectTransform.pivot;
				rect.anchorMin = Vector2.zero;
				rect.anchorMax = Vector2.one;
				rect.offsetMin = rect.offsetMax = Vector2.zero;
				overlay.sprite = sprite;
				overlay.preserveAspect = art.preserveAspect;
				overlay.useSpriteMesh = art.useSpriteMesh;
				spriteUv = DataUtility.GetOuterUV(sprite);
				material.SetVector(SpriteUv, spriteUv);
			}
			float sweep = Mathf.Lerp(-.18f, 1.18f, phase);
			overlay.color = tint;
			material.SetFloat(Sweep, sweep);
			overlay.enabled = true;
			// Under a UI Mask, Unity renders a stencil copy of our material.
			var renderer = overlay.canvasRenderer;
			var rendered = renderer.materialCount > 0 ? renderer.GetMaterial(0) : null;
			if (rendered != null && rendered != material && rendered.shader == material.shader)
			{
				rendered.SetVector(SpriteUv, spriteUv);
				rendered.SetFloat(Sweep, sweep);
			}
		}

		internal void Hide()
		{
			overlay.enabled = false;
		}

		private void OnEnable() => started = Time.unscaledTime;

		private void Update()
		{
			float phase = Mathf.Repeat(Time.unscaledTime - started - delay, 7.6f) / 1.05f;
			if (phase < 1f) Show(target, phase, overlay.color);
			else Hide();
		}

		private void OnDisable()
		{
			if (overlay != null) Hide();
		}

		private void OnDestroy()
		{
			if (material != null) Destroy(material);
		}
	}
}
