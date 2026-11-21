using System;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class XuanTianLingWnd : Window
	{
		public XuanTianLingPanel xuanTianLingPanel;

		private int JiaGe = 450;

		private MainCityWnd m_MainCityWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			xuanTianLingPanel = base.GameObject.GetComponent<XuanTianLingPanel>();
			m_MainCityWnd = paralist[0] as MainCityWnd;
			xuanTianLingPanel.TianShu.text = Singleton<RoleManager>.Instance.Role_Entity.YueKaDays + "天";
			AddButtonClickListener(xuanTianLingPanel.TuiChu, CloseThisWnd);
			AddButtonClickListener(xuanTianLingPanel.GoMai, GoMai);
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			xuanTianLingPanel.TianShu.text = Singleton<RoleManager>.Instance.Role_Entity.YueKaDays + "天";
		}

		private void GoMai()
		{
			if (CommonMethod.getTimeSpan(DateTime.Now.ToLocalTime(), "06:00", "07:00"))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请勿在每天清晨06:00-07:00购买玄天令");
			}
			else if (Singleton<RoleManager>.Instance.ShengYuLingYu(JiaGe) >= 0.0)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_BUY_YUEKA, null, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						BuyYuekaResp_1001004 buyYuekaResp_ = new BuyYuekaResp_1001004();
						buyYuekaResp_.MergeFrom(byteString);
						if (buyYuekaResp_.IsSuccess)
						{
							Singleton<RoleManager>.Instance.Role_Entity.YueKaDays = buyYuekaResp_.RemainingDay;
							Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = buyYuekaResp_.CurrentJade;
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							m_MainCityWnd.YueKaTianShu();
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buyYuekaResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}, isShowWaitUI: true);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("您的灵玉不足，无法购买");
			}
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("XuanTianLingWnd.prefab");
		}
	}
}
