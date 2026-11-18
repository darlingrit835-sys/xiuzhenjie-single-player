package server.game;

import xzj.Xzj;

/** 1002018 查询七日签到奖励列表。 */
public class SearchSignInAwardHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) {
        Xzj.SearchSignInAwardResp_1002018.Builder b = Xzj.SearchSignInAwardResp_1002018.newBuilder();
        SignInService.buildList(b);
        b.setIsSuccess(true).setCode(2000);
        session.send(1002018, msg.getMsid(), b.build());
    }
}
