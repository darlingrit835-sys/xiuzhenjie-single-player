using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DaoYouChatWnd : Window
	{
		public DaoYouChatPanel Panel;

		private string m_RoleName;

		private string m_RoleId;

		private int m_Sex = 1;

		private int m_HeadId = 1;

		private GameObject m_SelfChatItem;

		private GameObject m_OtherChatItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DaoYouChatPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiChat);
			AddButtonClickListener(Panel.PinBiBtn, PinBiChat);
			AddButtonClickListener(Panel.SendBtn, SendMsg);
			m_SelfChatItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/SelfChatItem.prefab");
			m_OtherChatItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/OtherChatItem.prefab");
			m_RoleId = paralist[0].ToString();
			m_RoleName = paralist[1].ToString();
			int.TryParse(paralist[2].ToString(), out m_Sex);
			int.TryParse(paralist[3].ToString(), out m_HeadId);
			Singleton<ChatManager>.Instance.EnterRoleChat(m_RoleId);
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveMsgAction = (UnityAction<FriendEntity>)Delegate.Combine(instance.ReceiveMsgAction, new UnityAction<FriendEntity>(Receive));
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Panel.TitleText.text = "与" + m_RoleName + "的传书";
			InitChat();
		}

		private void InitChat()
		{
			foreach (KeyValuePair<string, List<FriendEntity>> item in Singleton<ChatManager>.Instance.ChatHistory)
			{
				if (!item.Key.Equals(m_RoleId))
				{
					continue;
				}
				{
					foreach (FriendEntity item2 in item.Value)
					{
						if (item2.FromRoleId.Equals(Singleton<RoleManager>.Instance.Role_Entity.Id))
						{
							CreateSelfChatItem(item2.Content, item2.ReceiveTime);
						}
						else if (m_RoleId == item2.FromRoleId)
						{
							CreateOtherChatItem(item2.Content, item2.ReceiveTime);
						}
					}
					break;
				}
			}
		}

		private void LiKaiChat()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void PinBiChat()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention($"是否与{m_RoleName}绝交", PinBiFriend);
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

		private void SendMsg()
		{
			string contentText = Panel.WaitSendInput.text;
			if (string.IsNullOrEmpty(contentText))
			{
				return;
			}
			SendMessageRoleReq_4001007 data = new SendMessageRoleReq_4001007
			{
				Msg = contentText,
				RoleId = m_RoleId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SendMessageRole, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SendMessageRoleResp_4001007 sendMessageRoleResp_ = new SendMessageRoleResp_4001007();
					sendMessageRoleResp_.MergeFrom(byteString);
					Panel.WaitSendInput.text = "";
					if (sendMessageRoleResp_.IsSuccess)
					{
						ChatInformReq_1012005 data2 = new ChatInformReq_1012005
						{
							Type = 2
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.CHAT_INFORM, data2, null);
						string text = CommonMethod.GetTimeStampTime((sendMessageRoleResp_.MsgTime / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm");
						FriendEntity friendEntity = new FriendEntity
						{
							Content = contentText,
							FromRoleId = Singleton<RoleManager>.Instance.Role_Entity.Id,
							ReceiveTime = text
						};
						Singleton<ChatManager>.Instance.SendRoleMsg(friendEntity, m_RoleId);
						CreateSelfChatItem(contentText, text);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sendMessageRoleResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true, ServerType.ChatServer);
		}

		private void Receive(FriendEntity friendEntity)
		{
			if (friendEntity.FromRoleId.Equals(m_RoleId))
			{
				CreateOtherChatItem(friendEntity.Content, friendEntity.ReceiveTime);
				Singleton<ChatManager>.Instance.EnterRoleChat(friendEntity.FromRoleId);
			}
		}

		private void CreateSelfChatItem(string content, string time)
		{
			SetListContentParent(Panel.DaoYouContent, m_SelfChatItem).gameObject.GetComponent<SelfChatItem>().SetValue(content, time, Singleton<RoleManager>.Instance.Role_Entity.Sex, Singleton<RoleManager>.Instance.Role_Entity.HeadPortraitId, isOnline: true);
			float num = 0f;
			foreach (Transform item in Panel.DaoYouContent)
			{
				RectTransform component = item.gameObject.GetComponent<RectTransform>();
				num += component.sizeDelta.y + 20f;
			}
			if (num > 526.7f)
			{
				Panel.DaoYouContent.sizeDelta = new Vector2(Panel.DaoYouContent.sizeDelta.x, num);
				Panel.DaoYouContent.anchoredPosition = new Vector2(Panel.DaoYouContent.anchoredPosition.x, num - 526.7f);
			}
		}

		private void CreateOtherChatItem(string content, string time)
		{
			SetListContentParent(Panel.DaoYouContent, m_OtherChatItem).gameObject.GetComponent<OtherChatItem>().SetValue(content, time, m_Sex, m_HeadId, isOnline: true);
			float num = 0f;
			foreach (Transform item in Panel.DaoYouContent)
			{
				RectTransform component = item.gameObject.GetComponent<RectTransform>();
				num += component.sizeDelta.y + 20f;
			}
			if (num > 526.7f)
			{
				Panel.DaoYouContent.sizeDelta = new Vector2(Panel.DaoYouContent.sizeDelta.x, num);
				Panel.DaoYouContent.anchoredPosition = new Vector2(Panel.DaoYouContent.anchoredPosition.x, num - 526.7f);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveMsgAction = (UnityAction<FriendEntity>)Delegate.Remove(instance.ReceiveMsgAction, new UnityAction<FriendEntity>(Receive));
		}
	}
}
