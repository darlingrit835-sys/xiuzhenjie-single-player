using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DaoYouWnd : Window
	{
		public DaoYouPanel Panel;

		private readonly List<DaoYouEntity> friends = new List<DaoYouEntity>();
		private readonly List<DaoYouEntity> visibleFriends = new List<DaoYouEntity>();
		private readonly List<EnterAddFriendResp_1014003.Types.StrangerResp_1014004> inbox =
			new List<EnterAddFriendResp_1014003.Types.StrangerResp_1014004>();
		private DaoYouRows<DaoYouItem> friendRows;
		private DaoYouRows<MingCiItem> inboxRows;
		private Button friendsTab;
		private Button onlineFilter;
		private InputField nameFilter;
		private Text emptyMessage;
		private bool onlineOnly;
		private bool reading;
		private bool writing;
		private bool friendsLoaded;
		private bool inboxLoaded;
		private int waitingCount;
		private int listRevision;
		private int actionRevision;
		private Timer listTimeout;
		private Timer actionTimeout;
		private string listError;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = GameObject.GetComponent<DaoYouPanel>();
			BuildLayout();

			var friendPrefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DaoYouItem.prefab");
			var inboxPrefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/MingCiItem.prefab");
			Panel.DaoYouVerticalContent.enabled = false;
			Panel.MingCiVerticalContent.enabled = false;
			friendRows = new DaoYouRows<DaoYouItem>(Panel.DaoYouScroll, friendPrefab, item => item.StyleCard(), BindFriend);
			inboxRows = new DaoYouRows<MingCiItem>(Panel.MingCiScroll, inboxPrefab, StyleInboxCard, BindInbox);

			AddToggleClickListener(Panel.BaiHuiMingCiBtn, ShowInbox);
			AddButtonClickListener(friendsTab, ShowFriends);
			AddButtonClickListener(onlineFilter, ToggleOnlineOnly);
			AddButtonClickListener(Panel.JieShiDaoYouBtn, OpenSendForm);
			AddButtonClickListener(Panel.SouSuoBtn, SendNameCard);
			AddButtonClickListener(Panel.CloseBtn, CloseSendForm);
			AddButtonClickListener(Panel.DaoLvBtn, Refresh);
			AddButtonClickListener(Panel.ShiTuBtn, () => Singleton<UIManager>.Instance.PopUpWnd("ShiTuWnd.prefab", true));
			nameFilter.onValueChanged.AddListener(_ => FilterFriends());
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			FriendGoodsOffers.RequestRefresh();
			var chat = Singleton<ChatManager>.Instance;
			chat.ReceiveMsgAction -= Receive;
			chat.ReceiveMsgAction += Receive;
			ShowInbox(Panel.BaiHuiMingCiBtn.isOn);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Singleton<ChatManager>.Instance.ReceiveMsgAction -= Receive;
			Timer.Cancel(listTimeout);
			Timer.Cancel(actionTimeout);
			listRevision++;
			actionRevision++;
			FinishAction();
		}

		private void ShowFriends()
		{
			Panel.BaiHuiMingCiBtn.SetIsOnWithoutNotify(false);
			ShowInbox(false);
		}

		private void ShowInbox(bool showingInbox)
		{
			Panel.DaoYouScroll.gameObject.SetActive(!showingInbox);
			Panel.MingCiScroll.gameObject.SetActive(showingInbox);
			nameFilter.gameObject.SetActive(!showingInbox);
			onlineFilter.gameObject.SetActive(!showingInbox);
			DaoYouItem.StyleAction(friendsTab, "道友名册", primary: !showingInbox);
			InkInterfaceArt.Set(Panel.BaiHuiMingCiBtn.targetGraphic as Image, showingInbox ? "LightAction" : "LightPlaque", false);
			Panel.BaiHuiMingCiText.color = showingInbox ? InkDialogArt.Ivory : InkInterfaceArt.Ink;
			Canvas.ForceUpdateCanvases();
			Refresh();
		}

		private void ToggleOnlineOnly()
		{
			onlineOnly = !onlineOnly;
			DaoYouItem.StyleAction(onlineFilter, onlineOnly ? "仅在线" : "全部状态", primary: onlineOnly);
			FilterFriends();
		}

		private void FilterFriends()
		{
			string query = nameFilter.text.Trim();
			visibleFriends.Clear();
			foreach (DaoYouEntity friend in friends)
			{
				if ((!onlineOnly || friend.IsOnline) && (query.Length == 0 ||
				    friend.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0))
					visibleFriends.Add(friend);
			}
			friendRows.Reset(visibleFriends.Count);
			UpdateListState();
		}

		private void Refresh()
		{
			Timer.Cancel(listTimeout);
			int revision = ++listRevision;
			reading = false;
			listError = null;
			var network = Singleton<NetManager>.Instance;
			if (!network.GameServerIsAlive())
			{
				ListFailed("连接已断开，重连后可刷新。");
				return;
			}

			bool showingInbox = Panel.BaiHuiMingCiBtn.isOn;
			reading = true;
			UpdateListState();
			listTimeout = Timer.Register(8f, () =>
			{
				if (revision == listRevision)
				{
					listRevision++;
					ListFailed("读取超时，请刷新重试。");
				}
			}, useRealTime: true, autoDestroyOwner: Panel);
			network.SendMsg(showingInbox ? MsgType.ENTER_ADDFRIEND : MsgType.ENTER_FRIEND, null, payload =>
			{
				if (revision != listRevision || GameObject == null || !GameObject.activeInHierarchy)
					return;

				Timer.Cancel(listTimeout);
				reading = false;
				if (!(payload is ByteString bytes))
				{
					ListFailed("没有收到名单，请刷新重试。");
					return;
				}

				try
				{
					if (showingInbox)
						ReadInbox(EnterAddFriendResp_1014003.Parser.ParseFrom(bytes));
					else
						ReadFriends(EnterFriendResp_1014001.Parser.ParseFrom(bytes));
				}
				catch (InvalidProtocolBufferException)
				{
					ListFailed("暂时无法读取名单，请刷新重试。");
				}
			}, isShowWaitUI: false, serverType: ServerType.GamesNormalServer, showTimeout: false);
		}

		private void ListFailed(string message)
		{
			reading = false;
			listError = message;
			UpdateListState();
		}

		private void ReadFriends(EnterFriendResp_1014001 response)
		{
			if (!response.IsSuccess)
			{
				ListFailed(ErrorMessage(response.Code, "暂时无法读取道友名册。"));
				return;
			}

			friendsLoaded = true;
			waitingCount = response.StrangerCount;
			friends.Clear();
			foreach (var record in response.Friend)
			{
				friends.Add(new DaoYouEntity
				{
					Id = record.Id,
					Name = record.Name,
					headPortraitId = record.HeadPortraitId,
					Level = record.Level,
					Intimacy = record.Intimacy,
					IsOnline = record.IsOnLine,
					Sex = record.Sex ? 1 : 0,
					LiveType = new List<int>(record.LiveType)
				});
			}
			friends.Sort((left, right) =>
			{
				int byOnline = right.IsOnline.CompareTo(left.IsOnline);
				return byOnline != 0 ? byOnline : string.CompareOrdinal(left.Name, right.Name);
			});
			FilterFriends();
		}

		private void ReadInbox(EnterAddFriendResp_1014003 response)
		{
			if (!response.IsSuccess)
			{
				ListFailed(ErrorMessage(response.Code, "暂时无法读取名刺。"));
				return;
			}

			inboxLoaded = true;
			inbox.Clear();
			inbox.AddRange(response.Stranger);
			waitingCount = inbox.Count;
			inboxRows.Reset(inbox.Count);
			UpdateListState();
		}

		private void UpdateListState()
		{
			bool showingInbox = Panel.BaiHuiMingCiBtn.isOn;
			bool loaded = showingInbox ? inboxLoaded : friendsLoaded;
			int shownCount = showingInbox ? inbox.Count : visibleFriends.Count;
			Panel.DaoLvBtn.interactable = !reading;
			Panel.BaiHuiMingCiText.text = waitingCount == 0 ? "拜会名刺" : $"拜会名刺 · {waitingCount}";
			Panel.MinTitleText.text = reading ? "正在读取……" : listError != null ? "读取失败 · 点击刷新" :
				showingInbox ? $"待处理名刺 {waitingCount} 份" : $"道友 {friends.Count} 人 · 在线 {friends.Count(friend => friend.IsOnline)} 人";
			emptyMessage.text = reading ? (showingInbox ? "正在读取名刺……" : "正在读取道友名册……") : listError ??
				(showingInbox ? "还没有收到名刺。" : friends.Count == 0 ? "名册暂无道友。\n可输入角色名，向对方递一份名刺。" :
					"没有符合筛选条件的道友。");
			emptyMessage.gameObject.SetActive(shownCount == 0 && (loaded || reading || listError != null));
		}

		private void BindFriend(DaoYouItem item, int index)
		{
			var friend = visibleFriends[index];
			int unread = 0;
			var chat = Singleton<ChatManager>.Instance.ChatRoleMsgDic;
			if (chat != null && chat.TryGetValue(friend.Id, out var messages))
				unread = messages.Count;

			item.SetValue(index, friend.Id, friend.IsOnline, unread, friend.Sex == 1, friend.headPortraitId,
				friend.Name, friend.Level, friend.Intimacy, ChuanYinToFriend, ChuanWuToFriend,
				GetOccupation(friend.LiveType, 0), GetOccupation(friend.LiveType, 1), ConfirmRemove);
		}

		private void BindInbox(MingCiItem item, int index)
		{
			var card = inbox[index];
			item.FuZhiYi_Img.gameObject.SetActive(false);
			item.FuZhiEr_Img.gameObject.SetActive(false);
			item.SetValue(index, card.Id, false, card.Sex, card.HeadPortraitId, card.Name, card.Level,
				(_, __, id, name) => AcceptNameCard(id, name),
				(_, __, id, name) => RejectNameCard(id, name),
				GetOccupation(card.LiveType, 0), GetOccupation(card.LiveType, 1));
			item.QinMiDuText.text = "请求结识道友";
			item.RoleLevel.text = WorldSetting.GetRoleLevelNameByRoleLevel(card.Level);
		}

		private static RoleFuZhiType GetOccupation(IList<int> types, int index)
		{
			return types != null && index < types.Count ? (RoleFuZhiType)types[index] : RoleFuZhiType.None;
		}

		private void Receive(FriendEntity friend)
		{
			if (Singleton<UIManager>.Instance.FindWndByName<DaoYouChatWnd>("DaoYouChatWnd.prefab") == null)
				friendRows.Rebind();
		}

		public void DelectFriend(string roleId)
		{
			Singleton<ChatManager>.Instance.EnterRoleChat(roleId);
			friends.RemoveAll(friend => friend.Id == roleId);
			FilterFriends();
		}

		private void OpenSendForm()
		{
			Panel.JieShiDaoYouPanel.gameObject.SetActive(true);
			Panel.JieShiDaoYouPanel.SetAsLastSibling();
		}

		private void CloseSendForm()
		{
			Panel.JieShiDaoYouPanel.gameObject.SetActive(false);
		}

		private void SendNameCard()
		{
			string name = Panel.JueSeMing.text.Trim();
			if (name.Length == 0)
			{
				ShowToast("请输入对方的完整角色名。");
				return;
			}
			if (name == Singleton<RoleManager>.Instance.Role_Entity?.RoleName)
			{
				ShowToast("不能向自己递名刺。");
				return;
			}
			SendAction(MsgType.ADDFRIEND_BYNAME, new AddFriendByNameReq_1014007 { Name = name },
				AddFriendByNameResp_1014007.Parser, response =>
				{
					if (!response.IsSuccess)
					{
						ShowToast(ErrorMessage(response.Code, "递名刺失败，请核对角色名后重试。"));
						return;
					}
					Panel.JueSeMing.text = string.Empty;
					CloseSendForm();
					ShowToast($"已向{name}递出名刺，等待对方同意。");
				});
		}

		private void AcceptNameCard(string id, string name)
		{
			SendAction(MsgType.ADD_FRIEND, new AddFriendReq_1014006 { TargetFriendId = id },
				AddFriendResp_1014006.Parser, response =>
				{
					if (response.IsSuccess)
						FinishNameCard(id, $"你和{name}已结为道友。");
					else
						ShowToast(ErrorMessage(response.Code, "暂时无法接受这份名刺。"));
				});
		}

		private void RejectNameCard(string id, string name)
		{
			SendAction(MsgType.REFUSE_FRIEND, new RefuseFriendReq_1014009 { TargetFriendId = id },
				RefuseFriendResp_1014009.Parser, response =>
				{
					if (response.IsSuccess)
						FinishNameCard(id, $"已婉拒{name}的名刺。");
					else
						ShowToast(ErrorMessage(response.Code, "暂时无法拒绝这份名刺。"));
				});
		}

		private void FinishNameCard(string id, string message)
		{
			inbox.RemoveAll(card => card.Id == id);
			waitingCount = inbox.Count;
			inboxRows.Reset(inbox.Count, returnToTop: false);
			ShowToast(message);
			Refresh();
		}

		private void ConfirmRemove(DaoYouFirendEntity friend)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention($"确定与{friend.RoleName}解除道友关系？", () =>
			{
				if (GameObject == null)
					return;
				SendAction(MsgType.Delete_Friend, new DeleteFriendReq_1014010 { TargetFriendId = friend.RoleId },
					DeleteFriendResp_1014010.Parser, response =>
					{
						if (response.IsSuccess)
						{
							DelectFriend(friend.RoleId);
							ShowToast($"已与{friend.RoleName}解除道友关系。");
							Refresh();
						}
						else
							ShowToast(ErrorMessage(response.Code, "暂时无法解除道友关系。"));
					});
			});
		}

		private void SendAction<T>(MsgType command, IMessage request, MessageParser<T> parser, Action<T> complete)
			where T : IMessage<T>
		{
			if (writing)
			{
				ShowToast("正在处理，请稍候。");
				return;
			}
			var network = Singleton<NetManager>.Instance;
			if (!network.GameServerIsAlive())
			{
				ShowToast("连接已断开，重连后再试。");
				return;
			}
			writing = true;
			Panel.SouSuoBtn.interactable = false;
			int revision = ++actionRevision;
			actionTimeout = Timer.Register(8f, () =>
			{
				if (revision != actionRevision)
					return;
				actionRevision++;
				FinishAction();
				ShowToast("回应超时，请先刷新确认当前状态。");
			}, useRealTime: true, autoDestroyOwner: Panel);
			network.SendMsg(command, request, payload =>
			{
				if (revision != actionRevision || GameObject == null || !GameObject.activeInHierarchy)
					return;
				Timer.Cancel(actionTimeout);
				FinishAction();
				if (!(payload is ByteString bytes))
				{
					ShowToast("没有收到回应，请先刷新确认当前状态。");
					return;
				}
				try
				{
					complete(parser.ParseFrom(bytes));
				}
				catch (InvalidProtocolBufferException)
				{
					ShowToast("暂时无法读取回应，请先刷新确认当前状态。");
				}
			}, isShowWaitUI: false, serverType: ServerType.GamesNormalServer, showTimeout: false);
		}

		private void FinishAction()
		{
			writing = false;
			Panel.SouSuoBtn.interactable = true;
		}

		private static string ErrorMessage(int code, string fallback)
		{
			if (code == 4008)
				return "当前服务器暂未开放此操作。";
			string message = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code);
			return string.IsNullOrEmpty(message) || message.StartsWith("Error Code", StringComparison.Ordinal) ? fallback : message;
		}

		private static void ShowToast(string message)
		{
			MonoSingleton<GameStart>.Instance.PopRollingTextShow(message);
		}

		private void ChuanWuToFriend(DaoYouFirendEntity friend)
		{
			Singleton<UIManager>.Instance.PopUpWnd("DaoYouChuanWuWnd.prefab", true, friend.RoleId, friend.RoleName);
		}

		private void ChuanYinToFriend(DaoYouFirendEntity friend)
		{
			Singleton<UIManager>.Instance.PopUpWnd("DaoYouChatWnd.prefab", true, friend.RoleId,
				friend.RoleName, friend.RoleSex, friend.RoleHeadId);
		}

		private void BuildLayout()
		{
			Transform root = GameObject.transform;
			InkSocialArt.ApplyBackdrop(root, root.GetComponent<Image>());
			var heading = (RectTransform)root.Find("Title_image");
			heading.gameObject.SetActive(true);
			DaoYouItem.Place(heading, 0f, 1f, 1f, 14f, -14f, -47f, -8f);
			InkInterfaceArt.Set(heading.GetComponent<Image>(), "LightPaper", false);
			Text title = heading.Find("TitleName").GetComponent<Text>();
			title.text = "道友录";
			DaoYouItem.StyleText(title, 23, InkInterfaceArt.Ink);
			InkDialogArt.Inset(title.rectTransform, 72f, 72f, 0f, 0f);
			Button back = heading.Find("BackBtn").GetComponent<Button>();
			back.gameObject.SetActive(true);
			DaoYouItem.Place(back.GetComponent<RectTransform>(), 0f, 0f, .5f, 8f, 66f, -16f, 16f);
			DaoYouItem.StyleAction(back, "返回");
			var backIcon = back.transform.Find("Img");
			if (backIcon != null)
				backIcon.gameObject.SetActive(false);
			AddButtonClickListener(back, ReturnToCity);

			var summary = (RectTransform)root.Find("Title");
			DaoYouItem.Place(summary, 0f, 1f, 1f, 19f, -19f, -73f, -49f);
			summary.GetComponent<Image>().enabled = false;
			DaoYouItem.StyleText(Panel.MinTitleText, 14, InkInterfaceArt.SoftInk);

			Font font = title.font;
			friendsTab = CreateButton(root, "FriendsTab", font);
			DaoYouItem.Place(friendsTab.GetComponent<RectTransform>(), 0f, .5f, 1f, 14f, -4f, -111f, -78f);
			var inboxTab = (RectTransform)Panel.BaiHuiMingCiBtn.transform;
			inboxTab.SetParent(root, false);
			DaoYouItem.Place(inboxTab, .5f, 1f, 1f, 4f, -14f, -111f, -78f);
			Panel.BaiHuiMingCiBtn.transition = Selectable.Transition.ColorTint;
			if (Panel.BaiHuiMingCiBtn.graphic)
				Panel.BaiHuiMingCiBtn.graphic.enabled = false;
			DaoYouItem.StyleText(Panel.BaiHuiMingCiText, 16, InkInterfaceArt.Ink);
			Panel.MingCiNumImg.gameObject.SetActive(false);

			nameFilter = UnityEngine.Object.Instantiate(Panel.JueSeMing, root, false);
			nameFilter.name = "FriendNameFilter";
			nameFilter.onValueChanged.RemoveAllListeners();
			nameFilter.onEndEdit.RemoveAllListeners();
			nameFilter.text = string.Empty;
			InkSocialArt.StyleInput(nameFilter, "筛选道友名");
			DaoYouItem.Place(nameFilter.GetComponent<RectTransform>(), 0f, 1f, 1f, 14f, -110f, -151f, -118f);
			onlineFilter = CreateButton(root, "OnlineOnly", font);
			DaoYouItem.Place(onlineFilter.GetComponent<RectTransform>(), 1f, 1f, 1f, -102f, -14f, -151f, -118f);
			DaoYouItem.StyleAction(onlineFilter, "全部状态");

			PlaceList(Panel.DaoYouScroll, 160f);
			PlaceList(Panel.MingCiScroll, 120f);
			emptyMessage = CreateText(root, "FriendListStatus", font);
			DaoYouItem.Place(emptyMessage.rectTransform, 0f, 1f, .5f, 32f, -32f, -67f, 7f);
			DaoYouItem.StyleText(emptyMessage, 16, InkInterfaceArt.Ink);
			emptyMessage.horizontalOverflow = HorizontalWrapMode.Wrap;

			var footer = (RectTransform)root.Find("BottomBg");
			DaoYouItem.Place(footer, 0f, 1f, 0f, 10f, -10f, 3f, 50f);
			InkInterfaceArt.Set(footer.GetComponent<Image>(), "LightPaper", false);
			PlaceFooterButton(Panel.JieShiDaoYouBtn, footer, 0f, .34f, "递名刺", primary: true);
			PlaceFooterButton(Panel.ShiTuBtn, footer, .34f, .67f, "师徒");
			PlaceFooterButton(Panel.DaoLvBtn, footer, .67f, 1f, "刷新");
			var oldMentorBadge = Panel.ShiTuBtn.transform.Find("ReceiveMc");
			if (oldMentorBadge != null)
				oldMentorBadge.gameObject.SetActive(false);
			var oldMentorBackdrop = Panel.ShiTuBtn.transform.Find("Image");
			if (oldMentorBackdrop != null)
				oldMentorBackdrop.gameObject.SetActive(false);
			StyleSendForm(font);
		}

		private void ReturnToCity()
		{
			var main = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (main?._panel != null)
				main._panel.DaoYou_btn.onClick.Invoke();
			else
				CloseWindow();
		}

		private static void PlaceList(ScrollRect scroll, float top)
		{
			var rect = scroll.GetComponent<RectTransform>();
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(14f, 58f);
			rect.offsetMax = new Vector2(-14f, -top);
			var image = scroll.GetComponent<Image>();
			if (image != null)
				image.color = Color.clear;
		}

		private static void PlaceFooterButton(Button button, RectTransform footer, float left, float right, string title, bool primary = false)
		{
			var rect = button.GetComponent<RectTransform>();
			rect.SetParent(footer, false);
			button.gameObject.SetActive(true);
			DaoYouItem.Place(rect, left, right, 0f, 5f, -5f, 6f, 41f);
			DaoYouItem.StyleAction(button, title, primary);
		}

		private void StyleSendForm(Font font)
		{
			var overlay = Panel.JieShiDaoYouPanel;
			overlay.anchorMin = Vector2.zero;
			overlay.anchorMax = Vector2.one;
			overlay.offsetMin = Vector2.zero;
			overlay.offsetMax = Vector2.zero;
			InkDialogArt.SetWash(overlay);
			var card = (RectTransform)Panel.CloseBtn.transform.parent;
			card.anchorMin = card.anchorMax = new Vector2(.5f, .5f);
			card.anchoredPosition = Vector2.zero;
			card.sizeDelta = new Vector2(Mathf.Clamp(((RectTransform)GameObject.transform).rect.width - 28f, 248f, 316f), 226f);
			InkDialogArt.SetPaper(card.GetComponent<Image>());
			var heading = (RectTransform)card.Find("Title_image");
			DaoYouItem.Place(heading, 0f, 1f, 1f, 14f, -51f, -47f, -12f);
			InkInterfaceArt.Set(heading.GetComponent<Image>(), "LightPlaque", false);
			var title = heading.GetComponentInChildren<Text>(true);
			title.text = "结识道友";
			DaoYouItem.StyleText(title, 19, InkInterfaceArt.Ink);
			var oldTitleArt = heading.Find("Image");
			if (oldTitleArt != null)
				oldTitleArt.gameObject.SetActive(false);
			DaoYouItem.Place(Panel.CloseBtn.GetComponent<RectTransform>(), 1f, 1f, 1f, -45f, -13f, -45f, -13f);
			InkDialogArt.StyleButton(Panel.CloseBtn, "Close", InkInterfaceArt.Ink);

			Text hint = CreateText(card, "NameCardHint", font);
			hint.text = "输入对方完整角色名，对方同意后即可结为道友。";
			DaoYouItem.Place(hint.rectTransform, 0f, 1f, 1f, 19f, -19f, -95f, -53f);
			DaoYouItem.StyleText(hint, 13, InkInterfaceArt.SoftInk);
			hint.horizontalOverflow = HorizontalWrapMode.Wrap;
			var input = Panel.JueSeMing.GetComponent<RectTransform>();
			DaoYouItem.Place(input, 0f, 1f, 1f, 18f, -18f, -145f, -103f);
			InkSocialArt.StyleInput(Panel.JueSeMing, "完整角色名");
			Panel.JueSeMing.characterValidation = InputField.CharacterValidation.None;
			Panel.SendBtn.gameObject.SetActive(false);
			Panel.PlayerInfoRect.gameObject.SetActive(false);
			Panel.SouSuoBtn.transform.SetParent(card, false);
			var footer = card.Find("bottomImg");
			if (footer != null)
				footer.gameObject.SetActive(false);
			DaoYouItem.Place(Panel.SouSuoBtn.GetComponent<RectTransform>(), 0f, 1f, 0f, 18f, -18f, 17f, 59f);
			DaoYouItem.StyleAction(Panel.SouSuoBtn, "发送名刺", primary: true);
			CloseSendForm();
		}

		private static Button CreateButton(Transform parent, string name, Font font)
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			Button button = node.GetComponent<Button>();
			button.targetGraphic = node.GetComponent<Image>();
			var text = CreateText(node.transform, "Text", font);
			InkDialogArt.Inset(text.rectTransform, 3f, 3f, 2f, 2f);
			return button;
		}

		private static Text CreateText(Transform parent, string name, Font font)
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(Text));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			Text text = node.GetComponent<Text>();
			text.font = font;
			text.alignment = TextAnchor.MiddleCenter;
			text.raycastTarget = false;
			return text;
		}

		private static void StyleInboxCard(MingCiItem item)
		{
			DaoYouItem.StyleIdentity(item.transform, item.TouXiangIcon, item.DaoYouName, item.RoleLevel,
				item.QinMiDuText, item.QinMiDuIcon, item.FuZhiYi_Img.transform.parent, rightInset: 85f);
			DaoYouItem.Place(item.JieShiBtn.GetComponent<RectTransform>(), 1f, 1f, 1f, -77f, -8f, -37f, -8f);
			DaoYouItem.Place(item.JuJueBtn.GetComponent<RectTransform>(), 1f, 1f, 0f, -77f, -8f, 8f, 37f);
			DaoYouItem.StyleAction(item.JieShiBtn, "同意", primary: true);
			DaoYouItem.StyleAction(item.JuJueBtn, "婉拒");
			foreach (var button in new[] { item.JieShiBtn, item.JuJueBtn })
			{
				var mask = button.transform.Find("MaskImg");
				if (mask != null)
					mask.gameObject.SetActive(false);
				var badge = button.transform.Find("MsgNum");
				if (badge != null)
					badge.gameObject.SetActive(false);
			}
		}
	}
}
