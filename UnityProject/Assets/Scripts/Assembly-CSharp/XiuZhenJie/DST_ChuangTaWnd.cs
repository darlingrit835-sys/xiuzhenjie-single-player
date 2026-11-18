namespace XiuZhenJie
{
	public class DST_ChuangTaWnd : Window
	{
		public DST_ChuangTaPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<DST_ChuangTaPanel>();
			AddButtonClickListener(_panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(_panel.DaoJingBtn, DaoJingBtnClick);
			AddButtonClickListener(_panel.SuiYueBtn, SuiYueBtnClick);
			AddButtonClickListener(_panel.ChuangTaBtn, ChuangTaBtnClick);
			_panel.SetValue(9);
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("DST_ChuangTaWnd.prefab");
		}

		private void DaoJingBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DST_BangDanWnd.prefab", true, 1);
		}

		private void SuiYueBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DST_BangDanWnd.prefab", true, 2);
		}

		private void ChuangTaBtnClick()
		{
		}
	}
}
