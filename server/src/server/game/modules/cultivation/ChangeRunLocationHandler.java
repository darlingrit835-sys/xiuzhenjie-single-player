package server.game;

import xzj.Xzj;

/**
 * 1001007 切换修炼方式。客户端传 1=炼神 2=锻体 3=纳气，服务端做异或切换。
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
            if (newLocation < 1 || newLocation > 3) {
                b.setIsSuccess(false).setCode(4059);
            } else {
                int bit = 1 << (newLocation - 1);
                int mask = RoleUtil.activeMask(session.role) ^ bit;
                int count = Integer.bitCount(mask & 7);
                int level = RoleUtil.num(session.role, "Level", 1);
                if (count == 0) {
                    b.setIsSuccess(false).setCode(4059);
                } else if (count > 1 && level < 31) {
                    // 元婴之前只能一处修行
                    b.setIsSuccess(false).setCode(4059);
                } else {
                    session.role.put("RunMask", mask);
                    session.role.put("RunLocation", RoleUtil.locationType(mask));
                    RoleUtil.save(session.role);
                    b.setIsSuccess(true).setCode(4058);
                }
            }
        }
        session.send(1001007, msg.getMsid(), b.build());
    }
}
