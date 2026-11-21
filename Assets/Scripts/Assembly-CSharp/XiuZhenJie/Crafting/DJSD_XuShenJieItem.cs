using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DJSD_XuShenJieItem : MonoBehaviour
	{
		public UI_GoodsItem GoodsItem;

		public Text TitleNameText;

		public Text WanZhenDuNameText;

		public Text ContributionText;

		public Text HasChangeText;

		public Text ConvertibleText;

		public Button DuiHuanButton;

		private DJSD_GongFaEntity DataSource;

		public void SetItems(DJSD_GongFaEntity dataSource, string names, int fiveElement, int level, Sprite rankSpr, Sprite iconSpr, UnityAction<object, GameObject> clickAction)
		{
			DataSource = dataSource;
			ContributionText.text = $"{dataSource.ExchangeContribution}贡献点";
			HasChangeText.text = $"{dataSource.HasChange}";
			ConvertibleText.text = $"{dataSource.Convertible}";
			GoodsItem.gameObject.GetComponent<UI_GoodsItem>().SetItem(dataSource, names, fiveElement, level, rankSpr, iconSpr, clickAction);
			TitleNameText.text = names;
			WanZhenDuNameText.text = Progress(dataSource.Progress);
			DuiHuanButton.onClick.AddListener(DuiHuan);
		}

		private void DuiHuan()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DJSD_GongFaDuiHuanTipWnd.prefab", true, DataSource);
		}

		private static string Progress(int shu)
		{
			string result = "";
			switch (shu)
			{
			case 1:
				result = "33%";
				break;
			case 2:
				result = "66%";
				break;
			case 3:
				result = "100%";
				break;
			}
			return result;
		}
	}
}
