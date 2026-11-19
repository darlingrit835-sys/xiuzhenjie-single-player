package server.game;

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
            int mask = RoleUtil.activeMask(session.role);

            GameData.GongFaDef naqi = RoleUtil.equipped(session.role, RoleUtil.TYPE_NAQI);
            GameData.GongFaDef duanti = RoleUtil.equipped(session.role, RoleUtil.TYPE_DUANTI);
            GameData.GongFaDef lianshen = RoleUtil.equipped(session.role, RoleUtil.TYPE_LIANSHEN);

            b.setCode(4128).setIsSuccess(true);
            b.setLocationType(RoleUtil.locationType(mask));
            b.setSuperairCircle(RoleUtil.circle(session.role, RoleUtil.TYPE_NAQI));
            b.setPhysiqueCircle(RoleUtil.circle(session.role, RoleUtil.TYPE_DUANTI));
            b.setPerceptionCircle(RoleUtil.circle(session.role, RoleUtil.TYPE_LIANSHEN));
            b.setSuperairName(naqi == null ? "" : naqi.name);
            b.setPhysiqueName(duanti == null ? "" : duanti.name);
            b.setPerceptionName(lianshen == null ? "" : lianshen.name);
            b.setIsBrokenAcupoint(false);
            b.setBrokenRate(0);
            b.setIsBrokenBody(false);
            b.setGatherMatrixRank(RoleUtil.num(session.role, "JuLingRank"));
            b.setControllerMatrixRank(RoleUtil.num(session.role, "KongLingRank"));
            b.setGatherMatrixLessTime(0);
            b.setControllerMartixLessTime(0);
            b.setRefiningType(3);
            b.setRefiningValue(0);
            b.setIsBreak(RoleUtil.canBreak(session.role));
        }
        session.send(1005008, msg.getMsid(), b.build());
    }
}
