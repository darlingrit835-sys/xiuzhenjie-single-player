package server.game;

import xzj.Xzj;

/** 1005004 重建内室：把已有内室换成另一种副职区域，消耗灵玉。 */
public class RebuildLiveTypeHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RebuildLiveTypeReq_1005004 req = Xzj.RebuildLiveTypeReq_1005004.parseFrom(msg.getData());
        int newType = req.getNewLiveType();
        int location = req.getLocation();

        Xzj.RebuildLiveTypeResp_1005004.Builder b = Xzj.RebuildLiveTypeResp_1005004.newBuilder();
        String log;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            int oldType = HomeService.live(session.role, location);
            long jade = RoleUtil.lng(session.role, "ShinyJade", 0);
            int otherLoc = location == 2 ? 1 : 2;
            int otherType = HomeService.live(session.role, otherLoc);

            b.setShinyJade(jade);
            if (!HomeService.validLiveType(newType) || location < 1 || location > 2) {
                b.setIsSuccess(false).setCode(4116);
                log = "[Home] rebuild failed: bad param new=" + newType + " loc=" + location;
            } else if (oldType == 0) {
                b.setIsSuccess(false).setCode(4116);      // 此处并未开辟内室
                log = "[Home] rebuild failed: location " + location + " empty";
            } else if (newType == otherType) {
                b.setIsSuccess(false).setCode(4118);      // 不能重建为已有同类型
                log = "[Home] rebuild failed: duplicate type " + newType;
            } else if (jade < HomeService.REBUILD_JADE) {
                b.setIsSuccess(false).setCode(4117);      // 灵玉不足
                log = "[Home] rebuild failed: need jade " + HomeService.REBUILD_JADE + ", have " + jade;
            } else {
                jade -= HomeService.REBUILD_JADE;
                session.role.put("ShinyJade", jade);
                HomeService.setLive(session.role, location, newType);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(4119).setShinyJade(jade);
                log = "[Home] rebuilt loc " + location + ": " + HomeService.liveName(oldType)
                        + " -> " + HomeService.liveName(newType) + ", jade=" + jade;
            }
        }
        session.send(1005004, msg.getMsid(), b.build());
        System.out.println(log);
    }
}
