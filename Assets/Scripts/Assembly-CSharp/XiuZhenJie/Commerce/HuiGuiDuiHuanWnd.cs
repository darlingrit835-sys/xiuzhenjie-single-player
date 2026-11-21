using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class HuiGuiDuiHuanWnd : Window
	{
		public HuiGuiDuiHuanPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<HuiGuiDuiHuanPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.DuiHuanBtn, DuiHuan);
		}

		private void DuiHuan()
		{
			if (string.IsNullOrEmpty(Panel.PhoneNum.text))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入争取的手机号!");
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
	}
}
