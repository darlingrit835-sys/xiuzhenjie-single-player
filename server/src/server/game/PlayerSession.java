package server.game;

import com.google.protobuf.ByteString;
import com.google.protobuf.MessageLite;
import io.netty.channel.Channel;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import xzj.Xzj;

import java.util.HashMap;
import java.util.Map;

public class PlayerSession {
    public final Channel channel;
    public String userId;
    public String roleId;
    public final Map<String, Object> role = new HashMap<>();
    /** 修炼小数累积，避免低等级每 tick 取整后永远加 0。 */
    public double carryNaQi;
    public double carryDuanTi;
    public double carryLianShen;

    public PlayerSession(Channel channel) {
        this.channel = channel;
    }

    public void send(int cmd, int msid, MessageLite payload) {
        Xzj.Msg.Builder mb = Xzj.Msg.newBuilder();
        mb.setCmd(cmd);
        if (msid > 0) mb.setMsid(msid);
        if (payload != null) mb.setData(payload.toByteString());
        Xzj.Msg out = mb.build();
        RawSender.send(channel, out);
    }
}
