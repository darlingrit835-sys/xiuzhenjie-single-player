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

		private UnityAction showDetails;

		private void Awake()
		{
			var rowButton = GetComponent<Button>() ?? gameObject.AddComponent<Button>();
			rowButton.targetGraphic = GetComponent<Image>();
			rowButton.onClick.AddListener(OpenDetails);
			CanYuJingPai.onClick.AddListener(OpenDetails);
		}

		private void OpenDetails() => showDetails?.Invoke();

		public void SetValue(QZG_ItemModel _data, Sprite _rankSpr, Sprite _goodsSpr, UnityAction _clickAction, int num = 1)
		{
			InkDialogArt.SetPaper(GetComponent<Image>(), "Parchment");
			itemName.supportRichText = false;
			itemName.text = _data.name;
			DangQianChuJiaTitle.text = "整组总价";
			DangQianChuJiaTitle.color = InkInterfaceArt.SoftInk;
			DangQianChuJia.color = InkInterfaceArt.Ink;
			ShengYuTime.color = InkInterfaceArt.Ink;
			var remainingTitle = transform.Find("ShengYuTitle")?.GetComponent<Text>();
			if (remainingTitle != null) remainingTitle.color = InkInterfaceArt.SoftInk;
			InkDialogArt.StyleButton(CanYuJingPai, "JadeButton", InkDialogArt.Ivory);
			CanYuJingPai.GetComponentInChildren<Text>(true).text = "查看详情";
			LevelText.text = _data.level.ToString();
			FiveElementText.text = WorldSetting.GetSignleWuXingName((WuXing)_data.fiveElement);
			MeiCiJiaJiaTitle.gameObject.SetActive(true);
			MeiCiJiaJia.gameObject.SetActive(true);
			MeiCiJiaJiaTitle.color = InkInterfaceArt.SoftInk;
			MeiCiJiaJia.color = InkInterfaceArt.Ink;
			MeiCiJiaJiaTitle.text = num > 1 ? "每件单价" : "物品品阶";
			MeiCiJiaJia.text = num > 1 ? $"{_data.fixedPrice / num:N0} 灵石" : WorldSetting.GetRankAllName(_data.rank);
			DangQianChuJia.resizeTextForBestFit = true;
			DangQianChuJia.resizeTextMinSize = 9;
			DangQianChuJia.resizeTextMaxSize = 13;
			DangQianChuJia.text = $"{_data.fixedPrice:N0} 灵石";
			ShengYuTime.text = _data.remainHours >= 1 ? $"{_data.remainHours} 小时" : "不足 1 小时";
			RankIcon.sprite = _rankSpr;
			GoodsIcon.sprite = _goodsSpr;
			showDetails = _clickAction;
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
