package server.game;

import xzj.Xzj;

public class HandlerRegistry {
    public static void init() {
        GameServer.register(9000000, new HeartbeatHandler());
        GameServer.register(1002001, new RoleCreateHandler());
        GameServer.register(1002002, new RoleLoginHandler());
        GameServer.register(1002003, new RoleOfflineHandler());
        GameServer.register(1002004, new RoleGetOfflineHandler());
        GameServer.register(1002005, new CheckNameHandler());
        GameServer.register(1002018, new SearchSignInAwardHandler());
        GameServer.register(1002021, new GetSignInAwardHandler());

        // 新手引导 / 主线任务
        GameServer.register(1002013, new GuideInfoUpdateHandler());
        GameServer.register(1002014, new GuideInfoSkipHandler());
        GameServer.register(1016002, new ReceiveAwardHandler());

        // 修炼 / 洞府 / 功法
        GameServer.register(1001005, new UnderstandMethodHandler());
        GameServer.register(1001007, new ChangeRunLocationHandler());
        GameServer.register(1001008, new BreakthroughAskHandler());
        GameServer.register(1001010, new BreakthroughHandler());
        GameServer.register(1001011, new CanWuListHandler());
        GameServer.register(1001020, new UpgradeAchievementHandler());
        GameServer.register(1005008, new ExerciseRoomHandler());
        GameServer.register(1005009, new ChangeAchievementPanelHandler());
        GameServer.register(1005011, new ChangeAchievementHandler());
        GameServer.register(1011004, new MethodAchievementDetailHandler());

        // 背包 / 地图 / 洞府
        GameServer.register(1004002, new SpaceGoodsHandler());
        GameServer.register(1004004, new SpaceGoodsCheckHandler());
        GameServer.register(1004008, new GoodsBookAchievementDetailHandler());
        GameServer.register(1004018, new AttackEquipmentDetailHandler());
        GameServer.register(1004019, new DefenseEquipmentDetailHandler());
        GameServer.register(1004014, new DefenseFaQiDetailHandler());
        GameServer.register(1004023, new GoodsBindingHandler());
        GameServer.register(1004024, new GoodsUseHandler());
        GameServer.register(1004026, new PickUpGoodsHandler());
        GameServer.register(1004032, new UsedElixirHandler());
        GameServer.register(1004033, new EnterXuanTianLouHandler());
        GameServer.register(1004035, new BuyXuanTianLouGoodsHandler());
        GameServer.register(1005001, new EnterHomeHandler());
        GameServer.register(1005002, new UpgradeHomeHandler());
        GameServer.register(1005003, new BuildLiveTypeHandler());
        GameServer.register(1005004, new RebuildLiveTypeHandler());
        GameServer.register(1013001, new EnterPlantHandler());
        GameServer.register(1013003, new KaiKenLandHandler());
        GameServer.register(1007001, new SibingsMapHandler());
        GameServer.register(1007003, new MapChildrenHandler());
        GameServer.register(1007005, new ParentMapIdHandler());
        GameServer.register(1007006, new ParentMapHandler());
        GameServer.register(1007008, new AncestorMapHandler());
        GameServer.register(1007009, new MapInfoHandler());
        GameServer.register(1007010, new MapTransmitHandler());
        GameServer.register(1007011, new MapCityMigrateHandler());
        GameServer.register(1007012, new ParentMapAndSelfHandler());
        GameServer.register(1007013, new LiLianChildrenMapHandler());
        GameServer.register(1002015, new SearchBattleEquipmentHandler());

        // 装备 / 技能
        GameServer.register(1009001, new DressEquipmentHandler());
        GameServer.register(1009002, new UndressEquipmentHandler());
        GameServer.register(1009003, new SetSkillHandler());
        GameServer.register(1009004, new RemoveSkillHandler());
        GameServer.register(1009005, new EnterEquipmentHandler());
        GameServer.register(1009007, new EnterSkillHandler());

        // 战斗
        GameServer.register(1010001, new BuildDungeonHandler());
        GameServer.register(1010002, new JoinDungeonHandler());
        GameServer.register(1004037, new BattleEndLeaveHandler());

        GameServer.register(1012001, new BuildChatHandler());
    }
}
