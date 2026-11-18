using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeWoDeJingPaiWnd : Window
	{
		public QiZhenGeWoDeJingPaiPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int PAGE_SIZE = 10;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private KeyBoardWnd m_KeyWnd;

		private PageModel<QZG_ItemModel> m_DataSource;

		private QZG_SearchWuPinListParamModel m_SelectParams;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private StoreType m_CurrentStoreType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<QiZhenGeWoDeJingPaiPanel>();
			m_DataSource = new PageModel<QZG_ItemModel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			ResourceLoad();
			m_CurrentStoreType = (StoreType)paralist[0];
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			PAGE_SIZE = 10;
			m_SelectParams = new QZG_SearchWuPinListParamModel
			{
				pageNum = 1,
				pageSize = PAGE_SIZE
			};
			m_ScrollContentExtend = Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Remove(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.ShangLaAction = (UnityAction)Delegate.Combine(scrollContentExtend2.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend3 = m_ScrollContentExtend;
				scrollContentExtend3.XiaLaAction = (UnityAction)Delegate.Remove(scrollContentExtend3.XiaLaAction, new UnityAction(XiaLaAction));
				ScrollViewContentExtend scrollContentExtend4 = m_ScrollContentExtend;
				scrollContentExtend4.XiaLaAction = (UnityAction)Delegate.Combine(scrollContentExtend4.XiaLaAction, new UnityAction(XiaLaAction));
			}
			PageChage(m_CurrentPage);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_Xie.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ShouGu.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_YaoDan.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhiWu.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_PiMao.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FangJu.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanYao.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_GongFaShu.png");
			Sprite value11 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuShu.png");
			Sprite value12 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanShu.png");
			Sprite value13 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FaSuShu.png");
			Sprite value14 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuLu.png");
			m_GoodsIconSprite.Add(1, value10);
			m_GoodsIconSprite.Add(2, value12);
			m_GoodsIconSprite.Add(5, value11);
			m_GoodsIconSprite.Add(6, value13);
			m_GoodsIconSprite.Add(29, value);
			m_GoodsIconSprite.Add(10, value2);
			m_GoodsIconSprite.Add(15, value3);
			m_GoodsIconSprite.Add(11, value4);
			m_GoodsIconSprite.Add(12, value5);
			m_GoodsIconSprite.Add(8, value6);
			m_GoodsIconSprite.Add(16, value7);
			m_GoodsIconSprite.Add(19, value8);
			m_GoodsIconSprite.Add(26, value9);
			m_GoodsIconSprite.Add(21, value14);
			Sprite value15 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value16 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value17 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value18 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value15);
			m_RankBgSprite.Add(2, value16);
			m_RankBgSprite.Add(3, value17);
			m_RankBgSprite.Add(4, value18);
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
			m_SelectParams.pageSize = PAGE_SIZE;
			SelectGoods();
			ShowKeyBoardWnd();
		}

		public void ReflashCurrentPage()
		{
			PageChage(m_CurrentPage);
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
		}

		public void CreateData(List<QZG_ItemModel> data)
		{
			if (data == null)
			{
				return;
			}
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/QZG_WoDeJingPaiItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ScrollContent.GetComponent<VerticalLayoutGroup>();
			foreach (QZG_ItemModel item in data)
			{
				QiZhenGeWoDeJingPaiItem component2 = SetListContentParent(Panel.ScrollContent, gameObject).GetComponent<QiZhenGeWoDeJingPaiItem>();
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
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, SetVerticalListContentHight(data.Count, y, component));
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
