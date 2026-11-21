using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DangFang_YiZhuCePanel : MonoBehaviour
	{
		public Text ItemName;

		public Text ItemPrice;

		public Button ItemInformation;

		private DST_DanFangModel m_DataSource;

		public void SetValue(DST_DanFangModel dataSource, UnityAction<DST_DanFangModel> action)
		{
			m_DataSource = dataSource;
			if (m_DataSource == null)
			{
				return;
			}
			ItemName.text = m_DataSource.b;
			ItemPrice.text = m_DataSource.f + "点";
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
