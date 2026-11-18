using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class UIManager : Singleton<UIManager>
	{
		private static readonly HashSet<string> WindowTitleNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Title", "TitleText", "TitleNameText", "Title_text", "NameTitle", "BiaoTi"
		};

		public MonoBehaviour m_Mono;

		public RectTransform m_UiRoot;

		private RectTransform m_WndRoot;

		private Camera m_UICamera;

		private EventSystem m_EventSystem;

		private float m_CanvasRate;

		private string m_UIPrefabPath = "Assets/GameData/Prefabs/UIPrefabs/Window/";

		private Dictionary<string, Type> m_RegisterDic = new Dictionary<string, Type>();

		private Dictionary<string, Window> m_WindowDic = new Dictionary<string, Window>();

		private List<Window> m_WindowList = new List<Window>();

		public RectTransform WndRoot => m_WndRoot;

		public Camera UICamera => m_UICamera;

		public void Init(RectTransform uiRoot, RectTransform wndRoot, Camera uiCamera, EventSystem eventSystem, MonoBehaviour mono)
		{
			m_UiRoot = uiRoot;
			m_WndRoot = wndRoot;
			m_UICamera = uiCamera;
			m_EventSystem = eventSystem;
			m_CanvasRate = (float)Screen.height / (m_UICamera.orthographicSize * 2f);
			m_Mono = mono;
		}

		public void ReFlashAllOpenedWnd()
		{
			for (int i = 0; i < m_WindowList.Count; i++)
			{
				m_WindowList[i].ReFlashData();
			}
		}

		public void SetUIPrefabPath(string path)
		{
			m_UIPrefabPath = path;
		}

		internal static void FitTextToRect(Text text)
		{
			if (text == null)
			{
				return;
			}
			int maxSize = text.resizeTextForBestFit ? text.resizeTextMaxSize : text.fontSize;
			text.resizeTextMaxSize = maxSize;
			text.resizeTextMinSize = Mathf.Min(8, maxSize);
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.verticalOverflow = VerticalWrapMode.Truncate;
			text.resizeTextForBestFit = true;
		}

		private static void FitWindowTitles(GameObject window)
		{
			foreach (Text text in window.GetComponentsInChildren<Text>(true))
			{
				if (WindowTitleNames.Contains(text.name) || (text.name == "Text" && WindowTitleNames.Contains(text.transform.parent.name)))
				{
					FitTextToRect(text);
				}
			}
		}

		public void ShowOrHideUI(bool show)
		{
			if (m_UiRoot != null)
			{
				m_UiRoot.gameObject.SetActive(show);
			}
		}

		public void SetNormalSelectObj(GameObject obj)
		{
			if (m_EventSystem == null)
			{
				m_EventSystem = EventSystem.current;
			}
			m_EventSystem.firstSelectedGameObject = obj;
		}

		public void OnUpdate()
		{
			for (int i = 0; i < m_WindowList.Count; i++)
			{
				if (m_WindowList[i] != null)
				{
					m_WindowList[i].OnUpdate();
				}
			}
		}

		public void OnFixUpdate()
		{
			for (int i = 0; i < m_WindowList.Count; i++)
			{
				if (m_WindowList[i] != null)
				{
					m_WindowList[i].OnFixUpdate();
				}
			}
		}

		public void Register<T>(string name) where T : Window
		{
			m_RegisterDic[name] = typeof(T);
		}

		public void Register(string prefabName, string classType)
		{
			List<Type> list = (from type in Assembly.Load("Assembly-CSharp").GetTypes()
				where type.FullName == classType
				select type).ToList();
			if (list.Count > 0)
			{
				m_RegisterDic[prefabName] = list[0];
			}
		}

		public bool SendMessageToWnd(string name, UIMsgID msgID = UIMsgID.None, params object[] paralist)
		{
			return FindWndByName<Window>(name)?.OnMessage(msgID, paralist) ?? false;
		}

		public T FindWndByName<T>(string name) where T : Window
		{
			Window value = null;
			if (m_WindowDic.TryGetValue(name, out value))
			{
				return (T)value;
			}
			return null;
		}

		public Window FindWndByName(string name)
		{
			Window value = null;
			if (m_WindowDic.TryGetValue(name, out value))
			{
				return value;
			}
			return null;
		}

		public Window PopUpWnd(string wndName, bool resource, bool bTop = true, params object[] paralist)
		{
			Window window = FindWndByName<Window>(wndName);
			if (window == null)
			{
				Type value = null;
				if (!m_RegisterDic.TryGetValue(wndName, out value))
				{
					Debug.LogError("找不到窗口对应的脚本，窗口名是：" + wndName);
					return null;
				}
				window = Activator.CreateInstance(value) as Window;
				GameObject gameObject = null;
				gameObject = ((!resource) ? Singleton<ObjectManager>.Instance.InstantiateObject(m_UIPrefabPath + wndName, setSceneObj: false, bClear: false) : UnityEngine.Object.Instantiate(Resources.Load<GameObject>(("Prefab/Window/" + wndName).Replace(".prefab", ""))));
				if (gameObject == null)
				{
					Debug.Log("创建窗口Prefab失败：" + wndName);
					return null;
				}
				if (!m_WindowDic.ContainsKey(wndName))
				{
					m_WindowList.Add(window);
					m_WindowDic.Add(wndName, window);
				}
				window.Resource = resource;
				window.GameObject = gameObject;
				window.Transform = gameObject.transform;
				window.Name = wndName;
				InkInterfaceArt.Apply(gameObject);
			FitWindowTitles(gameObject);
				window.Awake(paralist);
				gameObject.transform.SetParent(m_WndRoot, worldPositionStays: false);
				if (bTop)
				{
					gameObject.transform.SetAsLastSibling();
				}
				window.OnShow(paralist);
				if (InkWindowMotion.AppliesTo(wndName, window.GameObject))
				{
					InkWindowMotion.For(window.GameObject).Open();
				}
			}
			else
			{
				ShowWnd(wndName, bTop, paralist);
			}
			return window;
		}

		public Window PopUpWnd(string wndName, bool bTop = true, params object[] paralist)
		{
			Window window = FindWndByName<Window>(wndName);
			if (window == null)
			{
				Type value = null;
				if (!m_RegisterDic.TryGetValue(wndName, out value))
				{
					Debug.LogError("找不到窗口对应的脚本，窗口名是：" + wndName);
					return null;
				}
				window = Activator.CreateInstance(value) as Window;
				GameObject gameObject = Singleton<ObjectManager>.Instance.InstantiateObject(m_UIPrefabPath + wndName, setSceneObj: false, bClear: false);
				if (gameObject == null)
				{
					Debug.Log("创建窗口Prefab失败：" + wndName);
					return null;
				}
				if (!m_WindowDic.ContainsKey(wndName))
				{
					m_WindowList.Add(window);
					m_WindowDic.Add(wndName, window);
				}
				window.Resource = false;
				window.GameObject = gameObject;
				window.Transform = gameObject.transform;
				window.Name = wndName;
				InkInterfaceArt.Apply(gameObject);
			FitWindowTitles(gameObject);
				window.Awake(paralist);
				gameObject.transform.SetParent(m_WndRoot, worldPositionStays: false);
				if (bTop)
				{
					gameObject.transform.SetAsLastSibling();
				}
				window.OnShow(paralist);
				if (InkWindowMotion.AppliesTo(wndName, window.GameObject))
				{
					InkWindowMotion.For(window.GameObject).Open();
				}
			}
			else
			{
				ShowWnd(wndName, bTop, paralist);
			}
			return window;
		}

		public void CloseWnd(string name, bool destory = true)
		{
			Window window = FindWndByName<Window>(name);
			CloseWnd(window, destory);
		}

		public void CloseWnd(Window window, bool destory = true)
		{
			if (window == null)
			{
				return;
			}
			window.OnDisable();
			window.OnClose();
			if (m_WindowDic.ContainsKey(window.Name))
			{
				m_WindowDic.Remove(window.Name);
				m_WindowList.Remove(window);
			}
			GameObject view = window.GameObject;
			bool resource = window.Resource;
			window.GameObject = null;
			if (view == null)
			{
				return;
			}
			void Release()
			{
				if (resource)
					UnityEngine.Object.Destroy(view);
				else if (destory)
					Singleton<ObjectManager>.Instance.ReleaseObject(view, 0, destoryCache: true);
				else
					Singleton<ObjectManager>.Instance.ReleaseObject(view, -1, destoryCache: false, recycleParent: false);
			}
			if (InkWindowMotion.AppliesTo(window.Name, view))
				InkWindowMotion.For(view).Close(Release);
			else
				Release();
		}

		public void CloseAllWnd(bool destory = true)
		{
			for (int num = m_WindowList.Count - 1; num >= 0; num--)
			{
				CloseWnd(m_WindowList[num], destory);
			}
		}

		public void CloseAllWnd(string[] windowNames)
		{
			if (windowNames == null || windowNames.Length == 0)
			{
				CloseAllWnd();
			}
			Window[] array = new Window[windowNames.Length];
			for (int i = 0; i < windowNames.Length; i++)
			{
				if (!m_WindowDic.TryGetValue(windowNames[i], out array[i]))
				{
					CloseAllWnd();
				}
			}
			for (int num = m_WindowList.Count - 1; num >= 0; num--)
			{
				if (!array.Contains(m_WindowList[num]))
				{
					CloseWnd(m_WindowList[num]);
				}
			}
		}

		public void SwitchStateByName(string name, bool bTop = true, bool resource = false, params object[] paralist)
		{
			CloseAllWnd();
			PopUpWnd(name, bTop, resource, paralist);
		}

		public void HideWnd(string name)
		{
			Window wnd = FindWndByName<Window>(name);
			HideWnd(wnd);
		}

		public void HideWnd(Window wnd)
		{
			if (wnd != null)
			{
				wnd.GameObject.SetActive(value: false);
				wnd.OnDisable();
			}
		}

		public void ShowWnd(string name, bool bTop = true, params object[] paralist)
		{
			Window wnd = FindWndByName<Window>(name);
			ShowWnd(wnd, bTop, paralist);
		}

		public void ShowWnd(Window wnd, bool bTop = true, params object[] paralist)
		{
			if (wnd != null)
			{
				bool reopening = wnd.GameObject != null && !wnd.GameObject.activeSelf;
				if (reopening)
				{
					wnd.GameObject.SetActive(value: true);
				}
				if (bTop)
				{
					wnd.Transform.SetAsLastSibling();
				}
				wnd.OnShow(paralist);
				if (reopening && InkWindowMotion.AppliesTo(wnd.Name, wnd.GameObject))
				{
					InkWindowMotion.For(wnd.GameObject).Open();
				}
			}
		}
	}
}
