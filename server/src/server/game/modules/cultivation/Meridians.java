package server.game;

import com.google.gson.Gson;
import server.common.Config;
import server.common.JsonStore;
import xzj.Xzj;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;

/** 穴位表同时决定前后关联、奇经归属与每层消耗。 */
final class Meridians {
    private static final List<String> THREADS = Arrays.asList("任脉", "督脉", "冲脉", "带脉", "阴跷脉", "阳跷脉", "阴维脉", "阳维脉");
    private static final double[] NORMAL_BONUS = {0, 0.5, 0.8, 1};
    private static final double[] BELT_BONUS = {0, 200, 500, 2000};

    static final class Node {
        private String a, e, h;
        private int i;
        final Set<String> parents = new HashSet<>();
        final Set<String> neighbors = new HashSet<>();
        final List<Integer> threads = new ArrayList<>();

        int cost(int level) { return i * (level * 2 + 1); }
    }

    private static final class Title {
        int Id, Type, EffectValue;
    }

    private static final class Rules {
        static final Map<String, Node> POINTS = loadPoints();
        static final Map<Integer, Integer> TITLES = loadTitles();
    }

    private static <T> T table(String name, Class<T> type) {
        Path path = Paths.get(Config.DATA_DIR, "config", name);
        if (!Files.exists(path)) return null;
        try {
            return new Gson().fromJson(JsonStore.decode(Files.readAllBytes(path)), type);
        } catch (IOException error) {
            throw new IllegalStateException("无法读取经脉配置 " + path, error);
        }
    }

    private static List<String> names(String csv) {
        return csv == null || csv.isEmpty() ? Collections.emptyList() : Arrays.asList(csv.split(","));
    }

    private static Map<String, Node> loadPoints() {
        Map<String, Node> nodes = new TreeMap<>();
        Node[] rows = table("b.json", Node[].class);
        if (rows == null) return nodes;
        for (Node node : rows) {
            if (node.a == null || node.a.isEmpty() || node.i < 1 || node.i > Integer.MAX_VALUE / 5 || nodes.put(node.a, node) != null)
                throw new IllegalStateException("无效穴位 " + node.a);
            for (String name : names(node.e)) {
                int idx = THREADS.indexOf(name);
                if (idx >= 0) node.threads.add(idx);
            }
        }
        if (!nodes.containsKey("1")) throw new IllegalStateException("经脉配置缺少胞中");
        for (Node node : rows) {
            for (String childId : names(node.h)) {
                Node child = nodes.get(childId);
                if (child == null || child == node) throw new IllegalStateException("无效后继穴位 " + childId);
                child.parents.add(node.a);
                node.neighbors.add(childId);
                child.neighbors.add(node.a);
            }
        }
        return nodes;
    }

    private static Map<Integer, Integer> loadTitles() {
        Map<Integer, Integer> bonuses = new HashMap<>();
        Title[] titles = table("chenghaoinfo.json", Title[].class);
        if (titles != null) for (Title title : titles) {
            if (title.Type == 14 && title.EffectValue > 0 && title.EffectValue <= 100) bonuses.put(title.Id, title.EffectValue);
        }
        return bonuses;
    }

    static Map<String, Node> nodes() { return Rules.POINTS; }

    @SuppressWarnings("unchecked")
    static Map<String, Integer> levels(Map<String, Object> role) {
        Map<String, Integer> levels = new TreeMap<>();
        Object entries = role.get("Acupoints");
        if (!(entries instanceof List)) return levels;
        for (Object value : (List<?>) entries) {
            if (!(value instanceof Map)) continue;
            Map<String, Object> row = (Map<String, Object>) value;
            String id = RoleUtil.str(row, "Id");
            int level = (int) Math.min(3, Math.max(0, RoleUtil.lng(row, "BrokenLevel")));
            if (!id.isEmpty() && level > 0) levels.merge(id, level, Math::max);
        }
        return levels;
    }

    static long balance(Map<String, Object> role) {
        if (role.get("AcupointValue") == null && RoleUtil.num(role, "Level") >= 2 && levels(role).isEmpty()) return 1;
        return RoleUtil.lng(role, "AcupointValue");
    }

    static int chance(Map<String, Object> role, Node node, Map<String, Integer> levels, int level) {
        if (level == 0) return 100;
        int titleBonus = 0;
        Object titles = role.get("TitleId");
        if (titles instanceof List) for (Object title : (List<?>) titles) {
            int id = RoleUtil.num(Collections.singletonMap("Id", title), "Id");
            titleBonus = Math.max(titleBonus, Rules.TITLES.getOrDefault(id, 0));
        }
        int links = 0;
        for (String id : node.neighbors) if (levels.containsKey(id)) links++;
        return Math.min(100, 90 - 40 * level + titleBonus + Math.min(2, links) * 10);
    }

    private static final class Progress {
        final int[] count = new int[8], floor = new int[8], total = new int[8];
        final double[] bonus = new double[8];

        Progress(Map<String, Object> role) {
            Map<String, Integer> levels = levels(role);
            if (levels.isEmpty()) return;
            for (Map.Entry<String, Node> entry : nodes().entrySet()) {
                int level = levels.getOrDefault(entry.getKey(), 0);
                for (int idx : entry.getValue().threads) {
                    total[idx]++;
                    if (level == 0) continue;
                    if (count[idx]++ == 0 || level < floor[idx]) floor[idx] = level;
                    if (idx == 3) bonus[idx] += BELT_BONUS[level];
                    else if (idx == 5 || idx == 7) bonus[idx] += level;
                    else bonus[idx] += NORMAL_BONUS[level];
                }
            }
            for (int i = 0; i < floor.length; i++) if (count[i] != total[i]) floor[i] = 0;
        }
    }

    static double breathFactor(Map<String, Object> role) {
        if (!(role.get("Acupoints") instanceof List) || ((List<?>) role.get("Acupoints")).isEmpty()) return 1;
        Progress p = new Progress(role);
        return (100 + p.bonus[1]) / (100 - p.bonus[0]);
    }

    static Xzj.AcupointDetailResp_1005020 overview(Map<String, Object> role) {
        Progress p = new Progress(role);
        Xzj.AcupointDetailResp_1005020.Builder out = Xzj.AcupointDetailResp_1005020.newBuilder()
                .setIsSuccess(true).setCode(2000).setAirSpeedUp(100 - p.bonus[0]).setAirValueUp(100 + p.bonus[1])
                .setPureBloodValueUp(100 + p.bonus[2]).setPureBloodRecoveryUp(100 + p.bonus[3])
                .setPerceptionValueUp(100 + p.bonus[4]).setFiveElementAttackUp(100 + p.bonus[5])
                .setPhysiqueValueUp(100 + p.bonus[6]).setFiveElementDefenseUp(100 + p.bonus[7]);
        for (int i = 0; i < p.count.length; i++) {
            String prefix = "ThreadType" + (i + 1);
            out.setField(out.getDescriptorForType().findFieldByName(prefix + "Count"), p.count[i]);
            out.setField(out.getDescriptorForType().findFieldByName(prefix + "Level"), p.floor[i]);
        }
        return out.build();
    }

    private Meridians() {}
}
