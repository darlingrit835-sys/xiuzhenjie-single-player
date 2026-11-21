using PENet;
using PEProtocol;

namespace XiuZhenJie
{
	public class ClientSession : PESession<GameMsg>
	{
		private NetSvc m_NetSvc;

		public override void Init(params object[] arg0)
		{
			base.Init(arg0);
			m_NetSvc = arg0[1] as NetSvc;
		}

		protected override void OnConnected()
		{
			m_NetSvc?.OnConnectedCallBack();
		}

		protected override void OnReciveMsgByte(byte[] msg)
		{
			m_NetSvc?.OnReceiveMsgCallBack(msg);
		}

		protected override void OnDisConnected()
		{
			m_NetSvc?.OnDisConnectedCallBack();
		}
	}
}
