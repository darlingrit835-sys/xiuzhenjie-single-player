using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChouJiangEventPanel : MonoBehaviour
	{
		public List<ChouJiangEventItem> ChouJiangItemList;

		public Button OneBtn;

		public Button TenBtn;

		public Text ChouQuTimesText;

		public Button JiFenDuiHuanBtn;

		public Text CurrentJiFenText;

		public Button LiKaiBtn;

		public Button OpenJifenBtn;

		public Dropdown StageDropDown;

		public ToggleGroup ContentGroup;

		public Text OncePriceText;

		public Text TenPriceText;

		public Button ShouQuBtn;

		public RectTransform JiangLiPanel;

		public RectTransform TenJiangLiGoodsContent;

		public UI_GoodsItem OneGoodsItem;
	}
}
