using System;
using UnityEngine;

namespace XiuZhenJie
{
    [Serializable]
    internal sealed class VoidRealmChoice
    {
        public string name, detail;
        public int[] values, targets;
    }

    [Serializable]
    internal sealed class VoidRealmHall
    {
        public string id, hall, job, work, glyph, room, tone, theme, constraint, routeLabel, adjustLabel;
        public int type;
        public string[] axes;
        public VoidRealmChoice[] routes, adjusts, briefs;
    }

    internal static class VoidRealmCatalog
    {
        [Serializable] private sealed class Book { public VoidRealmHall[] halls; }
        private static VoidRealmHall[] halls;
        internal static VoidRealmHall[] Halls
        {
            get
            {
                if (halls == null)
                {
                    var file = ReleasedResources.Load<TextAsset>("VoidRealm/halls");
                    halls = file == null ? Array.Empty<VoidRealmHall>() : JsonUtility.FromJson<Book>(file.text).halls;
                }
                return halls;
            }
        }
        internal static VoidRealmHall Find(string id) => Array.Find(Halls, h => h.id == id);

        private static readonly string[] Numbers = { "〇","一","二","三","四","五","六","七","八","九" };
        internal static string Stage(int n) => n > 0 && n < Numbers.Length ? Numbers[n] + "阶" : "未定阶";
        internal static string Rank(int n) => n > 0 && n < Numbers.Length ? Numbers[n] + "品" : "未入职";
        internal static string NameHint(string hall)
        {
            string ends;
            switch (hall)
            {
                case "dan": ends = "散药丹丸"; break;
                case "qi": ends = "爪斧弓剑靴钺衣裳戟琴印镯刀枪锤棒盾旗索矛珠幡氅绫鼎针钟铃尺甲"; break;
                case "dao": ends = "本诀谱录经功卷法书典"; break;
                case "shu": ends = "闪爆破斩劲杀轰劈吟击盾御体术印诀伤咒封解散毒瘴遁"; break;
                case "zhen": ends = "阵"; break;
                case "qiao": ends = "机轮锁匣弩傀儡枢"; break;
                case "shou": ends = "令契谱诀印"; break;
                case "zhi": ends = "竹芽叶兰果芝参根藤花枝木草子"; break;
                default: return "符书沿用所封法术的名称。";
            }
            return "题三至五个汉字，末字可用「" + ends + "」。";
        }
        internal static string SigilName(int brief) => new[] { "惊铃诀", "渡水盾", "换枢印" }[brief];
    }
}
