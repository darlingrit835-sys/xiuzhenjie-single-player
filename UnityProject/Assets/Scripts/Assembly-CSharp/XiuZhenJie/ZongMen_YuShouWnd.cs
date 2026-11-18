using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class ZongMen_YuShouWnd : Window
	{
		public ZongMen_YuShouPanel Panel;

		private ZongMenShanFengDuty m_HillDuty;

		private string m_HillName;

		private bool m_IsZongZhu;

		private int m_LingShouBuildTimeRemaining;

		private int m_YuShouYuannBuildTimeRemaining;

		private int m_SiLingDanBuildTimeRemaining;

		private float m_Time;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_YuShouPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.UpdateLingShouBtn, UpdateLingTianClick);
			AddButtonClickListener(Panel.UpdateYuShouYuanBtn, UpdateYuShouYuanClick);
			AddButtonClickListener(Panel.UpdateSiLingDanBtn, UpdateJuLingZhenClick);
			m_HillDuty = ZongMenShanFengDuty.炼丹;
			m_HillName = paralist[0].ToString();
			Panel.ShanFengNameText.text = m_HillName;
			m_Time = 0f;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
			}
			LingShouUpdating();
			SiLingDanUpdating();
			YuShouYuanUpdating();
		}

		private void SetLeaderInfo(int sex, string headId, string name, string battleValue, int level, string fuZhiValue)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, headId);
			Panel.TouXianImg.sprite = touXiangSprite;
			Panel.ZhangLaoNameText.text = name;
			Panel.ZhangLaoZhanLiText.text = battleValue;
			Panel.ZhangLaoEffectText.text = fuZhiValue;
			Panel.ZhangLaoJingJieText.text = WorldSetting.GetRoleLevelNameByRoleLevel(level);
		}

		private void SetItemLineValue(List<string> values)
		{
			Panel.Line_One_ValueText.text = values[0];
			Panel.Line_Two_ValueText.text = values[1];
			Panel.Line_Three_ValueText.text = values[2];
			Panel.Line_Four_ValueText.text = values[3];
			Panel.Line_Five_ValueInput.text = values[4];
			Panel.Line_Six_ValueText.text = values[5];
			Panel.Line_Seven_ValueText.text = values[6];
		}

		private void UpdateLingTianClick()
		{
			Panel.UpdateLingShouPanel.gameObject.SetActive(value: true);
			Panel.UpdateLingShouPanel.SetValue(new List<string>(), LingShouSureBtnClick);
		}

		private void UpdateYuShouYuanClick()
		{
			Panel.UpdateYuShouYuanPanel.gameObject.SetActive(value: true);
			Panel.UpdateYuShouYuanPanel.SetValue(new List<string>(), YuShouYuanSureBtnClick);
		}

		private void UpdateJuLingZhenClick()
		{
			Panel.UpdateSiLingDanPanel.gameObject.SetActive(value: true);
			Panel.UpdateSiLingDanPanel.SetValue(new List<string>(), SiLingDanSureBtnClick);
		}

		private void LingShouSureBtnClick()
		{
			if (Panel.UpdateLingShouBtnText.enabled)
			{
				Panel.UpdateLingShouBtnText.gameObject.SetActive(value: false);
			}
			Panel.UpdateLingShouOther.gameObject.SetActive(value: true);
		}

		private void YuShouYuanSureBtnClick()
		{
			if (Panel.UpdateYuShouYuanBtnText.enabled)
			{
				Panel.UpdateYuShouYuanBtnText.gameObject.SetActive(value: false);
			}
			Panel.UpdateYuShouYuanOther.gameObject.SetActive(value: true);
		}

		private void SiLingDanSureBtnClick()
		{
			if (Panel.UpdateSiLingDanBtnText.enabled)
			{
				Panel.UpdateSiLingDanBtnText.gameObject.SetActive(value: false);
			}
			Panel.UpdateYuShouYuanOther.gameObject.SetActive(value: true);
		}

		private void LingShouUpdating()
		{
			if (m_LingShouBuildTimeRemaining > 0)
			{
				m_LingShouBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_LingShouBuildTimeRemaining);
				Panel.UpdateLingShouOtherTimeText.text = text;
			}
		}

		private void YuShouYuanUpdating()
		{
			if (m_YuShouYuannBuildTimeRemaining > 0)
			{
				m_YuShouYuannBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_YuShouYuannBuildTimeRemaining);
				Panel.UpdateYuShouYuanOtherTimeText.text = text;
			}
		}

		private void SiLingDanUpdating()
		{
			if (m_SiLingDanBuildTimeRemaining > 0)
			{
				m_SiLingDanBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_SiLingDanBuildTimeRemaining);
				Panel.UpdateSiLingDanOtherTimeText.text = text;
			}
		}
	}
}
