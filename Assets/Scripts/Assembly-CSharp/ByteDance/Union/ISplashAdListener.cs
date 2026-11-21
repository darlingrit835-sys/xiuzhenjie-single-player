namespace ByteDance.Union
{
	public interface ISplashAdListener
	{
		void OnSplashLoadFail(int code, string message);

		void OnSplashLoadSuccess(BUSplashAd ad);

		void OnSplashRenderSuccess(BUSplashAd ad);

		void OnSplashRenderFail(int code, string message);
	}
}
