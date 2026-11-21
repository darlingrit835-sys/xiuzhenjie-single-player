using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace XiuZhenJie
{
	internal enum RingSortField { Rank, Level, Category, Quantity, Price, Name }

	[Serializable]
	internal sealed class RingSortRule
	{
		public RingSortField field;
		public bool selected;
		public bool descending;

		internal RingSortRule Copy() => (RingSortRule)MemberwiseClone();
	}

	[Serializable]
	internal sealed class RingSortPreferences
	{
		private static readonly RingSortField[] Fields = (RingSortField[])Enum.GetValues(typeof(RingSortField));
		private static readonly StringComparer NameOrder = StringComparer.Create(CultureInfo.GetCultureInfo("zh-CN"), false);
		[SerializeField] private List<RingSortRule> rules = new List<RingSortRule>();

		internal IReadOnlyList<RingSortRule> Rules => rules;
		internal int SelectedCount => rules.Count(rule => rule.selected);

		internal static string CurrentRoleKey() => RolePreferenceKey("RingSort");

		internal static string RolePreferenceKey(string feature)
		{
			var role = Singleton<RoleManager>.Instance;
			if (string.IsNullOrEmpty(role.currentRoleId)) return null;
			var parts = new[] { Constants.LoginServerIP, Constants.LoginServerPort.ToString(), role.ServerId, role.UserId, role.currentRoleId };
			return feature + ".v1|" + string.Join("|", parts.Select(part => Uri.EscapeDataString(part ?? string.Empty)));
		}

		internal static RingSortPreferences Load(string key)
		{
			RingSortPreferences saved = null;
			if (key != null && PlayerPrefs.HasKey(key))
			{
				try { saved = JsonUtility.FromJson<RingSortPreferences>(PlayerPrefs.GetString(key)); }
				catch (ArgumentException) { }
			}
			var clean = new RingSortPreferences();
			var seen = new HashSet<RingSortField>();
			if (saved?.rules != null)
			{
				foreach (var rule in saved.rules.OrderByDescending(rule => rule?.selected ?? false))
					if (rule != null && Enum.IsDefined(typeof(RingSortField), rule.field) && seen.Add(rule.field)) clean.rules.Add(rule);
			}
			foreach (var field in Fields)
				if (seen.Add(field)) clean.rules.Add(new RingSortRule { field = field, descending = field != RingSortField.Name && field != RingSortField.Category });
			return clean;
		}

		internal RingSortPreferences Copy() => new RingSortPreferences { rules = rules.Select(rule => rule.Copy()).ToList() };

		internal void Save(string key)
		{
			if (key == null) return;
			PlayerPrefs.SetString(key, JsonUtility.ToJson(this));
			PlayerPrefs.Save();
		}

		internal void Toggle(RingSortRule rule)
		{
			rules.Remove(rule);
			rule.selected = !rule.selected;
			rules.Insert(rule.selected ? SelectedCount : rules.Count, rule);
		}

		internal void Move(RingSortRule rule, int delta)
		{
			int index = rules.IndexOf(rule), target = index + delta;
			if (!rule.selected || target < 0 || target >= SelectedCount) return;
			rules.RemoveAt(index);
			rules.Insert(target, rule);
		}

		internal IEnumerable<BaseNaJieGoodEntity> Order(IEnumerable<BaseNaJieGoodEntity> snapshot, bool guide)
		{
			if (!guide && SelectedCount == 0) return snapshot;
			// LINQ preserves the incoming inventory order whenever selected values tie.
			var sorted = snapshot.OrderByDescending(g => guide && PracticeGuideFlow.IsStarter(g));
			foreach (var rule in rules)
			{
				if (!rule.selected) continue;
				switch (rule.field)
				{
				case RingSortField.Rank: sorted = Then(sorted, g => g.Rank, rule); break;
				case RingSortField.Level: sorted = Then(sorted, g => g.Level, rule); break;
				case RingSortField.Category: sorted = Then(sorted, RingGoodsCatalog.Section, rule); break;
				case RingSortField.Quantity: sorted = Then(sorted, g => g.GoodsNum, rule); break;
				case RingSortField.Price: sorted = Then(sorted, g => g.RecyclePrice, rule); break;
				case RingSortField.Name: sorted = Then(sorted, g => g.Name, rule, NameOrder); break;
				}
			}
			return sorted;
		}

		private static IOrderedEnumerable<BaseNaJieGoodEntity> Then<T>(IOrderedEnumerable<BaseNaJieGoodEntity> sorted,
			Func<BaseNaJieGoodEntity, T> value, RingSortRule rule, IComparer<T> comparer = null)
		{
			return rule.descending ? sorted.ThenByDescending(value, comparer) : sorted.ThenBy(value, comparer);
		}
	}
}
