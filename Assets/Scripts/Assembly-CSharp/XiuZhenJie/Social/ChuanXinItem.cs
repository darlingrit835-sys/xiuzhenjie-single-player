using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuanXinItem : MonoBehaviour
	{
		public Image IsReadedImg;

		public Text NameText;

		public Text ContentText;

		public string m_MailId;

		public string MailId => m_MailId;

		public bool IsReaded => IsReadedImg.gameObject.activeSelf;

		public void SetValue(string mailId, bool IsOpened, string nameText, string contentText, UnityAction _btnClick)
		{
			m_MailId = mailId;
			if (IsOpened)
			{
				IsReadedImg.gameObject.SetActive(value: false);
			}
			else
			{
				IsReadedImg.gameObject.SetActive(value: true);
			}
			NameText.text = "【" + nameText + "】";
			ContentText.text = contentText;
			base.gameObject.GetComponent<Button>().onClick.AddListener(_btnClick);
		}

		public void SetReaded()
		{
			IsReadedImg.gameObject.SetActive(value: false);
		}
	}
}
