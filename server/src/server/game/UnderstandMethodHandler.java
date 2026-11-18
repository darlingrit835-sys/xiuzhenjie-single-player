package server.game;

import xzj.Xzj;

import java.util.Map;

/**
 * 1001005 参悟功法/法术/丹方等。这里实现功法（Type=2）。
 */
public class UnderstandMethodHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.UnderstandMethodReq_1001005 req = Xzj.UnderstandMethodReq_1001005.parseFrom(msg.getData());
        Xzj.ActionBoolResp_1001001.Builder b = Xzj.ActionBoolResp_1001001.newBuilder();
        boolean questDone = false;
        String log = "[CanWu] understand type=" + req.getType() + " id=" + req.getId() + " failed";
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            if (req.getType() == 2) {
                GameData.GongFaDef g = GameData.gongFa(req.getId());
                if (g != null) {
                    RoleUtil.addOwned(session.role, g.id);
                    // 记下“已参悟”，否则重进参悟列表时又会变回可参悟
                    markKnown(session.role, g.id);
                    questDone = QuestService.onGongFa(session.role, g.rank);
                    RoleUtil.save(session.role);
                    b.setIsSuccess(true).setCode(4048);
                    log = "[CanWu] understood id=" + g.id + " name=" + g.name;
                } else {
                    b.setIsSuccess(false).setCode(4049);
                }
            } else {
                // 其它类型暂未设计，先返回“参悟失败”，避免客户端误以为成功
                b.setIsSuccess(false).setCode(4049);
            }
        }
        session.send(1001005, msg.getMsid(), b.build());
        System.out.println(log);
        if (questDone) GameServer.notifyTaskComplete(session);
    }

    /** 把已参悟的功法 id 记到 CanWuKnown，动作与读盘都在这里，免得动 RoleUtil。 */
    @SuppressWarnings("unchecked")
    private static void markKnown(Map<String, Object> r, String id) {
        if (id == null || id.isEmpty()) return;
        Object o = r.get("CanWuKnown");
        java.util.List<String> list = new java.util.ArrayList<>();
        if (o instanceof java.util.List) {
            for (Object x : (java.util.List<Object>) o) if (x != null) list.add(String.valueOf(x));
        }
        if (!list.contains(id)) {
            list.add(id);
            r.put("CanWuKnown", list);
        }
    }
}
