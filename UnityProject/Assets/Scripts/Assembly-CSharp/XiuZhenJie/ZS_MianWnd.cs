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
			InitJianJie();
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

		private void InitJianJie()
		{
			m_Panel.Des.text = "无数阵法大师探索着阴阳、八卦、九宫之理，穷尽一生专研阵法，以期悟得大道。这些阵法师利用虚神界特性，成立了阵师协会，共同专研阵法。他们利用虚神界，可以模拟创造阵法，确认成功的阵法可以绘制成阵图，供炼制使用。";
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
