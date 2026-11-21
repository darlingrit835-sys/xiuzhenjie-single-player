using System;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class AdvertisingManager : Singleton<AdvertisingManager>
	{
		public UnityAction PlayCompeleteAction;

		public UnityAction LoadCompeleteAction;

		public UnityAction<string, string> ErrorAction;

		private RewardVideoAdManager m_RewardVideoAdManager;

		public void Init()
		{
#if UNITY_ANDROID && !UNITY_EDITOR && !XIUZHENJIE_RECOVERY_BUILD
			m_RewardVideoAdManager = new RewardVideoAdManager();
			m_RewardVideoAdManager.Init();
#endif
		}

		public void LoadAd()
		{
			m_RewardVideoAdManager?.LoadRewardAd(Singleton<RoleManager>.Instance.currentRoleId);
			m_RewardVideoAdManager?.LoadRewardAd(Singleton<RoleManager>.Instance.currentRoleId);
		}

		public void ShowCompelete()
		{
			if (PlayCompeleteAction != null)
			{
				PlayCompeleteAction();
			}
		}

		public void HaveShown()
		{
			Debug.Log("展示过了!");
		}

		public void ADOverdue()
		{
		}

		public void ADClose()
		{
		}

		public void AdError(string code, string msg)
		{
			if (ErrorAction != null)
			{
				ErrorAction(code, msg);
			}
		}

		public bool ShowVedio()
		{
			if (m_RewardVideoAdManager == null)
			{
				return false;
			}
			try
			{
				m_RewardVideoAdManager.ShowRewardAd();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public void CloseMgr()
		{
			LoadCompeleteAction = null;
			PlayCompeleteAction = null;
			m_RewardVideoAdManager = null;
		}
	}
}
