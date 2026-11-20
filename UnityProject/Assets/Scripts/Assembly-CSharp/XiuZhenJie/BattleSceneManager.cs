using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class BattleSceneManager : MonoBehaviour
	{
		private float m_OverTime;

		private bool m_RecordTimeBegin;

		private MapEntity m_MapDataSource;

		private BattleType m_BattleType;

		private MapManager m_MapManager;

		private StateManager m_StateManager;

		public BattleSceneResourceManager m_BattleSceneResourceManager;

		private SkillManager m_SkillManager;

		private BattleWnd m_BattleWnd;

		public PlayerEntity SelfPlayerEntity;

		public EndBattleInfoModel m_EndInfo;

		public Camera PlayerCamera;

		private Dictionary<string, MonsterEntity> m_AliveMonsterList;

		private Dictionary<string, MonsterEntity> m_DeadMonsterList;

		public Dictionary<string, BattleLifeModel> AliveMonsterModelDic;

		public Dictionary<string, BattleLifeModel> DeadMonsterModelDic;

		private Dictionary<int, List<BaseNaJieGoods>> DiaoLuoDic;

		public Queue<BattleLifeModel> AllMonsterInBattle;

		public Queue<BattleNoLifeModel> AllGoodsInBattle;

		public Queue<XzjWorldGoodsMaterialsPlantsSpaceVO> PlantList;

		public Queue<XzjWorldGoodsMaterialsMineralSpaceVO> MaterialsList;

		public Queue<GoodsArticleSecretAreaVO> SecretAreaList;

		public List<BaseNaJieGoods> PickUpGoods;

		private Queue<int> MapMonsterRequestQue;

		private Queue<int> MiJingMonsterRequestQue;

		private GameObject m_NormalMonsterInsObj;

		private List<BattleLifeModel> m_AllMonster;

		public float m_KillProgress;

		private string m_DungeonId;

		private maxMin MapMaxAndMin;

		public float OverTime => m_OverTime;

		public BattleType CurrentBattleType => m_BattleType;

		public Dictionary<string, MonsterEntity> AliveMonsterList
		{
			get
			{
				return m_AliveMonsterList;
			}
			private set
			{
				m_AliveMonsterList = value;
			}
		}

		public Dictionary<string, MonsterEntity> DeadMonsterList
		{
			get
			{
				return m_DeadMonsterList;
			}
			private set
			{
				m_DeadMonsterList = value;
			}
		}

		public string DungenonId
		{
			get
			{
				return m_DungeonId;
			}
			private set
			{
				m_DungeonId = value;
			}
		}

		private void Awake()
		{
			m_KillProgress = 0f;
			m_AllMonster = new List<BattleLifeModel>();
			AliveMonsterList = new Dictionary<string, MonsterEntity>();
			DeadMonsterList = new Dictionary<string, MonsterEntity>();
			AliveMonsterModelDic = new Dictionary<string, BattleLifeModel>();
			DeadMonsterModelDic = new Dictionary<string, BattleLifeModel>();
			PickUpGoods = new List<BaseNaJieGoods>();
			AllMonsterInBattle = new Queue<BattleLifeModel>();
			AllGoodsInBattle = new Queue<BattleNoLifeModel>();
			SecretAreaList = new Queue<GoodsArticleSecretAreaVO>();
			PlantList = new Queue<XzjWorldGoodsMaterialsPlantsSpaceVO>();
			MaterialsList = new Queue<XzjWorldGoodsMaterialsMineralSpaceVO>();
			DiaoLuoDic = new Dictionary<int, List<BaseNaJieGoods>>();
		}

		private void Update()
		{
			if (AliveMonsterList != null)
			{
				foreach (KeyValuePair<string, MonsterEntity> aliveMonster in AliveMonsterList)
				{
					aliveMonster.Value.TickAILogic();
				}
			}
			if (SelfPlayerEntity != null)
			{
				SelfPlayerEntity.TickAILogic();
			}
			if (m_RecordTimeBegin)
			{
				m_OverTime -= Time.deltaTime;
				if (m_OverTime <= 0f)
				{
					TimeOver();
				}
			}
		}

		private void FixedUpdate()
		{
			if (AliveMonsterList != null)
			{
				foreach (KeyValuePair<string, MonsterEntity> aliveMonster in AliveMonsterList)
				{
					aliveMonster.Value.FixUpdate();
				}
			}
			if (SelfPlayerEntity != null)
			{
				SelfPlayerEntity.FixUpdate();
			}
		}

		public maxMin GetMapMaxAndMin()
		{
			return MapMaxAndMin;
		}

		public string GetMapName()
		{
			return m_MapDataSource.MapName;
		}

		public int GetMapStage()
		{
			return WorldSetting.GetStageByLevel(m_MapDataSource.MinLevel);
		}

		public void Init(MapEntity _map, BattleType _battleType, BattleLifeModel _roleInfo, List<BattleLifeModel> monsterList, List<BattleNoLifeModel> goodsList, string dungeonId, long dungeonOverTime)
		{
			m_DungeonId = dungeonId;
			m_OverTime = (float)dungeonOverTime / 1000f;
			m_MapDataSource = _map;
			m_BattleType = _battleType;
			m_StateManager = base.gameObject.AddComponent<StateManager>();
			m_StateManager.Init();
			m_SkillManager = base.gameObject.AddComponent<SkillManager>();
			m_SkillManager.Init();
			m_BattleSceneResourceManager = base.gameObject.AddComponent<BattleSceneResourceManager>();
			m_AllMonster = monsterList;
			AllMonsterInBattle.Clear();
			if (monsterList != null)
			{
				foreach (BattleLifeModel monster in monsterList)
				{
					AllMonsterInBattle.Enqueue(monster);
				}
			}
			if (goodsList != null)
			{
				foreach (BattleNoLifeModel goods in goodsList)
				{
					AllGoodsInBattle.Enqueue(goods);
				}
			}
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "FightingScene", () =>
			{
				m_BattleSceneResourceManager.Init();
			}, () =>
			{
				LoadBattleSceneCompelete(_map, _battleType, _roleInfo);
				StartCoroutine(CreateBattleItem());
			});
		}

		private IEnumerator CreateBattleItem()
		{
			while (true)
			{
				if (AllGoodsInBattle != null && AllGoodsInBattle.Count > 0)
				{
					BattleNoLifeModel battleNoLifeModel = AllGoodsInBattle.Dequeue();
					switch ((GoodsTypeEnum)battleNoLifeModel.ObjectType)
					{
					case GoodsTypeEnum.MATERIAL_PLANT:
						LoadPlant(battleNoLifeModel);
						break;
					case GoodsTypeEnum.MATERIAL_MINERAL:
						LoadMineral(battleNoLifeModel);
						break;
					case GoodsTypeEnum.SECREAT_AREA:
						LoadMiJing(battleNoLifeModel);
						break;
					case GoodsTypeEnum.GUILD_SECRET:
						LoadMiJing(battleNoLifeModel);
						break;
					}
				}
				if (AllMonsterInBattle != null && AllMonsterInBattle.Count > 0)
				{
					BattleLifeModel data = AllMonsterInBattle.Dequeue();
					InstanceEntity(data);
				}
				yield return null;
			}
		}

		private void InstanceEntity(BattleLifeModel data)
		{
			BattleEntityType battleEntityType = (BattleEntityType)(data.ObjectType / 1000);
			if (battleEntityType != BattleEntityType.Player && battleEntityType == BattleEntityType.Monster)
			{
				InitMonster(data, data.ObjectType % 1000);
			}
		}

		private void InitMonster(BattleLifeModel data, int monsterType)
		{
			switch ((MonsterType)monsterType)
			{
			case MonsterType.AnimalMonster:
				InstanceAnimalMonster(data);
				break;
			case MonsterType.PeopleMonster:
				InstancePeopleMonster(data);
				break;
			}
			AliveMonsterModelDic.Add(data.ObjectIndex.ToString(), data);
		}

		private void InstancePeopleMonster(BattleLifeModel data)
		{
			switch ((MonsterLevel)data.Type)
			{
			case MonsterLevel.YouNian:
			case MonsterLevel.ChengNian:
			case MonsterLevel.JingRui:
			case MonsterLevel.LingZhu:
				LoadNormalPeopleMonster(data);
				break;
			case MonsterLevel.BOSS:
				LoadNormalPeopleBossMonster(data);
				break;
			}
		}

		private void InstanceAnimalMonster(BattleLifeModel data)
		{
			switch ((MonsterLevel)data.Type)
			{
			case MonsterLevel.YouNian:
			case MonsterLevel.ChengNian:
				LoadNormalAnimalMonster(data);
				break;
			case MonsterLevel.JingRui:
				LoadHighOrderAnimalMonster(data);
				break;
			case MonsterLevel.LingZhu:
				if (m_BattleType != BattleType.MapBattle)
				{
					LoadTopAnimalMonster(data);
				}
				else
				{
					LoadNormalMapTopAnimalMonster(data);
				}
				break;
			case MonsterLevel.BOSS:
				LoadBossAnimalMonster(data);
				break;
			}
		}

		private string GetMonsterLevelName(MonsterModel monster)
		{
			string result = "";
			if (monster.monsterType == 1)
			{
				switch ((MonsterLevel)monster.type)
				{
				case MonsterLevel.YouNian:
					result = "平凡";
					break;
				case MonsterLevel.ChengNian:
					result = "优秀";
					break;
				case MonsterLevel.JingRui:
					result = "精英";
					break;
				case MonsterLevel.LingZhu:
					result = "天骄";
					break;
				case MonsterLevel.BOSS:
					result = "长老";
					break;
				}
			}
			else
			{
				switch ((MonsterLevel)monster.type)
				{
				case MonsterLevel.YouNian:
					result = "幼年";
					break;
				case MonsterLevel.ChengNian:
					result = "成年";
					break;
				case MonsterLevel.JingRui:
					result = "精锐";
					break;
				case MonsterLevel.LingZhu:
					result = "领主";
					break;
				case MonsterLevel.BOSS:
					result = "妖王";
					break;
				}
			}
			return result;
		}

		private void ContinueReqMonsterXST(int floor)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_TOWER_NEXT_LEVEL, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterTowerNextLevelResp_8001009 enterTowerNextLevelResp_ = new EnterTowerNextLevelResp_8001009();
					enterTowerNextLevelResp_.MergeFrom(byteString);
					if (enterTowerNextLevelResp_.IsSuccess)
					{
						AliveMonsterModelDic.Clear();
						m_DeadMonsterList.Clear();
						DeadMonsterModelDic.Clear();
						{
							foreach (EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010 @object in enterTowerNextLevelResp_.Objects)
							{
								BattleLifeModel item = new BattleLifeModel(@object);
								AllMonsterInBattle.Enqueue(item);
								m_OverTime = (float)enterTowerNextLevelResp_.OverTime / 1000f;
								m_RecordTimeBegin = true;
							}
							return;
						}
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterTowerNextLevelResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true, ServerType.BattleServer);
		}

		private void ScreeningMonster(MonsterModel item)
		{
			if (item.xzjWorldGoodsMaterialsMineralSpaceVO != null)
			{
				MaterialsList.Enqueue(item.xzjWorldGoodsMaterialsMineralSpaceVO);
			}
			else if (item.xzjWorldGoodsMaterialsPlantsSpaceVO != null)
			{
				PlantList.Enqueue(item.xzjWorldGoodsMaterialsPlantsSpaceVO);
			}
			else if (item.goodsArticleSecretArea != null)
			{
				SecretAreaList.Enqueue(item.goodsArticleSecretArea);
			}
			else if (item.goodsArticleSecretArea == null && item.xzjWorldGoodsMaterialsPlantsSpaceVO == null)
			{
				_ = item.xzjWorldGoodsMaterialsMineralSpaceVO;
			}
		}

		private void LoadBattleSceneCompelete(MapEntity _map, BattleType _battleType, BattleLifeModel _battleLifeModel)
		{
			Singleton<UIManager>.Instance.CloseAllWnd();
			LoadPlayer(_battleLifeModel);
			BattleMapType mapType = BattleMapType.NormalMap;
			MapMaxAndMin = new maxMin
			{
				maxX = (int)_map.MaxX,
				maxY = (int)_map.MaxY,
				minX = (int)_map.MinX,
				minY = (int)_map.MinY
			};
			LoadMap(mapType);
			m_BattleWnd = Singleton<UIManager>.Instance.PopUpWnd("BattleWnd.prefab", true) as BattleWnd;
			m_BattleWnd.SetPlayerInfo(_battleLifeModel);
			if ((Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2) & (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 13))
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
			}
			m_RecordTimeBegin = true;
		}

		private void LoadMap(BattleMapType mapType)
		{
			GameObject gameObject = GameObject.Find("Map");
			m_MapManager = gameObject.GetComponent<MapManager>();
			int wuXing = ((m_MapDataSource.FiveElement <= 0) ? Random.Range(1, 5) : m_MapDataSource.FiveElement);
			m_MapManager.Init(this, mapType, (WuXing)wuXing);
		}

		private BattleLifeModel GetMonsterLifeModel(MonsterModel monsterModel)
		{
			BattleLifeModel obj = new BattleLifeModel
			{
				AttackFiveElement = monsterModel.fiveElement,
				DefenseFiveElement = monsterModel.fiveElement,
				ShenShiFiveElement = monsterModel.fiveElement,
				MaxSuperAir = monsterModel.superAir,
				CurrentSuperAir = monsterModel.superAir,
				InitMoveSpeed = monsterModel.baseSpeed * 3.0,
				CurrentMoveSpeed = monsterModel.baseSpeed * 3.0,
				MaxPerception = monsterModel.perception,
				CurrentPerception = monsterModel.perception,
				Max_JianRen = monsterModel.toughTenacity,
				CurrentJianRen = monsterModel.toughTenacity,
				Max_Blood = monsterModel.maxPureBlood,
				CurrentBlood = monsterModel.maxPureBlood,
				RecoverSuperAirEverySecondBattle = monsterModel.superAir * 0.009999999776482582
			};
			obj.RecoverSuperAirEverySecondNormal = (double)obj.RecoverSuperAirEverySecondBattle * 5.0;
			string text = $"{GetMonsterLevelName(monsterModel)}·{monsterModel.name}";
			obj.Name = text;
			obj.Level = monsterModel.level;
			obj.Icon = monsterModel.icon;
			obj.Type = monsterModel.type;
			return obj;
		}

		private static Sprite ResolveMonsterPortrait(BattleLifeModel monster)
		{
			Sprite portrait = CatalogIconArt.MonsterFromLegacy(monster.ObjectType, monster.Icon);
			return portrait != null ? portrait : Singleton<FacadeManager>.Instance.SelectImgSprite(0, monster.Icon, 4);
		}

		public void LoadNormalAnimalMonster(BattleLifeModel monsterModel)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (!m_AliveMonsterList.ContainsKey(monsterModel.ObjectIndex.ToString()))
			{
				if (m_NormalMonsterInsObj == null)
				{
					m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
				}
				NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
				AnimalMonster_Normal_Entity animalMonster_Normal_Entity = new AnimalMonster_Normal_Entity();
				animalMonster_Normal_Entity.BattleMgr = this;
				animalMonster_Normal_Entity.SkillMgr = m_SkillManager;
				animalMonster_Normal_Entity.StateMgr = m_StateManager;
				animalMonster_Normal_Entity.SetBattleProps(monsterModel);
				animalMonster_Normal_Entity.SetCtrl(component, monsterModel.ObjectIndex.ToString(), new Vector2(monsterModel.Init_X, monsterModel.Init_Y));
				animalMonster_Normal_Entity.SetShenShiArea(monsterModel.AttackDistance);
				Sprite touXiangIcon = ResolveMonsterPortrait(monsterModel);
				animalMonster_Normal_Entity.SetTouXiangIcon(touXiangIcon);
				animalMonster_Normal_Entity.Idle();
				m_AliveMonsterList.Add(monsterModel.ObjectIndex.ToString(), animalMonster_Normal_Entity);
			}
		}

		public void LoadHighOrderAnimalMonster(BattleLifeModel monsterModel)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (!m_AliveMonsterList.ContainsKey(monsterModel.ObjectIndex.ToString()))
			{
				if (m_NormalMonsterInsObj == null)
				{
					m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
				}
				NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
				AnimalMonster_HighOrder_Entity animalMonster_HighOrder_Entity = new AnimalMonster_HighOrder_Entity();
				animalMonster_HighOrder_Entity.BattleMgr = this;
				animalMonster_HighOrder_Entity.SkillMgr = m_SkillManager;
				animalMonster_HighOrder_Entity.StateMgr = m_StateManager;
				animalMonster_HighOrder_Entity.SetBattleProps(monsterModel);
				animalMonster_HighOrder_Entity.SetCtrl(component, monsterModel.ObjectIndex.ToString(), new Vector2(monsterModel.Init_X, monsterModel.Init_Y));
				animalMonster_HighOrder_Entity.SetShenShiArea(monsterModel.AttackDistance);
				Sprite touXiangIcon = ResolveMonsterPortrait(monsterModel);
				animalMonster_HighOrder_Entity.SetTouXiangIcon(touXiangIcon);
				animalMonster_HighOrder_Entity.Idle();
				if (m_AliveMonsterList == null)
				{
					m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
				}
				m_AliveMonsterList.Add(monsterModel.ObjectIndex.ToString(), animalMonster_HighOrder_Entity);
			}
		}

		public void LoadTopAnimalMonster(BattleLifeModel monsterModel)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (!m_AliveMonsterList.ContainsKey(monsterModel.ObjectIndex.ToString()))
			{
				if (m_NormalMonsterInsObj == null)
				{
					m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
				}
				NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
				AnimalMonster_Top_Entity animalMonster_Top_Entity = new AnimalMonster_Top_Entity();
				animalMonster_Top_Entity.BattleMgr = this;
				animalMonster_Top_Entity.SkillMgr = m_SkillManager;
				animalMonster_Top_Entity.StateMgr = m_StateManager;
				animalMonster_Top_Entity.SetBattleProps(monsterModel);
				animalMonster_Top_Entity.SetCtrl(component, monsterModel.ObjectIndex.ToString(), new Vector2(monsterModel.Init_X, monsterModel.Init_Y));
				animalMonster_Top_Entity.SetShenShiArea(monsterModel.AttackDistance);
				Sprite touXiangIcon = ResolveMonsterPortrait(monsterModel);
				animalMonster_Top_Entity.SetTouXiangIcon(touXiangIcon);
				animalMonster_Top_Entity.Idle();
				if (m_AliveMonsterList == null)
				{
					m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
				}
				m_AliveMonsterList.Add(monsterModel.ObjectIndex.ToString(), animalMonster_Top_Entity);
			}
		}

		public void LoadBossAnimalMonster(BattleLifeModel monsterModel)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (!m_AliveMonsterList.ContainsKey(monsterModel.ObjectIndex.ToString()))
			{
				if (m_NormalMonsterInsObj == null)
				{
					m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
				}
				NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
				AnimalMonster_Top_Entity animalMonster_Top_Entity = new AnimalMonster_Top_Entity();
				animalMonster_Top_Entity.BattleMgr = this;
				animalMonster_Top_Entity.SkillMgr = m_SkillManager;
				animalMonster_Top_Entity.StateMgr = m_StateManager;
				animalMonster_Top_Entity.SetBattleProps(monsterModel);
				animalMonster_Top_Entity.SetCtrl(component, monsterModel.ObjectIndex.ToString(), new Vector2(monsterModel.Init_X, monsterModel.Init_Y));
				animalMonster_Top_Entity.SetShenShiArea(monsterModel.AttackDistance);
				Sprite touXiangIcon = ResolveMonsterPortrait(monsterModel);
				animalMonster_Top_Entity.SetTouXiangIcon(touXiangIcon);
				animalMonster_Top_Entity.Idle();
				if (m_AliveMonsterList == null)
				{
					m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
				}
				m_AliveMonsterList.Add(monsterModel.ObjectIndex.ToString(), animalMonster_Top_Entity);
			}
		}

		public void LoadNormalMapTopAnimalMonster(BattleLifeModel monsterModel)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (!m_AliveMonsterList.ContainsKey(monsterModel.ObjectIndex.ToString()))
			{
				if (m_NormalMonsterInsObj == null)
				{
					m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
				}
				NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
				AnimalMonster_Top_Entity animalMonster_Top_Entity = new AnimalMonster_Top_Entity();
				animalMonster_Top_Entity.BattleMgr = this;
				animalMonster_Top_Entity.SkillMgr = m_SkillManager;
				animalMonster_Top_Entity.StateMgr = m_StateManager;
				animalMonster_Top_Entity.SetBattleProps(monsterModel);
				animalMonster_Top_Entity.SetCtrl(component, monsterModel.ObjectIndex.ToString(), new Vector2(monsterModel.Init_X, monsterModel.Init_Y));
				animalMonster_Top_Entity.SetShenShiArea(monsterModel.AttackDistance);
				Sprite touXiangIcon = ResolveMonsterPortrait(monsterModel);
				animalMonster_Top_Entity.SetTouXiangIcon(touXiangIcon);
				animalMonster_Top_Entity.Idle();
				if (m_AliveMonsterList == null)
				{
					m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
				}
				m_AliveMonsterList.Add(monsterModel.ObjectIndex.ToString(), animalMonster_Top_Entity);
				animalMonster_Top_Entity.BaseController.gameObject.SetActive(value: false);
			}
		}

		public void ShowTopAnimalMonster()
		{
			MonsterEntity monsterEntity = null;
			foreach (KeyValuePair<string, MonsterEntity> aliveMonster in m_AliveMonsterList)
			{
				if (aliveMonster.Value.BattleModel.Type == 4 && aliveMonster.Value.BattleModel.ObjectType % 1000 == 2)
				{
					monsterEntity = aliveMonster.Value;
					break;
				}
			}
			if (monsterEntity != null)
			{
				monsterEntity.BaseController.gameObject.SetActive(value: true);
				monsterEntity.SetAlwaysAttack();
				monsterEntity.SetTarget(SelfPlayerEntity);
				m_BattleWnd.ShowBossWarning();
			}
		}

		public void LoadNormalPeopleMonster(BattleLifeModel data)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (m_AliveMonsterList.ContainsKey(data.ObjectIndex.ToString()))
			{
				return;
			}
			if (m_NormalMonsterInsObj == null)
			{
				m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
			}
			NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
			PeopleMonster_Normal_Entity peopleMonster_Normal_Entity = new PeopleMonster_Normal_Entity();
			peopleMonster_Normal_Entity.BattleMgr = this;
			peopleMonster_Normal_Entity.SkillMgr = m_SkillManager;
			peopleMonster_Normal_Entity.StateMgr = m_StateManager;
			peopleMonster_Normal_Entity.SetBattleProps(data);
			peopleMonster_Normal_Entity.SetCtrl(component, data.ObjectIndex.ToString(), new Vector2(data.Init_X, data.Init_Y));
			peopleMonster_Normal_Entity.SetShenShiArea(data.AttackDistance);
			List<BaseSkillModel> list = new List<BaseSkillModel>();
			List<BaseSkillModel> list2 = new List<BaseSkillModel>();
			foreach (BaseSkillModel lifeSkill in data.LifeSkills)
			{
				if (lifeSkill.type == 1)
				{
					list.Add(lifeSkill);
				}
			}
			foreach (BaseSkillModel lifeSkill2 in data.LifeSkills)
			{
				if (lifeSkill2.type == 2)
				{
					list2.Add(lifeSkill2);
				}
			}
			peopleMonster_Normal_Entity.SetSkill(list);
			peopleMonster_Normal_Entity.SetWepon(list2);
			Sprite touXiangIcon = ResolveMonsterPortrait(data);
			peopleMonster_Normal_Entity.SetTouXiangIcon(touXiangIcon);
			peopleMonster_Normal_Entity.Idle();
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			m_AliveMonsterList.Add(data.ObjectIndex.ToString(), peopleMonster_Normal_Entity);
		}

		public void LoadNormalPeopleBossMonster(BattleLifeModel data)
		{
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			if (m_AliveMonsterList.ContainsKey(data.ObjectIndex.ToString()))
			{
				return;
			}
			if (m_NormalMonsterInsObj == null)
			{
				m_NormalMonsterInsObj = m_BattleSceneResourceManager.MonsterInsObj;
			}
			NormalMonsterController component = Object.Instantiate(m_NormalMonsterInsObj).GetComponent<NormalMonsterController>();
			PeopleMonster_HighOrder_Entity peopleMonster_HighOrder_Entity = new PeopleMonster_HighOrder_Entity();
			peopleMonster_HighOrder_Entity.BattleMgr = this;
			peopleMonster_HighOrder_Entity.SkillMgr = m_SkillManager;
			peopleMonster_HighOrder_Entity.StateMgr = m_StateManager;
			peopleMonster_HighOrder_Entity.SetBattleProps(data);
			peopleMonster_HighOrder_Entity.SetCtrl(component, data.ObjectIndex.ToString(), new Vector2(data.Init_X, data.Init_Y));
			peopleMonster_HighOrder_Entity.SetShenShiArea(data.AttackDistance);
			List<BaseSkillModel> list = new List<BaseSkillModel>();
			List<BaseSkillModel> list2 = new List<BaseSkillModel>();
			foreach (BaseSkillModel lifeSkill in data.LifeSkills)
			{
				if (lifeSkill.type == 1)
				{
					list.Add(lifeSkill);
				}
			}
			foreach (BaseSkillModel lifeSkill2 in data.LifeSkills)
			{
				if (lifeSkill2.type == 2)
				{
					list2.Add(lifeSkill2);
				}
			}
			peopleMonster_HighOrder_Entity.SetSkill(list);
			peopleMonster_HighOrder_Entity.SetWepon(list2);
			Sprite touXiangIcon = ResolveMonsterPortrait(data);
			peopleMonster_HighOrder_Entity.SetTouXiangIcon(touXiangIcon);
			peopleMonster_HighOrder_Entity.Idle();
			if (m_AliveMonsterList == null)
			{
				m_AliveMonsterList = new Dictionary<string, MonsterEntity>();
			}
			m_AliveMonsterList.Add(data.ObjectIndex.ToString(), peopleMonster_HighOrder_Entity);
		}

		public void LoadPlant(BattleNoLifeModel plantModel)
		{
			Object.Instantiate(m_BattleSceneResourceManager.PlantInsObj, new Vector3(plantModel.PosX, plantModel.PosY), new Quaternion(0f, 0f, 0f, 0f)).GetComponent<DiaoLuoItem>().Init(plantModel.ObjectIndex, plantModel.Name, plantModel.GoodsId);
		}

		public void LoadMineral(BattleNoLifeModel mineralModel)
		{
			Object.Instantiate(m_BattleSceneResourceManager.MineralInsObj, new Vector3(mineralModel.PosX, mineralModel.PosY), new Quaternion(0f, 0f, 0f, 0f)).GetComponent<DiaoLuoItem>().Init(mineralModel.ObjectIndex, mineralModel.Name, mineralModel.GoodsId);
		}

		public void LoadMiJing(BattleNoLifeModel miJingModel)
		{
			GameObject obj = Object.Instantiate(m_BattleSceneResourceManager.MiJingInsObj, new Vector3(miJingModel.PosX, miJingModel.PosY), new Quaternion(0f, 0f, 0f, 0f));
			obj.GetComponent<MiJingItem>().Init(miJingModel.ObjectIndex, miJingModel.Name);
			obj.GetComponent<DiaoLuoItem>().Init(miJingModel.ObjectIndex, miJingModel.Name, miJingModel.GoodsId);
		}

		public void ShiQuMiJing(GoodsArticleSecretAreaVO miJingModel)
		{
			if (miJingModel != null)
			{
				MonsterDiaoLuo(miJingModel);
			}
		}

		private void LoadPlayer(BattleLifeModel blfmodel)
		{
			GameObject obj = Object.Instantiate(m_BattleSceneResourceManager.PlayerInsObj);
			obj.transform.position = new Vector3(blfmodel.Init_X, blfmodel.Init_Y, 0f);
			PlayerController component = obj.GetComponent<PlayerController>();
			SelfPlayerEntity = new PlayerEntity();
			SelfPlayerEntity.BattleMgr = this;
			SelfPlayerEntity.SkillMgr = m_SkillManager;
			SelfPlayerEntity.StateMgr = m_StateManager;
			SelfPlayerEntity.SetCtrl(component, Singleton<RoleManager>.Instance.currentRoleId, new Vector3(blfmodel.Init_X, blfmodel.Init_Y));
			SelfPlayerEntity.SetBattleProps(blfmodel);
			SelfPlayerEntity.SetShenShiArea(blfmodel.AttackDistance);
			SelfPlayerEntity.Idle();
		}

		public void AddBuff(string id, int fiveElement, int icon, float time, int skillType)
		{
			if (m_BattleWnd == null)
			{
				m_BattleWnd = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
			}
			m_BattleWnd.AddPlayerBuff(id, fiveElement, icon, time, skillType);
		}

		public void CancleBuff(string id)
		{
			if (m_BattleWnd == null)
			{
				m_BattleWnd = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
			}
			m_BattleWnd.CanclePlayerBuff(id);
		}

		public void ClearAllBuff()
		{
			m_BattleWnd = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
			m_BattleWnd?.ClearAllBuff();
		}

		public void PlayerMove(Vector2 vector2)
		{
			if (SelfPlayerEntity.CurrentAniState != AniState.DingShen && SelfPlayerEntity.CurrentAniState != AniState.Die)
			{
				SelfPlayerEntity.ChaseTarget(vector2.normalized);
			}
		}

		public void PlayerTargetLocked(BattleLifeModel _target)
		{
		}

		public bool FaQiIsFlying(string id)
		{
			if (SelfPlayerEntity == null)
			{
				return false;
			}
			return SelfPlayerEntity.FaQiIsActive(id);
		}

		public void PlayerFaQiAttack(BaseSkillModel _BaseSkillModel, Battle_SkillItem battle_FaQiItem)
		{
			double usedPercent = (int)_BaseSkillModel.usedPerception;
			float num = (float)(double)_BaseSkillModel.castTime / 1000f;
			num = ((num <= 0f) ? 3f : num);
			float num2 = 1f;
			num *= num2;
			double num3 = 0.0;
			usedPercent -= num3;
			if (!SelfPlayerEntity.FaQiIsActive(_BaseSkillModel.id))
			{
				if ((double)SelfPlayerEntity.BattleModel.CurrentPerception < usedPercent)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("当前神识不足");
					return;
				}
				SelfPlayerEntity.ShiFa(num, () =>
				{
					SelfPlayerEntity.FaQiInit(_BaseSkillModel, battle_FaQiItem);
					if (battle_FaQiItem != null)
					{
						battle_FaQiItem.SetFaQiRunningAni(isShowAni: true);
					}
					BattleLifeModel battleModel = SelfPlayerEntity.BattleModel;
					battleModel.CurrentPerception = (double)battleModel.CurrentPerception - usedPercent;
				});
			}
			else
			{
				CloseFaQi(_BaseSkillModel.id, usedPercent, battle_FaQiItem);
			}
		}

		public void ClearAllMonsterTarget()
		{
			foreach (KeyValuePair<string, MonsterEntity> aliveMonster in AliveMonsterList)
			{
				if (aliveMonster.Value.CurrentAniState == AniState.Attack && aliveMonster.Value.CurrentAniState != AniState.GoBack)
				{
					aliveMonster.Value.GoBack();
				}
				aliveMonster.Value.ClearFoundAndLockMonster();
			}
		}

		public void CloseFaQi(string id, double usedPercent, Battle_SkillItem battle_FaQiItem)
		{
			BattleLifeModel battleModel = SelfPlayerEntity.BattleModel;
			battleModel.CurrentPerception = (double)battleModel.CurrentPerception + usedPercent;
			battle_FaQiItem.SetFaQiRunningAni(isShowAni: false);
			SelfPlayerEntity.CloseFaQq(id);
		}

		public void PlayerFaShuAttack(BaseSkillModel baseSkillModel, UnityAction CompeleteCallBack = null)
		{
			float num = (float)(double)baseSkillModel.castTime / 1000f;
			num = ((num <= 0f) ? 3f : num);
			float num2 = 1f;
			num *= num2;
			float zhenYuanXiaoHao = num * SelfPlayerEntity.GetGongFaPower();
			baseSkillModel.usedSuperAir = (int)zhenYuanXiaoHao;
			if ((double)SelfPlayerEntity.BattleModel.CurrentSuperAir < (double)zhenYuanXiaoHao)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("真元不足!");
				return;
			}
			switch ((FaShuType)baseSkillModel.skillType)
			{
			case FaShuType.gongji_suoding:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					if (SelfPlayerEntity.DanTiFaShuInit(baseSkillModel))
					{
						SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
						if (CompeleteCallBack != null)
						{
							CompeleteCallBack();
						}
					}
				});
				break;
			case FaShuType.gongji_chixu:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					if (SelfPlayerEntity.DotFaShuInit(baseSkillModel))
					{
						SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
						if (CompeleteCallBack != null)
						{
							CompeleteCallBack();
						}
					}
				});
				break;
			case FaShuType.gongji_fanwei_chixu:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					if (SelfPlayerEntity.AoeFaShuInit(baseSkillModel))
					{
						SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
						if (CompeleteCallBack != null)
						{
							CompeleteCallBack();
						}
					}
				});
				break;
			case FaShuType.fangyu_fashu_hudun:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					SelfPlayerEntity.HuDunFaShuInit(baseSkillModel);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					if (CompeleteCallBack != null)
					{
						CompeleteCallBack();
					}
				});
				break;
			case FaShuType.zengyi_shanghai:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					SelfPlayerEntity.Buff_AddDamageInit(baseSkillModel);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					if (CompeleteCallBack != null)
					{
						CompeleteCallBack();
					}
				});
				break;
			case FaShuType.zengyi_fangyu:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					SelfPlayerEntity.Buff_AddDefenceInit(baseSkillModel);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					if (CompeleteCallBack != null)
					{
						CompeleteCallBack();
					}
				});
				break;
			case FaShuType.kongzhi_yisu:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					if (SelfPlayerEntity.DeBuff_AddJianSu(baseSkillModel))
					{
						SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
						if (CompeleteCallBack != null)
						{
							CompeleteCallBack();
						}
					}
				});
				break;
			case FaShuType.kongzhi_xingdong:
				SelfPlayerEntity.ShiFa(num, () =>
				{
					if (SelfPlayerEntity.DeBuff_AddDingShen(baseSkillModel))
					{
						SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
						if (CompeleteCallBack != null)
						{
							CompeleteCallBack();
						}
					}
				});
				break;
			}
		}

		public void PlayerFuLuAttack(BaseSkillModel playerFuLu)
		{
			if (m_BattleType == BattleType.TongTianTaBattle)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("虚神塔内无法使用符箓！");
				return;
			}
			if ((int)playerFuLu.canUseCount <= 0)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("符箓可使用次数耗尽!");
				return;
			}
			--playerFuLu.canUseCount;
			double zhenYuanXiaoHao = (long)playerFuLu.usedSuperAir;
			float time = 0.5f;
			switch ((FaShuType)playerFuLu.skillType)
			{
			case FaShuType.gongji_suoding:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.DanTiFaShuInit(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.gongji_chixu:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.DotFaShuInit(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.gongji_fanwei_chixu:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.AoeFaShuInit(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.fangyu_fashu_hudun:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.HuDunFaShuInit(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.zengyi_shanghai:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.Buff_AddDamageInit(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.zengyi_fangyu:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.Buff_AddDefenceInit(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.kongzhi_yisu:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.DeBuff_AddJianSu(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			case FaShuType.kongzhi_xingdong:
				SelfPlayerEntity.ShiFa(time, () =>
				{
					SelfPlayerEntity.DeBuff_AddDingShen(playerFuLu, isFromFuLu: true);
					SelfPlayerEntity.XiaoHaoSuperAir(zhenYuanXiaoHao);
					SelfPlayerEntity.UseFuLu(playerFuLu.id);
				});
				break;
			}
		}

		public void PlayerRunningMatrix(BaseSkillModel playerZhenFa, UnityAction CompeleteCallBack = null)
		{
			float num = (float)(double)playerZhenFa.castTime / 1000f;
			num = ((num <= 0f) ? 3f : num);
			float num2 = 1f;
			num *= num2;
			float num3 = num * SelfPlayerEntity.GetGongFaPower();
			playerZhenFa.usedSuperAir = (int)num3;
			if ((double)SelfPlayerEntity.BattleModel.CurrentSuperAir < (double)num3)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("真元不足!");
				return;
			}
			SelfPlayerEntity.ShiFa(num, () =>
			{
				SelfPlayerEntity.ZhenFaInit(playerZhenFa);
			});
			switch ((BattleZhenFaType)playerZhenFa.skillType)
			{
			}
		}

		public void PlayerTargetCancleLocked()
		{
		}

		public void PlayerDead()
		{
			Timer.Register(0.5f, () =>
			{
				FailLeaveBattle();
			});
		}

		public void MonsterDead(string id)
		{
			SelfPlayerEntity.MonsterDead(id);
			MonsterEntity monsterEntity = null;
			BattleLifeModel value = null;
			if (m_AliveMonsterList.ContainsKey(id))
			{
				monsterEntity = m_AliveMonsterList[id];
				m_AliveMonsterList.Remove(id);
			}
			if (monsterEntity != null)
			{
				m_DeadMonsterList.Add(id, monsterEntity);
			}
			if (AliveMonsterModelDic.TryGetValue(id, out value))
			{
				AliveMonsterModelDic.Remove(id);
				if (value != null)
				{
					DeadMonsterModelDic.Add(id, value);
				}
				if (value.ObjectType % 1000 == 2 && value.Type == 4 && m_BattleType == BattleType.MapBattle)
				{
					RecordBossMonster();
				}
				if (m_BattleWnd == null)
				{
					m_BattleWnd = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
				}
				float killProgress = m_KillProgress;
				m_KillProgress = (float)DeadMonsterModelDic.Count * 1f / ((float)(DeadMonsterModelDic.Count + m_AliveMonsterList.Count) * 1f);
				if (killProgress < 0.35f && m_KillProgress >= 0.35f && m_BattleType == BattleType.MapBattle)
				{
					ShowTopAnimalMonster();
				}
				m_BattleWnd.ReflashDeadMonsterNum(m_KillProgress);
			}
			if (m_AliveMonsterList.Count <= 0)
			{
				m_RecordTimeBegin = false;
				SuccessLeaveBattle();
			}
		}

		private void RecordBossMonster()
		{
			if (m_BattleWnd == null)
			{
				m_BattleWnd = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
			}
			m_BattleWnd.KilledBoss();
		}

		public void LeaveBattle()
		{
			FailLeaveBattle();
			BattleEndLeaveReq_1004037 data = new BattleEndLeaveReq_1004037
			{
				BattleId = m_DungeonId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BattleEndLeaveReq, data, (object _obj) =>
			{
			});
		}

		private EndBattleInfoModel ZhanDouJieSuan()
		{
			return null;
		}

		private void FailLeaveBattle()
		{
			m_RecordTimeBegin = false;
			Singleton<BattleManager>.Instance.DestroyBattle();
			if (m_BattleType == BattleType.TongTianTaBattle)
			{
				m_BattleWnd.ChuangTaResult(isSuccess: false);
				return;
			}
			if (m_BattleType == BattleType.OpenZongMenBattle)
			{
				LiKaiBattle();
				return;
			}
			Singleton<UIManager>.Instance.PopUpWnd("ZhaoDouJieGuo.prefab", true, SelfPlayerEntity.BattleModel.ObjectIndex, m_DungeonId);
		}

		private void LiKaiBattle()
		{
			RoleLeaveDungeonReq_8001008 data = new RoleLeaveDungeonReq_8001008
			{
				DungeonId = m_DungeonId,
				RoleIndex = SelfPlayerEntity.BattleModel.ObjectIndex
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_LEAVE_DUNGEON, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, () =>
				{
					Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
				});
			}, isShowWaitUI: true, ServerType.BattleServer);
		}

		private void SuccessLeaveBattle()
		{
			m_RecordTimeBegin = false;
			if (m_BattleType == BattleType.TongTianTaBattle)
			{
				SelfPlayerEntity.ClearFoundAndLockMonster();
				Timer.Register(2f, () =>
				{
					m_BattleWnd?.ChuangTaResult(isSuccess: true);
				});
			}
		}

		public void TongTianTaNextFloor()
		{
			MapEntity mapEntity = new MapEntity();
			int result = 0;
			int.TryParse(m_MapDataSource.Id, out result);
			result++;
			mapEntity.Id = result.ToString();
			mapEntity.MapName = $"虚神塔{result}层";
			m_MapDataSource = mapEntity;
			m_MapManager.ReSetMonsterPosQue();
			SelfPlayerEntity.RecoverAllSuperAir();
			ContinueReqMonsterXST(result);
		}

		public void TimeOver()
		{
			m_RecordTimeBegin = false;
			MonoSingleton<GameStart>.Instance.PopUpAttention("战斗时间耗尽！", () =>
			{
				FailLeaveBattle();
			}, () =>
			{
				FailLeaveBattle();
			});
		}

		private void MonsterDiaoLuo(BaseNaJieGoods item)
		{
			if (item != null && m_BattleType != BattleType.TongTianTaBattle)
			{
				if (!DiaoLuoDic.ContainsKey(item.c))
				{
					DiaoLuoDic.Add(item.c, new List<BaseNaJieGoods> { item });
				}
				else if (!DiaoLuoDic[item.c].Contains(item))
				{
					DiaoLuoDic[item.c].Add(item);
				}
				PickUpGoods.Add(item);
				if (m_BattleWnd == null)
				{
					m_BattleWnd = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
				}
				m_BattleWnd.DiaoLuoShow(item.c);
			}
		}

		public float GetPlayerHorizontalPosPercent()
		{
			return (SelfPlayerEntity.GetPos().x + (float)MapMaxAndMin.maxX) / ((float)MapMaxAndMin.maxX * 2f);
		}

		public float GetPlayerVerticalPercent()
		{
			return (SelfPlayerEntity.GetPos().y + (float)MapMaxAndMin.maxY) / ((float)MapMaxAndMin.maxY * 2f);
		}

		public void MonsterDiaoLuo(string goodsId, int index, string name, int rank, float posX, float posY)
		{
			if (string.IsNullOrEmpty(goodsId))
			{
				Debug.Log("出现goodsID为空的情况:" + name);
				return;
			}
			GameObject gameObject = null;
			switch (rank)
			{
			case 1:
			case 2:
			case 3:
			case 4:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Huang.prefab");
				break;
			case 5:
			case 6:
			case 7:
			case 8:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Xuan.prefab");
				break;
			case 9:
			case 10:
			case 11:
			case 12:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Di.prefab");
				break;
			case 13:
			case 14:
			case 15:
			case 16:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Tian.prefab");
				break;
			}
			if (!(gameObject == null))
			{
				GameObject obj = Object.Instantiate(gameObject);
				obj.GetComponent<DiaoLuoItem>().Init(index, name, goodsId);
				obj.transform.position = new Vector2(posX, posY);
			}
		}

		public void MonsterDiaoLuoLingShi(string goodsId, int index, int LingShi, float posX, float posY)
		{
			if (LingShi > 0)
			{
				GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/LingShi.prefab");
				if (!(gameObject == null))
				{
					GameObject obj = Object.Instantiate(gameObject);
					obj.GetComponent<DiaoLuoItem>().Init(index, "灵石", goodsId, LingShi);
					obj.transform.position = new Vector2(posX, posY);
				}
			}
		}
	}
}
