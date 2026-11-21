using System;
using System.Text;
using System.Globalization;

namespace XiuZhenJie
{
	// The world channel also serves older clients. Each card is a readable, bounded chat message.
	internal sealed class TavernCard
	{
		internal const string Item = "道具分享";
		internal const string Bounty = "悬赏分享";
		internal const string Wanted = "求购";
		internal readonly string Kind;
		internal readonly string Name;
		internal readonly string Details;

		internal TavernCard(string kind, string name, string details)
		{
			Kind = kind;
			Name = name;
			Details = details;
		}

		internal string Caption => $"【{Kind}】{Name}";
		internal string Summary => Kind == Wanted ? Details.Replace("\n", "　") : Details.Split('\n')[0];
		internal string Message => Caption + "\n" + Details;
		internal static bool Fits(string value) => Encoding.UTF8.GetByteCount(value) <= 512;

		internal static TavernCard FromMessage(string message)
		{
			if (string.IsNullOrEmpty(message) || message[0] != '【' || !Fits(message)) return null;
			int end = message.IndexOf('】');
			int newline = message.IndexOf('\n');
			if (end < 2 || newline <= end + 1 || newline >= message.Length - 1) return null;
			string kind = message.Substring(1, end - 1);
			string details = message.Substring(newline + 1);
			string firstField;
			switch (kind)
			{
				case Item: firstField = "品阶："; break;
				case Bounty: firstField = "报酬："; break;
				case Wanted:
					var lines = details.Split('\n');
					if (lines.Length != 2 || !PositiveField(lines[0], "数量：", "") || !PositiveField(lines[1], "单价：", " 灵石"))
						return null;
					firstField = "数量：";
					break;
				default: return null;
			}
			if (!details.StartsWith(firstField, StringComparison.Ordinal)) return null;
			return new TavernCard(kind, message.Substring(end + 1, newline - end - 1), details);
		}

		private static bool PositiveField(string line, string prefix, string suffix)
		{
			int length = line.Length - prefix.Length - suffix.Length;
			return length > 0 && line.StartsWith(prefix, StringComparison.Ordinal) && line.EndsWith(suffix, StringComparison.Ordinal)
				&& int.TryParse(line.Substring(prefix.Length, length), NumberStyles.None, CultureInfo.InvariantCulture, out int n) && n > 0;
		}

		internal static TavernCard Goods(SpaceGoodsResp_1004002.Types.GoodsInfoResp_1004003 item)
		{
			string rank = WorldSetting.GetRankAllName(item.Rank);
			string element = WorldSetting.GetWuXingName((WuXing)item.FiveElement);
			return new TavernCard(Item, item.Name, $"品阶：{rank}\n等级：{item.Level}　五行：{element}\n持有：{Math.Max(1, item.Count)} 件");
		}
	}
}
