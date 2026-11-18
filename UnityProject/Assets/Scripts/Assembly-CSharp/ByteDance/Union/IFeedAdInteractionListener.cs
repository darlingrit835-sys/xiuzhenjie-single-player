namespace ByteDance.Union
{
	public interface IFeedAdInteractionListener
	{
		void OnAdClicked();

		void OnAdCreativeClick();

		void OnAdShow();
	}
}
