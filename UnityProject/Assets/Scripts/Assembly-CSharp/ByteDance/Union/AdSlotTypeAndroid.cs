using System;

namespace ByteDance.Union
{
	internal static class AdSlotTypeAndroid
	{
		public static int ToAndroid(this AdSlotType type)
		{
			return type switch
			{
				AdSlotType.Banner => 1, 
				AdSlotType.InteractionAd => 2, 
				AdSlotType.Splash => 3, 
				AdSlotType.CachedSplash => 4, 
				AdSlotType.Feed => 5, 
				AdSlotType.RewardVideo => 7, 
				AdSlotType.FullScreenVideo => 8, 
				AdSlotType.DrawFeed => 9, 
				_ => throw new NotSupportedException($"Unknown AdSlotType: {type}"), 
			};
		}
	}
}
