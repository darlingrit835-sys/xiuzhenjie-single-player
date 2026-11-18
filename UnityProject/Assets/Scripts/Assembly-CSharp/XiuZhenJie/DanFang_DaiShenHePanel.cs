using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DanFang_DaiShenHePanel : MonoBehaviour
	{
		public Text ItemName;

		public Button ItemBtn;

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
			ItemBtn.onClick.RemoveAllListeners();
			ItemBtn.onClick.AddListener(() =>
			{
				if (action != null)
				{
					action(m_DataSource);
				}
			});
		}
	}
}
