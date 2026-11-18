using System;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public sealed class ExpressAd : IDisposable, IClientBidding
	{
		private sealed class ExpressAdInteractionCallback : AndroidJavaProxy
		{
			private IExpressAdInteractionListener listener;

			private bool callbackOnMainThread;

			public ExpressAdInteractionCallback(IExpressAdInteractionListener callback, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTNativeExpressAd$ExpressAdInteractionListener")
			{
				listener = callback;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onAdClicked(AndroidJavaObject view, int type)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClicked(null);
				}, callbackOnMainThread);
			}

			public void onAdShow(AndroidJavaObject view, int type)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdShow(null);
				}, callbackOnMainThread);
			}

			public void onRenderFail(AndroidJavaObject view, string msg, int code)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdViewRenderError(null, code, msg);
				}, callbackOnMainThread);
			}

			public void onRenderSuccess(AndroidJavaObject view, float width, float height)
			{
				Debug.Log("CSJM_Unity onRenderSuccess ");
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdViewRenderSucc(null, width, height);
				}, callbackOnMainThread);
			}
		}

		private IExpressAdInteractionListener interactionListener;

		private IDislikeInteractionListener dislikeListener;

		public AndroidJavaObject javaObject;

		public int index;

		public AndroidJavaObject handle => javaObject;

		internal ExpressAd(AndroidJavaObject expressAd)
		{
			javaObject = expressAd;
		}

		public int GetInteractionType()
		{
			return javaObject.Call<int>("getInteractionType", Array.Empty<object>());
		}

		public void SetExpressInteractionListener(IExpressAdInteractionListener listener, bool callbackOnMainThread = true)
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

		public void SetNotAllowSdkCountdown()
		{
		}

		public void ShowExpressAd(float x, float y)
		{
			NativeAdManager.Instance().ShowExpressFeedAd(Utils.GetActivity(), handle, interactionListener, dislikeListener, callbackOnMainThread: true, (int)x, (int)y);
		}

		public void Dispose()
		{
			NativeAdManager.Instance().DestroyExpressFeedAd(javaObject);
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

		public MediationNativeManager GetMediationManager()
		{
			return new MediationNativeManager(javaObject.Call<AndroidJavaObject>("getMediationManager", Array.Empty<object>()));
		}
	}
}
