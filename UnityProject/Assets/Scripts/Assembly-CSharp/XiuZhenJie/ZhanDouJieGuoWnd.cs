using System.Collections.Generic;

namespace XiuZhenJie
{
	public class ZhanDouJieGuoWnd : Window
	{
		public ZhanDouJieGuoPanel _panel;

		private Dictionary<int, List<BaseNaJieGoods>> dataSource;

		private List<BaseNaJieGoods> m_AllDiaoLuo;

		private EndBattleInfoModel endBattleSource;

		private bool isSuccess;

		private string m_DungeonId;

		private int roleIndex;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ZhanDouJieGuoPanel>();
			AddButtonClickListener(_panel.LingQuButton, LingQuJiangLi);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			dataSource = new Dictionary<int, List<BaseNaJieGoods>>();
			if (paralist.Length >= 2)
			{
				roleIndex = 0;
				int.TryParse(paralist[0].ToString(), out roleIndex);
				m_DungeonId = paralist[1].ToString();
			}
		}

		private void CloseShengLi()
		{
			Singleton<UIManager>.Instance.CloseWnd("ZhaoDouJieGuo.prefab");
		}

		private void LingQuJiangLi()
		{
			RoleLeaveDungeonReq_8001008 data = new RoleLeaveDungeonReq_8001008
			{
				DungeonId = m_DungeonId,
				RoleIndex = roleIndex
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_LEAVE_DUNGEON, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, () =>
				{
					Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
				});
			}, isShowWaitUI: true, ServerType.BattleServer);
		}
	}
}
