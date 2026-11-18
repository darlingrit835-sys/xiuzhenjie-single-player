using Google.Protobuf;

namespace XiuZhenJie
{
	public class BaiRuShanMenWnd : Window
	{
		public BaiRuShanMenPanel Panel;

		private string m_ZongMenId;

		private string m_ZongMenName;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<BaiRuShanMenPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.DiRuBaiTieBtn, DiRuBaiTieAction);
			m_ZongMenId = paralist[0].ToString();
			m_ZongMenName = paralist[1].ToString();
			Panel.TitleText.text = m_ZongMenName;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			GuildDetailReq_1015003 data = new GuildDetailReq_1015003
			{
				GuildId = int.Parse(m_ZongMenId)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildDetailResp_1015003 guildDetailResp_ = new GuildDetailResp_1015003();
					guildDetailResp_.MergeFrom(byteString);
					if (guildDetailResp_.IsSuccess)
					{
						Panel.ZongMenLevelValueText.text = WorldSetting.GetStageName(guildDetailResp_.GuildLevel);
						Panel.ZhangLaoRenShuValuteText.text = guildDetailResp_.GuildRoleCount + "人";
						Panel.DiZiRenShuValueText.text = guildDetailResp_.AiDiZiNum + "人";
						Panel.YingXiangLiValuteText.text = guildDetailResp_.YingXiangLi.ToString();
						Panel.ZhanDouLiValueText.text = guildDetailResp_.ZhanDouLi.ToString();
					}
					else
					{
						Singleton<UIManager>.Instance.CloseWnd(this);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DiRuBaiTieAction()
		{
			GuildJoinReq_1015004 data = new GuildJoinReq_1015004
			{
				GuildId = int.Parse(m_ZongMenId)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildJoinResp_1015004 guildJoinResp_ = new GuildJoinResp_1015004();
					guildJoinResp_.MergeFrom(byteString);
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildJoinResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
