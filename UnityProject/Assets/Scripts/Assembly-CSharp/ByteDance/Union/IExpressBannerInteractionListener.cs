namespace ByteDance.Union
{
	public interface IExpressBannerInteractionListener
	{
		void OnAdViewRenderSucc(float width, float height);

		void OnAdViewRenderError(int code, string message);

		void OnAdShow();

		void OnAdClicked();

		void OnAdClose();

		void onAdRemoved();
	}
}
