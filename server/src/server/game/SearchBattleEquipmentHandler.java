package server.game;

import xzj.Xzj;

/** 1002015 是否已设置战斗装备。私服直接视为已设置。 */
public class SearchBattleEquipmentHandler implements MsgHandler {
    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.SearchIsSettingBattleEquipmentResp_1002015.Builder b =
                Xzj.SearchIsSettingBattleEquipmentResp_1002015.newBuilder();
        b.setIsSuccess(true).setCode(4507).setIsSettingEquipmentBattle(true);
        session.send(1002015, msg.getMsid(), b.build());
    }
}
