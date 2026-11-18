package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1009001 穿戴装备。
 */
public class DressEquipmentHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.DressEquipmentReq_1009001 req = Xzj.DressEquipmentReq_1009001.parseFrom(msg.getData());
        Xzj.DressEquipmentResp_1009001.Builder b = Xzj.DressEquipmentResp_1009001.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getEquipmentId());
            if (g == null || !GoodsService.isEquipmentType(GoodsService.num(g, "Type", 0))) {
                b.setIsSuccess(false).setCode(4603);
            } else if (EquipmentService.equip(session.role, g)) {
                long perc = Math.max(100, RoleUtil.lng(session.role, "Perception", 500));
                long phys = EquipmentService.physique(session.role);
                b.setIsSuccess(true).setCode(2000);
                b.setCurrentPhysiqueDefense(phys);
                b.setCurrentPureBloodDefense((int) Math.min(Integer.MAX_VALUE, EquipmentService.pureBlood(session.role)));
                b.setCurrentMoveSpeed(5.0);
                b.setSuperAirRecoverValue(perc * 0.05);
                b.setDefenseRecoverValue(phys * 0.05);
                b.setSuperAirRecoverValueOnBattle(perc * 0.02);
                b.setDefenseRecoverValueOnBattle(phys * 0.02);
                b.setCanUsedPhysique(phys);
                RoleUtil.save(session.role);
            } else {
                b.setIsSuccess(false).setCode(4606);
            }
        }
        session.send(1009001, msg.getMsid(), b.build());
    }
}
