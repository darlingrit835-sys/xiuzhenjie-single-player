using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChouJiangEventItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Toggle ChooseToggle;

		public void SetValue(ToggleGroup toggleGroup, object dataSource, string name, int fiveElement, int level, int rank, int goodsType, UnityAction<object, GameObject> ClickAction, int num)
		{
			ChooseToggle.group = toggleGroup;
			Dictionary<int, Sprite> dictionary = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			Dictionary<int, Sprite> dictionary2 = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			Sprite value = null;
			Sprite value2 = null;
			int levelByRank = WorldSetting.GetLevelByRank(rank);
			dictionary.TryGetValue(levelByRank, out value);
			dictionary2.TryGetValue(goodsType, out value2);
			GoodsItem.SetItem(dataSource, name, fiveElement, level, value, value2, ClickAction, isUsing: false, num);
		}

		public void SetToggleSwitch(bool isOn)
		{
			ChooseToggle.isOn = isOn;
		}
	}
}
