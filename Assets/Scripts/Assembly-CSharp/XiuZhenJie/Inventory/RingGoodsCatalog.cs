using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace XiuZhenJie
{
	// Wire types are kept on the goods for old game actions. A number of different
	// workshop materials share those types, so the ledger resolves a display entry per item.
	internal static class RingGoodsCatalog
	{
		private sealed class Entry
		{
			internal readonly int Section, Filter, IconType;
			internal readonly string Kind, Art, Fallback;

			internal Entry(int section, string kind, int filter, int iconType, string art = null, string fallback = null)
			{
				Section = section;
				Kind = kind;
				Filter = filter;
				IconType = iconType;
				Art = art;
				Fallback = fallback;
			}
		}

		private static readonly Dictionary<int, Entry> Types = new Dictionary<int, Entry>();
		private static readonly Dictionary<string, Entry> Templates = new Dictionary<string, Entry>(StringComparer.Ordinal);
		private static readonly Dictionary<string, Sprite> Original = new Dictionary<string, Sprite>(StringComparer.Ordinal);
		private static readonly Entry Unknown = new Entry(5, "奇物", -1, 0);
		private static readonly Entry Seed = new Entry(2, "灵种", -1, 0, "original/LingTian_ZhoZi");
		private static readonly Entry Robe = new Entry(1, "道袍", 26, 26);
		private static readonly Entry Cuirass = new Entry(1, "护甲", 26, 16);

		static RingGoodsCatalog()
		{
			void Type(int id, int section, string kind, int filter = 0, string art = null)
			{
				Types.Add(id, new Entry(section, kind, filter == 0 ? id : filter, id, art));
			}
			void Items(Entry item, params string[] ids)
			{
				foreach (var id in ids) Templates.Add(id, item);
			}

			Type(1, 3, "功法"); Type(2, 3, "丹方"); Type(3, 3, "阵图"); Type(4, 3, "器图");
			Type(5, 3, "符书"); Type(6, 3, "法术书");
			Type(7, 2, "兽材"); Type(8, 2, "皮毛"); Type(9, 2, "精血");
			Type(10, 2, "兽骨"); Type(11, 2, "妖丹"); Type(12, 2, "灵植"); Type(13, 2, "矿材");
			Type(14, 1, "法器"); Type(15, 1, "兵器"); Type(16, 1, "防具");
			Type(17, 3, "兵器图", 4); Type(18, 3, "防具图", 4);
			Type(19, 4, "丹药"); Type(20, 5, "阵盘"); Type(21, 4, "符箓");
			Type(22, 5, "符笔", 25); Type(23, 2, "符纸"); Type(24, 2, "符墨", 24, "original/LingTian_LingYe");
			Type(25, 5, "工具"); Type(26, 1, "衣甲"); Type(29, 1, "足具");
			Type(28, 5, "秘境"); Type(30, 5, "藏宝秘境", 28); Type(31, 5, "传承秘境", 28); Type(32, 5, "洞天福地");
			Type(33, 3, "道经残卷"); Type(34, 5, "悟道石"); Type(35, 2, "陨铁", 13);
			Type(36, 5, "灵傀"); Type(37, 5, "替身傀儡"); Type(38, 4, "好运符", 21);
			Type(39, 3, "德经"); Type(40, 2, "仙金", 13); Type(41, 3, "神书");
			foreach (var id in new[] { 90, 92, 93, 94, 95, 96, 97, 98 }) Type(id, 5, "贡献");
			Type(99, 5, "灵玉"); Type(100, 5, "灵石");

			// IDs and families are from cultivation_affinities, smithing, the garden and sigil supplies.
			Items(Types[12], "mat.ninglu_herb", "mat.wild_herb", "mat.dream_lotus", "mat.ember_moss", "mat.dune_thorn", "mat.frost_reed");
			Items(Types[13], "mat.chilie_sand", "mat.black_iron_ore", "mat.sunstone", "mat.dream_mica", "mat.wind_slate", "mat.frost_silver");
			Items(Types[35], "yun_tie_1");
			Items(new Entry(2, "金属", 13, 13), "mat.hanwen_iron", "made.frost_iron");
			Items(new Entry(2, "灵液", -1, 0, "original/LingTian_LingYe"), "mat.cold_dew");
			Items(new Entry(2, "木材", -1, 0, "Goods/Timber", "ElementMu"), "mat.cloudwood");
			Items(new Entry(2, "灵土", -1, 0, "original/LingTian_LingTian"), "preview-soil-1");
			Items(Types[7], "mat.thunder_horn", "mat.wind_sinew", "mat.river_scale", "mat.thunder_whisker", "mat.marsh_plate");
			Items(new Entry(2, "翎羽", 7, 7, "Goods/Plume"), "mat.gold_plume");
			Items(Types[8], "mat.common_hide", "mat.moon_fur");
			Items(Types[9], "mat.common_blood");
			Items(Types[10], "mat.common_bone", "mat.flame_fang", "mat.earth_claw");
			Items(Types[11], "mat.common_core");
			Items(Types[22], "obj.jinling_brush");
			Items(Types[23], "mat.shuanglu_paper");
			Items(new Entry(2, "符墨", 24, 24, "Goods/Inkstone", "original/LingTian_LingYe"), "mat.chenglu_ink");
			Items(Types[19], "obj.ninglu_pill", "obj.qingmai_pill", "obj.chiyang_pill", "obj.zhenrang_pill", "obj.shuangmai_pill", "sys_zhu_ji_dan_huang", "pill_naqi", "pill_huichun");
			Items(Types[20], "preview-matrix");
			Items(Types[21], "preview-sign");
			Items(new Entry(1, "护心镜", 16, 16, "Goods/Compass"), "obj.hanguang_mirror", "eq_armor_xuantie");
			Items(Robe, "eq_armor_qingbu");
			Items(new Entry(1, "法衣", 26, 26), "obj.yunwen_robe");
			Items(new Entry(1, "灵靴", 29, 29), "obj.tafeng_boots");
			Items(Types[29], "eq_boots_qingbu");
			Items(new Entry(1, "剑", 15, 15, "Goods/Sword"), "made.frost_sword", "wu_xing_ju_ling_jian", "eq_weapon_qingfeng");
			Items(new Entry(1, "刀", 15, 15, "Goods/Saber"), "wp.duanxia_saber");
			Items(new Entry(1, "枪", 15, 15, "Goods/Spear"), "wp.minglei_spear");
			Items(new Entry(1, "杖", 15, 15, "Goods/Staff"), "wp.tingquan_staff");
			Items(new Entry(1, "弓", 15, 15, "Goods/Bow"), "wp.zhufeng_bow");
		}

		private static Entry Legacy(int type) => Types.TryGetValue(type, out var entry) ? entry : Unknown;

		private static Entry Resolve(BaseNaJieGoodEntity g)
		{
			if (g == null) return Unknown;
			// Workshop molds live in ForgeMolds, outside inventory. Their issued weapons
			// carry a BlankId. A learned diagram or manuscript never becomes a weapon here.
			if (Legacy(g.Type).Section == 1 && !string.IsNullOrEmpty(g.BlankId) && Templates.TryGetValue(g.BlankId, out var blank) && blank.Section == 1)
				return blank;
			if (g.Type == 4 || g.Type == 17 || g.Type == 18) return Legacy(g.Type);
			if (string.IsNullOrEmpty(g.TemplateId)) return Legacy(g.Type);
			// Dungeon drops share a template across names. Older saves also used
			// type 16 for these body-slot garments, now emitted as type 26.
			if ((g.Type == 16 || g.Type == 26) && (g.TemplateId.StartsWith("drop_16_", StringComparison.Ordinal) || g.TemplateId.StartsWith("drop_26_", StringComparison.Ordinal)))
			{
				switch (g.Name)
				{
					case "青布道袍": case "流云袍": case "太虚道袍": return Robe;
					case "玄铁护甲": case "玄冰甲": return Cuirass;
					case "赤霞衣": return Types[26];
				}
			}
			if (g.TemplateId == "preview-seed-1")
			{
				// The original trial harvest reused its seed ID for the crop.
				if (g.Name == "【试用】青芽") return Types[12];
				if (g.Name == "【试用】青芽种子") return Seed;
			}
			return Templates.TryGetValue(g.TemplateId, out var item) ? item : Legacy(g.Type);
		}

		internal static int Section(BaseNaJieGoodEntity g) => Resolve(g).Section;
		internal static int Section(int type) => Legacy(type).Section;
		internal static string Kind(BaseNaJieGoodEntity g) => Resolve(g).Kind;
		internal static string Kind(int type) => Legacy(type).Kind;

		internal static bool MatchesLegacyFilter(BaseNaJieGoodEntity g, string tag, string label)
		{
			var entry = Resolve(g);
			if (tag == "kind") return entry.Kind == label;
			if (string.IsNullOrEmpty(tag))
			{
				if (label == "法器") return entry.Section == 1;
				if (label == "动物") return entry.Filter >= 7 && entry.Filter <= 11;
				return true;
			}
			return int.TryParse(tag, out var type) && entry.Filter == type;
		}

		internal static bool IsHarvestablePlant(BaseNaJieGoodEntity g)
		{
			// The new garden keeps seed in the mature plot. Its harvested goods and
			// workshop supplies do not use the original bag-to-seed protocol.
			return g != null && g.Type == 12 &&
				(string.IsNullOrEmpty(g.TemplateId) || (g.TemplateId != "preview-seed-1" && !Templates.ContainsKey(g.TemplateId)));
		}

		internal static bool AddInstanceMaterialDetails(BaseNaJieGoodEntity g, List<string> rows)
		{
			if (g == null || g.Type != 12) return false;
			var item = Resolve(g);
			if (item.Section != 2 || item == Types[12]) return false;
			if (g.FiveElement >= 1 && g.FiveElement <= 5) rows.Add("五行," + g.GetFiveElementName());
			if (g.ConversionRate > 0) rows.Add("灵衍值," + g.ConversionRate.ToString("0.###", CultureInfo.InvariantCulture));
			if (g.ContentList == null) return true;
			// A cached legacy plant panel can have placeholders for all its missing
			// fields. Keep authored details such as an actual source, a unit or a trait.
			foreach (var line in g.ContentList)
			{
				if (string.IsNullOrEmpty(line)) continue;
				var parts = line.Split(new[] { ',' }, 3);
				if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1])) continue;
				switch (parts[0])
				{
					case "五行": case "灵衍值": case "品阶": case "等阶": continue;
					case "坚度": case "韧度": if (parts[1] == "无") continue; break;
					case "产地": if (parts[1] == "虚神界") continue; break;
					case "重量": case "导灵性": if (parts[1] == "0") continue; break;
				}
				rows.Add(line);
			}
			return true;
		}

		private static Sprite SpriteFor(string path)
		{
			if (path == null) return null;
			const string prefix = "original/";
			if (!path.StartsWith(prefix, StringComparison.Ordinal)) return InkInterfaceArt.Get(path);
			if (!Original.TryGetValue(path, out var sprite))
			{
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/" + path.Substring(prefix.Length) + ".png");
				Original[path] = sprite;
			}
			return sprite;
		}

		internal static Sprite Artwork(BaseNaJieGoodEntity g)
		{
			var entry = Resolve(g);
			var art = SpriteFor(entry.Art);
			if (art == null) art = SpriteFor(entry.Fallback);
			if (art != null) return art;
			var icons = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			int type = entry == Unknown && g != null ? g.Type : entry.IconType;
			return icons.TryGetValue(type, out var icon) && icon != null ? icon : InkInterfaceArt.Get("Satchel");
		}

		internal static Sprite SectionArtwork(int section)
		{
			switch (section)
			{
				case 1: return InkInterfaceArt.Get("Goods/Sword");
				case 2: return InkInterfaceArt.Get("Goods/Ore");
				case 3: return InkInterfaceArt.Get("Goods/Manual");
				case 4: return InkInterfaceArt.Get("Goods/Talisman");
				case 5: return SpriteFor("original/Goods_WuDaoShi");
				default: return InkInterfaceArt.Get("Satchel");
			}
		}

		// These original sheets use a white glyph and a transparent background.
		// Inventory paper needs dark ink. Painted sprites already contain their colors.
		internal static Color ArtTint(Sprite sprite)
		{
			switch (sprite != null ? sprite.name : "")
			{
				case "LingTian_LingYe": case "LingTian_ZhoZi": case "LingTian_LingTian":
				case "Goods_MiJing": case "Goods_WuDaoShi": case "Goods_LingKui": case "Goods_TiShenKuiLei":
					return InkInterfaceArt.Ink;
				default: return Color.white;
			}
		}
	}
}
