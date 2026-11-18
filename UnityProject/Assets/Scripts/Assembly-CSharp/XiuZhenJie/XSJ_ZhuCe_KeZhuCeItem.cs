using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XSJ_ZhuCe_KeZhuCeItem : MonoBehaviour
	{
		public Text KeZhuCeNameText;

		public Button KeZhuCeBtn;

		private object m_DataSource;

		public void SetValue<T>(string name, object dataSource, UnityAction<T> itemClickAction) where T : class
		{
			KeZhuCeNameText.text = name;
			m_DataSource = dataSource;
			KeZhuCeBtn.onClick.RemoveAllListeners();
			KeZhuCeBtn.onClick.AddListener(() =>
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
