using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class VipPowerPanel : MonoBehaviour
	{
		public Button BackLastBtn;

		public Button GoNextBtn;

		public Image VipLogo_1_3;

		public Image VipLogo_4_6;

		public Image VipLogo_7_9;

		public Image VipLogo_10_11;

		public Image VipLogo_12;

		public Image ProgressValueImg;

		public Text VipLevelText;

		public Text NextLevelDistanceText;

		public Button HelpBtn;

		public Button LiJiChongZhiBtn;

		public RectTransform EffectContent;

		private GameObject m_VipPowerEffectItem;

		private GameObject m_LiBaoObjItem;

		public Text LiBaoTitleText;

		public RectTransform LiBaoGoodsContent;

		public Button LingQuBtn;

		public Text LingQuBtnText;

		private int m_CurrentVipLevel;

		private int m_CurrentPanelLevel;

		private int m_TopUpMoney;

		private Dictionary<int, VipInfoEntity> m_VipInfoDic;

		private CashGoodsWnd m_CashGoodsWnd;

		private Dictionary<int, Sprite> m_GoodsTypeSpriteDic;

		private Dictionary<int, Sprite> m_GoodsRankSpriteDic;

		private Dictionary<int, bool> m_LingQuStatusDic;

		public void Init(CashGoodsWnd cashGoodsWnd)
		{
			m_CashGoodsWnd = cashGoodsWnd;
			m_LingQuStatusDic = new Dictionary<int, bool>();
			m_VipPowerEffectItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/VipPowerEffectItem.prefab");
			m_LiBaoObjItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_GoodsTypeSpriteDic = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_GoodsRankSpriteDic = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			m_VipInfoDic = Singleton<FacadeManager>.Instance.SelectVipInfo();
			ShowNextAndLastButton(Singleton<RoleManager>.Instance.Role_Entity.VipLevel);
		}

		public void ShowPanel()
		{
			SetVipName(m_CurrentPanelLevel);
			ShowVipLogo(m_CurrentPanelLevel);
			CreateVipEffect(m_CurrentPanelLevel);
			CreateLiBaoGoods(m_CurrentPanelLevel);
			SetNextLevelDistanceInfo(m_CurrentPanelLevel);
			ShowNextAndLastButton(m_CurrentPanelLevel);
			AddBtnListener();
			SetLingQuBtnStatus();
		}

		private void SetLingQuBtnStatus()
		{
			if (m_CurrentPanelLevel == 0)
			{
				LingQuBtn.interactable = false;
				LingQuBtnText.text = "领取";
				return;
			}
			bool flag = m_LingQuStatusDic[m_CurrentPanelLevel];
			if (m_CurrentVipLevel < m_CurrentPanelLevel)
			{
				LingQuBtn.interactable = false;
				LingQuBtnText.text = "领取";
			}
			else if (!flag)
			{
				LingQuBtn.interactable = true;
				LingQuBtnText.text = "领取";
			}
			else
			{
				LingQuBtn.interactable = false;
				LingQuBtnText.text = "已领取";
			}
		}

		public void GetLingQuStatus()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_VIP, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VipLevelResp_1002029 vipLevelResp_ = new VipLevelResp_1002029();
					vipLevelResp_.MergeFrom(byteString);
					if (vipLevelResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.VipLevel = vipLevelResp_.Level;
						Singleton<RoleManager>.Instance.Role_Entity.TopUpMoney = vipLevelResp_.PayAll;
						m_CurrentVipLevel = Singleton<RoleManager>.Instance.Role_Entity.VipLevel;
						m_TopUpMoney = Singleton<RoleManager>.Instance.Role_Entity.TopUpMoney;
						m_CurrentPanelLevel = m_CurrentVipLevel;
						m_LingQuStatusDic.Clear();
						m_LingQuStatusDic.Add(1, vipLevelResp_.V1IsGeted);
						m_LingQuStatusDic.Add(2, vipLevelResp_.V2IsGeted);
						m_LingQuStatusDic.Add(3, vipLevelResp_.V3IsGeted);
						m_LingQuStatusDic.Add(4, vipLevelResp_.V4IsGeted);
						m_LingQuStatusDic.Add(5, vipLevelResp_.V5IsGeted);
						m_LingQuStatusDic.Add(6, vipLevelResp_.V6IsGeted);
						m_LingQuStatusDic.Add(7, vipLevelResp_.V7IsGeted);
						m_LingQuStatusDic.Add(8, vipLevelResp_.V8IsGeted);
						m_LingQuStatusDic.Add(9, vipLevelResp_.V9IsGeted);
						m_LingQuStatusDic.Add(10, vipLevelResp_.V10IsGeted);
						m_LingQuStatusDic.Add(11, vipLevelResp_.V11IsGeted);
						m_LingQuStatusDic.Add(12, vipLevelResp_.V12IsGeted);
						ShowPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(vipLevelResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetVipName(int level)
		{
			if (level == m_CurrentVipLevel)
			{
				VipLevelText.text = WorldSetting.GetVipAllName(level) + " 【当前】";
			}
			else
			{
				VipLevelText.text = WorldSetting.GetVipAllName(level);
			}
		}

		private void SetNextLevelDistanceInfo(int targetLevel)
		{
			if (targetLevel < m_CurrentVipLevel)
			{
				ProgressValueImg.fillAmount = 1f;
				NextLevelDistanceText.gameObject.SetActive(value: false);
				return;
			}
			NextLevelDistanceText.gameObject.SetActive(value: true);
			VipInfoEntity value = null;
			if (targetLevel == m_CurrentVipLevel)
			{
				targetLevel++;
			}
			if (m_VipInfoDic.TryGetValue(targetLevel - 1, out value))
			{
				int num = value.TopPrice - Singleton<RoleManager>.Instance.Role_Entity.TopUpMoney;
				string vipAllName = WorldSetting.GetVipAllName(targetLevel);
				NextLevelDistanceText.text = $"再充值{num}元，升级到{vipAllName}";
				ProgressValueImg.fillAmount = (float)Singleton<RoleManager>.Instance.Role_Entity.TopUpMoney * 1f / (float)value.TopPrice * 1f;
			}
		}

		private void ShowVipLogo(int level)
		{
			switch (level)
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

		private void CreateVipEffect(int targetLevel)
		{
			VipInfoEntity value = null;
			if (!m_VipInfoDic.TryGetValue(targetLevel, out value))
			{
				return;
			}
			ClearEffectContent();
			foreach (string item in value.Effect)
			{
				if (!string.IsNullOrEmpty(item))
				{
					CreateVipEffectItem(item);
				}
			}
		}

		private void CreateVipEffectItem(string effectStr)
		{
			GameObject obj = m_CashGoodsWnd.SetListContentParent(EffectContent, m_VipPowerEffectItem);
			obj.SetActive(value: true);
			obj.transform.Find("Text").gameObject.GetComponent<Text>().text = effectStr.TrimEnd('\r', '\n');
		}

		private void ClearEffectContent()
		{
			foreach (Transform item in EffectContent)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void CreateLiBaoGoods(int targetLevel)
		{
			LiBaoTitleText.text = $"{WorldSetting.GetVipAllName(targetLevel)}特权礼包";
			VipInfoEntity value = null;
			if (!m_VipInfoDic.TryGetValue(targetLevel, out value))
			{
				return;
			}
			ClearLiBaoContent();
			foreach (VipLiBaoGoodsInfoEntity item in value.LiBao)
			{
				CreateLiBaoGoodsItem(item);
			}
		}

		private void CreateLiBaoGoodsItem(VipLiBaoGoodsInfoEntity data)
		{
			UI_GoodsItem component = m_CashGoodsWnd.SetListContentParent(LiBaoGoodsContent, m_LiBaoObjItem).GetComponent<UI_GoodsItem>();
			Sprite value = null;
			Sprite value2 = null;
			m_GoodsTypeSpriteDic.TryGetValue(data.GoodsType, out value);
			m_GoodsRankSpriteDic.TryGetValue(WorldSetting.GetLevelByRank(data.Rank), out value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, LiBaoGoodsClick, isUsing: false, data.Num);
		}

		private void LiBaoGoodsClick(object obj, GameObject gameObj)
		{
		}

		private void ClearLiBaoContent()
		{
			foreach (Transform item in LiBaoGoodsContent)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void AddBtnListener()
		{
			HelpBtn.onClick.RemoveAllListeners();
			LiJiChongZhiBtn.onClick.RemoveAllListeners();
			LingQuBtn.onClick.RemoveAllListeners();
			BackLastBtn.onClick.RemoveAllListeners();
			GoNextBtn.onClick.RemoveAllListeners();
			HelpBtn.onClick.AddListener(HelpChongZhi);
			LiJiChongZhiBtn.onClick.AddListener(LiJiChongZhi);
			LingQuBtn.onClick.AddListener(LingQuLiBao);
			BackLastBtn.onClick.AddListener(LastVipLevel);
			GoNextBtn.onClick.AddListener(NextVipLevel);
		}

		public void NextVipLevel()
		{
			if (m_CurrentPanelLevel != 12)
			{
				m_CurrentPanelLevel++;
				ShowNextAndLastButton(m_CurrentPanelLevel);
				ShowPanel();
			}
		}

		public void LastVipLevel()
		{
			if (m_CurrentPanelLevel != 0)
			{
				m_CurrentPanelLevel--;
				ShowNextAndLastButton(m_CurrentPanelLevel);
				ShowPanel();
			}
		}

		private void ShowNextAndLastButton(int targetLevel)
		{
			if (targetLevel <= 0)
			{
				BackLastBtn.gameObject.SetActive(value: false);
				GoNextBtn.gameObject.SetActive(value: true);
			}
			else if (targetLevel >= 12)
			{
				BackLastBtn.gameObject.SetActive(value: true);
				GoNextBtn.gameObject.SetActive(value: false);
			}
			else
			{
				BackLastBtn.gameObject.SetActive(value: true);
				GoNextBtn.gameObject.SetActive(value: true);
			}
		}

		public void HelpChongZhi()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("请通过QQ联系客服人员，QQ号：1143049162");
		}

		public void LiJiChongZhi()
		{
			base.gameObject.SetActive(value: false);
		}

		public void LingQuLiBao()
		{
			int num = 0;
			switch (m_CurrentPanelLevel)
			{
			case 1:
				num = 13;
				break;
			case 2:
				num = 14;
				break;
			case 3:
				num = 15;
				break;
			case 4:
				num = 16;
				break;
			case 5:
				num = 17;
				break;
			case 6:
				num = 18;
				break;
			case 7:
				num = 19;
				break;
			case 8:
				num = 20;
				break;
			case 9:
				num = 21;
				break;
			case 10:
				num = 22;
				break;
			case 11:
				num = 23;
				break;
			case 12:
				num = 24;
				break;
			}
			if (num != 0)
			{
				GouMaiLingYuLiBao(num);
			}
		}

		private void GouMaiLingYuLiBao(int type)
		{
			BuyLiBaoReq_1001018 data = new BuyLiBaoReq_1001018
			{
				Type = type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BuyLiBao, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuyLiBaoResp_1001018 buyLiBaoResp_ = new BuyLiBaoResp_1001018();
					buyLiBaoResp_.MergeFrom(byteString);
					if (buyLiBaoResp_.IsSuccess)
					{
						if (m_LingQuStatusDic.ContainsKey(m_CurrentPanelLevel))
						{
							m_LingQuStatusDic[m_CurrentPanelLevel] = true;
						}
						LingQuBtn.interactable = false;
						LingQuBtnText.text = "已领取";
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buyLiBaoResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public void ClosePanel()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
