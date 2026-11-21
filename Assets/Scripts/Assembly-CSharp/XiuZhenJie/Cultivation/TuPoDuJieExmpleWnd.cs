using System;
using UnityEngine;

namespace XiuZhenJie
{
	public class TuPoDuJieExmpleWnd : Window
	{
		public TuPoDuJieExmplePanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = GameObject.GetComponent<TuPoDuJieExmplePanel>();
			AddButtonClickListener(Panel.EnterLunHuiBtn, EnterLunHuiClick);
			Panel.TiShiPanel.gameObject.SetActive(false);
			InkThunderTrial.OpenMemory(Panel, won =>
			{
				InkCultivationPages.PaperDialog(Panel.transform, "TiShiPanel/LiXianBg", Panel.EnterLunHuiBtn, null);
				var heading = Panel.TiShiPanel.Find("LiXianBg/TitleBg/Text").GetComponent<UnityEngine.UI.Text>();
				var body = Panel.TiShiPanel.Find("LiXianBg/ContentTextBg/Text").GetComponent<UnityEngine.UI.Text>();
				heading.text = "前尘如梦";
				body.text = won ? "三道雷霆已过，玄玉真人的前尘随劫云散去。入轮回，再续仙途。" :
					"雷云遮住了玄玉真人的去路，前世的记忆止于此处。入轮回，再续仙途。";
				Panel.TiShiPanel.gameObject.SetActive(true);
				Panel.TiShiPanel.SetAsLastSibling();
			});
		}

		private void EnterLunHuiClick()
		{
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance2 = Singleton<NetManager>.Instance;
			instance2.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(instance2.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance3 = Singleton<NetManager>.Instance;
			instance3.NetErrorEvent = (Action<ServerType>)Delegate.Remove(instance3.NetErrorEvent, new Action<ServerType>(ConnectError));
			NetManager instance4 = Singleton<NetManager>.Instance;
			instance4.NetErrorEvent = (Action<ServerType>)Delegate.Combine(instance4.NetErrorEvent, new Action<ServerType>(ConnectError));
			Singleton<NetManager>.Instance.StartFirstConnect(Constants.SocketServerIP, Constants.SocketServerPort);
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
		}

		private void ConnectSuccess(ServerType serverType, bool isRestart)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance2 = Singleton<NetManager>.Instance;
			instance2.NetErrorEvent = (Action<ServerType>)Delegate.Remove(instance2.NetErrorEvent, new Action<ServerType>(ConnectError));
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.PopUpWnd("CreateRoleWnd.prefab", true);
		}

		private void ConnectError(ServerType serverType)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance2 = Singleton<NetManager>.Instance;
			instance2.NetErrorEvent = (Action<ServerType>)Delegate.Remove(instance2.NetErrorEvent, new Action<ServerType>(ConnectError));
			MonoSingleton<GameStart>.Instance.PopUpAttention("无法连接到服务器，请稍后再试");
		}
	}
}
