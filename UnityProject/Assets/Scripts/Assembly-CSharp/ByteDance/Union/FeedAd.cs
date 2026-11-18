using UnityEngine;

namespace ByteDance.Union
{
	public class FeedAd : NativeAd
	{
		private sealed class VideoAdListener : AndroidJavaProxy
		{
			private readonly FeedAd ad;

			private readonly IVideoAdListener listener;

			private bool callbackOnMainThread;

			public VideoAdListener(FeedAd ad, IVideoAdListener listener, bool callbackOnMainThread)
				: base("com.bytedance.sdk.openadsdk.TTFeedAd$VideoAdListener")
			{
				this.ad = ad;
				this.listener = listener;
				this.callbackOnMainThread = callbackOnMainThread;
			}

			public void onVideoLoad(AndroidJavaObject ad)
			{
				FeedAd feedAd = ((ad == this.ad.Handle) ? this.ad : new FeedAd(ad));
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoLoad(feedAd);
				}, callbackOnMainThread);
			}

			public void onVideoError(int var1, int var2)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoError(var1, var2);
				}, callbackOnMainThread);
			}

			public void onVideoAdStartPlay(AndroidJavaObject ad)
			{
				FeedAd feedAd = ((ad == this.ad.Handle) ? this.ad : new FeedAd(ad));
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoAdStartPlay(feedAd);
				}, callbackOnMainThread);
			}

			public void onVideoAdPaused(AndroidJavaObject ad)
			{
				FeedAd feedAd = ((ad == this.ad.Handle) ? this.ad : new FeedAd(ad));
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoAdPaused(feedAd);
				}, callbackOnMainThread);
			}

			public void onVideoAdContinuePlay(AndroidJavaObject ad)
			{
				FeedAd feedAd = ((ad == this.ad.Handle) ? this.ad : new FeedAd(ad));
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoAdContinuePlay(feedAd);
				}, callbackOnMainThread);
			}

			public void onProgressUpdate(long current, long duration)
			{
				UnityDispatcher.PostTask(() =>
				{
					listener.OnProgressUpdate(current, duration);
				}, callbackOnMainThread);
			}

			public void onVideoAdComplete(AndroidJavaObject ad)
			{
				FeedAd feedAd = ((ad == this.ad.Handle) ? this.ad : new FeedAd(ad));
				UnityDispatcher.PostTask(() =>
				{
					listener.OnVideoAdComplete(feedAd);
				}, callbackOnMainThread);
			}
		}

		private IFeedAdInteractionListener listener;

		private IDislikeInteractionListener dislike;

		internal AndroidJavaObject Handle;

		internal FeedAd(AndroidJavaObject ad)
			: base(ad)
		{
			Handle = ad;
		}

		public new void Dispose()
		{
			NativeAdManager.Instance().DestroyFeedAd(Handle);
		}

		public void ShowFeedAd(float x, float y)
		{
			NativeAdManager.Instance().ShowFeedAd(Utils.GetActivity(), Handle, listener, dislike, callbackOnMainThread: true, (int)x, (int)y);
		}

		public void SetFeedAdInteractionListener(IFeedAdInteractionListener listener)
		{
			this.listener = listener;
		}

		public void SetFeedAdDislikeListener(IDislikeInteractionListener dislike)
		{
			this.dislike = dislike;
		}

		public void SetVideoAdListener(IVideoAdListener listener, bool callbackOnMainThread = true)
		{
			VideoAdListener videoAdListener = new VideoAdListener(this, listener, callbackOnMainThread);
			Handle.Call("setVideoAdListener", videoAdListener);
		}
	}
}
