using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GengHuanFaZhenDengJielWnd : Window
	{
		public GengHuanFzZhenDengJiePanel Panel;

		public Dictionary<Button, Image> BtnLockImgDic;

		private ShengJiZhenFaModel m_CurrentModel;

		private Dictionary<string, JuLingLevelModel> m_JuLingDataSource;

		private Dictionary<string, KongLingLevelModel> m_KongLingLevelDataSource;

		private UnityAction<long, double> m_CompeleteCallBack;

		private int m_CurrentLevel;

		private string m_HuangPrice;

		private string m_XuanPrice;

		private string m_DiPrice;

		private string m_TianPrice;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<GengHuanFzZhenDengJiePanel>();
			AddButtonClickListener(Panel.CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("GengHuanFaZhenDengJieWnd.prefab");
			});
			BtnLockImgDic = new Dictionary<Button, Image>();
			m_JuLingDataSource = new Dictionary<string, JuLingLevelModel>();
			m_KongLingLevelDataSource = new Dictionary<string, KongLingLevelModel>();
			AddButtonClickListener(Panel.HuangJieBtn, HuangJieClick);
			AddButtonClickListener(Panel.XuanJieBtn, XuanJieClick);
			AddButtonClickListener(Panel.DiJieBtn, DiJieClick);
			AddButtonClickListener(Panel.TianJieBtn, TianJieClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 3)
			{
				m_CurrentModel = paralist[0] as ShengJiZhenFaModel;
				if (m_CurrentModel != null && m_CurrentModel.type == 1)
				{
					m_JuLingDataSource = paralist[1] as Dictionary<string, JuLingLevelModel>;
					m_HuangPrice = m_JuLingDataSource["1"].price.ToString();
					m_XuanPrice = m_JuLingDataSource["2"].price.ToString();
					m_DiPrice = m_JuLingDataSource["3"].price.ToString();
					m_TianPrice = m_JuLingDataSource["4"].price.ToString();
				}
				else if (m_CurrentModel != null && m_CurrentModel.type == 2)
				{
					m_KongLingLevelDataSource = paralist[1] as Dictionary<string, KongLingLevelModel>;
					m_HuangPrice = m_KongLingLevelDataSource["1"].price.ToString();
					m_XuanPrice = m_KongLingLevelDataSource["2"].price.ToString();
					m_DiPrice = m_KongLingLevelDataSource["3"].price.ToString();
					m_TianPrice = m_KongLingLevelDataSource["4"].price.ToString();
				}
				m_CompeleteCallBack = paralist[2] as UnityAction<long, double>;
			}
			if (m_CurrentModel != null)
			{
				Init();
			}
		}

		private void Init()
		{
			m_CurrentLevel = m_CurrentModel.level;
			switch (m_CurrentLevel)
			{
			case 1:
				UnLockedHuangJie();
				break;
			case 2:
				UnLockedHuangJie();
				UnLockedXuanJie();
				break;
			case 3:
				UnLockedHuangJie();
				UnLockedXuanJie();
				UnLockedDiJie();
				break;
			case 4:
				UnLockedHuangJie();
				UnLockedXuanJie();
				UnLockedDiJie();
				UnLockedTianJie();
				break;
			case 0:
				break;
			}
		}

		private void UnLockedHuangJie()
		{
			Panel.LockHuang.gameObject.SetActive(value: false);
			Panel.DangQianXuanZeText.text = "黄阶";
		}

		private void UnLockedXuanJie()
		{
			Panel.LockXuan.gameObject.SetActive(value: false);
			Panel.DangQianXuanZeText.text = "玄阶";
		}

		private void UnLockedDiJie()
		{
			Panel.LockDi.gameObject.SetActive(value: false);
			Panel.DangQianXuanZeText.text = "地阶";
		}

		private void UnLockedTianJie()
		{
			Panel.LockTian.gameObject.SetActive(value: false);
			Panel.DangQianXuanZeText.text = "天阶";
		}

		private void BuyClick(string levelId, Button btn, long price)
		{
			Singleton<UIManager>.Instance.PopUpWnd("FaZhenXiaoHaoLingShiWnd.prefab", true, price);
			FaZhenXiaoHaoLingShiWnd faZhenXiaoHaoLingShiWnd = Singleton<UIManager>.Instance.FindWndByName<FaZhenXiaoHaoLingShiWnd>("FaZhenXiaoHaoLingShiWnd.prefab");
			faZhenXiaoHaoLingShiWnd.JieSuoActionEvent = (Action<bool>)Delegate.Combine(faZhenXiaoHaoLingShiWnd.JieSuoActionEvent, (Action<bool>)((bool isSuccess) =>
			{
				if (isSuccess)
				{
					if (Singleton<RoleManager>.Instance.GetRoleStage() < int.Parse(levelId))
					{
						switch (int.Parse(levelId))
						{
						case 2:
							MonoSingleton<GameStart>.Instance.PopUpAttention("需要等阶:筑基！");
							break;
						case 3:
							MonoSingleton<GameStart>.Instance.PopUpAttention("需要等阶:结丹！");
							break;
						case 4:
							MonoSingleton<GameStart>.Instance.PopUpAttention("需要等阶:元婴！");
							break;
						}
					}
					else
					{
						if (Singleton<RoleManager>.Instance.ShengYuLingShi(price) >= 0.0)
						{
							if (m_CurrentModel.type == 1)
							{
								UpdateJuLingZhen();
							}
							else
							{
								UpdateKongLingZhen();
							}
							m_CurrentModel.id = levelId;
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("您的灵石不足！");
						}
						Singleton<UIManager>.Instance.CloseWnd("GengHuanFaZhenDengJieWnd.prefab");
					}
				}
			}));
		}

		private void UpdateJuLingZhen()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADE_GATHER_MATRIX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGatherMatrixResp_1005013 upgradeGatherMatrixResp_ = new UpgradeGatherMatrixResp_1005013();
					upgradeGatherMatrixResp_.MergeFrom(byteString);
					if (upgradeGatherMatrixResp_.IsSuccess)
					{
						if (m_CompeleteCallBack != null)
						{
							m_CompeleteCallBack(upgradeGatherMatrixResp_.GatherMatrixLessTime, upgradeGatherMatrixResp_.AchievementCircleValue);
						}
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = upgradeGatherMatrixResp_.ShinyStone;
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGatherMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGatherMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
				}
			}, isShowWaitUI: true);
		}

		private void UpdateKongLingZhen()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADE_CONTROLLER_MATRIX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeControllerMatrixResp_1005015 upgradeControllerMatrixResp_ = new UpgradeControllerMatrixResp_1005015();
					upgradeControllerMatrixResp_.MergeFrom(byteString);
					if (upgradeControllerMatrixResp_.IsSuccess)
					{
						if (m_CompeleteCallBack != null)
						{
							m_CompeleteCallBack(upgradeControllerMatrixResp_.ControllerMatrixLessTime, 0.0);
						}
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = upgradeControllerMatrixResp_.ShinyStone;
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeControllerMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeControllerMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
				}
			}, isShowWaitUI: true);
		}

		private void HuangJieClick()
		{
			BuyClick("1", Panel.HuangJieBtn, long.Parse(m_HuangPrice));
		}

		private void XuanJieClick()
		{
			if (m_CurrentLevel == 1)
			{
				BuyClick("2", Panel.XuanJieBtn, long.Parse(m_XuanPrice));
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先解锁黄阶等级");
			}
		}

		private void DiJieClick()
		{
			if (m_CurrentLevel == 2)
			{
				BuyClick("3", Panel.DiJieBtn, long.Parse(m_DiPrice));
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先解锁玄阶等级");
			}
		}

		private void TianJieClick()
		{
			if (m_CurrentLevel == 3)
			{
				BuyClick("4", Panel.TianJieBtn, long.Parse(m_TianPrice));
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先解锁地阶等级");
			}
		}
	}
}
