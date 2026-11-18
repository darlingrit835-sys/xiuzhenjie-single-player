using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class RoleManager : Singleton<RoleManager>
	{
		public string ServerId;

		public string IphoneNumber;

		public string UserId;

		public string currentRoleId;

		public string gameServerIpManageId;

		private List<HuoDongEntity> m_EventInfoList;

		private Dictionary<int, DanShiLevelInfoModel> m_DanShiLevelDic;

		public RoleInfoEntity Role_Entity;

		public Dictionary<int, XzjBoxEntity> RoleBoxDic;

		private List<MapEntity> m_CityBuilds;

		private Dictionary<string, RoleLevelModel> m_LevelDic = new Dictionary<string, RoleLevelModel>();

		public UnityAction<double> DuanTiUpdateUiValueEvent;

		public UnityAction<double> LianShenUpdateUiValueEvent;

		public UnityAction<double> NaQiUpdateUiValueEvent;

		public UnityAction KeTuPoActionEvent;

		public UnityAction OneDayNaQiCompeleteEvent;

		public UnityAction OneDayDuanTiCompeleteEvent;

		public UnityAction OneDayLianShenCompeleteEvent;

		public List<HuoDongEntity> EventInfoList
		{
			get
			{
				return m_EventInfoList;
			}
			private set
			{
				m_EventInfoList = value;
			}
		}

		public List<MapEntity> CityBuilds
		{
			get
			{
				return m_CityBuilds;
			}
			private set
			{
				m_CityBuilds = value;
			}
		}

		public void Update()
		{
		}

		public int GetGuideProcessByType(int _type)
		{
			switch (_type)
			{
			case 1:
				if (Role_Entity.PracticeGuideVal != 0)
				{
					return Role_Entity.PracticeGuideVal;
				}
				return 1;
			case 2:
				if (Role_Entity.BattleGuideVal != 0)
				{
					return Role_Entity.BattleGuideVal;
				}
				return 1;
			default:
				return -1;
			}
		}

		public bool RoleIsDie()
		{
			return false;
		}

		public void CheckRoleDie()
		{
			if (RoleIsDie() && Singleton<UIManager>.Instance.FindWndByName<ChongShenWnd>("ChongShengWnd.prefab") == null)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("天地不仁，天劫无情。寿元已尽，只能坠入轮回", () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("ChongShengWnd.prefab", true);
				}, () =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("ChongShengWnd.prefab", true);
				});
			}
		}

		public bool CheckJingLi(int value)
		{
			if (Role_Entity.CurrentVigor >= value)
			{
				return true;
			}
			return false;
		}

		public void Init(string iphoneNumber, string userId, string serverId, string gameServerIpManageId, List<HuoDongEntity> eventInfoList, string roleId = "")
		{
			IphoneNumber = iphoneNumber;
			this.gameServerIpManageId = gameServerIpManageId;
			UserId = userId;
			ServerId = serverId;
			currentRoleId = roleId;
			m_LevelDic = Singleton<FacadeManager>.Instance.SelectRoleLevelInfo();
			m_DanShiLevelDic = Singleton<FacadeManager>.Instance.SelectDanShiLevelInfo();
			m_EventInfoList = eventInfoList;
			if (m_EventInfoList == null)
			{
				m_EventInfoList = new List<HuoDongEntity>();
			}
		}

		public void SetValue(RoleLoginResp_1002002 _data, List<MapEntity> _mapSource)
		{
			Role_Entity = new RoleInfoEntity(_data);
			RoleBoxDic = new Dictionary<int, XzjBoxEntity>();
			XzjBoxEntity value = new XzjBoxEntity(10);
			XzjBoxEntity value2 = new XzjBoxEntity(1);
			XzjBoxEntity value3 = new XzjBoxEntity(2);
			XzjBoxEntity value4 = new XzjBoxEntity(3);
			XzjBoxEntity value5 = new XzjBoxEntity(4);
			XzjBoxEntity value6 = new XzjBoxEntity(5);
			RoleBoxDic.Add(10, value);
			RoleBoxDic.Add(1, value2);
			RoleBoxDic.Add(2, value3);
			RoleBoxDic.Add(3, value4);
			RoleBoxDic.Add(4, value5);
			RoleBoxDic.Add(5, value6);
			CityBuilds = _mapSource;
			Singleton<GuideManager>.Instance.InitProgress(Role_Entity.PracticeGuideVal, Role_Entity.BattleGuideVal);
		}

		public int GetRoleLingGenNum()
		{
			int num = 0;
			if (Singleton<RoleManager>.Instance.Role_Entity.JinLingGen)
			{
				num++;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.MuLingGen)
			{
				num++;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.ShuiLingGen)
			{
				num++;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.HuoLingGen)
			{
				num++;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.TuLingGen)
			{
				num++;
			}
			return num;
		}

		public Sprite GetTouXiangSprite(int sex, string headPortraitId)
		{
			return GetTouXiangSprite(sex == 1, headPortraitId, isOnline: true);
		}

		public Sprite GetTouXiangSprite(bool isMan, string portraitId, bool isOnline)
		{
			string spriteName = PortraitName(isMan, portraitId) + (isOnline ? "" : "_offline");
			return Resources.Load<Sprite>("InkPortraits/" + spriteName)
				?? Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/TouXiang/" + spriteName + ".png");
		}

		public Sprite GetCurrentRoleSprite()
		{
			return GetTouXiangSprite(Role_Entity.Sex, Role_Entity.HeadPortraitId.ToString());
		}

		public string GetSprite(bool isMan, string touXiangId)
		{
			return "Assets/GameData/Textures/TouXiang/" + PortraitName(isMan, touXiangId) + ".png";
		}

		private static string PortraitName(bool isMan, string portraitId)
		{
			if (portraitId == null || portraitId.Length != 1 || portraitId[0] < '1' || portraitId[0] > '6')
			{
				portraitId = "1";
			}
			return (isMan ? "Man_" : "Women_") + portraitId;
		}

		public void NewLetterNotice(bool isNew = false)
		{
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("收到新的信件~");
			Singleton<UIManager>.Instance.FindWndByName<MainCityWnd>("MainCityWnd.prefab")?.ReflashMail(isNew);
		}

		public double ShengYuLingShi(double huaFei)
		{
			return (double)Role_Entity.ShinyStone - huaFei;
		}

		public double ShengYuLingYu(double huaFei)
		{
			return (double)Role_Entity.ShinyJade - huaFei;
		}

		public int GetRoleStage()
		{
			return (Role_Entity.RoleLevel - 1) / 10 + 1;
		}

		public int GetNextLevelRoleStage()
		{
			return (Role_Entity.RoleLevel + 1 - 1) / 10 + 1;
		}

		public double GetUpgradeByLevelId(string id)
		{
			RoleLevelModel value = new RoleLevelModel();
			if (m_LevelDic.TryGetValue(id, out value))
			{
				return value.upgradeLimit;
			}
			return 0.0;
		}

		public void HaoYunFuCompelete()
		{
			Singleton<RoleManager>.Instance.Role_Entity.ExtendQiYun = 0;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
		}

		public void JingLiAdd(ByteString byteString)
		{
			if (!(byteString == null))
			{
				VigorUpResp_9001001 vigorUpResp_ = new VigorUpResp_9001001();
				vigorUpResp_.MergeFrom(byteString);
				long num = vigorUpResp_.CurrentVigor - Role_Entity.CurrentVigor;
				Role_Entity.CurrentVigor = vigorUpResp_.CurrentVigor;
				if (num > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", "精力", num));
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void WuXingAdd(ByteString byteString)
		{
			if (!(byteString == null))
			{
				CleverUpResp_9001002 cleverUpResp_ = new CleverUpResp_9001002();
				cleverUpResp_.MergeFrom(byteString);
				long num = cleverUpResp_.CurrentClever - Role_Entity.CurrentClever;
				Role_Entity.CurrentClever = cleverUpResp_.CurrentClever;
				if (num > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", "悟性", num));
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void TiLiAdd(ByteString byteString)
		{
			if (!(byteString == null))
			{
				PowerUpResp_9001008 powerUpResp_ = new PowerUpResp_9001008();
				powerUpResp_.MergeFrom(byteString);
				long num = powerUpResp_.CurrentPower - Role_Entity.CurrentPower;
				Role_Entity.CurrentPower = powerUpResp_.CurrentPower;
				if (num > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", "体力", num));
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void DanDuRemoveDown(ByteString byteString)
		{
			if (!(byteString == null))
			{
				PosionDownResp_9001003 posionDownResp_ = new PosionDownResp_9001003();
				posionDownResp_.MergeFrom(byteString);
				long num = Role_Entity.CurrentPoisonVal - posionDownResp_.CurrentPosion;
				Role_Entity.CurrentPoisonVal = posionDownResp_.CurrentPosion;
				if (num > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}降低{1}点", "丹毒", num));
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void EveryDayAddSuperAir(ByteString byteString)
		{
			if (byteString == null)
			{
				return;
			}
			SuperAirUpResp_9001004 superAirUpResp_ = new SuperAirUpResp_9001004();
			superAirUpResp_.MergeFrom(byteString);
			if (Role_Entity == null)
			{
				return;
			}
			float num = superAirUpResp_.CurrentSuperAir - Role_Entity.CurrentSuperAir;
			if (num > 0f)
			{
				string arg = "灵气";
				if (Role_Entity.RoleLevel < 31)
				{
					arg = "灵气";
				}
				else if (Role_Entity.RoleLevel < 61)
				{
					num /= 100f;
					arg = "真气";
				}
				else if (Role_Entity.RoleLevel >= 61)
				{
					num = num / 100f / 100f;
					arg = "真元";
				}
				if (num >= 1f && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", arg, num.ToString("0.0")));
				}
			}
			Role_Entity.SuperAir = superAirUpResp_.SuperAir;
			Role_Entity.CurrentSuperAir = superAirUpResp_.CurrentSuperAir;
			DongFuWnd dongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
			dongFuWnd?.ReflashDanYaoLianHua(GongFaType.NaQi, superAirUpResp_.SuperAirRefiningValue);
			dongFuWnd?.ReflashGongFaValue(GongFaType.NaQi, superAirUpResp_.CurrentRunValue);
			if (superAirUpResp_.IsBreak && KeTuPoActionEvent != null)
			{
				KeTuPoActionEvent();
			}
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
		}

		public void EveryDayAddPhysique(ByteString byteString)
		{
			if (!(byteString == null))
			{
				PhysiqueUpResp_9001005 physiqueUpResp_ = new PhysiqueUpResp_9001005();
				physiqueUpResp_.MergeFrom(byteString);
				long num = physiqueUpResp_.CurrentPhysique - Role_Entity.CurrentPhysique;
				int num2 = physiqueUpResp_.CurrentPureBlood - Role_Entity.CurrentPureBlood;
				Role_Entity.Physique = physiqueUpResp_.Physique;
				Role_Entity.CurrentPhysique = physiqueUpResp_.CurrentPhysique;
				Role_Entity.PureBlood = physiqueUpResp_.PureBlood;
				Role_Entity.CurrentPureBlood = physiqueUpResp_.CurrentPureBlood;
				if (num > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", "体魄", num));
				}
				if (num2 > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", "精血", num2));
				}
				DongFuWnd dongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
				dongFuWnd?.ReflashDanYaoLianHua(GongFaType.DuanTi, physiqueUpResp_.PhysiqueRefiningValue);
				dongFuWnd?.ReflashGongFaValue(GongFaType.DuanTi, physiqueUpResp_.CurrentRunValue);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void EveryDayAddPercent(ByteString byteString)
		{
			if (!(byteString == null))
			{
				PerceptionUpResp_9001006 perceptionUpResp_ = new PerceptionUpResp_9001006();
				perceptionUpResp_.MergeFrom(byteString);
				long num = perceptionUpResp_.CurrentPerception - Role_Entity.Perception;
				Role_Entity.Perception = perceptionUpResp_.Perception;
				Role_Entity.CurrentPerception = perceptionUpResp_.CurrentPerception;
				if (num > 0 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}点", "神识", num));
				}
				DongFuWnd dongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
				dongFuWnd?.ReflashDanYaoLianHua(GongFaType.LianShen, perceptionUpResp_.PerceptionRefiningValue);
				dongFuWnd?.ReflashGongFaValue(GongFaType.LianShen, perceptionUpResp_.CurrentRunValue);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void EveryDayAddAge(ByteString byteString)
		{
			if (!(byteString == null))
			{
				AgeUpResp_9001007 ageUpResp_ = new AgeUpResp_9001007();
				ageUpResp_.MergeFrom(byteString);
				int num = ageUpResp_.Age - Role_Entity.CurrentAge;
				Role_Entity.MaxAge = ageUpResp_.MaxAge;
				Role_Entity.CurrentAge = ageUpResp_.Age;
				if (num >= 1 && Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("{0}增长{1}岁", "年龄", num));
				}
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		public void SetNewMapInfo(List<MapEntity> _mapSource, string mapId, string mapName)
		{
			Singleton<RoleManager>.Instance.Role_Entity.MapId = mapId;
			Singleton<RoleManager>.Instance.Role_Entity.MapName = mapName;
			CityBuilds = _mapSource;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			MonoSingleton<GameStart>.Instance.PopRollingTextShow($"到达了{mapName}");
		}

		public string GetCurrentFuShiLevelName()
		{
			Dictionary<int, XzjWorldAssessSign> dictionary = Singleton<FacadeManager>.Instance.SelectSignLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldAssessSign value = null;
			if (Role_Entity.Live1Type == 3)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			if (Role_Entity.Live2Type == 3)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			return "无";
		}

		public int GetFuShiCurrentExp()
		{
			if (Role_Entity.Live1Type == 3)
			{
				return Role_Entity.Live1Value;
			}
			if (Role_Entity.Live2Type == 3)
			{
				return Role_Entity.Live2Value;
			}
			return 0;
		}

		public string GetFuShiMaxExp()
		{
			Dictionary<int, XzjWorldAssessSign> dictionary = Singleton<FacadeManager>.Instance.SelectSignLevelInfo();
			XzjWorldAssessSign value = null;
			if (Role_Entity.Live1Type == 3)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.drawingSignTopValue.ToString("f0");
				}
				return "100";
			}
			if (Role_Entity.Live2Type == 3)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.drawingSignTopValue.ToString("f0");
				}
				return "100";
			}
			return "100";
		}

		public bool IsFuShi()
		{
			if (Role_Entity.Live1Type == 3)
			{
				return true;
			}
			if (Role_Entity.Live2Type == 3)
			{
				return true;
			}
			return false;
		}

		public string GetCurrentLianQiShiLevelName()
		{
			Dictionary<int, XzjWorldAsserssLianQi> dictionary = Singleton<FacadeManager>.Instance.SelectLianQiLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldAsserssLianQi value = null;
			if (Role_Entity.Live1Type == 2)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			if (Role_Entity.Live2Type == 2)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			return "无";
		}

		public int GetCurrentLianQiShiLevel()
		{
			if (Role_Entity.Live1Type == 2)
			{
				return Role_Entity.Live1Level;
			}
			if (Role_Entity.Live2Type == 2)
			{
				return Role_Entity.Live2Level;
			}
			return 0;
		}

		public int GetLianQiShiCurrentExp()
		{
			if (Role_Entity.Live1Type == 2)
			{
				return Role_Entity.Live1Value;
			}
			if (Role_Entity.Live2Type == 2)
			{
				return Role_Entity.Live2Value;
			}
			return 0;
		}

		public string GetLianQiShiMaxExp()
		{
			Dictionary<int, XzjWorldAsserssLianQi> dictionary = Singleton<FacadeManager>.Instance.SelectLianQiLevelInfo();
			XzjWorldAsserssLianQi value = null;
			if (Role_Entity.Live1Type == 2)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.plantTopValue.ToString("0");
				}
				return "100";
			}
			if (Role_Entity.Live2Type == 2)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.plantTopValue.ToString("0");
				}
				return "100";
			}
			return "100";
		}

		public bool IsLianQiShi()
		{
			if (Role_Entity.Live1Type == 2)
			{
				return true;
			}
			if (Role_Entity.Live2Type == 2)
			{
				return true;
			}
			return false;
		}

		public string GetCurrentZhenFaShiLevelName()
		{
			Dictionary<int, XzjWorldAsserssZhenFa> dictionary = Singleton<FacadeManager>.Instance.SelectZhenFaLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldAsserssZhenFa value = null;
			if (Role_Entity.Live1Type == 5)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			if (Role_Entity.Live2Type == 5)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			return "无";
		}

		public int GetCurrentZhenFaShiLevel()
		{
			if (Role_Entity.Live1Type == 5)
			{
				return Role_Entity.Live1Level;
			}
			if (Role_Entity.Live2Type == 5)
			{
				return Role_Entity.Live2Level;
			}
			return 0;
		}

		public int GetZhenFaCurrentExp()
		{
			if (Role_Entity.Live1Type == 5)
			{
				return Role_Entity.Live1Value;
			}
			if (Role_Entity.Live2Type == 5)
			{
				return Role_Entity.Live2Value;
			}
			return 0;
		}

		public string GetZhenFaShiMaxExp()
		{
			Dictionary<int, XzjWorldAsserssZhenFa> dictionary = Singleton<FacadeManager>.Instance.SelectZhenFaLevelInfo();
			XzjWorldAsserssZhenFa value = null;
			if (Role_Entity.Live1Type == 5)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.plantTopValue.ToString("0");
				}
				return "100";
			}
			if (Role_Entity.Live2Type == 5)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.plantTopValue.ToString("0");
				}
				return "100";
			}
			return "100";
		}

		public bool IsZhenFaShi()
		{
			if (Role_Entity.Live1Type == 5)
			{
				return true;
			}
			if (Role_Entity.Live2Type == 5)
			{
				return true;
			}
			return false;
		}

		public string GetCurrentShuShiLevelName()
		{
			Dictionary<int, XzjWorldAssessMagicModel> dictionary = Singleton<FacadeManager>.Instance.SelectShuShiLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldAssessMagicModel value = null;
			if (Role_Entity.Live1Type == 4)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			if (Role_Entity.Live2Type == 4)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			return "无";
		}

		public int GetShuShiLevel()
		{
			if (Role_Entity.Live1Type == 4)
			{
				return Role_Entity.Live1Level;
			}
			if (Role_Entity.Live2Type == 4)
			{
				return Role_Entity.Live2Level;
			}
			return 0;
		}

		public int GetShuShiCurrentExp()
		{
			if (Role_Entity.Live1Type == 4)
			{
				return Role_Entity.Live1Value;
			}
			if (Role_Entity.Live2Type == 4)
			{
				return Role_Entity.Live2Value;
			}
			return 0;
		}

		public string GetShuShiMaxExp()
		{
			Dictionary<int, XzjWorldAssessMagicModel> dictionary = Singleton<FacadeManager>.Instance.SelectShuShiLevelInfo();
			XzjWorldAssessMagicModel value = null;
			if (Role_Entity.Live1Type == 4)
			{
				if (dictionary.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.magicLevelTopValue.ToString("f0");
				}
				return "100";
			}
			if (Role_Entity.Live2Type == 4)
			{
				if (dictionary.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.magicLevelTopValue.ToString("f0");
				}
				return "100";
			}
			return "100";
		}

		public bool IsShuShi()
		{
			if (Role_Entity.Live1Type == 4)
			{
				return true;
			}
			if (Role_Entity.Live2Type == 4)
			{
				return true;
			}
			return false;
		}

		public string GetCurrentBuDaoLevelName()
		{
			Dictionary<int, XzjWorldAsserssAchievement> dictionary = Singleton<FacadeManager>.Instance.SelectBuDaoLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldAsserssAchievement value = null;
			if (Role_Entity.Live4Type == 6)
			{
				if (dictionary.TryGetValue(Role_Entity.Live4Level, out value))
				{
					return value.name;
				}
				return "无";
			}
			return "无";
		}

		public bool IsBuDao()
		{
			if (Role_Entity.Live4Type == 6)
			{
				return true;
			}
			return false;
		}

		public int GetBuDaoLevel()
		{
			if (Role_Entity.Live4Type == 6)
			{
				return Role_Entity.Live4Level;
			}
			return 0;
		}

		public int GetBuDaoCurrentExp()
		{
			if (Role_Entity.Live4Type == 6)
			{
				return Role_Entity.Live4Value;
			}
			return 0;
		}

		public string GetBuDaoMaxExp()
		{
			Dictionary<int, XzjWorldAsserssAchievement> dictionary = Singleton<FacadeManager>.Instance.SelectBuDaoLevelInfo();
			XzjWorldAsserssAchievement value = null;
			if (Role_Entity.Live4Type == 6)
			{
				if (dictionary.TryGetValue(Role_Entity.Live4Level, out value))
				{
					return value.teacherTopValue.ToString("0");
				}
				return "100";
			}
			return "100";
		}

		public string GetCurrentDanShiLevelName()
		{
			Dictionary<int, DanShiLevelInfoModel> danShiLevelDic = m_DanShiLevelDic;
			if (danShiLevelDic == null)
			{
				return "无";
			}
			DanShiLevelInfoModel value = null;
			int num = 0;
			if (Role_Entity.Live1Type == 1)
			{
				num = Role_Entity.Live1Level;
			}
			else if (Role_Entity.Live2Type == 1)
			{
				num = Role_Entity.Live2Level;
			}
			if (num == 0)
			{
				return "无";
			}
			if (danShiLevelDic.TryGetValue(num, out value))
			{
				return value.name;
			}
			return "无";
		}

		public int GetCurrentDanShiLevel()
		{
			int result = 0;
			if (Role_Entity.Live1Type == 1)
			{
				result = Role_Entity.Live1Level;
			}
			else if (Role_Entity.Live2Type == 1)
			{
				result = Role_Entity.Live2Level;
			}
			return result;
		}

		public string GetNextDanShiLevelName()
		{
			Dictionary<int, DanShiLevelInfoModel> danShiLevelDic = m_DanShiLevelDic;
			if (danShiLevelDic == null)
			{
				return "无";
			}
			DanShiLevelInfoModel value = null;
			int num = 0;
			if (Role_Entity.Live1Type == 1)
			{
				num = Role_Entity.Live1Level + 1;
			}
			else if (Role_Entity.Live2Type == 1)
			{
				num = Role_Entity.Live2Level + 1;
			}
			if (num > 9)
			{
				num = 9;
			}
			if (danShiLevelDic.TryGetValue(num, out value))
			{
				return value.name;
			}
			return "无";
		}

		public string GetLianDanCurrentExp()
		{
			if (Role_Entity.Live1Type == 1)
			{
				return Role_Entity.Live1Value.ToString();
			}
			if (Role_Entity.Live2Type == 1)
			{
				return Role_Entity.Live2Value.ToString();
			}
			return "0";
		}

		public void SetFuZhiYeCurrentExp(int level, int value, RoleFuZhiType roleFuZhiType)
		{
			if (Role_Entity.Live1Type == (int)roleFuZhiType)
			{
				Role_Entity.Live1Level = level;
				Role_Entity.Live1Value = value;
			}
			else if (Role_Entity.Live2Type == (int)roleFuZhiType)
			{
				Role_Entity.Live2Level = level;
				Role_Entity.Live2Value = value;
			}
		}

		public string GetLianDanMaxExp()
		{
			DanShiLevelInfoModel value = null;
			if (Role_Entity.Live1Type == 1)
			{
				if (m_DanShiLevelDic.TryGetValue(Role_Entity.Live1Level, out value))
				{
					return value.practiceElixirTopValue.ToString("f0");
				}
				return "100";
			}
			if (Role_Entity.Live2Type == 1)
			{
				if (m_DanShiLevelDic.TryGetValue(Role_Entity.Live2Level, out value))
				{
					return value.practiceElixirTopValue.ToString("f0");
				}
				return "100";
			}
			return "100";
		}

		public bool IsDanShi()
		{
			if (Role_Entity.Live1Type == 1)
			{
				return true;
			}
			if (Role_Entity.Live2Type == 1)
			{
				return true;
			}
			return false;
		}

		public string GetCurrentLingZhiLevelName()
		{
			Dictionary<int, XzjWorldPlant> dictionary = Singleton<FacadeManager>.Instance.SelectPlantLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldPlant value = null;
			int num = 0;
			if (Role_Entity.Live3Type == 8)
			{
				num = Role_Entity.Live3Level;
			}
			else if (Role_Entity.Live3Type == 8)
			{
				num = Role_Entity.Live2Level;
			}
			if (num == 0)
			{
				return "无";
			}
			if (dictionary.TryGetValue(num, out value))
			{
				return value.name;
			}
			return "无";
		}

		public int GetCurrentLingZhiLevel()
		{
			int result = 0;
			if (Role_Entity.Live3Type == 8)
			{
				result = Role_Entity.Live3Level;
			}
			else if (Role_Entity.Live3Type == 8)
			{
				result = Role_Entity.Live3Level;
			}
			return result;
		}

		public string GetNextLingZhiLevelName()
		{
			Dictionary<int, XzjWorldPlant> dictionary = Singleton<FacadeManager>.Instance.SelectPlantLevelInfo();
			if (dictionary == null)
			{
				return "无";
			}
			XzjWorldPlant value = null;
			int num = 0;
			if (Role_Entity.Live3Type == 8)
			{
				num = Role_Entity.Live3Level + 1;
			}
			else if (Role_Entity.Live3Type == 8)
			{
				num = Role_Entity.Live3Level + 1;
			}
			if (num > 9)
			{
				num = 9;
			}
			if (dictionary.TryGetValue(num, out value))
			{
				return value.name;
			}
			return "无";
		}

		public string GetLingZhiCurrentExp()
		{
			if (Role_Entity.Live3Type == 8)
			{
				return Role_Entity.Live3Value.ToString();
			}
			if (Role_Entity.Live3Type == 8)
			{
				return Role_Entity.Live3Value.ToString();
			}
			return "0";
		}

		public void SetLingZhiCurrentExp(int level, int value)
		{
			if (Role_Entity.Live3Type == 8)
			{
				Role_Entity.Live3Level = level;
				Role_Entity.Live3Value = value;
			}
			else if (Role_Entity.Live3Type == 8)
			{
				Role_Entity.Live3Level = level;
				Role_Entity.Live3Value = value;
			}
		}

		public string GetLingZhiMaxExp()
		{
			XzjWorldPlant value = null;
			Dictionary<int, XzjWorldPlant> dictionary = Singleton<FacadeManager>.Instance.SelectPlantLevelInfo();
			if (Role_Entity.Live3Type == 8)
			{
				if (dictionary.TryGetValue(Role_Entity.Live3Level, out value))
				{
					return value.plantTopValue.ToString("f0");
				}
				return "100";
			}
			if (Role_Entity.Live3Type == 8)
			{
				if (dictionary.TryGetValue(Role_Entity.Live3Level, out value))
				{
					return value.plantTopValue.ToString("f0");
				}
				return "100";
			}
			return "100";
		}

		public bool IsLingZhi()
		{
			if (Role_Entity.Live3Type == 8)
			{
				return true;
			}
			if (Role_Entity.Live3Type == 8)
			{
				return true;
			}
			return false;
		}

		public void ClearData()
		{
			LianShenUpdateUiValueEvent = null;
			OneDayLianShenCompeleteEvent = null;
			DuanTiUpdateUiValueEvent = null;
			OneDayDuanTiCompeleteEvent = null;
			OneDayNaQiCompeleteEvent = null;
			NaQiUpdateUiValueEvent = null;
			KeTuPoActionEvent = null;
			m_LevelDic = null;
			CityBuilds = null;
			RoleBoxDic = null;
			Role_Entity = null;
		}

		public void SelectTemplateMaterialAnimalDetail(string templateId, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			TemplateMaterialAnimalDetailReq_1011001 data = new TemplateMaterialAnimalDetailReq_1011001
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_MATERIAL_ANIMAL_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					TemplateMaterialAnimalDetailResp_1011001 templateMaterialAnimalDetailResp_ = new TemplateMaterialAnimalDetailResp_1011001();
					templateMaterialAnimalDetailResp_.MergeFrom(byteString);
					if (templateMaterialAnimalDetailResp_.IsSuccess)
					{
						dataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(templateMaterialAnimalDetailResp_.Stage), "材料的等级，等级越高的材料价值越大"));
						dataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)templateMaterialAnimalDetailResp_.FiveElement), "分金、木、水、火、土五行"));
						dataList.Add(string.Format("{0},{1},{2}", "产地", templateMaterialAnimalDetailResp_.MapName, "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料"));
						dataList.Add(string.Format("{0},{1},{2}", "四气", WorldSetting.SiQiName(templateMaterialAnimalDetailResp_.FourGas), "四气包括：温、热、寒、凉，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "五味", WorldSetting.WuWeiName(templateMaterialAnimalDetailResp_.FiveFlavour), "五味包括：辛、甘、苦、咸、酸，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "补泻", WorldSetting.BuXieName(templateMaterialAnimalDetailResp_.ReinforcingDiarrhoea), "补泻包括：补、泻，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "归经", WorldSetting.GuiJingName(templateMaterialAnimalDetailResp_.AffiliationAcupoint), "归经包括：手太阳、手少阳、手太阴、手少阴、手明阳、手厥阴、足太阳、足少阳、足太阴、足少阴、足阳明、足厥阴，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "坚度", WorldSetting.JianDuName(templateMaterialAnimalDetailResp_.Intensity), "坚度即材料的硬度，越坚硬则越脆，是炼器中需要使用的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "韧度", WorldSetting.RenDuName(templateMaterialAnimalDetailResp_.Tenacity), "韧度级材料的柔韧性，越柔韧则可弯折的角度越大，是炼器中需要使用的属性"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(templateMaterialAnimalDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		public void SelectTemplateMaterialMineralDetail(string templateId, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			TemplateMaterialMineralDetailReq_1011002 data = new TemplateMaterialMineralDetailReq_1011002
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_MATERIAL_MINERAL_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					TemplateMaterialMineralDetailResp_1011002 templateMaterialMineralDetailResp_ = new TemplateMaterialMineralDetailResp_1011002();
					templateMaterialMineralDetailResp_.MergeFrom(byteString);
					if (templateMaterialMineralDetailResp_.IsSuccess)
					{
						dataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(templateMaterialMineralDetailResp_.Stage), "材料的等级，等级越高的材料价值越大"));
						dataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)templateMaterialMineralDetailResp_.FiveElement), "分金、木、水、火、土五行"));
						dataList.Add(string.Format("{0},{1},{2}", "产地", templateMaterialMineralDetailResp_.MapName, "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料"));
						dataList.Add(string.Format("{0},{1},{2}", "坚度", WorldSetting.JianDuName(templateMaterialMineralDetailResp_.Intensity), "坚度即材料的硬度，越坚硬则越脆，是炼器中需要使用的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "韧度", WorldSetting.RenDuName(templateMaterialMineralDetailResp_.Tenacity), "韧度级材料的柔韧性，越柔韧则可弯折的角度越大，是炼器中需要使用的属性"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(templateMaterialMineralDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		public void SelectTemplatePlantDetail(string templateId, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			TemplateMaterialPlantDetailReq_1011003 data = new TemplateMaterialPlantDetailReq_1011003
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_MATERIAL_PLANT_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					TemplateMaterialPlantDetailResp_1011003 templateMaterialPlantDetailResp_ = new TemplateMaterialPlantDetailResp_1011003();
					templateMaterialPlantDetailResp_.MergeFrom(byteString);
					if (templateMaterialPlantDetailResp_.IsSuccess)
					{
						dataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(templateMaterialPlantDetailResp_.Stage), "材料的等级，等级越高的材料价值越大"));
						dataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)templateMaterialPlantDetailResp_.FiveElement), "分金、木、水、火、土五行"));
						dataList.Add((templateMaterialPlantDetailResp_.MapName != "") ? string.Format("{0},{1},{2}", "产地", templateMaterialPlantDetailResp_.MapName, "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料") : string.Format("{0},{1},{2}", "产地", "虚神界", "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料"));
						dataList.Add(string.Format("{0},{1},{2}", "四气", WorldSetting.SiQiName(templateMaterialPlantDetailResp_.FourGas), "四气包括：温、热、寒、凉，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "五味", WorldSetting.WuWeiName(templateMaterialPlantDetailResp_.FiveFlavour), "五味包括：辛、甘、苦、咸、酸，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "补泻", WorldSetting.BuXieName(templateMaterialPlantDetailResp_.ReinforcingDiarrhoea), "补泻包括：补、泻，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "归经", WorldSetting.GuiJingName(templateMaterialPlantDetailResp_.AffiliationAcupoint), "归经包括：手太阳、手少阳、手太阴、手少阴、手明阳、手厥阴、足太阳、足少阳、足太阴、足少阴、足阳明、足厥阴，是丹方的配伍中需要参考的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "坚度", WorldSetting.JianDuName(templateMaterialPlantDetailResp_.Intensity), "坚度即材料的硬度，越坚硬则越脆，是炼器中需要使用的属性"));
						dataList.Add(string.Format("{0},{1},{2}", "韧度", WorldSetting.RenDuName(templateMaterialPlantDetailResp_.Tenacity), "韧度级材料的柔韧性，越柔韧则可弯折的角度越大，是炼器中需要使用的属性"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(templateMaterialPlantDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		public void SelectTemplateAchievementDetail(string templateId, int level, int rank, int fiveElement, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			if (string.IsNullOrEmpty(templateId))
			{
				return;
			}
			List<string> dataList = new List<string>();
			MethodAchievementDetailReq_1011004 data = new MethodAchievementDetailReq_1011004
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_ACHIEVEMENT_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MethodAchievementDetailResp_1011004 methodAchievementDetailResp_ = new MethodAchievementDetailResp_1011004();
					methodAchievementDetailResp_.MergeFrom(byteString);
					if (methodAchievementDetailResp_.IsSuccess)
					{
						switch ((GongFaType)methodAchievementDetailResp_.ActionScope)
						{
						case GongFaType.None:
							dataList.Add(string.Format("{0},{1},{2}", "功法类型", "未知", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						case GongFaType.LianShen:
							dataList.Add(string.Format("{0},{1},{2}", "功法类型", "炼神", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						case GongFaType.DuanTi:
							dataList.Add(string.Format("{0},{1},{2}", "功法类型", "锻体", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						case GongFaType.NaQi:
							dataList.Add(string.Format("{0},{1},{2}", "功法类型", "纳气", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						}
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						dataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(stageByLevel)));
						dataList.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(rank)));
						dataList.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement)));
						dataList.Add(string.Format("{0},{1},{2}", "灵衍值", methodAchievementDetailResp_.ConversionRate, "吸纳外界灵气转换为自身丹田内灵气的比值，灵衍值越高则转换效率越高，功法效果越强"));
						if (methodAchievementDetailResp_.EffectType1 != 0)
						{
							string title = "";
							string gongFaEffectType = WorldSetting.GetGongFaEffectType(methodAchievementDetailResp_.EffectType1, ref title);
							dataList.Add(title + "," + gongFaEffectType + methodAchievementDetailResp_.EffectValue1 + "%,功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。");
						}
						if (methodAchievementDetailResp_.EffectType2 != 0)
						{
							string title2 = "";
							string gongFaEffectType2 = WorldSetting.GetGongFaEffectType(methodAchievementDetailResp_.EffectType2, ref title2);
							dataList.Add(title2 + "," + gongFaEffectType2 + methodAchievementDetailResp_.EffectValue2 + "%,功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。");
						}
						if (methodAchievementDetailResp_.EffectType3 != 0)
						{
							string title3 = "";
							string gongFaEffectType3 = WorldSetting.GetGongFaEffectType(methodAchievementDetailResp_.EffectType3, ref title3);
							dataList.Add(title3 + "," + gongFaEffectType3 + methodAchievementDetailResp_.EffectValue3 + "%,功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。");
						}
						dataList.Add(string.Format("{0},{1},{2}", "创造人", methodAchievementDetailResp_.CreatedBy, "此功法由谁创造而来，开辟悟道台成为布道师，结丹期后在虚神界即可创造功法，创造一本自己署名的功法流芳百世"));
						string text = "";
						switch (methodAchievementDetailResp_.StudyLevel)
						{
						case 1:
							text = "一";
							break;
						case 2:
							text = "二";
							break;
						case 3:
							text = "三";
							break;
						}
						dataList.Add("功法重数,第" + text + "重");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodAchievementDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		public void SelectTemplateElixirDetail(string templateId, int level, int rank, int fiveElement, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			MethodElixirDetailReq_1011005 data = new MethodElixirDetailReq_1011005
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_ELIXIR_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MethodElixirDetailResp_1011005 methodElixirDetailResp_ = new MethodElixirDetailResp_1011005();
					methodElixirDetailResp_.MergeFrom(byteString);
					if (methodElixirDetailResp_.IsSuccess)
					{
						string effectString = GetEffectString(methodElixirDetailResp_.EffectType);
						string[] array = effectString.Split(',');
						effectString = string.Format("{0},{1},{2}", array[0], array[1], "丹药共有三种大类：恢复类、增长类、特殊类，恢复类用于恢复渡劫损伤，增长类用于增长属性（有些需要炼化），特殊类则是由玄天楼出售的丹药");
						dataList.Add(effectString);
						dataList.Add(string.Format("{0},{1},{2}", "学习所需的丹师等级", methodElixirDetailResp_.ElixirLevel, "每一阶丹方都需要对应的丹师等级，否则将难以学习或参悟"));
						string effectStringValue = GetEffectStringValue("药效", methodElixirDetailResp_.EffectType, methodElixirDetailResp_.EffectValue.ToString());
						if (!string.IsNullOrEmpty(effectStringValue))
						{
							dataList.Add(effectStringValue);
						}
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						dataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(stageByLevel)));
						dataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(methodElixirDetailResp_.Rank), "此丹方所炼制出来的丹药能获得的最小品阶"));
						dataList.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement)));
						if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
						{
							dataList.Add(string.Format("{0},{1},{2}", "服用所需灵气值", methodElixirDetailResp_.SpiritualityValue, "服用此丹药要求的最低灵气值，自身修为灵气值低于此值，将无法服用"));
						}
						else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
						{
							dataList.Add(string.Format("{0},{1},{2}", "服用所需真气值", methodElixirDetailResp_.SpiritualityValue / 100, "服用此丹药要求的最低真气值，自身修为灵气值低于此值，将无法服用"));
						}
						else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
						{
							dataList.Add(string.Format("{0},{1},{2}", "服用所需真元值", methodElixirDetailResp_.SpiritualityValue / 100 / 100, "服用此丹药要求的最低真元值，自身修为灵气值低于此值，将无法服用"));
						}
						dataList.Add(string.Format("{0},{1},{2}", "丹毒", methodElixirDetailResp_.PoisonValue, "此丹方所炼制出来的丹药能获得的最小丹毒"));
						dataList.Add(string.Format("{0},{1},{2}", "炼制步骤数", methodElixirDetailResp_.StepCount, "炼制此丹药一共所需的步骤数，等阶越高的丹药炼制步骤越繁琐"));
						for (int i = 0; i < methodElixirDetailResp_.MaterialNames.Count; i++)
						{
							dataList.Add(string.Format("{0},{1}", "材料" + (i + 1), methodElixirDetailResp_.MaterialNames[i]));
						}
						dataList.Add(string.Format("{0},{1},{2}", "创造人", methodElixirDetailResp_.CreatedBy, "此丹方由谁创造而来，开辟炼丹房成为丹师，炼制丹药即可使用或者售卖，结丹期后进入虚神界可以创造丹方"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodElixirDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		public void SelectTemplateDanYaoDetail(string templateId, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			MethodElixirDetailReq_1011005 data = new MethodElixirDetailReq_1011005
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_ELIXIR_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MethodElixirDetailResp_1011005 methodElixirDetailResp_ = new MethodElixirDetailResp_1011005();
					methodElixirDetailResp_.MergeFrom(byteString);
					if (methodElixirDetailResp_.IsSuccess)
					{
						string effectString = GetEffectString(methodElixirDetailResp_.EffectType);
						string[] array = effectString.Split(',');
						effectString = string.Format("{0},{1},{2}", array[0], array[1], "丹药共有三种大类：恢复类、增长类、特殊类，恢复类用于恢复渡劫损伤，增长类用于增长属性（有些需要炼化），特殊类则是由玄天楼出售的丹药");
						dataList.Add(effectString);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodElixirDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		private string GetEffectString(int effectType)
		{
			switch ((DanYaoEffect)effectType)
			{
			case DanYaoEffect.None:
				return string.Format("{0},{1}", "丹药类型", "未知");
			case DanYaoEffect.HUIFU_TIPO:
				return string.Format("{0},{1}", "丹药类型", "恢复类-体魄");
			case DanYaoEffect.HUIFU_SHENSHI:
				return string.Format("{0},{1}", "丹药类型", "恢复类-神识");
			case DanYaoEffect.HUIFU_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return string.Format("{0},{1}", "丹药类型", "恢复类-灵气");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					return string.Format("{0},{1}", "丹药类型", "恢复类-真气");
				}
				return string.Format("{0},{1}", "丹药类型", "恢复类-真元");
			case DanYaoEffect.HUIFU_JINGXUE:
				return string.Format("{0},{1}", "丹药类型", "恢复类-精血");
			case DanYaoEffect.HUIFU_SHOUYUAN:
				return string.Format("{0},{1}", "丹药类型", "恢复类-寿元");
			case DanYaoEffect.ZENGZHANG_TIPO:
				return string.Format("{0},{1}", "丹药类型", "增长类-体魄");
			case DanYaoEffect.ZENGZHANG_SHENSHI:
				return string.Format("{0},{1}", "丹药类型", "增长类-神识");
			case DanYaoEffect.ZENGZHANG_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return string.Format("{0},{1}", "丹药类型", "增长类-灵气");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					return string.Format("{0},{1}", "丹药类型", "增长类-真气");
				}
				return string.Format("{0},{1}", "丹药类型", "增长类-真元");
			case DanYaoEffect.ZENGZHANG_JINGXUE:
				return string.Format("{0},{1}", "丹药类型", "增长类-精血");
			case DanYaoEffect.ZENGZHANG_SHOUYUAN:
				return string.Format("{0},{1}", "丹药类型", "增长类-寿元");
			case DanYaoEffect.ZENGZHANG_JINGMAI:
				return string.Format("{0},{1}", "丹药类型", "增长类-经脉");
			case DanYaoEffect.TESHU_JINGLI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-增长精力");
			case DanYaoEffect.TESHU_WUXING:
				return string.Format("{0},{1}", "丹药类型", "特殊类-增长悟性");
			case DanYaoEffect.TESHU_JIEDU:
				return string.Format("{0},{1}", "丹药类型", "特殊类-解毒");
			case DanYaoEffect.TESHU_JINGJIETUPO_ZHUJI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-提升筑基成功率");
			case DanYaoEffect.TESHU_JINGJIETUPO_LEIJIE:
				return string.Format("{0},{1}", "丹药类型", "特殊类-降低雷劫伤害");
			case DanYaoEffect.TESHU_LINGGEN:
				return string.Format("{0},{1}", "丹药类型", "特殊类-补一种五行灵根");
			case DanYaoEffect.TESHU_JINGMAI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-修补受损经脉");
			case DanYaoEffect.TESHU_HUNLI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-增长魂力");
			default:
				return "";
			}
		}

		private string GetEffectStringValue(string title, int effectType, string value)
		{
			switch ((DanYaoEffect)effectType)
			{
			case DanYaoEffect.None:
				return string.Format("{0},{1}", "未知", "未知");
			case DanYaoEffect.HUIFU_TIPO:
				return title + string.Format("{0},{1},{2}", "恢复体魄", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_SHENSHI:
				return title + string.Format("{0},{1},{2}", "恢复神识", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return title + string.Format("{0},{1},{2}", "恢复灵气", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					return title + string.Format("{0},{1},{2}", "恢复真气", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				return title + string.Format("{0},{1},{2}", "恢复真元", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_JINGXUE:
				return title + string.Format("{0},{1},{2}", "恢复精血", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_SHOUYUAN:
				return title + string.Format("{0},{1},{2}", "恢复寿元", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_TIPO:
				return title + string.Format("{0},{1},{2}", "增长体魄", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_SHENSHI:
				return title + string.Format("{0},{1},{2}", "增长神识", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return title + string.Format("{0},{1},{2}", "增长灵气", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					return title + string.Format("{0},{1},{2}", "增长真气", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				return title + string.Format("{0},{1},{2}", "增长真元", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_JINGXUE:
				return title + string.Format("{0},{1},{2}", "增长精血", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_SHOUYUAN:
				return title + string.Format("{0},{1},{2}", "增长寿元", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_JINGMAI:
				return title + string.Format("{0},{1},{2}", "增长经脉", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGLI:
				return title + string.Format("{0},{1},{2}", "增长精力", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_WUXING:
				return title + string.Format("{0},{1},{2}", "增长悟性", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JIEDU:
				return title + string.Format("{0},{1},{2}", "解毒", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGJIETUPO_ZHUJI:
				return title + string.Format("{0},{1},{2}", "提升筑基成功率", value + "%", "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGJIETUPO_LEIJIE:
				return title + string.Format("{0},{1},{2}", "降低雷劫伤害", value + "%", "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_LINGGEN:
				return title + string.Format("{0},{1},{2}", "补灵根", WorldSetting.GetWuXingName((WuXing)int.Parse(value)), "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGMAI:
				return "";
			case DanYaoEffect.TESHU_HUNLI:
				return title + string.Format("{0},{1},{2}", "增长魂力", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			default:
				return "";
			}
		}

		public void SelectTemplateMatrixDetail(string templateId, int level, int rank, int fiveElement, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			MethodMatrixDetailReq_1011007 data = new MethodMatrixDetailReq_1011007
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_MATRIX_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MethodMatrixDetailResp_1011007 methodMatrixDetailResp_ = new MethodMatrixDetailResp_1011007();
					methodMatrixDetailResp_.MergeFrom(byteString);
					if (methodMatrixDetailResp_.IsSuccess)
					{
						dataList.Add(string.Format("{0},{1},{2}", "类型", GetZhenPanTypeName((BattleZhenFaType)methodMatrixDetailResp_.Type), "阵法包括4种类型：缚灵阵（降低妖兽移速且无法逃脱）、杀阵（定时掉血）、幻阵（妖兽攻击会产生未命中）、回复阵法（回复法力或回复防御）。"));
						dataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "高等阶的阵盘对低等阶战斗中，有额外加成作用，而低等级阵盘在高等阶战斗中，会被压制。"));
						dataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则阵法的效果越好"));
						dataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，对阵中对不同五行的妖兽会有克制或被克制效果"));
						dataList.Add(string.Format("{0},{1},{2}", "灵石消耗", methodMatrixDetailResp_.UnitConsume, "放置阵盘会消耗灵石，等阶越高品阶越高的阵盘，消耗的灵石越多"));
						dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodMatrixDetailResp_.Duration / 1000 + "息", "阵盘在战斗中可以持续的时间"));
						dataList.Add(string.Format("{0},{1},{2}", "灵衍值", methodMatrixDetailResp_.ConversionRate, "灵气注入阵盘后，灵气转换为最终效果的比例，越高则转换效率越高，造成的最终效果越好"));
						dataList.Add(string.Format("{0},{1},{2}", "创造人", methodMatrixDetailResp_.CreatedBy, "此阵盘由谁创造而来，开辟阵法室成为阵法大师，炼制阵法即可使用或者售卖，结丹期后进入虚神界可以创造阵法"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodMatrixDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		private string GetZhenPanTypeName(BattleZhenFaType battleZhenFaType)
		{
			return battleZhenFaType switch
			{
				BattleZhenFaType.Kill => "杀阵", 
				BattleZhenFaType.Kun => "缚灵阵", 
				BattleZhenFaType.Huan => "幻阵", 
				BattleZhenFaType.AddSuperAir => "复灵阵", 
				BattleZhenFaType.AddTiPo => "活血阵", 
				_ => "", 
			};
		}

		private string GetZhenPanTypeName(BattleZhenFaType battleZhenFaType, double value)
		{
			switch (battleZhenFaType)
			{
			case BattleZhenFaType.Kill:
				return "每息造成伤害：" + value.ToString("0");
			case BattleZhenFaType.Kun:
				return "无法逃离并减速：" + value.ToString("0");
			case BattleZhenFaType.Huan:
				return string.Format("{0}% 躲闪攻击", value.ToString("0"));
			case BattleZhenFaType.AddSuperAir:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 60)
				{
					return string.Format("每息真元恢复：{0}", (value / 100.0 / 100.0).ToString("0"));
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 30)
				{
					return string.Format("每息真气恢复：{0}", (value / 100.0).ToString("0"));
				}
				return string.Format("每息灵气恢复：{0}", value.ToString("0"));
			case BattleZhenFaType.AddTiPo:
				return string.Format("每息体魄恢复{0}：", value.ToString("0"));
			default:
				return "";
			}
		}

		public void SelectTemplateSignDetail(string templateId, int level, int rank, int fiveElement, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			MethodSignDetailReq_1011008 data = new MethodSignDetailReq_1011008
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_SIGN_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MethodSignDetailResp_1011008 methodSignDetailResp_ = new MethodSignDetailResp_1011008();
					methodSignDetailResp_.MergeFrom(byteString);
					if (methodSignDetailResp_.IsSuccess)
					{
						switch ((FaShuType)methodSignDetailResp_.SkillType)
						{
						case FaShuType.gongji_suoding:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-锁定攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_dingxiang:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-定向攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_chixu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-持续攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodSignDetailResp_.ContinueTime / 1000 + "息", "此法术持续存在的时间，拥有持续时间的法术包括：持续攻击、范围攻击、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_fanwei:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-范围攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_fanwei_chixu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-范围持续攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodSignDetailResp_.ContinueTime / 1000 + "息", "此法术持续存在的时间，拥有持续时间的法术包括：持续攻击、范围攻击、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.fangyu_fashu_hudun:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "防御-法术护盾", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.zengyi_shanghai:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "增益-伤害增益", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.zengyi_fangyu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "增益-防御增益", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.kongzhi_yisu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "控制-减低移速", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.kongzhi_xingdong:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "控制-限制行动", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.dunshu_wuxing:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "遁术-五行遁术", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.dunshu_yujian:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "遁术-御剑术", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						}
						if (dataList.Count > 0)
						{
							string[] array = dataList[0].Split(',');
							if (array.Length == 3)
							{
								array[2] = "符箓是通过封印法术而来，其类型与法术相同，分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术";
								dataList[0] = $"{array[0]},{array[1]},{array[2]}";
							}
						}
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						dataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(stageByLevel)));
						dataList.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(rank)));
						dataList.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement)));
						dataList.Add(string.Format("{0},{1},{2}", "施法消耗", methodSignDetailResp_.SpiritualityValue, "使用此符箓需要消耗的灵气值，如法力值消耗无法达到要求则无法使用符箓，法力消耗值=每息输出*施法时间"));
						dataList.Add(string.Format("{0},{1},{2}", "灵衍值", methodSignDetailResp_.ConversionRate, "符箓释放所封印的威能值转换为法术能量的比例，越高则转换效率越高，释放的能量越强"));
						dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodSignDetailResp_.ContinueTime / 1000 + "息", "此符箓使用后法术持续存在的时间，拥有持续时间的法术包括：持续攻击、范围攻击、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
						dataList.Add(string.Format("{0},{1},{2}", "施放时间", methodSignDetailResp_.CastTime / 1000 + "息", "使用此符箓需要消耗的时间"));
						dataList.Add(string.Format("{0},{1},{2}", "创造人", methodSignDetailResp_.CreatedBy, "此符箓由谁创造而来，开辟画符静室成为符师，炼制符箓即可使用或者售卖，结丹期后进入虚神界可以创造符箓"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodSignDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}

		public void SelectTemplateQiTuDetail(string templateId, int level, int rank, int fiveElement, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> detail = new List<string>();
			MethodEquipmentDetailReq_1011006 data = new MethodEquipmentDetailReq_1011006
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.METHOD_EQUIPMENT_DETAIL, data, (object obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					MethodEquipmentDetailResp_1011006 methodEquipmentDetailResp_ = new MethodEquipmentDetailResp_1011006();
					methodEquipmentDetailResp_.MergeFrom(byteString);
					if (methodEquipmentDetailResp_.IsSuccess)
					{
						if (detail.Count > 0)
						{
							string[] array = detail[0].Split(',');
							if (array.Length == 3)
							{
								array[2] = "符箓是通过封印法术而来，其类型与法术相同，分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术";
								detail[0] = array[0] + "," + array[1] + "," + array[2];
							}
						}
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						detail.Add("类型," + GetQiTuTypeName(methodEquipmentDetailResp_.Type));
						detail.Add("等阶," + WorldSetting.GetStageChineseName(stageByLevel) + ",法器的等级，限制了穿戴的最低等级");
						detail.Add("品阶," + WorldSetting.GetRankAllName(rank) + ",基于灵衍值给定的品级，品阶越高则法器造成的伤害越高");
						detail.Add("五行," + WorldSetting.GetWuXingName((WuXing)fiveElement) + ",分金、木、水、火、土五行，法器的五行如与灵根不符，则伤害会降低80%，请选择适合自身灵根的五行");
						detail.Add("可炼制最小品阶," + WorldSetting.GetRankAllName(methodEquipmentDetailResp_.MinRank) + ",该器图可炼制出法器的最小品阶");
						detail.Add("可炼制最大品阶," + WorldSetting.GetRankAllName(methodEquipmentDetailResp_.MaxRank) + ",该器图可炼制出法器的最大品阶");
						if (methodEquipmentDetailResp_.Type == 16 || methodEquipmentDetailResp_.Type == 29)
						{
							detail.Add(string.Format("占用体魄,{0},{1}", methodEquipmentDetailResp_.UsedPhysique, "穿戴此法器需要占用的体魄值，占用的体魄值越高则防御力越强"));
						}
						else if (methodEquipmentDetailResp_.Type == 15 || methodEquipmentDetailResp_.Type == 26)
						{
							detail.Add(string.Format("占用神识,{0},{1}", methodEquipmentDetailResp_.UsedPerception, "使用此法器时需要的神识，当神识不足时无法使用此法器，配置合理的神识占用可以在战斗中激活多件法器，攻击力会得到大幅提升"));
							if (stageByLevel > 6)
							{
								detail.Add(string.Format("激发真元值,{0},{1}", methodEquipmentDetailResp_.MinSpiritualityValue / 100 / 100, "激活此法器需要的真元值，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
								detail.Add(string.Format("最大真元值,{0},{1}", methodEquipmentDetailResp_.MaxSpiritualityValue / 100 / 100, "此法器可以承受的最大真元值上限，若注入法器的真元值超过此数值则以此数值为准，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
							}
							else if (stageByLevel > 3)
							{
								detail.Add(string.Format("激发真气值,{0},{1}", methodEquipmentDetailResp_.MinSpiritualityValue / 100, "激活此法器需要的真气值，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
								detail.Add(string.Format("最大真气值,{0},{1}", methodEquipmentDetailResp_.MaxSpiritualityValue / 100, "此法器可以承受的最大真气值上限，若注入法器的真气值超过此数值则以此数值为准，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
							}
							else if (stageByLevel > 0)
							{
								detail.Add(string.Format("激发灵气值,{0},{1}", methodEquipmentDetailResp_.MinSpiritualityValue, "激活此法器需要的灵气值，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
								detail.Add(string.Format("最大灵气值,{0},{1}", methodEquipmentDetailResp_.MaxSpiritualityValue, "此法器可以承受的最大灵气值上限，若注入法器的灵气值超过此数值则以此数值为准，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
							}
						}
						for (int i = 0; i < methodEquipmentDetailResp_.MaterialNames.Count; i++)
						{
							detail.Add("材料" + (i + 1) + "," + methodEquipmentDetailResp_.MaterialNames[i]);
						}
						detail.Add("创造人," + methodEquipmentDetailResp_.CreatedBy + ",此法器由谁创造而来，开辟炼器室成为炼器大师，炼制法器即可使用或者售卖，结丹期后进入虚神界可以创造法器");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodEquipmentDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(detail);
					}
				}
			}, isShowWaiting);
		}

		private string GetQiTuTypeName(int type)
		{
			return type switch
			{
				15 => "可配置攻击法器", 
				16 => "可穿戴防御法器", 
				26 => "可配置防御法器", 
				29 => "靴", 
				_ => "无", 
			};
		}

		public void SelectTemplateSkillDetail(string templateId, int level, int rank, int fiveElement, UnityAction<List<string>> callBack, bool isShowWaiting = true)
		{
			List<string> dataList = new List<string>();
			MethodSkillDetailReq_1011009 data = new MethodSkillDetailReq_1011009
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_SKILL_DETAIL, data, (object _obj) =>
			{
				if (isShowWaiting)
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MethodSkillDetailResp_1011009 methodSkillDetailResp_ = new MethodSkillDetailResp_1011009();
					methodSkillDetailResp_.MergeFrom(byteString);
					if (methodSkillDetailResp_.IsSuccess)
					{
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						switch ((FaShuType)methodSkillDetailResp_.SkillType)
						{
						case FaShuType.gongji_suoding:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-锁定攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_dingxiang:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-定向攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_chixu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-持续攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodSkillDetailResp_.ContinueTime / 1000 + "息", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_fanwei:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-范围攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.gongji_fanwei_chixu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-范围持续攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodSkillDetailResp_.ContinueTime / 1000 + "息", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.fangyu_fashu_hudun:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "防御-法术护盾", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.zengyi_shanghai:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "增益-伤害增益", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.zengyi_fangyu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "增益-防御增益", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.kongzhi_yisu:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "控制-减低移速", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.kongzhi_xingdong:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "控制-限制行动", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.dunshu_wuxing:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "遁术-五行遁术", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						case FaShuType.dunshu_yujian:
							dataList.Add(string.Format("{0},{1},{2}", "技能类型", "遁术-御剑术", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
							break;
						}
						dataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(stageByLevel)));
						dataList.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(rank)));
						dataList.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement)));
						dataList.Add(string.Format("{0},{1},{2}", "最低消耗", methodSkillDetailResp_.MinSpiritualityValue, "使用此法术的最低要求，依据消耗值计算法术伤害，消耗值与功法当前重数、功法与灵根的匹配、功法的灵衍值、角色等级等等数据均有关系，法力消耗值=每息输出*施法时间"));
						dataList.Add(string.Format("{0},{1},{2}", "最大消耗", methodSkillDetailResp_.MaxSpiritualityValue, "使用此法术的最高可用灵气，当法力消耗值超过最大则取最大消耗，消耗值与功法当前重数、功法与灵根的匹配、功法的灵衍值、角色等级等等数据均有关系，法力消耗值=每息输出*施法时间"));
						dataList.Add(string.Format("{0},{1},{2}", "灵衍值", methodSkillDetailResp_.ConversionRate, "将丹田内的灵气转换成法术能量的比例，越高则转换效率越高，法术的伤害越高"));
						dataList.Add(string.Format("{0},{1},{2}", "持续时间", methodSkillDetailResp_.ContinueTime / 1000 + "息", "此法术持续存在的时间，拥有持续时间的法术包括：持续攻击、范围攻击、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
						dataList.Add(string.Format("{0},{1},{2}", "施放时间", methodSkillDetailResp_.CastTime / 1000 + "息", "使用此法术需要消耗的时间"));
						dataList.Add(string.Format("{0},{1},{2}", "创造人", methodSkillDetailResp_.CreatedBy, "此法术由谁创造而来，开辟修法室成为术法师，结丹期后在虚神界即可创造法术，创造一本自己署名的大神通法术流芳百世"));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(methodSkillDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					if (callBack != null)
					{
						callBack(dataList);
					}
				}
			}, isShowWaiting);
		}
	}
}
