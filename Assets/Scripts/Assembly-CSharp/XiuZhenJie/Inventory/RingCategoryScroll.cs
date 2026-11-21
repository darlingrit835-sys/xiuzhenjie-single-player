using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class RingCategoryScroll : ScrollRect
    {
        internal Action LayoutTabs, SnapTabs;
        private float lastWidth = -1f;
        private float wheelEnd;

        protected override void LateUpdate()
        {
            base.LateUpdate();
            float width = viewport != null ? viewport.rect.width : 0;
            if (width >= 1f && Mathf.Abs(width - lastWidth) > .25f)
            {
                lastWidth = width;
                LayoutTabs?.Invoke();
            }
            if (wheelEnd > 0 && Time.unscaledTime >= wheelEnd)
            {
                wheelEnd = 0;
                SnapTabs?.Invoke();
            }
        }

        public override void OnBeginDrag(PointerEventData data)
        {
            wheelEnd = 0;
            base.OnBeginDrag(data);
        }

        public override void OnEndDrag(PointerEventData data)
        {
            base.OnEndDrag(data);
            if (data.button == PointerEventData.InputButton.Left) SnapTabs?.Invoke();
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);
            wheelEnd = Time.unscaledTime + .18f;
        }
    }
}
