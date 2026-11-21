package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1004008 功法书详情。客户端点纳戒里的《启灵经》会先请求这个。
 */
public class GoodsBookAchievementDetailHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsBookAchievementDetailReq_1004008 req =
                Xzj.GoodsBookAchievementDetailReq_1004008.parseFrom(msg.getData());
        Xzj.GoodsBookAchievementDetailResp_1004008.Builder b =
                Xzj.GoodsBookAchievementDetailResp_1004008.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getId());
            GameData.GongFaDef def = GoodsService.book(g);
            if (def == null) {
                b.setIsSuccess(false).setCode(4605);
            } else {
                b.setIsSuccess(true).setCode(2000);
                b.setActionScope(def.type);
                b.setLevel(def.levelReq);
                b.setBasedConcentration(def.baseCircle);
                b.setConversionRate(def.baseCircle);
                b.setRunSpeed((int) Math.max(1, Math.round(def.baseCircle * 10)));
                b.setCreatedBy("修真界");
                b.setEffectType1(def.effectType1);
                b.setEffectValue1(def.effectValue1);
                b.setEffectType2(def.effectType2);
                b.setEffectValue2(def.effectValue2);
                b.setVolume(1.0);
            }
        }
        session.send(1004008, msg.getMsid(), b.build());
    }
}
