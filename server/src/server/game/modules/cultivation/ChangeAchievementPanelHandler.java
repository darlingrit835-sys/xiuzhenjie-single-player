package server.game;

import xzj.Xzj;

/**
 * 1005009 更换功法面板：只显示当前类型（纳气/锻体/炼神）的已学功法。
 */
public class ChangeAchievementPanelHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ChangeAchievementPanelReq_1005009 req = Xzj.ChangeAchievementPanelReq_1005009.parseFrom(msg.getData());
        int type = req.getAchievementType();
        if (type < 1 || type > 3) type = 3;

        Xzj.ChangeAchievementPanelResp_1005009.Builder b = Xzj.ChangeAchievementPanelResp_1005009.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            GameData.GongFaDef current = RoleUtil.equipped(session.role, type);
            if (current != null) {
                b.setCurrentAchievement(build(session.role, current));
            }
            for (GameData.GongFaDef g : GameData.gongFaByType(type)) {
                if (RoleUtil.isLearned(session.role, g.id)) {
                    if (current != null && current.id.equals(g.id)) continue;
                    b.addAchievements(build(session.role, g));
                }
            }
            b.setCode(2000).setIsSuccess(true);
        }
        session.send(1005009, msg.getMsid(), b.build());
    }

    private Xzj.ChangeAchievementPanelResp_1005009.AchievementObjectResp_1005010.Builder build(
            java.util.Map<String, Object> role, GameData.GongFaDef g) {
        Xzj.ChangeAchievementPanelResp_1005009.AchievementObjectResp_1005010.Builder o =
                Xzj.ChangeAchievementPanelResp_1005009.AchievementObjectResp_1005010.newBuilder();
        o.setId(g.id);
        o.setName(g.name);
        o.setCircle(RoleUtil.circleFor(role, g));
        o.setAchievementLevel(RoleUtil.chong(role, g.id));
        o.setMaxCount(3);
        o.setUsedCount(0);
        o.setLevel(g.levelReq);
        o.setRank(g.rank);
        o.setFiveElement(g.fiveElement);
        return o;
    }
}
