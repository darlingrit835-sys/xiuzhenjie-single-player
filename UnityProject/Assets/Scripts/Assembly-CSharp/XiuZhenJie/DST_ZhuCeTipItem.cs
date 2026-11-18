using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_ZhuCeTipItem : MonoBehaviour
	{
		public Text KeyText;

		public Text ValueText;

		public Button HelpBtn;

		private string m_HelpContent;

		public void SetValue(string key, string value, string helpContent)
		{
			KeyText.text = key;
			ValueText.text = value;
			m_HelpContent = helpContent;
			if (!(HelpBtn == null))
			{
				if (string.IsNullOrEmpty(helpContent))
				{
					HelpBtn.gameObject.SetActive(value: false);
				}
				HelpBtn.onClick.AddListener(BtnClick);
			}
		}

		private void BtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, "", m_HelpContent);
		}
	}
}
