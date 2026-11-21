using System;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public sealed class AdSlot
	{
		public sealed class Builder
		{
			private AndroidJavaObject builder;

			public Builder()
			{
				builder = new AndroidJavaObject("com.bytedance.sdk.openadsdk.AdSlot$Builder");
			}

			public Builder SetCodeId(string codeId)
			{
				builder.Call<AndroidJavaObject>("setCodeId", new object[1] { codeId });
				return this;
			}

			public Builder SetImageAcceptedSize(int width, int height)
			{
				builder.Call<AndroidJavaObject>("setImageAcceptedSize", new object[2] { width, height });
				return this;
			}

			public Builder SetExpressViewAcceptedSize(float width, float height)
			{
				builder.Call<AndroidJavaObject>("setExpressViewAcceptedSize", new object[2] { width, height });
				return this;
			}

			public Builder SetSupportDeepLink(bool support)
			{
				builder.Call<AndroidJavaObject>("setSupportDeepLink", new object[1] { support });
				return this;
			}

			public Builder SetAdCount(int count)
			{
				builder.Call<AndroidJavaObject>("setAdCount", new object[1] { count });
				return this;
			}

			public Builder SetNativeAdType(AdSlotType type)
			{
				builder.Call<AndroidJavaObject>("setNativeAdType", new object[1] { type.ToAndroid() });
				return this;
			}

			public Builder SetRewardName(string name)
			{
				builder.Call<AndroidJavaObject>("setRewardName", new object[1] { name });
				return this;
			}

			public Builder SetRewardAmount(int amount)
			{
				builder.Call<AndroidJavaObject>("setRewardAmount", new object[1] { amount });
				return this;
			}

			public Builder SetUserID(string id)
			{
				builder.Call<AndroidJavaObject>("setUserID", new object[1] { id });
				return this;
			}

			public Builder SetOrientation(AdOrientation orientation)
			{
				builder.Call<AndroidJavaObject>("setOrientation", new object[1] { orientation.ToAndroid() });
				return this;
			}

			public Builder SetMediaExtra(string extra)
			{
				builder.Call<AndroidJavaObject>("setMediaExtra", new object[1] { extra });
				return this;
			}

			public Builder SetSlideIntervalTime(int intervalTime)
			{
				return this;
			}

			public Builder SetAdLoadType(AdLoadType adLoadType)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.bytedance.sdk.openadsdk.TTAdLoadType");
				AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("UNKNOWN");
				switch (adLoadType)
				{
				case AdLoadType.PreLoad:
					androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("PRELOAD");
					break;
				case AdLoadType.Load:
					androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("LOAD");
					break;
				}
				builder.Call<AndroidJavaObject>("setAdLoadType", new object[1] { androidJavaObject });
				return this;
			}

			public Builder SupportIconStyle()
			{
				builder.Call<AndroidJavaObject>("supportIconStyle", Array.Empty<object>());
				return this;
			}

			public Builder SetMediationAdSlot(MediationAdSlot mediationAdSlot)
			{
				builder.Call<AndroidJavaObject>("setMediationAdSlot", new object[1] { mediationAdSlot.Handle });
				return this;
			}

			public AdSlot Build()
			{
				return new AdSlot(builder.Call<AndroidJavaObject>("build", Array.Empty<object>()));
			}
		}

		private AndroidJavaObject slot;

		internal AndroidJavaObject Handle => slot;

		internal AdSlot(AndroidJavaObject slot)
		{
			this.slot = slot;
		}
	}
}
