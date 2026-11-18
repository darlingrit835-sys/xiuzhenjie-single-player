using Google.Protobuf;

namespace XiuZhenJie
{
	public class DanShiTaWnd : Window
	{
		public DanShiTaPanel Panel;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DanShiTaPanel>();
			AddButtonClickListener(Panel.FanHui, FanHuiBtnClick);
			AddButtonClickListener(Panel.Email, EmailBtnClick);
			AddButtonClickListener(Panel.DanShiQueRen, DanShiQueRenBtnClick);
			AddButtonClickListener(Panel.DanFangDuiHuan, DanFangDuiHuanBtnClick);
			AddButtonClickListener(Panel.MoMiChuangZao, MoMiChuangZaoBtnClick);
			AddButtonClickListener(Panel.DangFangZhuCeQueRen, DangFangZhuCeQueRenBtnClick);
			GetServerInfo();
			ReFlashData();
		}

		private void GetServerInfo()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_OPEN, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterElixirTowerResp_1008006 enterElixirTowerResp_ = new EnterElixirTowerResp_1008006();
					enterElixirTowerResp_.MergeFrom(byteString);
					if (enterElixirTowerResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterElixirTowerResp_.Contribution;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterElixirTowerResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.GongXianDian.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.DanShiDengJi_text.text = "【" + Singleton<RoleManager>.Instance.GetCurrentDanShiLevelName() + "】";
			Panel.LianDanJingYanText.text = Singleton<RoleManager>.Instance.GetLianDanCurrentExp() + "/" + Singleton<RoleManager>.Instance.GetLianDanMaxExp();
		}

		private void FanHuiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("DanShiTaWnd.prefab");
		}

		private void ActivcityBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("HuoDongWnd.prefab", true);
		}

		private void EmailBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChuanXinWnd.prefab", true);
		}

		private void DanShiQueRenBtnClick()
		{
			if (!Singleton<RoleManager>.Instance.IsDanShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非丹师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("DST_DanShiRenZhengWnd.prefab", true);
			}
		}

		private void DanFangDuiHuanBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DST_DanFangDuiHuanWnd.prefab", true);
		}

		private void MoMiChuangZaoBtnClick()
		{
			if (!Singleton<RoleManager>.Instance.IsDanShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非丹师,无法入内!");
				return;
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DiHuoGe_LianDan", DiHuoGeEnterType.ChuangZao);
		}

		private void DangFangZhuCeQueRenBtnClick()
		{
			if (!Singleton<RoleManager>.Instance.IsDanShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非丹师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("DST_ZhuCeWnd.prefab", true);
			}
		}
	}
}
