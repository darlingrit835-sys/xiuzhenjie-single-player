package server.game;

import xzj.Xzj;

/**
 * 1004004 背包变更校验。客户端用 MD5 判断是否需要重新拉取纳戒列表。
 */
public class SpaceGoodsCheckHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.SpaceGoodsCheckReq_1004004 req = Xzj.SpaceGoodsCheckReq_1004004.parseFrom(msg.getData());
        Xzj.SpaceGoodsCheckResp_1004004.Builder b = Xzj.SpaceGoodsCheckResp_1004004.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            GoodsService.ensureStarterKit(session.role);
            b.setIsSuccess(true).setCode(2000)
                    .setLocation(req.getLocation())
                    .setMd5(GoodsService.md5(session.role));
        }
        session.send(1004004, msg.getMsid(), b.build());
    }
}
