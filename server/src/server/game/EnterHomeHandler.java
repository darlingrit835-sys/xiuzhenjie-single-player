package server.game;

import xzj.Xzj;

/**
 * 1005001 进入洞府。
 *
 * Rank 必须是品阶 × 4（客户端 m_CurrentLevel = Rank / 4），
 * Live1Type / Live2Type 为 0 表示该位置还没开辟。
 */
public class EnterHomeHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.EnterHomeResp_1005001.Builder b = Xzj.EnterHomeResp_1005001.newBuilder();
        int rank;
        int live1;
        int live2;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            rank = HomeService.rank(session.role);
            live1 = HomeService.live(session.role, 1);
            live2 = HomeService.live(session.role, 2);
        }
        String name = GoodsService.str(session.role, "Name");
        b.setCode(4109).setIsSuccess(true);
        b.setName(name.isEmpty() ? "道友的洞府" : name + "的洞府");
        b.setRank(HomeService.clientRank(rank));
        b.setLive1Type(live1);
        b.setLive2Type(live2);
        session.send(1005001, msg.getMsid(), b.build());
        System.out.println("[Home] enter rank=" + rank + " clientRank=" + HomeService.clientRank(rank)
                + " live1=" + live1 + " live2=" + live2);
    }
}
