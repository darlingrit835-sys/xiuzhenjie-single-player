using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class GameInitInfoModel : BaseModel
	{
		public List<KongLingLevelModel> xzjWorldControllerMatrixList;

		public List<JuLingLevelModel> xzjWorldGatherMatrixList;

		public List<WorldHomeModel> xzjWorldHomeList;

		public List<RoleLevelModel> xzjRoleLevelList;

		public List<GameIconConfigModel> xzjIconPathList;

		public List<DanShiLevelInfoModel> xzjWorldAssessElixirList;

		public List<XzjWorldAsserssAchievement> a;

		public List<XzjWorldAsserssLianQi> xzjWorldAssessLianQiList;

		public List<XzjWorldAsserssZhenFa> xzjWorldAssessZhenFaList;

		public List<AcupointModel> b;

		public List<XzjWorldAssessMagicModel> c;

		public List<WorldHandSealModel> d;

		public List<XzjWorldAssessSign> e;

		public List<XzjWorldSecondaryOccupation> s;

		public List<CodeEntity> CodeEntities;

		public List<XzjWorldPlant> PlantInfo;

		public List<MissionEntity> MissionInfo;

		public List<CoefficientNumEntity> CoefficientLianDanInfo;

		public List<CoefficientNumEntity> CoefficientHuaFuInfo;

		public List<XuanTianLouEntity> XuanTianLouGoodsInfo;

		public List<ChengHaoInfoEntity> ChengHaoInfo;

		public List<CashLiBaoGoodsEntity> CashLiBaoInfo;

		public List<EventGoodsEntity> EventGoodsIndo;

		public List<VipInfoEntity> VipInfo;

		public List<ZM_NeiMenUpdateEntity> ZM_NeiMenUpdateInfo;

		public List<ZM_CangKuUpdateEntity> ZM_CangKuUpdateInfo;

		public List<ZM_FuZhiUpdateEntity> ZM_FuZhiUpdateInfo;

		public List<ZM_LianDanUpdateEntity> ZM_LianDanUpdateInfo;

		public List<ZM_YanWuChangUpdateEntity> ZM_YanWuChangUpdateInfo;

		public List<ZM_KuangMaiUpdateEntity> ZM_KuangMaiUpdateInfo;

		public List<XuanTianLouEntity> ZM_CangBaoGeGoodsInfo;
	}
}
