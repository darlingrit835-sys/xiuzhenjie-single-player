namespace XiuZhenJie
{
	public class ChuanSongTiShiWnd : Window
	{
		public ChuanSongTiShiPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ChuanSongTiShiPanel>();
			AddButtonClickListener(_panel.Clsoe_btn, CloseBtnClick);
			AddButtonClickListener(_panel.ChuanSong_btn, ChuanSongBtnClick);
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ChuanSongTiShiWnd.prefab");
		}

		private void ChuanSongBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ChuanSongTiShiWnd.prefab");
		}
	}
}
