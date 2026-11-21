using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LiPinItem : MonoBehaviour
	{
		public Image YiLingQu;

		public Text LiPintItemName;

		public Text LiPinItemNumber;

		public void SetValue(string name, string number, bool isLingQu)
		{
			LiPintItemName.text = name;
			LiPinItemNumber.text = number;
			YiLingQu.gameObject.SetActive(isLingQu);
		}
	}
}
