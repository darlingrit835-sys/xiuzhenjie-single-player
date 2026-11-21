using System;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public class BUSplashAd : IClientBidding
	{
		private sealed class AdInteractionListener : AndroidJavaProxy
		{
			private readonly ISplashAdInteractionListener listener;

			private bool callbackOnMainThread;

			public AdInteractionListener(ISplashAdInteractionListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.CSJSplashAd$SplashAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onSplashAdClick(AndroidJavaObject ad)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClicked(getType(ad));
				}, callbackOnMainThread);
			}

			public void onSplashAdShow(AndroidJavaObject ad)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdDidShow(getType(ad));
				}, callbackOnMainThread);
			}

			public void onSplashAdClose(AndroidJavaObject ad, int closeType)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClose(closeType);
				}, callbackOnMainThread);
			}

			public int getType(AndroidJavaObject ad)
			{
				int result = 0;
				if (ad != null)
				{
					result = ad.Call<int>("getInteractionType", Array.Empty<object>());
				}
				return result;
			}
		}

		private readonly AndroidJavaObject ad;

		internal BUSplashAd(AndroidJavaObject ad)
		{
			this.ad = ad;
		}

		public int GetInteractionType()
		{
			return ad.Call<int>("getInteractionType", Array.Empty<object>());
		}

		public void ShowSplashAd()
		{
			AndroidJavaObject javaObject = new AndroidJavaClass("com.bytedance.android.SplashAdManager").CallStatic<AndroidJavaObject>("getSplashAdManager", Array.Empty<object>());
			object[] obs = new object[2]
			{
				Utils.GetActivity(),
				getCurrentSplshAd()
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.CSJSplashAd;)V";
			CallJavaMethod(javaObject, "showSplashAd", signature, obs);
		}

		private static void CallJavaMethod(AndroidJavaObject javaObject, string methodName, string signature, object[] obs)
		{
			IntPtr methodID = AndroidJNIHelper.GetMethodID(javaObject.GetRawClass(), methodName, signature);
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(obs);
			try
			{
				AndroidJNI.CallVoidMethod(javaObject.GetRawObject(), methodID, array);
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(obs, array);
			}
		}

		public AndroidJavaObject getCurrentSplshAd()
		{
			return ad;
		}

		public void SetSplashInteractionListener(ISplashAdInteractionListener listener, bool callbackOnMainThread = true)
		{
			AdInteractionListener adInteractionListener = new AdInteractionListener(listener, callbackOnMainThread);
			ad.Call("setSplashAdListener", adInteractionListener);
		}

		public void SetDownloadListener(IAppDownloadListener listener, bool callbackOnMainThread = true)
		{
			AppDownloadListener appDownloadListener = new AppDownloadListener(listener, callbackOnMainThread);
			ad.Call("setDownloadListener", appDownloadListener);
		}

		public void SetNotAllowSdkCountdown()
		{
			ad.Call("hideSkipButton");
		}

		public void Dispose()
		{
			new AndroidJavaClass("com.bytedance.android.SplashAdManager").CallStatic<AndroidJavaObject>("getSplashAdManager", Array.Empty<object>()).Call("destorySplashView", Utils.GetActivity());
		}

		public void win(double auctionBidToWin)
		{
			ClientBiddingUtils.Win(ad, auctionBidToWin);
		}

		public void Loss(double auctionPrice = double.NaN, string lossReason = null, string winBidder = null)
		{
			ClientBiddingUtils.Loss(ad, auctionPrice, lossReason, winBidder);
		}

		public void setAuctionPrice(double auctionPrice = double.NaN)
		{
			ClientBiddingUtils.SetPrice(ad, auctionPrice);
		}

		public void SetAdInteractionListener(ITTAdInteractionListener ilistener, bool callbackOnMainThread = true)
		{
			TTAdInteractionListener tTAdInteractionListener = new TTAdInteractionListener(ilistener, callbackOnMainThread);
			ad.Call("setAdInteractionListener", tTAdInteractionListener);
		}

		public MediationSplashManager GetMediationManager()
		{
			return new MediationSplashManager(ad.Call<AndroidJavaObject>("getMediationManager", Array.Empty<object>()));
		}
	}
}
