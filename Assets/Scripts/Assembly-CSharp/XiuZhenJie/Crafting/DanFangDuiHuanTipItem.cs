using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DanFangDuiHuanTipItem : MonoBehaviour
	{
		public Text KeyText;

		public Text ValueText;

		public Button HelpBtn;

		private string m_HelpContent;

		private string m_Key;

		public void SetValue(string key, string value, string helpContent)
		{
			KeyText.text = key;
			m_Key = key;
			ValueText.text = value;
			if (string.IsNullOrEmpty(helpContent))
			{
				HelpBtn.gameObject.SetActive(value: false);
				return;
			}
			m_HelpContent = helpContent;
			HelpBtn.onClick.AddListener(BtnClick);
		}

		private void BtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, m_HelpContent, m_Key);
		}
	}
}
