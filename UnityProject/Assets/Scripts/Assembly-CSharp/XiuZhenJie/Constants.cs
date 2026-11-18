namespace XiuZhenJie
{
	public class Constants
	{
		public const string VersonCode = "1.3.5";

		public static string LoginServerIP = "127.0.0.1";

		public static int LoginServerPort = 9443;

		public static bool UsesLocalServer => LoginServerIP == "127.0.0.1" || string.Equals(LoginServerIP, "localhost", System.StringComparison.OrdinalIgnoreCase);

		public static string GameKey = "40027";

		public static string SocketServerIP = "";

		public static int SocketServerPort = 0;

		public static string SocketServerName = "";

		public static string PayIp = "";

		public static int PayPort = 0;

		public const string APP_ID = "q015x8jjq4pjru39";

		public const string APP_CHANNEL = "Tap";

		public const string APP_4399_CHANNEL = "m_4399";

		public const string RESOURCELOAD_WINDOW = "ResourceLoadWnd.prefab";

		public const string LOADING_WINDOW = "LoadingWnd.prefab";

		public const string STARTLOADSCENE_WINDOW = "StartLoadSceneWnd.prefab";

		public const string POPUP_WINDOW = "PopUpWnd.prefab";

		public const string ATTENTION_WINDOW = "AttentionWnd.prefab";

		public const string OPTION_WINDOW = "OptionWnd.prefab";

		public const string NETMESSAGE_WINDOW = "NetMessageWnd.prefab";

		public const string LIXIANSHOUY_WINDOW = "LiXianShouYiWnd.prefab";

		public const string LOGO_WINDOW = "StartLogoWnd.prefab";

		public const string MASK_WINDOW = "MaskWnd.prefab";

		public const string GUIDE_WINDOW = "GuideWnd.prefab";

		public const string COMMON_SHAIXUAN_WINDOW = "Common_ShaiXuanWnd.prefab";

		public const string COMMON_ZHIFU_WINDOW = "Common_ZhiFuWnd.prefab";

		public const string COMMON_TIP_WINDOW = "Common_TipWnd.prefab";

		public const string COMMON_QiRen_WINDOW = "Common_QiRenWnd.prefab";

		public const string COMMON_CHOOSEGOODS_WINDOW = "Common_ChooseGoodsNumWnd.prefab";

		public const string COMMON_WEB_WINDOW = "Common_WebWnd.prefab";

		public const string SIGNIN_WINDOW = "SignInWnd.prefab";

		public const string SIGNUP_WINDOW = "SignUpWnd.prefab";

		public const string FORGETPAS_WINDOW = "ForgetPasWnd.prefab";

		public const string CREATEROLE_WINDOW = "CreateRoleWnd.prefab";

		public const string CHOOSESERVER_WINDOW = "ChooseServerWnd.prefab";

		public const string CEHUA_WINDOW = "CeHuaWnd.prefab";

		public const string CHENGZHUFU_WINDOW = "ChengZhuFuWnd.prefab";

		public const string CHUANSONGDAZHEN_WINDOW = "ChuanSongDaZhenWnd.prefab";

		public const string DIHUOMISHI_WINDOW = "DiHuoMiShiWnd.prefab";

		public const string MAINCITY_WINDOW = "MainCityWnd.prefab";

		public const string MAINTITLE_WINDOW = "MainTitleWnd.prefab";

		public const string SHENGXIANTA_WINDOW = "ShengXianTaWnd.prefab";

		public const string WAICHULILIAN_WINDOW = "WaiChuLiLianWnd.prefab";

		public const string ZHONGJIEFANGSHI_WINDOW = "ZhongJieFangShiWnd.prefab";

		public const string YOUJIANDONGFU_WINDOW = "YouJianDongFuWnd.prefab";

		public const string CHENGNEIMIJING_WINDOW = "ChengNeiMiJingWnd.prefab";

		public const string HUODONG_WINDOW = "HuoDongWnd.prefab";

		public const string CHUANXIN_WINDOW = "ChuanXinWnd.prefab";

		public const string DUIHUANLINGYU_WINDOW = "DuiHuanLingYuWnd.prefab";

		public const string LIPIN_WINDOW = "LinPinWnd.prefab";

		public const string DUIHUANLINGSHI_WINDOW = "DuiHuanLingShiWnd.prefab";

		public const string CHONGSHENG_WINDOW = "ChongShengWnd.prefab";

		public const string CHOOSE_CHENGHAO_WINDOW = "ChooseChengHaoWnd.prefab";

		public const string LINGQUANJIUSI_WINDOW = "LingQuanJiuSiWnd.prefab";

		public const string DUIHUANLINGSHINEW_WINDOW = "DuiHuanLingShiNewWnd.prefab";

		public const string QIANDAO_WINDOW = "QianDaoWnd.prefab";

		public const string DAOYOU_WINDOW = "DaoYouWnd.prefab";

		public const string DAOYOUCHAT_WINDOW = "DaoYouChatWnd.prefab";

		public const string DAOYOUCHUANWU_WINDOW = "DaoYouChuanWuWnd.prefab";

		public const string LEVEL_RANK_WINDOW = "LevelRankWnd.prefab";

		public const string HUIGUI_DUIHUAN_WINDOW = "HuiGuiDuiHuanWnd.prefab";

		public const string LIANQIFANG_WINDOW = "LianQiFangWnd.prefab";

		public const string LIANQICREATECHOOSESTAGE_WINDOW = "LianQiCreateChooseStageWnd.prefab";

		public const string LIANQI_BUZHOU_SHEZHI_WINDOW = "LianQiBuZhouSheZhiWnd.prefab";

		public const string LIANQI_CAILIAO_XUANZHE_SHAIXUAN_WINDOW = "LianQiCaiLiaoXuanZeShaiXuanWnd.prefab";

		public const string LIANQI_CREATE_RESULT_WINDOW = "LianQiCreateResultWnd.prefab";

		public const string CHOUJIANG_EVENT_WINDOW = "ChouJiangEventWnd.prefab";

		public const string JIFEN_DUIHUAN_WINDOW = "JiFenDuiHuanWnd.prefab";

		public const string WORLDMAP_WINDOW = "WorldMapWnd.prefab";

		public const string DONGFU_WINDOW = "DongFuWnd.prefab";

		public const string GENGHUANGONGFA_WINDOW = "GengHuanGongFaWnd.prefab";

		public const string KONGLINGFAZHEN_WINDOW = "KongLingFaZhenWnd.prefab";

		public const string JULINGFAZHEN_WINDOW = "JuLingFaZhenWnd.prefab";

		public const string GENGHUANZHENFADENGJIE_WINDOW = "GengHuanFaZhenDengJieWnd.prefab";

		public const string FAZHENXIAOHAOLINGSHI_WINDOW = "FaZhenXiaoHaoLingShiWnd.prefab";

		public const string DONGFUCHOOSE_WINDOW = "DongFuChooseWnd.prefab";

		public const string CANWU_WINDOW = "CanWuWnd.prefab";

		public const string CANWUZHONG_WINDOW = "CanWuZhongWnd.prefab";

		public const string CANWU_GONGFA_WINDOW = "CanWuGongFaWnd.prefab";

		public const string TONGMAI_WINDOW = "TongMaiWnd.prefab";

		public const string HUAFU_WINDOW = "HuaFuWnd.prefab";

		public const string HUAFU_CHOOSE_WINDOW = "HuaFuChooseWnd.prefab";

		public const string JINGMAI_WINDOW = "JingMaiWnd.prefab";

		public const string JINGSHI_WINDOW = "JingShiWnd.prefab";

		public const string CHUWUXIANG_WINDOW = "ChuWuXiangWnd.prefab";

		public const string DONGFUMISHI_WINDOW = "DongFuMiShiWnd.prefab";

		public const string TUPODUJIE_WINDOW = "TuPoDuJieWnd.prefab";

		public const string TUPODUJIE_SUCCESS_WINDOW = "TuPoDuJieSuccessWnd.prefab";

		public const string TUPODUJIE_FAIL_WINDOW = "TuPoDuJieFailWnd.prefab";

		public const string DONGFUROOM_WINDOW = "DongFuRoomWnd.prefab";

		public const string DONGFUZHIFU_WINDOW = "DongFuZhiFuWnd.prefab";

		public const string DONGFUYONGTUCHOOSE_WINDOW = "DongFuYongTuChooseWnd.prefab";

		public const string ZHONGSHANG_SHOW_WINDOW = "ZhongShangShowWnd.prefab";

		public const string HUAFUCHOOSE_LK_WINDOW = "HuaFuChoose_LK_Wnd.prefab";

		public const string NEISHI_WINDOW = "NeiShiWnd.prefab";

		public const string ZONGLAN_WINDOW = "ZongLanWnd.prefab";

		public const string QZG_WODEPAIMAI_WINDOW = "QZG_WoDePaiMaiWnd.prefab";

		public const string QZG_WODEJINGPAI_WINDOW = "QZG_WoDeJingPaiWnd.prefab";

		public const string QZG_CHOOSE_WINDOW = "QZG_ChooseWnd.prefab";

		public const string QZG_MAIN_WINDOW = "QiZhenGeWnd.prefab";

		public const string QZG_ITEM_INFORMAITON_WINDOW = "QZG_ItmeInformation.prefab";

		public const string QZG_SHAIXUAN_WINDOW = "QZG_ShaiXuan.prefab";

		public const string QZG_PAIMAI_WINDOW = "QZG_PaiMaiWnd.prefab";

		public const string QZG_PAIMAI_ITEM_WINDOW = "QZG_PaiMaiWuPinXinXiWnd.prefab";

		public const string QZG_GOODSINFO_WINDOW = "QZG_GoodsInformationWnd.prefab";

		public const string SHANGPUXIANGQING_WINDOW = "ShangPuWuPinXiangQingWnd.prefab";

		public const string SHANGPU_WINDOW = "ShangPuWnd.prefab";

		public const string KEYBOARD_WINDOW = "KeyBoardWnd.prefab";

		public const string AH_ITEM = "Assets/GameData/Prefabs/Items/AH_Item.prefab";

		public const string AH_MAINSCENE_WINDOW = "AH_MainSceneWnd.prefab";

		public const string AH_WODEGUAPAI_WINDOW = "AH_WoDeGuaPaiWnd.prefab";

		public const string AH_WODEPAIMAO_WINDOW = "AH_WoDePaiMaoWnd.prefab";

		public const string AH_PAIPINXIANGQING_WINDOW = "AH_PaiPinXiangQingWnd.prefab";

		public const string AH_PAIMAIHU_WINDOW = "AH_PaiMaiHuWnd.prefab";

		public const string AH_PAIPINLIST_WINDOW = "AH_PaiPinListWnd.prefab";

		public const string AH_PAIPINGXIANGQING_WINDOW = "AH_PaiPingXiangQingWnd.prefab";

		public const string DIHUOGE_WINDOW = "DiHuoGeWnd.prefab";

		public const string DIHUOGE_LIANZHIBUZHOU_SHEZHI_WINDOW = "DiHuoGeLianZhiBuZhouSheZhiWnd.prefab";

		public const string DIHUOGE_KAISHILIANZHI_WINDOW = "DiHuoGeLianZhiWnd.prefab";

		public const string DIHUOGE_CAILIAOSHAIXUAN_WINDOW = "DiHuoGeCaiLiaoShaiXuanWnd.prefab";

		public const string DIHUOGE_CAILIAOSHAIXUAN_LK_WINDOW = "DiHuoGeCaiLiaoShaiXuan_LK_Wnd.prefab";

		public const string DIHUOGE_CAILIAOCHOOSE_WINDOW = "DiHuoGeCaiLiaoChooseWnd.prefab";

		public const string DIHUOGE_JIESUAN_WINDOW = "DiHuoGeLianDanJieSuanWnd.prefab";

		public const string DIHUOGE_WORLD_CAILIAO_WINDOW = "WorldCaiLiaoWnd.prefab";

		public const string DIHUOGE_LIANZHIDENGJIE_CHOOSE_WINDOW = "ChuangZhao_LianZhiDengJieWnd.prefab";

		public const string DIHUOGE_CAILIAOJIAGE_WINDOW = "DiHuoGeCaiLiaoJiaGeWnd.prefab";

		public const string LIANQICHOOSE_WINDOW = "LianQiChooseWnd.prefab";

		public const string LIANQICAILIAOSHAIXUAN_WINDOW = "LianQiCaiLiaoShaiXuanWnd.prefab";

		public const string LIANQICAILIAOSHAIXUAN_LK_WINDOW = "LianQiCaiLiaoShaiXuan_LK_Wnd.prefab";

		public const string LIANQIJIESUAN_CHENGGONG_WINDOW = "LianQiJieSuan_ChengGongWnd.prefab";

		public const string LIANQIJIESUAN_SHIBAIWND_WINDOW = "LianQiJieSuan_ShiBaiWnd.prefab";

		public const string LianQi_ChuangZaoMian_WINDOW = "LianQi_ChuangZaoMianWnd.prefab";

		public const string LianQi_DuiHuanWnd_WINDOW = "LianQi_DuiHuanWnd.prefab";

		public const string LQ_QiTuDuiHuanTipWnd_WINDOW = "LQ_QiTuDuiHuanTipWnd.prefab";

		public const string LIANQI_ZHUCE_WINDOW = "LianQi_ZhuCeWnd.prefab";

		public const string LQ_ZhuCeTipWnd_WINDOW = "LQ_ZhuCeTipWnd.prefab";

		public const string ChengZhuFuChengWeiKeQing_WINDOW = "ChengZhuFuChengWeiKeQingWnd.prefab";

		public const string XUANTIANLOU_WINDOW = "XuanTianLouWnd.prefab";

		public const string XUANTIANLOUXiangQing_WINDOW = "XuanTianLouXiangQingWnd.prefab";

		public const string XUANTIANLing_WINDOW = "XuanTianLingWnd.prefab";

		public const string GuangGao_WINDOW = "GuangGaoWnd.prefab";

		public const string WuDaoTai_WINDOW = "WuDaoTaiWnd.prefab";

		public const string WuDaoTaiItem_WINDOW = "WuDaoTaiIXiangQingWnd.prefab";

		public const string ZhanFaShiXiangQing_WINDOW = "ZhanFaShiXiangQingWnd.prefab";

		public const string ZhanFaShi_WINDOW = "ZhanFaShiWnd.prefab";

		public const string LT_ZHONGZI_WINDOW = "LT_ZhongZiWnd.prefab";

		public const string LT_LINGTIANMAI_WINDOW = "LT_LingTianMainWnd.prefab";

		public const string LT_LINGTU_WINDOW = "LT_LingTuWnd.prefab";

		public const string LT_TURANGGENHUAN_WINDOW = "LT_TuRangGenHuanWnd.prefab";

		public const string LT_SHOHUO_WINDOW = "LT_ShoHuoWnd.prefab";

		public const string LT_JIAOGUAN_WINDOW = "LT_JiaoGuanWnd.prefab";

		public const string LT_LINGYE_WINDOW = "LT_LingYeWnd.prefab";

		public const string LingTian_LingTian_Icon = "Assets/GameData/Textures/Item/LingTian_LingTian.png";

		public const string LingTian_LingYe_Icon = "Assets/GameData/Textures/Item/LingTian_LingYe.png";

		public const string LingTian_ZhoZi_Icon = "Assets/GameData/Textures/Item/LingTian_ZhoZi.png";

		public const string FAQIXILIAN_WINDOW = "FaQiXiLianWnd.prefab";

		public const string XiaoWenHao_WINDOW = "XiaoWenHaoWnd.prefab";

		public const string NAJIE_TIP_WINDOW = "NaJieTipWnd.prefab";

		public const string NAJIEONEKEYSOLE_WINDOW = "NaJieOneKeySoleWnd.prefab";

		public const string NAJIE_SPACE_WINDOW = "NaJieSpaceWnd.prefab";

		public const string HE_CHENG_WINDOW = "HeChengWnd.prefab";

		public const string ZHANDOU_SHEZHI_WINDOW = "DouMenSheZhiWnd.prefab";

		public const string ROLE_ZHUANGBEI_WINDOW = "RoleZhuangBeiWnd.prefab";

		public const string BattleSetting_Window = "BattleSettingWnd.prefab";

		public const string BattleSettingDetail_Window = "BattleSettingDetailWnd.prefab";

		public const string ROLE_ZHUANGBEI_TIP_WINDOW = "RoleZhuangBeiTipWnd.prefab";

		public const string CAILIAO_SHAIXUAN_WINDOW = "CaiLiaoXuanZeShaiXuanWnd.prefab";

		public const string ZHANDOU_WINDOW = "ZhanDou.prefab";

		public const string ZHANDOU_JIEGUO_WINDOW = "ZhaoDouJieGuo.prefab";

		public const string ZHANDOU_DIAOLUO_WINDOW = "GuaiWuDiaoLuo.prefab";

		public const string BATTLE_WINDOW = "BattleWnd.prefab";

		public const string BATTLE_END_WINDOW = "ZhanDouSettlementWnd.prefab";

		public const string XUSHENJIE_WINDOW = "XuShenJieWnd.prefab";

		public const string XUSHENTA_WINDOW = "XuShenTaWnd.prefab";

		public const string XUSHENTABANGDAN_WINDOW = "XuShenTaBangDanWnd.prefab";

		public const string XUSHENDIAN_WINDOW = "XuShenDianWnd.prefab";

		public const string DanShiTa_WINDOW = "DanShiTaWnd.prefab";

		public const string DanShiTaRengZhen_WINDOW = "DST_DanShiRenZhengWnd.prefab";

		public const string DanShiTaZhuCe_WINDOW = "DST_ZhuCeWnd.prefab";

		public const string DanFangXinXi_WINDOW = "DanFangXinXiWnd.prefab";

		public const string DanFangDuiHuan_WINDOW = "DST_DanFangDuiHuanWnd.prefab";

		public const string DanFangDuiHuan_SHAIXUAN_WINDOW = "DST_DanFangDuiHuanShaiXuanWnd.prefab";

		public const string DanFangDuiHuan_TIP_WINDOW = "DST_DanFangDuiHuanTipWnd.prefab";

		public const string ChuangTa_WINDOW = "DST_ChuangTaWnd.prefab";

		public const string BangDan_WINDOW = "DST_BangDanWnd.prefab";

		public const string DST_ZHUCETIP_WINDOW = "DST_ZhuCeTipWnd.prefab";

		public const string WFG_ZHUCE_WINDOW = "WFG_ZhuCeWnd.prefab";

		public const string WFG_ZHUCETIP_WINDOW = "WFG_ZhuCeTipWnd.prefab";

		public const string DJSD_ZHUCE_WINDOW = "DJSD_ZhuCeWnd.prefab";

		public const string DJSD_ZHUCETIP_WINDOW = "DJSD_ZhuCeTipWnd.prefab";

		public const string CHUANGZHAOGONGFA_WINDOW = "ChuangZhaoGongFaWnd.prefab";

		public const string CHUANGZHAOGONGFA_JIEGUO_WINDOW = "ChuangZhaoGongFaJieGuoWnd.prefab";

		public const string XSJ_ZHUCE_WINDOW = "XSJ_ZhuCeWnd.prefab";

		public const string XSJ_ZHUCE_TIP_WINDOW = "XSJ_ZhuCeTipWnd.prefab";

		public const string DAOJINGSHENGDIAN_WINDOW = "DaoJingShengDianWnd.prefab";

		public const string DJSD_GONGFADUIHUAN_WINDOW = "DJSD_GongFaDuiHuanWnd.prefab";

		public const string DJSD_GONGFADUIHUAN_SHAIXUAN_WINDOW = "DJSD_GongFaDuiHuanShaiXuanWnd.prefab";

		public const string DJSD_GONGFADUIHUAN_TIP_WINDOW = "DJSD_GongFaDuiHuanTipWnd.prefab";

		public const string DJSD_YIZHUCELIST_WINDOW = "DJSD_YiZhuCeListWnd.prefab";

		public const string WANFAGE_WINDOW = "WanFaGeWnd.prefab";

		public const string WFG_FASHUDUIHUAN_SHAIXUAN_WINDOW = "WFG_FaShuDuiHuanShaiXuanWnd.prefab";

		public const string WFG_FASHUDUIHUAN_WINDOW = "WFG_FaShuDuiHuanWnd.prefab";

		public const string WFG_FASHUDUIHUAN_TIP_WINDOW = "WFG_FaShuDuiHuanTipWnd.prefab";

		public const string CHUANGZHAOFASHU_WINDOW = "ChuangZaoFaShuWnd.prefab";

		public const string CHUANGZHAOFASHU_JIEGUO_WINDOW = "ChuangZhaoFaShuJieGuoWnd.prefab";

		public const string XSJ_COMMON_ZHUCE_WINDOW = "XSJ_CommonZhuCeWnd.prefab";

		public const string SFH_SHENFUHUI_WINDOW = "SFH_ShenFuHuiWnd.prefab";

		public const string SFH_FUSHUDUIHUAN_WINDOW = "SFH_FuShuDuiHuanWnd.prefab";

		public const string SFH_FUSHUDUIHUANTIP_WINDOW = "SFH_FuShuDuiHuanTipWnd.prefab";

		public const string SFH_ZHUCE_WINDOW = "SFH_ZhuCeWnd.prefab";

		public const string SFH_ZHUCETIP_WINDOW = "SFH_ZhuCeTipWnd.prefab";

		public const string SFH_CREATE_WINDOW = "SFH_CreateWnd.prefab";

		public const string SFH_CREATE_CHOOSE_WINDOW = "SFH_CreateChooseWnd.prefab";

		public const string SFH_CREATE_RESULT_WINDOW = "SFH_CreateResultWnd.prefab";

		public const string LZ_CHUANGZAOW_WINDOW = "LZ_ChuangZaoWnd.prefab";

		public const string LZ_CAILIAOSHAIXUAN_WINDOW = "LZ_CaiLiaoShaiXuanWnd.prefab";

		public const string LZ_WORLDCAILIAO_WINDOW = "LZ_WorldCaiLiaoWnd.prefab";

		public const string LZ_MIAN_WINDOW = "LZ_MianWnd.prefab";

		public const string LZ_LINGZHIDUIHUANTIP_WINDOW = "LZ_LingZhiDuiHuanTipWnd.prefab";

		public const string LZ_LINGZHIDUIHUAN_WINDOW = "LZ_LingZhiDuiHuanWnd.prefab";

		public const string LZ_ZHUCETIP_WINDOW = "LZ_ZhuCeTipWnd.prefab";

		public const string LZ_ZHUCE_WINDOW = "LZ_ZhuCeWnd.prefab";

		public const string ZS_ZHUCE_WINDOW = "ZS_ZhuCeWnd.prefab";

		public const string ZS_MIAN_WINDOW = "ZS_MianWnd.prefab";

		public const string ZS_DUIHUANTIP_WINDOW = "ZS_DuiHuanTipWnd.prefab";

		public const string ZS_DUIHUAN_WINDOW = "ZS_DuiHuanWnd.prefab";

		public const string ZS_ZHUCETIP_WINDOW = "ZS_ZhuCeTipWnd.prefab";

		public const string ZS_CHUANGZHAO_WINDOW = "ZS_ChuangZhaoWnd.prefab";

		public const string ZF_LIANZHI_WINDOW = "ZF_LianZhiWnd.prefab";

		public const string ZF_CHOOSE_LK_WINDOW = "ZF_Choose_LK_Wnd.prefab";

		public const string ZF_CREATECHOOSE_WINDOW = "ZF_CreateChooseWnd.prefab";

		public const string CASH_Goods_WINDOW = "CashGoodsWnd.prefab";

		public const string XSJ_XuShenJiTan_WINDOW = "XuShenJiTanWnd.prefab";

		public const string XSJ_HeDaoTai_WINDOW = "HeDaoTaiWnd.prefab";

		public const string ChuanSongTiShi_WINDOW = "ChuanSongTiShiWnd.prefab";

		public const string SaoDang_Window = "ShaoDangWnd.prefab";

		public const string GuideNewWnd_WINDOW = "GuideNewWnd.prefab";

		public const string Exmple_DongFu_WINDOW = "DongFuExmpleWnd.prefab";

		public const string Exmple_DuJie_WINDOW = "TuPoDuJieExmpleWnd.prefab";

		public const string SheLiZongMen_Window = "SheLiZongMenWnd.prefab";

		public const string ZongMenChat_Window = "ZongMenChatWnd.prefab";

		public const string ZongMenChengYuan_Window = "ZongMenChengYuanWnd.prefab";

		public const string ZongMen_Window = "ZongMenWnd.prefab";

		public const string ZongMen_BaiRuShanMen_Window = "BaiRuShanMenWnd.prefab";

		public const string ZongMen_ShouChao_Window = "ZongMenShouChaoWnd.prefab";

		public const string ZongMen_ChouJiang_Window = "ZongMenChouJiangEventWnd.prefab";

		public const string ZONGMEN_DADIAN_WINDOW = "ZongMen_DaDianWnd.prefab";

		public const string ZONGMEN_DADIANXIEREN_WINDOW = "ZongMen_DaDianXieRenWnd.prefab";

		public const string ZONGMEN_HUZONGDAZHEN_WINDOW = "ZongMen_HuZongDaZhenWnd.prefab";

		public const string ZONGMEN_HUZONGDAZHENUPGRADE_WINDOW = "ZongMen_HuZongDaZhenUpgradeWnd.prefab";

		public const string ZONGMEN_KUANGMAIWND_WINDOW = "ZongMen_KuangMaiWnd.prefab";

		public const string ZONGMEN_WAIWUDIAN_WINDOW = "ZongMen_WaiWuDianWnd.prefab";

		public const string ZONGMEN_WAIWUDIANXIANGQING_WINDOW = "ZongMen_WaiWuDianXiangQingWnd.prefab";

		public const string ZONGMEN_YANWUTAIWND_WINDOW = "ZongMen_YanWuTaiWnd.prefab";

		public const string ZONGMEN_ZHANGLAO_WINDOW = "ZongMen_ZhangLaoWnd.prefab";

		public const string ZONGMEN_ZHANGLAORENMING_WINDOW = "ZongMen_ZhangLaoRenMingWnd.prefab";

		public const string ZONGMEN_ZHIFADIANWND_WINDOW = "ZongMen_ZhiFaDianWnd.prefab";

		public const string ZONGMEN_NEIWUDIAN_WINDOW = "ZongMen_NeiWuDianWnd.prefab";

		public const string TISHENGPINGJIE_WINDOW = "TiShengPingJieWnd.prefab";

		public const string ZONGMEN_SHIKONGMIJING_WINDOW = "ZongMen_ShiKongMiJingWnd.prefab";

		public const string ZONGMEN_ZHUFENG_WINDOW = "ZongMenZhuFengWnd.prefab";

		public const string ZongMen_LingTianWnd = "ZongMen_LingTianWnd.prefab";

		public const string ZongMen_NeiMenWnd = "ZongMen_NeiMenWnd.prefab";

		public const string ZongMen_YeLianWnd = "ZongMen_YeLianWnd.prefab";

		public const string ZongMen_LianDanWnd = "ZongMen_LianDanWnd.prefab";

		public const string ZongMen_LianQiWnd = "ZongMen_LianQiWnd.prefab";

		public const string ZongMen_ZhenFaWnd = "ZongMen_ZhenFaWnd.prefab";

		public const string ZongMen_ZhiFuWnd = "ZongMen_ZhiFuWnd.prefab";

		public const string ZongMen_GongFangWnd = "ZongMen_GongFangWnd.prefab";

		public const string ZongMen_YuShouWnd = "ZongMen_YuShouWnd.prefab";

		public const string ZongMen_SpeedUpWnd = "ZongMen_SpeedUpWnd.prefab";

		public const string ZongMen_CangBaoGeWnd = "ZongMen_CangBaoGeWnd.prefab";

		public const string CZF_Main_Window = "ChengZhuFuMainWnd.prefab";

		public const string SHITUTOZI_WINDOW = "ShiTuToZiWnd.prefab";

		public const string SHITU_WINDOW = "ShiTuWnd.prefab";

		public const string COMMON_TOG_ITEM = "Assets/GameData/Prefabs/Items/Common_TogItem.prefab";

		public const string CEHUA_ITEM = "Assets/GameData/Prefabs/Items/CeHuaItem.prefab";

		public const string COMMON_BOX_ITEM = "Assets/GameData/Prefabs/Items/BoxItem.prefab";

		public const string COMMON_SHAIXUAN_BIG_ITEM = "Assets/GameData/Prefabs/Items/Common_ShaiXuanBigItem.prefab";

		public const string CHOOSESERVER_ITEM = "Assets/GameData/Prefabs/Items/ChooseServerItem.prefab";

		public const string CHUANXIN_ITEM = "Assets/GameData/Prefabs/Items/ChuanXinItem.prefab";

		public const string HUODONG_ITEM = "Assets/GameData/Prefabs/Items/HuoDongItem.prefab";

		public const string DUIHUANLINGYU_ITEM = "Assets/GameData/Prefabs/Items/DuiHuanXianYuItem.prefab";

		public const string LIPIN_ITEM = "Assets/GameData/Prefabs/Items/ChuanXinGoodsItem.prefab";

		public const string GENGHUANGONGFA_ITEM = "Assets/GameData/Prefabs/Items/GengHuanGongFaItem.prefab";

		public const string DONGFU_ITEM = "Assets/GameData/Prefabs/Items/DongFuItem.prefab";

		public const string CANWUXIANGQING_ITEM = "Assets/GameData/Prefabs/Items/CanWuXiangQingItem.prefab";

		public const string KECANWU_ITEM = "Assets/GameData/Prefabs/Items/KeCanWuItem.prefab";

		public const string YICANWU_ITEM = "Assets/GameData/Prefabs/Items/YiCanWuItem.prefab";

		public const string CANWU_SCROLL_ITEM = "Assets/GameData/Prefabs/Items/CanWuScrollItem.prefab";

		public const string HUAFUCEHUA_ITEM = "Assets/GameData/Prefabs/Items/HuaFuCeHuaItem.prefab";

		public const string HUAFUCEHUACHILD_ITEM = "Assets/GameData/Prefabs/Items/HuaFuCeHuaItemChild.prefab";

		public const string QiZhenGeItem = "Assets/GameData/Prefabs/Items/QiZhenGeWuPinItem.prefab";

		public const string QIZHENGE_ITEM = "Assets/GameData/Prefabs/Items/QiZhenGeItem.prefab";

		public const string QIZHENGEPAIMAI_ITEM = "Assets/GameData/Prefabs/Items/QiZhenGePaiMaiItem.prefab";

		public const string QIZHENGEBAIQIFANG_ITEM = "Assets/GameData/Prefabs/Items/ShangPuItem.prefab";

		public const string QZG_INFO_ITEM = "Assets/GameData/Prefabs/Items/QZG_InfoItem.prefab";

		public const string QZG_WODEJINGPAI_ITEM = "Assets/GameData/Prefabs/Items/QZG_WoDeJingPaiItem.prefab";

		public const string QZG_WODEPAIMAI_ITEM = "Assets/GameData/Prefabs/Items/QZG_WoDePaiMaiItem.prefab";

		public const string CHENGZHUFU_RENWU_ITEM = "Assets/GameData/Prefabs/Items/ChenZhuFuRenWuItem.prefab";

		public const string CHENGZHUFU_JINSHENG_ITEM = "Assets/GameData/Prefabs/Items/ChengZhuFuJinShengItem.prefab";

		public const string CHUANDAI_ZHUANGBEI_ITEM = "Assets/GameData/Prefabs/Items/ChuanDaiZhuangBeiItem.prefab";

		public const string UI_GOODS_ITEM = "Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab";

		public const string UI_GOODS_TOG_ITEM = "Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab";

		public const string UI_BOX_TITLE_ITEM = "Assets/GameData/Prefabs/Items/BoxTitleTogItem.prefab";

		public const string DOUMEN_SHEZHI_JINENG_ITEM = "Assets/GameData/Prefabs/Items/DouMenSheZhiFaShuItem.prefab";

		public const string DOUMEN_XUANZE_JINENG_ITEM = "Assets/GameData/Prefabs/Items/DouMenXuanXeFaShuItem.prefab";

		public const string NAJIETIP_ITEM = "Assets/GameData/Prefabs/Items/NaJieTipItem.prefab";

		public const string CAILIAOSHAIXUAN_SHUXING_ITEM = "Assets/GameData/Prefabs/Items/ShaiXuanShuXingItem.prefab";

		public const string CAILIAOSHAIXUAN_WEIZHI_ITEM = "Assets/GameData/Prefabs/Items/ShaiXuanWeiZhiItem.prefab";

		public const string ZHANDOU_JINENG_ITEM = "Assets/GameData/Prefabs/Items/JiNengItem.prefab";

		public const string ZHANDOU_DIAOLUO_ITEM = "Assets/GameData/Prefabs/Items/GuiWuDiaoLuoItem.prefab";

		public const string DONGFU_CHUWUXIANG_ITEM = "Assets/GameData/Prefabs/Items/ChuWuXiangItem.prefab";

		public const string DANSHITA_ITEM = "Assets/GameData/Prefabs/Items/DanShiItem.prefab";

		public const string YIZHUCE_ITEM = "Assets/GameData/Prefabs/Items/DanFang_YiZhuCe.prefab";

		public const string DAISHENHE_ITEM = "Assets/GameData/Prefabs/Items/DanFang_DaiShenHe.prefab";

		public const string KEZHUCE_ITEM = "Assets/GameData/Prefabs/Items/DanFang_KeZhuCe.prefab";

		public const string DANFANGDUIHUAN_ITEM = "Assets/GameData/Prefabs/Items/DanFangItem.prefab";

		public const string SUOXUYAOCAI_ITEM = "Assets/GameData/Prefabs/Items/DanFang_SuoXuYaoCaiItem.prefab";

		public const string PAIHANGBANG_ITEM = "Assets/GameData/Prefabs/Items/PaiHangBangItemPanel.prefab";

		public const string JINFASHUANIM = "Assets/GameData/Prefabs/Items/JinFaShuItem.prefab";

		public const string MUFASHUANIM = "Assets/GameData/Prefabs/Items/MuFaShuItem.prefab";

		public const string SHUIFASHUANIM = "Assets/GameData/Prefabs/Items/ShuiFaShuItem.prefab";

		public const string HUOFASHUANIM = "Assets/GameData/Prefabs/Items/HuoFaShuItem.prefab";

		public const string TUFASHUANIM = "Assets/GameData/Prefabs/Items/TuFaShuItem.prefab";

		public const string ALLFASHUANIM = "Assets/GameData/Prefabs/Items/AllFaShuItem.prefab";

		public const string DEBUFFICONITEM = "Assets/GameData/Prefabs/Items/DeBuffIconItem.prefab";

		public const string SCROLLINGTEXTITEM = "Assets/GameData/Prefabs/Items/ScrollingTextItem.prefab";

		public const string DONGFUBALLITEM = "Assets/GameData/Prefabs/Items/DongFuBall.prefab";

		public const string SHANGPUXIANGQING_ITEM = "Assets/GameData/Prefabs/Items/ShangPuXiangQingItem.prefab";

		public const string DONGFU_BUFF_ITEM = "Assets/GameData/Prefabs/Items/DongFuBuffItem.prefab";

		public const string CHOOSE_CHENGHAO_ITEM = "Assets/GameData/Prefabs/Items/ChooseChengHaoItem.prefab";

		public const string CHOOSE_CHENGHAO_EFFECT_ITEM = "Assets/GameData/Prefabs/Items/ChengHaoEffectItem.prefab";

		public const string XuanTianLouItem = "Assets/GameData/Prefabs/Items/XuanTianLouItem.prefab";

		public const string XuanTianLouScrollItem = "Assets/GameData/Prefabs/Items/XuanTianLouScrollItem.prefab";

		public const string ZhanFaShiItem = "Assets/GameData/Prefabs/Items/ZhanFaShiItem.prefab";

		public const string DST_DANFANGDUIHUAN_TIP_ITEM = "Assets/GameData/Prefabs/Items/DST_DanFangDuiHuanTipItem.prefab";

		public const string DST_DANFANGDUIHUAN_ITEM = "Assets/GameData/Prefabs/Items/DST_DanFangDuiHuanItem.prefab";

		public const string ZHANDOUSETTING_ROLE_ZHUANGBEI_ITEM = "Assets/GameData/Prefabs/Items/RoleZhuangBeiItem.prefab";

		public const string LT_LING_TIAN_ITEM = "Assets/GameData/Prefabs/Items/LT_ZhoZhiItem.prefab";

		public const string LT_DUI_HUAN_ITEM = "Assets/GameData/Prefabs/Items/LZ_DuiHuanItem.prefab";

		public const string BattleSettingDetailText_Item = "Assets/GameData/Prefabs/Items/BattleSettingDetailTextItem.prefab";

		public const string Map_Mu_0_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_00.prefab";

		public const string Map_Mu_1_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_01.prefab";

		public const string Map_Mu_2_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_02.prefab";

		public const string Map_Mu_3_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_03.prefab";

		public const string Map_Mu_4_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/JiChu/Cao_04.prefab";

		public const string Map_Mu_5_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_1.prefab";

		public const string Map_Mu_6_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_2.prefab";

		public const string Map_Mu_7_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_3.prefab";

		public const string Map_Mu_8_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_4.prefab";

		public const string Map_Mu_9_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_5.prefab";

		public const string Map_Mu_10_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_6.prefab";

		public const string Map_Mu_11_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_7.prefab";

		public const string Map_Mu_12_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_8.prefab";

		public const string Map_Mu_13_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_9.prefab";

		public const string Map_Mu_14_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_10.prefab";

		public const string Map_Mu_15_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_11.prefab";

		public const string Map_Mu_16_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_12.prefab";

		public const string Map_Mu_17_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_13.prefab";

		public const string Map_Mu_18_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_14.prefab";

		public const string Map_Mu_19_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_15.prefab";

		public const string Map_Mu_20_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_16.prefab";

		public const string Map_Mu_21_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_17.prefab";

		public const string Map_Mu_22_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_18.prefab";

		public const string Map_Mu_23_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_19.prefab";

		public const string Map_Mu_24_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_20.prefab";

		public const string Map_Mu_25_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_21.prefab";

		public const string Map_Mu_26_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_22.prefab";

		public const string Map_Mu_27_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_23.prefab";

		public const string Map_Mu_28_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_24.prefab";

		public const string Map_Mu_29_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_25.prefab";

		public const string Map_Mu_30_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_26.prefab";

		public const string Map_Mu_31_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_27.prefab";

		public const string Map_Mu_32_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_28.prefab";

		public const string Map_Mu_33_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Mu/Cao_29.prefab";

		public const string Map_Shui_0_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_00.prefab";

		public const string Map_Shui_1_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_01.prefab";

		public const string Map_Shui_2_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_02.prefab";

		public const string Map_Shui_3_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_03.prefab";

		public const string Map_Shui_JiChu_0_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_00.prefab";

		public const string Map_Shui_JiChu_1_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_01.prefab";

		public const string Map_Shui_JiChu_2_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_02.prefab";

		public const string Map_Shui_JiChu_3_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/JiChu/Bing_03.prefab";

		public const string Map_Shui_4_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_1.prefab";

		public const string Map_Shui_5_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_2.prefab";

		public const string Map_Shui_6_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_3.prefab";

		public const string Map_Shui_7_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_4.prefab";

		public const string Map_Shui_8_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_5.prefab";

		public const string Map_Shui_9_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_6.prefab";

		public const string Map_Shui_10_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_7.prefab";

		public const string Map_Shui_11_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_8.prefab";

		public const string Map_Shui_12_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_9.prefab";

		public const string Map_Shui_13_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_10.prefab";

		public const string Map_Shui_14_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_11.prefab";

		public const string Map_Shui_15_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_12.prefab";

		public const string Map_Shui_16_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_13.prefab";

		public const string Map_Shui_17_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_14.prefab";

		public const string Map_Shui_18_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_15.prefab";

		public const string Map_Shui_19_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Shui/Bing_16.prefab";

		public const string Map_Jin_Tu_Huo_0_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/JiChu/Sha_00.prefab";

		public const string Map_Jin_Tu_Huo_1_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/JiChu/Sha_01.prefab";

		public const string Map_Jin_Tu_Huo_3_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_1.prefab";

		public const string Map_Jin_Tu_Huo_4_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_2.prefab";

		public const string Map_Jin_Tu_Huo_5_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_3.prefab";

		public const string Map_Jin_Tu_Huo_6_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_4.prefab";

		public const string Map_Jin_Tu_Huo_7_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_5.prefab";

		public const string Map_Jin_Tu_Huo_8_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_6.prefab";

		public const string Map_Jin_Tu_Huo_9_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_7.prefab";

		public const string Map_Jin_Tu_Huo_10_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_8.prefab";

		public const string Map_Jin_Tu_Huo_11_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_9.prefab";

		public const string Map_Jin_Tu_Huo_12_item = "Assets/GameData/Prefabs/DiTu/DiTuWuXing/Jin_Tu_Huo/Sha_10.prefab";

		public const string BattlePlayerItem = "Assets/GameData/Prefabs/Items/Battle/Player/BattlePlayer.prefab";

		public const string BattleSkillItem = "Assets/GameData/Prefabs/Items/Battle/UI/Battle_SkillItem.prefab";

		public const string BattleBuffItem = "Assets/GameData/Prefabs/Items/Battle/UI/Battle_BuffItem.prefab";

		public const string BattleNormalMonsterItem = "Assets/GameData/Prefabs/Items/Battle/Monster/BattleNormalMonster.prefab";

		public const string BattlePlant_Item = "Assets/GameData/Prefabs/Items/Battle/Plant.prefab";

		public const string BattleMineral_Item = "Assets/GameData/Prefabs/Items/Battle/Mineral.prefab";

		public const string MiJingMineral_Item = "Assets/GameData/Prefabs/Items/Battle/MiJing.prefab";

		public const string Huang_DiaoLuo_Goods_Item = "Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Huang.prefab";

		public const string Xuan_DiaoLuo_Goods_Item = "Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Xuan.prefab";

		public const string Di_DiaoLuo_Goods_Item = "Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Di.prefab";

		public const string Tian_DiaoLuo_Goods_Item = "Assets/GameData/Prefabs/Items/Battle/DiaoLuo_Tian.prefab";

		public const string DiaoLuo_LingShi_Item = "Assets/GameData/Prefabs/Items/Battle/LingShi.prefab";

		public const string HUAFU_LINE_ITEM = "Assets/GameData/Prefabs/Items/HuaFuLineItem.prefab";

		public const string DIHUOGE_LIANZHIBUZHOU_SHEZHI_ITEM = "Assets/GameData/Prefabs/Items/LianZhiBuZhouSheZhiItem.prefab";

		public const string DIHUOGE_LIANZHIXUANZE_ITEM = "Assets/GameData/Prefabs/Items/DiHuoGeLianZhiMuBiaoItem.prefab";

		public const string DIHUOGE_LIANZHI_CAILIAOXUANZE_ITEM = "Assets/GameData/Prefabs/Items/LianZhiCaiLiaoXuanZe.prefab";

		public const string DIHUOGE_LIANZHIBUZHOU_SHUNXU_ITEM = "Assets/GameData/Prefabs/Items/LianZhiBuZhouShunXuItem.prefab";

		public const string DIHUOGE_CHOOSEPAGE_SMALL_ITEM = "Assets/GameData/Prefabs/Items/DiHuoGeChooseSmallItem.prefab";

		public const string DIHUOGE_CHOOSEPAGE_BIG_ITEM = "Assets/GameData/Prefabs/Items/DiHuoGeChooseBigItem.prefab";

		public const string DIHUOGE_STEP_ITEM = "Assets/GameData/Prefabs/Items/StepBgItem.prefab";

		public const string DIHUOGE_CAILIAO_ITEM = "Assets/GameData/Prefabs/Items/DiHuoGeCaiLiaoItem.prefab";

		public const string DIHUOGE_CAILIAOJiaGe_ITEM = "Assets/GameData/Prefabs/Items/DiHuoCaiLiaoJiaGeItem.prefab";

		public const string DIHUOGE_CAILIAO_CHOOSEITEM = "Assets/GameData/Prefabs/Items/DiHuoGeCaiLiaoChooseItem.prefab";

		public const string DIHUOGE_MAP_ITEM = "Assets/GameData/Prefabs/Items/DiHuoGe_MapItem.prefab";

		public const string DIHUOGE_WORLD_CAILIAO_ITEM = "Assets/GameData/Prefabs/Items/WorldCaiLiaoItem.prefab";

		public const string DIHUOGE_ZHUCETIP_ITEM = "Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab";

		public const string DIHUOGE_TOUZHI_LEFT_ITEM = "Assets/GameData/Prefabs/Items/TouZhiLeftCaiLiao.prefab";

		public const string DIHUOGE_TOUZHI_RIGHT_ITEM = "Assets/GameData/Prefabs/Items/TouZhiRightCaiLiao.prefab";

		public const string TUPODUJIE_SHANDIAN_ITEM = "Assets/GameData/Prefabs/Items/TuPo_ShanDian.prefab";

		public const string XUSHENTA_BANGDAN_ITEM = "Assets/GameData/Prefabs/Items/XuShenTaBangDanItem.prefab";

		public const string XUSHENJIE_ITEM = "Assets/GameData/Prefabs/Items/XuShenJieItem.prefab";

		public const string COMMON_XUSHENJIE_ITEM = "Assets/GameData/Prefabs/Items/Common_XuShenJieItem.prefab";

		public const string DJSD_XUSHENJIE_ITEM = "Assets/GameData/Prefabs/Items/DJSD_XuShenJieItem.prefab";

		public const string COMMON_XUSHENJIES_ITEM = "Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab";

		public const string CHANG_SHENLU_ITEM = "Assets/GameData/Prefabs/Items/ChangShengLuItem.prefab";

		public const string CHUANGZHAOGONGFATIP_SUCCESS_ITEM = "Assets/GameData/Prefabs/Items/CZGF_Tip_SuccessItem.prefab";

		public const string COMMON_ZHUCE_ITEM = "Assets/GameData/Prefabs/Items/Common_ZhuCeItem.prefab";

		public const string DJSD_GongFaDuiHuan_ITEM = "Assets/GameData/Prefabs/Items/DJSD_GongFaDuiHuanItem.prefab";

		public const string WFG_FaShuDuiHuan_ITEM = "Assets/GameData/Prefabs/Items/WFG_FaShuDuiHuanItem.prefab";

		public const string SFH_FuShuDuiHuan_ITEM = "Assets/GameData/Prefabs/Items/SFH_FuShuDuiHuanItem.prefab";

		public const string ChuangZaoFaShu_ITEM = "Assets/GameData/Prefabs/Items/ChuangZaoFaShuItem.prefab";

		public const string Huo_DanTi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/DanTi/Huo_Danti_Ani_Prefab.prefab";

		public const string Jin_DanTi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/DanTi/Jin_Danti_Ani_Prefab.prefab";

		public const string Mu_DanTi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/DanTi/Mu_Danti_Ani_Prefab.prefab";

		public const string Shui_DanTi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/DanTi/Shui_Danti_Ani_Prefab.prefab";

		public const string Tu_DanTi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/DanTi/Tu_Danti_Ani_Prefab.prefab";

		public const string WuXing_DanTi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/DanTi/WuXing_Danti_Ani_Prefab.prefab";

		public const string Huo_Aoe_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/Huo_GongJi_Ani_Prefab.prefab";

		public const string Jin_Aoe_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/Jin_GongJi_Ani_Prefab.prefab";

		public const string Mu_Aoe_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/Mu_GongJi_Ani_Prefab.prefab";

		public const string Shui_Aoe_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/Shui_GongJi_Ani_Prefab.prefab";

		public const string Tu_Aoe_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/Tu_GongJi_Ani_Prefab.prefab";

		public const string WuXing_Aoe_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/WuXing_GongJi_Ani_Prefab.prefab";

		public const string Aoe_Ani_Prefab_ITEM = "Assets/GameData/Prefabs/Items/Battle/Aoe/Aoe_Ani_Prefab.prefab";

		public const string ZhenFa_Prefab_ITEM = "Assets/GameData/Prefabs/Items/Battle/ZhenFa/ZhenFa_Prefab.prefab";

		public const string Huo_HuDun_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/HuDun/Huo_HuDun_Ani_Prefab.prefab";

		public const string Jin_HuDun_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/HuDun/Jin_HuDun_Ani_Prefab.prefab";

		public const string Mu_HuDun_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/HuDun/Mu_HuDun_Ani_Prefab.prefab";

		public const string Shui_HuDun_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/HuDun/Shui_HuDun_Ani_Prefab.prefab";

		public const string Tu_HuDun_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/HuDun/Tu_HuDun_Ani_Prefab.prefab";

		public const string WuXing_HuDun_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/HuDun/WuXing_HuDun_Ani_Prefab.prefab";

		public const string Huo_KongZhi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/KongZhi/Huo_KongZhi_Ani_Prefab.prefab";

		public const string Jin_KongZhi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/KongZhi/Jin_KongZhi_Ani_Prefab.prefab";

		public const string Mu_KongZhi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/KongZhi/Mu_KongZhi_Ani_Prefab.prefab";

		public const string Shui_KongZhi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/KongZhi/Shui_KongZhi_Ani_Prefab.prefab";

		public const string Tu_KongZhi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/KongZhi/Tu_KongZhi_Ani_Prefab.prefab";

		public const string WuXing_KongZhi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/KongZhi/WuXing_KongZhi_Ani_Prefab.prefab";

		public const string Huo_FaQi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/FaQi/Huo_WuQi_Ani_Prefab.prefab";

		public const string Jin_FaQi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/FaQi/Jin_WuQi_Ani_Prefab.prefab";

		public const string Mu_FaQi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/FaQi/Mu_WuQi_Ani_Prefab.prefab";

		public const string Shui_FaQi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/FaQi/Shui_WuQi_Ani_Prefab.prefab";

		public const string Tu_FaQi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/FaQi/Tu_WuQi_Ani_Prefab.prefab";

		public const string WuXing_FaQi_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/FaQi/WuXing_WuQi_Ani_Prefab.prefab";

		public const string Huo_Lei_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/LeiDian/Huo_Lei_Ani_Prefab";

		public const string Jin_Lei_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/LeiDian/Jin_Lei_Ani_Prefab.prefab";

		public const string Mu_Lei_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/LeiDian/Mu_Lei_Ani_Prefab.prefab";

		public const string Shui_Lei_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/LeiDian/Shui_Lei_Ani_Prefab.prefab";

		public const string Tu_Lei_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/LeiDian/Tu_Lei_Ani_Prefab.prefab";

		public const string WuXing_Lei_Ani_ITEM = "Assets/GameData/Prefabs/Items/Battle/LeiDian/WuXing_Lei_Ani_Prefab.prefab";

		public const string Battle_FaQi_Item = "Assets/GameData/Prefabs/Items/Battle/FaQi/WuQi_Item.prefab";

		public const string Battle_Gray_Num_0 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_0.prefab";

		public const string Battle_Gray_Num_1 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_1.prefab";

		public const string Battle_Gray_Num_2 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_2.prefab";

		public const string Battle_Gray_Num_3 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_3.prefab";

		public const string Battle_Gray_Num_4 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_4.prefab";

		public const string Battle_Gray_Num_5 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_5.prefab";

		public const string Battle_Gray_Num_6 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_6.prefab";

		public const string Battle_Gray_Num_7 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_7.prefab";

		public const string Battle_Gray_Num_8 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_8.prefab";

		public const string Battle_Gray_Num_9 = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_9.prefab";

		public const string Battle_Gray_Num_Wan = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_Wan.prefab";

		public const string Battle_Gray_Num_Yi = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_Yi.prefab";

		public const string Battle_Gray_Num_Zhao = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleDamageGrayItem_Zhao.prefab";

		public const string Battle_Gray_Content = "Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleGrayDamageContentItem.prefab";

		public const string Battle_Yellow_Num_0 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_0.prefab";

		public const string Battle_Yellow_Num_1 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_1.prefab";

		public const string Battle_Yellow_Num_2 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_2.prefab";

		public const string Battle_Yellow_Num_3 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_3.prefab";

		public const string Battle_Yellow_Num_4 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_4.prefab";

		public const string Battle_Yellow_Num_5 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_5.prefab";

		public const string Battle_Yellow_Num_6 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_6.prefab";

		public const string Battle_Yellow_Num_7 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_7.prefab";

		public const string Battle_Yellow_Num_8 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_8.prefab";

		public const string Battle_Yellow_Num_9 = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_9.prefab";

		public const string Battle_Yellow_Num_Wan = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_Wan.prefab";

		public const string Battle_Yellow_Num_Yi = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_Yi.prefab";

		public const string Battle_Yellow_Num_Zhao = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleDamageYellowItem_Zhao.prefab";

		public const string Battle_Yellow_Content = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleYellowDamageContentItem.prefab";

		public const string Battle_XiShou_Text = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_XiShou.prefab";

		public const string Battle_DiKang_Text = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_DiKang.prefab";

		public const string Battle_DingShen_Text = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_DingShen.prefab";

		public const string Battle_JianSu_Text = "Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleTextYellowItem_JianSu.prefab";

		public const string Battle_Red_Num_0 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_0.prefab";

		public const string Battle_Red_Num_1 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_1.prefab";

		public const string Battle_Red_Num_2 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_2.prefab";

		public const string Battle_Red_Num_3 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_3.prefab";

		public const string Battle_Red_Num_4 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_4.prefab";

		public const string Battle_Red_Num_5 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_5.prefab";

		public const string Battle_Red_Num_6 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_6.prefab";

		public const string Battle_Red_Num_7 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_7.prefab";

		public const string Battle_Red_Num_8 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_8.prefab";

		public const string Battle_Red_Num_9 = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_9.prefab";

		public const string Battle_Red_Num_Wan = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_Wan.prefab";

		public const string Battle_Red_Num_Yi = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_Yi.prefab";

		public const string Battle_Red_Num_Zhao = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleDamageRedItem_Zhao.prefab";

		public const string Battle_Red_Content = "Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleRedDamageContentItem.prefab";

		public const string RoleZhuangBei_XiangQing_Item = "Assets/GameData/Prefabs/Items/RoleZhuangBeiXiangQingItem.prefab";

		public const string RoleZhuangBei_Content_Item = "Assets/GameData/Prefabs/Items/RoleZhuangBeiShuXingContentItem.prefab";

		public const string Chat_Item = "Assets/GameData/Prefabs/Items/ChatItem.prefab";

		public const string Chat_DanMu_Item = "Assets/GameData/Prefabs/Items/DanMuItem.prefab";

		public const string Chat_Jiu_Item = "Assets/GameData/Prefabs/Items/PayContentItem.prefab";

		public const string DaoYou_Item = "Assets/GameData/Prefabs/Items/DaoYouItem.prefab";

		public const string MingCi_Item = "Assets/GameData/Prefabs/Items/MingCiItem.prefab";

		public const string SelfChat_Item = "Assets/GameData/Prefabs/Items/SelfChatItem.prefab";

		public const string OtherChat_Item = "Assets/GameData/Prefabs/Items/OtherChatItem.prefab";

		public const string MapZongMen_Item = "Assets/GameData/Prefabs/Items/MapZongMenItem.prefab";

		public const string ZongMenChengYuan_Item = "Assets/GameData/Prefabs/Items/ZongMenChengYuanItem.prefab";

		public const string ZongMenMingCi_Item = "Assets/GameData/Prefabs/Items/ZongMenMingCiItem.prefab";

		public const string ZongMen_ZhangLao_Item = "Assets/GameData/Prefabs/Items/ZongMen_ZhangLaoItem.prefab";

		public const string ZONGMEN_ZHANGLAODIAN_ITEM = "Assets/GameData/Prefabs/Items/ZongMen_ZhangLaoDianItem.prefab";

		public const string CZF_TSB_Item = "Assets/GameData/Prefabs/Items/TSB_Item.prefab";

		public const string ShoTu_Item = "Assets/GameData/Prefabs/Items/ShoTuItem.prefab";

		public const string ShiTu_Item = "Assets/GameData/Prefabs/Items/ShiTuItem.prefab";

		public const string VipPowerEffectItem = "Assets/GameData/Prefabs/Items/VipPowerEffectItem.prefab";

		public const string LINGYU_ZERO_IMAGE = "Assets/GameData/Textures/LingYuLogo.png";

		public const string LINGYU_ONE_IMAGE = "Assets/GameData/Textures/LingYuLogo1.png";

		public const string LINGYU_TWO_IMAGE = "Assets/GameData/Textures/LingYuLogo2.png";

		public const string LINGYU_THREE_IMAGE = "Assets/GameData/Textures/LingYuLogo3.png";

		public const string LINGYU_FOUR_IMAGE = "Assets/GameData/Textures/LingYuLogo4.png";

		public const string LINGYU_FIVE_IMAGE = "Assets/GameData/Textures/LingYuLogo5.png";

		public const string GENGHUANGONGFA_CHOOSE_IMAGE = "Assets/GameData/Textures/DongFu/Choose.png";

		public const string GENGHUANGONGFA_NOCHOOSE_IMAGE = "Assets/GameData/Textures/DongFu/NoChoose.png";

		public const string WUXING_CHOOSE_IMAGE = "Assets/GameData/Textures/DongFu/WuXingChoose.png";

		public const string WUXING_NOCHOOSE_IMAGE = "Assets/GameData/Textures/DongFu/WuXingNoChoose.png";

		public const string ZHENFA_CHOOSE_IMAGE = "Assets/GameData/Textures/DongFu/GengHuanFaZhenItemChoose.png";

		public const string ZHENFA_NOCHOOSE_IMAGE = "Assets/GameData/Textures/DongFu/GengHuanFaZhenItemUnChoose.png";

		public const string HUABI_0_IMAGE = "Assets/GameData/Textures/DongFu/MaoBi1.png";

		public const string HUABI_1_IMAGE = "Assets/GameData/Textures/DongFu/MaoBi2.png";

		public const string HUABI_2_IMAGE = "Assets/GameData/Textures/DongFu/MaoBi3.png";

		public const string HUANGFUZHI_IMAGE = "Assets/GameData/Textures/DongFu/HuangFuZi.png";

		public const string ZHUSHA_IMAGE = "Assets/GameData/Textures/DongFu/ZhuSha.png";

		public const string CEHUAITEM_IMAGE = "Assets/GameData/Textures/DongFu/MaoBiRedBg.png";

		public const string MASK_IMAGE = "Assets/GameData/Textures/Common/Mask.png";

		public const string BUTTON_BG_IMAGE = "Assets/GameData/Textures/btbg.png";

		public const string DONGFU_ICON_IMAGE = "Assets/GameData/Textures/Common/DongFuIcon.png";

		public const string NAJIE_ICON_IMAGE = "Assets/GameData/Textures/Common/NaJieIcon.png";

		public const string ZHANDOU_ICON_IMAGE = "Assets/GameData/Textures/Common/ZhanDouIcon.png";

		public const string CLOSE_IMAGE = "Assets/GameData/Textures/NaJie_slices/分组 26@3x.png";

		public const string HUO_BG_IMAGE = "Assets/GameData/Textures/ZhanDouSheZhi_slices/HuoItemBg.png";

		public const string SHUI_BG_IMAGE = "Assets/GameData/Textures/ZhanDouSheZhi_slices/ShuiItemBg.png";

		public const string TU_BG_IMAGE = "Assets/GameData/Textures/ZhanDouSheZhi_slices/TuItemBg.png";

		public const string MU_BG_IMAGE = "Assets/GameData/Textures/ZhanDouSheZhi_slices/MuItemBg.png";

		public const string JIN_BG_IMAGE = "Assets/GameData/Textures/ZhanDouSheZhi_slices/JinItemBg.png";

		public const string ROLE_MAN_IMG = "Assets/GameData/Textures/Item/NaJieNan.png";

		public const string ROLE_WOMEN_IMG = "Assets/GameData/Textures/Item/NaJieNv.png";

		public const string TOUXIANGMANYI_IMAGE = "Assets/GameData/Textures/TouXiang/Man_1.png";

		public const string TOUXIANGMANYI_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Man_1_offline.png";

		public const string TOUXIANGMANER_IMAGE = "Assets/GameData/Textures/TouXiang/Man_2.png";

		public const string TOUXIANGMANER_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Man_2_offline.png";

		public const string TOUXIANGMANSAN_IMAGE = "Assets/GameData/Textures/TouXiang/Man_3.png";

		public const string TOUXIANGMANSAN_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Man_3_offline.png";

		public const string TOUXIANGMANSI_IMAGE = "Assets/GameData/Textures/TouXiang/Man_4.png";

		public const string TOUXIANGMANSI_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Man_4_offline.png";

		public const string TOUXIANGMANWU_IMAGE = "Assets/GameData/Textures/TouXiang/Man_5.png";

		public const string TOUXIANGMANWU_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Man_5_offline.png";

		public const string TOUXIANGMANLIU_IMAGE = "Assets/GameData/Textures/TouXiang/Man_6.png";

		public const string TOUXIANGMANLIU_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Man_6_offline.png";

		public const string TOUXIANGWOMENYI_IMAGE = "Assets/GameData/Textures/TouXiang/Women_1.png";

		public const string TOUXIANGWOMENYI_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Women_1_offline.png";

		public const string TOUXIANGWOMENER_IMAGE = "Assets/GameData/Textures/TouXiang/Women_2.png";

		public const string TOUXIANGWOMENER_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Women_2_offline.png";

		public const string TOUXIANGWOMENSAN_IMAGE = "Assets/GameData/Textures/TouXiang/Women_3.png";

		public const string TOUXIANGWOMENSAN_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Women_3_offline.png";

		public const string TOUXIANGWOMENSI_IMAGE = "Assets/GameData/Textures/TouXiang/Women_4.png";

		public const string TOUXIANGWOMENSI_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Women_4_offline.png";

		public const string TOUXIANGWOMENWU_IMAGE = "Assets/GameData/Textures/TouXiang/Women_5.png";

		public const string TOUXIANGWOMENWU_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Women_5_offline.png";

		public const string TOUXIANGWOMENLIU_IMAGE = "Assets/GameData/Textures/TouXiang/Women_6.png";

		public const string TOUXIANGWOMENLIU_IMAGE_OFFLINE = "Assets/GameData/Textures/TouXiang/Women_6_offline.png";

		public const string ZHENFA_WEIXUANZHONG = "Assets/GameData/Textures/Common/Mask.png";

		public const string ZHENFA_YIXUANZHONG = "Assets/GameData/Textures/XuShenDian/ZhenFaXieHui/ZhenTu_lvJuXing.png";

		public const string RANK_1 = "Assets/GameData/Textures/Rank_1.png";

		public const string RANK_2 = "Assets/GameData/Textures/Rank_2.png";

		public const string RANK_3 = "Assets/GameData/Textures/Rank_3.png";

		public const string NAJIEITEMBG_1_IMAGE = "Assets/GameData/Textures/Item/bgItem1.png";

		public const string NAJIEITEMBG_2_IMAGE = "Assets/GameData/Textures/Item/bgItem2.png";

		public const string NAJIEITEMBG_3_IMAGE = "Assets/GameData/Textures/Item/bgItem3.png";

		public const string NAJIEITEMBG_4_IMAGE = "Assets/GameData/Textures/Item/bgItem4.png";

		public const string NAJIEITEMBG_5_IMAGE = "Assets/GameData/Textures/Item/bgItem5.png";

		public const string NAJIEITEMBG_6_IMAGE = "Assets/GameData/Textures/Item/bgItem6.png";

		public const string DONGFULEVEL_HUANG = "Assets/GameData/Textures/DongFu/HuangJieDongFu.png";

		public const string DONGFULEVEL_XUAN = "Assets/GameData/Textures/DongFu/XuanJieDongFu.png";

		public const string DONGFULEVEL_DI = "Assets/GameData/Textures/DongFu/DiJieDongFu.png";

		public const string DONGFULEVEL_TIAN = "Assets/GameData/Textures/DongFu/TianJieDongFu.png";

		public const string QZG_SORTUP_ICON = "Assets/GameData/Textures/Item/UpSortIcon.png";

		public const string QZG_SORTDOWN_ICON = "Assets/GameData/Textures/Item/DownSortIcon.png";

		public const string QZG_SORTUPDOWN_ICON = "Assets/GameData/Textures/Item/UpDownSortIcon.png";

		public const string DONGFUGONGFA_CHOOSE_ALL = "Assets/GameData/Textures/Item/FaShuItemWuXingChooseAll.png";

		public const string DONGFUGONGFA_CHOOSE_JIN = "Assets/GameData/Textures/Item/FaShuItemWuXingChooseJin.png";

		public const string DONGFUGONGFA_CHOOSE_MU = "Assets/GameData/Textures/Item/FaShuItemWuXingChooseMu.png";

		public const string DONGFUGONGFA_CHOOSE_SHUI = "Assets/GameData/Textures/Item/FaShuItemWuXingChooseShui.png";

		public const string DONGFUGONGFA_CHOOSE_HUO = "Assets/GameData/Textures/Item/FaShuItemWuXingChooseHuo.png";

		public const string DONGFUGONGFA_CHOOSE_TU = "Assets/GameData/Textures/Item/FaShuItemWuXingChooseTu.png";

		public const string DONGFUGONGFA_SMALL_ALL = "Assets/GameData/Textures/Item/FaShuItemWuXingSmallAll.png";

		public const string DONGFUGONGFA_SMALL_JIN = "Assets/GameData/Textures/Item/FaShuItemWuXingSmallJin.png";

		public const string DONGFUGONGFA_SMALL_MU = "Assets/GameData/Textures/Item/FaShuItemWuXingSmallMu.png";

		public const string DONGFUGONGFA_SMALL_SHUI = "Assets/GameData/Textures/Item/FaShuItemWuXingSmallShui.png";

		public const string DONGFUGONGFA_SMALL_HUO = "Assets/GameData/Textures/Item/FaShuItemWuXingSmallHuo.png";

		public const string DONGFUGONGFA_SMALL_TU = "Assets/GameData/Textures/Item/FaShuItemWuXingSmallTu.png";

		public const string DONGFUGONGFA_ALL = "Assets/GameData/Textures/Item/FaShuItemWuXingAll.png";

		public const string DONGFUGONGFA_JIN = "Assets/GameData/Textures/Item/FaShuItemWuXingJin.png";

		public const string DONGFUGONGFA_MU = "Assets/GameData/Textures/Item/FaShuItemWuXingMu.png";

		public const string DONGFUGONGFA_SHUI = "Assets/GameData/Textures/Item/FaShuItemWuXingShui.png";

		public const string DONGFUGONGFA_HUO = "Assets/GameData/Textures/Item/FaShuItemWuXingHuo.png";

		public const string DONGFUGONGFA_TU = "Assets/GameData/Textures/Item/FaShuItemWuXingTu.png";

		public const string DONGFUGONGFA_YIWANG_ALL = "Assets/GameData/Textures/Item/FaShuItemYiWangWuXingAll.png";

		public const string DONGFUGONGFA_YIWANG_JIN = "Assets/GameData/Textures/Item/FaShuItemYiWangWuXingJin.png";

		public const string DONGFUGONGFA_YIWANG_MU = "Assets/GameData/Textures/Item/FaShuItemYiWangWuXingMu.png";

		public const string DONGFUGONGFA_YIWANG_SHUI = "Assets/GameData/Textures/Item/FaShuItemYiWangWuXingShui.png";

		public const string DONGFUGONGFA_YIWANG_HUO = "Assets/GameData/Textures/Item/FaShuItemYiWangWuXingHuo.png";

		public const string DONGFUGONGFA_YIWANG_TU = "Assets/GameData/Textures/Item/FaShuItemYiWangWuXingTu.png";

		public const string LEVELTAG_1 = "Assets/GameData/Textures/Item/Level_Tag_1.png";

		public const string LEVELTAG_2 = "Assets/GameData/Textures/Item/Level_Tag_2.png";

		public const string LEVELTAG_3 = "Assets/GameData/Textures/Item/Level_Tag_3.png";

		public const string LEVELTAG_4 = "Assets/GameData/Textures/Item/Level_Tag_4.png";

		public const string LEVELBAGTAG_1 = "Assets/GameData/Textures/Item/Level_bag_Tag_1.png";

		public const string LEVELBAGTAG_2 = "Assets/GameData/Textures/Item/Level_bag_Tag_2.png";

		public const string LEVELBAGTAG_3 = "Assets/GameData/Textures/Item/Level_bag_Tag_3.png";

		public const string LEVELBAGTAG_4 = "Assets/GameData/Textures/Item/Level_bag_Tag_4.png";

		public const string WUXINGBAG_JIN = "Assets/GameData/Textures/Item/NaJieJinItem.png";

		public const string WUXINGBAG_MU = "Assets/GameData/Textures/Item/NaJieMuItem.png";

		public const string WUXINGBAG_SHUI = "Assets/GameData/Textures/Item/NaJieShuiItem.png";

		public const string WUXINGBAG_HUO = "Assets/GameData/Textures/Item/NaJieHuoItem.png";

		public const string WUXINGBAG_TU = "Assets/GameData/Textures/Item/NaJieTuItem.png";

		public const string WUXINGBAG_ALL = "Assets/GameData/Textures/Item/NaJieAllItem.png";

		public const string QZG_WUXINGBAG_JIN = "Assets/GameData/Textures/Item/Qzg_itemTitleBg_Jin.png";

		public const string QZG_WUXINGBAG_MU = "Assets/GameData/Textures/Item/Qzg_itemTitleBg_Mu.png";

		public const string QZG_WUXINGBAG_SHUI = "Assets/GameData/Textures/Item/Qzg_itemTitleBg_Shui.png";

		public const string QZG_WUXINGBAG_HUO = "Assets/GameData/Textures/Item/Qzg_itemTitleBg_Huo.png";

		public const string QZG_WUXINGBAG_TU = "Assets/GameData/Textures/Item/Qzg_itemTitleBg_Tu.png";

		public const string QZG_WUXINGBAG_ALL = "Assets/GameData/Textures/Item/Qzg_itemTitleBg_All.png";

		public const string QZG_WUXINGBAG_BTN_JIN = "Assets/GameData/Textures/Item/Qzg_BtnBg_Jin.png";

		public const string QZG_WUXINGBAG_BTN_MU = "Assets/GameData/Textures/Item/Qzg_BtnBg_Mu.png";

		public const string QZG_WUXINGBAG_BTN_SHUI = "Assets/GameData/Textures/Item/Qzg_BtnBg_Shui.png";

		public const string QZG_WUXINGBAG_BTN_HUO = "Assets/GameData/Textures/Item/Qzg_BtnBg_Huo.png";

		public const string QZG_WUXINGBAG_BTN_TU = "Assets/GameData/Textures/Item/Qzg_BtnBg_Tu.png";

		public const string QZG_WUXINGBAG_BTN_ALL = "Assets/GameData/Textures/Item/Qzg_BtnBg_All.png";

		public const string DST_WUXINGBAG_JIN = "Assets/GameData/Textures/Item/Dst_DuiHuan_itemTitleBg_Jin.png";

		public const string DST_WUXINGBAG_MU = "Assets/GameData/Textures/Item/Dst_DuiHuan_itemTitleBg_Mu.png";

		public const string DST_WUXINGBAG_SHUI = "Assets/GameData/Textures/Item/Dst_DuiHuan_itemTitleBg_Shui.png";

		public const string DST_WUXINGBAG_HUO = "Assets/GameData/Textures/Item/Dst_DuiHuan_itemTitleBg_Huo.png";

		public const string DST_WUXINGBAG_TU = "Assets/GameData/Textures/Item/Dst_DuiHuan_itemTitleBg_Tu.png";

		public const string DST_WUXINGBAG_ALL = "Assets/GameData/Textures/Item/Dst_DuiHuan_itemTitleBg_All.png";

		public const string WUXINGTAGITEM_JIN = "Assets/GameData/Textures/Item/JinItemType.png";

		public const string WUXINGTAGITEM_MU = "Assets/GameData/Textures/Item/MuItemType.png";

		public const string WUXINGTAGITEM_SHUI = "Assets/GameData/Textures/Item/ShuiItemType.png";

		public const string WUXINGTAGITEM_HUO = "Assets/GameData/Textures/Item/HuoItemType.png";

		public const string WUXINGTAGITEM_TU = "Assets/GameData/Textures/Item/TuItemType.png";

		public const string ZHANDOUSHEZHIITEMBG_ALL = "Assets/GameData/Textures/Item/ZhanDouSettingAll.png";

		public const string ZHANDOUSHEZHIITEMBG_JIN = "Assets/GameData/Textures/Item/ZhanDouSettingJin.png";

		public const string ZHANDOUSHEZHIITEMBG_MU = "Assets/GameData/Textures/Item/ZhanDouSettingMu.png";

		public const string ZHANDOUSHEZHIITEMBG_SHUI = "Assets/GameData/Textures/Item/ZhanDouSettingShui.png";

		public const string ZHANDOUSHEZHIITEMBG_HUO = "Assets/GameData/Textures/Item/ZhanDouSettingHuo.png";

		public const string ZHANDOUSHEZHIITEMBG_TU = "Assets/GameData/Textures/Item/ZhanDouSettingTu.png";

		public const string ZHANDOUSHEZHIITEMBG_1 = "Assets/GameData/Textures/Item/level1.png";

		public const string ZHANDOUSHEZHIITEMBG_2 = "Assets/GameData/Textures/Item/level2.png";

		public const string ZHANDOUSHEZHIITEMBG_3 = "Assets/GameData/Textures/Item/level4.png";

		public const string ZHANDOUSHEZHIITEMBG_4 = "Assets/GameData/Textures/Item/level5.png";

		public const string NAJIEPEOPLE_NAN = "Assets/GameData/Textures/Item/NaJieNan.png";

		public const string NAJIEPEOPLE_NV = "Assets/GameData/Textures/Item/NaJieNv.png";

		public const string TOUZHICAILIAO_JINGXUE_IMG = "Assets/GameData/Textures/Item/TouZhi_JingXue.png";

		public const string TOUZHICAILIAO_YAODAN_IMG = "Assets/GameData/Textures/Item/TouZhi_YaoDan.png";

		public const string TOUZHICAILIAO_CAOYAO_IMG = "Assets/GameData/Textures/Item/TouZhi_CaoYao.png";

		public const string Battle_Jin_ShenShi_Icon = "Assets/GameData/Textures/Battle/WuXing/Jin_Shen.png";

		public const string Battle_Jin_Ti_Icon = "Assets/GameData/Textures/Battle/WuXing/Jin_Ti.png";

		public const string Battle_Jin_Zhen_Icon = "Assets/GameData/Textures/Battle/WuXing/Jin_Zhen.png";

		public const string Battle_Huo_ShenShi_Icon = "Assets/GameData/Textures/Battle/WuXing/Huo_Shen.png";

		public const string Battle_Huo_Ti_Icon = "Assets/GameData/Textures/Battle/WuXing/Huo_Ti.png";

		public const string Battle_Huo_Zhen_Icon = "Assets/GameData/Textures/Battle/WuXing/Huo_Zhen.png";

		public const string Battle_Mu_ShenShi_Icon = "Assets/GameData/Textures/Battle/WuXing/Mu_Shen.png";

		public const string Battle_Mu_Ti_Icon = "Assets/GameData/Textures/Battle/WuXing/Mu_Ti.png";

		public const string Battle_Mu_Zhen_Icon = "Assets/GameData/Textures/Battle/WuXing/Mu_Zhen.png";

		public const string Battle_Shui_ShenShi_Icon = "Assets/GameData/Textures/Battle/WuXing/Shui_Shen.png";

		public const string Battle_Shui_Ti_Icon = "Assets/GameData/Textures/Battle/WuXing/Shui_Ti.png";

		public const string Battle_Shui_Zhen_Icon = "Assets/GameData/Textures/Battle/WuXing/Shui_Zhen.png";

		public const string Battle_Tu_ShenShi_Icon = "Assets/GameData/Textures/Battle/WuXing/Tu_Shen.png";

		public const string Battle_Tu_Ti_Icon = "Assets/GameData/Textures/Battle/WuXing/Tu_Ti.png";

		public const string Battle_Tu_Zhen_Icon = "Assets/GameData/Textures/Battle/WuXing/Tu_Zhen.png";

		public const string Battle_WuXing_ShenShi_Icon = "Assets/GameData/Textures/Battle/WuXing/WuXing_Shen.png";

		public const string Battle_WuXing_Ti_Icon = "Assets/GameData/Textures/Battle/WuXing/WuXing_Ti.png";

		public const string Battle_WuXing_Zhen_Icon = "Assets/GameData/Textures/Battle/WuXing/WuXing_Zhen.png";

		public const string BattlePlant_Icon = "Assets/GameData/Textures/Battle/Plant/Plant";

		public const string BattleMineral_Icon = "Assets/GameData/Textures/Battle/Mineral/Mineral";

		public const string Chat_Jiu_1_Icon = "Assets/GameData/Textures/LingQuanJiuShi/ChatJiu_1.png";

		public const string Chat_Jiu_2_Icon = "Assets/GameData/Textures/LingQuanJiuShi/ChatJiu_2.png";

		public const string Chat_Jiu_3_Icon = "Assets/GameData/Textures/LingQuanJiuShi/ChatJiu_3.png";

		public const string ZongMen_LastNameImg_Pai = "Assets/GameData/Textures/ZongMen/ZongMenFont_Pai.png";

		public const string ZongMen_LastNameImg_Men = "Assets/GameData/Textures/ZongMen/ZongMenFont_Men.png";

		public const string ZongMen_LastNameImg_Zong = "Assets/GameData/Textures/ZongMen/ZongMenFont_Zong.png";

		public const string ZongMen_LastNameImg_Bang = "Assets/GameData/Textures/ZongMen/ZongMenFont_Bang.png";

		public const string ZongMen_LastNameImg_Jiao = "Assets/GameData/Textures/ZongMen/ZongMenFont_Jiao.png";

		public const string ZongMen_LastNameImg_Gong = "Assets/GameData/Textures/ZongMen/ZongMenFont_Gong.png";

		public const string ZongMen_Icon_1 = "Assets/GameData/Textures/ZongMen/ZongMenRiYaoIcon.png";

		public const string ZongMen_Icon_2 = "Assets/GameData/Textures/ZongMen/ZongMenYueHuiIcon.png";

		public const string ZongMen_Icon_3 = "Assets/GameData/Textures/ZongMen/ZongMenXingGuangIcon.png";

		public const string ZongMen_Icon_4 = "Assets/GameData/Textures/ZongMen/ZongMenXuanTieIcon.png";

		public const string Goods_KuangShi_Icon = "Assets/GameData/Textures/Item/Goods_KuangShi.png";

		public const string Goods_Xie_Icon = "Assets/GameData/Textures/Item/Goods_Xie.png";

		public const string Goods_FangJu_Icon = "Assets/GameData/Textures/Item/Goods_FangJu.png";

		public const string Goods_DanYao_Icon = "Assets/GameData/Textures/Item/Goods_DanYao.png";

		public const string Goods_PiMao_Icon = "Assets/GameData/Textures/Item/Goods_PiMao.png";

		public const string Goods_ShouGu_Icon = "Assets/GameData/Textures/Item/Goods_ShouGu.png";

		public const string Goods_WuQi_Icon = "Assets/GameData/Textures/Item/Goods_WuQi.png";

		public const string Goods_Dun_Icon = "Assets/GameData/Textures/Item/Goods_DefenceWeapon.png";

		public const string Goods_FuLu_Icon = "Assets/GameData/Textures/Item/Goods_FuLu.png";

		public const string Goods_ShinyStone = "Assets/GameData/Textures/Item/QianDaoLingShiItem.png";

		public const string Goods_ShinyJade = "Assets/GameData/Textures/Item/QianDaoLingYuItem.png";

		public const string Goods_GongXianDian = "Assets/GameData/Textures/Item/Goods_XuShenDian.png";

		public const string Goods_Shu_DanYao_Icon = "Assets/GameData/Textures/Item/Goods_DanShu.png";

		public const string Goods_Shu_FuLu_Icon = "Assets/GameData/Textures/Item/Goods_FuShu.png";

		public const string Goods_Shu_FaShu_Icon = "Assets/GameData/Textures/Item/Goods_FaSuShu.png";

		public const string Goods_Shu_GongFa_Icon = "Assets/GameData/Textures/Item/Goods_GongFaShu.png";

		public const string Goods_Shu_QiQiao_Icon = "Assets/GameData/Textures/Item/Goods_QiaoShu.png";

		public const string Goods_Shu_QiTu_Icon = "Assets/GameData/Textures/Item/Goods_QiTu.png";

		public const string Goods_Shu_YuShou_Icon = "Assets/GameData/Textures/Item/Goods_ShouTu.png";

		public const string Goods_Shu_ZhenTu_Icon = "Assets/GameData/Textures/Item/Goods_ZhenTu.png";

		public const string Goods_Shu_ZhenPan_Icon = "Assets/GameData/Textures/Item/Goods_ZhenPan.png";

		public const string Goods_Shu_ZhongZhi_Icon = "Assets/GameData/Textures/Item/Goods_ZhiWuShu.png";

		public const string Goods_YaoDan_Icon = "Assets/GameData/Textures/Item/Goods_YaoDan.png";

		public const string Goods_JingXue_Icon = "Assets/GameData/Textures/Item/Goods_JingXue.png";

		public const string Goods_ZhiWu_Icon = "Assets/GameData/Textures/Item/Goods_ZhiWu.png";

		public const string Goods_MiJIng_Icon = "Assets/GameData/Textures/Item/Goods_MiJing.png";

		public const string Goods_CanJuan_Icon = "Assets/GameData/Textures/Item/Goods_CanJuan.png";

		public const string Goods_LingKui_Icon = "Assets/GameData/Textures/Item/Goods_LingKui.png";

		public const string Goods_TiShenKuiLei_Icon = "Assets/GameData/Textures/Item/Goods_TiShenKuiLei.png";

		public const string Goods_WuDaoShi_Icon = "Assets/GameData/Textures/Item/Goods_WuDaoShi.png";

		public const string Goods_YunTie_Icon = "Assets/GameData/Textures/Item/Goods_YunTie.png";

		public const string Goods_Rank_Huang_Bg = "Assets/GameData/Textures/Item/Rank_Huang.png";

		public const string Goods_Rank_Xuan_Bg = "Assets/GameData/Textures/Item/Rank_Xuan.png";

		public const string Goods_Rank_Di_Bg = "Assets/GameData/Textures/Item/Rank_Di.png";

		public const string Goods_Rank_Tian_Bg = "Assets/GameData/Textures/Item/Rank_Tian.png";

		public const string FaShu_AreaDamage_Icon = "Assets/GameData/Textures/Item/FaShu_AreaDamage.png";

		public const string FaShu_OneDamage_Icon = "Assets/GameData/Textures/Item/FaShu_OneDamage.png";

		public const string FaShu_ContinueDamage_Icon = "Assets/GameData/Textures/Item/FaShu_ContinueDamage.png";

		public const string FaShu_DefenceSkill_Icon = "Assets/GameData/Textures/Item/FaShu_DefenceSkill.png";

		public const string FaShu_DownStatus_Icon = "Assets/GameData/Textures/Item/FaShu_DownStatus.png";

		public const string FaShu_UpStatus_Icon = "Assets/GameData/Textures/Item/FaShu_UpStatus.png";

		public const string FaShu_NoMove_Icon = "Assets/GameData/Textures/Item/FaShu_NoMove.png";

		public const string FaShu_DunShu_Icon = "Assets/GameData/Textures/Item/FaShu_MoveAdd.png";

		public const string FaShu_TaoYi_Icon = "Assets/GameData/Textures/Item/FaShu_MoveAdd.png";

		public const string City_Low_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-2.png";

		public const string City_Normal_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-3.png";

		public const string City_Hight_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-4.png";

		public const string City_Largest_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-5.png";

		public const string Battle_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ZhanDou.png";

		public const string Disable_City_Low_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-2_Disable.png";

		public const string Disable_City_Normal_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-3_Disable.png";

		public const string Disable_City_Hight_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-4_Disable.png";

		public const string Disable_City_Largest_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ChenShi1-5_Disable.png";

		public const string Disable_Battle_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/ChengChi/ZhanDou_Disable.png";

		public const string Disable_YuanDian_Icon = "Assets/GameData/Textures/ChuanSongZhen/DaLiuBai/Disable_YXHen.png";

		public const string LianQi_Jian_Icon = "Assets/GameData/Textures/LianQi/LQ_Jian.png";

		public const string LianQi_Bang_Icon = "Assets/GameData/Textures/LianQi/LQ_Bang.png";

		public const string LianQi_Chang_Icon = "Assets/GameData/Textures/LianQi/LQ_Chang.png";

		public const string LianQi_Chi_Icon = "Assets/GameData/Textures/LianQi/LQ_Chi.png";

		public const string LianQi_Chui_Icon = "Assets/GameData/Textures/LianQi/LQ_Chui.png";

		public const string LianQi_Dao_Icon = "Assets/GameData/Textures/LianQi/LQ_Dao.png";

		public const string LianQi_Ding_Icon = "Assets/GameData/Textures/LianQi/LQ_Ding.png";

		public const string LianQi_Dun_Icon = "Assets/GameData/Textures/LianQi/LQ_Dun.png";

		public const string LianQi_Fan_Icon = "Assets/GameData/Textures/LianQi/LQ_Fan.png";

		public const string LianQi_Fu_Icon = "Assets/GameData/Textures/LianQi/LQ_Fu.png";

		public const string LianQi_Gong_Icon = "Assets/GameData/Textures/LianQi/LQ_Gong.png";

		public const string LianQi_Ji_Icon = "Assets/GameData/Textures/LianQi/LQ_Ji.png";

		public const string LianQi_Jia_Icon = "Assets/GameData/Textures/LianQi/LQ_Jia.png";

		public const string LianQi_Ling_Icon = "Assets/GameData/Textures/LianQi/LQ_Ling.png";

		public const string LianQi_LingDang_Icon = "Assets/GameData/Textures/LianQi/LQ_LingDang.png";

		public const string LianQi_Mao_Icon = "Assets/GameData/Textures/LianQi/LQ_Mao.png";

		public const string LianQi_Qi_Icon = "Assets/GameData/Textures/LianQi/LQ_Qi.png";

		public const string LianQi_Qiang_Icon = "Assets/GameData/Textures/LianQi/LQ_Qiang.png";

		public const string LianQi_Qin_Icon = "Assets/GameData/Textures/LianQi/LQ_Qin.png";

		public const string LianQi_Shang_Icon = "Assets/GameData/Textures/LianQi/LQ_Shang.png";

		public const string LianQi_Suo_Icon = "Assets/GameData/Textures/LianQi/LQ_Suo.png";

		public const string LianQi_Xue_Icon = "Assets/GameData/Textures/LianQi/LQ_Xue.png";

		public const string LianQi_Yi_Icon = "Assets/GameData/Textures/LianQi/LQ_Yi.png";

		public const string LianQi_Yin_Icon = "Assets/GameData/Textures/LianQi/LQ_Yin.png";

		public const string LianQi_Yue_Icon = "Assets/GameData/Textures/LianQi/LQ_Yue.png";

		public const string LianQi_Zhen_Icon = "Assets/GameData/Textures/LianQi/LQ_Zhen.png";

		public const string LianQi_Zhong_Icon = "Assets/GameData/Textures/LianQi/LQ_Zhong.png";

		public const string LianQi_Zhu_Icon = "Assets/GameData/Textures/LianQi/LQ_Zhu.png";

		public const string LianQi_Zhua_Icon = "Assets/GameData/Textures/LianQi/LQ_Zhua.png";

		public const string LianQi_Zhuo_Icon = "Assets/GameData/Textures/LianQi/LQ_Zhuo.png";

		public const string FIGHTINGSCENE = "FightingScene";

		public const string SIGNINSCENE = "LoginScene";

		public const string MAINSCENE = "MainScene";

		public const string STARTLOADSCENE = "StartLoadScene";

		public const string DONGFUSCENE = "DongFuScene";

		public const string DONGFU_EXMPLE_SCENE = "DongFuExmpleScene";

		public const string LOGOSCENE = "LogoScene";

		public const string DIHUOGE_LIANDAN_SCENE = "DiHuoGe_LianDan";

		public const string DONGFU_DUJIE_SCENE = "TuPoScene";

		public const string DONGFU_DUJIE_EXMPLE_SCENE = "TuPoExmpleScene";

		public const string CHUANGZHAOGONGFA_SCENE = "ChuangZhaoGongFaScene";

		public const string CHUANGZAOFASHU_SCENE = "ChuangZhaoFaShuScene";

		public const string NEISHI_SCENE = "NeiShiScene";

		public const string ZONGMEN_SCENE = "ZongMenScene";

		public const string LUAPARENTTEXTPATH = "Assets/GameData/LuaTxt/";

		public const string MP3_MAIN_BG = "Assets/GameData/Music/MainCityBgMusic.mp3";

		public const string MP3_DONGFU_BG = "Assets/GameData/Music/DongFuBgMusic.mp3";

		public const string MP3_BATTLE_BG = "Assets/GameData/Music/BattleBgMusic.mp3";

		public const string WAV_BUTTON_CLICK = "Assets/GameData/Music/ButtonVoice.wav";

		public const string WAV_ATTACK_JIN = "Assets/GameData/Music/Attack_JinVoice.wav";

		public const string WAV_ATTACK_MU = "Assets/GameData/Music/Attack_MuVoice.wav";

		public const string WAV_ATTACK_SHUI = "Assets/GameData/Music/Attack_ShuiVoice.wav";

		public const string WAV_ATTACK_HUO = "Assets/GameData/Music/Attack_HuoVoice.wav";

		public const string WAV_ATTACK_TU = "Assets/GameData/Music/Attack_TuVoice.wav";

		public const string WAV_ATTACK_ALL = "Assets/GameData/Music/Attack_AllVoice.wav";

		public const string WAV_LIANDAN_SUCCESS = "Assets/GameData/Music/LianDanSuccess.wav";

		public const string WAV_LIANDAN_FAIL = "Assets/GameData/Music/LianDanFail.wav";

		public const string WAV_LIANDAN_FIRE = "Assets/GameData/Music/LianDanFire.wav";

		public const string LIANQI_VOICE = "Assets/GameData/Music/LianQiVoice.wav";

		public const string LIANQI_BG_MUSIC = "Assets/GameData/Music/LianQiBgMusic.wav";

		public const string ZHENFA_BG_MUSIC = "Assets/GameData/Music/ZhenFaBgMusic.wav";

		public const string GUIDE_Shader = "Assets/GameData/Shaders/CubeGuideMask.shader";

		public const string GUIDE_Material = "Assets/GameData/Materials/GuideMat.mat";

		public const string BuDaoLevel_JSON = "Assets/GameData/Json/a.json";

		public const string ChuangZaoGongFa_JSON = "Assets/GameData/Json/b.json";

		public const string FaShuLevel_JSON = "Assets/GameData/Json/c.json";

		public const string ShouYin_JSON = "Assets/GameData/Json/d.json";

		public const string FuLuLevel_JSON = "Assets/GameData/Json/e.json";

		public const string LingZhi_JSON = "Assets/GameData/Json/xzjPlantingList.json";

		public const string FuZhiYe_JSON = "Assets/GameData/Json/s.json";

		public const string XzjIconPathList_JSON = "Assets/GameData/Json/xzjIconPathList.json";

		public const string XzjRoleLevelList_JSON = "Assets/GameData/Json/xzjRoleLevelList.json";

		public const string XzjWorldAssessElixirList_JSON = "Assets/GameData/Json/xzjWorldAssessElixirList.json";

		public const string XzjWorldAssessLianQiList_JSON = "Assets/GameData/Json/xzjLianQiList.json";

		public const string XzjWorldAssessZhenFaList_JSON = "Assets/GameData/Json/xzjZhenFaList.json";

		public const string XzjWorldControllerMatrixList_JSON = "Assets/GameData/Json/xzjWorldControllerMatrixList.json";

		public const string XzjWorldGatherMatrixList_JSON = "Assets/GameData/Json/xzjWorldGatherMatrixList.json";

		public const string XzjWorldHomeList_JSON = "Assets/GameData/Json/xzjWorldHomeList.json";

		public const string CodeList_JSON = "Assets/GameData/Json/Code.json";

		public const string MissionList_JSON = "Assets/GameData/Json/Mission.json";

		public const string CoefficientHuaFuNum_JSON = "Assets/GameData/Json/CoefficientHuaFuNum.json";

		public const string CoefficientLianDanNum_JSON = "Assets/GameData/Json/CoefficientLianDanNum.json";

		public const string XuanTianLouGoods_JSON = "Assets/GameData/Json/XuanTianLouGoods.json";

		public const string ChengHaoInfo_JSON = "Assets/GameData/Json/ChengHaoInfo.json";

		public const string CashLiBao_JSON = "Assets/GameData/Json/LiBaoGoods.json";

		public const string EventGoods_JSON = "Assets/GameData/Json/EventGoods.json";

		public const string VipInfo_JSON = "Assets/GameData/Json/VipLevelInfo.json";

		public const string ZongMenShanFengCangKuUpdateInfo_JSON = "Assets/GameData/Json/ZongMenShanFengCangKuUpdateInfo.json";

		public const string ZongMenShanFengFuZhiUpdateInfo_JSON = "Assets/GameData/Json/ZongMenShanFengFuZhiUpdateInfo.json";

		public const string ZongMenShanFengLianDanUpdateInfo_JSON = "Assets/GameData/Json/ZongMenShanFengLianDanUpdateInfo.json";

		public const string ZongMenShanFengNeiMenUpdateInfo_JSON = "Assets/GameData/Json/ZongMenShanFengNeiMenUpdateInfo.json";

		public const string ZongMenShanFengYanWuChangUpdateInfo_JSON = "Assets/GameData/Json/ZongMenShanFengYanWuChangUpdateInfo.json";

		public const string ZongMenShanFengKuangMaiUpdateInfo_JSON = "Assets/GameData/Json/ZongMenShanFengKuangMaiUpdateInfo.json";

		public const string ZongMenCangBaoGeGoods_JSON = "Assets/GameData/Json/ZongMenCangBaoGe.json";

		public const string WorldChatID = "world";
	}
}
