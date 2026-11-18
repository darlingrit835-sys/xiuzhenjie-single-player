namespace ByteDance.Union
{
	public interface IRewardVideoAdListener
	{
		void OnError(int code, string message);

		void OnRewardVideoAdLoad(RewardVideoAd ad);

		void OnRewardVideoCached();

		void OnRewardVideoCached(RewardVideoAd ad);
	}
}
