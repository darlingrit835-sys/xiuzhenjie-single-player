namespace XiuZhenJie
{
	public class NetInterfaceConstants
	{
		public const string HTTP_BEGIN_UPLOAD_AB = "/client/update/open/lock";

		public const string HTTP_END_UPLOAD_AB = "/client/update/close/lock";

		public const string HTTP_UPLOAD_AB = "/client/update/cos/upload";

		public const string HTTP_GET_SERVER_STATUS = "/gameService/statusSearch";

		public const string HTTP_INITGAMEINFO = "/client/data/queryClientDataModel";

		public const string HTTP_LOGIN = "/login";

		public const string HTTP_SDK_LOGIN = "/channel/login";

		public const string HTTP_GONGGAO = "/announcement/queryInfo";

		public const string HTTP_GONGGAO_ISSHOW = "/announcement/isShow";

		public const string HTTP_REGISTER = "/user/register";

		public const string HTTP_CHONGSHENG = "/user/roleSuicide";

		public const string HTTP_FORGETPAS = "/user/pwdUpdate";

		public const string HTTP_SENDCHECKCODE = "/user/sendCheckCode";

		public const string HTTP_SENDCDKEY = "/login/roleCdKey/cdkeyExchange";

		public const string HTTP_ERROR_UPDATE = "/client/log/error";

		public const string HTTP_PAY_CREATEORDER = "/pay/createOrder";

		public const string HTTP_PAY_UPDATEORDER = "/pay/updatePayStatus";

		public const string HTTP_PAY_SELECT_LINGYU_STATUS = "/pay/firstRechargeList";

		public const string HTTP_GONGLUE_OPEN = "/externalLinks/query";

		public const string HTTP_HUIGUI_DUIHUAN = "/login/roleCdKey/returnGiftBag";

		public const string SOCKET_CHONGSHENG_ACTION = "role/survivalStatusUpdate";

		public const string SOCKET_GAMEINFO_INIT = "world/client/data/queryClientDataModel";

		public const string SOCKET_DUIHUANJIANGLI = "role/cdkeyExchange";

		public const string SOCKET_ROLE_LOGIN = "role/login";

		public const string SOCKET_ROLE_QUERYROLEINFO = "role/queryRoleInfo";

		public const string SOCKET_ROLE_CREATE = "role/createRole";

		public const string SOCKET_ROLE_CHECKTROLENAME = "role/checkRoleName";

		public const string SOCKET_ROLE_EMALIQUERY = "role/queryletter";

		public const string SOCKET_ROLE_READEMAIL = "role/readLetter";

		public const string SOCKET_ROLE_DELETEMAIL = "role/deleteLetter";

		public const string SOCKET_ROLE_SPACE = "role/querySpaceInfoList";

		public const string SOCKET_ROLE_USEGOODS = "role/useGoods";

		public const string SOCKET_ROLE_DISCARDGOODS = "role/discardGoods";

		public const string SOCKET_ROLE_SOLEGOODS = "role/sellGoods";

		public const string SOCKET_ROLE_ZHUANGBEITUOXIA = "role/deleteEquipment";

		public const string SOCKET_ROLE_DONGFUSELECT = "role/useingAchievementOrMatrix";

		public const string SOCKET_ROLE_OPENCLOSEKONGLING = "role/adjustFileElements";

		public const string SOCKET_ROLE_CANWUACTION = "role/changeInfoByType";

		public const string SOCKET_ROLE_YIWANGFASHU = "role/forgetSkill";

		public const string SOCKET_ROLE_YIWANGGONGFA = "role/forgetAchievement";

		public const string SOCKET_ROLE_YIWANGDANFANG = "role/forgetElixir";

		public const string SOCKET_ROLE_YIWANGFUSHU = "role/forgetSign";

		public const string SOCKET_ROLE_CANWUSELECT = "role/queryAllMethodInfo";

		public const string SOCKET_ROLE_DONGFUBALL = "role/getExperienceValue";

		public const string SOCKET_ROLE_SHENGJIDONGFU = "role/buyHome";

		public const string SOCKET_ROLE_UPDATEZHENFA = "role/upgradeMatrix";

		public const string SOCKET_ROLE_GONGFALEVELUPDATE = "role/queryAchievementLevel";

		public const string SOCKET_ROLE_GENGHUANGONGFALIST = "role/queryMayUseAchievementMethodList";

		public const string SOCKET_ROLE_GENGHUANGONGFAACTION = "role/changeAchievementMethod";

		public const string SOCKET_ROLE_GENGHUANGONGFATYPE = "role/changeRunningLocation";

		public const string SOCKET_ROLE_ZHOUTIANACTION = "role/updateRunAchievement";

		public const string ROLE_RUNACHIEVEMENT_ON_SUPERAIR = "role/runAchievementOnSuperAir";

		public const string ROLE_RUNACHIEVEMENT_ON_BOOLD = "role/runAchievementOnBoold";

		public const string ROLE_RUNACHIEVEMENT_ON_UNREAL = "role/runAchievementOnUnreal";

		public const string ROLE_NAQITUPO_XUNWEN = "role/breakXunWen";

		public const string SOCKET_ROLE_NAQITUPO = "role/breakRealm";

		public const string SOCKET_ROLE_SETTINGCOMBATINFO = "role/querySettingCombat";

		public const string SOCKET_ROLE_SETTINGITEMINFO = "role/queryRoleCombatInfo";

		public const string SOCKET_ROLE_SETTINGACTION = "role/settingCombat";

		public const string SOCEKT_WORLD_MAPCHUANSONG = "role/changeMapId";

		public const string SOCKET_WORLD_MAPBROTHER = "world/queryMapSibingsInfoMapId";

		public const string SOCKET_WORLD_MAPCHILDS = "world/queryMapchildrenIdsInfoMapId";

		public const string SOCKET_WORLD_MAPCHILDSBYTYPE = "world/queryInfoType";

		public const string SOCKET_WORLD_QUERYMAPINFO = "world/queryInfoMapId";

		public const string SOCKET_WORLD_MAPPARENTIDS = "world/queryParentsMapIdListByMapId";

		public const string SOCKET_WORLD_MAPPARENT_MODEL = "world/queryParentsMapInfoListByMapId";

		public const string SOCKET_WORLD_CREATEMONSTER = "world/createMonster";

		public const string SOCKET_WORLD_GETROLEBATTLESETTINGINFO = "role/queryCombatSettingInfoListByRoleId";

		public const string SOCKET_WORLD_GETMONSTERRECORD = "role/recordMonster";

		public const string SOCKET_WORLD_GETGOODS = "role/getGoodsInfo";

		public const string SOCKET_WORLD_USEDANYAO = "role/useElixir";

		public const string SOCKET_SHOP_SELECTGOODS = "world/shop/queryShopGoodsByMapId";

		public const string XuanTianLou_SOCKET_SHOP_SELECTGOODS = "world/shinyJadeShop/queryShopGoods";

		public const string SOCKET_ROLE_BUYGOODS = "role/buyGoods";

		public const string SOCKET_ROLE_BUYCAOYAO = "role/buyBookAchievement";

		public const string SOCKET_ROLE_BUYGONGFA = "role/buyBookAchievement";

		public const string SOCKET_ROLE_BUYJINENG = "role/buyBookSkill";

		public const string SOCKET_ROLE_BUYZHUANGBEI = "role/buyGoodsEquipment";

		public const string SOCKET_ROLE_BUYDANYAO = "role/buyElixir";

		public const string SOCKET_ROLE_BUYDANFANG = "role/buyBookElixir";

		public const string SOCKET_ROLE_BUYELIXIR = "role/xuanTianLou/buyElixir";

		public const string ROLE_BUY_YUEKA = "role/buyYueKa";

		public const string ROLE_BUY_GuangGao = "role/lookAdvertising";

		public const string ROLE_BUY_GuangGaoPanDuan = "role/whetherLookAdvertising";

		public const string ROLE_BUY_WuDaoTai = "role/upgrade/achievement";

		public const string ROLE_BUY_ZhanFaShi = "role/upgrade/skill";

		public const string SOCKET_ROLE_LIXIANSHOUYI = "role/calculateOfflineValue";

		public const string SOCKET_ROLE_HUIFUJINGLI = "role/recoverVigor";

		public const string SOCKET_ROLE_HUIFUWUXING = "role/recoverClever";

		public const string SOCKET_PAIMAI_ENTRUST_AUCTION = "role/entrustAuction";

		public const string SOCKET_PAIMAI_SELECTGOODS = "world/paiMaiHang/searchWuPinList";

		public const string SOCKET_LETTER_GET = "role/getLetterAward";

		public const string SOCKET_CHUWUXIANG_PUT = "role/getGoodsByBox";

		public const string SOCKET_CHUWUXIANG_GET = "role/deleteGoodsByBox";

		public const string SOCKET_QUERY_ROLE_INFO = "role/queryRoleInfo";

		public const string SOCKET_PAIMAI_YIKOUJIA = "role/auctionBuy";

		public const string SOCKET_PAIMAI_GOODSXIAJIA = "role/auctionGoodsCancel";

		public const string SOCKET_PAIMAI_JINGPAI = "role/auctionBidding";

		public const string SOCKET_PAIMAI_WODEJINGPAI = "role/myBidGoods";

		public const string SOCKET_PAIMAI_WODEPAIMAI = "role/myAuctionGoods";

		public const string SOCKET_ZHANDOUSETTING_CANCLESETTED = "role/cancelSettingCombat";

		public const string SOCKET_ZHUANGBEI_BINDING = "role/equipment/identityHuman";

		public const string SOCKET_LIANDAN_COMPELETE = "role/practiceArticeleElixir";

		public const string SOCKET_CHUWUXIANGSELECT = "role/query/storage/box";

		public const string SOCKET_WORLDCAILIAO = "world/xsjMaterial";

		public const string SOCKET_DST_KAOSHINEIRONG = "role/authentication/elixir/get/content";

		public const string SOCKET_DST_KAOSHI_COMPELETE = "role/authentication/elixir";

		public const string SOCKET_REDUCE_KAOHETIMES = "role/reduce/assess";

		public const string SOCKET_ZHUCEDANFANG = "role/register/elixir";

		public const string SOCKET_CHUANGZHAO_IS_SUCCESS = "role/elixirCreatedValidate";

		public const string SOCKET_DANFANGDUIHUAN_LIST = "world/xsj/searchElixirIdList";

		public const string ROLE_BUY_SHANGDIAN = "role/longmoShopSearch";

		public const string ROLE_BUY_YIJIANGOUMAI = "role/oneKeyBuyMaterials";

		public const string SOCKET_DANFANGDUIHUAN = "role/elixirCreatedExchange";

		public const string SOCKET_DANFANG_ZHUCE = "role/query/myself/elixir";

		public const string SOCKET_CHUANGZAO_RESULT = "role/elixirCreated";

		public const string SOCKET_CREATE_TOWER_MONSTER = "world/towerMonster";

		public const string SOCKET_PASS_TOWER_MONSTER = "role/pass/tower";

		public const string SOCKET_XUSHENTA_DAOJING_BANGDAN = "world/rankingListByLevel";

		public const string SOCKET_XUSHENTA_SUIYUE_BANGDAN = "world/rankingListByAge";

		public const string SOCKET_TUPODUJIE = "role/dujie";

		public const string SOCKET_GET_XUSHENTA_RECORD = "role/pass/maxFloorNumber";

		public const string SOCKET_ISCANCHUANGTA = "role/tower/authInto";

		public const string SOCKET_DANYAO_XIANGQING = "role/elixirUseNum";

		public const string SOCKET_FULU_XIANGQING = "role/signUseNum";

		public const string SOCKET_CHUANGZHAO_GONGFA = "role/achievementCreatedValidate";

		public const string SOCKET_ZHUCE_GONGFA = "role/register/achievement";

		public const string SOCKET_ZHUCE_GONGFALIST = "role/query/myself/achievement";

		public const string SOCKET_DUIHUAN_GONGFALIST = "world/xsj/searchAchievementList";

		public const string SOCKET_DUIHUAN_GONGFA = "role/achievementCreatedExchange";

		public const string SOCKET_DUIHUAN_FASHULIST = "world/xsj/searchSkillList";

		public const string SOCKET_DUIHUAN_FASHU = "role/skillCreatedExchange";

		public const string SOCKET_ZHUCE_FASHULIST = "role/query/myself/skill";

		public const string SOCKET_ZHUCE_FASHU = "role/register/skill";

		public const string SOCKET_CHUANGZHAO_FASHU = "role/skillCreatedValidate";

		public const string SOCKET_DRAW_SIGN = "role/drawingArticeleSign";

		public const string SOCKET_ROLE_AUCPOINT_REL = "role/aucpointRel";

		public const string SOCKET_ROLE_TUPO_ACUPOINT = "role/tuPoAcupoint";

		public const string SOCKET_ROLE_ZONGLAN = "role/tuPoAcupointCount";

		public const string SOCKET_ADD_KONGLING_TIME = "role/add/controllerMatrixTime";

		public const string SOCKET_ADD_JULING_TIME = "role/add/gatherMatrixTime";

		public const string SOCKET_OPEN_DONGFU = "role/open/occupation";

		public const string SOCKET_CLOSE_DONGFU = "role/destroy/occupation";

		public const string SOCKET_UPDATE_HOMENAME = "role/updateHomeName";

		public const string SOCKET_BUY_DONGFU_ROOM = "role/openGoodsByBox";

		public const string SOCKET_QUERY_CHUWUXIANG = "role/queryGoodsByBox";

		public const string SOCKET_UPDATE_NAJIE = "role/gather/space";

		public const string SOCKET_GET_GOODS_INFO = "role/space/goods/details";

		public const string SOCKET_GET_ZHUANGBEI_INFO = "role/equipment/details";

		public const string SOCKET_OFFINE = "role/gather/offline";

		public const string ROLE_UPDATE_GUIDANCE = "role/updateGuidance";

		public const string ROLE_SAODANG = "role/sweep/monster";

		public const string ROLE_ENTER_MAP = "role/go/map";

		public const string ROLE_CREATE_MJ_MONSTER = "world/miJing/createMonster";

		public const string ROLE_ENTER_MIJING = "role/jinru/mijing";

		public const string ROLE_GET_HANDSEAL = "world/client/data/queryHandSealList";

		public const string ROLE_UPDATE_BATTLESETTING = "role/open/settingLocation";

		public const string ROLE_ZHONGSHANG_OVERTIME = "role/sunShang/overtime";

		public const string ROLE_JINGMAI_ADDITION = "role/acupointJiaCheng";

		public const string ROLE_ACHIEVEMENT_DETOXIFY = "role/achievement/detoxify";

		public const string ROLE_TITLE_USE = "role/tileUse";

		public const string ROLE_FANGQI_ZHONGSHANG = "role/abandonSunShang";

		public const string ROLE_REBUILD_DONGFU = "role/update/occupation";
	}
}
