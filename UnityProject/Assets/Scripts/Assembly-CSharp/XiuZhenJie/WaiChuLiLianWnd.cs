namespace XiuZhenJie
{
	public class WaiChuLiLianWnd : Window
	{
		public WaiChuLiLianPanel waiChuLiLianPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			waiChuLiLianPanel = base.GameObject.GetComponent<WaiChuLiLianPanel>();
			AddButtonClickListener(waiChuLiLianPanel.LiKai_btn, LiKaiBtnClick);
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("WaiChuLiLianWnd.prefab");
		}
	}
}
