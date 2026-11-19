package server.game;

import xzj.Xzj;

import java.util.ArrayList;
import java.util.List;

/** 1007005 当前地图的所有父节点 id（从自己往上一直到修真界）。 */
public class ParentMapIdHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ParentMapIdReq_1007005 req = Xzj.ParentMapIdReq_1007005.parseFrom(msg.getData());
        Xzj.ParentMapIdResp_1007005.Builder b = Xzj.ParentMapIdResp_1007005.newBuilder();
        b.setIsSuccess(true).setCode(4177);
        MapTree.Node n = MapTree.find(req.getMapId());
        while (n != null) {
            b.addId(n.id);
            if ("0".equals(n.parentId)) break;
            n = MapTree.find(n.parentId);
        }
        session.send(1007005, msg.getMsid(), b.build());
    }
}
