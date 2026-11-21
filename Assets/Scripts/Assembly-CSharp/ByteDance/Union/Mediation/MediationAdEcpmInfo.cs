using System.Collections.Generic;

namespace ByteDance.Union.Mediation
{
	public sealed class MediationAdEcpmInfo
	{
		public Dictionary<string, string> customData { get; set; }

		public string sdkName { get; set; }

		public string customSdkName { get; set; }

		public string slotId { get; set; }

		public string levelTag { get; set; }

		public string ecpm { get; set; }

		public int reqBiddingType { get; set; }

		public string errorMsg { get; set; }

		public string requestId { get; set; }

		public string ritType { get; set; }

		public string segmentId { get; set; }

		public string channel { get; set; }

		public string subChannel { get; set; }

		public string abTestId { get; set; }

		public string scenarioId { get; set; }

		public override string ToString()
		{
			string text = "";
			if (customData != null)
			{
				foreach (KeyValuePair<string, string> customDatum in customData)
				{
					text = text + customDatum.Key + ": " + customDatum.Value + " ";
				}
			}
			return "MediationAdEcpmInfo: sdkName:" + sdkName + ", customSdkName:" + customSdkName + ", slotId:" + slotId + ", levelTag:" + levelTag + ", ecpm:" + ecpm + ", reqBiddingType:" + reqBiddingType + ", errorMsg:" + errorMsg + ", requestId:" + requestId + ", ritType:" + ritType + ", segmentId:" + segmentId + ", channel:" + channel + ", subChannel:" + subChannel + ", abTestId:" + abTestId + ", scenarioId:" + scenarioId + ", customData:" + text;
		}
	}
}
