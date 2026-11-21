using UnityEngine;

namespace ByteDance.Union
{
	public sealed class AdDislike
	{
		private readonly AndroidJavaObject ad;

		internal AdDislike(AndroidJavaObject ad)
		{
			this.ad = ad;
		}

		public void ShowDislikeDialog()
		{
			ad.Call("showDislikeDialog");
		}

		public void SetDislikeInteractionCallback(IDislikeInteractionListener listener, bool callbackOnMainThread)
		{
			DislikeInteractionCallback dislikeInteractionCallback = new DislikeInteractionCallback(listener, callbackOnMainThread);
			ad.Call("setDislikeInteractionCallback", dislikeInteractionCallback);
		}
	}
}
