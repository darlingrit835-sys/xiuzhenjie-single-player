namespace ByteDance.Union
{
	public interface IClientBidding
	{
		void setAuctionPrice(double price);

		void win(double price);

		void Loss(double price, string reason, string bidder);

		void SetAdInteractionListener(ITTAdInteractionListener listener, bool callbackOnMainThread = true);
	}
}
