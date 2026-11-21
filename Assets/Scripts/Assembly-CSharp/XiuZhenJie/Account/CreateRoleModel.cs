using System;

namespace XiuZhenJie
{
	[Serializable]
	public class CreateRoleModel : BaseModel
	{
		public string serverId;

		public string clientIp;

		public string userId;

		public string mapId;

		public bool sex;

		public int headPortraitId;

		public string name;

		public double gold;

		public double wood;

		public double water;

		public double fire;

		public double earth;
	}
}
