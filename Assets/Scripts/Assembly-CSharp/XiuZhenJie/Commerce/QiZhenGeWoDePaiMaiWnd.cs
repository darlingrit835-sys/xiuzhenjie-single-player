using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeWoDePaiMaiWnd : Window
	{
		public QiZhenGeWoDePaiMaiPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private const int PAGE_SIZE = 10;

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
			Panel = base.GameObject.GetComponent<QiZhenGeWoDePaiMaiPanel>();
			Panel.transform.Find("Title_image/TitleName").GetComponent<Text>().text = "我的挂牌";
			m_DataSource = new PageModel<QZG_ItemModel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			if (paralist != null && paralist.Length == 1)
			{
				m_StoreType = (StoreType)paralist[0];
			}
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

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		public void PageChage(int targetPage)
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
			EnterMyAuctionReq_1006003 data = new EnterMyAuctionReq_1006003
			{
				Type = (int)m_StoreType,
				PageNum = m_SelectParams.pageNum,
				PageSize = m_SelectParams.pageSize
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.QZG_MY_SELECT_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterMyAuctionResp_1006003 enterMyAuctionResp_ = new EnterMyAuctionResp_1006003();
					enterMyAuctionResp_.MergeFrom(byteString);
					if (enterMyAuctionResp_.IsSuccess)
					{
						m_DataSource.pages = enterMyAuctionResp_.Pages;
						m_DataSource.pageNum = enterMyAuctionResp_.PageNum;
						m_DataSource.pageSize = enterMyAuctionResp_.PageSize;
						m_DataSource.list.Clear();
						foreach (EnterMyAuctionResp_1006003.Types.MyAuctionObjectResp_1006004 item2 in enterMyAuctionResp_.Object)
						{
							QZG_ItemModel item = new QZG_ItemModel
							{
								id = item2.Id,
								level = item2.Level,
								name = item2.Name,
								fiveElement = item2.FiveElement,
								goodsType = item2.GoodsType,
								remainHours = item2.RemainHours,
								stage = item2.Stage,
								rank = item2.Rank,
								fixedPrice = item2.FixedPrice,
								Num = item2.Count
							};
							m_DataSource.list.Add(item);
						}
						m_MaxPagNum = m_DataSource.pages;
						CreateData(m_DataSource.list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMyAuctionResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void CreateData(List<QZG_ItemModel> _data)
		{
			if (_data == null)
			{
				return;
			}
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/QZG_WoDePaiMaiItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ScrollContent.GetComponent<VerticalLayoutGroup>();
			foreach (QZG_ItemModel item in _data)
			{
				QiZhenGeWoDePaiMaiItem component2 = SetListContentParent(Panel.ScrollContent, gameObject).GetComponent<QiZhenGeWoDePaiMaiItem>();
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(item.goodsType, out value);
				int levelByRank = WorldSetting.GetLevelByRank(item.rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				component2.SetValue(item, value2, value, () =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention($"是否确定下架'{item.name}',下架后委托手续费不退回！", () =>
					{
						GoodsXiaJia(item);
					});
				}, item.Num);
			}
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, SetVerticalListContentHight(_data.Count, y, component));
		}

		private void GoodsXiaJia(QZG_ItemModel item)
		{
			AuctionGoodsCancelReq_1006009 data = new AuctionGoodsCancelReq_1006009
			{
				AuctionId = item.id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.QZG_GOODS_XIAJIA, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AuctionGoodsCancelResp_1006009 auctionGoodsCancelResp_ = new AuctionGoodsCancelResp_1006009();
					auctionGoodsCancelResp_.MergeFrom(byteString);
					if (auctionGoodsCancelResp_.IsSuccess)
					{
						PageChage(m_CurrentPage);
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(auctionGoodsCancelResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void ClearItems()
		{
			foreach (Transform item in Panel.ScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
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

		public override void OnClose()
		{
			base.OnClose();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Remove(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.XiaLaAction = (UnityAction)Delegate.Remove(scrollContentExtend2.XiaLaAction, new UnityAction(XiaLaAction));
			}
			if (m_KeyWnd != null)
			{
				KeyBoardWnd keyWnd = m_KeyWnd;
				keyWnd.ChangePageAction = (Action<int>)Delegate.Remove(keyWnd.ChangePageAction, new Action<int>(PageChage));
			}
		}
	}
}
