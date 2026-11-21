using ByteDance.Union.Mediation;

namespace ByteDance.Union
{
	public class PrivacyConfiguration
	{
		public bool CanUseWifiState { get; set; } = true;

		public bool CanUseLocation { get; set; } = true;

		public bool CanReadAppList { get; set; } = true;

		public bool CanUsePhoneState { get; set; } = true;

		public bool CanUseWriteExternal { get; set; } = true;

		public string MacAddress { get; set; }

		public double Latitude { get; set; } = double.NaN;

		public double Longitude { get; set; } = double.NaN;

		public string DevImei { get; set; }

		public string DevOaid { get; set; }

		public bool CanUseAndroidId { get; set; } = true;

		public string AndroidId { get; set; }

		public MediationPrivacyConfig MediationPrivacyConfig { get; set; }

		public string CustomIdfa { get; set; }
	}
}
