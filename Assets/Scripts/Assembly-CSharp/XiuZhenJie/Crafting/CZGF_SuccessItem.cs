using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CZGF_SuccessItem : MonoBehaviour
	{
		public Text TitleText;

		public Text ValueText;

		public void SetValue(string title, string value)
		{
			TitleText.text = title;
			ValueText.text = value;
		}
	}
}
