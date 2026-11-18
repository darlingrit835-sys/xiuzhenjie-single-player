using System;

namespace XiuZhenJie
{
	[Serializable]
	public class XzjRoleLetterAwardVO : BaseModel
	{
		public string id;

		public string letterId;

		public int type;

		public double volume;

		public string goodsId;

		public string name;

		public double count;

		public bool status;

		public long getTime;

		public int rank;
	}
}
