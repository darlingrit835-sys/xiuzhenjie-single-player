package server.game;

import server.common.ServerConfig;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 角色数值读写与修炼公式。所有角色数据都是 roles.json 里的一个 Map。
 */
public final class RoleUtil {
    public static final int TYPE_LIANSHEN = 1;
    public static final int TYPE_DUANTI = 2;
    public static final int TYPE_NAQI = 3;

    private RoleUtil() {}

    // ------------------------------------------------------------ basic get/set

    public static int num(Map<String, Object> r, String k) {
        return num(r, k, 0);
    }

    public static int num(Map<String, Object> r, String k, int def) {
        Object o = r.get(k);
        if (o == null) return def;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o)); } catch (Exception e) { return def; }
    }

    public static long lng(Map<String, Object> r, String k) {
        return lng(r, k, 0);
    }

    public static long lng(Map<String, Object> r, String k, long def) {
        Object o = r.get(k);
        if (o == null) return def;
        if (o instanceof Number) return ((Number) o).longValue();
        try { return (long) Double.parseDouble(String.valueOf(o)); } catch (Exception e) { return def; }
    }

    public static double dbl(Map<String, Object> r, String k, double def) {
        Object o = r.get(k);
        if (o == null) return def;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o)); } catch (Exception e) { return def; }
    }

    public static String str(Map<String, Object> r, String k) {
        Object o = r.get(k);
        return o == null ? "" : String.valueOf(o);
    }

    public static boolean bool(Map<String, Object> r, String k) {
        Object o = r.get(k);
        return o != null && Boolean.parseBoolean(String.valueOf(o));
    }

    public static void set(Map<String, Object> r, String k, Object v) {
        r.put(k, v);
    }

    public static void save(Map<String, Object> r) {
        RoleStore.INSTANCE.save(r);
    }

    // ------------------------------------------------------------ 功法数据

    public static String equipField(int type) {
        switch (type) {
            case TYPE_LIANSHEN: return "GongFaLianShen";
            case TYPE_DUANTI: return "GongFaDuanTi";
            default: return "GongFaNaQi";
        }
    }

    private static List<String> methodIds(Map<String, Object> r, String field) {
        Object o = r.get(field);
        List<String> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object x : (List<?>) o) if (x instanceof String) out.add((String) x);
        }
        return out;
    }

    private static void addMethodId(Map<String, Object> r, String field, String id) {
        List<String> list = methodIds(r, field);
        if (!list.contains(id)) {
            list.add(id);
            r.put(field, list);
        }
    }

    static void addOwned(Map<String, Object> r, String id) {
        addMethodId(r, "GongFaOwned", id);
    }

    public static boolean isLearned(Map<String, Object> r, String id) {
        return containsMethod(r, "GongFaOwned", id);
    }

    private static boolean containsMethod(Map<String, Object> r, String field, String id) {
        Object ids = r.get(field);
        return ids instanceof List && ((List<?>) ids).contains(id);
    }

    static boolean isRecorded(Map<String, Object> r, GameData.GongFaDef g) {
        return g.isSystem() || containsMethod(r, "GongFaRead", g.id) ||
                (GuideRewardService.STARTER_METHOD.equals(g.id) &&
                        (bool(r, "PracticeStarterRead") || GuideRewardService.needsStarterPractice(r)));
    }

    static void recordBook(Map<String, Object> r, String id) {
        addMethodId(r, "GongFaRead", id);
    }

    static int methodElement(Map<String, Object> r, GameData.GongFaDef g) {
        if (!GuideRewardService.STARTER_METHOD.equals(g.id)) return g.fiveElement;
        for (int element = 1; element < ROOT_FIELDS.length; element++) {
            if (bool(r, ROOT_FIELDS[element])) return element;
        }
        return 0;
    }

    static boolean matchesRoot(Map<String, Object> r, GameData.GongFaDef g) {
        int element = methodElement(r, g);
        return GuideRewardService.STARTER_METHOD.equals(g.id) ||
                (element > 0 && element < ROOT_FIELDS.length && bool(r, ROOT_FIELDS[element]));
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> chongMap(Map<String, Object> r) {
        Object o = r.get("GongFaChong");
        if (o instanceof Map) return (Map<String, Object>) o;
        Map<String, Object> m = new HashMap<>();
        r.put("GongFaChong", m);
        return m;
    }

    public static int chong(Map<String, Object> r, String id) {
        Object o = chongMap(r).get(id);
        if (o instanceof Number) return ((Number) o).intValue();
        return 1;
    }

    public static void setChong(Map<String, Object> r, String id, int chong) {
        chongMap(r).put(id, Math.max(1, Math.min(3, chong)));
    }

    public static GameData.GongFaDef equipped(Map<String, Object> r, int type) {
        GameData.GongFaDef g = GameData.gongFa(str(r, equipField(type)));
        return g != null && g.type == type && isLearned(r, g.id) ? g : null;
    }

    public static void equip(Map<String, Object> r, int type, String id) {
        r.put(equipField(type), id);
    }

    // ------------------------------------------------------------ 修炼速度

    public static int rootCount(Map<String, Object> r) {
        int n = 0;
        if (bool(r, "HasGold")) n++;
        if (bool(r, "HasWood")) n++;
        if (bool(r, "HasWater")) n++;
        if (bool(r, "HasFire")) n++;
        if (bool(r, "HasEarth")) n++;
        return n;
    }

    public static double rootFactor(Map<String, Object> r) {
        switch (rootCount(r)) {
            case 0: return 1.0;   // 老数据没记录灵根时按单灵根算
            case 1: return 1.0;
            case 2: return 0.75;
            case 3: return 0.60;
            case 4: return 0.50;
            default: return 0.45;
        }
    }

    /** 洞府/聚灵阵/控灵阵的加成。 */
    public static double matrixFactor(Map<String, Object> r) {
        double f = 1.0;
        f *= 1.0 + 0.10 * Math.max(0, Math.min(4, num(r, "DongFuRank")));
        f *= 1.0 + 0.10 * Math.max(0, Math.min(4, num(r, "JuLingRank")));
        if (rootCount(r) == 1) {
            f *= 1.0 + 0.10 * Math.max(0, Math.min(4, num(r, "KongLingRank")));
        }
        return f;
    }

    /**
     * 内部运行组合用位掩码：1=炼神，2=锻体，4=纳气。
     * 客户端 RuningGongFaZuHeType 的编号不是位掩码，必须转换。
     */
    public static int activeMask(Map<String, Object> r) {
        int mask = num(r, "RunMask", 0);
        if (mask < 1 || mask > 7) {
            // 兼容老数据里的 RunLocation
            int old = num(r, "RunLocation", 3);
            mask = switchMask(old);
            r.put("RunMask", mask);
        }
        return mask;
    }

    static boolean canRunLocation(Map<String, Object> r, int location) {
        return location >= 1 && location <= 6 && (location <= 3 || num(r, "Level", 1) >= 31);
    }

    static int switchMask(int locationType) {
        switch (locationType) {
            case 1: return 1; // 炼神
            case 2: return 2; // 锻体
            case 3: return 4; // 纳气
            case 4: return 6; // 锻体+纳气
            case 5: return 3; // 锻体+炼神
            case 6: return 5; // 炼神+纳气
            default: return 4;
        }
    }

    /** 位掩码 -> 客户端 LocationType。 */
    public static int locationType(int mask) {
        switch (mask & 7) {
            case 1: return 1; // 炼神
            case 2: return 2; // 锻体
            case 4: return 3; // 纳气
            case 6: return 4; // 锻体+纳气
            case 3: return 5; // 锻体+炼神
            case 5: return 6; // 炼神+纳气
            default: return 3;
        }
    }

    public static boolean isActive(Map<String, Object> r, int type) {
        int bit = type == TYPE_LIANSHEN ? 1 : (type == TYPE_DUANTI ? 2 : 4);
        return (activeMask(r) & bit) != 0;
    }

    /** 同时运行两门功法时，各自效率 70%。 */
    public static double locationEfficiency(Map<String, Object> r) {
        int n = 0;
        for (int t = 1; t <= 3; t++) if (isActive(r, t)) n++;
        return n >= 2 ? 0.70 : 1.0;
    }

    private static final String[] ROOT_FIELDS = {"", "HasGold", "HasWood", "HasWater", "HasFire", "HasEarth"};
    private static final int[] GENERATES = {0, 3, 4, 2, 5, 1};

    private static double affinity(Map<String, Object> r, GameData.GongFaDef method) {
        int element = methodElement(r, method);
        if (element < 1 || element > 5) return 1.0;
        double factor = bool(r, ROOT_FIELDS[element]) ? 1.20 : 1.0;
        if (!isActive(r, method.type)) return factor;
        for (int kind = 1; kind <= 3; kind++) {
            if (kind == method.type || !isActive(r, kind)) continue;
            GameData.GongFaDef other = equipped(r, kind);
            int otherElement = other == null ? 0 : methodElement(r, other);
            if (otherElement > 0 && otherElement < GENERATES.length && GENERATES[otherElement] == element)
                return factor * 1.20;
        }
        return factor;
    }

    /**
     * 指定功法类型每秒增加的修为/神识/体魄量。
     *
     * 基准：练气1层 黄阶下品一重、单灵根、无阵法时，250 秒升一级（与设计表一致）。
     * 等级越高，基础耗时缓慢增加；功法品阶/重数、洞府、聚灵、控灵都会缩短耗时。
     */
    public static double rate(Map<String, Object> r, int type) {
        return rateFor(r, equipped(r, type), type);
    }

    private static double rateFor(Map<String, Object> r, GameData.GongFaDef g, int type) {
        int lv = Math.max(1, Math.min(GameData.maxLevel(), num(r, "Level", 1)));
        GameData.LevelInfo li = GameData.level(lv);

        double baseTarget = 250.0 + 10.0 * (lv - 1); // 秒
        double gfFactor;
        if (g == null) {
            gfFactor = 0.5; // 没运行功法，只剩基础纳气
        } else {
            gfFactor = (g.baseCircle / 0.8) * GameData.chongMultiplier(chong(r, g.id)) * affinity(r, g);
        }
        double bonus = matrixFactor(r) * rootFactor(r) * locationEfficiency(r)
                * ServerConfig.get().cultivationSpeed;
        double seconds = baseTarget / Math.max(0.05, gfFactor * bonus);
        double rate = li.upgradeLimit / Math.max(1.0, seconds);
        return type == TYPE_NAQI ? rate * Meridians.breathFactor(r) : rate;
    }

    /** 客户端洞府里显示的“X/周天”。一个周天按 2 秒计。 */
    public static double circle(Map<String, Object> r, int type) {
        return rate(r, type) * 2.0;
    }

    /** 某本功法在更换面板里显示的单本周天效率（按当前装备功法换算）。 */
    public static double circleFor(Map<String, Object> r, GameData.GongFaDef def) {
        return def == null ? 0 : 2.0 * rateFor(r, def, def.type);
    }

    // ------------------------------------------------------------ 初始化 / 升级

    public static void initNewRole(Map<String, Object> r) {
        int lv = 1;
        GameData.LevelInfo li = GameData.level(lv);
        r.put("Level", lv);
        r.put("SuperAir", (long) li.upgradeLimit);
        r.put("Perception", (long) li.perception);
        r.put("Physique", (long) li.toughTenacity);
        r.put("CurrentSuperAir", (long) (li.perception * 2));
        r.put("CurrentPerception", (long) li.perception);
        r.put("CurrentPhysique", (long) li.toughTenacity);
        r.put("PureBlood", Math.max(100, (int) Math.round(li.perception * 0.01)));
        r.put("CurrentPureBlood", Math.max(100, (int) Math.round(li.perception * 0.01)));
        r.put("MaxAge", GameData.lifespan(lv));
        r.put("CurrentAge", 16);
        r.put("HunLiVal", 1000);
        r.put("CurrentVigor", 100);
        r.put("CurrentClever", 100);
        r.put("Vigor", 100);
        r.put("Clever", 100);
        r.put("CurrentPoisonVal", 0);
        r.put("RunMask", 4);          // 默认纳气
        r.put("RunLocation", 3);
        r.put("DongFuRank", 0);
        r.put("JuLingRank", 0);
        r.put("KongLingRank", 0);
        r.put("BattleGuideVal", 1);
        r.put("PracticeGuideVal", 1);
        r.put("AcupointGuideVal", 1);
        r.put("ShopGuideVal", 1);
        r.put("SignGuideVal", 1);
        r.put("ElixirGuideVal", 1);
        r.put("TaskId", 1);
        r.put("TaskStatus", 0);
        r.put("StatsInitialized", true);
        r.put("StatsVersion", 2);
        r.put("LastOnline", System.currentTimeMillis());

        r.put("GongFaOwned", new ArrayList<String>());
        r.put("GongFaRead", new ArrayList<String>());
        r.put("GongFaChong", new HashMap<String, Object>());
        QuestService.ensureRole(r);
        GoodsService.ensureStarterKit(r);
        recomputePower(r);
    }

    public static void ensureDefaults(Map<String, Object> r) {
        // 旧存档可能只保存建角时的五行点数。
        for (int element = 1; element < ROOT_FIELDS.length; element++) {
            String flag = ROOT_FIELDS[element];
            if (!r.containsKey(flag)) r.put(flag, dbl(r, flag.substring(3), 0) > 0);
        }
        int lv = num(r, "Level", 1);
        if (lv < 1) lv = 1;
        if (lv > GameData.maxLevel()) lv = GameData.maxLevel();
        GameData.LevelInfo li = GameData.level(lv);
        boolean first = !bool(r, "StatsInitialized") || num(r, "StatsVersion", 0) < 2;

        r.put("Level", lv);
        if (!r.containsKey("SuperAir") || lng(r, "SuperAir") < 0) {
            r.put("SuperAir", (long) li.upgradeLimit);
        }
        if (first || !r.containsKey("Perception") || lng(r, "Perception") <= 0) {
            r.put("Perception", (long) Math.max(lng(r, "Perception"), li.perception));
        }
        if (first || !r.containsKey("Physique") || lng(r, "Physique") <= 0) {
            r.put("Physique", (long) Math.max(lng(r, "Physique"), li.toughTenacity));
        }
        if (first || !r.containsKey("PureBlood") || num(r, "PureBlood") <= 0) {
            r.put("PureBlood", Math.max(num(r, "PureBlood"),
                    Math.max(100, (int) Math.round(li.perception * 0.01))));
        }
        if (first || !r.containsKey("MaxAge") || num(r, "MaxAge") <= 0) {
            r.put("MaxAge", Math.max(num(r, "MaxAge"), GameData.lifespan(lv)));
        }
        if (num(r, "CurrentAge") <= 0) r.put("CurrentAge", 16);
        if (first || lng(r, "CurrentSuperAir") <= 0) r.put("CurrentSuperAir", lng(r, "Perception") * 2);
        if (first || lng(r, "CurrentPerception") <= 0) r.put("CurrentPerception", lng(r, "Perception"));
        if (first || lng(r, "CurrentPhysique") <= 0) r.put("CurrentPhysique", lng(r, "Physique"));
        if (first || num(r, "CurrentPureBlood") <= 0) r.put("CurrentPureBlood", num(r, "PureBlood"));
        if (first || num(r, "CurrentVigor") <= 0) r.put("CurrentVigor", 100 + lv * 5);
        if (first || num(r, "CurrentClever") <= 0) r.put("CurrentClever", 100 + lv * 5);
        r.put("Vigor", Math.max(100, Math.max(num(r, "Vigor"), num(r, "CurrentVigor"))));
        r.put("Clever", Math.max(100, Math.max(num(r, "Clever"), num(r, "CurrentClever"))));
        if (!r.containsKey("HunLiVal") || num(r, "HunLiVal") < 500) r.put("HunLiVal", 1000);
        if (!r.containsKey("RunMask") || num(r, "RunMask") < 1 || num(r, "RunMask") > 7) {
            r.put("RunMask", switchMask(num(r, "RunLocation", 3)));
        }
        r.put("RunLocation", locationType(num(r, "RunMask", 4)));
        if (!r.containsKey("DongFuRank")) r.put("DongFuRank", 0);
        if (!r.containsKey("JuLingRank")) r.put("JuLingRank", 0);
        if (!r.containsKey("KongLingRank")) r.put("KongLingRank", 0);

        // 新手引导：0/缺失视为未开始，从第 1 步起；100 = 已跳过/完成
        if (num(r, "PracticeGuideVal") <= 0) r.put("PracticeGuideVal", 1);
        if (num(r, "BattleGuideVal") <= 0) r.put("BattleGuideVal", 1);
        if (num(r, "AcupointGuideVal") <= 0) r.put("AcupointGuideVal", 1);
        if (num(r, "ShopGuideVal") <= 0) r.put("ShopGuideVal", 1);
        if (num(r, "SignGuideVal") <= 0) r.put("SignGuideVal", 1);
        if (num(r, "ElixirGuideVal") <= 0) r.put("ElixirGuideVal", 1);
        GoodsService.ensureStarterKit(r);

        if (lng(r, "LastOnline") <= 0) r.put("LastOnline", System.currentTimeMillis());

        // 从旧档保留的装备槽恢复缺失的领悟记录，再同步主线计数。
        if (!(r.get("GongFaChong") instanceof Map)) r.put("GongFaChong", new HashMap<String, Object>());
        if (GuideRewardService.needsStarterPractice(r) &&
                !containsMethod(r, "GongFaRead", GuideRewardService.STARTER_METHOD)) {
            recordBook(r, GuideRewardService.STARTER_METHOD);
        }
        for (int t = 1; t <= 3; t++) {
            GameData.GongFaDef g = GameData.gongFa(str(r, equipField(t)));
            if (g != null && g.type == t && !isLearned(r, g.id)) {
                addOwned(r, g.id);
            }
        }

        QuestService.ensureRole(r);
        r.put("StatsInitialized", true);
        r.put("StatsVersion", 2);
        recomputePower(r);
    }

    public static void recomputePower(Map<String, Object> r) {
        int lv = num(r, "Level", 1);
        long p = lv * 1000L + lng(r, "Perception") * 10 + lng(r, "Physique") * 10
                + lng(r, "SuperAir") / 10;
        if (p > Integer.MAX_VALUE) p = Integer.MAX_VALUE;
        r.put("Power", (int) p);
        r.put("CurrentPower", (int) p);
    }

    /** 突破成功，升一级并重算基础属性。 */
    public static void levelUp(Map<String, Object> r) {
        int lv = num(r, "Level", 1);
        if (lv >= GameData.maxLevel()) return;
        lv++;
        GameData.LevelInfo li = GameData.level(lv);
        r.put("Level", lv);
        r.put("SuperAir", (long) (li.upgradeLimit * 0.10));
        r.put("Perception", (long) Math.max(lng(r, "Perception"), li.perception));
        r.put("Physique", (long) Math.max(lng(r, "Physique"), li.toughTenacity));
        r.put("CurrentPerception", lng(r, "Perception"));
        r.put("CurrentPhysique", lng(r, "Physique"));
        r.put("CurrentSuperAir", lng(r, "Perception") * 2);
        int pure = Math.max(num(r, "PureBlood"), Math.max(100, (int) Math.round(li.perception * 0.01)));
        r.put("PureBlood", pure);
        r.put("CurrentPureBlood", pure);
        r.put("MaxAge", GameData.lifespan(lv));
        r.put("CurrentVigor", 100 + lv * 5);
        r.put("CurrentClever", 100 + lv * 5);
        r.put("Vigor", 100 + lv * 5);
        r.put("Clever", 100 + lv * 5);
        r.put("CurrentPoisonVal", 0);
        recomputePower(r);
        RoleStore.INSTANCE.savePromotion(r, lv - 1, li.acupointPromotePoint);
    }

    public static boolean canBreak(Map<String, Object> r) {
        int lv = num(r, "Level", 1);
        if (lv >= GameData.maxLevel()) return false;
        return lng(r, "SuperAir") >= GameData.level(lv).upgradeLimit;
    }

    // ------------------------------------------------------------ 离线收益

    /** 返回离线秒数，并把收益写进角色。 */
    public static long applyOffline(Map<String, Object> r) {
        long now = System.currentTimeMillis();
        long last = lng(r, "LastOnline", now);
        r.put("LastOnline", now);
        long sec = Math.max(0, (now - last) / 1000L);
        if (sec < 60) {
            r.put("OfflineIsFull", false);
            r.put("OfflineTime", 0);
            r.put("OfflineSuperAir", 0L);
            r.put("OfflinePerception", 0L);
            r.put("OfflinePhysique", 0L);
            return 0;
        }
        long cap = ServerConfig.get().offlineCapHours * 3600L;
        sec = Math.min(sec, cap);

        int lv = num(r, "Level", 1);
        GameData.LevelInfo li = GameData.level(lv);
        String[] keys = {"Perception", "Physique", "SuperAir"};
        double[] ceilings = {li.limitPerception * 1.5, li.limitToughTenacity * 1.5, li.upgradeLimit};
        for (int i = 0; i < keys.length; i++) {
            String key = keys[i];
            long total = lng(r, key);
            long gained = 0;
            if (isActive(r, i + 1)) {
                double perSecond = rate(r, i + 1) * 0.8 * (i < 2 ? 0.5 : 1.0);
                gained = Math.min((long) Math.floor(perSecond * sec), Math.max(0L, (long) ceilings[i] - total));
                r.put(key, total + gained);
                if (i < 2 && gained > 0) r.put("Current" + key, total + gained);
            }
            r.put("Offline" + key, gained);
        }
        if (lng(r, "OfflinePhysique") > 0) {
            long blood = Math.max(Math.max(100L, lng(r, "PureBlood")), Math.round(lng(r, "Physique") * 0.01));
            r.put("PureBlood", blood);
            r.put("CurrentPureBlood", blood);
        }
        r.put("OfflineTime", (int) Math.min(Integer.MAX_VALUE, sec));
        r.put("OfflineIsFull", isActive(r, TYPE_NAQI) && lng(r, "OfflineSuperAir") == 0
                && lng(r, "SuperAir") >= (long) li.upgradeLimit);
        recomputePower(r);
        save(r);
        return sec;
    }
}
