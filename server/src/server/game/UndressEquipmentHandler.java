package server.game;

import xzj.Xzj;

import java.util.List;
import java.util.Map;

/** 1009002 卸下装备。 */
public class UndressEquipmentHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.UndressEquipmentReq_1009002 req = Xzj.UndressEquipmentReq_1009002.parseFrom(msg.getData());
        Xzj.UndressEquipmentResp_1009002.Builder b = Xzj.UndressEquipmentResp_1009002.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            List<Map<String, Object>> list = EquipmentService.dressed(session.role);
            Map<String, Object> hit = null;
            for (Map<String, Object> g : list) {
                if (GoodsService.str(g, "GoodsId").equals(req.getEquipmentId())) {
                    hit = g;
                    break;
                }
            }
            if (hit == null) {
                b.setIsSuccess(false).setCode(4603);
            } else {
                list.remove(hit);
                session.role.put("EquippedGoods", list);
                GoodsService.add(session.role, GoodsService.str(hit, "Name"), GoodsService.num(hit, "Type", 0),
                        GoodsService.num(hit, "Rank", 1), GoodsService.num(hit, "Level", 1),
                        GoodsService.num(hit, "FiveElement", 1), GoodsService.str(hit, "TemplateId"),
                        Math.max(1, GoodsService.num(hit, "Count", 1)), 1.0,
                        GoodsService.num(hit, "RecyclePrice", 20));
                long perc = Math.max(100, RoleUtil.lng(session.role, "Perception", 500));
                long phys = EquipmentService.physique(session.role);
                b.setIsSuccess(true).setCode(2000);
                b.setCurrentPhysiqueDefense(phys);
                b.setCurrentPureBloodDefense((int) Math.min(Integer.MAX_VALUE,
                        EquipmentService.pureBlood(session.role)));
                b.setCurrentMoveSpeed(5.0);
                b.setSuperAirRecoverValue(perc * 0.05);
                b.setDefenseRecoverValue(phys * 0.05);
                b.setSuperAirRecoverValueOnBattle(perc * 0.02);
                b.setDefenseRecoverValueOnBattle(phys * 0.02);
                b.setCanUsedPhysique(phys);
                RoleUtil.save(session.role);
            }
        }
        session.send(1009002, msg.getMsid(), b.build());
    }
}
