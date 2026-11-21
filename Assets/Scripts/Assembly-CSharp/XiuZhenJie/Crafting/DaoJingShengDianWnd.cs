using Google.Protobuf;

namespace XiuZhenJie
{
	public class DaoJingShengDianWnd : Window
	{
		public DaoJingShengDianPanel Panel;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DaoJingShengDianPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.ChuangZaoGongFaBtn, ChuangZaoGongFaBtnClick);
			AddButtonClickListener(Panel.GongFaZhuCeBtn, GongFaZhuCeBtnClick);
			AddButtonClickListener(Panel.GongFaDuiHuanBtn, GongFaDuiHuanBtnClick);
			AddButtonClickListener(Panel.CreateButton, Create);
			AddButtonClickListener(Panel.Completion, Completion);
			InkDialogArt.StyleButton(Panel.CreateButton, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(Panel.Completion, "ParchmentHeading", InkInterfaceArt.Ink);
			GetServerInfo();
			InkCraftHalls.StyleDirectory(Panel.transform, "道境圣殿",
				(Panel.GongFaDuiHuanBtn, "功法兑换", "查阅殿藏功法"),
				(Panel.ChuangZaoGongFaBtn, "推演功法", "创法或补全"),
				(Panel.GongFaZhuCeBtn, "功法注册", "登记自创功法"));
			InkCraftHalls.PlaceHallNotice(Panel.JianJieText, "道境圣殿收录各家功法。推演时可创造新功法，也可补全已有功法。殿中另设功法注册与兑换。");
			ReFlashData();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_LevelInfo = "【" + Singleton<RoleManager>.Instance.GetCurrentBuDaoLevelName() + "】";
			Panel.GongXianDianText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.LevelText.text = m_LevelInfo;
			Panel.BuDaoShiLevelExpText.text = $"{Singleton<RoleManager>.Instance.GetBuDaoCurrentExp()}/{Singleton<RoleManager>.Instance.GetBuDaoMaxExp()}";
		}

		private void GetServerInfo()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DJSD_OPEN, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterAchievementHallResp_1008029 enterAchievementHallResp_ = new EnterAchievementHallResp_1008029();
					enterAchievementHallResp_.MergeFrom(byteString);
					if (enterAchievementHallResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterAchievementHallResp_.Contribution;
						ReFlashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterAchievementHallResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChuangZaoGongFaBtnClick()
		{
			Panel.BGIamg.gameObject.SetActive(value: true);
		}

		private void Create()
		{
			Panel.BGIamg.gameObject.SetActive(value: false);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "ChuangZhaoGongFaScene");
		}

		private void Completion()
		{
			Panel.BGIamg.gameObject.SetActive(value: false);
			Singleton<UIManager>.Instance.PopUpWnd("DJSD_YiZhuCeListWnd.prefab", true);
		}

		private void GongFaDuiHuanBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DJSD_GongFaDuiHuanWnd.prefab", true);
		}

		private void GongFaZhuCeBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DJSD_ZhuCeWnd.prefab", true);
		}

		private string GetNameMinMingGuiZe()
		{
			return "命名规则:\n1、至少3个字\n2、至多5个字\n3、最后一个字以“本,诀,谱,录,经,功,卷,法,书,典”结尾\n\n\n";
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
