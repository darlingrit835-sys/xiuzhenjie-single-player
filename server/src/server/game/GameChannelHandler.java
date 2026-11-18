package server.game;

import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import xzj.Xzj;

import java.net.InetSocketAddress;

public class GameChannelHandler extends SimpleChannelInboundHandler<Xzj.Msg> {
    private PlayerSession session;

    @Override
    public void channelActive(ChannelHandlerContext ctx) {
        session = new PlayerSession(ctx.channel());
        GameServer.SESSIONS.put(ctx.channel(), session);
        System.out.println("[GameServer] connected: " + ctx.channel().remoteAddress());
    }

    @Override
    public void channelInactive(ChannelHandlerContext ctx) {
        GameServer.SESSIONS.remove(ctx.channel());
        if (session != null && session.roleId != null && !session.role.isEmpty()) {
            synchronized (session.role) {
                session.role.put("LastOnline", System.currentTimeMillis());
                RoleUtil.save(session.role);
            }
        }
        System.out.println("[GameServer] disconnected: " + ctx.channel().remoteAddress());
    }

    @Override
    protected void channelRead0(ChannelHandlerContext ctx, Xzj.Msg msg) {
        int cmd = msg.getCmd();
        int msid = msg.getMsid();
        MsgHandler h = GameServer.HANDLERS.get(cmd);
        if (h != null) {
            try {
                System.out.println("[Req] cmd=" + cmd + " msid=" + msid
                        + " len=" + msg.getData().size()
                        + " -> " + h.getClass().getSimpleName());
                h.handle(session, msg);
            } catch (Exception e) {
                System.err.println("[GameServer] handler error cmd=" + cmd + ": " + e);
                e.printStackTrace();
            }
        } else {
            System.out.println("[GameServer] generic cmd=" + cmd + " msid=" + msid + " dataLen=" + msg.getData().size());
            try {
                new GenericHandler().handle(session, msg);
            } catch (Exception e) {
                System.err.println("[GameServer] generic error cmd=" + cmd + ": " + e);
            }
        }
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        System.err.println("[GameServer] channel error: " + cause);
        ctx.close();
    }
}
