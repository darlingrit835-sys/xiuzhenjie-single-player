using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ScrollViewInfiniteVertical : MonoBehaviour
	{
		public RectTransform ScrollViewRect;

		public float ItemHight;

		public bool CreateCompelete;

		public float SpacingValue;

		public UnityAction<GameObject> UpScrollAction;

		public UnityAction<GameObject> DownScrollAction;

		private List<GameObject> m_Childs;

		private float m_ContentHight;

		private float m_ScrollViewHight;

		private int m_ChildCount;

		private int m_CurrentStartRow;

		private float m_CurrentContentPosY;

		private RectTransform m_CurrentContentRect;

		private Queue<int> m_UpQue;

		private Queue<int> m_DownQue;

		private int m_MaxChildCount;

		private void Awake()
		{
			m_CurrentStartRow = 0;
			m_Childs = new List<GameObject>();
			m_UpQue = new Queue<int>();
			m_DownQue = new Queue<int>();
			CreateCompelete = false;
			m_ScrollViewHight = ScrollViewRect.sizeDelta.y;
			m_CurrentContentRect = base.gameObject.GetComponent<RectTransform>();
		}

		private void Update()
		{
			if (!CreateCompelete)
			{
				return;
			}
			m_CurrentContentPosY = m_CurrentContentRect.anchoredPosition.y;
			if (!(m_CurrentContentRect.sizeDelta.y > m_ScrollViewHight) || (!(m_CurrentContentPosY > 0f) && !(m_CurrentContentPosY < m_CurrentContentRect.sizeDelta.y - m_ScrollViewHight)) || !(m_CurrentContentPosY > m_CurrentContentRect.anchoredPosition.y - m_ScrollViewHight))
			{
				return;
			}
			int num = Mathf.FloorToInt(m_CurrentContentPosY / (ItemHight + SpacingValue));
			if (num > m_CurrentStartRow)
			{
				if (m_CurrentStartRow == m_MaxChildCount - m_ChildCount)
				{
					return;
				}
				for (int i = 0; i < num - m_CurrentStartRow; i++)
				{
					if (m_CurrentStartRow + m_ChildCount >= m_MaxChildCount)
					{
						break;
					}
					DownDealWith();
				}
			}
			else
			{
				if (num >= m_CurrentStartRow || m_CurrentStartRow == 0)
				{
					return;
				}
				for (int j = 0; j < m_CurrentStartRow - num; j++)
				{
					if (m_CurrentStartRow == 0)
					{
						break;
					}
					UpDealWith();
				}
			}
		}

		private void DownDealWith()
		{
			GameObject gameObject = m_Childs[0];
			GameObject gameObject2 = m_Childs[m_Childs.Count - 1];
			if (!(gameObject == null) && !(gameObject2 == null))
			{
				gameObject.GetComponent<RectTransform>();
				_ = gameObject2.GetComponent<RectTransform>().anchoredPosition;
				m_Childs.Remove(gameObject);
				m_Childs.Add(gameObject);
				m_CurrentStartRow++;
				gameObject.transform.SetAsLastSibling();
				if (DownScrollAction != null)
				{
					DownScrollAction(gameObject);
				}
			}
		}

		private void UpDealWith()
		{
			GameObject gameObject = m_Childs[m_Childs.Count - 1];
			GameObject gameObject2 = m_Childs[m_Childs.Count - 1];
			if (!(gameObject == null) && !(gameObject2 == null))
			{
				gameObject.GetComponent<RectTransform>();
				_ = gameObject2.GetComponent<RectTransform>().anchoredPosition;
				m_Childs.Remove(gameObject);
				m_Childs.Insert(0, gameObject);
				m_CurrentStartRow--;
				gameObject.transform.SetAsFirstSibling();
				if (UpScrollAction != null)
				{
					UpScrollAction(gameObject);
				}
			}
		}

		public void SetCompelete(int maxCount)
		{
			m_MaxChildCount = maxCount;
			CreateCompelete = true;
			m_Childs.Clear();
			foreach (Transform item in base.transform)
			{
				m_Childs.Add(item.gameObject);
			}
			m_ChildCount = base.transform.childCount;
			m_ContentHight = base.gameObject.GetComponent<RectTransform>().sizeDelta.y;
		}

		public void ReduceGameObj()
		{
			if (m_CurrentStartRow != 0)
			{
				m_CurrentStartRow--;
			}
		}
	}
}
