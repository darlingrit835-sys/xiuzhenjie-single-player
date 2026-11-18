package server.game;

import xzj.Xzj;

/** 1005003 开辟内室（洞府专职区域）。 */
public class BuildLiveTypeHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.BuildLiveTypeReq_1005003 req = Xzj.BuildLiveTypeReq_1005003.parseFrom(msg.getData());
        int liveType = req.getLiveType();
        int location = req.getLocation();

        Xzj.BuildLiveTypeResp_1005003.Builder b = Xzj.BuildLiveTypeResp_1005003.newBuilder();
        String log;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            int rank = HomeService.rank(session.role);
            long stone = RoleUtil.lng(session.role, "ShinyStone", 0);
            int already = HomeService.live(session.role, location);
            int used = HomeService.usedRooms(session.role);
            int limit = HomeService.roomLimit(rank);
            int price = HomeService.buildPrice(used);

            b.setShinyStone(stone);
            if (!HomeService.validLiveType(liveType) || location < 1 || location > 2) {
                b.setIsSuccess(false).setCode(4331);
                log = "[Home] build failed: bad param live=" + liveType + " loc=" + location;
            } else if (rank <= 0) {
                b.setIsSuccess(false).setCode(4331);      // 需要黄阶洞府才有位置
                log = "[Home] build failed: rank 0 (需先升到黄阶)";
            } else if (already != 0) {
                b.setIsSuccess(false).setCode(4116);      // 该位置已有内室，只能重建
                log = "[Home] build failed: location " + location + " already has " + already;
            } else if (used >= limit) {
                b.setIsSuccess(false).setCode(4113);      // 可达上限
                log = "[Home] build failed: limit " + limit + " reached";
            } else if (stone < price) {
                b.setIsSuccess(false).setCode(4115);      // 灵石不足
                log = "[Home] build failed: need " + price + " stone, have " + stone;
            } else {
                stone -= price;
                session.role.put("ShinyStone", stone);
                HomeService.setLive(session.role, location, liveType);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(4114).setShinyStone(stone);
                log = "[Home] built " + HomeService.liveName(liveType) + " at loc " + location
                        + ", cost " + price + " stone, stone=" + stone;
            }
        }
        session.send(1005003, msg.getMsid(), b.build());
        System.out.println(log);
    }
}
