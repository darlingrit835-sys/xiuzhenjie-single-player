using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SignUpWnd : Window
	{
		public SignUpPanel signUpPanel;

		private IEnumerator GetCheckCodeSuccessIE;

		private IEnumerator SendCodeIE;

		private IEnumerator RegisterIE;

		private const int EmailCodeLength = 6;

		private const int EmailCodeResendSeconds = 60;

		private bool sendingEmailCode;

		private bool registeringEmail;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			signUpPanel = base.GameObject.GetComponent<SignUpPanel>();
			AddButtonClickListener(signUpPanel.JinRuXiuZhenJieBtn, JinRuXiuXianJieBtnClick);
			AddButtonClickListener(signUpPanel.BackBtn, BackLastUIClick);
			if (Constants.UseEmailRegistration)
			{
				ConfigureEmailRegistration();
				AddButtonClickListener(signUpPanel.HuoQuAnHaoBtn, SubmitEmailCodeRequest);
				return;
			}
			AddButtonClickListener(signUpPanel.HuoQuAnHaoBtn, HuoQuAnHaoBtnClick);
			GetCheckCodeSuccessIE = GetCheckCodeSuccess();
			SendCodeIE = SendCheckCode();
			RegisterIE = Register();
		}

		private void ConfigureEmailRegistration()
		{
			var form = (RectTransform)signUpPanel.ShengXianHaoPaiIpt.transform.parent;
			var background = signUpPanel.transform.Find("ShanShuiBg").GetComponent<Image>();
			InkInterfaceArt.Set(background, "SignInLandscape", false);
			InkDialogArt.Inset(background.rectTransform, 0f, 0f, 0f, 0f);
			background.raycastTarget = false;
			if (background.sprite != null)
			{
				var cover = background.gameObject.AddComponent<AspectRatioFitter>();
				cover.aspectRatio = background.sprite.rect.width / background.sprite.rect.height;
				cover.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
			}

			signUpPanel.transform.Find("Title").gameObject.SetActive(false);
			form.anchorMin = new Vector2(.06f, .47f);
			form.anchorMax = new Vector2(.94f, .47f);
			form.pivot = new Vector2(.5f, .5f);
			form.sizeDelta = new Vector2(0f, 349f);
			form.anchoredPosition = Vector2.zero;
			InkDialogArt.SetPaper(form.GetComponent<Image>(), "Parchment");

			Text heading = signUpPanel.transform.Find("describeTitle").GetComponent<Text>();
			heading.transform.SetParent(form, false);
			PlaceFormRow(heading.rectTransform, 16f, 31f);
			heading.text = "邮箱注册";
			heading.color = InkInterfaceArt.Ink;
			heading.fontSize = 23;
			heading.alignment = TextAnchor.MiddleLeft;
			heading.raycastTarget = false;

			var email = signUpPanel.ShengXianHaoPaiIpt;
			var code = signUpPanel.AnHaoIpt;
			var password = signUpPanel.ShengXianMiLingIpt;
			email.contentType = InputField.ContentType.EmailAddress;
			code.contentType = InputField.ContentType.IntegerNumber;
			code.characterLimit = EmailCodeLength;
			code.onValidateInput = (_, __, character) => IsCodeDigit(character) ? character : '\0';
			password.contentType = InputField.ContentType.Password;
			StyleFormRow(form, email, "ShengXianHaoPaiText", "邮箱", "请输入常用邮箱", 62f);
			StyleFormRow(form, code, "AnHaoText", "验证码", "6 位验证码", 133f, 114f);
			StyleFormRow(form, password, "ShengXianMiLingText", "密码", "8 至 72 字节", 204f);
			form.Find("QueRenMiLingText").gameObject.SetActive(false);
			signUpPanel.QueRenMiLingIpt.gameObject.SetActive(false);
			email.onSubmit.AddListener(_ => code.ActivateInputField());
			code.onSubmit.AddListener(_ => password.ActivateInputField());
			password.onSubmit.AddListener(_ => SubmitEmailRegistration());

			var send = signUpPanel.HuoQuAnHaoBtn;
			send.transform.SetParent(form, false);
			send.gameObject.SetActive(true);
			var sendRect = (RectTransform)send.transform;
			sendRect.anchorMin = sendRect.anchorMax = Vector2.one;
			sendRect.pivot = Vector2.one;
			sendRect.sizeDelta = new Vector2(88f, 39f);
			sendRect.anchoredPosition = new Vector2(-18f, -155f);
			StyleFormAction(send, "获取验证码", "ParchmentHeading", InkInterfaceArt.Ink);
			PlaceFormRow((RectTransform)signUpPanel.JinRuXiuZhenJieBtn.transform, 287f, 44f);
			StyleFormAction(signUpPanel.JinRuXiuZhenJieBtn, "注册", "JadeButton", InkDialogArt.Ivory);
			var back = (RectTransform)signUpPanel.BackBtn.transform;
			back.sizeDelta = new Vector2(40f, 40f);
			InkInterfaceArt.Set(signUpPanel.BackBtn.image, "Back");
		}

		private static void PlaceFormRow(RectTransform row, float top, float height, float right = 18f)
		{
			row.anchorMin = new Vector2(0f, 1f);
			row.anchorMax = Vector2.one;
			row.offsetMin = new Vector2(18f, -top - height);
			row.offsetMax = new Vector2(-right, -top);
		}

		private static void StyleFormRow(Transform form, InputField input, string labelName, string title,
			string placeholder, float top, float right = 18f)
		{
			Text label = form.Find(labelName).GetComponent<Text>();
			label.gameObject.SetActive(true);
			PlaceFormRow(label.rectTransform, top, 21f);
			label.text = title;
			label.fontSize = 14;
			label.alignment = TextAnchor.MiddleLeft;
			label.color = InkInterfaceArt.Ink;
			label.raycastTarget = false;

			input.gameObject.SetActive(true);
			PlaceFormRow((RectTransform)input.transform, top + 22f, 39f, right);
			Image surface = input.gameObject.GetComponent<Image>() ?? input.gameObject.AddComponent<Image>();
			InkInterfaceArt.Set(surface, "LightPaper", false);
			input.targetGraphic = surface;
			input.transform.Find("Image")?.gameObject.SetActive(false);
			var hint = (Text)input.placeholder;
			hint.text = placeholder;
			hint.color = InkInterfaceArt.SoftInk;
			input.textComponent.color = InkInterfaceArt.Ink;
			input.customCaretColor = true;
			input.caretColor = InkInterfaceArt.Ink;
			foreach (var text in new[] { input.textComponent, hint })
			{
				InkDialogArt.Inset(text.rectTransform, 9f, 9f, 3f, 3f);
				text.fontSize = 14;
				text.alignment = TextAnchor.MiddleLeft;
				text.raycastTarget = false;
			}
		}

		private static void StyleFormAction(Button button, string title, string surface, Color ink)
		{
			InkDialogArt.StyleButton(button, surface, ink);
			Text label = button.GetComponentInChildren<Text>(true);
			InkDialogArt.Inset(label.rectTransform, 4f, 4f, 0f, 0f);
			label.text = title;
			label.fontSize = 15;
			label.alignment = TextAnchor.MiddleCenter;
			label.raycastTarget = false;
		}

		public void HuoQuAnHaoBtnClick()
		{
			if (GetCheckCodeSuccessIE != null)
			{
				MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(SendCodeIE);
			}
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(GetCheckCodeSuccessIE);
		}

		public void JinRuXiuXianJieBtnClick()
		{
			if (Constants.UseEmailRegistration)
			{
				SubmitEmailRegistration();
				return;
			}
			string text = signUpPanel.ShengXianHaoPaiIpt.text;
			string text2 = signUpPanel.AnHaoIpt.text;
			string text3 = signUpPanel.ShengXianMiLingIpt.text;
			string text4 = signUpPanel.QueRenMiLingIpt.text;
			if (!CheckPhoneNum(text))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入正确的手机号");
				return;
			}
			if (string.IsNullOrEmpty(text2))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("邀请码不能为空");
				return;
			}
			if (!CheckPassword(text3))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("密令必须为6位以上");
				return;
			}
			if (text4 != text3)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("两次输入的密令不一致,请确认");
				return;
			}
			MonoSingleton<GameStart>.Instance.m_mono.StopCoroutine(RegisterIE);
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(RegisterIE);
		}

		public void BackLastUIClick()
		{
			if (!Constants.UseEmailRegistration)
			{
				MonoSingleton<GameStart>.Instance.StopCoroutine(RegisterIE);
				MonoSingleton<GameStart>.Instance.StopCoroutine(SendCodeIE);
				MonoSingleton<GameStart>.Instance.StopCoroutine(GetCheckCodeSuccessIE);
			}
			Singleton<UIManager>.Instance.CloseWnd("SignUpWnd.prefab");
		}

		private static bool IsCodeDigit(char character) => character >= '0' && character <= '9';

		private bool TryReadRegistrationEmail(out string email)
		{
			email = signUpPanel.ShengXianHaoPaiIpt.text.Trim();
			int separator = email.IndexOf('@');
			if (separator > 0 && separator < email.Length - 1 && separator == email.LastIndexOf('@') && Encoding.UTF8.GetByteCount(email) <= 254)
			{
				return true;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention("请输入有效的邮箱");
			return false;
		}

		private void SubmitEmailCodeRequest()
		{
			if (sendingEmailCode || registeringEmail || !signUpPanel.HuoQuAnHaoBtn.interactable ||
				!TryReadRegistrationEmail(out string email))
			{
				return;
			}
			sendingEmailCode = true;
			signUpPanel.HuoQuAnHaoBtn.interactable = false;
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(SendEmailCode(email));
		}

		private IEnumerator SendEmailCode(string email)
		{
			var form = new WWWForm();
			form.AddField("email", email);
			using (UnityWebRequest request = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/sendEmailCode", form))
			{
				request.timeout = 20;
				yield return request.SendWebRequest();
				sendingEmailCode = false;
				if (GameObject == null || !GameObject.activeInHierarchy)
				{
					yield break;
				}

				string response = request.downloadHandler.text;
				if (string.IsNullOrEmpty(response))
				{
					signUpPanel.HuoQuAnHaoBtn.interactable = true;
					MonoSingleton<GameStart>.Instance.PopUpAttention("验证码发送失败，服务器没有响应");
					yield break;
				}
				if (ReadEmailMutationResult(response, out string message))
				{
					MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(EmailCodeResendCountdown());
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("验证码已发送，请查收邮箱");
					signUpPanel.AnHaoIpt.ActivateInputField();
				}
				else
				{
					signUpPanel.HuoQuAnHaoBtn.interactable = true;
					MonoSingleton<GameStart>.Instance.PopUpAttention(message);
				}
			}
		}

		private IEnumerator EmailCodeResendCountdown()
		{
			var button = signUpPanel.HuoQuAnHaoBtn;
			var label = button.GetComponentInChildren<Text>();
			float expiresAt = Time.realtimeSinceStartup + EmailCodeResendSeconds;
			while (button != null && Time.realtimeSinceStartup < expiresAt)
			{
				int remaining = Mathf.CeilToInt(expiresAt - Time.realtimeSinceStartup);
				label.text = $"{remaining}秒后重发";
				yield return new WaitForSecondsRealtime(1f);
			}
			if (button != null)
			{
				label.text = "获取验证码";
				button.interactable = true;
			}
		}

		private void SubmitEmailRegistration()
		{
			if (registeringEmail || sendingEmailCode || !TryReadRegistrationEmail(out string email))
			{
				return;
			}
			string code = signUpPanel.AnHaoIpt.text;
			if (code.Length != EmailCodeLength || !code.All(IsCodeDigit))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入6位验证码");
				return;
			}
			string password = signUpPanel.ShengXianMiLingIpt.text;
			int passwordBytes = Encoding.UTF8.GetByteCount(password);
			if (string.IsNullOrWhiteSpace(password) || passwordBytes < 8 || passwordBytes > 72)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("密码长度须为 8 至 72 字节");
				return;
			}
			registeringEmail = true;
			signUpPanel.JinRuXiuZhenJieBtn.interactable = false;
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(RegisterEmail(email, password, code));
		}

		private IEnumerator RegisterEmail(string email, string password, string code)
		{
			var form = new WWWForm();
			form.AddField("email", email);
			form.AddField("password", password);
			form.AddField("checkCode", code);
			using (UnityWebRequest request = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/register", form))
			{
				request.timeout = 10;
				yield return request.SendWebRequest();
				registeringEmail = false;
				if (GameObject == null || !GameObject.activeInHierarchy)
				{
					yield break;
				}
				signUpPanel.JinRuXiuZhenJieBtn.interactable = true;
				string response = request.downloadHandler.text;
				if (string.IsNullOrEmpty(response))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("注册失败，服务器没有响应");
					yield break;
				}
				if (ReadEmailMutationResult(response, out string message))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("注册成功，请登录", () =>
					{
						var ui = Singleton<UIManager>.Instance;
						ui.CloseWnd(this);
						ui.FindWndByName<SignInWnd>("SignInWnd.prefab")?.PrefillRegisteredEmail(email.ToLowerInvariant());
					});
				}
				else
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(message);
				}
			}
		}

		private static bool ReadEmailMutationResult(string response, out string message)
		{
			if (!HttpsRequest.TryReadAccountSuccess(response, out string data, out message))
			{
				return false;
			}
			message = "无法读取服务器响应，请重试";
			return data == "true";
		}

		private void RegisterSuccessful()
		{
			ObscuredPrefs.SetString("yonghuming", signUpPanel.ShengXianHaoPaiIpt.text);
			ObscuredPrefs.SetString("mima", signUpPanel.ShengXianMiLingIpt.text);
			MonoSingleton<GameStart>.Instance.StopCoroutine(RegisterIE);
			MonoSingleton<GameStart>.Instance.StopCoroutine(SendCodeIE);
			MonoSingleton<GameStart>.Instance.StopCoroutine(GetCheckCodeSuccessIE);
			Singleton<UIManager>.Instance.CloseWnd("SignUpWnd.prefab");
			Singleton<UIManager>.Instance.FindWndByName<SignInWnd>("SignInWnd.prefab").TongYiGongShi();
		}

		private bool CheckPhoneNum(string number)
		{
			if (string.IsNullOrEmpty(number))
			{
				return false;
			}
			string text = number.Substring(0, 1);
			if (number.Length == 11 && text == "1")
			{
				return true;
			}
			return false;
		}

		private bool CheckPassword(string passWord)
		{
			if (string.IsNullOrEmpty(passWord))
			{
				return false;
			}
			if (passWord.Length >= 6)
			{
				return true;
			}
			return false;
		}

		private IEnumerator SendCheckCode()
		{
			string text = signUpPanel.ShengXianHaoPaiIpt.text;
			if (string.IsNullOrEmpty(text))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入手机号");
				yield break;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("mobilePhone", text);
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/sendCheckCode", dictionary);
			yield return www.SendWebRequest();
			string text2 = www.downloadHandler.text;
			if (string.IsNullOrEmpty(text2))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text2, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				if (status != HttpsRequestStatus.ServerError)
				{
					_ = 1;
				}
				else
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(content);
				}
			});
		}

		private IEnumerator Register()
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("mobilePhone", signUpPanel.ShengXianHaoPaiIpt.text);
			wWWForm.AddField("checkCode", signUpPanel.AnHaoIpt.text);
			wWWForm.AddField("password", signUpPanel.ShengXianMiLingIpt.text);
			UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/register", wWWForm);
			yield return www.SendWebRequest();
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
						MonoSingleton<GameStart>.Instance.PopUpAttention("注册成功", () =>
						{
							RegisterSuccessful();
						});
						break;
					}
				});
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("注册失败,服务器没有响应");
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
		}

		private IEnumerator GetCheckCodeSuccess()
		{
			Text daoShuText = signUpPanel.HuoQuAnHaoBtn.transform.Find("Text").gameObject.GetComponent<Text>();
			signUpPanel.HuoQuAnHaoBtn.interactable = false;
			for (int i = 0; i < 60; i++)
			{
				daoShuText.text = (60 - i).ToString();
				yield return new WaitForSeconds(1f);
			}
			daoShuText.text = "获取暗号";
			signUpPanel.HuoQuAnHaoBtn.interactable = true;
		}
	}
}
