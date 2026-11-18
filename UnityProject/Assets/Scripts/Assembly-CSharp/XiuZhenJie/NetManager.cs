using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using LitJson;
using UnityEngine;

namespace XiuZhenJie
{
	public class NetManager : Singleton<NetManager>
	{
		private static readonly string _obj = "lock";

		private static readonly string _obj_1 = "lock_1";

		private static readonly string _obj_2 = "lock_2";

		private static readonly string _obj_3 = "lock_3";

		private static readonly string _obj_4 = "lock_4";

		private static readonly string _obj_5 = "lock_5";

		private string m_Ip;

		private int m_Port;

		private string m_BattleIp;

		private int m_BattlePort;

		private string m_ChatIp;

		private int m_ChatPort;

		private int m_MsgId;

		private Queue<NetPackget> NetErrorQue = new Queue<NetPackget>();

		private Queue<NetPackget> NetMsgQue = new Queue<NetPackget>();

		private Queue<NetPackget> ChatServerMsgQue = new Queue<NetPackget>();

		private Queue<NetPackget> BattleServerMsgQue = new Queue<NetPackget>();

		private Queue<NetPackget> NetConnectSuccessQue = new Queue<NetPackget>();

		private Queue<NetPackget> NetDisConnectQue = new Queue<NetPackget>();

		public Action<ServerType, bool> ConnectSuccessEvent;

		public Action<ServerType, NetPackget> ReceiveNetMessageEvent;

		public Action<ServerType> DisConnectEvent;

		public Action<ServerType> NetErrorEvent;

		public Dictionary<ServerType, NetSvc> ConnectedSocket;

		private int m_ReceiveByteLength;

		private int m_ReceiveByteHeadLength;

		private byte[] m_ReceiveByte;

		private int m_ReceiveBattleByteLength;

		private int m_ReceiveBattleByteHeadLength;

		private byte[] m_ReceiveBattleByte;

		private int m_ReceiveChatByteLength;

		private int m_ReceiveChatByteHeadLength;

		private byte[] m_ReceiveChatByte;

		public Action<string> GameRadioAction;

		public Action GameExceptionAction;

		public Action<string> NewMailReceiveAction;

		private List<byte[]> BufferList = new List<byte[]>();

		private Dictionary<int, Action<object>> WaitCallBackDic = new Dictionary<int, Action<object>>();

		public MonoBehaviour Mono;

		public bool IsReConnect;

		public static object locker = new object();

		private AH_PaiMaiHuWnd m_AHPaiMaiHuWnd;

		public void Init(MonoBehaviour _mono)
		{
			ConnectedSocket = new Dictionary<ServerType, NetSvc>();
			NetSvc netSvc = new NetSvc(ServerType.GamesNormalServer);
			if (!ConnectedSocket.ContainsKey(ServerType.GamesNormalServer))
			{
				ConnectedSocket.Add(ServerType.GamesNormalServer, netSvc);
			}
			else
			{
				ConnectedSocket[ServerType.GamesNormalServer] = netSvc;
			}
			m_ReceiveByteLength = 0;
			NormalSocketInit(netSvc);
			IsReConnect = false;
			Mono = _mono;
			m_AHPaiMaiHuWnd = Singleton<UIManager>.Instance.FindWndByName<AH_PaiMaiHuWnd>("AH_PaiMaiHuWnd.prefab");
		}

		public void FixUpdate()
		{
			if (ConnectedSocket == null || ConnectedSocket.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<ServerType, NetSvc> item in ConnectedSocket)
			{
				item.Value?.FixUpdate();
			}
		}

