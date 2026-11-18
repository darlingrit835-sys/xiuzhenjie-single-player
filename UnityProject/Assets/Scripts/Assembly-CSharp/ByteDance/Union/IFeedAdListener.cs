using System.Collections.Generic;

namespace ByteDance.Union
{
	public interface IFeedAdListener
	{
		void OnError(int code, string message);

		void OnFeedAdLoad(IList<FeedAd> ads);
	}
}
