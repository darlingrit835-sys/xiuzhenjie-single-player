using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZongLanWnd : Window
	{
		public ZongLanPanel Panel;

		private ZongLanInfoModel m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongLanPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseBtn);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.ACUPOINT_DETAIL, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AcupointDetailResp_1005020 acupointDetailResp_ = new AcupointDetailResp_1005020();
					acupointDetailResp_.MergeFrom(byteString);
					if (acupointDetailResp_.IsSuccess)
					{
						m_DataSource = new ZongLanInfoModel(acupointDetailResp_);
						JingMaiShouYiModel shouYi = new JingMaiShouYiModel(acupointDetailResp_);
						Init(shouYi);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(acupointDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void Init(JingMaiShouYiModel _shouYi)
		{
			if (m_DataSource != null && _shouYi != null)
			{
				Panel.DuMaiNumText.text = $"{m_DataSource.a}/{m_DataSource.i}";
				Panel.RenMaiText.text = $"{m_DataSource.b}/{m_DataSource.j}";
				Panel.ChongMaiText.text = $"{m_DataSource.c}/{m_DataSource.k}";
				Panel.YangQiaoMaiText.text = $"{m_DataSource.e}/{m_DataSource.m}";
				Panel.YinQiaoMaiText.text = $"{m_DataSource.f}/{m_DataSource.n}";
				Panel.YangWeiMaiText.text = $"{m_DataSource.g}/{m_DataSource.o}";
				Panel.YinWeiMaiText.text = $"{m_DataSource.h}/{m_DataSource.p}";
				Panel.DaiMaiText.text = $"{m_DataSource.d}/{m_DataSource.l}";
				Panel.Extra_NaQiXiaoLvText.text = (_shouYi.a - 100.0).ToString("0.00") + "%";
				Panel.Extra_NaQiSuLvText.text = (100.0 - _shouYi.b).ToString("0.00") + "%";
				Panel.Extra_JingXueShangXianText.text = (_shouYi.g - 100.0).ToString("0.00") + "%";
				Panel.Extra_WuXingShangHaiText.text = (_shouYi.c - 100.0).ToString("0.00") + "%";
				Panel.Extra_ShenShiShangXianText.text = (_shouYi.e - 100.0).ToString("0.00") + "%";
				Panel.Extra_WuXingFangYuText.text = (_shouYi.d - 100.0).ToString("0.00") + "%";
				Panel.Extra_TiPoShangXianText.text = (_shouYi.f - 100.0).ToString("0.00") + "%";
				Panel.Extra_JingXueHuiFuText.text = (_shouYi.h - 100.0).ToString("0.00") + "%";
			}
		}

		private void CloseBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
