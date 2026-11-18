package server.game;

import xzj.Xzj;

import java.util.Map;

/** 1004018 攻击法器详情。 */
public class AttackEquipmentDetailHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsArticleSetableAttackEquipmentReq_1004018 req =
                Xzj.GoodsArticleSetableAttackEquipmentReq_1004018.parseFrom(msg.getData());
        Xzj.GoodsArticleSetableAttackEquipmentResp_1004018.Builder b =
                Xzj.GoodsArticleSetableAttackEquipmentResp_1004018.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getId());
            if (g == null) {
                b.setIsSuccess(false).setCode(4605);
            } else {
                int rank = GoodsService.num(g, "Rank", 1);
                b.setIsSuccess(true).setCode(2000);
                b.setMinSpiritualityValue(10L * rank);
                b.setMaxSpiritualityValue(100L * rank);
                b.setConversionRate(1.0);
                b.setUsedPerception(0);
                b.setToughTenacity(rank * 10);
                b.setEffectType1(301);
                b.setEffectValue1(5 + rank);
                b.setEffectType2(302);
                b.setEffectValue2(2 + rank / 2);
                b.setCreatedBy("修真界");
                b.setVolume(1.0);
                b.setRechooseCount(0);
                b.setCoolDown(0);
            }
        }
        session.send(1004018, msg.getMsid(), b.build());
    }
}
