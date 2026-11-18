using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZaoFaShuSmallPanel : MonoBehaviour
	{
		public Image No1_Img;

		public Text No1_Text;

		public Image No2_Img;

		public Text No2_Text;

		public Image No3_Img;

		public Text No3_Text;

		public Image No4_Img;

		public Text No4_Text;

		public Image No5_Img;

		public Text No5_Text;

		public Image No6_Img;

		public Text No6_Text;

		public Image No7_Img;

		public Text No7_Text;

		public Image No8_Img;

		public Text No8_Text;

		public Image No9_Img;

		public Text No9_Text;

		public Image No10_Img;

		public Text No10_Text;

		public Image No11_Img;

		public Text No11_Text;

		public Image No12_Img;

		public Text No12_Text;

		public Image No13_Img;

		public Text No13_Text;

		private int m_CurrentIndex;

		private void Start()
		{
			m_CurrentIndex = 0;
		}

		public void AddItemImg(string Name)
		{
			switch (m_CurrentIndex)
			{
			case 0:
				No1_Img.gameObject.SetActive(value: true);
				No1_Text.text = Name;
				break;
			case 1:
				No2_Img.gameObject.SetActive(value: true);
				No2_Text.text = Name;
				break;
			case 2:
				No3_Img.gameObject.SetActive(value: true);
				No3_Text.text = Name;
				break;
			case 3:
				No4_Img.gameObject.SetActive(value: true);
				No4_Text.text = Name;
				break;
			case 4:
				No5_Img.gameObject.SetActive(value: true);
				No5_Text.text = Name;
				break;
			case 5:
				No6_Img.gameObject.SetActive(value: true);
				No6_Text.text = Name;
				break;
			case 6:
				No7_Img.gameObject.SetActive(value: true);
				No7_Text.text = Name;
				break;
			case 7:
				No8_Img.gameObject.SetActive(value: true);
				No8_Text.text = Name;
				break;
			case 8:
				No9_Img.gameObject.SetActive(value: true);
				No9_Text.text = Name;
				break;
			case 9:
				No10_Img.gameObject.SetActive(value: true);
				No10_Text.text = Name;
				break;
			case 10:
				No11_Img.gameObject.SetActive(value: true);
				No11_Text.text = Name;
				break;
			case 11:
				No12_Img.gameObject.SetActive(value: true);
				No12_Text.text = Name;
				break;
			case 12:
				No13_Img.gameObject.SetActive(value: true);
				No13_Text.text = Name;
				break;
			}
			m_CurrentIndex++;
		}

		public void ClearAllImg()
		{
			No1_Img.gameObject.SetActive(value: false);
			No2_Img.gameObject.SetActive(value: false);
			No3_Img.gameObject.SetActive(value: false);
			No4_Img.gameObject.SetActive(value: false);
			No5_Img.gameObject.SetActive(value: false);
			No6_Img.gameObject.SetActive(value: false);
			No7_Img.gameObject.SetActive(value: false);
			No8_Img.gameObject.SetActive(value: false);
			No9_Img.gameObject.SetActive(value: false);
			No10_Img.gameObject.SetActive(value: false);
			No11_Img.gameObject.SetActive(value: false);
			No12_Img.gameObject.SetActive(value: false);
			No13_Img.gameObject.SetActive(value: false);
			m_CurrentIndex = 0;
		}
	}
}
