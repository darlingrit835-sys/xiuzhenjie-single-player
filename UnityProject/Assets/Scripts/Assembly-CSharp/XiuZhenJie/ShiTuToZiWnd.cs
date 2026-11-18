using Google.Protobuf;

namespace XiuZhenJie
{
	public class ShiTuToZiWnd : Window
	{
		private ShiTuToZiPanel m_Panel;

		private string m_Id;

		private ShiTuToZiList m_DangQianShiTuToZi;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ShiTuToZiPanel>();
			if (paralist != null)
			{
				m_DangQianShiTuToZi = (ShiTuToZiList)paralist[0];
				PanDuan();
				if (m_DangQianShiTuToZi != ShiTuToZiList.QingAn)
				{
					m_Id = (string)paralist[1];
				}
			}
			else
			{
				GuanBi();
			}
		}

		private void PanDuan()
		{
			switch (m_DangQianShiTuToZi)
			{
			case ShiTuToZiList.QingAn:
				QingAn();
				break;
			case ShiTuToZiList.ChuShi:
				ChuShi();
				break;
			case ShiTuToZiList.ZhuChu:
				ZhuChu();
				break;
			case ShiTuToZiList.ChuangGong:
				ChuangGong();
				break;
			case ShiTuToZiList.ToMai:
				ToMai();
				break;
			}
		}

		private void QingAn()
		{
			m_Panel.QingAnBgImage.gameObject.SetActive(value: true);
			AddButtonClickListener(m_Panel.QingAnGuanBiButton, GuanBi);
			AddButtonClickListener(m_Panel.QingAnQueRenButton, QingAnJiaoHu);
		}

		private void QingAnJiaoHu()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.QING_AN, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					QingAnResp_1014015 qingAnResp_ = new QingAnResp_1014015();
					qingAnResp_.MergeFrom(byteString);
					if (qingAnResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab")?.Intimecy(qingAnResp_.Intimacy);
						GuanBi();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(qingAnResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void ChuShi()
		{
			m_Panel.ChuShiBgImage.gameObject.SetActive(value: true);
			AddButtonClickListener(m_Panel.ChuShiGuanBiButton, GuanBi);
			AddButtonClickListener(m_Panel.ChuShiQueRenButton, ChuShiJiaoHu);
		}

		private void ChuShiJiaoHu()
		{
			ChuShiReq_1014027 data = new ChuShiReq_1014027
			{
				RoleId = m_Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHU_SHI, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ChuShiResp_1014027 chuShiResp_ = new ChuShiResp_1014027();
					chuShiResp_.MergeFrom(byteString);
					if (chuShiResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab")?.HuoQuShiFuXIngXi();
						Singleton<RoleManager>.Instance.Role_Entity.QiYun = chuShiResp_.QiYun;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						GuanBi();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(chuShiResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void ZhuChu()
		{
			m_Panel.ZhuChuBgImage.gameObject.SetActive(value: true);
			AddButtonClickListener(m_Panel.ZhuChuGuanBiButton, GuanBi);
			AddButtonClickListener(m_Panel.ZhuChuQueRenButton, ZhuChuJiaoHu);
		}

		private void ZhuChuJiaoHu()
		{
			ZhuChuShiMenReq_1014026 data = new ZhuChuShiMenReq_1014026
			{
				RoleId = m_Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ZHU_CHU_SHI_MEN, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ZhuChuShiMenResp_1014026 zhuChuShiMenResp_ = new ZhuChuShiMenResp_1014026();
					zhuChuShiMenResp_.MergeFrom(byteString);
					if (zhuChuShiMenResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab")?.HuoQuShiFuXIngXi();
						GuanBi();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(zhuChuShiMenResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void ChuangGong()
		{
			m_Panel.ChuangGongBgImage.gameObject.SetActive(value: true);
			AddButtonClickListener(m_Panel.ChuangGongGuanBiButton, GuanBi);
			AddButtonClickListener(m_Panel.ChuangGongQueRenButton, ChuangGongJiaoHu);
		}

		private void ChuangGongJiaoHu()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor > 4)
			{
				ChuanGongReq_1014017 data = new ChuanGongReq_1014017
				{
					RoleId = m_Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.CHAN_GONG, data, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						ChuanGongResp_1014017 chuanGongResp_ = new ChuanGongResp_1014017();
						chuanGongResp_.MergeFrom(byteString);
						if (chuanGongResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab")?.HuoQuShiFuXIngXi();
							Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = chuanGongResp_.CurrentVigor;
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							GuanBi();
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(chuanGongResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}, isShowWaitUI: true);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前精力不足5点，无法传功！");
			}
		}

		private void ToMai()
		{
			m_Panel.ToMaiBgImage.gameObject.SetActive(value: true);
			AddButtonClickListener(m_Panel.ToMaiGuanBiButton, GuanBi);
			AddButtonClickListener(m_Panel.ToMaiQueRenButton, ToMaiJiaoHu);
		}

		private void ToMaiJiaoHu()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor > 19)
			{
				TongMaiReq_1014016 data = new TongMaiReq_1014016
				{
					RoleId = m_Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.TONG_MAI, data, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						TongMaiResp_1014016 tongMaiResp_ = new TongMaiResp_1014016();
						tongMaiResp_.MergeFrom(byteString);
						if (tongMaiResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab")?.HuoQuShiFuXIngXi();
							Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = tongMaiResp_.CurrentVigor;
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							GuanBi();
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(tongMaiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}, isShowWaitUI: true);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前精力不足20点，无法通脉！");
			}
		}

		private void GuanBi()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
