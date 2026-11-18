package server.game;

import xzj.Xzj;

/**
 * 1001010 突破境界。成功升一级，并按等级表重算神识/体魄/精血/寿元/战力。
 */
public class BreakthroughHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RoleBreakRealmResp_1001010.Builder b = Xzj.RoleBreakRealmResp_1001010.newBuilder();
        boolean questDone = false;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            if (!RoleUtil.canBreak(session.role)) {
                b.setIsSuccess(false).setCode(4062);
            } else {
                RoleUtil.levelUp(session.role);
                b.setIsSuccess(true).setCode(4306);
                questDone = QuestService.onLevelUp(session.role);
                if (questDone) RoleUtil.save(session.role);
            }
            b.setPureBlood(RoleUtil.num(session.role, "PureBlood"));
            b.setFlexibleRoot(RoleUtil.str(session.role, "FlexibleRoot"));
            b.setSuperAir(RoleUtil.lng(session.role, "SuperAir"));
            b.setPerception(RoleUtil.lng(session.role, "Perception"));
            b.setPhysique(RoleUtil.lng(session.role, "Physique"));
            b.setCurrentSuperAir(RoleUtil.lng(session.role, "CurrentSuperAir"));
            b.setCurrentPerception(RoleUtil.lng(session.role, "CurrentPerception"));
            b.setCurrentPhysique(RoleUtil.lng(session.role, "CurrentPhysique"));
            b.setCurrentVigor(RoleUtil.num(session.role, "CurrentVigor"));
            b.setCurrentClever(RoleUtil.num(session.role, "CurrentClever"));
            b.setCurrentPoisonVal(RoleUtil.num(session.role, "CurrentPoisonVal"));
            b.setCurrentPureBlood(RoleUtil.num(session.role, "CurrentPureBlood"));
            b.setAge(RoleUtil.num(session.role, "MaxAge"));
            b.setCurrentPower(RoleUtil.num(session.role, "CurrentPower"));
        }
        session.send(1001010, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }
}
