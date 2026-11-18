package server.game;

import xzj.Xzj;

/** 1002003 离线收益面板。 */
public class RoleOfflineHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) {
        Xzj.RoleOfflineResp_1002003.Builder b = Xzj.RoleOfflineResp_1002003.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            // 面板显示的是“本次离线增长量”，不是当前总值
            b.setSuperAir(Math.min(RoleUtil.lng(session.role, "OfflineSuperAir"), Integer.MAX_VALUE));
            b.setPerception(Math.min(RoleUtil.lng(session.role, "OfflinePerception"), Integer.MAX_VALUE));
            b.setPhysique(Math.min(RoleUtil.lng(session.role, "OfflinePhysique"), Integer.MAX_VALUE));
            b.setAge(0);
            b.setOfflineTime(RoleUtil.num(session.role, "OfflineTime"));
            b.setOfflineVigor(1);
            b.setOfflineClever(1);
            b.setOfflinePower(0);
            b.setIsFullTime(RoleUtil.bool(session.role, "OfflineIsFull"));
            b.setIsSuccess(true).setCode(4181);
        }
        session.send(1002003, msg.getMsid(), b.build());
    }
}
