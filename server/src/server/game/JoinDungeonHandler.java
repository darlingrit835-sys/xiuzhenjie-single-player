package server.game;

import xzj.Xzj;

/**
 * 1010002 加入副本，下发角色在战斗中的属性与技能。
 *
 * 技能来自 SkillService：3 组 × 3 个，支持单体、持续、范围、护盾、增益、减速、定身和法器。
 */
public class JoinDungeonHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RoleJoinDungeonReq_1010002 req = Xzj.RoleJoinDungeonReq_1010002.parseFrom(msg.getData());
        Xzj.RoleJoinDungeonResp_1010002.Builder b = Xzj.RoleJoinDungeonResp_1010002.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Dungeon d = DungeonManager.get(req.getDungeonId());
            if (d == null) {
                d = DungeonManager.create(session.role, req.getMapId());
            }
            long perc = Math.max(100, RoleUtil.lng(session.role, "Perception"));
            long phys = EquipmentService.physique(session.role);
            long hp = EquipmentService.pureBlood(session.role);

            b.setDungeonId(d.id);
            b.setRoleId(RoleUtil.str(session.role, "Id"));
            b.setName(RoleUtil.str(session.role, "Name"));
            b.setCurrentPhysiqueDefense(phys);
            b.setCurrentPureBloodDefense((int) Math.min(Integer.MAX_VALUE, hp));
            b.setCurrentSuperAir(perc * 2);
            b.setCurrentPerception((int) Math.min(Integer.MAX_VALUE, perc));
            b.setCurrentMoveSpeed(5.0);
            b.setLevel(RoleUtil.num(session.role, "Level", 1));
            b.setPhysiqueFiveElement(1);
            b.setSuperAirFiveElement(1);
            b.setPerceptionFiveElement(1);
            b.setIcon(1);
            b.setSuperAirRecoverValue(perc * 0.05);
            b.setDefenseRecoverValue(phys * 0.05);
            b.setSuperAirRecoverValueOnBattle(perc * 0.02);
            b.setDefenseRecoverValueOnBattle(phys * 0.02);
            b.setGoldDamageAvoidance(1.0);
            b.setWoodDamageAvoidance(1.0);
            b.setWaterDamageAvoidance(1.0);
            b.setFireDamageAvoidance(1.0);
            b.setEarthDamageAvoidance(1.0);
            b.setAttackDistance(6.0);

            double attack = EquipmentService.attack(session.role);
            int level = RoleUtil.num(session.role, "Level", 1);
            int index = 1;
            for (SkillService.SkillDef def : SkillService.battleDefs(session.role)) {
                b.addSkills(battleSkill(def, attack, level, index++));
            }

            b.setCode(4502).setIsSuccess(true);
        }
        session.send(1010002, msg.getMsid(), b.build());
    }

    private Xzj.RoleJoinDungeonResp_1010002.BattleSkillResp_1010003.Builder battleSkill(
            SkillService.SkillDef d, double attack, int level, int index) {
        Xzj.RoleJoinDungeonResp_1010002.BattleSkillResp_1010003.Builder s =
                Xzj.RoleJoinDungeonResp_1010002.BattleSkillResp_1010003.newBuilder();

        double damage;
        switch (d.skillType) {
            case 20:                       // 护盾：吸收量
                damage = attack * d.damageMult;
                break;
            case 30:                       // 伤害增益
            case 31:                       // 防御增益
                damage = 1.0;
                break;
            default:
                damage = attack * d.damageMult;
                break;
        }

        s.setIcon(d.icon);
        s.setType(d.type);
        s.setSkillType(d.skillType);
        s.setName(d.name);
        s.setTemplateId(d.id);
        s.setCastTime(d.castTime);
        s.setContinueTime(d.continueTime);
        s.setCoolDown(d.coolDown);
        s.setFiveElement(d.fiveElement);
        s.setConversionRate(d.conversionRate);
        s.setBaseDamage(damage);
        s.setUsedPerception(0);
        s.setLevel(level);
        s.setSkillIndex(index);
        s.setUsedSuperAir(0);
        s.setCanUseCount(9999);
        s.setRank(1);
        s.setUnitUsed(0);
        s.setGoldDamageAdd(1.0);
        s.setWoodDamageAdd(1.0);
        s.setWaterDamageAdd(1.0);
        s.setFireDamageAdd(1.0);
        s.setEarthDamageAdd(1.0);
        return s;
    }
}
