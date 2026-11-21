using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie
{
	// A tap selects the field; dragging its name changes the order in the same list.
	internal sealed class RingOrderDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		private RectTransform handle, sheet;
		private float stepHeight, grabOffset;
		private Action<int> moveByOne;
		private int? activePointer;

		internal static RingOrderDrag Attach(Button handle, RectTransform sheet, float stepHeight, Action<int> moveByOne)
		{
			var drag = handle.gameObject.AddComponent<RingOrderDrag>();
			drag.handle = (RectTransform)handle.transform;
			drag.sheet = sheet;
			drag.stepHeight = stepHeight;
			drag.moveByOne = moveByOne;
			return drag;
		}

		private float RowCenter => sheet.InverseTransformPoint(handle.TransformPoint(handle.rect.center)).y;

		public void OnBeginDrag(PointerEventData pointer)
		{
			if (pointer.button != PointerEventData.InputButton.Left) return;
			pointer.eligibleForClick = false;
			if (activePointer.HasValue || !RectTransformUtility.ScreenPointToLocalPointInRectangle(
				sheet, pointer.pressPosition, pointer.pressEventCamera, out var pressed)) return;
			activePointer = pointer.pointerId;
			grabOffset = pressed.y - RowCenter;
		}

		public void OnDrag(PointerEventData pointer)
		{
			if (activePointer != pointer.pointerId) return;
			pointer.eligibleForClick = false;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(sheet, pointer.position, pointer.pressEventCamera, out var point)) return;
			float target = point.y - grabOffset;
			float center = RowCenter;
			// Compare to the actual row center after each move. Reversing a drag at
			// either end therefore never advances the row before the finger returns.
			while (Mathf.Abs(target - center) > stepHeight * .55f)
			{
				moveByOne(target > center ? -1 : 1);
				float next = RowCenter;
				if (Mathf.Approximately(next, center)) break;
				center = next;
			}
		}

		public void OnEndDrag(PointerEventData pointer)
		{
			if (activePointer != pointer.pointerId) return;
			pointer.eligibleForClick = false;
			activePointer = null;
		}

		private void OnDisable() => activePointer = null;
	}
}
