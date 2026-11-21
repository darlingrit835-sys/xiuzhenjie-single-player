namespace XiuZhenJie
{
	public class DiHuoMiShiWnd : Window
	{
		private DiHuoMiShiPanel diHuoMiShiPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			diHuoMiShiPanel = base.GameObject.GetComponent<DiHuoMiShiPanel>();
			AddButtonClickListener(diHuoMiShiPanel.LiKai_btn, LiKaiBtnClick);
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("DiHuoMiShiWnd.prefab");
		}
	}
}
