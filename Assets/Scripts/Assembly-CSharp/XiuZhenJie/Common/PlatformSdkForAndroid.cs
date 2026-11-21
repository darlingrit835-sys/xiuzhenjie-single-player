using System;
using UnityEngine;

namespace XiuZhenJie
{
	public class PlatformSdkForAndroid : PlatformSdkBase
	{
		private AndroidJavaObject jo;

		public PlatformSdkForAndroid()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		private void Call(string funcName, params object[] args)
		{
			try
			{
				if (jo != null)
				{
					jo.Call(funcName, args);
				}
				else
				{
					Debug.Log(funcName + ":null");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
			}
		}

		private T Call<T>(string funcName, params object[] args)
		{
			try
			{
				if (jo != null)
				{
					return jo.Call<T>(funcName, args);
				}
				Debug.Log(funcName + ":null");
				return default;
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
				return default;
			}
		}

		public override void Init()
		{
			Call("init");
		}

		public override void Login()
		{
			Call("login");
		}

		public override void Logout()
		{
			Call("logout");
		}

		public override void Pay(int amount, string productId, string productName, int count, string productDesc, string extraInfo)
		{
			Call("pay", amount, productId, productName, count, productDesc, extraInfo);
		}

		public override void LoginGame(string uid, string sid)
		{
			if (CheckIsInitSDK() && platformId != 0)
			{
				Call("LoginGame", uid, sid);
			}
		}

		public override void CreateRole(string uid, string sid, string roleId, string roleName)
		{
			if (CheckIsInitSDK())
			{
				_ = platformId;
			}
		}

		public override void ExitGame()
		{
			Call("gameExitMethod");
		}

		public override void JoinQQ(string key)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否加群", () =>
			{
				Call("joinQQGroup", key);
			});
		}

		public override bool GetLoginStatus()
		{
			if (!CheckIsInitSDK())
			{
				return false;
			}
			if (platformId == 0)
			{
				return false;
			}
			return Call<bool>("GetOperateUserStatus", Array.Empty<object>());
		}

		public override void SetRoleInfo(string type, string roleId, string roleName, int roleLevel, string zoneId, string zoneName, string partyName)
		{
			Call("setRoleInfo", type, roleId, roleName, roleLevel, zoneId, zoneName, 0, partyName);
		}

		public override void ShowAd()
		{
			Call("ViewAd");
		}
	}
}
