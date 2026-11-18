package server.game;

import xzj.Xzj;

public class EnterHomeHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        System.out.println("[EnterHome]");
        String name = String.valueOf(session.role.getOrDefault("Name", "道友"));
        Xzj.EnterHomeResp_1005001.Builder b = Xzj.EnterHomeResp_1005001.newBuilder();
        b.setCode(2000).setIsSuccess(true);
        b.setName(name + "的洞府");
        b.setRank(1);
        b.setLive1Type(1);
        b.setLive2Type(2);
        session.send(1005001, msg.getMsid(), b.build());
    }
}
