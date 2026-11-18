using UnityEngine;

namespace XiuZhenJie
{
	public class WorldSetting
	{
		public static string simplifyNumber(ulong number)
		{
			if (number >= 100000 && number < 1000000000)
			{
				return number / 10000 + "万";
			}
			if (number >= 1000000000 && number < 10000000000000L)
			{
				return number / 100000000 + "亿";
			}
			if (number >= 10000000000000L)
			{
				return number / 1000000000000L + "兆";
			}
			return number.ToString();
		}

		public static Color GetLevelColorByIndex(int LevelIndex)
		{
			Color result = Color.white;
			switch (LevelIndex)
			{
			case 1:
			case 2:
			case 3:
			case 4:
				result = new Color(0f, 0f, 0f, 255f);
				break;
			case 5:
			case 6:
			case 7:
			case 8:
				result = new Color(127f, 210f, 49f, 255f);
				break;
			case 9:
			case 10:
			case 11:
			case 12:
				result = new Color(163f, 53f, 237f, 1f);
				break;
			case 13:
			case 14:
			case 15:
			case 16:
				result = new Color(255f, 128f, 1f, 255f);
				break;
			}
			return result;
		}

		public static Color WuXingColor(WuXing wuXing)
		{
			Color result = Color.black;
			switch (wuXing)
			{
			case WuXing.All:
				return Color.black;
			case WuXing.Jin:
				result = new Color(0.7568628f, 0.7843138f, 208f / 255f, 1f);
				break;
			case WuXing.Mu:
				result = new Color(0.1921569f, 0.3921569f, 96f / 255f, 1f);
				break;
			case WuXing.Shui:
				result = new Color(1f / 15f, 48f / 255f, 71f / 255f, 1f);
				break;
			case WuXing.Huo:
				result = new Color(105f / 255f, 1f / 15f, 14f / 255f, 1f);
				break;
			case WuXing.Tu:
				result = new Color(0.8705883f, 176f / 255f, 0.4196079f, 1f);
				break;
			}
			return result;
		}

		public static int GetLevelByRank(int rank)
		{
			int result = 1;
			switch (rank)
			{
			case 1:
			case 2:
			case 3:
			case 4:
				result = 1;
				break;
			case 5:
			case 6:
			case 7:
			case 8:
				result = 2;
				break;
			case 9:
			case 10:
			case 11:
			case 12:
				result = 3;
				break;
			case 13:
			case 14:
			case 15:
			case 16:
				result = 4;
				break;
			}
			return result;
		}

		public static string GetLevelByRankName(int rank)
		{
			string text = "黄阶";
			switch (rank)
			{
			case 1:
			case 2:
			case 3:
			case 4:
				return "黄阶";
			case 5:
			case 6:
			case 7:
			case 8:
				return "玄阶";
			case 9:
			case 10:
			case 11:
			case 12:
				return "地阶";
			case 13:
			case 14:
			case 15:
			case 16:
				return "天阶";
			default:
				return "无";
			}
		}

		public static string GetRankAllName(int rank)
		{
			string result = "无";
			switch (rank)
			{
			case 1:
				result = "黄阶下品";
				break;
			case 2:
				result = "黄阶中品";
				break;
			case 3:
				result = "黄阶上品";
				break;
			case 4:
				result = "黄阶极品";
				break;
			case 5:
				result = "玄阶下品";
				break;
			case 6:
				result = "玄阶中品";
				break;
			case 7:
				result = "玄阶上品";
				break;
			case 8:
				result = "玄阶极品";
				break;
			case 9:
				result = "地阶下品";
				break;
			case 10:
				result = "地阶中品";
				break;
			case 11:
				result = "地阶上品";
				break;
			case 12:
				result = "地阶极品";
				break;
			case 13:
				result = "天阶下品";
				break;
			case 14:
				result = "天阶中品";
				break;
			case 15:
				result = "天阶上品";
				break;
			case 16:
				result = "天阶极品";
				break;
			}
			return result;
		}

