package server.game;

import xzj.Xzj;

/**
 * 1005011 更换正在修行的功法。
 */
public class ChangeAchievementHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ChangeAchievementReq_1005011 req = Xzj.ChangeAchievementReq_1005011.parseFrom(msg.getData());
        Xzj.ChangeAchievementResp_1005011.Builder b = Xzj.ChangeAchievementResp_1005011.newBuilder();
        boolean questDone = false;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            GameData.GongFaDef g = GameData.gongFa(req.getAchievementId());
            int scope = req.getScopeType();
            if (scope < 1 || scope > 3) scope = 3;
            if (g == null || g.type != scope) {
                b.setIsSuccess(false).setCode(4057);
            } else if (!RoleUtil.isLearned(session.role, g.id)) {
                b.setIsSuccess(false).setCode(4326);
            } else {
                int lv = RoleUtil.num(session.role, "Level", 1);
                if (lv < g.levelReq) {
                    // 境界不足，不能越阶使用
                    b.setIsSuccess(false).setCode(4057);
                } else {
                    RoleUtil.equip(session.role, scope, g.id);
                    questDone = QuestService.onGongFa(session.role, g.rank);
                    RoleUtil.save(session.role);
                    b.setIsSuccess(true).setCode(4056);
                    b.setAchievementCircleValue(RoleUtil.circle(session.role, scope));
                }
            }
        }
        session.send(1005011, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }
}
