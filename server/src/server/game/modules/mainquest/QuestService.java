package server.game;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;
import server.common.Config;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.Writer;
import java.lang.reflect.Type;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 主线任务系统。
 *
 * 任务名称/描述/奖励直接读客户端的 data/config/mission.json（126 条主线），
 * 完成条件由 data/config/quests.json 配置；未配置的任务会自动推断，
 * 推断不出来的任务会在 5 秒后自动完成，保证任何玩家都不会卡任务。
 *
 * 角色字段：
 *   TaskId / TaskStatus          当前任务与状态（0 进行中，1 可领取）
 *   QuestKills / QuestBossKills  本轮任务击杀数
 *   QuestSweeps / QuestChats / QuestDungeons / QuestCycles
 *   QuestGongFaRank              本轮任务中装备过的最高功法品阶
 *   TaskAutoAt                   自动完成任务的到期时间
 */
public final class QuestService {
    private static final Gson GSON = new GsonBuilder().setPrettyPrinting().create();

    public static final class QuestDef {
        public int id;
        public String name = "";
        public String condition = "";
        public String content = "";
        public String award = "";
        public int awardGoodsType;
        public int awardGoodsRank;
        public int awardGoodsStage;
        public int awardNum = 1;
    }

    public static final class Rule {
        public String type = "AUTO";
        public int target = 1;

        public Rule() {}
        public Rule(String type, int target) {
            this.type = type;
            this.target = target;
        }
    }

    private static final Map<Integer, QuestDef> DEFS = new LinkedHashMap<>();
    private static final Map<Integer, Rule> RULES = new HashMap<>();
    private static final Map<Integer, Rule> INFERRED = new HashMap<>();
    private static boolean loaded;

    private QuestService() {}

    // ------------------------------------------------------------------ 数据加载

    private static synchronized void load() {
        if (loaded) return;
        loaded = true;
        loadDefs();
        loadRules();
    }

    @SuppressWarnings("unchecked")
    private static void loadDefs() {
        File f = new File(Config.DATA_DIR + "/config/mission.json");
        if (!f.exists()) {
            System.err.println("[Quest] mission.json not found: " + f.getAbsolutePath());
            return;
        }
        try (Reader r = new InputStreamReader(new FileInputStream(f), StandardCharsets.UTF_8)) {
            Type listType = new TypeToken<List<Map<String, Object>>>() {}.getType();
            List<Map<String, Object>> rows = GSON.fromJson(r, listType);
            if (rows == null) return;
            for (Map<String, Object> row : rows) {
                QuestDef d = new QuestDef();
                d.id = i(row.get("id"), 0);
                d.name = s(row.get("name"));
                d.condition = s(row.get("condition"));
                d.content = s(row.get("content"));
                d.award = s(row.get("award"));
                d.awardGoodsType = i(row.get("awardGoodsType"), 0);
                d.awardGoodsRank = i(row.get("awardGoodsRank"), 1);
                d.awardGoodsStage = i(row.get("awardGoodsStage"), 1);
                d.awardNum = Math.max(1, i(row.get("awardNum"), 1));
                if (d.id > 0) DEFS.put(d.id, d);
            }
            System.out.println("[Quest] loaded " + DEFS.size() + " mission definitions");
        } catch (Exception e) {
            System.err.println("[Quest] load mission.json failed: " + e.getMessage());
        }
    }

