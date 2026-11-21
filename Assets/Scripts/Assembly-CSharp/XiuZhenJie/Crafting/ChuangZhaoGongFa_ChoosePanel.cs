using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZhaoGongFa_ChoosePanel : MonoBehaviour
	{
		public Button ChooseBtn;

		public Button CloseBtn;

		public Text TitleText;

		public Text XueWeiNameText;

		public Text XueWeiWuXingText;

		public Text XueWeiDaoLingText;

		public Text XueWeiGuiJingText;

		public Text XueWeiGuiMaiText;

		private AcupointModel m_DataSource;

		private void Start()
		{
			CloseBtn.onClick.RemoveAllListeners();
			CloseBtn.onClick.AddListener(() =>
			{
				base.gameObject.SetActive(value: false);
			});
		}

		public void SetValue(AcupointModel model, UnityAction<AcupointModel> CallBack)
		{
			m_DataSource = model;
			if (m_DataSource == null)
			{
				return;
			}
			TitleText.text = m_DataSource.b;
			XueWeiNameText.text = m_DataSource.b;
			XueWeiWuXingText.text = m_DataSource.c;
			XueWeiDaoLingText.text = m_DataSource.f;
			XueWeiGuiJingText.text = m_DataSource.d;
			XueWeiGuiMaiText.text = m_DataSource.e;
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
