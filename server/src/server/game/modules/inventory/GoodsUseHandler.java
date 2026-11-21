package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1004024 使用物品。
 * 功法书收录到参悟书目，丹药增加修为。
 */
public class GoodsUseHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsUseReq_1004024 req = Xzj.GoodsUseReq_1004024.parseFrom(msg.getData());
        Xzj.GoodsUseResp_1004024.Builder b = Xzj.GoodsUseResp_1004024.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getGoodsId());
            if (g == null) {
                b.setIsSuccess(false).setCode(4603).setNowCapacity(GoodsService.capacity(session.role));
            } else {
                int type = GoodsService.num(g, "Type", 0);
                boolean ok = false;
                if (type == 1) {
                    GameData.GongFaDef def = GoodsService.book(g);
                    if (def != null) {
                        boolean guideRead = GuideRewardService.STARTER_METHOD.equals(def.id) &&
                                RoleUtil.num(session.role, "PracticeGuideVal") == 1 && !RoleUtil.isRecorded(session.role, def);
                        int rejection = 0;
                        if (!RoleUtil.matchesRoot(session.role, def)) rejection = 4961;
                        else if (!guideRead && (RoleUtil.isRecorded(session.role, def) || RoleUtil.isLearned(session.role, def.id))) rejection = 4963;
                        if (rejection != 0) {
                            b.setIsSuccess(false).setCode(rejection).setNowCapacity(GoodsService.capacity(session.role));
                            session.send(1004024, msg.getMsid(), b.build());
                            return;
                        }
                        RoleUtil.recordBook(session.role, def.id);
                        GoodsService.remove(session.role, req.getGoodsId(), 1);
                        if (guideRead) session.role.put("PracticeStarterRead", true);
                        ok = true;
                    }
                } else if (type == 19) {
                    int lv = RoleUtil.num(session.role, "Level", 1);
                    GameData.LevelInfo li = GameData.level(lv);
                    long cur = RoleUtil.lng(session.role, "SuperAir", 0);
                    long gain = Math.max(1, Math.round(li.upgradeLimit * 0.35));
                    session.role.put("SuperAir", Math.min((long) (li.upgradeLimit * 3), cur + gain));
                    GoodsService.remove(session.role, req.getGoodsId(), 1);
                    ok = true;
                } else if (GoodsService.isEquipmentType(type)) {
                    // 装备请在装备界面穿戴，这里不消耗
                    b.setIsSuccess(false).setCode(4604).setNowCapacity(GoodsService.capacity(session.role));
                    session.send(1004024, msg.getMsid(), b.build());
                    return;
                }
                int successCode = type == 1 ? 4964 : 2000;
                b.setIsSuccess(ok).setCode(ok ? successCode : 4605)
                        .setNowCapacity(GoodsService.capacity(session.role));
                RoleUtil.save(session.role);
            }
        }
        session.send(1004024, msg.getMsid(), b.build());
    }

}
