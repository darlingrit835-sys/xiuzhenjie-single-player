package server.game;

import xzj.Xzj;

import java.util.LinkedHashMap;
import java.util.Map;

/** 1013003 开垦土地。 */
public class KaiKenLandHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.KaiKenLandReq_1013003 req = Xzj.KaiKenLandReq_1013003.parseFrom(msg.getData());
        int index = req.getLandIndex();
        Xzj.KaiKenLandResp_1013003.Builder b = Xzj.KaiKenLandResp_1013003.newBuilder();
        String log;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            PlantService.tick(session.role);
            long vigor = RoleUtil.lng(session.role, "CurrentVigor", 0);
            Map<String, Object> land = PlantService.land(session.role, index);
            b.setCurrentVigor((int) Math.min(Integer.MAX_VALUE, vigor));
            // 地块数 = 洞府品阶 × 4（客户端 m_MaxLingTianShu 也是这样算的）
            int maxLands = HomeService.rank(session.role) * 4;
            if (index < 1 || index > maxLands) {
                b.setIsSuccess(false).setCode(4391);
                log = "[Plant] kaiken failed: land index " + index + " out of 1.." + maxLands;
            } else if (land != null && GoodsService.num(land, "Status", 0) > 0) {
                b.setIsSuccess(false).setCode(4376);
                b.setLandStatus(GoodsService.num(land, "Status", 0));
                log = "[Plant] kaiken failed: land " + index + " already opened";
            } else if (vigor < PlantService.KAIKEN_VIGOR) {
                b.setIsSuccess(false).setCode(4391);
                log = "[Plant] kaiken failed: not enough vigor=" + vigor;
            } else {
                vigor -= PlantService.KAIKEN_VIGOR;
                session.role.put("CurrentVigor", vigor);
                if (land == null) {
                    land = new LinkedHashMap<>();
                    land.put("Id", index);
                    land.put("PlantId", "");
                    land.put("PlantName", "");
                    PlantService.lands(session.role).add(land);
                }
                land.put("Status", 1);
                land.put("StartAt", System.currentTimeMillis());
                land.put("TotalMs", PlantService.KAIKEN_MS);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(2000);
                b.setLandStatus(1);
                b.setTimeRemaining(PlantService.KAIKEN_MS);
                b.setTotalWasteTime(PlantService.KAIKEN_MS);
                b.setCurrentVigor((int) Math.min(Integer.MAX_VALUE, vigor));
                log = "[Plant] kaiken land " + index + " started, vigor=" + vigor;
            }
        }
        session.send(1013003, msg.getMsid(), b.build());
        System.out.println(log);
    }
}
