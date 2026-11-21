using UnityEngine;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

namespace XiuZhenJie
{
	[Serializable]
	public class BaseSkillModel : BaseModel
	{
		public string id;

		public int SkillIndex;

		public int PracticeAffix;

		public int ForgeResonancePercent;

		public string name;

		public double conversionRate;

		public int fiveElement;

		public int rank;

		public int type;

		public int skillType;

		public ObscuredDouble continued;

		public ObscuredDouble castTime;

		public int icon;

		public ObscuredInt level;

		public ObscuredLong usedSuperAir;

		public ObscuredDouble coolDown;

		public ObscuredDouble baseDamage;

		public ObscuredDouble usedUnit;

		public ObscuredInt usedPerception;

		public ObscuredInt canUseCount;

		public ObscuredDouble GoldDamageAdd;

		public ObscuredDouble WoodDamageAdd;

		public ObscuredDouble WaterDamageAdd;

		public ObscuredDouble FireDamageAdd;

		public ObscuredDouble EarthDamageAdd;

		public ObscuredInt ZhenPanCastShinyStone;

		// Older control spells supplied only a conversion ratio. Modern entries carry their resolved duration in seconds.
		internal float MovementSeconds(float legacyMultiplier) => (double)continued > 0
			? (float)(double)continued : Mathf.Max(0, (float)conversionRate * legacyMultiplier * (1f + ForgeResonancePercent / 100f));
		internal float SlowFraction() => (double)continued > 0 && (double)baseDamage > 0 && (double)baseDamage <= 1
			? (float)(double)baseDamage : .2f * Mathf.Max(1, WorldSetting.GetLevelByRank(rank));

		public BaseSkillModel()
		{
		}

		public BaseSkillModel(object data)
		{
		}

		public BaseSkillModel(RoleJoinDungeonResp_1010002.Types.BattleSkillResp_1010003 data)
		{
			PracticeAffix = data.PracticeAffix;
			ForgeResonancePercent = data.ForgeResonancePercent;
			baseDamage = data.BaseDamage;
			castTime = data.CastTime;
			continued = data.ContinueTime / 1000.0;
			conversionRate = data.ConversionRate;
			coolDown = data.CoolDown;
			fiveElement = data.FiveElement;
			icon = data.Icon;
			level = data.Level;
			name = data.Name;
			type = data.Type;
			id = data.TemplateId;
			skillType = data.SkillType;
			usedPerception = data.UsedPerception;
			canUseCount = data.CanUseCount;
			SkillIndex = data.SkillIndex;
			usedSuperAir = data.UsedSuperAir;
			rank = data.Rank;
			usedUnit = data.UnitUsed;
			GoldDamageAdd = ((data.GoldDamageAdd == 0.0) ? 1.0 : data.GoldDamageAdd);
			WoodDamageAdd = ((data.WoodDamageAdd == 0.0) ? 1.0 : data.WoodDamageAdd);
			WaterDamageAdd = ((data.WaterDamageAdd == 0.0) ? 1.0 : data.WaterDamageAdd);
			FireDamageAdd = ((data.FireDamageAdd == 0.0) ? 1.0 : data.FireDamageAdd);
			EarthDamageAdd = ((data.EarthDamageAdd == 0.0) ? 1.0 : data.EarthDamageAdd);
			ZhenPanCastShinyStone = data.UnitConsume;
		}

		public BaseSkillModel(BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Types.ObjectInfoSkillResp_8001003 data)
		{
			SkillIndex = data.SkillIndex;
			baseDamage = data.BaseDamage;
			castTime = data.CastTime;
			continued = data.ContinueTime / 1000.0;
			conversionRate = data.ConversionRate;
			coolDown = data.CoolDown;
			fiveElement = data.FiveElement;
			icon = data.Icon;
			level = data.Level;
			name = data.Name;
			type = data.Type;
			id = data.TemplateId;
			skillType = data.SkillType;
			usedPerception = data.UsedPerception;
			usedSuperAir = data.UsedSuperAir;
			GoldDamageAdd = ((data.GoldDamageAdd == 0.0) ? 1.0 : data.GoldDamageAdd);
			WoodDamageAdd = ((data.WoodDamageAdd == 0.0) ? 1.0 : data.WoodDamageAdd);
			WaterDamageAdd = ((data.WaterDamageAdd == 0.0) ? 1.0 : data.WaterDamageAdd);
			FireDamageAdd = ((data.FireDamageAdd == 0.0) ? 1.0 : data.FireDamageAdd);
			EarthDamageAdd = ((data.EarthDamageAdd == 0.0) ? 1.0 : data.EarthDamageAdd);
		}

		public BaseSkillModel(EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Types.ObjectInfoSkillResp_8001012 data)
		{
			SkillIndex = data.SkillIndex;
			baseDamage = data.BaseDamage;
			castTime = data.CastTime;
			continued = data.ContinueTime / 1000.0;
			conversionRate = data.ConversionRate;
			coolDown = data.CoolDown;
			fiveElement = data.FiveElement;
			icon = data.Icon;
			level = data.Level;
			name = data.Name;
			type = data.Type;
			id = data.TemplateId;
			skillType = data.SkillType;
			usedPerception = data.UsedPerception;
			usedSuperAir = data.UsedSuperAir;
			GoldDamageAdd = 1.0;
			WoodDamageAdd = 1.0;
			WaterDamageAdd = 1.0;
			FireDamageAdd = 1.0;
			EarthDamageAdd = 1.0;
		}

		public BaseSkillModel(EnterThunderCalamityResp_1002009.Types.BattleSkillResp_1002010 data)
		{
			SkillIndex = data.SkillIndex;
			baseDamage = data.BaseDamage;
			castTime = data.CastTime;
			continued = data.ContinueTime;
			conversionRate = data.ConversionRate;
			usedSuperAir = data.UsedSuperAir;
			coolDown = data.CoolDown;
			fiveElement = data.FiveElement;
			icon = data.Icon;
			level = data.Level;
			name = data.Name;
			type = data.Type;
			id = data.TemplateId;
			skillType = data.SkillType;
			usedPerception = data.UsedPerception;
			canUseCount = data.CanUseCount;
			usedUnit = data.UnitUsed;
			GoldDamageAdd = 1.0;
			WoodDamageAdd = 1.0;
			WaterDamageAdd = 1.0;
			FireDamageAdd = 1.0;
			EarthDamageAdd = 1.0;
		}
	}
}
