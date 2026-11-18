package server.game;

import xzj.Xzj;

/** 1002021 领取当日签到奖励。 */
public class GetSignInAwardHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) {
        Xzj.GetSignInAwardResp_1002021.Builder b = Xzj.GetSignInAwardResp_1002021.newBuilder();
        String award;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            award = SignInService.claim(session.role);
            if (award != null) RoleUtil.save(session.role);
        }
        if (award == null) {
            b.setIsSuccess(false).setCode(2008);
        } else {
            b.setIsSuccess(true).setCode(2000);
            GameServer.notifyText(session, "签到成功：" + award);
            System.out.println("[SignIn] " + session.roleId + " 领取 " + award);
        }
        session.send(1002021, msg.getMsid(), b.build());
    }
}
