package server.game;

import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.*;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import io.netty.handler.codec.protobuf.ProtobufDecoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32FrameDecoder;
import server.common.Config;
import xzj.Xzj;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class GameServer {
    public static final Map<Integer, MsgHandler> HANDLERS = new ConcurrentHashMap<>();
    /** 在线游戏会话，修炼心跳遍历这个表。 */
    public static final Map<io.netty.channel.Channel, PlayerSession> SESSIONS = new ConcurrentHashMap<>();
    private EventLoopGroup boss;
    private EventLoopGroup worker;

    public static void register(int cmd, MsgHandler h) {
        HANDLERS.put(cmd, h);
    }

    /** 按角色 ID 找在线会话。 */
    public static PlayerSession sessionOfRole(String roleId) {
        if (roleId == null) return null;
        for (PlayerSession s : SESSIONS.values()) {
            if (s != null && roleId.equals(s.roleId) && s.channel.isActive()) return s;
        }
        return null;
    }

    /** 通知客户端“当前任务已完成”。 */
    public static void notifyTaskComplete(String roleId) {
        PlayerSession s = sessionOfRole(roleId);
        if (s != null) {
            s.send(1016001, 0, null);
        }
    }

    public static void notifyTaskComplete(PlayerSession s) {
        if (s != null && s.channel.isActive()) s.send(1016001, 0, null);
    }

    /** 屏幕中间/跑马灯提示。 */
    public static void notifyText(PlayerSession s, String text) {
        if (s == null || !s.channel.isActive() || text == null || text.isEmpty()) return;
        Xzj.BroadcastResp_1001014.Builder b = Xzj.BroadcastResp_1001014.newBuilder().setMsg(text);
        s.send(1001014, 0, b.build());
    }

    public void start() {
        HandlerRegistry.init();
        CultivationService.start();
        boss = new NioEventLoopGroup(1);
        worker = new NioEventLoopGroup();
        try {
            ServerBootstrap b = new ServerBootstrap();
            b.group(boss, worker)
             .channel(NioServerSocketChannel.class)
             .childOption(ChannelOption.TCP_NODELAY, true)
             .childHandler(new ChannelInitializer<SocketChannel>() {
                 @Override
                 protected void initChannel(SocketChannel ch) {
                     ch.pipeline()
                       .addLast(new ProtobufVarint32FrameDecoder())
                       .addLast(new ProtobufDecoder(Xzj.Msg.getDefaultInstance()))
                       .addLast(new GameChannelHandler());
                 }
             });
            java.net.InetSocketAddress address = Config.listenAddress(Config.GAME_PORT);
            ChannelFuture f = b.bind(address).sync();
            System.out.println("[GameServer] listening on tcp://" + address.getHostString() + ":" + address.getPort());
            f.channel().closeFuture().sync();
        } catch (Exception e) {
            throw new IllegalStateException("[GameServer] failed on port " + Config.GAME_PORT, e);
        } finally {
            boss.shutdownGracefully();
            worker.shutdownGracefully();
        }
    }
}
