using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using PENet;
using PEProtocol;
using UnityEngine;
using XiuZhenJie.Client.Protocol;

namespace XiuZhenJie
{
	public class NetSvc
	{
		public static readonly string obj = "lock";

		public PESocket<ClientSession, GameMsg> client;

		public Queue<GameMsg> msgQue = new Queue<GameMsg>();

		public Queue<Msg> SendMsgQue = new Queue<Msg>();

		public Queue<Msg> StrandedMsgQue = new Queue<Msg>();

		public Queue<Msg> ReceiveMsgQue = new Queue<Msg>();

		private const int ThreadSleepSeconds = 3000;

		private IEnumerator HeartThr;

		public volatile bool isConnect;

		private bool ConnectHeart;

		private readonly object directGate = new object();

		private volatile DirectCloudSocket directSocket;

		public Action<ServerType> ConnectSuccessEvent;

		public Action<ServerType, byte[]> ReceiveMessageEvent;

		public Action<ServerType> DisConnectEvent;

		public Action<ServerType, string> NetErrorEvent;

		private ServerType m_CurrentType;

		public bool IsConnect => isConnect;

		public NetSvc(ServerType serverType)
		{
			m_CurrentType = serverType;
		}

		public void Init()
		{
			HeartThr = SendHeartPack();
		}

		public void StartLongConnect(string ip, int port)
		{
			InitNetSvc(ip, port);
			try
			{
				MonoSingleton<GameStart>.Instance.StartCoroutine(HeartThr);
			}
			catch (Exception ex)
			{
				Debug.Log("心跳线程启动失败" + ex.Message);
			}
		}

		public void AddSendMsgQue(Msg _msg)
		{
			SendMsgQue.Enqueue(_msg);
		}

		public void SendMessage(Msg _msg)
		{
			if (directSocket == null && client?.session == null)
			{
				return;
			}
			try
			{
				byte[] data = MsgBuild(_msg);
				if (IsConnect && TrySendFrame(data))
				{
					return;
				}
				StrandedMsgQue.Enqueue(_msg);
				Debug.Log("检测到断开连接,无法发送消息!");
			}
			catch (Exception ex)
			{
				NetErrorEvent?.Invoke(m_CurrentType, "发送消息失败");
				isConnect = false;
				StrandedMsgQue.Enqueue(_msg);
				Debug.LogError("发送消息失败" + ex.Message);
			}
		}

		public void SendStrandedMsg()
		{
			for (int i = 0; i < StrandedMsgQue.Count; i++)
			{
				Msg msg = StrandedMsgQue.Dequeue();
				byte[] data = MsgBuild(msg);
				if (IsConnect && TrySendFrame(data))
				{
					continue;
				}
				StrandedMsgQue.Enqueue(msg);
				Debug.Log("检测到断开连接,无法发送消息!");
			}
		}

		public void OnConnectedCallBack()
		{
			isConnect = true;
			if (ConnectSuccessEvent != null)
			{
				ConnectSuccessEvent(m_CurrentType);
			}
		}

		public void OnReceiveMsgCallBack(byte[] byteArray)
		{
			lock (obj)
			{
				if (ReceiveMessageEvent != null)
				{
					ReceiveMessageEvent(m_CurrentType, byteArray);
				}
			}
		}

		public void OnDisConnectedCallBack()
		{
			isConnect = false;
			if (DisConnectEvent != null)
			{
				DisConnectEvent(m_CurrentType);
			}
		}

		public IEnumerator SendMsgIE()
		{
			while (true)
			{
				if (SendMsgQue.Count > 0 && IsConnect)
				{
					SendMessage(SendMsgQue.Dequeue());
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public void FixUpdate()
		{
			if (SendMsgQue.Count > 0 && IsConnect)
			{
				SendMessage(SendMsgQue.Dequeue());
			}
		}

		public void InitNetSvc(string ip, int port)
		{
			if (DirectCloudTrial.Enabled && (m_CurrentType == ServerType.GamesNormalServer || m_CurrentType == ServerType.BattleServer))
			{
				lock (directGate)
				{
					directSocket?.Close();
					isConnect = false;
					ConnectHeart = true;
					directSocket = new DirectCloudSocket(m_CurrentType, OnDirectConnected, OnDirectReceived, OnDirectDisconnected);
					directSocket.Start();
				}
				return;
			}
			client = new PESocket<ClientSession, GameMsg>(this);
			client.SetLog(log: true, (string msg, int lv) =>
			{
				switch (lv)
				{
				case 0:
					msg = "Log:" + msg;
					Debug.Log(msg);
					break;
				case 1:
					msg = "Warn:" + msg;
					Debug.Log(msg);
					break;
				case 2:
					msg = "Error:" + msg;
					isConnect = false;
					NetErrorEvent?.Invoke(m_CurrentType, msg);
					Debug.Log("NetError:" + msg);
					break;
				case 3:
					msg = "Info:" + msg;
					Debug.Log(msg);
					break;
				}
			});
			ConnectHeart = true;
			client.StartAsClient(ip, port);
		}

		private bool TrySendFrame(byte[] frame)
		{
			DirectCloudSocket cloud = directSocket;
			if (cloud != null)
			{
				return cloud.TrySend(frame);
			}
			if (client?.session == null)
			{
				return false;
			}
			client.session.SendMsg(frame);
			return true;
		}

		private void OnDirectConnected(DirectCloudSocket socket)
		{
			lock (directGate)
			{
				if (socket == directSocket)
				{
					OnConnectedCallBack();
				}
			}
		}

		private void OnDirectReceived(DirectCloudSocket socket, byte[] frame)
		{
			lock (directGate)
			{
				if (socket == directSocket)
				{
					OnReceiveMsgCallBack(frame);
				}
			}
		}

		private void OnDirectDisconnected(DirectCloudSocket socket, bool established)
		{
			lock (directGate)
			{
				if (socket != directSocket)
				{
					return;
				}
				if (established)
				{
					OnDisConnectedCallBack();
				}
				else
				{
					NetErrorEvent?.Invoke(m_CurrentType, "无法连接云端试用服");
				}
			}
		}

		public byte[] MsgBuild(IMessage msg)
		{
			return ProtobufFrame.Encode(msg);
		}

		public IEnumerator SendHeartPack()
		{
			Msg HeartMsg = new Msg
			{
				Cmd = 9000000
			};
			int i = 60;
			while (ConnectHeart)
			{
				if (IsConnect && i == 60)
				{
					TrySendFrame(MsgBuild(HeartMsg));
				}
				yield return new WaitForSeconds(1f);
				i--;
				if (i <= 0)
				{
					i = 60;
				}
			}
		}

		public void NetSvcClose()
		{
			ConnectHeart = false;
			lock (directGate)
			{
				if (directSocket != null)
				{
					DirectCloudSocket old = directSocket;
					directSocket = null;
					isConnect = false;
					old.Close();
				}
			}
			client?.Close();
		}
	}
}
