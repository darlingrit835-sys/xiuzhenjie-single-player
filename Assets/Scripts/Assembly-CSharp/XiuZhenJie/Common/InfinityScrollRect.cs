using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class InfinityScrollRect : MonoBehaviour
	{
		private int m_NumOneLine;

		private int m_LineNum;

		private float m_ItemHight;

		private List<GameObject> m_DataList;

		private GridLayoutGroup m_LayoutGroup;

		private float m_OneLineHight;

		private int m_CurrentShowIndexCount;

		private RectTransform m_ThisRect;

		public UnityAction<List<GameObject>> NextLineInstanceAction;

		public UnityAction<List<GameObject>> LastLineInstanceAction;

		private void Awake()
		{
			m_DataList = new List<GameObject>();
			m_ThisRect = base.gameObject.GetComponent<RectTransform>();
			m_ItemHight = m_LayoutGroup.cellSize.y;
			m_LayoutGroup = base.gameObject.GetComponent<GridLayoutGroup>();
			m_OneLineHight = m_LayoutGroup.cellSize.y + m_LayoutGroup.spacing.y;
			m_CurrentShowIndexCount = (int)(m_ThisRect.anchoredPosition.y / m_OneLineHight);
		}

		private void Start()
		{
			GetOneRowLineNum();
			GetShowNum();
		}

		private void Update()
		{
			int num = (int)(m_ThisRect.anchoredPosition.y / m_OneLineHight);
			if (num > m_CurrentShowIndexCount)
			{
				m_CurrentShowIndexCount = num;
				NextLineInstance();
			}
			else if (num < m_CurrentShowIndexCount && m_CurrentShowIndexCount != 0)
			{
				m_CurrentShowIndexCount = num;
				LastLineInstance();
			}
		}

		private void NextLineInstance()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < m_NumOneLine; i++)
			{
				GameObject item = m_DataList[0];
				list.Add(item);
				m_DataList.RemoveAt(i);
				m_DataList.Add(item);
			}
			if (NextLineInstanceAction != null)
			{
				NextLineInstanceAction(list);
			}
		}

		private void LastLineInstance()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < m_NumOneLine; i++)
			{
				GameObject item = m_DataList[m_DataList.Count - 1];
				list.Add(item);
				m_DataList.RemoveAt(m_DataList.Count - 1);
				m_DataList.Insert(0, item);
			}
			if (LastLineInstanceAction != null)
			{
				LastLineInstanceAction(list);
			}
		}

		private void GetOneRowLineNum()
		{
			int num = 0;
			float num2 = m_ThisRect.rect.size.x;
			float x = m_LayoutGroup.cellSize.x;
			while (x != 0f && num2 >= x)
			{
				num2 -= x;
				num++;
				if (!(num2 > m_LayoutGroup.spacing.x))
				{
					break;
				}
				num2 -= m_LayoutGroup.spacing.x;
			}
			m_NumOneLine = num;
		}

		private void GetShowNum()
		{
			float y = base.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.y;
			m_LineNum = (int)(y / m_OneLineHight);
			m_LineNum++;
		}

		public bool AddItem(GameObject item)
		{
			m_DataList.Add(item);
			if (m_DataList.Count >= m_NumOneLine * m_LineNum)
			{
				return false;
			}
			return true;
		}
	}
}
