using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class TiShengPingJieWnd : Window
	{
		private TiShengPingJiePanel m_Panel;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private XzjBoxEntity m_NaJieBox;

		private string m_ID;

		private string GoodsId;

		private int m_WuPingLeiXing;

		private int m_Level;

		private int m_Rank;

		private string m_LingYanZhi;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<TiShengPingJiePanel>();
			ResourceLoad();
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(null);
			if (paralist != null)
			{
				m_ID = (string)paralist[0];
				m_WuPingLeiXing = (int)paralist[1];
				m_Level = (int)paralist[2];
				m_LingYanZhi = (string)paralist[3];
				m_Rank = (int)paralist[4];
			}
			ChaXun();
			AddButtonClickListener(m_Panel.GuanBi, LiKaiBtn);
			AddButtonClickListener(m_Panel.BuQiBtn, BuQi);
		}

		private void ChaXun()
		{
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Level == m_Level && goods.Type == m_WuPingLeiXing)
				{
					m_Panel.XianShiOne.gameObject.SetActive(value: true);
					m_Panel.WuImage.gameObject.SetActive(value: false);
					UI_GoodsTogItem component = m_Panel.XianShiOne.GetComponent<UI_GoodsTogItem>();
					m_GoodsIconSprite.TryGetValue(goods.Type, out var value);
					int levelByRank = WorldSetting.GetLevelByRank(goods.Rank);
					m_RankBgSprite.TryGetValue(levelByRank, out var value2);
					component.SetValue(goods, goods.Name, goods.FiveElement, goods.Level, value2, value, null, null);
					GoodsId = goods.GoodsId;
					m_Panel.BuQiBtn.interactable = true;
					break;
				}
			}
		}

		private void BuQi()
		{
			switch (m_WuPingLeiXing)
			{
			case 39:
				DeJing();
				break;
			case 40:
				XianJing();
				break;
			case 41:
				ShenShu();
				break;
			}
		}

		private void DeJing()
		{
			UpgradeAchievementConversionRateReq_1001020 data = new UpgradeAchievementConversionRateReq_1001020
			{
				AchievementId = m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeAchievementConversionRate, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeAchievementConversionRateResp_1001020 upgradeAchievementConversionRateResp_ = new UpgradeAchievementConversionRateResp_1001020();
					upgradeAchievementConversionRateResp_.MergeFrom(byteString);
					if (upgradeAchievementConversionRateResp_.IsSuccess)
					{
						GetGongFaEffect(upgradeAchievementConversionRateResp_);
						NaJieSpaceWnd naJieSpaceWnd = Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = upgradeAchievementConversionRateResp_.NowCapacity;
						naJieSpaceWnd?.RemoveWuPin(GoodsId, 1);
						naJieSpaceWnd?.FlashData();
						Singleton<UIManager>.Instance.FindWndByName<CanWuWnd>("CanWuWnd.prefab")?.EnterCanWu();
						Singleton<UIManager>.Instance.CloseWnd("CanWuGongFaWnd.prefab");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeAchievementConversionRateResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetGongFaEffect(UpgradeAchievementConversionRateResp_1001020 upgradeAchievementConversionRateResp1001020)
		{
			string title = "";
			List<string> list = new List<string>
			{
				WorldSetting.GetRankAllName(m_Rank),
				WorldSetting.GetRankAllName(upgradeAchievementConversionRateResp1001020.Rank),
				$"{m_LingYanZhi:F}",
				$"{upgradeAchievementConversionRateResp1001020.ConversionRate:F}"
			};
			if (upgradeAchievementConversionRateResp1001020.EffectValue1 > 0)
			{
				list.Add(WorldSetting.GetGongFaEffectType(upgradeAchievementConversionRateResp1001020.EffectType1, ref title) + $"{upgradeAchievementConversionRateResp1001020.EffectValue1}%");
			}
			else
			{
				list.Add("无");
			}
			if (upgradeAchievementConversionRateResp1001020.EffectType2 > 0)
			{
				list.Add(WorldSetting.GetGongFaEffectType(upgradeAchievementConversionRateResp1001020.EffectType2, ref title) + $"{upgradeAchievementConversionRateResp1001020.EffectType2}%");
			}
			else
			{
				list.Add("无");
			}
			if (upgradeAchievementConversionRateResp1001020.EffectType3 > 0)
			{
				list.Add(WorldSetting.GetGongFaEffectType(upgradeAchievementConversionRateResp1001020.EffectType3, ref title) + $"{upgradeAchievementConversionRateResp1001020.EffectType3}%");
			}
			else
			{
				list.Add("无");
			}
			m_Panel.UpdatePanel.gameObject.SetActive(value: true);
			m_Panel.UpdatePanel.SetValue(list, LiKaiBtn);
		}

		private void XianJing()
		{
			UpgradeEquipmentConversionRateReq_1001021 data = new UpgradeEquipmentConversionRateReq_1001021
			{
				EquipmentId = m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeEquipmentConversionRate, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeEquipmentConversionRateResp_1001021 upgradeEquipmentConversionRateResp_ = new UpgradeEquipmentConversionRateResp_1001021();
					upgradeEquipmentConversionRateResp_.MergeFrom(byteString);
					if (upgradeEquipmentConversionRateResp_.IsSuccess)
					{
						FaQiXiLian(upgradeEquipmentConversionRateResp_);
						NaJieSpaceWnd naJieSpaceWnd = Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
						BaseNaJieGoodEntity goodsInfo = Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfo(m_ID);
						if (goodsInfo != null)
						{
							goodsInfo.ConversionRate = upgradeEquipmentConversionRateResp_.ConversionRate;
							goodsInfo.Rank = upgradeEquipmentConversionRateResp_.Rank;
							goodsInfo.ContentList.Clear();
							NaJieSpaceWnd naJieSpaceWnd2 = Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
							Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
							naJieSpaceWnd2?.OnShow();
							naJieSpaceWnd?.RemoveWuPin(GoodsId, 1);
							Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = upgradeEquipmentConversionRateResp_.NowCapacity;
							naJieSpaceWnd?.FlashData();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeEquipmentConversionRateResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FaQiXiLian(UpgradeEquipmentConversionRateResp_1001021 upgradeEquipmentConversionRateResp1001021)
		{
			string title = "";
			List<string> list = new List<string>
			{
				WorldSetting.GetRankAllName(m_Rank),
				WorldSetting.GetRankAllName(upgradeEquipmentConversionRateResp1001021.Rank),
				$"{m_LingYanZhi:F}",
				$"{upgradeEquipmentConversionRateResp1001021.ConversionRate:F}"
			};
			if (upgradeEquipmentConversionRateResp1001021.EffectValue1 > 0)
			{
				list.Add(WorldSetting.FaQiXiLianType(upgradeEquipmentConversionRateResp1001021.EffectType1, ref title) + $"{upgradeEquipmentConversionRateResp1001021.EffectValue1}%");
			}
			else
			{
				list.Add("无");
			}
			if (upgradeEquipmentConversionRateResp1001021.EffectType2 > 0)
			{
				list.Add(WorldSetting.FaQiXiLianType(upgradeEquipmentConversionRateResp1001021.EffectType2, ref title) + $"{upgradeEquipmentConversionRateResp1001021.EffectType2}%");
			}
			else
			{
				list.Add("无");
			}
			if (upgradeEquipmentConversionRateResp1001021.EffectType3 > 0)
			{
				list.Add(WorldSetting.FaQiXiLianType(upgradeEquipmentConversionRateResp1001021.EffectType3, ref title) + $"{upgradeEquipmentConversionRateResp1001021.EffectType3}%");
			}
			else
			{
				list.Add("无");
			}
			m_Panel.UpdatePanel.gameObject.SetActive(value: true);
			m_Panel.UpdatePanel.SetValue(list, LiKaiBtn);
		}

		private void ShenShu()
		{
			UpgradeSkillConversionRateReq_1001022 data = new UpgradeSkillConversionRateReq_1001022
			{
				SkillId = m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeSkillConversionRate, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeSkillConversionRateResp_1001022 upgradeSkillConversionRateResp_ = new UpgradeSkillConversionRateResp_1001022();
					upgradeSkillConversionRateResp_.MergeFrom(byteString);
					if (upgradeSkillConversionRateResp_.IsSuccess)
					{
						FaQiXiLian(upgradeSkillConversionRateResp_);
						Singleton<UIManager>.Instance.FindWndByName<CanWuWnd>("CanWuWnd.prefab")?.EnterCanWu();
						Singleton<UIManager>.Instance.CloseWnd("Common_TipWnd.prefab");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeSkillConversionRateResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FaQiXiLian(UpgradeSkillConversionRateResp_1001022 upgradeSkillConversionRateResp1001022)
		{
			List<string> values = new List<string>
			{
				WorldSetting.GetRankAllName(m_Rank),
				WorldSetting.GetRankAllName(upgradeSkillConversionRateResp1001022.Rank),
				$"{m_LingYanZhi:F}",
				$"{upgradeSkillConversionRateResp1001022.ConversionRate:F}"
			};
			m_Panel.UpdatePanel2.gameObject.SetActive(value: true);
			m_Panel.UpdatePanel2.SetValue(values, LiKaiBtn);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_GongFaShu.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_YunTie.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FaSuShu.png");
			m_GoodsIconSprite.Add(39, value);
			m_GoodsIconSprite.Add(40, value2);
			m_GoodsIconSprite.Add(41, value3);
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value4);
			m_RankBgSprite.Add(2, value5);
			m_RankBgSprite.Add(3, value6);
			m_RankBgSprite.Add(4, value7);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
