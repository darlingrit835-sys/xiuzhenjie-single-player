using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DropdownExtend : Dropdown
	{
		public UnityAction<PointerEventData> ClickActionEvent;

		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			if (ClickActionEvent != null)
			{
				ClickActionEvent(eventData);
			}
		}

		public override void OnCancel(BaseEventData eventData)
		{
			base.OnCancel(eventData);
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
		}
	}
}
