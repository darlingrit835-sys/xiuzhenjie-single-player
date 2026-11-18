using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AH_Item : MonoBehaviour
	{
		public Text Name;

		public Text WuXingText;

		public Text ProductOrderText;

		public Text Price;

		public Image MaskBG;

		public Text WenBen;

		public void ShuaXin(PaiMai shuju)
		{
			MaskBG.gameObject.SetActive(value: false);
			Name.text = shuju.GoodsName;
			WuXingText.text = WorldSetting.GetSignleWuXingName((WuXing)shuju.FiveElement) + "系";
			ProductOrderText.text = WorldSetting.GetLevelByRankName(shuju.Rank);
			Price.text = $"{shuju.StartPrice}灵石起";
		}

		public void ShuaXinState(PaiMai shuju)
		{
			Name.text = shuju.GoodsName;
			WuXingText.text = WorldSetting.GetSignleWuXingName((WuXing)shuju.FiveElement) + "系";
			ProductOrderText.text = WorldSetting.GetLevelByRankName(shuju.Rank);
			Price.text = $"{shuju.StartPrice}灵石";
			MaskBG.gameObject.SetActive(value: true);
			if (shuju.State == 1)
			{
				WenBen.text = "成交";
			}
			else if (shuju.State == 2)
			{
				WenBen.text = "流拍";
			}
			else
			{
				MaskBG.gameObject.SetActive(value: false);
			}
		}
	}
}
