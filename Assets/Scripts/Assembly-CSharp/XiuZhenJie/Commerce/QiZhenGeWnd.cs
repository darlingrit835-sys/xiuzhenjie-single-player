using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeWnd : Window
	{
		public QiZhenGePanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private const int PAGE_SIZE = 10;

		private Common_ShaiXuanWnd m_Common_ShaiXuanWnd;

		private PageModel<QZG_ItemModel> m_DataSource;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Text m_PriceSortLabel;
		private Text m_RankSortLabel;
		private int m_SearchRevision;
		private EnterAuctionReq_1006001 m_LastBrowse;
		private float m_LastBrowseAt;

		private QZG_SearchWuPinListParamModel m_SelectParams;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private UpDonwnSortStatus m_JiaGeSortStatus;

		private UpDonwnSortStatus m_RankSortStatus;


		private StoreType m_CurrentStoreType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<QiZhenGePanel>();
			m_PriceSortLabel = Panel.DangQianJiaGe.GetComponentInChildren<Text>(true);
			m_RankSortLabel = Panel.PinJieSortBtn.GetComponentInChildren<Text>(true);
			Panel.DangQianJiaGe.transform.Find("DangQianJiaGe_image")?.gameObject.SetActive(false);
			Panel.PinJieSortBtn.transform.Find("PinJie_image")?.gameObject.SetActive(false);
			StyleSortButton(Panel.DangQianJiaGe, m_PriceSortLabel);
			StyleSortButton(Panel.PinJieSortBtn, m_RankSortLabel);
			InkInterfaceArt.Set(Panel.transform.Find("TitleBg").GetComponent<Image>(), "LightPaper", false);
			InkDialogArt.StyleButton(Panel.ShaiXuan, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(Panel.SelectBtn, "JadeButton", InkDialogArt.Ivory);
			Panel.WeiTuoPaiMai.GetComponentInChildren<Text>(true).text = "标价\n上架";
			if (Panel.SouSuo.placeholder is Text placeholder) placeholder.text = "输入物品名";
			Panel.EmptyGoodsPanel.GetComponentInChildren<Text>(true).text = "本坊暂无符合条件的挂牌。可更换分类或清除筛选。";
			AddButtonClickListener(Panel.DangQianJiaGe, ChangePriceSort);
			AddButtonClickListener(Panel.PinJieSortBtn, ChangeRankSort);
			AddButtonClickListener(Panel.ShaiXuan, ShaiXuanBtnClick);
			AddButtonClickListener(Panel.LiKai, LiKaiBtnClick);
			AddButtonClickListener(Panel.WeiTuoPaiMai, WeiTuoPaiMaiBtnClick);
			AddButtonClickListener(Panel.ChooseBtn, ChooseBtnClick);
			AddButtonClickListener(Panel.SelectBtn, SelectGoods);
			m_ScrollContentExtend = Panel.Scroll.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				m_ScrollContentExtend.ShangLaAction += ShangLaAction;
				m_ScrollContentExtend.XiaLaAction += XiaLaAction;
			}
			ResourceLoad();
		}

		private static void StyleSortButton(Button button, Text label)
		{
			// The rank button used its arrow as the target graphic, while the price button has its own surface.
			var surface = button.GetComponent<Image>() ?? button.gameObject.AddComponent<Image>();
			button.targetGraphic = surface;
			InkDialogArt.StyleButton(button, "ParchmentHeading", InkInterfaceArt.Ink);
			InkDialogArt.Inset(label.rectTransform, 3, 3, 0, 0);
			label.alignment = TextAnchor.MiddleCenter;
			label.raycastTarget = false;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length > 0 && paralist[0] is StoreType store)
			{
				if (store != m_CurrentStoreType) Panel.SouSuo.text = "";
				m_CurrentStoreType = store;
			}
			m_LastBrowse = null;
			Init();
			FlashShaiXuanData();
		}

		private void Init()
		{
			m_DataSource = new PageModel<QZG_ItemModel>();
			m_JiaGeSortStatus = UpDonwnSortStatus.None;
			m_RankSortStatus = UpDonwnSortStatus.None;
			m_CurrentPage = 1;
			m_MaxPagNum = 1;
			m_SelectParams = new QZG_SearchWuPinListParamModel
			{
				pageNum = 1,
				pageSize = PAGE_SIZE,
				fiveElement = -1,
				name = (string.IsNullOrEmpty(Panel.SouSuo.text) ? null : Panel.SouSuo.text)
			};
			List<ShaiXuanItemType> list = new List<ShaiXuanItemType>();
			switch (m_CurrentStoreType)
			{
			case StoreType.FaQi:
				Panel.TitleText.text = "百器坊 · 法器";
				list.Add(ShaiXuanItemType.QZG_FaQi);
				list.Add(ShaiXuanItemType.QZG_FaQiType);
				break;
			case StoreType.GongFa:
				Panel.TitleText.text = "道经阁 · 功法";
				list.Add(ShaiXuanItemType.QZG_GongFa);
				list.Add(ShaiXuanItemType.GongFaType);
				break;
			case StoreType.FaShu:
				Panel.TitleText.text = "术法阁 · 法术";
				list.Add(ShaiXuanItemType.QZG_FaShu);
				list.Add(ShaiXuanItemType.FaShuType);
				break;
			case StoreType.DanYao:
				Panel.TitleText.text = "灵丹坊 · 丹药";
				list.Add(ShaiXuanItemType.QZG_DanYao);
				list.Add(ShaiXuanItemType.DanFangType);
				break;
			case StoreType.FuLu:
				Panel.TitleText.text = "神符阁 · 符箓";
				list.Add(ShaiXuanItemType.QZG_FuLu);
				list.Add(ShaiXuanItemType.FaShuType);
				break;
			case StoreType.QiTa:
				Panel.TitleText.text = "杂货铺 · 材料";
				list.Add(ShaiXuanItemType.QZG_QiTa);
				break;
			case StoreType.ZhenFa:
				Panel.TitleText.text = "阵法阁 · 阵法";
				list.Add(ShaiXuanItemType.QZG_ZhenFa);
				break;
			}
			UpdateSortLabels();
			list.Add(ShaiXuanItemType.WuXing);
			list.Add(ShaiXuanItemType.PinJie);
			list.Add(ShaiXuanItemType.DengJie);
			m_Common_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, list, true) as Common_ShaiXuanWnd;
			if (m_Common_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd common_ShaiXuanWnd = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(common_ShaiXuanWnd.ReChooseAction, new UnityAction(FlashShaiXuanData));
				Common_ShaiXuanWnd common_ShaiXuanWnd2 = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(common_ShaiXuanWnd2.ReChooseAction, new UnityAction(FlashShaiXuanData));
			}
		}

		private void FlashShaiXuanData()
		{
			Dictionary<string, ShaiXuanParamsModel> result = m_Common_ShaiXuanWnd.GetResult();
			switch (m_CurrentStoreType)
			{
			case StoreType.FaQi:
			{
				ShaiXuanParamsModel value10 = null;
				result.TryGetValue("类别", out value10);
				ShaiXuanParamsModel value11 = null;
				result.TryGetValue("功能类别", out value11);
				if (value10 != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value10.ItemTag)) ? int.Parse(value10.ItemTag) : 0);
				}
				if (value11 != null)
				{
					m_SelectParams.smallType = ((!string.IsNullOrEmpty(value11.ItemTag)) ? int.Parse(value11.ItemTag) : 0);
				}
				break;
			}
			case StoreType.GongFa:
			{
				ShaiXuanParamsModel value8 = null;
				result.TryGetValue("类别", out value8);
				if (value8 != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value8.ItemTag)) ? int.Parse(value8.ItemTag) : 0);
				}
				ShaiXuanParamsModel value9 = null;
				result.TryGetValue("功法类别", out value9);
				if (value9 != null)
				{
					m_SelectParams.smallType = ((!string.IsNullOrEmpty(value9.ItemTag)) ? int.Parse(value9.ItemTag) : 0);
				}
				break;
			}
			case StoreType.FaShu:
			{
				ShaiXuanParamsModel value12 = null;
				result.TryGetValue("类别", out value12);
				if (value12 != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value12.ItemTag)) ? int.Parse(value12.ItemTag) : 0);
				}
				ShaiXuanParamsModel value13 = null;
				result.TryGetValue("法术类别", out value13);
				if (value13 != null)
				{
					m_SelectParams.smallType = ((!string.IsNullOrEmpty(value13.ItemTag)) ? int.Parse(value13.ItemTag) : 0);
				}
				break;
			}
			case StoreType.DanYao:
			{
				ShaiXuanParamsModel value6 = null;
				result.TryGetValue("类别", out value6);
				if (value6 != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value6.ItemTag)) ? int.Parse(value6.ItemTag) : 0);
				}
				ShaiXuanParamsModel value7 = null;
				result.TryGetValue("药效", out value7);
				if (value7 != null)
				{
					m_SelectParams.smallType = ((!string.IsNullOrEmpty(value7.ItemTag)) ? int.Parse(value7.ItemTag) : 0);
				}
				break;
			}
			case StoreType.FuLu:
			{
				ShaiXuanParamsModel value3 = null;
				result.TryGetValue("类别", out value3);
				if (value3 != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value3.ItemTag)) ? int.Parse(value3.ItemTag) : 0);
				}
				ShaiXuanParamsModel value4 = null;
				result.TryGetValue("法术类别", out value4);
				if (value4 != null)
				{
					m_SelectParams.smallType = ((!string.IsNullOrEmpty(value4.ItemTag)) ? int.Parse(value4.ItemTag) : 0);
				}
				break;
			}
			case StoreType.QiTa:
			{
				ShaiXuanParamsModel value5 = null;
				result.TryGetValue("类别", out value5);
				if (value5 != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value5.ItemTag)) ? int.Parse(value5.ItemTag) : 0);
				}
				break;
			}
			case StoreType.ZhenFa:
			{
				ShaiXuanParamsModel value = null;
				result.TryGetValue("类别", out value);
				ShaiXuanParamsModel value2 = null;
				result.TryGetValue("功能类别", out value2);
				if (value != null)
				{
					m_SelectParams.type = ((!string.IsNullOrEmpty(value.ItemTag)) ? int.Parse(value.ItemTag) : 0);
				}
				if (value2 != null)
				{
					m_SelectParams.smallType = ((!string.IsNullOrEmpty(value2.ItemTag)) ? int.Parse(value2.ItemTag) : 0);
				}
				break;
			}
			}
			ShaiXuanParamsModel value14 = null;
			if (result.TryGetValue("五行", out value14))
			{
				m_SelectParams.fiveElement = (string.IsNullOrEmpty(value14.ItemTag) ? (-1) : int.Parse(value14.ItemTag));
			}
			if (result.TryGetValue("品阶", out value14))
			{
				m_SelectParams.rank = ((!string.IsNullOrEmpty(value14.ItemTag)) ? int.Parse(value14.ItemTag) : 0);
			}
			if (result.TryGetValue("等阶", out value14))
			{
				m_SelectParams.stage = ((!string.IsNullOrEmpty(value14.ItemTag)) ? int.Parse(value14.ItemTag) : 0);
			}
			SelectGoods();
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		public void PageChage(int targetPage)
		{
			m_SelectParams.pageNum = Mathf.Clamp(targetPage, 1, m_MaxPagNum);
			SelectGoodsResp();
		}

		public void ReflashCurrentPage()
		{
			m_LastBrowse = null;
			PageChage(m_CurrentPage);
		}

		private void SelectGoods()
		{
			m_SelectParams.pageNum = 1;
			m_SelectParams.name = Panel.SouSuo.text.Trim();
			SelectGoodsResp();
		}

		private void SelectGoodsResp()
		{
			EnterAuctionReq_1006001 data = new EnterAuctionReq_1006001
			{
				FinalPriceSort = ((m_SelectParams.finalPriceSort == null) ? "" : m_SelectParams.finalPriceSort),
				FiveElement = m_SelectParams.fiveElement == 0 ? -2 : m_SelectParams.fiveElement,
				GoodsType = m_SelectParams.type,
				Type = m_SelectParams.smallType,
				MaxFinalPrice = m_SelectParams.maxFinalPrice,
				MinFinalPrice = m_SelectParams.minFinalPrice,
				Name = ((m_SelectParams.name == null) ? "" : m_SelectParams.name),
				PageNum = m_SelectParams.pageNum,
				PageSize = m_SelectParams.pageSize,
				RankSort = ((m_SelectParams.rankSort == null) ? "" : m_SelectParams.rankSort),
				Stage = m_SelectParams.stage,
				ShopType = (int)m_CurrentStoreType,
				Rank = -m_SelectParams.rank
			};
			// Rank groups use -1..-4; -2 in the element field explicitly requests neutral goods.
			// Positive rank values and an omitted element retain their legacy meanings.
			if (data.Equals(m_LastBrowse) && Time.unscaledTime - m_LastBrowseAt < 1.5f) return;
			m_LastBrowse = data;
			m_LastBrowseAt = Time.unscaledTime;
			int revision = ++m_SearchRevision;
			Singleton<NetManager>.Instance.SendMsg(MsgType.QZG_SELECT_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (revision != m_SearchRevision || GameObject == null || !GameObject.activeInHierarchy) return;
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterAuctionResp_1006001 enterAuctionResp_ = new EnterAuctionResp_1006001();
					enterAuctionResp_.MergeFrom(byteString);
					if (enterAuctionResp_.IsSuccess)
					{
						m_DataSource.pages = enterAuctionResp_.Pages;
						m_DataSource.pageNum = enterAuctionResp_.PageNum;
						m_DataSource.pageSize = enterAuctionResp_.PageSize;
						m_DataSource.list.Clear();
						foreach (EnterAuctionResp_1006001.Types.AuctionObjectResp_1006002 item2 in enterAuctionResp_.Object)
						{
							QZG_ItemModel item = new QZG_ItemModel
							{
								auctionType = 2,
								fiveElement = item2.FiveElement,
								fixedPrice = item2.FixedPrice,
								goodsId = item2.GoodsId,
								goodsType = item2.GoodsType,
								id = item2.Id,
								level = item2.Level,
								name = item2.Name,
								rank = item2.Rank,
								remainHours = item2.RemainHours,
								stage = item2.Stage,
								Num = item2.Count
							};
							m_DataSource.list.Add(item);
						}
						m_MaxPagNum = Mathf.Max(1, m_DataSource.pages);
						m_CurrentPage = Mathf.Clamp(m_DataSource.pageNum, 1, m_MaxPagNum);
						ClearItems();
						Panel.ItemListContent.anchoredPosition = Vector2.zero;
						if (m_MaxPagNum > 1 || m_KeyWnd != null)
							ShowKeyBoardWnd();
						Panel.EmptyGoodsPanel.SetActive(m_DataSource.list.Count == 0);
						CreateData(m_DataSource.list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterAuctionResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
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

		public void CreateData(List<QZG_ItemModel> _data)
		{
			if (_data == null)
			{
				return;
			}
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/QiZhenGeItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ItemListContent.GetComponent<VerticalLayoutGroup>();
			foreach (QZG_ItemModel item in _data)
			{
				QiZhenGeItem component2 = SetListContentParent(Panel.ItemListContent, gameObject).GetComponent<QiZhenGeItem>();
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(item.goodsType, out value);
				int levelByRank = WorldSetting.GetLevelByRank(item.rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				component2.SetValue(item, value2, value, () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("QZG_ItmeInformation.prefab", true, item);
				}, item.Num);
			}
			Panel.ItemListContent.sizeDelta = new Vector2(Panel.ItemListContent.sizeDelta.x, SetVerticalListContentHight(_data.Count, y, component));
		}

		private static UpDonwnSortStatus NextSort(UpDonwnSortStatus status)
		{
			return status == UpDonwnSortStatus.None ? UpDonwnSortStatus.Down :
				status == UpDonwnSortStatus.Down ? UpDonwnSortStatus.Up : UpDonwnSortStatus.None;
		}

		private static string SortOrder(UpDonwnSortStatus status)
		{
			return status == UpDonwnSortStatus.Down ? "desc" : status == UpDonwnSortStatus.Up ? "asc" : null;
		}

		private void ChangePriceSort()
		{
			m_JiaGeSortStatus = m_JiaGeSortStatus == UpDonwnSortStatus.None ? UpDonwnSortStatus.Up :
				m_JiaGeSortStatus == UpDonwnSortStatus.Up ? UpDonwnSortStatus.Down : UpDonwnSortStatus.None;
			m_SelectParams.finalPriceSort = SortOrder(m_JiaGeSortStatus);
			m_RankSortStatus = UpDonwnSortStatus.None;
			m_SelectParams.rankSort = null;
			UpdateSortLabels();
			SelectGoods();
		}

		private void ChangeRankSort()
		{
			m_RankSortStatus = NextSort(m_RankSortStatus);
			m_SelectParams.rankSort = SortOrder(m_RankSortStatus);
			m_JiaGeSortStatus = UpDonwnSortStatus.None;
			m_SelectParams.finalPriceSort = null;
			UpdateSortLabels();
			SelectGoods();
		}

		private void UpdateSortLabels()
		{
			LabelSort(m_PriceSortLabel, "总价", m_JiaGeSortStatus);
			LabelSort(m_RankSortLabel, "品阶", m_RankSortStatus);
		}

		private static void LabelSort(Text label, string title, UpDonwnSortStatus sort)
		{
			label.text = title + (sort == UpDonwnSortStatus.Down ? " ↓" : sort == UpDonwnSortStatus.Up ? " ↑" : "");
			label.color = sort == UpDonwnSortStatus.None ? InkInterfaceArt.SoftInk : InkInterfaceArt.Seal;
			label.fontStyle = sort == UpDonwnSortStatus.None ? FontStyle.Normal : FontStyle.Bold;
		}

		private void ShaiXuanBtnClick()
		{
			m_Common_ShaiXuanWnd.PopUpPanel(isPop: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("QiZhenGeWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("Common_ShaiXuanWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
		}

		private void WeiTuoPaiMaiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QZG_PaiMaiWnd.prefab", true, m_CurrentStoreType);
		}

		private void ShangLaAction()
		{
			PageChage(Mathf.Max(1, m_CurrentPage - 1));
		}

		private void XiaLaAction()
		{
			if (m_CurrentPage < m_MaxPagNum)
				PageChage(m_CurrentPage + 1);
		}

		private void ChooseBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QZG_ChooseWnd.prefab", true, m_CurrentStoreType);
		}

		private void ClearItems()
		{
			foreach (Transform item in Panel.ItemListContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			m_SearchRevision++;
			m_LastBrowse = null;
			ClearItems();
		}

		public override void OnClose()
		{
			base.OnClose();
			if (m_Common_ShaiXuanWnd != null)
				m_Common_ShaiXuanWnd.ReChooseAction -= FlashShaiXuanData;
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
