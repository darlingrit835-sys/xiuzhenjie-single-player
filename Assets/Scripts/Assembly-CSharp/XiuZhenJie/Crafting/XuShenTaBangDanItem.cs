using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuShenTaBangDanItem : MonoBehaviour
	{
		public Text No1_text;

		public Text No2_text;

		public Text No3_text;

		public Text No4_text;

		public void SetValue(string text_1, string text_2, string text_3, string text_4)
		{
			No1_text.text = text_1;
			No2_text.text = text_2;
			No3_text.text = text_3;
			No4_text.text = text_4;
		}
	}
}
