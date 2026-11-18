using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DoubleClikeButton : Button
	{
		[Serializable]
		public class DoubleClickEvent : UnityEvent
		{
		}

		[SerializeField]
		private DoubleClickEvent _onDoubleClick = new DoubleClickEvent();

		private DateTime m_firstTime;

		private DateTime m_SecondTime;

		public DoubleClickEvent OnDoubleClick
		{
			get
			{
				return _onDoubleClick;
			}
			set
			{
				_onDoubleClick = value;
			}
		}

		private void ResetTime()
		{
			m_firstTime = default;
			m_SecondTime = default;
		}

		private void Press()
		{
			if (OnDoubleClick != null)
			{
				OnDoubleClick.Invoke();
			}
			else
			{
				ResetTime();
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			if (m_firstTime.Equals(default))
			{
				m_firstTime = DateTime.Now;
			}
			else
			{
				m_SecondTime = DateTime.Now;
			}
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			if (!m_firstTime.Equals(default) && !m_SecondTime.Equals(default))
			{
				TimeSpan timeSpan = m_SecondTime - m_firstTime;
				if ((float)(timeSpan.Seconds * 1000 + timeSpan.Milliseconds) < 400f)
				{
					Press();
				}
				else
				{
					ResetTime();
				}
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			ResetTime();
		}
	}
}
