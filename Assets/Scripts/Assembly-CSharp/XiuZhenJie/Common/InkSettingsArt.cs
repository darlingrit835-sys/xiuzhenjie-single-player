using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkSettingsArt
	{
		internal static void Build(OptionPanel panel)
		{
			Font font = panel.Title_text.font;
			var root = (RectTransform)panel.transform;
			foreach (Transform child in root)
				child.gameObject.SetActive(false);
			InkInterfaceArt.Set(root.GetComponent<Image>(), "Parchment", false);

			var banner = Node(root, "SettingsBanner");
			banner.anchorMin = new Vector2(0f, 1f);
			banner.anchorMax = Vector2.one;
			banner.offsetMin = new Vector2(0f, -84f);
			banner.offsetMax = Vector2.zero;
			banner.gameObject.AddComponent<RectMask2D>();
			var landscape = Node(banner, "MountainHall").gameObject.AddComponent<Image>();
			InkInterfaceArt.Set(landscape, "MountainHall", false);
			landscape.raycastTarget = false;
			if (landscape.sprite != null)
			{
				landscape.rectTransform.pivot = new Vector2(.5f, .86f);
				var cover = landscape.gameObject.AddComponent<AspectRatioFitter>();
				cover.aspectRatio = landscape.sprite.rect.width / landscape.sprite.rect.height;
				cover.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
			}

			var title = panel.Title_text;
			var titleArea = Node(banner, "Heading");
			titleArea.anchorMin = Vector2.zero;
			titleArea.anchorMax = Vector2.up;
			titleArea.offsetMin = new Vector2(16f, 15f);
			titleArea.offsetMax = new Vector2(107f, -17f);
			Image wash = titleArea.gameObject.AddComponent<Image>();
			wash.raycastTarget = false;
			InkInterfaceArt.Set(wash, "ParchmentHeading", false);
			Adopt(title, titleArea);
			InkDialogArt.Inset(title.rectTransform, 4f, 4f, 0f, 0f);
			StyleText(title, "设置", 23, TextAnchor.MiddleCenter);
			Adopt(panel.FanHui_btn, banner);
			var backRect = (RectTransform)panel.FanHui_btn.transform;
			backRect.anchorMin = backRect.anchorMax = new Vector2(1f, .5f);
			backRect.pivot = new Vector2(1f, .5f);
			backRect.sizeDelta = new Vector2(59f, 39f);
			backRect.anchoredPosition = new Vector2(-15f, 0f);
			panel.FanHui_btn.transform.Find("Img")?.gameObject.SetActive(false);
			StyleButton(panel.FanHui_btn, "返回", false);

			var viewport = Node(root, "SettingsScroll");
			InkDialogArt.Inset(viewport, 0f, 0f, 84f, 0f);
			viewport.gameObject.AddComponent<Image>().color = Color.clear;
			viewport.gameObject.AddComponent<RectMask2D>();
			var scroll = viewport.gameObject.AddComponent<ScrollRect>();
			scroll.viewport = viewport;
			scroll.horizontal = false;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			var content = Node(viewport, "Content");
			content.anchorMin = new Vector2(0f, 1f);
			content.anchorMax = Vector2.one;
			content.pivot = new Vector2(.5f, 1f);
			content.anchoredPosition = Vector2.zero;
			content.sizeDelta = Vector2.zero;
			var layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
			layout.padding = new RectOffset(16, 16, 10, 16);
			layout.spacing = 5f;
			layout.childControlWidth = layout.childControlHeight = true;
			layout.childForceExpandWidth = true;
			layout.childForceExpandHeight = false;
			content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scroll.content = content;

			AddHeading(content, font, "当前账号");
			Text account = panel.ShengXianHaoPai_text;
			Adopt(account, content);
			StyleText(account, account.text, 16, TextAnchor.MiddleLeft);
			account.resizeTextForBestFit = true;
			account.resizeTextMinSize = 11;
			account.resizeTextMaxSize = 16;
			Row(account.transform, 39f);
			AddHeading(content, font, "声音与显示");
			AddToggle(content, panel.BeiJingYinYue_btn, "背景音乐", font);
			AddToggle(content, panel.YinXiaoPeiYue_btn, "游戏音效", font);
			AddToggle(content, panel.PingMuChangLiang_btn, "屏幕常亮", font);
			AddHeading(content, font, "礼品兑换");
			AddRedeemRow(content, panel.ShuRuYanZhengMa_input, panel.DuiHuan_btn, "输入兑换码", "兑换");
			var more = Node(content, "ReturnGiftToggle");
			Row(more, 41f);
			more.gameObject.AddComponent<Image>();
			var moreButton = more.gameObject.AddComponent<Button>();
			moreButton.targetGraphic = more.GetComponent<Image>();
			Text moreLabel = Label(more, font);
			StyleButton(moreButton, "回归礼包", false);
			var legacy = AddRedeemRow(content, panel.PhoneNum, panel.HuiGuiDuiHuanBtn, "原账号手机号", "领取");
			void ShowLegacy(bool show)
			{
				legacy.gameObject.SetActive(show);
				moreLabel.text = show ? "收起回归礼包" : "展开回归礼包";
			}
			ShowLegacy(!Constants.UseEmailRegistration);
			moreButton.onClick.AddListener(() => ShowLegacy(!legacy.gameObject.activeSelf));

			Adopt(panel.LoginOut_btn, content);
			Row(panel.LoginOut_btn.transform, 41f);
			StyleButton(panel.LoginOut_btn, "退出登录", false);
			panel.LoginOut_btn.GetComponentInChildren<Text>().color = InkInterfaceArt.Seal;
		}

		private static RectTransform Node(Transform parent, string name)
		{
			var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
			rect.gameObject.layer = parent.gameObject.layer;
			rect.SetParent(parent, false);
			return rect;
		}

		private static void Adopt(Component child, Transform parent)
		{
			child.transform.SetParent(parent, false);
			child.gameObject.SetActive(true);
		}

		private static void Row(Transform row, float height)
		{
			var element = row.gameObject.GetComponent<LayoutElement>() ?? row.gameObject.AddComponent<LayoutElement>();
			element.minHeight = element.preferredHeight = height;
		}

		private static Text Label(Transform parent, Font font)
		{
			var text = Node(parent, "Text").gameObject.AddComponent<Text>();
			text.font = font;
			return text;
		}

		private static void StyleText(Text text, string caption, int size, TextAnchor alignment)
		{
			text.text = caption;
			text.fontSize = size;
			text.resizeTextForBestFit = false;
			text.alignment = alignment;
			text.color = InkInterfaceArt.Ink;
			text.raycastTarget = false;
		}

		private static void AddHeading(RectTransform content, Font font, string caption)
		{
			var heading = Label(content, font);
			StyleText(heading, caption, 12, TextAnchor.LowerLeft);
			heading.color = InkInterfaceArt.SoftInk;
			Row(heading.transform, 23f);
		}

		private static void AddToggle(RectTransform content, Toggle toggle, string caption, Font font)
		{
			Adopt(toggle, content);
			Row(toggle.transform, 41f);
			if (toggle.graphic != null && toggle.graphic.gameObject != toggle.gameObject)
				toggle.graphic.gameObject.SetActive(false);
			toggle.graphic = null;
			var tray = toggle.GetComponent<Image>();
			InkInterfaceArt.Set(tray, "LightPaper", false);
			tray.raycastTarget = true;
			toggle.targetGraphic = tray;
			var label = Label(toggle.transform, font);
			StyleText(label, caption, 15, TextAnchor.MiddleLeft);
			InkDialogArt.Inset(label.rectTransform, 12f, 66f, 0f, 0f);

			var status = Label(toggle.transform, font);
			StyleText(status, "", 13, TextAnchor.MiddleRight);
			InkDialogArt.Inset(status.rectTransform, 12f, 12f, 0f, 0f);
			void Show(bool on)
			{
				status.text = on ? "已开启" : "已关闭";
				status.color = on ? new Color32(44, 98, 69, 255) : InkInterfaceArt.SoftInk;
			}
			Show(toggle.isOn);
			toggle.onValueChanged.AddListener(Show);
		}

		private static RectTransform AddRedeemRow(RectTransform parent, InputField input, Button action, string hint, string buttonText)
		{
			var row = Node(parent, input.name + "Row");
			Row(row, 41f);
			Adopt(input, row);
			InkDialogArt.Inset((RectTransform)input.transform, 0f, 73f, 0f, 0f);
			var surface = input.GetComponent<Image>() ?? input.gameObject.AddComponent<Image>();
			InkInterfaceArt.Set(surface, "LightPaper", false);
			input.targetGraphic = surface;
			StyleText((Text)input.placeholder, hint, 14, TextAnchor.MiddleLeft);
			input.placeholder.color = InkInterfaceArt.SoftInk;
			StyleText(input.textComponent, input.textComponent.text, 14, TextAnchor.MiddleLeft);
			InkDialogArt.Inset(input.placeholder.rectTransform, 9f, 9f, 1f, 1f);
			InkDialogArt.Inset(input.textComponent.rectTransform, 9f, 9f, 1f, 1f);
			Adopt(action, row);
			var rect = (RectTransform)action.transform;
			rect.anchorMin = Vector2.right;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(-66f, 0f);
			rect.offsetMax = Vector2.zero;
			StyleButton(action, buttonText, true);
			return row;
		}

		private static void StyleButton(Button button, string caption, bool primary)
		{
			var image = button.GetComponent<Image>() ?? button.gameObject.AddComponent<Image>();
			button.targetGraphic = image;
			InkDialogArt.StyleButton(button, primary ? "JadeButton" : "ParchmentHeading", InkInterfaceArt.Ink);
			Text label = button.GetComponentInChildren<Text>(true);
			StyleText(label, caption, 14, TextAnchor.MiddleCenter);
			label.color = primary ? InkDialogArt.Ivory : InkInterfaceArt.Ink;
			InkDialogArt.Inset(label.rectTransform, 4f, 4f, 0f, 0f);
		}
	}
}
