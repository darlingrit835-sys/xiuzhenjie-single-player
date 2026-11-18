namespace XiuZhenJie
{
	public class TuPoDuJieSuccessWnd : Window
	{
		public TuPoDuJieSuccessPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<TuPoDuJieSuccessPanel>();
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtn);
			_ = Singleton<RoleManager>.Instance.Role_Entity.MaxAge;
			if (paralist[0] is RoleInfoEntity roleInfoEntity)
			{
				Init(roleInfoEntity);
			}
		}

		private void Init(RoleInfoEntity roleInfoEntity)
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			Panel.ShouYuanOldText.text = roleInfoEntity.MaxAge.ToString("f0");
			Panel.ZhenYuanOldText.text = roleInfoEntity.SuperAir.ToString("f0");
			Panel.TiPoOldText.text = roleInfoEntity.Physique.ToString("f0");
			Panel.ShenShiOldText.text = roleInfoEntity.Perception.ToString("f0");
			Panel.JingXueOldText.text = roleInfoEntity.PureBlood.ToString("f0");
			Panel.ShouYuanNewText.text = role_Entity.MaxAge.ToString("f0");
			Panel.ZhenYuanNewText.text = role_Entity.SuperAir.ToString("f0");
			Panel.TiPoNewText.text = role_Entity.Physique.ToString("f0");
			Panel.ShenShiNewText.text = role_Entity.Perception.ToString("f0");
			Panel.JingXueNewText.text = role_Entity.PureBlood.ToString("f0");
			TuPoYinDao(3);
		}

		private void QueRenBtn()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 31)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("恭喜您晋升元婴期，灵气将转变成为真气!", CloseThisWindow);
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 61)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("恭喜您晋升合体期，真气将转变成为真元!", CloseThisWindow);
			}
			else
			{
				CloseThisWindow();
			}
		}

		private void CloseThisWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("TuPoDuJieWnd.prefab");
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DongFuScene");
		}

		private void TuPoYinDao(int Shu)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 20 || Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 21)
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = Shu;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 9;
				Singleton<GuideManager>.Instance.ShowGuideNew(9);
			}
		}
	}
}
