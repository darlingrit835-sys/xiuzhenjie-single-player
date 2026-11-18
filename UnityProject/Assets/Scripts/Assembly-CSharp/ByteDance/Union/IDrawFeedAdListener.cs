using System.Collections.Generic;

namespace ByteDance.Union
{
	public interface IDrawFeedAdListener
	{
		void OnError(int code, string message);

		void OnDrawFeedAdLoad(IList<DrawFeedAd> ads);
	}
}
