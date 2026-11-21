using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChooseServerWnd : Window
	{
		public ChooseServerPanel _panel;

		public List<GameServerInfo> _serverList = new List<GameServerInfo>();

		public Action<string, int, GameServerInfo> _action;

		public string _lastServerId;

		private Coroutine m_EnterRealm;

		private InkSealFeedback m_SealFeedback;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ChooseServerPanel>();
			StyleDialog();
			AddButtonClickListener(_panel.CloseBtn, CloseWindow);
		}

		private void StyleDialog()
		{
			InkDialogArt.SetWash(_panel.transform);
			Transform card = _panel.transform.Find("ChooseServerBg");
			InkDialogArt.SetPaper(card?.GetComponent<Image>());
			if (card != null)
			{
				foreach (Text label in card.GetComponentsInChildren<Text>(true))
				{
					label.color = InkInterfaceArt.Ink;
				}
				Transform title = card.Find("Title");
				string titleArt = InkInterfaceArt.Get("LightPlaque") != null ? "LightPlaque" : "IvoryPlaque";
				InkInterfaceArt.Set(title?.GetComponent<Image>(), titleArt, false);
				Text heading = title?.GetComponentInChildren<Text>(true);
				if (heading != null)
				{
					heading.text = "选择修真界";
					heading.color = InkInterfaceArt.Ink;
					UIManager.FitTextToRect(heading);
				}
				Image band = card.Find("TitleLine")?.GetComponent<Image>();
				if (band != null)
				{
					band.color = Color.clear;
				}
				var close = (RectTransform)_panel.CloseBtn.transform;
				close.SetParent(card, false);
				close.anchorMin = close.anchorMax = Vector2.one;
				close.sizeDelta = new Vector2(29f, 29f);
				close.anchoredPosition = new Vector2(-21f, -19f);
			}
			ChooseServerItem.StyleRow(_panel.LastChooseBtn.transform.parent.GetComponent<Image>(), _panel.LastChooseBtn);
			InkDialogArt.StyleButton(_panel.CloseBtn, "Close", InkInterfaceArt.Ink);
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
					Transform roleMarker = _panel.LastChooseBtn.transform.parent.Find("Image");
					if (roleMarker != null)
					{
						roleMarker.gameObject.SetActive(!string.IsNullOrEmpty(item.currentRoleVO?.id));
					}
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
						CheckLogin(item, _panel.LastChooseBtn);
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
				ChooseServerItem row = SetListContentParent(component, gameObject).GetComponent<ChooseServerItem>();
				row.SetValue(item, () => CheckLogin(item, row.ChooseBtn));
			}
			float y = SetVerticalListContentHight(_serverList.Count, gameObject.GetComponent<RectTransform>().sizeDelta.y, _panel.Content);
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		}

		private void CheckLogin(GameServerInfo item, Button button)
		{
			if (m_EnterRealm != null)
			{
				return;
			}
			if (!"1.3.5".Equals(item.serverVersion))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("客户端版本不正确，请下载最新客户端");
				return;
			}
			m_SealFeedback = InkSealFeedback.Play(button, "入界");
			m_EnterRealm = Singleton<UIManager>.Instance.m_Mono.StartCoroutine(EnterRealm(item, _action));
		}

		private IEnumerator EnterRealm(GameServerInfo item, Action<string, int, GameServerInfo> enter)
		{
			// Let the click draw before the connection starts loading role data.
			yield return null;
			m_EnterRealm = null;
			m_SealFeedback = null;
			Constants.SocketServerIP = item.serverIp;
			Constants.SocketServerPort = item.port;
			Singleton<UIManager>.Instance.CloseWnd(this);
			enter?.Invoke(item.serverIp, item.port, item);
		}

		public override void OnDisable()
		{
			if (m_EnterRealm != null)
			{
				Singleton<UIManager>.Instance.m_Mono.StopCoroutine(m_EnterRealm);
				m_EnterRealm = null;
				if (m_SealFeedback != null)
					m_SealFeedback.Finish();
				m_SealFeedback = null;
			}
			base.OnDisable();
			foreach (Transform item in _panel.Content.gameObject.GetComponent<RectTransform>())
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}
}
