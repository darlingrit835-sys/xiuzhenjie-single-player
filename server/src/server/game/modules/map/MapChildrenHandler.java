package server.game;

import xzj.Xzj;

import java.util.List;

public class MapChildrenHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ChildrenMapReq_1007003 req = Xzj.ChildrenMapReq_1007003.parseFrom(msg.getData());
        System.out.println("[MapChildren] mapId=" + req.getMapId());

        Xzj.ChildrenMapResp_1007003.Builder b = Xzj.ChildrenMapResp_1007003.newBuilder();
        b.setCode(2000).setIsSuccess(true);

        for (MapTree.Node n : MapTree.children(req.getMapId())) {
            Xzj.ChildrenMapResp_1007003.MapObjectResp_1007004.Builder city =
                    Xzj.ChildrenMapResp_1007003.MapObjectResp_1007004.newBuilder();
            city.setId(n.id);
            city.setParentId(n.parentId);
            city.setParentName(MapTree.find(n.parentId) != null ? MapTree.find(n.parentId).name : "");
            city.setName(n.name);
            city.setSibingsId("");
            city.setChildrenIds("");
            city.setType(n.type);
            city.setMinLevel(n.minLevel);
            city.setMaxLevel(n.maxLevel);
            city.setStatus(1);
            city.setRestrict("");
            city.setBasedConcentration(1.0);
            city.setFiveElements(0);
            city.setSort(1);
            city.setLevel(n.level);
            b.addObject(city);
        }
        session.send(1007003, msg.getMsid(), b.build());
    }
}
