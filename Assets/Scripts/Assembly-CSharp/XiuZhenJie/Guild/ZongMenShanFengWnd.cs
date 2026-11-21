namespace XiuZhenJie
{
	public class ZongMenShanFengWnd : Window
	{
		private int m_CurrentBuildModle;

		private int m_HillIndex;

		private string m_HillName;

		private bool m_IsZongZhu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
		}

		private void CloseThisWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void SwitchBuildModle(bool isOn)
		{
		}

		private void ChengYuanGuanLiClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMenChengYuanWnd.prefab", false, true, m_IsZongZhu);
		}

		private void ShanFengDaDianClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_DaDianWnd.prefab", false, true, m_IsZongZhu);
		}
	}
}
