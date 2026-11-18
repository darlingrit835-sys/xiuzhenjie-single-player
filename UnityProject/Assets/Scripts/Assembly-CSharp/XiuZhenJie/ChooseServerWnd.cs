using System;
using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class ChooseServerWnd : Window
	{
		public ChooseServerPanel _panel;

		public List<GameServerInfo> _serverList = new List<GameServerInfo>();

		public Action<string, int, GameServerInfo> _action;

		public string _lastServerId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ChooseServerPanel>();
			AddButtonClickListener(_panel.CloseBtn, CloseWindow);
		}

		public override void CloseWindow()
		{
			base.CloseWindow();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			_serverList = new List<GameServerInfo>();
			if (paralist.Length != 0)
			{
				_serverList = ((paralist[0] == null) ? new List<GameServerInfo>() : (paralist[0] as List<GameServerInfo>));
			}
			if (paralist.Length > 1)
			{
				_action = paralist[1] as Action<string, int, GameServerInfo>;
			}
			if (paralist.Length > 2)
			{
				_lastServerId = ((paralist[2] == null) ? "" : paralist[2].ToString());
			}
			if (_serverList == null || _serverList.Count == 0)
			{
				Debug.Log("沒有服務器列表");
				return;
			}
			SetServiceListData();
			if (!string.IsNullOrEmpty(_lastServerId))
			{
				_panel.LastChooseItem.gameObject.SetActive(value: true);
				SetLastServiceData(_lastServerId);
			}
			else
			{
				_panel.LastChooseItem.gameObject.SetActive(value: false);
			}
		}

		public void SetLastServiceData(string serviceId)
		{
			foreach (GameServerInfo item in _serverList)
			{
				if (item.gameServerId == serviceId)
				{
					if (item.status == -1)
					{
						_panel.LastChooseBtnText.text = "维护中";
						_panel.LastChooseBtn.interactable = false;
						break;
					}
					if (item.status == 2)
					{
						_panel.LastChooseBtnText.text = "已满";
						_panel.LastChooseBtn.interactable = false;
						break;
					}
					_panel.LastChooseBtnText.text = "入界";
					_panel.LastChooseBtn.interactable = true;
					_panel.LastServerNameText.text = item.name;
					AddButtonClickListener(_panel.LastChooseBtn, () =>
					{
						CheckLogin(item);
					});
				}
			}
		}

		public void SetServiceListData()
		{
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChooseServerItem.prefab");
			RectTransform component = _panel.Content.gameObject.GetComponent<RectTransform>();
			foreach (Transform item2 in component)
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
			foreach (GameServerInfo item in _serverList)
			{
				SetListContentParent(component, gameObject).GetComponent<ChooseServerItem>().SetValue(item, () =>
				{
					CheckLogin(item);
				});
			}
			float y = SetVerticalListContentHight(_serverList.Count, gameObject.GetComponent<RectTransform>().sizeDelta.y, _panel.Content);
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		}

		private void CheckLogin(GameServerInfo item)
		{
			Constants.SocketServerIP = item.serverIp;
			Constants.SocketServerPort = item.port;
			if (!"1.3.5".Equals(item.serverVersion))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("客户端版本不正确，请下载最新客户端");
				return;
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
			if (_action != null)
			{
				_action(item.serverIp, item.port, item);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			foreach (Transform item in _panel.Content.gameObject.GetComponent<RectTransform>())
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}
}
