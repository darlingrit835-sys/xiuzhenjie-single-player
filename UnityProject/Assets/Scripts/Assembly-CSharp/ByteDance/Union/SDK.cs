using System;
using System.Collections.Generic;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public static class SDK
	{
		private sealed class ExitInstallListener : AndroidJavaProxy
		{
			private readonly Action callback;

			public ExitInstallListener(Action callback)
				: base("com.bytedance.sdk.openadsdk.downloadnew.core.ExitInstallListener")
			{
				this.callback = callback;
			}

			public void onExitInstall()
			{
				UnityDispatcher.PostTask(callback);
			}
		}

		private static AndroidJavaObject activity;

		private static AndroidJavaObject adManager;

		public static string Version => GetAdManager().Call<string>("getSDKVersion", Array.Empty<object>());

		public static AdNative CreateAdNative()
		{
			AndroidJavaObject androidJavaObject = GetAdManager();
			AndroidJavaObject androidJavaObject2 = Utils.GetActivity();
			return new AdNative(androidJavaObject.Call<AndroidJavaObject>("createAdNative", new object[1] { androidJavaObject2 }));
		}

		public static void RequestPermissionIfNecessary()
		{
			AndroidJavaObject androidJavaObject = GetAdManager();
			AndroidJavaObject androidJavaObject2 = Utils.GetActivity();
			androidJavaObject.Call("requestPermissionIfNecessary", androidJavaObject2);
		}

		public static bool TryShowInstallDialogWhenExit(Action onExitInstall)
		{
			AndroidJavaObject androidJavaObject = GetAdManager();
			AndroidJavaObject androidJavaObject2 = Utils.GetActivity();
			ExitInstallListener exitInstallListener = new ExitInstallListener(onExitInstall);
			return androidJavaObject.Call<bool>("tryShowInstallDialogWhenExit", new object[2] { androidJavaObject2, exitInstallListener });
		}

		private static AndroidJavaObject GetAdManager()
		{
			if (adManager == null)
			{
				adManager = new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdSdk").CallStatic<AndroidJavaObject>("getAdManager", Array.Empty<object>());
			}
			return adManager;
		}

		public static void MediationPreload(AndroidJavaObject activity, List<MediationPreloadRequestInfo> requestInfos, int parallelNum, int requestIntervalS)
		{
			AndroidJavaObject mediationManager = Utils.GetMediationManager();
			if (mediationManager != null && requestInfos != null && requestInfos.Count > 0)
			{
				mediationManager.Call("preload", activity, Utils.MakeMediationPreloadReqInfos(requestInfos), parallelNum, requestIntervalS);
			}
		}

		public static void MediationSetUserInfoForSegment(MediationConfigUserInfoForSegment segment)
		{
			AndroidJavaObject mediationManager = Utils.GetMediationManager();
			if (mediationManager != null)
			{
				AndroidJavaObject androidJavaObject = Utils.MakeMediationConfigUserInfoForSegment(segment);
				if (androidJavaObject != null)
				{
					mediationManager.Call("setUserInfoForSegment", androidJavaObject);
				}
			}
		}

		public static void MediationSetPublisherDid(string publisherDid)
		{
			Utils.GetMediationManager()?.Call("setPulisherDid", publisherDid);
		}

		public static void MediationSetThemeStatus(int themeStatus)
		{
			Utils.GetMediationManager()?.Call("setThemeStatus", themeStatus);
		}

		public static void MediationUpdatePrivacyConfig(PrivacyConfiguration privacyConfig)
		{
			AndroidJavaObject mediationManager = Utils.GetMediationManager();
			if (mediationManager != null)
			{
				AndroidJavaObject androidJavaObject = Utils.MakeCustomController(privacyConfig);
				if (androidJavaObject != null)
				{
					mediationManager.Call("updatePrivacyConfig", androidJavaObject);
				}
			}
		}

		public static Dictionary<string, object> GetMediationExtraInfo()
		{
			AndroidJavaObject mediationManager = Utils.GetMediationManager();
			if (mediationManager != null)
			{
				AndroidJavaObject androidJavaObject = mediationManager.Call<AndroidJavaObject>("getMediationExtraInfo", Array.Empty<object>());
				if (androidJavaObject != null)
				{
					Debug.Log("CSJM_Unity getMediationExtraInfo返回的是原生端对象，根据需要通过反射获取信息");
					Debug.Log("CSJM_Unity getMediationExtraInfo: " + androidJavaObject.Call<string>("toString", Array.Empty<object>()));
				}
			}
			return null;
		}
	}
}
