namespace XiuZhenJie
{
	public class QiZhenGeChooseWnd : Window
	{
		public QiZhenGeChoosePanel Panel;

		private StoreType m_CurrentStoreType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<QiZhenGeChoosePanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.WoDeJingPaiBtn, WoDeJingPaiClick);
			AddButtonClickListener(Panel.WoDePaiMaiBtn, WoDePaiMaiClick);
			if (paralist != null && paralist.Length == 1)
			{
				m_CurrentStoreType = (StoreType)paralist[0];
			}
		}

		private void WoDePaiMaiClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QZG_WoDePaiMaiWnd.prefab", true, m_CurrentStoreType);
			CloseWnd();
		}

		private void WoDeJingPaiClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QZG_WoDeJingPaiWnd.prefab", true);
			CloseWnd();
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
