namespace XiuZhenJie
{
	public class XuShenJieWnd : Window
	{
		public XuShenJiePanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<XuShenJiePanel>();
			AddButtonClickListener(Panel.BackBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.XuShenDianBtn, EnterXuShenDian);
			AddButtonClickListener(Panel.MuCangShanBtn, EnterXuShenTa);
			AddButtonClickListener(Panel.XuShenJiTanBtn, EnterXuShenJiTan);
		}

		private void EnterXuShenDian()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XuShenDianWnd.prefab", true);
		}

		private void EnterXuShenTa()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XuShenTaWnd.prefab", true);
		}

		private void EnterXuShenJiTan()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XuShenJiTanWnd.prefab", true);
		}

		private void EnterHeDaoTai()
		{
			Singleton<UIManager>.Instance.PopUpWnd("HeDaoTaiWnd.prefab", true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