    private static void loadRules() {
        for (Map.Entry<Integer, Rule> e : defaultOverrides().entrySet()) RULES.put(e.getKey(), e.getValue());
        File f = new File(Config.DATA_DIR + "/config/quests.json");
        if (!f.exists()) {
            try {
                File parent = f.getParentFile();
                if (parent != null) parent.mkdirs();
                try (Writer w = new OutputStreamWriter(new FileOutputStream(f), StandardCharsets.UTF_8)) {
                    GSON.toJson(defaultOverrides(), w);
                }
                System.out.println("[Quest] created default quests.json");
            } catch (Exception e) {
                System.err.println("[Quest] create quests.json failed: " + e.getMessage());
            }
            return;
        }
        try (Reader r = new InputStreamReader(new FileInputStream(f), StandardCharsets.UTF_8)) {
            Type t = new TypeToken<Map<String, Rule>>() {}.getType();
            Map<String, Rule> m = GSON.fromJson(r, t);
            if (m != null) {
                for (Map.Entry<String, Rule> e : m.entrySet()) {
                    try {
                        int id = Integer.parseInt(e.getKey());
                        if (e.getValue() != null) RULES.put(id, e.getValue());
                    } catch (NumberFormatException ignored) {}
                }
            }
            System.out.println("[Quest] loaded " + RULES.size() + " quest rules");
        } catch (Exception e) {
            System.err.println("[Quest] load quests.json failed: " + e.getMessage());
        }
    }

    private static Map<Integer, Rule> defaultOverrides() {
        Map<Integer, Rule> m = new LinkedHashMap<>();
        m.put(1, new Rule("CULTIVATE_NAQI", 1));
        m.put(2, new Rule("KILL", 1));
        m.put(3, new Rule("AUTO", 1));
        m.put(4, new Rule("LEVEL", 2));
        m.put(5, new Rule("AUTO", 1));
        m.put(6, new Rule("SWEEP", 1));
        m.put(7, new Rule("AUTO", 1));
        m.put(8, new Rule("AUTO", 1));
        m.put(9, new Rule("LEVEL", 3));
        m.put(10, new Rule("KILL", 5));
        m.put(11, new Rule("SWEEP", 3));
        m.put(12, new Rule("AUTO", 1));
        m.put(13, new Rule("LEVEL", 4));
        m.put(14, new Rule("CHAT", 1));
        m.put(15, new Rule("STONE", 1500));
        m.put(16, new Rule("AUTO", 1));
        m.put(17, new Rule("GONGFA", 3));
        m.put(18, new Rule("AUTO", 1));
        m.put(19, new Rule("AUTO", 1));
        m.put(20, new Rule("AUTO", 1));
        m.put(21, new Rule("AUTO", 1));
        m.put(22, new Rule("AUTO", 1));
        m.put(23, new Rule("LEVEL", 7));
        m.put(24, new Rule("AUTO", 1));
        m.put(25, new Rule("DUNGEON", 1));
        m.put(26, new Rule("AUTO", 1));
        m.put(27, new Rule("LEVEL", 10));
        m.put(28, new Rule("AUTO", 1));
        m.put(29, new Rule("LEVEL", 11));
        m.put(30, new Rule("CULTIVATE_DUANTI", 1));
        m.put(31, new Rule("CYCLES", 30));
        m.put(32, new Rule("GONGFA", 1));
        m.put(33, new Rule("AUTO", 1));
        m.put(34, new Rule("BOSS_KILL", 1));
        m.put(35, new Rule("SWEEP", 2));
        m.put(36, new Rule("STONE", 15000));
        m.put(37, new Rule("CHAT", 1));
        m.put(38, new Rule("AUTO", 1));
        m.put(39, new Rule("GONGFA", 3));
        m.put(40, new Rule("AUTO", 1));
        m.put(41, new Rule("AUTO", 1));
        m.put(42, new Rule("AUTO", 1));
        m.put(43, new Rule("BOSS_KILL", 1));
        return m;
    }

    public static QuestDef def(int id) {
        load();
        return DEFS.get(id);
    }

    public static Rule rule(int id) {
        load();
        Rule r = RULES.get(id);
        if (r != null) return r;
        r = INFERRED.get(id);
        if (r != null) return r;
        QuestDef d = DEFS.get(id);
        r = d == null ? new Rule("AUTO", 1) : infer(d);
        INFERRED.put(id, r);
        return r;
    }

    public static int maxTaskId() {
        load();
        int max = 0;
        for (int id : DEFS.keySet()) max = Math.max(max, id);
        return max;
    }

    // ------------------------------------------------------------------ 角色初始化

