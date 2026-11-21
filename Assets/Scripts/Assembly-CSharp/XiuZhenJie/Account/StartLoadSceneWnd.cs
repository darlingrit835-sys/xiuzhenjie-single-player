using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XiuZhenJie.Client.Release;

namespace XiuZhenJie
{
	public class StartLoadSceneWnd : Window
	{
		public StartLoadScenePanel m_panel;

		private StartupLoadingSurface surface;

		private GameObject clock;

		private bool clockWasActive;

		private bool m_Failed;

		private float m_SumTime;

		private IContentBootstrap m_PackagedContent;

		private bool m_StartupQueued;

		private bool m_StartingGame;

		private bool m_Closed;

		private bool m_CheckingServerStatus;

		private const string PrepareLocalText = "正在准备本地资源…";

		private const string ServerMaintenanceText = "当前服务器维护中";

		private static float LocalPreparationStart => ReleaseRuntime.IsActive ? .90f : 0f;

		internal static float GameInitializationStart => ReleaseRuntime.IsActive ? .93f : 0f;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_panel = base.GameObject.GetComponent<StartLoadScenePanel>();
			var game = MonoSingleton<GameStart>.Instance;
			m_PackagedContent = game.PackagedContent;
			if (game.TimeText != null)
			{
				clock = game.TimeText.gameObject;
				clockWasActive = clock.activeSelf;
				clock.SetActive(false);
			}
			// Older release bundles can still contain the original painted background and progress bar.
			m_panel.GetComponent<Image>().enabled = false;
			for (int i = 0; i < m_panel.transform.childCount; i++)
				m_panel.transform.GetChild(i).gameObject.SetActive(false);
			surface = StartupLoadingSurface.Create(m_panel.transform, m_panel.ProgressText.font);
			surface.Retry.gameObject.SetActive(false);
			m_panel.ProgressText = surface.Status;
			m_panel.ProgressValue = surface.Progress;
			m_panel.ProgressValue.fillAmount = LocalPreparationStart;
			SetStatus(PrepareLocalText);
			HotPatchManager hotPatch = Singleton<HotPatchManager>.Instance;
			hotPatch.ServerInfoError += ServerInfoError;
			hotPatch.ItemError += ItemError;
			hotPatch.VersionNoSize += VersionNoSize;
		}

		private void SetStatus(string text, bool failed = false)
		{
			m_Failed = failed;
			surface.SetFailure(failed);
			m_panel.ProgressText.text = text;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (m_StartupQueued)
				return;

			m_StartupQueued = true;
			MonoSingleton<GameStart>.Instance.StartCoroutine(PrepareAfterFirstFrame());
		}

		private IEnumerator PrepareAfterFirstFrame()
		{
			// Give the parented loading window a render frame before preparing any resources.
			yield return null;
			yield return ClientHotUpdate.Initialize(MonoSingleton<GameStart>.Instance);
			PrepareLocalContent();
		}

		private void PrepareLocalContent()
		{
			if (m_Closed)
				return;

			m_panel.ProgressValue.fillAmount = LocalPreparationStart;
			SetStatus(PrepareLocalText);
			m_PackagedContent.Prepare(HotFix, PreparationFailed);
		}

		private void PreparationFailed(string details)
		{
			if (m_Closed)
				return;

			Debug.LogWarning("本地资源准备失败：" + details);
			SetStatus("本地资源准备失败", failed: true);
			MonoSingleton<GameStart>.Instance.PopUpAttention(details, PrepareLocalContent);
		}

		private void VersionNoSize()
		{
			if (m_Closed)
				return;
			SetStatus("客户端版本不匹配", failed: true);
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
			if (m_Closed)
				return;

			if (Constants.UsesPackagedTrialData)
			{
				StartOnFinish();
				return;
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				SetStatus("暂无网络连接", failed: true);
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前未连接网络。恢复连接后点击确定重试。", HotFix);
			}
			else
			{
				CheckServerVersion();
			}
		}

