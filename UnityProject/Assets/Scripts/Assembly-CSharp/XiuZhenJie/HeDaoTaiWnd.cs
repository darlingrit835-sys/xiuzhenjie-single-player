using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class HeDaoTaiWnd : Window
	{
		public HeDaoTaiPanel Panel;

		private GameObject m_UiGoodsItem;

		private GameObject m_UiGoodsTogItem;

		public Dictionary<int, Sprite> m_GoodsIconSprite;

		public Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<string, BaseNaJieGoodEntity> m_GoodsListDic = new Dictionary<string, BaseNaJieGoodEntity>();

		private List<BaseNaJieGoodEntity> m_ChooseGoods = new List<BaseNaJieGoodEntity>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<HeDaoTaiPanel>();
			m_UiGoodsItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_UiGoodsTogItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			GetNaJieData();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.AddBtn, AddGoods);
			AddButtonClickListener(Panel.XianJiBtn, XianJiBtnClick);
			AddButtonClickListener(Panel.NaJieCompeleteBtn, CloseNaJie);
			AddButtonClickListener(Panel.NaJieMaskBtn, CloseNaJie);
		}

		private void GetNaJieData()
		{
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetAllGoods(() =>
			{
				foreach (BaseNaJieGoodEntity goods in Singleton<RoleManager>.Instance.RoleBoxDic[10].GoodsList)
				{
					if (goods.Type == 1 || goods.Type == 2 || ((goods.Type == 4) | (goods.Type == 17)) || goods.Type == 18 || goods.Type == 3 || goods.Type == 5 || goods.Type == 6)
					{
						m_GoodsListDic.Add(goods.GoodsId, goods.Copy());
					}
				}
			});
		}

		private void SetItemTextValue(List<string> details)
		{
			for (int i = 0; i < details.Count; i++)
			{
				Panel.Item_1_Texts[i].text = details[i];
			}
		}

		private void AddGoods()
		{
			OpenNaJie();
		}

		private void OpenNaJie()
		{
			NaJieDataInit();
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => Panel.NaJiePanel.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJiePanel.anchoredPosition = v;
			}, new Vector2(0f, -1.5f), 0.5f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				Panel.NaJieMaskBtn.gameObject.SetActive(value: true);
			}));
		}

		private void NaJieDataInit()
		{
			CleraData(Panel.NaJieGoodsContent);
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> item in m_GoodsListDic)
			{
				CreateItem(item.Value);
			}
		}

		private void CreateItem(BaseNaJieGoodEntity goods)
		{
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(goods.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(goods.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			UI_GoodsItem component = SetListContentParent(Panel.NaJieGoodsContent, m_UiGoodsItem).GetComponent<UI_GoodsItem>();
			Panel.NaJieGoodsContent.GetComponent<ToggleGroup>();
			component.SetItem(goods, goods.Name, goods.FiveElement, goods.Level, value2, value, GoodsItemChoose, isUsing: false, goods.GoodsNum);
		}

		private void GoodsItemChoose(object dataSource, GameObject itemObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			UI_GoodsItem ui_Item = itemObj.GetComponent<UI_GoodsItem>();
			if (baseNaJieGoodEntity.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = baseNaJieGoodEntity,
					QueRenCallBack = (int num) =>
					{
						if (num + m_ChooseGoods.Count > 10)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("最多允许献祭10本书籍");
						}
						else
						{
							BaseNaJieGoodEntity baseNaJieGoodEntity2 = baseNaJieGoodEntity.Copy();
							baseNaJieGoodEntity2.GoodsNum = num;
							if (num >= baseNaJieGoodEntity.GoodsNum)
							{
								m_GoodsListDic.Remove(baseNaJieGoodEntity.GoodsId);
								UnityEngine.Object.Destroy(itemObj);
							}
							else
							{
								baseNaJieGoodEntity.GoodsNum -= num;
								ui_Item.ResetNum(baseNaJieGoodEntity.GoodsNum);
							}
							for (int i = 0; i < num; i++)
							{
								baseNaJieGoodEntity2.Copy();
								baseNaJieGoodEntity2.GoodsNum = 1;
								m_ChooseGoods.Add(baseNaJieGoodEntity2);
							}
						}
					}
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else if (1 + m_ChooseGoods.Count > 10)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最多允许献祭10本书籍");
			}
			else
			{
				m_GoodsListDic.Remove(baseNaJieGoodEntity.GoodsId);
				UnityEngine.Object.Destroy(itemObj);
				m_ChooseGoods.Add(baseNaJieGoodEntity);
			}
		}

		private void CleraData(RectTransform rectTransform)
		{
			for (int i = 0; i < rectTransform.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(rectTransform.transform.GetChild(i).gameObject);
			}
		}

		private void CreateChooseItem(BaseNaJieGoodEntity goods)
		{
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(goods.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(goods.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			SetListContentParent(Panel.GoodsContent, m_UiGoodsItem).GetComponent<UI_GoodsItem>().SetItem(goods, goods.Name, goods.FiveElement, goods.Level, value2, value, CancleGoodsItemChoose, isUsing: false, goods.GoodsNum);
		}

		private void CancleGoodsItemChoose(object dataSource, GameObject itemObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (m_ChooseGoods.Contains(baseNaJieGoodEntity))
			{
				m_ChooseGoods.Remove(baseNaJieGoodEntity);
			}
			BaseNaJieGoodEntity value = null;
			if (m_GoodsListDic.TryGetValue(baseNaJieGoodEntity.GoodsId, out value))
			{
				m_GoodsListDic[baseNaJieGoodEntity.GoodsId].GoodsNum++;
			}
			else
			{
				m_GoodsListDic.Add(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity);
			}
			UnityEngine.Object.Destroy(itemObj);
			if (m_ChooseGoods.Count <= 0)
			{
				Panel.AddBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
			}
		}

		private void CloseNaJie()
		{
			CleraData(Panel.NaJieGoodsContent);
			CleraData(Panel.GoodsContent);
			for (int i = 0; i < m_ChooseGoods.Count; i++)
			{
				CreateChooseItem(m_ChooseGoods[i]);
			}
			SetGrticalListContentHight(m_ChooseGoods.Count, Panel.GoodsContent, Panel.GoodsContent.GetComponent<GridLayoutGroup>());
			if (m_ChooseGoods.Count > 0)
			{
				Panel.AddBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 140f);
			}
			else
			{
				Panel.AddBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
			}
			DOTween.To(() => Panel.NaJiePanel.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJiePanel.anchoredPosition = v;
			}, new Vector2(0f, -522.22f), 0.5f);
			Panel.NaJieMaskBtn.gameObject.SetActive(value: false);
		}

		private void XianJiBtnClick()
		{
		}
	}
}
