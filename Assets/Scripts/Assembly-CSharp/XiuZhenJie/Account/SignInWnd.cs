using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Google.Protobuf;
using LitJson;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SignInWnd : Window
	{
		private const int AnnouncementTimeoutSeconds = 5;

		public SignInPanel _panel;

		private RoleVO currentRoleVo;

		private SignInStatus signInStatus;

		private Timer m_ServerConnectionTimer;

		private string m_ServerName = "";

		public override void Awake(params object[] paralist)
		{
			_panel = base.GameObject.GetComponent<SignInPanel>();
			StyleSignIn();
			AddButtonClickListener(_panel.SignInBtn, SignInBtnClick);
			AddButtonClickListener(_panel.SignUpBtn, SignUpBtnClick);
			AddButtonClickListener(_panel.GongGaoBtn, GongGaoBtnClick);
			ConfigureSignInFields();
			_panel.UserNameFiled.onSubmit.AddListener(_ => _panel.PassWordFiled.ActivateInputField());
			_panel.PassWordFiled.onSubmit.AddListener(_ => SignInBtnClick());
			if (!Constants.UseEmailRegistration)
				AddButtonClickListener(_panel.ForgetPasBtn, ForgetPasWndShow);
			AddButtonClickListener(_panel.TongYiBtn, TongYiGongShi);
			AddButtonClickListener(_panel.JuJueBtn, JuJueGongShi);
			Transform agreement = _panel.GongShiPanel;
			Transform agreementCard = agreement?.Find("LiXianBg");
			InkDialogArt.StyleAgreement(agreement, agreementCard, agreementCard?.Find("TitleBg/Text")?.GetComponent<Text>(),
				agreementCard?.Find("ContentTextBg/Text")?.GetComponent<Text>(), _panel.TongYiBtn, _panel.JuJueBtn);
			_panel.InputRect.gameObject.SetActive(value: true);
			string account = ObscuredPrefs.GetString("yonghuming");
			bool emailMode = Constants.UseEmailRegistration;
			_panel.UserNameFiled.text = !emailMode || IsEmailAddress(account) ? account : string.Empty;
			_panel.PassWordFiled.text = string.Empty;
			if (RememberedLogin.TryLoad(out RememberedLogin session) && (!emailMode || IsEmailAddress(session.Account)))
			{
				_panel.UserNameFiled.text = session.Account;
				Singleton<UIManager>.Instance.m_Mono.StartCoroutine(ResumeAfterFirstFrame(session));
			}
			else if (RememberedLogin.SupportsLegacyPassword && !string.IsNullOrEmpty(account) && !IsEmailIdentifier(account))
			{
				string password = ObscuredPrefs.GetString("mima");
				if (!string.IsNullOrEmpty(password))
				{
					_panel.PassWordFiled.text = password;
					Singleton<UIManager>.Instance.m_Mono.StartCoroutine(SignInAfterFirstFrame(account, password));
				}
			}
		}

		private IEnumerator ResumeAfterFirstFrame(RememberedLogin session)
		{
			yield return null;
			if (!session.MatchesEndpoint || GameObject == null || !GameObject.activeInHierarchy ||
				signInStatus == SignInStatus.SignInIng || m_ServerConnectionTimer != null)
				yield break;

			signInStatus = SignInStatus.SignInIng;
			var form = new WWWForm();
			form.AddField("userId", session.UserId);
			form.AddField("rememberToken", session.Token);
			using UnityWebRequest request = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/session/resume", form);
			request.timeout = Constants.UsesLocalServer ? 6 : 12;
			request.redirectLimit = 0;

			UIManager ui = Singleton<UIManager>.Instance;
			var progress = ui.PopUpWnd("NetMessageWnd.prefab", true, "正在恢复登录") as NetMessageWnd;
			bool cancelled = false;
			progress?.AddCloseCallBack(() =>
			{
				cancelled = true;
				request.Abort();
			});
			yield return request.SendWebRequest();
			signInStatus = SignInStatus.None;
			progress?.AddCloseCallBack(null);
			if (progress?.GameObject != null)
				ui.CloseWnd(progress, destory: false);

			if (cancelled || !session.MatchesEndpoint || GameObject == null || !GameObject.activeInHierarchy)
				yield break;

			GameStart game = MonoSingleton<GameStart>.Instance;
			ShellModel response = request.result == UnityWebRequest.Result.Success
				? ReadLoginResponse<ShellModel>(request.downloadHandler.text)
				: null;
			BizInfo result = response?.msgBody?.sysInfo?.httpCode == 200 ? response.msgBody.bizInfo : null;
			if (result?.code == "2105")
			{
				session.ForgetIfCurrent();
				game.PopUpAttention("登录状态已失效，请重新输入密码。");
				yield break;
			}

			LoginModel login = result?.code == "2000" ? ReadLoginResponse<LoginModel>(result.dataString) : null;
			if (login?.userId != session.UserId)
			{
				game.PopUpAttention("暂时无法恢复登录，请检查网络后点击登录重试。");
				yield break;
			}

			if (LoginSuccess(login))
			{
				ObscuredPrefs.SetString("yonghuming", session.Account);
				if (!RememberedLogin.TrySave(login, session.Account))
					PlayerPrefs.Save();
				IsShowGongGao();
			}
		}

		private IEnumerator SignInAfterFirstFrame(string account, string password)
		{
			yield return null;
			if (GameObject != null && GameObject.activeInHierarchy)
				yield return webrequest(account, password, isShowGongGao: true);
		}

		private void ConfigureSignInFields()
		{
			bool emailMode = Constants.UseEmailRegistration;
			_panel.InputRect.Find("UserNameText").GetComponent<Text>().text = emailMode ? "邮箱" : "账号";
			_panel.InputRect.Find("PasswordText").GetComponent<Text>().text = "密码";
			((Text)_panel.UserNameFiled.placeholder).text = emailMode ? "请输入注册邮箱" : "请输入账号";
			((Text)_panel.PassWordFiled.placeholder).text = "请输入密码";
			if (emailMode)
			{
				_panel.UserNameFiled.contentType = InputField.ContentType.Standard;
				_panel.UserNameFiled.keyboardType = TouchScreenKeyboardType.EmailAddress;
				_panel.SignUpBtn.GetComponentInChildren<Text>().text = "邮箱注册";
				_panel.ForgetPasBtn.gameObject.SetActive(false);
			}
		}

		public void PrefillRegisteredEmail(string email)
		{
			_panel.UserNameFiled.text = email;
			_panel.PassWordFiled.text = string.Empty;
		}

		private static bool IsEmailIdentifier(string account) => account != null && account.IndexOf('@') >= 0;

		private static bool IsEmailAddress(string account)
		{
			int separator = account.IndexOf('@');
			return separator > 0 && separator < account.Length - 1 && separator == account.LastIndexOf('@');
		}

		private void StyleSignIn()
		{
			_panel.transform.Find("Text").GetComponent<Text>().text = "当前版本 " + Application.version;
			Image background = _panel.transform.Find("ShanShuiBg")?.GetComponent<Image>();
			InkInterfaceArt.Set(background, "SignInLandscape", false);
			if (background != null && background.sprite != null && background.sprite.name == "SignInLandscape")
			{
				var cover = background.gameObject.AddComponent<AspectRatioFitter>();
				cover.aspectRatio = background.sprite.rect.width / background.sprite.rect.height;
				cover.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;

				var wash = new GameObject("LandscapeWash", typeof(RectTransform), typeof(Image));
				wash.layer = background.gameObject.layer;
				var image = wash.GetComponent<Image>();
				image.rectTransform.SetParent(background.rectTransform, false);
				InkDialogArt.Inset(image.rectTransform, 0f, 0f, 0f, 0f);
				image.color = new Color32(255, 253, 246, 92);
				image.raycastTarget = false;
			}

			RectTransform form = _panel.InputRect;
			form.anchorMin = new Vector2(0.06f, form.anchorMin.y);
			form.anchorMax = new Vector2(0.94f, form.anchorMax.y);
			form.sizeDelta = new Vector2(0f, form.sizeDelta.y);
			form.anchoredPosition = new Vector2(0f, form.anchoredPosition.y);
			InkDialogArt.SetPaper(form.GetComponent<Image>(), "Parchment");
			foreach (Text label in _panel.InputRect.GetComponentsInChildren<Text>(true))
			{
				label.color = InkInterfaceArt.Ink;
			}
			StyleInput(_panel.UserNameFiled);
			StyleInput(_panel.PassWordFiled);
			InkDialogArt.StyleButton(_panel.SignInBtn, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(_panel.SignUpBtn, "ParchmentHeading", InkInterfaceArt.Ink);
			PlaceSignInActions();
			InkDialogArt.StyleButton(_panel.GongGaoBtn, "ParchmentHeading", InkInterfaceArt.Ink);
			Text forgot = _panel.ForgetPasBtn.GetComponentInChildren<Text>(true);
			if (forgot != null)
			{
				forgot.color = InkInterfaceArt.Seal;
			}
			Text version = _panel.transform.Find("Text")?.GetComponent<Text>();
			if (version != null)
			{
				version.color = InkDialogArt.Ivory;
				Outline outline = version.gameObject.AddComponent<Outline>();
				outline.effectColor = new Color32(32, 59, 53, 220);
				outline.effectDistance = new Vector2(1f, -1f);
			}
			Image titleRule = _panel.transform.Find("TitleLine")?.GetComponent<Image>();
			if (titleRule != null)
			{
				titleRule.color = new Color32(108, 91, 61, 155);
			}
		}

		private void PlaceSignInActions()
		{
			var primary = (RectTransform)_panel.SignInBtn.transform;
			primary.anchorMin = Vector2.zero;
			primary.anchorMax = Vector2.right;
			primary.sizeDelta = new Vector2(-48f, 44f);
			primary.anchoredPosition = new Vector2(0f, 65f);
			var secondary = (RectTransform)_panel.SignUpBtn.transform;
			secondary.anchorMin = Vector2.zero;
			secondary.anchorMax = Vector2.right;
			secondary.sizeDelta = new Vector2(-48f, 30f);
			secondary.anchoredPosition = new Vector2(0f, 22f);
			Text label = _panel.SignInBtn.GetComponentInChildren<Text>(true);
			if (label != null)
			{
				label.rectTransform.anchorMin = Vector2.zero;
				label.rectTransform.anchorMax = Vector2.one;
				label.rectTransform.offsetMin = new Vector2(10f, 0f);
				label.rectTransform.offsetMax = new Vector2(-10f, 0f);
				label.alignment = TextAnchor.MiddleCenter;
			}
		}

		private static void StyleInput(InputField field)
		{
			field.textComponent.color = InkInterfaceArt.Ink;
			field.placeholder.color = InkInterfaceArt.SoftInk;
			Image rule = field.transform.Find("Image")?.GetComponent<Image>();
			if (rule != null)
			{
				rule.color = new Color32(108, 91, 61, 120);
			}
		}

		private void ForgetPasWndShow()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ForgetPasWnd.prefab", true);
		}

		private void SignUpBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("SignUpWnd.prefab", true);
		}

		public void SignInBtnClick()
		{
			string account = Constants.UseEmailRegistration ? _panel.UserNameFiled.text.Trim() : _panel.UserNameFiled.text;
			if (string.IsNullOrEmpty(account))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow(Constants.UseEmailRegistration ? "请输入注册邮箱" : "请输入账号");
			}
			else if (Constants.UseEmailRegistration && !IsEmailAddress(account))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入有效的邮箱");
			}
			else if (string.IsNullOrEmpty(_panel.PassWordFiled.text))
			{
				if (RememberedLogin.TryLoad(out RememberedLogin session) &&
					string.Equals(account, session.Account, StringComparison.OrdinalIgnoreCase))
					Singleton<UIManager>.Instance.m_Mono.StartCoroutine(ResumeAfterFirstFrame(session));
				else
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入密码");
			}
			else
			{
				Singleton<UIManager>.Instance.m_Mono.StartCoroutine(webrequest(account, _panel.PassWordFiled.text, isShowGongGao: true, showSeal: true));
			}
		}

		public void SignIn(string result)
		{
			if (!string.IsNullOrEmpty(result))
			{
				string[] array = result.Split(';');
				if (array.Length >= 3)
				{
					Singleton<UIManager>.Instance.m_Mono.StartCoroutine(webrequest(array[0].ToString(), array[1].ToString(), array[2].ToString()));
				}
			}
		}

		private void IsShowGongGao()
		{
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(IsGongGaoShow());
		}

		private void GongGaoBtnClick()
		{
			Singleton<UIManager>.Instance.m_Mono.StartCoroutine(GongGaoShow());
		}

		private IEnumerator GongGaoShow(bool requested = true)
		{
			return MonoSingleton<GameStart>.Instance.OpenAnnouncement(
				() => GameObject != null && GameObject.activeInHierarchy && (requested || CanShowAutomaticAnnouncement()),
				requested);
		}

		private IEnumerator IsGongGaoShow()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("version", "1.3.5");
			using UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/announcement/isShow", dictionary);
			www.timeout = AnnouncementTimeoutSeconds;
			yield return www.SendWebRequest();
			string text = www.downloadHandler.text;
			if (!CanShowAutomaticAnnouncement() || string.IsNullOrEmpty(text))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				if (status != HttpsRequestStatus.ServerError && status == HttpsRequestStatus.SuccessFul && !string.IsNullOrEmpty(sm.msgBody.bizInfo.dataString) && sm.msgBody.bizInfo.dataString == "true")
				{
					Singleton<UIManager>.Instance.m_Mono.StartCoroutine(GongGaoShow(requested: false));
				}
			});
		}

		private bool CanShowAutomaticAnnouncement()
		{
			return GameObject != null && GameObject.activeInHierarchy && m_ServerConnectionTimer == null
				&& !Singleton<NetManager>.Instance.GameServerIsAlive();
		}

		private IEnumerator webrequest(string loginId, string sign, string time)
		{
			if (signInStatus == SignInStatus.SignInIng || m_ServerConnectionTimer != null)
			{
				yield break;
			}
			signInStatus = SignInStatus.SignInIng;
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("loginId", loginId);
			wWWForm.AddField("sign", sign);
			wWWForm.AddField("timestamp", time);
			wWWForm.AddField("type", 2);
			wWWForm.AddField("clientVersion", "1.3.5");
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			using UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/channel/login", wWWForm);
			www.timeout = Constants.UsesLocalServer ? 6 : 12;
			yield return www.SendWebRequest();
			signInStatus = SignInStatus.None;
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			string text = www.downloadHandler.text;
			if (!string.IsNullOrEmpty(text))
			{
				HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
				{
					switch (status)
					{
					case HttpsRequestStatus.ServerError:
						MonoSingleton<GameStart>.Instance.PopUpAttention(content);
						break;
					case HttpsRequestStatus.SuccessFul:
						if (LoginSuccess(ReadLoginResponse<LoginModel>(sm.msgBody.bizInfo.dataString)))
							IsShowGongGao();
						break;
					}
				});
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("登录失败，服务器没有响应");
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
		}

		private IEnumerator webrequest(string userName, string password, bool isShowGongGao = false, bool showSeal = false)
		{
			if (signInStatus == SignInStatus.SignInIng || m_ServerConnectionTimer != null)
			{
				yield break;
			}
			signInStatus = SignInStatus.SignInIng;
			if (showSeal)
			{
				InkSealFeedback feedback = InkSealFeedback.Play(_panel.SignInBtn, "登录");
				yield return null;
				if (GameObject == null || !GameObject.activeInHierarchy)
				{
					if (feedback != null)
						feedback.Finish();
					yield break;
				}
			}
			bool emailLogin = Constants.UseEmailRegistration;
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField(emailLogin ? "email" : "mobilePhone", userName);
			wWWForm.AddField("password", password);
			wWWForm.AddField("clientVersion", "1.3.5");
			bool remember = RememberedLogin.CanUse;
			if (remember)
				wWWForm.AddField("rememberLogin", "1");
			Singleton<UIManager>.Instance.PopUpWnd("NetMessageWnd.prefab", true, "正在登录");
			using UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/login", wWWForm);
			www.timeout = Constants.UsesLocalServer ? 6 : 12;
			if (remember)
				www.redirectLimit = 0;
			yield return www.SendWebRequest();
			signInStatus = SignInStatus.None;
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
			string text = www.downloadHandler.text;
			if (!string.IsNullOrEmpty(text))
			{
				if (emailLogin)
				{
					if (HttpsRequest.TryReadAccountSuccess(text, out string data, out string message) && !string.IsNullOrEmpty(data))
					{
						CompleteSignIn(data, userName, password, emailLogin, isShowGongGao);
					}
					else
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention(message);
					}
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
						CompleteSignIn(sm.msgBody.bizInfo.dataString, userName, password, emailLogin, isShowGongGao);
						break;
					}
				});
			}
			else
			{
				string message = "登录失败，服务器没有响应";
				if (Constants.UsesLocalServer)
				{
					message = $"无法连接本机服务 {Constants.LoginServerIP}:{Constants.LoginServerPort}。请启动本机服务后重试。";
				}
				else if (DirectCloudTrial.Enabled)
				{
					message = "无法连接云端试用服，请检查网络后重试。";
				}
				MonoSingleton<GameStart>.Instance.PopUpAttention(message);
			}
		}

		private void CompleteSignIn(string data, string account, string password, bool emailLogin, bool showAnnouncement)
		{
			LoginModel login = ReadLoginResponse<LoginModel>(data);
			if (!LoginSuccess(login))
				return;

			account = emailLogin ? account.ToLowerInvariant() : account;
			ObscuredPrefs.SetString("yonghuming", account);
			if (!RememberedLogin.TrySave(login, account))
			{
				RememberedLogin.Forget();
				if (RememberedLogin.SupportsLegacyPassword && !IsEmailIdentifier(account) &&
					string.IsNullOrEmpty(login.rememberToken))
				{
					ObscuredPrefs.SetString("mima", password);
					PlayerPrefs.Save();
				}
			}
			if (showAnnouncement)
			{
				IsShowGongGao();
			}
		}

		private static T ReadLoginResponse<T>(string result) where T : class
		{
			if (string.IsNullOrEmpty(result))
				return null;
			try
			{
				return JsonMapper.ToObject<T>(result);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private bool LoginSuccess(LoginModel lm)
		{
			if (lm?.gameServersInfoList == null || lm.gameServersInfoList.Count == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("暂时没有可用的服务器，请稍后重试。");
				return false;
			}
			object[] paralist = new object[3]
			{
				lm.gameServersInfoList,
				(Action<string, int, GameServerInfo>)((string ip, int port, GameServerInfo serverInfo) =>
				{
					if (m_ServerConnectionTimer != null)
					{
						return;
					}
					NetManager network = Singleton<NetManager>.Instance;
					if (network.GameServerIsAlive())
					{
						network.NetClose();
					}
					currentRoleVo = serverInfo.currentRoleVO;
					Constants.PayIp = lm.payIp;
					Constants.PayPort = lm.payPort;
					Constants.SocketServerIP = ip;
					Constants.SocketServerPort = port;
					Constants.SocketServerName = serverInfo.name;
					Singleton<RoleManager>.Instance.Init(lm.mobilePhone, lm.userId, serverInfo.gameServerId,
						serverInfo.gameServerIpManageId, lm.huoDongList, currentRoleVo?.serversRoleId ?? "");
					Singleton<UIManager>.Instance.PopUpWnd("NetMessageWnd.prefab", true, "正在获取角色信息");
					network.ConnectSuccessEvent -= ConnectSuccess;
					network.ConnectSuccessEvent += ConnectSuccess;
					GameStart game = MonoSingleton<GameStart>.Instance;
					m_ServerConnectionTimer = Timer.Register(10f,
						() => game.ReturnToServerSignIn("连接游戏服务器超时，请重新登录。"),
						useRealTime: true, autoDestroyOwner: game);
					network.StartFirstConnect(ip, port);
				}),
				lm.lastGameServersId
			};
			Singleton<UIManager>.Instance.PopUpWnd("ChooseServerWnd.prefab", bTop: true, paralist);
			return true;
		}

		private void ConnectSuccess(ServerType serverType, bool isRestart)
		{
			if (serverType != ServerType.GamesNormalServer)
			{
				return;
			}
			StopWaitingForServerConnection();
			if (!Singleton<NetManager>.Instance.GameServerIsAlive())
			{
				MonoSingleton<GameStart>.Instance.ReturnToServerSignIn("游戏服务器连接已断开，请重新登录。");
				return;
			}
			if (currentRoleVo != null)
			{
				GetRoleAndMapInFo();
				return;
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
			Singleton<UIManager>.Instance.PopUpWnd("CreateRoleWnd.prefab", true);
		}

		private void StopWaitingForServerConnection()
		{
			Singleton<NetManager>.Instance.ConnectSuccessEvent -= ConnectSuccess;
			Timer.Cancel(m_ServerConnectionTimer);
			m_ServerConnectionTimer = null;
		}

		internal void CancelServerSelection()
		{
			StopWaitingForServerConnection();
			currentRoleVo = null;
		}

		private void InitGameInfo(UnityAction callBack)
		{
		}

		private void GetRoleAndMapInFo()
		{
			Singleton<AdvertisingManager>.Instance.LoadAd();
			MonoSingleton<GameStart>.Instance.EnterMainCity(Singleton<RoleManager>.Instance.currentRoleId, CheckOfflineRequest);
		}

		private void CheckOfflineRequest()
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			if (role_Entity != null && role_Entity.PracticeGuideVal == 100)
			{
				Timer.Register(0.5f, OfflineRequest);
			}
		}

		private void OfflineRequest()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.RoleOffline, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleOfflineResp_1002003 roleOfflineResp_ = new RoleOfflineResp_1002003();
					roleOfflineResp_.MergeFrom(byteString);
					if (roleOfflineResp_.IsSuccess)
					{
						if (Constants.UsesPackagedTrialData && roleOfflineResp_.OfflineTime == 0)
							return;
						OfflineEntity offlineEntity = new OfflineEntity
						{
							Age = roleOfflineResp_.Age,
							OfflineTime = roleOfflineResp_.OfflineTime,
							Perception = roleOfflineResp_.Perception,
							Physique = roleOfflineResp_.Physique,
							SuperAir = roleOfflineResp_.SuperAir,
							OfflineClever = roleOfflineResp_.OfflineClever,
							OfflineVigor = roleOfflineResp_.OfflineVigor,
							IsTopValue = roleOfflineResp_.IsFullTime
						};
						Singleton<UIManager>.Instance.PopUpWnd("LiXianShouYiWnd.prefab", true, offlineEntity);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleOfflineResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		public void TongYiGongShi()
		{
		}

		private void JuJueGongShi()
		{
			MonoSingleton<GameRoot>.Instance.ExitApplication();
		}

		public override void OnDisable()
		{
			signInStatus = SignInStatus.None;
			CancelServerSelection();
			base.OnDisable();
		}
	}
}