		private void NormalSocketInit(NetSvc netSvc)
		{
			netSvc.ConnectSuccessEvent = (Action<ServerType>)Delegate.Combine(netSvc.ConnectSuccessEvent, new Action<ServerType>(ConnectSuccess));
			netSvc.DisConnectEvent = (Action<ServerType>)Delegate.Combine(netSvc.DisConnectEvent, new Action<ServerType>(DisConnect));
			netSvc.ReceiveMessageEvent = (Action<ServerType, byte[]>)Delegate.Combine(netSvc.ReceiveMessageEvent, new Action<ServerType, byte[]>(ReceiveMessage));
			netSvc.NetErrorEvent = (Action<ServerType, string>)Delegate.Combine(netSvc.NetErrorEvent, new Action<ServerType, string>(NetError));
			netSvc.Init();
		}

		public void StartFirstConnect(string ip, int port)
		{
			m_Ip = ip;
			m_Port = port;
			Mono.StartCoroutine(IEUpdate());
			NetSvc value = null;
			if (ConnectedSocket.TryGetValue(ServerType.GamesNormalServer, out value))
			{
				value.StartLongConnect(ip, port);
				return;
			}
			value = new NetSvc(ServerType.GamesNormalServer);
			NormalSocketInit(value);
			ConnectedSocket.Add(ServerType.GamesNormalServer, value);
			value.StartLongConnect(ip, port);
		}

		public void StartBattleServer(string ip, int port)
		{
			lock (locker)
			{
				if (!ConnectedSocket.ContainsKey(ServerType.BattleServer))
				{
					m_BattleIp = ip;
					m_BattlePort = port;
					NetSvc netSvc = new NetSvc(ServerType.BattleServer);
					ConnectedSocket.Add(ServerType.BattleServer, netSvc);
					NormalSocketInit(netSvc);
					netSvc.StartLongConnect(ip, port);
				}
			}
		}

		public void StartChatServer(string ip, int port)
		{
			lock (locker)
			{
				if (!ConnectedSocket.ContainsKey(ServerType.ChatServer))
				{
					m_ChatIp = ip;
					m_ChatPort = port;
					NetSvc netSvc = new NetSvc(ServerType.ChatServer);
					ConnectedSocket.Add(ServerType.ChatServer, netSvc);
					NormalSocketInit(netSvc);
					netSvc.StartLongConnect(ip, port);
				}
			}
		}

		public bool GameServerIsAlive()
		{
			if (ConnectedSocket.ContainsKey(ServerType.GamesNormalServer))
			{
				return true;
			}
			return false;
		}

		public bool ChatServerIsAlive()
		{
			if (ConnectedSocket.ContainsKey(ServerType.ChatServer))
			{
				return true;
			}
			return false;
		}

		public bool BattleServerIsAlive()
		{
			if (ConnectedSocket.ContainsKey(ServerType.BattleServer))
			{
				return true;
			}
			return false;
		}

		public void SendMsg(MsgType msgType, IMessage data, Action<object> SendCallBack, bool isShowWaitUI = false, ServerType serverType = ServerType.GamesNormalServer)
		{
			m_MsgId++;
			NetSvc value = null;
			if (ConnectedSocket.TryGetValue(serverType, out value) && value.isConnect)
			{
				if (isShowWaitUI)
				{
					MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
				}
				ByteString data2 = ((data != null) ? data.ToByteString() : ByteString.Empty);
				Msg msg = new Msg
				{
					Cmd = (int)msgType,
					Msid = m_MsgId,
					Data = data2
				};
				if (SendCallBack != null)
				{
					WaitCallBackDic.Add(m_MsgId, SendCallBack);
				}
				value.AddSendMsgQue(msg);
				SendOverTime(msg.Cmd, m_MsgId, value);
			}
		}

		private void SendOverTime(int cmd, int guid, NetSvc netSvc)
		{
			Timer.Register(60f, () =>
			{
				if (netSvc.isConnect && WaitCallBackDic.ContainsKey(guid))
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					MonoSingleton<GameStart>.Instance.PopUpAttention("Code:" + cmd + "  连接超时");
					WaitCallBackDic.Remove(guid);
				}
			});
		}

		public void SendStrandedMsg()
		{
			foreach (KeyValuePair<ServerType, NetSvc> item in ConnectedSocket)
			{
				item.Value.SendStrandedMsg();
			}
		}

