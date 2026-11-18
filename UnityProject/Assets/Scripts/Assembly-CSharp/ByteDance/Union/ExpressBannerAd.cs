using System;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public sealed class ExpressBannerAd : IClientBidding
	{
		public AndroidJavaObject javaObject;

		private IExpressBannerInteractionListener interactionListener;

		private IDislikeInteractionListener dislikeListener;

		public AndroidJavaObject handle => javaObject;

		internal ExpressBannerAd(AndroidJavaObject expressAd)
		{
			javaObject = expressAd;
		}

		public void SetExpressInteractionListener(IExpressBannerInteractionListener listener, bool callbackOnMainThread = true)
		{
			interactionListener = listener;
		}

		public void SetDislikeCallback(IDislikeInteractionListener dislikeCallback, bool callbackOnMainThread = true)
		{
			dislikeListener = dislikeCallback;
		}

		public void SetDownloadListener(IAppDownloadListener listener, bool callbackOnMainThread = true)
		{
			AppDownloadListener appDownloadListener = new AppDownloadListener(listener, callbackOnMainThread);
			javaObject.Call("setDownloadListener", appDownloadListener);
		}

		public void ShowExpressAd(float x, float y)
		{
			NativeAdManager.Instance().ShowExpressBannerAd(Utils.GetActivity(), handle, interactionListener, dislikeListener, callbackOnMainThread: true, (int)x, (int)y);
		}

		public void Dispose()
		{
			NativeAdManager.Instance().DestroyExpressBannerAd(javaObject);
		}

		public void SetSlideIntervalTime(int intervalTime)
		{
			javaObject.Call("setSlideIntervalTime", intervalTime);
		}

		public void win(double auctionBidToWin)
		{
			ClientBiddingUtils.Win(javaObject, auctionBidToWin);
		}

		public void Loss(double auctionPrice = double.NaN, string lossReason = null, string winBidder = null)
		{
			ClientBiddingUtils.Loss(javaObject, auctionPrice, lossReason, winBidder);
		}

		public void setAuctionPrice(double auctionPrice = double.NaN)
		{
			ClientBiddingUtils.SetPrice(javaObject, auctionPrice);
		}

		public void SetAdInteractionListener(ITTAdInteractionListener ilistener, bool callbackOnMainThread = true)
		{
			TTAdInteractionListener tTAdInteractionListener = new TTAdInteractionListener(ilistener, callbackOnMainThread);
			javaObject.Call("setAdInteractionListener", tTAdInteractionListener);
		}

		public void UploadDislikeEvent(string dislikeStr)
		{
			if (dislikeStr != null)
			{
				javaObject.Call("uploadDislikeEvent", dislikeStr);
			}
		}

		public MediationBannerManager GetMediationManager()
		{
			return new MediationBannerManager(javaObject.Call<AndroidJavaObject>("getMediationManager", Array.Empty<object>()));
		}
	}
}
