package server.game;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.ThreadLocalRandom;

/**
 * 战斗奖励：击杀修为、掉落物品、清场奖励。
 *
 * 掉落不会直接进背包，而是先写到角色 BattleLoot；客户端拾取（1004026）
 * 后才真正进纳戒，避免“没捡也算拿到”。
 */
public final class BattleRewardService {
    public static final class Drop {
        public String dropId;
        public String name;
        public String templateId;
        public int type;
        public int rank;
        public int level;
        public int fiveElement;
        public int count;
        public double x;
        public double y;
    }

    public static final class Result {
        public final List<Drop> drops = new ArrayList<>();
        public boolean questDone;
    }

    private static final String[] WEAPONS = {"青锋剑", "寒铁剑", "赤炎剑", "流光剑", "玄天剑", "太虚剑"};
    private static final String[] ARMORS = {"青布道袍", "玄铁护甲", "赤霞衣", "流云袍", "玄冰甲", "太虚道袍"};
    private static final String[] BOOTS = {"青布靴", "疾风靴", "踏云靴", "流云履", "玄冰靴", "太虚履"};
    private static final String[] MATERIALS = {"妖兽皮", "妖兽血", "妖骨", "妖丹", "灵草", "玄铁矿石"};
    private static final int[] MATERIAL_TYPES = {8, 9, 10, 11, 12, 13};
    private static final String[] PILLS = {"纳气丹", "回春散", "解毒丹", "经脉丹"};

    private BattleRewardService() {}

    public static Result onKill(String roleId, Dungeon.Monster m) {
        Result result = new Result();
        if (roleId == null || m == null) return result;
        Map<String, Object> r = roleForUpdate(roleId);
        if (r == null) return result;
        synchronized (r) {
            RoleUtil.ensureDefaults(r);
            grantCultivation(r, m);
            result.questDone = QuestService.applyKill(r, m.unusual == 4);
            result.drops.addAll(rollDrops(r, m));
            RoleUtil.save(r);
        }
        if (result.questDone) GameServer.notifyTaskComplete(roleId);
        return result;
    }

    public static Result onSweep(String roleId, int playerLevel) {
        Result result = new Result();
        if (roleId == null) return result;
        Map<String, Object> r = roleForUpdate(roleId);
        if (r == null) return result;
        synchronized (r) {
            RoleUtil.ensureDefaults(r);
            result.questDone = QuestService.applySweep(r);
            long stone = 30L + playerLevel * 12L;
            r.put("ShinyStone", RoleUtil.lng(r, "ShinyStone", 0) + stone);
            // 清场额外掉一颗妖丹，让“扫荡”有明确收益
            Drop d = newDrop(r, "妖丹", 11, Math.max(1, stageRank(playerLevel)),
                    playerLevel, 1, 0, 0);
            result.drops.add(d);
            RoleUtil.save(r);
        }
        if (result.questDone) GameServer.notifyTaskComplete(roleId);
        return result;
    }

    /** 拾取时把 BattleLoot 中的物品真正放进纳戒。 */
    public static String pickup(Map<String, Object> role, String dropId) {
        List<Map<String, Object>> loot = loot(role);
        Map<String, Object> hit = null;
        for (Map<String, Object> m : loot) {
            if (dropId.equals(GoodsService.str(m, "DropId"))) {
                hit = m;
                break;
            }
        }
        if (hit == null) return null;
        loot.remove(hit);
        role.put("BattleLoot", loot);
        Map<String, Object> g = GoodsService.add(role,
                GoodsService.str(hit, "Name"),
                GoodsService.num(hit, "Type", 0),
                GoodsService.num(hit, "Rank", 1),
                GoodsService.num(hit, "Level", 1),
                GoodsService.num(hit, "FiveElement", 1),
                GoodsService.str(hit, "TemplateId"),
                Math.max(1, GoodsService.num(hit, "Count", 1)),
                1.0, 10 + GoodsService.num(hit, "Rank", 1) * 5);
        return g == null ? null : GoodsService.str(g, "GoodsId");
    }

