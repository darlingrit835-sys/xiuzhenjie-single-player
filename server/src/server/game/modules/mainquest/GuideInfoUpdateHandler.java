package server.game;

import xzj.Xzj;

/**
 * 1002013 新手引导进度上报。
 *
 * 客户端每完成一段引导会发 Type：
 *   1 修行引导   -> PracticeGuideVal +1（到 4 记 100）
 *   2 战斗引导   -> BattleGuideVal +1（到 5 记 100）
 *   5 中阶坊市   -> ShopGuideVal = 100
 *   6 经脉引导   -> AcupointGuideVal +1（到 3 记 100）
 *   7 符箓引导   -> SignGuideVal = 100
 *   8 炼丹引导   -> ElixirGuideVal = 100
 * 其余类型只回成功，避免客户端卡引导。
 */
public class GuideInfoUpdateHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GuideInfoUpdateReq_1002013 req = Xzj.GuideInfoUpdateReq_1002013.parseFrom(msg.getData());
        int type = req.getType();
        Xzj.GuideInfoUpdateResp_1002013.Builder b = Xzj.GuideInfoUpdateResp_1002013.newBuilder();
        String reward = null;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            switch (type) {
                case 1: {
                    int v = Math.max(1, RoleUtil.num(session.role, "PracticeGuideVal", 1)) + 1;
                    int nv = v >= 4 ? 100 : v;
                    session.role.put("PracticeGuideVal", nv);
                    reward = GuideRewardService.grant(session.role, 1, nv);
                    break;
                }
                case 2: {
                    int v = Math.max(1, RoleUtil.num(session.role, "BattleGuideVal", 1)) + 1;
                    int nv = v >= 5 ? 100 : v;
                    session.role.put("BattleGuideVal", nv);
                    reward = GuideRewardService.grant(session.role, 2, nv);
                    break;
                }
                case 5:
                    session.role.put("ShopGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 5, 100);
                    break;
                case 6: {
                    int v = Math.max(1, RoleUtil.num(session.role, "AcupointGuideVal", 1)) + 1;
                    int nv = v >= 3 ? 100 : v;
                    session.role.put("AcupointGuideVal", nv);
                    reward = GuideRewardService.grant(session.role, 6, nv);
                    break;
                }
                case 7:
                    session.role.put("SignGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 7, 100);
                    break;
                case 8:
                    session.role.put("ElixirGuideVal", 100);
                    reward = GuideRewardService.grant(session.role, 8, 100);
                    break;
                default:
                    break;
            }
            RoleUtil.save(session.role);
            b.setIsSuccess(true).setCode(2000);
        }
        session.send(1002013, msg.getMsid(), b.build());
        if (reward != null) GameServer.notifyText(session, reward);
    }
}
