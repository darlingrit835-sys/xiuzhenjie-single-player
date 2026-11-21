using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeWoDeJingPaiWnd : Window
	{
		public QiZhenGeWoDeJingPaiPanel Panel;
		[Serializable] private class Query { public int page; }
		[Serializable] private class Book { public int page, pages; public Purchase[] rows; }
		[Serializable] private class Purchase
		{
			public string name, seller;
			public int count, kind, rank;
			public long price, at;
		}

		private Text title;
		private Font font;
		private bool reading;
		private int revision;
		private int page = 1;
		private const float RowHeight = 76;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = GameObject.GetComponent<QiZhenGeWoDeJingPaiPanel>();
			title = Panel.transform.Find("Title_image/TitleName").GetComponent<Text>();
			font = title.font;
			AddButtonClickListener(Panel.LiKaiBtn, () => Singleton<UIManager>.Instance.CloseWnd(this));
			Panel.ChooseBtn.gameObject.SetActive(false);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ReflashCurrentPage();
		}

		public void ReflashCurrentPage() => PageChage(page);

		public void PageChage(int targetPage)
		{
			if (reading) return;
			reading = true;
			int sent = ++revision;
			title.text = "我的买入";
			ForgeWire.Send((MsgType)4901041, JsonUtility.ToJson(new Query { page = Mathf.Max(1, targetPage) }), (ok, json) =>
			{
				if (sent != revision || GameObject == null) return;
				reading = false;
				Book book = null;
				if (ok)
				{
					try { book = JsonUtility.FromJson<Book>(json); } catch (ArgumentException) { }
				}
				Render(book);
			});
		}

		private RectTransform Row(string name, float height)
		{
			var row = RoleFolio.Box(Panel.ScrollContent, name);
			row.sizeDelta = new Vector2(0, height);
			RoleFolio.Height(row, height);
			InkDialogArt.SetPaper(row.gameObject.AddComponent<Image>(), "Parchment");
			return row;
		}

		private void PageButton(RectTransform row, float from, float to, string label, Action action)
		{
			var node = RoleFolio.Box(row, label);
			RoleFolio.Span(node, from, to, 5, 5);
			var btn = node.gameObject.AddComponent<Button>();
			btn.targetGraphic = node.gameObject.AddComponent<Image>();
			var text = RoleFolio.Label(node, font, label, 14, InkDialogArt.Ivory, TextAnchor.MiddleCenter);
			RoleFolio.Fill(text.rectTransform);
			InkDialogArt.StyleButton(btn, "JadeButton", InkDialogArt.Ivory);
			btn.onClick.AddListener(() => action());
		}

		private void Render(Book book)
		{
			foreach (Transform old in Panel.ScrollContent)
			{
				old.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(old.gameObject);
			}
			var rows = book?.rows ?? Array.Empty<Purchase>();
			if (book != null) page = book.page;
			float height = 0;
			if (rows.Length == 0)
			{
				var empty = Row("买入簿", RowHeight);
				string prompt = book == null ? "买入簿暂未送达" : "尚无买入记录";
				var text = RoleFolio.Label(empty, font, prompt, 16, RoleFolio.Ink, TextAnchor.MiddleCenter);
				RoleFolio.Span(text.rectTransform, 0, book == null ? .7f : 1);
				if (book == null) PageButton(empty, .7f, 1, "重试", ReflashCurrentPage);
				height += RowHeight + 6;
			}
			var icons = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			foreach (var purchase in rows)
			{
				var row = Row(purchase.name, RowHeight);
				var icon = RoleFolio.Image(row, "物品", Color.white);
				RoleFolio.Fixed(icon.rectTransform, 0, 9, 12, 48, 48);
				icons.TryGetValue(purchase.kind, out var sprite);
				icon.sprite = sprite;
				icon.enabled = sprite != null;
				icon.preserveAspect = true;
				var item = RoleFolio.Label(row, font, purchase.name + " ×" + purchase.count, 16, RoleFolio.Ink, TextAnchor.MiddleLeft, true);
				item.supportRichText = false;
				RoleFolio.Band(item.rectTransform, 68, 10, 3, 25);
				var paid = RoleFolio.Label(row, font, $"实付 {purchase.price:N0} 灵石　·　{WorldSetting.GetRankAllName(purchase.rank)}", 13, RoleFolio.Jade);
				RoleFolio.Band(paid.rectTransform, 68, 10, 29, 19);
				string stamp = DateTimeOffset.FromUnixTimeMilliseconds(purchase.at).ToOffset(TimeSpan.FromHours(8)).ToString("MM-dd HH:mm");
				var byline = RoleFolio.Label(row, font, $"{stamp}　卖家 {purchase.seller}", 12, RoleFolio.Secondary);
				byline.supportRichText = false;
				RoleFolio.Band(byline.rectTransform, 68, 10, 49, 20);
				height += RowHeight + 6;
			}
			if (book != null && book.pages > 1)
			{
				var nav = Row("翻页", 40);
				var counter = RoleFolio.Label(nav, font, $"{page} / {book.pages}", 13, RoleFolio.Ink, TextAnchor.MiddleCenter);
				RoleFolio.Span(counter.rectTransform, .34f, .66f);
				if (page > 1) PageButton(nav, 0, .34f, "上一页", () => PageChage(page - 1));
				if (page < book.pages) PageButton(nav, .66f, 1, "下一页", () => PageChage(page + 1));
				height += 46;
			}
			Panel.ScrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			Panel.ScrollView.verticalNormalizedPosition = 1;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			reading = false;
			revision++;
		}
	}
}
