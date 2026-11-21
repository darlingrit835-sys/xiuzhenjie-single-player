namespace ByteDance.Union
{
	public interface IRewardBundleModel
	{
		int ServerErrorCode { get; }

		string ServerErrorMsg { get; }

		string RewardName { get; }

		float RewardAmount { get; }

		float RewardPropose { get; }

		bool GMIsServerSideVerify { get; }

		string GMExtra { get; }

		string GMAdnName { get; }

		string GMTransId { get; }

		int GMReason { get; }

		int GMErrorCode { get; }

		string GMErrorMsg { get; }

		string GMEcpm { get; }
	}
}
