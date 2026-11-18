using System;

namespace XiuZhenJie
{
	public class QiZhenGeItemWnd : Window
	{
		public QiZhenGeItem _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<QiZhenGeItem>();
			AddButtonClickListener(_panel.CanYuJingPai, CanYuJingPaiBtnClick);
		}

		private void ItemInformationBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QZG_ItmeInformation.prefab", true);
		}

		private void CanYuJingPaiBtnClick()
		{
			throw new NotImplementedException();
		}
	}
}
