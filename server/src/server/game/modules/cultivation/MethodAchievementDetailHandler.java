package server.game;

import xzj.Xzj;

/**
 * 1011004 功法详情。参悟面板点开一本功法时显示。
 */
public class MethodAchievementDetailHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.MethodAchievementDetailReq_1011004 req = Xzj.MethodAchievementDetailReq_1011004.parseFrom(msg.getData());
        Xzj.MethodAchievementDetailResp_1011004.Builder b = Xzj.MethodAchievementDetailResp_1011004.newBuilder();
        synchronized (session.role) {
            GameData.GongFaDef g = GameData.gongFa(req.getId());
            if (g == null) {
                b.setIsSuccess(false).setCode(4326);
            } else {
                b.setIsSuccess(true).setCode(2000);
                b.setActionScope(g.type);
                b.setLevel(g.levelReq);
                b.setStudyLevel(g.isSystem() ? RoleUtil.chong(session.role, g.id) : 1);
                b.setBasedConcentration(1);
                b.setConversionRate(g.baseCircle);
                b.setRunSpeed(2);
                b.setCreatedBy(g.isSystem() ? "天道传承" : "天道");
                b.setEffectType1(g.effectType1);
                b.setEffectValue1(g.effectValue1);
                b.setEffectType2(g.effectType2);
                b.setEffectValue2(g.effectValue2);
                b.setUsedCount(1);
                b.setMaxCount(1);
            }
        }
        session.send(1011004, msg.getMsid(), b.build());
    }
}
