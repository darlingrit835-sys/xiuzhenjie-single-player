using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DongFuYongTuChooseItem : MonoBehaviour
	{
		public Image YiKaiPi;

		public Text ChooseText;

		public Text ItemText;

		public bool IsCanClick;

		public void SetItem(UnityAction<bool> unityAction, bool isEnable)
		{
			Toggle component = base.gameObject.GetComponent<Toggle>();
			component.onValueChanged.RemoveAllListeners();
			IsCanClick = isEnable;
			if (!isEnable)
			{
				component.interactable = false;
				YiKaiPi.gameObject.SetActive(value: true);
				ChooseText.gameObject.SetActive(value: false);
				ItemText.gameObject.SetActive(value: false);
				return;
			}
			component.onValueChanged.AddListener((bool isOn) =>
			{
				if (isOn)
				{
					ItemText.gameObject.SetActive(value: false);
				}
				else
				{
					ItemText.gameObject.SetActive(value: true);
				}
				if (unityAction != null)
				{
					unityAction(isOn);
				}
			});
		}

		public void SetTrue()
		{
			base.gameObject.GetComponent<Toggle>().isOn = true;
		}
	}
}
