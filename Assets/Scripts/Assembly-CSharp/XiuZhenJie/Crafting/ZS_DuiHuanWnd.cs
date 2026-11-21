using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_DuiHuanWnd : Window
	{
		private ZS_DuiHuanPanel m_Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int m_PageSize = 10;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_LingZhiType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InstanceObj;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private PageModel<ZS_ZhenTuEntity> m_Result;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZS_DuiHuanPanel>();
			m_MaxPagNum = 1;
			m_CurrentPage = 1;
			m_CurrentWuXing = -1;
			m_CurrentPinJie = 0;
			m_CurrentStage = 0;
			ResourceLoad();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.ShaiXuanButton, ShaiXuanBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			m_Result = new PageModel<ZS_ZhenTuEntity>();
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.DengJie,
				ShaiXuanItemType.XSJ_ZhenFa
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
			m_ScrollContentExtend = m_Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
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
			m_Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
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
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			if (dictionary != null && dictionary.TryGetValue("五行", out var value))
			{
				int.TryParse(value.ItemTag, out var result);
				m_CurrentWuXing = ((result == 0) ? (-1) : result);
				if (value.ItemName != "全部")
				{
					text += value.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("品阶", out var value2))
			{
				int.TryParse(value2.ItemTag, out var result2);
				m_CurrentPinJie = result2;
				if (value2.ItemName != "全部")
				{
					text = text + " " + value2.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("等阶", out var value3))
			{
				int.TryParse(value3.ItemTag, out var result3);
				m_CurrentStage = result3;
				if (value3.ItemName != "全部")
				{
					text = text + " " + value3.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("类别", out var value4))
			{
				int.TryParse(value4.ItemTag, out var result4);
				m_LingZhiType = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			ShaiXuanXianShi(text);
			SelectFuShu();
		}

		private void SelectFuShu()
		{
			ClearData();
			EnterMethodMatrixExchangeReq_1008085 data = new EnterMethodMatrixExchangeReq_1008085
			{
				FiveElement = m_CurrentWuXing,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				Stage = m_CurrentStage,
				Type = m_LingZhiType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTERMETHODMATRIXEXCHANGE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterMethodMatrixExchangeResp_1008085 enterMethodMatrixExchangeResp_ = new EnterMethodMatrixExchangeResp_1008085();
					enterMethodMatrixExchangeResp_.MergeFrom(byteString);
					if (enterMethodMatrixExchangeResp_.IsSuccess)
					{
						m_Result.list.Clear();
						m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
						if (enterMethodMatrixExchangeResp_.Object != null && enterMethodMatrixExchangeResp_.Object.Count > 0)
						{
							foreach (EnterMethodMatrixExchangeResp_1008085.Types.ExchangeMethodMatrixObjectResp_1008086 item2 in enterMethodMatrixExchangeResp_.Object)
							{
								ZS_ZhenTuEntity item = new ZS_ZhenTuEntity(item2);
								m_Result.list.Add(item);
							}
						}
						else
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻阵图";
						}
						m_Result.pages = enterMethodMatrixExchangeResp_.Pages;
						m_Result.pageSize = enterMethodMatrixExchangeResp_.PageSize;
						m_Result.pageNum = enterMethodMatrixExchangeResp_.PageNum;
						m_MaxPagNum = m_Result.pages;
						CreateData(m_Result);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMethodMatrixExchangeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData(PageModel<ZS_ZhenTuEntity> result)
		{
			if (m_InstanceObj == null)
			{
				m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			}
			GridLayoutGroup component = m_Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (ZS_ZhenTuEntity item in result.list)
			{
				Common_XuShenJieSItem component2 = SetListContentParent(m_Panel.XianShiListRectTransform, m_InstanceObj).gameObject.GetComponent<Common_XuShenJieSItem>();
				m_GoodsIconSprite.TryGetValue(3, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItems(item, item.Name, item.FiveElement, item.Level, value2, value, ItemBtnClick, Common_XiShenJie.ZhenFa, item.ExchangeContribution);
			}
			SetGrticalListContentHight(result.list.Count, m_Panel.XianShiListRectTransform, component);
		}

		private void ItemBtnClick(object model, GameObject gameObj)
		{
			ZS_ZhenTuEntity entity = model as ZS_ZhenTuEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(entity.Id, entity.Level, entity.Rank, entity.FiveElement, (List<string> list) =>
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
			string text = $"使用贡献点可以兑换不超过当前阵师等级的阵图，所有的阵图兑换价格各不相同。每一种阵图兑换成功后，会将兑换所需的5%贡献基于阵图的创造者。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "兑换阵图");
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			m_Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			m_Panel.JingYanText.text = $"{Singleton<RoleManager>.Instance.GetZhenFaCurrentExp()}/{Singleton<RoleManager>.Instance.GetZhenFaShiMaxExp()}";
			m_Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentZhenFaShiLevelName() + "】";
		}

		private void ResourceLoad()
		{
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenTu.png");
			m_GoodsIconSprite.Add(3, value);
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
			foreach (Transform item in m_Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}
}