    public static void clearLoot(String roleId) {
        if (roleId == null) return;
        Map<String, Object> r = roleForUpdate(roleId);
        if (r == null) return;
        synchronized (r) {
            r.put("BattleLoot", new ArrayList<>());
            RoleUtil.save(r);
        }
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> loot(Map<String, Object> role) {
        Object o = role.get("BattleLoot");
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object x : (List<Object>) o) if (x instanceof Map) out.add((Map<String, Object>) x);
        }
        return out;
    }

    private static Map<String, Object> roleForUpdate(String roleId) {
        PlayerSession s = GameServer.sessionOfRole(roleId);
        if (s != null && !s.role.isEmpty()) return s.role;
        return RoleStore.INSTANCE.getRole(roleId);
    }

    private static void grantCultivation(Map<String, Object> r, Dungeon.Monster m) {
        int lv = RoleUtil.num(r, "Level", 1);
        GameData.LevelInfo li = GameData.level(lv);
        double factor = m.unusual == 4 ? 0.15 : (m.unusual == 3 ? 0.06 : 0.02);
        long gain = Math.max(1, Math.round(li.upgradeLimit * factor));
        long cur = RoleUtil.lng(r, "SuperAir", 0);
        r.put("SuperAir", Math.min((long) (li.upgradeLimit * 2), cur + gain));
        long perc = RoleUtil.lng(r, "Perception", 0) + Math.max(1, Math.round(gain * 0.02));
        long phys = RoleUtil.lng(r, "Physique", 0) + Math.max(1, Math.round(gain * 0.02));
        r.put("Perception", perc);
        r.put("CurrentPerception", perc);
        r.put("Physique", phys);
        r.put("CurrentPhysique", phys);
        RoleUtil.recomputePower(r);
    }

    private static List<Drop> rollDrops(Map<String, Object> r, Dungeon.Monster m) {
        List<Drop> out = new ArrayList<>();
        ThreadLocalRandom rnd = ThreadLocalRandom.current();
        int lv = Math.max(1, m.level);
        int rank = stageRank(lv);
        double roll = rnd.nextDouble();

        double matChance = m.unusual == 4 ? 1.0 : (m.unusual == 3 ? 0.85 : 0.45);
        double pillChance = m.unusual == 4 ? 0.55 : (m.unusual == 3 ? 0.25 : 0.10);
        double equipChance = m.unusual == 4 ? 0.55 : (m.unusual == 3 ? 0.16 : 0.035);

        if (roll < matChance) {
            int idx = rnd.nextInt(MATERIALS.length);
            out.add(newDrop(r, MATERIALS[idx], MATERIAL_TYPES[idx], rank, lv,
                    m.unusual == 4 ? 2 : 1, m.x, m.y));
        } else if (roll < matChance + pillChance) {
            int idx = rnd.nextInt(PILLS.length);
            out.add(newDrop(r, PILLS[idx], 19, Math.max(1, rank), lv, 1, m.x, m.y));
        } else if (roll < matChance + pillChance + equipChance) {
            int kind = rnd.nextInt(3);
            if (kind == 0) {
                out.add(newDrop(r, pick(WEAPONS, rank), 15, rank, lv, 1, m.x, m.y));
            } else if (kind == 1) {
                out.add(newDrop(r, pick(ARMORS, rank), 16, rank, lv, 1, m.x, m.y));
            } else {
                out.add(newDrop(r, pick(BOOTS, rank), 29, rank, lv, 1, m.x, m.y));
            }
        }
        // 领主额外再掉一件
        if (m.unusual == 4 && out.size() < 2) {
            int kind = rnd.nextBoolean() ? 16 : 29;
            out.add(newDrop(r, pick(kind == 16 ? ARMORS : BOOTS, rank), kind, rank, lv, 1, m.x, m.y));
        }
        return out;
    }

    private static Drop newDrop(Map<String, Object> r, String name, int type, int rank,
                                int level, int count, double x, double y) {
        Drop d = new Drop();
        d.dropId = "loot_" + UUID.randomUUID().toString().replace("-", "").substring(0, 14);
        d.name = name;
        d.templateId = "drop_" + type + "_" + rank + "_" + level;
        d.type = type;
        d.rank = Math.max(1, Math.min(16, rank));
        d.level = Math.max(1, Math.min(90, level));
        d.fiveElement = ThreadLocalRandom.current().nextInt(1, 6);
        d.count = Math.max(1, count);
        d.x = x;
        d.y = y;

        List<Map<String, Object>> loot = loot(r);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("DropId", d.dropId);
        m.put("Name", d.name);
        m.put("Type", d.type);
        m.put("Rank", d.rank);
        m.put("Level", d.level);
        m.put("FiveElement", d.fiveElement);
        m.put("TemplateId", d.templateId);
        m.put("Count", d.count);
        loot.add(m);
        r.put("BattleLoot", loot);
        return d;
    }

    private static int stageRank(int level) {
        int stage = Math.max(1, (level - 1) / 10 + 1);
        return Math.max(1, Math.min(16, stage * 4 - 3 + ThreadLocalRandom.current().nextInt(0, 3)));
    }

    private static String pick(String[] arr, int rank) {
        int idx = Math.min(arr.length - 1, Math.max(0, (rank - 1) / 3));
        return arr[idx];
    }
}
