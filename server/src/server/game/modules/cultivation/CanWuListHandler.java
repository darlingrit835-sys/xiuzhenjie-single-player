package server.game;

import xzj.Xzj;

/**
 * 1001011 参悟书目包含已悟功法、已读旧谱和本灵根的天道传承。
 */
public class CanWuListHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.OpenUnderstandPanelResp_1001011.Builder b = Xzj.OpenUnderstandPanelResp_1001011.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            for (GameData.GongFaDef g : GameData.gongFaAll()) {
                boolean learned = RoleUtil.isLearned(session.role, g.id);
                if (!learned && !(RoleUtil.matchesRoot(session.role, g) && RoleUtil.isRecorded(session.role, g))) continue;
                Xzj.OpenUnderstandPanelResp_1001011.UnderstandMethodObjectResp_1001012.Builder o =
                        Xzj.OpenUnderstandPanelResp_1001011.UnderstandMethodObjectResp_1001012.newBuilder();
                o.setId(g.id);
                o.setName(g.name);
                o.setFiveElement(RoleUtil.methodElement(session.role, g));
                o.setLevel(g.levelReq);
                o.setRank(g.rank);
                o.setType(2); // CanWuTitleType.GongFa
                boolean starterPractice = GuideRewardService.STARTER_METHOD.equals(g.id) && GuideRewardService.needsStarterPractice(session.role);
                o.setIsUnderstand(!starterPractice && learned);
                if (starterPractice) b.addObject(0, o);
                else b.addObject(o);
            }
            b.setCode(4263).setIsSuccess(true);
        }
        session.send(1001011, msg.getMsid(), b.build());
    }
}
