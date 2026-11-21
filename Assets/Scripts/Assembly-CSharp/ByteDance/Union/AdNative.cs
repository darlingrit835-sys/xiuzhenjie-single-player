using System;
using System.Collections.Generic;
using UnityEngine;

namespace ByteDance.Union
{
	public sealed class AdNative
	{
		private sealed class FeedAdListener : AndroidJavaProxy
		{
			private readonly IFeedAdListener listener;

			private bool callbackOnMainThread;

			public FeedAdListener(IFeedAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$FeedAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onFeedAdLoad(AndroidJavaObject list)
			{
				int num = list.Call<int>("size", Array.Empty<object>());
				FeedAd[] ads = new FeedAd[num];
				for (int i = 0; i < num; i++)
				{
					ads[i] = new FeedAd(list.Call<AndroidJavaObject>("get", new object[1] { i }));
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnFeedAdLoad(ads);
				}, callbackOnMainThread);
			}
		}

		private sealed class DrawFeedAdListener : AndroidJavaProxy
		{
			private readonly IDrawFeedAdListener listener;

			private bool callbackOnMainThread;

			public DrawFeedAdListener(IDrawFeedAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$DrawFeedAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onDrawFeedAdLoad(AndroidJavaObject list)
			{
				int num = list.Call<int>("size", Array.Empty<object>());
				DrawFeedAd[] ads = new DrawFeedAd[num];
				for (int i = 0; i < num; i++)
				{
					ads[i] = new DrawFeedAd(list.Call<AndroidJavaObject>("get", new object[1] { i }));
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnDrawFeedAdLoad(ads);
				}, callbackOnMainThread);
			}
		}

		private sealed class NativeAdListener : AndroidJavaProxy
		{
			private readonly INativeAdListener listener;

			private bool callbackOnMainThread;

			public NativeAdListener(INativeAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$NativeAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onNativeAdLoad(AndroidJavaObject list)
			{
				int num = list.Call<int>("size", Array.Empty<object>());
				NativeAd[] ads = new NativeAd[num];
				for (int i = 0; i < num; i++)
				{
					ads[i] = new NativeAd(list.Call<AndroidJavaObject>("get", new object[1] { i }));
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnNativeAdLoad(ads);
				}, callbackOnMainThread);
			}
		}

		private sealed class SplashAdListener : AndroidJavaProxy
		{
			private BUSplashAd buSplashAd;

			private readonly ISplashAdListener listener;

			private bool callbackOnMainThread;

			public SplashAdListener(ISplashAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$CSJSplashAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onSplashLoadSuccess(AndroidJavaObject handle)
			{
				if (buSplashAd == null)
				{
					buSplashAd = new BUSplashAd(handle);
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnSplashLoadSuccess(buSplashAd);
				}, callbackOnMainThread);
			}

			public void onSplashLoadFail(AndroidJavaObject error)
			{
				int code = 0;
				string message = "";
				if (error != null)
				{
					code = error.Call<int>("getCode", Array.Empty<object>());
					message = error.Call<string>("getMsg", Array.Empty<object>());
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnSplashLoadFail(code, message);
				}, callbackOnMainThread);
			}

			public void onSplashRenderSuccess(AndroidJavaObject handle)
			{
				if (buSplashAd == null)
				{
					buSplashAd = new BUSplashAd(handle);
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnSplashRenderSuccess(buSplashAd);
				}, callbackOnMainThread);
			}

