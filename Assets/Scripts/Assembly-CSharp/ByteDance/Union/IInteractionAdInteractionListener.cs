namespace ByteDance.Union
{
	public interface IInteractionAdInteractionListener
	{
		void OnAdClicked();

		void OnAdCreativeClick();

		void OnAdShow();

		void OnAdDismiss();

		void onAdRemoved();
	}
}
