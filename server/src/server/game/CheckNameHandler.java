package server.game;

import xzj.Xzj;

public class CheckNameHandler implements MsgHandler {
    private final RoleStore roles = RoleStore.INSTANCE;

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.CheckNameReq_1002005 req = Xzj.CheckNameReq_1002005.parseFrom(msg.getData());
        System.out.println("[CheckName] name=" + req.getName());

        Xzj.CheckNameResp_1002005.Builder b = Xzj.CheckNameResp_1002005.newBuilder();
        if (roles.nameExists(req.getName())) {
            b.setIsSuccess(false).setCode(3002);
        } else {
            b.setIsSuccess(true).setCode(2000);
        }
        session.send(1002005, msg.getMsid(), b.build());
    }
}
