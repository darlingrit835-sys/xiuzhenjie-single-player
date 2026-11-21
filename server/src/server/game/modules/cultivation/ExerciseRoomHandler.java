package server.game;

import java.util.HashMap;
import java.util.Map;
import xzj.Xzj;

/**
 * 1005008 进入练功房：洞府主界面所有修炼数据都来自这里。
 */
public class ExerciseRoomHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterExerciseRoomResp_1005008.Builder b = Xzj.EnterExerciseRoomResp_1005008.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            int requested = Xzj.ChangeRunLocationReq_1001007.parseFrom(msg.getData()).getNewLocation();
            Map<String, Object> role = session.role;
            if (requested != 0) {
                if (!RoleUtil.canRunLocation(role, requested)) {
                    session.send(1005008, msg.getMsid(), b.setIsSuccess(false).setCode(4059).build());
                    return;
                }
                role = new HashMap<>(role);
                role.put("RunMask", RoleUtil.switchMask(requested));
            }
            int mask = RoleUtil.activeMask(role);

            GameData.GongFaDef naqi = RoleUtil.equipped(role, RoleUtil.TYPE_NAQI);
            GameData.GongFaDef duanti = RoleUtil.equipped(role, RoleUtil.TYPE_DUANTI);
            GameData.GongFaDef lianshen = RoleUtil.equipped(role, RoleUtil.TYPE_LIANSHEN);

            b.setCode(requested == 0 ? 4128 : 2000).setIsSuccess(true);
            b.setLocationType(RoleUtil.locationType(mask));
            b.setSuperairCircle(RoleUtil.circle(role, RoleUtil.TYPE_NAQI));
            b.setPhysiqueCircle(RoleUtil.circle(role, RoleUtil.TYPE_DUANTI));
            b.setPerceptionCircle(RoleUtil.circle(role, RoleUtil.TYPE_LIANSHEN));
            b.setSuperairName(naqi == null ? "" : naqi.name);
            b.setPhysiqueName(duanti == null ? "" : duanti.name);
            b.setPerceptionName(lianshen == null ? "" : lianshen.name);
            b.setIsBrokenAcupoint(RoleUtil.bool(role, "IsBrokenAcupoint"));
            b.setBrokenRate(0);
            b.setIsBrokenBody(false);
            b.setGatherMatrixRank(RoleUtil.num(role, "JuLingRank"));
            b.setControllerMatrixRank(RoleUtil.num(role, "KongLingRank"));
            b.setGatherMatrixLessTime(0);
            b.setControllerMartixLessTime(0);
            b.setRefiningType(3);
            b.setRefiningValue(0);
            b.setIsBreak(RoleUtil.canBreak(role));
        }
        session.send(1005008, msg.getMsid(), b.build());
    }
}
