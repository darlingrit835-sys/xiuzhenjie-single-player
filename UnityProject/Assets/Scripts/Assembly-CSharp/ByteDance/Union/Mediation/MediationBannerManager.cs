using System;
using System.Collections.Generic;
using UnityEngine;

namespace ByteDance.Union.Mediation
{
	public sealed class MediationBannerManager : IDisposable
	{
		private AndroidJavaObject handle;

		internal MediationBannerManager(AndroidJavaObject javaObject)
		{
			handle = javaObject;
		}

		public void Dispose()
		{
		}

		public bool IsReady()
		{
			if (handle != null)
			{
				return handle.Call<bool>("isReady", Array.Empty<object>());
			}
			return true;
		}

		public List<MediationAdLoadInfo> GetAdLoadInfo()
		{
			if (handle != null)
			{
				return Utils.GetAdLoadInfo(handle);
			}
			return null;
		}

		public MediationAdEcpmInfo GetBestEcpm()
		{
			if (handle != null)
			{
				return Utils.GetBestEcpm(handle);
			}
			return null;
		}

		public List<MediationAdEcpmInfo> GetMultiBiddingEcpm()
		{
			if (handle != null)
			{
				return Utils.GetMultiBiddingEcpm(handle);
			}
			return null;
		}

		public MediationAdEcpmInfo GetShowEcpm()
		{
			if (handle != null)
			{
				return Utils.GetShowEcpm(handle);
			}
			return null;
		}

		public List<MediationAdEcpmInfo> GetCacheList()
		{
			if (handle != null)
			{
				return Utils.GetCacheList(handle);
			}
			return null;
		}

		public void destroy()
		{
		}
	}
}