		public static int GetWuPinType(int _type)
		{
			int result = 0;
			switch (_type)
			{
			case 1:
				result = 2;
				break;
			case 2:
				result = 2;
				break;
			case 3:
				result = 2;
				break;
			case 4:
				result = 2;
				break;
			case 5:
				result = 2;
				break;
			case 6:
				result = 2;
				break;
			case 7:
				result = 3;
				break;
			case 8:
				result = 3;
				break;
			case 9:
				result = 3;
				break;
			case 10:
				result = 3;
				break;
			case 11:
				result = 3;
				break;
			case 12:
				result = 3;
				break;
			case 13:
				result = 3;
				break;
			case 14:
				result = 1;
				break;
			case 15:
				result = 1;
				break;
			case 16:
				result = 1;
				break;
			case 17:
				result = 4;
				break;
			case 18:
				result = 4;
				break;
			case 19:
				result = 4;
				break;
			case 20:
				result = 4;
				break;
			case 21:
				result = 4;
				break;
			case 22:
				result = 4;
				break;
			case 23:
				result = 4;
				break;
			case 24:
				result = 4;
				break;
			case 25:
				result = 4;
				break;
			case 26:
				result = 1;
				break;
			}
			return result;
		}

		public static string GetWuPinIsCanUse(int _type)
		{
			string result = "";
			switch (_type)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				result = "学习";
				break;
			case 12:
				result = "采集";
				break;
			case 19:
				result = "使用";
				break;
			case 15:
			case 26:
				result = "法器";
				break;
			case 16:
				result = "穿戴";
				break;
			case 20:
			case 28:
			case 30:
			case 31:
			case 32:
			case 38:
				result = "使用";
				break;
			case 33:
			case 34:
			case 35:
			case 39:
			case 40:
			case 41:
				result = "合成";
				break;
			}
			return result;
		}

		public static int GetWuXingComplement(WuXing SourceWuXin, WuXing FromWuXing)
		{
			switch (SourceWuXin)
			{
			case WuXing.Jin:
				switch (FromWuXing)
				{
				case WuXing.Huo:
					return 1;
				case WuXing.Mu:
					return 2;
				}
				break;
			case WuXing.Mu:
				switch (FromWuXing)
				{
				case WuXing.Jin:
					return 1;
				case WuXing.Tu:
					return 2;
				}
				break;
			case WuXing.Shui:
				switch (FromWuXing)
				{
				case WuXing.Tu:
					return 1;
				case WuXing.Huo:
					return 2;
				}
				break;
			case WuXing.Huo:
				switch (FromWuXing)
				{
				case WuXing.Shui:
					return 1;
				case WuXing.Jin:
					return 2;
				}
				break;
			case WuXing.Tu:
				switch (FromWuXing)
				{
				case WuXing.Mu:
					return 1;
				case WuXing.Shui:
					return 2;
				}
				break;
			default:
				return 0;
			}
			return 0;
		}

		public static int GetWuXingGenerating(WuXing SourceWuXin, WuXing FromWuXing)
		{
			switch (SourceWuXin)
			{
			case WuXing.Jin:
				switch (FromWuXing)
				{
				case WuXing.Huo:
					return 2;
				case WuXing.Shui:
					return 1;
				}
				break;
			case WuXing.Mu:
				switch (FromWuXing)
				{
				case WuXing.Jin:
					return 2;
				case WuXing.Huo:
					return 1;
				}
				break;
			case WuXing.Shui:
				switch (FromWuXing)
				{
				case WuXing.Tu:
					return 2;
				case WuXing.Mu:
					return 1;
				}
				break;
			case WuXing.Huo:
				switch (FromWuXing)
				{
				case WuXing.Shui:
					return 2;
				case WuXing.Tu:
					return 1;
				}
				break;
			case WuXing.Tu:
				switch (FromWuXing)
				{
				case WuXing.Mu:
					return 2;
				case WuXing.Jin:
					return 1;
				}
				break;
			default:
				return 0;
			}
			return 0;
		}

