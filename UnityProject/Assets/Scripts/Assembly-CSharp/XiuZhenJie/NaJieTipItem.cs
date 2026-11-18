using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NaJieTipItem : MonoBehaviour
	{
		public Text KeyText;

		public Text ValueText;

		public Button WenHaoBtn;

		private string m_HelpContent;

		private string m_TitleStr;

		public void SetItem(string key, string value, string helpContent)
		{
			m_TitleStr = key;
			KeyText.text = string.Format(KeyText.text, key);
			ValueText.text = string.Format(ValueText.text, value);
			m_HelpContent = helpContent;
			if (!(WenHaoBtn == null))
			{
				if (string.IsNullOrEmpty(helpContent))
				{
					WenHaoBtn.gameObject.SetActive(value: false);
				}
				WenHaoBtn.onClick.AddListener(BtnClick);
			}
		}

		private void BtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, m_HelpContent, m_TitleStr);
		}
	}
}
