using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DJSD_GongFaDuiHuanShaiXuanWnd : Window
	{
		public DJSD_GongFaDuiHuanShaiXuanPanel Panel;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DJSD_GongFaDuiHuanShaiXuanPanel>();
			AddButtonClickListener(Panel.MaskBtn, PopBackWindow);
			AddButtonClickListener(Panel.QueDingBtn, PopBackWindow);
			AddToggleClickListener(Panel.WuXing_All, WuXing_All);
			AddToggleClickListener(Panel.WuXing_Jin, WuXing_Jin);
			AddToggleClickListener(Panel.WuXing_Mu, WuXing_Mu);
			AddToggleClickListener(Panel.WuXing_Shui, WuXing_Shui);
			AddToggleClickListener(Panel.WuXing_Huo, WuXing_Huo);
			AddToggleClickListener(Panel.WuXing_Tu, WuXing_Tu);
			AddToggleClickListener(Panel.PinJie_Huang, PinJie_Huang);
			AddToggleClickListener(Panel.PinJie_Xuan, PinJie_Xuan);
			AddToggleClickListener(Panel.PinJie_Di, PinJie_Di);
			AddToggleClickListener(Panel.PinJie_Tian, PinJie_Tian);
			AddToggleClickListener(Panel.DengJie_One, DengJie_One);
			AddToggleClickListener(Panel.DengJie_Two, DengJie_Two);
			AddToggleClickListener(Panel.DengJie_Three, DengJie_Three);
			AddToggleClickListener(Panel.DengJie_Four, DengJie_Four);
			AddToggleClickListener(Panel.DengJie_Five, DengJie_Five);
			AddToggleClickListener(Panel.DengJie_Six, DengJie_Six);
			AddToggleClickListener(Panel.DengJie_Seven, DengJie_Seven);
			AddToggleClickListener(Panel.DengJie_Eight, DengJie_Eight);
			AddToggleClickListener(Panel.DengJie_Nine, DengJie_Nine);
			AddToggleClickListener(Panel.Type_NaQi, YaoXiao_ZhenYuan);
			AddToggleClickListener(Panel.Type_DuanTi, YaoXiao_TiPo);
			AddToggleClickListener(Panel.Type_LianShen, YaoXiao_ShenShi);
			m_CurrentWuXing = -1;
			m_CurrentStage = 1;
			m_CurrentPinJie = 1;
			m_CurrentType = 3;
		}

		private void WuXing_All(bool isOn)
		{
			ChangeTextColor(Panel.WuXing_All, isOn);
			if (isOn)
			{
				m_CurrentWuXing = -1;
			}
		}

		private void WuXing_Jin(bool isOn)
		{
			ChangeTextColor(Panel.WuXing_Jin, isOn);
			if (isOn)
			{
				m_CurrentWuXing = 1;
			}
		}

		private void WuXing_Mu(bool isOn)
		{
			ChangeTextColor(Panel.WuXing_Mu, isOn);
			if (isOn)
			{
				m_CurrentWuXing = 2;
			}
		}

		private void WuXing_Shui(bool isOn)
		{
			ChangeTextColor(Panel.WuXing_Shui, isOn);
			if (isOn)
			{
				m_CurrentWuXing = 3;
			}
		}

		private void WuXing_Huo(bool isOn)
		{
			ChangeTextColor(Panel.WuXing_Huo, isOn);
			if (isOn)
			{
				m_CurrentWuXing = 4;
			}
		}

		private void WuXing_Tu(bool isOn)
		{
			ChangeTextColor(Panel.WuXing_Tu, isOn);
			if (isOn)
			{
				m_CurrentWuXing = 5;
			}
		}

		private void PinJie_Huang(bool isOn)
		{
			ChangeTextColor(Panel.PinJie_Huang, isOn);
			if (isOn)
			{
				m_CurrentPinJie = 1;
			}
		}

		private void PinJie_Xuan(bool isOn)
		{
			ChangeTextColor(Panel.PinJie_Xuan, isOn);
			if (isOn)
			{
				m_CurrentPinJie = 2;
			}
		}

		private void PinJie_Di(bool isOn)
		{
			ChangeTextColor(Panel.PinJie_Di, isOn);
			if (isOn)
			{
				m_CurrentPinJie = 3;
			}
		}

		private void PinJie_Tian(bool isOn)
		{
			ChangeTextColor(Panel.PinJie_Tian, isOn);
			if (isOn)
			{
				m_CurrentPinJie = 4;
			}
		}

		private void DengJie_One(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_One, isOn);
			if (isOn)
			{
				m_CurrentStage = 1;
			}
		}

		private void DengJie_Two(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Two, isOn);
			if (isOn)
			{
				m_CurrentStage = 2;
			}
		}

		private void DengJie_Three(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Three, isOn);
			if (isOn)
			{
				m_CurrentStage = 3;
			}
		}

		private void DengJie_Four(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Four, isOn);
			if (isOn)
			{
				m_CurrentStage = 4;
			}
		}

		private void DengJie_Five(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Five, isOn);
			if (isOn)
			{
				m_CurrentStage = 5;
			}
		}

		private void DengJie_Six(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Six, isOn);
			if (isOn)
			{
				m_CurrentStage = 6;
			}
		}

		private void DengJie_Seven(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Seven, isOn);
			if (isOn)
			{
				m_CurrentStage = 7;
			}
		}

		private void DengJie_Eight(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Eight, isOn);
			if (isOn)
			{
				m_CurrentStage = 8;
			}
		}

		private void DengJie_Nine(bool isOn)
		{
			ChangeTextColor(Panel.DengJie_Nine, isOn);
			if (isOn)
			{
				m_CurrentStage = 9;
			}
		}

		private void YaoXiao_ZhenYuan(bool isOn)
		{
			ChangeTextColor(Panel.Type_NaQi, isOn);
			if (isOn)
			{
				m_CurrentType = 3;
			}
		}

		private void YaoXiao_TiPo(bool isOn)
		{
			ChangeTextColor(Panel.Type_DuanTi, isOn);
			if (isOn)
			{
				m_CurrentType = 2;
			}
		}

		private void YaoXiao_ShenShi(bool isOn)
		{
			ChangeTextColor(Panel.Type_LianShen, isOn);
			if (isOn)
			{
				m_CurrentType = 1;
			}
		}

		public void PopBackWindow()
		{
			PopUpPanel(isPop: false);
		}

		public void PopUpWindow()
		{
			PopUpPanel(isPop: true);
		}

		private void PopUpPanel(bool isPop)
		{
			RectTransform rect = Panel.gameObject.GetComponent<RectTransform>();
			_ = rect.sizeDelta;
			if (isPop)
			{
				DOTween.To(() => rect.anchoredPosition, (Vector2 v) =>
				{
					rect.anchoredPosition = v;
				}, new Vector2(0f, 0f), 0.5f);
				Panel.MaskBtn.gameObject.SetActive(value: true);
			}
			else
			{
				DOTween.To(() => rect.anchoredPosition, (Vector2 v) =>
				{
					rect.anchoredPosition = v;
				}, new Vector2(215f, 0f), 0.5f);
				Panel.MaskBtn.gameObject.SetActive(value: false);
			}
		}

		private void ChangeTextColor(Toggle tog, bool isChoose)
		{
			Text component = tog.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isChoose)
			{
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		public void GetShaiXuanResult(out int? WuXing, out int Stage, out int PinJie, out int gongFaType)
		{
			WuXing = m_CurrentWuXing;
			Stage = m_CurrentStage;
			PinJie = m_CurrentPinJie;
			gongFaType = m_CurrentType;
		}
	}
}