		public static int GetWuXingAdd(WuXing SourceWuXin, WuXing TargetWuXing)
		{
			switch (SourceWuXin)
			{
			case WuXing.Jin:
				switch (TargetWuXing)
				{
				case WuXing.Huo:
					return 2;
				case WuXing.Jin:
					return 1;
				}
				break;
			case WuXing.Mu:
				switch (TargetWuXing)
				{
				case WuXing.Jin:
					return 2;
				case WuXing.Mu:
					return 1;
				}
				break;
			case WuXing.Shui:
				switch (TargetWuXing)
				{
				case WuXing.Tu:
					return 2;
				case WuXing.Shui:
					return 1;
				}
				break;
			case WuXing.Huo:
				switch (TargetWuXing)
				{
				case WuXing.Shui:
					return 2;
				case WuXing.Huo:
					return 1;
				}
				break;
			case WuXing.Tu:
				switch (TargetWuXing)
				{
				case WuXing.Mu:
					return 2;
				case WuXing.Tu:
					return 1;
				}
				break;
			default:
				return 0;
			}
			return 0;
		}

		public static string GetWuXingName(WuXing wuXing)
		{
			return wuXing switch
			{
				WuXing.All => "其它", 
				WuXing.Jin => "金", 
				WuXing.Mu => "木", 
				WuXing.Shui => "水", 
				WuXing.Huo => "火", 
				WuXing.Tu => "土", 
				_ => "其它", 
			};
		}

		public static string GetSignleWuXingName(WuXing wuXing)
		{
			return wuXing switch
			{
				WuXing.All => "五", 
				WuXing.Jin => "金", 
				WuXing.Mu => "木", 
				WuXing.Shui => "水", 
				WuXing.Huo => "火", 
				WuXing.Tu => "土", 
				_ => "五", 
			};
		}

		public static string GetStageName(int stage)
		{
			return stage switch
			{
				1 => "一阶", 
				2 => "二阶", 
				3 => "三阶", 
				4 => "四阶", 
				5 => "五阶", 
				6 => "六阶", 
				7 => "七阶", 
				8 => "八阶", 
				9 => "九阶", 
				_ => "无", 
			};
		}

		public static string GetStageChineseName(int stage)
		{
			return stage switch
			{
				1 => "练气", 
				2 => "筑基", 
				3 => "结丹", 
				4 => "元婴", 
				5 => "分神", 
				6 => "炼虚", 
				7 => "合体", 
				8 => "渡劫", 
				9 => "大乘", 
				_ => "无", 
			};
		}

