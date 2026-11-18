using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AH_WoDePaiMaoWnd : Window
	{
		public AH_WoDePaiMaoPanel Panel;

		private GameObject m_InsObj;

		private GameObject m_TitleInsObj;

		private GameObject m_XiangQingInstanceObj;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private StoreType m_CurrentStoreType;

		private XzjBoxEntity m_NaJieBox;

		private BaseNaJieGoodEntity m_TargetEntity;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<AH_WoDePaiMaoPanel>();
			ResourceLoad();
			AddButtonClickListener(Panel.CloseBtn, CloseWndClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			Singleton<RoleManager>.Instance.RoleBoxDic.TryGetValue(10, out m_NaJieBox);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			m_TitleInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/BoxTitleTogItem.prefab");
			m_XiangQingInstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiXiangQingItem.prefab");
			InitFuLu();
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenClick()
		{
			if (m_TargetEntity != null)
			{
				Singleton<UIManager>.Instance.PopUpWnd("AH_PaiPinXiangQingWnd.prefab", true, m_TargetEntity);
				CloseWndClick();
			}
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void InitFuLu()
		{
			InitTitleObj("功法", GoodsTypeEnum.BOOK_ACHIEVEMENT, isOn: true);
			InitTitleObj("法术", GoodsTypeEnum.BOOK_SKILL, isOn: false);
			InitTitleObj("法器", GoodsTypeEnum.EQUIPMENT_DEFENSE_APPAREL, isOn: false);
			InitTitleObj("丹方", GoodsTypeEnum.BOOK_ELIXIR, isOn: false);
			InitTitleObj("符书", GoodsTypeEnum.BOOK_SIGN, isOn: false);
			InitTitleObj("洞天福地", GoodsTypeEnum.GUILD_SECRET, isOn: false);
		}

		private void InitTitleObj(string name, GoodsTypeEnum goodsTypeEnum, bool isOn)
		{
			BoxTitleTogItem component = Object.Instantiate(m_TitleInsObj, Panel.BoxTitleContent).GetComponent<BoxTitleTogItem>();
			ToggleGroup component2 = Panel.BoxTitleContent.gameObject.GetComponent<ToggleGroup>();
			component.SetValue(goodsTypeEnum, component2, name, isOn, TitleClick);
		}

		private void TitleClick(bool isOn, object obj)
		{
			if (isOn)
			{
				GoodsTypeEnum goodsTypeEnum = (GoodsTypeEnum)obj;
				m_NaJieBox.GetAllGoods(() =>
				{
					CreateData(goodsTypeEnum);
				});
			}
		}

		private void CreateData(GoodsTypeEnum goodsTypeEnum)
		{
			foreach (Transform item in Panel.BoxContentScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			if (goodsTypeEnum == GoodsTypeEnum.EQUIPMENT_DEFENSE_APPAREL)
			{
				CreateDressedEquipment();
				return;
			}
			int num = 0;
			GridLayoutGroup component = Panel.BoxContentScrollViewContent.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Type == (int)goodsTypeEnum && goods.Rank > 12 && !goods.IsUsing && !goods.IsBinding)
				{
					CreatBoxItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, Panel.BoxContentScrollViewContent, component);
		}

		private void CreateDressedEquipment()
		{
			int num = 0;
			GridLayoutGroup component = Panel.BoxContentScrollViewContent.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Rank > 12 && (goods.Type == 16 || goods.Type == 29 || goods.Type == 26 || goods.Type == 15) && !goods.IsUsing && !goods.IsBinding && !goods.IsLock)
				{
					CreatBoxItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, Panel.BoxContentScrollViewContent, component);
		}

		private void CreatBoxItem(BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(Panel.BoxContentScrollViewContent, m_InsObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsTogItem component = gameObject.GetComponent<UI_GoodsTogItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetValue(group: Panel.BoxContentScrollViewContent.gameObject.GetComponent<ToggleGroup>(), dataSource: data, name: data.Name, fiveElement: data.FiveElement, level: data.Level, rankSpr: value2, iconSpr: value, ClickAction: ChoosedItem);
		}

		private void ChoosedItem(bool isOn, object data, GameObject gameObj)
		{
			if (isOn)
			{
				BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
				m_NaJieBox.GetGoodsInfomation(baseNaJieGoodEntity, CreateXiangQing);
				if (baseNaJieGoodEntity != null)
				{
					m_GoodsIconSprite.TryGetValue(baseNaJieGoodEntity.Type, out var value);
					int levelByRank = WorldSetting.GetLevelByRank(baseNaJieGoodEntity.Rank);
					m_RankBgSprite.TryGetValue(levelByRank, out var value2);
					Panel.ChooseItem.SetItem(baseNaJieGoodEntity, baseNaJieGoodEntity.Name, baseNaJieGoodEntity.FiveElement, baseNaJieGoodEntity.Level, value2, value, null, isUsing: false, baseNaJieGoodEntity.GoodsNum);
					m_TargetEntity = baseNaJieGoodEntity;
				}
			}
		}

		private void CreateXiangQing(List<string> contentList)
		{
			int num = 0;
			foreach (Transform item in Panel.ChooseItemXiangQingContent)
			{
				Object.Destroy(item.gameObject);
			}
			float y = m_XiangQingInstanceObj.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ChooseItemXiangQingContent.gameObject.GetComponent<VerticalLayoutGroup>();
			foreach (string content2 in contentList)
			{
				RoleZhuangBeiXiangQingItem component2 = Object.Instantiate(m_XiangQingInstanceObj, Panel.ChooseItemXiangQingContent).GetComponent<RoleZhuangBeiXiangQingItem>();
				string[] array = content2.Split(',');
				if (array[0] != "名称")
				{
					string content = array[0] + "：" + array[1];
					if (array.Length >= 2)
					{
						component2.SetValue(num, content);
					}
				}
				else
				{
					Panel.ChooseTitleText.text = array[0] + "：" + array[1];
				}
				num++;
			}
			float y2 = SetVerticalListContentHight(contentList.Count, y, component);
			Panel.ChooseItemXiangQingContent.sizeDelta = new Vector2(Panel.ChooseItemXiangQingContent.sizeDelta.x, y2);
		}
	}
}
