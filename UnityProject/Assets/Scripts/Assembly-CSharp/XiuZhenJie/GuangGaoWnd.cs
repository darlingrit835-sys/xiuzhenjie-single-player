using System;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class GuangGaoWnd : Window
	{
		public GuangGaoPanel Panel;

		private float ShengYuTime;

		private bool m_isVip;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<GuangGaoPanel>();
			m_isVip = false;
			AdvertisingManager instance = Singleton<AdvertisingManager>.Instance;
			instance.ErrorAction = (UnityAction<string, string>)Delegate.Combine(instance.ErrorAction, new UnityAction<string, string>(ErrorMsg));
			AdvertisingManager instance2 = Singleton<AdvertisingManager>.Instance;
			instance2.PlayCompeleteAction = (UnityAction)Delegate.Combine(instance2.PlayCompeleteAction, new UnityAction(RequestJiangLi));
			AddButtonClickListener(Panel.TuiChu, CloseThisWnd);
			AddButtonClickListener(Panel.GUanKan, ShowRewardAd);
			GetDateTime();
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (ShengYuTime > 0f)
			{
				ShengYuTime -= Time.fixedDeltaTime;
				string text = "";
				text = ((!(ShengYuTime > 60f)) ? string.Format("0:{0}", ShengYuTime.ToString("0")) : string.Format("1:{0}", (ShengYuTime - 60f).ToString("0")));
				Panel.BtnText.text = text;
			}
			else if (!Panel.GUanKan.interactable && ShengYuTime <= 0f)
			{
				ShengYuTime = 0f;
				Panel.BtnText.text = "观看广告";
				Panel.GUanKan.interactable = true;
			}
		}

		public void SetDateTime()
		{
			Panel.GUanKan.interactable = false;
			PlayerPrefs.SetString("GuangGao", DateTime.Now.ToShortTimeString());
			ShengYuTime = 119f;
		}

		public void GetDateTime()
		{
			string text = PlayerPrefs.GetString("GuangGao");
			if (!string.IsNullOrEmpty(text))
			{
				DateTime now = DateTime.Now;
				DateTime dateTime = DateTime.Parse(text);
				TimeSpan timeSpan = now - dateTime;
				if (timeSpan.TotalSeconds >= 120.0 || timeSpan.TotalSeconds < 0.0)
				{
					ShengYuTime = 0f;
					Panel.BtnText.text = "观看广告";
					Panel.GUanKan.interactable = true;
				}
				else
				{
					ShengYuTime = 120f - (float)timeSpan.TotalSeconds;
					Panel.GUanKan.interactable = false;
					ShengYuTime = ((ShengYuTime > 120f) ? 120f : ShengYuTime);
				}
			}
		}

		private void ShowRewardAd()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_WHETHER_LOOK, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ActionBoolResp_1001001 actionBoolResp_ = new ActionBoolResp_1001001();
					actionBoolResp_.MergeFrom(byteString);
					if (actionBoolResp_.IsSuccess)
					{
						if (actionBoolResp_.Code == 2005)
						{
							m_isVip = true;
							RequestJiangLi();
						}
						else
						{
							Singleton<AdvertisingManager>.Instance.ShowVedio();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void RequestJiangLi()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_LOOK_ADVERTISING, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					LookAdvertisingResp_1001003 lookAdvertisingResp_ = new LookAdvertisingResp_1001003();
					lookAdvertisingResp_.MergeFrom(byteString);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(lookAdvertisingResp_.Code);
					if (lookAdvertisingResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = lookAdvertisingResp_.CurrentJade;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
						if (!m_isVip)
						{
							SetDateTime();
						}
					}
					else
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("GuangGaoWnd.prefab");
		}

		private void ErrorMsg(string code, string msg)
		{
			Debug.Log("广告异常：code:" + code + "msg" + msg);
		}

		public override void OnClose()
		{
			base.OnClose();
			AdvertisingManager instance = Singleton<AdvertisingManager>.Instance;
			instance.ErrorAction = (UnityAction<string, string>)Delegate.Remove(instance.ErrorAction, new UnityAction<string, string>(ErrorMsg));
			AdvertisingManager instance2 = Singleton<AdvertisingManager>.Instance;
			instance2.PlayCompeleteAction = (UnityAction)Delegate.Remove(instance2.PlayCompeleteAction, new UnityAction(RequestJiangLi));
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}
	}
}
