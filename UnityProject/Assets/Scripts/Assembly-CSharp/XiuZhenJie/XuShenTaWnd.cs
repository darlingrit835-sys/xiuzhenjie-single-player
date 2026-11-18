using Google.Protobuf;

namespace XiuZhenJie
{
	public class XuShenTaWnd : Window
	{
		public XuShenTaPanel Panel;

		public int Record;

		private int m_CurrentFloor;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<XuShenTaPanel>();
			AddButtonClickListener(Panel.BackBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.ChuangTaBtn, ChuangTa);
			AddButtonClickListener(Panel.DaoJingBangBtn, DaoJingBangClick);
			AddButtonClickListener(Panel.SuiYueBangBtn, SuiYueBangClick);
			Panel.MyRecord.text = "0层";
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_XST_OPEN, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterUnrealTowerResp_1008001 enterUnrealTowerResp_ = new EnterUnrealTowerResp_1008001();
					enterUnrealTowerResp_.MergeFrom(byteString);
					if (enterUnrealTowerResp_.IsSuccess)
					{
						if (enterUnrealTowerResp_.RushTowerNum > 0)
						{
							m_CurrentFloor = enterUnrealTowerResp_.RushTowerNum;
							Panel.MyRecord.text = enterUnrealTowerResp_.RushTowerNum + "层";
						}
						else
						{
							Panel.MyRecord.text = "暂无排名";
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterUnrealTowerResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public void ChuangTa()
		{
			BuildDungeonReq_1010001 data = new BuildDungeonReq_1010001
			{
				MapType = 63
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUILDDUNGEON, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuildDungeonResp_1010001 buildDungeonResp_ = new BuildDungeonResp_1010001();
					buildDungeonResp_.MergeFrom(byteString);
					string text = "";
					string text2 = "";
					if (buildDungeonResp_.IsSuccess)
					{
						text = buildDungeonResp_.MapId;
						text2 = buildDungeonResp_.DungeonId;
						MapEntity mapModel = new MapEntity
						{
							Id = (m_CurrentFloor + 1).ToString(),
							MapName = $"虚神塔{m_CurrentFloor + 1}层",
							FiveElement = 1,
							MapType = 4
						};
						RoleEnterDungeon(text, text2, mapModel, buildDungeonResp_.Address, buildDungeonResp_.Port);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Singleton<RoleManager>.Instance.Role_Entity.CurrentPower = buildDungeonResp_.CurrentPower;
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				}
			}, isShowWaitUI: true);
		}

		private void RoleEnterDungeon(string mapId, string dungeonId, MapEntity mapModel, string ipAddress, int port)
		{
			RoleJoinDungeonReq_1010002 data = new RoleJoinDungeonReq_1010002
			{
				DungeonId = dungeonId,
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLEJOINDUNGEON, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleJoinDungeonResp_1010002 roleJoinDungeonResp_ = new RoleJoinDungeonResp_1010002();
					roleJoinDungeonResp_.MergeFrom(byteString);
					if (roleJoinDungeonResp_.IsSuccess)
					{
						BattleLifeModel roleInfo = new BattleLifeModel(roleJoinDungeonResp_);
						Singleton<BattleManager>.Instance.StartBattle(mapModel, BattleType.TongTianTaBattle, roleInfo, dungeonId, ipAddress, port);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleJoinDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void DaoJingBangClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XuShenTaBangDanWnd.prefab", true, "role_level");
		}

		public void SuiYueBangClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XuShenTaBangDanWnd.prefab", true, "role_age");
		}
	}
}
