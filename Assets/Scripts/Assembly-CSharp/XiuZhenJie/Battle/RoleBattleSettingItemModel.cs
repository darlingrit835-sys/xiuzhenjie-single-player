using System;

namespace XiuZhenJie
{
	[Serializable]
	public class RoleBattleSettingItemModel : BaseModel
	{
		public string id;

		public string name;

		public int rank;

		public int fiveElements;

		public int icon;

		public double usedPerception;

		public int level;
	}
}
