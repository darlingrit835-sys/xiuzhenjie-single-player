using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class OptionWnd : Window
	{
		public OptionPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<OptionPanel>();
			AddButtonClickListener(Panel.DuiHuan_btn, DuiHuanBtnClick);
			AddButtonClickListener(Panel.FanHui_btn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("OptionWnd.prefab");
			});
			AddToggleClickListener(Panel.BeiJingYinYue_btn, SwitchBackMusic);
			AddToggleClickListener(Panel.YinXiaoPeiYue_btn, SwitchVoice);
			AddToggleClickListener(Panel.PingMuChangLiang_btn, SwitchAlawaysLight);
			AddButtonClickListener(Panel.LoginOut_btn, LoginOut);
			AddButtonClickListener(Panel.HuiGuiDuiHuanBtn, HuiGuiDuiHuan);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Panel.ShengXianHaoPai_text.text = ObscuredPrefs.GetString("yonghuming");
			if (PlayerPrefs.GetString("SwitchYinYue") == "false")
			{
				Panel.BeiJingYinYue_btn.isOn = false;
			}
			else
			{
				Panel.BeiJingYinYue_btn.isOn = true;
			}
			if (PlayerPrefs.GetString("SwitchYinXiao") == "false")
			{
				Panel.YinXiaoPeiYue_btn.isOn = false;
			}
			else
			{
				Panel.YinXiaoPeiYue_btn.isOn = true;
			}
			if (PlayerPrefs.GetString("PingMuChangLiang") == "false")
			{
				Panel.PingMuChangLiang_btn.isOn = false;
			}
			else
			{
				Panel.PingMuChangLiang_btn.isOn = true;
			}
		}

		private void SwitchBackMusic(bool isOn)
		{
			if (isOn)
			{
				Singleton<VoiceManager>.Instance.OpenMusic();
				PlayerPrefs.SetString("SwitchYinYue", "true");
			}
			else
			{
				Singleton<VoiceManager>.Instance.PauseMusic();
				PlayerPrefs.SetString("SwitchYinYue", "false");
			}
		}

		private void SwitchVoice(bool isOn)
		{
			if (isOn)
			{
				Singleton<VoiceManager>.Instance.OpenVoice();
				PlayerPrefs.SetString("SwitchYinXiao", "true");
			}
			else
			{
				Singleton<VoiceManager>.Instance.StopVoice();
				PlayerPrefs.SetString("SwitchYinXiao", "false");
			}
		}

		private void SwitchAlawaysLight(bool isOn)
		{
			if (isOn)
			{
				Screen.sleepTimeout = -1;
				PlayerPrefs.SetString("PingMuChangLiang", "true");
			}
			else
			{
				Screen.sleepTimeout = -2;
				PlayerPrefs.SetString("PingMuChangLiang", "false");
			}
		}

		private void DuiHuanBtnClick()
		{
			string text = Panel.ShuRuYanZhengMa_input.text;
			if (string.IsNullOrEmpty(text))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入兑换码!");
			}
			else
			{
				MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(SendCdKey(text));
			}
		}

		private IEnumerator SendCdKey(string code)
		{
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("roleId", Singleton<RoleManager>.Instance.Role_Entity.Id);
			dictionary.Add("cdKey", code);
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/login/roleCdKey/cdkeyExchange", dictionary);
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
					MonoSingleton<GameStart>.Instance.PopUpAttention("兑换成功,请去邮件中领取!");
					break;
				}
			});
		}

		private void HuiGuiDuiHuan()
		{
			if (string.IsNullOrEmpty(Panel.PhoneNum.text))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入正确的手机号!");
			}
			else
			{
				MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(ShowGongLue(Panel.PhoneNum.text));
			}
		}

		private IEnumerator ShowGongLue(string phoneNum)
		{
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("mobilePhone", phoneNum);
			dictionary.Add("serverRoleId", Singleton<RoleManager>.Instance.Role_Entity.Id);
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/login/roleCdKey/returnGiftBag", dictionary);
			yield return www.SendWebRequest();
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
					MonoSingleton<GameStart>.Instance.PopUpAttention("兑换成功");
					Singleton<UIManager>.Instance.CloseWnd(this);
					break;
				}
			});
		}

		private void LoginOut()
		{
			Singleton<UIManager>.Instance.CloseAllWnd();
			MonoSingleton<GameStart>.Instance.ClearDataSource();
		}
	}
}
