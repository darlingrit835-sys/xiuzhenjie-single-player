package server.game;

import xzj.Xzj;

/** 1009004 从技能栏移除技能。 */
public class RemoveSkillHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RemoveSkillReq_1009004 req = Xzj.RemoveSkillReq_1009004.parseFrom(msg.getData());
        Xzj.RemoveSkillResp_1009004.Builder b = Xzj.RemoveSkillResp_1009004.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            SkillService.remove(session.role, req.getSkillId());
            RoleUtil.save(session.role);
            b.setIsSuccess(true).setCode(2000);
            b.setCurrentPerception(RoleUtil.num(session.role, "CurrentPerception"));
            b.setFullLoadPerception((int) Math.min(Integer.MAX_VALUE,
                    RoleUtil.lng(session.role, "Perception", 500)));
        }
        session.send(1009004, msg.getMsid(), b.build());
    }
}
