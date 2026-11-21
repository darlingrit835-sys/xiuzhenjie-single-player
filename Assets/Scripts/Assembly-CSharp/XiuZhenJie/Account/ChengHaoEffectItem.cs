using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChengHaoEffectItem : MonoBehaviour
	{
		public Text ContentText;

		public void SetValue(string content, int value, bool isPercent)
		{
			if (value <= 0)
			{
				ContentText.text = $"{content}";
			}
			else if (isPercent)
			{
				ContentText.text = string.Format("{0}: <color=#A00200>{1}</color>", content, value + "%");
			}
			else
			{
				ContentText.text = $"{content}: <color=#A00200>{value}</color>";
			}
		}
	}
}
