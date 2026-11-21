using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DoubleXiuLianProgressPanel : MonoBehaviour
	{
		public RectTransform LeftProgressRect;

		public RectTransform RightProgressRect;

		public Button BiGuanTuPoBtn;

		public Image LeftProgressImg;

		public Text LeftTitleText;

		public Text LeftProgressText;

		public Text LeftProgressPercentText;

		public Image RightProgressImg;

		public Text RightTitleText;

		public Text RightProgressText;

		public Text RightProgressPercentText;

		public Image LianShenYuanImg;

		public Image DuanTiYuanImg;

		public Image NaQiYuanImg;

		private RuningGongFaZuHeType m_CurrentRunningType;

		private void Update()
		{
			if (!LeftProgressText.Equals(null) && !RightProgressText.Equals(null))
			{
				switch (m_CurrentRunningType)
				{
				case RuningGongFaZuHeType.DuanTiNaQi:
					LeftProgressText.text = Singleton<RoleManager>.Instance.Role_Entity.Physique.ToString();
					ReflashSuperAir();
					break;
				case RuningGongFaZuHeType.DuanTiLianShen:
					LeftProgressText.text = Singleton<RoleManager>.Instance.Role_Entity.Physique.ToString();
					RightProgressText.text = Singleton<RoleManager>.Instance.Role_Entity.Perception.ToString();
					break;
				case RuningGongFaZuHeType.LianShenNaQi:
					LeftProgressText.text = Singleton<RoleManager>.Instance.Role_Entity.Perception.ToString();
					ReflashSuperAir();
					break;
				}
			}
		}

		private void ReflashSuperAir()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				RightProgressText.text = Singleton<RoleManager>.Instance.Role_Entity.SuperAir.ToString();
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				RightProgressText.text = (Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100).ToString();
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				RightProgressText.text = (Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100 / 100).ToString();
			}
			double num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir;
			double upgradeByLevelId = Singleton<RoleManager>.Instance.GetUpgradeByLevelId(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel.ToString());
			RightProgressPercentText.text = string.Format("{0}%", (num / upgradeByLevelId * 100.0).ToString("0.00"));
			RightProgressImg.fillAmount = (float)(num / upgradeByLevelId);
		}

		public void SetRunningGongFaType(RuningGongFaZuHeType runningType)
		{
			m_CurrentRunningType = runningType;
			switch (m_CurrentRunningType)
			{
			case RuningGongFaZuHeType.DuanTiNaQi:
				DuanTiValue();
				NaQiValue();
				break;
			case RuningGongFaZuHeType.DuanTiLianShen:
				DuanTiValue();
				LianShenValue();
				break;
			case RuningGongFaZuHeType.LianShenNaQi:
				LianShenValue();
				NaQiValue();
				break;
			}
		}

		private void DuanTiValue()
		{
			LeftTitleText.text = "锻体";
			LeftProgressPercentText.gameObject.SetActive(value: false);
			LeftProgressImg.gameObject.SetActive(value: false);
		}

		private void LianShenValue()
		{
			switch (m_CurrentRunningType)
			{
			case RuningGongFaZuHeType.DuanTiLianShen:
				RightTitleText.text = "炼神";
				RightProgressImg.gameObject.SetActive(value: false);
				RightProgressPercentText.gameObject.SetActive(value: false);
				break;
			case RuningGongFaZuHeType.LianShenNaQi:
				LeftTitleText.text = "炼神";
				LeftProgressImg.gameObject.SetActive(value: false);
				LeftProgressPercentText.gameObject.SetActive(value: false);
				break;
			}
		}

		private void NaQiValue()
		{
			RightTitleText.text = "纳气";
			RightProgressImg.gameObject.SetActive(value: true);
			RightProgressPercentText.gameObject.SetActive(value: true);
		}
	}
}
