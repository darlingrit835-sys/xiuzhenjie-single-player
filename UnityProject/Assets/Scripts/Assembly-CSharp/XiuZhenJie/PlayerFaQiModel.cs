using System;

namespace XiuZhenJie
{
	[Serializable]
	public class PlayerFaQiModel : BaseModel
	{
		public string id;

		public double receiveVal;

		public double minSpiritualityValue;

		public double maxSpiritualityValue;

		public double usedPerception;

		public double toughTenacity;

		public double conversionRate;

		public double durability;

		public double coolDown;

		public int rank;

		public int type;

		public int fiveElements;

		public string name;

		public int icon;

		public int level;

		public EquipmentEffectTypeEnum effect_First;

		public EquipmentEffectTypeEnum effect_Second;
	}
}
