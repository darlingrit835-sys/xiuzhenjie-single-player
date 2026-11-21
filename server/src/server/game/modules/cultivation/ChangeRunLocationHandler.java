package server.game;

import xzj.Xzj;

/**
 * 1001007 设置修炼方式。客户端传目标组合，1 至 3 为单门，4 至 6 为双修。
 * 元婴（31级）之前只能同时运行一门。
 */
public class ChangeRunLocationHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ChangeRunLocationReq_1001007 req = Xzj.ChangeRunLocationReq_1001007.parseFrom(msg.getData());
        int newLocation = req.getNewLocation();
        Xzj.ActionBoolResp_1001001.Builder b = Xzj.ActionBoolResp_1001001.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            if (!RoleUtil.canRunLocation(session.role, newLocation)) {
                b.setIsSuccess(false).setCode(4059);
            } else {
                session.role.put("RunMask", RoleUtil.switchMask(newLocation));
                session.role.put("RunLocation", newLocation);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(4058);
            }
        }
        session.send(1001007, msg.getMsid(), b.build());
    }
}
