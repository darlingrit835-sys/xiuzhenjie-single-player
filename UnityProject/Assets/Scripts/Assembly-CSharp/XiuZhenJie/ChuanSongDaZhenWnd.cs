namespace XiuZhenJie
{
	public class ChuanSongDaZhenWnd : Window
	{
		public ChuanSongDaZhenPanel chuanSongDaZhenPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			chuanSongDaZhenPanel = base.GameObject.GetComponent<ChuanSongDaZhenPanel>();
			AddButtonClickListener(chuanSongDaZhenPanel.LiKai_btn, LiKaiBtnClick);
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ChuanSongDaZhenWnd.prefab");
		}
	}
}
