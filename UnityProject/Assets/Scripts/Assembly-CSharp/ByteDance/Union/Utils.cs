using System;
using System.Collections.Generic;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public class Utils
	{
		public static AndroidJavaObject MakeCustomController(PrivacyConfiguration controller)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.bytedance.android.CustomController");
			if (controller == null)
			{
				return androidJavaObject;
			}
			androidJavaObject.Call("canUseWifiState", controller.CanUseWifiState);
			androidJavaObject.Call("setMacAddress", controller.MacAddress);
			androidJavaObject.Call("canUseLocation", controller.CanUseLocation);
			if (!double.IsNaN(controller.Latitude) || !double.IsNaN(controller.Longitude))
			{
				androidJavaObject.Call("setLocationInfo", controller.Longitude, controller.Latitude);
			}
			androidJavaObject.Call("canReadAppList", controller.CanReadAppList);
			androidJavaObject.Call("canUsePhoneState", controller.CanUsePhoneState);
			androidJavaObject.Call("setDevImei", controller.DevImei);
			androidJavaObject.Call("canUseWriteExternal", controller.CanUseWriteExternal);
			androidJavaObject.Call("setDevOaid", controller.DevOaid);
			androidJavaObject.Call("canUseAndroidId", controller.CanUseAndroidId);
			androidJavaObject.Call("setAndroidId", controller.AndroidId);
			if (controller.MediationPrivacyConfig != null)
			{
				androidJavaObject.Call("setMediationPrivacyConfig", MakeMediationPrivacyConfig(controller.MediationPrivacyConfig));
			}
			return androidJavaObject;
		}

		public static AndroidJavaObject MakeMediationConfig(MediationConfig mediationConfig)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.bytedance.sdk.openadsdk.mediation.init.MediationConfig$Builder");
			androidJavaObject.Call<AndroidJavaObject>("setPublisherDid", new object[1] { mediationConfig.PublisherDid });
			if (mediationConfig.MediationConfigUserInfoForSegment != null)
			{
				androidJavaObject.Call<AndroidJavaObject>("setMediationConfigUserInfoForSegment", new object[1] { MakeMediationConfigUserInfoForSegment(mediationConfig.MediationConfigUserInfoForSegment) });
			}
			if (mediationConfig.LocalExtra != null)
			{
				androidJavaObject.Call<AndroidJavaObject>("setLocalExtra", new object[1] { GetMapFromDictionary(mediationConfig.LocalExtra) });
			}
			androidJavaObject.Call<AndroidJavaObject>("setHttps", new object[1] { mediationConfig.UseHttps });
			if (mediationConfig.CustomLocalConfig != null && mediationConfig.CustomLocalConfig.Length > 0)
			{
				androidJavaObject.Call<AndroidJavaObject>("setCustomLocalConfig", new object[1] { GetJsonObjFromJsonStr(mediationConfig.CustomLocalConfig) });
			}
			androidJavaObject.Call<AndroidJavaObject>("setWxInstalled", new object[1] { mediationConfig.WxInstalled });
			androidJavaObject.Call<AndroidJavaObject>("setOpensdkVer", new object[1] { mediationConfig.OpensdkVer });
			androidJavaObject.Call<AndroidJavaObject>("setSupportH265", new object[1] { mediationConfig.SupportH265 });
			androidJavaObject.Call<AndroidJavaObject>("setSupportSplashZoomout", new object[1] { mediationConfig.SupportSplashZoomout });
			androidJavaObject.Call<AndroidJavaObject>("setWxAppId", new object[1] { mediationConfig.WxAppId });
			return androidJavaObject.Call<AndroidJavaObject>("build", Array.Empty<object>());
		}

		public static AndroidJavaObject MakeMediationConfigUserInfoForSegment(MediationConfigUserInfoForSegment segment)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.bytedance.sdk.openadsdk.mediation.init.MediationConfigUserInfoForSegment");
			if (segment.CustomInfos != null)
			{
				androidJavaObject.Call("setCustomInfos", GetMapFromDictionary(segment.CustomInfos));
			}
			androidJavaObject.Call("setUserId", segment.UserId);
			androidJavaObject.Call("setChannel", segment.Channel);
			androidJavaObject.Call("setSubChannel", segment.SubChannel);
			androidJavaObject.Call("setAge", segment.Age);
			androidJavaObject.Call("setGender", segment.Gender);
			androidJavaObject.Call("setUserValueGroup", segment.UserValueGroup);
			return androidJavaObject;
		}

		public static AndroidJavaObject MakeMediationPrivacyConfig(MediationPrivacyConfig config)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.bytedance.android.MediationPrivacyConfig");
			androidJavaObject.Call("setCustomAppList", GetJavaListFromList(config.CustomAppList));
			androidJavaObject.Call("setCustomDevImeis", GetJavaListFromList(config.CustomDevImeis));
			androidJavaObject.Call("setCanUseOaid", config.CanUseOaid);
			androidJavaObject.Call("setLimitPersonalAds", config.LimitPersonalAds);
			androidJavaObject.Call("setProgrammaticRecommend", config.ProgrammaticRecommend);
			return androidJavaObject;
		}

		public static AndroidJavaObject MakeMediationPreloadReqInfos(List<MediationPreloadRequestInfo> reqInfos)
		{
			if (reqInfos == null || reqInfos.Count <= 0)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
			foreach (MediationPreloadRequestInfo reqInfo in reqInfos)
			{
				AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.bytedance.sdk.openadsdk.mediation.MediationPreloadRequestInfo", reqInfo.AdType, reqInfo.AdSlot.Handle, GetJavaListFromList(reqInfo.PrimeRitList));
				androidJavaObject.Call<bool>("add", new object[1] { androidJavaObject2 });
			}
			return androidJavaObject;
		}

		public static AndroidJavaObject GetMapFromDictionary(Dictionary<string, object> dict)
		{
			if (dict == null)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			foreach (KeyValuePair<string, object> item in dict)
			{
				androidJavaObject.Call<AndroidJavaObject>("put", new object[2] { item.Key, item.Value });
			}
			return androidJavaObject;
		}

		public static AndroidJavaObject GetMapFromDictionary(Dictionary<string, string> dict)
		{
			if (dict == null)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			foreach (KeyValuePair<string, string> item in dict)
			{
				androidJavaObject.Call<AndroidJavaObject>("put", new object[2] { item.Key, item.Value });
			}
			return androidJavaObject;
		}

		public static AndroidJavaObject GetJsonObjFromJsonStr(string jsonStr)
		{
			if (jsonStr == null || jsonStr.Length <= 0)
			{
				return null;
			}
			Debug.Log("CSJM_Unity, start json, localConfigJsonStr: " + jsonStr);
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("org.json.JSONObject", jsonStr);
			Debug.Log("CSJM_Unity, after json, localConfigJsonStr: " + androidJavaObject.Call<string>("toString", Array.Empty<object>()));
			return androidJavaObject;
		}

		public static AndroidJavaObject GetJavaListFromList(List<string> list)
		{
			if (list == null)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.ArrayList");
			foreach (string item in list)
			{
				androidJavaObject.Call<bool>("add", new object[1] { item });
			}
			return androidJavaObject;
		}

		public static AndroidJavaObject GetMediationManager()
		{
			return new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdSdk").CallStatic<AndroidJavaObject>("getMediationManager", Array.Empty<object>());
		}

		public static AndroidJavaObject GetActivity()
		{
			return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		}

		public static List<MediationAdLoadInfo> GetAdLoadInfo(AndroidJavaObject handle)
		{
			if (handle != null)
			{
				AndroidJavaObject androidJavaObject = handle.Call<AndroidJavaObject>("getAdLoadInfo", Array.Empty<object>());
				if (androidJavaObject != null)
				{
					List<MediationAdLoadInfo> list = new List<MediationAdLoadInfo>();
					int num = androidJavaObject.Call<int>("size", Array.Empty<object>());
					for (int i = 0; i < num; i++)
					{
						AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i });
						if (androidJavaObject2 != null)
						{
							MediationAdLoadInfo mediationAdLoadInfo = new MediationAdLoadInfo();
							mediationAdLoadInfo.adnName = androidJavaObject2.Call<string>("getAdnName", Array.Empty<object>());
							mediationAdLoadInfo.mediationRit = androidJavaObject2.Call<string>("getMediationRit", Array.Empty<object>());
							mediationAdLoadInfo.adType = androidJavaObject2.Call<string>("getAdType", Array.Empty<object>());
							mediationAdLoadInfo.errCode = androidJavaObject2.Call<int>("getErrCode", Array.Empty<object>());
							mediationAdLoadInfo.errMsg = androidJavaObject2.Call<string>("getErrMsg", Array.Empty<object>());
							list.Add(mediationAdLoadInfo);
						}
					}
					return list;
				}
			}
			return null;
		}

		public static List<MediationAdEcpmInfo> GetMultiBiddingEcpm(AndroidJavaObject handle)
		{
			return GetMediationAdEcpmInfos(handle, "getMultiBiddingEcpm");
		}

		public static List<MediationAdEcpmInfo> GetCacheList(AndroidJavaObject handle)
		{
			return GetMediationAdEcpmInfos(handle, "getCacheList");
		}

		public static MediationAdEcpmInfo GetBestEcpm(AndroidJavaObject handle)
		{
			if (handle != null)
			{
				return GetMediationAdEcpmInfo(handle.Call<AndroidJavaObject>("getBestEcpm", Array.Empty<object>()));
			}
			return null;
		}

		public static MediationAdEcpmInfo GetShowEcpm(AndroidJavaObject handle)
		{
			if (handle != null)
			{
				return GetMediationAdEcpmInfo(handle.Call<AndroidJavaObject>("getShowEcpm", Array.Empty<object>()));
			}
			return null;
		}

		private static List<MediationAdEcpmInfo> GetMediationAdEcpmInfos(AndroidJavaObject handle, string javaMethodName)
		{
			if (handle != null)
			{
				AndroidJavaObject androidJavaObject = handle.Call<AndroidJavaObject>(javaMethodName, Array.Empty<object>());
				if (androidJavaObject != null)
				{
					List<MediationAdEcpmInfo> list = new List<MediationAdEcpmInfo>();
					int num = androidJavaObject.Call<int>("size", Array.Empty<object>());
					for (int i = 0; i < num; i++)
					{
						AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("get", new object[1] { i });
						if (androidJavaObject2 != null)
						{
							MediationAdEcpmInfo mediationAdEcpmInfo = GetMediationAdEcpmInfo(androidJavaObject2);
							if (mediationAdEcpmInfo != null)
							{
								list.Add(mediationAdEcpmInfo);
							}
						}
					}
					return list;
				}
			}
			return null;
		}

		private static MediationAdEcpmInfo GetMediationAdEcpmInfo(AndroidJavaObject jEcpmInfo)
		{
			if (jEcpmInfo != null)
			{
				MediationAdEcpmInfo mediationAdEcpmInfo = new MediationAdEcpmInfo();
				mediationAdEcpmInfo.sdkName = jEcpmInfo.Call<string>("getSdkName", Array.Empty<object>());
				mediationAdEcpmInfo.customSdkName = jEcpmInfo.Call<string>("getCustomSdkName", Array.Empty<object>());
				mediationAdEcpmInfo.slotId = jEcpmInfo.Call<string>("getSlotId", Array.Empty<object>());
				mediationAdEcpmInfo.levelTag = jEcpmInfo.Call<string>("getLevelTag", Array.Empty<object>());
				mediationAdEcpmInfo.ecpm = jEcpmInfo.Call<string>("getEcpm", Array.Empty<object>());
				mediationAdEcpmInfo.reqBiddingType = jEcpmInfo.Call<int>("getReqBiddingType", Array.Empty<object>());
				mediationAdEcpmInfo.errorMsg = jEcpmInfo.Call<string>("getErrorMsg", Array.Empty<object>());
				mediationAdEcpmInfo.requestId = jEcpmInfo.Call<string>("getRequestId", Array.Empty<object>());
				mediationAdEcpmInfo.ritType = jEcpmInfo.Call<string>("getRitType", Array.Empty<object>());
				mediationAdEcpmInfo.segmentId = jEcpmInfo.Call<string>("getSegmentId", Array.Empty<object>());
				mediationAdEcpmInfo.channel = jEcpmInfo.Call<string>("getChannel", Array.Empty<object>());
				mediationAdEcpmInfo.subChannel = jEcpmInfo.Call<string>("getSubChannel", Array.Empty<object>());
				mediationAdEcpmInfo.abTestId = jEcpmInfo.Call<string>("getAbTestId", Array.Empty<object>());
				mediationAdEcpmInfo.scenarioId = jEcpmInfo.Call<string>("getScenarioId", Array.Empty<object>());
				AndroidJavaObject androidJavaObject = jEcpmInfo.Call<AndroidJavaObject>("getCustomData", Array.Empty<object>());
				if (androidJavaObject != null)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("entrySet", Array.Empty<object>()).Call<AndroidJavaObject>("iterator", Array.Empty<object>());
					while (androidJavaObject2.Call<bool>("hasNext", Array.Empty<object>()))
					{
						AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("next", Array.Empty<object>());
						dictionary.Add(androidJavaObject3.Call<string>("getKey", Array.Empty<object>()), androidJavaObject3.Call<string>("getValue", Array.Empty<object>()));
					}
					mediationAdEcpmInfo.customData = dictionary;
				}
				return mediationAdEcpmInfo;
			}
			return null;
		}
	}
}
