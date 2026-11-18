package server.game;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 一场战斗（副本）的数据。游戏服创建，战斗服读取，两者在同一个 JVM 内。
 */
public class Dungeon {
    public static class Monster {
        public int index;
        public double x, y;
        public int level;
        public int unusual;      // 1 幼年 2 成年 3 精英 4 BOSS
        public int objectType;   // 2001 人形怪, 2002 妖兽
        public int fiveElement;
        public String name;
        public int icon;
        public long hp;          // 精血（客户端会按 2 倍折算成有效生命）
        public long armor;       // 体魄护甲
        public long mana;
        public int perception;
        public long attack;
        public double speed;
        /** 击杀后掉落的灵石数量。 */
        public long dropGold;
    }

    public String id;
    public String mapId;
    public String roleId;
    public int playerLevel;
    public int roleIndex = 1;
    public long createdAt;
    public long expiresAt;
    public final List<Monster> monsters = new ArrayList<>();
    public final Set<Integer> dead = ConcurrentHashMap.newKeySet();
    /** 是否已经结算过“扫荡完成”任务。 */
    public boolean sweepCounted;

    public boolean isExpired() {
        return System.currentTimeMillis() > expiresAt;
    }

    public boolean allDead() {
        return dead.size() >= monsters.size();
    }
}
