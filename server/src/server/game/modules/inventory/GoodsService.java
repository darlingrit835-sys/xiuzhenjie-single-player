package server.game;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

/**
 * 纳戒（背包）与新手物资。
 *
 * 角色数据里 Goods 是一个 List<Map>，字段与客户端
 * SpaceGoodsResp_1004002.GoodsInfoResp_1004003 一一对应。
 */
public final class GoodsService {
    public static final int LOCATION_NAJIE = 10;
    public static final int MAX_CAPACITY = 100;
    private static final int STARTER_KIT_VERSION = 2;

    private GoodsService() {}

    // ------------------------------------------------------------------ 基础查询

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> all(Map<String, Object> role) {
        Object o = role.get("Goods");
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object x : (List<Object>) o) {
                if (x instanceof Map) out.add((Map<String, Object>) x);
            }
        }
        return out;
    }

    public static void saveAll(Map<String, Object> role, List<Map<String, Object>> list) {
        role.put("Goods", list);
    }

    public static Map<String, Object> find(Map<String, Object> role, String goodsId) {
        if (goodsId == null) return null;
        for (Map<String, Object> g : all(role)) {
            if (goodsId.equals(str(g, "GoodsId"))) return g;
        }
        return null;
    }

    public static int capacity(Map<String, Object> role) {
        return all(role).size();
    }

    static GameData.GongFaDef book(Map<String, Object> goods) {
        if (goods == null || num(goods, "Type", 0) != 1) return null;
        return GameData.book(str(goods, "TemplateId"), str(goods, "Name"));
    }

    public static boolean isEquipmentType(int type) {
        return type == 14 || type == 15 || type == 16 || type == 21
                || type == 26 || type == 29 || type == 32;
    }

    // ------------------------------------------------------------------ 增删

    public static Map<String, Object> add(Map<String, Object> role, String name, int type, int rank,
                                          int level, int fiveElement, String templateId, int count) {
        return add(role, name, type, rank, level, fiveElement, templateId, count, 1.0, 20);
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> add(Map<String, Object> role, String name, int type, int rank,
                                          int level, int fiveElement, String templateId, int count,
                                          double volume, int recyclePrice) {
        List<Map<String, Object>> list = all(role);
        GameData.GongFaDef method = type == 1 ? GameData.book(templateId, name) : null;
        if (method != null) {
            templateId = method.id;
            name = method.name;
            rank = method.rank;
            level = method.levelReq;
            fiveElement = RoleUtil.methodElement(role, method);
        }
        if (count <= 0) count = 1;
        if (!isEquipmentType(type)) {
            for (Map<String, Object> g : list) {
                boolean sameTemplate = str(g, "TemplateId").equals(templateId) && num(g, "Type", 0) == type;
                boolean sameItem = str(g, "Name").equals(name) && num(g, "Type", 0) == type
                        && num(g, "Rank", 1) == Math.max(1, rank)
                        && num(g, "Level", 1) == Math.max(1, level);
                if (sameTemplate || sameItem) {
                    g.put("Count", Math.max(1, num(g, "Count", 1)) + count);
                    saveAll(role, list);
                    return g;
                }
            }
        }
        if (list.size() >= MAX_CAPACITY) return null;
        Map<String, Object> g = new LinkedHashMap<>();
        g.put("GoodsId", "g_" + UUID.randomUUID().toString().replace("-", "").substring(0, 16));
        g.put("Name", name);
        g.put("Type", type);
        g.put("Rank", Math.max(1, rank));
        g.put("Level", Math.max(1, level));
        g.put("FiveElement", method != null ? fiveElement : Math.max(1, Math.min(5, fiveElement)));
        g.put("TemplateId", templateId);
        g.put("Count", count);
        g.put("Volume", volume <= 0 ? 1.0 : volume);
        g.put("RecyclePrice", Math.max(0, recyclePrice));
        g.put("ConversionRate", 1.0);
        g.put("IsLock", false);
        g.put("IsUsing", false);
        g.put("IsBinding", false);
        list.add(g);
        saveAll(role, list);
        return g;
    }

    public static boolean remove(Map<String, Object> role, String goodsId, int count) {
        List<Map<String, Object>> list = all(role);
        Map<String, Object> hit = null;
        for (Map<String, Object> g : list) {
            if (goodsId != null && goodsId.equals(str(g, "GoodsId"))) {
                hit = g;
                break;
            }
        }
        if (hit == null) return false;
        int have = Math.max(1, num(hit, "Count", 1));
        if (have <= count) {
            list.remove(hit);
        } else {
            hit.put("Count", have - count);
        }
        saveAll(role, list);
        return true;
    }

    // ------------------------------------------------------------------ 新手礼包

    /**
     * 新角色/老角色补发一次新手物资。
     * 其中《启灵经》是客户端新手引导第 4~5 步明确检查的物品（按名字查找）。
     */
    public static void ensureStarterKit(Map<String, Object> role) {
        int version = RoleUtil.num(role, "StarterKitVersion", 0);
        if (version >= STARTER_KIT_VERSION) return;
        int element = primaryElement(role);
        if (version < 1) {
            add(role, "启灵经", 1, 1, 1, element, "gf_3_1", 1, 1.0, 10);
            add(role, "青锋剑", 15, 1, 1, element, "eq_weapon_qingfeng", 1, 1.0, 20);
            add(role, "青布道袍", 16, 1, 1, element, "eq_armor_qingbu", 1, 1.0, 20);
            add(role, "纳气丹", 19, 1, 1, element, "pill_naqi", 3, 0.5, 5);
            add(role, "回春散", 19, 1, 1, element, "pill_huichun", 3, 0.5, 5);
            role.put("StarterKitVersion", 1);
        }
        if (RoleUtil.num(role, "StarterKitVersion", 0) < 2) {
            add(role, "玄铁护心镜", 16, 2, 1, element, "eq_armor_xuantie", 1, 1.0, 40);
            add(role, "青布靴", 29, 1, 1, element, "eq_boots_qingbu", 1, 1.0, 15);
            role.put("StarterKitVersion", 2);
        }
        role.put("StarterKitNotice", true);
    }

    private static int primaryElement(Map<String, Object> role) {
        if (RoleUtil.bool(role, "HasGold")) return 1;
        if (RoleUtil.bool(role, "HasWood")) return 2;
        if (RoleUtil.bool(role, "HasWater")) return 3;
        if (RoleUtil.bool(role, "HasFire")) return 4;
        if (RoleUtil.bool(role, "HasEarth")) return 5;
        return 1;
    }

    /** 任务/引导奖励统一从这里入包，保证客户端能看见。 */
    public static Map<String, Object> addReward(Map<String, Object> role, String name, int type,
                                                int rank, int level, int count) {
        int element = primaryElement(role);
        String template = "reward_" + type + "_" + Math.max(1, rank) + "_" + Math.max(1, level);
        return add(role, name, type, rank, level, element, template, Math.max(1, count),
                0.5, 5 + rank * 5);
    }

    // ------------------------------------------------------------------ MD5

    /** 与客户端 XzjBoxEntity.GetIdsMd5 一致：按 GoodsId 排序，拼 GoodsId+Count，取小写 MD5。 */
    public static String md5(Map<String, Object> role) {
        List<Map<String, Object>> list = all(role);
        list.sort(Comparator.comparing(g -> str(g, "GoodsId")));
        StringBuilder sb = new StringBuilder();
        for (Map<String, Object> g : list) {
            sb.append(str(g, "GoodsId")).append(num(g, "Count", 1));
        }
        return md5Hex(sb.toString().toLowerCase());
    }

    private static String md5Hex(String text) {
        try {
            MessageDigest md = MessageDigest.getInstance("MD5");
            byte[] digest = md.digest(text.getBytes(StandardCharsets.UTF_8));
            StringBuilder sb = new StringBuilder();
            for (byte b : digest) sb.append(String.format("%02x", b & 0xff));
            return sb.toString();
        } catch (Exception e) {
            return "";
        }
    }

    // ------------------------------------------------------------------ 小工具

    public static int num(Map<String, Object> m, String k, int def) {
        Object o = m.get(k);
        if (o instanceof Number) return ((Number) o).intValue();
        if (o != null) {
            try { return (int) Double.parseDouble(String.valueOf(o)); } catch (Exception ignored) {}
        }
        return def;
    }

    public static String str(Map<String, Object> m, String k) {
        Object o = m.get(k);
        return o == null ? "" : String.valueOf(o);
    }

    public static boolean bool(Map<String, Object> m, String k) {
        Object o = m.get(k);
        return o != null && Boolean.parseBoolean(String.valueOf(o));
    }
}
