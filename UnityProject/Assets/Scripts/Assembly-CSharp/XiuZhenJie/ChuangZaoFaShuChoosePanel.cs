using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZaoFaShuChoosePanel : MonoBehaviour
	{
		public Button ChooseBtn;

		public Button CloseBtn;

		public Text TitleText;

		public Text YinNameText;

		public Text YinWuXingText;

		public Text YinDaoLingText;

		private WorldHandSealModel m_DataSource;

		private void Start()
		{
			CloseBtn.onClick.RemoveAllListeners();
			CloseBtn.onClick.AddListener(() =>
			{
				base.gameObject.SetActive(value: false);
			});
		}

		public void SetValue(WorldHandSealModel model, UnityAction<WorldHandSealModel> CallBack)
		{
			m_DataSource = model;
			if (m_DataSource == null)
			{
				return;
			}
			TitleText.text = m_DataSource.b;
			YinNameText.text = m_DataSource.b;
			YinWuXingText.text = m_DataSource.c;
			YinDaoLingText.text = m_DataSource.d;
			base.gameObject.SetActive(value: true);
			ChooseBtn.onClick.RemoveAllListeners();
			ChooseBtn.onClick.AddListener(() =>
			{
				if (CallBack != null)
				{
					CallBack(m_DataSource);
				}
				base.gameObject.SetActive(value: false);
			});
		}
	}
}
