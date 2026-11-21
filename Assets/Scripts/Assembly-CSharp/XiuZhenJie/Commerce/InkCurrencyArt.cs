using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkCurrencyArt
	{
		internal static void StyleExchange(DuiHuanLingShiNewPanel panel)
		{
			var card = (RectTransform)panel.TuChu.transform.parent;
			InkDialogArt.SetWash(panel.transform, new Color32(27, 43, 37, 146));
			InkDialogArt.SetPaper(card.GetComponent<Image>(), "Parchment");
			card.sizeDelta = new Vector2(318f, 315f);

			var title = (RectTransform)card.Find("Title");
			Band(title, 12f, 45f, 9f, 40f);
			Surface(title, "BrushPlaque");
			var oldPlate = title.Find("Image (1)").GetComponent<Image>();
			oldPlate.enabled = false;
			var heading = oldPlate.GetComponentInChildren<Text>(true);
			heading.rectTransform.SetParent(title, false);
			InkDialogArt.Inset(heading.rectTransform, 49f, 5f, 1f, 1f);
			Typeset(heading, 20, InkInterfaceArt.Ink, TextAnchor.MiddleLeft, true);

			var node = new GameObject("ExchangeIngot", typeof(RectTransform), typeof(Image));
			node.layer = title.gameObject.layer;
			var emblem = node.GetComponent<Image>();
			emblem.transform.SetParent(title, false);
			AtEdge(emblem.rectTransform, false, 5f, 36f);
			InkInterfaceArt.Set(emblem, "SilverIngot");
			emblem.raycastTarget = false;
			InkArtGlint.Attach(emblem, new Color32(255, 233, 188, 96));

			InkDialogArt.StyleButton(panel.TuChu, "Close", InkInterfaceArt.Ink);
			var close = panel.TuChu.image.rectTransform;
			close.anchorMin = close.anchorMax = Vector2.one;
			close.anchoredPosition = new Vector2(-24f, -28f);
			close.sizeDelta = Vector2.one * 31f;
			close.SetAsLastSibling();

			var quote = (RectTransform)panel.KeDuihHuan.transform.parent;
			Band(quote, 14f, 14f, 58f, 98f);
			Surface(quote, "LightPaper");
			Band(panel.KeDuihHuan.rectTransform, 12f, 10f, 5f, 32f);
			Typeset(panel.KeDuihHuan, 17, InkInterfaceArt.Ink, TextAnchor.MiddleLeft, true);
			Band(panel.ZoLiang.rectTransform, 12f, 10f, 38f, 22f);
			Typeset(panel.ZoLiang, 13, InkInterfaceArt.Ink, TextAnchor.MiddleLeft);
			Band(panel.TimeGenXing.rectTransform, 12f, 65f, 65f, 25f);
			Typeset(panel.TimeGenXing, 11, InkInterfaceArt.SoftInk, TextAnchor.MiddleLeft);
			InkDialogArt.StyleButton(panel.ShuaXin, "LightPlaque", InkInterfaceArt.Ink);
			var refresh = (RectTransform)panel.ShuaXin.transform;
			refresh.anchorMin = refresh.anchorMax = new Vector2(1f, 0f);
			refresh.sizeDelta = new Vector2(48f, 25f);
			refresh.anchoredPosition = new Vector2(-32f, 21f);
			refresh.Find("Image").gameObject.SetActive(false);
			var refreshLabel = refresh.GetComponentInChildren<Text>();
			InkDialogArt.Inset(refreshLabel.rectTransform, 2f, 2f, 1f, 1f);
			Typeset(refreshLabel, 12, InkInterfaceArt.Ink, TextAnchor.MiddleCenter);

			var stepper = (RectTransform)panel.JianHao.transform.parent.parent;
			Band(stepper, 16f, 16f, 169f, 34f);
			stepper.GetComponent<Image>().enabled = false;
			var strip = stepper.Find("JuXingHong");
			strip.GetComponent<Image>().enabled = false;
			strip.Find("CiShu").gameObject.SetActive(false);
			var cost = strip.Find("XiuXing").GetComponent<Text>();
			cost.rectTransform.SetParent(stepper, false);
			AtEdge(cost.rectTransform, false, 1f, 78f);
			cost.text = "花费灵玉";
			Typeset(cost, 14, InkInterfaceArt.Ink, TextAnchor.MiddleLeft);
			StyleStep(panel.JianHao, stepper, 143f, "−", heading.font);
			StyleStep(panel.JiaHao, stepper, 0f, "+", heading.font);
			var input = panel.LingYuShu;
			input.transform.SetParent(stepper, false);
			AtEdge((RectTransform)input.transform, true, 36f, 100f);
			Surface(input.transform, "LightPaper");
			Typeset(input.textComponent, 17, InkInterfaceArt.Ink, TextAnchor.MiddleCenter, true);
			input.placeholder.color = InkInterfaceArt.SoftInk;

			var result = (RectTransform)panel.ShuLianDuZengJia.transform.parent;
			result.SetParent(card, false);
			Band(result, 16f, 16f, 216f, 33f);
			Surface(result, "JadeSelection");
			InkDialogArt.Inset(panel.ShuLianDuZengJia.rectTransform, 6f, 6f, 0f, 0f);
			Typeset(panel.ShuLianDuZengJia, 15, InkInterfaceArt.Ink, TextAnchor.MiddleCenter, true);

			var footer = (RectTransform)panel.DuiHuan.transform.parent;
			Band(footer, 38f, 38f, 262f, 36f);
			footer.GetComponent<Image>().enabled = false;
			InkDialogArt.Inset(panel.DuiHuan.image.rectTransform, 0f, 0f, 0f, 0f);
			InkDialogArt.Inset(panel.NoduiHuan.image.rectTransform, 0f, 0f, 0f, 0f);
			InkDialogArt.StyleButton(panel.DuiHuan, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(panel.NoduiHuan, "LightPlaque", InkInterfaceArt.SoftInk);
		}

		private static void StyleStep(Button button, RectTransform row, float right, string sign, Font font)
		{
			var backing = button.transform.parent.GetComponent<Image>();
			backing.enabled = false;
			button.transform.SetParent(row, false);
			AtEdge((RectTransform)button.transform, true, right, 32f);
			InkDialogArt.StyleButton(button, "LightPlaque", InkInterfaceArt.Ink);
			var node = new GameObject("StepSign", typeof(RectTransform), typeof(Text));
			node.layer = button.gameObject.layer;
			var label = node.GetComponent<Text>();
			label.transform.SetParent(button.transform, false);
			label.font = font;
			label.text = sign;
			InkDialogArt.Inset(label.rectTransform, 0f, 0f, 0f, 0f);
			Typeset(label, 18, InkInterfaceArt.Ink, TextAnchor.MiddleCenter);
		}

		private static void Band(RectTransform rect, float left, float right, float top, float height)
		{
			rect.anchorMin = Vector2.up;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, -top - height);
			rect.offsetMax = new Vector2(-right, -top);
		}

		private static void AtEdge(RectTransform rect, bool right, float inset, float width)
		{
			float anchor = right ? 1f : 0f;
			rect.anchorMin = new Vector2(anchor, 0f);
			rect.anchorMax = new Vector2(anchor, 1f);
			float left = right ? -inset - width : inset;
			rect.offsetMin = new Vector2(left, 0f);
			rect.offsetMax = new Vector2(left + width, 0f);
		}

		private static void Typeset(Text label, int size, Color color, TextAnchor align, bool bold = false)
		{
			label.fontSize = size;
			label.resizeTextMaxSize = size;
			label.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
			label.color = color;
			label.alignment = align;
			label.raycastTarget = false;
			UIManager.FitTextToRect(label);
		}

		internal static void StyleStore(CashGoodsPanel panel)
		{
			Surface(panel.transform.Find("Title_image"), "ParchmentHeading");
			StyleTab(panel.YueKaTog);
			StyleTab(panel.LiBaoTog);
			StyleTab(panel.LingYuTog);
			StyleTab(panel.ChangShengLuTog);
			var jade = panel.LingYuPanel;
			Surface(jade.transform, "InkLedgerPaper");
			Surface(jade.transform.Find("LingYuContent"), "Parchment");
			Surface(jade.VipLevelText.transform.parent, "LightPaper");
			jade.VipLevelText.color = InkInterfaceArt.Ink;
			jade.NextLevelDistanceText.color = InkInterfaceArt.SoftInk;
			UIManager.FitTextToRect(jade.NextLevelDistanceText);
			InkDialogArt.StyleButton(jade.ChanKanTeQuanBtn, "JadeButton", InkDialogArt.Ivory);

			InkDialogArt.StyleButton(jade.HelpBtn, "LightPlaque", InkInterfaceArt.Ink);
			var help = (RectTransform)jade.HelpBtn.transform;
			help.anchorMin = help.anchorMax = Vector2.zero;
			help.pivot = new Vector2(0f, .5f);
			help.anchoredPosition = new Vector2(18f, 47f);
			help.sizeDelta = new Vector2(94f, 25f);
			var helpLabel = help.GetComponentInChildren<Text>(true);
			InkDialogArt.Inset(helpLabel.rectTransform, 2f, 2f, 1f, 1f);
			Typeset(helpLabel, 12, InkInterfaceArt.Ink, TextAnchor.MiddleCenter);

			for (int i = 0; i < jade.LingYuItemList.Count; i++)
			{
				var item = jade.LingYuItemList[i];
				var face = item.transform.Find("HeadContent");
				Surface(face, "LightPaper");
				var art = face.Find("Image").GetComponent<Image>();
				InkInterfaceArt.Set(art, "JadeGoods/Tier" + (i + 1));
				InkDialogArt.Inset(art.rectTransform, 6f, 6f, 3f, 11f);
				art.raycastTarget = false;
				var price = face.Find("Price");
				Surface(price, "JadeButton");
				price.GetComponentInChildren<Text>(true).color = InkDialogArt.Ivory;
				Surface(item.LingYuNum.transform.parent, "ParchmentHeading");
				item.LingYuNum.color = InkInterfaceArt.Ink;
				item.LingYuNum.fontStyle = FontStyle.Bold;
			}
			// The six packages use different jade illustrations. Keep a single glint in the grid.
			if (jade.LingYuItemList.Count > 0)
			{
				var art = jade.LingYuItemList[0].transform.Find("HeadContent/Image").GetComponent<Image>();
				InkArtGlint.Attach(art, new Color32(236, 247, 216, 90));
				var glint = art.GetComponentInChildren<InkArtGlint>(true);
				if (glint != null)
				{
					// This page is parked offscreen, but its GameObject stays active when another tab opens.
					glint.enabled = panel.LingYuTog.isOn;
					panel.LingYuTog.onValueChanged.AddListener(show => glint.enabled = show);
				}
			}
		}

		private static void StyleTab(Toggle tab)
		{
			InkInterfaceArt.Set(tab.targetGraphic as Image, "LightPaper", false);
			InkInterfaceArt.Set(tab.graphic as Image, "JadeSelection", false);
			tab.GetComponentInChildren<Text>(true).color = InkInterfaceArt.Ink;
			if (tab.TryGetComponent(out ToggleItemTextChangeColor caption))
			{
				caption.IsOnTrueColor = InkInterfaceArt.Ink;
				caption.IsOnFalseColor = InkInterfaceArt.SoftInk;
			}
		}

		private static void Surface(Transform node, string sprite)
		{
			InkInterfaceArt.Set(node.GetComponent<Image>(), sprite, false);
		}
	}
}
