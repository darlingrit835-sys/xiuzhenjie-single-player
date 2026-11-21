using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_LingZhiDuiHuanWnd : Window
	{
		public LZ_LingZhiDuiHuanPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int m_PageSize = 10;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentSiQi;

		private int m_CurrentWuWei;

		private int m_CurrentBuXie;

		private int m_CurrentGuiJing;

		private int m_LingZhiType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InstanceObj;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private PageModel<LZ_LingZhiEntity> m_Result;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LZ_LingZhiDuiHuanPanel>();
			m_MaxPagNum = 1;
			m_CurrentPage = 1;
			m_CurrentWuXing = -1;
			m_CurrentPinJie = 0;
			m_CurrentStage = 0;
			Panel.TitleNameText.text = "灵植兑换";
			ResourceLoad();
			AddButtonClickListener(Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(Panel.ShaiXuanButton, ShaiXuanBtn);
			AddButtonClickListener(Panel.WenHaoButton, WenHaoTogClick);
			m_Result = new PageModel<LZ_LingZhiEntity>();
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.PinJie,
				ShaiXuanItemType.DengJie,
				ShaiXuanItemType.XSJ_LingZhi
			}, true) as Common_ShaiXuanWnd;
			if (m_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
				shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(ShaiXuanType));
				Common_ShaiXuanWnd shaiXuanWnd2 = m_ShaiXuanWnd;
				shaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(shaiXuanWnd2.ReChooseAction, new UnityAction(ShaiXuanType));
			}
			ShaiXuanXianShi(" ");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_ScrollContentExtend = Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Combine(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.XiaLaAction = (UnityAction)Delegate.Combine(scrollContentExtend2.XiaLaAction, new UnityAction(XiaLaAction));
			}
			m_ShaiXuanWnd.PopUpPanel(isPop: true);
			SelectFuShu();
			ReFlashData();
		}

		private void ShangLaAction()
		{
			m_CurrentPage--;
			if (m_CurrentPage <= 0)
			{
				m_CurrentPage = 1;
			}
			PageChage(m_CurrentPage);
		}

		private void XiaLaAction()
		{
			if (m_CurrentPage != m_MaxPagNum)
			{
				m_CurrentPage++;
				if (m_CurrentPage >= m_MaxPagNum)
				{
					m_CurrentPage = m_MaxPagNum;
				}
				PageChage(m_CurrentPage);
			}
		}

		private void PageChage(int targetPage)
		{
			if (targetPage > m_MaxPagNum)
			{
				targetPage = m_MaxPagNum;
			}
			if (targetPage <= 0)
			{
				targetPage = 1;
			}
			m_CurrentPage = targetPage;
			SelectFuShu();
			ShowKeyBoardWnd();
			Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		private void ShowKeyBoardWnd()
		{
			if (m_KeyWnd == null)
			{
				Singleton<UIManager>.Instance.PopUpWnd("KeyBoardWnd.prefab", true, m_CurrentPage, m_MaxPagNum);
				m_KeyWnd = Singleton<UIManager>.Instance.FindWndByName<KeyBoardWnd>("KeyBoardWnd.prefab");
				KeyBoardWnd keyWnd = m_KeyWnd;
				keyWnd.ChangePageAction = (Action<int>)Delegate.Remove(keyWnd.ChangePageAction, new Action<int>(PageChage));
				KeyBoardWnd keyWnd2 = m_KeyWnd;
				keyWnd2.ChangePageAction = (Action<int>)Delegate.Combine(keyWnd2.ChangePageAction, new Action<int>(PageChage));
			}
			else
			{
				m_KeyWnd.OnShow(m_CurrentPage, m_MaxPagNum);
			}
		}

		private void ShaiXuanType()
		{
			string text = "";
			Dictionary<string, ShaiXuanParamsModel> obj = m_ShaiXuanWnd?.GetResult();
			ShaiXuanParamsModel value = null;
			if (obj.TryGetValue("五行", out value))
			{
				int result = -1;
				int.TryParse(value.ItemTag, out result);
				m_CurrentWuXing = ((result == 0) ? (-1) : result);
				if (value.ItemName != "全部")
				{
					text += value.ItemName;
				}
			}
			ShaiXuanParamsModel value2 = null;
			if (obj.TryGetValue("品阶", out value2))
			{
				int result2 = 0;
				int.TryParse(value2.ItemTag, out result2);
				m_CurrentPinJie = result2;
				if (value2.ItemName != "全部")
				{
					text = text + " " + value2.ItemName;
				}
			}
			if (obj.TryGetValue("等阶", out var value3))
			{
				int result3 = 0;
				int.TryParse(value3.ItemTag, out result3);
				m_CurrentStage = result3;
				if (value3.ItemName != "全部")
				{
					text = text + " " + value3.ItemName;
				}
			}
			ShaiXuanParamsModel value4 = null;
			if (obj.TryGetValue("四气", out value4))
			{
				int result4 = 0;
				int.TryParse(value4.ItemTag, out result4);
				m_CurrentSiQi = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			ShaiXuanParamsModel value5 = null;
			if (obj.TryGetValue("五味", out value5))
			{
				int result5 = 0;
				int.TryParse(value5.ItemTag, out result5);
				m_CurrentWuWei = result5;
				if (value5.ItemName != "全部")
				{
					text = text + " " + value5.ItemName;
				}
			}
			ShaiXuanParamsModel value6 = null;
			if (obj.TryGetValue("补泻", out value6))
			{
				int result6 = 0;
				int.TryParse(value6.ItemTag, out result6);
				m_CurrentBuXie = result6;
				if (value6.ItemName != "全部")
				{
					text = text + " " + value6.ItemName;
				}
			}
			ShaiXuanParamsModel value7 = null;
			if (obj.TryGetValue("归经", out value7))
			{
				int result7 = 0;
				int.TryParse(value7.ItemTag, out result7);
				m_CurrentGuiJing = result7;
				if (value7.ItemName != "全部")
				{
					text = text + " " + value7.ItemName;
				}
			}
			ShaiXuanXianShi(text);
			SelectFuShu();
		}

		private void SelectFuShu()
		{
			ClearData();
			EnterTemplatePlantExchangeReq_1008055 data = new EnterTemplatePlantExchangeReq_1008055
			{
				FiveElement = m_CurrentWuXing,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				Stage = m_CurrentStage,
				SiQi = m_CurrentSiQi,
				WuWei = m_CurrentWuWei,
				BuXie = m_CurrentBuXie,
				GuiJing = m_CurrentGuiJing
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_TEMPLATE_PLANT_EXCHANGE_RESP, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterTemplatePlantExchangeResp_1008055 enterTemplatePlantExchangeResp_ = new EnterTemplatePlantExchangeResp_1008055();
					enterTemplatePlantExchangeResp_.MergeFrom(byteString);
					if (enterTemplatePlantExchangeResp_.IsSuccess)
					{
						m_Result.list.Clear();
						Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
						if (enterTemplatePlantExchangeResp_.Object != null && enterTemplatePlantExchangeResp_.Object.Count > 0)
						{
							foreach (EnterTemplatePlantExchangeResp_1008055.Types.ExchangeTemplatePlantObjectResp_1008056 item2 in enterTemplatePlantExchangeResp_.Object)
							{
								LZ_LingZhiEntity item = new LZ_LingZhiEntity(item2);
								m_Result.list.Add(item);
							}
						}
						else
						{
							Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻灵植";
						}
						m_Result.pages = enterTemplatePlantExchangeResp_.Pages;
						m_Result.pageSize = enterTemplatePlantExchangeResp_.PageSize;
						m_Result.pageNum = enterTemplatePlantExchangeResp_.PageNum;
						m_MaxPagNum = m_Result.pages;
						CreateData(m_Result);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterTemplatePlantExchangeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData(PageModel<LZ_LingZhiEntity> result)
		{
			if (m_InstanceObj == null)
			{
				m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			}
			GridLayoutGroup component = Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (LZ_LingZhiEntity item in result.list)
			{
				Common_XuShenJieSItem component2 = SetListContentParent(Panel.XianShiListRectTransform, m_InstanceObj).gameObject.GetComponent<Common_XuShenJieSItem>();
				m_GoodsIconSprite.TryGetValue(12, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItems(item, item.Name, item.FiveElement, item.Level, value2, value, ItemBtnClick, Common_XiShenJie.LZ, item.ExchangeContribution);
			}
			SetGrticalListContentHight(result.list.Count, Panel.XianShiListRectTransform, component);
		}

		private void ItemBtnClick(object model, GameObject gameObj)
		{
			LZ_LingZhiEntity entity = model as LZ_LingZhiEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(entity.Id, (List<string> list) =>
				{
					PopUpXiangQingPanel(entity.Name, "确定", null, list);
				});
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

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd(m_ShaiXuanWnd);
		}

		private void ShaiXuanBtn()
		{
			m_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void WenHaoTogClick()
		{
			string text = $"使用贡献点可以兑换不超过当前灵植师品阶的灵植，不同的灵植师兑换的价格不同。每一种灵植兑换成功后，会将兑换所需贡献的5%给予灵植的创造者。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "兑换灵植");
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.JingYanText.text = Singleton<RoleManager>.Instance.GetLingZhiCurrentExp() + "/" + Singleton<RoleManager>.Instance.GetLingZhiMaxExp();
			Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentLingZhiLevelName() + "】";
		}

		private void ResourceLoad()
		{
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhiWuShu.png");
			m_GoodsIconSprite.Add(12, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void ClearData()
		{
			foreach (Transform item in Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}
}
