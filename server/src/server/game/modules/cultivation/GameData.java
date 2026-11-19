package server.game;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.File;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.lang.reflect.Type;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * 服务端数值表。
 *
 * 等级表直接读取客户端 xzjrolelevellist.json（90 级，字段与该文件一致），
 * 功法表按 黄/玄/地/天 四阶 × 下/中/上/极品 × 纳气/锻体/炼神 生成，共 48 本。
 */
public final class GameData {
    public static final class LevelInfo {
        public int id;
        public String name = "";
        public double upgradeLimit;
        public double topLimit;
        public double perception;
        public double limitPerception;
        public double toughTenacity;
        public double limitToughTenacity;
        public double trainingPerception;
        public double trainingToughTenacity;
        public double increaseSuperAir;
        public int acupointPromotePoint;
    }

    public static final class GongFaDef {
        public String id;
        public String name;
        public int type;          // 1=炼神 2=锻体 3=纳气
        public int rank;          // 1..16
        public int fiveElement;   // 1金 2木 3水 4火 5土
        public double baseCircle;
        public int levelReq;
        public int effectType1, effectType2;
        public int effectValue1, effectValue2;

        public String rankName() { return GameData.rankName(rank); }
        public String typeName() {
            switch (type) {
                case 1: return "炼神";
                case 2: return "锻体";
                default: return "纳气";
            }
        }
    }

    private static final LevelInfo[] LEVELS = new LevelInfo[91];
    private static final List<GongFaDef> GONGFA = new ArrayList<>();
    private static final java.util.Map<String, GongFaDef> GONGFA_BY_ID = new java.util.HashMap<>();

    private GameData() {}

    // ---------------------------------------------------------------- levels

    @SuppressWarnings("unchecked")
    private static void loadLevels() {
        File f = new File(server.common.Config.DATA_DIR + "/config/xzjrolelevellist.json");
        if (f.exists()) {
            try (Reader reader = new InputStreamReader(new FileInputStream(f), Charset.forName("GBK"))) {
                Type t = new TypeToken<List<Map<String, Object>>>() {}.getType();
                List<Map<String, Object>> rows = new Gson().fromJson(reader, t);
                if (rows != null) {
                    for (Map<String, Object> row : rows) {
                        LevelInfo li = new LevelInfo();
                        li.id = i(row.get("id"), 0);
                        li.upgradeLimit = d(row.get("upgradeLimit"), 100);
                        li.topLimit = d(row.get("topLimit"), li.upgradeLimit * 1.3);
                        li.perception = d(row.get("perception"), 500);
                        li.limitPerception = d(row.get("limitPerception"), li.perception);
                        li.toughTenacity = d(row.get("toughTenacity"), 500);
                        li.limitToughTenacity = d(row.get("limitToughTenacity"), li.toughTenacity);
                        li.trainingPerception = d(row.get("trainingPerception"), li.perception);
                        li.trainingToughTenacity = d(row.get("trainingToughTenacity"), li.toughTenacity);
                        li.increaseSuperAir = d(row.get("increaseSuperAir"), 10);
                        li.acupointPromotePoint = i(row.get("acupointPromotePoint"), 3);
                        li.name = levelName(li.id);
                        if (li.id >= 1 && li.id <= 90) LEVELS[li.id] = li;
                    }
                }
            } catch (Exception e) {
                System.err.println("[GameData] load level table failed: " + e.getMessage());
            }
        }
        // 缺行时用平滑公式补齐，保证 1..90 永远可查
        for (int lv = 1; lv <= 90; lv++) {
            if (LEVELS[lv] != null) continue;
            LevelInfo li = new LevelInfo();
            li.id = lv;
            li.upgradeLimit = 100.0 * Math.pow(1.5, lv - 1);
            li.topLimit = li.upgradeLimit * 1.3;
            li.perception = 500.0 * Math.pow(1.18, lv - 1);
            li.limitPerception = li.perception;
            li.toughTenacity = li.perception;
            li.limitToughTenacity = li.perception;
            li.trainingPerception = li.perception;
            li.trainingToughTenacity = li.perception;
            li.increaseSuperAir = li.upgradeLimit * 0.1;
            li.acupointPromotePoint = (lv % 10 == 1) ? 10 : 3;
            li.name = levelName(lv);
            LEVELS[lv] = li;
        }
    }

    private static int i(Object o, int def) {
        return o instanceof Number ? ((Number) o).intValue() : def;
    }

    private static double d(Object o, double def) {
        return o instanceof Number ? ((Number) o).doubleValue() : def;
    }

    public static LevelInfo level(int lv) {
        if (lv < 1) lv = 1;
        if (lv > 90) lv = 90;
        return LEVELS[lv];
    }

    public static int maxLevel() { return 90; }

