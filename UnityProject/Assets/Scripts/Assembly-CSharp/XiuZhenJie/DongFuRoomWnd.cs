using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DongFuRoomWnd : Window
	{
		private const int Other_1_Room_Price = 1000;

		private const int Other_2_Room_Price = 10000;

		private const int Other_3_Room_Price = 100000;

		public DongFuRoomPanel Panel;

		private List<RoleSecondaryOccupationVo> m_DataSource;

		private Dictionary<string, XzjWorldSecondaryOccupation> m_FuZhiDic;

		private int m_CurrentLevel;

		private int m_FuZhiCount;

		private List<RoleFuZhiType> m_OpenedRoomType;

		public override void Awake(params object[] paralist)
		{
			m_FuZhiCount = 0;
			base.Awake(paralist);
			m_OpenedRoomType = new List<RoleFuZhiType>();
			m_DataSource = new List<RoleSecondaryOccupationVo>();
			Panel = base.GameObject.GetComponent<DongFuRoomPanel>();
			AddButtonClickListener(Panel.LiKai_Btn, LiKaiBtn);
			AddButtonClickListener(Panel.MiShiBtn, MiShiBtn);
			AddButtonClickListener(Panel.LianGongFaBtn, LianGongFangBtn);
			AddButtonClickListener(Panel.DiHuoShiBtn, DiHuoGeBtn);
			AddButtonClickListener(Panel.WuDaoTaiBtn, WuDaoTaiBtn);
			AddButtonClickListener(Panel.ShengJi_Btn, ShengJiBtn);
			AddButtonClickListener(Panel.LingTianBtn, LingTianBtn);
			AddButtonClickListener(Panel.KaiPiLingTianBtn, LingTianBtn);
			AddButtonClickListener(Panel.TitleBtn, ChangeDongFuName);
			AddButtonClickListener(Panel.ChongFuMingQueRenZhiFuBtn, QueRenZhiFuBtn);
			AddButtonClickListener(Panel.ChongFuMingQuXiaoZhiFuBtn, CancleZhiFuBtn);
			AddButtonClickListener(Panel.ChongFuMingQuXiaoBtn, CancleChongMingMingBtn);
			AddButtonClickListener(Panel.ChongFuMingQueRenBtn, QueRenChongMingMingBtn);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			m_FuZhiDic = Singleton<FacadeManager>.Instance.SelectFuZhiInfo();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_DONGFU, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterHomeResp_1005001 enterHomeResp_ = new EnterHomeResp_1005001();
					enterHomeResp_.MergeFrom(byteString);
					if (enterHomeResp_.IsSuccess)
					{
						m_CurrentLevel = enterHomeResp_.Rank / 4;
						PanDuanLingTian(0);
						Panel.TitleText.text = enterHomeResp_.Name;
						if (enterHomeResp_.Live1Type != 0)
						{
							m_DataSource.Add(new RoleSecondaryOccupationVo
							{
								i = enterHomeResp_.Live1Type.ToString(),
								l = 1,
								n = m_FuZhiDic[enterHomeResp_.Live1Type.ToString()].name
							});
						}
						if (enterHomeResp_.Live2Type != 0)
						{
							m_DataSource.Add(new RoleSecondaryOccupationVo
							{
								i = enterHomeResp_.Live2Type.ToString(),
								l = 2,
								n = m_FuZhiDic[enterHomeResp_.Live2Type.ToString()].name
							});
						}
						InitRoom();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterHomeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void UpdateDongFuLevel()
		{
			m_CurrentLevel++;
		}

		private void ChangeDongFuName()
		{
			Panel.ChongFuMingQueRenRect.gameObject.SetActive(value: true);
		}

		private void QueRenZhiFuBtn()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.ShinyJade < 5)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵玉不足!");
				return;
			}
			Panel.ChongFuMingQueRenRect.gameObject.SetActive(value: false);
			Panel.ChongFuMingRect.gameObject.SetActive(value: true);
		}

		private void CancleZhiFuBtn()
		{
			Panel.ChongFuMingQueRenRect.gameObject.SetActive(value: false);
		}

		private void QueRenChongMingMingBtn()
		{
			string name = Panel.ChongFuMingInput.text;
			if (string.IsNullOrEmpty(name))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入名称!");
				return;
			}
			RenameHomeReq_1005006 data = new RenameHomeReq_1005006
			{
				NewName = name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RENAME_DONGFU, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RenameHomeResp_1005006 renameHomeResp_ = new RenameHomeResp_1005006();
					renameHomeResp_.MergeFrom(byteString);
					if (renameHomeResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)renameHomeResp_.ShinyJade;
						Panel.TitleText.text = name;
						Panel.ChongFuMingRect.gameObject.SetActive(value: false);
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(renameHomeResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void WenHaoTogClick()
		{
			string text = $"不同品阶洞府，不但占地面积有区别，洞府内的灵气浓郁程度也有巨大的差别。黄阶洞府可以开辟一处专职区域，玄阶可开辟两处。已开辟的专职区域可以选择摧毁重开，但这样也会失去专职称号，需要慎重考虑。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "洞府");
		}

		private void CancleChongMingMingBtn()
		{
			Panel.ChongFuMingRect.gameObject.SetActive(value: false);
		}

		private void InitRoom()
		{
			Panel.Other_1_Room.InitClosed(1, Other_1_KaiPiBtn);
			Panel.Other_2_Room.InitClosed(2, Other_2_KaiPiBtn);
			Panel.Other_3_Room.InitClosed(3, Other_3_KaiPiBtn);
			if (m_DataSource == null || m_DataSource.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < m_DataSource.Count; i++)
			{
				XzjWorldSecondaryOccupation value = null;
				switch (m_DataSource[i].l)
				{
				case 1:
					if (m_FuZhiDic.TryGetValue(m_DataSource[i].i, out value))
					{
						Panel.Other_1_Room.OpenLock(m_DataSource[i].n, 1, (RoleFuZhiType)value.occupationType, GetRoomClick(value.occupationType), ReBuild);
						m_OpenedRoomType.Add((RoleFuZhiType)value.occupationType);
					}
					break;
				case 2:
					if (m_FuZhiDic.TryGetValue(m_DataSource[i].i, out value))
					{
						Panel.Other_2_Room.OpenLock(m_DataSource[i].n, 2, (RoleFuZhiType)value.occupationType, GetRoomClick(value.occupationType), ReBuild);
						m_OpenedRoomType.Add((RoleFuZhiType)value.occupationType);
					}
					break;
				case 3:
					if (m_FuZhiDic.TryGetValue(m_DataSource[i].i, out value))
					{
						Panel.Other_3_Room.OpenLock(m_DataSource[i].n, 3, (RoleFuZhiType)value.occupationType, GetRoomClick(value.occupationType), ReBuild);
						m_OpenedRoomType.Add((RoleFuZhiType)value.occupationType);
					}
					break;
				}
				m_FuZhiCount++;
			}
		}

		private void LiKaiBtn()
		{
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "MainScene")
			{
				mainTitleWnd?.CloseDongFuNoChangeSceneInMain();
			}
			else
			{
				mainTitleWnd?.CloseDongFuClick();
			}
		}

		private void ReBuild(int locationId, RoleFuZhiType roleFuZhiType)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("重建会损失该副职的虚神界贡献点，是否继续重建？", () =>
			{
				ReBuildRoom(locationId, roleFuZhiType);
			});
		}

		private void ReBuildRoom(int locationID, RoleFuZhiType oldFuZhiType)
		{
			double price = 20.0;
			Singleton<UIManager>.Instance.PopUpWnd("DongFuYongTuChooseWnd.prefab", true, m_OpenedRoomType, (UnityAction<RoleFuZhiType>)((RoleFuZhiType type) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, price, (UnityAction<int, int>)((int jiaGe, int ZhiFuType) =>
				{
					if (Singleton<RoleManager>.Instance.ShengYuLingShi(price) < 0.0)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("灵玉不足！");
					}
					else
					{
						ReChooseSureClickAction(oldFuZhiType, type, locationID);
					}
				}), 2);
			}));
		}

		public UnityAction GetRoomClick(int type)
		{
			return GetRoomClick((RoleFuZhiType)type);
		}

		public UnityAction GetRoomClick(RoleFuZhiType type)
		{
			UnityAction result = null;
			switch (type)
			{
			case RoleFuZhiType.LianDan:
				result = () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DiHuoGe_LianDan", DiHuoGeEnterType.DongFu);
				};
				break;
			case RoleFuZhiType.LianQi:
				result = () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("LianQiChooseWnd.prefab", true);
				};
				break;
			case RoleFuZhiType.ZhiFu:
				result = () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("HuaFuChooseWnd.prefab", true);
				};
				break;
			case RoleFuZhiType.FaShu:
				result = () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("ZhanFaShiWnd.prefab", true);
				};
				break;
			case RoleFuZhiType.ZhenTu:
				result = () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("ZF_CreateChooseWnd.prefab", true);
				};
				break;
			case RoleFuZhiType.BuDao:
				result = () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("WuDaoTaiWnd.prefab", true);
				};
				break;
			case RoleFuZhiType.QiaoJiang:
				result = () =>
				{
				};
				break;
			case RoleFuZhiType.LingZhi:
				result = () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("LT_LingTianMainWnd.prefab", true, m_CurrentLevel);
				};
				break;
			case RoleFuZhiType.YuShou:
				result = () =>
				{
				};
				break;
			}
			return result;
		}

		private void ShengJiBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DongFuChooseWnd.prefab", true, m_CurrentLevel);
		}

		private void MiShiBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DongFuMiShiWnd.prefab", true);
		}

		public void LianGongFangBtn()
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "DongFuScene")
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DongFuScene");
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.PopUpWnd("DongFuWnd.prefab", true);
			}
		}

		public void WuDaoTaiBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("WuDaoTaiWnd.prefab", true);
		}

		public void DiHuoGeBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("FaQiXiLianWnd.prefab", true);
		}

		public void LingTianBtn()
		{
			if (m_CurrentLevel != 0)
			{
				PanDuanLingTian(1);
				Singleton<UIManager>.Instance.PopUpWnd("LT_LingTianMainWnd.prefab", true, m_CurrentLevel);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("黄阶洞府，才能开辟灵田！");
			}
		}

		public void PanDuanLingTian(int i)
		{
			if (!(Panel == null) && !(Panel.Other_4_Room == null))
			{
				if (m_CurrentLevel == 0)
				{
					Panel.Other_4_Room.gameObject.SetActive(value: true);
				}
				else if (i == 1)
				{
					Panel.Other_4_Room.gameObject.SetActive(value: false);
				}
			}
		}

		private void Other_1_KaiPiBtn()
		{
			int price = 0;
			switch (m_FuZhiCount)
			{
			case 0:
				price = 1000;
				break;
			case 1:
				price = 10000;
				break;
			default:
				price = 10000;
				break;
			}
			Singleton<UIManager>.Instance.PopUpWnd("DongFuYongTuChooseWnd.prefab", true, m_OpenedRoomType, (UnityAction<RoleFuZhiType>)((RoleFuZhiType type) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, price, (UnityAction<int, int>)((int jiaGe, int ZhiFuType) =>
				{
					if (Singleton<RoleManager>.Instance.ShengYuLingShi(price) < 0.0)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足！");
					}
					else
					{
						ChooseSureClickAction(type, 1);
					}
				}), 1);
			}));
		}

		private void Other_2_KaiPiBtn()
		{
			int price = 0;
			switch (m_FuZhiCount)
			{
			case 0:
				price = 1000;
				break;
			case 1:
				price = 10000;
				break;
			default:
				price = 10000;
				break;
			}
			Singleton<UIManager>.Instance.PopUpWnd("DongFuYongTuChooseWnd.prefab", true, m_OpenedRoomType, (UnityAction<RoleFuZhiType>)((RoleFuZhiType type) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, price, (UnityAction<int, int>)((int jiaGe, int ZhiFuType) =>
				{
					if (Singleton<RoleManager>.Instance.ShengYuLingShi(price) < 0.0)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足！");
					}
					else
					{
						ChooseSureClickAction(type, 2);
					}
				}), 1);
			}));
		}

		private void Other_3_KaiPiBtn()
		{
			int price = 0;
			switch (m_FuZhiCount)
			{
			case 0:
				price = 1000;
				break;
			case 1:
				price = 10000;
				break;
			default:
				price = 10000;
				break;
			}
			Singleton<UIManager>.Instance.PopUpWnd("DongFuYongTuChooseWnd.prefab", true, m_OpenedRoomType, (UnityAction<RoleFuZhiType>)((RoleFuZhiType type) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, price, (UnityAction<int, int>)((int jiaGe, int ZhiFuType) =>
				{
					if (Singleton<RoleManager>.Instance.ShengYuLingShi(price) < 0.0)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足！");
					}
					else
					{
						ChooseSureClickAction(type, 3);
					}
				}), 1);
			}));
		}

		private void ChooseSureClickAction(RoleFuZhiType type, int location)
		{
			int num = 0;
			BuildLiveTypeReq_1005003 data = new BuildLiveTypeReq_1005003
			{
				LiveType = (int)type,
				Location = location
			};
			int fuZhiCount = m_FuZhiCount;
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(fuZhiCount switch
			{
				0 => 1000, 
				1 => 10000, 
				_ => 10000, 
			}) < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足！");
			}
			else if (type == RoleFuZhiType.LianDan)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_LIVE_TYPE, data, (object _obj) =>
				{
					OpenLianDan(_obj, type, location);
				}, isShowWaitUI: true);
			}
			else if (type == RoleFuZhiType.ZhiFu)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_LIVE_TYPE, data, (object _obj) =>
				{
					OpenZhiFu(_obj, type, location);
				}, isShowWaitUI: true);
			}
			else if (type == RoleFuZhiType.BuDao)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_LIVE_TYPE, data, (object _obj) =>
				{
					OpenBuDao(_obj, type, location);
				}, isShowWaitUI: true);
			}
			else if (type == RoleFuZhiType.FaShu)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_LIVE_TYPE, data, (object _obj) =>
				{
					OpenFaShu(_obj, type, location);
				}, isShowWaitUI: true);
			}
			else if (type == RoleFuZhiType.ZhenTu)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_LIVE_TYPE, data, (object _obj) =>
				{
					OpenZhenFa(_obj, type, location);
				}, isShowWaitUI: true);
			}
			else if (type == RoleFuZhiType.LianQi)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_LIVE_TYPE, data, (object _obj) =>
				{
					OpenLianQi(_obj, type, location);
				}, isShowWaitUI: true);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("暂未开放！");
			}
		}

		private void OpenLianDan(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("炼丹", 1, RoleFuZhiType.LianDan, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("炼丹", 2, RoleFuZhiType.LianDan, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("炼丹", 3, RoleFuZhiType.LianDan, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.LianDan);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.LianDan))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.LianDan);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void OpenZhiFu(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("制符", 1, RoleFuZhiType.ZhiFu, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("制符", 2, RoleFuZhiType.ZhiFu, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("制符", 3, RoleFuZhiType.ZhiFu, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.ZhiFu);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.ZhiFu))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.ZhiFu);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void OpenBuDao(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("悟道", 1, RoleFuZhiType.BuDao, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("悟道", 2, RoleFuZhiType.BuDao, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("悟道", 3, RoleFuZhiType.BuDao, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.BuDao);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.BuDao))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.BuDao);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void OpenFaShu(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("战法", 1, RoleFuZhiType.FaShu, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("战法", 2, RoleFuZhiType.FaShu, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("战法", 3, RoleFuZhiType.FaShu, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.FaShu);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.FaShu))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.FaShu);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void OpenLianQi(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("炼器", 1, RoleFuZhiType.LianQi, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("炼器", 2, RoleFuZhiType.LianQi, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("炼器", 3, RoleFuZhiType.LianQi, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.LianQi);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.LianQi))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.LianQi);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void OpenLingTian(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("灵田", 1, RoleFuZhiType.LingZhi, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("灵田", 2, RoleFuZhiType.LingZhi, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("灵田", 3, RoleFuZhiType.LingZhi, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.LingZhi);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.LingZhi))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.LingZhi);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void OpenZhenFa(object _obj, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			BuildLiveTypeResp_1005003 buildLiveTypeResp_ = new BuildLiveTypeResp_1005003();
			buildLiveTypeResp_.MergeFrom(byteString);
			if (buildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buildLiveTypeResp_.ShinyStone;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("阵法", 1, RoleFuZhiType.ZhenTu, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("阵法", 2, RoleFuZhiType.ZhenTu, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("阵法", 3, RoleFuZhiType.ZhenTu, GetRoomClick(type), ReBuild);
					break;
				}
				KaiPiSuccess(RoleFuZhiType.ZhenTu);
				if (!m_OpenedRoomType.Contains(RoleFuZhiType.ZhenTu))
				{
					m_OpenedRoomType.Add(RoleFuZhiType.ZhenTu);
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void KaiPiSuccess(RoleFuZhiType type)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.Live1Type == 0)
			{
				Singleton<RoleManager>.Instance.Role_Entity.Live1Type = (int)type;
				Singleton<RoleManager>.Instance.Role_Entity.Live1Level = 1;
				Singleton<RoleManager>.Instance.Role_Entity.Live1Value = 0;
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.Live1Type != 0 && Singleton<RoleManager>.Instance.Role_Entity.Live2Type == 0)
			{
				Singleton<RoleManager>.Instance.Role_Entity.Live2Type = (int)type;
				Singleton<RoleManager>.Instance.Role_Entity.Live2Level = 1;
				Singleton<RoleManager>.Instance.Role_Entity.Live2Value = 0;
			}
		}

		private void ReChooseSureClickAction(RoleFuZhiType oldType, RoleFuZhiType fuZhiType, int location)
		{
			RebuildLiveTypeReq_1005004 data = new RebuildLiveTypeReq_1005004
			{
				Location = location,
				NewLiveType = (int)fuZhiType
			};
			if (fuZhiType == RoleFuZhiType.LianDan)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.REBUILD_LIVE_TYPE, data, (object _obj) =>
				{
					ReBuildLianDan(_obj, oldType, fuZhiType, location);
				}, isShowWaitUI: true);
			}
			else if (fuZhiType == RoleFuZhiType.ZhiFu)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.REBUILD_LIVE_TYPE, data, (object _obj) =>
				{
					ReBuildZhiFu(_obj, oldType, fuZhiType, location);
				}, isShowWaitUI: true);
			}
			else if (fuZhiType == RoleFuZhiType.BuDao)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.REBUILD_LIVE_TYPE, data, (object _obj) =>
				{
					ReBuildBuDao(_obj, oldType, fuZhiType, location);
				}, isShowWaitUI: true);
			}
			else if (fuZhiType == RoleFuZhiType.FaShu)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.REBUILD_LIVE_TYPE, data, (object _obj) =>
				{
					ReBuildFaShu(_obj, oldType, fuZhiType, location);
				}, isShowWaitUI: true);
			}
			else if (fuZhiType == RoleFuZhiType.ZhenTu)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.REBUILD_LIVE_TYPE, data, (object _obj) =>
				{
					ReBuildZhenTu(_obj, oldType, fuZhiType, location);
				}, isShowWaitUI: true);
			}
			else if (fuZhiType == RoleFuZhiType.LianQi)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.REBUILD_LIVE_TYPE, data, (object _obj) =>
				{
					ReBuildLianQi(_obj, oldType, fuZhiType, location);
				}, isShowWaitUI: true);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("暂未开放！");
			}
		}

		private void ReBuildLianDan(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("炼丹", 1, RoleFuZhiType.LianDan, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("炼丹", 2, RoleFuZhiType.LianDan, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("炼丹", 3, RoleFuZhiType.LianDan, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void ReBuildLianQi(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("炼器", 1, RoleFuZhiType.LianQi, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("炼器", 2, RoleFuZhiType.LianQi, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("炼器", 3, RoleFuZhiType.LianQi, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void ReBuildZhiFu(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("制符", 1, RoleFuZhiType.ZhiFu, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("制符", 2, RoleFuZhiType.ZhiFu, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("制符", 3, RoleFuZhiType.ZhiFu, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void ReBuildBuDao(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("悟道", 1, RoleFuZhiType.BuDao, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("悟道", 2, RoleFuZhiType.BuDao, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("悟道", 3, RoleFuZhiType.BuDao, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void ReBuildFaShu(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("战法", 1, RoleFuZhiType.FaShu, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("战法", 2, RoleFuZhiType.FaShu, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("战法", 3, RoleFuZhiType.FaShu, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void ReBuildLingTian(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("灵田", 1, RoleFuZhiType.LingZhi, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("灵田", 2, RoleFuZhiType.LingZhi, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("灵田", 3, RoleFuZhiType.LingZhi, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void ReBuildZhenTu(object _obj, RoleFuZhiType oldType, RoleFuZhiType type, int location)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString byteString = _obj as ByteString;
			if (byteString == null)
			{
				return;
			}
			RebuildLiveTypeResp_1005004 rebuildLiveTypeResp_ = new RebuildLiveTypeResp_1005004();
			rebuildLiveTypeResp_.MergeFrom(byteString);
			if (rebuildLiveTypeResp_.IsSuccess)
			{
				Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)rebuildLiveTypeResp_.ShinyJade;
				switch (location)
				{
				case 1:
					Panel.Other_1_Room.OpenLock("阵法", 1, RoleFuZhiType.ZhenTu, GetRoomClick(type), ReBuild);
					break;
				case 2:
					Panel.Other_2_Room.OpenLock("阵法", 2, RoleFuZhiType.ZhenTu, GetRoomClick(type), ReBuild);
					break;
				case 3:
					Panel.Other_3_Room.OpenLock("阵法", 3, RoleFuZhiType.ZhenTu, GetRoomClick(type), ReBuild);
					break;
				}
				if (m_OpenedRoomType.Contains(oldType))
				{
					m_OpenedRoomType.Remove(oldType);
				}
				m_OpenedRoomType.Add(type);
				RebuildSuccess(oldType, type);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rebuildLiveTypeResp_.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
		}

		private void RebuildSuccess(RoleFuZhiType oldType, RoleFuZhiType targetType)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.Live1Type == (int)oldType)
			{
				Singleton<RoleManager>.Instance.Role_Entity.Live1Type = (int)targetType;
				Singleton<RoleManager>.Instance.Role_Entity.Live1Level = 1;
				Singleton<RoleManager>.Instance.Role_Entity.Live1Value = 0;
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.Live2Type == (int)oldType)
			{
				Singleton<RoleManager>.Instance.Role_Entity.Live2Type = (int)targetType;
				Singleton<RoleManager>.Instance.Role_Entity.Live2Level = 1;
				Singleton<RoleManager>.Instance.Role_Entity.Live2Value = 0;
			}
		}
	}
}