		public void NetClose(ServerType serverType = ServerType.GamesNormalServer, bool isRemove = true)
		{
			NetSvc value = null;
			if (ConnectedSocket == null || !ConnectedSocket.TryGetValue(serverType, out value))
			{
				return;
			}
			try
			{
				NetSvc netSvc = value;
				netSvc.ConnectSuccessEvent = (Action<ServerType>)Delegate.Remove(netSvc.ConnectSuccessEvent, new Action<ServerType>(ConnectSuccess));
				NetSvc netSvc2 = value;
				netSvc2.DisConnectEvent = (Action<ServerType>)Delegate.Remove(netSvc2.DisConnectEvent, new Action<ServerType>(DisConnect));
				NetSvc netSvc3 = value;
				netSvc3.ReceiveMessageEvent = (Action<ServerType, byte[]>)Delegate.Remove(netSvc3.ReceiveMessageEvent, new Action<ServerType, byte[]>(ReceiveMessage));
				Delegate[] array = ConnectSuccessEvent?.GetInvocationList();
				if (array != null)
				{
					Delegate[] array2 = array;
					foreach (Delegate obj in array2)
					{
						obj.GetType().GetProperty("Method").GetValue(obj, null);
						ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(ConnectSuccessEvent, obj as Action<ServerType, bool>);
					}
				}
				if (isRemove)
				{
					ConnectedSocket.Remove(serverType);
				}
				value.NetSvcClose();
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}

		public void ReConnect(ServerType serverType = ServerType.GamesNormalServer)
		{
			NetSvc value = null;
			if (ConnectedSocket.TryGetValue(serverType, out value))
			{
				NormalSocketInit(value);
				IsReConnect = false;
				switch (serverType)
				{
				case ServerType.GamesNormalServer:
					value.StartLongConnect(m_Ip, m_Port);
					break;
				case ServerType.BattleServer:
					StartBattleServer(m_BattleIp, m_BattlePort);
					break;
				case ServerType.ChatServer:
					StartChatServer(m_ChatIp, m_ChatPort);
					break;
				}
				return;
			}
			switch (serverType)
			{
			case ServerType.GamesNormalServer:
				value = new NetSvc(ServerType.GamesNormalServer);
				lock (locker)
				{
					if (!ConnectedSocket.ContainsKey(ServerType.GamesNormalServer))
					{
						ConnectedSocket.Add(ServerType.GamesNormalServer, value);
					}
				}
				NormalSocketInit(value);
				IsReConnect = false;
				value.StartLongConnect(m_Ip, m_Port);
				break;
			case ServerType.BattleServer:
				StartBattleServer(m_BattleIp, m_BattlePort);
				break;
			case ServerType.ChatServer:
				StartChatServer(m_ChatIp, m_ChatPort);
				break;
			}
		}

		private void ConnectSuccess(ServerType serverType)
		{
			NetPackget data = new NetPackget
			{
				MessageType = ReceiveMessageActionType.None,
				CallBackType = NetCallBackType.ConnectSuccess,
				FromServer = serverType,
				Data = null
			};
			AddToMsgQue(data);
		}

		private void ReceiveMessage(ServerType serverType, byte[] _data)
		{
			ReceiveMessageActionType messageType = ReceiveMessageActionType.None;
			NetPackget data = new NetPackget
			{
				MessageType = messageType,
				CallBackType = NetCallBackType.ReceiveMessage,
				FromServer = serverType,
				Data = _data
			};
			AddToMsgQue(data);
		}

		private string DeShellData(string jsonData)
		{
			BizInfo bizInfo = null;
			try
			{
				bizInfo = JsonMapper.ToObject<BizInfo>(jsonData);
			}
			catch (Exception ex)
			{
				Debug.Log("解析外壳数据" + ex.Message);
				return "error";
			}
			if (bizInfo == null)
			{
				return "error";
			}
			int num = int.Parse(bizInfo.code);
			if (num >= 2000 && num < 3000)
			{
				if (string.IsNullOrEmpty(bizInfo.dataString) || bizInfo.dataString == "[]" || bizInfo.dataString == "null" || bizInfo.dataString == "NULL")
				{
					return null;
				}
				return bizInfo.dataString;
			}
			return "error";
		}

		public Msg MsgSplit<T>(byte[] data, ref byte[] receiveByte, ref int receiveByteLength, ref int receiveByteHeadLength) where T : class, IMessage, new()
		{
			try
			{
				if (data != null)
				{
					if (receiveByte == null || receiveByte.Length == 0)
					{
						receiveByte = data;
					}
					else
					{
						byte[] array = new byte[receiveByte.Length + data.Length];
						receiveByte.CopyTo(array, 0);
						data.CopyTo(array, receiveByte.Length);
						receiveByte = array;
					}
				}
				Msg msg = new Msg();
				try
				{
					if (receiveByteLength == 0)
					{
						CodedInputStream codedInputStream = new CodedInputStream(receiveByte);
						receiveByteLength = codedInputStream.ReadLength();
						receiveByteHeadLength = CodedOutputStream.ComputeLengthSize(receiveByteLength);
					}
					if (receiveByteLength + receiveByteHeadLength > receiveByte.Length)
					{
						return null;
					}
					byte[] array2 = new byte[receiveByteLength];
					Array.Copy(receiveByte, receiveByteHeadLength, array2, 0, receiveByteLength);
					ByteString data2 = ByteString.CopyFrom(array2);
					msg.MergeFrom(data2);
					BufferList.Clear();
					int num = receiveByte.Length - (receiveByteLength + receiveByteHeadLength);
					if (num > 0)
					{
						byte[] array3 = new byte[num];
						Array.Copy(receiveByte, receiveByteLength + receiveByteHeadLength, array3, 0, num);
						receiveByte = array3;
					}
					else
					{
						receiveByte = null;
					}
					receiveByteLength = 0;
					receiveByteHeadLength = 0;
				}
				catch (Exception ex)
				{
					Debug.Log("netManager:接受数据字节数组出错" + ex);
					return null;
				}
				return msg;
			}
			catch (Exception ex2)
			{
				Debug.Log("消息解碼出現問題:" + ex2.Message);
				return null;
			}
		}

		private void DisConnect(ServerType serverType)
		{
			NetPackget data = new NetPackget
			{
				MessageType = ReceiveMessageActionType.None,
				CallBackType = NetCallBackType.DisConnect,
				FromServer = serverType,
				Data = "与服务器断开连接..."
			};
			AddToMsgQue(data);
			NetClose(serverType);
			IsReConnect = true;
		}

		private void NetError(ServerType serverType, string errorMsg)
		{
			NetPackget data = new NetPackget
			{
				MessageType = ReceiveMessageActionType.None,
				CallBackType = NetCallBackType.Error,
				Data = "连接服务器出错...",
				FromServer = serverType
			};
			AddToMsgQue(data);
		}

		private void AddToMsgQue(NetPackget _data)
		{
			lock (_obj)
			{
				switch (_data.CallBackType)
				{
				case NetCallBackType.ConnectSuccess:
					lock (_obj)
					{
						NetConnectSuccessQue.Enqueue(_data);
						break;
					}
				case NetCallBackType.DisConnect:
					lock (_obj_1)
					{
						NetDisConnectQue.Enqueue(_data);
						break;
					}
				case NetCallBackType.ReceiveMessage:
					if (_data.FromServer == ServerType.GamesNormalServer)
					{
						lock (_obj_2)
						{
							NetMsgQue.Enqueue(_data);
						}
					}
					if (_data.FromServer == ServerType.BattleServer)
					{
						lock (_obj_3)
						{
							BattleServerMsgQue.Enqueue(_data);
						}
					}
					if (_data.FromServer == ServerType.ChatServer)
					{
						lock (_obj_4)
						{
							ChatServerMsgQue.Enqueue(_data);
							break;
						}
					}
					break;
				case NetCallBackType.Error:
					lock (_obj_5)
					{
						NetErrorQue.Enqueue(_data);
						break;
					}
				}
			}
		}

		private void dealWithResMsg(Msg _data)
		{
			if (_data == null || _data.Data == null)
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				return;
			}
			switch ((MsgType)_data.Cmd)
			{
			case MsgType.NOTICE_VIGOR_UP:
				Singleton<RoleManager>.Instance.JingLiAdd(_data.Data);
				break;
			case MsgType.NOTICE_CLEVEL_UP:
				Singleton<RoleManager>.Instance.WuXingAdd(_data.Data);
				break;
			case MsgType.NOTICE_POWER_UP:
				Singleton<RoleManager>.Instance.TiLiAdd(_data.Data);
				break;
			case MsgType.NOTICE_POSION_DOWN:
				Singleton<RoleManager>.Instance.DanDuRemoveDown(_data.Data);
				break;
			case MsgType.NOTICE_SUPERAIR_UP:
				Singleton<RoleManager>.Instance.EveryDayAddSuperAir(_data.Data);
				break;
			case MsgType.NOTICE_PHYSIQUE_UP:
				Singleton<RoleManager>.Instance.EveryDayAddPhysique(_data.Data);
				break;
			case MsgType.HAOYUNFU_COMPELETE:
				Singleton<RoleManager>.Instance.HaoYunFuCompelete();
				break;
			case MsgType.NOTICE_PERCEPTION_UP:
				Singleton<RoleManager>.Instance.EveryDayAddPercent(_data.Data);
				break;
			case MsgType.NOTICE_AGE_UP:
				Singleton<RoleManager>.Instance.EveryDayAddAge(_data.Data);
				break;
			case MsgType.MONSTER_DEAD:
				Singleton<BattleManager>.Instance.NoticeDiaoLuo(_data.Data);
				break;
			case MsgType.LETTER_NOTICE:
				DealWithMail();
				break;
			case MsgType.RADIO_NOTICE:
				DealWithRadionMsg(_data);
				break;
			case MsgType.KILL_PLAYER:
				DealWithGameException(_data);
				break;
			case MsgType.RECEIVE_MESSAGE:
				Singleton<ChatManager>.Instance.ReceiveChatMsg(_data.Data);
				break;
			case MsgType.PLAYER_DEAD:
				Singleton<UIManager>.Instance.PopUpWnd("ChongShengWnd.prefab", true, "true");
				break;
			case MsgType.ReceiveMessageRole:
				Singleton<ChatManager>.Instance.ReceiveRoleMsg(_data.Data);
				break;
			case MsgType.GUILD_INFORM_ROLE_JOINSUCCESS:
				DealwithJoinZongMen(_data.Data);
				break;
			case MsgType.GUILD_EXPEL_ROLEINFORM:
				DealExitZongMen(_data.Data);
				break;
			case MsgType.INFORM_TASK_COMPLETION:
				DealwithTaskCompelete();
				break;
			case MsgType.INFORM_TIANSHUBEI:
				DealwithTSBCompelete();
				break;
			case MsgType.EXIT_DUNGEON:
				ExitDungeon();
				break;
			case MsgType.InformTitle:
				GetNewRoleTitle(_data.Data);
				break;
			case MsgType.TONG_ZHI_TMCG:
				TongZhiTMCG(_data.Data);
				break;
			case MsgType.GuildMonsterFightBeginBroadcast:
				GuildMonsterFightBegin(_data.Data);
				break;
			case MsgType.GuildMonsterFightingBroadcast:
				GuildMonsterFighting(_data.Data);
				break;
			case MsgType.GuildMonsterFightMessageBroadcast:
				GuildMonsterFightingRadio(_data.Data);
				break;
			case MsgType.GuildMonsterFinished:
				Debug.Log("收到了兽潮结束战");
				GuildMonsterFightFinish(_data.Data);
				break;
			case MsgType.GuildJieSanRoleInform:
				ZongMenJieSan();
				break;
			default:
				DealWithActionCallBack(_data);
				break;
			case MsgType.HEART_BEAT:
				break;
			}
		}