    public static String levelName(int lv) {
        String[] big = {"练气", "筑基", "结丹", "元婴", "分神", "炼虚", "合体", "渡劫", "大乘"};
        int stage = (lv - 1) / 10;
        int sub = (lv - 1) % 10 + 1;
        if (stage < 0 || stage >= big.length) return "境界" + lv;
        return big[stage] + sub + "层";
    }

    /** 寿元（设计表：练气100、筑基200、结丹400……每阶翻倍）。 */
    public static int lifespan(int lv) {
        int stage = Math.max(0, (lv - 1) / 10);
        return 100 * (1 << Math.min(stage, 8));
    }

    // ---------------------------------------------------------------- 功法

    private static final String[] RANK_NAMES = {
            "黄阶下品", "黄阶中品", "黄阶上品", "黄阶极品",
            "玄阶下品", "玄阶中品", "玄阶上品", "玄阶极品",
            "地阶下品", "地阶中品", "地阶上品", "地阶极品",
            "天阶下品", "天阶中品", "天阶上品", "天阶极品"
    };

    private static final String[] NAQI_NAMES = {
            "启灵经", "聚元功", "青木长生诀", "玄水真经",
            "赤炎焚天功", "厚土载物诀", "紫气东来篇", "太虚纳灵经",
            "九转归元功", "混元一气诀", "大衍聚灵篇", "万象归宗经",
            "太初吞天诀", "鸿蒙纳气经", "混沌无极功", "天道归元法"
    };
    private static final String[] DUANTI_NAMES = {
            "铁骨功", "淬体诀", "金刚不坏体", "龙象般若功",
            "玄龟炼体术", "蛮荒战体", "不灭金身诀", "九幽淬骨经",
            "星辰炼体篇", "万劫不坏身", "盘古真身诀", "混沌魔躯",
            "太初道体", "鸿蒙圣体", "不灭道躯", "天道法身"
    };
    private static final String[] LIANSHEN_NAMES = {
            "凝神诀", "观想图", "清心咒", "太乙炼神篇",
            "识海无涯经", "紫府凝神法", "神魂淬炼诀", "九幽炼魂经",
            "太虚神识篇", "万象归识经", "星辰炼神法", "混沌神魂诀",
            "太初元神道", "鸿蒙神识经", "不灭元神篇", "天道神魂法"
    };

    private static void buildGongFa() {
        for (int type = 1; type <= 3; type++) {
            String[] names = type == 1 ? LIANSHEN_NAMES : (type == 2 ? DUANTI_NAMES : NAQI_NAMES);
            for (int rank = 1; rank <= 16; rank++) {
                GongFaDef g = new GongFaDef();
                g.type = type;
                g.rank = rank;
                g.id = "gf_" + type + "_" + rank;
                g.name = names[rank - 1];
                g.fiveElement = ((rank - 1) % 5) + 1;
                int tier = (rank - 1) / 4;          // 0..3
                int sub = (rank - 1) % 4;           // 0..3
                double tierMul = new double[]{1.0, 4.0, 16.0, 64.0}[tier];
                double subMul = new double[]{1.0, 1.1, 1.2, 1.3}[sub];
                g.baseCircle = 0.8 * tierMul * subMul;
                g.levelReq = new int[]{1, 11, 21, 31}[tier];
                if (type == 3) {                    // 纳气
                    g.effectType1 = 105; g.effectValue1 = 5 + rank;
                    g.effectType2 = 601; g.effectValue2 = 2 + rank / 2;
                } else if (type == 2) {             // 锻体
                    g.effectType1 = 301; g.effectValue1 = 5 + rank;
                    g.effectType2 = 302; g.effectValue2 = 2 + rank / 2;
                } else {                            // 炼神
                    g.effectType1 = 101; g.effectValue1 = 5 + rank;
                    g.effectType2 = 102; g.effectValue2 = 2 + rank / 2;
                }
                GONGFA.add(g);
                GONGFA_BY_ID.put(g.id, g);
            }
        }
    }

    public static List<GongFaDef> gongFaAll() { return GONGFA; }

    public static GongFaDef gongFa(String id) { return id == null ? null : GONGFA_BY_ID.get(id); }

    public static List<GongFaDef> gongFaByType(int type) {
        List<GongFaDef> out = new ArrayList<>();
        for (GongFaDef g : GONGFA) if (g.type == type) out.add(g);
        return out;
    }

    public static String rankName(int rank) {
        if (rank < 1) return "无";
        if (rank > 16) return "天阶极品";
        return RANK_NAMES[rank - 1];
    }

    /** 功法重数（1..3）对周天效率的加成。 */
    public static double chongMultiplier(int chong) {
        if (chong <= 1) return 1.0;
        if (chong == 2) return 1.2;
        return 1.5;
    }

    static {
        loadLevels();
        buildGongFa();
    }
}
