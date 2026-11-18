using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CashGoodsLingYuPanel : MonoBehaviour
	{
		public List<CashLingYuItem> LingYuItemList;

		public VipPowerPanel VipPowerPanel;

		private CashGoodsWnd m_CashGoodsWnd;

		public Text VipLevelText;

		public Text NextLevelDistanceText;

		public Image ProgressValueImg;

		public Image VipLogo_1_3;

		public Image VipLogo_4_6;

		public Image VipLogo_7_9;

		public Image VipLogo_10_11;

		public Image VipLogo_12;

		private Dictionary<int, VipInfoEntity> m_VipInfoDic;

		public Button ChanKanTeQuanBtn;

		public Button HelpBtn;

		public void Init(CashGoodsWnd cashGoodsWnd, List<bool> lingYuStatus)
		{
			m_CashGoodsWnd = cashGoodsWnd;
			m_VipInfoDic = Singleton<FacadeManager>.Instance.SelectVipInfo();
			if (LingYuItemList != null)
			{
				for (int i = 0; i < LingYuItemList.Count; i++)
				{
					LingYuItemList[i].SetValue(i + 1, lingYuStatus[i], LingYuGouMai);
				}
				SetNextLevelDistanceInfo();
				ShowVipLogo();
				VipPowerPanel.Init(cashGoodsWnd);
				ChanKanTeQuanBtn.onClick.RemoveAllListeners();
				ChanKanTeQuanBtn.onClick.AddListener(ShowVipPowerPanel);
				HelpBtn.onClick.RemoveAllListeners();
				HelpBtn.onClick.AddListener(HelpMsgShow);
			}
		}

		private void ShowVipPowerPanel()
		{
			VipPowerPanel.gameObject.SetActive(value: true);
			VipPowerPanel.GetLingQuStatus();
		}

		private void HelpMsgShow()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("请通过QQ联系客服人员，QQ号：1143049162");
		}

		private void SetNextLevelDistanceInfo()
		{
			int vipLevel = Singleton<RoleManager>.Instance.Role_Entity.VipLevel;
			VipInfoEntity value = null;
			if (m_VipInfoDic.TryGetValue(vipLevel, out value))
			{
				VipLevelText.text = WorldSetting.GetVipAllName(vipLevel) + " 【当前】";
				int num = value.TopPrice - Singleton<RoleManager>.Instance.Role_Entity.TopUpMoney;
				string vipAllName = WorldSetting.GetVipAllName(vipLevel + 1);
				NextLevelDistanceText.text = $"再充值{num}元，升级到{vipAllName}";
				ProgressValueImg.fillAmount = (float)Singleton<RoleManager>.Instance.Role_Entity.TopUpMoney * 1f / (float)value.TopPrice * 1f;
			}
			else if (vipLevel >= 12)
			{
				NextLevelDistanceText.text = $"您已达到最高等级";
				ProgressValueImg.fillAmount = 1f;
			}
		}

		private void LingYuGouMai(int id)
		{
			switch (id)
			{
			}
		}

		private void ShowVipLogo()
		{
			switch (Singleton<RoleManager>.Instance.Role_Entity.VipLevel)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				VipLogo_1_3.gameObject.SetActive(value: true);
				VipLogo_4_6.gameObject.SetActive(value: false);
				VipLogo_7_9.gameObject.SetActive(value: false);
				VipLogo_10_11.gameObject.SetActive(value: false);
				VipLogo_12.gameObject.SetActive(value: false);
				break;
			case 4:
			case 5:
			case 6:
				VipLogo_1_3.gameObject.SetActive(value: false);
				VipLogo_4_6.gameObject.SetActive(value: true);
				VipLogo_7_9.gameObject.SetActive(value: false);
				VipLogo_10_11.gameObject.SetActive(value: false);
				VipLogo_12.gameObject.SetActive(value: false);
				break;
			case 7:
			case 8:
			case 9:
				VipLogo_1_3.gameObject.SetActive(value: false);
				VipLogo_4_6.gameObject.SetActive(value: false);
				VipLogo_7_9.gameObject.SetActive(value: true);
				VipLogo_10_11.gameObject.SetActive(value: false);
				VipLogo_12.gameObject.SetActive(value: false);
				break;
			case 10:
			case 11:
				VipLogo_1_3.gameObject.SetActive(value: false);
				VipLogo_4_6.gameObject.SetActive(value: false);
				VipLogo_7_9.gameObject.SetActive(value: false);
				VipLogo_10_11.gameObject.SetActive(value: true);
				VipLogo_12.gameObject.SetActive(value: false);
				break;
			case 12:
				VipLogo_1_3.gameObject.SetActive(value: false);
				VipLogo_4_6.gameObject.SetActive(value: false);
				VipLogo_7_9.gameObject.SetActive(value: false);
				VipLogo_10_11.gameObject.SetActive(value: false);
				VipLogo_12.gameObject.SetActive(value: true);
				break;
			}
		}

		public void ClosePanel()
		{
			VipPowerPanel.ClosePanel();
		}
	}
}
