using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class FaQiXiLianWnd : Window
	{
		private FaQiXiLianPanel m_Panel;

		private string m_Name;

		private int[] m_Special1 = new int[2];

		private int[] m_Special2 = new int[2];

		private int[] m_Special3 = new int[2];

		private int m_XiLianCount;

		private int m_Stage;

		private int m_ToEnterAuctionTime;

		private int m_YunTie;

		private string m_YunTiID;

		private bool m_Check1;

		private bool m_Check2;

		private bool m_Check3;

		private int m_Check;

		private bool m_CiTiao1;

		private bool m_CiTiao2;

		private bool m_CiTiao3;

		private int m_YunTieCount;

		private double m_LingShiCount;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private XzjBoxEntity m_NaJieBox;

		private GameObject m_InsObj;

		private BaseNaJieGoodEntity m_DataNew;

		private int m_CiTiaoShu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<FaQiXiLianPanel>();
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(ResourceLoad);
			AddButtonClickListener(m_Panel.RegularForgingButton, RegularForging);
			AddButtonClickListener(m_Panel.AdvancedForgingButton, AdvancedForging);
			AddButtonClickListener(m_Panel.TuiChuBut, TuiChu);
			AddButtonClickListener(m_Panel.XiaoWenHaoBut, XiaoWenHao);
			AddButtonClickListener(m_Panel.WeiXuanZhoButton, WeiXuanZho);
			AddButtonClickListener(m_Panel.GuanBiBut, GuanBi);
			AddToggleClickListener(m_Panel.Special_3, CheckThree);
			AddToggleClickListener(m_Panel.Special_2, CheckTwo);
			AddToggleClickListener(m_Panel.Special_1, CheckOne);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
		}

		private void RegularForging()
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

		private void AdvancedForging()
		{
			if (m_YunTie >= m_YunTieCount)
			{
				ChuanShu(4);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前陨铁不足！");
			}
		}

		private void ChuanShu(int pay)
		{
			RechooseEquipmentEffectReq_1005044 data = new RechooseEquipmentEffectReq_1005044
			{
				PayType = pay,
				IsLock1 = m_Check1,
				IsLock2 = m_Check2,
				IsLock3 = m_Check3,
				GoodsId = m_DataNew.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RECHOOSE_EQUIPMENT_EFFECT, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					RechooseEquipmentEffectResp_1005044 rechooseEquipmentEffectResp_ = new RechooseEquipmentEffectResp_1005044();
					rechooseEquipmentEffectResp_.MergeFrom(byteString);
					if (rechooseEquipmentEffectResp_.IsSuccess)
					{
						m_XiLianCount++;
						m_YunTie = rechooseEquipmentEffectResp_.YuntieStone;
						if (rechooseEquipmentEffectResp_.YuntieStone > 0)
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].ResetGoodsNum(m_YunTiID, rechooseEquipmentEffectResp_.YuntieStone);
						}
						else
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].RemoveGoods(m_YunTiID, rechooseEquipmentEffectResp_.YuntieStone);
						}
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = rechooseEquipmentEffectResp_.ShinyStone;
						m_Special1[0] = rechooseEquipmentEffectResp_.EffectType1;
						m_Special1[1] = rechooseEquipmentEffectResp_.EffectValue1;
						m_Special2[0] = rechooseEquipmentEffectResp_.EffectType2;
						m_Special2[1] = rechooseEquipmentEffectResp_.EffectValue2;
						m_Special3[0] = rechooseEquipmentEffectResp_.EffectType3;
						m_Special3[1] = rechooseEquipmentEffectResp_.EffectValue3;
						JiaGe();
						CiTiaoXianShi();
						DataList();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(rechooseEquipmentEffectResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void DataList()
		{
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfo(m_DataNew.GoodsId).ContentList = null;
		}

		private void Display1()
		{
			string title = "";
			m_CiTiao1 = false;
			if (m_Special1[0] != 0)
			{
				m_CiTiaoShu = 1;
				m_Panel.RegularForgingButton.interactable = true;
				m_Panel.AdvancedForgingButton.interactable = true;
				m_Panel.Special_1.gameObject.SetActive(value: true);
				m_Panel.Special_1Text.text = WorldSetting.FaQiXiLianType(m_Special1[0], ref title) + m_Special1[1] + "%";
				m_Panel.SpecialName_1Text.text = title ?? "";
			}
			else
			{
				m_Panel.RegularForgingButton.interactable = false;
				m_Panel.AdvancedForgingButton.interactable = false;
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

		private void Display2()
		{
			string title = "";
			m_CiTiao2 = false;
			if (m_Special2[0] != 0)
			{
				m_CiTiaoShu = 2;
				m_Panel.Special_2.gameObject.SetActive(value: true);
				m_Panel.Special_2Text.text = WorldSetting.FaQiXiLianType(m_Special2[0], ref title) + m_Special2[1] + "%";
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
				m_Panel.Special_3Text.text = WorldSetting.FaQiXiLianType(m_Special3[0], ref title) + m_Special3[1] + "%";
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

		private void FaQiShuJu()
		{
			foreach (Transform item in m_Panel.XuanZHeRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			int num = 0;
			GridLayoutGroup component = m_Panel.XuanZHeRectTransform.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Rank > 4 && (goods.Type == 26 || goods.Type == 15 || goods.Type == 29 || goods.Type == 16))
				{
					num++;
					CreatBoxItem(goods);
				}
			}
			SetGrticalListContentHight(num, m_Panel.XuanZHeRectTransform, component);
		}

		private void CreatBoxItem(BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(m_Panel.XuanZHeRectTransform, m_InsObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsTogItem component = gameObject.GetComponent<UI_GoodsTogItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetValue(data, data.Name, data.FiveElement, data.Level, value2, value, null, ChoosedItem);
		}

		private void ChoosedItem(bool isOn, object data, GameObject gameObj)
		{
			m_DataNew = data as BaseNaJieGoodEntity;
			if (m_DataNew != null)
			{
				m_Panel.Name.text = "法器名称： " + m_DataNew.Name;
				m_Stage = (m_DataNew.Level - 1) / 10 + 1;
				BackpackWuDao();
				QueryMultiplier();
				m_GoodsIconSprite.TryGetValue(m_DataNew.Type, out var value);
				int levelByRank = WorldSetting.GetLevelByRank(m_DataNew.Rank);
				m_RankBgSprite.TryGetValue(levelByRank, out var value2);
				m_Panel.GoodsItem.SetValue(m_DataNew, m_DataNew.Name, m_DataNew.FiveElement, m_DataNew.Level, value2, value, null, WeiXuanZho);
				m_Panel.XuanZhoBGImage.gameObject.SetActive(value: false);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("数据异常！");
				GuanBi();
			}
		}

		private void QueryMultiplier()
		{
			ChooseEquipmentReq_1005047 data = new ChooseEquipmentReq_1005047
			{
				GoodsId = m_DataNew.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHOOSE_EQUIPMENT, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ChooseEquipmentResp_1005047 chooseEquipmentResp_ = new ChooseEquipmentResp_1005047();
					chooseEquipmentResp_.MergeFrom(byteString);
					if (chooseEquipmentResp_.IsSuccess)
					{
						m_XiLianCount = chooseEquipmentResp_.RechoosedCount;
						m_Special1[0] = chooseEquipmentResp_.EffectType1;
						m_Special1[1] = chooseEquipmentResp_.EffectValue1;
						m_Special2[0] = chooseEquipmentResp_.EffectType2;
						m_Special2[1] = chooseEquipmentResp_.EffectValue2;
						m_Special3[0] = chooseEquipmentResp_.EffectType3;
						m_Special3[1] = chooseEquipmentResp_.EffectValue3;
						List<string> dataList = new List<string>();
						Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(m_DataNew, (List<string> strList) =>
						{
							if (strList != null)
							{
								foreach (string str in strList)
								{
									dataList.Add(str);
								}
							}
						});
						JiaGe();
						CiTiaoXianShi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(chooseEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void BackpackWuDao()
		{
			foreach (BaseNaJieGoodEntity item in m_NaJieBox.GoodsList.Where((BaseNaJieGoodEntity item) => item.Type == 35 && item.GetGoodsStage() == m_Stage))
			{
				m_YunTie = item.GoodsNum;
				m_YunTiID = item.GoodsId;
			}
			WuDaoNum();
		}

		private void WuDaoNum()
		{
			if (m_Stage == 0)
			{
				m_Panel.AdvancedForgingText.text = $"陨铁：{m_YunTie}/0";
				return;
			}
			m_YunTieCount = m_XiLianCount / 5 + 1 + m_Check;
			m_Panel.AdvancedForgingText.text = $"陨铁：{m_YunTie}/{m_YunTieCount}";
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
			if (m_Stage == 0)
			{
				m_Panel.RegularForgingText.text = "灵石：0";
				return;
			}
			m_LingShiCount = (double)num * Math.Pow(1.4, m_XiLianCount) * Math.Pow(2.0, m_Check + 1);
			m_Panel.RegularForgingText.text = $"灵石：{Math.Ceiling(m_LingShiCount)}";
		}

		private void CiTiaoXianShi()
		{
			if (m_Stage == 0)
			{
				m_Panel.Special_1.gameObject.SetActive(value: false);
				m_Panel.Special_2.gameObject.SetActive(value: false);
				m_Panel.Special_3.gameObject.SetActive(value: false);
				return;
			}
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

		private void WeiXuanZho(bool isOn, object data, GameObject gameObj)
		{
			m_Panel.WeiXuanZhoButton.gameObject.SetActive(value: false);
			m_Panel.XuanZhoImage.gameObject.SetActive(value: true);
			m_Panel.XuanZhoBGImage.gameObject.SetActive(value: true);
			FaQiShuJu();
		}

		private void WeiXuanZho()
		{
			m_Panel.WeiXuanZhoButton.gameObject.SetActive(value: false);
			m_Panel.XuanZhoImage.gameObject.SetActive(value: true);
			m_Panel.XuanZhoBGImage.gameObject.SetActive(value: true);
			FaQiShuJu();
		}

		private void GuanBi()
		{
			m_Special1[0] = 0;
			m_Special1[1] = 0;
			m_Special2[0] = 0;
			m_Special2[1] = 0;
			m_Special3[0] = 0;
			m_Special3[1] = 0;
			m_XiLianCount = 0;
			m_Stage = 0;
			JiaGe();
			CiTiaoXianShi();
			m_Panel.XuanZhoBGImage.gameObject.SetActive(value: false);
			m_Panel.XuanZhoImage.gameObject.SetActive(value: false);
			m_Panel.WeiXuanZhoButton.gameObject.SetActive(value: true);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_Xie.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FangJu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			m_GoodsIconSprite.Add(29, value);
			m_GoodsIconSprite.Add(15, value2);
			m_GoodsIconSprite.Add(16, value3);
			m_GoodsIconSprite.Add(26, value4);
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value5);
			m_RankBgSprite.Add(2, value6);
			m_RankBgSprite.Add(3, value7);
			m_RankBgSprite.Add(4, value8);
		}

		private static void TuiChu()
		{
			Singleton<UIManager>.Instance.CloseWnd("FaQiXiLianWnd.prefab");
		}

		private static void XiaoWenHao()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, "1、每次洗炼将重置法器特效。 2、法器特效中：五行防护、五行伤害、攻击速度为稀有属性，出现概率极低。 3、每次洗炼后，价格将不断提高。 4、选择陨铁洗炼则每5次多收取一块陨铁。 5、每锁定一个特效，则价格相应提升。", "法器洗炼");
		}

		private void JiaGe()
		{
			WuDaoNum();
			Stage();
		}
	}
}
