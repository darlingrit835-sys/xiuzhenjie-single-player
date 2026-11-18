using System.Collections.Generic;

namespace ByteDance.Union
{
	public interface IExpressAdListener
	{
		void OnError(int code, string message);

		void OnExpressAdLoad(List<ExpressAd> ads);
	}
}
