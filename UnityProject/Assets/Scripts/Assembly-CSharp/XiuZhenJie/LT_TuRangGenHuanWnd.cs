using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LT_TuRangGenHuanWnd : Window
	{
		public LT_TuRangGenHuanPanel Panel;

		private GameObject m_InsObj;

		private GameObject m_TitleInsObj;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private LingTianEntily m_TargetEntity;

		private List<LingTianEntily> m_LingTianEntily;

		private List<string> ShuJu;

		private LT_LingTianItem m_LingTianItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LT_TuRangGenHuanPanel>();
			if (paralist.Length != 0)
			{
				m_LingTianItem = paralist[0] as LT_LingTianItem;
			}
			AddButtonClickListener(Panel.GuanBiButton, CloseWndClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			m_TargetEntity = new LingTianEntily();
			ResourceLoad();
			TitleClick();
			BiaoTi();
		}

		private void BiaoTi()
		{
			Panel.TitleName.text = $"第{m_LingTianItem.m_ID}块灵田-土壤";
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/LingTian_LingTian.png");
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

		private void TitleClick()
		{
			EnterSoilReq_1013004 data = new EnterSoilReq_1013004
			{
				LandId = m_LingTianItem.m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_SOIL_RESP, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterSoilResp_1013004 enterSoilResp_ = new EnterSoilResp_1013004();
					m_LingTianEntily = new List<LingTianEntily>();
					enterSoilResp_.MergeFrom(byteString);
					if (enterSoilResp_.IsSuccess)
					{
						if (enterSoilResp_.CurrentSoil != null)
						{
							m_TargetEntity.Level = enterSoilResp_.CurrentSoil.Level;
							m_TargetEntity.Name = enterSoilResp_.CurrentSoil.Name;
							m_TargetEntity.FiveElement = enterSoilResp_.CurrentSoil.FiveElement;
							m_TargetEntity.GoodsId = enterSoilResp_.CurrentSoil.Id;
							m_TargetEntity.Level = enterSoilResp_.CurrentSoil.Level;
							m_TargetEntity.Rank = enterSoilResp_.CurrentSoil.Rank;
							m_TargetEntity.ConversionRate = enterSoilResp_.CurrentSoil.ConversionRate;
							m_TargetEntity.ReceiveVal = enterSoilResp_.CurrentSoil.ReceiveVal;
							ChoosedItem(isOn: true, m_TargetEntity, base.GameObject);
							m_TargetEntity = new LingTianEntily();
						}
						if (enterSoilResp_.SoilAll.Count != 0 && enterSoilResp_.SoilAll[0].Name != null)
						{
							foreach (EnterSoilResp_1013004.Types.SoilResp_1013005 item2 in enterSoilResp_.SoilAll)
							{
								LingTianEntily item = new LingTianEntily
								{
									Name = item2.Name,
									FiveElement = item2.FiveElement,
									GoodsId = item2.Id,
									Level = item2.Level,
									Rank = item2.Rank,
									ConversionRate = item2.ConversionRate,
									ReceiveVal = item2.ReceiveVal
								};
								m_LingTianEntily.Add(item);
							}
						}
						else
						{
							m_LingTianEntily.Clear();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSoilResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						CloseWndClick();
					}
					FuZhi();
				}
			});
		}

		private void FuZhi()
		{
			if (m_LingTianEntily == null)
			{
				return;
			}
			foreach (LingTianEntily item in m_LingTianEntily)
			{
				ItemInit(item);
			}
		}

		private void ItemInit(LingTianEntily Entity)
		{
			GameObject gameObject = SetListContentParent(Panel.BoxContentScrollViewContent, m_InsObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsTogItem component = gameObject.GetComponent<UI_GoodsTogItem>();
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(1, out value);
			int levelByRank = WorldSetting.GetLevelByRank(Entity.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			component.SetValue(group: Panel.BoxContentScrollViewContent.gameObject.GetComponent<ToggleGroup>(), dataSource: Entity, name: Entity.Name, fiveElement: Entity.FiveElement, level: Entity.Level, rankSpr: value2, iconSpr: value, ClickAction: ChoosedItem);
		}

		private void ChoosedItem(bool isOn, object data, GameObject gameObj)
		{
			if (isOn)
			{
				LingTianEntily lingTianEntily = data as LingTianEntily;
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(1, out value);
				int levelByRank = WorldSetting.GetLevelByRank(lingTianEntily.Rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				Panel.ChooseItem.SetItem(lingTianEntily, lingTianEntily.Name, lingTianEntily.FiveElement, lingTianEntily.Level, value2, value, null, isUsing: true);
				m_TargetEntity = lingTianEntily;
				CreateXiangQing();
			}
			else
			{
				m_TargetEntity = new LingTianEntily();
			}
		}

		private void CreateXiangQing()
		{
			Panel.ChooseTitleText.text = m_TargetEntity.Name;
			Panel.One.text = string.Format("{0}:{1}", "五行", WorldSetting.GetWuXingName((WuXing)m_TargetEntity.FiveElement));
			Panel.Two.text = string.Format("{0}:{1}", "品阶", WorldSetting.GetRankAllName(m_TargetEntity.Rank));
			Panel.Three.text = string.Format("{0}:{1}", "灵衍值", m_TargetEntity.ConversionRate);
			Panel.Four.text = string.Format("{0}:{1}", "蕴灵值", m_TargetEntity.ReceiveVal);
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenClick()
		{
			if (m_TargetEntity.GoodsId == null)
			{
				return;
			}
			ChooseSoilReq_1013006 data = new ChooseSoilReq_1013006
			{
				LandId = m_LingTianItem.m_ID,
				SoilId = m_TargetEntity.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHOOSE_SOIL_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ChooseSoilResp_1013006 chooseSoilResp_ = new ChooseSoilResp_1013006();
					m_LingTianEntily = new List<LingTianEntily>();
					chooseSoilResp_.MergeFrom(byteString);
					if (chooseSoilResp_.IsSuccess)
					{
						m_LingTianItem.m_Status = chooseSoilResp_.LandStatus;
						m_LingTianItem.ShuXingShuJu();
						CloseWndClick();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(chooseSoilResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}
	}
}