		private void ZongMenJieSan()
		{
			Singleton<ChatManager>.Instance.LiKaiChatRoom(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
			string zongMenName = Singleton<RoleManager>.Instance.Role_Entity.ZongMenName;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenId = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenName = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenSimpleName = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenIconIndex = 0;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenLastNameImgIndex = 0;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "ZongMenScene")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("您已经脱离了" + zongMenName, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				}, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				});
				return;
			}
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("您已经脱离了" + zongMenName);
			ZongMenChatWnd zongMenChatWnd = Singleton<UIManager>.Instance.FindWndByName<ZongMenChatWnd>("ZongMenChatWnd.prefab");
			if (zongMenChatWnd != null)
			{
				Singleton<UIManager>.Instance.CloseWnd(zongMenChatWnd);
			}
		}

		private void GuildMonsterFightBegin(ByteString byteStr)
		{
			GuildMonsterFightBeginBroadcastResp_1018001 guildMonsterFightBeginBroadcastResp_ = new GuildMonsterFightBeginBroadcastResp_1018001();
			guildMonsterFightBeginBroadcastResp_.MergeFrom(byteStr);
			if (guildMonsterFightBeginBroadcastResp_.GuildId == int.Parse(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId) && GameRadioAction != null)
			{
				GameRadioAction("宗门兽潮战已经开启！");
			}
		}

		private void GuildMonsterFighting(ByteString byteStr)
		{
			GuildMonsterFightingBroadcastResp_1018002 guildMonsterFightingBroadcastResp_ = new GuildMonsterFightingBroadcastResp_1018002();
			guildMonsterFightingBroadcastResp_.MergeFrom(byteStr);
			if (guildMonsterFightingBroadcastResp_.GuildId == int.Parse(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId))
			{
				Singleton<UIManager>.Instance.FindWndByName<ZongMenShouChaoWnd>("ZongMenShouChaoWnd.prefab")?.GetData(byteStr);
			}
		}

		private void GuildMonsterFightingRadio(ByteString byteStr)
		{
			new GuildMonsterFightMessageBroadcastResp_1018005().MergeFrom(byteStr);
			Singleton<UIManager>.Instance.FindWndByName<ZongMenShouChaoWnd>("ZongMenShouChaoWnd.prefab")?.ReceiveBattleRadio(byteStr);
		}

		private void GuildMonsterFightFinish(ByteString byteStr)
		{
			new GuildMonsterFinishedResp_1018004().MergeFrom(byteStr);
			ZongMenShouChaoWnd zongMenShouChaoWnd = Singleton<UIManager>.Instance.FindWndByName<ZongMenShouChaoWnd>("ZongMenShouChaoWnd.prefab");
			if (zongMenShouChaoWnd != null)
			{
				Debug.Log("收到了兽潮结束战222");
				zongMenShouChaoWnd.ShouChaoFinish(byteStr);
			}
		}

		private void GetNewRoleTitle(ByteString byteStr)
		{
			if (Singleton<RoleManager>.Instance != null && Singleton<RoleManager>.Instance.Role_Entity != null)
			{
				Singleton<RoleManager>.Instance.Role_Entity.AddNewRoleTitle(byteStr);
			}
		}

		private void DealwithTaskCompelete()
		{
			Singleton<RoleManager>.Instance.Role_Entity.TaskStatus = 1;
			Singleton<UIManager>.Instance.FindWndByName<MainCityWnd>("MainCityWnd.prefab")?.ReflashMissionStatus();
		}

		private void DealwithTSBCompelete()
		{
			Singleton<RoleManager>.Instance.Role_Entity.TianShuBeiStatus = 1;
			Singleton<UIManager>.Instance.FindWndByName<MainCityWnd>("MainCityWnd.prefab")?.ReflashMissionStatus();
		}

		private void TongZhiTMCG(ByteString obj)
		{
			if (!(obj == null))
			{
				TongZhiTMCGResp_1014024 tongZhiTMCGResp_ = new TongZhiTMCGResp_1014024();
				tongZhiTMCGResp_.MergeFrom(obj);
				ShiTuWnd shiTuWnd = Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab");
				if (shiTuWnd != null)
				{
					shiTuWnd.ToZi(tongZhiTMCGResp_.TmcgType, tongZhiTMCGResp_.TmcgNum);
				}
				else
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("收到来自师傅的关怀！");
				}
			}
		}

		private void ExitDungeon()
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName.Equals("FightingScene"))
			{
				Singleton<BattleManager>.Instance.m_BattleSceneMgr?.TimeOver();
			}
		}

		private void DealwithJoinZongMen(ByteString byteStr)
		{
			if (!(byteStr == null))
			{
				GuildInformRoleJoinSuccessResp_1015018 guildInformRoleJoinSuccessResp_ = new GuildInformRoleJoinSuccessResp_1015018();
				guildInformRoleJoinSuccessResp_.MergeFrom(byteStr);
				Singleton<RoleManager>.Instance.Role_Entity.ZongMenId = guildInformRoleJoinSuccessResp_.GuildId.ToString();
				Singleton<RoleManager>.Instance.Role_Entity.ZongMenName = guildInformRoleJoinSuccessResp_.GuildName.ToString();
				Singleton<RoleManager>.Instance.Role_Entity.ZongMenSimpleName = guildInformRoleJoinSuccessResp_.GuildSimpleName.ToString();
				Singleton<RoleManager>.Instance.Role_Entity.ZongMenIconIndex = guildInformRoleJoinSuccessResp_.TokenIndex;
				Singleton<RoleManager>.Instance.Role_Entity.ZongMenLastNameImgIndex = guildInformRoleJoinSuccessResp_.LastNameIndex;
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				Singleton<ChatManager>.Instance.JoinZongMenChatServer(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("您加入了" + Singleton<RoleManager>.Instance.Role_Entity.ZongMenName);
			}
		}

		private void DealExitZongMen(ByteString byteStr)
		{
			if (byteStr == null)
			{
				return;
			}
			new GuildExpelRoleInformResp_1015019().MergeFrom(byteStr);
			Singleton<ChatManager>.Instance.LiKaiChatRoom(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
			string zongMenName = Singleton<RoleManager>.Instance.Role_Entity.ZongMenName;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenId = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenName = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenSimpleName = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenIconIndex = 0;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenLastNameImgIndex = 0;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "ZongMenScene")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("您已经脱离了" + zongMenName, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				}, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				});
				return;
			}
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("您已经脱离了" + zongMenName);
			ZongMenChatWnd zongMenChatWnd = Singleton<UIManager>.Instance.FindWndByName<ZongMenChatWnd>("ZongMenChatWnd.prefab");
			if (zongMenChatWnd != null)
			{
				Singleton<UIManager>.Instance.CloseWnd(zongMenChatWnd);
			}
		}

		private void DealWithActionCallBack(Msg _data)
		{
			if (WaitCallBackDic.ContainsKey(_data.Msid))
			{
				if (WaitCallBackDic[_data.Msid] != null && _data != null && _data.Data != null)
				{
					WaitCallBackDic[_data.Msid](_data.Data);
				}
				WaitCallBackDic.Remove(_data.Msid);
			}
			else
			{
				Debug.Log("发现了一个未记录标识的消息!!!" + _data.Msid + "Type: " + (MsgType)_data.Cmd/*cast due to constrained. prefix*/);
			}
		}

		private void DealWithRadionMsg(Msg _data)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			ByteString data = _data.Data;
			if (!(data == null))
			{
				BroadcastResp_1001014 broadcastResp_ = new BroadcastResp_1001014();
				broadcastResp_.MergeFrom(data);
				string msg = broadcastResp_.Msg;
				if (GameRadioAction != null && !string.IsNullOrEmpty(msg))
				{
					GameRadioAction(msg);
				}
			}
		}

		private void DealWithGameException(Msg _data)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			if (GameExceptionAction != null)
			{
				GameExceptionAction();
			}
		}

		private void DealWithMail()
		{
			Singleton<RoleManager>.Instance.NewLetterNotice(isNew: true);
		}

		private IEnumerator IEUpdate()
		{
#if UNITY_EDITOR
			object nextFrame = null;
#else
			var nextFrame = new WaitForEndOfFrame();
#endif
			while (true)
			{
				if (NetMsgQue != null && NetMsgQue.Count > 0)
				{
					byte[] data = NetMsgQue.Dequeue().Data as byte[];
					Msg msg = MsgSplit<Msg>(data, ref m_ReceiveByte, ref m_ReceiveByteLength, ref m_ReceiveByteHeadLength);
					if (msg != null)
					{
						dealWithResMsg(msg);
					}
				}
				else if (m_ReceiveByte != null && m_ReceiveByte.Length != 0)
				{
					Msg msg2 = MsgSplit<Msg>(null, ref m_ReceiveByte, ref m_ReceiveByteLength, ref m_ReceiveByteHeadLength);
					if (msg2 != null)
					{
						dealWithResMsg(msg2);
					}
				}
				if (BattleServerMsgQue != null && BattleServerMsgQue.Count > 0)
				{
					byte[] data2 = BattleServerMsgQue.Dequeue().Data as byte[];
					Msg msg3 = MsgSplit<Msg>(data2, ref m_ReceiveBattleByte, ref m_ReceiveBattleByteLength, ref m_ReceiveBattleByteHeadLength);
					if (msg3 != null)
					{
						dealWithResMsg(msg3);
					}
				}
				else if (m_ReceiveBattleByte != null && m_ReceiveBattleByte.Length != 0)
				{
					Msg msg4 = MsgSplit<Msg>(null, ref m_ReceiveBattleByte, ref m_ReceiveBattleByteLength, ref m_ReceiveBattleByteHeadLength);
					if (msg4 != null)
					{
						dealWithResMsg(msg4);
					}
				}
				if (ChatServerMsgQue != null && ChatServerMsgQue.Count > 0)
				{
					byte[] data3 = ChatServerMsgQue.Dequeue().Data as byte[];
					Msg msg5 = MsgSplit<Msg>(data3, ref m_ReceiveChatByte, ref m_ReceiveChatByteLength, ref m_ReceiveChatByteHeadLength);
					if (msg5 != null)
					{
						dealWithResMsg(msg5);
					}
				}
				else if (m_ReceiveChatByte != null && m_ReceiveChatByte.Length != 0)
				{
					Msg msg6 = MsgSplit<Msg>(null, ref m_ReceiveChatByte, ref m_ReceiveChatByteLength, ref m_ReceiveChatByteHeadLength);
					if (msg6 != null)
					{
						dealWithResMsg(msg6);
					}
				}
				if (NetDisConnectQue.Count > 0)
				{
					NetPackget netPackget = NetDisConnectQue.Dequeue();
					if (DisConnectEvent != null)
					{
						DisConnectEvent(netPackget.FromServer);
					}
				}
				if (NetConnectSuccessQue.Count > 0)
				{
					NetPackget netPackget2 = NetConnectSuccessQue.Dequeue();
					if (ConnectSuccessEvent != null)
					{
						ConnectSuccessEvent(netPackget2.FromServer, IsReConnect);
					}
				}
				if (NetErrorQue.Count > 0)
				{
					NetPackget netPackget3 = NetErrorQue.Dequeue();
					if (NetErrorEvent != null && netPackget3 != null)
					{
						NetErrorEvent(netPackget3.FromServer);
					}
				}
				yield return nextFrame;
			}
		}
	}
}
