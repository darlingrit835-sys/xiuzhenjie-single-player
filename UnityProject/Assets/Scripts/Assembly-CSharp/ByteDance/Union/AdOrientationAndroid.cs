using System;

namespace ByteDance.Union
{
	internal static class AdOrientationAndroid
	{
		public static int ToAndroid(this AdOrientation orientation)
		{
			return orientation switch
			{
				AdOrientation.Vertical => 1, 
				AdOrientation.Horizontal => 2, 
				_ => throw new NotSupportedException($"Unknown AdOrientation:{orientation}"), 
			};
		}
	}
}
