using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using static XiuZhenJie.AuctionHallState;

namespace XiuZhenJie
{
    internal static class AuctionGuide
    {
        [Serializable] private sealed class Catalogue { public Group[] groups; }
        [Serializable] private sealed class Group { public string key; public string[] lines; }
        private static readonly Regex Slots=new Regex(@"\{([^{}]*)\}");
        private static Dictionary<string,string[]> groups;

        private static Dictionary<string,string[]> Read()
        {
            var found=new Dictionary<string,string[]>(StringComparer.Ordinal);
            var file=ReleasedResources.Load<TextAsset>("AuctionHall/GuideLines");
            if (file==null) return found;
            try
            {
                var data=JsonUtility.FromJson<Catalogue>(file.text);
                foreach (var group in data?.groups??Array.Empty<Group>())
                {
                    if (group!=null && !string.IsNullOrEmpty(group.key) && group.lines?.Length>0)
                        found[group.key]=group.lines;
                }
            }
            catch (ArgumentException) { }
            return found;
        }

        internal static string Select(string key,string sessionKey,Lot lot,int limit,string fallback)
        {
            if (groups==null) groups=Read();
            if (!groups.TryGetValue(key,out var lines)) return fallback;
            uint seed=2166136261u;
            string origin=sessionKey+"|"+key+"|"+lot?.id;
            unchecked { foreach(char ch in origin) seed=(seed^ch)*16777619u; }
            for (int i=0;i<lines.Length;i++)
            {
                string line=lines[((int)(seed%(uint)lines.Length)+i)%lines.Length];
                if (string.IsNullOrWhiteSpace(line)) continue;
                bool complete=true;
                string text=Slots.Replace(line,m=>
                {
                    string v=m.Groups[1].Value switch
                    {
                        "name" => lot?.name,
                        "price" => lot?.price.ToString("N0",CultureInfo.InvariantCulture),
                        "leader" => lot?.leader,
                        "count" => lot?.count.ToString(CultureInfo.InvariantCulture),
                        "limit" => limit>0 ? limit.ToString(CultureInfo.InvariantCulture) : null,
                        _ => null
                    };
                    if (string.IsNullOrEmpty(v)) complete=false;
                    return v??"";
                });
                if (complete) return text;
            }
            return fallback;
        }
    }
}
