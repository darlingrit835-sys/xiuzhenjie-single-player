using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeItem : MonoBehaviour
	{
		public Text itemName;

		public Text FiveElementText;

		public Text LevelText;

		public Text DangQianChuJiaTitle;

		public Text DangQianChuJia;

		public Text MeiCiJiaJiaTitle;

		public Text MeiCiJiaJia;

		public Text ShengYuTime;

		public Button CanYuJingPai;

		public Image RankIcon;

		public Image GoodsIcon;

		public Image NumImg;

		public Text NumText;

		public void SetValue(QZG_ItemModel _data, Sprite _rankSpr, Sprite _goodsSpr, UnityAction _clickAction, int num = 1)
		{
			itemName.text = _data.name;
			LevelText.text = _data.level.ToString();
			FiveElementText.text = WorldSetting.GetSignleWuXingName((WuXing)_data.fiveElement);
			MeiCiJiaJia.gameObject.SetActive(value: false);
			MeiCiJiaJiaTitle.gameObject.SetActive(value: false);
			DangQianChuJia.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", _data.fixedPrice, "灵石");
			if (_data.remainHours >= 1)
			{
				ShengYuTime.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", _data.remainHours, "小时");
			}
			else
			{
				ShengYuTime.text = string.Format("<color=white><size=13>{0}</size></color>  {1}", 1, "小时");
			}
			RankIcon.sprite = _rankSpr;
			GoodsIcon.sprite = _goodsSpr;
			CanYuJingPai.onClick.AddListener(() =>
			{
				if (_clickAction != null)
				{
					_clickAction();
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
