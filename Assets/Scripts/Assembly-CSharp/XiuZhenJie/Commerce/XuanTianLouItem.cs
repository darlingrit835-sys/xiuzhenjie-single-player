using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuanTianLouItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Text PriceText;

		public Image HotIcon;

		public void SetValue(object dataSource, bool isHot, int price, ZhiFuType priceType, string name, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> unityAction)
		{
			GoodsItem.SetItem(dataSource, name, fiveElement, level, rankSpr, iconSpr, unityAction);
			string arg = "灵石";
			switch (priceType)
			{
			case ZhiFuType.LingYu:
				arg = "灵玉";
				break;
			case ZhiFuType.GongXianDian:
				arg = "贡献";
				break;
			}
			PriceText.text = $"{WorldSetting.simplifyNumber((ulong)price)}{arg}";
		}
	}
}
