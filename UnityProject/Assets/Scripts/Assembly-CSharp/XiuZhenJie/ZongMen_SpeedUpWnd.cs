using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_SpeedUpWnd : Window
	{
		public ZongMen_SpeedUpPanel Panel;

		private int m_TouRuLingYu;

		private bool m_JiaBtnIsPressed;

		private bool m_JianBtnIsPressed;

		private int m_CurrentMaxLingYu;

		private float m_Timing;

		private UnityAction<int> SureBtnClickAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_SpeedUpPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.TouRuBtn, QueRenTouRuBtnClick);
			Panel.JiaBtn.OnLongButtonDown.AddListener(JiaBtnLongButtonDown);
			Panel.JiaBtn.OnLongButtonDoUp.AddListener(JiaBtnLongButtonUp);
			Panel.JianBtn.OnLongButtonDown.AddListener(JianBtnLongButtonDown);
			Panel.JianBtn.OnLongButtonDoUp.AddListener(JianBtnLongButtonUp);
			m_CurrentMaxLingYu = (int)paralist[0];
			SureBtnClickAction = paralist[1] as UnityAction<int>;
			m_TouRuLingYu = 0;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_JiaBtnIsPressed)
			{
				m_Timing += Time.deltaTime;
				if (m_Timing > 2f)
				{
					m_Timing = 0f;
					AddLingYu(10);
				}
			}
			else if (m_JianBtnIsPressed)
			{
				m_Timing += Time.deltaTime;
				if (m_Timing > 2f)
				{
					m_Timing = 0f;
					AddLingYu(-10);
				}
			}
		}

		private void QueRenTouRuBtnClick()
		{
			if (SureBtnClickAction != null)
			{
				SureBtnClickAction(m_TouRuLingYu);
			}
			CloseWindow();
		}

		private void JiaBtnLongButtonDown()
		{
			m_JiaBtnIsPressed = true;
			AddLingYu(1);
		}

		private void JiaBtnLongButtonUp()
		{
			m_JiaBtnIsPressed = false;
		}

		private void JiaBtnLongButtonClick()
		{
			m_JiaBtnIsPressed = false;
			m_Timing = 0f;
		}

		private void JianBtnLongButtonDown()
		{
			m_JianBtnIsPressed = true;
			AddLingYu(-1);
		}

		private void JianBtnLongButtonUp()
		{
			m_JianBtnIsPressed = false;
			m_Timing = 0f;
		}

		private void AddLingYu(int num)
		{
			if (m_TouRuLingYu + num < 0)
			{
				m_TouRuLingYu = 0;
			}
			else
			{
				m_TouRuLingYu += num;
			}
			Panel.LingYuText.text = m_TouRuLingYu.ToString();
			Panel.JianShaoSecondText.text = $"投入后可减少时间:  <size=18>{m_TouRuLingYu * 60}</size>  秒";
		}

		public override void OnClose()
		{
			base.OnClose();
			Panel.JiaBtn.OnLongButtonDoUp.RemoveAllListeners();
			Panel.JianBtn.OnLongButtonDown.RemoveAllListeners();
			Panel.JiaBtn.OnLongButtonDoUp.RemoveAllListeners();
			Panel.JiaBtn.OnLongButtonDown.RemoveAllListeners();
		}
	}
}
