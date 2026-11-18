using Google.Protobuf;

namespace XiuZhenJie
{
	public class SFH_ShenFuHuiWnd : Window
	{
		public SFH_ShenFuHuiPanel Panel;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<SFH_ShenFuHuiPanel>();
			AddButtonClickListener(Panel.FanHui, LiKaiBtnClick);
			AddButtonClickListener(Panel.FuShuDuiHuan, FuShuDuiHuan);
			AddButtonClickListener(Panel.FuShuZhuCe, FuShuZhuCe);
			AddButtonClickListener(Panel.MoMiChuangZao, MoMiChuangZao);
			EnterShenFuHui();
			InitJianJie();
			ReFlashData();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_LevelInfo = "【" + Singleton<RoleManager>.Instance.GetCurrentFuShiLevelName() + "】";
			Panel.GongXianDian.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.ExperienceText.text = $"{Singleton<RoleManager>.Instance.GetFuShiCurrentExp()}/{Singleton<RoleManager>.Instance.GetFuShiMaxExp()}";
			Panel.FuShiDengJi_text.text = m_LevelInfo;
		}

		private void InitJianJie()
		{
			Panel.Des.text = "符箓，是修士提升瞬时战斗实力的重要手段。神符阁收入修真界所有的符箓，符箓是封印法术而成，故修真界中每有一个法术，就能对应有一个符箓。神符会为符道高人所创立，所有的符师都潜行研究如何沟通天地灵气，将法术封印。为激励符师互通有无，神符会保护符师的创造成果同时，为符师提供了兑换场所，花费一定的贡献可兑换其他的符书。这种方式另大量符师受益，也使更多修士能在战斗中游刃有余";
		}

		private void EnterShenFuHui()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.OPEN_SHENFUHUI, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterSignUnionResp_1008043 enterSignUnionResp_ = new EnterSignUnionResp_1008043();
					enterSignUnionResp_.MergeFrom(byteString);
					if (enterSignUnionResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterSignUnionResp_.Contribution;
						ReFlashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSignUnionResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void FuShuDuiHuan()
		{
			Singleton<UIManager>.Instance.PopUpWnd("SFH_FuShuDuiHuanWnd.prefab", true);
		}

		private void FuShuZhuCe()
		{
			if (!Singleton<RoleManager>.Instance.IsFuShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非符师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("SFH_ZhuCeWnd.prefab", true);
			}
		}

		private void MoMiChuangZao()
		{
			if (!Singleton<RoleManager>.Instance.IsFuShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非符师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("SFH_CreateChooseWnd.prefab", true);
			}
		}
	}
}
