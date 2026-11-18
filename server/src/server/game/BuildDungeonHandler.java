package server.game;

import server.common.Config;
import server.common.ServerConfig;
import xzj.Xzj;

/**
 * 1010001 创建副本。返回战斗服地址和端口。
 */
public class BuildDungeonHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.BuildDungeonReq_1010001 req = Xzj.BuildDungeonReq_1010001.parseFrom(msg.getData());
        Xzj.BuildDungeonResp_1010001.Builder b = Xzj.BuildDungeonResp_1010001.newBuilder();
        boolean questDone = false;
        synchronized (session.role) {
            RoleUtil.ensureDefaults(session.role);
            Dungeon d = DungeonManager.create(session.role, req.getMapId());
            questDone = QuestService.onDungeon(session.role);
            RoleUtil.save(session.role);
            b.setIsSuccess(true).setCode(4500);
            b.setDungeonId(d.id);
            b.setMapId(d.mapId);
            b.setAddress(ServerConfig.get().publicIp);
            b.setPort(ServerConfig.get().battlePort);
            b.setCurrentPower(RoleUtil.num(session.role, "CurrentPower"));
        }
        session.send(1010001, msg.getMsid(), b.build());
        if (questDone) GameServer.notifyTaskComplete(session);
    }
}
