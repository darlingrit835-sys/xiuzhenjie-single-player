package server.game;

import xzj.Xzj;

import java.util.Map;

public class RoleCreateHandler implements MsgHandler {
    private final RoleStore roles = RoleStore.INSTANCE;

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        Xzj.RoleCreateReq_1002001 req = Xzj.RoleCreateReq_1002001.parseFrom(msg.getData());
        System.out.println("[CreateRole] userId=" + req.getUserId() + " name=" + req.getName() + " sex=" + req.getSex());

        Xzj.RoleCreateResp_1002001.Builder b = Xzj.RoleCreateResp_1002001.newBuilder();
        if (req.getName().isEmpty() || roles.nameExists(req.getName())) {
            b.setIsSuccess(false).setCode(3002);
        } else {
            Map<String, Object> r = roles.createRole(req.getUserId(), req.getName(),
                    req.getSex(), req.getHeadPortraitId(), req.getMapId(),
                    req.getGold(), req.getWood(), req.getWater(), req.getFire(), req.getEarth());
            b.setIsSuccess(true).setCode(2000).setRoleId((String) r.get("Id"));
        }
        session.send(1002001, msg.getMsid(), b.build());
    }
}
