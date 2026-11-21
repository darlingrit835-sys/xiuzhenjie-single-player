using System.Collections.Generic;

namespace XiuZhenJie
{
	public sealed class BattleFieldLedger
	{
		private readonly HashSet<string> received = new HashSet<string>();
		private readonly SortedDictionary<string, long> totals = new SortedDictionary<string, long>(System.StringComparer.Ordinal);

		public IReadOnlyDictionary<string, long> Totals => totals;

		public int Revision => received.Count;

		public void Record(string pickupId, string itemName, long count)
		{
			if (string.IsNullOrEmpty(pickupId) || string.IsNullOrEmpty(itemName) || count <= 0 || !received.Add(pickupId))
				return;
			totals.TryGetValue(itemName, out long previous);
			totals[itemName] = previous + count;
		}
	}
}
