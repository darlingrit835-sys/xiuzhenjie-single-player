package server.game;

import xzj.Xzj;

/** 1007011 迁城。 */
public class MapCityMigrateHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.MapCityMigrateReq_1007011 req = Xzj.MapCityMigrateReq_1007011.parseFrom(msg.getData());
        Xzj.MapCityMigrateResp_1007011.Builder b = Xzj.MapCityMigrateResp_1007011.newBuilder();
        synchronized (session.role) {
            MapTree.Node n = MapTree.find(req.getMapId());
            if (n != null) {
                session.role.put("MapId", n.id);
                session.role.put("MapName", n.name);
                RoleUtil.save(session.role);
            }
            b.setIsSuccess(true).setCode(2000);
            b.setCurrentAge(RoleUtil.num(session.role, "CurrentAge"));
            b.setCurrentPower(RoleUtil.num(session.role, "CurrentPower"));
        }
        session.send(1007011, msg.getMsid(), b.build());
    }
}
