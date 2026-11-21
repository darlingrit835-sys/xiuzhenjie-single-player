using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XSJ_ZhuCe_YiZhuCeItem : MonoBehaviour
	{
		public Text YiZhuCeNameText;

		public Text YiZhuCeJiaGeText;

		public Button YiZhuCeBtn;

		private object m_DataSource;

		public void SetValue<T>(XSJ_YiZhuCeItemModel dataObj, object dataSource, UnityAction<T> itemClickAction) where T : class
		{
			YiZhuCeNameText.text = dataObj.Name;
			YiZhuCeJiaGeText.text = dataObj.GonXianDian;
			m_DataSource = dataSource;
			YiZhuCeBtn.onClick.RemoveAllListeners();
			YiZhuCeBtn.onClick.AddListener(() =>
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
