namespace XiuZhenJie
{
	public class ShengXianTaWnd : Window
	{
		public ShengXianTaPanel shengXianTaPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			shengXianTaPanel = base.GameObject.GetComponent<ShengXianTaPanel>();
			AddButtonClickListener(shengXianTaPanel.LiKai_btn, LiKaiBtnClick);
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ShengXianTaWnd.prefab");
		}
	}
}
