using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DongFuRoomWnd : Window
	{
		public DongFuRoomPanel Panel;
		private ResidenceAtlasView atlas;
		private int level;
		private int readVersion;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = GameObject.GetComponent<DongFuRoomPanel>();
			atlas = new ResidenceAtlasView(Panel, this);
			AddButtonClickListener(Panel.MiShiBtn, () => Open("DongFuMiShiWnd.prefab"));
			AddButtonClickListener(Panel.LianGongFaBtn, LianGongFangBtn);
			AddButtonClickListener(Panel.WuDaoTaiBtn, WuDaoTaiBtn);
			AddButtonClickListener(Panel.DiHuoShiBtn, DiHuoGeBtn);
			AddButtonClickListener(Panel.LingTianBtn, LingTianBtn);
			AddButtonClickListener(Panel.ShengJi_Btn, () => Singleton<UIManager>.Instance.PopUpWnd("DongFuChooseWnd.prefab", true, level));
			AddButtonClickListener(Panel.LiKai_Btn, Leave);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			string roleId = DongFuResidenceState.CurrentRoleId;
			int version = ++readVersion;
			atlas.BeginRead();
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ENTER_DONGFU, null, bytes =>
			{
				if (GameObject == null || version != readVersion || roleId != DongFuResidenceState.CurrentRoleId)
					return;
				if (bytes == null)
				{
					atlas.ReadFailed();
					return;
				}
				var state = EnterHomeResp_1005001.Parser.ParseFrom(bytes);
				if (!state.IsSuccess)
				{
					atlas.ReadFailed();
					return;
				}
				ApplyResidenceState(DongFuResidenceState.ApplyRank(roleId, state.Rank), state.Name);
				atlas.SetOccupations(state.Live1Type, state.Live2Type);
			}))
				atlas.ReadFailed();
		}

		public override void OnDisable()
		{
			readVersion++;
			base.OnDisable();
		}

		internal void ApplyResidenceState(int currentLevel, string name)
		{
			level = currentLevel;
			atlas.ShowGrade(level, name);
		}

		private static void Open(string window)
		{
			Singleton<UIManager>.Instance.PopUpWnd(window, true);
		}

		public void LianGongFangBtn()
		{
			var ui = Singleton<UIManager>.Instance;
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				ui.CloseWnd(this);
				Open("DongFuWnd.prefab");
				return;
			}
			ui.CloseWnd("GuideNewWnd.prefab");
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DongFuScene");
		}

		public void WuDaoTaiBtn()
		{
			// The study shelf closes via its main HUD button. Use that same navigation path.
			var ui = Singleton<UIManager>.Instance;
			var hud = ui.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			var study = hud?._panel?.CanWu_btn;
			if (study == null)
				return;
			ui.CloseWnd(this);
			study.onClick.Invoke();
			// Closing the study tab should return to the atlas it was opened from.
			hud.AddButtonClickListener(study, hud.DongFuBtnClick);
		}

		public void DiHuoGeBtn()
		{
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DiHuoGe_LianDan", DiHuoGeEnterType.DongFu);
		}

		public void LingTianBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_LingTianMainWnd.prefab", true, level);
		}

		public UnityAction GetRoomClick(int type) => GetRoomClick((RoleFuZhiType)type);

		public UnityAction GetRoomClick(RoleFuZhiType type)
		{
			switch (type)
			{
				case RoleFuZhiType.LianDan: return DiHuoGeBtn;
				case RoleFuZhiType.LianQi: return () => Open("LianQiChooseWnd.prefab");
				case RoleFuZhiType.ZhiFu: return () => Open("HuaFuChooseWnd.prefab");
				case RoleFuZhiType.FaShu: return () => Open("ZhanFaShiWnd.prefab");
				case RoleFuZhiType.ZhenTu: return () => Open("ZF_CreateChooseWnd.prefab");
				case RoleFuZhiType.BuDao: return WuDaoTaiBtn;
				case RoleFuZhiType.LingZhi: return LingTianBtn;
				default: return null;
			}
		}

		private void Leave()
		{
			var title = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (title == null)
				Singleton<UIManager>.Instance.CloseWnd(this);
			else if (Singleton<GameMapManager>.Instance.CurrentMapName == "MainScene")
				title.CloseDongFuNoChangeSceneInMain();
			else
				title.CloseDongFuClick();
		}
	}
}
