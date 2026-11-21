using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class FangYuBtnItem : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler
	{
		public UnityAction FangYuInputDown;

		public UnityAction FangYuInputUp;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (FangYuInputDown != null)
			{
				FangYuInputDown();
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (FangYuInputUp != null)
			{
				FangYuInputUp();
			}
		}
	}
}
