using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CeHuaItem : MonoBehaviour
	{
		public Text TitleUIText;
		public Text ValueText;
		public Button HelpBtn;

		internal RoleFuZhiType Profession { get; private set; }
		private string help;
		private bool helpBound;

		public void SetItem(RoleFuZhiType profession)
		{
			Profession = profession;
			Reflash();
		}

		public void Reflash()
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			int type = (int)Profession;
			if (role != null && role.Live3Type == type)
				SetItem(Profession, role.Live3Level, role.Live3Value);
			else if (role != null && role.Live4Type == type)
				SetItem(Profession, role.Live4Level, role.Live4Value);
			else if (role != null && role.Live1Type == type)
				SetItem(Profession, role.Live1Level, role.Live1Value);
			else if (role != null && role.Live2Type == type)
				SetItem(Profession, role.Live2Level, role.Live2Value);
			else
				SetItem(Profession, 0, 0);
		}

		internal void SetItem(RoleFuZhiType profession, int level, int experience)
		{
			Profession = profession;
			var tables = Singleton<FacadeManager>.Instance;
			string title;
			string rankName = null;
			double limit = 0;
			help = null;
			switch (profession)
			{
			case RoleFuZhiType.LianDan:
			{
				title = "丹师";
				var rank = FindRank(tables.SelectDanShiLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.practiceElixirTopValue ?? 0;
				help = "通过将草药、精血、妖丹凝练为丹药，服用以助自身修为增长。一品、二品丹师无需考核，积累炼制经验后即可自动晋级。三品及以上需前往虚神界丹师塔进行丹师品阶考核，考核前需将炼制经验积累至阈值。每一品丹师皆可尝试创造同等或低等级的丹方，丹方在丹师塔注册后可获得贡献点，用以兑换虚神殿中其他物品。丹师可以在洞府开辟炼丹室，丹师在服用丹药时，会有额外的药效加成。";
				break;
			}
			case RoleFuZhiType.LianQi:
			{
				title = "炼器师";
				var rank = FindRank(tables.SelectLianQiLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.plantTopValue ?? 0;
				help = "妖兽毛皮、骨骼，植物枝干，稀有矿石，炼器师通过熔炼、锻造，再辅以特殊的器阵纹，可以打造出各式各样的法器、法宝，甚至于灵宝、仙宝。一品、二品炼器师无需考核，积累炼器经验后会自动晋级。三品及以上需前往虚神界炼器协会进行炼器师品阶考核，考核前需将炼器经验积累至阈值。每一品炼器师可尝试创造同等或低等级的器图，器图在炼器协会注册后可获得贡献点，用以兑换虚神殿中其他物品。炼器师可以在洞府中开辟炼器室，炼器师可以降低战斗时使用法器的灵气消耗，品阶越高降低的消耗越多。";
				break;
			}
			case RoleFuZhiType.ZhiFu:
			{
				title = "符师";
				var rank = FindRank(tables.SelectSignLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.drawingSignTopValue ?? 0;
				help = "符师，将法术封印在符纸中，战斗时可以利用最低的消耗引动最大的效果。高阶符箓若被低阶修士引动，发挥出越级斩杀强敌的效果也成为可能。符师封印法术入符，可以将此法交付于神符会，兑换贡献点，这些贡献点可以兑换虚神殿中其他的物品。符师可以在洞府里开辟画符室，在交战时，符师使用符箓的效果会有所提升。";
				break;
			}
			case RoleFuZhiType.FaShu:
			{
				title = "术师";
				var rank = FindRank(tables.SelectShuShiLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.magicLevelTopValue ?? 0;
				help = "每一个手印，均是对天地灵气内在流动规律的控制，通过不同的手印组合，能创造出不同的法术。术师精于此道，他们对天地灵气的感悟非常人能及。术师同样不需要考核，所有修士均可以创造不同的法术，且对法术的使用效果高于常人。术师可以将自创的法术交付于万法阁，兑换贡献点，用以兑换虚神殿中其他的物品。术师可在洞府开辟战法室。修士也可由斗法配置进入战法室，消耗精力研习法术。凝元加强威力，疾诀缩短结印，小成时领悟第一道法纹，之后可重悟并择一保留。";
				break;
			}
			case RoleFuZhiType.ZhenTu:
			{
				title = "阵法师";
				var rank = FindRank(tables.SelectZhenFaLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.plantTopValue ?? 0;
				help = "深入专研太极八卦，了解天地的运行的规律，以创造各种不同的的阵法。积累经验后可自动升级，阵法师将自创的阵法交予阵师协会，可以兑换贡献点，用以兑换其他阵法。";
				break;
			}
			case RoleFuZhiType.BuDao:
			{
				title = "布道师";
				var rank = FindRank(tables.SelectBuDaoLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.teacherTopValue ?? 0;
				help = "通过对天道的感悟，了解真气在体内流动的规律，用以创造各种不同的功法。布道师无需考核，积累经验后可自动晋级。布道师将自创的功法，交付于道境圣殿，可以兑换贡献点，用以兑换虚神殿中其他的物品。布道师可以在洞府中开辟悟道台，用于参悟纳气功法，纳气功法参悟可以提升功法的运行重数，布道师可以比常人更快的晋升功法重数。";
				break;
			}
			case RoleFuZhiType.LingZhi:
			{
				title = "灵植师";
				var rank = FindRank(tables.SelectPlantLevelInfo(), level);
				rankName = rank?.name;
				limit = rank?.plantTopValue ?? 0;
				help = "灵植师可以在灵植上采集灵植种子，放入育种室。他们可以利用对天地灵气的了解，制造灵土、调配灵液，迅速的培育出成熟的灵植。灵植师了解植物的特性，可以通过嫁接培育出全新的植物，丰富修真界的物产。灵植师模拟嫁接的全新灵植可以交付至灵植协会，兑换贡献点，而贡献点可以兑换虚神殿中其他的物品。";
				break;
			}
			case RoleFuZhiType.QiaoJiang:
				title = "奇巧";
				break;
			case RoleFuZhiType.YuShou:
				title = "御兽";
				break;
			default:
				title = "副职";
				break;
			}
			if (string.IsNullOrWhiteSpace(rankName))
				rankName = level > 0 ? $"等级 {level}" : "未入品";
			else if (rankName.Length > title.Length && rankName.EndsWith(title, System.StringComparison.Ordinal))
				rankName = rankName.Substring(0, rankName.Length - title.Length);

			if (!helpBound)
			{
				HelpBtn.onClick.AddListener(ShowHelp);
				helpBound = true;
			}
			HelpBtn.gameObject.SetActive(!string.IsNullOrEmpty(help));
			RoleProfessionView.Bind(this, title, rankName, experience, (long)System.Math.Round(limit));
		}

		private static T FindRank<T>(Dictionary<int, T> table, int level) where T : class
		{
			return table != null && table.TryGetValue(level, out var entry) ? entry : null;
		}

		private void ShowHelp()
		{
			if (!string.IsNullOrEmpty(help))
				Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, help, TitleUIText.text);
		}
	}
}
