package server.game;

import xzj.Xzj;

/**
 * 1016002 领取主线任务奖励。
 * 客户端在领奖成功后会自己把 TaskId +1，所以服务端必须使用同一套推进规则。
 */
public class ReceiveAwardHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.ReceiveAwardReq_1016002 req = Xzj.ReceiveAwardReq_1016002.parseFrom(msg.getData());
        Xzj.ReceiveAwardResp_1016002.Builder b = Xzj.ReceiveAwardResp_1016002.newBuilder();
        QuestService.QuestDef def = QuestService.def(req.getTaskId());

        int status = QuestService.receiveAward(session, req.getTaskId());
        if (status < 0) {
            b.setIsSuccess(false).setCode(status == -1 ? 4601 : 4602);
            b.setStatus(0);
        } else {
            b.setIsSuccess(true).setCode(2000);
            b.setStatus(status);
        }
        session.send(1016002, msg.getMsid(), b.build());

        if (status >= 0 && def != null) {
            String where = def.awardGoodsType == 99 ? "" : "（已放入纳戒）";
            GameServer.notifyText(session, "获得任务奖励：" + def.award + "×" + Math.max(1, def.awardNum) + where);
        }
        // 下一条任务如果已经满足（例如等级早已超过），立即通知客户端刷新“可领取”
        if (status == 1) GameServer.notifyTaskComplete(session);
    }
}
