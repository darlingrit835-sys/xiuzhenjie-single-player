package server.game;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 战斗技能与技能栏配置。
 *
 * 客户端 BattleWnd 支持 3 组 × 3 个技能；这里提供 8 个法术 + 1 个法器，
 * 通过 1009003 SetSkill / 1009004 RemoveSkill 保存配置。
 */
public final class SkillService {
    public static final class SkillDef {
        public String id;
        public String name;
        public int type;          // 1 法术 2 法器
        public int skillType;     // FaShuType 10/12/14/20/30/31/40/41，法器 15
        public double damageMult;
        public int castTime;      // ms
        public int coolDown;      // 秒
        public int continueTime;  // ms
        public double conversionRate = 1.0;
        public int fiveElement = 1;
        public int icon = 1;

        SkillDef(String id, String name, int type, int skillType, double damageMult,
                 int castTime, int coolDown, int continueTime, int fiveElement, int icon) {
            this.id = id;
            this.name = name;
            this.type = type;
            this.skillType = skillType;
            this.damageMult = damageMult;
            this.castTime = castTime;
            this.coolDown = coolDown;
            this.continueTime = continueTime;
            this.fiveElement = fiveElement;
            this.icon = icon;
        }
    }

    private static final List<SkillDef> ALL = new ArrayList<>();

    static {
        // 第一组：输出
        ALL.add(new SkillDef("skill_yujian", "御剑术", 1, 10, 1.00, 100, 1, 0, 1, 1));
        ALL.add(new SkillDef("skill_liehuo", "烈火咒", 1, 12, 1.80, 300, 7, 5000, 4, 2));
        ALL.add(new SkillDef("skill_fentian", "焚天诀", 1, 14, 1.20, 500, 10, 4000, 4, 3));
        // 第二组：生存/增益
        ALL.add(new SkillDef("skill_jingang", "金刚护体", 1, 20, 2.00, 300, 14, 8000, 5, 4));
        ALL.add(new SkillDef("skill_juling", "聚灵诀", 1, 30, 0.00, 200, 14, 8000, 2, 5));
        ALL.add(new SkillDef("skill_xuangui", "玄龟诀", 1, 31, 0.00, 200, 14, 8000, 3, 6));
        // 第三组：控制/法器
        ALL.add(new SkillDef("skill_hanbing", "寒冰咒", 1, 40, 0.30, 250, 9, 0, 3, 7));
        ALL.add(new SkillDef("skill_dingshen", "定身符", 1, 41, 0.20, 250, 12, 0, 5, 8));
        ALL.add(new SkillDef("skill_qingfeng", "青锋剑", 2, 15, 1.20, 100, 1, 0, 1, 9));
    }

    private SkillService() {}

    public static List<SkillDef> all() {
        return ALL;
    }

    public static SkillDef get(String id) {
        for (SkillDef d : ALL) if (d.id.equals(id)) return d;
        return null;
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> settings(Map<String, Object> role) {
        Object o = role.get("SkillSettings");
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object x : (List<Object>) o) {
                if (x instanceof Map) out.add((Map<String, Object>) x);
            }
        }
        return out;
    }

    private static void saveSettings(Map<String, Object> role, List<Map<String, Object>> list) {
        role.put("SkillSettings", list);
    }

    public static void set(Map<String, Object> role, String skillId, int skillType, int combatSettingId) {
        List<Map<String, Object>> list = settings(role);
        list.removeIf(m -> GoodsService.num(m, "CombatSettingId", -1) == combatSettingId
                || GoodsService.str(m, "SkillId").equals(skillId));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("SkillId", skillId);
        m.put("SkillType", skillType);
        m.put("CombatSettingId", combatSettingId);
        list.add(m);
        list.sort((a, b) -> Integer.compare(GoodsService.num(a, "CombatSettingId", 0),
                GoodsService.num(b, "CombatSettingId", 0)));
        saveSettings(role, list);
    }

    public static void remove(Map<String, Object> role, String skillId) {
        List<Map<String, Object>> list = settings(role);
        list.removeIf(m -> GoodsService.str(m, "SkillId").equals(skillId));
        saveSettings(role, list);
    }

    /** 战斗时下发的技能顺序：已配置优先，未配置时给一套默认三招。 */
    public static List<SkillDef> battleDefs(Map<String, Object> role) {
        List<SkillDef> out = new ArrayList<>();
        for (Map<String, Object> m : settings(role)) {
            SkillDef d = get(GoodsService.str(m, "SkillId"));
            if (d != null && !out.contains(d)) out.add(d);
        }
        if (out.isEmpty()) {
            out.add(get("skill_yujian"));
            out.add(get("skill_liehuo"));
            out.add(get("skill_qingfeng"));
        }
        // 补足到 9 个，让三组技能栏都有东西
        for (SkillDef d : ALL) {
            if (out.size() >= 9) break;
            if (!out.contains(d)) out.add(d);
        }
        while (out.size() > 9) out.remove(out.size() - 1);
        return out;
    }
}
