using System;
using System.Linq;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class WuDaoTaiWnd : Window
	{
		private WuDaoTaiPanel m_Panel;

		private string m_Name;

		private int[] m_Special1 = new int[2];

		private int[] m_Special2 = new int[2];

		private int[] m_Special3 = new int[2];

		private int m_XiLianCount;

		private int m_Stage;

		private int m_WuDaoShi;

		private int m_ToEnterAuctionTime;

		private string m_WuDaoShiID;

		private bool m_Check1;

		private bool m_Check2;

		private bool m_Check3;

		private int m_Check;

		private bool m_CiTiao1;

		private bool m_CiTiao2;

		private bool m_CiTiao3;

		private int m_WuDaoShiCount;

		private double m_LingShiCount;

		private XzjBoxEntity m_NaJieBox;

		private int m_CiTiaoShu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<WuDaoTaiPanel>();
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(SetActiveNew);
			AddButtonClickListener(m_Panel.SeniorTookButton, SeniorTook);
			AddButtonClickListener(m_Panel.GeneralEnlightenmentBut, GeneralEnlightenment);
			AddButtonClickListener(m_Panel.TuiChuBut, TuiChu);
			AddButtonClickListener(m_Panel.XiaoWenHaoBut, XiaoWenHao);
			AddToggleClickListener(m_Panel.Special_1, CheckOne);
			AddToggleClickListener(m_Panel.Special_3, CheckThree);
			AddToggleClickListener(m_Panel.Special_2, CheckTwo);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_WUDAOTAI, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterWuDaoTaiResp_1005041 enterWuDaoTaiResp_ = new EnterWuDaoTaiResp_1005041();
					enterWuDaoTaiResp_.MergeFrom(byteString);
					m_XiLianCount = enterWuDaoTaiResp_.RechooseCount;
					m_Stage = enterWuDaoTaiResp_.Stage;
					m_Panel.Name.text = "功法名称    " + enterWuDaoTaiResp_.AchievementName;
					m_Special1[0] = enterWuDaoTaiResp_.EffectType1;
					m_Special1[1] = enterWuDaoTaiResp_.EffectValue1;
					Display1();
					m_Special2[0] = enterWuDaoTaiResp_.EffectType2;
					m_Special2[1] = enterWuDaoTaiResp_.EffectValue2;
					Display2();
					m_Special3[0] = enterWuDaoTaiResp_.EffectType3;
					m_Special3[1] = enterWuDaoTaiResp_.EffectValue3;
					Display3();
					BackpackWuDao();
				}
			}, isShowWaitUI: true);
		}

		private void SetActiveNew()
		{
		}

		private void CiTiaoXianShi()
		{
			m_Panel.Special_1.gameObject.SetActive(value: false);
			m_Panel.Special_2.gameObject.SetActive(value: false);
			m_Panel.Special_3.gameObject.SetActive(value: false);
			m_ToEnterAuctionTime = 120000;
			m_CiTiao1 = true;
			m_CiTiao2 = true;
			m_CiTiao3 = true;
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_ToEnterAuctionTime <= 100000 && m_CiTiao1)
			{
				Display1();
			}
			else if (m_ToEnterAuctionTime <= 60000 && m_CiTiao2)
			{
				Display2();
			}
			else if (m_ToEnterAuctionTime <= 1 && m_CiTiao3)
			{
				Display3();
			}
			m_ToEnterAuctionTime -= (int)(Time.fixedDeltaTime * 100000f);
		}

		private void Display1()
		{
			string title = "";
			m_CiTiao1 = false;
			if (m_Special1[0] != 0)
			{
				m_CiTiaoShu = 1;
				m_Panel.Special_1.gameObject.SetActive(value: true);
				m_Panel.WuText.gameObject.SetActive(value: false);
				m_Panel.SeniorTookButton.interactable = true;
				m_Panel.GeneralEnlightenmentBut.interactable = true;
				m_Panel.Special_1Text.text = WorldSetting.GetGongFaEffectType(m_Special1[0], ref title) + m_Special1[1] + "%";
				m_Panel.SpecialName_1Text.text = title ?? "";
			}
			else
			{
				m_Panel.WuText.gameObject.SetActive(value: true);
				m_Panel.WuText.text = "当前功法品阶过低，无法悟道！";
				m_Panel.SeniorTookButton.interactable = false;
				m_Panel.GeneralEnlightenmentBut.interactable = false;
			}
		}

		private void Display2()
		{
			string title = "";
			m_CiTiao2 = false;
			if (m_Special2[0] != 0)
			{
				m_CiTiaoShu = 2;
				m_Panel.Special_2.gameObject.SetActive(value: true);
				m_Panel.Special_2Text.text = WorldSetting.GetGongFaEffectType(m_Special2[0], ref title) + m_Special2[1] + "%";
				m_Panel.SpecialName_2Text.text = title ?? "";
			}
		}

		private void Display3()
		{
			string title = "";
			m_CiTiao3 = false;
			if (m_Special3[0] != 0)
			{
				m_CiTiaoShu = 3;
				m_Panel.Special_3.gameObject.SetActive(value: true);
				m_Panel.Special_3Text.text = WorldSetting.GetGongFaEffectType(m_Special3[0], ref title) + m_Special3[1] + "%";
				m_Panel.SpecialName_3Text.text = title ?? "";
			}
		}

		private void CheckOne(bool panDuan)
		{
			if (panDuan)
			{
				m_Check++;
				if (Panduan())
				{
					m_Check1 = true;
					JiaGe();
				}
				else
				{
					m_Panel.Special_1.isOn = false;
				}
			}
			else
			{
				m_Check--;
				m_Check1 = false;
				JiaGe();
			}
		}

		private void CheckTwo(bool panDuan)
		{
			if (panDuan)
			{
				m_Check++;
				if (Panduan())
				{
					m_Check2 = true;
					JiaGe();
				}
				else
				{
					m_Panel.Special_2.isOn = false;
				}
			}
			else
			{
				m_Check--;
				m_Check2 = false;
				JiaGe();
			}
		}

		private void CheckThree(bool panDuan)
		{
			if (panDuan)
			{
				m_Check++;
				if (Panduan())
				{
					m_Check3 = true;
					JiaGe();
				}
				else
				{
					m_Panel.Special_3.isOn = false;
				}
			}
			else
			{
				m_Check--;
				m_Check3 = false;
				JiaGe();
			}
		}

		private void JiaGe()
		{
			WuDaoNum();
			Stage();
		}

		private void BackpackWuDao()
		{
			foreach (BaseNaJieGoodEntity item in m_NaJieBox.GoodsList.Where((BaseNaJieGoodEntity item) => item.Type == 34 && item.GetGoodsStage() == m_Stage))
			{
				m_WuDaoShi = item.GoodsNum;
				m_WuDaoShiID = item.GoodsId;
			}
			WuDaoNum();
			Stage();
		}

		private void WuDaoNum()
		{
			if (m_Special1[0] == 0)
			{
				m_Panel.SeniorTookText.text = "0/0";
				return;
			}
			m_WuDaoShiCount = m_XiLianCount / 5 + 1 + m_Check;
			m_Panel.SeniorTookText.text = $"悟道石：{m_WuDaoShi}/{m_WuDaoShiCount}";
		}

		private void Stage()
		{
			int num = 1;
			switch (m_Stage)
			{
			case 1:
				num = 10;
				break;
			case 2:
				num = 50;
				break;
			case 3:
				num = 100;
				break;
			case 4:
				num = 500;
				break;
			case 5:
				num = 1500;
				break;
			case 6:
				num = 4000;
				break;
			case 7:
				num = 10000;
				break;
			case 8:
				num = 20000;
				break;
			case 9:
				num = 30000;
				break;
			}
			if (m_Special1[0] == 0)
			{
				m_Panel.GeneralEnlightenmentText.text = "0";
				return;
			}
			m_LingShiCount = (double)num * Math.Pow(1.4, m_XiLianCount) * Math.Pow(2.0, m_Check + 1);
			m_Panel.GeneralEnlightenmentText.text = $"灵石：{Math.Ceiling(m_LingShiCount)}";
		}

		private void SeniorTook()
		{
			if (m_WuDaoShi >= m_WuDaoShiCount)
			{
				ChuanShu(3);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前悟道石不足！");
			}
		}

		private void GeneralEnlightenment()
		{
			if ((double)Singleton<RoleManager>.Instance.Role_Entity.ShinyStone >= m_LingShiCount)
			{
				ChuanShu(1);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前灵石不足！");
			}
		}

		private bool Panduan()
		{
			if (m_Check == m_CiTiaoShu)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("无法锁定所有特效！");
				return false;
			}
			return true;
		}

		private void ChuanShu(int pay)
		{
			RechooseAchievementEffectReq_1005042 data = new RechooseAchievementEffectReq_1005042
			{
				PayType = pay,
				IsLock1 = m_Check1,
				IsLock2 = m_Check2,
				IsLock3 = m_Check3
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RECHOOSE_ACHIEVEMENT_EFFECT, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					RechooseAchievementEffectResp_1005042 rechooseAchievementEffectResp_ = new RechooseAchievementEffectResp_1005042();
					rechooseAchievementEffectResp_.MergeFrom(byteString);
					if (rechooseAchievementEffectResp_.IsSuccess)
					{
						m_XiLianCount++;
						m_WuDaoShi = rechooseAchievementEffectResp_.WudaoStone;
						if (rechooseAchievementEffectResp_.WudaoStone > 0)
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].ResetGoodsNum(m_WuDaoShiID, rechooseAchievementEffectResp_.WudaoStone);
						}
						else
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].RemoveGoods(m_WuDaoShiID, rechooseAchievementEffectResp_.WudaoStone);
						}
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = rechooseAchievementEffectResp_.ShinyStone;
						m_Special1[0] = rechooseAchievementEffectResp_.EffectType1;
						m_Special1[1] = rechooseAchievementEffectResp_.EffectValue1;
						m_Special2[0] = rechooseAchievementEffectResp_.EffectType2;
						m_Special2[1] = rechooseAchievementEffectResp_.EffectValue2;
						m_Special3[0] = rechooseAchievementEffectResp_.EffectType3;
						m_Special3[1] = rechooseAchievementEffectResp_.EffectValue3;
						JiaGe();
						SetActiveNew();
						CiTiaoXianShi();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rechooseAchievementEffectResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private static void XiaoWenHao()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, "1、每次洗炼将重置功法特效。 2、法器特效中：气运、降低雷劫伤害为稀有属性，出现概率较低。 3、每次洗炼后，价格将不断提高。 4、选择悟道石洗炼则每5次多收取一块悟道石。 5、每锁定一个特效，则价格相应提升。", "功法洗炼");
		}

		private static void TuiChu()
		{
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiWnd.prefab");
		}
	}
}
