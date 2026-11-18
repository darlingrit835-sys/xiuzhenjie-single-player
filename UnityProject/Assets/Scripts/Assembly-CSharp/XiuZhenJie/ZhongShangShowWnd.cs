namespace XiuZhenJie
{
	public class ZhongShangShowWnd : Window
	{
		public ZhongShangShowPanel Panel;

		private int m_MaxAge;

		public int m_BrokenAge;

		private BrokenStateResp_1005012 m_BrokenStateResp_1005012;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZhongShangShowPanel>();
			AddButtonClickListener(Panel.FangQiBtn, CloseWindow);
			AddButtonClickListener(Panel.JingXueTitleButton, JingXueTitlXiaoWenHao);
			AddButtonClickListener(Panel.ZhenYuanTitleTButton, ZhenYuanTitleTXiaoWenHao);
			AddButtonClickListener(Panel.TiPoTitleButton, TiPoTitleXiaoWenHao);
			AddButtonClickListener(Panel.ShenShiTitleButton, ShenShiTitleXiaoWenHao);
			AddButtonClickListener(Panel.ShouYuanTitleButton, ShouYuanTitleXiaoWenHao);
			m_BrokenStateResp_1005012 = paralist[0] as BrokenStateResp_1005012;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Init();
		}

		private void Init()
		{
			Panel.ZhenYuanOldText.text = m_BrokenStateResp_1005012.SuperAir.ToString("0.00");
			Panel.TiPoOldText.text = m_BrokenStateResp_1005012.Physique.ToString("0.00");
			Panel.ShenShiOldText.text = m_BrokenStateResp_1005012.Perception.ToString("0.00");
			Panel.JingXueOldText.text = m_BrokenStateResp_1005012.PureBlood.ToString("0.00");
			Panel.ZhenYuanNewText.text = m_BrokenStateResp_1005012.CurrentSuperAir.ToString("0.00");
			Panel.TiPoNewText.text = m_BrokenStateResp_1005012.CurrentPhysique.ToString("0.00");
			Panel.ShenShiNewText.text = m_BrokenStateResp_1005012.CurrentPerception.ToString("0.00");
			Panel.JingXueNewText.text = m_BrokenStateResp_1005012.CurrentPureBlood.ToString("0.00");
			Panel.JingXueDownIcon.gameObject.SetActive(m_BrokenStateResp_1005012.CurrentPureBlood < m_BrokenStateResp_1005012.PureBlood);
			Panel.TiPoDownIcon.gameObject.SetActive(m_BrokenStateResp_1005012.CurrentPhysique < m_BrokenStateResp_1005012.Physique);
			Panel.ShenShiDownIcon.gameObject.SetActive(m_BrokenStateResp_1005012.CurrentPerception < m_BrokenStateResp_1005012.Perception);
			Panel.ZhenQiDownIcon.gameObject.SetActive(m_BrokenStateResp_1005012.CurrentSuperAir < m_BrokenStateResp_1005012.SuperAir);
			Panel.ShouYuanDownIcon.gameObject.SetActive(m_BrokenAge < m_MaxAge);
		}

		private void JingXueTitlXiaoWenHao()
		{
			string text = "锻体或者服用恢复精血的丹药，可以将受损的精血弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "精血");
		}

		private void ZhenYuanTitleTXiaoWenHao()
		{
			string text = "纳气或者服用恢复灵气的丹药，可以将受损的灵气弥补回来，若不移除重伤状态，将无法再次渡劫";
			string text2 = "灵气";
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 30 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				text2 = "真气";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 60 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 91)
			{
				text2 = "真元";
			}
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, text2);
		}

		private void TiPoTitleXiaoWenHao()
		{
			string text = "锻体或者服用恢复体魄的丹药，可以将受损的体魄弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "体魄");
		}

		private void ShenShiTitleXiaoWenHao()
		{
			string text = "炼神或者服用恢复神识的丹药，可以将受损的神识弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "神识");
		}

		private void ShouYuanTitleXiaoWenHao()
		{
			string text = "服用恢复寿元的丹药，可以将受损的寿元弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "寿元");
		}
	}
}
