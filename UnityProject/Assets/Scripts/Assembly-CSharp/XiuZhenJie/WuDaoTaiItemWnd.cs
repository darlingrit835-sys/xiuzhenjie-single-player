namespace XiuZhenJie
{
	public class WuDaoTaiItemWnd : Window
	{
		public WuDaoTaiIXiangQingPanel Panel;

		private WuDaoTaiWnd m_WuDaoTaiWnd;

		private GongFaEntity m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WuDaoTaiIXiangQingPanel>();
			m_WuDaoTaiWnd = paralist[0] as WuDaoTaiWnd;
			m_DataSource = paralist[1] as GongFaEntity;
			GenXing();
		}

		public void GenXing()
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			Panel.GoFaName.text = m_DataSource.Name;
			Panel.JingLiZhi.text = role_Entity.CurrentVigor.ToString();
			Panel.JingLiZhiNew.text = (role_Entity.CurrentVigor - 10).ToString();
			Panel.ZhouTianShu.text = m_DataSource.Count.ToString();
			if (m_DataSource.Count + role_Entity.RoleLevel * 2 >= m_DataSource.UpCount)
			{
				if (m_DataSource.AchievementLevel < m_DataSource.MaxAchievementLevel)
				{
					Panel.ZhouTianShuNew.text = (m_DataSource.Count + role_Entity.RoleLevel * 2 - m_DataSource.UpCount).ToString();
				}
				else
				{
					Panel.ZhouTianShuNew.text = (m_DataSource.Count + role_Entity.RoleLevel * 2).ToString();
				}
			}
			else
			{
				Panel.ZhouTianShuNew.text = (m_DataSource.Count + role_Entity.RoleLevel * 2).ToString();
			}
			AddButtonClickListener(Panel.TuiChu, TuiChuBut);
			AddButtonClickListener(Panel.QueRen, QueRenBut);
		}

		private void TuiChuBut()
		{
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiIXiangQingWnd.prefab");
		}

		private void QueRenBut()
		{
		}
	}
}
