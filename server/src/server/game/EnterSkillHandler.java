package server.game;

import xzj.Xzj;

import java.util.HashSet;
import java.util.Map;
import java.util.Set;

/**
 * 1009007 打开技能/法器配置界面。
 * 返回已配置技能（Setable）和可配置技能（Canuse）。
 */
public class EnterSkillHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterSkillResp_1009007.Builder b = Xzj.EnterSkillResp_1009007.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            int lv = RoleUtil.num(session.role, "Level", 1);
            long perc = Math.max(100, RoleUtil.lng(session.role, "Perception", 500));

            Set<String> configured = new HashSet<>();
            for (Map<String, Object> m : SkillService.settings(session.role)) {
                String id = GoodsService.str(m, "SkillId");
                SkillService.SkillDef d = SkillService.get(id);
                if (d == null) continue;
                configured.add(id);
                b.addSetableSkills(skill(d, lv, m));
            }
            for (SkillService.SkillDef d : SkillService.all()) {
                if (!configured.contains(d.id)) b.addCanuseSkills(skill(d, lv, null));
            }
            b.setIsSuccess(true).setCode(2000);
        }
        session.send(1009007, msg.getMsid(), b.build());
    }

    private Xzj.EnterSkillResp_1009007.SkillInfoResp_1009008.Builder skill(
            SkillService.SkillDef d, int level, Map<String, Object> setting) {
        Xzj.EnterSkillResp_1009007.SkillInfoResp_1009008.Builder s =
                Xzj.EnterSkillResp_1009007.SkillInfoResp_1009008.newBuilder();
        s.setGoodsId(d.id);
        s.setName(d.name);
        s.setFiveElement(d.fiveElement);
        s.setRank(1);
        s.setLevel(level);
        s.setType(d.type);
        s.setSettingType(d.type);
        s.setCombatSettingId(setting == null ? 0 : GoodsService.num(setting, "CombatSettingId", 0));
        return s;
    }
}
