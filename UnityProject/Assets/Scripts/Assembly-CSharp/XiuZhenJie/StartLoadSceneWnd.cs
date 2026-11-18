using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class StartLoadSceneWnd : Window
	{
		public StartLoadScenePanel m_panel;

		private float m_SumTime;

		private string m_ServerMaintenanceText = "当前服务器维护中~!";

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_panel = base.GameObject.GetComponent<StartLoadScenePanel>();
			LoadingCrane.AddTo(m_panel.transform, m_panel.MaskImg, m_panel.transform.Find("Bg")?.GetComponent<UnityEngine.UI.Image>());
			m_SumTime = 0f;
			m_panel.ProgressValue.fillAmount = 0f;
			m_panel.ProgressText.text = $"{0:F}M/S";
			HotPatchManager instance = Singleton<HotPatchManager>.Instance;
			instance.ServerInfoError = (Action)Delegate.Remove(instance.ServerInfoError, new Action(ServerInfoError));
			HotPatchManager instance2 = Singleton<HotPatchManager>.Instance;
			instance2.ServerInfoError = (Action)Delegate.Combine(instance2.ServerInfoError, new Action(ServerInfoError));
			HotPatchManager instance3 = Singleton<HotPatchManager>.Instance;
			instance3.ItemError = (Action<string>)Delegate.Remove(instance3.ItemError, new Action<string>(ItemError));
			HotPatchManager instance4 = Singleton<HotPatchManager>.Instance;
			instance4.ItemError = (Action<string>)Delegate.Combine(instance4.ItemError, new Action<string>(ItemError));
			HotPatchManager instance5 = Singleton<HotPatchManager>.Instance;
			instance5.VersionNoSize = (UnityAction)Delegate.Remove(instance5.VersionNoSize, new UnityAction(VersionNoSize));
			HotPatchManager instance6 = Singleton<HotPatchManager>.Instance;
			instance6.VersionNoSize = (UnityAction)Delegate.Combine(instance6.VersionNoSize, new UnityAction(VersionNoSize));
			if (Singleton<HotPatchManager>.Instance.ComputeUnPackFile())
			{
				m_panel.ProgressText.text = "解压中(此过程不消耗流量)...";
				Singleton<HotPatchManager>.Instance.StartUnackFile(() =>
				{
					m_SumTime = 0f;
					HotFix();
				});
			}
			else
			{
				m_panel.ProgressText.text = "正在对比版本...";
				HotFix();
			}
		}

		private void VersionNoSize()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("当前客户端版本不匹配,请重新下载客户端", () =>
			{
				OnClickCancleDownLoad();
			}, () =>
			{
				OnClickCancleDownLoad();
			});
		}

		private void HotFix()
		{
			if (Constants.UsesLocalServer)
			{
				StartOnFinish();
				return;
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("网络链接失败，请检查网络链接是否正常？", () =>
				{
					MonoSingleton<GameRoot>.Instance.ExitApplication();
				});
			}
			else
			{
				CheckServerVersion();
			}
		}

		private IEnumerator GetServerStatusIEnumerator()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("osType", "1");
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/gameService/statusSearch", dictionary);
			www.timeout = 10;
			yield return www.SendWebRequest();
			while (!www.isDone)
			{
			}
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			string text = www.downloadHandler.text;
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				switch (status)
				{
				case HttpsRequestStatus.ServerError:
					MonoSingleton<GameStart>.Instance.PopUpAttention(content);
					break;
				case HttpsRequestStatus.SuccessFul:
					switch (sm.msgBody.bizInfo.dataString)
					{
					case "1":
						CheckServerVersion();
						break;
					case "0":
						MonoSingleton<GameStart>.Instance.PopUpAttention(m_ServerMaintenanceText, HotFix, HotFix);
						break;
					case "-1":
						MonoSingleton<GameStart>.Instance.PopUpAttention(m_ServerMaintenanceText, HotFix, HotFix);
						break;
					}
					break;
				}
			});
		}

		private void GetServerStatus()
		{
			MonoSingleton<GameStart>.Instance.StartCoroutine(GetServerStatusIEnumerator());
		}

		private void CheckServerVersion()
		{
			Singleton<HotPatchManager>.Instance.CheckVersion((bool hot) =>
			{
				if (hot)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention($"有{Singleton<HotPatchManager>.Instance.LoadSumSize / 1024f:F}M文件，是否确定下载？", OnClickStartDownLoad, OnClickCancleDownLoad);
				}
				else
				{
					StartOnFinish();
				}
			});
		}

		private void OnClickStartDownLoad()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("当前使用的是手机流量，是否继续下载？", StartDownLoad, OnClickCancleDownLoad);
				}
				else
				{
					StartDownLoad();
				}
			}
			else
			{
				StartDownLoad();
			}
		}

		private void OnClickCancleDownLoad()
		{
			MonoSingleton<GameRoot>.Instance.ExitApplication();
		}

		private void StartDownLoad()
		{
			m_panel.ProgressText.text = "下载中...";
			MonoSingleton<GameStart>.Instance.StartCoroutine(Singleton<HotPatchManager>.Instance.StartDownLoadAB(StartOnFinish));
		}

		private void StartOnFinish()
		{
			MonoSingleton<GameStart>.Instance.StartCoroutine(MonoSingleton<GameStart>.Instance.StartGame(m_panel.ProgressValue, m_panel.ProgressText));
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (Singleton<HotPatchManager>.Instance.StartUnPack)
			{
				m_SumTime += Time.deltaTime;
				m_panel.ProgressValue.fillAmount = Singleton<HotPatchManager>.Instance.GetUnpackProgress();
				float num = Singleton<HotPatchManager>.Instance.AlreadyUnPackSize / 1024f / m_SumTime;
				m_panel.ProgressText.text = string.Format("{0} : {1:F}  M/S", "解压中(此过程不消耗流量)", num);
			}
			if (Singleton<HotPatchManager>.Instance.StartDownload)
			{
				m_SumTime += Time.deltaTime;
				float progress = Singleton<HotPatchManager>.Instance.GetProgress();
				m_panel.ProgressValue.fillAmount = progress;
				float num2 = Singleton<HotPatchManager>.Instance.GetLoadSize() / 1024f / m_SumTime;
				m_panel.ProgressText.text = string.Format("{0} : {1:F}  M/S", "下载中", num2);
			}
		}

		public override void OnClose()
		{
			HotPatchManager instance = Singleton<HotPatchManager>.Instance;
			instance.ServerInfoError = (Action)Delegate.Remove(instance.ServerInfoError, new Action(ServerInfoError));
			HotPatchManager instance2 = Singleton<HotPatchManager>.Instance;
			instance2.ItemError = (Action<string>)Delegate.Remove(instance2.ItemError, new Action<string>(ItemError));
		}

		private void ServerInfoError()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("服务器列表获取失败，请检查网络链接是否正常？尝试重新下载！", GetServerStatus);
		}

		private void ItemError(string all)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention($"{all}等资源下载失败，请重新尝试下载！", AnewDownload);
		}

		private void AnewDownload()
		{
			Singleton<HotPatchManager>.Instance.CheckVersion((bool hot) =>
			{
				if (hot)
				{
					StartDownLoad();
				}
				else
				{
					StartOnFinish();
				}
			});
		}
	}
}
