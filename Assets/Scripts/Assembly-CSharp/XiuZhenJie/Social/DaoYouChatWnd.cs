using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DaoYouChatWnd : Window
	{
		public DaoYouChatPanel Panel;

		private string m_RoleName;

		private string m_RoleId;
		internal string FriendRoleId => m_RoleId;

		private int m_Sex = 1;

		private int m_HeadId = 1;

		private GameObject m_SelfChatItem;

		private GameObject m_OtherChatItem;

		private readonly SortedList<long, FriendEntity> visibleMessages = new SortedList<long, FriendEntity>();
		private readonly List<FriendEntity> arriving = new List<FriendEntity>();
		private readonly Dictionary<long, GameObject> bubbles = new Dictionary<long, GameObject>();
		private int pageRequest;
		private bool windowOpen;
		private bool waitingForLatest;
		private bool showingLatest;
		private bool hasOlder;
		private long oldest;
		private GameObject emptyState;
		private bool sending;
		private Text sendCaption;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DaoYouChatPanel>();
			InkSocialArt.ApplyBackdrop(Panel.transform, Panel.GetComponent<Image>());
			sendCaption = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
			sendCaption.gameObject.layer = Panel.SendBtn.gameObject.layer;
			sendCaption.transform.SetParent(Panel.SendBtn.transform, false);
			sendCaption.font = Panel.TitleText.font;
			sendCaption.raycastTarget = false;
			StyleChat();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiChat);
			AddButtonClickListener(Panel.PinBiBtn, PinBiChat);
			AddButtonClickListener(Panel.SendBtn, SendMsg);
			Panel.WaitSendInput.onValueChanged.AddListener(_ => RefreshSendAction());
			Panel.WaitSendInput.onSubmit.AddListener(_ => SendMsg());
			RefreshSendAction();
			m_SelfChatItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/SelfChatItem.prefab");
			m_OtherChatItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/OtherChatItem.prefab");
			m_RoleId = paralist[0].ToString();
			m_RoleName = paralist[1].ToString();
			int.TryParse(paralist[2].ToString(), out m_Sex);
			int.TryParse(paralist[3].ToString(), out m_HeadId);
		}

		private void StyleChat()
		{
			var header = (RectTransform)Panel.TitleText.transform.parent;
			DaoYouItem.Place(header, 0f, 1f, 1f, 0f, 0f, -52f, 0f);
			InkDialogArt.SetPaper(header.GetComponent<Image>());
			DaoYouItem.Place(Panel.TitleText.rectTransform, 0f, 1f, .5f, 67f, -55f, -18f, 18f);
			DaoYouItem.StyleText(Panel.TitleText, 18, InkInterfaceArt.Ink);
			DaoYouItem.Place((RectTransform)Panel.LiKaiBtn.transform, 0f, 0f, .5f, 10f, 58f, -16f, 16f);
			DaoYouItem.StyleAction(Panel.LiKaiBtn, "返回");
			Panel.LiKaiBtn.transform.Find("Img")?.gameObject.SetActive(false);
			DaoYouItem.Place((RectTransform)Panel.PinBiBtn.transform, 1f, 1f, .5f, -44f, -12f, -16f, 16f);
			InkSocialArt.StyleQuietMenu(Panel.PinBiBtn);

			var footer = (RectTransform)Panel.transform.Find("BottomBg");
			DaoYouItem.Place(footer, 0f, 1f, 0f, 0f, 0f, 0f, 67f);
			InkDialogArt.SetPaper(footer.GetComponent<Image>());
			var input = (RectTransform)Panel.WaitSendInput.transform;
			input.SetParent(footer, false);
			DaoYouItem.Place(input, 0f, 1f, .5f, 14f, -91f, -20f, 20f);
			InkSocialArt.StyleInput(Panel.WaitSendInput, "写一封传书…");
			Panel.WaitSendInput.textComponent.fontSize = 14;
			footer.Find("XianLiaoInput")?.gameObject.SetActive(false);
			Panel.SendBtn.transform.SetParent(footer, false);
			DaoYouItem.Place((RectTransform)Panel.SendBtn.transform, 1f, 1f, .5f, -79f, -14f, -20f, 20f);
			DaoYouItem.StyleAction(Panel.SendBtn, "发送", primary: true);

			var scroll = Panel.DaoYouScrollView;
			InkDialogArt.Inset((RectTransform)scroll.transform, 8f, 8f, 52f, 67f);
			if (scroll.GetComponent<Image>() is Image tray)
			{
				tray.sprite = null;
				tray.color = new Color32(248, 248, 229, 170);
			}
			if (scroll.viewport != null && scroll.viewport.GetComponent<Mask>() is Mask mask)
				mask.showMaskGraphic = false;
			var rows = Panel.DaoYouContent.GetComponent<VerticalLayoutGroup>();
			rows.padding = new RectOffset(8, 8, 15, 15);
			rows.spacing = 11f;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Panel.TitleText.text = m_RoleName + " · 传书";
			var chat = Singleton<ChatManager>.Instance;
			chat.EnterRoleChat(m_RoleId);
			chat.ReceiveMsgAction -= Receive;
			chat.ReceiveMsgAction += Receive;
			windowOpen = true;
			RefreshSendAction();
			FriendGoodsOffers.RequestRefresh();
			LoadPage(0);
		}

		private void LoadPage(long before)
		{
			int requestNumber = ++pageRequest;
			waitingForLatest = before == 0;
			arriving.Clear();
			var request = FriendChatPage.Request(m_RoleId, before);
			Singleton<NetManager>.Instance.SendMsg(MsgType.FriendChatHistory, request, response =>
			{
				if (!windowOpen || requestNumber != pageRequest || !(response is ByteString bytes))
				{
					return;
				}
				waitingForLatest = false;
				var page = FriendChatPage.Read(bytes);
				if (!page.Success)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(page.Code));
					return;
				}
				showingLatest = before == 0;
				hasOlder = page.HasMore;
				oldest = page.Messages.Count > 0 ? page.Messages[0].Timestamp : 0;
				visibleMessages.Clear();
				foreach (var message in page.Messages)
				{
					visibleMessages[message.Timestamp] = message;
				}
				if (showingLatest)
				{
					foreach (var message in arriving)
					{
						visibleMessages[message.Timestamp] = message;
					}
				}
				arriving.Clear();
				while (visibleMessages.Count > 100)
				{
					hasOlder = true;
					visibleMessages.RemoveAt(0);
				}
				if (visibleMessages.Count > 0) oldest = visibleMessages.Keys[0];
				RenderPage();
			});
		}

		private void RenderPage()
		{
			bubbles.Clear();
			emptyState = null;
			foreach (Transform child in Panel.DaoYouContent)
			{
				UnityEngine.Object.Destroy(child.gameObject);
				child.gameObject.SetActive(false);
			}
			if (hasOlder && oldest > 0)
			{
				AddPageButton("查看更早的传书", 0, true);
			}
			foreach (var message in visibleMessages.Values)
			{
				DrawMessage(message);
			}
			if (visibleMessages.Count == 0)
			{
				var text = new GameObject("EmptyConversation", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
				text.transform.SetParent(Panel.DaoYouContent, false);
				text.font = Panel.TitleText.font;
				text.text = "还没有传书";
				text.raycastTarget = false;
				DaoYouItem.StyleText(text, 14, InkInterfaceArt.SoftInk);
				text.rectTransform.sizeDelta = new Vector2(0f, 66f);
				emptyState = text.gameObject;
			}
			if (!showingLatest)
			{
				AddPageButton("返回最新传书", 0);
			}
			ResizeContent(true);
		}

		private void AddPageButton(string label, long before, bool older = false)
		{
			var row = DefaultControls.CreateButton(new DefaultControls.Resources());
			var rect = row.GetComponent<RectTransform>();
			rect.SetParent(Panel.DaoYouContent, false);
			rect.sizeDelta = new Vector2(180f, 48f);
			var caption = row.GetComponentInChildren<Text>();
			caption.font = Panel.TitleText.font;
			var button = row.GetComponent<Button>();
			DaoYouItem.StyleAction(button, label);
			if (older) rect.SetAsFirstSibling();
			button.onClick.AddListener(() => LoadPage(older ? oldest : before));
		}

		private void ShowMessage(FriendEntity message)
		{
			if (!windowOpen) return;
			if (waitingForLatest) arriving.Add(message);
			if (!showingLatest || visibleMessages.ContainsKey(message.Timestamp)) return;
			if (emptyState != null)
			{
				emptyState.SetActive(false);
				UnityEngine.Object.Destroy(emptyState);
				emptyState = null;
			}
			bool followLatest = Panel.DaoYouScrollView.verticalNormalizedPosition <= .02f
				|| visibleMessages.Count == 0 || message.FromRoleId == Singleton<RoleManager>.Instance.Role_Entity.Id;
			visibleMessages.Add(message.Timestamp, message);
			DrawMessage(message);
			if (visibleMessages.Count > 100)
			{
				long expired = visibleMessages.Keys[0];
				visibleMessages.RemoveAt(0);
				var bubble = bubbles[expired];
				bubble.SetActive(false);
				UnityEngine.Object.Destroy(bubble);
				bubbles.Remove(expired);
				oldest = visibleMessages.Keys[0];
				if (!hasOlder)
				{
					hasOlder = true;
					AddPageButton("查看更早的传书", 0, true);
				}
			}
			int index = visibleMessages.IndexOfKey(message.Timestamp);
			if (index >= 0 && index + 1 < visibleMessages.Count)
			{
				var next = bubbles[visibleMessages.Keys[index + 1]].transform;
				bubbles[message.Timestamp].transform.SetSiblingIndex(next.GetSiblingIndex());
			}
			ResizeContent(followLatest);
		}

		private void DrawMessage(FriendEntity message)
		{
			var self = Singleton<RoleManager>.Instance.Role_Entity;
			if (message.FromRoleId == self.Id)
			{
				var row = SetListContentParent(Panel.DaoYouContent, m_SelfChatItem).GetComponent<SelfChatItem>();
				bubbles[message.Timestamp] = row.gameObject;
				row.SetValue(message.Content, message.ReceiveTime, self.Sex, self.HeadPortraitId, isOnline: true);
			}
			else if (message.FromRoleId == m_RoleId)
			{
				var row = SetListContentParent(Panel.DaoYouContent, m_OtherChatItem).GetComponent<OtherChatItem>();
				bubbles[message.Timestamp] = row.gameObject;
				row.SetValue(message.Content, message.ReceiveTime, m_Sex, m_HeadId, isOnline: true);
			}
		}

		private void ResizeContent(bool followLatest)
		{
			float viewportHeight = Panel.DaoYouScrollView.viewport.rect.height;
			float oldRange = Mathf.Max(0f, Panel.DaoYouContent.rect.height - viewportHeight);
			float distanceFromTop = (1f - Panel.DaoYouScrollView.verticalNormalizedPosition) * oldRange;
			Canvas.ForceUpdateCanvases();
			var rows = Panel.DaoYouContent.GetComponent<VerticalLayoutGroup>();
			float height = rows.padding.vertical;
			foreach (RectTransform row in Panel.DaoYouContent)
			{
				if (!row.gameObject.activeSelf)
					continue;
				if (row.TryGetComponent<SelfChatItem>(out var own))
					InkSocialArt.FitChatBubbleHeight(row, own.ChatBgRect, own.ChatText);
				else if (row.TryGetComponent<OtherChatItem>(out var contact))
					InkSocialArt.FitChatBubbleHeight(row, contact.ChatBgRect, contact.ChatText);
				height += row.rect.height + rows.spacing;
			}
			Panel.DaoYouContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(viewportHeight, height));
			Canvas.ForceUpdateCanvases();
			float range = Mathf.Max(0f, height - viewportHeight);
			Panel.DaoYouScrollView.verticalNormalizedPosition = followLatest || range == 0f
				? 0f : 1f - Mathf.Clamp01(distanceFromTop / range);
		}

		private void LiKaiChat()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void PinBiChat()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention($"确定与 {m_RoleName} 解除道友关系？", PinBiFriend, title: "道友管理");
		}

		private void PinBiFriend()
		{
			DeleteFriendReq_1014010 data = new DeleteFriendReq_1014010
			{
				TargetFriendId = m_RoleId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.Delete_Friend, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					DeleteFriendResp_1014010 deleteFriendResp_ = new DeleteFriendResp_1014010();
					deleteFriendResp_.MergeFrom(byteString);
					if (deleteFriendResp_.IsSuccess)
					{
						DaoYouWnd daoYouWnd = Singleton<UIManager>.Instance.FindWndByName<DaoYouWnd>("DaoYouWnd.prefab");
						daoYouWnd?.DelectFriend(m_RoleId);
						daoYouWnd?.OnShow();
						Singleton<UIManager>.Instance.CloseWnd(this);
						MonoSingleton<GameStart>.Instance.PopUpAttention("您已与" + m_RoleName + "绝交");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteFriendResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void RefreshSendAction()
		{
			sendCaption.text = sending ? "发送中" : "发送";
			Panel.SendBtn.interactable = !sending && !string.IsNullOrWhiteSpace(Panel.WaitSendInput.text);
		}

		private void SendMsg()
		{
			string contentText = Panel.WaitSendInput.text;
			if (!windowOpen || sending || string.IsNullOrWhiteSpace(contentText))
				return;

			var data = new SendMessageRoleReq_4001007 { Msg = contentText, RoleId = m_RoleId };
			sending = true;
			RefreshSendAction();
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.SendMessageRole, data, bytes =>
			{
				sending = false;
				if (windowOpen)
					RefreshSendAction();
				if (bytes == null)
				{
					if (windowOpen)
						MonoSingleton<GameStart>.Instance.PopRollingTextShow("未收到发送回执，草稿已保留");
					return;
				}
				var reply = SendMessageRoleResp_4001007.Parser.ParseFrom(bytes);
				if (!reply.IsSuccess)
				{
					if (windowOpen)
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(reply.Code));
					return;
				}
				if (windowOpen && Panel.WaitSendInput.text == contentText)
					Panel.WaitSendInput.text = "";
				var message = new FriendEntity
				{
					Content = contentText,
					Timestamp = reply.MsgTime,
					FromRoleId = Singleton<RoleManager>.Instance.Role_Entity.Id,
					ReceiveTime = CommonMethod.GetTimeStampTime((reply.MsgTime / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm")
				};
				Singleton<ChatManager>.Instance.SendRoleMsg(message, m_RoleId);
				ShowMessage(message);
			}))
			{
				sending = false;
				RefreshSendAction();
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("连接已断开，草稿已保留");
			}
		}

		private void Receive(FriendEntity friendEntity)
		{
			if (friendEntity.FromRoleId.Equals(m_RoleId))
			{
				ShowMessage(friendEntity);
				Singleton<ChatManager>.Instance.EnterRoleChat(friendEntity.FromRoleId);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			windowOpen = false;
			++pageRequest;
			arriving.Clear();
			Singleton<ChatManager>.Instance.ReceiveMsgAction -= Receive;
		}
	}
}
