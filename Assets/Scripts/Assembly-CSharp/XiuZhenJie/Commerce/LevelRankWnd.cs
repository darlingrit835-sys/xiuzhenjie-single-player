using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class LevelRankWnd : Window
	{
		public LevelRankPanel Panel;

		private List<RankListTypeEnum> m_AllRankList;

		private int m_CurrentListIndex;

		private Vector2 m_HideAnPos;

		private Vector2 m_ShowAnPos;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LevelRankPanel>();
			m_HideAnPos = new Vector2(0f, -1000f);
			m_ShowAnPos = new Vector2(0f, 217.24f);
			m_AllRankList = new List<RankListTypeEnum>
			{
				RankListTypeEnum.SaiJi,
				RankListTypeEnum.DaoJing,
				RankListTypeEnum.TianJiao,
				RankListTypeEnum.Level,
				RankListTypeEnum.ZongMen,
				RankListTypeEnum.LianDan,
				RankListTypeEnum.LianQi,
				RankListTypeEnum.ZhiFu,
				RankListTypeEnum.FaShu,
				RankListTypeEnum.BuDao,
				RankListTypeEnum.LingZhi,
				RankListTypeEnum.ZhenTu
			};
			m_CurrentListIndex = 1;
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.MoBaiBtn_1, Mobai_1BtnClick);
			AddButtonClickListener(Panel.MoBaiBtn_2, Mobai_2BtnClick);
			AddButtonClickListener(Panel.MoBaiBtn_3, Mobai_3BtnClick);
			AddButtonClickListener(Panel.NextPanelBtn, NextPanelAction);
			AddButtonClickListener(Panel.LastPanelBtn, LastPanelAction);
			CreateData(m_CurrentListIndex);
		}

		private void CreateData(int targetIndex)
		{
			if (targetIndex == 0)
			{
				return;
			}
			if (targetIndex == 1)
			{
				GetDaoJingBangRankList();
				return;
			}
			if (targetIndex == 2)
			{
				GetTianJiaoRankList();
				return;
			}
			RealmRankedReq_1002024 data = new RealmRankedReq_1002024
			{
				Type = (int)m_AllRankList[targetIndex]
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REAL_RANK, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RealmRankedResp_1002024 realmRankedResp_ = new RealmRankedResp_1002024();
					realmRankedResp_.MergeFrom(byteString);
					if (realmRankedResp_.IsSuccess)
					{
						m_CurrentListIndex = targetIndex;
						if (m_AllRankList[m_CurrentListIndex] == RankListTypeEnum.Level)
						{
							SetLevelRankList(realmRankedResp_);
						}
						else if (m_AllRankList[m_CurrentListIndex] == RankListTypeEnum.ZongMen)
						{
							SetZongMenRankList(realmRankedResp_);
						}
						else
						{
							SetFuZhiRankList(realmRankedResp_);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(realmRankedResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetLevelRankList(RealmRankedResp_1002024 realmRankedResp_1002024)
		{
			if (realmRankedResp_1002024.MyRanked != 0)
			{
				Panel.MyRankText.text = ((realmRankedResp_1002024.MyRanked > 999) ? "999+" : realmRankedResp_1002024.MyRanked.ToString());
				Panel.MyNameText.text = Singleton<RoleManager>.Instance.Role_Entity.RoleName;
				Panel.LevelText.text = WorldSetting.GetRoleLevelNameByRoleLevel(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel);
				Panel.HotValueText.text = realmRankedResp_1002024.MyHeat.ToString();
			}
			for (int i = 0; i < Panel.RankList.Count; i++)
			{
				Panel.RankList[i].ClearItem();
			}
			for (int j = 0; j < realmRankedResp_1002024.Object.Count; j++)
			{
				Panel.RankList[j].SetItem(realmRankedResp_1002024.Object[j].Id, realmRankedResp_1002024.Object[j].Name.ToString(), WorldSetting.GetRoleLevelNameByRoleLevel(realmRankedResp_1002024.Object[j].Level), realmRankedResp_1002024.Object[j].Heat.ToString());
			}
			Panel.NextPanelBtn.gameObject.SetActive(value: true);
			Panel.LastPanelBtn.gameObject.SetActive(value: true);
			Panel.ZM_Panel.anchoredPosition = m_HideAnPos;
			Panel.DJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.TJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.Fz_Panel.anchoredPosition = m_HideAnPos;
			Panel.SJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.Level_Panel.anchoredPosition = m_ShowAnPos;
		}

		private void SetFuZhiRankList(RealmRankedResp_1002024 realmRankedResp_1002024)
		{
			if (realmRankedResp_1002024.MyRanked > 0)
			{
				Panel.Fz_MyRankText.text = ((realmRankedResp_1002024.MyRanked > 999) ? "999+" : realmRankedResp_1002024.MyRanked.ToString());
				Panel.Fz_MyNameText.text = Singleton<RoleManager>.Instance.Role_Entity.RoleName;
				Panel.Fz_LevelText.text = WorldSetting.GetRoleLevelNameByRoleLevel(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel);
				Panel.Fz_HotValueText.text = realmRankedResp_1002024.MyHeat.ToString();
			}
			else
			{
				Panel.Fz_MyRankText.text = string.Empty;
				Panel.Fz_MyNameText.text = string.Empty;
				Panel.Fz_LevelText.text = string.Empty;
				Panel.Fz_HotValueText.text = string.Empty;
			}
			for (int i = 0; i < Panel.Fz_RankList.Count; i++)
			{
				Panel.Fz_RankList[i].ClearItem();
			}
			for (int j = 0; j < realmRankedResp_1002024.Object.Count; j++)
			{
				string level = WorldSetting.GetChineseNumber(realmRankedResp_1002024.Object[j].Level) + "阶" + GetFuZhiLevelName((int)m_AllRankList[m_CurrentListIndex]);
				Panel.Fz_RankList[j].SetItem(realmRankedResp_1002024.Object[j].Id, realmRankedResp_1002024.Object[j].Name.ToString(), level, realmRankedResp_1002024.Object[j].Heat.ToString());
			}
			if (Panel.Fz_Panel.anchoredPosition.y < 0f)
			{
				Panel.ZM_Panel.anchoredPosition = m_HideAnPos;
				Panel.DJ_Panel.anchoredPosition = m_HideAnPos;
				Panel.TJ_Panel.anchoredPosition = m_HideAnPos;
				Panel.SJ_Panel.anchoredPosition = m_HideAnPos;
				Panel.Level_Panel.anchoredPosition = m_HideAnPos;
				Panel.Fz_Panel.anchoredPosition = m_ShowAnPos;
			}
			if (m_CurrentListIndex >= m_AllRankList.Count - 1)
			{
				Panel.NextPanelBtn.gameObject.SetActive(value: false);
				Panel.LastPanelBtn.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.NextPanelBtn.gameObject.SetActive(value: true);
				Panel.LastPanelBtn.gameObject.SetActive(value: true);
			}
			SetFuZhiTitleName();
		}

		private void SetZongMenRankList(RealmRankedResp_1002024 realmRankedResp_1002024)
		{
			if (realmRankedResp_1002024.MyRanked > 0)
			{
				Panel.ZM_MyRankText.text = ((realmRankedResp_1002024.MyRanked > 999) ? "999+" : realmRankedResp_1002024.MyRanked.ToString());
				Panel.ZM_MyNameText.text = Singleton<RoleManager>.Instance.Role_Entity.ZongMenName;
				Panel.ZM_LevelText.text = WorldSetting.GetStageName(realmRankedResp_1002024.MyStage) + "宗门";
				Panel.ZM_HotValueText.text = realmRankedResp_1002024.MyHeat.ToString();
			}
			else
			{
				Panel.ZM_MyRankText.text = string.Empty;
				Panel.ZM_MyNameText.text = string.Empty;
				Panel.ZM_LevelText.text = string.Empty;
				Panel.ZM_HotValueText.text = string.Empty;
			}
			for (int i = 0; i < Panel.ZM_RankList.Count; i++)
			{
				Panel.ZM_RankList[i].ClearItem();
			}
			for (int j = 0; j < realmRankedResp_1002024.Object.Count; j++)
			{
				string level = WorldSetting.GetChineseNumber(realmRankedResp_1002024.Object[j].Level) + "阶宗门";
				Panel.ZM_RankList[j].SetItem(realmRankedResp_1002024.Object[j].Id, realmRankedResp_1002024.Object[j].Name.ToString(), level, realmRankedResp_1002024.Object[j].Heat.ToString());
			}
			Panel.NextPanelBtn.gameObject.SetActive(value: true);
			Panel.LastPanelBtn.gameObject.SetActive(value: true);
			Panel.SJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.TJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.DJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.Fz_Panel.anchoredPosition = m_HideAnPos;
			Panel.Level_Panel.anchoredPosition = m_HideAnPos;
			Panel.ZM_Panel.anchoredPosition = m_ShowAnPos;
		}

		private void GetTianJiaoRankList()
		{
			EnterRushTowerAgeRankedReq_1008004 data = new EnterRushTowerAgeRankedReq_1008004
			{
				Stage = 1
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_XST_TIANJIAOBANG, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterRushTowerAgeRankedResp_1008004 enterRushTowerAgeRankedResp_ = new EnterRushTowerAgeRankedResp_1008004();
					enterRushTowerAgeRankedResp_.MergeFrom(byteString);
					if (enterRushTowerAgeRankedResp_.IsSuccess)
					{
						SetTianJiaoRankList(enterRushTowerAgeRankedResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterRushTowerAgeRankedResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetTianJiaoRankList(EnterRushTowerAgeRankedResp_1008004 enterRushTowerAgeRankedResp_1008004)
		{
			if (enterRushTowerAgeRankedResp_1008004.MyRanked > 0)
			{
				Panel.TJ_MyRankText.text = ((enterRushTowerAgeRankedResp_1008004.MyRanked > 999) ? "999+" : enterRushTowerAgeRankedResp_1008004.MyRanked.ToString());
				Panel.TJ_MyNameText.text = Singleton<RoleManager>.Instance.Role_Entity.RoleName;
				Panel.TJ_LevelText.text = Singleton<RoleManager>.Instance.Role_Entity.ZongMenName.ToString();
				Panel.TJ_HotValueText.text = enterRushTowerAgeRankedResp_1008004.Object[enterRushTowerAgeRankedResp_1008004.Object.Count - 1].TierNum.ToString();
				Panel.TJ_AgeValueText.text = enterRushTowerAgeRankedResp_1008004.Object[enterRushTowerAgeRankedResp_1008004.Object.Count - 1].Age.ToString();
			}
			else
			{
				Panel.TJ_MyRankText.text = string.Empty;
				Panel.TJ_MyNameText.text = string.Empty;
				Panel.TJ_LevelText.text = string.Empty;
				Panel.TJ_HotValueText.text = string.Empty;
			}
			for (int i = 0; i < Panel.TJ_RankList.Count; i++)
			{
				Panel.TJ_RankList[i].ClearItem();
			}
			for (int j = 0; j < enterRushTowerAgeRankedResp_1008004.Object.Count; j++)
			{
				string faction = enterRushTowerAgeRankedResp_1008004.Object[j].Faction;
				Panel.TJ_RankList[j].SetItem(enterRushTowerAgeRankedResp_1008004.Object[j].Name.ToString(), faction, enterRushTowerAgeRankedResp_1008004.Object[j].TierNum.ToString(), enterRushTowerAgeRankedResp_1008004.Object[j].Age.ToString());
			}
			Panel.NextPanelBtn.gameObject.SetActive(value: true);
			Panel.LastPanelBtn.gameObject.SetActive(value: true);
			Panel.DJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.TJ_Panel.anchoredPosition = m_ShowAnPos;
			Panel.SJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.ZM_Panel.anchoredPosition = m_HideAnPos;
			Panel.Level_Panel.anchoredPosition = m_HideAnPos;
			Panel.Fz_Panel.anchoredPosition = m_HideAnPos;
		}

		private void GetDaoJingBangRankList()
		{
			EnterRushTowerTierNumRankedReq_1008002 data = new EnterRushTowerTierNumRankedReq_1008002
			{
				Stage = 1
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_XST_DAOJINGBANG, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterRushTowerTierNumRankedResp_1008002 enterRushTowerTierNumRankedResp_ = new EnterRushTowerTierNumRankedResp_1008002();
					enterRushTowerTierNumRankedResp_.MergeFrom(byteString);
					if (enterRushTowerTierNumRankedResp_.IsSuccess)
					{
						SetDaoJingBangRankList(enterRushTowerTierNumRankedResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterRushTowerTierNumRankedResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetDaoJingBangRankList(EnterRushTowerTierNumRankedResp_1008002 enterRushTowerTierNumRankedResp_1008002)
		{
			if (enterRushTowerTierNumRankedResp_1008002.MyRanked > 0)
			{
				Panel.DJ_MyRankText.text = ((enterRushTowerTierNumRankedResp_1008002.MyRanked > 999) ? "999+" : enterRushTowerTierNumRankedResp_1008002.MyRanked.ToString());
				Panel.DJ_MyNameText.text = Singleton<RoleManager>.Instance.Role_Entity.RoleName;
				Panel.DJ_LevelText.text = WorldSetting.GetRoleLevelNameByRoleLevel(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel);
				Panel.DJ_HotValueText.text = Singleton<RoleManager>.Instance.Role_Entity.ZongMenName.ToString();
				Panel.DJ_AgeValueText.text = enterRushTowerTierNumRankedResp_1008002.Object[enterRushTowerTierNumRankedResp_1008002.Object.Count - 1].TierNum.ToString();
			}
			else
			{
				Panel.DJ_MyRankText.text = string.Empty;
				Panel.DJ_MyNameText.text = string.Empty;
				Panel.DJ_LevelText.text = string.Empty;
				Panel.DJ_HotValueText.text = string.Empty;
				Panel.DJ_AgeValueText.text = string.Empty;
				new List<string>(10);
				new Dictionary<int, int>(10);
			}
			for (int i = 0; i < Panel.DJ_RankList.Count; i++)
			{
				Panel.DJ_RankList[i].ClearItem();
			}
			for (int j = 0; j < enterRushTowerTierNumRankedResp_1008002.Object.Count; j++)
			{
				string faction = enterRushTowerTierNumRankedResp_1008002.Object[j].Faction;
				Panel.DJ_RankList[j].SetItem(enterRushTowerTierNumRankedResp_1008002.Object[j].Name.ToString(), WorldSetting.GetRoleLevelNameByRoleLevel(enterRushTowerTierNumRankedResp_1008002.Object[j].Level), faction, enterRushTowerTierNumRankedResp_1008002.Object[j].TierNum.ToString());
			}
			Panel.NextPanelBtn.gameObject.SetActive(value: true);
			Panel.LastPanelBtn.gameObject.SetActive(value: false);
			Panel.DJ_Panel.anchoredPosition = m_ShowAnPos;
			Panel.TJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.SJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.ZM_Panel.anchoredPosition = m_HideAnPos;
			Panel.Level_Panel.anchoredPosition = m_HideAnPos;
			Panel.Fz_Panel.anchoredPosition = m_HideAnPos;
		}

		private void SetSaiJiShuoMing()
		{
			Panel.SJ_Panel.anchoredPosition = m_ShowAnPos;
			Panel.NextPanelBtn.gameObject.SetActive(value: true);
			Panel.LastPanelBtn.gameObject.SetActive(value: false);
			Panel.DJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.TJ_Panel.anchoredPosition = m_HideAnPos;
			Panel.ZM_Panel.anchoredPosition = m_HideAnPos;
			Panel.Fz_Panel.anchoredPosition = m_HideAnPos;
			Panel.Level_Panel.anchoredPosition = m_HideAnPos;
		}

		private void SetFuZhiTitleName()
		{
			if (m_CurrentListIndex >= 1)
			{
				switch (m_AllRankList[m_CurrentListIndex])
				{
				case RankListTypeEnum.LianDan:
					Panel.Fz_TitleText.text = "炼丹排行";
					break;
				case RankListTypeEnum.LianQi:
					Panel.Fz_TitleText.text = "炼器排行";
					break;
				case RankListTypeEnum.ZhiFu:
					Panel.Fz_TitleText.text = "制符排行";
					break;
				case RankListTypeEnum.FaShu:
					Panel.Fz_TitleText.text = "术法排行";
					break;
				case RankListTypeEnum.ZhenTu:
					Panel.Fz_TitleText.text = "阵法排行";
					break;
				case RankListTypeEnum.BuDao:
					Panel.Fz_TitleText.text = "布道排行";
					break;
				case RankListTypeEnum.LingZhi:
					Panel.Fz_TitleText.text = "灵植排行";
					break;
				case RankListTypeEnum.QiaoJiang:
				case RankListTypeEnum.YuShou:
					break;
				}
			}
		}

		private string GetFuZhiLevelName(int type)
		{
			return m_AllRankList[m_CurrentListIndex] switch
			{
				RankListTypeEnum.LianDan => "丹师", 
				RankListTypeEnum.LianQi => "器师", 
				RankListTypeEnum.ZhiFu => "符师", 
				RankListTypeEnum.FaShu => "术师", 
				RankListTypeEnum.ZhenTu => "阵法师", 
				RankListTypeEnum.BuDao => "布道师", 
				RankListTypeEnum.QiaoJiang => "巧匠", 
				RankListTypeEnum.LingZhi => "灵植师", 
				RankListTypeEnum.YuShou => "兽王", 
				RankListTypeEnum.ZongMen => "宗门", 
				_ => "", 
			};
		}

		private void Mobai_1BtnClick()
		{
			MoBai(0, Panel.RankList[0].RoleId);
		}

		private void Mobai_2BtnClick()
		{
			MoBai(1, Panel.RankList[1].RoleId);
		}

		private void Mobai_3BtnClick()
		{
			MoBai(2, Panel.RankList[2].RoleId);
		}

		private void MoBai(int index, string roleId)
		{
			if (string.IsNullOrEmpty(roleId))
			{
				return;
			}
			MoBaiReq_1002026 moBaiReq_ = new MoBaiReq_1002026();
			moBaiReq_.RoleId = roleId;
			Singleton<NetManager>.Instance.SendMsg(MsgType.MO_BAI, moBaiReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MoBaiResp_1002026 moBaiResp_ = new MoBaiResp_1002026();
					moBaiResp_.MergeFrom(byteString);
					if (moBaiResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = moBaiResp_.ShinyJade;
						Panel.RankList[index].AddHotValue();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(moBaiResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void NextPanelAction()
		{
			if (m_CurrentListIndex < m_AllRankList.Count - 1)
			{
				CreateData(++m_CurrentListIndex);
			}
		}

		private void LastPanelAction()
		{
			if (m_CurrentListIndex > 0)
			{
				CreateData(--m_CurrentListIndex);
			}
		}
	}
}
