using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenChouJiangEventPanel : MonoBehaviour
	{
		public List<ChouJiangEventItem> ChouJiangItemList;

		public Button OneBtn;

		public Button TenBtn;

		public Text ChouQuTimesText;

		public Button LiKaiBtn;

		public Dropdown StageDropDown;

		public ToggleGroup ContentGroup;

		public Button ShouQuBtn;

		public RectTransform JiangLiPanel;

		public RectTransform TenJiangLiGoodsContent;

		public UI_GoodsItem OneGoodsItem;
	}
}
