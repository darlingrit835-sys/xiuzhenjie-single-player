namespace XiuZhenJie
{
	public class YouJianDongFuWnd : Window
	{
		public YouJianDongFuPanel youJianDongFuPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			youJianDongFuPanel = base.GameObject.GetComponent<YouJianDongFuPanel>();
			AddButtonClickListener(youJianDongFuPanel.LiKai_btn, LiKaiBtnClick);
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("YouJianDongFuWnd.prefab");
		}
	}
}
