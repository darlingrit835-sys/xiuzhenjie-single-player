using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class HttpsRequest
	{
		public static string SendSSLProxyRequest(string url, string proxyIp, string method = "GET", Dictionary<string, string> paramsMap = null, int timeout = 2000)
		{
			if (method == "GET")
			{
				url = BuildURL(url, paramsMap);
			}
			Uri uri = new Uri(proxyIp + url);
			TcpClient tcpClient = null;
			SslStream sslStream = null;
			try
			{
				tcpClient = new TcpClient("192.168.0.22", 10003);
				sslStream = new SslStream(tcpClient.GetStream(), leaveInnerStreamOpen: false, certificateValidationCallback, null);
				sslStream.ReadTimeout = timeout;
				sslStream.WriteTimeout = timeout;
				sslStream.AuthenticateAsClient(uri.Host);
				string format = "{0} {1} HTTP/1.1\r\nHost: {2}\r\n";
				format = string.Format(format, method, uri.AbsoluteUri, uri.Host);
				int num = -1;
				if ("POST".Equals(method) && paramsMap != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, string> item in paramsMap)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("&");
						}
						stringBuilder.Append(item.Key).Append("=").Append(item.Value);
					}
					num = Encoding.UTF8.GetBytes(stringBuilder.ToString()).Length;
					format += $"Content-Type: application/x-www-form-urlencoded; charset=UTF-8\r\nContent-Length: {num}\r\n\r\n{stringBuilder.ToString()}";
				}
				else
				{
					format += "\r\n";
				}
				byte[] bytes = Encoding.UTF8.GetBytes(format);
				sslStream.Write(bytes, 0, bytes.Length);
				sslStream.Flush();
				byte[] array = new byte[2048];
				num = -1;
				int num2 = -1;
				int num3 = -1;
				int num4 = -1;
				StringBuilder stringBuilder2 = new StringBuilder();
				do
				{
					num = sslStream.Read(array, 0, array.Length);
					if (num <= 0)
					{
						break;
					}
					string text = Encoding.UTF8.GetString(array, 0, num);
					stringBuilder2.Append(text);
					if (num2 == -1)
					{
						int num5 = text.IndexOf("\r\n\r\n");
						if (num5 > 0)
						{
							text = stringBuilder2.ToString();
							string[] array2 = text.Substring(0, text.IndexOf("\r\n")).Split(' ');
							num4 = Convert.ToInt32(array2[1]);
							_ = array2[2];
							int num6 = text.IndexOf("Content-Length:");
							int num7 = text.IndexOf("\r\n", num6);
							num2 = Convert.ToInt32(text.Substring(num6 + 16, num7 - num6 - 16));
							text = text.Substring(num5 + 4);
							stringBuilder2.Length = 0;
							stringBuilder2.Append(text);
							num3 = Encoding.UTF8.GetBytes(text).Length;
						}
					}
					else
					{
						num3 += num;
					}
				}
				while (num3 < num2 && num != 0);
				if (num4 == 200)
				{
					return stringBuilder2.ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			finally
			{
				sslStream?.Close();
				tcpClient?.Close();
			}
			return null;
		}

		private static string BuildURL(string url, Dictionary<string, string> paramsMap)
		{
			if (paramsMap == null)
			{
				return url;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> item in paramsMap)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(item.Key).Append("=").Append(UrlEncode(item.Value));
			}
			url = ((url.IndexOf("?") == -1) ? (url + "?" + stringBuilder.ToString()) : (url + "&" + stringBuilder.ToString()));
			return url;
		}

		private static string UrlEncode(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			for (int i = 0; i < bytes.Length; i++)
			{
				stringBuilder.Append("%" + Convert.ToString(bytes[i], 16));
			}
			return stringBuilder.ToString();
		}

		private static bool certificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors != SslPolicyErrors.None)
			{
				return false;
			}
			X509Certificate2 certificate2 = new X509Certificate2(certificate);
			chain.Build(certificate2);
			return true;
		}

		public static UnityWebRequest UnityPostRequest(string uri, int port, string action, WWWForm form)
		{
			UnityWebRequest unityWebRequest = UnityWebRequest.Post($"https://{uri}:{port}{action}", form);
			if (!DirectCloudTrial.TryConfigureAccountRequest(unityWebRequest))
			{
				unityWebRequest.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
			}
			return unityWebRequest;
		}

		public static UnityWebRequest UnityGetRequest(string uri, int port, string action, Dictionary<string, string> argDic)
		{
			string text = $"https://{uri}:{port}{action}";
			int num = 0;
			if (argDic != null)
			{
				foreach (KeyValuePair<string, string> item in argDic)
				{
					text = ((num != 0) ? string.Format(text + "&{0}", item.Key + "=" + item.Value) : string.Format(text + "?{0}", item.Key + "=" + item.Value));
					num++;
				}
			}
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
			if (!DirectCloudTrial.TryConfigureAccountRequest(unityWebRequest))
			{
				unityWebRequest.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
			}
			return unityWebRequest;
		}

		internal static bool TryReadAccountSuccess(string response, out string data, out string message)
		{
			data = null;
			message = "无法读取服务器响应，请重试";
			try
			{
				var body = JsonMapper.ToObject<ShellModel>(response)?.msgBody;
				var result = body?.bizInfo;
				if (body?.sysInfo?.httpCode != 200 || result == null)
				{
					return false;
				}
				if (!string.IsNullOrEmpty(result.msg))
				{
					message = result.msg;
				}
				data = result.dataString;
				return result.code == "2000";
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static void DealwithShellModel(string jsonData, Action<HttpsRequestStatus, ShellModel, string> action)
		{
			ShellModel shellModel = null;
			try
			{
				shellModel = JsonMapper.ToObject<ShellModel>(jsonData);
				if (shellModel == null)
				{
					action?.Invoke(HttpsRequestStatus.ServerError, shellModel, "连接服务器失败");
				}
			}
			catch (Exception ex)
			{
				Debug.Log("反序列化失败:" + ex.Message);
				action?.Invoke(HttpsRequestStatus.ServerError, shellModel, "连接服务器失败" + ex.Message);
			}
			try
			{
				if (shellModel.msgBody == null)
				{
					if (action != null)
					{
						action(HttpsRequestStatus.ServerError, shellModel, "无法获取服务器信息,请重试");
						Debug.Log("sm.msgBody为空");
					}
				}
				else if (shellModel.msgBody.sysInfo == null)
				{
					action?.Invoke(HttpsRequestStatus.ServerError, shellModel, "无法获取服务器信息,请重试");
				}
				else if (shellModel.msgBody.sysInfo.httpCode.Equals(200))
				{
					if (shellModel.msgBody.bizInfo == null)
					{
						action?.Invoke(HttpsRequestStatus.ServerError, shellModel, "无法获取服务器信息,请重试");
					}
					int num = int.Parse(shellModel.msgBody.bizInfo.code);
					if (num >= 2000 && num < 3000)
					{
						action?.Invoke(HttpsRequestStatus.SuccessFul, shellModel, shellModel.msgBody.bizInfo.msg);
					}
					else
					{
						action?.Invoke(HttpsRequestStatus.ServerError, shellModel, shellModel.msgBody.bizInfo.msg);
					}
				}
				else
				{
					action?.Invoke(HttpsRequestStatus.ServerError, shellModel, "连接服务器失败");
				}
			}
			catch (Exception ex2)
			{
				action?.Invoke(HttpsRequestStatus.ServerError, shellModel, "连接服务器失败" + ex2.Message);
			}
		}

		public static void DealwithShellModel(string jsonData, out HttpsRequestStatus status, out ShellModel model, out string action)
		{
			ShellModel shellModel = null;
			try
			{
				shellModel = JsonMapper.ToObject<ShellModel>(jsonData);
				if (shellModel == null)
				{
					status = HttpsRequestStatus.ServerError;
					model = null;
					action = "";
					return;
				}
			}
			catch (Exception ex)
			{
				status = HttpsRequestStatus.ServerError;
				model = null;
				action = ex.Message;
				return;
			}
			try
			{
				if (shellModel.msgBody == null)
				{
					status = HttpsRequestStatus.ServerError;
					model = null;
					action = "";
				}
				else if (shellModel.msgBody.sysInfo == null)
				{
					status = HttpsRequestStatus.ServerError;
					model = null;
					action = "";
				}
				else if (shellModel.msgBody.sysInfo.httpCode.Equals(200))
				{
					if (shellModel.msgBody.bizInfo == null)
					{
						status = HttpsRequestStatus.ServerError;
						model = null;
						action = "";
						return;
					}
					int num = int.Parse(shellModel.msgBody.bizInfo.code);
					if (num >= 2000 && num < 3000)
					{
						status = HttpsRequestStatus.SuccessFul;
						model = shellModel;
						action = shellModel.msgBody.bizInfo.msg;
					}
					else
					{
						status = HttpsRequestStatus.ServerError;
						model = null;
						action = "";
					}
				}
				else
				{
					status = HttpsRequestStatus.ServerError;
					model = null;
					action = "";
				}
			}
			catch (Exception)
			{
				status = HttpsRequestStatus.ServerError;
				model = null;
				action = "";
			}
		}
	}
}
