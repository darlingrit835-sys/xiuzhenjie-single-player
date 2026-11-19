package server.game;

import server.common.ServerConfig;

import java.util.Map;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 副本管理。怪物数值统一由 MonsterBalance 生成，保证练气期也能打得动。
 */
public final class DungeonManager {
    private static final Map<String, Dungeon> DUNGEONS = new ConcurrentHashMap<>();

    private DungeonManager() {}

    public static Dungeon create(Map<String, Object> role, String mapId) {
        ServerConfig cfg = ServerConfig.get();
        int lv = RoleUtil.num(role, "Level", 1);

        Dungeon d = new Dungeon();
        d.id = "d_" + UUID.randomUUID().toString().replace("-", "");
        d.mapId = mapId == null || mapId.isEmpty() ? "1" : mapId;
        d.roleId = RoleUtil.str(role, "Id");
        d.playerLevel = lv;
        d.createdAt = System.currentTimeMillis();
        d.expiresAt = d.createdAt + cfg.battleDurationSeconds * 1000L;

        d.monsters.addAll(MonsterBalance.createMonsters(role, d.mapId,
                Math.max(1, cfg.monsterCount)));
        DUNGEONS.put(d.id, d);
        cleanup();
        return d;
    }

    public static Dungeon get(String id) {
        if (id == null) return null;
        Dungeon d = DUNGEONS.get(id);
        if (d != null && d.isExpired()) {
            DUNGEONS.remove(id);
            return null;
        }
        return d;
    }

    public static void remove(String id) {
        if (id != null) DUNGEONS.remove(id);
    }

    public static Dungeon findAnyForRole(String roleId) {
        for (Dungeon d : DUNGEONS.values()) {
            if (d.roleId != null && d.roleId.equals(roleId)) return d;
        }
        return null;
    }

    private static void cleanup() {
        long now = System.currentTimeMillis();
        DUNGEONS.entrySet().removeIf(e -> e.getValue().isExpired() && now - e.getValue().expiresAt > 300_000L);
    }
}
