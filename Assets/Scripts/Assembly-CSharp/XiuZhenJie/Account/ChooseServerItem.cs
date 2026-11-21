using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChooseServerItem : MonoBehaviour
	{
		public Text NameText;

		public Image IsHaveRole;

		public Button ChooseBtn;

		public Text ChooseBtnText;

		internal static void StyleRow(Image background, Button button)
		{
			InkInterfaceArt.Set(background, "IvoryPlaque", false);
			InkDialogArt.StyleButton(button, "SealButton", InkDialogArt.Ivory);
			var frame = (RectTransform)button.transform;
			frame.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 36f);
			frame.anchoredPosition = new Vector2(frame.anchoredPosition.x, 0f);
			Text label = button.GetComponentInChildren<Text>(true);
			if (label != null)
			{
				RectTransform text = label.rectTransform;
				text.anchorMin = Vector2.zero;
				text.anchorMax = Vector2.one;
				text.offsetMin = new Vector2(5f, 0f);
				text.offsetMax = new Vector2(-5f, 0f);
				label.alignment = TextAnchor.MiddleCenter;
				UIManager.FitTextToRect(label);
			}
		}

		public void SetValue(GameServerInfo serverInfo, Action _action)
		{
			StyleRow(GetComponent<Image>(), ChooseBtn);
			NameText.color = InkInterfaceArt.Ink;
			NameText.text = serverInfo.name;
			if (serverInfo.currentRoleVO == null || string.IsNullOrEmpty(serverInfo.currentRoleVO.id))
			{
				IsHaveRole.gameObject.SetActive(value: false);
			}
			else
			{
				IsHaveRole.gameObject.SetActive(value: true);
			}
			if (serverInfo.status == -1)
			{
				ChooseBtnText.text = "维护中";
				ChooseBtn.interactable = false;
				return;
			}
			if (serverInfo.status == 2)
			{
				ChooseBtnText.text = "已满";
				ChooseBtn.interactable = false;
				return;
			}
			ChooseBtnText.text = "入界";
			ChooseBtn.interactable = true;
			ChooseBtn.onClick.AddListener(() =>
			{
				_action();
			});
		}
	}
}
