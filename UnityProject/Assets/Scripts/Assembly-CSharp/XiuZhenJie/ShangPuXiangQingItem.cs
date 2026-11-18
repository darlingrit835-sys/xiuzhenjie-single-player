using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShangPuXiangQingItem : MonoBehaviour
	{
		public Text KeyText;

		public Text ValueText;

		public void SetValue(string key, string value)
		{
			KeyText.text = key;
			ValueText.text = value;
		}
	}
}
