package server.game;

import xzj.Xzj;

/**
 * 1002014 跳过新手引导。
 * Type=1 跳过修行引导，Type=2 跳过战斗引导；其它类型也按已跳过处理。
 */
public class GuideInfoSkipHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GuideInfoSkipReq_1002014 req = Xzj.GuideInfoSkipReq_1002014.parseFrom(msg.getData());
        int type = req.getType();
        Xzj.GuideInfoSkipResp_1002014.Builder b = Xzj.GuideInfoSkipResp_1002014.newBuilder();
        String reward = null;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            switch (type) {
                case 1:
                    session.role.put("PracticeGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 1, 100);
                    break;
                case 2:
                    session.role.put("BattleGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 2, 100);
                    break;
                case 5:
                    session.role.put("ShopGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 5, 100);
                    break;
                case 6:
                    session.role.put("AcupointGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 6, 100);
                    break;
                case 7:
                    session.role.put("SignGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 7, 100);
                    break;
                case 8:
                    session.role.put("ElixirGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 8, 100);
                    break;
                default:
                    session.role.put("PracticeGuideVal", 100);
                    session.role.put("BattleGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 1, 100);
                    GuideRewardService.grant(session.role, 2, 100);
                    break;
            }
            RoleUtil.save(session.role);
            b.setIsSuccess(true).setCode(2000);
        }
        session.send(1002014, msg.getMsid(), b.build());
        if (reward != null) GameServer.notifyText(session, reward);
    }
}
