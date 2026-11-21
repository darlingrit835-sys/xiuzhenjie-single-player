using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LingQuanJiuSiWnd : Window
	{
		public LingQuanJiuSiPanel Panel;

		private int m_CurrentFloor;
		private int m_WorldJoinAttempt;

		private LingQuanJiuSiModeEnum m_CurrentMode;

		private string m_Address;

		private int m_Port;

		private Queue<OtherChatItem> m_ChatItemQue;

		private Queue<DanMuItem> m_CanUseDanMuQue;

		private TavernView view;
		private TavernShareSheet shares;

		private string m_CurrentRoomId;

		private string m_NextRoomId;

		private string m_BeforeRoomId;

		private GameObject m_ChatInObj;

		private GameObject m_ChatDanMuItem;

		private GameObject m_JiuItem;

		private bool TimeLimite;

		private DateTime m_BeginTime;

		private TimeSpan m_CurrentTime;

		private long m_CurrentTimeSpan;

		private string m_GroupKey = "Eok4DbNeNOvGDuzyS6fBrAwBVei38riq";

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LingQuanJiuSiPanel>();
			view = new TavernView(Panel);
			shares = new TavernShareSheet(Panel, SendContent);
			AddButtonClickListener(view.Share, shares.Menu);
			AddButtonClickListener(Panel.BackBtn, CloseWindow);
			AddButtonClickListener(Panel.ChangeModeBtn, ChangeChatMode);
			AddButtonClickListener(Panel.PayContent_CloseBtn, PayContent_Close);
			AddButtonClickListener(Panel.SendBtn, SendCheck);
			AddButtonClickListener(Panel.YiJianJiaQunBtn, YiJianJiaQunBtnClick);
			m_ChatItemQue = new Queue<OtherChatItem>();
			m_CanUseDanMuQue = new Queue<DanMuItem>();
			m_CurrentMode = LingQuanJiuSiModeEnum.Chat;
			m_ChatInObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/OtherChatItem.prefab");
			m_ChatDanMuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DanMuItem.prefab");
			m_JiuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/PayContentItem.prefab");
			m_BeginTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeLimite = true;
			m_CurrentFloor = 1;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ChatManager chat = Singleton<ChatManager>.Instance;
			chat.ReceiveRoomMsgAction -= ReceiveMsg;
			chat.ReceiveRoomMsgAction += ReceiveMsg;
			if (Constants.UsesPackagedTrialData)
			{
				EnterWorldChat();
				return;
			}
			if (!Singleton<NetManager>.Instance.ConnectedSocket.ContainsKey(ServerType.ChatServer))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("无法连接聊天服务器，是否重试?", () =>
				{
					Singleton<ChatManager>.Instance.Init(ReConnectServer, isShowWait: true);
				}, () =>
				{
					Singleton<UIManager>.Instance.CloseWnd(this);
				});
			}
			else
			{
				ReConnectServer(ChatRoomType.LingQuanJiuSi);
			}
		}

		private void EnterWorldChat()
		{
			int attempt = ++m_WorldJoinAttempt;
			Panel.SendBtn.interactable = view.Share.interactable = false;
			Panel.TitleText.text = "正在进入酒肆…";
			Singleton<ChatManager>.Instance.JoinWorldChatOverGame(joined =>
			{
				if (attempt != m_WorldJoinAttempt)
				{
					return;
				}
				Panel.SendBtn.interactable = view.Share.interactable = joined;
				if (joined)
				{
					ReConnectServer(ChatRoomType.LingQuanJiuSi);
					return;
				}
				Panel.TitleText.text = "酒肆连接未就绪";
				MonoSingleton<GameStart>.Instance.PopUpAttention("暂时无法进入酒肆，是否重试？", () =>
				{
					if (attempt == m_WorldJoinAttempt)
					{
						EnterWorldChat();
					}
				}, () =>
				{
					if (attempt == m_WorldJoinAttempt)
					{
						CloseWindow();
					}
				});
			});
		}

		private void ReConnectServer(ChatRoomType chatRoomType)
		{
			if (chatRoomType != ChatRoomType.LingQuanJiuSi || Panel == null || Panel.ChatContent_ScrollRectContent == null || Panel.ChatContent_ScrollRectStandByContent == null)
			{
				return;
			}
			ClearChatContent();
			m_ChatItemQue.Clear();
			Panel.JieShiPanel.PanelHide();
			ReflashFloor();
			foreach (ChatModel item in Singleton<ChatManager>.Instance.ChatQue)
			{
				CreateChatItem(item);
			}
			view.SetEmpty(m_ChatItemQue.Count == 0);
			ScrollToLatest();
		}

		private void ReflashFloor()
		{
			Panel.TitleText.text = $"当前文明值：{Singleton<ChatManager>.Instance.CreditPoints}，发言间隔：{Singleton<ChatManager>.Instance.TimeLag}息";
			ClearDanMuContent();
			view.SetMode(m_CurrentMode);
			if (m_CurrentMode == LingQuanJiuSiModeEnum.Chat)
			{
				Panel.ChatContent.gameObject.SetActive(value: true);
				Panel.DanMuContent_Total.gameObject.SetActive(value: false);
			}
			else
			{
				Panel.ChatContent.gameObject.SetActive(value: false);
				Panel.DanMuContent_Total.gameObject.SetActive(value: true);
			}
		}

		private void ClearDanMuContent()
		{
			foreach (Transform item in Panel.DanMuContent_1)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in Panel.DanMuContent_2)
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
			foreach (Transform item3 in Panel.DanMuContent_3)
			{
				UnityEngine.Object.Destroy(item3.gameObject);
			}
			foreach (Transform item4 in Panel.DanMuContent_4)
			{
				UnityEngine.Object.Destroy(item4.gameObject);
			}
			foreach (Transform item5 in Panel.DanMuContent_5)
			{
				UnityEngine.Object.Destroy(item5.gameObject);
			}
			foreach (Transform item6 in Panel.DanMuContent_6)
			{
				UnityEngine.Object.Destroy(item6.gameObject);
			}
			foreach (Transform item7 in Panel.DanMuContent_7)
			{
				UnityEngine.Object.Destroy(item7.gameObject);
			}
		}

		private void ClearChatContent()
		{
			foreach (var row in m_ChatItemQue)
			{
				row.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(row.gameObject);
			}
		}

		public void ReceiveMsg(ChatModel chatModel)
		{
			if (chatModel.ChatId == Constants.WorldChatID)
			{
				CreateChatItem(chatModel);
				view.SetEmpty(false);
				ScrollToLatest();
				if (m_CurrentMode == LingQuanJiuSiModeEnum.DanMu)
				{
					CreateChatDanMuItem(chatModel);
				}
			}
		}

		private void PayContent_Close()
		{
			Panel.PayContent.gameObject.SetActive(value: false);
		}

		private void SendCheck()
		{
			SendMsg();
		}

		private void BuyWinde(int wineType, int chatCount)
		{
			BuyWineReq_1012002 data = new BuyWineReq_1012002
			{
				WineType = wineType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUY_WINE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuyWineResp_1012002 buyWineResp_ = new BuyWineResp_1012002();
					buyWineResp_.MergeFrom(byteString);
					if (buyWineResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buyWineResp_.ShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						SendMsg();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buyWineResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.PayContent.gameObject.SetActive(value: false);
				}
			}, isShowWaitUI: true);
		}

		private void SendMsg()
		{
			string draft = Panel.ChatInput.text;
			if (string.IsNullOrWhiteSpace(draft))
			{
				return;
			}
			SendContent(draft, success =>
			{
				if (success && Panel.ChatInput.text == draft) Panel.ChatInput.text = string.Empty;
			});
		}

		private void SendContent(string message, Action<bool> completed)
		{
			SendMessageReq_4001003 data = new SendMessageReq_4001003
			{
				Msg = message,
				ChatId = Constants.WorldChatID
			};
			if (Constants.UsesPackagedTrialData)
			{
				int attempt = m_WorldJoinAttempt;
				bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.SEND_MESSAGE, data, payload =>
				{
					if (attempt != m_WorldJoinAttempt)
					{
						return;
					}
					var response = payload == null ? null : SendMessageResp_4001003.Parser.ParseFrom(payload);
					if (response != null && response.IsSuccess)
					{
						completed(true);
						Panel.TitleText.text = $"当前文明值：{response.CreditPoints}，发言间隔：{response.TimeLag / 1000}息";
					}
					else if (response == null || response.Code == 4347)
					{
						completed(false);
						EnterWorldChat();
					}
					else
					{
						completed(false);
						MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(response.Code));
					}
				});
				if (!sent)
				{
					completed(false);
					EnterWorldChat();
				}
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEND_MESSAGE, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SendMessageResp_4001003 sendMessageResp_ = new SendMessageResp_4001003();
					sendMessageResp_.MergeFrom(byteString);
					if (!sendMessageResp_.IsSuccess)
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sendMessageResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					else
					{
						ChatInformReq_1012005 data2 = new ChatInformReq_1012005
						{
							Type = 1
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.CHAT_INFORM, data2, null);
						Panel.TitleText.text = $"当前文明值：{sendMessageResp_.CreditPoints}，发言间隔：{sendMessageResp_.TimeLag / 1000}息";
					}
					completed(sendMessageResp_.IsSuccess);
				}
				else completed(false);
			}, isShowWaitUI: false, ServerType.ChatServer);
		}

		private void CreateChatItem(ChatModel chatModel)
		{
			OtherChatItem row;
			if (m_ChatItemQue.Count == 100)
			{
				row = m_ChatItemQue.Dequeue();
				row.transform.SetAsLastSibling();
			}
			else
			{
				row = SetListContentParent(Panel.ChatContent_ScrollRectContent, m_ChatInObj).GetComponent<OtherChatItem>();
				var button = row.gameObject.AddComponent<Button>();
				RoleFolio.Touch(button);
				row.TouXiang.gameObject.AddComponent<Button>().targetGraphic = row.TouXiang;
			}

			bool own = IsSelf(chatModel);
			string sender = string.IsNullOrEmpty(chatModel.ChengHao) ? chatModel.RoleName : chatModel.RoleName + " · " + chatModel.ChengHao;
			var card = chatModel.Card;
			string content = card == null ? chatModel.Content : card.Caption + "\n" + card.Summary + "\n查看详情 ›";
			row.SetMessage(content, sender, chatModel.Sex, chatModel.HeadId, true, own);
			row.SendTime.supportRichText = false;
			if (card != null)
			{
				InkInterfaceArt.Set(row.ChatBgRect.GetComponent<Image>(), "ParchmentHeading", false);
				row.ChatText.color = InkInterfaceArt.Ink;
			}
			row.TouXiang.raycastTarget = true;
			var portrait = row.TouXiang.GetComponent<Button>();
			portrait.onClick.RemoveAllListeners();
			portrait.onClick.AddListener(() => ShowSpeaker(chatModel));
			var settings = row.ChatText.GetGenerationSettings(new Vector2(row.ChatBgRect.rect.width - 24f, 0f));
			float textHeight = row.ChatText.cachedTextGeneratorForLayout.GetPreferredHeight(content, settings) / row.ChatText.pixelsPerUnit;
			float height = Mathf.Max(38f, Mathf.Ceil(textHeight) + 18f);
			row.ChatBgRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			((RectTransform)row.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + 21f);
			var click = row.GetComponent<Button>();
			click.onClick.RemoveAllListeners();
			click.onClick.AddListener(() => OpenMessage(chatModel));
			m_ChatItemQue.Enqueue(row);
		}

		private void ScrollToLatest()
		{
			if (Panel.ChatContent.gameObject.activeInHierarchy)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(Panel.ChatContent_ScrollRectContent);
				Panel.ChatContent_ScrollRect.verticalNormalizedPosition = 0f;
			}
		}

		private static bool IsSelf(ChatModel model)
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			return string.IsNullOrEmpty(model.RoleId) ? model.RoleName == role.RoleName : model.RoleId == role.Id;
		}

		private void OpenMessage(ChatModel model)
		{
			if (model.Card == null) ShowSpeaker(model);
			else shares.Read(model.Card, model.RoleName, IsSelf(model), () => ShowSpeaker(model));
		}

		private void ShowSpeaker(ChatModel model)
		{
			Sprite portrait = Singleton<RoleManager>.Instance.GetTouXiangSprite(model.Sex, model.HeadId.ToString());
			Panel.JieShiPanel.SetValueAndShow(model.RoleName, model.Level, portrait, SendMingCiClick, model.HeadFrame);
		}

		private void CreateChatDanMuItem(ChatModel chatModel)
		{
			RectTransform rectTransform = ChooseCanUseDanMuContent();
			if (m_CanUseDanMuQue.Count == 0)
			{
				GameObject gameObject = SetListContentParent(rectTransform, m_ChatDanMuItem);
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				var danmu = gameObject.GetComponent<DanMuItem>();
				InkInterfaceArt.Set(danmu.Content.GetComponent<Image>(), "JadeButton", false);
				danmu.ContentText.color = InkDialogArt.Ivory;
				danmu.SetValue(chatModel.HeadFrame, chatModel.TitleFrame, chatModel.Sex, chatModel.Level, chatModel.HeadId.ToString(), chatModel.ChengHao, chatModel.RoleName, chatModel.Card?.Caption ?? chatModel.Content, DanMuMoveCompelete, _ => OpenMessage(chatModel));
			}
			else
			{
				DanMuItem danMuItem = m_CanUseDanMuQue.Dequeue();
				danMuItem.transform.SetParent(rectTransform);
				danMuItem.ReSetValue(chatModel.HeadFrame, chatModel.TitleFrame, chatModel.Sex, chatModel.Level, chatModel.HeadId.ToString(), chatModel.ChengHao, chatModel.RoleName, chatModel.Card?.Caption ?? chatModel.Content, _ => OpenMessage(chatModel));
			}
			Panel.ChatScrollBar.value = 0f;
		}

		private void DanMuMoveCompelete(DanMuItem danMuItem)
		{
			danMuItem.transform.SetParent(Panel.DanMuStandByContent);
			m_CanUseDanMuQue.Enqueue(danMuItem);
		}

		private RectTransform ChooseCanUseDanMuContent()
		{
			RectTransform danMuContent_ = Panel.DanMuContent_1;
			Dictionary<float, RectTransform> dictionary = new Dictionary<float, RectTransform>();
			List<float> list = new List<float>();
			if (Panel.DanMuContent_1.childCount == 0)
			{
				return Panel.DanMuContent_1;
			}
			float endDistance = Panel.DanMuContent_1.GetChild(Panel.DanMuContent_1.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance))
			{
				dictionary.Add(endDistance, Panel.DanMuContent_1);
				list.Add(endDistance);
			}
			if (Panel.DanMuContent_2.childCount == 0)
			{
				return Panel.DanMuContent_2;
			}
			float endDistance2 = Panel.DanMuContent_2.GetChild(Panel.DanMuContent_2.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance2))
			{
				dictionary.Add(endDistance2, Panel.DanMuContent_2);
				list.Add(endDistance2);
			}
			if (Panel.DanMuContent_3.childCount == 0)
			{
				return Panel.DanMuContent_3;
			}
			float endDistance3 = Panel.DanMuContent_3.GetChild(Panel.DanMuContent_3.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance3))
			{
				dictionary.Add(endDistance3, Panel.DanMuContent_3);
				list.Add(endDistance3);
			}
			if (Panel.DanMuContent_4.childCount == 0)
			{
				return Panel.DanMuContent_4;
			}
			float endDistance4 = Panel.DanMuContent_4.GetChild(Panel.DanMuContent_4.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance4))
			{
				dictionary.Add(endDistance4, Panel.DanMuContent_4);
				list.Add(endDistance4);
			}
			if (Panel.DanMuContent_5.childCount == 0)
			{
				return Panel.DanMuContent_5;
			}
			float endDistance5 = Panel.DanMuContent_5.GetChild(Panel.DanMuContent_5.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance5))
			{
				dictionary.Add(endDistance5, Panel.DanMuContent_5);
				list.Add(endDistance5);
			}
			if (Panel.DanMuContent_6.childCount == 0)
			{
				return Panel.DanMuContent_6;
			}
			float endDistance6 = Panel.DanMuContent_6.GetChild(Panel.DanMuContent_6.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance6))
			{
				dictionary.Add(endDistance6, Panel.DanMuContent_6);
				list.Add(endDistance6);
			}
			if (Panel.DanMuContent_7.childCount == 0)
			{
				return Panel.DanMuContent_7;
			}
			float endDistance7 = Panel.DanMuContent_7.GetChild(Panel.DanMuContent_7.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance7))
			{
				dictionary.Add(endDistance7, Panel.DanMuContent_7);
				list.Add(endDistance7);
			}
			list.Sort((float left, float right) =>
			{
				if (left > right)
				{
					return 1;
				}
				return (left != right) ? (-1) : 0;
			});
			if (dictionary[list[0]] == null)
			{
				return danMuContent_;
			}
			return dictionary[list[0]];
		}

		private void SendMingCiClick(string roleName)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleName.Equals(roleName))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("无法给向自己发送名刺！");
				return;
			}
			AddFriendByNameReq_1014007 data = new AddFriendByNameReq_1014007
			{
				Name = roleName
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ADDFRIEND_BYNAME, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AddFriendByNameResp_1014007 addFriendByNameResp_ = new AddFriendByNameResp_1014007();
					addFriendByNameResp_.MergeFrom(byteString);
					if (addFriendByNameResp_.IsSuccess)
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addFriendByNameResp_.Code);
						hintMsgByCode = "发送名刺成功!";
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addFriendByNameResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
					}
					Panel.JieShiPanel.PanelHide();
				}
			}, isShowWaitUI: true);
		}

		private void ChangeChatMode()
		{
			if (m_CurrentMode == LingQuanJiuSiModeEnum.Chat)
			{
				m_CurrentMode = LingQuanJiuSiModeEnum.DanMu;
			}
			else
			{
				m_CurrentMode = LingQuanJiuSiModeEnum.Chat;
			}
			ReflashFloor();
			ScrollToLatest();
		}

		public override void CloseWindow()
		{
			LeaveJiuGuan();
		}

		public void YiJianJiaQunBtnClick()
		{
		}

		private void LeaveJiuGuan()
		{
			if (!Constants.UsesPackagedTrialData)
			{
				Singleton<ChatManager>.Instance.LiKaiChatRoom(Constants.WorldChatID);
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public override void OnDisable()
		{
			++m_WorldJoinAttempt;
			shares.Close();
			if (Constants.UsesPackagedTrialData)
			{
				Singleton<ChatManager>.Instance.LiKaiChatRoom(Constants.WorldChatID);
			}
			base.OnDisable();
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
		}
	}
}
