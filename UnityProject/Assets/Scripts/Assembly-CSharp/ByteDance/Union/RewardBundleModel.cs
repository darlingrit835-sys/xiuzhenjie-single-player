using System;
using UnityEngine;

namespace ByteDance.Union
{
	public class RewardBundleModel : IRewardBundleModel
	{
		private static string REWARD_EXTRA_KEY_ERROR_CODE = "reward_extra_key_error_code";

		private static string REWARD_EXTRA_KEY_ERROR_MSG = "reward_extra_key_error_msg";

		private static string REWARD_EXTRA_KEY_REWARD_NAME = "reward_extra_key_reward_name";

		private static string REWARD_EXTRA_KEY_REWARD_AMOUNT = "reward_extra_key_reward_amount";

		private static string REWARD_EXTRA_KEY_REWARD_PROPOSE = "reward_extra_key_reward_propose";

		private const string GM_IS_SERVER_SIDE_VERIFY = "isGroMoreServerSideVerify";

		private const string GM_EXTRA = "gromoreExtra";

		private const string GM_TRANS_ID = "transId";

		private const string GM_REASON = "reason";

		private const string GM_ERROR_CODE = "errorCode";

		private const string GM_ERROR_MSG = "errorMsg";

		private const string GM_ADN_NAME = "adnName";

		private const string GM_ECPM = "ecpm";

		public int ServerErrorCode { get; }

		public string ServerErrorMsg { get; }

		public string RewardName { get; }

		public float RewardAmount { get; }

		public float RewardPropose { get; }

		public bool GMIsServerSideVerify { get; }

		public string GMExtra { get; }

		public string GMAdnName { get; }

		public string GMTransId { get; }

		public int GMReason { get; }

		public int GMErrorCode { get; }

		public string GMErrorMsg { get; }

		public string GMEcpm { get; }

		private RewardBundleModel(int serverErrorCode, string serverErrorMsg, string rewardName, float rewardAmount, float rewardPropose, bool isGromoreServersideVerify, string gromoreExtra, string transId, int reason, int errCode, string errMsg, string adnName, string ecpm)
		{
			ServerErrorCode = serverErrorCode;
			ServerErrorMsg = serverErrorMsg;
			RewardName = rewardName;
			RewardAmount = rewardAmount;
			RewardPropose = rewardPropose;
			GMIsServerSideVerify = isGromoreServersideVerify;
			GMExtra = gromoreExtra;
			GMTransId = transId;
			GMReason = reason;
			GMErrorCode = errCode;
			GMErrorMsg = errMsg;
			GMAdnName = adnName;
			GMEcpm = ecpm;
		}

		public static RewardBundleModel Create(AndroidJavaObject extraInfo)
		{
			if (extraInfo == null)
			{
				return null;
			}
			try
			{
				int serverErrorCode = extraInfo.Call<int>("getInt", new object[1] { REWARD_EXTRA_KEY_ERROR_CODE });
				Debug.Log("CSJM_Unity serverErrorCode : " + serverErrorCode);
				string text = extraInfo.Call<string>("getString", new object[1] { REWARD_EXTRA_KEY_ERROR_MSG });
				Debug.Log("CSJM_Unity serverErrorMsg : " + text);
				string text2 = extraInfo.Call<string>("getString", new object[1] { REWARD_EXTRA_KEY_REWARD_NAME });
				Debug.Log("CSJM_Unity rewardName : " + text2);
				float rewardAmount = extraInfo.Call<float>("getFloat", new object[1] { REWARD_EXTRA_KEY_REWARD_AMOUNT });
				Debug.Log("CSJM_Unity rewardAmount : " + rewardAmount);
				float rewardPropose = extraInfo.Call<float>("getFloat", new object[1] { REWARD_EXTRA_KEY_REWARD_PROPOSE });
				Debug.Log("CSJM_Unity rewardPropose : " + rewardPropose);
				bool isGromoreServersideVerify = extraInfo.Call<bool>("getBoolean", new object[1] { "isGroMoreServerSideVerify" });
				Debug.Log("CSJM_Unity isGromoreVerify : " + isGromoreServersideVerify);
				string text3 = extraInfo.Call<string>("getString", new object[1] { "gromoreExtra" });
				Debug.Log("CSJM_Unity gromoreExtra : " + text3);
				string text4 = extraInfo.Call<string>("getString", new object[1] { "transId" });
				Debug.Log("CSJM_Unity transId : " + text4);
				int reason = extraInfo.Call<int>("getInt", new object[1] { "reason" });
				Debug.Log("CSJM_Unity reason : " + reason);
				int errCode = extraInfo.Call<int>("getInt", new object[1] { "errorCode" });
				Debug.Log("CSJM_Unity errorCode : " + errCode);
				string text5 = extraInfo.Call<string>("getString", new object[1] { "errorMsg" });
				Debug.Log("CSJM_Unity errorMsg : " + text5);
				string text6 = extraInfo.Call<string>("getString", new object[1] { "adnName" });
				Debug.Log("CSJM_Unity adnName : " + text6);
				string text7 = extraInfo.Call<string>("getString", new object[1] { "ecpm" });
				Debug.Log("CSJM_Unity ecpm : " + text7);
				return new RewardBundleModel(serverErrorCode, text, text2, rewardAmount, rewardPropose, isGromoreServersideVerify, text3, text4, reason, errCode, text5, text6, text7);
			}
			catch (Exception ex)
			{
				Debug.Log("CSJM_Unity RewardBundleModel.Create error: " + ex.ToString());
				return null;
			}
		}

		public override string ToString()
		{
			return $"ServerErrorCode : {ServerErrorCode}, ServerErrorMsg:{ServerErrorMsg}, RewardName:{RewardName}," + $" RewardAmount:{RewardAmount}, RewardPropose:{RewardPropose}," + $" isGromoreServersideVerify:{GMIsServerSideVerify}, GMExtra: {GMExtra}, GMAdnName: {GMAdnName}," + $" GMTransId:{GMTransId}, GMReason:{GMReason}, GMErrCode:{GMErrorCode}, GMErrMsg:{GMErrorMsg}," + " ecpm:" + GMEcpm;
		}
	}
}
