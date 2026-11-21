using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CashLingYuItem : MonoBehaviour
	{
		public Image ShouChongTagImg;

		public Text LingYuNum;

		public Button GouMaiBtn;

		private int id;

		public void SetValue(int id, bool isFirst, UnityAction<int> GouMaiAction)
		{
			if (isFirst)
			{
				ShouChongTagImg.gameObject.SetActive(value: true);
			}
			else
			{
				ShouChongTagImg.gameObject.SetActive(value: false);
			}
			GouMaiBtn.onClick.RemoveAllListeners();
			GouMaiBtn.onClick.AddListener(() =>
			{
				if (GouMaiAction != null)
				{
					GouMaiAction(id);
				}
			});
		}
	}
}
