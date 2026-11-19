package server.game;

import xzj.Xzj;

/**
 * 1001011 参悟列表。服务端把 48 本功法全部下发，
 * 已学会的 IsUnderstand=true，未学会的可以点“参悟”。
 */
public class CanWuListHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.OpenUnderstandPanelResp_1001011.Builder b = Xzj.OpenUnderstandPanelResp_1001011.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            for (GameData.GongFaDef g : GameData.gongFaAll()) {
                Xzj.OpenUnderstandPanelResp_1001011.UnderstandMethodObjectResp_1001012.Builder o =
                        Xzj.OpenUnderstandPanelResp_1001011.UnderstandMethodObjectResp_1001012.newBuilder();
                o.setId(g.id);
                o.setName(g.name);
                o.setFiveElement(g.fiveElement);
                o.setLevel(g.levelReq);
                o.setRank(g.rank);
                o.setType(2); // CanWuTitleType.GongFa
                o.setIsUnderstand(RoleUtil.isLearned(session.role, g.id));
                b.addObject(o);
            }
            b.setCode(4263).setIsSuccess(true);
        }
        session.send(1001011, msg.getMsid(), b.build());
    }
}
