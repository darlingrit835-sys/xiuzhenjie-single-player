using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class BattleManager : Singleton<BattleManager>
	{
		public BattleType m_CurrentBattleType;

		public ResourceManager m_ResMgr;

		public NetManager m_NetMgr;

		public VoiceManager m_VoiceMgr;

		public BattleSceneManager m_BattleSceneMgr;

		public List<GameObject> Jin_Tu_HuoMapObjList = new List<GameObject>();

		public List<GameObject> MuMapObjList = new List<GameObject>();

		public List<GameObject> ShuiMapObjList = new List<GameObject>();

		private MapEntity m_MapModel;

		private BattleType m_BattleType;

		private BattleLifeModel m_RoleInfo;

		private string m_DungeonId;
		private readonly HashSet<string> displayedLoot = new HashSet<string>();

		public void Init()
		{
			m_ResMgr = Singleton<ResourceManager>.Instance;
			m_NetMgr = Singleton<NetManager>.Instance;
			m_VoiceMgr = Singleton<VoiceManager>.Instance;
		}

		public void BuildDungeon(MapEntity mapModel, BattleType battleType, MapLevelType mapType)
		{
			BuildDungeonReq_1010001 data = new BuildDungeonReq_1010001
			{
				MapId = mapModel.Id,
				MapType = (int)mapType
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
						m_CurrentBattleType = battleType;
						RoleEnterDungeon(text, text2, mapModel, buildDungeonResp_.Address, buildDungeonResp_.Port, battleType);
					}
					else
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Singleton<RoleManager>.Instance.Role_Entity.CurrentPower = buildDungeonResp_.CurrentPower;
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				}
			}, isShowWaitUI: true);
		}

		private void RoleEnterDungeon(string mapId, string dungeonId, MapEntity mapModel, string ipAddress, int port, BattleType battleType)
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
						BattleLifeModel roleInfo = ReadEntryRole(roleJoinDungeonResp_, byteString);
						StartBattle(mapModel, battleType, roleInfo, roleJoinDungeonResp_.DungeonId, ipAddress, port);
					}
					else
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						string hintMsgByCode = roleJoinDungeonResp_.Code == 4290
							? $"精力不足，本次入场需 {ExpeditionPlace.DungeonVigor(mapId)} 点精力。"
							: MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleJoinDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		// Optional extensions supplied by the game server on successful admission.
		internal static BattleLifeModel ReadEntryRole(RoleJoinDungeonResp_1010002 entry, ByteString reply)
		{
			var input = reply.CreateCodedInput();
			var role = new BattleLifeModel(entry);
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				switch (tag)
				{
				case 232: role.CurrentHuDun = Math.Max(0, input.ReadInt32()); break;
				case 240: Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = input.ReadInt32(); break;
				case 248: role.RestBloodFractionPerSecond = input.ReadInt32() / 1000000.0; break;
				case 256: role.SigilMovePercent = input.ReadInt32(); break;
				case 264: role.MethodMovePercent = input.ReadInt32(); break;
				default: input.SkipLastField(); break;
				}
			}
			return role;
		}

		public void StartBattle(MapEntity _mapModel, BattleType _battleType, BattleLifeModel _roleInfo, string dungeonId, string address, int port)
		{
			m_MapModel = _mapModel;
			m_BattleType = _battleType;
			m_RoleInfo = _roleInfo;
			m_DungeonId = dungeonId;
			m_CurrentBattleType = _battleType;
			if (!Singleton<NetManager>.Instance.BattleServerIsAlive())
			{
				NetManager instance = Singleton<NetManager>.Instance;
				instance.ConnectSuccessEvent -= InitBattle;
				instance.ConnectSuccessEvent += InitBattle;
				Singleton<NetManager>.Instance.StartBattleServer(address, port);
			}
			else
			{
				InitBattle(ServerType.BattleServer, isFirst: false);
			}
		}

		public void ReConnectBatlle()
		{
			BattleRoleEnterReq_8001001 data = new BattleRoleEnterReq_8001001
			{
				DungeonId = m_DungeonId,
				WorldVersion = 1,
				RoleId = Singleton<RoleManager>.Instance.Role_Entity.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_ENTER, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BattleRoleEnterResp_8001001 battleRoleEnterResp_ = new BattleRoleEnterResp_8001001();
					battleRoleEnterResp_.MergeFrom(byteString);
					if (battleRoleEnterResp_.IsSuccess)
					{
						if (m_BattleSceneMgr == null || data.DungeonId != m_DungeonId) return;
						var scene = m_BattleSceneMgr;
						if (scene.FieldView != null && !battleRoleEnterResp_.WorldTiles.IsEmpty)
						{
							var player = scene.SelfPlayerEntity;
							Vector3 pos = player.GetPos();
							pos.x = (float)battleRoleEnterResp_.PosX;
							pos.y = (float)battleRoleEnterResp_.PosY;
							player.RestorePosition(pos);
							scene.FieldView.RestoreExplored(battleRoleEnterResp_.Explored.ToByteArray());
							var remaining = new HashSet<string>();
							foreach (var plant in battleRoleEnterResp_.Plants) remaining.Add(plant.GoodsId);
							foreach (var ore in battleRoleEnterResp_.Minerals) remaining.Add(ore.GoodsId);
							// Remaining loose drops follow the entry reply on the same battle connection.
							displayedLoot.Clear();
							foreach (var drop in UnityEngine.Object.FindObjectsOfType<DiaoLuoItem>(true))
							{
								bool resource = drop.CompareTag("Plant") || drop.CompareTag("Mineral");
								if (resource ? !remaining.Contains(drop.GoodsId) : drop.CompareTag("BattleDiaoLuoGoods") || drop.CompareTag("BattleDiaoLuoLingShi"))
								{
									drop.gameObject.SetActive(false);
									UnityEngine.Object.Destroy(drop.gameObject);
								}
							}
						}
					}
					else
					{
						Debug.Log("重新进入副本失败");
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(battleRoleEnterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode, ExitBattle, ExitBattle);
					}
				}
			}, isShowWaitUI: false, ServerType.BattleServer);
		}

		public void ExitBattle(bool isExitScene)
		{
			DestroyBattle();
			if (isExitScene)
			{
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, () =>
				{
					Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
				});
			}
			else
			{
				Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
			}
		}

		public void ExitBattle()
		{
			DestroyBattle();
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, () =>
			{
				Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
			});
		}

		public void ExitBattle(UnityAction unityAction)
		{
			DestroyBattle();
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, () =>
			{
				Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
				if (unityAction != null)
				{
					unityAction();
				}
			});
		}

		private void InitBattle(ServerType serverType, bool isFirst)
		{
			if (serverType != ServerType.BattleServer)
			{
				return;
			}
			Singleton<NetManager>.Instance.ConnectSuccessEvent -= InitBattle;
			BattleRoleEnterReq_8001001 data = new BattleRoleEnterReq_8001001
			{
				DungeonId = m_DungeonId,
				WorldVersion = 1,
				RoleId = Singleton<RoleManager>.Instance.Role_Entity.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_ENTER, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (data.DungeonId != m_DungeonId || m_BattleSceneMgr != null) return;
				if (!(byteString == null))
				{
					BattleRoleEnterResp_8001001 battleRoleEnterResp_ = new BattleRoleEnterResp_8001001();
					CodedInputStream input = byteString.CreateCodedInput();
					battleRoleEnterResp_.MergeFrom(input);
					if (battleRoleEnterResp_.IsSuccess)
					{
						BattleFieldLayout layout;
						try { layout = BattleFieldLayout.Read(battleRoleEnterResp_); }
						catch (FormatException error)
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							MonoSingleton<GameStart>.Instance.PopUpAttention(error.Message);
							Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
							return;
						}
						GameObject gameObject = new GameObject
						{
							name = "BattleRoot"
						};
						gameObject.transform.SetParent(MonoSingleton<GameRoot>.Instance.gameObject.transform);
						m_BattleSceneMgr = gameObject.AddComponent<BattleSceneManager>();
						m_BattleSceneMgr.Layout = layout;
						if (layout != null) m_BattleSceneMgr.Flourish = gameObject.AddComponent<BattleFlourish>();
						m_MapModel.MaxX = battleRoleEnterResp_.MapMaxX;
						m_MapModel.MinX = battleRoleEnterResp_.MapMinX;
						m_MapModel.MaxY = battleRoleEnterResp_.MapMaxY;
						m_MapModel.MinY = battleRoleEnterResp_.MapMinY;
						m_RoleInfo.ObjectIndex = battleRoleEnterResp_.RoleIndex;
						m_RoleInfo.Init_X = (float)battleRoleEnterResp_.PosX;
						m_RoleInfo.Init_Y = (float)battleRoleEnterResp_.PosY;
						List<BattleLifeModel> list = new List<BattleLifeModel>();
						List<BattleNoLifeModel> list2 = new List<BattleNoLifeModel>();
						if (battleRoleEnterResp_.Objects != null)
						{
							foreach (BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002 @object in battleRoleEnterResp_.Objects)
							{
								BattleLifeModel item = new BattleLifeModel(@object);
								list.Add(item);
							}
						}
						if (battleRoleEnterResp_.Plants != null)
						{
							foreach (BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013 plant in battleRoleEnterResp_.Plants)
							{
								list2.Add(new BattleNoLifeModel(plant)
								{
									ObjectType = 12
								});
							}
						}
						if (battleRoleEnterResp_.Minerals != null)
						{
							foreach (BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013 mineral in battleRoleEnterResp_.Minerals)
							{
								list2.Add(new BattleNoLifeModel(mineral)
								{
									ObjectType = 13
								});
							}
						}
						if (battleRoleEnterResp_.SecretArea != null)
						{
							BattleNoLifeModel battleNoLifeModel = new BattleNoLifeModel(battleRoleEnterResp_.SecretArea);
							if (m_CurrentBattleType == BattleType.MiJingBattle)
							{
								battleNoLifeModel.ObjectType = 32;
							}
							else
							{
								battleNoLifeModel.ObjectType = 28;
							}
							list2.Add(battleNoLifeModel);
						}
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						m_BattleSceneMgr.Init(m_MapModel, m_BattleType, m_RoleInfo, list, list2, m_DungeonId, battleRoleEnterResp_.OverTime);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(battleRoleEnterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: false, ServerType.BattleServer);
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetAllGoods(null, isShowWaiting: false);
		}

		public void NoticeDiaoLuo(object data)
		{
			if (m_BattleSceneMgr == null)
			{
				return;
			}
			ByteString byteString = data as ByteString;
			if (!(byteString == null))
			{
				MonsterDeadResp_8002003 monsterDeadResp_ = new MonsterDeadResp_8002003();
				monsterDeadResp_.MergeFrom(byteString);
				if (m_BattleSceneMgr.Layout != null && !displayedLoot.Add(monsterDeadResp_.GoodsId)) return;
				if (monsterDeadResp_.GoodsType % 1000 != 100)
				{
					m_BattleSceneMgr.MonsterDiaoLuo(monsterDeadResp_.GoodsId, monsterDeadResp_.ObjectIndex, monsterDeadResp_.Name, monsterDeadResp_.Rank, (float)monsterDeadResp_.PosX, (float)monsterDeadResp_.PosY, monsterDeadResp_.Count);
				}
				else
				{
					m_BattleSceneMgr.MonsterDiaoLuoLingShi(monsterDeadResp_.GoodsId, monsterDeadResp_.ObjectIndex, monsterDeadResp_.Count, (float)monsterDeadResp_.PosX, (float)monsterDeadResp_.PosY);
				}
			}
		}

		public void DestroyBattle()
		{
			m_DungeonId = null;
			displayedLoot.Clear();
			Singleton<NetManager>.Instance.ConnectSuccessEvent -= InitBattle;
			if (!(m_BattleSceneMgr == null))
			{
				m_BattleSceneMgr.StopAllCoroutines();
				UnityEngine.Object.Destroy(m_BattleSceneMgr.gameObject);
				MuMapObjList.Clear();
				ShuiMapObjList.Clear();
				Jin_Tu_HuoMapObjList.Clear();
				m_BattleSceneMgr = null;
				Singleton<ResourceManager>.Instance.ClearCache();
				GC.Collect();
			}
		}

		public void MapPrefabLoad()
		{
			if (m_BattleSceneMgr != null && m_BattleSceneMgr.Layout != null) return;
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_00.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_01.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_02.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_03.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_04.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_1.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_2.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_3.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_4.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_5.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_6.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_7.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_8.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_9.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_10.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_11.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_12.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_13.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_14.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_15.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_16.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_17.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_18.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_19.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_20.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_21.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_22.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_23.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_24.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_25.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_26.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_27.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_28.prefab"));
			MuMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_29.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_00.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_01.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_02.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_03.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_1.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_2.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_3.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_4.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_5.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_6.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_7.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_8.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_9.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_10.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_11.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_12.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_13.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_14.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_15.prefab"));
			ShuiMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_16.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/JiChu/Sha_00.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/JiChu/Sha_01.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_1.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_2.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_3.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_4.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_5.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_6.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_7.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_8.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_9.prefab"));
			Jin_Tu_HuoMapObjList.Add(Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_10.prefab"));
		}
	}
}
