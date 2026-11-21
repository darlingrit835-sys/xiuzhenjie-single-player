using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeWoDePaiMaiItem : MonoBehaviour
	{
		public Image RankImg;

		public Image GoodsImg;

		public Text LevelText;

		public Text FiveElementText;

		public Text GoodsName;

		public Text TimeRemaining;

		public Text CurrentJiaGe;

		public Button XiaJiaBtn;

		public Image NumImg;

		public Text NumText;

		public void SetValue(QZG_ItemModel item, Sprite _rankSpr, Sprite _goodsSpr, UnityAction clickAction, int num = 1)
		{
			GoodsName.text = item.name;
			LevelText.text = item.level.ToString();
			FiveElementText.text = WorldSetting.GetSignleWuXingName((WuXing)item.fiveElement);
			CurrentJiaGe.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", item.fixedPrice, "灵石");
			if (item.remainHours >= 1)
			{
				TimeRemaining.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", item.remainHours, "小时");
			}
			else
			{
				TimeRemaining.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", 1, "小时");
			}
			RankImg.sprite = _rankSpr;
			GoodsImg.sprite = _goodsSpr;
			XiaJiaBtn.onClick.AddListener(() =>
			{
				if (clickAction != null)
				{
					clickAction();
				}
			});
			if (num >= 1)
			{
				NumImg.gameObject.SetActive(value: true);
				NumText.text = num.ToString();
			}
			else
			{
				NumImg.gameObject.SetActive(value: false);
			}
		}
	}
}
