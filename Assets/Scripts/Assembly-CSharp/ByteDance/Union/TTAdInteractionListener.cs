using System.Collections.Generic;
using UnityEngine;

namespace ByteDance.Union
{
	internal sealed class TTAdInteractionListener : AndroidJavaProxy
	{
		private readonly ITTAdInteractionListener listener;

		private bool callbackOnMainThread;

		public TTAdInteractionListener(ITTAdInteractionListener listener, bool callbackOnMainThread)
			: base("com.bytedance.sdk.openadsdk.TTAdInteractionListener")
		{
			this.listener = listener;
			this.callbackOnMainThread = callbackOnMainThread;
		}

		public void onAdEvent(int code, AndroidJavaObject jMap)
		{
			if (listener != null)
			{
				Dictionary<string, object> map = new Dictionary<string, object>();
				if (jMap != null && code == 1)
				{
					string value = jMap.Call<string>("get", new object[1] { "open_uid" });
					map.Add("open_uid", value);
				}
				UnityDispatcher.PostTask(() =>
				{
					listener.OnAdEvent(code, map);
				}, callbackOnMainThread);
			}
		}
	}
}
