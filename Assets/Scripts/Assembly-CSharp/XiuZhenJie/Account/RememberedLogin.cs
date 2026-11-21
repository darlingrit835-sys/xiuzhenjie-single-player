using System;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace XiuZhenJie
{
	[Serializable]
	internal sealed class RememberedLogin
	{
		private const string PreferencePrefix = "xzj.remembered-login.v1|https://";

		public string UserId;
		public string Token;
		public long ExpiresAt;
		public string Account;

		[NonSerialized] private string preferenceKey;

		internal static bool CanUse => Constants.UsesLocalServer ||
			(DirectCloudTrial.Enabled && string.Equals(Constants.LoginServerIP, DirectCloudTrial.Host, StringComparison.OrdinalIgnoreCase)
				&& Constants.LoginServerPort == DirectCloudTrial.Port);

		internal static bool SupportsLegacyPassword =>
			Constants.UsesLocalServer && !Constants.UseEmailRegistration && !DirectCloudTrial.Enabled;

		private static string PreferenceKey => CanUse
			? PreferencePrefix + Constants.LoginServerIP.ToLowerInvariant() + ":" + Constants.LoginServerPort
			: null;

		internal bool MatchesEndpoint => preferenceKey != null && preferenceKey == PreferenceKey;

		private bool IsUsable => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Token) &&
			!string.IsNullOrEmpty(Account) && ExpiresAt > DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		internal static bool TryLoad(out RememberedLogin session)
		{
			session = null;
			string key = PreferenceKey;
			if (key == null)
				return false;

			string saved = ObscuredPrefs.GetString(key);
			if (string.IsNullOrEmpty(saved))
				return false;

			try
			{
				session = JsonUtility.FromJson<RememberedLogin>(saved);
			}
			catch (ArgumentException)
			{
			}
			if (session == null || !session.IsUsable)
			{
				session = null;
				ObscuredPrefs.DeleteKey(key);
				PlayerPrefs.Save();
				return false;
			}

			session.preferenceKey = key;
			return true;
		}

		internal static bool TrySave(LoginModel login, string account)
		{
			string key = PreferenceKey;
			if (key == null)
				return false;

			var session = new RememberedLogin
			{
				UserId = login.userId,
				Token = login.rememberToken,
				ExpiresAt = login.RememberTokenExpiresAt,
				Account = account
			};
			if (!session.IsUsable)
				return false;

			ObscuredPrefs.SetString(key, JsonUtility.ToJson(session));
			ObscuredPrefs.DeleteKey("mima");
			PlayerPrefs.Save();
			return true;
		}

		internal void ForgetIfCurrent()
		{
			if (MatchesEndpoint && TryLoad(out RememberedLogin current) && current.Token == Token)
				Forget();
		}

		internal static void Forget()
		{
			string key = PreferenceKey;
			if (key != null)
				ObscuredPrefs.DeleteKey(key);
			ObscuredPrefs.DeleteKey("mima");
			PlayerPrefs.Save();
		}
	}
}
