using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WFG_FaShuDuiHuanItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Text CreateName;

		public Text PointNum;

		private object m_DataSource;

		public void SetValue(WFG_FaShuEntity data, Dictionary<WuXing, Sprite> wuXingSpriteDic, Dictionary<int, Sprite> rankSpriteDic, UnityAction<WFG_FaShuEntity> btnClickAction)
		{
		}

		public void SetValue(object dataSource, string goodsName, int fiveElement, int level, string roleName, int pointNum, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> ClickAction)
		{
			m_DataSource = dataSource;
			CreateName.text = roleName;
			PointNum.text = string.Format(PointNum.text, pointNum.ToString());
			GoodsItem.SetItem(dataSource, goodsName, fiveElement, level, rankSpr, iconSpr, ClickAction);
		}
	}
}
