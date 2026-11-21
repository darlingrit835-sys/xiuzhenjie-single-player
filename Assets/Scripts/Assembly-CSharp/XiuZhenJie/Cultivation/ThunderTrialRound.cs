using System;

namespace XiuZhenJie
{
	internal sealed class ThunderTrialRound
	{
		internal const int MaxWard = 66;
		internal const int MaxQi = 3;
		internal const int Waves = 3;
		private static readonly int[] PowerByWave = { 30, 38, 48 };
		internal static readonly string[] Stages = { "探脉", "叠雷", "破关" };
		internal static readonly string[] Names = { "", "金", "木", "水", "火", "土" };
		private static readonly WuXing[] Overcomes = { WuXing.All, WuXing.Mu, WuXing.Tu, WuXing.Huo, WuXing.Jin, WuXing.Shui };
		private static readonly WuXing[] Generates = { WuXing.All, WuXing.Shui, WuXing.Huo, WuXing.Mu, WuXing.Tu, WuXing.Jin };
		private readonly WuXing[] sky = { WuXing.Jin, WuXing.Mu, WuXing.Shui, WuXing.Huo, WuXing.Tu };
		private readonly int rootMask;

		internal int Wave { get; private set; }
		internal int Ward { get; private set; } = MaxWard;
		internal int Qi { get; private set; } = 2;
		internal bool Ended => Ward == 0 || Wave == Waves;
		internal bool Won => Wave == Waves && Ward > 0;
		internal WuXing Thunder => sky[Math.Min(Wave, Waves - 1)];
		internal WuXing Next => Wave + 1 < Waves ? sky[Wave + 1] : WuXing.All;
		internal int Power => PowerByWave[Math.Min(Wave, Waves - 1)];

		internal ThunderTrialRound(int rootMask, Random random)
		{
			this.rootMask = rootMask;
			for (int i = 0; i < Waves; i++)
			{
				int j = random.Next(i, sky.Length);
				(sky[i], sky[j]) = (sky[j], sky[i]);
			}
		}

		internal bool HasRoot(WuXing seal) => (rootMask & (1 << (int)seal)) != 0;

		internal readonly struct Response
		{
			internal readonly string Name;
			internal readonly int Hit, QiChange;
			internal readonly bool Ready;

			internal Response(string name, int hit, int qiChange, bool ready)
			{
				Name = name;
				Hit = hit;
				QiChange = qiChange;
				Ready = ready;
			}
		}

		internal Response Preview(WuXing seal)
		{
			if (Ended || seal < WuXing.Jin || seal > WuXing.Tu)
				return default;
			string name = "护脉";
			int percent = 80, qiChange = 0;
			if (Overcomes[(int)seal] == Thunder)
			{
				name = "截雷";
				percent = 20;
				qiChange = -1;
			}
			else if (Generates[(int)Thunder] == seal)
			{
				name = "引息";
				percent = 55;
				qiChange = Qi == MaxQi ? 0 : 1;
			}
			else if (Overcomes[(int)Thunder] == seal)
			{
				name = "逆冲";
				percent = 100;
			}
			int hit = (Power * percent + 99) / 100 - (HasRoot(seal) ? 3 : 0);
			return new Response(name, hit, qiChange, Qi + qiChange >= 0);
		}

		internal bool Resolve(WuXing seal)
		{
			Response response = Preview(seal);
			if (!response.Ready)
				return false;
			Ward = Math.Max(0, Ward - response.Hit);
			Qi += response.QiChange;
			Wave++;
			return true;
		}
	}
}
