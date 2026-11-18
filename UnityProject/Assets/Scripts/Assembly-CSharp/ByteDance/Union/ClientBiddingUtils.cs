using UnityEngine;

namespace ByteDance.Union
{
	public static class ClientBiddingUtils
	{
		public static void Win(AndroidJavaObject adObject, double auctionBidToWin)
		{
			if (!double.IsNaN(auctionBidToWin))
			{
				adObject?.Call("win", ToDoubleObject(auctionBidToWin));
			}
		}

		public static void Loss(AndroidJavaObject adObject, double auctionPrice, string lossReason, string winBidder)
		{
			adObject?.Call("loss", ToDoubleObject(auctionPrice), lossReason, winBidder);
		}

		public static void SetPrice(AndroidJavaObject adObject, double auctionPrice)
		{
			adObject?.Call("setPrice", ToDoubleObject(auctionPrice));
		}

		private static AndroidJavaObject ToDoubleObject(double value)
		{
			if (double.IsNaN(value))
			{
				return null;
			}
			return new AndroidJavaClass("com.bytedance.android.ClientBiddingBridge").CallStatic<AndroidJavaObject>("toDouble", new object[1] { value });
		}
	}
}
