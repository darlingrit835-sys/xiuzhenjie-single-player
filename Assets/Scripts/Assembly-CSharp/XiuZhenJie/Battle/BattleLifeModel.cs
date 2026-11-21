using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

namespace XiuZhenJie
{
	public class BattleLifeModel
	{
		public ObscuredFloat Init_X;

		public ObscuredFloat Init_Y;

		public int ObjectIndex;

		public ObscuredDouble CurrentBlood;

		public ObscuredDouble Max_Blood;

		public double RestBloodFractionPerSecond;

		public int MethodMovePercent, SigilMovePercent;

		public ObscuredDouble CurrentJianRen;

		public ObscuredDouble Max_JianRen;

		public ObscuredDouble CurrentHuDun;

		public int HuDunFiveElement;

		public ObscuredDouble CurrentSuperAir;

		public ObscuredDouble MaxSuperAir;

		public ObscuredDouble CurrentPerception;

		public ObscuredDouble MaxPerception;

		public int AttackFiveElement;

		public int DefenseFiveElement;

		public int ShenShiFiveElement;

		public ObscuredDouble InitMoveSpeed;

		public ObscuredDouble CurrentMoveSpeed;

		public ObscuredDouble DamageAdd = 0.0;

		public ObscuredDouble DefeneceAdd = 0.0;

		public ObscuredDouble ShiFaYanChang = 1.0;

		public bool DiKangDamage;

		public string Name;

		public int Level;

		public int Icon;

		public int Type;

		public int ObjectType;

		public ObscuredDouble RecoverSuperAirEverySecondBattle;

		public ObscuredDouble RecoverSuperAirEverySecondNormal;

		public ObscuredDouble RecoverJianRenEverySecondBattle;

		public ObscuredDouble RecoverJianRenEverySecondNormal;

		public ObscuredDouble AttackDistance;

		public ObscuredDouble GoldDamageAvoidance;

		public ObscuredDouble WoodDamageAvoidance;

		public ObscuredDouble WaterDamageAvoidance;

		public ObscuredDouble FireDamageAvoidance;

		public ObscuredDouble EarthDamageAvoidance;

		public List<BaseSkillModel> LifeSkills;

		public BattleLifeModel()
		{
		}

		public BattleLifeModel(RoleJoinDungeonResp_1010002 data)
		{
			CurrentBlood = data.CurrentPureBloodDefense;
			Max_Blood = data.CurrentPureBloodDefense;
			AttackFiveElement = data.SuperAirFiveElement;
			CurrentHuDun = 0.0;
			CurrentJianRen = data.CurrentPhysiqueDefense;
			CurrentMoveSpeed = data.CurrentMoveSpeed;
			CurrentPerception = data.CurrentPerception;
			CurrentSuperAir = data.CurrentSuperAir;
			DefenseFiveElement = data.PhysiqueFiveElement;
			Icon = data.Icon;
			InitMoveSpeed = data.CurrentMoveSpeed;
			DamageAdd = 0.0;
			DefeneceAdd = 0.0;
			DiKangDamage = false;
			Level = data.Level;
			MaxPerception = data.CurrentPerception;
			MaxSuperAir = data.CurrentSuperAir;
			Max_JianRen = data.CurrentPhysiqueDefense;
			Name = data.Name;
			RecoverJianRenEverySecondBattle = data.DefenseRecoverValueOnBattle;
			RecoverJianRenEverySecondNormal = data.DefenseRecoverValue;
			RecoverSuperAirEverySecondBattle = data.SuperAirRecoverValueOnBattle;
			RecoverSuperAirEverySecondNormal = data.SuperAirRecoverValue;
			ShenShiFiveElement = data.PerceptionFiveElement;
			ShiFaYanChang = 1.0;
			GoldDamageAvoidance = ((data.GoldDamageAvoidance == 0.0) ? 1.0 : data.GoldDamageAvoidance);
			WoodDamageAvoidance = ((data.WoodDamageAvoidance == 0.0) ? 1.0 : data.WoodDamageAvoidance);
			WaterDamageAvoidance = ((data.WaterDamageAvoidance == 0.0) ? 1.0 : data.WaterDamageAvoidance);
			FireDamageAvoidance = ((data.FireDamageAvoidance == 0.0) ? 1.0 : data.FireDamageAvoidance);
			EarthDamageAvoidance = ((data.EarthDamageAvoidance == 0.0) ? 1.0 : data.EarthDamageAvoidance);
			AttackDistance = data.AttackDistance;
			LifeSkills = new List<BaseSkillModel>();
			if (data.Skills == null)
			{
				return;
			}
			foreach (RoleJoinDungeonResp_1010002.Types.BattleSkillResp_1010003 skill in data.Skills)
			{
				LifeSkills.Add(new BaseSkillModel(skill));
			}
		}

