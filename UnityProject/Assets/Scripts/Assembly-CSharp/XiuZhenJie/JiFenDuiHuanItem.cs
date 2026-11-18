using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class JiFenDuiHuanItem : MonoBehaviour
	{
		public Button DuiHuanBtn;

		public Image YiDuiHuanImg;

		private int index;

		public void SetValue(int index, bool IsDuiHuan, UnityAction<int> DuiHuanAction)
		{
			if (!IsDuiHuan)
			{
				YiDuiHuanImg.gameObject.SetActive(value: false);
				DuiHuanBtn.gameObject.SetActive(value: true);
				DuiHuanBtn.onClick.RemoveAllListeners();
				DuiHuanBtn.onClick.AddListener(() =>
				{
					DuiHuanAction?.Invoke(index);
				});
			}
			else
			{
				YiDuiHuanImg.gameObject.SetActive(value: true);
				DuiHuanBtn.gameObject.SetActive(value: false);
			}
		}

		public void ChangeStatus()
		{
			YiDuiHuanImg.gameObject.SetActive(value: true);
			DuiHuanBtn.gameObject.SetActive(value: false);
		}
	}
}
