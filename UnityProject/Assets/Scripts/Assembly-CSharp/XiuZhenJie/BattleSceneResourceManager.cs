using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class BattleSceneResourceManager : MonoBehaviour
	{
		private Dictionary<string, GameObject> m_RedNumDic;

		private Dictionary<string, GameObject> m_YellowNumDic;

		private Dictionary<string, GameObject> m_GrayNumDic;

		public GameObject MonsterInsObj;

		public GameObject PlayerInsObj;

		public GameObject PlantInsObj;

		public GameObject MineralInsObj;

		public GameObject MiJingInsObj;

		public GameObject FaQiInsObj;

		public GameObject AoeInsObj;

		public GameObject ZhenFaInsObj;

		public Dictionary<WuXing, GameObject> FaQiDic;

		public Dictionary<WuXing, GameObject> HuDunFaShuDic;

		public Dictionary<WuXing, GameObject> DanTiFaShuDic;

		public Dictionary<WuXing, GameObject> AoeFaShuDic;

		public Dictionary<string, GameObject> RedNumDic => m_RedNumDic;

		public Dictionary<string, GameObject> YellowNumDic => m_YellowNumDic;

		public Dictionary<string, GameObject> GrayNumDic => m_GrayNumDic;

		public void Init()
		{
			StartLoad();
		}

		private void StartLoad()
		{
			ResourceMonster();
			ResourceNumSprite();
			ResourceFaQi();
			HuDunFaShu();
			DanTiFaShu();
			AoeFaShu();
			ZhenFa();
		}

		private void ResourceMonster()
		{
			PlayerInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Player/BattlePlayer.prefab");
			MonsterInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Monster/BattleNormalMonster.prefab");
			PlantInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Plant.prefab");
			MineralInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Mineral.prefab");
			MiJingInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/MiJing.prefab");
		}

		private void ResourceNumSprite()
		{
			m_RedNumDic = new Dictionary<string, GameObject>();
			m_YellowNumDic = new Dictionary<string, GameObject>();
			m_GrayNumDic = new Dictionary<string, GameObject>();
			GameObject value = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_0.prefab");
			GameObject value2 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_1.prefab");
			GameObject value3 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_2.prefab");
			GameObject value4 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_3.prefab");
			GameObject value5 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_4.prefab");
			GameObject value6 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_5.prefab");
			GameObject value7 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_6.prefab");
			GameObject value8 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_7.prefab");
			GameObject value9 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_8.prefab");
			GameObject value10 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_9.prefab");
			GameObject value11 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_Wan.prefab");
			GameObject value12 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_Yi.prefab");
			GameObject value13 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_Zhao.prefab");
			m_RedNumDic.Add("0", value);
			m_RedNumDic.Add("1", value2);
			m_RedNumDic.Add("2", value3);
			m_RedNumDic.Add("3", value4);
			m_RedNumDic.Add("4", value5);
			m_RedNumDic.Add("5", value6);
			m_RedNumDic.Add("6", value7);
			m_RedNumDic.Add("7", value8);
			m_RedNumDic.Add("8", value9);
			m_RedNumDic.Add("9", value10);
			m_RedNumDic.Add("万", value11);
			m_RedNumDic.Add("亿", value12);
			m_RedNumDic.Add("兆", value13);
			GameObject value14 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_0.prefab");
			GameObject value15 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_1.prefab");
			GameObject value16 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_2.prefab");
			GameObject value17 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_3.prefab");
			GameObject value18 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_4.prefab");
			GameObject value19 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_5.prefab");
			GameObject value20 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_6.prefab");
			GameObject value21 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_7.prefab");
			GameObject value22 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_8.prefab");
			GameObject value23 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_9.prefab");
			GameObject value24 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_Wan.prefab");
			GameObject value25 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_Yi.prefab");
			GameObject value26 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_Zhao.prefab");
			GameObject value27 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_XiShou.prefab");
			GameObject value28 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_DiKang.prefab");
			GameObject value29 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_JianSu.prefab");
			GameObject value30 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_DingShen.prefab");
			m_YellowNumDic.Add("0", value14);
			m_YellowNumDic.Add("1", value15);
			m_YellowNumDic.Add("2", value16);
			m_YellowNumDic.Add("3", value17);
			m_YellowNumDic.Add("4", value18);
			m_YellowNumDic.Add("5", value19);
			m_YellowNumDic.Add("6", value20);
			m_YellowNumDic.Add("7", value21);
			m_YellowNumDic.Add("8", value22);
			m_YellowNumDic.Add("9", value23);
			m_YellowNumDic.Add("万", value24);
			m_YellowNumDic.Add("亿", value25);
			m_YellowNumDic.Add("兆", value26);
			m_YellowNumDic.Add("定身", value30);
			m_YellowNumDic.Add("吸收", value27);
			m_YellowNumDic.Add("抵抗", value28);
			m_YellowNumDic.Add("减速", value29);
			GameObject value31 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_0.prefab");
			GameObject value32 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_1.prefab");
			GameObject value33 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_2.prefab");
			GameObject value34 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_3.prefab");
			GameObject value35 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_4.prefab");
			GameObject value36 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_5.prefab");
			GameObject value37 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_6.prefab");
			GameObject value38 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_7.prefab");
			GameObject value39 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_8.prefab");
			GameObject value40 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_9.prefab");
			GameObject value41 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_Wan.prefab");
			GameObject value42 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_Yi.prefab");
			GameObject value43 = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_Zhao.prefab");
			m_GrayNumDic.Add("0", value31);
			m_GrayNumDic.Add("1", value32);
			m_GrayNumDic.Add("2", value33);
			m_GrayNumDic.Add("3", value34);
			m_GrayNumDic.Add("4", value35);
			m_GrayNumDic.Add("5", value36);
			m_GrayNumDic.Add("6", value37);
			m_GrayNumDic.Add("7", value38);
			m_GrayNumDic.Add("8", value39);
			m_GrayNumDic.Add("9", value40);
			m_GrayNumDic.Add("万", value41);
			m_GrayNumDic.Add("亿", value42);
			m_GrayNumDic.Add("兆", value43);
		}

		private void ResourceFaQi()
		{
			FaQiDic = new Dictionary<WuXing, GameObject>();
			FaQiInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/WuQi_Item.prefab");
			FaQiDic.Add(WuXing.All, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/WuXing_WuQi_Ani_Prefab.prefab"));
			FaQiDic.Add(WuXing.Jin, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/Jin_WuQi_Ani_Prefab.prefab"));
			FaQiDic.Add(WuXing.Mu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/Mu_WuQi_Ani_Prefab.prefab"));
			FaQiDic.Add(WuXing.Shui, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/Shui_WuQi_Ani_Prefab.prefab"));
			FaQiDic.Add(WuXing.Huo, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/Huo_WuQi_Ani_Prefab.prefab"));
			FaQiDic.Add(WuXing.Tu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/FaQi/Tu_WuQi_Ani_Prefab.prefab"));
		}

		private void HuDunFaShu()
		{
			HuDunFaShuDic = new Dictionary<WuXing, GameObject>();
			HuDunFaShuDic.Add(WuXing.All, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/HuDun/WuXing_HuDun_Ani_Prefab.prefab"));
			HuDunFaShuDic.Add(WuXing.Jin, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/HuDun/Jin_HuDun_Ani_Prefab.prefab"));
			HuDunFaShuDic.Add(WuXing.Mu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/HuDun/Mu_HuDun_Ani_Prefab.prefab"));
			HuDunFaShuDic.Add(WuXing.Shui, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/HuDun/Shui_HuDun_Ani_Prefab.prefab"));
			HuDunFaShuDic.Add(WuXing.Huo, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/HuDun/Shui_HuDun_Ani_Prefab.prefab"));
			HuDunFaShuDic.Add(WuXing.Tu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/HuDun/Tu_HuDun_Ani_Prefab.prefab"));
		}

		private void DanTiFaShu()
		{
			DanTiFaShuDic = new Dictionary<WuXing, GameObject>();
			DanTiFaShuDic.Add(WuXing.All, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/WuXing_Danti_Ani_Prefab.prefab"));
			DanTiFaShuDic.Add(WuXing.Jin, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Jin_Danti_Ani_Prefab.prefab"));
			DanTiFaShuDic.Add(WuXing.Mu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Mu_Danti_Ani_Prefab.prefab"));
			DanTiFaShuDic.Add(WuXing.Shui, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Shui_Danti_Ani_Prefab.prefab"));
			DanTiFaShuDic.Add(WuXing.Huo, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Huo_Danti_Ani_Prefab.prefab"));
			DanTiFaShuDic.Add(WuXing.Tu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/DanTi/Tu_Danti_Ani_Prefab.prefab"));
		}

		private void AoeFaShu()
		{
			AoeFaShuDic = new Dictionary<WuXing, GameObject>();
			AoeInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/Aoe_Ani_Prefab.prefab");
			AoeFaShuDic.Add(WuXing.All, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/WuXing_GongJi_Ani_Prefab.prefab"));
			AoeFaShuDic.Add(WuXing.Jin, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/Jin_GongJi_Ani_Prefab.prefab"));
			AoeFaShuDic.Add(WuXing.Mu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/Mu_GongJi_Ani_Prefab.prefab"));
			AoeFaShuDic.Add(WuXing.Shui, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/Shui_GongJi_Ani_Prefab.prefab"));
			AoeFaShuDic.Add(WuXing.Huo, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/Huo_GongJi_Ani_Prefab.prefab"));
			AoeFaShuDic.Add(WuXing.Tu, Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/Aoe/Tu_GongJi_Ani_Prefab.prefab"));
		}

		private void ZhenFa()
		{
			ZhenFaInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/ZhenFa/ZhenFa_Prefab.prefab");
		}

		public void ClearDataSource()
		{
			m_RedNumDic = null;
			m_YellowNumDic = null;
			m_GrayNumDic = null;
			MonsterInsObj = null;
			PlayerInsObj = null;
			PlantInsObj = null;
			MineralInsObj = null;
			MiJingInsObj = null;
			FaQiInsObj = null;
			AoeInsObj = null;
			FaQiDic = null;
			HuDunFaShuDic = null;
			DanTiFaShuDic = null;
			AoeFaShuDic = null;
			ZhenFaInsObj = null;
		}
	}
}
