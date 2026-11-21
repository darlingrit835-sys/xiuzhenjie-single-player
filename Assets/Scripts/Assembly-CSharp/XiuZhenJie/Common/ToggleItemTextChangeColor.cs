using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ToggleItemTextChangeColor : MonoBehaviour
	{
		public Toggle ToggleItem;

		public Text ContentText;

		public Color IsOnFalseColor;

		public Color IsOnTrueColor;

		private void Update()
		{
			if (ToggleItem.isOn)
			{
				if (!ContentText.color.Equals(IsOnTrueColor))
				{
					ContentText.color = IsOnTrueColor;
				}
			}
			else if (!ContentText.color.Equals(IsOnFalseColor))
			{
				ContentText.color = IsOnFalseColor;
			}
		}
	}
}
