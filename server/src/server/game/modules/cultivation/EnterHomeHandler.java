package server.game;

import xzj.Xzj;

import java.util.Map;

public class EnterHomeHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterHomeResp_1005001.Builder b = Xzj.EnterHomeResp_1005001.newBuilder();
        synchronized (session.role) {
            try {
                Map<String, Object> saved = session.roleId == null ? null : RoleStore.INSTANCE.getRole(session.roleId);
                if (saved == null) {
                    b.setCode(4007).setIsSuccess(false);
                } else {
                    int grade = HomeGrades.grade(saved);
                    session.role.put("DongFuRank", grade);
                    b.setCode(2000).setIsSuccess(true);
                    b.setName(HomeGrades.homeName(saved));
                    b.setRank(HomeGrades.wireRank(grade));
                    b.setLive1Type(RoleUtil.num(saved, "Live1Type"));
                    b.setLive2Type(RoleUtil.num(saved, "Live2Type"));
                }
            } catch (RuntimeException storageFailure) {
                b.setCode(5000).setIsSuccess(false);
            }
        }
        session.send(1005001, msg.getMsid(), b.build());
    }
}
