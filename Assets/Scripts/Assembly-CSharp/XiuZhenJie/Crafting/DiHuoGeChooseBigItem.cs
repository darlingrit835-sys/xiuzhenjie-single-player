using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeChooseBigItem : MonoBehaviour
	{
		public Toggle ItemTog;

		public Image MaskBg;

		public Text NameText;

		public Text LingShiValueText;

		private object m_DataSource;

		public void SetValue(object dataSource, bool isChoose, string name, int LingShiHuaFei)
		{
			m_DataSource = dataSource;
			NameText.text = name;
			LingShiValueText.text = $"{LingShiHuaFei}灵石/次";
			ItemTog.isOn = isChoose;
			if (isChoose)
			{
				MaskBg.gameObject.SetActive(value: false);
			}
			else
			{
				MaskBg.gameObject.SetActive(value: true);
			}
		}
	}
}
