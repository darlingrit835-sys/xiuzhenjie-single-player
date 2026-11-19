package server.game;

import xzj.Xzj;

/** 1002004 领取/放弃离线收益。 */
public class RoleGetOfflineHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RoleGetOfflineResp_1002004.Builder b = Xzj.RoleGetOfflineResp_1002004.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            b.setIsSuccess(true).setCode(2000);
            b.setSuperAir(RoleUtil.lng(session.role, "SuperAir"));
            b.setCurrentSuperAir(RoleUtil.lng(session.role, "CurrentSuperAir"));
            b.setPerception((int) RoleUtil.lng(session.role, "Perception"));
            b.setCurrentPerception((int) RoleUtil.lng(session.role, "CurrentPerception"));
            b.setPhysique((int) RoleUtil.lng(session.role, "Physique"));
            b.setCurrentPhysique((int) RoleUtil.lng(session.role, "CurrentPhysique"));
            b.setAge(RoleUtil.num(session.role, "CurrentAge"));
            b.setVigor(RoleUtil.num(session.role, "CurrentVigor"));
            b.setClever(RoleUtil.num(session.role, "CurrentClever"));
            b.setPoisonVal(RoleUtil.num(session.role, "CurrentPoisonVal"));
            b.setPureBlood(RoleUtil.num(session.role, "PureBlood"));
            b.setCurrentPureBlood(RoleUtil.num(session.role, "CurrentPureBlood"));
            b.setPower(RoleUtil.num(session.role, "CurrentPower"));
        }
        session.send(1002004, msg.getMsid(), b.build());
    }
}
