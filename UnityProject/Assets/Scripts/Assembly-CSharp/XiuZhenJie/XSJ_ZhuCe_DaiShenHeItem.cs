using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XSJ_ZhuCe_DaiShenHeItem : MonoBehaviour
	{
		public Text DaiShenHeNameText;

		public Button DaiShenHeBtn;

		private object m_DataSource;

		public void SetValue<T>(string name, object dataSource, UnityAction<T> itemClickAction) where T : class
		{
			DaiShenHeNameText.text = name;
			m_DataSource = dataSource;
			DaiShenHeBtn.onClick.RemoveAllListeners();
			DaiShenHeBtn.onClick.AddListener(() =>
			{
				T arg = m_DataSource as T;
				if (itemClickAction != null)
				{
					itemClickAction(arg);
				}
			});
		}
	}
}
