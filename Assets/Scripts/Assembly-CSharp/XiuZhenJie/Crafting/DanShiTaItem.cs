using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DanShiTaItem : MonoBehaviour
	{
		public Text Name;

		public Text TiaoJian_1;

		public Text TiaoJian_2;

		public Text text;

		public Toggle toggle;

		public RectTransform ChooseBg;

		public Text ChooseTitleText;

		public Text ChooseTimesText;

		public Text ChooseChengGongLvText;

		private DST_KaoShiItemModel m_DataSource;

		public void SetValue(DST_KaoShiItemModel dataSource, UnityAction<bool, DST_KaoShiItemModel> ClickAction, bool isChoose = false)
		{
			m_DataSource = dataSource;
			if (m_DataSource == null)
			{
				return;
			}
			Name.text = "炼制" + dataSource.name;
			ChooseTitleText.text = "炼制" + dataSource.name;
			ChooseTimesText.text = dataSource.num + "炉";
			TiaoJian_1.text = dataSource.num + "炉";
			TiaoJian_2.text = string.Format("<color=#A00200>{0}%</color>", dataSource.ask.ToString("f0"));
			ChooseChengGongLvText.text = string.Format("成功率要求<color=#A00200>{0}%</color>", dataSource.ask.ToString("f0"));
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((bool isOn) =>
			{
				ChooseBg.gameObject.SetActive(isOn);
				if (ClickAction != null)
				{
					ClickAction(isOn, m_DataSource);
				}
			});
			toggle.isOn = isChoose;
		}
	}
}
