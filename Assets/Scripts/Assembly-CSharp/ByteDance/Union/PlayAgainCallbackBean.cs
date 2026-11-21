namespace ByteDance.Union
{
	public class PlayAgainCallbackBean
	{
		public const string KEY_PLAY_AGAIN_ALLOW = "play_again_allow";

		public const string KEY_PLAY_AGAIN_REWARD_NAME = "play_again_reward_name";

		public const string KEY_PLAY_AGAIN_REWARD_AMOUNT = "play_again_reward_amount";

		public bool IsAgain { get; set; }

		public string RewardName { get; set; }

		public string RewardCount { get; set; }

		public PlayAgainCallbackBean(bool isAgain, string rewardName, string rewardCount)
		{
			IsAgain = isAgain;
			RewardName = rewardName;
			RewardCount = rewardCount;
		}
	}
}
