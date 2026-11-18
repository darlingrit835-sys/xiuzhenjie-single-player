using UnityEngine;

namespace ByteDance.Union
{
	internal sealed class AppDownloadListener : AndroidJavaProxy
	{
		private readonly IAppDownloadListener listener;

		private bool callbackOnMainThread;

		public AppDownloadListener(IAppDownloadListener listener, bool callbackOnMainThread)
			: base("com.bytedance.sdk.openadsdk.TTAppDownloadListener")
		{
			this.listener = listener;
			this.callbackOnMainThread = callbackOnMainThread;
		}

		public void onIdle()
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnIdle();
			}, callbackOnMainThread);
		}

		public void onDownloadActive(long totalBytes, long currBytes, string fileName, string appName)
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnDownloadActive(totalBytes, currBytes, fileName, appName);
			}, callbackOnMainThread);
		}

		public void onDownloadPaused(long totalBytes, long currBytes, string fileName, string appName)
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnDownloadPaused(totalBytes, currBytes, fileName, appName);
			}, callbackOnMainThread);
		}

		public void onDownloadFailed(long totalBytes, long currBytes, string fileName, string appName)
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnDownloadFailed(totalBytes, currBytes, fileName, appName);
			}, callbackOnMainThread);
		}

		public void onDownloadFinished(long totalBytes, string fileName, string appName)
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnDownloadFinished(totalBytes, fileName, appName);
			}, callbackOnMainThread);
		}

		public void onInstalled(string fileName, string appName)
		{
			UnityDispatcher.PostTask(() =>
			{
				listener.OnInstalled(fileName, appName);
			}, callbackOnMainThread);
		}
	}
}
