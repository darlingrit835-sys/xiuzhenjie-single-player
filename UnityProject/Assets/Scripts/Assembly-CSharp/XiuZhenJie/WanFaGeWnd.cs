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
			InitJianJie();
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

		private void InitJianJie()
		{
			string text = "法术，是修士战斗的主要手段。万法阁收录修真界所有的法术，其中不乏大神通术。万法阁历史悠久，由大神通术师法源真人组织。法源真人曾创立多种天阶大神通，包括天遁、天劫雷法等，移山填海威能之大，令众修士唏嘘。法源真人有感于修真界之修行应为众修士共同探索、创造的过程，将其大神通术均贡献分享出来。并以此精神作为万法阁的核心精神，为修真界的繁荣添上了浓墨重彩的一笔。";
			Panel.JianJieText.text = text;
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
