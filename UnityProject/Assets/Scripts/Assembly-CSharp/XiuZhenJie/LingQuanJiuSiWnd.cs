using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class LingQuanJiuSiWnd : Window
	{
		public LingQuanJiuSiPanel Panel;

		private int m_CurrentFloor;

		private LingQuanJiuSiModeEnum m_CurrentMode;

		private string m_Address;

		private int m_Port;

		private Queue<ChatItem> m_ChatItemQue;

		private Queue<DanMuItem> m_CanUseDanMuQue;

		private ChatItem m_StandbyChatItem;

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
			AddButtonClickListener(Panel.BackBtn, CloseWindow);
			AddButtonClickListener(Panel.ChangeModeBtn, ChangeChatMode);
			AddButtonClickListener(Panel.PayContent_CloseBtn, PayContent_Close);
			AddButtonClickListener(Panel.SendBtn, SendCheck);
			AddButtonClickListener(Panel.YiJianJiaQunBtn, YiJianJiaQunBtnClick);
			m_ChatItemQue = new Queue<ChatItem>();
			m_CanUseDanMuQue = new Queue<DanMuItem>();
			m_CurrentMode = LingQuanJiuSiModeEnum.Chat;
			m_ChatInObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChatItem.prefab");
			m_ChatDanMuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DanMuItem.prefab");
			m_JiuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/PayContentItem.prefab");
			m_BeginTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeLimite = true;
			m_CurrentFloor = 1;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
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
				ChatManager instance = Singleton<ChatManager>.Instance;
				instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
				ChatManager instance2 = Singleton<ChatManager>.Instance;
				instance2.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Combine(instance2.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
			}
		}

		private void ReConnectServer(ChatRoomType chatRoomType)
		{
			if (chatRoomType != ChatRoomType.LingQuanJiuSi || Panel == null || Panel.ChatContent_ScrollRectContent == null || Panel.ChatContent_ScrollRectStandByContent == null)
			{
				return;
			}
			ClearChatContent();
			foreach (ChatModel item in Singleton<ChatManager>.Instance.ChatQue)
			{
				CreateChatItem(item);
			}
			ReflashFloor();
		}

		private void ReflashFloor()
		{
			Panel.TitleText.text = $"当前文明值：{Singleton<ChatManager>.Instance.CreditPoints}，发言间隔：{Singleton<ChatManager>.Instance.TimeLag / 1000}息";
			ClearDanMuContent();
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
			foreach (Transform item in Panel.ChatContent_ScrollRectContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in Panel.ChatContent_ScrollRectStandByContent)
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
		}

		public void ReceiveMsg(ChatModel chatModel)
		{
			if (!(chatModel.ChatId != "world"))
			{
				CreateChatItem(chatModel);
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
			if (string.IsNullOrEmpty(Panel.ChatInput.text))
			{
				return;
			}
			SendMessageReq_4001003 data = new SendMessageReq_4001003
			{
				Msg = Panel.ChatInput.text,
				ChatId = "world"
			};
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
					Panel.ChatInput.text = string.Empty;
				}
			}, isShowWaitUI: false, ServerType.ChatServer);
		}

		private void CreateChatItem(ChatModel chatModel)
		{
			ChatItem chatItem = null;
			if (m_StandbyChatItem == null)
			{
				GameObject gameObject = SetListContentParent(Panel.ChatContent_ScrollRectContent, m_ChatInObj);
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				chatItem = gameObject.GetComponent<ChatItem>();
			}
			else
			{
				chatItem = m_StandbyChatItem;
				m_StandbyChatItem.gameObject.transform.SetParent(Panel.ChatContent_ScrollRectContent);
			}
			chatItem.SetValue(chatModel.HeadFrame, chatModel.TitleFrame, chatModel.Sex, chatModel.Level, chatModel.HeadId.ToString(), chatModel.ChengHao, chatModel.RoleName, chatModel.Content, ChatItemClick);
			m_ChatItemQue.Enqueue(chatItem);
			if (m_ChatItemQue.Count > 100)
			{
				m_StandbyChatItem = m_ChatItemQue.Dequeue();
				if (m_StandbyChatItem == null)
				{
					return;
				}
				m_StandbyChatItem.ClearData();
				m_StandbyChatItem.gameObject.transform.SetParent(Panel.ChatContent_ScrollRectStandByContent);
			}
			Panel.ChatContent_ScrollRectContent.anchoredPosition = new Vector2(Panel.ChatContent_ScrollRectContent.anchoredPosition.x, Panel.ChatContent_ScrollRectContent.anchoredPosition.y + 45f);
		}

		private void ChatItemClick(ChatItemEntity data)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(data.Sex, data.HeadId);
			Panel.JieShiPanel.SetValueAndShow(data.RoleName, data.Level, touXiangSprite, SendMingCiClick, data.HeadFrameId);
		}

		private void CreateChatDanMuItem(ChatModel chatModel)
		{
			RectTransform rectTransform = ChooseCanUseDanMuContent();
			if (m_CanUseDanMuQue.Count == 0)
			{
				GameObject gameObject = SetListContentParent(rectTransform, m_ChatDanMuItem);
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				gameObject.GetComponent<DanMuItem>().SetValue(chatModel.HeadFrame, chatModel.TitleFrame, chatModel.Sex, chatModel.Level, chatModel.HeadId.ToString(), chatModel.ChengHao, chatModel.RoleName, chatModel.Content, DanMuMoveCompelete, DanMuClick);
			}
			else
			{
				DanMuItem danMuItem = m_CanUseDanMuQue.Dequeue();
				danMuItem.transform.SetParent(rectTransform);
				danMuItem.ReSetValue(chatModel.HeadFrame, chatModel.TitleFrame, chatModel.Sex, chatModel.Level, chatModel.HeadId.ToString(), chatModel.ChengHao, chatModel.RoleName, chatModel.Content, DanMuClick);
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

		private void DanMuClick(DanMuItemEntity data)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(data.Sex, data.HeadId);
			Panel.JieShiPanel.SetValueAndShow(data.RoleName, data.Level, touXiangSprite, SendMingCiClick, data.HeadFrameId);
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
			Singleton<ChatManager>.Instance.LiKaiChatRoom("world");
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
		}
	}
}
