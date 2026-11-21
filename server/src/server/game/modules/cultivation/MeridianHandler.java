package server.game;

import com.google.protobuf.InvalidProtocolBufferException;
import com.google.protobuf.MessageLite;
import xzj.Xzj;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ThreadLocalRandom;

final class MeridianHandler implements MsgHandler {
    static final class Receipt {
        final String roleId, node;
        final int sequence;
        final Xzj.BreakAcupointResp_1005018 response;

        Receipt(String roleId, String node, int sequence, Xzj.BreakAcupointResp_1005018 response) {
            this.roleId = roleId;
            this.node = node;
            this.sequence = sequence;
            this.response = response;
        }
    }

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) {
        MessageLite response;
        synchronized (session.role) {
            synchronized (RoleStore.INSTANCE) {
                try {
                    String point = msg.getCmd() == 1005018 ? Xzj.BreakAcupointReq_1005018.parseFrom(msg.getData()).getAcupointId() : "";
                    Map<String, Object> role = session.roleId == null ? null : RoleStore.INSTANCE.getRole(session.roleId);
                    if (role == null) response = error(msg.getCmd(), 4007);
                    else {
                        RoleStore.copyAcupoints(role, session.role);
                        if (msg.getCmd() == 1005018) response = attempt(session, role, point, msg.getMsid());
                        else if (msg.getCmd() == 1005020) response = Meridians.overview(role);
                        else response = open(role);
                    }
                } catch (InvalidProtocolBufferException malformed) {
                    response = error(msg.getCmd(), 4008);
                } catch (RuntimeException storageError) {
                    System.err.println("[Meridians] " + storageError);
                    response = error(msg.getCmd(), 5000);
                }
            }
        }
        session.send(msg.getCmd(), msg.getMsid(), response);
    }

    private static MessageLite error(int cmd, int code) {
        if (cmd == 1005016) return Xzj.OpenAcupointResp_1005016.newBuilder().setCode(code).build();
        if (cmd == 1005020) return Xzj.AcupointDetailResp_1005020.newBuilder().setCode(code).build();
        return rejected(code);
    }

    private static Xzj.OpenAcupointResp_1005016 open(Map<String, Object> role) {
        int points = (int) Math.max(0, Math.min(Integer.MAX_VALUE, Meridians.balance(role)));
        Xzj.OpenAcupointResp_1005016.Builder out = Xzj.OpenAcupointResp_1005016.newBuilder()
                .setIsSuccess(true).setCode(2000).setAcupointValue(points);
        for (Map.Entry<String, Integer> entry : Meridians.levels(role).entrySet())
            out.addAcupointsBuilder().setId(entry.getKey()).setBrokenLevel(entry.getValue());
        return out.build();
    }

    @SuppressWarnings("unchecked")
    private static void setLevel(Map<String, Object> role, String id, int level) {
        Object previous = role.get("Acupoints");
        List<Object> rows = previous instanceof List ? new ArrayList<>((List<?>) previous) : new ArrayList<>();
        for (int index = 0; index < rows.size(); index++) {
            Object value = rows.get(index);
            if (!(value instanceof Map)) continue;
            Map<String, Object> existing = (Map<String, Object>) value;
            if (id.equals(RoleUtil.str(existing, "Id"))) {
                Map<String, Object> changed = new HashMap<>(existing);
                changed.put("BrokenLevel", level);
                rows.set(index, changed);
                role.put("Acupoints", rows);
                return;
            }
        }
        Map<String, Object> row = new HashMap<>();
        row.put("Id", id);
        row.put("BrokenLevel", level);
        rows.add(row);
        role.put("Acupoints", rows);
    }

    private static Xzj.BreakAcupointResp_1005018 rejected(int code) {
        return Xzj.BreakAcupointResp_1005018.newBuilder().setCode(code).build();
    }

    private static Xzj.BreakAcupointResp_1005018 attempt(PlayerSession session, Map<String, Object> role, String id, int sequence) {
        if (sequence > 0) for (Receipt receipt : session.acupointReceipts) {
            if (receipt.sequence == sequence && receipt.roleId.equals(session.roleId))
                return receipt.node.equals(id) ? receipt.response : rejected(4008);
        }
        if (Meridians.nodes().isEmpty()) return rejected(4008);
        Meridians.Node node = Meridians.nodes().get(id);
        if (node == null) return rejected(4146);
        Map<String, Integer> levels = Meridians.levels(role);
        int current = levels.getOrDefault(id, 0);
        if (current >= 3) return rejected(4141);
        if (RoleUtil.bool(role, "IsBrokenAcupoint")) return rejected(4143);
        if (current == 0 && !"1".equals(id) && Collections.disjoint(node.parents, levels.keySet())) return rejected(4146);

        long points = Meridians.balance(role);
        int cost = node.cost(current);
        if (points < cost) return rejected(4144);
        if (points > Integer.MAX_VALUE) return rejected(4008);
        int chance = Meridians.chance(role, node, levels, current);
        boolean success = chance == 100 || ThreadLocalRandom.current().nextInt(100) < chance;
        int remaining = (int) points - cost;
        role.put("AcupointValue", remaining);
        if (success) setLevel(role, id, ++current);
        RoleStore.INSTANCE.saveWithAcupoints(role);
        RoleStore.copyAcupoints(role, session.role);
        Xzj.BreakAcupointResp_1005018 result = Xzj.BreakAcupointResp_1005018.newBuilder().setIsSuccess(true)
                .setCode(success ? 4142 : 4145).setBreakSuccess(success).setBrokenLevel(current).setAcupointValue(remaining).build();
        if (sequence > 0) {
            if (session.acupointReceipts.size() == 16) session.acupointReceipts.removeFirst();
            session.acupointReceipts.addLast(new Receipt(session.roleId, id, sequence, result));
        }
        return result;
    }
}
