namespace XiuZhenJie
{
	public class ZhanFaShiXIangQingWnd : Window
	{
		public ZhanFaShiXiangQingPanel Panel;

		private ZhanFaShiXianQingGoMai FashuDanGe;

		private ZhanFaShiItem zhanFaShi;

		private int m_CiShu = 1;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZhanFaShiXiangQingPanel>();
			FashuDanGe = paralist[0] as ZhanFaShiXianQingGoMai;
			zhanFaShi = paralist[1] as ZhanFaShiItem;
			FuZhi();
			AddButtonClickListener(Panel.JiaHao, JiaHaoXiaoGuo);
			AddButtonClickListener(Panel.JianHao, JianHaoXiaoGuo);
			AddButtonClickListener(Panel.QuXiao, QuXiaoXiaoGuo);
			AddButtonClickListener(Panel.QueRen, QueRenXiaoGuo);
			Panel.CiShu.onEndEdit.AddListener(SrCiShu);
		}

		private void SrCiShu(string count)
		{
			if (!string.IsNullOrEmpty(Panel.CiShu.text))
			{
				string text = Panel.CiShu.text;
				m_CiShu = int.Parse(text);
			}
			else
			{
				m_CiShu = 1;
			}
			XiaoGuo(m_CiShu);
		}

		private void FuZhi()
		{
			Panel.FaShuName.text = FashuDanGe.Name;
			Panel.ShuLianDu.text = FashuDanGe.ShuLianDu;
			Panel.ShuLianLevel.text = FashuDanGe.ShuLianLevel;
			Panel.ShiFaTime.text = FashuDanGe.ShiFaTime;
			XiaoGuo(m_CiShu);
		}

		private void XiaoGuo(int ciShu)
		{
			Panel.CiShu.text = ciShu.ToString();
			Panel.JingLIXiaoHao.text = $"消耗： {ciShu}点精力";
			Panel.ShuLianDuZengJia.text = $"增长: {ciShu * 5} 点熟练 ";
		}

		private void JiaHaoXiaoGuo()
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			m_CiShu++;
			if (role_Entity.CurrentVigor < m_CiShu)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("已超过人物当前最大精力值！");
				m_CiShu = role_Entity.CurrentVigor;
			}
			XiaoGuo(m_CiShu);
		}

		private void JianHaoXiaoGuo()
		{
			m_CiShu--;
			if (m_CiShu <= 0)
			{
				m_CiShu = 0;
			}
			XiaoGuo(m_CiShu);
		}

		private void QueRenXiaoGuo()
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			if (role_Entity.CurrentVigor == 0)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("最低输入值为1");
				XiaoGuo(1);
				m_CiShu = 1;
			}
			else if (role_Entity.CurrentVigor >= m_CiShu)
			{
				zhanFaShi.DianJiShiJian(zhanFaShi.FashuDanGe.id, m_CiShu);
				Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiXiangQingWnd.prefab");
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow($"当前精力不够，还差:{m_CiShu - role_Entity.CurrentVigor}点");
				XiaoGuo(role_Entity.CurrentVigor);
				m_CiShu = role_Entity.CurrentVigor;
			}
		}

		private void QuXiaoXiaoGuo()
		{
			Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiXiangQingWnd.prefab");
		}
	}
}
