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
                Map<String, Object> role = saved == null ? session.role : saved;
                int grade = HomeGrades.grade(role);
                if (saved != null) session.role.put("DongFuRank", grade);
                b.setCode(2000).setIsSuccess(true);
                b.setName(HomeGrades.homeName(role));
                b.setRank(HomeGrades.wireRank(grade));
                b.setLive1Type(1);
                b.setLive2Type(2);
            } catch (RuntimeException storageFailure) {
                b.setCode(5000).setIsSuccess(false);
            }
        }
        session.send(1005001, msg.getMsid(), b.build());
    }
}
