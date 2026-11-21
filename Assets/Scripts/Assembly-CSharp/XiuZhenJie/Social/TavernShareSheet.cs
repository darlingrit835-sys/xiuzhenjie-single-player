using System;
using System.Globalization;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
	internal sealed class TavernShareSheet
	{
		private readonly LingQuanJiuSiPanel panel;
		private readonly Action<string, Action<bool>> publish;
		private readonly Font font;
		private readonly RectTransform overlay;
		private readonly RectTransform rows;
		private readonly ScrollRect scroll;
		private readonly Text heading;
		private int page;

		internal TavernShareSheet(LingQuanJiuSiPanel panel, Action<string, Action<bool>> publish)
		{
			this.panel = panel;
			this.publish = publish;
			font = panel.TitleNameText.font;
			var shade = RoleFolio.Image(panel.transform, "酒肆分享面板", new Color32(34, 52, 42, 120), true);
			overlay = shade.rectTransform;
			Fill(overlay);
			var sheet = RoleFolio.Image(overlay, "笺纸", Paper, true);
			sheet.rectTransform.anchorMin = new Vector2(.05f, .18f);
			sheet.rectTransform.anchorMax = new Vector2(.95f, .82f);
			sheet.rectTransform.offsetMin = sheet.rectTransform.offsetMax = Vector2.zero;
			InkDialogArt.SetPaper(sheet);
			heading = Label(sheet.transform, font, "酒肆分享", 21, Ink, TextAnchor.MiddleLeft, true);
			Band(heading.rectTransform, 15, 72, 10, 42);
			var close = MakeButton(sheet.transform, "关闭", Close);
			Fixed((RectTransform)close.transform, 1, -12, 11, 52, 36);
			var host = RoleFolio.Image(sheet.transform, "内容", Color.clear, true);
			Fill(host.rectTransform, 12, 12, 58, 13);
			scroll = host.gameObject.AddComponent<ScrollRect>();
			scroll.horizontal = false;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			scroll.viewport = Box(host.transform, "Viewport");
			Fill(scroll.viewport);
			scroll.viewport.gameObject.AddComponent<RectMask2D>();
			rows = Box(scroll.viewport, "Content");
			rows.pivot = new Vector2(.5f, 1);
			Band(rows, 2, 2, 0, 0);
			var list = rows.gameObject.AddComponent<VerticalLayoutGroup>();
			list.spacing = 7;
			list.childControlWidth = list.childControlHeight = true;
			list.childForceExpandWidth = true;
			list.childForceExpandHeight = false;
			rows.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scroll.content = rows;
			Close();
		}

		internal void Menu()
		{
			Begin("酒肆分享");
			ActionRow("分享纳戒道具", Items);
			ActionRow("分享我的悬赏", Bounties);
			ActionRow("发布求购", WantedForm);
		}

		internal void Close()
		{
			page++;
			overlay.gameObject.SetActive(false);
		}

		private int Begin(string title)
		{
			page++;
			heading.text = title;
			overlay.SetAsLastSibling();
			overlay.gameObject.SetActive(true);
			for (int i = rows.childCount - 1; i >= 0; i--)
			{
				var child = rows.GetChild(i).gameObject;
				child.SetActive(false);
				UnityEngine.Object.Destroy(child);
			}
			scroll.StopMovement();
			rows.anchoredPosition = Vector2.zero;
			return page;
		}

		private bool Current(int version) => page == version && overlay != null && overlay.gameObject.activeInHierarchy;

		private Button MakeButton(Transform parent, string title, UnityAction action, bool primary = false)
		{
			var face = RoleFolio.Image(parent, title, Card, true);
			var button = face.gameObject.AddComponent<Button>();
			button.targetGraphic = face;
			var caption = Label(face.transform, font, title, 15, Ink, TextAnchor.MiddleCenter);
			Fill(caption.rectTransform, 6, 6);
			InkDialogArt.StyleButton(button, primary ? "JadeButton" : "LightPlaque", primary ? InkDialogArt.Ivory : Ink);
			button.onClick.AddListener(action);
			return button;
		}

		private Button ActionRow(string title, UnityAction action, bool primary = false)
		{
			var button = MakeButton(rows, title, action, primary);
			Height((RectTransform)button.transform, 42);
			return button;
		}

		private Text Line(string message, int height = 32)
		{
			var label = Label(rows, font, message, 15, Secondary);
			Height(label.rectTransform, height);
			return label;
		}

		private void Load<T>(MsgType type, IMessage request, MessageParser<T> parser, Action<T> show) where T : IMessage<T>
		{
			int version = page;
			var status = Line("正在读取…");
			void Receive(ByteString bytes)
			{
				if (!Current(version)) return;
				if (bytes == null) { status.text = "读取失败，请关闭后重试"; return; }
				status.gameObject.SetActive(false);
				show(parser.ParseFrom(bytes));
			}
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(type, request, Receive)) Receive(null);
		}

		private void Items()
		{
			Begin("选择纳戒道具");
			Load(MsgType.ROLE_NAJIE_SPACE, new SpaceGoodsReq_1004002 { Location = 0 }, SpaceGoodsResp_1004002.Parser, reply =>
			{
				if (!reply.IsSuccess) { Line("暂时无法读取纳戒"); return; }
				if (reply.GoodsInfos.Count == 0) Line("纳戒里还没有道具");
				foreach (var item in reply.GoodsInfos)
				{
					var card = TavernCard.Goods(item);
					ActionRow($"{item.Name}　{WorldSetting.GetRankAllName(item.Rank)} ×{Math.Max(1, item.Count)}", () => Preview(card));
				}
			});
		}

		private void Bounties()
		{
			Begin("选择我的悬赏");
			string guild = Singleton<RoleManager>.Instance.Role_Entity.ZongMenId;
			if (string.IsNullOrEmpty(guild) || guild == "0") { Line("加入宗门后，可分享外务殿的寻物悬赏。", 48); return; }
			Load(MsgType.INTOWAIWU, null, IntoWaiWuResp_1015038.Parser, reply =>
			{
				if (!reply.IsSuccess) { Line("暂时无法读取宗门悬赏"); return; }
				if (reply.XuanShangInfo.Count == 0) Line("当前没有可分享的寻物悬赏");
				foreach (var order in reply.XuanShangInfo)
				{
					string state = order.Status == 0 ? "寻物中" : order.Status == 1 ? "可结算" : "暂不可办理";
					var card = new TavernCard(TavernCard.Bounty, order.GoodsName,
						$"报酬：{order.Stone} 灵石\n成功率：{order.SuccessRate}%\n分享时进度：{state}");
					ActionRow($"{order.GoodsName}　{order.Stone} 灵石", () => Preview(card));
				}
			});
		}

		private void Preview(TavernCard card)
		{
			Begin(card.Kind);
			Describe(card);
			PublishButton("分享到酒肆", () => card.Message);
		}

		private void Describe(TavernCard card)
		{
			var title = Line(card.Name, 40);
			Style(title, 20, Ink, TextAnchor.MiddleLeft, true, 12);
			foreach (var line in card.Details.Split('\n')) Line(line);
		}

		private InputField Field(string name, int limit, bool number = false)
		{
			var input = UnityEngine.Object.Instantiate(panel.ChatInput, rows, false);
			input.gameObject.name = name;
			input.text = "";
			input.characterLimit = limit;
			input.contentType = number ? InputField.ContentType.IntegerNumber : InputField.ContentType.Standard;
			input.lineType = InputField.LineType.SingleLine;
			InkSocialArt.StyleInput(input, name);
			Height((RectTransform)input.transform, 42);
			return input;
		}

		private void WantedForm()
		{
			Begin("发布求购");
			var name = Field("物品名称", 32);
			var amount = Field("求购数量", 9, true);
			var price = Field("单价（灵石）", 9, true);
			var hint = Line("填写数量和每件的灵石出价");
			PublishButton("发布到酒肆", () =>
			{
				string item = name.text.Trim();
				if (item.Length == 0 || !Positive(amount.text, out int n) || !Positive(price.text, out int unit))
				{
					hint.text = "请填写物品名，数量和单价须大于零";
					return null;
				}
				return new TavernCard(TavernCard.Wanted, item, $"数量：{n}\n单价：{unit} 灵石").Message;
			});
		}

		private static bool Positive(string text, out int value) =>
			int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value) && value > 0;

		internal void Read(TavernCard card, string author, bool own, UnityAction showAuthor)
		{
			Begin(card.Kind);
			Describe(card);
			ActionRow("查看道友 · " + author, () => { Close(); showAuthor(); });
			if (card.Kind == TavernCard.Wanted && !own)
				ActionRow("回应求购", () => Reply(card, author), true);
		}

		private void Reply(TavernCard card, string author)
		{
			Begin("回应求购");
			Line(author + " 求购 " + card.Name, 42);
			Line(card.Details.Replace("\n", "　"), 42);
			var input = Field("给求购方的留言", 72);
			input.text = "我可以提供，欢迎联系。";
			var hint = Line("回应将发到酒肆");
			PublishButton("发送回应", () =>
			{
				if (string.IsNullOrWhiteSpace(input.text)) { hint.text = "请写下回应内容"; return null; }
				return $"@{author} 回应求购「{card.Name}」\n{card.Details.Replace("\n", "　")}\n{input.text.Trim()}";
			});
		}

		private void PublishButton(string title, Func<string> compose)
		{
			int version = page;
			Button button = null;
			button = ActionRow(title, () =>
			{
				string message = compose();
				if (message == null) return;
				if (!TavernCard.Fits(message)) { MonoSingleton<GameStart>.Instance.PopRollingTextShow("消息太长，请缩短内容"); return; }
				button.interactable = false;
				publish(message, success =>
				{
					if (!Current(version)) return;
					if (success) Close();
					else button.interactable = true;
				});
			}, true);
		}
	}
}
