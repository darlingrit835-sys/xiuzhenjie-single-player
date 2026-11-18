package server.game;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 洞府灵田。
 *
 * 客户端地块状态（LT_LingTianItem 的 switch）：
 *   0 未开垦（显示"开垦"按钮）
 *   1 开垦中（进度条，时间到 0 客户端自己推进到 2）
 *   2 已开垦，待放置灵土
 *   3 可种植
 *   4/5 生长中   6 成熟可收获
 *
 * 原版开垦要等很久，单机版把开垦耗时压到 20 秒，方便自己玩。
 */
public class PlantService {

    public static final long KAIKEN_MS = 20_000L;
    public static final int KAIKEN_VIGOR = 3;

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> lands(Map<String, Object> role) {
        Object o = role.get("Lands");
        if (o instanceof List) return (List<Map<String, Object>>) o;
        List<Map<String, Object>> list = new ArrayList<>();
        role.put("Lands", list);
        return list;
    }

    public static Map<String, Object> land(Map<String, Object> role, int id) {
        for (Map<String, Object> m : lands(role)) {
            if (GoodsService.num(m, "Id", -1) == id) return m;
        }
        return null;
    }

    /** 开垦中的地块到时推进为"已开垦待放土"。 */
    public static void tick(Map<String, Object> role) {
        long now = System.currentTimeMillis();
        for (Map<String, Object> m : lands(role)) {
            if (GoodsService.num(m, "Status", 0) == 1) {
                long start = ((Number) m.getOrDefault("StartAt", now)).longValue();
                long total = ((Number) m.getOrDefault("TotalMs", KAIKEN_MS)).longValue();
                if (now - start >= total) m.put("Status", 2);
            }
        }
    }

    public static long remaining(Map<String, Object> m) {
        if (GoodsService.num(m, "Status", 0) != 1) return 0;
        long now = System.currentTimeMillis();
        long start = ((Number) m.getOrDefault("StartAt", now)).longValue();
        long total = ((Number) m.getOrDefault("TotalMs", KAIKEN_MS)).longValue();
        return Math.max(0, total - (now - start));
    }

    /** 每块地一份完整状态，用于 1013001 下发。 */
    public static Map<String, Object> snapshot(Map<String, Object> role, Map<String, Object> m) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", GoodsService.num(m, "Id", 1));
        out.put("Status", GoodsService.num(m, "Status", 0));
        out.put("TimeRemaining", remaining(m));
        out.put("TotalWasteTime", ((Number) m.getOrDefault("TotalMs", KAIKEN_MS)).longValue());
        out.put("PlantId", GoodsService.str(m, "PlantId"));
        out.put("PlantName", GoodsService.str(m, "PlantName"));
        return out;
    }
}
