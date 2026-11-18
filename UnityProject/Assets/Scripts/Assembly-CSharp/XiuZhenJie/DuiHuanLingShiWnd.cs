using UnityEngine;

namespace XiuZhenJie
{
	public class DuiHuanLingShiWnd : Window
	{
		public DuiHuanLingShiPanel duiHuanLingShiPanel;

		public long LingShiValue;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			LingShiValue = 0L;
			duiHuanLingShiPanel = base.GameObject.GetComponent<DuiHuanLingShiPanel>();
			AddButtonClickListener(duiHuanLingShiPanel.CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("DuiHuanLingShiWnd.prefab");
			});
			AddButtonClickListener(duiHuanLingShiPanel.JiaBtn, JiaBtnClick);
			AddButtonClickListener(duiHuanLingShiPanel.JianBtn, JianBtnClick);
			AddButtonClickListener(duiHuanLingShiPanel.QueRenBtn, QueRenBtnClick);
		}

		public void JiaBtnClick()
		{
			LingShiValue += 1000L;
			duiHuanLingShiPanel.LingShiValue.text = CommonMethod.HuoBiLongToString(LingShiValue, 4);
		}

		public void JianBtnClick()
		{
			if (LingShiValue >= 1000)
			{
				LingShiValue -= 1000L;
			}
			duiHuanLingShiPanel.LingShiValue.text = CommonMethod.HuoBiLongToString(LingShiValue, 4);
		}

		public void QueRenBtnClick()
		{
			Debug.Log("确认");
		}
	}
}
