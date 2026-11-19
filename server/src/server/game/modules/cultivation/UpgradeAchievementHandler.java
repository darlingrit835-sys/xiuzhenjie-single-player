package server.game;

import xzj.Xzj;

/**
 * 1001020 提升功法品阶。私服没有德经材料，返回明确提示。
 * 各阶级功法已经直接下发，不需要靠升品解锁。
 */
public class UpgradeAchievementHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.UpgradeAchievementConversionRateReq_1001020 req =
                Xzj.UpgradeAchievementConversionRateReq_1001020.parseFrom(msg.getData());
        Xzj.UpgradeAchievementConversionRateResp_1001020.Builder b =
                Xzj.UpgradeAchievementConversionRateResp_1001020.newBuilder();
        GameData.GongFaDef g = GameData.gongFa(req.getAchievementId());
        if (g == null) {
            b.setIsSuccess(false).setCode(4326);
        } else {
            b.setIsSuccess(false).setCode(4703);
            b.setRank(g.rank);
            b.setConversionRate(g.baseCircle);
            b.setEffectType1(g.effectType1);
            b.setEffectValue1(g.effectValue1);
            b.setEffectType2(g.effectType2);
            b.setEffectValue2(g.effectValue2);
            b.setNowCapacity(0);
        }
        session.send(1001020, msg.getMsid(), b.build());
    }
}
