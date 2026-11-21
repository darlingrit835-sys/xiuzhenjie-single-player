using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
	internal sealed class TavernView
	{
		private readonly Text modeCaption;
		private readonly Text empty;
		internal readonly Button Share;

		internal TavernView(LingQuanJiuSiPanel panel)
		{
			var root = (RectTransform)panel.transform;
			var navigation = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?._panel.Bottom_img;
			float bottom = navigation != null && navigation.gameObject.activeInHierarchy ? navigation.rectTransform.rect.height : 0f;
			Fill(root, 0, 0, 94, bottom);
			Flat(root.GetComponent<Image>(), Paper, true);
			var font = panel.TitleNameText.font;

			var header = (RectTransform)panel.TitleNameText.transform.parent;
			Band(header, 0, 0, 0, 112);
			Flat(header.GetComponent<Image>(), Card);
			header.gameObject.AddComponent<RectMask2D>();
			var landscape = RoleFolio.Image(header, "山门题图", Color.white);
			landscape.transform.SetAsFirstSibling();
			InkInterfaceArt.Set(landscape, "GuildHall", false);
			var crop = landscape.gameObject.AddComponent<AspectRatioFitter>();
			crop.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
			if (landscape.sprite != null)
				crop.aspectRatio = landscape.sprite.rect.width / landscape.sprite.rect.height;
			var wash = RoleFolio.Image(header, "衬纸", new Color32(250, 244, 226, 154));
			Fill(wash.rectTransform);
			wash.transform.SetSiblingIndex(1);

			var backPlate = panel.BackBtn.transform.parent;
			panel.BackBtn.transform.SetParent(header, false);
			backPlate.gameObject.SetActive(false);
			panel.BackBtn.transform.Find("Img").gameObject.SetActive(false);
			Fixed((RectTransform)panel.BackBtn.transform, 0, 10, 5, 58, 34);
			DaoYouItem.StyleAction(panel.BackBtn, "返回");
			Fixed((RectTransform)panel.ChangeModeBtn.transform, 1, -10, 5, 78, 34);
			modeCaption = Label(panel.ChangeModeBtn.transform, font, "看弹幕", 14, Jade);
			DaoYouItem.StyleAction(panel.ChangeModeBtn, "看弹幕");

			Band(panel.TitleNameText.rectTransform, 16, 12, 44, 35);
			Style(panel.TitleNameText, 27, Ink, TextAnchor.MiddleLeft, true);
			var status = (RectTransform)panel.TitleText.transform.parent;
			Band(status, 0, 0, 82, 30);
			Flat(status.GetComponent<Image>(), new Color32(248, 243, 225, 219));
			Fill(panel.TitleText.rectTransform, 17, 10);
			Style(panel.TitleText, 12, Jade, TextAnchor.MiddleLeft, false, 10);

			Fill(panel.ChatContent, 0, 0, 112, 56);
			Flat(panel.ChatContent.GetComponent<Image>(), Color.clear, true);
			Fill((RectTransform)panel.ChatContent_ScrollRect.transform, 6, 6, 6, 6);
			panel.ChatScrollBar.targetGraphic.color = Rule;
			empty = Label(panel.ChatContent, font, "酒肆里还没有人发言", 14, Secondary, TextAnchor.MiddleCenter);
			Band(empty.rectTransform, 12, 12, 30, 30);
			Fill(panel.DanMuContent_Total, 0, 0, 112, 56);

			var footer = (RectTransform)panel.ChatInput.transform.parent;
			DaoYouItem.Place(footer, 0, 1, 0, 0, 0, 0, 56);
			Flat(footer.GetComponent<Image>(), Card, true);
			panel.transform.Find("BottomImg").gameObject.SetActive(false);
			var shareFace = RoleFolio.Image(footer, "分享", Card, true);
			Fixed(shareFace.rectTransform, 0, 8, 8, 51, 40);
			Share = shareFace.gameObject.AddComponent<Button>();
			Share.targetGraphic = shareFace;
			var shareLabel = Label(shareFace.transform, font, "分享", 15, Jade, TextAnchor.MiddleCenter);
			Fill(shareLabel.rectTransform);
			InkDialogArt.StyleButton(Share, "LightPlaque", Jade);
			Fill((RectTransform)panel.ChatInput.transform, 65, 80, 8, 8);
			InkSocialArt.StyleInput(panel.ChatInput, "写下你的传音…");
			Fill(panel.ChatInput.textComponent.rectTransform, 9, 9, 4, 4);
			Fill(panel.ChatInput.placeholder.rectTransform, 9, 9, 4, 4);
			panel.ChatInput.textComponent.fontSize = 15;
			panel.SendBtn.transform.SetParent(footer, false);
			Fixed((RectTransform)panel.SendBtn.transform, 1, -10, 8, 62, 40);
			var send = Label(panel.SendBtn.transform, font, "发送", 15, InkDialogArt.Ivory, TextAnchor.MiddleCenter, true);
			Fill(send.rectTransform);
			InkDialogArt.StyleButton(panel.SendBtn, "JadeButton", InkDialogArt.Ivory);

			StyleProfile(panel.JieShiPanel);
		}

		private static void StyleProfile(LingQuanJiuSiJieShiPanel profile)
		{
			Fill((RectTransform)profile.transform);
			InkDialogArt.SetWash(profile.transform);
			var card = (RectTransform)profile.CloseBtn.transform.parent;
			card.anchorMin = new Vector2(.08f, .5f);
			card.anchorMax = new Vector2(.92f, .5f);
			card.sizeDelta = new Vector2(0f, 208f);
			card.anchoredPosition = Vector2.zero;
			InkDialogArt.SetPaper(card.GetComponent<Image>());
			var heading = (RectTransform)card.Find("Title_image");
			Band(heading, 12, 12, 10, 39);
			Flat(heading.GetComponent<Image>(), Card);
			heading.Find("Image").gameObject.SetActive(false);
			var title = heading.Find("TitleName").GetComponent<Text>();
			Fill(title.rectTransform, 10, 42);
			Style(title, 18, Ink, TextAnchor.MiddleLeft, true);
			Fixed((RectTransform)profile.CloseBtn.transform, 1, -12, 10, 36, 39);
			profile.CloseBtn.transform.SetAsLastSibling();
			InkDialogArt.StyleButton(profile.CloseBtn, "Close", Ink);
			var info = (RectTransform)profile.RoleName.transform.parent;
			Band(info, 16, 16, 55, 84);
			Flat(info.GetComponent<Image>(), Paper);
			var frame = (RectTransform)profile.TouXiang.transform.parent;
			Fixed(frame, 0, 6, 9, 60, 65);
			InkInterfaceArt.Set(frame.GetComponent<Image>(), "LightPlaque", false);
			Fill(profile.TouXiang.rectTransform, 3, 3, 3, 3);
			profile.TouXiang.preserveAspect = true;
			Band(profile.RoleName.rectTransform, 77, 6, 10, 34);
			Band(profile.LevelName.rectTransform, 77, 6, 44, 26);
			Style(profile.RoleName, 19, Ink, TextAnchor.MiddleLeft, true, 12);
			Style(profile.LevelName, 14, Secondary, TextAnchor.MiddleLeft, false, 11);
			var footer = (RectTransform)profile.SendBtn.transform.parent;
			Band(footer, 14, 14, 148, 44);
			Flat(footer.GetComponent<Image>(), Color.clear);
			Fill((RectTransform)profile.SendBtn.transform, 26, 26);
			DaoYouItem.StyleAction(profile.SendBtn, "发出名刺");
		}

		internal void SetMode(LingQuanJiuSiModeEnum mode)
		{
			modeCaption.text = mode == LingQuanJiuSiModeEnum.Chat ? "看弹幕" : "看聊天";
		}

		internal void SetEmpty(bool visible)
		{
			empty.gameObject.SetActive(visible);
		}
	}
}
