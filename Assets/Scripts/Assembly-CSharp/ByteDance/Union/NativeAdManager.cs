using System;
using UnityEngine;

namespace ByteDance.Union
{
	public class NativeAdManager
	{
		private sealed class FeedInteractionListener : AndroidJavaProxy
		{
			private IFeedAdInteractionListener mInteractionListener;

			public FeedInteractionListener(IFeedAdInteractionListener listener)
				: base("com.bytedance.sdk.openadsdk.TTNativeAd$AdInteractionListener")
			{
				mInteractionListener = listener;
			}

			public void onAdClicked(AndroidJavaObject view, AndroidJavaObject feedAd)
			{
				if (mInteractionListener != null)
				{
					mInteractionListener.OnAdClicked();
				}
			}

			public void onAdCreativeClick(AndroidJavaObject view, AndroidJavaObject feedAd)
			{
				if (mInteractionListener != null)
				{
					mInteractionListener.OnAdCreativeClick();
				}
			}

			public void onAdShow(AndroidJavaObject feedAd)
			{
				if (mInteractionListener != null)
				{
					mInteractionListener.OnAdShow();
				}
			}
		}

		private sealed class ExpressAdInteractionCallback : AndroidJavaProxy
		{
			private IExpressAdInteractionListener listener;

			private readonly bool callbackOnMainThread;

