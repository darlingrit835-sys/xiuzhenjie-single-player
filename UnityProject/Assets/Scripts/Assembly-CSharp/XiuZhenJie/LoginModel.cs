using System.Collections.Generic;

namespace XiuZhenJie
{
	public class LoginModel : BaseModel
	{
		public string userId;

		public string mobilePhone;

		public List<GameServerInfo> gameServersInfoList;

		public string lastGameServersId;

		public bool whetherNewUser;

		public string payIp;

		public int payPort;

		public List<HuoDongEntity> huoDongList;
	}
}
