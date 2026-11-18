using UnityEngine;

namespace ByteDance.Union
{
	public sealed class DislikeInteractionCallback : AndroidJavaProxy
	{
		private readonly IDislikeInteractionListener listener;

		private bool callbackOnMainThread;

		public DislikeInteractionCallback(IDislikeInteractionListener listener, bool callbackOnMainThread)
			: base("com.bytedance.sdk.openadsdk.TTAdDislike$DislikeInteractionCallback")
		{
			this.listener = listener;
			this.callbackOnMainThread = callbackOnMainThread;
		}

		public void onSelected(int var1, string var2, bool enforce)
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnSelected(var1, var2, enforce);
			}, callbackOnMainThread);
		}

		public void onCancel()
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnCancel();
			}, callbackOnMainThread);
		}

		public void onShow()
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnShow();
			}, callbackOnMainThread);
		}
	}
}
