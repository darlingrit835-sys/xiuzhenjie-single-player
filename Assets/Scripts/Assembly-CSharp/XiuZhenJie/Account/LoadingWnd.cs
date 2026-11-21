using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class LoadingWnd : Window
	{
		public string SceneName;

		public LoadingWndPanel Panel;

		public UnityAction m_CallBackAction;

		private Dictionary<string, object[]> m_SceneNameDic = new Dictionary<string, object[]>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LoadingWndPanel>();
			LoadingCrane.AddTo(Panel.transform, Panel.MaskImg, Panel.bgImg);
			if (Panel.transform.Find("SuCaiImg")?.GetComponent<UnityEngine.UI.Image>() is UnityEngine.UI.Image oldMountains)
			{
				oldMountains.enabled = false;
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_SceneNameDic.Clear();
			if (paralist.Length == 2)
			{
				SceneName = (string)paralist[0];
				m_CallBackAction = paralist[1] as UnityAction;
			}
			Color color = Panel.MaskImg.color;
			Panel.MaskImg.color = new Color(color.r, color.g, color.b, 0f);
			m_CallBackAction?.Invoke();
		}

		public override void OnUpdate()
		{
			base.Transform.SetAsLastSibling();
			if (GameMapManager.LoadingProgress >= 100)
			{
				GameMapManager.LoadingProgress = 0;
				LoadOtherScene();
			}
		}

		public override bool OnMessage(UIMsgID msgID, params object[] paralist)
		{
			if (m_SceneNameDic.ContainsKey(SceneName))
			{
				return false;
			}
			m_SceneNameDic.Add(SceneName, paralist);
			return true;
		}

		public void LoadOtherScene()
		{
			CloseLoadingWnd();
		}

		private void CloseLoadingWnd()
		{
			object[] paralist = null;
			if (SceneName == "LoginScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd();
				Singleton<UIManager>.Instance.PopUpWnd("StartLoadSceneWnd.prefab", resource: true, bTop: true, null);
			}
			else if (SceneName == "MainScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[1] { "LiXianShouYiWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("MainTitleWnd.prefab", true);
				Singleton<UIManager>.Instance.PopUpWnd("MainCityWnd.prefab", true);
				Singleton<UIManager>.Instance.PopUpWnd("CeHuaWnd.prefab", true);
				Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: true);
			}
			else if (SceneName == "FightingScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
			}
			else if (SceneName == "DongFuScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("DongFuWnd.prefab", bTop: true, paralist);
				Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: true);
			}
			else if (SceneName == "DiHuoGe_LianDan")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeWnd.prefab", bTop: true, paralist);
			}
			else if (SceneName == "TuPoScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("TuPoDuJieWnd.prefab", bTop: true, paralist);
			}
			else if (SceneName == "ChuangZhaoGongFaScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("ChuangZhaoGongFaWnd.prefab", bTop: true, paralist);
			}
			else if (SceneName == "ChuangZhaoFaShuScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("ChuangZaoFaShuWnd.prefab", bTop: true, paralist);
			}
			else if (SceneName == "NeiShiScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("NeiShiWnd.prefab", bTop: true, paralist);
			}
			else if (SceneName == "DongFuExmpleScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd();
				Singleton<UIManager>.Instance.PopUpWnd("DongFuExmpleWnd.prefab", true);
			}
			else if (SceneName == "TuPoExmpleScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd();
				Singleton<UIManager>.Instance.PopUpWnd("TuPoDuJieExmpleWnd.prefab", true);
			}
			else if (SceneName == "ZongMenScene")
			{
				if (m_SceneNameDic.ContainsKey(SceneName))
				{
					paralist = m_SceneNameDic[SceneName];
					m_SceneNameDic.Remove(SceneName);
				}
				Singleton<UIManager>.Instance.CloseAllWnd(new string[2] { "MainTitleWnd.prefab", "CeHuaWnd.prefab" });
				Singleton<UIManager>.Instance.PopUpWnd("ZongMenWnd.prefab", bTop: true, paralist);
			}
			Singleton<UIManager>.Instance.CloseWnd("LoadingWnd.prefab");
		}

		public override void OnClose()
		{
			base.OnClose();
		}
	}
}
