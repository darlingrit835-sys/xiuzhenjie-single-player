using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace XiuZhenJie
{
	public class FacadeManager : Singleton<FacadeManager>
	{
		private Dictionary<string, RoleLevelModel> RoleInFoDic = new Dictionary<string, RoleLevelModel>();

		private Dictionary<string, WorldHomeModel> DongFuInFoDic = new Dictionary<string, WorldHomeModel>();

		private Dictionary<string, JuLingLevelModel> JuLingZhenInFoDic = new Dictionary<string, JuLingLevelModel>();

		private Dictionary<string, KongLingLevelModel> KongLingZhenInFoDic = new Dictionary<string, KongLingLevelModel>();

		private Dictionary<int, GameIconConfigModel> GameIconInFoDic = new Dictionary<int, GameIconConfigModel>();

		private Dictionary<int, DanShiLevelInfoModel> DanShiLevelDic = new Dictionary<int, DanShiLevelInfoModel>();

		private Dictionary<int, XzjWorldAsserssAchievement> BuDaoLevelDic = new Dictionary<int, XzjWorldAsserssAchievement>();

		private Dictionary<int, XzjWorldAssessMagicModel> ShuShiLevelDic = new Dictionary<int, XzjWorldAssessMagicModel>();

		private Dictionary<int, XzjWorldAsserssLianQi> m_LianQiLevelDic = new Dictionary<int, XzjWorldAsserssLianQi>();

		private Dictionary<int, XzjWorldAsserssZhenFa> m_ZhenFaLevelDic = new Dictionary<int, XzjWorldAsserssZhenFa>();

		private List<WorldHandSealModel> m_FaShuInfoList = new List<WorldHandSealModel>();

		private List<AcupointModel> m_AcupointList = new List<AcupointModel>();

		private Dictionary<int, XzjWorldAssessSign> m_SignLevelDic = new Dictionary<int, XzjWorldAssessSign>();

		private Dictionary<string, XzjWorldSecondaryOccupation> m_FuZhiDic = new Dictionary<string, XzjWorldSecondaryOccupation>();

		public Dictionary<int, string> m_CodeMsg = new Dictionary<int, string>();

		private Dictionary<int, XzjWorldPlant> m_LingZhiDic = new Dictionary<int, XzjWorldPlant>();

		private Dictionary<int, MissionEntity> m_MissionDic = new Dictionary<int, MissionEntity>();

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_GoodsRankBgSprite;

		private List<CoefficientNumEntity> m_CoefficientLianDanList;

		private List<CoefficientNumEntity> m_CoefficientHuaFuList;

		private Dictionary<int, XuanTianLouEntity> m_XuanTianLouGoodsDic = new Dictionary<int, XuanTianLouEntity>();

		private Dictionary<string, XuanTianLouEntity> m_XuanTianLouTempleteGoodsDic = new Dictionary<string, XuanTianLouEntity>();

		private Dictionary<int, ChengHaoInfoEntity> m_ChengHaoInfoDic = new Dictionary<int, ChengHaoInfoEntity>();

		private Dictionary<int, CashLiBaoGoodsEntity> m_CashGoodsLiBaoDic = new Dictionary<int, CashLiBaoGoodsEntity>();

		private Dictionary<int, List<EventGoodsEntity>> m_EventGoodsDic = new Dictionary<int, List<EventGoodsEntity>>();

		private Dictionary<int, VipInfoEntity> m_VipInfoDic = new Dictionary<int, VipInfoEntity>();

		private Dictionary<int, ZM_NeiMenUpdateEntity> m_ZM_NeiMenUpdateDic = new Dictionary<int, ZM_NeiMenUpdateEntity>();

		private Dictionary<int, ZM_CangKuUpdateEntity> m_ZM_CangKuUpdateDic = new Dictionary<int, ZM_CangKuUpdateEntity>();

		private Dictionary<int, ZM_FuZhiUpdateEntity> m_ZM_FuZhiUpdateDic = new Dictionary<int, ZM_FuZhiUpdateEntity>();

		private Dictionary<int, ZM_LianDanUpdateEntity> m_ZM_LianDanUpdateDic = new Dictionary<int, ZM_LianDanUpdateEntity>();

		private Dictionary<int, ZM_YanWuChangUpdateEntity> m_ZM_YanWuChangUpdateDic = new Dictionary<int, ZM_YanWuChangUpdateEntity>();

		private Dictionary<int, ZM_KuangMaiUpdateEntity> m_ZM_KuangMaiUpdateDic = new Dictionary<int, ZM_KuangMaiUpdateEntity>();

		private Dictionary<int, List<XuanTianLouEntity>> m_ZM_CangBaoGeGoodsDic = new Dictionary<int, List<XuanTianLouEntity>>();

		public void Init()
		{
			GameInitInfoModel gameInitInfoModel = new GameInitInfoModel();
			TextAsset textAsset = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/a.json");
			TextAsset textAsset2 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/b.json");
			TextAsset textAsset3 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/c.json");
			TextAsset textAsset4 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/e.json");
			TextAsset textAsset5 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/s.json");
			TextAsset textAsset6 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/d.json");
			TextAsset textAsset7 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjIconPathList.json");
			TextAsset textAsset8 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjRoleLevelList.json");
			TextAsset textAsset9 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjWorldAssessElixirList.json");
			TextAsset textAsset10 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjWorldControllerMatrixList.json");
			TextAsset textAsset11 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjWorldGatherMatrixList.json");
			TextAsset textAsset12 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjWorldHomeList.json");
			TextAsset textAsset13 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/Code.json");
			TextAsset textAsset14 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjPlantingList.json");
			TextAsset textAsset15 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/Mission.json");
			TextAsset textAsset16 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/CoefficientHuaFuNum.json");
			TextAsset textAsset17 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/CoefficientLianDanNum.json");
			TextAsset textAsset18 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/XuanTianLouGoods.json");
			TextAsset textAsset19 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ChengHaoInfo.json");
			TextAsset textAsset20 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/LiBaoGoods.json");
			TextAsset textAsset21 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjLianQiList.json");
			TextAsset textAsset22 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/xzjZhenFaList.json");
			TextAsset textAsset23 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/EventGoods.json");
			TextAsset textAsset24 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/VipLevelInfo.json");
			TextAsset textAsset25 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenShanFengCangKuUpdateInfo.json");
			TextAsset textAsset26 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenShanFengNeiMenUpdateInfo.json");
			TextAsset textAsset27 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenShanFengFuZhiUpdateInfo.json");
			TextAsset textAsset28 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenShanFengLianDanUpdateInfo.json");
			TextAsset textAsset29 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenShanFengYanWuChangUpdateInfo.json");
			TextAsset textAsset30 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenShanFengKuangMaiUpdateInfo.json");
			TextAsset textAsset31 = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>("Assets/GameData/Json/ZongMenCangBaoGe.json");
			gameInitInfoModel.a = JsonMapper.ToObject<List<XzjWorldAsserssAchievement>>(textAsset.text);
			gameInitInfoModel.b = JsonMapper.ToObject<List<AcupointModel>>(textAsset2.text);
			gameInitInfoModel.c = JsonMapper.ToObject<List<XzjWorldAssessMagicModel>>(textAsset3.text);
			gameInitInfoModel.d = JsonMapper.ToObject<List<WorldHandSealModel>>(textAsset6.text);
			gameInitInfoModel.e = JsonMapper.ToObject<List<XzjWorldAssessSign>>(textAsset4.text);
			gameInitInfoModel.s = JsonMapper.ToObject<List<XzjWorldSecondaryOccupation>>(textAsset5.text);
			gameInitInfoModel.xzjIconPathList = new List<GameIconConfigModel>();
			gameInitInfoModel.xzjIconPathList = JsonMapper.ToObject<List<GameIconConfigModel>>(textAsset7.text);
			gameInitInfoModel.xzjRoleLevelList = JsonMapper.ToObject<List<RoleLevelModel>>(textAsset8.text);
			gameInitInfoModel.xzjWorldAssessElixirList = JsonMapper.ToObject<List<DanShiLevelInfoModel>>(textAsset9.text);
			gameInitInfoModel.xzjWorldControllerMatrixList = JsonMapper.ToObject<List<KongLingLevelModel>>(textAsset10.text);
			gameInitInfoModel.xzjWorldGatherMatrixList = JsonMapper.ToObject<List<JuLingLevelModel>>(textAsset11.text);
			gameInitInfoModel.xzjWorldHomeList = JsonMapper.ToObject<List<WorldHomeModel>>(textAsset12.text);
			gameInitInfoModel.CodeEntities = JsonMapper.ToObject<List<CodeEntity>>(textAsset13.text);
			gameInitInfoModel.PlantInfo = JsonMapper.ToObject<List<XzjWorldPlant>>(textAsset14.text);
			gameInitInfoModel.MissionInfo = JsonMapper.ToObject<List<MissionEntity>>(textAsset15.text);
			gameInitInfoModel.CoefficientLianDanInfo = JsonMapper.ToObject<List<CoefficientNumEntity>>(textAsset17.text);
			gameInitInfoModel.CoefficientHuaFuInfo = JsonMapper.ToObject<List<CoefficientNumEntity>>(textAsset16.text);
			gameInitInfoModel.XuanTianLouGoodsInfo = JsonMapper.ToObject<List<XuanTianLouEntity>>(textAsset18.text);
			gameInitInfoModel.ChengHaoInfo = JsonMapper.ToObject<List<ChengHaoInfoEntity>>(textAsset19.text);
			gameInitInfoModel.CashLiBaoInfo = JsonMapper.ToObject<List<CashLiBaoGoodsEntity>>(textAsset20.text);
			gameInitInfoModel.xzjWorldAssessLianQiList = JsonMapper.ToObject<List<XzjWorldAsserssLianQi>>(textAsset21.text);
			gameInitInfoModel.xzjWorldAssessZhenFaList = JsonMapper.ToObject<List<XzjWorldAsserssZhenFa>>(textAsset22.text);
			gameInitInfoModel.EventGoodsIndo = JsonMapper.ToObject<List<EventGoodsEntity>>(textAsset23.text);
			gameInitInfoModel.VipInfo = JsonMapper.ToObject<List<VipInfoEntity>>(textAsset24.text);
			gameInitInfoModel.ZM_NeiMenUpdateInfo = JsonMapper.ToObject<List<ZM_NeiMenUpdateEntity>>(textAsset26.text);
			gameInitInfoModel.ZM_CangKuUpdateInfo = JsonMapper.ToObject<List<ZM_CangKuUpdateEntity>>(textAsset25.text);
			gameInitInfoModel.ZM_FuZhiUpdateInfo = JsonMapper.ToObject<List<ZM_FuZhiUpdateEntity>>(textAsset27.text);
			gameInitInfoModel.ZM_LianDanUpdateInfo = JsonMapper.ToObject<List<ZM_LianDanUpdateEntity>>(textAsset28.text);
			gameInitInfoModel.ZM_YanWuChangUpdateInfo = JsonMapper.ToObject<List<ZM_YanWuChangUpdateEntity>>(textAsset29.text);
			gameInitInfoModel.ZM_KuangMaiUpdateInfo = JsonMapper.ToObject<List<ZM_KuangMaiUpdateEntity>>(textAsset30.text);
			gameInitInfoModel.ZM_CangBaoGeGoodsInfo = JsonMapper.ToObject<List<XuanTianLouEntity>>(textAsset31.text);
			InitInfo(gameInitInfoModel);
			ResourceGoodsInfo();
		}

		private void ResourceGoodsInfo()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_GoodsRankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_Xie.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ShouGu.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_GongFaShu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_YaoDan.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhiWu.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_PiMao.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FangJu.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanYao.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			Sprite value11 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuShu.png");
			Sprite value12 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanShu.png");
			Sprite value13 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FaSuShu.png");
			Sprite value14 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuLu.png");
			Sprite value15 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_MiJing.png");
			Sprite value16 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_JingXue.png");
			Sprite value17 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_KuangShi.png");
			Sprite value18 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_QiTu.png");
			Sprite value19 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenTu.png");
			Sprite value20 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_LingKui.png");
			Sprite value21 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_CanJuan.png");
			Sprite value22 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuDaoShi.png");
			Sprite value23 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_YunTie.png");
			Sprite value24 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_TiShenKuiLei.png");
			Sprite value25 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/QianDaoLingShiItem.png");
			Sprite value26 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/QianDaoLingYuItem.png");
			Sprite value27 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_XuShenDian.png");
			Sprite value28 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenPan.png");
			m_GoodsIconSprite.Add(1, value21);
			m_GoodsIconSprite.Add(2, value12);
			m_GoodsIconSprite.Add(5, value11);
			m_GoodsIconSprite.Add(6, value13);
			m_GoodsIconSprite.Add(29, value);
			m_GoodsIconSprite.Add(10, value2);
			m_GoodsIconSprite.Add(15, value4);
			m_GoodsIconSprite.Add(11, value5);
			m_GoodsIconSprite.Add(12, value6);
			m_GoodsIconSprite.Add(8, value7);
			m_GoodsIconSprite.Add(16, value8);
			m_GoodsIconSprite.Add(19, value9);
			m_GoodsIconSprite.Add(26, value10);
			m_GoodsIconSprite.Add(21, value14);
			m_GoodsIconSprite.Add(28, value15);
			m_GoodsIconSprite.Add(31, value15);
			m_GoodsIconSprite.Add(30, value15);
			m_GoodsIconSprite.Add(32, value15);
			m_GoodsIconSprite.Add(9, value16);
			m_GoodsIconSprite.Add(13, value17);
			m_GoodsIconSprite.Add(4, value18);
			m_GoodsIconSprite.Add(33, value21);
			m_GoodsIconSprite.Add(35, value23);
			m_GoodsIconSprite.Add(34, value22);
			m_GoodsIconSprite.Add(36, value20);
			m_GoodsIconSprite.Add(37, value24);
			m_GoodsIconSprite.Add(100, value25);
			m_GoodsIconSprite.Add(99, value26);
			m_GoodsIconSprite.Add(93, value27);
			m_GoodsIconSprite.Add(97, value27);
			m_GoodsIconSprite.Add(95, value27);
			m_GoodsIconSprite.Add(94, value27);
			m_GoodsIconSprite.Add(96, value27);
			m_GoodsIconSprite.Add(98, value27);
			m_GoodsIconSprite.Add(92, value27);
			m_GoodsIconSprite.Add(90, value27);
			m_GoodsIconSprite.Add(38, value14);
			m_GoodsIconSprite.Add(3, value19);
			m_GoodsIconSprite.Add(20, value28);
			m_GoodsIconSprite.Add(39, value3);
			m_GoodsIconSprite.Add(40, value23);
			m_GoodsIconSprite.Add(41, value13);
			Sprite value29 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value30 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value31 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value32 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_GoodsRankBgSprite.Add(1, value29);
			m_GoodsRankBgSprite.Add(2, value30);
			m_GoodsRankBgSprite.Add(3, value31);
			m_GoodsRankBgSprite.Add(4, value32);
		}

		private void InitInfo(GameInitInfoModel infoModel)
		{
			RoleInFoDic?.Clear();
			DongFuInFoDic?.Clear();
			JuLingZhenInFoDic?.Clear();
			KongLingZhenInFoDic?.Clear();
			GameIconInFoDic?.Clear();
			DanShiLevelDic?.Clear();
			BuDaoLevelDic?.Clear();
			m_FuZhiDic?.Clear();
			ShuShiLevelDic?.Clear();
			m_LianQiLevelDic?.Clear();
			m_ZhenFaLevelDic?.Clear();
			m_SignLevelDic?.Clear();
			m_AcupointList?.Clear();
			m_FaShuInfoList?.Clear();
			m_CodeMsg?.Clear();
			m_LingZhiDic?.Clear();
			m_MissionDic?.Clear();
			m_CoefficientLianDanList?.Clear();
			m_CoefficientHuaFuList?.Clear();
			m_XuanTianLouGoodsDic?.Clear();
			m_XuanTianLouTempleteGoodsDic?.Clear();
			m_ChengHaoInfoDic?.Clear();
			m_CashGoodsLiBaoDic?.Clear();
			m_EventGoodsDic?.Clear();
			m_VipInfoDic?.Clear();
			m_ZM_CangKuUpdateDic?.Clear();
			m_ZM_FuZhiUpdateDic?.Clear();
			m_ZM_NeiMenUpdateDic?.Clear();
			m_ZM_LianDanUpdateDic?.Clear();
			m_ZM_YanWuChangUpdateDic?.Clear();
			m_ZM_CangBaoGeGoodsDic?.Clear();
			m_ZM_KuangMaiUpdateDic?.Clear();
			if (infoModel == null)
			{
				return;
			}
			if (infoModel.xzjIconPathList != null)
			{
				foreach (GameIconConfigModel xzjIconPath in infoModel.xzjIconPathList)
				{
					GameIconInFoDic.Add(xzjIconPath.id, xzjIconPath);
				}
			}
			if (infoModel.xzjRoleLevelList != null)
			{
				foreach (RoleLevelModel xzjRoleLevel in infoModel.xzjRoleLevelList)
				{
					RoleInFoDic.Add(xzjRoleLevel.id.ToString(), xzjRoleLevel);
				}
			}
			if (infoModel.xzjWorldControllerMatrixList != null)
			{
				foreach (KongLingLevelModel xzjWorldControllerMatrix in infoModel.xzjWorldControllerMatrixList)
				{
					KongLingZhenInFoDic.Add(xzjWorldControllerMatrix.id, xzjWorldControllerMatrix);
				}
			}
			if (infoModel.xzjWorldGatherMatrixList != null)
			{
				foreach (JuLingLevelModel xzjWorldGatherMatrix in infoModel.xzjWorldGatherMatrixList)
				{
					JuLingZhenInFoDic.Add(xzjWorldGatherMatrix.id, xzjWorldGatherMatrix);
				}
			}
			if (infoModel.xzjWorldHomeList != null)
			{
				foreach (WorldHomeModel xzjWorldHome in infoModel.xzjWorldHomeList)
				{
					DongFuInFoDic.Add(xzjWorldHome.id, xzjWorldHome);
				}
			}
			if (infoModel.xzjWorldAssessElixirList != null)
			{
				foreach (DanShiLevelInfoModel xzjWorldAssessElixir in infoModel.xzjWorldAssessElixirList)
				{
					DanShiLevelDic.Add(xzjWorldAssessElixir.id, xzjWorldAssessElixir);
				}
			}
			if (infoModel.a != null)
			{
				foreach (XzjWorldAsserssAchievement item in infoModel.a)
				{
					BuDaoLevelDic.Add(item.id, item);
				}
			}
			if (infoModel.b != null)
			{
				m_AcupointList = infoModel.b;
			}
			if (infoModel.c != null)
			{
				foreach (XzjWorldAssessMagicModel item2 in infoModel.c)
				{
					ShuShiLevelDic.Add(item2.id, item2);
				}
			}
			if (infoModel.d != null)
			{
				m_FaShuInfoList = infoModel.d;
			}
			if (infoModel.e != null)
			{
				foreach (XzjWorldAssessSign item3 in infoModel.e)
				{
					m_SignLevelDic.Add(item3.id, item3);
				}
			}
			if (infoModel.s != null)
			{
				foreach (XzjWorldSecondaryOccupation item4 in infoModel.s)
				{
					m_FuZhiDic.Add(item4.id, item4);
				}
			}
			if (infoModel.CodeEntities != null)
			{
				foreach (CodeEntity codeEntity in infoModel.CodeEntities)
				{
					m_CodeMsg.Add(codeEntity.code, codeEntity.msg);
				}
			}
			if (infoModel.PlantInfo != null)
			{
				foreach (XzjWorldPlant item5 in infoModel.PlantInfo)
				{
					m_LingZhiDic.Add(item5.id, item5);
				}
			}
			if (infoModel.MissionInfo != null)
			{
				foreach (MissionEntity item6 in infoModel.MissionInfo)
				{
					m_MissionDic.Add(item6.id, item6);
				}
			}
			if (infoModel.CoefficientLianDanInfo != null)
			{
				m_CoefficientLianDanList = infoModel.CoefficientLianDanInfo;
			}
			if (infoModel.CoefficientHuaFuInfo != null)
			{
				m_CoefficientHuaFuList = infoModel.CoefficientHuaFuInfo;
			}
			if (infoModel.XuanTianLouGoodsInfo != null)
			{
				foreach (XuanTianLouEntity item7 in infoModel.XuanTianLouGoodsInfo)
				{
					if (!m_XuanTianLouGoodsDic.ContainsKey(item7.Id))
					{
						m_XuanTianLouGoodsDic.Add(item7.Id, item7);
					}
					if (!m_XuanTianLouTempleteGoodsDic.ContainsKey(item7.TemplateId))
					{
						m_XuanTianLouTempleteGoodsDic.Add(item7.TemplateId, item7);
					}
				}
			}
			if (infoModel.ChengHaoInfo != null)
			{
				foreach (ChengHaoInfoEntity item8 in infoModel.ChengHaoInfo)
				{
					if (!m_ChengHaoInfoDic.ContainsKey(item8.Id))
					{
						m_ChengHaoInfoDic.Add(item8.Id, item8);
					}
				}
			}
			if (infoModel.CashLiBaoInfo != null)
			{
				foreach (CashLiBaoGoodsEntity item9 in infoModel.CashLiBaoInfo)
				{
					if (!m_CashGoodsLiBaoDic.ContainsKey(item9.TypeId))
					{
						m_CashGoodsLiBaoDic.Add(item9.TypeId, item9);
					}
				}
			}
			if (infoModel.xzjWorldAssessLianQiList != null)
			{
				foreach (XzjWorldAsserssLianQi xzjWorldAssessLianQi in infoModel.xzjWorldAssessLianQiList)
				{
					if (!m_LianQiLevelDic.ContainsKey(xzjWorldAssessLianQi.id))
					{
						m_LianQiLevelDic.Add(xzjWorldAssessLianQi.id, xzjWorldAssessLianQi);
					}
				}
			}
			if (infoModel.xzjWorldAssessZhenFaList != null)
			{
				foreach (XzjWorldAsserssZhenFa xzjWorldAssessZhenFa in infoModel.xzjWorldAssessZhenFaList)
				{
					if (!m_ZhenFaLevelDic.ContainsKey(xzjWorldAssessZhenFa.id))
					{
						m_ZhenFaLevelDic.Add(xzjWorldAssessZhenFa.id, xzjWorldAssessZhenFa);
					}
				}
			}
			if (infoModel.EventGoodsIndo != null)
			{
				foreach (EventGoodsEntity item10 in infoModel.EventGoodsIndo)
				{
					if (m_EventGoodsDic.ContainsKey(item10.Stage))
					{
						m_EventGoodsDic[item10.Stage].Add(item10);
						continue;
					}
					m_EventGoodsDic.Add(item10.Stage, new List<EventGoodsEntity> { item10 });
				}
			}
			if (infoModel.VipInfo != null)
			{
				foreach (VipInfoEntity item11 in infoModel.VipInfo)
				{
					m_VipInfoDic.Add(item11.Id, item11);
				}
			}
			if (infoModel.ZM_CangKuUpdateInfo != null)
			{
				foreach (ZM_CangKuUpdateEntity item12 in infoModel.ZM_CangKuUpdateInfo)
				{
					m_ZM_CangKuUpdateDic.Add(item12.Stage, item12);
				}
			}
			if (infoModel.ZM_NeiMenUpdateInfo != null)
			{
				foreach (ZM_NeiMenUpdateEntity item13 in infoModel.ZM_NeiMenUpdateInfo)
				{
					m_ZM_NeiMenUpdateDic.Add(item13.Stage, item13);
				}
			}
			if (infoModel.ZM_FuZhiUpdateInfo != null)
			{
				foreach (ZM_FuZhiUpdateEntity item14 in infoModel.ZM_FuZhiUpdateInfo)
				{
					m_ZM_FuZhiUpdateDic.Add(item14.Stage, item14);
				}
			}
			if (infoModel.ZM_LianDanUpdateInfo != null)
			{
				foreach (ZM_LianDanUpdateEntity item15 in infoModel.ZM_LianDanUpdateInfo)
				{
					m_ZM_LianDanUpdateDic.Add(item15.Stage, item15);
				}
			}
			if (infoModel.ZM_YanWuChangUpdateInfo != null)
			{
				foreach (ZM_YanWuChangUpdateEntity item16 in infoModel.ZM_YanWuChangUpdateInfo)
				{
					m_ZM_YanWuChangUpdateDic.Add(item16.Stage, item16);
				}
			}
			if (infoModel.ZM_KuangMaiUpdateInfo != null)
			{
				foreach (ZM_KuangMaiUpdateEntity item17 in infoModel.ZM_KuangMaiUpdateInfo)
				{
					m_ZM_KuangMaiUpdateDic.Add(item17.Stage, item17);
				}
			}
			if (infoModel.ZM_CangBaoGeGoodsInfo == null)
			{
				return;
			}
			foreach (XuanTianLouEntity item18 in infoModel.ZM_CangBaoGeGoodsInfo)
			{
				if (m_ZM_CangBaoGeGoodsDic.ContainsKey(item18.GoodsType))
				{
					m_ZM_CangBaoGeGoodsDic[item18.GoodsType].Add(item18);
					continue;
				}
				m_ZM_CangBaoGeGoodsDic.Add(item18.GoodsType, new List<XuanTianLouEntity> { item18 });
			}
		}

		public Sprite SelectImgSprite(int fiveElement, int index, int type)
		{
			Sprite result = null;
			if (GameIconInFoDic != null)
			{
				foreach (KeyValuePair<int, GameIconConfigModel> item in GameIconInFoDic)
				{
					if ((int)item.Value.fiveElement == fiveElement && (int)item.Value.index == index && (int)item.Value.type == type)
					{
						string path = $"{item.Value.path}/{item.Value.name}{item.Value.extendPath}.png";
						result = Singleton<ResourceManager>.Instance.LoadResource<Sprite>(path);
						return result;
					}
				}
			}
			return result;
		}

		public Dictionary<string, RoleLevelModel> SelectRoleLevelInfo()
		{
			return RoleInFoDic;
		}

		public Dictionary<string, WorldHomeModel> SelectDongFuLevelInfo()
		{
			return DongFuInFoDic;
		}

		public Dictionary<string, JuLingLevelModel> SelectJuLingLevelInfo()
		{
			return JuLingZhenInFoDic;
		}

		public Dictionary<string, KongLingLevelModel> SelectKongLingLevelInfo()
		{
			return KongLingZhenInFoDic;
		}

		public Dictionary<int, DanShiLevelInfoModel> SelectDanShiLevelInfo()
		{
			return DanShiLevelDic;
		}

		public Dictionary<int, XzjWorldAsserssAchievement> SelectBuDaoLevelInfo()
		{
			return BuDaoLevelDic;
		}

		public Dictionary<int, XzjWorldAsserssAchievement> SelectLingTianLevelInfo()
		{
			return BuDaoLevelDic;
		}

		public List<AcupointModel> SelectAcupointModels()
		{
			return m_AcupointList;
		}

		public Dictionary<int, XzjWorldAssessMagicModel> SelectShuShiLevelInfo()
		{
			return ShuShiLevelDic;
		}

		public List<WorldHandSealModel> SelectMagicModels()
		{
			return m_FaShuInfoList;
		}

		public Dictionary<int, XzjWorldAssessSign> SelectSignLevelInfo()
		{
			return m_SignLevelDic;
		}

		public Dictionary<int, XzjWorldAsserssLianQi> SelectLianQiLevelInfo()
		{
			return m_LianQiLevelDic;
		}

		public Dictionary<int, XzjWorldAsserssZhenFa> SelectZhenFaLevelInfo()
		{
			return m_ZhenFaLevelDic;
		}

		public Dictionary<string, XzjWorldSecondaryOccupation> SelectFuZhiInfo()
		{
			return m_FuZhiDic;
		}

		public Dictionary<int, XzjWorldPlant> SelectPlantLevelInfo()
		{
			return m_LingZhiDic;
		}

		public Dictionary<int, MissionEntity> SelectMissionInfo()
		{
			return m_MissionDic;
		}

		public Dictionary<int, Sprite> SelectGoodsSprite()
		{
			return m_GoodsIconSprite;
		}

		public Dictionary<int, Sprite> SelectGoodsRankBg()
		{
			return m_GoodsRankBgSprite;
		}

		public CoefficientNumEntity SelectLianDanCoefficientNum(int rank, int level)
		{
			rank = WorldSetting.GetLevelByRank(rank);
			new CoefficientNumEntity();
			return m_CoefficientLianDanList.FindAll((CoefficientNumEntity s) => s.Rank.Equals(rank)).Find((CoefficientNumEntity s) => s.Level.Equals(level));
		}

		public CoefficientNumEntity SelectHuaFuCoefficientNum(int rank, int level)
		{
			rank = WorldSetting.GetLevelByRank(rank);
			new CoefficientNumEntity();
			return m_CoefficientHuaFuList.FindAll((CoefficientNumEntity s) => s.Rank.Equals(rank)).Find((CoefficientNumEntity s) => s.Level.Equals(level));
		}

		public Dictionary<int, XuanTianLouEntity> SelectXuanTianLouGoodsInfo()
		{
			return m_XuanTianLouGoodsDic;
		}

		public XuanTianLouEntity SelectXuanTianLouGoodsInfoByTempleteId(string templeteId)
		{
			if (string.IsNullOrEmpty(templeteId))
			{
				return null;
			}
			if (m_XuanTianLouTempleteGoodsDic.ContainsKey(templeteId))
			{
				return m_XuanTianLouTempleteGoodsDic[templeteId];
			}
			return null;
		}

		public Dictionary<int, ChengHaoInfoEntity> SelectChengHaoInfo()
		{
			return m_ChengHaoInfoDic;
		}

		public Dictionary<int, CashLiBaoGoodsEntity> SelectCashGoodsLiBaoInfo()
		{
			return m_CashGoodsLiBaoDic;
		}

		public Dictionary<int, List<EventGoodsEntity>> SelectEventGoods()
		{
			return m_EventGoodsDic;
		}

		public Dictionary<int, VipInfoEntity> SelectVipInfo()
		{
			return m_VipInfoDic;
		}

		public Dictionary<int, ZM_CangKuUpdateEntity> SelectCangKuUpdateEntity()
		{
			return m_ZM_CangKuUpdateDic;
		}

		public Dictionary<int, ZM_NeiMenUpdateEntity> SelectNeiMenUpdateEntity()
		{
			return m_ZM_NeiMenUpdateDic;
		}

		public Dictionary<int, ZM_FuZhiUpdateEntity> SelectFuZhiUpdateEntity()
		{
			return m_ZM_FuZhiUpdateDic;
		}

		public Dictionary<int, ZM_LianDanUpdateEntity> SelectLianDanUpdateEntity()
		{
			return m_ZM_LianDanUpdateDic;
		}

		public Dictionary<int, ZM_YanWuChangUpdateEntity> SelectYanWuChangUpdateEntities()
		{
			return m_ZM_YanWuChangUpdateDic;
		}

		public Dictionary<int, ZM_KuangMaiUpdateEntity> SelectKuangMaiUpdateEntities()
		{
			return m_ZM_KuangMaiUpdateDic;
		}

		public Dictionary<int, List<XuanTianLouEntity>> SelectZongMeCangBaoGeGoodsEntities()
		{
			return m_ZM_CangBaoGeGoodsDic;
		}

		public List<XuanTianLouEntity> SelectZongMeCangBaoGeGoodsByTypeEntities(int type)
		{
			if (m_ZM_CangBaoGeGoodsDic.ContainsKey(type))
			{
				return m_ZM_CangBaoGeGoodsDic[type];
			}
			return null;
		}
	}
}
