package server.game;

import xzj.Xzj;

import java.time.LocalDate;
import java.util.Map;

/**
 * 七日签到。
 *
 * 客户端逻辑（QianDaoWnd）：打开签到面板时若 !IsGetSignIn 且 SignInDay<=6 就自动请求 1002021 领当天那份，
 * 领成功后本地 IsGetSignIn=true、SignInDay+1。原私服这两个协议都没有实现（走通用兜底只回 success，
 * 既不发奖也不推进天数），所以这里补齐：
 *   1002018 下发七日奖励列表，1002021 真正发奖 + 推进 SignInDay + 存档。
 * 每天一份：登录时按自然日重置 IsGetSignIn；领满七天后再开启下一轮。
 */
public final class SignInService {
    private SignInService() {}

    /** 货币型（客户端 QianDaoItem 用 99/100 判断是否放大图标） */
    private static final int TYPE_STONE = 99;
    private static final int TYPE_JADE = 100;

    /** 第 1-6 天：{名称, 类型, 数量} */
    private static final Object[][] DAILY = {
            {"灵石", TYPE_STONE, 1000},
            {"灵玉", TYPE_JADE, 25},
            {"灵石", TYPE_STONE, 1500},
            {"灵玉", TYPE_JADE, 40},
            {"纳气丹", 19, 2},
            {"灵玉", TYPE_JADE, 60},
    };

    /** 第 7 天：双奖励 */
    private static final Object[][] DAY7 = {
            {"灵石", TYPE_STONE, 3000},
            {"灵玉", TYPE_JADE, 100},
    };

    private static Object[][] rewardsOf(int day) {
        return day >= 6 ? DAY7 : new Object[][]{DAILY[Math.max(0, day)]};
    }

    /** 1002018 响应体：七天列表。 */
    public static void buildList(Xzj.SearchSignInAwardResp_1002018.Builder b) {
        for (int day = 0; day <= 6; day++) {
            Xzj.SearchSignInAwardResp_1002018.SignInAwardResp_1002019.Builder d =
                    Xzj.SearchSignInAwardResp_1002018.SignInAwardResp_1002019.newBuilder();
            d.setSignInDay(day);
            for (Object[] a : rewardsOf(day)) {
                d.addAward(Xzj.SearchSignInAwardResp_1002018.SignInAwardResp_1002019.AwardResp_1002020.newBuilder()
                        .setGoodsName((String) a[0])
                        .setType((Integer) a[1])
                        .setNum((Integer) a[2])
                        .setRank(1));
            }
            b.addSignInAward(d);
        }
    }

    /**
     * 领当天签到奖励。返回奖励描述，领不到返回 null。
     * 调用方需持有 role 锁并自行 save。
     */
    public static String claim(Map<String, Object> role) {
        int day = RoleUtil.num(role, "SignInDay", 0);
        if (day < 0 || day > 6) return null;

        StringBuilder sb = new StringBuilder();
        for (Object[] a : rewardsOf(day)) {
            String name = (String) a[0];
            int type = (Integer) a[1];
            int n = (Integer) a[2];
            grant(role, name, type, n);
            if (sb.length() > 0) sb.append("、");
            sb.append(name).append("×").append(n);
        }

        role.put("SignInDay", day + 1);
        role.put("IsGetSignIn", true);
        role.put("SignInDate", today());
        return sb.toString();
    }

    private static void grant(Map<String, Object> role, String name, int type, int n) {
        if (type == TYPE_STONE) {
            role.put("ShinyStone", RoleUtil.lng(role, "ShinyStone", 0) + n);
        } else if (type == TYPE_JADE) {
            role.put("ShinyJade", RoleUtil.num(role, "ShinyJade", 0) + n);
        } else {
            // 物品统一走 GoodsService，纳戒里能直接看到
            GoodsService.addReward(role, name, type, 1, 1, n);
        }
    }

    /** 登录时调用：跨天则允许再签一次；七天领完后开启新一轮。返回是否有变化。 */
    public static boolean dailyReset(Map<String, Object> role) {
        boolean changed = false;
        String last = RoleUtil.str(role, "SignInDate");
        if (!today().equals(last)) {
            if (RoleUtil.bool(role, "IsGetSignIn")) {
                role.put("IsGetSignIn", false);
                changed = true;
            }
        }
        if (RoleUtil.num(role, "SignInDay", 0) > 6) {
            role.put("SignInDay", 0);
            changed = true;
        }
        return changed;
    }

    private static String today() {
        return LocalDate.now().toString();
    }
}
