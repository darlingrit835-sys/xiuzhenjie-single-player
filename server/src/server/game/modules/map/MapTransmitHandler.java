package server.game;

import xzj.Xzj;

/** 1007010 传送大阵。 */
public class MapTransmitHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.MapTransmitReq_1007010 req = Xzj.MapTransmitReq_1007010.parseFrom(msg.getData());
        Xzj.MapTransmitResp_1007010.Builder b = Xzj.MapTransmitResp_1007010.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            MapTree.Node n = MapTree.find(req.getMapId());
            if (n == null) {
                b.setIsSuccess(false).setCode(4259);
            } else {
                session.role.put("MapId", n.id);
                session.role.put("MapName", n.name);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(4185);
                b.setShinyStone(RoleUtil.lng(session.role, "ShinyStone"));
            }
        }
        session.send(1007010, msg.getMsid(), b.build());
    }
}
