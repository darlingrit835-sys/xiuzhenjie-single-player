using UnityEngine;

namespace ByteDance.Union
{
	public sealed class DrawFeedAd : FeedAd
	{
		internal DrawFeedAd(AndroidJavaObject ad)
			: base(ad)
		{
		}

		public void SetCanInterruptVideoPlay(bool support)
		{
			ad.Call("setCanInterruptVideoPlay", support);
		}
	}
}
