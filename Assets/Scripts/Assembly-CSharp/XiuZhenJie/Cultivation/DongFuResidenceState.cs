using System;
using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	internal static class DongFuResidenceState
	{
		internal const int MaxLevel = 4;
		private const int RankStep = 4;
		private static readonly Dictionary<string, int> Levels = new Dictionary<string, int>(StringComparer.Ordinal);

		internal static event Action<string, int> LevelChanged;

		internal static string CurrentRoleId
		{
			get
			{
				RoleManager roles = Singleton<RoleManager>.Instance;
				string id = roles.Role_Entity?.Id;
				return string.IsNullOrEmpty(id) ? roles.currentRoleId : id;
			}
		}

		internal static int GetLevel(string roleId)
		{
			return !string.IsNullOrEmpty(roleId) && Levels.TryGetValue(roleId, out int level) ? level : 0;
		}

		internal static int ApplyRank(string roleId, int wireRank)
		{
			int level = Mathf.Clamp(wireRank / RankStep, 0, MaxLevel);
			if (!string.IsNullOrEmpty(roleId) && (!Levels.TryGetValue(roleId, out int oldLevel) || oldLevel != level))
			{
				Levels[roleId] = level;
				LevelChanged?.Invoke(roleId, level);
			}
			return level;
		}
	}
}
