namespace ByteDance.Union
{
	public interface IRewardAdInteractionListener
	{
		void OnAdShow();

		void OnAdVideoBarClick();

		void OnAdClose();

		void OnVideoComplete();

		void OnVideoSkip();

		void OnVideoError();

		void OnRewardArrived(bool isRewardValid, int rewardType, IRewardBundleModel extraInfo);
	}
}
