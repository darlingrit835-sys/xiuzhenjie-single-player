using System;
using System.Collections.Generic;
using Google.Protobuf;
using LitJson;
using UnityEngine;

namespace XiuZhenJie
{
	public class XuShenTaBangDanWnd : Window
	{
		public XuShenTaBangDanPanel Panel;

		private GameObject m_BangDanItemObj;

		private List<XuShenTaBangDanItem> m_DataSourceList;

		private XuShenTaLevelModel m_DataSource;

		private Dictionary<string, RoleLevelModel> m_RoleInFoDic;

		private MsgType m_MsgType;

		private string m_BangDangType;

		private int m_CurrentStage;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_BangDanItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuShenTaBangDanItem.prefab");
			m_DataSourceList = new List<XuShenTaBangDanItem>();
			Panel = base.GameObject.GetComponent<XuShenTaBangDanPanel>();
			AddButtonClickListener(Panel.JiangLiPanel_QueRenBtn, JiangLiPanel_QueRenBtnClick);
			AddButtonClickListener(Panel.LingQuBtn, LingQuBtnClick);
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddToggleClickListener(Panel.JieDan, JieDanTogClick);
			AddToggleClickListener(Panel.YuanYing, YuanYinTogClick);
			AddToggleClickListener(Panel.HuaShen, HuaShenTogClick);
			AddToggleClickListener(Panel.LianXu, LianXuTogClick);
			AddToggleClickListener(Panel.HeTi, HeTiTogClick);
			AddToggleClickListener(Panel.DuJie, DuJieTogClick);
			AddToggleClickListener(Panel.DaCheng, DaChengTogClick);
			AddButtonClickListener(Panel.WenHao_Btn, WenHaoTogClick);
			m_RoleInFoDic = Singleton<FacadeManager>.Instance.SelectRoleLevelInfo();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 1)
			{
				m_BangDangType = paralist[0].ToString();
			}
			if (m_BangDangType == "role_level")
			{
				Panel.BangDanNameText.text = "道境榜";
				m_MsgType = MsgType.XSJ_XST_DAOJINGBANG;
			}
			else if (m_BangDangType == "role_age")
			{
				Panel.BangDanNameText.text = "天骄榜";
				m_MsgType = MsgType.XSJ_XST_TIANJIAOBANG;
			}
			else
			{
				Panel.BangDanNameText.text = "天骄榜";
				m_MsgType = MsgType.XSJ_XST_TIANJIAOBANG;
			}
			JieDanTogClick(isOn: true);
		}

		private void GetNetMsg()
		{
			JsonMapper.ToJson(new XuShenTaParamsModel
			{
				limit = 10,
				stage = m_CurrentStage
			});
			if (m_MsgType == MsgType.XSJ_XST_DAOJINGBANG)
			{
				EnterRushTowerTierNumRankedReq_1008002 data = new EnterRushTowerTierNumRankedReq_1008002
				{
					Stage = m_CurrentStage
				};
				Singleton<NetManager>.Instance.SendMsg(m_MsgType, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						EnterRushTowerTierNumRankedResp_1008002 enterRushTowerTierNumRankedResp_ = new EnterRushTowerTierNumRankedResp_1008002();
						enterRushTowerTierNumRankedResp_.MergeFrom(byteString);
						if (enterRushTowerTierNumRankedResp_.IsSuccess)
						{
							m_DataSource = new XuShenTaLevelModel(enterRushTowerTierNumRankedResp_);
							if (m_DataSource.number == 0)
							{
								Panel.PaiMingText.text = "未入榜单";
							}
							else
							{
								Panel.PaiMingText.text = "第" + m_DataSource.number + "名";
							}
							CreateBangDanItem();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterRushTowerTierNumRankedResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
			else
			{
				if (m_MsgType != MsgType.XSJ_XST_TIANJIAOBANG)
				{
					return;
				}
				EnterRushTowerAgeRankedReq_1008004 data2 = new EnterRushTowerAgeRankedReq_1008004
				{
					Stage = m_CurrentStage
				};
				Singleton<NetManager>.Instance.SendMsg(m_MsgType, data2, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						EnterRushTowerAgeRankedResp_1008004 enterRushTowerAgeRankedResp_ = new EnterRushTowerAgeRankedResp_1008004();
						enterRushTowerAgeRankedResp_.MergeFrom(byteString);
						if (enterRushTowerAgeRankedResp_.IsSuccess)
						{
							m_DataSource = new XuShenTaLevelModel(enterRushTowerAgeRankedResp_);
							if (m_DataSource.number == 0)
							{
								Panel.PaiMingText.text = "未入榜单";
							}
							else
							{
								Panel.PaiMingText.text = "第" + m_DataSource.number + "名";
							}
							CreateBangDanItem();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterRushTowerAgeRankedResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
		}

		private void SetItemModelValue(XuShenTaBangDanItem item, int i)
		{
			XuShenTaBangDanRankItemModel xuShenTaBangDanRankItemModel = null;
			if (m_DataSource != null && m_DataSource.list != null && m_DataSource.list.Count > i)
			{
				xuShenTaBangDanRankItemModel = m_DataSource.list[i];
			}
			if (xuShenTaBangDanRankItemModel != null)
			{
				if (m_BangDangType == "role_level")
				{
					RoleLevelModel value = null;
					m_RoleInFoDic.TryGetValue(xuShenTaBangDanRankItemModel.roleLevel.ToString(), out value);
					if (value != null)
					{
						item.SetValue(xuShenTaBangDanRankItemModel.roleName, value.level, xuShenTaBangDanRankItemModel.factionName, xuShenTaBangDanRankItemModel.floorNumber + "层");
					}
					else
					{
						item.SetValue(xuShenTaBangDanRankItemModel.roleName, xuShenTaBangDanRankItemModel.roleLevel.ToString(), xuShenTaBangDanRankItemModel.factionName, xuShenTaBangDanRankItemModel.floorNumber + "层");
					}
				}
				else if (m_BangDangType == "role_age")
				{
					item.SetValue(xuShenTaBangDanRankItemModel.roleName, xuShenTaBangDanRankItemModel.roleAge + "岁", xuShenTaBangDanRankItemModel.factionName, xuShenTaBangDanRankItemModel.floorNumber + "层");
				}
			}
			else
			{
				item.SetValue("无", "无", "无", "无");
			}
		}

		private void CreateBangDanItem()
		{
			if (m_DataSourceList.Count == 0)
			{
				if (m_BangDanItemObj == null)
				{
					m_BangDanItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuShenTaBangDanItem.prefab");
				}
				for (int i = 0; i < 10; i++)
				{
					XuShenTaBangDanItem component = SetListContentParent(Panel.Content, m_BangDanItemObj).gameObject.GetComponent<XuShenTaBangDanItem>();
					SetItemModelValue(component, i);
					m_DataSourceList.Add(component);
				}
			}
			else if (m_DataSourceList.Count == 10)
			{
				for (int j = 0; j < 10; j++)
				{
					SetItemModelValue(m_DataSourceList[j], j);
				}
			}
		}

		private void JieDanTogClick(bool isOn)
		{
			ChangeText(Panel.JieDanChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 3;
				GetNetMsg();
			}
		}

		private void YuanYinTogClick(bool isOn)
		{
			ChangeText(Panel.YuanYinChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 4;
				GetNetMsg();
			}
		}

		private void HuaShenTogClick(bool isOn)
		{
			ChangeText(Panel.HuaShenChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 5;
				GetNetMsg();
			}
		}

		private void LianXuTogClick(bool isOn)
		{
			ChangeText(Panel.LianXuChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 6;
				GetNetMsg();
			}
		}

		private void HeTiTogClick(bool isOn)
		{
			ChangeText(Panel.HeTiChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 7;
				GetNetMsg();
			}
		}

		private void DuJieTogClick(bool isOn)
		{
			ChangeText(Panel.DuJieChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 8;
				GetNetMsg();
			}
		}

		private void DaChengTogClick(bool isOn)
		{
			ChangeText(Panel.DaChengChooseText.gameObject, isOn);
			if (isOn)
			{
				m_CurrentStage = 9;
				GetNetMsg();
			}
		}

		private void WenHaoTogClick()
		{
			if (m_BangDangType == "role_level")
			{
				string text = $"道境榜按照所有同一大境界中，闯过塔层最高进行排序，每个大境界的排行记录都会永久存在。无数修士在无数岁月中创造的神话，都会被记录在册。初始塔层在修士每提升一个大境界后会提升四十层，即结丹期初始在一层，元婴期初始在四十一层，依次类推。道境榜每周日23时至24时结算，结算奖励以邮件形式发放至各精英修士手中。";
				Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "道境榜");
			}
			else if (m_BangDangType == "role_age")
			{
				string text2 = $"天骄榜是所有天才修士的闯塔记录，每四十层为一个界限，越年轻闯过则排名越靠前。修真界的历史中，天骄修士层出不穷，一代代天骄入榜，又被一代代天骄超越。正是如此，修真界才欣欣向荣。天骄榜每周日23时至24时结算，结算奖励以邮件形式发放至各天骄手中。";
				Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text2, "天骄榜");
			}
		}

		private void ChangeText(GameObject obj, bool isOn)
		{
			obj.SetActive(isOn);
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void LingQuBtnClick()
		{
			if (m_MsgType == MsgType.XSJ_XST_DAOJINGBANG)
			{
				RushTowerTierNumRankedAwardReq_1008037 data = new RushTowerTierNumRankedAwardReq_1008037
				{
					Stage = m_CurrentStage
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_XST_AWARD_DAOJINGBANG, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						RushTowerTierNumRankedAwardResp_1008037 rushTowerTierNumRankedAwardResp_ = new RushTowerTierNumRankedAwardResp_1008037();
						rushTowerTierNumRankedAwardResp_.MergeFrom(byteString);
						if (rushTowerTierNumRankedAwardResp_.IsSuccess)
						{
							m_DataSource.shinyJade = rushTowerTierNumRankedAwardResp_.ShinyJade;
							ShowAwardXiangQing();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rushTowerTierNumRankedAwardResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
			else
			{
				if (m_MsgType != MsgType.XSJ_XST_TIANJIAOBANG)
				{
					return;
				}
				RushTowerAgeRankedAwardReq_1008038 data2 = new RushTowerAgeRankedAwardReq_1008038
				{
					Stage = m_CurrentStage
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_XST_AWARD_TIANJIAOBANG, data2, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						RushTowerAgeRankedAwardResp_1008038 rushTowerAgeRankedAwardResp_ = new RushTowerAgeRankedAwardResp_1008038();
						rushTowerAgeRankedAwardResp_.MergeFrom(byteString);
						if (rushTowerAgeRankedAwardResp_.IsSuccess)
						{
							m_DataSource.shinyJade = rushTowerAgeRankedAwardResp_.ShinyJade;
							ShowAwardXiangQing();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rushTowerAgeRankedAwardResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
		}

		private void ShowAwardXiangQing()
		{
			Panel.JiangLiPanel.gameObject.SetActive(value: true);
			Panel.JiangLiPanel_LingYu.text = ((m_DataSource == null) ? "0个" : (m_DataSource.shinyJade.ToString("f0") + "个"));
			DateTime now = DateTime.Now;
			DateTime dateTime = now.AddDays(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))).AddDays(7.0);
			Panel.JianLiPanel_Time.text = "发放时间: " + dateTime.ToString("yyyy/MM/dd") + "  00:00";
		}

		private void JiangLiPanel_QueRenBtnClick()
		{
			Panel.JiangLiPanel.gameObject.SetActive(value: false);
		}
	}
}
