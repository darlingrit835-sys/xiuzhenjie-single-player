using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DanFang_KeZhuCePanel : MonoBehaviour
	{
		public Text ItemName;

		public Button ItemInformation;

		private DST_DanFangModel m_DataSource;

		public void SetValue(DST_DanFangModel dataSource, UnityAction<DST_DanFangModel> action)
		{
			m_DataSource = dataSource;
			if (m_DataSource != null)
			{
				ItemName.text = m_DataSource.b;
				BtnOnClick(action);
			}
		}

		private void BtnOnClick(UnityAction<DST_DanFangModel> action)
		{
			ItemInformation.onClick.RemoveAllListeners();
			ItemInformation.onClick.AddListener(() =>
			{
				if (action != null)
				{
					action(m_DataSource);
				}
			});
		}
	}
}
