using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AH_WoDeGuaPaiWnd : Window
	{
		public AH_WoDeGuaPaiPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private const int PageSize = 10;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private KeyBoardWnd m_KeyWnd;

		private PageModel<QZG_ItemModel> m_DataSource;

		private QZG_SearchWuPinListParamModel m_SelectParams;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private StoreType m_StoreType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<AH_WoDeGuaPaiPanel>();
			m_DataSource = new PageModel<QZG_ItemModel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			ResourceLoad();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_SelectParams = new QZG_SearchWuPinListParamModel
			{
				pageNum = 1,
				pageSize = 10
			};
			m_ScrollContentExtend = Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Combine(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.XiaLaAction = (UnityAction)Delegate.Combine(scrollContentExtend2.XiaLaAction, new UnityAction(XiaLaAction));
			}
			PageChage(m_CurrentPage);
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
			m_SelectParams.pageNum = targetPage;
			m_SelectParams.pageSize = 10;
			SelectGoods();
			ShowKeyBoardWnd();
		}

		private void SelectGoods()
		{
			ClearItems();
			SelectGoodsRequest();
		}

		private void ClearItems()
		{
			foreach (Transform item in Panel.ScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
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

		private void SelectGoodsRequest()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ON_SHELF_LIST_RESP, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					OnShelfListResp_1006014 onShelfListResp_ = new OnShelfListResp_1006014();
					onShelfListResp_.MergeFrom(byteString);
					if (onShelfListResp_.IsSuccess)
					{
						m_DataSource.list.Clear();
						foreach (OnShelfListResp_1006014.Types.OnShelfGoodsResp_1006015 goods in onShelfListResp_.GoodsList)
						{
							QZG_ItemModel item = new QZG_ItemModel
							{
								id = goods.Id.ToString(),
								level = goods.Level,
								name = goods.Name,
								fiveElement = goods.FiveElement,
								goodsType = goods.GoodsType,
								remainHours = (int)goods.AuctionTime,
								stage = goods.Stage,
								rank = goods.Rank,
								fixedPrice = goods.StartPrice
							};
							m_DataSource.list.Add(item);
						}
						m_MaxPagNum = m_DataSource.pages;
						CreateData(m_DataSource.list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(onShelfListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData(List<QZG_ItemModel> data)
		{
			if (data == null)
			{
				return;
			}
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/QZG_WoDePaiMaiItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ScrollContent.GetComponent<VerticalLayoutGroup>();
			foreach (QZG_ItemModel item in data)
			{
				QiZhenGeWoDePaiMaiItem component2 = SetListContentParent(Panel.ScrollContent, gameObject).GetComponent<QiZhenGeWoDePaiMaiItem>();
				m_GoodsIconSprite.TryGetValue(item.goodsType, out var value);
				int levelByRank = WorldSetting.GetLevelByRank(item.rank);
				m_RankBgSprite.TryGetValue(levelByRank, out var value2);
				component2.SetValue(item, value2, value, () =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("是否确定下架'" + item.name + "'！", () =>
					{
						GoodsXiaJia(item);
					});
				}, item.Num);
			}
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, SetVerticalListContentHight(data.Count, y, component));
		}

		private void GoodsXiaJia(QZG_ItemModel item)
		{
			TreasureGoodsCancelReq_1006016 data = new TreasureGoodsCancelReq_1006016
			{
				TreasureId = Convert.ToInt32(item.id)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TREASURE_GOODS_CANCEL_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					TreasureGoodsCancelResp_1006016 treasureGoodsCancelResp_ = new TreasureGoodsCancelResp_1006016();
					treasureGoodsCancelResp_.MergeFrom(byteString);
					if (treasureGoodsCancelResp_.IsSuccess)
					{
						PageChage(m_CurrentPage);
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(treasureGoodsCancelResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
