using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CashGoodsLiBaoItem : MonoBehaviour
	{
		public int LiBaoTypeId;

		public List<UI_GoodsItem> Goods;

		public Text TitleText;

		public Button GouMaiBtn;

		public Image GouMaiCompeleteImg;

		public Text GouMaiBtnText;

		public void SetGoodsData(int typeId, CashGoodsBuyStatus isCanBuy, string titleStr, List<XuanTianLouEntity> dataSource, List<int> goodsNums, UnityAction<int, CashGoodsBuyStatus> GouMaiCompeleteAction)
		{
			LiBaoTypeId = typeId;
			SetObjValue(dataSource, goodsNums);
			DealWithBuyStatus(typeId, isCanBuy, titleStr);
			GouMaiBtn.onClick.RemoveAllListeners();
			GouMaiBtn.onClick.AddListener(() =>
			{
				if (GouMaiCompeleteAction != null)
				{
					GouMaiCompeleteAction(typeId, isCanBuy);
				}
			});
		}

		private void SetObjValue(List<XuanTianLouEntity> dataSource, List<int> goodsNums)
		{
			if (dataSource.Count == Goods.Count && dataSource.Count == goodsNums.Count)
			{
				for (int i = 0; i < dataSource.Count; i++)
				{
					SetGoodsItem(Goods[i], dataSource[i], goodsNums[i]);
				}
			}
		}

		private void DealWithBuyStatus(int typeId, CashGoodsBuyStatus isCanBuy, string titleStr)
		{
			switch (isCanBuy)
			{
			case CashGoodsBuyStatus.UNFINISHED:
			case CashGoodsBuyStatus.UNGET:
				DealWithLingQu(typeId, titleStr);
				break;
			case CashGoodsBuyStatus.GET:
				DealWithCanNotLingQu(typeId, titleStr);
				break;
			}
		}

		private void DealWithLingQu(int typeId, string titleStr)
		{
			if (GouMaiCompeleteImg != null)
			{
				GouMaiCompeleteImg.gameObject.SetActive(value: false);
			}
			GouMaiBtn.gameObject.SetActive(value: true);
		}

		private void DealWithCanNotLingQu(int typeId, string titleStr)
		{
			TitleText.text = string.Format(titleStr, 0);
			if (GouMaiCompeleteImg != null)
			{
				GouMaiCompeleteImg.gameObject.SetActive(value: true);
			}
			GouMaiBtn.gameObject.SetActive(value: false);
		}

		private void SetGoodsItem(UI_GoodsItem uiItem, XuanTianLouEntity entity, int goodsNum)
		{
			Sprite value = null;
			Sprite value2 = null;
			Dictionary<int, Sprite> dictionary = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(entity.GoodsType, out value2);
			dictionary.TryGetValue(WorldSetting.GetLevelByRank(entity.Rank), out value);
			uiItem.SetItem(entity, entity.Name, entity.FiveElement, entity.Level, value, value2, ClickGoods, isUsing: false, goodsNum);
		}

		private void ClickGoods(object dataSource, GameObject itemObj)
		{
			XuanTianLouEntity xuanTianLouEntity = dataSource as XuanTianLouEntity;
			if (!(xuanTianLouEntity.Name == "灵玉"))
			{
				Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
				{
					BtnName = "确定",
					TitleName = xuanTianLouEntity.Name,
					ContentList = new List<string>(xuanTianLouEntity.GoodsDetail)
				};
				string item = string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(xuanTianLouEntity.Rank));
				string item2 = string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(xuanTianLouEntity.Stage));
				common_TipWndParamEntity.ContentList.Insert(0, item);
				common_TipWndParamEntity.ContentList.Insert(0, item2);
				Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
			}
		}
	}
}
