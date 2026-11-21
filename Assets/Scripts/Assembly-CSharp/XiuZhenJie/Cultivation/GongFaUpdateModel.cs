using System;

namespace XiuZhenJie
{
	[Serializable]
	public class GongFaUpdateModel : BaseModel
	{
		public string name;

		public int level;

		public int upCount;

		public string nextLevelId;

		public double basedConcentration;

		private string achievementId;
	}
}
