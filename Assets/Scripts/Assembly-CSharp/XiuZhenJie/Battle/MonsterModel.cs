using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class MonsterModel : BaseModel
	{
		public string id;

		public string name;

		public int monsterType;

		public int fiveElement;

		public string mapId;

		public int level;

		public double superAir;

		public double toughTenacity;

		public double maxPureBlood;

		public int type;

		public int icon;

		public int worldProbability;

		public List<MonsterJiNengModel> skillVoList;

		public List<XzjMonsterEquipmentVO> monsterEquipmentList;

		public double methodTransmissionSpeed;

		public List<XzjWorldGoodsBookAchievementMethodSpaceVO> bookAchievementList;

		public List<XzjWorldGoodsBookSkillSpaceVO> skillList;

		public List<GoodsBookElixir> bookElixirList;

		public List<GoodsBookSignSpaceVO> bookSignList;

		public List<XzjWorldGoodsArticleEquipmentSpaceVO> equipmentList;

		public List<XzjWorldGoodsMaterialsAnimalSpaceVO> materialAnimalsList;

		public XzjWorldGoodsMaterialsMineralSpaceVO xzjWorldGoodsMaterialsMineralSpaceVO;

		public XzjWorldGoodsMaterialsPlantsSpaceVO xzjWorldGoodsMaterialsPlantsSpaceVO;

		public GoodsArticleSecretAreaVO goodsArticleSecretArea;

		public double baseSpeed;

		public double perception;

		public int sort;

		public double strengthen;
	}
}
