using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class BoxTitleTogItem : MonoBehaviour
	{
		public Text NameText;

		public Toggle TitleTog;

		public void SetValue(object dataSource, ToggleGroup toggleGroup, string Value, bool isOn, UnityAction<bool, object> ClickAction)
		{
			NameText.text = Value;
			TitleTog.group = toggleGroup;
			TitleTog.onValueChanged.AddListener((bool isOb) =>
			{
				if (ClickAction != null)
				{
					ClickAction(isOb, dataSource);
				}
			});
			TitleTog.isOn = isOn;
		}
	}
}
