namespace ByteDance.Union
{
	public interface IExpressAdInteractionListener
	{
		void OnAdViewRenderSucc(ExpressAd ad, float width, float height);

		void OnAdViewRenderError(ExpressAd ad, int code, string message);

		void OnAdShow(ExpressAd ad);

		void OnAdClicked(ExpressAd ad);

		void OnAdClose(ExpressAd ad);

		void onAdRemoved(ExpressAd ad);
	}
}
