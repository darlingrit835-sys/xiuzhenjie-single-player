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
			Panel = base.GameObject.GetComponent<DuiHuanLingShiNewPanel>();
			InkCurrencyArt.StyleExchange(Panel);
			Panel.KeDuihHuan.text = "正在查询汇率…";
			Panel.ZoLiang.text = Panel.TimeGenXing.text = string.Empty;
			Panel.DuiHuan.interactable = false;
			DanJia();
			ShuXing();
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
						Panel.KeDuihHuan.text = $"1 灵玉  =  {searchShinyStoneRatioResp_.ExchangeRatio:N0} 灵石";
						m_DanJia = searchShinyStoneRatioResp_.ExchangeRatio;
						Panel.ZoLiang.text = $"可兑库存　{searchShinyStoneRatioResp_.ExchangeShinyStoneSum:N0} 灵石";
						m_ZoShu = searchShinyStoneRatioResp_.ExchangeShinyStoneSum;
						string arg = CommonMethod.GetTimeStampTime((searchShinyStoneRatioResp_.UpdateAt / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm");
						Panel.TimeGenXing.text = $"更新于 {arg}";
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
			Panel.ShuLianDuZengJia.text = $"预计获得　{m_LingYuShu * m_DanJia:N0} 灵石";
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
			Panel.DuiHuan.interactable = m_LingYuShu > 0;
			DanJia();
		}

		private void LingYuShu(string count)
		{
			if (!string.IsNullOrEmpty(Panel.LingYuShu.text))
			{
				string text = Panel.LingYuShu.text;
				if (!int.TryParse(text, out m_LingYuShu))
					m_LingYuShu = 0;
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
						m_LingYuShu = 0;
						ShuXing();
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
