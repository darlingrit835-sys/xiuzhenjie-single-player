namespace XiuZhenJie
{
	public class GameServerInfo : BaseModel
	{
		public string gameServerId;

		public string name;

		public int osType;

		public string serverIp;

		public int port;

		public int registerCount;

		public int onlineCount;

		public int maxCount;

		public RoleVO currentRoleVO;

		public string gameServerIpManageId;

		public int status;

		public string serverVersion;
	}
}
