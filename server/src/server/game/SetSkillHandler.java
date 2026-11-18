package server.game;

import xzj.Xzj;

/** 1009003 配置技能/法器到技能栏。 */
public class SetSkillHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.SetSkillReq_1009003 req = Xzj.SetSkillReq_1009003.parseFrom(msg.getData());
        Xzj.SetSkillResp_1009003.Builder b = Xzj.SetSkillResp_1009003.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            SkillService.set(session.role, req.getSkillId(), req.getSkillType(), req.getCombatSettingId());
            RoleUtil.save(session.role);
            b.setIsSuccess(true).setCode(2000);
            b.setCurrentPerception(RoleUtil.num(session.role, "CurrentPerception"));
            b.setFullLoadPerception((int) Math.min(Integer.MAX_VALUE,
                    RoleUtil.lng(session.role, "Perception", 500)));
        }
        session.send(1009003, msg.getMsid(), b.build());
    }
}
