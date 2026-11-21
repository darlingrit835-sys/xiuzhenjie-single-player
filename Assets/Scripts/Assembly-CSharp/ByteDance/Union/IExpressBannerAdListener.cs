namespace ByteDance.Union
{
	public interface IExpressBannerAdListener
	{
		void OnError(int code, string message);

		void OnBannerAdLoad(ExpressBannerAd ad);
	}
}
