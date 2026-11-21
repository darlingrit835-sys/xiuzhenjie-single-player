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

		private bool rewardPending;

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
			AddButtonClickListener(Panel.LoginOut_btn, () =>
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("退出后将返回登录界面。", LoginOut, title: "退出登录");
				Singleton<UIManager>.Instance.FindWndByName<AttentionWnd>("AttentionWnd.prefab")?.SetChoices("退出登录", "取消", () => {});
			});
			AddButtonClickListener(Panel.HuiGuiDuiHuanBtn, HuiGuiDuiHuan);
			Panel.ShuRuYanZhengMa_input.onSubmit.AddListener(_ => DuiHuanBtnClick());
			Panel.PhoneNum.onSubmit.AddListener(_ => HuiGuiDuiHuan());
			InkSettingsArt.Build(Panel);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			var account = ObscuredPrefs.GetString("yonghuming");
			Panel.ShengXianHaoPai_text.text = string.IsNullOrEmpty(account) ? "未记录账号" : account;
			Panel.BeiJingYinYue_btn.isOn = PlayerPrefs.GetString("SwitchYinYue") != "false";
			Panel.YinXiaoPeiYue_btn.isOn = PlayerPrefs.GetString("SwitchYinXiao") != "false";
			Panel.PingMuChangLiang_btn.isOn = PlayerPrefs.GetString("PingMuChangLiang") != "false";
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
			string code = Panel.ShuRuYanZhengMa_input.text.Trim();
			if (string.IsNullOrEmpty(code))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入兑换码");
				return;
			}
			var query = new Dictionary<string, string>
			{
				["roleId"] = Singleton<RoleManager>.Instance.Role_Entity.Id,
				["cdKey"] = UnityWebRequest.EscapeURL(code)
			};
			MonoSingleton<GameStart>.Instance.StartCoroutine(Redeem("/login/roleCdKey/cdkeyExchange", query, false));
		}

		private void HuiGuiDuiHuan()
		{
			string phone = Panel.PhoneNum.text.Trim();
			if (string.IsNullOrEmpty(phone))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入原账号手机号");
				return;
			}
			var query = new Dictionary<string, string>
			{
				["mobilePhone"] = UnityWebRequest.EscapeURL(phone),
				["serverRoleId"] = Singleton<RoleManager>.Instance.Role_Entity.Id
			};
			MonoSingleton<GameStart>.Instance.StartCoroutine(Redeem("/login/roleCdKey/returnGiftBag", query, true));
		}

		private IEnumerator Redeem(string path, Dictionary<string, string> query, bool returning)
		{
			if (rewardPending)
				yield break;
			rewardPending = true;
			var game = MonoSingleton<GameStart>.Instance;
			game.ShowWaitReauestWnd();
			using UnityWebRequest request = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, path, query);
			request.timeout = 10;
			yield return request.SendWebRequest();
			rewardPending = false;
			game.HideWaitReauestWnd();
			if (GameObject == null || !GameObject.activeInHierarchy)
				yield break;
			if (string.IsNullOrEmpty(request.downloadHandler.text))
			{
				game.PopUpAttention("未收到兑换结果，请检查邮件或稍后查询。");
				yield break;
			}
			HttpsRequest.DealwithShellModel(request.downloadHandler.text, (HttpsRequestStatus status, ShellModel response, string message) =>
			{
				if (status != HttpsRequestStatus.SuccessFul)
				{
					game.PopUpAttention(string.IsNullOrEmpty(message) ? "暂时无法读取兑换结果。" : message);
					return;
				}
				if (returning)
					Singleton<UIManager>.Instance.CloseWnd(this);
				else
					Panel.ShuRuYanZhengMa_input.text = "";
				game.PopUpAttention(returning ? "回归礼包兑换成功。" : "兑换成功，请到传信中领取。");
			});
		}

		private void LoginOut()
		{
			GameStart game = MonoSingleton<GameStart>.Instance;
			bool revoke = RememberedLogin.TryLoad(out RememberedLogin session);
			RememberedLogin.Forget();
			if (revoke)
				game.StartCoroutine(RevokeRememberedLogin(session));
			Singleton<UIManager>.Instance.CloseAllWnd();
			game.ClearDataSource();
		}

		private static IEnumerator RevokeRememberedLogin(RememberedLogin session)
		{
			var form = new WWWForm();
			form.AddField("userId", session.UserId);
			form.AddField("rememberToken", session.Token);
			using UnityWebRequest request = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort,
				"/user/session/logout", form);
			request.redirectLimit = 0;
			request.timeout = 4;
			yield return request.SendWebRequest();
		}
	}
}
