using System;

namespace CodeStage.AntiCheat.Utils
{
	public class ThreadSafeRandom
	{
		private static readonly Random Global = new Random();

		[ThreadStatic]
		private static Random local;

		public static int Next(int minInclusive, int maxExclusive)
		{
			Random random = local;
			if (random != null)
			{
				return random.Next(minInclusive, maxExclusive);
			}
			int seed;
			lock (Global)
			{
				seed = Global.Next();
			}
			return (local = new Random(seed)).Next(minInclusive, maxExclusive);
		}

		public static int Next()
		{
			return Next(1, int.MaxValue);
		}

		public static int Next(int maxExclusive)
		{
			return Next(1, maxExclusive);
		}
	}
}
