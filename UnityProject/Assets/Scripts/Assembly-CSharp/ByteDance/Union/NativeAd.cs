using System;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public class NativeAd : IClientBidding
	{
		protected readonly AndroidJavaObject ad;

		private IInteractionAdInteractionListener interactionListener;

		private IDislikeInteractionListener dislikeLinstener;

		internal NativeAd(AndroidJavaObject ad)
		{
			this.ad = ad;
		}

		public string GetTitle()
		{
			return ad.Call<string>("getTitle", Array.Empty<object>());
		}

		public string GetDescription()
		{
			return ad.Call<string>("getDescription", Array.Empty<object>());
		}

		public string GetButtonText()
		{
			return ad.Call<string>("getButtonText", Array.Empty<object>());
		}

		public int GetAppScore()
		{
			return ad.Call<int>("getAppScore", Array.Empty<object>());
		}

		public int GetAppCommentNum()
		{
			return ad.Call<int>("getAppCommentNum", Array.Empty<object>());
		}

		public int GetAppSize()
		{
			return ad.Call<int>("getAppSize", Array.Empty<object>());
		}

		public string GetSource()
		{
			return ad.Call<string>("getSource", Array.Empty<object>());
		}

		public int GetInteractionType()
		{
			return ad.Call<int>("getInteractionType", Array.Empty<object>());
		}

		public int GetImageMode()
		{
			return ad.Call<int>("getImageMode", Array.Empty<object>());
		}

		public void SetDownloadListener(IAppDownloadListener listener, bool callbackOnMainThread = true)
		{
			AppDownloadListener appDownloadListener = new AppDownloadListener(listener, callbackOnMainThread);
			ad.Call("setDownloadListener", appDownloadListener);
		}

		public void Dispose()
		{
			NativeAdManager.Instance().DestroyNativeAd(ad);
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

		public void UploadDislikeEvent(string dislikeStr)
		{
			if (dislikeStr != null)
			{
				ad.Call("uploadDislikeEvent", dislikeStr);
			}
		}

		public MediationNativeManager GetMediationManager()
		{
			return new MediationNativeManager(ad.Call<AndroidJavaObject>("getMediationManager", Array.Empty<object>()));
		}

		public void RenderNative(AndroidJavaObject activity, IDislikeInteractionListener dislikeInteractionListener, AdSlotType type, bool callbackOnMainThread = true)
		{
		}

		public void ShowNativeAd(AdSlotType type, float x, float y)
		{
			NativeAdManager.Instance().ShowNativeBannerAd(Utils.GetActivity(), ad, interactionListener, dislikeLinstener, (int)x, (int)y);
		}

		public void SetNativeAdInteractionListener(IInteractionAdInteractionListener listener, bool callbackOnMainThread = true)
		{
			interactionListener = listener;
		}

		public void SetNativeAdDislikeListener(IDislikeInteractionListener dislikeInteractionListener)
		{
			dislikeLinstener = dislikeInteractionListener;
		}
	}
}
