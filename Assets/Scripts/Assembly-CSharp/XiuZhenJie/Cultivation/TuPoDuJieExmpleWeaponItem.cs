using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class TuPoDuJieExmpleWeaponItem : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		private bool _MouseHaveDown;

		private UnityAction MouseUpCallBack;

		private UnityAction MouseDownCallBack;

		private void Update()
		{
			if (Input.GetMouseButtonUp(0) && _MouseHaveDown)
			{
				_MouseHaveDown = false;
				if (MouseUpCallBack != null)
				{
					MouseUpCallBack();
				}
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_MouseHaveDown = true;
			if (MouseDownCallBack != null)
			{
				MouseDownCallBack();
			}
		}

		public void SetValue(UnityAction MouseUpAction, UnityAction MouseDownAction)
		{
			MouseUpCallBack = MouseUpAction;
			MouseDownCallBack = MouseDownAction;
		}
	}
}
