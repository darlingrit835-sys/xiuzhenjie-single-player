using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZhaoGongFaItem : MonoBehaviour
	{
		public Text GreenName;

		public Image YelloImg;

		public Text YelloName;

		public Button btn;

		public AcupointModel m_DataSource;

		private UnityAction<ChuangZhaoGongFaItem> m_Action;

		private bool m_isOn;

		public void SetValue(ChuangZhaoGongFaPanel Panel, AcupointModel dataSource, UnityAction<ChuangZhaoGongFaItem> SureClickAtcion, UnityAction<ChuangZhaoGongFaItem> CancleClickAtcion)
		{
			m_isOn = false;
			m_DataSource = dataSource;
			GreenName.text = dataSource.b;
			YelloName.text = dataSource.b;
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener(() =>
			{
				if (!m_isOn)
				{
					Panel.ChoosePanel.SetValue(m_DataSource, (AcupointModel obj) =>
					{
						BtnClick();
						if (SureClickAtcion != null)
						{
							SureClickAtcion(this);
						}
					});
				}
				else
				{
					BtnClick();
					if (CancleClickAtcion != null)
					{
						CancleClickAtcion(this);
					}
				}
			});
		}

		public void Choose()
		{
			GreenName.gameObject.SetActive(value: false);
			YelloName.gameObject.SetActive(value: true);
			YelloImg.gameObject.SetActive(value: true);
			m_isOn = true;
		}

		public void CancleChoose()
		{
			GreenName.gameObject.SetActive(value: true);
			YelloName.gameObject.SetActive(value: false);
			YelloImg.gameObject.SetActive(value: false);
			m_isOn = false;
		}

		private void BtnClick()
		{
			if (!m_isOn)
			{
				GreenName.gameObject.SetActive(value: false);
				YelloName.gameObject.SetActive(value: true);
				YelloImg.gameObject.SetActive(value: true);
				m_isOn = true;
			}
			else
			{
				GreenName.gameObject.SetActive(value: true);
				YelloName.gameObject.SetActive(value: false);
				YelloImg.gameObject.SetActive(value: false);
				m_isOn = false;
			}
		}
	}
}
