namespace XiuZhenJie
{
	public class ZhongJieFangShiWnd : Window
	{
		public ZhongJieFangShiPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZhongJieFangShiPanel>();
			AddButtonClickListener(Panel.LiKai_Btn, LiKaiBtnClick);
			AddButtonClickListener(Panel.ShenFuGe_Btn, ShenFuGeClick);
			AddButtonClickListener(Panel.LingDanFang_Btn, LingDanFangBtnClick);
			AddButtonClickListener(Panel.BaiQiFang_Btn, BaiQiFangBtnClick);
			AddButtonClickListener(Panel.ZaHuoPu_Btn, CaiLiaoPaiMaiBtnClick);
			AddButtonClickListener(Panel.DaoJingGe_Btn, GongFaPaiMaiBtnClick);
			AddButtonClickListener(Panel.ShuFaGe_Btn, FaShuPaiMaiBtnClick);
			AddButtonClickListener(Panel.QiZhenGe_Btn, QiZhenGeBtnClick);
			AddButtonClickListener(Panel.ZhenFaGe_Btn, XuanZhenGeBtnClick);
			ZhongJieFangShiPanDuan();
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ZhongJieFangShiWnd.prefab");
		}

		public void ShenFuGeClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.FuLu);
		}

		public void LingDanFangBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.DanYao);
		}

		public void BaiQiFangBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.FaQi);
		}

		public void LingShouPaiMaiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.LingShou);
		}

		public void CaiLiaoPaiMaiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.QiTa);
		}

		public void GongFaPaiMaiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.GongFa);
		}

		public void FaShuPaiMaiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.FaShu);
		}

		public void QiZhenGeBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("AH_MainSceneWnd.prefab", true);
		}

		public void XuanZhenGeBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.ZhenFa);
		}

		public void QiQiaoBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("QiZhenGeWnd.prefab", true, StoreType.QiQiao);
		}

		private void ZhongJieFangShiPanDuan()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.ShopGuideVal == 1)
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 5;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
				Singleton<GuideManager>.Instance.ShowGuideNew(5);
			}
		}
	}
}
