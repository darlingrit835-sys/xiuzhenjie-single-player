package server.game;

import java.util.Map;

/**
 * 洞府状态。
 *
 * 客户端 EnterHomeResp_1005001 的 Rank 并不直接是“等级”，而是模板表
 * xzjWorldHomeList 里的 level 字段：普通 0 / 黄阶 4 / 玄阶 8 / 地阶 12 / 天阶 16，
 * 客户端自己算 m_CurrentLevel = Rank / 4 得到品阶。
 *
 * 存档里用 DongFuRank 直接存品阶（0=普通，1=黄，2=玄，3=地，4=天），
 * 下发时再乘 4，避免各处重复换算。
 *
 * 内室只有两个位置（Live1Type / Live2Type），黄阶可开 1 处，玄阶起 2 处；
 * 值为 RoleFuZhiType：1 炼丹 2 炼器 3 制符 4 法术 5 阵图 6 布道 7 巧匠 8 灵植 9 御兽。
 */
public class HomeService {

    /** 0→1 黄阶、1→2 玄阶、2→3 地阶、3→4 天阶 */
    public static final int[] UPGRADE_PRICE = {3000, 20000, 100000, 1000000};
    public static final int MAX_RANK = 4;

    /** 开辟第一处内室 1000 灵石，第二处 10000 */
    public static final int[] BUILD_PRICE = {1000, 10000};
    /** 重建内室消耗灵玉 */
    public static final int REBUILD_JADE = 20;

    public static int rank(Map<String, Object> role) {
        int r = GoodsService.num(role, "DongFuRank", 0);
        return Math.max(0, Math.min(MAX_RANK, r));
    }

    /** 客户端要的 Rank = 品阶 × 4 */
    public static int clientRank(int rank) {
        return rank * 4;
    }

    public static int live(Map<String, Object> role, int location) {
        return GoodsService.num(role, location == 2 ? "Live2Type" : "Live1Type", 0);
    }

    public static void setLive(Map<String, Object> role, int location, int type) {
        role.put(location == 2 ? "Live2Type" : "Live1Type", type);
    }

    public static int usedRooms(Map<String, Object> role) {
        return (live(role, 1) != 0 ? 1 : 0) + (live(role, 2) != 0 ? 1 : 0);
    }

    /** 当前品阶能开几处内室 */
    public static int roomLimit(int rank) {
        if (rank <= 0) return 0;
        return rank == 1 ? 1 : 2;
    }

    public static int upgradePrice(int rank) {
        if (rank >= MAX_RANK) return -1;
        return UPGRADE_PRICE[rank];
    }

    public static int buildPrice(int usedRooms) {
        return usedRooms <= 0 ? BUILD_PRICE[0] : BUILD_PRICE[1];
    }

    /** 内室类型是否合法（1..9） */
    public static boolean validLiveType(int type) {
        return type >= 1 && type <= 9;
    }

    public static String liveName(int type) {
        switch (type) {
            case 1: return "炼丹室";
            case 2: return "炼器室";
            case 3: return "制符室";
            case 4: return "法术室";
            case 5: return "阵法室";
            case 6: return "悟道台";
            case 7: return "巧匠室";
            case 8: return "灵田";
            case 9: return "御兽室";
            default: return "无";
        }
    }
}
