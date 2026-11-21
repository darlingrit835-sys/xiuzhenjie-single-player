using System;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public class Pangle
	{
		public delegate void PangleInitializeCallBack(bool success, string message);

		private sealed class SdkInitCallback : AndroidJavaProxy
		{
			private readonly PangleInitializeCallBack listener;

			public SdkInitCallback(PangleInitializeCallBack listener)
				: base("com.bytedance.sdk.openadsdk.TTAdSdk$Callback")
			{
				this.listener = listener;
			}

			public void fail(int code, string message)
			{
				listener(success: false, message);
			}

			public void success()
			{
				listener(success: true, "sdk 初始化成功");
			}
		}

		public static void Init(SDKConfiguration sdkConfiguration)
		{
			Debug.Log("CSJM_Unity Pangle Init");
			AndroidJavaObject activity = Utils.GetActivity();
			AndroidJavaRunnable androidJavaRunnable = () =>
			{
				initTTSdk(sdkConfiguration);
			};
			activity.Call("runOnUiThread", androidJavaRunnable);
		}

		public static bool IsSdkReady()
		{
			return new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdSdk").CallStatic<bool>("isSdkReady", Array.Empty<object>());
		}

		private static void initTTSdk(SDKConfiguration sdkConfiguration)
		{
			Debug.Log("CSJM_Unity Pangle initTTSdk ");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.bytedance.sdk.openadsdk.TTAdConfig$Builder");
			Debug.Log("CSJM_Unity Pangle InitializeSDK 开始设置config");
			androidJavaObject.Call<AndroidJavaObject>("appId", new object[1] { sdkConfiguration.appId }).Call<AndroidJavaObject>("useTextureView", new object[1] { sdkConfiguration.useTextureView }).Call<AndroidJavaObject>("appName", new object[1] { sdkConfiguration.appName })
				.Call<AndroidJavaObject>("allowShowNotify", new object[1] { sdkConfiguration.allowShowNotify })
				.Call<AndroidJavaObject>("debug", new object[1] { sdkConfiguration.debug })
				.Call<AndroidJavaObject>("directDownloadNetworkType", new object[1] { sdkConfiguration.directDownloadNetworkType })
				.Call<AndroidJavaObject>("themeStatus", new object[1] { sdkConfiguration.themeStatus })
				.Call<AndroidJavaObject>("supportMultiProcess", new object[1] { sdkConfiguration.supportMultiProcess })
				.Call<AndroidJavaObject>("useMediation", new object[1] { sdkConfiguration.useMediation })
				.Call<AndroidJavaObject>("setAgeGroup", new object[1] { sdkConfiguration.ageGroup })
				.Call<AndroidJavaObject>("data", new object[1] { "[{\"name\":\"unity_version\",\"value\":\"6.0.0.1\"}]" });
			PrivacyConfiguration privacyConfiguration = sdkConfiguration.privacyConfiguration;
			if (privacyConfiguration != null)
			{
				androidJavaObject.Call<AndroidJavaObject>("customController", new object[1] { Utils.MakeCustomController(privacyConfiguration) });
			}
			MediationConfig mediationConfig = sdkConfiguration.mediationConfig;
			if (mediationConfig != null)
			{
				androidJavaObject.Call<AndroidJavaObject>("setMediationConfig", new object[1] { Utils.MakeMediationConfig(mediationConfig) });
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>());
			new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdSdk").CallStatic<bool>("init", new object[2]
			{
				Utils.GetActivity(),
				androidJavaObject2
			});
		}

		public static void Start(PangleInitializeCallBack callback)
		{
			AndroidJavaObject activity = Utils.GetActivity();
			AndroidJavaRunnable androidJavaRunnable = () =>
			{
				SdkInitCallback sdkInitCallback = new SdkInitCallback(callback);
				new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdSdk").CallStatic("start", sdkInitCallback);
			};
			activity.Call("runOnUiThread", androidJavaRunnable);
		}
	}
}