			public void onSplashRenderFail(AndroidJavaObject ad, AndroidJavaObject error)
			{
				int code = 0;
				string message = "";
				if (error != null)
				{
					code = error.Call<int>("getCode", Array.Empty<object>());
					message = error.Call<string>("getMsg", Array.Empty<object>());
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnSplashRenderFail(code, message);
				}, callbackOnMainThread);
			}
		}

		private sealed class RewardVideoAdListener : AndroidJavaProxy
		{
			private readonly IRewardVideoAdListener listener;

			private bool callbackOnMainThread;

			public RewardVideoAdListener(IRewardVideoAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$RewardVideoAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onRewardVideoAdLoad(AndroidJavaObject handle)
			{
				RewardVideoAd ad = new RewardVideoAd(handle);
				UnityDispatcher.PostTask(() =>
				{
					listener.OnRewardVideoAdLoad(ad);
				}, callbackOnMainThread);
			}

			public void onRewardVideoCached()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnRewardVideoCached();
				}, callbackOnMainThread);
			}

			public void onRewardVideoCached(AndroidJavaObject handle)
			{
				RewardVideoAd ad = new RewardVideoAd(handle);
				UnityDispatcher.PostTask(() =>
				{
					listener.OnRewardVideoCached(ad);
				}, callbackOnMainThread);
			}
		}

		private sealed class FullScreenVideoAdListener : AndroidJavaProxy
		{
			private readonly IFullScreenVideoAdListener listener;

			private bool callbackOnMainThread;

			public FullScreenVideoAdListener(IFullScreenVideoAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$FullScreenVideoAdListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onFullScreenVideoAdLoad(AndroidJavaObject handle)
			{
				FullScreenVideoAd ad = new FullScreenVideoAd(handle);
				UnityDispatcher.PostTask(() =>
				{
					listener.OnFullScreenVideoAdLoad(ad);
				}, callbackOnMainThread);
			}

			public void onFullScreenVideoCached()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnFullScreenVideoCached();
				}, callbackOnMainThread);
			}

			public void onFullScreenVideoCached(AndroidJavaObject handle)
			{
				FullScreenVideoAd ad = new FullScreenVideoAd(handle);
				UnityDispatcher.PostTask(() =>
				{
					listener.OnFullScreenVideoCached(ad);
				}, callbackOnMainThread);
			}
		}

		private sealed class ExpressAdListener : AndroidJavaProxy
		{
			private readonly IExpressAdListener callback;

			private bool callbackOnMainThread;

			public ExpressAdListener(IExpressAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$NativeExpressAdListener")
			{
				callback = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					callback.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onNativeExpressAdLoad(AndroidJavaObject ads)
			{
				int num = ads.Call<int>("size", Array.Empty<object>());
				List<ExpressAd> expressAds = new List<ExpressAd>();
				for (int i = 0; i < num; i++)
				{
					ExpressAd item = new ExpressAd(ads.Call<AndroidJavaObject>("get", new object[1] { i }));
					expressAds.Insert(i, item);
				}
				UnityDispatcher.PostTask(() =>
				{
					callback.OnExpressAdLoad(expressAds);
				}, callbackOnMainThread);
			}
		}

		private sealed class ExpressBannerAdListener : AndroidJavaProxy
		{
			private readonly IExpressBannerAdListener callback;

			private bool callbackOnMainThread;

			public ExpressBannerAdListener(IExpressBannerAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdNative$NativeExpressAdListener")
			{
				callback = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onError(int code, string message)
			{
				UnityDispatcher.PostTask(() =>
				{
					callback.OnError(code, message);
				}, callbackOnMainThread);
			}

			public void onNativeExpressAdLoad(AndroidJavaObject ads)
			{
				if (ads.Call<int>("size", Array.Empty<object>()) > 0)
				{
					ExpressBannerAd ad = new ExpressBannerAd(ads.Call<AndroidJavaObject>("get", new object[1] { 0 }));
					UnityDispatcher.PostTask(() =>
					{
						callback.OnBannerAdLoad(ad);
					}, callbackOnMainThread);
				}
			}
		}

		private readonly AndroidJavaObject adNative;

		internal AdNative(AndroidJavaObject adNative)
		{
			this.adNative = adNative;
		}

		public void LoadFeedAd(AdSlot adSlot, IFeedAdListener listener, bool callbackOnMainThread = true)
		{
			FeedAdListener feedAdListener = new FeedAdListener(listener, callbackOnMainThread);
			adNative.Call("loadFeedAd", adSlot.Handle, feedAdListener);
		}

		public void LoadDrawFeedAd(AdSlot adSlot, IDrawFeedAdListener listener, bool callbackOnMainThread = true)
		{
			DrawFeedAdListener drawFeedAdListener = new DrawFeedAdListener(listener, callbackOnMainThread);
			adNative.Call("loadDrawFeedAd", adSlot.Handle, drawFeedAdListener);
		}

		public void LoadNativeAd(AdSlot adSlot, INativeAdListener listener, bool callbackOnMainThread = true)
		{
			NativeAdListener nativeAdListener = new NativeAdListener(listener, callbackOnMainThread);
			adNative.Call("loadNativeAd", adSlot.Handle, nativeAdListener);
		}

		public void LoadSplashAd(AdSlot adSlot, ISplashAdListener listener, int timeOut, bool callbackOnMainThread = true)
		{
			SplashAdListener splashAdListener = new SplashAdListener(listener, callbackOnMainThread);
			adNative.Call("loadSplashAd", adSlot.Handle, splashAdListener, timeOut);
		}

		public void LoadSplashAd(AdSlot adSlot, ISplashAdListener listener, bool callbackOnMainThread = true)
		{
			SplashAdListener splashAdListener = new SplashAdListener(listener, callbackOnMainThread);
			adNative.Call("loadSplashAd", adSlot.Handle, splashAdListener);
		}

		public void LoadRewardVideoAd(AdSlot adSlot, IRewardVideoAdListener listener, bool callbackOnMainThread = true)
		{
			RewardVideoAdListener rewardVideoAdListener = new RewardVideoAdListener(listener, callbackOnMainThread);
			adNative.Call("loadRewardVideoAd", adSlot.Handle, rewardVideoAdListener);
		}

		public void LoadFullScreenVideoAd(AdSlot adSlot, IFullScreenVideoAdListener listener, bool callbackOnMainThread = true)
		{
			FullScreenVideoAdListener fullScreenVideoAdListener = new FullScreenVideoAdListener(listener, callbackOnMainThread);
			adNative.Call("loadFullScreenVideoAd", adSlot.Handle, fullScreenVideoAdListener);
		}

		public void LoadNativeExpressAd(AdSlot adSlot, IExpressAdListener listener, bool callbackOnMainThread = true)
		{
			ExpressAdListener expressAdListener = new ExpressAdListener(listener, callbackOnMainThread);
			adNative.Call("loadNativeExpressAd", adSlot.Handle, expressAdListener);
		}

		public void LoadExpressBannerAd(AdSlot adSlot, IExpressBannerAdListener listener, bool callbackOnMainThread = true)
		{
			ExpressBannerAdListener expressBannerAdListener = new ExpressBannerAdListener(listener, callbackOnMainThread);
			adNative.Call("loadBannerExpressAd", adSlot.Handle, expressBannerAdListener);
		}
	}
}
