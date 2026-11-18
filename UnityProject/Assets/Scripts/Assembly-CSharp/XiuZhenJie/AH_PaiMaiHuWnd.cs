using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AH_PaiMaiHuWnd : Window
	{
		private AH_PaiMaiHuPanel m_Panel;

		private ChatItem m_StandbyChatItem;

		private string m_TreasureId;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private int m_Stage;

		private PaiMaiPing m_PaiMaiPing;

		private bool m_TimeLimite;

		private bool m_TOldMan;

		private int m_CanAuction;

		private long m_CurrentBid;

		private Queue<DanMuItem> m_CanUseDanMuQue;

		private AH_PaiMaiHuPanel m_AhPaiMaiHuPanel;

		private GameObject m_ChatDanMuItem;

		private bool m_OneCi;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<AH_PaiMaiHuPanel>();
			m_ChatDanMuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DanMuItem.prefab");
			m_CanUseDanMuQue = new Queue<DanMuItem>();
			m_Stage = (int)paralist[0];
			m_PaiMaiPing = new PaiMaiPing();
			AddButtonClickListener(m_Panel.PaiMaiListBtn, PaiMaiListBtn);
			AddButtonClickListener(m_Panel.ExitBtn, ExitBtn);
			AddButtonClickListener(m_Panel.SendBtn, SendCheck);
			AddButtonClickListener(m_Panel.JiaJiaBtn, ChuJiaBut);
			ResourceLoad();
			EnterTreasure();
		}

		private void EnterTreasure()
		{
			EnterTreasureReq_1006017 data = new EnterTreasureReq_1006017
			{
				Stage = m_Stage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_TREASURE_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterTreasureResp_1006017 enterTreasureResp_ = new EnterTreasureResp_1006017();
					enterTreasureResp_.MergeFrom(byteString);
					if (enterTreasureResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = enterTreasureResp_.ShinyStone;
						m_TreasureId = enterTreasureResp_.TreasureId.ToString();
						m_PaiMaiPing.DetermineWhetherItCanBeAuctioned = enterTreasureResp_.IsMayAddPrice;
						m_OneCi = false;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						SelectGoodsRequest();
						ChatDetailsRoom();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterTreasureResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SelectGoodsRequest()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.TREASURE_CURRENT_GOODS_RESP, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					TreasureCurrentGoodsResp_1006021 treasureCurrentGoodsResp_ = new TreasureCurrentGoodsResp_1006021();
					treasureCurrentGoodsResp_.MergeFrom(byteString);
					if (treasureCurrentGoodsResp_.IsSuccess)
					{
						m_PaiMaiPing.CurrentPrice = treasureCurrentGoodsResp_.CurrentPrice;
						m_PaiMaiPing.PersonName = treasureCurrentGoodsResp_.PersonName;
						m_PaiMaiPing.GoodsType = treasureCurrentGoodsResp_.GoodsType;
						m_PaiMaiPing.Level = treasureCurrentGoodsResp_.Level;
						m_PaiMaiPing.Rank = treasureCurrentGoodsResp_.Rank;
						m_PaiMaiPing.Stage = treasureCurrentGoodsResp_.Stage;
						m_PaiMaiPing.FiveElement = treasureCurrentGoodsResp_.FiveElement;
						m_PaiMaiPing.GoodsName = treasureCurrentGoodsResp_.GoodsName;
						m_PaiMaiPing.CurrentPerson = treasureCurrentGoodsResp_.CurrentPerson;
						m_PaiMaiPing.PersonImage = treasureCurrentGoodsResp_.PersonImage;
						m_PaiMaiPing.PersonSex = treasureCurrentGoodsResp_.PersonSex;
						m_PaiMaiPing.GoodsId = treasureCurrentGoodsResp_.GoodsId;
						m_PaiMaiPing.Id = treasureCurrentGoodsResp_.Id;
						m_PaiMaiPing.StartPrice = treasureCurrentGoodsResp_.StartPrice;
						if (m_OneCi)
						{
							m_PaiMaiPing.DetermineWhetherItCanBeAuctioned = false;
						}
						else
						{
							m_OneCi = true;
						}
						Current();
						Auctioneer();
						NewXiangQing();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(treasureCurrentGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void Current()
		{
			m_GoodsIconSprite.TryGetValue(m_PaiMaiPing.GoodsType, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(m_PaiMaiPing.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			m_Panel.GoodsItem.SetItem(m_PaiMaiPing, m_PaiMaiPing.GoodsName, m_PaiMaiPing.FiveElement, m_PaiMaiPing.Level, value2, value, (object obj, GameObject gameObj) =>
			{
				PaiPingXiangQing();
			});
		}

		private void PaiPingXiangQing()
		{
			Singleton<UIManager>.Instance.PopUpWnd("AH_PaiPingXiangQingWnd.prefab", true, m_PaiMaiPing);
		}

		private void Auctioneer()
		{
			Sprite sprite = null;
			sprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(m_PaiMaiPing.PersonSex ? 1 : 0, m_PaiMaiPing.PersonImage.ToString());
			if (m_PaiMaiPing.PersonName != "")
			{
				m_Panel.ToXiangImg.gameObject.SetActive(value: true);
				m_Panel.ToXiangImg.gameObject.GetComponent<Image>().sprite = sprite;
				m_Panel.Name.text = m_PaiMaiPing.PersonName;
			}
			else
			{
				m_Panel.ToXiangImg.gameObject.SetActive(value: false);
			}
			m_Panel.JiaGeText.text = $"{m_PaiMaiPing.CurrentPrice}灵石";
		}

		private void ExitBtn()
		{
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(m_ReceiveMsg));
			Singleton<ChatManager>.Instance.LiKaiChatRoom(m_TreasureId);
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void PaiMaiListBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("AH_PaiPinListWnd.prefab", true, m_Stage);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void SendCheck()
		{
			if (string.IsNullOrEmpty("消息不能为空！"))
			{
				return;
			}
			SendMessageReq_4001003 data = new SendMessageReq_4001003
			{
				ChatId = m_TreasureId,
				Msg = m_Panel.ChatInput.text
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEND_MESSAGE, data, (object obj) =>
			{
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SendMessageResp_4001003 sendMessageResp_ = new SendMessageResp_4001003();
					sendMessageResp_.MergeFrom(byteString);
					if (!sendMessageResp_.IsSuccess)
					{
						Debug.Log(sendMessageResp_.Code);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sendMessageResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					else
					{
						m_Panel.ChatInput.text = string.Empty;
					}
				}
			}, isShowWaitUI: false, ServerType.ChatServer);
		}

		private void ChatDetailsRoom()
		{
			Singleton<ChatManager>.Instance.JoinPMHChatServer(m_TreasureId);
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Remove(instance.ReceiveRoomMsgAction, new UnityAction<ChatModel>(m_ReceiveMsg));
			ChatManager instance2 = Singleton<ChatManager>.Instance;
			instance2.ReceiveRoomMsgAction = (UnityAction<ChatModel>)Delegate.Combine(instance2.ReceiveRoomMsgAction, new UnityAction<ChatModel>(m_ReceiveMsg));
		}

		private void m_ReceiveMsg(ChatModel chatModel)
		{
			if (!(chatModel.ChatId != m_TreasureId))
			{
				switch (chatModel.Type)
				{
				case 0:
					CreateChatDanMuItem(chatModel);
					break;
				case 1:
					AuctionRefreshItem(chatModel);
					break;
				case 2:
					SelectGoodsRequest();
					break;
				case 3:
					OldMan(chatModel);
					break;
				case 4:
					Zhanting();
					break;
				}
			}
		}

		private void Zhanting()
		{
			m_PaiMaiPing.DetermineWhetherItCanBeAuctioned = true;
			m_Panel.JiaJiaBtn.gameObject.SetActive(value: true);
			NewXiangQing();
		}

		private void NewXiangQing()
		{
			Singleton<UIManager>.Instance.FindWndByName<AH_PaiPingXiangQingWnd>("AH_PaiPingXiangQingWnd.prefab")?.NewShuJu(m_PaiMaiPing);
		}

		private void OldMan(ChatModel chatModel)
		{
			if (!m_TOldMan)
			{
				m_TOldMan = true;
				m_Panel.YuLuText.gameObject.SetActive(value: true);
			}
			m_Panel.YuLuText.text = chatModel.Content;
		}

		private void AuctionRefreshItem(ChatModel chatModel)
		{
			m_PaiMaiPing.CurrentPrice = long.Parse(chatModel.Content);
			m_PaiMaiPing.PersonName = chatModel.RoleName;
			m_PaiMaiPing.PersonImage = chatModel.HeadId;
			switch (chatModel.Sex)
			{
			case 0:
				m_PaiMaiPing.PersonSex = false;
				break;
			case 1:
				m_PaiMaiPing.PersonSex = true;
				break;
			}
			Current();
			Auctioneer();
			NewXiangQing();
		}

		private void CreateChatDanMuItem(ChatModel chatModel)
		{
			RectTransform rectTransform = ChooseCanUseDanMuContentNew();
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
		}

		private void DanMuMoveCompelete(DanMuItem danMuItem)
		{
			danMuItem.transform.SetParent(m_Panel.DanMuStandByContent);
			m_CanUseDanMuQue.Enqueue(danMuItem);
		}

		private RectTransform ChooseCanUseDanMuContentNew()
		{
			RectTransform danMuContent_ = m_Panel.DanMuContent_1;
			Dictionary<float, RectTransform> dictionary = new Dictionary<float, RectTransform>();
			List<float> list = new List<float>();
			if (m_Panel.DanMuContent_1.childCount == 0)
			{
				return m_Panel.DanMuContent_1;
			}
			float endDistance = m_Panel.DanMuContent_1.GetChild(m_Panel.DanMuContent_1.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance))
			{
				dictionary.Add(endDistance, m_Panel.DanMuContent_1);
				list.Add(endDistance);
			}
			if (m_Panel.DanMuContent_2.childCount == 0)
			{
				return m_Panel.DanMuContent_2;
			}
			float endDistance2 = m_Panel.DanMuContent_2.GetChild(m_Panel.DanMuContent_2.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance2))
			{
				dictionary.Add(endDistance2, m_Panel.DanMuContent_2);
				list.Add(endDistance2);
			}
			if (m_Panel.DanMuContent_3.childCount == 0)
			{
				return m_Panel.DanMuContent_3;
			}
			float endDistance3 = m_Panel.DanMuContent_3.GetChild(m_Panel.DanMuContent_3.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance3))
			{
				dictionary.Add(endDistance3, m_Panel.DanMuContent_3);
				list.Add(endDistance3);
			}
			if (m_Panel.DanMuContent_4.childCount == 0)
			{
				return m_Panel.DanMuContent_4;
			}
			float endDistance4 = m_Panel.DanMuContent_4.GetChild(m_Panel.DanMuContent_4.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance4))
			{
				dictionary.Add(endDistance4, m_Panel.DanMuContent_4);
				list.Add(endDistance4);
			}
			if (m_Panel.DanMuContent_5.childCount == 0)
			{
				return m_Panel.DanMuContent_5;
			}
			float endDistance5 = m_Panel.DanMuContent_5.GetChild(m_Panel.DanMuContent_5.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
			if (!dictionary.ContainsKey(endDistance5))
			{
				dictionary.Add(endDistance5, m_Panel.DanMuContent_5);
				list.Add(endDistance5);
			}
			list.Sort((float left, float right) =>
			{
				if (left > right)
				{
					return 1;
				}
				return (!((double)Math.Abs(left - right) < 1E-05)) ? (-1) : 0;
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
			m_Panel.JieShiPanel.SetValueAndShow(data.RoleName, data.Level, touXiangSprite, SendMingCiClick, data.HeadFrameId);
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
			Singleton<NetManager>.Instance.SendMsg(MsgType.ADDFRIEND_BYNAME, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					AddFriendByNameResp_1014007 addFriendByNameResp_ = new AddFriendByNameResp_1014007();
					addFriendByNameResp_.MergeFrom(byteString);
					if (addFriendByNameResp_.IsSuccess)
					{
						string msg = "发送名刺成功!";
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(msg);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addFriendByNameResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					m_Panel.JieShiPanel.PanelHide();
				}
			}, isShowWaitUI: true);
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_CanAuction >= 0)
			{
				m_CanAuction -= (int)Time.fixedDeltaTime;
			}
		}

		private void ChuJiaBut()
		{
			m_CurrentBid = (long)((double)m_PaiMaiPing.CurrentPrice + (double)m_PaiMaiPing.StartPrice * 0.1);
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(m_CurrentBid) < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前灵石不足");
				return;
			}
			if (m_CanAuction < 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("快速加价CD3秒！");
				return;
			}
			if (!m_PaiMaiPing.DetermineWhetherItCanBeAuctioned)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("加价还未开始！");
				return;
			}
			TreasureBuyReq_1006020 data = new TreasureBuyReq_1006020
			{
				TreasureId = m_PaiMaiPing.Id,
				Price = m_CurrentBid
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TREASURE_BUY_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				TreasureBuyResp_1006020 treasureBuyResp_ = new TreasureBuyResp_1006020();
				treasureBuyResp_.MergeFrom(byteString);
				if (!(byteString == null))
				{
					if (treasureBuyResp_.IsSuccess)
					{
						string textValue = "加价成功!";
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
						m_CanAuction = 3;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = treasureBuyResp_.ShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(treasureBuyResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}
	}
}
