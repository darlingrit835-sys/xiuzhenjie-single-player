using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenShanFengTipPanel : MonoBehaviour
	{
		public List<Text> ValueTexts;

		public List<ContentSizeFitter> ContentSizeFitters;

		public Button SureBtn;

		public Button LiKaiBtn;

		private void OnEnable()
		{
			LiKaiBtn.onClick.AddListener(ClosePanel);
		}

		private void OnDisable()
		{
			SureBtn.onClick.RemoveAllListeners();
			LiKaiBtn.onClick.RemoveAllListeners();
		}

		public void SetValue(List<string> values, UnityAction SureClickAction)
		{
			if (values == null || ValueTexts.Count != values.Count)
			{
				return;
			}
			for (int i = 0; i < values.Count; i++)
			{
				ValueTexts[i].text = values[i];
			}
			if (SureClickAction != null)
			{
				SureBtn.onClick.AddListener(SureClickAction);
			}
			SureBtn.onClick.AddListener(ClosePanel);
			if (ContentSizeFitters == null || ContentSizeFitters.Count <= 0)
			{
				return;
			}
			foreach (ContentSizeFitter contentSizeFitter in ContentSizeFitters)
			{
				contentSizeFitter.gameObject.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
			}
		}

		private void ClosePanel()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