		public static string GetGongFaEffectValuetType(int effectValueType)
		{
			return (GongFaEffectValueEnum)effectValueType switch
			{
				GongFaEffectValueEnum.JIN_SKILL_HARM_10 => "增加金系法术伤害10%", 
				GongFaEffectValueEnum.JIN_SKILL_HARM_20 => "增加金系法术伤害20%", 
				GongFaEffectValueEnum.JIN_SKILL_HARM_25 => "增加金系法术伤害25%", 
				GongFaEffectValueEnum.MU_SKILL_HARM_10 => "增加木系法术伤害10%", 
				GongFaEffectValueEnum.MU_SKILL_HARM_20 => "增加木系法术伤害20%", 
				GongFaEffectValueEnum.MU_SKILL_HARM_25 => "增加木系法术伤害25%", 
				GongFaEffectValueEnum.SHUI_SKILL_HARM_10 => "增加水系法术伤害10%", 
				GongFaEffectValueEnum.SHUI_SKILL_HARM_20 => "增加水系法术伤害20%", 
				GongFaEffectValueEnum.SHUI_SKILL_HARM_25 => "增加水系法术伤害25%", 
				GongFaEffectValueEnum.HUO_SKILL_HARM_10 => "增加火系法术伤害10%", 
				GongFaEffectValueEnum.HUO_SKILL_HARM_20 => "增加火系法术伤害20%", 
				GongFaEffectValueEnum.HUO_SKILL_HARM_25 => "增加火系法术伤害25%", 
				GongFaEffectValueEnum.TU_SKILL_HARM_10 => "增加土系法术伤害10%", 
				GongFaEffectValueEnum.TU_SKILL_HARM_20 => "增加土系法术伤害20%", 
				GongFaEffectValueEnum.TU_SKILL_HARM_25 => "增加土系法术伤害25%", 
				GongFaEffectValueEnum.JIN_EQUIPMENT_HARM_10 => "增加金系法器伤害10%", 
				GongFaEffectValueEnum.JIN_EQUIPMENT_HARM_20 => "增加金系法器伤害20%", 
				GongFaEffectValueEnum.JIN_EQUIPMENT_HARM_25 => "增加金系法器伤害25%", 
				GongFaEffectValueEnum.MU_EQUIPMENT_HARM_10 => "增加木系法器伤害10%", 
				GongFaEffectValueEnum.MU_EQUIPMENT_HARM_20 => "增加木系法器伤害20%", 
				GongFaEffectValueEnum.MU_EQUIPMENT_HARM_25 => "增加木系法器伤害25%", 
				GongFaEffectValueEnum.SHUI_EQUIPMENT_HARM_10 => "增加水系法器伤害10%", 
				GongFaEffectValueEnum.SHUI_EQUIPMENT_HARM_20 => "增加水系法器伤害20%", 
				GongFaEffectValueEnum.SHUI_EQUIPMENT_HARM_25 => "增加水系法器伤害25%", 
				GongFaEffectValueEnum.HUO_EQUIPMENT_HARM_10 => "增加火系法器伤害10%", 
				GongFaEffectValueEnum.HUO_EQUIPMENT_HARM_20 => "增加火系法器伤害20%", 
				GongFaEffectValueEnum.HUO_EQUIPMENT_HARM_25 => "增加火系法器伤害25%", 
				GongFaEffectValueEnum.TU_EQUIPMENT_HARM_10 => "增加土系法器伤害10%", 
				GongFaEffectValueEnum.TU_EQUIPMENT_HARM_20 => "增加土系法器伤害20%", 
				GongFaEffectValueEnum.TU_EQUIPMENT_HARM_25 => "增加土系法器伤害25%", 
				GongFaEffectValueEnum.JIN_SIGN_HARM_10 => "增加金系符箓伤害10%", 
				GongFaEffectValueEnum.JIN_SIGN_HARM_20 => "增加金系符箓伤害20%", 
				GongFaEffectValueEnum.JIN_SIGN_HARM_25 => "增加金系符箓伤害25%", 
				GongFaEffectValueEnum.MU_SIGN_HARM_10 => "增加木系符箓伤害10%", 
				GongFaEffectValueEnum.MU_SIGN_HARM_20 => "增加木系符箓伤害20%", 
				GongFaEffectValueEnum.MU_SIGN_HARM_25 => "增加木系符箓伤害25%", 
				GongFaEffectValueEnum.SHUI_SIGN_HARM_10 => "增加水系符箓伤害10%", 
				GongFaEffectValueEnum.SHUI_SIGN_HARM_20 => "增加水系符箓伤害20%", 
				GongFaEffectValueEnum.SHUI_SIGN_HARM_25 => "增加水系符箓伤害25%", 
				GongFaEffectValueEnum.HUO_SIGN_HARM_10 => "增加火系符箓伤害10%", 
				GongFaEffectValueEnum.HUO_SIGN_HARM_20 => "增加火系符箓伤害20%", 
				GongFaEffectValueEnum.HUO_SIGN_HARM_25 => "增加火系符箓伤害25%", 
				GongFaEffectValueEnum.TU_SIGN_HARM_10 => "增加土系符箓伤害10%", 
				GongFaEffectValueEnum.TU_SIGN_HARM_20 => "增加土系符箓伤害20%", 
				GongFaEffectValueEnum.TU_SIGN_HARM_25 => "增加土系符箓伤害25%", 
				GongFaEffectValueEnum.JIN_ENTRENCH_10 => "降低金系伤害10%", 
				GongFaEffectValueEnum.JIN_ENTRENCH_20 => "降低金系伤害20%", 
				GongFaEffectValueEnum.JIN_ENTRENCH_25 => "降低金系伤害25%", 
				GongFaEffectValueEnum.MU_ENTRENCH_10 => "降低木系伤害10%", 
				GongFaEffectValueEnum.MU_ENTRENCH_20 => "降低木系伤害20%", 
				GongFaEffectValueEnum.MU_ENTRENCH_25 => "降低木系伤害25%", 
				GongFaEffectValueEnum.SHUI_ENTRENCH_10 => "降低水系伤害10%", 
				GongFaEffectValueEnum.SHUI_ENTRENCH_20 => "降低水系伤害20%", 
				GongFaEffectValueEnum.SHUI_ENTRENCH_25 => "降低水系伤害25%", 
				GongFaEffectValueEnum.HUO_ENTRENCH_10 => "降低火系伤害10%", 
				GongFaEffectValueEnum.HUO_ENTRENCH_20 => "降低火系伤害20%", 
				GongFaEffectValueEnum.HUO_ENTRENCH_25 => "降低火系伤害25%", 
				GongFaEffectValueEnum.TU_ENTRENCH_10 => "降低土系伤害10%", 
				GongFaEffectValueEnum.TU_ENTRENCH_20 => "降低土系伤害20%", 
				GongFaEffectValueEnum.TU_ENTRENCH_25 => "降低土系伤害25%", 
				GongFaEffectValueEnum.JIN_AIR_SPEED_10 => "增加金系真气输出效率10%", 
				GongFaEffectValueEnum.JIN_AIR_SPEED_20 => "增加金系真气输出效率20%", 
				GongFaEffectValueEnum.JIN_AIR_SPEED_25 => "增加金系真气输出效率25%", 
				GongFaEffectValueEnum.MU_AIR_SPEED_10 => "增加木系真气输出效率10%", 
				GongFaEffectValueEnum.MU_AIR_SPEED_20 => "增加木系真气输出效率20%", 
				GongFaEffectValueEnum.MU_AIR_SPEED_25 => "增加木系真气输出效率25%", 
				GongFaEffectValueEnum.SHUI_AIR_SPEED_10 => "增加水系真气输出效率10%", 
				GongFaEffectValueEnum.SHUI_AIR_SPEED_20 => "增加水系真气输出效率20%", 
				GongFaEffectValueEnum.SHUI_AIR_SPEED_25 => "增加水系真气输出效率25%", 
				GongFaEffectValueEnum.HUO_AIR_SPEED_10 => "增加火系真气输出效率10%", 
				GongFaEffectValueEnum.HUO_AIR_SPEED_20 => "增加火系真气输出效率20%", 
				GongFaEffectValueEnum.HUO_AIR_SPEED_25 => "增加火系真气输出效率25%", 
				GongFaEffectValueEnum.TU_AIR_SPEED_10 => "增加土系真气输出效率10%", 
				GongFaEffectValueEnum.TU_AIR_SPEED_20 => "增加土系真气输出效率20%", 
				GongFaEffectValueEnum.TU_AIR_SPEED_25 => "增加土系真气输出效率25%", 
				GongFaEffectValueEnum.UNREAL_CONSUME_5 => "降低神识消耗5%", 
				GongFaEffectValueEnum.UNREAL_CONSUME_15 => "降低神识消耗15%", 
				GongFaEffectValueEnum.UNREAL_CONSUME_20 => "降低神识消耗20%", 
				GongFaEffectValueEnum.EXPLORE_ADD_UNREAL_SCOPE_10 => "增加神识覆盖范围10%", 
				GongFaEffectValueEnum.EXPLORE_ADD_UNREAL_SCOPE_20 => "增加神识覆盖范围20%", 
				GongFaEffectValueEnum.EXPLORE_ADD_UNREAL_SCOPE_25 => "增加神识覆盖范围25%", 
				GongFaEffectValueEnum.ENTRENCH_LOWER_SKILL_10 => "降低最终承受伤害10%", 
				GongFaEffectValueEnum.ENTRENCH_LOWER_SKILL_20 => "降低最终承受伤害20%", 
				GongFaEffectValueEnum.ENTRENCH_LOWER_SKILL_25 => "降低最终承受伤害25%", 
				GongFaEffectValueEnum.THUNDER_LOWER_SKILL_HARM_10 => "降低雷劫伤害10%", 
				GongFaEffectValueEnum.THUNDER_LOWER_SKILL_HARM_20 => "降低雷劫伤害20%", 
				GongFaEffectValueEnum.THUNDER_LOWER_SKILL_HARM_25 => "降低雷劫伤害25%", 
				GongFaEffectValueEnum.THUNDER_LOWER_DODGE_TIME_2 => "降低雷劫闪躲冷却时间2秒", 
				GongFaEffectValueEnum.DETOXIFY_EFFECT_1 => "每2小时降低1点丹毒", 
				GongFaEffectValueEnum.DETOXIFY_EFFECT_2 => "每2小时降低2点丹毒", 
				GongFaEffectValueEnum.DETOXIFY_EFFECT_3 => "每2小时降低3点丹毒", 
				GongFaEffectValueEnum.FATE_ADD_DROP_10 => "增加扫荡地图掉落几率10%", 
				GongFaEffectValueEnum.FATE_ADD_DROP_15 => "增加扫荡地图掉落几率15%", 
				GongFaEffectValueEnum.FATE_ADD_DROP_5 => "增加扫荡地图掉落几率5%", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_NUM_1 => "炼丹增加成丹数量1个", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_NUM_2 => "炼丹增加成丹数量2个", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_NUM_3 => "炼丹增加成丹数量3个", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_NUM_5 => "炼丹增加成丹数量5个", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_RANK_1 => "炼丹提升丹药品阶1阶", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_RANK_2 => "炼丹提升丹药品阶2阶", 
				GongFaEffectValueEnum.PROFESSION_ELIXIR_ADD_RANK_3 => "炼丹提升丹药品阶3阶", 
				GongFaEffectValueEnum.PROFESSION_EQUIPMENT_ADD_RANK_1 => "炼器提升法器品阶1阶", 
				GongFaEffectValueEnum.PROFESSION_EQUIPMENT_ADD_RANK_2 => "炼器提升法器品阶2阶", 
				GongFaEffectValueEnum.PROFESSION_EQUIPMENT_ADD_RANK_3 => "炼器提升法器品阶3阶", 
				GongFaEffectValueEnum.PROFESSION_SIGN_ADD_NUM_1 => "制符增加符箓可使用1次", 
				GongFaEffectValueEnum.PROFESSION_SIGN_ADD_NUM_2 => "制符增加符箓可使用2次", 
				GongFaEffectValueEnum.PROFESSION_SIGN_ADD_NUM_3 => "制符增加符箓可使用3次", 
				GongFaEffectValueEnum.PROFESSION_SIGN_ADD_NUM_4 => "制符增加符箓可使用4次", 
				_ => null, 
			};
		}

