package server.game;

import xzj.Xzj;

public interface MsgHandler {
    void handle(PlayerSession session, Xzj.Msg msg) throws Exception;
}
