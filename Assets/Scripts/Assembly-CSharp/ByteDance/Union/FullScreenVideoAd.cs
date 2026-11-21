using System;
using System.Collections.Generic;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public sealed class FullScreenVideoAd : IClientBidding
	{
		private sealed class FullScreenVideoAdInteractionListener : AndroidJavaProxy
		{
			private readonly IFullScreenVideoAdInteractionListener listener;

			private bool callbackOnMainThread;

			public FullScreenVideoAdInteractionListener(IFullScreenVideoAdInteractionListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTFullScreenVideoAd$FullScreenVideoAdInteractionListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onAdShow()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdShow();
				}, callbackOnMainThread);
			}

			public void onAdVideoBarClick()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdVideoBarClick();
				}, callbackOnMainThread);
			}

			public void onAdClose()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClose();
				}, callbackOnMainThread);
			}

			public void onVideoComplete()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoComplete();
				}, callbackOnMainThread);
			}

			public void onSkippedVideo()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnSkippedVideo();
				}, callbackOnMainThread);
			}
		}

		private readonly AndroidJavaObject ad;

		internal FullScreenVideoAd(AndroidJavaObject ad)
		{
			this.ad = ad;
		}

		public void Dispose()
		{
		}

		public void SetFullScreenVideoAdInteractionListener(IFullScreenVideoAdInteractionListener listener, bool callbackOnMainThread = true)
		{
			FullScreenVideoAdInteractionListener fullScreenVideoAdInteractionListener = new FullScreenVideoAdInteractionListener(listener, callbackOnMainThread);
			ad.Call("setFullScreenVideoAdInteractionListener", fullScreenVideoAdInteractionListener);
		}

		public void SetDownloadListener(IAppDownloadListener listener, bool callbackOnMainThread = true)
		{
			AppDownloadListener appDownloadListener = new AppDownloadListener(listener, callbackOnMainThread);
			ad.Call("setDownloadListener", appDownloadListener);
		}

		public int GetInteractionType()
		{
			return ad.Call<int>("getInteractionType", Array.Empty<object>());
		}

		public void ShowFullScreenVideoAd()
		{
			AndroidJavaObject activity = Utils.GetActivity();
			AndroidJavaRunnable androidJavaRunnable = () =>
			{
				ad.Call("showFullScreenVideoAd", activity);
			};
			activity.Call("runOnUiThread", androidJavaRunnable);
		}

		public void SetShowDownLoadBar(bool show)
		{
			ad.Call("setShowDownLoadBar", show);
		}

		public Dictionary<string, string> GetMediaExtraInfo()
		{
			AndroidJavaObject androidJavaObject = ad.Call<AndroidJavaObject>("getMediaExtraInfo", Array.Empty<object>());
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("entrySet", Array.Empty<object>()).Call<AndroidJavaObject>("iterator", Array.Empty<object>());
			while (androidJavaObject2.Call<bool>("hasNext", Array.Empty<object>()))
			{
				AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("next", Array.Empty<object>());
				string key = androidJavaObject3.Call<AndroidJavaObject>("getKey", Array.Empty<object>()).Call<string>("toString", Array.Empty<object>());
				string value = androidJavaObject3.Call<AndroidJavaObject>("getValue", Array.Empty<object>()).Call<string>("toString", Array.Empty<object>());
				dictionary.Add(key, value);
			}
			return dictionary;
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

		public MediationFullScreenManager GetMediationManager()
		{
			return new MediationFullScreenManager(ad.Call<AndroidJavaObject>("getMediationManager", Array.Empty<object>()));
		}
	}
}
