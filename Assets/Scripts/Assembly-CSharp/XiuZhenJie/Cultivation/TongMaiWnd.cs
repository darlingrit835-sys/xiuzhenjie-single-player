namespace XiuZhenJie
{
	public class TongMaiWnd : Window
	{
		public TongMaiPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<TongMaiPanel>();
			AddButtonClickListener(_panel.CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("TongMaiWnd.prefab");
			});
			AddButtonClickListener(_panel.WaitBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("TongMaiWnd.prefab");
			});
		}
	}
}