		public static string FaQiXiLianType(int effectType, ref string title)
		{
			switch ((ZhuangBeiEffectEnum)effectType)
			{
			case ZhuangBeiEffectEnum.JIN_ATTACK_HARM:
				title = "攻击";
				return "对金系敌人伤害增加";
			case ZhuangBeiEffectEnum.MU_ATTACK_HARM:
				title = "攻击";
				return " 对木系敌人伤害增加";
			case ZhuangBeiEffectEnum.SHUI_ATTACK_HARM:
				title = "攻击";
				return "对水系敌人伤害增加";
			case ZhuangBeiEffectEnum.HUO_ATTACK_HARM:
				title = "攻击";
				return "对火系敌人伤害增加";
			case ZhuangBeiEffectEnum.TU_ATTACK_HARM:
				title = "攻击";
				return "对土系敌人伤害增加";
			case ZhuangBeiEffectEnum.ALL_ATTACK_HARM:
				title = "攻击";
				return "全系伤害提升";
			case ZhuangBeiEffectEnum.TU_ATTACK_SPEED:
				title = "攻击";
				return "攻击速度提升";
			case ZhuangBeiEffectEnum.ATTACK_WOOD_SIGN_EFFECT:
				title = "防护";
				return "受到金系伤害降低 ";
			case ZhuangBeiEffectEnum.MU_ENTRENCH:
				title = "防护";
				return "受到木系伤害降低";
			case ZhuangBeiEffectEnum.SHUI_ENTRENCH:
				title = "防护";
				return "受到水系伤害降低";
			case ZhuangBeiEffectEnum.HUO_ENTRENCH:
				title = "防护";
				return "受到火系伤害降低";
			case ZhuangBeiEffectEnum.TU_ENTRENCH:
				title = "防护";
				return "受到土系伤害降低";
			case ZhuangBeiEffectEnum.ALL_ENTRENCH:
				title = "防护";
				return "全系防护提升";
			case ZhuangBeiEffectEnum.JI_XING:
				title = "疾行";
				return "疾行";
			default:
				return "";
			}
		}

