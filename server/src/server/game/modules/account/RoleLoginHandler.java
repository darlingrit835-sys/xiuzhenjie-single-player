package server.game;

import xzj.Xzj;

import java.util.Map;

public class RoleLoginHandler implements MsgHandler {
    private final RoleStore roles = RoleStore.INSTANCE;

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RoleLoginReq_1002002 req = Xzj.RoleLoginReq_1002002.parseFrom(msg.getData());
        System.out.println("[Login] roleId=" + req.getRoleId() + " server=" + req.getGameServerIpManageId() + " ip=" + req.getLoginIp());

        Map<String, Object> r = roles.getRole(req.getRoleId());
        Xzj.RoleLoginResp_1002002.Builder b = Xzj.RoleLoginResp_1002002.newBuilder();
        if (r == null) {
            b.setIsSuccess(false).setCode(3001);
        } else {
            RoleUtil.ensureDefaults(r);
            GuideRewardService.resumePractice(r);
            QuestService.prepareLogin(r);
            RoleUtil.applyOffline(r);
            RoleUtil.save(r);
            session.roleId = (String) r.get("Id");
            session.lastPracticeGuideMessageId = 0;
            session.role.clear();
            session.role.putAll(r);
            fill(b, r);
            b.setIsSuccess(true).setCode(2000);
        }
        session.send(1002002, msg.getMsid(), b.build());

        if (r != null && boolval(r, "StarterKitNotice")) {
            r.put("StarterKitNotice", false);
            RoleUtil.save(r);
            GameServer.notifyText(session,
                    "新手礼包已放入纳戒：启灵经、青锋剑、青布道袍、玄铁护心镜、青布靴、纳气丹×3、回春散×3");
        }
    }

    static void fill(Xzj.RoleLoginResp_1002002.Builder b, Map<String, Object> r) {
        b.setId(str(r, "Id"));
        b.setMapId(str(r, "MapId"));
        b.setMapName(str(r, "MapName"));
        b.setSex(num(r, "Sex"));
        b.setHeadPortraitId(num(r, "HeadPortraitId"));
        b.setName(str(r, "Name"));
        b.setLevel(num(r, "Level"));
        b.setCurrentAge(num(r, "CurrentAge"));
        b.setMaxAge(num(r, "MaxAge"));
        b.setShinyStone(longnum(r, "ShinyStone"));
        b.setShinyJade(num(r, "ShinyJade"));
        b.setPureBlood(num(r, "PureBlood"));
        b.setFlexibleRoot(str(r, "FlexibleRoot"));
        b.setSuperAir(longnum(r, "SuperAir"));
        b.setPerception(longnum(r, "Perception"));
        b.setPhysique(longnum(r, "Physique"));
        b.setCurrentSuperAir(longnum(r, "CurrentSuperAir"));
        b.setCurrentPerception(longnum(r, "CurrentPerception"));
        b.setCurrentPhysique(longnum(r, "CurrentPhysique"));
        b.setCurrentVigor(num(r, "CurrentVigor"));
        b.setCurrentClever(num(r, "CurrentClever"));
        b.setCurrentPoisonVal(num(r, "CurrentPoisonVal"));
        b.setCurrentPureBlood(num(r, "CurrentPureBlood"));
        b.setLiveRelId(num(r, "LiveRelId"));
        b.setLive1Type(num(r, "Live1Type"));
        b.setLive2Type(num(r, "Live2Type"));
        b.setLive1Level(num(r, "Live1Level"));
        b.setLive2Level(num(r, "Live2Level"));
        b.setLive1Value(num(r, "Live1Value"));
        b.setLive2Value(num(r, "Live2Value"));
        b.setYueKaDays(num(r, "YueKaDays"));
        b.setTitle(num(r, "Title"));
        b.setHasWood(boolval(r, "HasWood"));
        b.setHasGold(boolval(r, "HasGold"));
        b.setHasWater(boolval(r, "HasWater"));
        b.setHasFire(boolval(r, "HasFire"));
        b.setHasEarth(boolval(r, "HasEarth"));
        // 战斗引导：客户端离开战斗时硬性要求 BattleGuideVal==100（或引导步数≥16），
        // 私服没有完整引导流程会导致玩家被永久困在战斗地图，故默认视为已完成。
        b.setBattleGuideVal(server.common.ServerConfig.get().completeBattleGuide ? 100 : num(r, "BattleGuideVal"));
        System.out.println("[Guide] BattleGuideVal=" + b.getBattleGuideVal());
        b.setPracticeGuideVal(num(r, "PracticeGuideVal"));
        b.setIsNewLetter(boolval(r, "IsNewLetter"));
        b.setAcupointGuideVal(num(r, "AcupointGuideVal"));
        b.setShopGuideVal(num(r, "ShopGuideVal"));
        b.setHunLiVal(num(r, "HunLiVal"));
        b.setSignGuideVal(num(r, "SignGuideVal"));
        b.setElixirGuideVal(num(r, "ElixirGuideVal"));
        b.setIsGetSignIn(boolval(r, "IsGetSignIn"));
        b.setSignInDay(num(r, "SignInDay"));

        b.setPower(num(r, "Power"));
        b.setCurrentPower(num(r, "CurrentPower"));
        b.setVigor(Math.max(100, num(r, "Vigor")));
        b.setClever(Math.max(100, num(r, "Clever")));
        b.setQiYun(num(r, "QiYun"));
        b.setTitleFrame(num(r, "TitleFrame"));
        b.setHeadFrame(num(r, "HeadFrame"));
        b.setIsTianDaoLing(boolval(r, "TianDaoLing"));
        b.setLeiChong(num(r, "LeiChong"));
        b.setVipLevel(num(r, "VipLevel"));
        b.setLive3Type(num(r, "Live3Type"));
        b.setLive3Level(num(r, "Live3Level"));
        b.setLive3Value(num(r, "Live3Value"));
        b.setLive4Type(num(r, "Live4Type"));
        b.setLive4Level(num(r, "Live4Level"));
        b.setLive4Value(num(r, "Live4Value"));
        b.setGuildId(num(r, "GuildId"));
        b.setGuildName(str(r, "GuildName"));
        b.setGuildSimpleName(str(r, "GuildSimpleName"));
        b.setTaskId(num(r, "TaskId"));
        b.setTaskStatus(num(r, "TaskStatus"));
        b.setTianShuBeiStatus(num(r, "TianShuBeiStatus"));
    }

    static String str(Map<String, Object> m, String k) {
        Object o = m.get(k);
        return o == null ? "" : String.valueOf(o);
    }

    static int num(Map<String, Object> m, String k) {
        Object o = m.get(k);
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try {
            return (int) Double.parseDouble(String.valueOf(o));
        } catch (NumberFormatException e) {
            return 0;
        }
    }

    static long longnum(Map<String, Object> m, String k) {
        Object o = m.get(k);
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).longValue();
        try {
            return (long) Double.parseDouble(String.valueOf(o));
        } catch (NumberFormatException e) {
            return 0;
        }
    }

    static boolean boolval(Map<String, Object> m, String k) {
        Object o = m.get(k);
        return o != null && Boolean.parseBoolean(String.valueOf(o));
    }
}