		public BattleLifeModel(object data)
		{
		}

		public BattleLifeModel(BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002 data)
		{
			ObjectIndex = data.ObjectIndex;
			CurrentBlood = data.CurrentPureBloodDefense;
			Max_Blood = data.CurrentPureBloodDefense;
			CurrentHuDun = 0.0;
			CurrentJianRen = data.CurrentPhysiqueDefense;
			CurrentMoveSpeed = data.CurrentMoveSpeed;
			CurrentPerception = data.CurrentPerception;
			CurrentSuperAir = data.CurrentSuperAir;
			Icon = data.Icon;
			InitMoveSpeed = data.CurrentMoveSpeed;
			DamageAdd = 0.0;
			DefeneceAdd = 0.0;
			DiKangDamage = false;
			Level = data.Level;
			MaxPerception = data.CurrentPerception;
			MaxSuperAir = data.CurrentSuperAir;
			Max_JianRen = data.CurrentPhysiqueDefense;
			Name = data.Name;
			RecoverJianRenEverySecondBattle = data.DefenseRecoverValueOnBattle;
			RecoverJianRenEverySecondNormal = data.DefenseRecoverValue;
			RecoverSuperAirEverySecondBattle = data.SuperAirRecoverValueOnBattle;
			RecoverSuperAirEverySecondNormal = data.SuperAirRecoverValue;
			ShiFaYanChang = 1.0;
			AttackDistance = data.AttackDistance;
			ObjectType = data.ObjectType;
			Type = data.UnusualLevel;
			AttackFiveElement = data.FiveElement;
			DefenseFiveElement = data.FiveElement;
			ShenShiFiveElement = data.FiveElement;
			Init_X = (float)data.PosX;
			Init_Y = (float)data.PosY;
			LifeSkills = new List<BaseSkillModel>();
			if (data.Skills == null)
			{
				return;
			}
			foreach (BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Types.ObjectInfoSkillResp_8001003 skill in data.Skills)
			{
				LifeSkills.Add(new BaseSkillModel(skill));
			}
		}

		public BattleLifeModel(EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010 data)
		{
			ObjectIndex = data.ObjectIndex;
			CurrentBlood = data.CurrentPureBloodDefense;
			Max_Blood = data.CurrentPureBloodDefense;
			CurrentHuDun = 0.0;
			CurrentJianRen = data.CurrentPhysiqueDefense;
			CurrentMoveSpeed = data.CurrentMoveSpeed;
			CurrentPerception = data.CurrentPerception;
			CurrentSuperAir = data.CurrentSuperAir;
			Icon = data.Icon;
			InitMoveSpeed = data.CurrentMoveSpeed;
			DamageAdd = 0.0;
			DefeneceAdd = 0.0;
			DiKangDamage = false;
			Level = data.Level;
			MaxPerception = data.CurrentPerception;
			MaxSuperAir = data.CurrentSuperAir;
			Max_JianRen = data.CurrentPhysiqueDefense;
			Name = data.Name;
			RecoverJianRenEverySecondBattle = data.DefenseRecoverValueOnBattle;
			RecoverJianRenEverySecondNormal = data.DefenseRecoverValue;
			RecoverSuperAirEverySecondBattle = data.SuperAirRecoverValueOnBattle;
			RecoverSuperAirEverySecondNormal = data.SuperAirRecoverValue;
			ShiFaYanChang = 1.0;
			AttackDistance = data.AttackDistance;
			ObjectType = data.ObjectType;
			Type = data.UnusualLevel;
			Init_X = (float)data.PosX;
			Init_Y = (float)data.PosY;
			LifeSkills = new List<BaseSkillModel>();
			if (data.Skills == null)
			{
				return;
			}
			foreach (EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Types.ObjectInfoSkillResp_8001012 skill in data.Skills)
			{
				LifeSkills.Add(new BaseSkillModel(skill));
			}
		}
	}
}
