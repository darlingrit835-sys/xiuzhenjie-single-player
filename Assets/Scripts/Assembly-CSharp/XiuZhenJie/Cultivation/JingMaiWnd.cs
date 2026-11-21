namespace XiuZhenJie
{
	public class JingMaiWnd : Window
	{
		public JingMaiPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<JingMaiPanel>();
			InkCultivationPages.PaperDialog(_panel.transform, "NengLiangShengYu", _panel.JiXuTuPoBtn, null);
			Init();
			AddButtonClickListener(_panel.LiKaiBtn, () =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("DongFuWnd.prefab", true);
				Singleton<UIManager>.Instance.CloseWnd("JingMaiWnd.prefab");
			});
			AddButtonClickListener(_panel.JiXuTuPoBtn, () =>
			{
			});
		}

		public override void OnClose()
		{
			base.OnClose();
			MonoSingleton<DongFuSceneManager>.Instance.ExitJingMaiWnd();
		}

		public void Init()
		{
			MonoSingleton<DongFuSceneManager>.Instance.EnterJingMaiWnd();
		}
	}
}
