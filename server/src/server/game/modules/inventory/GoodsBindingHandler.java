package server.game;

import xzj.Xzj;

import java.util.Map;

/** 1004023 滴血认主。 */
public class GoodsBindingHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.GoodsBindingReq_1004023 req = Xzj.GoodsBindingReq_1004023.parseFrom(msg.getData());
        Xzj.GoodsBindingResp_1004023.Builder b = Xzj.GoodsBindingResp_1004023.newBuilder();
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Map<String, Object> g = GoodsService.find(session.role, req.getGoodsId());
            if (g == null) {
                b.setIsSuccess(false).setCode(4603);
            } else {
                g.put("IsBinding", true);
                RoleUtil.save(session.role);
                b.setIsSuccess(true).setCode(2000);
            }
        }
        session.send(1004023, msg.getMsid(), b.build());
    }
}
