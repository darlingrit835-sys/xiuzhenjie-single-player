using ByteDance.Union.Mediation;

namespace ByteDance.Union
{
	public sealed class SDKConfiguration
	{
		public class Builder
		{
			private SDKConfiguration configuration = new SDKConfiguration();

			public Builder SetAppId(string appId)
			{
				configuration.appId = appId;
				return this;
			}

			public Builder SetAppName(string appName)
			{
				configuration.appName = appName;
				return this;
			}

			public Builder SetUseMediation(bool useMediation)
			{
				configuration.useMediation = useMediation;
				return this;
			}

			public Builder SetUseTextureView(bool useTextureView)
			{
				configuration.useTextureView = useTextureView;
				return this;
			}

			public Builder SetAllowShowNotify(bool allowShowNotify)
			{
				configuration.allowShowNotify = allowShowNotify;
				return this;
			}

			public Builder SetDebug(bool debug)
			{
				configuration.debug = debug;
				return this;
			}

			public Builder SetThemeStatus(int themeStatus)
			{
				configuration.themeStatus = themeStatus;
				return this;
			}

			public Builder SetAgeGroup(int ageGroup)
			{
				configuration.ageGroup = ageGroup;
				return this;
			}

			public Builder SetSupportMultiProcesse(bool supportMultiProcess)
			{
				configuration.supportMultiProcess = supportMultiProcess;
				return this;
			}

			public Builder SetPrivacyConfigurationn(PrivacyConfiguration configuration)
			{
				this.configuration.privacyConfiguration = configuration;
				return this;
			}

			public Builder SetMediationConfig(MediationConfig mediationConfig)
			{
				configuration.mediationConfig = mediationConfig;
				return this;
			}

			public Builder SetDirectDownloadNetworkType(int[] directDownloadNetworkType)
			{
				configuration.directDownloadNetworkType = directDownloadNetworkType;
				return this;
			}

			public Builder SetData(string data)
			{
				configuration.data = data;
				return this;
			}

			public SDKConfiguration Build()
			{
				return configuration;
			}
		}

		internal string appId;

		internal string appName;

		internal bool useMediation;

		internal bool useTextureView;

		internal bool allowShowNotify;

		internal bool debug;

		internal int themeStatus;

		internal bool supportMultiProcess;

		internal int[] directDownloadNetworkType;

		internal string data;

		internal PrivacyConfiguration privacyConfiguration;

		internal MediationConfig mediationConfig;

		internal int ageGroup;
	}
}
