using System;

namespace XiuZhenJie
{
	public class FaZhenXiaoHaoLingShiWnd : Window
	{
		public FaZhenXiaoHaoLingShiPanel _panel;

		public Action<bool> JieSuoActionEvent;

		private long LingShiValue;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<FaZhenXiaoHaoLingShiPanel>();
			AddButtonClickListener(_panel.CloseBtn, () =>
			{
				if (JieSuoActionEvent != null)
				{
					JieSuoActionEvent(obj: false);
				}
				Singleton<UIManager>.Instance.CloseWnd("FaZhenXiaoHaoLingShiWnd.prefab");
			});
			AddButtonClickListener(_panel.QueRenBtn, QueRenBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			long num = (LingShiValue = long.Parse(paralist[0]?.ToString()));
			_panel.LingShiText.text = CommonMethod.HuoBiLongToString(num, 4);
			_panel.ShengYuLingShiText.text = ((num > Singleton<RoleManager>.Instance.Role_Entity.ShinyStone) ? "0" : (Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - num).ToString());
		}

		public void QueRenBtnClick()
		{
			if (JieSuoActionEvent != null)
			{
				JieSuoActionEvent(obj: true);
			}
			Singleton<UIManager>.Instance.CloseWnd("FaZhenXiaoHaoLingShiWnd.prefab");
		}
	}
}
