package server.game;

import xzj.Xzj;

/**
 * 1001008 突破询问。返回 HasThunder=0，客户端走普通突破流程。
 */
public class BreakthroughAskHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RoleBreakAskResp_1001008.Builder b = Xzj.RoleBreakAskResp_1001008.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            int lv = RoleUtil.num(session.role, "Level", 1);
            if (lv >= GameData.maxLevel()) {
                b.setIsSuccess(false).setCode(4062);
            } else if (!RoleUtil.canBreak(session.role)) {
                b.setIsSuccess(false).setCode(4062);
            } else {
                // HasThunder=0：本私服不做雷劫小游戏，直接突破
                b.setIsSuccess(true).setCode(4063).setHasThunder(0);
            }
        }
        session.send(1001008, msg.getMsid(), b.build());
    }
}
