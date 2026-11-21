using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZS_MianWnd : Window
	{
		private ZS_MianPanel m_Panel;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZS_MianPanel>();
			AddButtonClickListener(m_Panel.FanHui, LiKaiBtnClick);
			AddButtonClickListener(m_Panel.LingZhiDuiHuan, LingZhiDuiHuan);
			AddButtonClickListener(m_Panel.LingZhiZhuCe, LingZhiZhuCe);
			AddButtonClickListener(m_Panel.MoMiChuangZao, MoMiChuangZao);
			EnterShenFuHui();
			InkCraftHalls.StyleDirectory(m_Panel.transform, "阵师协会",
				(m_Panel.LingZhiDuiHuan, "阵图兑换", "查阅协会阵图"),
				(m_Panel.MoMiChuangZao, "模拟布阵", "进入阵盘推演"),
				(m_Panel.LingZhiZhuCe, "阵图注册", "登记自创阵图"));
			InkCraftHalls.PlaceHallNotice(m_Panel.Des, "阵师协会以阴阳、八卦与九宫推演阵法。修士可在此兑换阵图，成为阵法师后可模拟布阵，并注册成功推演的阵图。");
			ReFlashData();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_Panel.GongXianDian.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			m_LevelInfo = "【" + Singleton<RoleManager>.Instance.GetCurrentZhenFaShiLevelName() + "】";
			m_Panel.ExperienceText.text = $"{Singleton<RoleManager>.Instance.GetZhenFaCurrentExp()}/{Singleton<RoleManager>.Instance.GetZhenFaShiMaxExp()}";
			m_Panel.LingZhiDengJi_text.text = m_LevelInfo;
		}

		private void EnterShenFuHui()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTERMATRIXASSOCIATION, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterMatrixAssociationResp_1008084 enterMatrixAssociationResp_ = new EnterMatrixAssociationResp_1008084();
					enterMatrixAssociationResp_.MergeFrom(byteString);
					if (enterMatrixAssociationResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterMatrixAssociationResp_.Contribution;
						ReFlashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMatrixAssociationResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void LingZhiDuiHuan()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZS_DuiHuanWnd.prefab", true);
		}

		private void LingZhiZhuCe()
		{
			if (!Singleton<RoleManager>.Instance.IsZhenFaShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非阵法师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("ZS_ZhuCeWnd.prefab", true);
			}
		}

		private void MoMiChuangZao()
		{
			if (!Singleton<RoleManager>.Instance.IsZhenFaShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非阵法师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("ZS_ChuangZhaoWnd.prefab", true);
			}
		}
	}
}