    public static void ensureRole(Map<String, Object> r) {
        load();
        if (RoleUtil.num(r, "TaskId", 0) <= 0) r.put("TaskId", 1);
        int st = RoleUtil.num(r, "TaskStatus", 0);
        if (st != 0 && st != 1) r.put("TaskStatus", 0);
        putIfMissing(r, "QuestKills", 0);
        putIfMissing(r, "QuestBossKills", 0);
        putIfMissing(r, "QuestSweeps", 0);
        putIfMissing(r, "QuestChats", 0);
        putIfMissing(r, "QuestDungeons", 0);
        putIfMissing(r, "QuestCycles", 0);
        putIfMissing(r, "QuestGongFaRank", 0);
        putIfMissing(r, "QuestGongFaCount", 0);
        putIfMissing(r, "TaskAutoAt", 0L);
        putIfMissing(r, "QuestTotalKills", 0);
        putIfMissing(r, "QuestTotalSweeps", 0);
    }

    private static void putIfMissing(Map<String, Object> r, String k, Object v) {
        if (!r.containsKey(k) || r.get(k) == null) r.put(k, v);
    }

    /** 登录时调用：同步已装备功法，并把已经满足的任务标记为可领取。 */
    public static void prepareLogin(Map<String, Object> r) {
        ensureRole(r);
        syncEquippedGongFa(r);
        syncLearnedCount(r);
        if (RoleUtil.num(r, "TaskStatus", 0) == 0) evaluate(r, false);
    }

    private static void syncLearnedCount(Map<String, Object> r) {
        int learned = 0;
        for (GameData.GongFaDef method : GameData.gongFaAll()) {
            if (RoleUtil.isLearned(r, method.id)) learned++;
        }
        r.put("QuestGongFaCount", Math.max(RoleUtil.num(r, "QuestGongFaCount"), learned));
    }

    private static void syncEquippedGongFa(Map<String, Object> r) {
        int max = 0;
        for (int t = 1; t <= 3; t++) {
            GameData.GongFaDef g = RoleUtil.equipped(r, t);
            if (g != null) max = Math.max(max, g.rank);
        }
        r.put("QuestGongFaRank", Math.max(RoleUtil.num(r, "QuestGongFaRank", 0), max));
    }

    // ------------------------------------------------------------------ 任务判定

    private static boolean evaluate(Map<String, Object> r, boolean setAutoTimer) {
        int tid = RoleUtil.num(r, "TaskId", 1);
        Rule rule = rule(tid);
        int status = RoleUtil.num(r, "TaskStatus", 0);
        if (status == 1) return false;

        boolean done;
        if ("AUTO".equals(rule.type)) {
            long at = RoleUtil.lng(r, "TaskAutoAt", 0);
            if (at <= 0) {
                if (setAutoTimer) r.put("TaskAutoAt", System.currentTimeMillis() + 5000L);
                return false;
            }
            done = System.currentTimeMillis() >= at;
        } else {
            done = check(r, rule);
        }
        if (done) {
            r.put("TaskStatus", 1);
            r.put("TaskAutoAt", 0L);
            return true;
        }
        return false;
    }

    private static boolean check(Map<String, Object> r, Rule rule) {
        int target = Math.max(1, rule.target);
        switch (rule.type) {
            case "LEVEL":
                return RoleUtil.num(r, "Level", 1) >= target;
            case "KILL":
                return RoleUtil.num(r, "QuestKills", 0) >= target;
            case "BOSS_KILL":
                return RoleUtil.num(r, "QuestBossKills", 0) >= target;
            case "SWEEP":
                return RoleUtil.num(r, "QuestSweeps", 0) >= target;
            case "STONE":
                return RoleUtil.lng(r, "ShinyStone", 0) >= target;
            case "GONGFA":
                return RoleUtil.num(r, "QuestGongFaRank", 0) >= target;
            case "GONGFA_COUNT":
                return RoleUtil.num(r, "QuestGongFaCount", 0) >= target;
            case "CHAT":
                return RoleUtil.num(r, "QuestChats", 0) >= target;
            case "DUNGEON":
                return RoleUtil.num(r, "QuestDungeons", 0) >= target;
            case "CYCLES":
                return RoleUtil.num(r, "QuestCycles", 0) >= target;
            case "CULTIVATE_NAQI":
                return RoleUtil.isActive(r, RoleUtil.TYPE_NAQI);
            case "CULTIVATE_DUANTI":
                return RoleUtil.isActive(r, RoleUtil.TYPE_DUANTI);
            default:
                return false;
        }
    }

