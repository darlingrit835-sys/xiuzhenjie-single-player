using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ForgetPasWnd : Window
	{
		public ForgetPasPanel _panel;

		private IEnumerator GetCheckCodeSuccessIE;

		private IEnumerator SendCodeIE;

		private IEnumerator RegisterIE;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ForgetPasPanel>();
			AddButtonClickListener(_panel.HuoQuAnHaoBtn, HuoQuAnHaoBtnClick);
			AddButtonClickListener(_panel.TiJiaoBtn, JinRuXiuXianJieBtnClick);
			AddButtonClickListener(_panel.BackBtn, BackLastUIClick);
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
			string text = _panel.ShengXianHaoPaiIpt.text;
			string text2 = _panel.AnHaoIpt.text;
			string text3 = _panel.ShengXianMiLingIpt.text;
			string text4 = _panel.QueRenMiLingIpt.text;
			if (!CheckPhoneNum(text))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入正确的手机号");
				return;
			}
			if (string.IsNullOrEmpty(text2))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("验证码不能为空");
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
			Singleton<UIManager>.Instance.CloseWnd("ForgetPasWnd.prefab");
		}

		private void RegisterSuccessful()
		{
			MonoSingleton<GameStart>.Instance.StopCoroutine(RegisterIE);
			MonoSingleton<GameStart>.Instance.StopCoroutine(SendCodeIE);
			MonoSingleton<GameStart>.Instance.StopCoroutine(GetCheckCodeSuccessIE);
			Singleton<UIManager>.Instance.CloseWnd("ForgetPasWnd.prefab");
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
			string text = _panel.ShengXianHaoPaiIpt.text;
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
			wWWForm.AddField("mobilePhone", _panel.ShengXianHaoPaiIpt.text);
			wWWForm.AddField("checkCode", _panel.AnHaoIpt.text);
			wWWForm.AddField("password", _panel.ShengXianMiLingIpt.text);
			UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/pwdUpdate", wWWForm);
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
						MonoSingleton<GameStart>.Instance.PopUpAttention("修改密令成功", () =>
						{
							RegisterSuccessful();
						});
						break;
					}
				});
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("修改密令成功失败,服务器没有响应");
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
		}

		private IEnumerator GetCheckCodeSuccess()
		{
			Text daoShuText = _panel.HuoQuAnHaoBtn.transform.Find("Text").gameObject.GetComponent<Text>();
			_panel.HuoQuAnHaoBtn.interactable = false;
			for (int i = 0; i < 60; i++)
			{
				daoShuText.text = (60 - i).ToString();
				yield return new WaitForSeconds(1f);
			}
			daoShuText.text = "获取暗号";
			_panel.HuoQuAnHaoBtn.interactable = true;
		}
	}
}
