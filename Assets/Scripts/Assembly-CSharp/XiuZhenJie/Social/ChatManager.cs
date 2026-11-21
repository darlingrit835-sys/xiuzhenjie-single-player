using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ChatManager : Singleton<ChatManager>
	{
		private long m_NextSendTime;

		private int m_CreditPoints;

		private int m_TimeLag;

		private Queue<ChatModel> m_ChatQue;

		private Queue<ChatModel> m_ZongMenChatQue;

		private Dictionary<string, List<string>> m_ChatRoleMsgDic;

		private Dictionary<string, List<FriendEntity>> m_ChatHistory;

		public UnityAction<ChatRoomType> ConnectAction;

		public UnityAction<FriendEntity> ReceiveMsgAction;

		public event Action FriendUnreadChanged;

		public bool HasUnreadFriendMessages { get; private set; }

		public UnityAction<ChatModel> ReceiveRoomMsgAction;

		public UnityAction ConnectServerAction;

		public long NextSendTime
		{
			get
			{
				return m_NextSendTime;
			}
			private set
			{
				m_NextSendTime = value;
			}
		}

		public int CreditPoints
		{
			get
			{
				return m_CreditPoints;
			}
			private set
			{
				m_CreditPoints = value;
			}
		}

		public int TimeLag
		{
			get
			{
				return m_TimeLag;
			}
			private set
			{
				m_TimeLag = value;
			}
		}

		public Queue<ChatModel> ChatQue
		{
			get
			{
				return m_ChatQue;
			}
			private set
			{
				m_ChatQue = value;
			}
		}

		public Queue<ChatModel> ZongMenChatQue
		{
			get
			{
				return m_ZongMenChatQue;
			}
			private set
			{
				m_ZongMenChatQue = value;
			}
		}

		public Dictionary<string, List<string>> ChatRoleMsgDic
		{
			get
			{
				return m_ChatRoleMsgDic;
			}
			private set
			{
				m_ChatRoleMsgDic = value;
			}
		}

		public Dictionary<string, List<FriendEntity>> ChatHistory
		{
			get
			{
				return m_ChatHistory;
			}
			private set
			{
				m_ChatHistory = value;
			}
		}

		public void Init(bool isShowWait = false)
		{
			m_ChatQue = new Queue<ChatModel>();
			m_ZongMenChatQue = new Queue<ChatModel>();
			m_ChatRoleMsgDic = new Dictionary<string, List<string>>();
			m_ChatHistory = new Dictionary<string, List<FriendEntity>>();
			RefreshFriendUnread();
			ConnectChatServer(isShowWait);
		}

		public void Init(UnityAction<ChatRoomType> CompeleteAction, bool isShowWait = false)
		{
			if (CompeleteAction != null)
			{
				ConnectAction = (UnityAction<ChatRoomType>)Delegate.Combine(ConnectAction, CompeleteAction);
			}
			Init(isShowWait);
		}

		private void ConnectChatServer(bool isShowWait)
		{
			if (isShowWait)
			{
				MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUILD_CHAT, null, (object _obj) =>
			{
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuildChatResp_1012001 buildChatResp_ = new BuildChatResp_1012001();
					buildChatResp_.MergeFrom(byteString);
					if (buildChatResp_.IsSuccess)
					{
						if (isShowWait)
						{
							MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
						}
						NetManager instance = Singleton<NetManager>.Instance;
						instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
						Singleton<NetManager>.Instance.StartChatServer(buildChatResp_.Address, buildChatResp_.Port);
					}
					else
					{
						Debug.Log("无法创建聊天室：" + buildChatResp_.Code);
					}
				}
			});
		}

		private void ConnectSuccess(ServerType serverType, bool isFirstConnect)
		{
			Debug.Log("连接上了" + serverType);
			if (ConnectServerAction != null)
			{
				ConnectServerAction();
			}
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			if (serverType == ServerType.ChatServer)
			{
				NetManager instance = Singleton<NetManager>.Instance;
				instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
				JoinChatServer();
				JoinZongMenChatServer(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
			}
		}

		public void JoinChatServer()
		{
			ChengHaoInfoEntity value = null;
			Singleton<FacadeManager>.Instance.SelectChengHaoInfo()?.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId, out value);
			ChatJoinReq_4001001 data = new ChatJoinReq_4001001
			{
				RoleId = Singleton<RoleManager>.Instance.Role_Entity.Id,
				RoleName = Singleton<RoleManager>.Instance.Role_Entity.RoleName,
				HeadPortraitId = Singleton<RoleManager>.Instance.Role_Entity.HeadPortraitId.ToString(),
				Level = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel,
				Sex = Singleton<RoleManager>.Instance.Role_Entity.Sex,
				ChatId = "world",
				Title = ((value == null) ? "" : value.Name),
				HeadFrame = Singleton<RoleManager>.Instance.Role_Entity.HeadFrameId,
				TitleFrame = Singleton<RoleManager>.Instance.Role_Entity.ChengHaoFrameId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHAT_JOIN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChatJoinResp_4001001 chatJoinResp_ = new ChatJoinResp_4001001();
					chatJoinResp_.MergeFrom(byteString);
					if (chatJoinResp_.IsSuccess)
					{
						m_CreditPoints = chatJoinResp_.CreditPoints;
						m_TimeLag = chatJoinResp_.TimeLag / 1000;
						if (ConnectAction != null)
						{
							ConnectAction(ChatRoomType.LingQuanJiuSi);
						}
					}
				}
			}, isShowWaitUI: false, ServerType.ChatServer);
		}

		public void JoinWorldChatOverGame(UnityAction<bool> completed)
		{
			var request = new ChatJoinReq_4001001 { ChatId = Constants.WorldChatID };
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.CHAT_JOIN, request, payload =>
			{
				var response = payload == null ? null : ChatJoinResp_4001001.Parser.ParseFrom(payload);
				if (response != null && response.IsSuccess)
				{
					m_CreditPoints = response.CreditPoints;
					m_TimeLag = response.TimeLag / 1000;
					m_NextSendTime = response.NextSendTime;
					completed(true);
					return;
				}
				completed(false);
			});
			if (!sent)
			{
				completed(false);
			}
		}

		public void JoinZongMenChatServer(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return;
			}
			ChengHaoInfoEntity value = null;
			Singleton<FacadeManager>.Instance.SelectChengHaoInfo()?.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId, out value);
			ChatJoinReq_4001001 data = new ChatJoinReq_4001001
			{
				RoleId = Singleton<RoleManager>.Instance.Role_Entity.Id,
				RoleName = Singleton<RoleManager>.Instance.Role_Entity.RoleName,
				Title = ((value == null) ? "" : value.Name),
				HeadPortraitId = Singleton<RoleManager>.Instance.Role_Entity.HeadPortraitId.ToString(),
				Level = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel,
				Sex = Singleton<RoleManager>.Instance.Role_Entity.Sex,
				ChatId = id,
				HeadFrame = Singleton<RoleManager>.Instance.Role_Entity.HeadFrameId,
				TitleFrame = Singleton<RoleManager>.Instance.Role_Entity.ChengHaoFrameId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHAT_JOIN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChatJoinResp_4001001 chatJoinResp_ = new ChatJoinResp_4001001();
					chatJoinResp_.MergeFrom(byteString);
					if (chatJoinResp_.IsSuccess)
					{
						m_CreditPoints = chatJoinResp_.CreditPoints;
						m_TimeLag = chatJoinResp_.TimeLag / 1000;
						if (ConnectAction != null)
						{
							ConnectAction(ChatRoomType.ZongMenChat);
						}
					}
				}
			}, isShowWaitUI: false, ServerType.ChatServer);
		}

		public void JoinPMHChatServer(string chatId)
		{
			ChengHaoInfoEntity value = null;
			Singleton<FacadeManager>.Instance.SelectChengHaoInfo()?.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId, out value);
			ChatJoinReq_4001001 data = new ChatJoinReq_4001001
			{
				RoleId = Singleton<RoleManager>.Instance.Role_Entity.Id,
				RoleName = Singleton<RoleManager>.Instance.Role_Entity.RoleName,
				Title = ((value == null) ? "" : value.Name),
				HeadPortraitId = Singleton<RoleManager>.Instance.Role_Entity.HeadPortraitId.ToString(),
				Level = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel,
				Sex = Singleton<RoleManager>.Instance.Role_Entity.Sex,
				ChatId = chatId,
				HeadFrame = Singleton<RoleManager>.Instance.Role_Entity.HeadFrameId,
				TitleFrame = Singleton<RoleManager>.Instance.Role_Entity.ChengHaoFrameId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHAT_JOIN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChatJoinResp_4001001 chatJoinResp_ = new ChatJoinResp_4001001();
					chatJoinResp_.MergeFrom(byteString);
					if (chatJoinResp_.IsSuccess)
					{
						m_CreditPoints = chatJoinResp_.CreditPoints;
						m_TimeLag = chatJoinResp_.TimeLag / 1000;
						if (ConnectAction != null)
						{
							ConnectAction(ChatRoomType.PaiMaiHuiChat);
						}
					}
				}
			}, isShowWaitUI: true, ServerType.ChatServer);
		}

		public void JoinRoomChatServer()
		{
			JoinChatServer();
			JoinZongMenChatServer(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
		}

		public void ReceiveRoleMsg(object data)
		{
			ByteString byteString = data as ByteString;
			if (byteString == null)
			{
				return;
			}
			ReceiveMessageRoleResp_4001008 receiveMessageRoleResp_ = new ReceiveMessageRoleResp_4001008();
			receiveMessageRoleResp_.MergeFrom(byteString);
			if (receiveMessageRoleResp_.IsSuccess)
			{
				string receiveTime = CommonMethod.GetTimeStampTime((receiveMessageRoleResp_.MsgTime / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm");
				FriendEntity friendEntity = new FriendEntity
				{
					Content = receiveMessageRoleResp_.Msg,
					FromRoleId = receiveMessageRoleResp_.RoleId,
					Timestamp = receiveMessageRoleResp_.MsgTime,
					ReceiveTime = receiveTime
				};
				if (!RememberMessage(friendEntity.FromRoleId, friendEntity))
				{
					return;
				}
				if (!m_ChatRoleMsgDic.TryGetValue(friendEntity.FromRoleId, out List<string> unread))
				{
					unread = new List<string>();
					m_ChatRoleMsgDic.Add(friendEntity.FromRoleId, unread);
				}
				unread.Add(friendEntity.Content);
				if (ReceiveMsgAction != null)
				{
					ReceiveMsgAction(friendEntity);
				}
				// An open conversation may have consumed this message synchronously.
				RefreshFriendUnread();
			}
		}

		public void SendRoleMsg(FriendEntity friendEntity, string targetFriendId)
		{
			RememberMessage(targetFriendId, friendEntity);
		}

		private bool RememberMessage(string friendId, FriendEntity message)
		{
			if (m_ChatHistory == null || m_ChatRoleMsgDic == null)
			{
				return false;
			}
			if (!m_ChatHistory.TryGetValue(friendId, out List<FriendEntity> recent))
			{
				recent = new List<FriendEntity>();
				m_ChatHistory.Add(friendId, recent);
			}
			if (recent.Exists(row => row.Timestamp == message.Timestamp))
			{
				return false;
			}
			if (recent.Count >= 100)
			{
				recent.RemoveAt(0);
			}
			recent.Add(message);
			return true;
		}

		public void SendMsgUpLoad()
		{
		}

		public void EnterRoleChat(string roleId)
		{
			if (!string.IsNullOrEmpty(roleId) && m_ChatRoleMsgDic?.Remove(roleId) == true)
			{
				RefreshFriendUnread();
			}
		}

		private void RefreshFriendUnread()
		{
			bool hasUnread = m_ChatRoleMsgDic != null && m_ChatRoleMsgDic.Count > 0;
			if (HasUnreadFriendMessages == hasUnread) return;
			HasUnreadFriendMessages = hasUnread;
			FriendUnreadChanged?.Invoke();
		}

		public void LiKaiChatRoom(string chatId)
		{
			WineOutReq_4001004 data = new WineOutReq_4001004
			{
				ChatId = chatId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.WINE_OUT, data, (object _obj) =>
			{
			}, isShowWaitUI: false, chatId == Constants.WorldChatID && Constants.UsesPackagedTrialData
				? ServerType.GamesNormalServer : ServerType.ChatServer);
		}

		public void ReceiveChatMsg(object data)
		{
			ByteString byteString = data as ByteString;
			if (byteString == null)
			{
				return;
			}
			ReceiveMessageResp_4001002 receiveMessageResp_ = new ReceiveMessageResp_4001002();
			receiveMessageResp_.MergeFrom(byteString);
			if (!receiveMessageResp_.IsSuccess)
			{
				return;
			}
			ChatModel chatModel = new ChatModel
			{
				ChengHao = receiveMessageResp_.Title,
				Content = receiveMessageResp_.Msg,
				RoleId = receiveMessageResp_.RoleId,
				RoleName = receiveMessageResp_.RoleName,
				Sex = receiveMessageResp_.Sex,
				Level = receiveMessageResp_.Level,
				ChatId = receiveMessageResp_.ChatId,
				Type = receiveMessageResp_.Type,
				TitleFrame = receiveMessageResp_.TitleFrame,
				HeadFrame = receiveMessageResp_.HeadFrame
			};
			if (int.TryParse(receiveMessageResp_.HeadPortraitId, out var result))
			{
				chatModel.HeadId = result;
			}
			if (receiveMessageResp_.ChatId == "world")
			{
				chatModel.Card = TavernCard.FromMessage(chatModel.Content);
				m_ChatQue.Enqueue(chatModel);
				if (m_ChatQue.Count > 100)
				{
					m_ChatQue.Dequeue();
				}
			}
			else if (receiveMessageResp_.ChatId == Singleton<RoleManager>.Instance.Role_Entity.ZongMenId)
			{
				m_ZongMenChatQue.Enqueue(chatModel);
				if (m_ZongMenChatQue.Count > 100)
				{
					m_ZongMenChatQue.Dequeue();
				}
			}
			ReceiveRoomMsgAction?.Invoke(chatModel);
		}

		public void ClearData()
		{
			ChatQue.Clear();
			ZongMenChatQue.Clear();
			m_ChatHistory.Clear();
			m_ChatRoleMsgDic.Clear();
			RefreshFriendUnread();
			ReceiveMsgAction = null;
			ConnectAction = null;
		}
	}
}
