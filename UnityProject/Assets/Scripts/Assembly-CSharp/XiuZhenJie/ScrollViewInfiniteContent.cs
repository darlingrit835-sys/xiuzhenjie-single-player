using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ScrollViewInfiniteContent : MonoBehaviour
	{
		private GridLayoutGroup m_GridLayoutGroup;

		private List<GameObject> m_ObjList;

		private RectTransform m_RectTransform;

		private int m_MinRow;

		private int m_MaxRow;

		private int m_BetweenMinAndMax;

		private float itemHight;

		public UnityAction<int, int> ChangeRowAction;

		private void Awake()
		{
			m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			m_GridLayoutGroup = base.gameObject.GetComponent<GridLayoutGroup>();
			m_ObjList = new List<GameObject>();
			itemHight = m_GridLayoutGroup.cellSize.y;
		}

		private void Update()
		{
			if (m_MinRow == 0 || m_MaxRow == 0 || m_BetweenMinAndMax == 0 || !(m_RectTransform.anchoredPosition.y >= (float)m_GridLayoutGroup.padding.top))
			{
				return;
			}
			int minRow = m_MinRow;
			minRow = Mathf.CeilToInt((m_RectTransform.anchoredPosition.y - (float)m_GridLayoutGroup.padding.top + m_GridLayoutGroup.spacing.y) / (itemHight + m_GridLayoutGroup.spacing.y));
			if (minRow != m_MinRow)
			{
				m_MinRow = minRow;
				m_MaxRow = m_MinRow + m_BetweenMinAndMax;
				if (ChangeRowAction != null)
				{
					ChangeRowAction(m_MinRow, m_MaxRow);
				}
			}
		}

		public void SetMinAndMax(int minRow, int maxRow)
		{
			m_MinRow = minRow;
			m_MaxRow = maxRow;
			m_BetweenMinAndMax = maxRow - minRow;
		}
	}
}
