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

		internal void SetDetailItem(string key, string value, string helpContent, int index)
		{
			SetItem(key, value, helpContent);
			const float left = 12f;
			const float valueStart = 89f;
			float right = string.IsNullOrEmpty(helpContent) ? left : 35f;
			Image background = transform.Find("ValueBg").GetComponent<Image>();
			InkInterfaceArt.Set(background, "LightPaper", false);
			background.color = index % 2 == 0 ? new Color32(226, 241, 233, 209) : new Color32(246, 249, 243, 205);
			background.raycastTarget = false;
			InkDialogArt.Inset(background.rectTransform, 0f, 0f, 0f, 0f);

			KeyText.rectTransform.anchorMin = Vector2.zero;
			KeyText.rectTransform.anchorMax = Vector2.up;
			KeyText.rectTransform.offsetMin = new Vector2(left, 3f);
			KeyText.rectTransform.offsetMax = new Vector2(valueStart - 5f, -3f);
			InkDialogArt.Inset(ValueText.rectTransform, valueStart, right, 3f, 3f);
			float keyHeight = StyleDetailText(KeyText, InkInterfaceArt.SoftInk, valueStart - left - 5f);
			float valueHeight = StyleDetailText(ValueText, InkInterfaceArt.Ink, InkDialogArt.DetailsWidth - valueStart - right);
			((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
				Mathf.Max(25f, Mathf.Ceil(Mathf.Max(keyHeight, valueHeight)) + 8f));

			var helpRect = (RectTransform)WenHaoBtn.transform;
			helpRect.anchorMin = helpRect.anchorMax = new Vector2(1f, 0.5f);
			helpRect.sizeDelta = new Vector2(24f, 24f);
			helpRect.anchoredPosition = new Vector2(-17f, 0f);
			InkDialogArt.StyleButton(WenHaoBtn, "Help", InkInterfaceArt.Ink);
		}

		private static float StyleDetailText(Text label, Color color, float width)
		{
			label.color = color;
			label.fontSize = 12;
			label.resizeTextForBestFit = false;
			label.alignment = TextAnchor.MiddleLeft;
			label.horizontalOverflow = HorizontalWrapMode.Wrap;
			label.verticalOverflow = VerticalWrapMode.Truncate;
			label.raycastTarget = false;
			TextGenerationSettings settings = label.GetGenerationSettings(new Vector2(width, 0f));
			return label.cachedTextGeneratorForLayout.GetPreferredHeight(label.text, settings) / label.pixelsPerUnit;
		}

		private void BtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, m_HelpContent, m_TitleStr);
		}
	}
}
