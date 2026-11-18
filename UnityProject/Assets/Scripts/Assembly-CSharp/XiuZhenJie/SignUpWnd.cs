using System.Collections;
using System.Collections.Generic;
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

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			signUpPanel = base.GameObject.GetComponent<SignUpPanel>();
			AddButtonClickListener(signUpPanel.HuoQuAnHaoBtn, HuoQuAnHaoBtnClick);
			AddButtonClickListener(signUpPanel.JinRuXiuZhenJieBtn, JinRuXiuXianJieBtnClick);
			AddButtonClickListener(signUpPanel.BackBtn, BackLastUIClick);
			GetCheckCodeSuccessIE = GetCheckCodeSuccess();
			SendCodeIE = SendCheckCode();
			RegisterIE = Register();
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
			MonoSingleton<GameStart>.Instance.StopCoroutine(RegisterIE);
			MonoSingleton<GameStart>.Instance.StopCoroutine(SendCodeIE);
			MonoSingleton<GameStart>.Instance.StopCoroutine(GetCheckCodeSuccessIE);
			Singleton<UIManager>.Instance.CloseWnd("SignUpWnd.prefab");
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
