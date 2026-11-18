using System;
using UnityEngine;

namespace ByteDance.Union.Mediation
{
	public sealed class MediationAdSlot
	{
		public class Builder
		{
			private AndroidJavaObject jBuilder;

			public Builder()
			{
				jBuilder = new AndroidJavaObject("com.bytedance.sdk.openadsdk.mediation.ad.MediationAdSlot$Builder");
			}

			public Builder SetSplashShakeButton(bool splashShakeBtn)
			{
				jBuilder.Call<AndroidJavaObject>("setSplashShakeButton", new object[1] { splashShakeBtn });
				return this;
			}

			public Builder SetAllowShowCloseBtn(bool allowShowCloseBtn)
			{
				jBuilder.Call<AndroidJavaObject>("setAllowShowCloseBtn", new object[1] { allowShowCloseBtn });
				return this;
			}

			public Builder SetShakeViewSize(float w, float h)
			{
				jBuilder.Call<AndroidJavaObject>("setShakeViewSize", new object[2] { w, h });
				return this;
			}

			public Builder SetWxAppId(string wxAppId)
			{
				jBuilder.Call<AndroidJavaObject>("setWxAppId", new object[1] { wxAppId });
				return this;
			}

			public Builder SetMediationSplashRequestInfo(MediationSplashRequestInfo info)
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.bytedance.android.MediationSplashReqInfo", info.AdnName, info.AdnSlotId, info.AppId, info.Appkey);
				jBuilder.Call<AndroidJavaObject>("setMediationSplashRequestInfo", new object[1] { androidJavaObject });
				return this;
			}

			public Builder SetSplashPreLoad(bool isPreload)
			{
				jBuilder.Call<AndroidJavaObject>("setSplashPreLoad", new object[1] { isPreload });
				return this;
			}

			public Builder SetMuted(bool isMuted)
			{
				jBuilder.Call<AndroidJavaObject>("setMuted", new object[1] { isMuted });
				return this;
			}

			public Builder SetVolume(float volume)
			{
				jBuilder.Call<AndroidJavaObject>("setVolume", new object[1] { volume });
				return this;
			}

			public Builder SetUseSurfaceView(bool isUseSurfaceview)
			{
				jBuilder.Call<AndroidJavaObject>("setUseSurfaceView", new object[1] { isUseSurfaceview });
				return this;
			}

			public Builder SetBidNotify(bool bidNotify)
			{
				jBuilder.Call<AndroidJavaObject>("setBidNotify", new object[1] { bidNotify });
				return this;
			}

			public Builder SetScenarioId(string scenarioId)
			{
				jBuilder.Call<AndroidJavaObject>("setScenarioId", new object[1] { scenarioId });
				return this;
			}

			public Builder SetRewardName(string rewardName)
			{
				jBuilder.Call<AndroidJavaObject>("setRewardName", new object[1] { rewardName });
				return this;
			}

			public Builder SetRewardAmount(int amount)
			{
				jBuilder.Call<AndroidJavaObject>("setRewardAmount", new object[1] { amount });
				return this;
			}

			public Builder SetExtraObject(string key, string value)
			{
				jBuilder.Call<AndroidJavaObject>("setExtraObject", new object[2] { key, value });
				return this;
			}

			public MediationAdSlot Build()
			{
				AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.bytedance.android.MediationNativeToBannerUtils").CallStatic<AndroidJavaObject>("getMediationNativeToBannerListener", new object[1] { Utils.GetActivity() });
				jBuilder.Call<AndroidJavaObject>("setMediationNativeToBannerListener", new object[1] { androidJavaObject });
				return new MediationAdSlot(jBuilder.Call<AndroidJavaObject>("build", Array.Empty<object>()));
			}
		}

		internal AndroidJavaObject jMediationAdSlot;

		internal AndroidJavaObject Handle => jMediationAdSlot;

		public MediationAdSlot(AndroidJavaObject slot)
		{
			jMediationAdSlot = slot;
		}
	}
}