		public static string GetGongFaEffectType(int effectType, ref string title)
		{
			switch ((GongFaEffectEnum)effectType)
			{
			case GongFaEffectEnum.ATTACK_GOLD_SKILL_EFFECT:
				title = "道术";
				return "对金系敌人法术伤害增加";
			case GongFaEffectEnum.ATTACK_WOOD_SKILL_EFFECT:
				title = "道术";
				return "对木系敌人法术伤害增加";
			case GongFaEffectEnum.ATTACK_WATER_SKILL_EFFECT:
				title = "道术";
				return "对水系敌人法术伤害增加";
			case GongFaEffectEnum.ATTACK_FIRE_SKILL_EFFECT:
				title = "道术";
				return "对火系敌人法术伤害增加";
			case GongFaEffectEnum.ATTACK_EARTH_SKILL_EFFECT:
				title = "道术";
				return "对土系敌人法术伤害增加";
			case GongFaEffectEnum.ATTACK_GOLD_EQUIPMENT_EFFECT:
				title = "器魂";
				return "对金系敌人法器伤害增加";
			case GongFaEffectEnum.ATTACK_WOOD_EQUIPMENT_EFFECT:
				title = "器魂";
				return "对木系敌人法器伤害增加";
			case GongFaEffectEnum.ATTACK_WATER_EQUIPMENT_EFFECT:
				title = "器魂";
				return "对水系敌人法器伤害增加";
			case GongFaEffectEnum.ATTACK_FIRE_EQUIPMENT_EFFECT:
				title = "器魂";
				return "对火系敌人法器伤害增加";
			case GongFaEffectEnum.ATTACK_EARTH_EQUIPMENT_EFFECT:
				title = "器魂";
				return "对土系敌人法器伤害增加";
			case GongFaEffectEnum.ATTACK_GOLD_SIGN_EFFECT:
				title = "神符";
				return "对金系敌人符箓伤害增加";
			case GongFaEffectEnum.ATTACK_WOOD_SIGN_EFFECT:
				title = "神符";
				return "对木系敌人符箓伤害增加";
			case GongFaEffectEnum.ATTACK_WATER_SIGN_EFFECT:
				title = "神符";
				return "对水系敌人符箓伤害增加";
			case GongFaEffectEnum.ATTACK_FIRE_SIGN_EFFECT:
				title = "神符";
				return "对火系敌人符箓伤害增加";
			case GongFaEffectEnum.ATTACK_EARTH_SIGN_EFFECT:
				title = "神符";
				return "对土系敌人符箓伤害增加";
			case GongFaEffectEnum.ATTACK_UNREAL_CONSUME_EFFECT:
				title = "凝神";
				return "降低神识消耗";
			case GongFaEffectEnum.EXPLORE_ADD_UNREAL_SCOPE:
				title = "探索";
				return "增加神识覆盖范围";
			case GongFaEffectEnum.ENTRENCH_GOLD_ADD:
				title = "防护";
				return "受到金系伤害降低";
			case GongFaEffectEnum.ENTRENCH_WOOD_ADD:
				title = "防护";
				return "受到木系伤害降低";
			case GongFaEffectEnum.ENTRENCH_WATER_ADD:
				title = "防护";
				return "受到水系伤害降低";
			case GongFaEffectEnum.ENTRENCH_FIRE_ADD:
				title = "防护";
				return "受到火系伤害降低";
			case GongFaEffectEnum.ENTRENCH_EARTH_ADD:
				title = "防护";
				return "受到土系伤害降低";
			case GongFaEffectEnum.ENTRENCH_ADD:
				title = "防护";
				return "受到全系伤害降低";
			case GongFaEffectEnum.THUNDER_LOWER_HARM:
				title = "破劫";
				return "降低雷劫伤害";
			case GongFaEffectEnum.FATE_ADD_DROP:
				title = "气运";
				return "增加掉落几率";
			default:
				return "";
			}
		}