			public ExpressAdInteractionCallback(IExpressAdInteractionListener callback, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTNativeExpressAd$AdInteractionListener")
			{
				listener = callback;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onAdDismiss()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClose(null);
				}, callbackOnMainThread);
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
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdViewRenderSucc(null, width, height);
				}, callbackOnMainThread);
			}
		}

		private sealed class ExpressBannerAdInteractionCallback : AndroidJavaProxy
		{
			private IExpressBannerInteractionListener listener;

			private readonly bool callbackOnMainThread;

			public ExpressBannerAdInteractionCallback(IExpressBannerInteractionListener callback, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTNativeExpressAd$AdInteractionListener")
			{
				listener = callback;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onAdDismiss()
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClose();
				}, callbackOnMainThread);
			}

			public void onAdClicked(AndroidJavaObject view, int type)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdClicked();
				}, callbackOnMainThread);
			}

			public void onAdShow(AndroidJavaObject view, int type)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdShow();
				}, callbackOnMainThread);
			}

			public void onRenderFail(AndroidJavaObject view, string msg, int code)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdViewRenderError(code, msg);
				}, callbackOnMainThread);
			}

			public void onRenderSuccess(AndroidJavaObject view, float width, float height)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdViewRenderSucc(width, height);
				}, callbackOnMainThread);
			}
		}

		private sealed class DisLikeCallback : AndroidJavaProxy
		{
			private IDislikeInteractionListener dislikeInteractionCallback;

			private readonly bool callbackOnMainThread;

			public DisLikeCallback(IDislikeInteractionListener dislike, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTAdDislike$DislikeInteractionCallback")
			{
				dislikeInteractionCallback = dislike;
			}

			private void onSelected(int position, string value, bool enforce)
			{
				Debug.Log("CSJM_Unity DisLikeCallback -->onSelected position -" + position + " value---" + value);
				Debug.Log("CSJM_Unity " + $"position -{position} value---{value} dislike onSelected");
				UnityDispatcher.PostTask(() =>
				{
					dislikeInteractionCallback.OnSelected(position, value, enforce);
				}, callbackOnMainThread);
			}

			private void onCancel()
			{
				UnityDispatcher.PostTask(() =>
				{
					dislikeInteractionCallback.OnCancel();
				}, callbackOnMainThread);
			}

			public void onShow()
			{
				UnityDispatcher.PostTask(() =>
				{
					dislikeInteractionCallback.OnShow();
				}, callbackOnMainThread);
			}
		}

		public class NativeAdInteractionListener : AndroidJavaProxy
		{
			private IInteractionAdInteractionListener listener;

			private bool callbackOnMainThread;

			public NativeAdInteractionListener(IInteractionAdInteractionListener listener, bool callbackOnMainThread = true)
				: base("com.bytedance.sdk.openadsdk.TTNativeAd$AdInteractionListener")
			{
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onAdClicked(AndroidJavaObject view, AndroidJavaObject ad)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener?.OnAdClicked();
				}, callbackOnMainThread);
			}

			public void onAdCreativeClick(AndroidJavaObject view, AndroidJavaObject ad)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener?.OnAdCreativeClick();
				}, callbackOnMainThread);
			}

			public void onAdShow(AndroidJavaObject ad)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener?.OnAdShow();
				}, callbackOnMainThread);
			}
		}

		protected readonly AndroidJavaObject nativeAdManager;

		private static NativeAdManager sNativeAdManager = new NativeAdManager();

		private NativeAdManager()
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.bytedance.android.NativeAdManager").CallStatic<AndroidJavaObject>("getNativeAdManager", Array.Empty<object>());
			nativeAdManager = androidJavaObject;
		}

		public static NativeAdManager Instance()
		{
			return sNativeAdManager;
		}

		public void ShowExpressFeedAd(AndroidJavaObject activity, AndroidJavaObject expressAd, IExpressAdInteractionListener listener, IDislikeInteractionListener dislikeInteractionListener, bool callbackOnMainThread, int x, int y)
		{
			object[] objs = new object[6]
			{
				activity,
				expressAd,
				new ExpressAdInteractionCallback(listener, callbackOnMainThread),
				new DisLikeCallback(dislikeInteractionListener, callbackOnMainThread),
				x,
				y
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeExpressAd;Lcom.bytedance.sdk.openadsdk.TTNativeExpressAd$AdInteractionListener;Lcom.bytedance.sdk.openadsdk.TTAdDislike$DislikeInteractionCallback;II)V";
			CallJavaMethod("showExpressFeedAd", signature, objs);
		}

		private void CallJavaMethod(string methodName, string signature, object[] objs)
		{
			IntPtr methodID = AndroidJNIHelper.GetMethodID(nativeAdManager.GetRawClass(), methodName, signature);
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(objs);
			try
			{
				AndroidJNI.CallVoidMethod(nativeAdManager.GetRawObject(), methodID, array);
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(objs, array);
			}
		}

		public void ShowFeedAd(AndroidJavaObject activity, AndroidJavaObject feedAd, IFeedAdInteractionListener listener, IDislikeInteractionListener dislike, bool callbackOnMainThread, int x, int y)
		{
			object[] objs = new object[6]
			{
				activity,
				feedAd,
				new FeedInteractionListener(listener),
				new DisLikeCallback(dislike, callbackOnMainThread),
				x,
				y
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeAd;Lcom.bytedance.sdk.openadsdk.TTNativeAd$AdInteractionListener;Lcom.bytedance.sdk.openadsdk.TTAdDislike$DislikeInteractionCallback;II)V";
			CallJavaMethod("showFeedAd", signature, objs);
		}

		public void ShowExpressBannerAd(AndroidJavaObject activity, AndroidJavaObject expressAd, IExpressBannerInteractionListener listener, IDislikeInteractionListener dislikeInteractionListener, bool callbackOnMainThread, int x, int y)
		{
			object[] objs = new object[6]
			{
				activity,
				expressAd,
				new ExpressBannerAdInteractionCallback(listener, callbackOnMainThread),
				new DisLikeCallback(dislikeInteractionListener, callbackOnMainThread),
				x,
				y
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeExpressAd;Lcom.bytedance.sdk.openadsdk.TTNativeExpressAd$AdInteractionListener;Lcom.bytedance.sdk.openadsdk.TTAdDislike$DislikeInteractionCallback;II)V";
			CallJavaMethod("showExpressBannerAd", signature, objs);
		}

		public void DestroyExpressFeedAd(AndroidJavaObject expressFeedAd)
		{
			object[] objs = new object[2]
			{
				Utils.GetActivity(),
				expressFeedAd
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeExpressAd;)V";
			CallJavaMethod("destroyExpressFeedAd", signature, objs);
		}

		public void DestroyExpressBannerAd(AndroidJavaObject expressBannerAd)
		{
			object[] objs = new object[2]
			{
				Utils.GetActivity(),
				expressBannerAd
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeExpressAd;)V";
			CallJavaMethod("destroyExpressBannerAd", signature, objs);
		}

		public void DestroyFeedAd(AndroidJavaObject feedAd)
		{
			object[] objs = new object[2]
			{
				Utils.GetActivity(),
				feedAd
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeAd;)V";
			CallJavaMethod("destroyFeedAd", signature, objs);
		}

		public void DestroyNativeAd(AndroidJavaObject nativeAd)
		{
			object[] objs = new object[2]
			{
				Utils.GetActivity(),
				nativeAd
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeAd;)V";
			CallJavaMethod("destroyBannerAd", signature, objs);
		}

		public void ShowNativeBannerAd(AndroidJavaObject activity, AndroidJavaObject nativeAd, IInteractionAdInteractionListener listener, IDislikeInteractionListener dislikeInteractionListener, int x, int y)
		{
			object[] objs = new object[6]
			{
				activity,
				nativeAd,
				new NativeAdInteractionListener(listener),
				new DisLikeCallback(dislikeInteractionListener, callbackOnMainThread: true),
				x,
				y
			};
			string signature = "(Landroid.content.Context;Lcom.bytedance.sdk.openadsdk.TTNativeAd;Lcom.bytedance.sdk.openadsdk.TTNativeAd$AdInteractionListener;Lcom.bytedance.sdk.openadsdk.TTAdDislike$DislikeInteractionCallback;II)V";
			CallJavaMethod("showNativeBannerAd", signature, objs);
		}
	}
}
