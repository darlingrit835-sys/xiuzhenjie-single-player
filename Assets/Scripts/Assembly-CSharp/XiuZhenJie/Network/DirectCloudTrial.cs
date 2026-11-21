using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;
using XiuZhenJie.Client.Release;

namespace XiuZhenJie
{
	internal static class DirectCloudTrial
	{
		internal static bool Enabled { get; private set; }
		internal static string Host { get; private set; } = "";
		internal static int Port { get; private set; }
		private static string CertificateSha256 { get; set; } = "";
		internal static string Capability { get; private set; } = "";
		internal const string CapabilityHeader = "X-XZJ-Trial";

		internal static void ApplyReleaseConnection()
		{
			ReleaseChannel config = ReleaseRuntime.Channel;
			Uri endpoint = ReleaseRuntime.Endpoint;
			if (endpoint == null || string.IsNullOrEmpty(config?.gatewayCapability))
				return;

			Host = Constants.LoginServerIP = endpoint.Host;
			Port = Constants.LoginServerPort = endpoint.Port;
			CertificateSha256 = config.certificateSha256;
			Capability = config.gatewayCapability;
			Constants.UseEmailRegistration = config.useEmailRegistration;
			Enabled = true;
		}

		internal static bool TryConfigureAccountRequest(UnityWebRequest request)
		{
			if (!IsDirectOrigin(request.url, out _))
			{
				return false;
			}
			request.redirectLimit = 0;
			request.timeout = 12;
			request.certificateHandler = new PinnedCertificateHandler();
			request.SetRequestHeader(CapabilityHeader, Capability);
			return true;
		}

		internal static void ConfigureHotUpdateRequest(UnityWebRequest request)
		{
			if (IsDirectOrigin(request.url, out Uri uri) &&
				uri.AbsolutePath.StartsWith("/_xzj/hot/android/", StringComparison.Ordinal))
			{
				request.certificateHandler = new PinnedCertificateHandler();
			}
		}

		private static bool IsDirectOrigin(string url, out Uri uri)
		{
			return Uri.TryCreate(url, UriKind.Absolute, out uri) && Enabled &&
				uri.Scheme == Uri.UriSchemeHttps && uri.Host == Host && uri.Port == Port;
		}

		internal static bool MatchesCertificate(byte[] der)
		{
			if (der == null || der.Length == 0 || CertificateSha256.Length != 64)
			{
				return false;
			}
			using (var sha = SHA256.Create())
			{
				string digest = BitConverter.ToString(sha.ComputeHash(der)).Replace("-", "");
				return string.Equals(digest, CertificateSha256, StringComparison.OrdinalIgnoreCase);
			}
		}

		private sealed class PinnedCertificateHandler : CertificateHandler
		{
			protected override bool ValidateCertificate(byte[] certificateData)
			{
				try
				{
					using (var certificate = new X509Certificate2(certificateData))
					{
						return MatchesCertificate(certificate.RawData);
					}
				}
				catch (CryptographicException)
				{
					return false;
				}
			}
		}
	}
}
