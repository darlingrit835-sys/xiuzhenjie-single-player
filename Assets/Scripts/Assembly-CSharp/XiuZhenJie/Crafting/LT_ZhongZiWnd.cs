using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class LT_ZhongZiWnd : Window
	{
		public LT_ZhongZiPanel Panel;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<ZhoZiWuXingType, CanWuScrollItem> m_CanWuScrollItemDic;

		private List<BaseNaJieGoodEntity> m_Jin;

		private List<BaseNaJieGoodEntity> m_Mu;

		private List<BaseNaJieGoodEntity> m_Shui;

		private List<BaseNaJieGoodEntity> m_Huo;

		private List<BaseNaJieGoodEntity> m_Tu;

		private GameObject m_CanWuScrollItem;

		private GameObject m_CanWuObj;

		private int m_DanQian = 1;

		private LT_LingTianItem m_LingTianItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			ResourceLoad();
			Panel = base.GameObject.GetComponent<LT_ZhongZiPanel>();
			m_Jin = new List<BaseNaJieGoodEntity>();
			m_Mu = new List<BaseNaJieGoodEntity>();
			m_Shui = new List<BaseNaJieGoodEntity>();
			m_Huo = new List<BaseNaJieGoodEntity>();
			m_Tu = new List<BaseNaJieGoodEntity>();
			m_CanWuScrollItemDic = new Dictionary<ZhoZiWuXingType, CanWuScrollItem>();
			m_CanWuScrollItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/CanWuScrollItem.prefab");
			AddButtonClickListener(Panel.TuiChu, TuiChu);
			LingTianNumberNew();
			if (paralist.Length != 0)
			{
				m_DanQian = 2;
				m_LingTianItem = paralist[0] as LT_LingTianItem;
			}
			EnterCanWu();
		}

		private void ResourceLoad()
		{
			m_CanWuObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/LingTian_ZhoZi.png");
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

		private void LingYeClearData()
		{
			foreach (RectTransform item in Panel.ScrollContent.gameObject.GetComponent<RectTransform>())
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void EnterCanWu()
		{
			ClearNaJie();
			LingYeClearData();
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_SEED_RESP, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterSeedResp_1013011 enterSeedResp_ = new EnterSeedResp_1013011();
					enterSeedResp_.MergeFrom(byteString);
					if (enterSeedResp_.IsSuccess)
					{
						m_Jin.Clear();
						m_Mu.Clear();
						m_Shui.Clear();
						m_Huo.Clear();
						m_Tu.Clear();
						m_CanWuScrollItemDic.Clear();
						foreach (EnterSeedResp_1013011.Types.SeedInfoResp_1013012 item2 in enterSeedResp_.SeedInfo)
						{
							BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
							{
								Name = item2.Name,
								FiveElement = item2.FiveElement,
								GoodsId = item2.Id,
								Level = item2.Level,
								Rank = item2.Rank
							};
							switch ((ZhoZiWuXingType)item2.FiveElement)
							{
							case ZhoZiWuXingType.Jin:
								m_Jin.Add(item);
								CreateScrollItem("金系种子", ZhoZiWuXingType.Jin);
								break;
							case ZhoZiWuXingType.Mu:
								m_Mu.Add(item);
								CreateScrollItem("木系种子", ZhoZiWuXingType.Mu);
								break;
							case ZhoZiWuXingType.Shui:
								m_Shui.Add(item);
								CreateScrollItem("水系种子", ZhoZiWuXingType.Shui);
								break;
							case ZhoZiWuXingType.Huo:
								m_Huo.Add(item);
								CreateScrollItem("火系种子", ZhoZiWuXingType.Huo);
								break;
							case ZhoZiWuXingType.Tu:
								m_Tu.Add(item);
								CreateScrollItem("土系种子", ZhoZiWuXingType.Tu);
								break;
							default:
								throw new ArgumentOutOfRangeException();
							}
						}
						CreateData();
						LingTianNumberNew();
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateScrollItem(string titleName, ZhoZiWuXingType ints)
		{
			if (!m_CanWuScrollItemDic.ContainsKey(ints))
			{
				CanWuScrollItem component = SetListContentParent(Panel.ScrollContent, m_CanWuScrollItem).gameObject.GetComponent<CanWuScrollItem>();
				component.SetValue(titleName, this);
				m_CanWuScrollItemDic.Add(ints, component);
			}
		}

		private void ClearNaJie()
		{
			foreach (RectTransform item in Panel.ScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void CreateData()
		{
			CreateJin();
			CreateMu();
			CreateShui();
			CreateHuo();
			CreateHTu();
			float num = 0f;
			foreach (CanWuScrollItem value in m_CanWuScrollItemDic.Values)
			{
				num += value.GetContentHight();
			}
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, num);
		}

		private void CreateJin()
		{
			if (m_Jin.Count > 0)
			{
				foreach (BaseNaJieGoodEntity item in m_Jin)
				{
					ItemInit(item, ZhoZiWuXingType.Jin);
				}
				return;
			}
			if (m_CanWuScrollItemDic.ContainsKey(ZhoZiWuXingType.Jin))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[ZhoZiWuXingType.Jin];
				m_CanWuScrollItemDic.Remove(ZhoZiWuXingType.Jin);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
		}

		private void CreateMu()
		{
			if (m_Mu.Count > 0)
			{
				foreach (BaseNaJieGoodEntity item in m_Mu)
				{
					ItemInit(item, ZhoZiWuXingType.Mu);
				}
				return;
			}
			if (m_CanWuScrollItemDic.ContainsKey(ZhoZiWuXingType.Mu))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[ZhoZiWuXingType.Mu];
				m_CanWuScrollItemDic.Remove(ZhoZiWuXingType.Mu);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
		}

		private void CreateShui()
		{
			if (m_Shui.Count > 0)
			{
				foreach (BaseNaJieGoodEntity item in m_Shui)
				{
					ItemInit(item, ZhoZiWuXingType.Shui);
				}
				return;
			}
			if (m_CanWuScrollItemDic.ContainsKey(ZhoZiWuXingType.Shui))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[ZhoZiWuXingType.Shui];
				m_CanWuScrollItemDic.Remove(ZhoZiWuXingType.Shui);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
		}

		private void CreateHuo()
		{
			if (m_Huo.Count > 0)
			{
				foreach (BaseNaJieGoodEntity item in m_Huo)
				{
					ItemInit(item, ZhoZiWuXingType.Huo);
				}
				return;
			}
			if (m_CanWuScrollItemDic.ContainsKey(ZhoZiWuXingType.Huo))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[ZhoZiWuXingType.Huo];
				m_CanWuScrollItemDic.Remove(ZhoZiWuXingType.Huo);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
		}

		private void CreateHTu()
		{
			if (m_Tu.Count > 0)
			{
				foreach (BaseNaJieGoodEntity item in m_Tu)
				{
					ItemInit(item, ZhoZiWuXingType.Tu);
				}
				return;
			}
			if (m_CanWuScrollItemDic.ContainsKey(ZhoZiWuXingType.Tu))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[ZhoZiWuXingType.Tu];
				m_CanWuScrollItemDic.Remove(ZhoZiWuXingType.Tu);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
		}

		private void ItemInit(BaseNaJieGoodEntity entity, ZhoZiWuXingType shu)
		{
			RectTransform component = m_CanWuScrollItemDic[shu].gameObject.GetComponent<RectTransform>();
			UI_GoodsItem component2 = SetListContentParent(component, m_CanWuObj).GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(entity.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component2.SetItem(null, entity.Name, entity.FiveElement, entity.Level, value2, value, (object objects, GameObject gameObject) =>
			{
				switch (m_DanQian)
				{
				case 1:
					Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(entity.GoodsId, (List<string> list) =>
					{
						PopUpXiangQingPanel(entity.Name, "遗忘", Action2, list);
					});
					break;
				case 2:
					Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(entity.GoodsId, (List<string> list) =>
					{
						PopUpXiangQingPanel(entity.Name, "种植", Action, list);
					});
					break;
				}
			});
			m_CanWuScrollItemDic[shu].AddItem(component2);
			void Action(object obj)
			{
				ZhoZhi(entity.GoodsId, m_LingTianItem.m_ID, entity.Name);
			}
			void Action2(object obj)
			{
				YiWangYiCanWu(entity.GoodsId, entity.Name);
			}
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

		private void YiWangYiCanWu(string id, string name)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确定遗忘" + name + "?", () =>
			{
				ForgetSeedReq_1013020 data = new ForgetSeedReq_1013020
				{
					SeedId = id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.FORGET_SEED_RESP, data, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						ForgetSeedResp_1013020 forgetSeedResp_ = new ForgetSeedResp_1013020();
						forgetSeedResp_.MergeFrom(byteString);
						if (forgetSeedResp_.IsSuccess)
						{
							EnterCanWu();
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(forgetSeedResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}, isShowWaitUI: true);
			});
		}

		private void ZhoZhi(string seedId, int lanId, string zHiWuName)
		{
			SeedChooseReq_1013013 data = new SeedChooseReq_1013013
			{
				LandId = lanId,
				SeedId = seedId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEED_CHOOSE_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SeedChooseResp_1013013 seedChooseResp_ = new SeedChooseResp_1013013();
					seedChooseResp_.MergeFrom(byteString);
					if (seedChooseResp_.IsSuccess)
					{
						if (seedChooseResp_.TimeRemaining != 0L)
						{
							m_LingTianItem.m_Status = seedChooseResp_.LandStatus;
							m_LingTianItem.m_TimeRemaining = seedChooseResp_.TimeRemaining;
							m_LingTianItem.m_TotalWasteTime = seedChooseResp_.TotalWasteTime;
							m_LingTianItem.ZhiWuID = seedId;
							m_LingTianItem.ZhiWuName.text = zHiWuName;
							m_LingTianItem.ShuXingShuJu();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(seedChooseResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
						TuiChu();
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(seedChooseResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
					}
				}
			}, isShowWaitUI: true);
		}

		private void TuiChu()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void LingTianNumberNew()
		{
			Panel.LingTianNumber.text = $"已收藏{m_Huo.Count + m_Jin.Count + m_Mu.Count + m_Shui.Count + m_Tu.Count}种，可收藏种子{(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel + 50) / 5}种";
		}
	}
}
