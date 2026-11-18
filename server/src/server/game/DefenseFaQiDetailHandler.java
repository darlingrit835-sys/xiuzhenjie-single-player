package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1004014 法器详情（点开纳戒/装备页里的法器看信息时发）。
 * 原来没注册，走兜底只回 IsSuccess，灵衍值 / 占用体魄 / 坚韧 / 体积全是 0。
 * 口径与 EquipmentService 的防御加成算法保持一致。
 */
public class DefenseFaQiDetailHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsArticleDefenseEquipmentReq_1004014 req =
                Xzj.GoodsArticleDefenseEquipmentReq_1004014.parseFrom(msg.getData());
        Xzj.GoodsArticleDefenseEquipmentResp_1004014.Builder b =
                Xzj.GoodsArticleDefenseEquipmentResp_1004014.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getId());
            if (g == null || !GoodsService.isEquipmentType(GoodsService.num(g, "Type", 0))) {
                b.setIsSuccess(false).setCode(4605);
            } else {
                int rank = Math.max(1, GoodsService.num(g, "Rank", 1));
                int level = Math.max(1, GoodsService.num(g, "Level", 1));
                b.setIsSuccess(true).setCode(2000);
                b.setConversionRate(GoodsService.num(g, "ConversionRate", 1));
                b.setUsedPhysique(level * 10 + rank * 5);
                b.setToughTenacity(rank * 12 + level * 2);
                b.setVolume(numD(g, "Volume", 1.0));
                b.setCreatedBy("修真界");
                b.setRechooseCount(0);
            }
        }
        session.send(1004014, msg.getMsid(), b.build());
    }

    private static double numD(Map<String, Object> m, String k, double def) {
        Object o = m.get(k);
        if (o instanceof Number) return ((Number) o).doubleValue();
        return def;
    }
}
