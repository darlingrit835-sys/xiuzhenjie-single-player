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
			GetServerInfo();
			InitJianJie();
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

		private void InitJianJie()
		{
			Panel.JianJieText.text = "道法为修行之根本，窥得一丝天道使凡人得以修行。道境圣殿由来已久，可以追溯至旧历甚至于太古。道境圣殿管理着修真界大大小小各种功法，而在道境圣殿中人，以布道为己任，是促使修真界繁荣昌盛的基石。道境圣殿收集修真界所有的修行功法，并且保护功法创造者最基本的权益，让广大修士可以放心创造和兑换功法。";
		}

		private void ChuangZaoGongFaBtnClick()
		{
			Panel.BGIamg.gameObject.SetActive(value: true);
			AddButtonClickListener(Panel.CreateButton, Create);
			AddButtonClickListener(Panel.Completion, Completion);
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
