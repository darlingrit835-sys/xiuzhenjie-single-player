using System;

namespace XiuZhenJie
{
	[Serializable]
	public class PlayerFuLuModel : BaseModel
	{
		public string id;

		public string name;

		public double spiritualityValue;

		public double spiritualityValueInBattle;

		public double receiveVal;

		public int fiveElements;

		public int type;

		public string skillId;

		public int stage;

		public double volume;

		public string unit;

		public int rank;

		public int level;

		public string points;

		public int count;

		public int icon;

		public FaShuModel faShuData;
	}
}
