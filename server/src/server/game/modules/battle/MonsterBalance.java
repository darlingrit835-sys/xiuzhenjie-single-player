package server.game;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import server.common.Config;
import server.common.ServerConfig;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.Writer;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ThreadLocalRandom;

/**
 * 怪物数值与刷怪配置。
 *
 * 数值全部以“玩家当前神识”为基准，保证任何境界下：
 *  - 普通怪约 3 下打死
 *  - 精英怪约 5 下打死
 *  - 领主约 10 下打死
 *  - 一整波怪（含精英/领主）站着不动约 45~70 秒才会被打死
 *
 * 客户端战斗公式（反编译确认）：
 *  玩家有效伤害 = 技能 BaseDamage（御剑术 = 神识×0.8，青锋剑 = 神识×0.96）
 *  怪物有效生命 = 体魄护甲 + 精血×2
 * 因此这里把目标有效生命拆成 10% 护甲 + 45% 精血（精血按 2 倍折算）。
 *
 * 配置文件：data/config/monsters.json，改完重启服务端即可。
 */
public final class MonsterBalance {
    private static final Gson GSON = new GsonBuilder().setPrettyPrinting().create();
    private static final String CFG_PATH = Config.DATA_DIR + "/config/monsters.json";

    public static final class Tier {
        /** 精血 = 神识 × blood */
        public double blood = 1.30;
        /** 护甲 = 神识 × armor */
        public double armor = 0.29;
        /** 每击伤害 = 神识 × attack */
        public double attack = 0.0375;
        /** 真元 = 神识 × mana */
        public double mana = 1.0;
        /** 客户端显示的神识 = 神识 × perception */
        public double perception = 0.70;
        /** 掉落：基础灵石 + 每级灵石。 */
        public int goldBase = 15;
        public int goldPerLevel = 3;
    }

    public static final class Wave {
        public int normal = 4;
        public int elite = 1;
        public int boss = 0;
    }

    public static final class ConfigData {
        public double difficulty = 1.0;
        public double variance = 0.08;
        public Map<String, Tier> tiers = new LinkedHashMap<>();
        public Map<String, Wave> waves = new LinkedHashMap<>();
    }

    private static ConfigData cfg;

    private static final String[] ANIMAL_NAMES = {
            "赤炎狼", "玄水蟒", "青木猿", "厚土熊", "金翅雕", "幽冥狐", "雷纹豹", "白泽幼兽"
    };
    private static final String[] PEOPLE_NAMES = {
            "散修", "邪修", "魔修", "妖修", "剑修", "刀客", "阵师", "丹师"
    };
    private static final String[] NORMAL_PREFIX = {"幼年", "成年"};

    private MonsterBalance() {}

    public static synchronized ConfigData config() {
        if (cfg != null) return cfg;
        cfg = load();
        return cfg;
    }

    private static ConfigData load() {
        File f = new File(CFG_PATH);
        ConfigData c = null;
        if (f.exists()) {
            try (Reader r = new InputStreamReader(new FileInputStream(f), StandardCharsets.UTF_8)) {
                c = GSON.fromJson(r, ConfigData.class);
            } catch (Exception e) {
                System.err.println("[MonsterBalance] load " + CFG_PATH + " failed: " + e.getMessage());
            }
        }
        if (c == null) c = defaults();
        if (c.tiers == null || c.tiers.isEmpty()) c.tiers = defaults().tiers;
        if (c.waves == null || c.waves.isEmpty()) c.waves = defaults().waves;
        normalize(c);
        if (c.difficulty <= 0) c.difficulty = 1.0;
        if (c.variance < 0) c.variance = 0.08;
        saveIfMissing(f, c);
        return c;
    }

    private static void normalize(ConfigData c) {
        ConfigData d = defaults();
        for (String key : new String[]{"normal", "elite", "boss"}) {
            Tier t = c.tiers.get(key);
            Tier def = d.tiers.get(key);
            if (t == null) {
                c.tiers.put(key, def);
                continue;
            }
            if (t.blood <= 0) t.blood = def.blood;
            if (t.armor < 0) t.armor = def.armor;
            if (t.attack <= 0) t.attack = def.attack;
            if (t.mana <= 0) t.mana = def.mana;
            if (t.perception <= 0) t.perception = def.perception;
            if (t.goldBase <= 0) t.goldBase = def.goldBase;
            if (t.goldPerLevel <= 0) t.goldPerLevel = def.goldPerLevel;
        }
        for (String key : new String[]{"outer", "deep", "lair", "default"}) {
            Wave w = c.waves.get(key);
            if (w == null) {
                c.waves.put(key, d.waves.get(key));
            } else if (w.normal <= 0 && w.elite <= 0 && w.boss <= 0) {
                c.waves.put(key, d.waves.get(key));
            }
        }
    }

    private static ConfigData defaults() {
        ConfigData c = new ConfigData();

        Tier normal = new Tier();
        normal.blood = 0.90;
        normal.armor = 0.15;
        normal.attack = 0.022;
        normal.mana = 0.85;
        normal.perception = 0.60;
        normal.goldBase = 15;
        normal.goldPerLevel = 3;

        Tier elite = new Tier();
        elite.blood = 1.50;
        elite.armor = 0.28;
        elite.attack = 0.038;
        elite.mana = 1.30;
        elite.perception = 0.95;
        elite.goldBase = 55;
        elite.goldPerLevel = 8;

        Tier boss = new Tier();
        boss.blood = 2.40;
        boss.armor = 0.45;
        boss.attack = 0.060;
        boss.mana = 2.00;
        boss.perception = 1.40;
        boss.goldBase = 180;
        boss.goldPerLevel = 22;

        c.tiers.put("normal", normal);
        c.tiers.put("elite", elite);
        c.tiers.put("boss", boss);

        c.waves.put("outer", wave(5, 1, 0));    // 外围：5 普通 + 1 精英
        c.waves.put("deep", wave(4, 1, 1));     // 深处：4 普通 + 1 精英 + 1 领主
        c.waves.put("lair", wave(1, 2, 2));     // 兽王巢：1 普通 + 2 精英 + 2 领主
        c.waves.put("default", wave(4, 1, 0));  // 其它副本：4 普通 + 1 精英
        return c;
    }

