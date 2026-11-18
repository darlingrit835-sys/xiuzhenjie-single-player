package server.game;

import com.google.protobuf.Descriptors;
import com.google.protobuf.DynamicMessage;
import xzj.Xzj;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class GenericHandler implements MsgHandler {
    private static final Map<Integer, Descriptors.Descriptor> RESP_CACHE = new ConcurrentHashMap<>();
    private static final Map<Integer, Boolean> MISS_CACHE = new ConcurrentHashMap<>();

    public static Descriptors.Descriptor findResp(int cmd) {
        Descriptors.Descriptor d = RESP_CACHE.get(cmd);
        if (d != null) return d;
        if (MISS_CACHE.containsKey(cmd)) return null;
        for (Descriptors.Descriptor t : Xzj.getDescriptor().getMessageTypes()) {
            if (t.getName().endsWith("Resp_" + cmd)) {
                RESP_CACHE.put(cmd, t);
                return t;
            }
        }
        MISS_CACHE.put(cmd, true);
        return null;
    }

    @Override
    public void handle(PlayerSession session, Xzj.Msg msg) throws Exception {
        int cmd = msg.getCmd();
        Descriptors.Descriptor d = findResp(cmd);
        if (d == null) {
            System.out.println("[Generic] no resp type for cmd=" + cmd + ", no reply");
            return;
        }
        DynamicMessage.Builder b = DynamicMessage.newBuilder(d);
        Descriptors.FieldDescriptor isSuccess = d.findFieldByName("IsSuccess");
        Descriptors.FieldDescriptor code = d.findFieldByName("Code");
        if (isSuccess != null) b.setField(isSuccess, true);
        if (code != null) b.setField(code, 2000);
        session.send(cmd, msg.getMsid(), b.build());
    }
}
