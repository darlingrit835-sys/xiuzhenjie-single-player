using System.Collections.Generic;

namespace XiuZhenJie
{
	public class LoginModel : BaseModel
	{
		public string userId;

		public string mobilePhone;

		public List<GameServerInfo> gameServersInfoList;

		public string lastGameServersId;

		public string rememberToken;

		// LitJson reads Unix seconds as Int32 or Int64 depending on their value.
		public object rememberTokenExpiresAt;

		internal long RememberTokenExpiresAt => rememberTokenExpiresAt is int seconds ? seconds :
			rememberTokenExpiresAt is long unixSeconds ? unixSeconds : 0L;

		public bool whetherNewUser;

		public string payIp;

		public int payPort;

		public List<HuoDongEntity> huoDongList;
	}
}
