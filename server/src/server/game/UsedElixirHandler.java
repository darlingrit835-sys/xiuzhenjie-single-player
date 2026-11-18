package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1004032 服用丹药。任务奖励的纳气丹/回春散都走这里。
 */
public class UsedElixirHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.UsedElixirReq_1004032 req = Xzj.UsedElixirReq_1004032.parseFrom(msg.getData());
        Xzj.UsedElixirResp_1004032.Builder b = Xzj.UsedElixirResp_1004032.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getGoodsId());
            if (g == null || GoodsService.num(g, "Type", 0) != 19) {
                b.setIsSuccess(false).setCode(4605);
            } else {
                String name = GoodsService.str(g, "Name");
                int lv = RoleUtil.num(session.role, "Level", 1);
                GameData.LevelInfo li = GameData.level(lv);
                if (name.contains("纳气") || name.contains("聚气") || name.contains("补气")) {
                    long cur = RoleUtil.lng(session.role, "SuperAir", 0);
                    long gain = Math.max(1, Math.round(li.upgradeLimit * 0.35));
                    session.role.put("SuperAir", Math.min((long) (li.upgradeLimit * 3), cur + gain));
                } else if (name.contains("回春") || name.contains("疗伤")) {
                    int vigor = RoleUtil.num(session.role, "CurrentVigor", 100);
                    session.role.put("CurrentVigor", vigor + 30);
                    session.role.put("CurrentPureBlood", Math.max(1,
                            RoleUtil.num(session.role, "CurrentPureBlood", 100)) + 30);
                } else if (name.contains("解毒")) {
                    session.role.put("CurrentPoisonVal",
                            Math.max(0, RoleUtil.num(session.role, "CurrentPoisonVal", 0) - 20));
                } else {
                    long cur = RoleUtil.lng(session.role, "SuperAir", 0);
                    session.role.put("SuperAir", Math.min((long) (li.upgradeLimit * 3),
                            cur + Math.round(li.upgradeLimit * 0.15)));
                }
                GoodsService.remove(session.role, req.getGoodsId(), 1);
                b.setIsSuccess(true).setCode(2000);
                b.setNowCapacity(GoodsService.capacity(session.role));
                fill(b, session.role);
                RoleUtil.save(session.role);
            }
        }
        session.send(1004032, msg.getMsid(), b.build());
    }

    private void fill(Xzj.UsedElixirResp_1004032.Builder b, Map<String, Object> r) {
        b.setPureBlood(RoleUtil.num(r, "PureBlood"));
        b.setFlexibleRoot(RoleUtil.str(r, "FlexibleRoot"));
        b.setSuperAir(RoleUtil.lng(r, "SuperAir"));
        b.setPerception(RoleUtil.lng(r, "Perception"));
        b.setPhysique(RoleUtil.lng(r, "Physique"));
        b.setCurrentSuperAir(RoleUtil.lng(r, "CurrentSuperAir"));
        b.setCurrentPerception(RoleUtil.lng(r, "CurrentPerception"));
        b.setCurrentPhysique(RoleUtil.lng(r, "CurrentPhysique"));
        b.setCurrentVigor(RoleUtil.num(r, "CurrentVigor"));
        b.setCurrentClever(RoleUtil.num(r, "CurrentClever"));
        b.setCurrentPoisonVal(RoleUtil.num(r, "CurrentPoisonVal"));
        b.setCurrentPureBlood(RoleUtil.num(r, "CurrentPureBlood"));
        b.setHasGold(RoleUtil.bool(r, "HasGold"));
        b.setHasWood(RoleUtil.bool(r, "HasWood"));
        b.setHasWater(RoleUtil.bool(r, "HasWater"));
        b.setHasFire(RoleUtil.bool(r, "HasFire"));
        b.setHasEarth(RoleUtil.bool(r, "HasEarth"));
        b.setHunLiVal(RoleUtil.num(r, "HunLiVal"));
        b.setPower(RoleUtil.num(r, "Power"));
        b.setCurrentPower(RoleUtil.num(r, "CurrentPower"));
        b.setAge(RoleUtil.num(r, "CurrentAge"));
        b.setMaxAge(RoleUtil.num(r, "MaxAge"));
    }
}
