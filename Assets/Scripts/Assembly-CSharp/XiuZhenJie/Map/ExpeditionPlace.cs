using UnityEngine;

namespace XiuZhenJie
{
    // A marker and its action sheet use the same decision. Browsing a forest does
    // not spend travel vigor, and the upper end of a range is not a travel lock.
    internal readonly struct ExpeditionPlace
    {
        internal readonly string Art, Kind, Status, Action, Detail;
        internal readonly bool Available, Here;

        private ExpeditionPlace(string art, string kind, string status, string action, string detail, bool available = true, bool here = false)
        {
            Art = art;
            Kind = kind;
            Status = status;
            Action = action;
            Detail = detail;
            Available = available;
            Here = here;
        }

        internal static int TravelVigor(int level) => Mathf.Max(0, WorldMapWnd.WAICHU_JINGLI - Mathf.Max(0, level - 11) * 2);
        internal static float TravelYears(int level) => Mathf.Max(0, WorldMapWnd.WAICHU_SHOUYUAN - Mathf.Max(0, level - 11) * .05f);
        internal static bool IsForest(MapEntity place) => place.Depth == 4 && place.MapType == (int)MapLevelType.MASSIF;

        internal static ExpeditionPlace Read(MapEntity place, RoleInfoEntity role, bool seekGuild)
        {
            var kind = (MapLevelType)place.MapType;
            if (IsForest(place))
                return new ExpeditionPlace("AtlasPines", "林地", seekGuild ? "可查访" : "可探索", seekGuild ? "查访山门" : "展开林地",
                    seekGuild ? "查阅这片林地的宗派与驻地。" : "可深入各处历练，也可查访当地山门。");
            if (place.Depth == 5)
            {
                string name = CSZ_CityItemPanel.LocalMapName(place);
                bool den = name.Contains("巢") || name.Contains("核心");
                bool outer = name.Contains("外围");
                string art = den ? "AtlasLair" : outer ? "AtlasPines" : "AtlasRavine";
                string terrain = den ? "核心地带" : outer ? "林缘" : name.Contains("深处") ? "林地腹地" : "历练地";
                if (kind == MapLevelType.MASSIF_INHERIT_SECRET) terrain = "传承秘境";
                if (kind == MapLevelType.MASSIF_GOODS_SECRET) terrain = "寻宝秘境";
                string detail = name.Contains("兽王巢") ? "巢穴中可能遭遇领主与精英妖兽。" :
                    outer ? "沿林口探路，留意游荡的精英妖兽。" :
                    name.Contains("深处") ? "深入山谷时可能遇到妖兽领主。" : "进入此地与妖兽交手。";
                return new ExpeditionPlace(art, terrain, place.IsCleanMap ? "可扫荡" : "可历练", "进入历练",
                    detail + $" 入场耗 {DungeonVigor(place.Id)} 点精力，首次历练免费。");
            }
            if (place.Depth == 4 && (kind == MapLevelType.CITY || kind == MapLevelType.CITY_LOW ||
                kind == MapLevelType.CITY_PREFECTURE || kind == MapLevelType.CITY_STATE || kind == MapLevelType.CITY_SUPER))
            {
                bool village = place.MapName != null && place.MapName.EndsWith("村", System.StringComparison.Ordinal);
                string art = village ? "AtlasVillage" : "AtlasCity";
                string type = village ? "村落" : "城池";
                if (role == null) return new ExpeditionPlace(art, type, "待确认", "暂不可去", "等待角色资料。", false);
                if (role.MapId == place.Id)
                    return new ExpeditionPlace(art, type, "所在地", "已在此地", "可在舆图上选择邻城或周边林地。", false, true);
                if (role.RoleLevel < place.MinLevel)
                    return new ExpeditionPlace(art, type, "未解锁", "境界不足", $"需达到 {place.MinLevel} 级，当前 {role.RoleLevel} 级。", false);
                int cost = TravelVigor(role.RoleLevel);
                if (role.CurrentVigor < cost)
                    return new ExpeditionPlace(art, type, "精力不足", "精力不足", $"迁行需 {cost} 点精力，当前 {role.CurrentVigor} 点。", false);
                return new ExpeditionPlace(art, type, "可前往", "前往此地", $"迁行耗 {cost} 点精力，年龄增加 {TravelYears(role.RoleLevel):0.##} 年。");
            }
            if (place.Depth >= 1 && place.Depth <= 3)
                return new ExpeditionPlace("FoldedMap", "地域", "可查看", "展开舆图", "查看辖下地点。");
            return new ExpeditionPlace("FoldedMap", "地点", "未开放", "暂不可去", "此处尚无历练入口。", false);
        }

        internal static int DungeonVigor(string mapId)
        {
            if (string.IsNullOrEmpty(mapId)) return 2;
            int mark = mapId.IndexOf('W');
            if (mark < 0 || mark + 3 != mapId.Length) return 2;
            return mapId[mark + 2] == '3' ? 6 : mapId[mark + 2] == '2' ? 4 : 2;
        }

        internal static string Range(MapEntity place)
        {
            int min = Mathf.Max(1, place.MinLevel);
            if (place.MaxLevel <= 0 && place.MinLevel <= 0) return "";
            return place.MaxLevel >= 999 || place.MaxLevel < min ? $"{min} 级起" :
                place.MaxLevel == min ? $"{min} 级" : $"{min}–{place.MaxLevel} 级";
        }
    }
}
