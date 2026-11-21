using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class DaoYouRows<T> where T : Component
	{
		private const float Height = 78f;
		private const float Gap = 7f;
		private const float RowStep = Height + Gap;
		private readonly ScrollRect scroll;
		private readonly GameObject prefab;
		private readonly Action<T> style;
		private readonly Action<T, int> bind;
		private readonly List<T> rows = new List<T>();
		private int total;
		private int firstIndex = -1;
		private int previousDrawCount = -1;

		internal DaoYouRows(ScrollRect scroll, GameObject prefab, Action<T> style, Action<T, int> bind)
		{
			this.scroll = scroll;
			this.prefab = prefab;
			this.style = style;
			this.bind = bind;
			scroll.onValueChanged.AddListener(_ => Draw());
		}

		internal void Reset(int count, bool returnToTop = true)
		{
			total = count;
			scroll.StopMovement();
			float height = count > 0 ? count * RowStep - Gap : 0f;
			scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			float current = Mathf.Clamp(scroll.content.anchoredPosition.y, 0f, Mathf.Max(0, height - scroll.viewport.rect.height));
			scroll.content.anchoredPosition = new Vector2(0f, returnToTop ? 0f : current);
			Rebind();
		}

		internal void Rebind()
		{
			firstIndex = -1;
			Draw();
		}

		private void Draw()
		{
			int slots = Mathf.CeilToInt(scroll.viewport.rect.height / RowStep) + 2;
			int first = Mathf.Clamp(Mathf.FloorToInt(scroll.content.anchoredPosition.y / RowStep) - 1,
				0, Mathf.Max(total - slots, 0));
			int count = Mathf.Min(slots, total - first);
			if (firstIndex == first && previousDrawCount == count)
				return;
			firstIndex = first;
			previousDrawCount = count;

			while (rows.Count < count)
			{
				T item = UnityEngine.Object.Instantiate(prefab, scroll.content, false).GetComponent<T>();
				style(item);
				rows.Add(item);
			}

			for (int i = 0; i < rows.Count; i++)
			{
				T row = rows[i];
				row.gameObject.SetActive(i < count);
				if (i >= count)
					continue;

				int index = first + i;
				bind(row, index);
				DaoYouItem.Place((RectTransform)row.transform, 0f, 1f, 1f, 0f, 0f, -index * RowStep - Height, -index * RowStep);
			}
		}
	}
}
