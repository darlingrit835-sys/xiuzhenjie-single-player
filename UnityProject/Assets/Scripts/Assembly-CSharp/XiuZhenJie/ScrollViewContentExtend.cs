using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ScrollViewContentExtend : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler
	{
		public UnityAction XiaLaAction;

		public UnityAction ShangLaAction;

		private RectTransform m_CurrentRect;

		public RectTransform ContentRect;

		private const int PULL_UP_BEYOND = -30;

		private const int PULL_DOWN_BEYOND = 30;

		private void Start()
		{
			m_CurrentRect = base.gameObject.GetComponent<RectTransform>();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (ContentRect.anchoredPosition.y < -30f)
			{
				if (ShangLaAction != null)
				{
					ShangLaAction();
				}
			}
			else if (ContentRect.anchoredPosition.y > ContentRect.sizeDelta.y - m_CurrentRect.sizeDelta.y + 30f && XiaLaAction != null)
			{
				XiaLaAction();
			}
		}
	}
}
