namespace ByteDance.Union
{
	public interface IFullScreenVideoAdInteractionListener
	{
		void OnAdShow();

		void OnAdVideoBarClick();

		void OnAdClose();

		void OnVideoComplete();

		void OnSkippedVideo();

		void OnVideoError();
	}
}
