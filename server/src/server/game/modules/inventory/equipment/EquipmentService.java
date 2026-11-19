package server.game;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * 极简装备系统：只处理“穿戴/已穿戴/属性加成”，够新手引导和前期战斗使用。
 * 装备本身仍然以 GoodsService 的物品结构保存。
 */
public final class EquipmentService {
    private EquipmentService() {}

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> dressed(Map<String, Object> role) {
        Object o = role.get("EquippedGoods");
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object x : (List<Object>) o) {
                if (x instanceof Map) out.add((Map<String, Object>) x);
            }
        }
        return out;
    }

    private static void saveDressed(Map<String, Object> role, List<Map<String, Object>> list) {
        role.put("EquippedGoods", list);
    }

    /** 装备槽：15 武器，16/26 衣服，29 鞋，20 阵盘，其它按类型单独处理。 */
    public static int slot(int type) {
        if (type == 15) return 15;
        if (type == 16 || type == 26) return 16;
        if (type == 29) return 29;
        if (type == 20) return 20;
        return type;
    }

    public static boolean equip(Map<String, Object> role, Map<String, Object> item) {
        if (item == null) return false;
        int type = GoodsService.num(item, "Type", 0);
        int slot = slot(type);
        List<Map<String, Object>> list = dressed(role);

        // 卸下同槽旧装备并放回纳戒
        for (int i = list.size() - 1; i >= 0; i--) {
            Map<String, Object> old = list.get(i);
            if (slot(GoodsService.num(old, "Type", 0)) == slot) {
                list.remove(i);
                GoodsService.add(role, GoodsService.str(old, "Name"), GoodsService.num(old, "Type", 0),
                        GoodsService.num(old, "Rank", 1), GoodsService.num(old, "Level", 1),
                        GoodsService.num(old, "FiveElement", 1),
                        GoodsService.str(old, "TemplateId"), Math.max(1, GoodsService.num(old, "Count", 1)),
                        1.0, GoodsService.num(old, "RecyclePrice", 20));
            }
        }

        if (!GoodsService.remove(role, GoodsService.str(item, "GoodsId"), 1)) return false;
        item.put("Count", 1);
        item.put("IsUsing", true);
        item.put("IsBinding", true);
        list.add(item);
        saveDressed(role, list);
        return true;
    }

    /** 武器攻击加成（新手青锋剑 rank1 → +12），三件套额外 +10%。 */
    public static int attackBonus(Map<String, Object> role) {
        int bonus = 0;
        boolean weapon = false, armor = false, boots = false;
        for (Map<String, Object> g : dressed(role)) {
            int type = GoodsService.num(g, "Type", 0);
            if (type == 15) {
                weapon = true;
                bonus += GoodsService.num(g, "Rank", 1) * 10 + GoodsService.num(g, "Level", 1) * 2;
            } else if (type == 16 || type == 26) {
                armor = true;
            } else if (type == 29) {
                boots = true;
            }
        }
        if (weapon && armor && boots) bonus += Math.max(1, bonus / 10);
        return bonus;
    }

    /** 防具防御加成，衣服+鞋子两件套额外 +15%。 */
    public static int defenseBonus(Map<String, Object> role) {
        int bonus = 0;
        boolean armor = false, boots = false;
        for (Map<String, Object> g : dressed(role)) {
            int type = GoodsService.num(g, "Type", 0);
            if (type == 16 || type == 26 || type == 29) {
                bonus += GoodsService.num(g, "Rank", 1) * 8 + GoodsService.num(g, "Level", 1) * 2;
                if (type == 29) boots = true;
                else armor = true;
            }
        }
        if (armor && boots) bonus += Math.max(1, Math.round(bonus * 0.15f));
        return bonus;
    }

    public static Map<String, Object> dressedByType(Map<String, Object> role, int type) {
        for (Map<String, Object> g : dressed(role)) {
            if (GoodsService.num(g, "Type", 0) == type) return g;
        }
        return null;
    }

    public static long physique(Map<String, Object> role) {
        return Math.max(100, RoleUtil.lng(role, "Physique", 500) + defenseBonus(role));
    }

    public static long pureBlood(Map<String, Object> role) {
        long perc = Math.max(100, RoleUtil.lng(role, "Perception", 500));
        return Math.max(1000, perc * 4);
    }

    public static long attack(Map<String, Object> role) {
        long perc = Math.max(100, RoleUtil.lng(role, "Perception", 500));
        return Math.max(10, Math.round(perc * 0.8) + attackBonus(role));
    }
}
