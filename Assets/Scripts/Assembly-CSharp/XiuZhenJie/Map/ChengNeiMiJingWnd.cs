namespace XiuZhenJie
{
	public class ChengNeiMiJingWnd : Window
	{
		public ChengNeiMiJingPanel chengNeiMiJingPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			chengNeiMiJingPanel = base.GameObject.GetComponent<ChengNeiMiJingPanel>();
			AddButtonClickListener(chengNeiMiJingPanel.LiKai_btn, LiKaiBtnClick);
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ChengNeiMiJingWnd.prefab");
		}
	}
}
