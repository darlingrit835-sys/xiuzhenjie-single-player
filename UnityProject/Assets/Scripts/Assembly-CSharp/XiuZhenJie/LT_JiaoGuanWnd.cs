using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class LT_JiaoGuanWnd : Window
	{
		public LT_JiaoGuanPanel Panel;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private List<LingYeEntily> m_LingTianEntily;

		private List<string> ShuJu;

		private LT_LingTianItem m_LingTianItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LT_JiaoGuanPanel>();
			AddButtonClickListener(Panel.GuanBiBoButton, CloseWndClick);
			AddButtonClickListener(Panel.Lingua, Lingua);
			AddButtonClickListener(Panel.Lingub, Lingub);
			AddButtonClickListener(Panel.Linguc, Linguc);
			AddButtonClickListener(Panel.Lingud, Lingud);
			if (paralist.Length != 0)
			{
				m_LingTianItem = paralist[0] as LT_LingTianItem;
			}
			ResourceLoad();
			AssignmentId();
			Clean();
			Obtain();
		}

		private void Obtain()
		{
			EnterIrrigateReq_1013014 data = new EnterIrrigateReq_1013014
			{
				LandId = m_LingTianItem.m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_IRRIGATE_RESP, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterIrrigateResp_1013014 enterIrrigateResp_ = new EnterIrrigateResp_1013014();
					m_LingTianEntily = new List<LingYeEntily>();
					enterIrrigateResp_.MergeFrom(byteString);
					if (enterIrrigateResp_.IsSuccess)
					{
						if (enterIrrigateResp_.WaterInfo.Count == 0 || enterIrrigateResp_.WaterInfo == null)
						{
							string textValue = "当前无灵液可浇灌！";
							MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
							CloseWndClick();
						}
						else
						{
							foreach (EnterIrrigateResp_1013014.Types.WaterInfoResp_1013015 item2 in enterIrrigateResp_.WaterInfo)
							{
								LingYeEntily item = new LingYeEntily
								{
									Name = item2.Name,
									FiveElement = item2.FiveElement,
									GoodsId = item2.Id,
									Level = item2.Level,
									Rank = item2.Rank,
									ConversionRate = item2.ConversionRate,
									ReceiveVal = item2.ReceiveVal,
									ReduceTime = item2.ReduceTime
								};
								m_LingTianEntily.Add(item);
							}
							Buttons();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterIrrigateResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void Clean()
		{
			Panel.Lingua.gameObject.SetActive(value: false);
			Panel.Lingub.gameObject.SetActive(value: false);
			Panel.Linguc.gameObject.SetActive(value: false);
			Panel.Lingud.gameObject.SetActive(value: false);
		}

		private void Buttons()
		{
			for (int i = 0; i < m_LingTianEntily.Count; i++)
			{
				switch (i)
				{
				case 0:
				{
					Panel.Lingua.gameObject.SetActive(value: true);
					Panel.LingaText.text = $"加速{m_LingTianEntily[i].ReduceTime / 60 / 1000}分钟";
					Sprite value7 = null;
					m_GoodsIconSprite.TryGetValue(1, out value7);
					int levelByRank4 = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
					Sprite value8 = null;
					m_RankBgSprite.TryGetValue(levelByRank4, out value8);
					Panel.UI_Goodsa.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[0].FiveElement, m_LingTianEntily[0].Level, value8, value7, (object _obj, GameObject _gameObj) =>
					{
						Lingua();
					});
					break;
				}
				case 1:
				{
					Panel.Lingub.gameObject.SetActive(value: true);
					Panel.LingbText.text = $"加速{m_LingTianEntily[i].ReduceTime / 60 / 1000}分钟";
					Sprite value5 = null;
					m_GoodsIconSprite.TryGetValue(1, out value5);
					int levelByRank3 = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
					Sprite value6 = null;
					m_RankBgSprite.TryGetValue(levelByRank3, out value6);
					Panel.UI_Goodsb.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value6, value5, (object _obj, GameObject _gameObj) =>
					{
						Lingub();
					});
					break;
				}
				case 2:
				{
					Panel.Linguc.gameObject.SetActive(value: true);
					Panel.LingcText.text = $"加速{m_LingTianEntily[i].ReduceTime / 60 / 1000}分钟";
					Sprite value3 = null;
					m_GoodsIconSprite.TryGetValue(1, out value3);
					int levelByRank2 = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
					Sprite value4 = null;
					m_RankBgSprite.TryGetValue(levelByRank2, out value4);
					Panel.UI_Goodsc.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value4, value3, (object _obj, GameObject _gameObj) =>
					{
						Linguc();
					});
					break;
				}
				case 3:
				{
					Panel.Lingud.gameObject.SetActive(value: true);
					Panel.LingdText.text = $"加速{m_LingTianEntily[i].ReduceTime / 60 / 1000}分钟";
					Sprite value = null;
					m_GoodsIconSprite.TryGetValue(1, out value);
					int levelByRank = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
					Sprite value2 = null;
					m_RankBgSprite.TryGetValue(levelByRank, out value2);
					Panel.UI_Goodsd.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value2, value, (object _obj, GameObject _gameObj) =>
					{
						Lingud();
					});
					break;
				}
				}
			}
		}

		private void Lingua()
		{
			FuZhi(0);
		}

		private void Lingub()
		{
			FuZhi(1);
		}

		private void Linguc()
		{
			FuZhi(2);
		}

		private void Lingud()
		{
			FuZhi(3);
		}

		private void FuZhi(int a)
		{
			List<string> list = new List<string>();
			list.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(m_LingTianEntily[a].Level)));
			list.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)m_LingTianEntily[a].FiveElement)));
			list.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(m_LingTianEntily[a].Rank)));
			list.Add(string.Format("{0},{1}", "灵衍值", m_LingTianEntily[a].ConversionRate));
			list.Add(string.Format("{0},{1}", "蕴灵值", m_LingTianEntily[a].ReceiveVal));
			UnityAction<object> sureBtnCallBack = (object obj) =>
			{
				JiaoGuan(a);
			};
			PopUpXiangQingPanel(m_LingTianEntily[a].Name, "浇灌", sureBtnCallBack, list);
		}

		private void JiaoGuan(int Shu)
		{
			WaterChooseReq_1013016 data = new WaterChooseReq_1013016
			{
				WaterId = m_LingTianEntily[Shu].GoodsId,
				LandId = m_LingTianItem.m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.WATER_CHOOSE_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					WaterChooseResp_1013016 waterChooseResp_ = new WaterChooseResp_1013016();
					waterChooseResp_.MergeFrom(byteString);
					if (waterChooseResp_.IsSuccess)
					{
						m_LingTianItem.m_Status = waterChooseResp_.LandStatus;
						m_LingTianItem.m_TimeRemaining = waterChooseResp_.TimeRemaining;
						m_LingTianItem.m_TotalWasteTime = waterChooseResp_.TotalWasteTime;
						m_LingTianItem.ShuXingShuJu();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(waterChooseResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					CloseWndClick();
				}
			});
		}

		private void AssignmentId()
		{
			string text = $"第{m_LingTianItem.m_ID}块灵田-收获";
			Panel.TiTleText.text = text;
		}

		private void PopUpXiangQingPanel(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/LingTian_LingYe.png");
			m_GoodsIconSprite.Add(1, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
