using System.Collections;
using UnityEngine;

namespace XiuZhenJie
{
	[RequireComponent(typeof(CanvasGroup))]
	internal sealed class BattleHudReveal : MonoBehaviour
	{
		private CanvasGroup group;
		private Vector3 rest;
		private float wait;

		internal static void Attach(Transform panel, float delay = 0f)
		{
			var motion = panel.gameObject.AddComponent<BattleHudReveal>();
			motion.wait = delay;
		}

		private void Awake()
		{
			rest = transform.localScale;
			group = GetComponent<CanvasGroup>();
		}

		private void OnEnable() { StartCoroutine(Reveal()); }

		private IEnumerator Reveal()
		{
			group.alpha = 0;
			yield return null;
			float elapsed = -wait;
			while (elapsed < .28f)
			{
				elapsed += Time.unscaledDeltaTime;
				float t = Mathf.SmoothStep(0, 1, elapsed / .28f);
				group.alpha = t;
				transform.localScale = rest * Mathf.Lerp(.956f, 1f, t);
				yield return null;
			}
			group.alpha = 1f;
			transform.localScale = rest;
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			if (!group) return;
			group.alpha = 1f;
			transform.localScale = rest;
		}
	}
}
