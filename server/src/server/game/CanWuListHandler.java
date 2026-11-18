package server.game;

import xzj.Xzj;

import java.util.Arrays;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

/**
 * 1001011 参悟列表。
 *
 * 存档 GongFaOwned 里其实 48 本功法都有（老存档是这么初始化的），如果直接拿它当
 * “已学会”，客户端每本书都是已领悟状态，参悟界面就一本可参悟的都没有——
 * 这就是“参悟 0、点了没反应”的原因。
 *
 * 这里改成：只有当前正在修行/装备的那几本才算已学会，其余全部放出来给玩家参悟。
 */
public class CanWuListHandler implements MsgHandler {

    private static Set<String> learnedIds(Map<String, Object> r) {
        Set<String> s = new HashSet<>();
        for (String f : new String[]{"GongFaNaQi", "GongFaDuanTi", "GongFaLianShen"}) {
            String v = GoodsService.str(r, f);
            if (!v.isEmpty()) s.add(v);
        }
        // 入门功法（启灵经）默认已领悟
        s.add("gf_3_1");
        // 之后参悟成功的功法记在这里，避免重进又变回“可参悟”
        for (String id : RoleUtil.stringList(r, "CanWuKnown")) {
            if (!id.isEmpty()) s.add(id);
        }
        return s;
    }

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.OpenUnderstandPanelResp_1001011.Builder b = Xzj.OpenUnderstandPanelResp_1001011.newBuilder();
        int learned;
        int total;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Set<String> known = learnedIds(session.role);
            int n = 0;
            for (GameData.GongFaDef g : GameData.gongFaAll()) {
                boolean isLearned = known.contains(g.id);
                if (isLearned) n++;
                Xzj.OpenUnderstandPanelResp_1001011.UnderstandMethodObjectResp_1001012.Builder o =
                        Xzj.OpenUnderstandPanelResp_1001011.UnderstandMethodObjectResp_1001012.newBuilder();
                o.setId(g.id)
                        .setName(g.name)
                        .setFiveElement(g.fiveElement)
                        .setLevel(g.levelReq)
                        .setRank(g.rank)
                        .setType(2) // CanWuTitleType.GongFa
                        .setIsUnderstand(isLearned);
                b.addObject(o);
            }
            learned = n;
            total = GameData.gongFaAll().size();
            // 通过列表把“已学会”写回存档，保证修行/切换功法时一致
            java.util.List<String> owned = RoleUtil.owned(session.role);
            for (GameData.GongFaDef g : GameData.gongFaAll()) {
                if (known.contains(g.id) && !owned.contains(g.id)) owned.add(g.id);
            }
            RoleUtil.save(session.role);
            b.setCode(4263).setIsSuccess(true);
        }
        session.send(1001011, msg.getMsid(), b.build());
        System.out.println("[CanWu] list sent: learned=" + learned + "/" + total
                + " canwu=" + (total - learned));
    }
}
