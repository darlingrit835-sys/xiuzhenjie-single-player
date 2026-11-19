package server.game;

import xzj.Xzj;

public class BuildChatHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        System.out.println("[BuildChat]");
        boolean questDone = false;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            questDone = QuestService.onChat(session.role);
            RoleUtil.save(session.role);
        }
        Xzj.BuildChatResp_1012001.Builder b = Xzj.BuildChatResp_1012001.newBuilder();
        b.setCode(3003).setIsSuccess(false);
        b.setAddress("");
        b.setPort(0);
        session.send(1012001, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }
}
