package server.game;

import xzj.Xzj;

/**
 * 1005002 升级洞府。
 *
 * 客户端的处理是“先按响应里的 Rank 更新显示，再本地 ++”，
 * 所以这里回的 Rank 用升级前的品阶 ×4，客户端自增后就正好是新品阶；
 * 下次进洞府（1005001）再按存档下发新值，两边一致，不会出现“重进又退回去”。
 */
public class UpgradeHomeHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.UpgradeHomeResp_1005002.Builder b = Xzj.UpgradeHomeResp_1005002.newBuilder();
        String log;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            int rank = HomeService.rank(session.role);
            long stone = RoleUtil.lng(session.role, "ShinyStone", 0);
            int price = HomeService.upgradePrice(rank);
            b.setShinyStone(stone);

            if (price < 0) {
                b.setIsSuccess(false).setCode(4110);          // 已最高等级
                b.setRank(HomeService.clientRank(rank));
                log = "[Home] upgrade failed: already max rank " + rank;
            } else if (stone < price) {
                b.setIsSuccess(false).setCode(4112);          // 灵石不足
                b.setRank(HomeService.clientRank(rank));
                log = "[Home] upgrade failed: need " + price + " stone, have " + stone;
            } else {
                stone -= price;
                session.role.put("ShinyStone", stone);
                session.role.put("DongFuRank", rank + 1);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(4111);
                b.setShinyStone(stone);
                b.setRank(HomeService.clientRank(rank));      // 旧品阶，客户端 ++ 后为新品阶
                String name = GoodsService.str(session.role, "Name");
                b.setName(name.isEmpty() ? "道友的洞府" : name + "的洞府");
                log = "[Home] upgraded " + rank + " -> " + (rank + 1)
                        + ", cost " + price + " stone, stone=" + stone;
            }
        }
        session.send(1005002, msg.getMsid(), b.build());
        System.out.println(log);
    }
}
