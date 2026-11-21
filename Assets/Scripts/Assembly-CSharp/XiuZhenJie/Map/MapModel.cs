using System;

namespace XiuZhenJie
{
	[Serializable]
	public class MapModel : BaseModel
	{
		public string id;

		public string parentId;

		public string parentName;

		public string name;

		public int type;

		public int minLevel;

		public int maxLevel;

		public int sort;

		public int level;

		public double a;

		public int fiveElements;
	}
}
