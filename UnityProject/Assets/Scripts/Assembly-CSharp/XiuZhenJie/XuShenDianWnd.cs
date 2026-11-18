using System.Collections.Generic;

namespace XiuZhenJie
{
	public class XuShenDianWnd : Window
	{
		public XuShenDianPanel _panel;

		public Queue<string> MessageRadioQueue = new Queue<string>();

		private bool isRolling;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<XuShenDianPanel>();
			AddButtonClickListener(_panel.FanHui, FanHuiBtnClick);
			AddButtonClickListener(_panel.DST_btn, DanShiTaEnter);
			AddButtonClickListener(_panel.WFG_btn, WanFaGeEnter);
			AddButtonClickListener(_panel.SFH_btn, ShenFuHuiEnter);
			AddButtonClickListener(_panel.LZXH_btn, LingZhiXieHuiEnter);
			AddButtonClickListener(_panel.ZFXH_btn, ZhenFaXieHuiEnter);
			AddButtonClickListener(_panel.LianQI, LianQIXieHuiEnter);
			AddButtonClickListener(_panel.DJSD_btn, DaoJingShengDianEnter);
		}

		private void DanShiTaEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DanShiTaWnd.prefab", true);
		}

		private void WanFaGeEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("WanFaGeWnd.prefab", true);
		}

		private void DaoJingShengDianEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DaoJingShengDianWnd.prefab", true);
		}

		private void ShenFuHuiEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("SFH_ShenFuHuiWnd.prefab", true);
		}

		private void LingZhiXieHuiEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_MianWnd.prefab", true);
		}

		private void ZhenFaXieHuiEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZS_MianWnd.prefab", true);
		}

		private void LianQIXieHuiEnter()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LianQi_ChuangZaoMianWnd.prefab", true);
		}

		private void FanHuiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
