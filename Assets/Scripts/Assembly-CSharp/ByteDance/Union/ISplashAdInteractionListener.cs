namespace ByteDance.Union
{
	public interface ISplashAdInteractionListener
	{
		void OnAdClicked(int type);

		void OnAdDidShow(int type);

		void OnAdWillShow(int type);

		void OnAdClose(int type);
	}
}
