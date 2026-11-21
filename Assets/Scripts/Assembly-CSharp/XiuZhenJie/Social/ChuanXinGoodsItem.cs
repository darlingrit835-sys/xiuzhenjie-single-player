using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuanXinGoodsItem : MonoBehaviour
	{
		public Image GoodsIcon;

		public Image GoodsRank;

		public Image YiLingQuIcon;

		public Text GoodsNameText;

		public void SetValue(string name, string number, Sprite rankSprite, Sprite iconSprite, bool isLingQu)
		{
			GoodsNameText.text = name + "*" + number;
			GoodsRank.sprite = rankSprite;
			GoodsIcon.sprite = iconSprite;
			if (isLingQu)
			{
				YiLingQuIcon.gameObject.SetActive(value: true);
			}
			else
			{
				YiLingQuIcon.gameObject.SetActive(value: false);
			}
		}
	}
}
