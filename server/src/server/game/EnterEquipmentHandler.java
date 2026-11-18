package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1009005 打开装备界面：返回已穿戴装备与总属性。
 */
public class EnterEquipmentHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterEquipmentResp_1009005.Builder b = Xzj.EnterEquipmentResp_1009005.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            GoodsService.ensureStarterKit(session.role);
            for (Map<String, Object> g : EquipmentService.dressed(session.role)) {
                Xzj.EnterEquipmentResp_1009005.EquipmentInfoResp_1009006.Builder e =
                        Xzj.EnterEquipmentResp_1009005.EquipmentInfoResp_1009006.newBuilder();
                e.setGoodsId(GoodsService.str(g, "GoodsId"));
                e.setName(GoodsService.str(g, "Name"));
                e.setFiveElement(GoodsService.num(g, "FiveElement", 1));
                e.setRank(GoodsService.num(g, "Rank", 1));
                e.setLevel(GoodsService.num(g, "Level", 1));
                e.setType(GoodsService.num(g, "Type", 0));
                e.setIcon(GoodsService.num(g, "Type", 1));
                b.addDressedEquipments(e);
            }
            long perc = Math.max(100, RoleUtil.lng(session.role, "Perception", 500));
            long phys = EquipmentService.physique(session.role);
            b.setIsSuccess(true).setCode(2000);
            b.setCurrentPhysiqueDefense(phys);
            b.setCurrentPureBloodDefense((int) Math.min(Integer.MAX_VALUE, EquipmentService.pureBlood(session.role)));
            b.setFullLoadPerception((int) Math.min(Integer.MAX_VALUE, perc));
            b.setCurrentMoveSpeed(5.0);
            b.setSuperAirRecoverValue(perc * 0.05);
            b.setDefenseRecoverValue(phys * 0.05);
            b.setSuperAirRecoverValueOnBattle(perc * 0.02);
            b.setDefenseRecoverValueOnBattle(phys * 0.02);
            b.setSuperAirPower(EquipmentService.attack(session.role));
            b.setCanUsePhysique((int) Math.min(Integer.MAX_VALUE, phys));
        }
        session.send(1009005, msg.getMsid(), b.build());
    }
}
