using System.Text.RegularExpressions;

namespace XiuZhenJie
{
	public class PlatformSdkBase
	{
		protected int platformId;

		private string uid = "";

		public virtual void Init()
		{
		}

		public virtual void Login()
		{
		}

		public virtual void Logout()
		{
		}

		public virtual void Pay(int amount, string productId, string productName, int count, string productDesc, string extraInfo)
		{
		}

		public virtual void LoginGame(string uid, string sid)
		{
		}

		public virtual void CreateRole(string uid, string sid, string roleId, string roleName)
		{
		}

		public virtual void ExitGame()
		{
		}

		public virtual void ShowAd()
		{
		}

		public virtual bool GetLoginStatus()
		{
			return false;
		}

		public virtual void SetRoleInfo(string type, string roleId, string roleName, int roleLevel, string zoneId, string zoneName, string partyName)
		{
		}

		public virtual void JoinQQ(string id)
		{
		}

		public void SetPlatformId(string platformIdStr)
		{
			if (!string.IsNullOrEmpty(platformIdStr) && IsNumber(platformIdStr))
			{
				platformId = int.Parse(platformIdStr);
			}
		}

		public int GetPlatformId()
		{
			return platformId;
		}

		public bool CheckIsInitSDK()
		{
			return platformId != 0;
		}

		public string GetUid()
		{
			return uid;
		}

		public void SetUid(string uid)
		{
			this.uid = uid;
		}

		public bool IsNumber(string value)
		{
			string pattern = "^\\d*$";
			return Regex.IsMatch(value, pattern);
		}
	}
}
