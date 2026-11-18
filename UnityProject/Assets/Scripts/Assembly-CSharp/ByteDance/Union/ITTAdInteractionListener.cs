using System.Collections.Generic;

namespace ByteDance.Union
{
	public interface ITTAdInteractionListener
	{
		void OnAdEvent(int code, Dictionary<string, object> map);
	}
}
