using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGePaiMaiWnd : Window
	{
		public QiZhenGePaiMaiPanel _panel;

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
			_panel = base.GameObject.GetComponent<QiZhenGePaiMaiPanel>();
			AddButtonClickListener(_panel.CloseBtn, CloseWndClick);
			AddButtonClickListener(_panel.QueRenBtn, QueRenClick);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			m_TitleInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/BoxTitleTogItem.prefab");
			m_XiangQingInstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiXiangQingItem.prefab");
			ResourceLoad();
			m_CurrentStoreType = (StoreType)paralist[0];
			Singleton<RoleManager>.Instance.RoleBoxDic.TryGetValue(10, out m_NaJieBox);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Init();
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenClick()
		{
			if (m_TargetEntity != null)
			{
				Singleton<UIManager>.Instance.PopUpWnd("QZG_PaiMaiWuPinXinXiWnd.prefab", true, m_TargetEntity, m_CurrentStoreType);
			}
		}

		private void Init()
		{
			switch (m_CurrentStoreType)
			{
			case StoreType.FuLu:
				InitFuLu();
				break;
			case StoreType.DanYao:
				InitDanYao();
				break;
			case StoreType.FaQi:
				InitFaQi();
				break;
			case StoreType.ZhenFa:
				InitZhenFa();
				break;
			case StoreType.GongFa:
				InitGongFa();
				break;
			case StoreType.FaShu:
				InitFaShu();
				break;
			case StoreType.QiTa:
				InitQiTa();
				break;
			case StoreType.LingShou:
			case StoreType.QiQiao:
				break;
			}
		}

		private void InitFuLu()
		{
			InitTitleObj("符箓", GoodsTypeEnum.SIGN, isOn: true);
			InitTitleObj("符书", GoodsTypeEnum.BOOK_SIGN, isOn: false);
		}

		private void InitDanYao()
		{
			InitTitleObj("丹药", GoodsTypeEnum.ELIXIR, isOn: true);
			InitTitleObj("丹方", GoodsTypeEnum.BOOK_ELIXIR, isOn: false);
		}

		private void InitFaQi()
		{
			InitTitleObj("可配置防御", GoodsTypeEnum.EQUIPMENT_DEFENSE_APPAREL, isOn: true);
			InitTitleObj("可配置攻击", GoodsTypeEnum.EQUIPMENT_ATTACK, isOn: false);
			InitTitleObj("可穿戴防御", GoodsTypeEnum.EQUIPMENT_DEFENSE, isOn: false);
			InitTitleObj("器图", GoodsTypeEnum.BOOK_EQUIPMENT, isOn: false);
		}

		private void InitGongFa()
		{
			InitTitleObj("功法", GoodsTypeEnum.BOOK_ACHIEVEMENT, isOn: true);
		}

		private void InitFaShu()
		{
			InitTitleObj("法术", GoodsTypeEnum.BOOK_SKILL, isOn: true);
		}

		private void InitQiTa()
		{
			InitTitleObj("动物", GoodsTypeEnum.MATERIAL_ANIMAL, isOn: true);
			InitTitleObj("植物", GoodsTypeEnum.MATERIAL_PLANT, isOn: false);
			InitTitleObj("矿物", GoodsTypeEnum.MATERIAL_MINERAL, isOn: false);
			InitTitleObj("秘境", GoodsTypeEnum.SECREAT_AREA, isOn: false);
			InitTitleObj("洞天福地", GoodsTypeEnum.GUILD_SECRET, isOn: false);
		}

		private void InitZhenFa()
		{
			InitTitleObj("阵盘", GoodsTypeEnum.MATRIX, isOn: true);
			InitTitleObj("阵图", GoodsTypeEnum.BOOK_MATRIX, isOn: false);
		}

		private void InitTitleObj(string name, GoodsTypeEnum goodsTypeEnum, bool isOn)
		{
			BoxTitleTogItem component = Object.Instantiate(m_TitleInsObj, _panel.BoxTitleContent).GetComponent<BoxTitleTogItem>();
			ToggleGroup component2 = _panel.BoxTitleContent.gameObject.GetComponent<ToggleGroup>();
			component.SetValue(goodsTypeEnum, component2, name, isOn, TitleClick);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void TitleClick(bool isOn, object _obj)
		{
			if (isOn)
			{
				GoodsTypeEnum goodsTypeEnum = (GoodsTypeEnum)_obj;
				m_NaJieBox.GetAllGoods(() =>
				{
					CreateData(goodsTypeEnum);
				});
			}
		}

		private void CreateData(GoodsTypeEnum goodsTypeEnum)
		{
			if (_panel == null || _panel.BoxContentScrollViewContent == null)
			{
				return;
			}
			foreach (Transform item in _panel.BoxContentScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			switch (goodsTypeEnum)
			{
			case GoodsTypeEnum.MATERIAL_ANIMAL:
				CreateAnimalData();
				return;
			case GoodsTypeEnum.EQUIPMENT_DEFENSE:
				CreateDressedEquipment();
				return;
			case GoodsTypeEnum.SECREAT_AREA:
				CreateMiJing();
				return;
			}
			int num = 0;
			new List<BaseNaJieGoodEntity>();
			GridLayoutGroup component = _panel.BoxContentScrollViewContent.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Type == (int)goodsTypeEnum && !goods.IsUsing && !goods.IsBinding)
				{
					CreatBoxItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, _panel.BoxContentScrollViewContent, component);
		}

		private void CreateAnimalData()
		{
			int num = 0;
			new List<BaseNaJieGoodEntity>();
			GridLayoutGroup component = _panel.BoxContentScrollViewContent.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if ((goods.Type == 9 || goods.Type == 10 || goods.Type == 11 || goods.Type == 8) && (!goods.IsUsing || !goods.IsBinding))
				{
					CreatBoxItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, _panel.BoxContentScrollViewContent, component);
		}

		private void CreateDressedEquipment()
		{
			int num = 0;
			new List<BaseNaJieGoodEntity>();
			GridLayoutGroup component = _panel.BoxContentScrollViewContent.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if ((goods.Type == 16 || goods.Type == 29) && !goods.IsUsing && !goods.IsBinding && !goods.IsLock)
				{
					CreatBoxItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, _panel.BoxContentScrollViewContent, component);
		}

		private void CreateMiJing()
		{
			int num = 0;
			new List<BaseNaJieGoodEntity>();
			GridLayoutGroup component = _panel.BoxContentScrollViewContent.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if ((goods.Type == 30 || goods.Type == 31) && !goods.IsUsing && !goods.IsBinding && !goods.IsLock)
				{
					CreatBoxItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, _panel.BoxContentScrollViewContent, component);
		}

		private void CreatBoxItem(BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(_panel.BoxContentScrollViewContent, m_InsObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsTogItem component = gameObject.GetComponent<UI_GoodsTogItem>();
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(_data.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(_data.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			component.SetValue(group: _panel.BoxContentScrollViewContent.gameObject.GetComponent<ToggleGroup>(), dataSource: _data, name: _data.Name, fiveElement: _data.FiveElement, level: _data.Level, rankSpr: value2, iconSpr: value, ClickAction: ChoosedItem, isOn: false, num: _data.GoodsNum);
		}

		private void ChoosedItem(bool isOn, object data, GameObject gameObj)
		{
			if (isOn)
			{
				BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
				m_NaJieBox.GetGoodsInfomation(baseNaJieGoodEntity, CreateXiangQing);
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(baseNaJieGoodEntity.Type, out value);
				int levelByRank = WorldSetting.GetLevelByRank(baseNaJieGoodEntity.Rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				_panel.ChooseItem.SetItem(baseNaJieGoodEntity, baseNaJieGoodEntity.Name, baseNaJieGoodEntity.FiveElement, baseNaJieGoodEntity.Level, value2, value, null);
				m_TargetEntity = baseNaJieGoodEntity;
			}
		}

		private void CreateXiangQing(List<string> contentList)
		{
			int num = 0;
			foreach (Transform item in _panel.ChooseItemXiangQingContent)
			{
				Object.Destroy(item.gameObject);
			}
			float y = m_XiangQingInstanceObj.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = _panel.ChooseItemXiangQingContent.gameObject.GetComponent<VerticalLayoutGroup>();
			foreach (string content2 in contentList)
			{
				RoleZhuangBeiXiangQingItem component2 = Object.Instantiate(m_XiangQingInstanceObj, _panel.ChooseItemXiangQingContent).GetComponent<RoleZhuangBeiXiangQingItem>();
				string[] array = content2.Split(',');
				string content = $"{array[0]}：{array[1]}";
				if (array.Length >= 2)
				{
					component2.SetValue(num, content);
				}
				num++;
			}
			float y2 = SetVerticalListContentHight(contentList.Count, y, component);
			_panel.ChooseItemXiangQingContent.sizeDelta = new Vector2(_panel.ChooseItemXiangQingContent.sizeDelta.x, y2);
		}

		private void ChangeTextColor(Text _text, bool isChoose)
		{
			if (isChoose)
			{
				_text.color = Color.white;
			}
			else
			{
				_text.color = Color.black;
			}
		}
	}
}
