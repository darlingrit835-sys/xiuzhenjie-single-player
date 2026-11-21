using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanDouSettlementPanel : MonoBehaviour
	{
		private Text m_LootCount;
		private Text m_Empty;

		internal void ShowLootCount(int count)
		{
			m_LootCount.text = $"待收  {count}  项";
			m_Empty.gameObject.SetActive(count == 0);
			YiJianLingQuBtn.interactable = count > 0;
		}

		internal void ApplyArt()
		{
			if (m_LootCount != null) return;
			var title = transform.Find("Title_image").GetComponent<Image>();
			InkInterfaceArt.Set(title,"JadeButton",false);
			InkBattleHud.Band(title.rectTransform,8,8,39,48);
			Text titleText = title.transform.Find("TitleName").GetComponent<Text>();
			titleText.text = "历练所得";
			InkBattleHud.Tint(titleText,20,InkDialogArt.Ivory);
			InkBattleHud.Place(titleText.rectTransform,new Vector2(0,.5f),13,0,130,32);
			m_LootCount = InkBattleHud.Label(title.transform,RongLiangText.font,"LootCount","",13,InkBattleHud.Gold);
			m_LootCount.alignment = TextAnchor.MiddleRight;
			InkBattleHud.Place(m_LootCount.rectTransform,new Vector2(1,.5f),-12,0,99,28);
			var list = (RectTransform)transform.Find("Scroll View");
			InkDialogArt.Inset(list,9,9,96,155);
			m_Empty = InkBattleHud.Label(list,RongLiangText.font,"Empty","暂无待收物品",15,InkInterfaceArt.SoftInk);
			m_Empty.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(m_Empty.rectTransform,0,0,0,0);
			m_Empty.gameObject.SetActive(false);
			RectTransform capacity = (RectTransform)transform.Find("RongLiang");
			capacity.anchorMin = Vector2.zero;
			capacity.anchorMax = new Vector2(1,0);
			capacity.offsetMin = new Vector2(10,117);
			capacity.offsetMax = new Vector2(-10,141);
			// All three actions stay below the inventory viewport.
			float width = (((RectTransform)transform).rect.width - 30f) / 3f;
			InkBattleHud.Place((RectTransform)LiKaiBtn.transform,Vector2.zero,8,62,width,42);
			InkBattleHud.Place((RectTransform)DaKaiNaJieBtn.transform,new Vector2(.5f,0),0,62,width,42);
			InkBattleHud.Place((RectTransform)YiJianLingQuBtn.transform,new Vector2(1,0),-8,62,width,42);
			BattleHudReveal.Attach(title.transform);
			BattleHudReveal.Attach(list,.07f);
			InkInterfaceArt.Set(transform.Find("BG")?.GetComponent<Image>(), "InkLandscapePaper", false);
			InkInterfaceArt.Set(RongLiangImg.transform.parent.GetComponent<Image>(), "ParchmentHeading", false);
			// The bar stays a Filled image so the inventory callback can update its length.
			RongLiangImg.color = new Color32(179, 205, 174, 255);
			RongLiangText.color = InkInterfaceArt.Ink;
			Image capacityTitle = transform.Find("RongLiang/TitleBg")?.GetComponent<Image>();
			InkInterfaceArt.Set(capacityTitle, "JadeButton", false);
			if (capacityTitle != null)
				capacityTitle.GetComponentInChildren<Text>(true).color = InkDialogArt.Ivory;

			InkInterfaceArt.Set(NaJieRectTransform.GetComponent<Image>(), "InkLedgerPaper", false);
			InkInterfaceArt.Set(NaJieRectTransform.Find("Scroll View")?.GetComponent<Image>(), "InkLedgerPaper", false);
			InkInterfaceArt.Set(NaJieRectTransform.Find("Title")?.GetComponent<Image>(), "ParchmentHeading", false);
			InkInterfaceArt.Set(NaJieRectTransform.Find("Title/TitleImg")?.GetComponent<Image>(), "ParchmentHeading", false);
			Image drawerTitle = NaJieRectTransform.Find("Title/TitleImg/TitleBg")?.GetComponent<Image>();
			InkInterfaceArt.Set(drawerTitle, "ParchmentHeading", false);
			if (drawerTitle != null)
				drawerTitle.GetComponentInChildren<Text>(true).color = InkInterfaceArt.Ink;

			InkDialogArt.StyleButton(LiKaiBtn, "ParchmentHeading", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(YiJianLingQuBtn, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(WanChengBtn, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(DaKaiNaJieBtn, "ParchmentHeading", InkInterfaceArt.Ink);
			// The original button's caption was baked into its sprite.
			Text caption = DaKaiNaJieBtn.GetComponentInChildren<Text>(true);
			if (caption == null)
			{
				var label = new GameObject("Label", typeof(RectTransform), typeof(Text));
				label.layer = DaKaiNaJieBtn.gameObject.layer;
				label.transform.SetParent(DaKaiNaJieBtn.transform, false);
				caption = label.GetComponent<Text>();
				caption.font = RongLiangText.font;
				caption.fontSize = 13;
				caption.alignment = TextAnchor.MiddleCenter;
				caption.raycastTarget = false;
				caption.rectTransform.anchorMin = Vector2.zero;
				caption.rectTransform.anchorMax = Vector2.one;
				caption.rectTransform.offsetMin = Vector2.zero;
				caption.rectTransform.offsetMax = Vector2.zero;
			}
			caption.text = "打开纳戒";
			caption.color = InkInterfaceArt.Ink;
		}

		public Button LiKaiBtn;

		public Button YiJianLingQuBtn;

		public Button DaKaiNaJieBtn;

		public Button WanChengBtn;

		public Button ShaiXuanBtn;

		public Button NaJieMaskBtn;

		public RectTransform ScrollContent;

		public RectTransform NaJieRectTransform;

		public RectTransform NaJieContent;

		public RectTransform DiaoLuoContent;

		public Text RongLiangText;

		public Image RongLiangImg;
	}
}
