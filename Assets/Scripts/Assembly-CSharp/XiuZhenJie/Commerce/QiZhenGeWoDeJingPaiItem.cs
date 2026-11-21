using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeWoDeJingPaiItem : MonoBehaviour
	{
		public Image RankImg;

		public Image GoodsImg;

		public Text LevelText;

		public Text GoodsName;

		public Text FiveElementText;

		public Text TimeRemaining;

		public Text CurrentJiaGe;

		public Text WoDeJiaGe;

		public Button JiaJiaBtn;

		public Image BeiChaoYueImg;

		public Image ZuiGaoImg;

		public Image NumImg;

		public Text NumText;

		public void SetValue(QZG_ItemModel item, Sprite rankSpr, Sprite goodsSpr, UnityAction clickAction, int num = 1)
		{
			GoodsName.text = item.name;
			LevelText.text = item.level.ToString();
			FiveElementText.text = WorldSetting.GetSignleWuXingName((WuXing)item.fiveElement);
			CurrentJiaGe.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", item.currentPrice, "灵石");
			double totalHours = (new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(item.finishTime) - DateTime.Now).TotalHours;
			if (totalHours >= 1.0)
			{
				TimeRemaining.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", (int)totalHours, "小时");
			}
			else
			{
				TimeRemaining.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", 1, "小时");
			}
			RankImg.sprite = rankSpr;
			GoodsImg.sprite = goodsSpr;
			if (item.whetherBidPriceMax)
			{
				ZuiGaoImg.gameObject.SetActive(value: true);
			}
			else
			{
				BeiChaoYueImg.gameObject.SetActive(value: true);
			}
			if (num >= 1)
			{
				NumImg.gameObject.SetActive(value: true);
				NumText.text = num.ToString();
			}
			else
			{
				NumImg.gameObject.SetActive(value: false);
			}
			JiaJiaBtn.onClick.AddListener(() =>
			{
				if (clickAction != null)
				{
					clickAction();
				}
			});
		}
	}
}
