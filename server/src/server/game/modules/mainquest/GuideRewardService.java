package server.game;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * 新手引导阶段奖励。
 *
 * 客户端引导本身没有奖励面板，所以这里在服务端按引导进度发物品，
 * 玩家能在纳戒里直接看到。每个阶段只发一次。
 */
public final class GuideRewardService {
    private GuideRewardService() {}

    static final String STARTER_METHOD = "gf_3_1";

    static boolean needsStarterPractice(Map<String, Object> role) {
        return RoleUtil.num(role, "PracticeGuideVal") == 2 && !RoleUtil.bool(role, "PracticeStarterUnderstood");
    }

    static void resumePractice(Map<String, Object> role) {
        if (RoleUtil.num(role, "PracticeGuideVal") != 1 || RoleUtil.num(role, "StarterKitVersion") < 1) return;
        if (!RoleUtil.bool(role, "PracticeStarterRead")) {
            for (Map<String, Object> item : GoodsService.all(role)) {
                if (GoodsService.num(item, "Type", 0) == 1 &&
                        (STARTER_METHOD.equals(item.get("TemplateId")) || "启灵经".equals(item.get("Name")))) return;
            }
        }
        role.put("PracticeGuideVal", 2);
        grant(role, 1, 2);
    }

    @SuppressWarnings("unchecked")
    private static List<String> claimed(Map<String, Object> role) {
        Object o = role.get("GuideRewardClaimed");
        List<String> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object x : (List<Object>) o) if (x != null) out.add(String.valueOf(x));
        }
        return out;
    }

    public static String grant(Map<String, Object> role, int guideType, int value) {
        if (value <= 1) return null;
        String key = guideType + "_" + value;
        List<String> list = claimed(role);
        if (list.contains(key)) return null;
        list.add(key);
        role.put("GuideRewardClaimed", list);
        String desc = "获得新手引导奖励，请打开纳戒查看";

        switch (guideType) {
            case 1:
                if (value == 2 || value == 3) {
                    addStone(role, 100);
                    GoodsService.addReward(role, "纳气丹", 19, 1, 1, 1);
                    desc = "引导奖励：灵石×100、纳气丹×1";
                } else if (value >= 100) {
                    addStone(role, 300);
                    addJade(role, 10);
                    GoodsService.addReward(role, "纳气丹", 19, 1, 1, 3);
                    GoodsService.addReward(role, "聚元功", 1, 2, 1, 1);
                    desc = "修行引导完成：灵石×300、灵玉×10、纳气丹×3、聚元功";
                }
                break;
            case 2:
                if (value == 2) {
                    addStone(role, 100);
                    GoodsService.addReward(role, "回春散", 19, 1, 1, 1);
                    desc = "引导奖励：灵石×100、回春散×1";
                } else if (value == 3 || value == 4) {
                    addStone(role, 100);
                    desc = "引导奖励：灵石×100";
                } else if (value >= 100) {
                    addStone(role, 300);
                    addJade(role, 10);
                    GoodsService.addReward(role, "回春散", 19, 1, 1, 3);
                    desc = "战斗引导完成：灵石×300、灵玉×10、回春散×3";
                }
                break;
            case 6:
                if (value == 2) {
                    addStone(role, 100);
                    desc = "引导奖励：灵石×100";
                } else if (value >= 100) {
                    addStone(role, 300);
                    addJade(role, 10);
                    desc = "经脉引导完成：灵石×300、灵玉×10";
                }
                break;
            case 5:
            case 7:
            case 8:
                addStone(role, 100);
                desc = "引导奖励：灵石×100";
                break;
            default:
                return null;
        }
        return desc;
    }

    private static void addStone(Map<String, Object> role, long n) {
        role.put("ShinyStone", RoleUtil.lng(role, "ShinyStone", 0) + n);
    }

    private static void addJade(Map<String, Object> role, int n) {
        role.put("ShinyJade", RoleUtil.num(role, "ShinyJade", 0) + n);
    }
}
