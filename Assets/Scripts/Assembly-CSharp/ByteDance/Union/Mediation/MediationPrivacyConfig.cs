using System.Collections.Generic;

namespace ByteDance.Union.Mediation
{
	public class MediationPrivacyConfig
	{
		public List<string> CustomAppList { get; set; }

		public List<string> CustomDevImeis { get; set; }

		public bool CanUseOaid { get; set; } = true;

		public bool LimitPersonalAds { get; set; }

		public bool ProgrammaticRecommend { get; set; } = true;

		public bool forbiddenCAID { get; set; }
	}
}
