using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CanWuXiangQingItem : MonoBehaviour
	{
		public Text ItemNameText;

		public Text ItemValueText;

		public void SetValue(string _itemNameText, string _itemValueText)
		{
			ItemNameText.text = _itemNameText;
			ItemValueText.text = _itemValueText;
		}
	}
}