		public static string GetRoleLevelNameByRoleLevel(int level)
		{
			RoleLevelModel value = null;
			Singleton<FacadeManager>.Instance.SelectRoleLevelInfo().TryGetValue(level.ToString(), out value);
			if (value == null)
			{
				return level.ToString();
			}
			return value.level;
		}

		public static int GetStageByLevel(int level)
		{
			return (level - 1) / 10 + 1;
		}

		public static int GetLevelByStage(int stage)
		{
			return (stage - 1) * 10 + 1;
		}

		public static bool GetIsSuccess(float _gaiLv)
		{
			if (_gaiLv > 0f && _gaiLv < 100f)
			{
				if (Random.Range(0.1f, 99.9f) <= _gaiLv)
				{
					return true;
				}
			}
			else if (_gaiLv >= 100f)
			{
				return true;
			}
			return false;
		}

		public static string JianDuName(double value)
		{
			if (value >= 0.5 && value < 1.5)
			{
				return "软";
			}
			if (value >= 1.5 && value < 3.0)
			{
				return "固";
			}
			if (value >= 3.0)
			{
				return "硬";
			}
			return "无";
		}

		public static string RenDuName(double value)
		{
			if (value >= 0.5 && value < 1.5)
			{
				return "脆";
			}
			if (value >= 1.5 && value < 3.0)
			{
				return "柔";
			}
			if (value >= 3.0)
			{
				return "韧";
			}
			return "无";
		}