		private IEnumerator GetServerStatusIEnumerator()
		{
			var parameters = new Dictionary<string, string> { ["osType"] = "1" };
			using var request = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/gameService/statusSearch", parameters);
			request.timeout = 10;
			yield return request.SendWebRequest();
			m_CheckingServerStatus = false;
			if (m_Closed)
				yield break;

			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			if (request.result != UnityWebRequest.Result.Success || string.IsNullOrWhiteSpace(request.downloadHandler.text))
			{
				ServerInfoError();
				yield break;
			}

			HttpsRequest.DealwithShellModel(request.downloadHandler.text, out HttpsRequestStatus status, out ShellModel response, out string _);
			if (status != HttpsRequestStatus.SuccessFul)
			{
				ServerInfoError();
				yield break;
			}

			switch (response.msgBody.bizInfo.dataString)
			{
				case "1":
					CheckServerVersion();
					break;
				case "0":
				case "-1":
					SetStatus(ServerMaintenanceText, failed: true);
					MonoSingleton<GameStart>.Instance.PopUpAttention(ServerMaintenanceText, GetServerStatus);
					break;
				default:
					ServerInfoError();
					break;
			}
		}

		private void GetServerStatus()
		{
			if (m_Closed || m_CheckingServerStatus)
				return;

			m_CheckingServerStatus = true;
			SetStatus("正在检查服务器状态…");
			MonoSingleton<GameStart>.Instance.StartCoroutine(GetServerStatusIEnumerator());
		}

		private void CheckServerVersion()
		{
			SetStatus("正在检查资源版本…");
			Singleton<HotPatchManager>.Instance.CheckVersion((bool hot) =>
			{
				if (m_Closed)
					return;

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
			if (m_Closed)
				return;

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
			if (m_Closed)
				return;

			m_SumTime = 0f;
			SetStatus("正在下载资源…");
			MonoSingleton<GameStart>.Instance.StartCoroutine(Singleton<HotPatchManager>.Instance.StartDownLoadAB(StartOnFinish));
		}

		private void StartOnFinish()
		{
			if (m_Closed || m_StartingGame)
				return;

			m_StartingGame = true;
			SetStatus("正在初始化游戏…");
			MonoSingleton<GameStart>.Instance.StartCoroutine(MonoSingleton<GameStart>.Instance.StartGame(m_panel.ProgressValue, m_panel.ProgressText));
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_Failed)
				return;
			if (m_PackagedContent.IsPreparing)
			{
				float progress = m_PackagedContent.PreparationProgress;
				if (!float.IsNaN(progress) && !float.IsInfinity(progress))
					m_panel.ProgressValue.fillAmount = ReleaseRuntime.IsActive
						? Mathf.Lerp(LocalPreparationStart, GameInitializationStart, progress)
						: Mathf.Clamp01(progress);
				return;
			}
			if (Singleton<HotPatchManager>.Instance.StartDownload)
			{
				m_SumTime += Time.unscaledDeltaTime;
				float progress = Singleton<HotPatchManager>.Instance.GetProgress();
				if (!float.IsNaN(progress) && !float.IsInfinity(progress))
					m_panel.ProgressValue.fillAmount = Mathf.Clamp01(progress);
				float megabytesPerSecond = Singleton<HotPatchManager>.Instance.GetLoadSize() / 1024f / Mathf.Max(0.001f, m_SumTime);
				m_panel.ProgressText.text = $"正在下载资源  {megabytesPerSecond:F1} MB/s";
			}
		}

		public override void OnClose()
		{
			m_Closed = true;
			if (clock != null)
				clock.SetActive(clockWasActive);
			HotPatchManager hotPatch = Singleton<HotPatchManager>.Instance;
			hotPatch.ServerInfoError -= ServerInfoError;
			hotPatch.ItemError -= ItemError;
			hotPatch.VersionNoSize -= VersionNoSize;
			base.OnClose();
		}

		private void ServerInfoError()
		{
			if (m_Closed)
				return;

			SetStatus("无法连接服务器", failed: true);
			MonoSingleton<GameStart>.Instance.PopUpAttention("无法获取服务器状态，请检查网络后点击确定重试。", GetServerStatus);
		}

		private void ItemError(string all)
		{
			if (m_Closed)
				return;
			SetStatus("部分资源下载失败", failed: true);
			MonoSingleton<GameStart>.Instance.PopUpAttention($"{all}等资源下载失败，请重新尝试下载！", AnewDownload);
		}

		private void AnewDownload()
		{
			if (m_Closed)
				return;

			SetStatus("正在检查资源版本…");
			Singleton<HotPatchManager>.Instance.CheckVersion((bool hot) =>
			{
				if (m_Closed)
					return;

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