    private static Wave wave(int normal, int elite, int boss) {
        Wave w = new Wave();
        w.normal = normal;
        w.elite = elite;
        w.boss = boss;
        return w;
    }

    private static void saveIfMissing(File f, ConfigData c) {
        if (f.exists()) return;
        try {
            File parent = f.getParentFile();
            if (parent != null) parent.mkdirs();
            try (Writer w = new OutputStreamWriter(new FileOutputStream(f), StandardCharsets.UTF_8)) {
                GSON.toJson(c, w);
            }
            System.out.println("[MonsterBalance] created default " + CFG_PATH);
        } catch (Exception e) {
            System.err.println("[MonsterBalance] save default failed: " + e.getMessage());
        }
    }

    /** 根据副本地图判断刷怪波次。 */
    public static String waveKey(String mapId) {
        MapTree.Node node = MapTree.find(mapId);
        String name = node == null ? "" : node.name;
        if (name.contains("兽王巢")) return "lair";
        if (name.contains("深处")) return "deep";
        if (name.contains("外围")) return "outer";
        return "default";
    }

    public static List<Dungeon.Monster> createMonsters(Map<String, Object> role, String mapId, int requestedCount) {
        ConfigData cfg = config();
        int lv = Math.max(1, RoleUtil.num(role, "Level", 1));
        GameData.LevelInfo li = GameData.level(lv);
        // 和 JoinDungeonHandler 完全一致的玩家基准，避免两边算法漂移
        long perc = Math.max(100L, RoleUtil.lng(role, "Perception", (long) li.perception));

        // 按当前境界的修为进度微调难度：刚突破时 85%，接近突破时 100%
        double progress = 0;
        if (li.upgradeLimit > 0) {
            progress = Math.max(0, Math.min(1, RoleUtil.lng(role, "SuperAir") / li.upgradeLimit));
        }
        double power = 0.85 + 0.15 * progress;

        Wave wave = cfg.waves.get(waveKey(mapId));
        if (wave == null) wave = cfg.waves.get("default");
        if (wave == null) wave = wave(4, 1, 0);

        List<Integer> plan = new ArrayList<>();
        for (int i = 0; i < wave.normal; i++) plan.add(1);
        for (int i = 0; i < wave.elite; i++) plan.add(3);
        for (int i = 0; i < wave.boss; i++) plan.add(4);

        int count = Math.max(1, requestedCount > 0 ? requestedCount : plan.size());
        List<Dungeon.Monster> out = new ArrayList<>();
        for (int i = 0; i < count; i++) {
            int unusual = plan.isEmpty() ? 1 : plan.get(i % plan.size());
            out.add(createMonster(role, lv, perc, unusual, i, count, cfg, power));
        }
        return out;
    }

    private static Dungeon.Monster createMonster(Map<String, Object> role, int lv, long perc,
                                                 int unusual, int index, int count, ConfigData cfg, double power) {
        boolean boss = unusual == 4;
        boolean elite = unusual == 3;
        Tier t = cfg.tiers.get(boss ? "boss" : (elite ? "elite" : "normal"));
        if (t == null) t = cfg.tiers.get("normal");

        ThreadLocalRandom rnd = ThreadLocalRandom.current();
        double variance = 1.0 + (rnd.nextDouble() * 2.0 - 1.0) * cfg.variance;
        double diff = cfg.difficulty * Math.max(0.1, ServerConfig.get().monsterDifficulty);

        Dungeon.Monster m = new Dungeon.Monster();
        m.index = 1001 + index;
        m.level = lv;
        m.unusual = unusual;
        boolean people = (index % 3 == 2) && !boss;
        m.objectType = people ? 2001 : 2002;
        m.fiveElement = (index % 5) + 1;
        String base = people
                ? PEOPLE_NAMES[index % PEOPLE_NAMES.length]
                : ANIMAL_NAMES[index % ANIMAL_NAMES.length];
        String prefix = boss ? "领主" : (elite ? "精英" : NORMAL_PREFIX[index % NORMAL_PREFIX.length]);
        m.name = prefix + "·" + base;
        m.icon = 1 + (index % 8);

        m.hp = Math.max(30L, Math.round(perc * t.blood * diff * variance * power));
        m.armor = Math.max(0L, Math.round(perc * t.armor * diff * variance * power));
        m.mana = Math.max(50L, Math.round(perc * t.mana * diff));
        m.perception = (int) Math.min(Integer.MAX_VALUE, Math.round(perc * t.perception * diff));
        m.attack = Math.max(1L, Math.round(perc * t.attack * diff * variance * power));
        m.speed = boss ? 3.3 : (elite ? 3.9 : 4.4);
        m.dropGold = Math.max(1L, Math.round(
                (t.goldBase + (long) lv * t.goldPerLevel) * ServerConfig.get().dropMultiplier
                        * (0.85 + rnd.nextDouble() * 0.3)));

        double x = -8.0 + 16.0 * ((index + 0.5) / Math.max(1, count));
        double y = 0.5 + 5.0 * ((index % 3) / 2.0);
        m.x = Math.round(x * 10) / 10.0;
        m.y = Math.round(y * 10) / 10.0;
        return m;
    }
}
