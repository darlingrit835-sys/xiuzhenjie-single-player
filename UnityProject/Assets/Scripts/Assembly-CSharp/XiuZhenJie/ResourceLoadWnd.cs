namespace XiuZhenJie
{
	public class ResourceLoadWnd : Window
	{
		public ResourceLoadPanel resourceLoadPanel;

		public string m_SceneName;

		public override void Awake(params object[] paralist)
		{
			resourceLoadPanel = base.GameObject.GetComponent<ResourceLoadPanel>();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_SceneName = (string)paralist[0];
			base.GameObject.SetActive(value: true);
		}

		public override void OnUpdate()
		{
			if (!(resourceLoadPanel == null))
			{
				resourceLoadPanel.LoadProgressbar.size = (float)GameMapManager.LoadingProgress / 100f;
				int.Parse(GameMapManager.LoadingProgress.ToString());
				if (GameMapManager.LoadingProgress >= 100)
				{
					LoadOtherScene();
				}
			}
		}

		public void LoadOtherScene()
		{
			if (m_SceneName == "LoginScene")
			{
				Singleton<UIManager>.Instance.PopUpWnd("SignInWnd.prefab", true);
			}
			else if (m_SceneName == "MainScene")
			{
				Singleton<UIManager>.Instance.PopUpWnd("MainTitleWnd.prefab", true);
				Singleton<UIManager>.Instance.PopUpWnd("MainCityWnd.prefab", true);
				Singleton<UIManager>.Instance.PopUpWnd("CeHuaWnd.prefab", true);
			}
			else
			{
				_ = m_SceneName == "FightingScene";
			}
			resourceLoadPanel.LoadProgressbar.size = 0f;
			resourceLoadPanel.LoadPaneltext.text = "0";
			Singleton<UIManager>.Instance.CloseWnd("ResourceLoadWnd.prefab");
		}
	}
}
