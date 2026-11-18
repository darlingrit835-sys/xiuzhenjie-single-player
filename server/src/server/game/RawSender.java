package server.game;

import com.google.protobuf.MessageLite;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.Channel;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelFutureListener;

import java.io.ByteArrayOutputStream;

public class RawSender {
    public static byte[] frame(MessageLite msg) {
        byte[] body = msg.toByteArray();
        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        int n = body.length;
        while ((n & ~0x7F) != 0) {
            bos.write((n & 0x7F) | 0x80);
            n >>>= 7;
        }
        bos.write(n);
        bos.write(body, 0, body.length);
        return bos.toByteArray();
    }

    public static void send(Channel ch, MessageLite msg) {
        byte[] f = frame(msg);
        ByteBuf buf = Unpooled.wrappedBuffer(f);
        ChannelFuture fut = ch.writeAndFlush(buf);
        fut.addListener((ChannelFutureListener) future -> {
            if (!future.isSuccess()) {
                System.out.println("[raw] write FAILED: " + future.cause());
            }
        });
    }
}
