using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using PENet;
using PEProtocol;
using UnityEngine;

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

		public bool isConnect;

		private bool ConnectHeart;

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
			if (client.session == null)
			{
				return;
			}
			try
			{
				byte[] data = MsgBuild(_msg);
				if (IsConnect)
				{
					client.session.SendMsg(data);
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
				if (IsConnect)
				{
					client.session.SendMsg(data);
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

		public byte[] MsgBuild(IMessage msg)
		{
			byte[] array = msg.ToByteArray();
			uint value = (uint)array.Length;
			int num = CodedOutputStream.ComputeRawVarint32Size(value);
			byte[] array2 = new byte[array.Length + num];
			array.CopyTo(array2, num);
			new CodedOutputStream(array2).WriteUInt32(value);
			return array2;
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
					client.session?.SendMsg(MsgBuild(HeartMsg));
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
			client?.Close();
		}
	}
}
