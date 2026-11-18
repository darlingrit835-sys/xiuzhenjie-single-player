package server.game;

import xzj.Xzj;

/** 1007006 当前地图的父节点信息（不含自己）。 */
public class ParentMapHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ParentMapReq_1007006 req = Xzj.ParentMapReq_1007006.parseFrom(msg.getData());
        Xzj.ParentMapResp_1007006.Builder b = Xzj.ParentMapResp_1007006.newBuilder();
        b.setIsSuccess(true).setCode(4178);
        MapTree.Node n = MapTree.find(req.getMapId());
        if (n != null) n = MapTree.find(n.parentId);
        while (n != null) {
            b.addObject(toObj(n));
            if ("0".equals(n.parentId)) break;
            n = MapTree.find(n.parentId);
        }
        session.send(1007006, msg.getMsid(), b.build());
    }

    static Xzj.ParentMapResp_1007006.MapObjectResp_1007007.Builder toObj(MapTree.Node n) {
        Xzj.ParentMapResp_1007006.MapObjectResp_1007007.Builder o =
                Xzj.ParentMapResp_1007006.MapObjectResp_1007007.newBuilder();
        o.setId(n.id);
        o.setParentId(n.parentId);
        MapTree.Node p = MapTree.find(n.parentId);
        o.setParentName(p == null ? "" : p.name);
        o.setName(n.name);
        o.setSibingsId("");
        o.setChildrenIds("");
        o.setType(n.type);
        o.setMinLevel(n.minLevel);
        o.setMaxLevel(n.maxLevel);
        o.setStatus(1);
        o.setRestrict("");
        o.setBasedConcentration(1.0);
        o.setFiveElements(0);
        o.setSort(1);
        o.setLevel(n.level);
        return o;
    }
}
