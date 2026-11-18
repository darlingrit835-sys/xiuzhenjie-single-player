using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_WaiWuDianXiangQingWnd : Window
	{
		private ZongMen_WaiWuDianXiangQingPanel m_Panel;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<UpDonwnSortStatus, Sprite> m_UpDownSortIconSprite;

		private List<BaseNaJieGoodEntity> m_DataSource;

		private BaseNaJieGoodEntity m_XuanZhoWuPing;

		private long m_LingShiShu;

		private int m_Current = 12;

		private string m_WuPingMingCheng;

		private string m_XuanShangLingShiShu;

		private int zhangLaoTisheng;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			zhangLaoTisheng = (int)paralist[0];
			m_Panel = base.GameObject.GetComponent<ZongMen_WaiWuDianXiangQingPanel>();
			ResourceLoad();
			AddButtonClickListener(m_Panel.GuanBiButton, CloseWindow);
			m_XuanZhoWuPing = new BaseNaJieGoodEntity();
			m_Panel.RewardButton.interactable = false;
			AddToggleClickListener(m_Panel.PowerPlantItem, PowerPlant);
			AddToggleClickListener(m_Panel.MineralItem, Mineral);
			AddToggleClickListener(m_Panel.BloodItem, Blood);
			AddToggleClickListener(m_Panel.SkeletonItem, Skeleton);
			AddToggleClickListener(m_Panel.DemonDanItem, DemonDan);
			AddButtonClickListener(m_Panel.ShoSuoButton, ShoSuo);
			AddButtonClickListener(m_Panel.RewardButton, Reward);
			AddButtonClickListener(m_Panel.FaBuXuanShangButton, FaBuXuanShang);
		}

		private void FaBuXuanShang()
		{
			if (m_LingShiShu <= 0)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("至少需要1灵石");
				return;
			}
			if (m_XuanZhoWuPing.Name == null && string.IsNullOrEmpty(m_XuanZhoWuPing.Name))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("未指定物品！");
				return;
			}
			WaiWuFaBuXuanShangReq_1015059 data = new WaiWuFaBuXuanShangReq_1015059
			{
				GoodsTemplateId = m_XuanZhoWuPing.GoodsId,
				GoodsType = m_Current,
				Award = (int)m_LingShiShu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.WaiWuFaBuXuanShang, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					WaiWuFaBuXuanShangResp_1015059 waiWuFaBuXuanShangResp_ = new WaiWuFaBuXuanShangResp_1015059();
					waiWuFaBuXuanShangResp_.MergeFrom(byteString);
					if (waiWuFaBuXuanShangResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = (int)waiWuFaBuXuanShangResp_.CurrentStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						Singleton<UIManager>.Instance.FindWndByName<ZongMen_WaiWuDianWnd>("ZongMen_WaiWuDianWnd.prefab")?.HuoQuZhangLaoXingXi();
						CloseWindow();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(waiWuFaBuXuanShangResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void Reward()
		{
			if (m_Panel.RewardInputField.text.Length < 1)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("至少需要1灵石");
				return;
			}
			string text = m_Panel.RewardInputField.text;
			m_LingShiShu = long.Parse(text);
			JiSuan();
		}

		private void PowerPlant(bool isOn)
		{
			m_Current = 12;
			if (!isOn)
			{
				m_Current = 0;
			}
		}

		private void Mineral(bool isOn)
		{
			m_Current = 13;
			if (!isOn)
			{
				m_Current = 0;
			}
		}

		private void Blood(bool isOn)
		{
			m_Current = 9;
			if (!isOn)
			{
				m_Current = 0;
			}
		}

		private void Skeleton(bool isOn)
		{
			m_Current = 10;
			if (!isOn)
			{
				m_Current = 0;
			}
		}

		private void DemonDan(bool isOn)
		{
			m_Current = 11;
			if (!isOn)
			{
				m_Current = 0;
			}
		}

		private void LingYeClearData()
		{
			if (m_Panel == null || m_Panel.ScrollRect == null)
			{
				return;
			}
			foreach (RectTransform item in m_Panel.ScrollRect.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void ShoSuo()
		{
			LingYeClearData();
			if (string.IsNullOrEmpty(m_Panel.ShoSuoInputField.text) && m_Panel.ShoSuoInputField.name.Length == 1)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("至少需要两个关键字");
				return;
			}
			WaiWuXuanShangGoodsSearchReq_1015143 data = new WaiWuXuanShangGoodsSearchReq_1015143
			{
				Name = m_Panel.ShoSuoInputField.text,
				GoodsType = m_Current
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.WaiWuXuanShangGoodsSearch, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					WaiWuXuanShangGoodsSearchResp_1015143 waiWuXuanShangGoodsSearchResp_ = new WaiWuXuanShangGoodsSearchResp_1015143();
					waiWuXuanShangGoodsSearchResp_.MergeFrom(byteString);
					if (waiWuXuanShangGoodsSearchResp_.IsSuccess)
					{
						m_DataSource = new List<BaseNaJieGoodEntity>();
						foreach (WaiWuXuanShangGoodsSearchResp_1015143.Types.GoodsInfoResp_1015144 goodsInfo in waiWuXuanShangGoodsSearchResp_.GoodsInfoList)
						{
							BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
							{
								Name = goodsInfo.Name,
								FiveElement = goodsInfo.FiveElement,
								GoodsId = goodsInfo.Id,
								Level = goodsInfo.Level,
								Rank = goodsInfo.Rank,
								Type = m_Current,
								RecyclePrice = goodsInfo.RecyclePrice
							};
							m_DataSource.Add(item);
						}
						if (m_DataSource != null || m_DataSource.Count != 0)
						{
							Addset(m_DataSource);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(waiWuXuanShangGoodsSearchResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void Addset(List<BaseNaJieGoodEntity> naJieData)
		{
			GameObject gmeObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			foreach (BaseNaJieGoodEntity naJieDatum in naJieData)
			{
				CreateObjItem(gmeObject, naJieDatum);
			}
			GridLayoutGroup component = m_Panel.ScrollRect.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(naJieData.Count, m_Panel.ScrollRect, component);
		}

		private void CreateObjItem(GameObject gmeObject, BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(m_Panel.ScrollRect, gmeObject);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, (object obj, GameObject gameObj) =>
			{
				ShuXing(data);
			});
		}

		private void ShuXing(BaseNaJieGoodEntity data)
		{
			m_Panel.RewardButton.interactable = true;
			m_XuanZhoWuPing = data;
			JiSuan();
		}

		private void JiSuan()
		{
			m_Panel.SuccessRateText.text = $"{(double)(m_LingShiShu / (m_XuanZhoWuPing.RecyclePrice * 20)) * 0.05 * 100.0 + (double)zhangLaoTisheng}%";
			m_Panel.ConsumeText.text = "1小时";
			m_Panel.ContributionText.text = $"{m_XuanZhoWuPing.Level * 20}点";
		}

		private static string TimeToString(int result)
		{
			int num = result * 3600000 / 60000;
			if (num > 0 && num < 2)
			{
				num = 1;
			}
			return $"{num}分钟";
		}

		private void ResourceLoad()
		{
			m_UpDownSortIconSprite = new Dictionary<UpDonwnSortStatus, Sprite>();
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		public override void CloseWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
