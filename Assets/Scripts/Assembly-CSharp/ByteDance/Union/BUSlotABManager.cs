using System;
using UnityEngine;

namespace ByteDance.Union
{
	public static class BUSlotABManager
	{
		private class TtCodeGroupRitListener : AndroidJavaProxy
		{
			private IBUSlotABManagerListener managerListener;

			public TtCodeGroupRitListener(IBUSlotABManagerListener listener)
				: base("com.bytedance.sdk.openadsdk.TTCodeGroupRit$TTCodeGroupRitListener")
			{
				managerListener = listener;
			}

			public void onSuccess(AndroidJavaObject ritObject)
			{
				string slotId = ritObject.Call<string>("getRit", Array.Empty<object>());
				AdSlotType adSlotType = GetAdSlotType(ritObject.Call<int>("getSlotType", Array.Empty<object>()));
				managerListener?.onComplete(slotId, adSlotType, 0, null);
			}

			public void onFail(int code, string message)
			{
				managerListener?.onComplete(null, AdSlotType.UNKOWN, code, message);
			}

			private static AdSlotType GetAdSlotType(int slotType)
			{
				return slotType switch
				{
					1 => AdSlotType.Banner, 
					2 => AdSlotType.InteractionAd, 
					3 => AdSlotType.Splash, 
					4 => AdSlotType.CachedSplash, 
					5 => AdSlotType.Feed, 
					7 => AdSlotType.RewardVideo, 
					8 => AdSlotType.FullScreenVideo, 
					9 => AdSlotType.DrawFeed, 
					_ => AdSlotType.UNKOWN, 
				};
			}
		}

		private const string TtAdSdkClassString = "com.bytedance.sdk.openadsdk.TTAdSdk";

		public static void FetchSlotWithCodeGroupId(long codeGroupId, IBUSlotABManagerListener listener)
		{
			if (listener != null)
			{
				new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdSdk").CallStatic("getCodeGroupRit", codeGroupId, new TtCodeGroupRitListener(listener));
			}
		}
	}
}
