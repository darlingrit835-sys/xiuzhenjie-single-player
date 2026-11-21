using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChongShenWnd : Window
	{
		public ChongShengPanel Panel;

		private string m_ChongShengFailText = "重生失败,请稍后再试！";

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChongShengPanel>();
			if (paralist.Length == 0)
			{
				AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			}
			AddButtonClickListener(Panel.GetCodeBtn, GetCode);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			AddToggleClickListener(Panel.GoXuan, GoXuanHo);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
		}

		private void CloseWnd()
		{
			if (!Singleton<RoleManager>.Instance.RoleIsDie())
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			}
		}

		private void GetCode()
		{
			if (!string.IsNullOrEmpty(Panel.PhoneNumInput.text))
			{
				Panel.StartCoroutine(SendCheckCode());
				Panel.StartCoroutine(GetCheckCodeSuccess());
			}
		}

		private void QueRenClick()
		{
			if (Panel.YanZhengCode.text != "确认重生")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请正确输入“确认重生”");
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确认重生?", () =>
			{
				Panel.StartCoroutine(ChongShengRequest());
			});
		}

		private void GoXuanHo(bool t)
		{
			if (Panel.GoXuan.isOn)
			{
				Panel.QueRenBtn.interactable = true;
			}
			else
			{
				Panel.QueRenBtn.interactable = false;
			}
		}

		public IEnumerator SendCheckCode()
		{
			string text = Panel.PhoneNumInput.text;
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

		public IEnumerator GetCheckCodeSuccess()
		{
			Text daoShuText = Panel.GetCodeBtn.transform.Find("Text").gameObject.GetComponent<Text>();
			Panel.GetCodeBtn.interactable = false;
			for (int i = 0; i < 60; i++)
			{
				daoShuText.text = (60 - i).ToString();
				yield return new WaitForSeconds(1f);
			}
			daoShuText.text = "获取暗号";
			Panel.GetCodeBtn.interactable = true;
		}

		public IEnumerator ChongShengRequest()
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("roleId", Singleton<RoleManager>.Instance.Role_Entity.Id);
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			UnityWebRequest www = HttpsRequest.UnityPostRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/user/roleSuicide", wWWForm);
			yield return www.SendWebRequest();
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
						ChongShenAction();
						break;
					}
				});
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("重生失败,服务器没有响应");
			}
			Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab", destory: false);
		}

		private void ChongShenAction()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_SUICIDE, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleSuicideResp_1002008 roleSuicideResp_ = new RoleSuicideResp_1002008();
					roleSuicideResp_.MergeFrom(byteString);
					if (roleSuicideResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.CloseAllWnd();
						MonoSingleton<GameStart>.Instance.ComeBackToLoginScene("重生成功！请重新登陆");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleSuicideResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnClose()
		{
			base.OnClose();
			MonoSingleton<GameStart>.Instance.StopCoroutine(SendCheckCode());
			MonoSingleton<GameStart>.Instance.StopCoroutine(GetCheckCodeSuccess());
			MonoSingleton<GameStart>.Instance.StopCoroutine(ChongShengRequest());
		}
	}
}
