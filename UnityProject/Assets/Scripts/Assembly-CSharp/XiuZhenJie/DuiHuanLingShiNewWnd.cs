using Google.Protobuf;

namespace XiuZhenJie
{
	public class DuiHuanLingShiNewWnd : Window
	{
		public DuiHuanLingShiNewPanel Panel;

		private int m_LingYuShu;

		private long m_DanJia;

		private long m_ZoShu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			ShuXing();
			Panel = base.GameObject.GetComponent<DuiHuanLingShiNewPanel>();
			AddButtonClickListener(Panel.TuChu, TuChu);
			AddButtonClickListener(Panel.ShuaXin, ShuaXin);
			AddButtonClickListener(Panel.JiaHao, JiaHaoXiaoGuo);
			AddButtonClickListener(Panel.JianHao, JianHaoXiaoGuo);
			AddButtonClickListener(Panel.DuiHuan, DuiHuan);
			Panel.LingYuShu.onEndEdit.AddListener(LingYuShu);
		}

		private void ShuXing()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.Search_Shiny_Stone_RatioResp, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchShinyStoneRatioResp_1002016 searchShinyStoneRatioResp_ = new SearchShinyStoneRatioResp_1002016();
					searchShinyStoneRatioResp_.MergeFrom(byteString);
					if (searchShinyStoneRatioResp_.IsSuccess)
					{
						string text = $"当前 1 灵玉可兑换灵石: {searchShinyStoneRatioResp_.ExchangeRatio}";
						Panel.KeDuihHuan.text = text;
						m_DanJia = searchShinyStoneRatioResp_.ExchangeRatio;
						string text2 = $"可兑换灵石总量: {searchShinyStoneRatioResp_.ExchangeShinyStoneSum}";
						Panel.ZoLiang.text = text2;
						m_ZoShu = searchShinyStoneRatioResp_.ExchangeShinyStoneSum;
						string arg = CommonMethod.GetTimeStampTime((searchShinyStoneRatioResp_.UpdateAt / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm");
						string text3 = $"汇率更新时间:  {arg}";
						Panel.TimeGenXing.text = text3;
						PanDU();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchShinyStoneRatioResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void DanJia()
		{
			Panel.LingYuShu.text = m_LingYuShu.ToString();
			Panel.ShuLianDuZengJia.text = $"可兑换 {m_LingYuShu * m_DanJia} 灵石";
		}

		private void TuChu()
		{
			Singleton<UIManager>.Instance.CloseWnd("DuiHuanLingShiNewWnd.prefab");
		}

		private void ShuaXin()
		{
			ShuXing();
			PanDU();
		}

		private void JiaHaoXiaoGuo()
		{
			m_LingYuShu += 10;
			PanDU();
		}

		private void JianHaoXiaoGuo()
		{
			m_LingYuShu -= 10;
			PanDU();
		}

		private void PanDU()
		{
			double num = Singleton<RoleManager>.Instance.ShengYuLingYu(m_LingYuShu);
			Panel.NoduiHuan.interactable = false;
			if (num < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前灵玉不足");
				m_LingYuShu = 0;
			}
			else if (m_LingYuShu < 10)
			{
				m_LingYuShu = 0;
			}
			else if (m_LingYuShu * m_DanJia > m_ZoShu)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("超过当前可兑换灵石总数，请重新输入！");
				m_LingYuShu = 0;
			}
			else if (m_ZoShu < 1 || m_DanJia < 1)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前无灵石可兑换！");
				m_LingYuShu = 0;
			}
			else
			{
				Panel.DuiHuan.interactable = true;
			}
			DanJia();
		}

		private void LingYuShu(string count)
		{
			if (!string.IsNullOrEmpty(Panel.LingYuShu.text))
			{
				string text = Panel.LingYuShu.text;
				m_LingYuShu = int.Parse(text);
			}
			else
			{
				m_LingYuShu = 10;
			}
			PanDU();
		}

		private void DuiHuan()
		{
			PanDU();
			if (m_LingYuShu == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("兑换失败，请重新输入！");
			}
			else
			{
				DuiHuanLingYu();
			}
		}

		private void DuiHuanLingYu()
		{
			ShinyJadeExchangeShinyStoneReq_1002017 data = new ShinyJadeExchangeShinyStoneReq_1002017
			{
				ShinyJade = m_LingYuShu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ShinyJadeExchangeShinyStoneReq, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ShinyJadeExchangeShinyStoneResp_1002017 shinyJadeExchangeShinyStoneResp_ = new ShinyJadeExchangeShinyStoneResp_1002017();
					shinyJadeExchangeShinyStoneResp_.MergeFrom(byteString);
					if (shinyJadeExchangeShinyStoneResp_.IsSuccess)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("兑换成功!");
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = shinyJadeExchangeShinyStoneResp_.ShinyJade;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = shinyJadeExchangeShinyStoneResp_.ShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(shinyJadeExchangeShinyStoneResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}
	}
}
