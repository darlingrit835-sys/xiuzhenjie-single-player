using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace XiuZhenJie
{
    // 导表时生成路径和兼容键。进界面时只加载当前要显示的 Sprite。
    internal static class CatalogIconArt
    {
        [Serializable]
        private class Icon
        {
            public int id;
            public int kind;
            public string path;
        }

        [Serializable]
        private class MethodEntry
        {
            public int id;
            public int icon;
            public string legacy;
        }

        [Serializable]
        private class MonsterEntry
        {
            public int id;
            public int icon;
            public int oldType;
            public int oldIcon;
        }

        [Serializable]
        private class Pool
        {
            public string key;
            public int kind;
            public int[] icons;
        }

        [Serializable]
        private class Index
        {
            public Icon[] icons;
            public MethodEntry[] methods;
            public MonsterEntry[] monsters;
            public Pool[] pools;
        }

        private const int MonsterKind = 2;
        private const int MethodKind = 3;
        private static readonly Dictionary<int, Icon> Icons = new Dictionary<int, Icon>();
        private static readonly Dictionary<int, int> Methods = new Dictionary<int, int>();
        private static readonly Dictionary<string, int> LegacyMethods = new Dictionary<string, int>(StringComparer.Ordinal);
        private static readonly Dictionary<int, int> Monsters = new Dictionary<int, int>();
        private static readonly Dictionary<long, int> LegacyMonsters = new Dictionary<long, int>();
        private static readonly Dictionary<string, Pool> Pools = new Dictionary<string, Pool>(StringComparer.Ordinal);
        private static readonly Dictionary<int, Sprite> Loaded = new Dictionary<int, Sprite>();

        static CatalogIconArt()
        {
            TextAsset asset = Resources.Load<TextAsset>("CatalogIcons/index");
            if (asset == null)
                return;
            Index index = JsonUtility.FromJson<Index>(asset.text);
            foreach (Icon icon in index.icons)
                Icons.Add(icon.id, icon);
            foreach (MethodEntry method in index.methods)
            {
                Methods.Add(method.id, method.icon);
                if (!string.IsNullOrEmpty(method.legacy))
                    LegacyMethods.Add(method.legacy, method.icon);
            }
            foreach (Pool pool in index.pools)
                Pools.Add(pool.key, pool);
            foreach (MonsterEntry monster in index.monsters)
            {
                Monsters.Add(monster.id, monster.icon);
                if (monster.oldType > 0 && monster.oldIcon > 0)
                    LegacyMonsters.Add(LegacyKey(monster.oldType, monster.oldIcon), monster.icon);
            }
        }

        private static long LegacyKey(int objectTypeId, int oldIconId)
        {
            return ((long)objectTypeId << 32) | (uint)oldIconId;
        }

        internal static Sprite MonsterFromLegacy(int objectTypeId, int oldIconId)
        {
            return LegacyMonsters.TryGetValue(LegacyKey(objectTypeId, oldIconId), out int icon) ? ById(MonsterKind, icon) : null;
        }

        internal static Sprite Monster(int templateId)
        {
            return Monsters.TryGetValue(templateId, out int icon) ? ById(MonsterKind, icon) : null;
        }

        internal static Sprite Method(int templateId)
        {
            return Methods.TryGetValue(templateId, out int icon) ? ById(MethodKind, icon) : null;
        }

        internal static Sprite Method(string templateOrLegacyId)
        {
            if (string.IsNullOrEmpty(templateOrLegacyId))
                return null;
            if (LegacyMethods.TryGetValue(templateOrLegacyId, out int icon))
                return ById(MethodKind, icon);
            return int.TryParse(templateOrLegacyId, NumberStyles.None, CultureInfo.InvariantCulture, out int id) ? Method(id) : null;
        }

        internal static Sprite RandomFromPool(string key)
        {
            if (!Pools.TryGetValue(key, out Pool pool) || pool.icons.Length == 0)
                return null;
            int iconId = pool.icons[UnityEngine.Random.Range(0, pool.icons.Length)];
            return ById(pool.kind, iconId);
        }

        internal static Sprite ById(int kindId, int iconId)
        {
            if (!Icons.TryGetValue(iconId, out Icon entry) || (entry.kind != 0 && entry.kind != kindId))
                return null;
            if (!Loaded.TryGetValue(iconId, out Sprite sprite) || sprite == null)
            {
                sprite = Resources.Load<Sprite>(entry.path);
                if (sprite != null)
                    Loaded[iconId] = sprite;
            }
            return sprite;
        }
    }
}
