using System;

namespace ByteDance.Union
{
	public interface IRewardAdPlayAgainController
	{
		void GetPlayAgainCondition(int nextPlayAgainCount, Action<PlayAgainCallbackBean> callback);
	}
}
