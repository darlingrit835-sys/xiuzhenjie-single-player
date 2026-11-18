package server.game;

import xzj.Xzj;

import java.util.List;

/**
 * 1007013 外出历练：把一片妖兽林地展开成 3 个可战斗区域。
 */
public class LiLianChildrenMapHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.LiLianChildrenMapReq_1007013 req = Xzj.LiLianChildrenMapReq_1007013.parseFrom(msg.getData());
        Xzj.LiLianChildrenMapResp_1007013.Builder b = Xzj.LiLianChildrenMapResp_1007013.newBuilder();
        b.setIsSuccess(true).setCode(4312);

        MapTree.Node n = MapTree.find(req.getMapId());
        List<MapTree.Node> children;
        if (n != null && n.type == 4 && n.level == 4) {
            children = MapTree.wildChildren(n.id);
        } else if (n != null && n.type == 51) {
            children = MapTree.wildsOf(n.parentId);
            if (children.size() > 3) children = children.subList(0, 3);
        } else {
            children = MapTree.wildChildren(req.getMapId());
        }
        for (MapTree.Node c : children) {
            Xzj.LiLianChildrenMapResp_1007013.LiLianMapObjectResp_1007014.Builder o =
                    Xzj.LiLianChildrenMapResp_1007013.LiLianMapObjectResp_1007014.newBuilder();
            o.setId(c.id);
            o.setParentId(c.parentId);
            MapTree.Node p = MapTree.find(c.parentId);
            o.setParentName(p == null ? "" : p.name);
            o.setName(c.name);
            o.setSibingsId("");
            o.setChildrenIds("");
            o.setType(c.type);
            o.setMinLevel(c.minLevel);
            o.setMaxLevel(c.maxLevel);
            o.setStatus(1);
            o.setRestrict("");
            o.setBasedConcentration(1.0);
            o.setFiveElements(0);
            o.setSort(1);
            o.setLevel(c.level);
            o.setIsCleanMap(false);
            b.addObject(o);
        }
        session.send(1007013, msg.getMsid(), b.build());
    }
}
