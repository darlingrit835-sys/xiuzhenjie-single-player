using System.Collections.Generic;

namespace ByteDance.Union.Mediation
{
	public class MediationPreloadRequestInfo
	{
		public int AdType { get; set; }

		public AdSlot AdSlot { get; set; }

		public List<string> PrimeRitList { get; set; }
	}
}
