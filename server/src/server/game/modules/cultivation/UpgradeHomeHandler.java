package server.game;

import com.google.protobuf.Int32Value;
import com.google.protobuf.InvalidProtocolBufferException;
import xzj.Xzj;

import java.util.Map;

/** 1005002 upgrades one grade. Field 1 optionally identifies the requested normalized grade. */
public final class UpgradeHomeHandler implements MsgHandler {
    private static final int SUCCESS = 4111;
    private static final int GRADE_CAP = 4110;
    private static final int INSUFFICIENT_STONES = 4112;
    private static final int REALM_TOO_LOW = 4105;
    private static final int INVALID_REQUEST = 4008;
    private static final int NO_ROLE = 4007;
    private static final int SERVER_ERROR = 5000;

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) {
        Xzj.UpgradeHomeResp_1005002.Builder result;
        synchronized (session.role) {
            synchronized (RoleStore.INSTANCE) {
                result = upgrade(session, msg);
            }
        }
        session.send(1005002, msg.getMsid(), result.build());
    }

    private Xzj.UpgradeHomeResp_1005002.Builder upgrade(PlayerSession session, Xzj.Msg msg) {
        if (session.roleId == null) return reply(session.role, NO_ROLE);

        Map<String, Object> role;
        try {
            role = RoleStore.INSTANCE.getRole(session.roleId);
        } catch (RuntimeException storageFailure) {
            return reply(session.role, SERVER_ERROR);
        }
        if (role == null) return reply(session.role, NO_ROLE);

        int seq = msg.getMsid();
        if (seq > 0 && seq <= session.lastHomeUpgradeMessageId) return reply(role, INVALID_REQUEST);
        if (seq > 0) session.lastHomeUpgradeMessageId = seq;

        int grade = HomeGrades.grade(role);
        if (grade == HomeGrades.MAX_GRADE) return reply(role, GRADE_CAP);
        int next = grade + 1;
        if (!msg.getData().isEmpty()) {
            try {
                if (Int32Value.parseFrom(msg.getData()).getValue() != next) return reply(role, INVALID_REQUEST);
            } catch (InvalidProtocolBufferException malformed) {
                return reply(role, INVALID_REQUEST);
            }
        }

        int level = Math.max(1, RoleUtil.num(role, "Level", 1));
        if ((level - 1) / 10 + 1 < next) return reply(role, REALM_TOO_LOW);
        long price = HomeGrades.upgradePrice(next, RoleUtil.num(role, "VipLevel"));
        if (price <= 0) return reply(role, SERVER_ERROR);
        long balance = RoleUtil.lng(role, "ShinyStone");
        if (balance < price) return reply(role, INSUFFICIENT_STONES);

        role.put("DongFuRank", next);
        role.put("ShinyStone", balance - price);
        try {
            RoleStore.INSTANCE.save(role);
        } catch (RuntimeException storageFailure) {
            role.put("DongFuRank", grade);
            role.put("ShinyStone", balance);
            return reply(role, SERVER_ERROR);
        }
        session.role.put("DongFuRank", next);
        session.role.put("ShinyStone", balance - price);
        return reply(role, SUCCESS);
    }

    private static Xzj.UpgradeHomeResp_1005002.Builder reply(Map<String, Object> role, int code) {
        return Xzj.UpgradeHomeResp_1005002.newBuilder()
                .setCode(code)
                .setIsSuccess(code == SUCCESS)
                .setShinyStone(RoleUtil.lng(role, "ShinyStone"))
                .setName(HomeGrades.homeName(role))
                .setRank(HomeGrades.wireRank(HomeGrades.grade(role)));
    }
}
