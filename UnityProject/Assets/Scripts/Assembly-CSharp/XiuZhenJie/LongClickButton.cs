using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LongClickButton : Button
	{
		[Serializable]
		public class LongButtonEvent : UnityEvent
		{
		}

		[SerializeField]
		private LongButtonEvent _onLongButtonClick = new LongButtonEvent();

		[SerializeField]
		private LongButtonEvent _onLongButtonShortClick = new LongButtonEvent();

		[SerializeField]
		private LongButtonEvent _onLongButtonDown = new LongButtonEvent();

		[SerializeField]
		private LongButtonEvent _onLongButtonUp = new LongButtonEvent();

		private DateTime m_FirstTime;

		private DateTime m_SecondTime;

		public LongButtonEvent OnLongButtonClick
		{
			get
			{
				return _onLongButtonClick;
			}
			set
			{
				_onLongButtonClick = value;
			}
		}

		public LongButtonEvent OnLongButtonShortClick
		{
			get
			{
				return _onLongButtonShortClick;
			}
			set
			{
				_onLongButtonShortClick = value;
			}
		}

		public LongButtonEvent OnLongButtonDown
		{
			get
			{
				return _onLongButtonDown;
			}
			set
			{
				_onLongButtonDown = value;
			}
		}

		public LongButtonEvent OnLongButtonDoUp
		{
			get
			{
				return _onLongButtonUp;
			}
			set
			{
				_onLongButtonUp = value;
			}
		}

		private void ResetTime()
		{
			m_FirstTime = default;
			m_SecondTime = default;
		}

		private void Press()
		{
			if (OnLongButtonClick != null)
			{
				OnLongButtonClick.Invoke();
			}
			else
			{
				ResetTime();
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			if (m_FirstTime.Equals(default))
			{
				m_FirstTime = DateTime.Now;
			}
			if (OnLongButtonDown != null)
			{
				OnLongButtonDown.Invoke();
			}
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			if (!m_FirstTime.Equals(default))
			{
				m_SecondTime = DateTime.Now;
			}
			if (!m_FirstTime.Equals(default) && !m_SecondTime.Equals(default))
			{
				TimeSpan timeSpan = m_SecondTime - m_FirstTime;
				if ((float)(timeSpan.Seconds * 1000 + timeSpan.Milliseconds) > 600f)
				{
					Press();
				}
				else
				{
					ResetTime();
					if (OnLongButtonShortClick != null)
					{
						OnLongButtonShortClick.Invoke();
					}
				}
			}
			if (OnLongButtonDoUp != null)
			{
				OnLongButtonDoUp.Invoke();
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			ResetTime();
		}
	}
}