		public static string SiQiName(double value)
		{
			if (value < -2.5)
			{
				return "寒";
			}
			if (value < 0.0)
			{
				return "凉";
			}
			if (value < 2.5)
			{
				return "温";
			}
			if (value < 5.0)
			{
				return "热";
			}
			return "无";
		}

		public static string WuWeiName(double value)
		{
			if (value < -0.6)
			{
				return "辛";
			}
			if (value < -0.2)
			{
				return "苦";
			}
			if (value < 0.2)
			{
				return "酸";
			}
			if (value < 0.6)
			{
				return "甘";
			}
			if (value < 1.0)
			{
				return "咸";
			}
			return "无";
		}

		public static string BuXieName(double value)
		{
			if (value < 0.0)
			{
				return "泻";
			}
			if (value < 2.0)
			{
				return "补";
			}
			return "无";
		}

		public static string GuiJingName(int value)
		{
			return value switch
			{
				1 => "手厥阴", 
				2 => "手少阴", 
				3 => "足少阳", 
				4 => "手少阳", 
				5 => "足太阳", 
				6 => "足太阴", 
				7 => "手太阳", 
				8 => "手阳明", 
				9 => "手太阴", 
				10 => "足阳明", 
				11 => "足厥阴", 
				12 => "足少阴", 
				_ => "无", 
			};
		}

		public static string GetChineseNumber(int num)
		{
			return num switch
			{
				1 => "一", 
				2 => "二", 
				3 => "三", 
				4 => "四", 
				5 => "五", 
				6 => "六", 
				7 => "七", 
				8 => "八", 
				9 => "九", 
				10 => "十", 
				_ => "", 
			};
		}

		public static string GetVipAllName(int vipLevel)
		{
			return vipLevel switch
			{
				0 => "无天命等阶", 
				1 => "天命一阶", 
				2 => "天命二阶", 
				3 => "天命三阶", 
				4 => "天命四阶", 
				5 => "天命五阶", 
				6 => "天命六阶", 
				7 => "天命七阶", 
				8 => "天命八阶", 
				9 => "天命九阶", 
				10 => "天命十阶", 
				11 => "天命十一阶", 
				12 => "天命十二阶", 
				_ => "无", 
			};
		}
	}
}
