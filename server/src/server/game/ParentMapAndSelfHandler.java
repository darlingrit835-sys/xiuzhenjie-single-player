package server.game;

import xzj.Xzj;

/** 1007012 从自己到根节点的名称链，用于地图面包屑。 */
public class ParentMapAndSelfHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ParentMapAndOneselfMapReq_1007012 req = Xzj.ParentMapAndOneselfMapReq_1007012.parseFrom(msg.getData());
        Xzj.ParentMapAndOneselfMapResp_1007012.Builder b = Xzj.ParentMapAndOneselfMapResp_1007012.newBuilder();
        b.setIsSuccess(true).setCode(4312);
        MapTree.Node n = MapTree.find(req.getMapId());
        while (n != null) {
            b.addName(n.name);
            if ("0".equals(n.parentId)) break;
            n = MapTree.find(n.parentId);
        }
        session.send(1007012, msg.getMsid(), b.build());
    }
}
