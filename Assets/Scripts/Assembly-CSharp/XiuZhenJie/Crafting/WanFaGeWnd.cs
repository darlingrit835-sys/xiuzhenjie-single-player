using Google.Protobuf;

namespace XiuZhenJie
{
	public class WanFaGeWnd : Window
	{
		public WanFaGePanel Panel;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WanFaGePanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.FaShuDuiHuanBtn, FaShuDuiHuanBtn);
			AddButtonClickListener(Panel.FaShuZhuCeBtn, FaShuZhuCeBtnClick);
			AddButtonClickListener(Panel.MoNiChuangZaoBtn, MoNiChuangZaoBtn);
			ReFlashData();
			InkCraftHalls.StyleDirectory(Panel.transform, "万法阁",
				(Panel.FaShuDuiHuanBtn, "法术兑换", "查阅阁中法术"),
				(Panel.MoNiChuangZaoBtn, "模拟创法", "进入演法场"),
				(Panel.FaShuZhuCeBtn, "法术注册", "登记自创法术"));
			InkCraftHalls.PlaceHallNotice(Panel.JianJieText, "万法阁收录天下法术。修士可在此查阅与兑换法术，成为术师后可模拟创法，并登记自创法术。");
			GetServerInfo();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_LevelInfo = "【" + Singleton<RoleManager>.Instance.GetCurrentShuShiLevelName() + "】";
			Panel.GongXianDianText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.LevelInfoText.text = m_LevelInfo;
			Panel.ExpText.text = $"{Singleton<RoleManager>.Instance.GetShuShiCurrentExp()}/{Singleton<RoleManager>.Instance.GetShuShiMaxExp()}";
		}

		private void GetServerInfo()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_WFG_OPEN, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterSkillLoftResp_1008021 enterSkillLoftResp_ = new EnterSkillLoftResp_1008021();
					enterSkillLoftResp_.MergeFrom(byteString);
					if (enterSkillLoftResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterSkillLoftResp_.Contribution;
						ReFlashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSkillLoftResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FaShuDuiHuanBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("WFG_FaShuDuiHuanWnd.prefab", true);
		}

		private void FaShuZhuCeBtnClick()
		{
			if (!Singleton<RoleManager>.Instance.IsShuShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非术师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("WFG_ZhuCeWnd.prefab", true);
			}
		}

		private void MoNiChuangZaoBtn()
		{
			if (!Singleton<RoleManager>.Instance.IsShuShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非术师,无法入内!");
			}
			else
			{
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "ChuangZhaoFaShuScene");
			}
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
