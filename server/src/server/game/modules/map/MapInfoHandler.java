package server.game;

import xzj.Xzj;

/** 1007009 地图详情。 */
public class MapInfoHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.MapInfoReq_1007009 req = Xzj.MapInfoReq_1007009.parseFrom(msg.getData());
        Xzj.MapInfoResp_1007009.Builder b = Xzj.MapInfoResp_1007009.newBuilder();
        MapTree.Node n = MapTree.find(req.getMapId());
        if (n == null) {
            b.setIsSuccess(false).setCode(4259);
        } else {
            b.setIsSuccess(true).setCode(4180);
            b.setId(n.id);
            b.setParentId(n.parentId);
            MapTree.Node p = MapTree.find(n.parentId);
            b.setParentName(p == null ? "" : p.name);
            b.setName(n.name);
            b.setSibingsId("");
            b.setChildrenIds("");
            b.setType(n.type);
            b.setMinLevel(n.minLevel);
            b.setMaxLevel(n.maxLevel);
            b.setStatus(1);
            b.setRestrict("");
            b.setBasedConcentration(1.0);
            b.setFiveElements(0);
            b.setSort(1);
            b.setLevel(n.level);
        }
        session.send(1007009, msg.getMsid(), b.build());
    }
}
