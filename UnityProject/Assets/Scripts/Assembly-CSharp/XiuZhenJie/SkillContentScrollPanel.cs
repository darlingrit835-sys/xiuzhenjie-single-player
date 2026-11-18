using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SkillContentScrollPanel : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler
	{
		public HorizontalLayoutGroup itemConnect;

		[SerializeField]
		private bool buttonPageEnable;

		private int m_nowPage;

		private int m_pageCount;

		public int m_dragNum;

		private float m_pageAreaSize;

		private const float SCROLL_SMOOTH_TIME = 0.2f;

		private const float SCROLL_MOVE_SPEED = 1f;

		private float scrollMoveSpeed;

		private bool scrollNeedMove;

		private float scrollTargetValue;

		public ScrollRect scrollRect;

		public RectTransform item;

		private ZhanDouWnd zhanDouWnd;

		private bool isRegistEvent;

		public bool SetButtonStatus
		{
			set
			{
				buttonPageEnable = value;
			}
		}

		public int GetPageNum => m_nowPage;

		private void Start()
		{
			InitManager(3);
			zhanDouWnd = Singleton<UIManager>.Instance.FindWndByName<ZhanDouWnd>("ZhanDou.prefab");
		}

		public void InitManager(int _allItemNum, Vector2 pageItemSize, bool isNeedChangeSize = true, int pageNum = 0, bool isShowAnim = false)
		{
			RegistEvent();
			int num = (int)(pageItemSize.x * pageItemSize.y);
			m_pageCount = _allItemNum / num + ((_allItemNum % num != 0) ? 1 : 0);
			m_pageAreaSize = 1f / (float)(m_pageCount - 1);
			ChangePage(pageNum);
			if (isNeedChangeSize)
			{
				itemConnect.GetComponent<RectTransform>().sizeDelta = new Vector2((item.sizeDelta.x * pageItemSize.x + itemConnect.spacing * pageItemSize.x) * (float)m_pageCount, (float)(itemConnect.padding.top + itemConnect.padding.bottom) + item.sizeDelta.y * pageItemSize.y + (pageItemSize.y - 1f) * itemConnect.spacing);
			}
		}

		public void InitManager(int pageNum, int targetPage = 0, bool isShowAnim = false)
		{
			RegistEvent();
			m_pageCount = pageNum;
			m_pageAreaSize = 1f / (float)(m_pageCount - 1);
			ChangePage(targetPage, isShowAnim);
		}

		private void RegistEvent()
		{
			if (!isRegistEvent)
			{
				isRegistEvent = true;
			}
		}

		private void Paging(int num)
		{
			num = ((num >= 0) ? 1 : (-1));
			int num2 = Mathf.Clamp(m_nowPage + num, 0, m_pageCount - 1);
			if (m_nowPage != num2)
			{
				ChangePage(num2);
			}
		}

		private void Update()
		{
			ScrollControl();
		}

		private void ScrollControl()
		{
			if (scrollNeedMove)
			{
				if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - scrollTargetValue) < 0.01f)
				{
					scrollRect.horizontalNormalizedPosition = scrollTargetValue;
					scrollNeedMove = false;
				}
				else
				{
					scrollRect.horizontalNormalizedPosition = Mathf.SmoothDamp(scrollRect.horizontalNormalizedPosition, scrollTargetValue, ref scrollMoveSpeed, 0.2f);
				}
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			scrollNeedMove = false;
			scrollTargetValue = 0f;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			int num = m_nowPage;
			int num2 = ((scrollRect.horizontalNormalizedPosition - (float)m_nowPage * m_pageAreaSize >= 0f) ? 1 : (-1));
			if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - (float)m_nowPage * m_pageAreaSize) >= m_pageAreaSize / 86f * (float)m_dragNum)
			{
				num += num2;
			}
			ChangePage(num);
		}

		public void ChangePage(int pageNum, bool isShowAnim = true)
		{
			if (pageNum >= m_pageCount)
			{
				pageNum = m_pageCount - 1;
			}
			if (pageNum < 0)
			{
				pageNum = 0;
			}
			m_nowPage = pageNum;
			ChangePageText(pageNum);
			if (isShowAnim)
			{
				scrollTargetValue = (float)m_nowPage * m_pageAreaSize;
				scrollNeedMove = true;
				scrollMoveSpeed = 0f;
			}
			else
			{
				scrollRect.horizontalNormalizedPosition = (float)m_nowPage * m_pageAreaSize;
			}
			ChangePageText(m_nowPage);
		}

		public void ChangePageText(int num)
		{
			int num2 = m_pageCount - 1;
			m_nowPage = Mathf.Clamp(num, 0, num2);
			switch (m_nowPage)
			{
			}
			if (num2 == 0)
			{
				scrollRect.enabled = false;
				return;
			}
			scrollRect.enabled = true;
			SetButtonStatus = buttonPageEnable;
			_ = buttonPageEnable;
		}
	}
}
