using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DongFuZhiFuWnd : Window
	{
		public DongFuZhiFuPanel Panel;

		private UnityAction QueRenAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuZhiFuPanel>();
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtn);
			AddButtonClickListener(Panel.QuXiaoBtn, QuXiaoBtn);
			if (paralist.Length >= 3)
			{
				Panel.LingYuValueText.text = paralist[0].ToString();
				Panel.XiaoFeiHouShengYuText.text = $"消耗后还剩余{paralist[1].ToString()}灵玉";
				QueRenAction = paralist[2] as UnityAction;
			}
		}

		private void QueRenBtn()
		{
			if (QueRenAction != null)
			{
				QueRenAction();
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QuXiaoBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
