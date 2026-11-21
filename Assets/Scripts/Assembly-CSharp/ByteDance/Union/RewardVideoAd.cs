using System;
using System.Collections.Generic;
using ByteDance.Union.Mediation;
using UnityEngine;

namespace ByteDance.Union
{
	public sealed class RewardVideoAd : IDisposable, IClientBidding
	{
		private sealed class RewardAdInteractionListener : AndroidJavaProxy
		{
			private readonly IRewardAdInteractionListener listener;

			private readonly bool callbackOnMainThread;

			public RewardAdInteractionListener(IRewardAdInteractionListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTRewardVideoAd$RewardAdInteractionListener")
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

			public void onVideoError()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoError();
				}, callbackOnMainThread);
			}

			public void onRewardVerify(bool rewardVerify, int rewardAmount, string rewardName, int errorCode, string errorMsg)
			{
			}

			public void onRewardArrived(bool isRewardValid, int rewardType, AndroidJavaObject extraInfo)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnRewardArrived(isRewardValid, rewardType, RewardBundleModel.Create(extraInfo));
				}, callbackOnMainThread);
			}

			public void onSkippedVideo()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoSkip();
				}, callbackOnMainThread);
			}
		}

		private sealed class RewardPlayAgainControllerListener : AndroidJavaProxy
		{
			private readonly IRewardAdPlayAgainController listener;

			private readonly bool callbackOnMainThread;

			public RewardPlayAgainControllerListener(IRewardAdPlayAgainController listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTRewardVideoAd$RewardAdPlayAgainController")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void getPlayAgainCondition(int nextPlayAgainCount, AndroidJavaObject callback)
			{
				listener.GetPlayAgainCondition(nextPlayAgainCount, (PlayAgainCallbackBean bean) =>
				{
					AndroidBundleWrapper androidBundleWrapper = new AndroidBundleWrapper();
					androidBundleWrapper.putBoolean("play_again_allow", bean.IsAgain);
					androidBundleWrapper.putString("play_again_reward_name", bean.RewardName);
					androidBundleWrapper.putString("play_again_reward_amount", bean.RewardCount);
					callback.Call("onConditionReturn", androidBundleWrapper.getAndroidBundle());
				});
			}
		}

		private readonly AndroidJavaObject ad;

		internal RewardVideoAd(AndroidJavaObject ad)
		{
			this.ad = new AndroidJavaObject("com.bytedance.android.RewardVideoAdWrapper", ad);
		}

		public void Dispose()
		{
		}

		public void SetRewardAdInteractionListener(IRewardAdInteractionListener listener, bool callbackOnMainThread = true)
		{
			RewardAdInteractionListener rewardAdInteractionListener = new RewardAdInteractionListener(listener, callbackOnMainThread);
			ad.Call("setRewardAdInteractionListener", rewardAdInteractionListener);
		}

		public void SetAgainRewardAdInteractionListener(IRewardAdInteractionListener againListener, bool callbackOnMainThread = true)
		{
			RewardAdInteractionListener rewardAdInteractionListener = new RewardAdInteractionListener(againListener, callbackOnMainThread);
			ad.Call("setRewardPlayAgainInteractionListener", rewardAdInteractionListener);
		}

		public void SetRewardPlayAgainController(IRewardAdPlayAgainController againListener, bool callbackOnMainThread = true)
		{
			RewardPlayAgainControllerListener rewardPlayAgainControllerListener = new RewardPlayAgainControllerListener(againListener, callbackOnMainThread);
			ad.Call("setRewardPlayAgainController", rewardPlayAgainControllerListener);
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

		public void ShowRewardVideoAd()
		{
			AndroidJavaObject activity = Utils.GetActivity();
			AndroidJavaRunnable androidJavaRunnable = () =>
			{
				ad.Call("showRewardVideoAd", activity);
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

		public MediationRewardManager GetMediationManager()
		{
			return new MediationRewardManager(ad.Call<AndroidJavaObject>("getMediationManager", Array.Empty<object>()));
		}
	}
}
