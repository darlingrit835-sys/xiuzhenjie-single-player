package server.game;

import xzj.Xzj;

/** 1004037 战斗结束离开（游戏服）。 */
public class BattleEndLeaveHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.BattleEndLeaveReq_1004037 req = Xzj.BattleEndLeaveReq_1004037.parseFrom(msg.getData());
        DungeonManager.remove(req.getBattleId());
        Xzj.BattleEndLeaveResp_1004037.Builder b = Xzj.BattleEndLeaveResp_1004037.newBuilder();
        b.setIsSuccess(true).setCode(4537);
        session.send(1004037, msg.getMsid(), b.build());
    }
}
