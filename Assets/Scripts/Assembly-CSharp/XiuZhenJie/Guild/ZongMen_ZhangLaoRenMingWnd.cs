using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZongMen_ZhangLaoRenMingWnd : Window
	{
		private ZongMen_ZhangLaoRenMingPanel m_Panel;

		private string m_Id;

		private string m_Name;

		private string m_TiShenglevelvet;

		private int m_Levelvet;

		private ZongMenZhangLaoRoleEntity m_zongMenZhangLaoRoleEntity;

		private int m_Type;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_ZhangLaoRenMingPanel>();
			AddButtonClickListener(m_Panel.TuiChuButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.RenMingButton, RenMingBut);
			m_Id = (string)paralist[0];
			m_Name = (string)paralist[1];
			m_zongMenZhangLaoRoleEntity = (ZongMenZhangLaoRoleEntity)paralist[2];
			m_Levelvet = (m_zongMenZhangLaoRoleEntity.RoleLevel - 1) / 10 + 1;
			m_Panel.RenMingNameText.text = "";
			m_Panel.RenMingButton.interactable = false;
			GetGuildRoleListData();
			RenMing();
		}

		private void RenMing()
		{
			m_Panel.RenMingNameText.text = "任命" + m_Name + "为";
		}

		private void TiSheng()
		{
			m_Panel.TiShengText.text = m_TiShenglevelvet ?? "";
		}

		private void GetGuildRoleListData()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTERMING, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterRenMingResp_1015023 enterRenMingResp_ = new EnterRenMingResp_1015023();
					enterRenMingResp_.MergeFrom(byteString);
					if (enterRenMingResp_.IsSuccess)
					{
						ZhangLaoFuZHi(enterRenMingResp_);
						BangDing();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterRenMingResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZhangLaoFuZHi(EnterRenMingResp_1015023 hillZhangLaoInfo)
		{
			foreach (EnterRenMingResp_1015023.Types.HillZhangLaoInfoResp_1015024 item in hillZhangLaoInfo.HillZhangLaoInfo)
			{
				XuanZho(item.Type, item.RoleName);
			}
		}

		private void XuanZho(int type, string names)
		{
			if (string.IsNullOrEmpty(names))
			{
				names = "无";
			}
			switch ((ZhangLaoRenMing)type)
			{
			case ZhangLaoRenMing.执法长老:
				m_Panel.ZhiFaZhangLao.ZhangLaoMingCheng("执法长老");
				m_Panel.ZhiFaZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.内务长老:
				m_Panel.NeiWuZhangLao.ZhangLaoMingCheng("内务长老");
				m_Panel.NeiWuZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.外务长老:
				m_Panel.WaiWuZhangLao.ZhangLaoMingCheng("外务长老");
				m_Panel.WaiWuZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.矿脉长老:
				m_Panel.KuangMaiZhangLao.ZhangLaoMingCheng("矿脉长老");
				m_Panel.KuangMaiZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.护宗长老:
				m_Panel.HuZongZhangLao.ZhangLaoMingCheng("护宗长老");
				m_Panel.HuZongZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.灵田长老:
				m_Panel.LianQiZhangLao.ZhangLaoMingCheng("灵田长老");
				m_Panel.LianQiZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.内门长老:
				m_Panel.FuliZhangLao.ZhangLaoMingCheng("内门长老");
				m_Panel.FuliZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.冶炼长老:
				m_Panel.LingShoZhangLao.ZhangLaoMingCheng("冶炼长老");
				m_Panel.LingShoZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.炼丹长老:
				m_Panel.ZhenFaZhangLao.ZhangLaoMingCheng("炼丹长老");
				m_Panel.ZhenFaZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.炼器长老:
				m_Panel.LingYaoZhangLao.ZhangLaoMingCheng("炼器长老");
				m_Panel.LingYaoZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.御兽长老:
				m_Panel.WuZhiWeiZhangLao.ZhangLaoMingCheng("御兽长老");
				m_Panel.WuZhiWeiZhangLao.XianShi(names, type);
				break;
			case ZhangLaoRenMing.阵法长老:
				m_Panel.WuZhiWeiZhangLao11.ZhangLaoMingCheng("阵法长老");
				m_Panel.WuZhiWeiZhangLao11.XianShi(names, type);
				break;
			case ZhangLaoRenMing.制符长老:
				m_Panel.WuZhiWeiZhangLao12.ZhangLaoMingCheng("制符长老");
				m_Panel.WuZhiWeiZhangLao12.XianShi(names, type);
				break;
			case ZhangLaoRenMing.工坊长老:
				m_Panel.WuZhiWeiZhangLao13.ZhangLaoMingCheng("工坊长老");
				m_Panel.WuZhiWeiZhangLao13.XianShi(names, type);
				break;
			case ZhangLaoRenMing.宗主:
				m_Panel.WuZhiWeiZhangLao14.ZhangLaoMingCheng("宗主");
				m_Panel.WuZhiWeiZhangLao14.XianShi(names, type);
				break;
			case ZhangLaoRenMing.无:
				m_Panel.WuZhiWeiZhangLao15.ZhangLaoMingCheng("无");
				m_Panel.WuZhiWeiZhangLao15.XianShi(names, type);
				break;
			}
		}

		private void BangDing()
		{
			AddButtonClickListener(m_Panel.ZhiFaZhangLao.DianJiButton, ZhiFaZhangLao);
			AddButtonClickListener(m_Panel.NeiWuZhangLao.DianJiButton, NeiWuZhangLao);
			AddButtonClickListener(m_Panel.WaiWuZhangLao.DianJiButton, WaiWuZhangLao);
			AddButtonClickListener(m_Panel.KuangMaiZhangLao.DianJiButton, KuangMaiZhangLao);
			AddButtonClickListener(m_Panel.HuZongZhangLao.DianJiButton, HuZongZhangLao);
			AddButtonClickListener(m_Panel.LianQiZhangLao.DianJiButton, LianQiZhangLao);
			AddButtonClickListener(m_Panel.FuliZhangLao.DianJiButton, FuliZhangLao);
			AddButtonClickListener(m_Panel.LingShoZhangLao.DianJiButton, LingShoZhangLao);
			AddButtonClickListener(m_Panel.ZhenFaZhangLao.DianJiButton, ZhenFaZhangLao);
			AddButtonClickListener(m_Panel.LingYaoZhangLao.DianJiButton, LingYaoZhangLao);
			AddButtonClickListener(m_Panel.WuZhiWeiZhangLao.DianJiButton, WuZhiWeiZhangLao);
			AddButtonClickListener(m_Panel.WuZhiWeiZhangLao11.DianJiButton, WuZhiWeiZhangLao11);
			AddButtonClickListener(m_Panel.WuZhiWeiZhangLao12.DianJiButton, WuZhiWeiZhangLao12);
			AddButtonClickListener(m_Panel.WuZhiWeiZhangLao13.DianJiButton, WuZhiWeiZhangLao13);
			AddButtonClickListener(m_Panel.WuZhiWeiZhangLao14.DianJiButton, WuZhiWeiZhangLao14);
			AddButtonClickListener(m_Panel.WuZhiWeiZhangLao15.DianJiButton, WuZhiWeiZhangLao15);
		}

		private void ZhiFaZhangLao()
		{
			m_Type = m_Panel.ZhiFaZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void NeiWuZhangLao()
		{
			m_Type = m_Panel.NeiWuZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void WaiWuZhangLao()
		{
			m_Type = m_Panel.WaiWuZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void KuangMaiZhangLao()
		{
			m_Type = m_Panel.KuangMaiZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void HuZongZhangLao()
		{
			m_Type = m_Panel.HuZongZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void LianQiZhangLao()
		{
			m_Type = m_Panel.LianQiZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void FuliZhangLao()
		{
			m_Type = m_Panel.FuliZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void LingShoZhangLao()
		{
			m_Type = m_Panel.LingShoZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void ZhenFaZhangLao()
		{
			m_Type = m_Panel.ZhenFaZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void LingYaoZhangLao()
		{
			m_Type = m_Panel.LingYaoZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void WuZhiWeiZhangLao()
		{
			m_Type = m_Panel.WuZhiWeiZhangLao.m_Type;
			XuanZho(m_Type);
		}

		private void WuZhiWeiZhangLao11()
		{
			m_Type = m_Panel.WuZhiWeiZhangLao11.m_Type;
			XuanZho(m_Type);
		}

		private void WuZhiWeiZhangLao12()
		{
			m_Type = m_Panel.WuZhiWeiZhangLao12.m_Type;
			XuanZho(m_Type);
		}

		private void WuZhiWeiZhangLao13()
		{
			m_Type = m_Panel.WuZhiWeiZhangLao13.m_Type;
			XuanZho(m_Type);
		}

		private void WuZhiWeiZhangLao14()
		{
			m_Type = m_Panel.WuZhiWeiZhangLao14.m_Type;
			XuanZho(m_Type);
		}

		private void WuZhiWeiZhangLao15()
		{
			m_Type = m_Panel.WuZhiWeiZhangLao15.m_Type;
			XuanZho(m_Type);
		}

		private void XuanZho(int type)
		{
			YingCang();
			switch ((ZhangLaoRenMing)type)
			{
			case ZhangLaoRenMing.执法长老:
				m_Panel.ZhiFaZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加稳定系数{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.内务长老:
				m_Panel.NeiWuZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"结算降低薪俸发放数额{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.外务长老:
				m_Panel.WaiWuZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"提升悬赏任务成功率{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.矿脉长老:
				m_Panel.KuangMaiZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加矿脉产量{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.护宗长老:
				m_Panel.HuZongZhangLao.XuanZho(zhangLao: true);
				if (m_zongMenZhangLaoRoleEntity.Profession1 == 5)
				{
					m_TiShenglevelvet = $"防御力增加{m_zongMenZhangLaoRoleEntity.Profession1Stage}%";
				}
				else if (m_zongMenZhangLaoRoleEntity.Profession2 == 5)
				{
					m_TiShenglevelvet = $"防御力增加{m_zongMenZhangLaoRoleEntity.Profession2Stage}%";
				}
				else
				{
					m_TiShenglevelvet = "防御力增加0%";
				}
				break;
			case ZhangLaoRenMing.灵田长老:
				m_Panel.LianQiZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加灵植产量{m_zongMenZhangLaoRoleEntity.PlantLiveStage}%";
				break;
			case ZhangLaoRenMing.内门长老:
				m_Panel.FuliZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"提升内门{m_Levelvet}%战力";
				break;
			case ZhangLaoRenMing.冶炼长老:
				m_Panel.LingShoZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加灵矿产量{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.炼丹长老:
				m_Panel.ZhenFaZhangLao.XuanZho(zhangLao: true);
				if (m_zongMenZhangLaoRoleEntity.Profession1 == 1)
				{
					m_TiShenglevelvet = $"增加丹药产量{m_zongMenZhangLaoRoleEntity.Profession1Stage}%";
				}
				else if (m_zongMenZhangLaoRoleEntity.Profession2 == 1)
				{
					m_TiShenglevelvet = $"增加丹药产量{m_zongMenZhangLaoRoleEntity.Profession2Stage}%";
				}
				else
				{
					m_TiShenglevelvet = "增加丹药产量0%";
				}
				break;
			case ZhangLaoRenMing.炼器长老:
				m_Panel.LingYaoZhangLao.XuanZho(zhangLao: true);
				if (m_zongMenZhangLaoRoleEntity.Profession1 == 2)
				{
					m_TiShenglevelvet = $"提升炼器产量{m_zongMenZhangLaoRoleEntity.Profession1Stage}%";
				}
				else if (m_zongMenZhangLaoRoleEntity.Profession2 == 2)
				{
					m_TiShenglevelvet = $"提升炼器产量{m_zongMenZhangLaoRoleEntity.Profession2Stage}%";
				}
				else
				{
					m_TiShenglevelvet = "提升炼器产量0%";
				}
				break;
			case ZhangLaoRenMing.御兽长老:
				m_Panel.WuZhiWeiZhangLao.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加灵矿产量{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.阵法长老:
				m_Panel.WuZhiWeiZhangLao11.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加阵盘产量{m_Levelvet}%";
				if (m_zongMenZhangLaoRoleEntity.Profession1 == 5)
				{
					m_TiShenglevelvet = $"增加阵盘产量{m_zongMenZhangLaoRoleEntity.Profession1Stage}%";
				}
				else if (m_zongMenZhangLaoRoleEntity.Profession2 == 5)
				{
					m_TiShenglevelvet = $"增加阵盘产量{m_zongMenZhangLaoRoleEntity.Profession2Stage}%";
				}
				else
				{
					m_TiShenglevelvet = "增加阵盘产量0%";
				}
				break;
			case ZhangLaoRenMing.制符长老:
				m_Panel.WuZhiWeiZhangLao12.XuanZho(zhangLao: true);
				if (m_zongMenZhangLaoRoleEntity.Profession1 == 3)
				{
					m_TiShenglevelvet = $"增加符箓产量{m_zongMenZhangLaoRoleEntity.Profession1Stage}%";
				}
				else if (m_zongMenZhangLaoRoleEntity.Profession2 == 3)
				{
					m_TiShenglevelvet = $"增加符箓产量{m_zongMenZhangLaoRoleEntity.Profession2Stage}%";
				}
				else
				{
					m_TiShenglevelvet = "增加符箓产量0%";
				}
				break;
			case ZhangLaoRenMing.工坊长老:
				m_Panel.WuZhiWeiZhangLao13.XuanZho(zhangLao: true);
				m_TiShenglevelvet = $"增加原材产量{m_Levelvet}%";
				break;
			case ZhangLaoRenMing.宗主:
				m_Panel.WuZhiWeiZhangLao14.XuanZho(zhangLao: true);
				break;
			case ZhangLaoRenMing.无:
				m_Panel.WuZhiWeiZhangLao15.XuanZho(zhangLao: true);
				m_TiShenglevelvet = "无";
				break;
			}
			m_Panel.RenMingButton.interactable = true;
			TiSheng();
		}

		private void YingCang()
		{
			m_Panel.ZhiFaZhangLao.XuanZho(zhangLao: false);
			m_Panel.NeiWuZhangLao.XuanZho(zhangLao: false);
			m_Panel.WaiWuZhangLao.XuanZho(zhangLao: false);
			m_Panel.KuangMaiZhangLao.XuanZho(zhangLao: false);
			m_Panel.HuZongZhangLao.XuanZho(zhangLao: false);
			m_Panel.LianQiZhangLao.XuanZho(zhangLao: false);
			m_Panel.FuliZhangLao.XuanZho(zhangLao: false);
			m_Panel.LingShoZhangLao.XuanZho(zhangLao: false);
			m_Panel.ZhenFaZhangLao.XuanZho(zhangLao: false);
			m_Panel.LingYaoZhangLao.XuanZho(zhangLao: false);
			m_Panel.WuZhiWeiZhangLao.XuanZho(zhangLao: false);
			m_Panel.WuZhiWeiZhangLao11.XuanZho(zhangLao: false);
			m_Panel.WuZhiWeiZhangLao12.XuanZho(zhangLao: false);
			m_Panel.WuZhiWeiZhangLao13.XuanZho(zhangLao: false);
			m_Panel.WuZhiWeiZhangLao14.XuanZho(zhangLao: false);
			m_Panel.WuZhiWeiZhangLao15.XuanZho(zhangLao: false);
		}

		private void RenMingBut()
		{
			RenMingReq_1015025 data = new RenMingReq_1015025
			{
				RoleId = m_Id,
				Type = m_Type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RENMING, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					RenMingResp_1015025 renMingResp_ = new RenMingResp_1015025();
					renMingResp_.MergeFrom(byteString);
					if (renMingResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<ZongMen_ZhangLaoWnd>("ZongMen_ZhangLaoWnd.prefab")?.GetGuildRoleListData();
						LiKaiBtn();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(renMingResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
