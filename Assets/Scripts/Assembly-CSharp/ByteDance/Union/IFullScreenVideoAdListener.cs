namespace ByteDance.Union
{
	public interface IFullScreenVideoAdListener
	{
		void OnError(int code, string message);

		void OnFullScreenVideoAdLoad(FullScreenVideoAd ad);

		void OnFullScreenVideoCached();

		void OnFullScreenVideoCached(FullScreenVideoAd ad);
	}
}
