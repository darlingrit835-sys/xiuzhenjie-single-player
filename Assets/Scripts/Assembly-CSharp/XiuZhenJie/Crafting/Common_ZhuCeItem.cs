using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Common_ZhuCeItem : MonoBehaviour
	{
		public Image YiZhuCeImg;

		public Text YiZhuCeNameText;

		public Button YiZhuCeBtn;

		public Image DaiShenHeImg;

		public Text DaiShenHeNameText;

		public Button DaiShenHeBtn;

		public Image WeiZhuCeImg;

		public Text WeiZhuCeNameText;

		public Button WeiZhuCeBtn;

		public void SetValue(string Id, string Name, ZhuCeState zhuCeState, UnityAction<string> ClickAction)
		{
			switch (zhuCeState)
			{
			case ZhuCeState.KeZhuCe:
				YiZhuCeImg.gameObject.SetActive(value: true);
				YiZhuCeNameText.text = "【" + base.name + "】";
				if (ClickAction != null)
				{
					YiZhuCeBtn.onClick.AddListener(() =>
					{
						ClickAction(Id);
					});
				}
				break;
			case ZhuCeState.DaiShenHe:
				DaiShenHeImg.gameObject.SetActive(value: true);
				DaiShenHeNameText.text = "【" + base.name + "】";
				if (ClickAction != null)
				{
					DaiShenHeBtn.onClick.AddListener(() =>
					{
						ClickAction(Id);
					});
				}
				break;
			case ZhuCeState.YiZhuCe:
				YiZhuCeImg.gameObject.SetActive(value: true);
				YiZhuCeNameText.text = "【" + base.name + "】";
				if (ClickAction != null)
				{
					WeiZhuCeBtn.onClick.AddListener(() =>
					{
						ClickAction(Id);
					});
				}
				break;
			}
		}
	}
}
