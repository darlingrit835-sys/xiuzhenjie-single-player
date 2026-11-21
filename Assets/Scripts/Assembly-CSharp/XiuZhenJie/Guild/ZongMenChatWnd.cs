using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMenChatWnd : Window
	{
		public ZongMenChatPanel Panel;

		private GameObject m_ChatInObj;

		private ChatItem m_StandbyChatItem;

		private Queue<ChatItem> m_ChatItemQue;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenChatPanel>();
			AddButtonClickListener(Panel.ZongMenXiangQingBtn, ZongMenXiangQingShow);
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiClick);
			AddButtonClickListener(Panel.SendBtn, SendBtnClick);
			AddButtonClickListener(Panel.JieShiDaoYou_CloseBtn, JieShiDaoYouClose);
			AddButtonClickListener(Panel.ZongMenInfo_CloseBtn, ZongMenXiangQingClose);
			AddButtonClickListener(Panel.ZongMenInfo_ChuanSongBtn, BackZongMen);
			m_ChatItemQue = new Queue<ChatItem>();
			m_ChatInObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChatItem.prefab");
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
				ReConnectServer(ChatRoomType.ZongMenChat);
				ChatManager instance = Singleton<ChatManager>.Instance;
				instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
				ChatManager instance2 = Singleton<ChatManager>.Instance;
				instance2.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Combine(instance2.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
			}
		}

		private void ReConnectServer(ChatRoomType chatRoomType)
		{
			if (chatRoomType != ChatRoomType.ZongMenChat)
			{
				return;
			}
			ClearChatContent();
			foreach (ChatModel item in Singleton<ChatManager>.Instance.ZongMenChatQue)
			{
				CreateChatItem(item);
			}
			Panel.MinTitleText.text = $"当前文明值：{Singleton<ChatManager>.Instance.CreditPoints}，发言间隔：{Singleton<ChatManager>.Instance.TimeLag / 1000}息";
		}

		public void ReceiveMsg(ChatModel chatModel)
		{
			if (!(chatModel.ChatId != Singleton<RoleManager>.Instance.Role_Entity.ZongMenId))
			{
				CreateChatItem(chatModel);
			}
		}

		private void CreateChatItem(ChatModel chatModel)
		{
			ChatItem chatItem = null;
			if (m_StandbyChatItem == null)
			{
				GameObject gameObject = SetListContentParent(Panel.ChatScrollContent, m_ChatInObj);
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				chatItem = gameObject.GetComponent<ChatItem>();
			}
			else
			{
				chatItem = m_StandbyChatItem;
				m_StandbyChatItem.gameObject.transform.SetParent(Panel.ChatScrollContent);
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
				m_StandbyChatItem.gameObject.transform.SetParent(Panel.ChatScrollContent);
			}
			Panel.ChatScrollContent.anchoredPosition = new Vector2(Panel.ChatScrollContent.anchoredPosition.x, Panel.ChatScrollContent.anchoredPosition.y + 45f);
		}

		private void ChatItemClick(ChatItemEntity data)
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

		private void JieShiDaoYouClose()
		{
			Panel.JieShiDaoYou_Panel.gameObject.SetActive(value: false);
		}

		private void ZongMenXiangQingClose()
		{
			Panel.ZongMenInfoPanel.gameObject.SetActive(value: false);
		}

		private void ZongMenXiangQingShow()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_INFO, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildInfoResp_1015015 guildInfoResp_ = new GuildInfoResp_1015015();
					guildInfoResp_.MergeFrom(byteString);
					if (guildInfoResp_.IsSuccess)
					{
						Panel.ZongMenInfo_TitleText.text = guildInfoResp_.GuildName;
						Panel.ZongMenInfo_SuoZaiDiText.text = guildInfoResp_.Name;
						Panel.ZongMenInfo_ZongMenLevelText.text = WorldSetting.GetStageName(guildInfoResp_.GuildStage);
						Panel.ZongMenInfo_ZhangLaoRenShuText.text = $"{guildInfoResp_.MemberCount}人";
						Panel.ZongMenInfo_DiZiRenShuText.text = $"{guildInfoResp_.AiMemberCount}人";
						Panel.ZongMenInfo_YingXiangLiText.text = guildInfoResp_.YingXiangLi.ToString();
						Panel.ZongMenInfo_ZhanDouLiDiText.text = guildInfoResp_.ZhanDouLi.ToString();
						Panel.ZongMenInfoPanel.gameObject.SetActive(value: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildInfoResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void SendBtnClick()
		{
			if (string.IsNullOrEmpty(Panel.SendInputField.text))
			{
				return;
			}
			SendMessageReq_4001003 data = new SendMessageReq_4001003
			{
				Msg = Panel.SendInputField.text,
				ChatId = Singleton<RoleManager>.Instance.Role_Entity.ZongMenId
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
						Panel.MinTitleText.text = $"当前文明值：{sendMessageResp_.CreditPoints}，发言间隔：{sendMessageResp_.TimeLag / 1000}息";
					}
					Panel.SendInputField.text = string.Empty;
				}
			}, isShowWaitUI: false, ServerType.ChatServer);
		}

		private void BackZongMen()
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "ZongMenScene")
			{
				ZongMenXiangQingClose();
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("当前已在宗门!");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_DENTER, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildEnterResp_1015005 guildEnterResp_ = new GuildEnterResp_1015005();
					guildEnterResp_.MergeFrom(byteString);
					if (guildEnterResp_.IsSuccess)
					{
						Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "ZongMenScene", Singleton<RoleManager>.Instance.Role_Entity.ZongMenId, Singleton<RoleManager>.Instance.Role_Entity.ZongMenName, guildEnterResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildEnterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ClearChatContent()
		{
			foreach (Transform item in Panel.ChatScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in Panel.ChatScrollStandByContent)
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(ReceiveMsg));
		}
	}
}