    // ------------------------------------------------------------------ 心跳 / 事件

    /** 修炼心跳调用：处理自动任务、锻体周天和“运行功法”类任务。 */
    public static boolean tick(PlayerSession s) {
        if (s == null || s.role == null || s.role.isEmpty()) return false;
        synchronized (s.role) {
            ensureRole(s.role);
            if (RoleUtil.num(s.role, "TaskStatus", 0) == 1) return false;
            int tid = RoleUtil.num(s.role, "TaskId", 1);
            Rule rule = rule(tid);
            int mask = RoleUtil.activeMask(s.role);

            boolean duanti = (mask & RoleUtil.TYPE_DUANTI) != 0;
            if ("CYCLES".equals(rule.type) && duanti) {
                int c = RoleUtil.num(s.role, "QuestCycles", 0) + 1;
                s.role.put("QuestCycles", c);
            }
            boolean done = evaluate(s.role, true);
            if (done) RoleUtil.save(s.role);
            return done;
        }
    }

    /** 突破成功。 */
    public static boolean onLevelUp(Map<String, Object> r) {
        ensureRole(r);
        if (RoleUtil.num(r, "TaskStatus", 0) == 1) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"LEVEL".equals(rule.type)) return false;
        boolean done = check(r, rule);
        if (done) markComplete(r);
        return done;
    }

    /** 拾取灵石 / 奖励到账后检查“拥有灵石”任务。 */
    public static boolean onStone(Map<String, Object> r) {
        ensureRole(r);
        if (RoleUtil.num(r, "TaskStatus", 0) == 1) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"STONE".equals(rule.type)) return false;
        boolean done = check(r, rule);
        if (done) markComplete(r);
        return done;
    }

    /** 装备/参悟功法。 */
    public static boolean onGongFa(Map<String, Object> r, int rank) {
        ensureRole(r);
        syncLearnedCount(r);
        int old = RoleUtil.num(r, "QuestGongFaRank", 0);
        if (rank > old) r.put("QuestGongFaRank", rank);
        if (RoleUtil.num(r, "TaskStatus", 0) == 1) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"GONGFA".equals(rule.type) && !"GONGFA_COUNT".equals(rule.type)) return false;
        boolean done = check(r, rule);
        if (done) markComplete(r);
        return done;
    }

    /** 聊天/传音。 */
    public static boolean onChat(Map<String, Object> r) {
        ensureRole(r);
        r.put("QuestChats", RoleUtil.num(r, "QuestChats", 0) + 1);
        if (RoleUtil.num(r, "TaskStatus", 0) == 1) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"CHAT".equals(rule.type)) return false;
        if (RoleUtil.num(r, "QuestChats", 0) >= Math.max(1, rule.target)) {
            markComplete(r);
            return true;
        }
        return false;
    }

    /** 进入副本。 */
    public static boolean onDungeon(Map<String, Object> r) {
        ensureRole(r);
        r.put("QuestDungeons", RoleUtil.num(r, "QuestDungeons", 0) + 1);
        if (RoleUtil.num(r, "TaskStatus", 0) == 1) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"DUNGEON".equals(rule.type)) return false;
        if (RoleUtil.num(r, "QuestDungeons", 0) >= Math.max(1, rule.target)) {
            markComplete(r);
            return true;
        }
        return false;
    }

    /** 在当前角色对象上累加击杀进度，供战斗服/奖励服务共用。 */
    public static boolean applyKill(Map<String, Object> r, boolean boss) {
        ensureRole(r);
        r.put("QuestKills", RoleUtil.num(r, "QuestKills", 0) + 1);
        r.put("QuestTotalKills", RoleUtil.num(r, "QuestTotalKills", 0) + 1);
        if (boss) {
            r.put("QuestBossKills", RoleUtil.num(r, "QuestBossKills", 0) + 1);
            r.put("QuestTotalBossKills", RoleUtil.num(r, "QuestTotalBossKills", 0) + 1);
        }
        if (RoleUtil.num(r, "TaskStatus", 0) != 0) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"KILL".equals(rule.type) && !"BOSS_KILL".equals(rule.type)) return false;
        boolean done = check(r, rule);
        if (done) markComplete(r);
        return done;
    }

    /** 在当前角色对象上累加扫荡进度。 */
    public static boolean applySweep(Map<String, Object> r) {
        ensureRole(r);
        r.put("QuestSweeps", RoleUtil.num(r, "QuestSweeps", 0) + 1);
        r.put("QuestTotalSweeps", RoleUtil.num(r, "QuestTotalSweeps", 0) + 1);
        if (RoleUtil.num(r, "TaskStatus", 0) != 0) return false;
        Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
        if (!"SWEEP".equals(rule.type)) return false;
        boolean done = check(r, rule);
        if (done) markComplete(r);
        return done;
    }

    /** 战斗服击杀怪物（跨线程，优先更新在线会话，避免覆盖未落盘的修炼数据）。 */
    public static void onKill(String roleId, boolean boss) {
        if (roleId == null || roleId.isEmpty()) return;
        load();
        PlayerSession s = GameServer.sessionOfRole(roleId);
        boolean online = s != null && !s.role.isEmpty();
        Map<String, Object> r = online ? s.role : RoleStore.INSTANCE.getRole(roleId);
        if (r == null) return;
        boolean done = false;
        synchronized (r) {
            ensureRole(r);
            r.put("QuestKills", RoleUtil.num(r, "QuestKills", 0) + 1);
            r.put("QuestTotalKills", RoleUtil.num(r, "QuestTotalKills", 0) + 1);
            if (boss) {
                r.put("QuestBossKills", RoleUtil.num(r, "QuestBossKills", 0) + 1);
                r.put("QuestTotalBossKills", RoleUtil.num(r, "QuestTotalBossKills", 0) + 1);
            }
            if (RoleUtil.num(r, "TaskStatus", 0) == 0) {
                Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
                if ("KILL".equals(rule.type) || "BOSS_KILL".equals(rule.type)) {
                    done = check(r, rule);
                    if (done) markComplete(r);
                }
            }
            RoleUtil.save(r);
        }
        if (done) GameServer.notifyTaskComplete(roleId);
    }

    /** 一波怪全部清空（扫荡完成）。 */
    public static void onSweep(String roleId) {
        if (roleId == null || roleId.isEmpty()) return;
        load();
        PlayerSession s = GameServer.sessionOfRole(roleId);
        boolean online = s != null && !s.role.isEmpty();
        Map<String, Object> r = online ? s.role : RoleStore.INSTANCE.getRole(roleId);
        if (r == null) return;
        boolean done = false;
        synchronized (r) {
            ensureRole(r);
            r.put("QuestSweeps", RoleUtil.num(r, "QuestSweeps", 0) + 1);
            r.put("QuestTotalSweeps", RoleUtil.num(r, "QuestTotalSweeps", 0) + 1);
            if (RoleUtil.num(r, "TaskStatus", 0) == 0) {
                Rule rule = rule(RoleUtil.num(r, "TaskId", 1));
                if ("SWEEP".equals(rule.type)) {
                    done = check(r, rule);
                    if (done) markComplete(r);
                }
            }
            RoleUtil.save(r);
        }
        if (done) GameServer.notifyTaskComplete(roleId);
    }

    private static void markComplete(Map<String, Object> r) {
        r.put("TaskStatus", 1);
        r.put("TaskAutoAt", 0L);
    }

    // ------------------------------------------------------------------ 领奖 / 推进

    /**
     * 领取任务奖励。
     * @return 新任务状态；-1 = 任务号不对，-2 = 尚未完成
     */
    public static int receiveAward(PlayerSession s, int reqTaskId) {
        int result;
        synchronized (s.role) {
            ensureRole(s.role);
            int tid = RoleUtil.num(s.role, "TaskId", 1);
            if (reqTaskId != tid) return -1;
            if (RoleUtil.num(s.role, "TaskStatus", 0) != 1) return -2;

            QuestDef d = def(tid);
            if (d != null) grant(s.role, d);

            int next = tid + 1;
            if (next > maxTaskId()) {
                // 主线全部完成，停在最后一条
                s.role.put("TaskStatus", 0);
                s.role.put("TaskCompletedAll", true);
                RoleUtil.save(s.role);
                return 0;
            }
            s.role.put("TaskId", next);
            s.role.put("TaskStatus", 0);
            resetTaskCounters(s.role);
            s.role.put("TaskAutoAt", 0L);
            Rule nr = rule(next);
            if ("AUTO".equals(nr.type)) {
                s.role.put("TaskAutoAt", System.currentTimeMillis() + 5000L);
            } else if (check(s.role, nr)) {
                markComplete(s.role);
            }
            RoleUtil.save(s.role);
            result = RoleUtil.num(s.role, "TaskStatus", 0);
        }
        return result;
    }

    private static void resetTaskCounters(Map<String, Object> r) {
        r.put("QuestKills", 0);
        r.put("QuestBossKills", 0);
        r.put("QuestSweeps", 0);
        r.put("QuestChats", 0);
        r.put("QuestDungeons", 0);
        r.put("QuestCycles", 0);
        r.put("QuestGongFaRank", 0);
        syncEquippedGongFa(r);
    }

    @SuppressWarnings("unchecked")
    private static void grant(Map<String, Object> r, QuestDef d) {
        int num = Math.max(1, d.awardNum);
        long stone = 30L * num;
        long jade = 0;
        if (d.awardGoodsType == 99) {
            jade = num;
        } else if (d.awardGoodsType > 0) {
            // 任务奖励直接进纳戒，客户端任务面板展示的物品就是背包里实际拿到的
            GoodsService.addReward(r, d.award, d.awardGoodsType, d.awardGoodsRank,
                    d.awardGoodsStage, num);
        }
        if (jade > 0) {
            r.put("ShinyJade", RoleUtil.num(r, "ShinyJade", 0) + (int) jade);
        }
        if (stone > 0) {
            r.put("ShinyStone", RoleUtil.lng(r, "ShinyStone", 0) + stone);
        }

        List<Map<String, Object>> bag = new ArrayList<>();
        Object old = r.get("QuestBag");
        if (old instanceof List) {
            for (Object o : (List<Object>) old) {
                if (o instanceof Map) bag.add((Map<String, Object>) o);
            }
        }
        Map<String, Object> item = new LinkedHashMap<>();
        item.put("name", d.award);
        item.put("type", d.awardGoodsType);
        item.put("rank", d.awardGoodsRank);
        item.put("stage", d.awardGoodsStage);
        item.put("num", num);
        item.put("taskId", d.id);
        item.put("time", System.currentTimeMillis());
        bag.add(item);
        while (bag.size() > 80) bag.remove(0);
        r.put("QuestBag", bag);
        r.put("LastQuestId", d.id);
        r.put("LastQuestAward", d.award);
    }

    // ------------------------------------------------------------------ 条件推断

    private static Rule infer(QuestDef d) {
        String c = d.condition == null ? "" : d.condition;

        if (c.contains("突破")) {
            int lv = parseBreakLevel(c);
            if (lv > 0) return new Rule("LEVEL", lv);
        }
        if (c.contains("击杀")) {
            if (c.contains("领主") || c.contains("长老") || c.contains("首领") || c.contains("兽王")) {
                return new Rule("BOSS_KILL", Math.max(1, parseSmallNumber(c)));
            }
            return new Rule("KILL", Math.max(1, parseSmallNumber(c)));
        }
        if (c.contains("扫荡")) {
            return new Rule("SWEEP", Math.max(1, parseSmallNumber(c)));
        }
        if (c.contains("灵石") && (c.contains("拥有") || c.contains("获取") || c.contains("超过"))) {
            return new Rule("STONE", Math.max(1, parseMoney(c)));
        }
        if (c.contains("功法") && (c.contains("修习") || c.contains("参悟") || c.contains("补全"))) {
            if (c.contains("十种") || c.contains("10种")) return new Rule("GONGFA_COUNT", 10);
            if (c.contains("玄阶")) return new Rule("GONGFA", 5);
            if (c.contains("地阶")) return new Rule("GONGFA", 9);
            if (c.contains("天阶")) return new Rule("GONGFA", 13);
            return new Rule("GONGFA", 1);
        }
        if (c.contains("发言") || c.contains("传音")) return new Rule("CHAT", 1);
        if (c.contains("秘境")) return new Rule("DUNGEON", 1);
        if (c.contains("锻体修行")) {
            if (c.contains("周天")) return new Rule("CYCLES", Math.max(1, parseSmallNumber(c)));
            return new Rule("CULTIVATE_DUANTI", 1);
        }
        return new Rule("AUTO", 1);
    }

    private static int parseBreakLevel(String c) {
        String[] realms = {"练气", "筑基", "结丹", "元婴", "分神", "炼虚", "合体", "渡劫", "大乘"};
        int base = -1;
        for (int i = 0; i < realms.length; i++) {
            if (c.contains(realms[i])) {
                base = i * 10;
                break;
            }
        }
        if (base < 0) return 0;
        int idx = c.indexOf('层');
        if (idx > 0) {
            int n = chineseDigit(c.charAt(idx - 1));
            if (n > 0) return base + n;
        }
        return base + 1;
    }

    private static int parseSmallNumber(String c) {
        int wan = c.indexOf('万');
        if (wan > 0) {
            int n = digitsBefore(c, wan);
            if (n > 0) return n * 10000;
        }
        int i = firstArabic(c);
        if (i > 0) return i;
        for (int k = 0; k < c.length(); k++) {
            int n = chineseDigit(c.charAt(k));
            if (n > 0) return n;
        }
        return 1;
    }

    private static int parseMoney(String c) {
        int i = firstArabic(c);
        if (i > 0) {
            int p = c.indexOf('万');
            if (p >= 0) return i * 10000;
            return i;
        }
        return parseSmallNumber(c);
    }

    private static int firstArabic(String s) {
        int n = 0;
        boolean found = false;
        for (int i = 0; i < s.length(); i++) {
            char ch = s.charAt(i);
            if (ch >= '0' && ch <= '9') {
                found = true;
                n = n * 10 + (ch - '0');
            } else if (found) {
                break;
            }
        }
        return found ? n : 0;
    }

    private static int digitsBefore(String s, int end) {
        int n = 0;
        boolean found = false;
        for (int i = 0; i < end; i++) {
            char ch = s.charAt(i);
            if (ch >= '0' && ch <= '9') {
                found = true;
                n = n * 10 + (ch - '0');
            }
        }
        return found ? n : 0;
    }

    private static int chineseDigit(char ch) {
        switch (ch) {
            case '一': return 1;
            case '两': return 2;
            case '二': return 2;
            case '三': return 3;
            case '四': return 4;
            case '五': return 5;
            case '六': return 6;
            case '七': return 7;
            case '八': return 8;
            case '九': return 9;
            case '十': return 10;
            default: return 0;
        }
    }

    // ------------------------------------------------------------------ 小工具

    private static int i(Object o, int def) {
        return o instanceof Number ? ((Number) o).intValue() : def;
    }

    private static String s(Object o) {
        return o == null ? "" : String.valueOf(o);
    }
}
