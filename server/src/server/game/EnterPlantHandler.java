package server.game;

import xzj.Xzj;

import java.util.List;
import java.util.Map;

/** 1013001 进入灵田：返回已开垦地块的状态。 */
public class EnterPlantHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterPlantResp_1013001.Builder b = Xzj.EnterPlantResp_1013001.newBuilder();
        b.setCode(2000).setIsSuccess(true).setIsOneKeyKaiKen(true);
        int count;
        int rank;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            PlantService.tick(session.role);
            RoleUtil.save(session.role);
            rank = HomeService.rank(session.role);
            List<Map<String, Object>> list = PlantService.lands(session.role);
            count = list.size();
            for (Map<String, Object> m : list) {
                Xzj.EnterPlantResp_1013001.LandResp_1013002.Builder lb =
                        Xzj.EnterPlantResp_1013001.LandResp_1013002.newBuilder();
                lb.setId(GoodsService.num(m, "Id", 1))
                        .setStatus(GoodsService.num(m, "Status", 0))
                        .setTimeRemaining(PlantService.remaining(m))
                        .setTotalWasteTime(((Number) m.getOrDefault("TotalMs", PlantService.KAIKEN_MS)).longValue())
                        .setPlantId(GoodsService.str(m, "PlantId"))
                        .setPlantName(GoodsService.str(m, "PlantName"));
                b.addLand(lb.build());
            }
        }
        session.send(1013001, msg.getMsid(), b.build());
        System.out.println("[Plant] enter rank=" + rank + " maxLands=" + (rank * 4) + " opened=" + count);
    }
}
