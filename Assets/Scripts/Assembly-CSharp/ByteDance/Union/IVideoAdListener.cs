namespace ByteDance.Union
{
	public interface IVideoAdListener
	{
		void OnVideoLoad(FeedAd feedAd);

		void OnVideoError(int var1, int var2);

		void OnVideoAdStartPlay(FeedAd feedAd);

		void OnVideoAdPaused(FeedAd feedAd);

		void OnVideoAdContinuePlay(FeedAd feedAd);

		void OnProgressUpdate(long current, long duration);

		void OnVideoAdComplete(FeedAd feedAd);
	}
}
