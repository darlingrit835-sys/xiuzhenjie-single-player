package server.game;

import xzj.Xzj;

/**
 * 1004026 拾取掉落。战斗服下发的灵石 GoodsId 形如 lingshi_12，
 * 这里解析数量并写回角色，保证重登后灵石还在。
 */
public class PickUpGoodsHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.PickUpGoodsReq_1004026 req = Xzj.PickUpGoodsReq_1004026.parseFrom(msg.getData());
        Xzj.PickUpGoodsResp_1004026.Builder b = Xzj.PickUpGoodsResp_1004026.newBuilder();
        boolean questDone = false;
        String outGoodsId = req.getGoodsId() == null ? "" : req.getGoodsId();
        synchronized (session.role) {
            String id = req.getGoodsId();
            if (id != null && id.startsWith("loot_")) {
                String bagId = BattleRewardService.pickup(session.role, id);
                if (bagId != null) {
                    outGoodsId = bagId;
                    b.setIsSuccess(true).setCode(4526);
                } else {
                    b.setIsSuccess(false).setCode(4610);
                }
            } else if (id != null && id.startsWith("lingshi")) {
                int count = req.getCount() > 0 ? req.getCount() : 1;
                int us = id.indexOf('_');
                if (us >= 0) {
                    try { count = Integer.parseInt(id.substring(us + 1)); } catch (Exception ignored) {}
                }
                long stone = RoleUtil.lng(session.role, "ShinyStone") + count;
                session.role.put("ShinyStone", stone);
                questDone = QuestService.onStone(session.role);
                b.setIsSuccess(true).setCode(4526);
            } else {
                // 旧战斗掉落或未知物品，按成功处理，避免客户端卡住
                b.setIsSuccess(true).setCode(4526);
            }
            b.setNowCapacity(GoodsService.capacity(session.role));
            b.setGoodsId(outGoodsId);
            RoleUtil.save(session.role);
        }
        session.send(1004026, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }
}
