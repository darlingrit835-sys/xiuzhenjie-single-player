using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZaoFaShuItem : MonoBehaviour
	{
		public Button ItemTog;

		public Image ChooseImg;

		public Text NoChooseText;

		public Text ChooseText;

		public WorldHandSealModel m_DataSource;

		private bool isOn;

		public void SetValue(ChuangZaoFaShuPanel panel, WorldHandSealModel dataSource, UnityAction<bool, ChuangZaoFaShuItem> ChangeAction)
		{
			isOn = false;
			m_DataSource = dataSource;
			if (m_DataSource == null)
			{
				return;
			}
			NoChooseText.text = dataSource.b;
			ChooseText.text = dataSource.b;
			ItemTog.onClick.RemoveAllListeners();
			ItemTog.onClick.AddListener(() =>
			{
				if (!isOn)
				{
					panel.ChoosePanel.SetValue(m_DataSource, (WorldHandSealModel data) =>
					{
						isOn = true;
						ChooseImg.gameObject.SetActive(value: true);
						ChooseText.gameObject.SetActive(value: true);
						NoChooseText.gameObject.SetActive(value: false);
						if (ChangeAction != null)
						{
							ChangeAction(arg0: true, this);
						}
					});
				}
				else
				{
					isOn = false;
					ChooseImg.gameObject.SetActive(value: false);
					ChooseText.gameObject.SetActive(value: false);
					NoChooseText.gameObject.SetActive(value: true);
					if (ChangeAction != null)
					{
						ChangeAction(arg0: false, this);
					}
				}
			});
		}

		public void ClearChoose()
		{
			isOn = false;
			ChooseImg.gameObject.SetActive(value: false);
			ChooseText.gameObject.SetActive(value: false);
			NoChooseText.gameObject.SetActive(value: true);
		}

		public void Choose()
		{
			isOn = true;
			ChooseImg.gameObject.SetActive(value: true);
			ChooseText.gameObject.SetActive(value: true);
			NoChooseText.gameObject.SetActive(value: false);
		}
	}
}
