using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoCaiLiaoJiaGeItem : MonoBehaviour
	{
		public Text Name;

		public Text JiaGe;

		public Image HaveImg;

		public Image NoHaveImg;

		public int JiaGeValue;

		public void SetValue(string name, int jiaGe)
		{
			Name.text = name;
			JiaGe.text = $"{jiaGe} 灵石";
			JiaGeValue = jiaGe;
		}

		public void ShuXing(int PanDuan)
		{
			if (PanDuan == 1)
			{
				HaveImg.gameObject.SetActive(value: true);
				NoHaveImg.gameObject.SetActive(value: false);
			}
			else
			{
				HaveImg.gameObject.SetActive(value: false);
				NoHaveImg.gameObject.SetActive(value: true);
			}
		}
	}
}
