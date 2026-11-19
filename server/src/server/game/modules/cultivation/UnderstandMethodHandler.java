package server.game;

import xzj.Xzj;

/**
 * 1001005 参悟功法/法术/丹方等。这里实现功法（Type=2）。
 */
public class UnderstandMethodHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.UnderstandMethodReq_1001005 req = Xzj.UnderstandMethodReq_1001005.parseFrom(msg.getData());
        Xzj.ActionBoolResp_1001001.Builder b = Xzj.ActionBoolResp_1001001.newBuilder();
        boolean questDone = false;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            if (req.getType() == 2) {
                GameData.GongFaDef g = GameData.gongFa(req.getId());
                if (g != null) {
                    RoleUtil.addOwned(session.role, g.id);
                    questDone = QuestService.onGongFa(session.role, g.rank);
                    RoleUtil.save(session.role);
                    b.setIsSuccess(true).setCode(4048);
                } else {
                    b.setIsSuccess(false).setCode(4049);
                }
            } else {
                // 其它类型暂未设计，先返回“参悟失败”，避免客户端误以为成功
                b.setIsSuccess(false).setCode(4049);
            }
        }
        session.send(1001005, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }
}
