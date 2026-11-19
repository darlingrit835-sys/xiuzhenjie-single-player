package server.game;

import xzj.Xzj;

public class AncestorMapHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        System.out.println("[AncestorMap]");
        Xzj.AncestorMapResp_1007008.Builder b = Xzj.AncestorMapResp_1007008.newBuilder();
        MapTree.Node a = MapTree.ancestor();
        b.setIsSuccess(true).setCode(2000);
        b.setId(a.id);
        b.setName(a.name);
        session.send(1007008, msg.getMsid(), b.build());
    }
}
