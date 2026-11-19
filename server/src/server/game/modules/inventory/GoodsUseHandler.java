package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1004024 使用物品。
 * 目前支持：功法书（学习并装备）、丹药（增加修为），其它物品返回“暂不可用”。
 */
public class GoodsUseHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsUseReq_1004024 req = Xzj.GoodsUseReq_1004024.parseFrom(msg.getData());
        Xzj.GoodsUseResp_1004024.Builder b = Xzj.GoodsUseResp_1004024.newBuilder();
        boolean questDone = false;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getGoodsId());
            if (g == null) {
                b.setIsSuccess(false).setCode(4603).setNowCapacity(GoodsService.capacity(session.role));
            } else {
                int type = GoodsService.num(g, "Type", 0);
                boolean ok = false;
                if (type == 1) {
                    GameData.GongFaDef def = findGongFa(g);
                    if (def != null) {
                        RoleUtil.addOwned(session.role, def.id);
                        RoleUtil.equip(session.role, def.type, def.id);
                        GoodsService.remove(session.role, req.getGoodsId(), 1);
                        questDone = QuestService.onGongFa(session.role, def.rank);
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
                b.setIsSuccess(ok).setCode(ok ? 2000 : 4605)
                        .setNowCapacity(GoodsService.capacity(session.role));
                RoleUtil.save(session.role);
            }
        }
        session.send(1004024, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }

    private static GameData.GongFaDef findGongFa(Map<String, Object> g) {
        String template = GoodsService.str(g, "TemplateId");
        GameData.GongFaDef def = GameData.gongFa(template);
        if (def != null) return def;
        String name = GoodsService.str(g, "Name");
        for (GameData.GongFaDef d : GameData.gongFaAll()) {
            if (d.name.equals(name)) return d;
        }
        return null;
    }
}
