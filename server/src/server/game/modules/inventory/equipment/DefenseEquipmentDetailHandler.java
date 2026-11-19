package server.game;

import xzj.Xzj;

import java.util.Map;

/** 1004019 防御法器详情。 */
public class DefenseEquipmentDetailHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsArticleSetableDefenseEquipmentReq_1004019 req =
                Xzj.GoodsArticleSetableDefenseEquipmentReq_1004019.parseFrom(msg.getData());
        Xzj.GoodsArticleSetableDefenseEquipmentResp_1004019.Builder b =
                Xzj.GoodsArticleSetableDefenseEquipmentResp_1004019.newBuilder();
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
                b.setToughTenacity(rank * 12);
                b.setEffectType1(302);
                b.setEffectValue1(5 + rank);
                b.setCreatedBy("修真界");
                b.setVolume(1.0);
                b.setRechooseCount(0);
            }
        }
        session.send(1004019, msg.getMsid(), b.build());
    }
}
